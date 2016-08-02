/*
Authors:
Szymon Rusinkiewicz, Princeton University
Doug DeCarlo, Rutgers University

With contributions by:
Xiaofeng Mi, Rutgers University
Tilke Judd, MIT

rtsc.cc
Real-time suggestive contours - these days, it also draws many other lines.
*/

#include <stdio.h>
#include <stdlib.h>
//#include "TriMesh.h"
#include "TriMesh_algo.h"
#include "XForm.h"
#include "apparentridge.h"
#include "GLCamera.h"
#include "timestamp.h"
#include "GL/glui.h"
#ifndef DARWIN
//#include <GL/glext.h>
#endif
#include <algorithm>
using namespace std;

//opengl
vector<float> gl_alpha;
vector<point> gl_vertex;
//

//zdd++ Email:zddmail@gmail.com
#ifndef M_PI_2
#	define M_PI_2 1.57079632679489661923 /* pi/2 */
#endif

#ifndef M_2_PI
#	define M_2_PI 6.283185307179586232    /* 2*pi  */
#endif

#ifndef M_SQRT1_2 
#	define M_SQRT1_2 0.707106781186547524401 /* 1/sqrt(2)*/
#endif
//zdd++ Email:zddmail@gmail.com


// Set to false for hardware that has problems with display lists
const bool use_dlists = true;
// Set to false for hardware that has problems with supplying 3D texture coords
const bool use_3dtexc = false;


// Globals: mesh...
TriMesh *themesh;

// Two cameras: the primary one, and an alternate one to fix the lines
// and see them from a different direction
int dual_vpmode = false, mouse_moves_alt = false;
GLCamera camera, camera_alt;
xform xf, xf_alt;
float fov = 0.7f;
double alt_projmatrix[16];
char *xffilename; // Filename where we look for "home" position
point viewpos = point(0, 0, 4);    // Current view position

// Toggles for drawing various lines
int draw_extsil = 0, draw_c = 1, draw_sc = 1;
int draw_sh = 0, draw_phridges = 0, draw_phvalleys = 0;
int draw_ridges = 0, draw_valleys = 0, draw_apparent = 0;
int draw_K = 0, draw_H = 0, draw_DwKr = 0;
int draw_bdy = 0, draw_isoph = 0, draw_topo = 0;
int niso = 20, ntopo = 20;
float topo_offset = 0.0f;

// Toggles for tests we perform
int draw_hidden = 0;
int test_c = 1, test_sc = 1, test_sh = 1, test_ph = 1, test_rv = 1, test_ar = 1;
float sug_thresh = 0.01, sh_thresh = 0.02, ph_thresh = 0.04;
float rv_thresh = 0.1, ar_thresh = 0.1;

// Toggles for style
int use_texture = 0;
int draw_faded = 1;
int draw_colors = 0;
int use_hermite = 0;

// Mesh colorization
enum { COLOR_WHITE, COLOR_GRAY, COLOR_CURV, COLOR_GCURV, COLOR_MESH };
const int ncolor_styles = 5;
int color_style = COLOR_WHITE;
vector<Color> curv_colors, gcurv_colors;
int draw_edges = false;

// Lighting
enum { LIGHTING_NONE, LIGHTING_LAMBERTIAN, LIGHTING_LAMBERTIAN2,
       LIGHTING_HEMISPHERE, LIGHTING_TOON, LIGHTING_TOONBW, LIGHTING_GOOCH };
const int nlighting_styles = 7;
int lighting_style = LIGHTING_NONE;
GLUI_Rotation *lightdir = NULL;
float lightdir_matrix[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
int light_wrt_camera = true;

// Per-vertex vectors
int draw_norm = 0, draw_curv1 = 0, draw_curv2 = 0, draw_asymp = 0;
int draw_w = 0, draw_wperp = 0;

// Other miscellaneous variables
float feature_size;	// Used to make thresholds dimensionless
float currsmooth;	// Used in smoothing
vec currcolor;		// Current line color


// Draw triangle strips.  They are stored as length followed by values.
void draw_tstrips()
{
	const int *t = &themesh->tstrips[0];
	const int *end = t + themesh->tstrips.size();
	while (likely(t < end)) {
		int striplen = *t++;
		glDrawElements(GL_TRIANGLE_STRIP, striplen, GL_UNSIGNED_INT, t);
		t += striplen;
	}
}


// Create a texture with a black line of the given width.
void make_texture(float width)
{
	glPixelStorei(GL_UNPACK_ALIGNMENT, 1);
	int texsize = 1024;
	static unsigned char *texture = new unsigned char[texsize*texsize];
	int miplevel = 0;
	while (texsize) {
		for (int i = 0; i < texsize*texsize; i++) {
			float x = (float) (i%texsize) - 0.5f * texsize + 0.5f;
			float y = (float) (i/texsize) - 0.5f * texsize + 0.5f;
			float val = 1;
			if (texsize >= 4)
				if (fabs(x) < width && y > 0.0f)
					val = sqr(max(1.0f - y, 0.0f));
			texture[i] = min(max(int(256.0f * val), 0), 255);
		}
		glTexImage2D(GL_TEXTURE_2D, miplevel, GL_LUMINANCE,
			     texsize, texsize, 0,
			     GL_LUMINANCE, GL_UNSIGNED_BYTE, texture);
		texsize >>= 1;
		miplevel++;
	}

	float bgcolor[] = { 1, 1, 1, 1 };
	glTexParameterfv(GL_TEXTURE_2D, GL_TEXTURE_BORDER_COLOR, bgcolor);
	glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP);
	glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP);
	glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
	glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
#ifdef GL_EXT_texture_filter_anisotropic
	glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MAX_ANISOTROPY_EXT, 1);
#endif
}


// Draw contours and suggestive contours using texture mapping
void draw_c_sc_texture(const vector<float> &ndotv,
		       const vector<float> &kr,
		       const vector<float> &sctest_num,
		       const vector<float> &sctest_den)
{
	glEnableClientState(GL_VERTEX_ARRAY);
	glVertexPointer(3, GL_FLOAT, 0, &themesh->vertices[0][0]);

	static vector<float> texcoords;
	int nv = themesh->vertices.size();
	texcoords.resize(2*nv);
	glEnableClientState(GL_TEXTURE_COORD_ARRAY);
	glTexCoordPointer(2, GL_FLOAT, 0, &texcoords[0]);

	// Remap texture coordinates from [-1..1] to [0..1]
	glMatrixMode(GL_TEXTURE);
	glLoadIdentity();
	glTranslatef(0.5, 0.5, 0.0);
	glScalef(0.5, 0.5, 0.0);
	glMatrixMode(GL_MODELVIEW);

	float bgcolor[] = { 1, 1, 1, 1 };
	glTexEnvi(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_BLEND);
	glTexEnvfv(GL_TEXTURE_ENV, GL_TEXTURE_ENV_COLOR, bgcolor);
	glHint(GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST);
	glEnable(GL_BLEND);
	glBlendFunc(GL_DST_COLOR, GL_ZERO); // Multiplies texture into FB
	glEnable(GL_TEXTURE_2D);
	glDepthFunc(GL_LEQUAL);


	// First drawing pass for contours
	if (draw_c) {
		// Set up the texture for the contour pass
		static GLuint texcontext_c = 0;
		if (!texcontext_c) {
			glGenTextures(1, &texcontext_c);
			glBindTexture(GL_TEXTURE_2D, texcontext_c);
			make_texture(4.0);
		}
		glBindTexture(GL_TEXTURE_2D, texcontext_c);
		if (draw_colors)
			glColor3f(0.0, 0.6, 0.0);
		else
			glColor3f(0.05, 0.05, 0.05);

		// Compute texture coordinates and draw
		for (int i = 0; i < nv; i++) {
			texcoords[2*i] = ndotv[i];
			texcoords[2*i+1] = 0.5f;
		}
		draw_tstrips();
	}

	// Second drawing pass for suggestive contours.  This should eventually
	// be folded into the previous one with multitexturing.
	if (draw_sc) {
		static GLuint texcontext_sc = 0;
		if (!texcontext_sc) {
			glGenTextures(1, &texcontext_sc);
			glBindTexture(GL_TEXTURE_2D, texcontext_sc);
			make_texture(2.0);
		}
		glBindTexture(GL_TEXTURE_2D, texcontext_sc);
		if (draw_colors)
			glColor3f(0.0, 0.0, 0.8);
		else
			glColor3f(0.05, 0.05, 0.05);


		float feature_size2 = sqr(feature_size);
		for (int i = 0; i < nv; i++) {
			texcoords[2*i] = feature_size * kr[i];
			texcoords[2*i+1] = feature_size2 *
					   sctest_num[i] / sctest_den[i];
		}
		draw_tstrips();
	}

	glDisable(GL_TEXTURE_2D);
	glDisableClientState(GL_VERTEX_ARRAY);
	glDisableClientState(GL_TEXTURE_COORD_ARRAY);
	glMatrixMode(GL_TEXTURE);
	glLoadIdentity();
	glMatrixMode(GL_MODELVIEW);
}


// Color the mesh by curvatures
void compute_curv_colors()
{
	float cscale = sqr(8.0f * feature_size);

	int nv = themesh->vertices.size();
	curv_colors.resize(nv);
	for (int i = 0; i < nv; i++) {
		float H = 0.5f * (themesh->curv1[i] + themesh->curv2[i]);
		float K = themesh->curv1[i] * themesh->curv2[i];
		float h = 4.0f / 3.0f * fabs(atan2(H*H-K,H*H*sgn(H)));
		float s = M_2_PI * atan((2.0f*H*H-K)*cscale);
		curv_colors[i] = Color::hsv(h,s,1.0);
	}
}


// Similar, but grayscale mapping of mean curvature H
void compute_gcurv_colors()
{
	float cscale = 10.0f * feature_size;

	int nv = themesh->vertices.size();
	gcurv_colors.resize(nv);
	for (int i = 0; i < nv; i++) {
		float H = 0.5f * (themesh->curv1[i] + themesh->curv2[i]);
		float c = (atan(H*cscale) + M_PI_2) / M_PI;
		c = sqrt(c);
		int C = int(min(max(256.0 * c, 0.0), 255.99));
		gcurv_colors[i] = Color(C,C,C);
	}
}


// Set up textures to be used for the lighting.
// These are indexed by (n dot l), though they are actually 2D textures
// with a height of 1 because some hardware (cough, cough, ATI) is
// thoroughly broken for 1D textures...
void make_light_textures(GLuint *texture_contexts)
{
	glPixelStorei(GL_UNPACK_ALIGNMENT, 1);
	const int texsize = 256;
	unsigned char texture[3*texsize];

	glGenTextures(nlighting_styles, texture_contexts);

	// Simple diffuse shading
	glBindTexture(GL_TEXTURE_2D, texture_contexts[LIGHTING_LAMBERTIAN]);
	for (int i = 0; i < texsize; i++) {
		float z = float(i + 1 - texsize/2) / (0.5f * texsize);
		texture[i] = max(0, int(255 * z));
	}
	glTexImage2D(GL_TEXTURE_2D, 0, 1, texsize, 1, 0,
		     GL_LUMINANCE, GL_UNSIGNED_BYTE, texture);

	// Diffuse shading with gamma = 2
	glBindTexture(GL_TEXTURE_2D, texture_contexts[LIGHTING_LAMBERTIAN2]);
	for (int i = 0; i < texsize; i++) {
		float z = float(i + 1 - texsize/2) / (0.5f * texsize);
		texture[i] = max(0, int(255 * sqrt(z)));
	}
	glTexImage2D(GL_TEXTURE_2D, 0, 1, texsize, 1, 0,
		     GL_LUMINANCE, GL_UNSIGNED_BYTE, texture);

	// Lighting from a hemisphere of light
	glBindTexture(GL_TEXTURE_2D, texture_contexts[LIGHTING_HEMISPHERE]);
	for (int i = 0; i < texsize; i++) {
		float z = float(i + 1 - texsize/2) / (0.5f * texsize);
		texture[i] = max(0, int(255 * (0.5f + 0.5f * z)));
	}
	glTexImage2D(GL_TEXTURE_2D, 0, 1, texsize, 1, 0,
		     GL_LUMINANCE, GL_UNSIGNED_BYTE, texture);

	// A soft gray/white toon shader
	glBindTexture(GL_TEXTURE_2D, texture_contexts[LIGHTING_TOON]);
	for (int i = 0; i < texsize; i++) {
		float z = float(i + 1 - texsize/2) / (0.5f * texsize);
		int tmp = int(255 * z);
		texture[i] = min(max(2*(tmp-50), 210), 255);
	}
	glTexImage2D(GL_TEXTURE_2D, 0, 1, texsize, 1, 0,
		     GL_LUMINANCE, GL_UNSIGNED_BYTE, texture);

	// A hard black/white toon shader
	glBindTexture(GL_TEXTURE_2D, texture_contexts[LIGHTING_TOONBW]);
	for (int i = 0; i < texsize; i++) {
		float z = float(i + 1 - texsize/2) / (0.5f * texsize);
		int tmp = int(255 * z);
		texture[i] = min(max(25*(tmp-20), 0), 255);
	}
	glTexImage2D(GL_TEXTURE_2D, 0, 1, texsize, 1, 0,
		     GL_LUMINANCE, GL_UNSIGNED_BYTE, texture);

	// A Gooch-inspired yellow-to-blue color ramp
	glBindTexture(GL_TEXTURE_2D, texture_contexts[LIGHTING_GOOCH]);
	for (int i = 0; i < texsize; i++) {
		float z = float(i + 1 - texsize/2) / (0.5f * texsize);
		float r = 0.75f + 0.25f * z;
		float g = r;
		float b = 0.9f - 0.1f * z;
		texture[3*i  ] = max(0, int(255 * r));
		texture[3*i+1] = max(0, int(255 * g));
		texture[3*i+2] = max(0, int(255 * b));
	}
	glTexImage2D(GL_TEXTURE_2D, 0, 3, texsize, 1, 0,
		     GL_RGB, GL_UNSIGNED_BYTE, texture);
}


// Draw the basic mesh, which we'll overlay with lines
void draw_base_mesh()
{
	int nv = themesh->vertices.size();

	// Enable the vertex array
	glEnableClientState(GL_VERTEX_ARRAY);
	glVertexPointer(3, GL_FLOAT, 0, &themesh->vertices[0][0]);

	// Set up for color
	switch (color_style) {
		case COLOR_WHITE:
			glColor3f(1,1,1);
			break;
		case COLOR_GRAY:
			glColor3f(0.65, 0.65, 0.65);
			break;
		case COLOR_CURV:
			if (curv_colors.empty())
				compute_curv_colors();
			glEnableClientState(GL_COLOR_ARRAY);
			glColorPointer(3, GL_FLOAT, 0,
				&curv_colors[0][0]);
			break;
		case COLOR_GCURV:
			if (gcurv_colors.empty())
				compute_gcurv_colors();
			glEnableClientState(GL_COLOR_ARRAY);
			glColorPointer(3, GL_FLOAT, 0,
				&gcurv_colors[0][0]);
			break;
		case COLOR_MESH:
			glEnableClientState(GL_COLOR_ARRAY);
			glColorPointer(3, GL_FLOAT, 0,
				&themesh->colors[0][0]);
			break;
	}

	// Set up for lighting
	vector<float> ndotl;
	if (use_3dtexc) {
		glEnableClientState(GL_TEXTURE_COORD_ARRAY);
		glTexCoordPointer(3, GL_FLOAT, 0, &themesh->normals[0][0]);
	}
	if (lighting_style != LIGHTING_NONE) {
		// Set up texture
		static GLuint texture_contexts[nlighting_styles];
		static bool havetextures = false;
		if (!havetextures) {
			make_light_textures(texture_contexts);
			havetextures = true;
		}

		// Compute lighting direction -- the Z axis from the widget
		vec lightdir(&lightdir_matrix[8]);
		if (light_wrt_camera)
			lightdir = rot_only(inv(xf)) * lightdir;
		float rotamount = 180.0f / M_PI * acos(lightdir DOT vec(1,0,0));
		vec rotaxis = lightdir CROSS vec(1,0,0);

		// Texture matrix: remap from normals to texture coords
		glMatrixMode(GL_TEXTURE);
		glLoadIdentity();
		glTranslatef(0.5, 0.5, 0); // Remap [-0.5 .. 0.5] -> [0 .. 1]
		glScalef(0.496, 0, 0);     // Remap [-1 .. 1] -> (-0.5 .. 0.5)
		if (use_3dtexc)            // Rotate normals, else see below
			glRotatef(rotamount, rotaxis[0], rotaxis[1], rotaxis[2]);
		glMatrixMode(GL_MODELVIEW);

		// Bind and enable the texturing
		glBindTexture(GL_TEXTURE_2D, texture_contexts[lighting_style]);
		glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
		glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
		glTexEnvi(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
		glHint(GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST);
		glEnable(GL_TEXTURE_2D);

		// On broken hardware, compute 1D tex coords by hand
		if (!use_3dtexc) {
			ndotl.resize(nv);
			for (int i = 0; i < nv; i++)
				ndotl[i] = themesh->normals[i] DOT lightdir;
			glEnableClientState(GL_TEXTURE_COORD_ARRAY);
			glTexCoordPointer(1, GL_FLOAT, 0, &ndotl[0]);
		}
	}


	// Draw the mesh, possibly with color and/or lighting
	glDepthFunc(GL_LESS);
	glEnable(GL_DEPTH_TEST);
	glPolygonOffset(5.0f, 30.0f);
	glEnable(GL_POLYGON_OFFSET_FILL);
	glEnable(GL_CULL_FACE);

	if (use_dlists && !glIsEnabled(GL_COLOR_ARRAY) &&
	    (use_3dtexc || lighting_style == LIGHTING_NONE)) {
		// Draw the geometry - using display list
		if (!glIsList(1)) {
			glNewList(1, GL_COMPILE);
			draw_tstrips();
			glEndList();
		}
		glCallList(1);
	} else {
		// Draw geometry, no display list
		draw_tstrips();
	}

	// Reset everything
	glDisableClientState(GL_COLOR_ARRAY);
	glDisableClientState(GL_TEXTURE_COORD_ARRAY);
	glDisable(GL_CULL_FACE);
	glDisable(GL_TEXTURE_2D);
	glMatrixMode(GL_TEXTURE);
	glLoadIdentity();
	glMatrixMode(GL_MODELVIEW);
	glDisable(GL_POLYGON_OFFSET_FILL);
	glDepthFunc(GL_LEQUAL);
	glDepthMask(GL_FALSE); // Do not remove me, else get dotted lines

	// Draw the mesh edges on top, if requested
	glLineWidth(1);
	if (draw_edges) {
		glPolygonMode(GL_FRONT, GL_LINE);
		glColor3f(0.5, 1.0, 1.0);
		draw_tstrips();
		glPolygonMode(GL_FRONT, GL_FILL);
	}

	// Draw various per-vertex vectors, if requested
	float line_len = 0.5f * themesh->feature_size();
	if (draw_norm) {
		// Normals
		glColor3f(0.7, 0.7, 0);
		glBegin(GL_LINES);
		for (int i = 0; i < nv; i++) {
			glVertex3fv(themesh->vertices[i]);
			glVertex3fv(themesh->vertices[i] +
				    2.0f * line_len * themesh->normals[i]);
		}
		glEnd();
		glPointSize(3);
		glDrawArrays(GL_POINTS, 0, nv);
	}
	if (draw_curv1) {
		// Maximum-magnitude principal direction
		glColor3f(0.2, 0.7, 0.2);
		glBegin(GL_LINES);
		for (int i = 0; i < nv; i++) {
			glVertex3fv(themesh->vertices[i] -
				    line_len * themesh->pdir1[i]);
			glVertex3fv(themesh->vertices[i] +
				    line_len * themesh->pdir1[i]);
		}
		glEnd();
	}
	if (draw_curv2) {
		// Minimum-magnitude principal direction
		glColor3f(0.7, 0.2, 0.2);
		glBegin(GL_LINES);
		for (int i = 0; i < nv; i++) {
			glVertex3fv(themesh->vertices[i] -
				    line_len * themesh->pdir2[i]);
			glVertex3fv(themesh->vertices[i] +
				    line_len * themesh->pdir2[i]);
		}
		glEnd();
	}
	if (draw_asymp) {
		// Asymptotic directions, scaled by sqrt(-K)
		float ascale2 = sqr(5.0f * line_len * feature_size);
		glColor3f(1, 0.5, 0);
		glBegin(GL_LINES);
		for (int i = 0; i < nv; i++) {
			const float &k1 = themesh->curv1[i];
			const float &k2 = themesh->curv2[i];
			float scale2 = -k1 * k2 * ascale2;
			if (scale2 <= 0.0f)
				continue;
			vec ax = sqrt(scale2 * k2 / (k2-k1)) *
				 themesh->pdir1[i];
			vec ay = sqrt(scale2 * k1 / (k1-k2)) *
				 themesh->pdir2[i];
			glVertex3fv(themesh->vertices[i] + ax + ay);
			glVertex3fv(themesh->vertices[i] - ax - ay);
			glVertex3fv(themesh->vertices[i] + ax - ay);
			glVertex3fv(themesh->vertices[i] - ax + ay);
		}
		glEnd();
	}
	if (draw_w) {
		// Projected view direction
		glColor3f(0, 0, 1);
		glBegin(GL_LINES);
		for (int i = 0; i < nv; i++) {
			vec w = viewpos - themesh->vertices[i];
			w -= themesh->normals[i] * (w DOT themesh->normals[i]);
			normalize(w);
			glVertex3fv(themesh->vertices[i]);
			glVertex3fv(themesh->vertices[i] + line_len * w);
		}
		glEnd();
	}
	if (draw_wperp) {
		// Perpendicular to projected view direction
		glColor3f(0, 0, 1);
		glBegin(GL_LINES);
		for (int i = 0; i < nv; i++) {
			vec w = viewpos - themesh->vertices[i];
			w -= themesh->normals[i] * (w DOT themesh->normals[i]);
			vec wperp = themesh->normals[i] CROSS w;
			normalize(wperp);
			glVertex3fv(themesh->vertices[i]);
			glVertex3fv(themesh->vertices[i] + line_len * wperp);
		}
		glEnd();
	}

	glDisableClientState(GL_VERTEX_ARRAY);
}


// Compute per-vertex n dot l, n dot v, radial curvature, and
// derivative of curvature for the current view
void compute_perview(vector<float> &ndotv, vector<float> &kr,
		     vector<float> &sctest_num, vector<float> &sctest_den,
		     vector<float> &shtest_num, vector<float> &q1,
		     vector<vec2> &t1, vector<float> &Dt1q1,
		     bool extra_sin2theta = false)
{
	if (draw_apparent)
		themesh->need_adjacentfaces();

	int nv = themesh->vertices.size();

	float scthresh = sug_thresh / sqr(feature_size);
	float shthresh = sh_thresh / sqr(feature_size);
	bool need_DwKr = (draw_sc || draw_sh || draw_DwKr);

	ndotv.resize(nv);
	kr.resize(nv);
	if (draw_apparent) {
		q1.resize(nv);
		t1.resize(nv);
		Dt1q1.resize(nv);
	}
	if (need_DwKr) {
		sctest_num.resize(nv);
		sctest_den.resize(nv);
		if (draw_sh)
			shtest_num.resize(nv);
	}

	// Compute quantities at each vertex
#pragma omp parallel for
	for (int i = 0; i < nv; i++) {
		// Compute n DOT v
		vec viewdir = viewpos - themesh->vertices[i];
		float rlv = 1.0f / len(viewdir);
		viewdir *= rlv;
		ndotv[i] = viewdir DOT themesh->normals[i];

		float u = viewdir DOT themesh->pdir1[i], u2 = u*u;
		float v = viewdir DOT themesh->pdir2[i], v2 = v*v;

		// Note:  this is actually Kr * sin^2 theta
		kr[i] = themesh->curv1[i] * u2 + themesh->curv2[i] * v2;

		if (draw_apparent) {
			float csc2theta = 1.0f / (u2 + v2);
			compute_viewdep_curv(themesh, i, ndotv[i],
				u2*csc2theta, u*v*csc2theta, v2*csc2theta,
				q1[i], t1[i]);
		}
		if (!need_DwKr)
			continue;

		// Use DwKr * sin(theta) / cos(theta) for cutoff test
		sctest_num[i] = u2 * (     u*themesh->dcurv[i][0] +
				      3.0f*v*themesh->dcurv[i][1]) +
				v2 * (3.0f*u*themesh->dcurv[i][2] +
					   v*themesh->dcurv[i][3]);
		float csc2theta = 1.0f / (u2 + v2);
		sctest_num[i] *= csc2theta;
		float tr = (themesh->curv2[i] - themesh->curv1[i]) *
			   u * v * csc2theta;
		sctest_num[i] -= 2.0f * ndotv[i] * sqr(tr);
		if (extra_sin2theta)
			sctest_num[i] *= u2 + v2;

		sctest_den[i] = ndotv[i];

		if (draw_sh) {
			shtest_num[i] = -sctest_num[i];
			shtest_num[i] -= shthresh * sctest_den[i];
		}
		sctest_num[i] -= scthresh * sctest_den[i];
	}
	if (draw_apparent) {
#pragma omp parallel for
		for (int i = 0; i < nv; i++)
			compute_Dt1q1(themesh, i, ndotv[i], q1, t1, Dt1q1[i]);
	}
}


// Compute gradient of (kr * sin^2 theta) at vertex i
static inline vec gradkr(int i)
{
	vec viewdir = viewpos - themesh->vertices[i];
	float rlen_viewdir = 1.0f / len(viewdir);
	viewdir *= rlen_viewdir;

	float ndotv = viewdir DOT themesh->normals[i];
	float sintheta = sqrt(1.0f - sqr(ndotv));
	float csctheta = 1.0f / sintheta;
	float u = (viewdir DOT themesh->pdir1[i]) * csctheta;
	float v = (viewdir DOT themesh->pdir2[i]) * csctheta;
	float kr = themesh->curv1[i] * u*u + themesh->curv2[i] * v*v;
	float tr = u*v * (themesh->curv2[i] - themesh->curv1[i]);
	float kt = themesh->curv1[i] * (1.0f - u*u) +
		   themesh->curv2[i] * (1.0f - v*v);
	vec w     = u * themesh->pdir1[i] + v * themesh->pdir2[i];
	vec wperp = u * themesh->pdir2[i] - v * themesh->pdir1[i];
	const Vec<4> &C = themesh->dcurv[i];

	vec g = themesh->pdir1[i] * (u*u*C[0] + 2.0f*u*v*C[1] + v*v*C[2]) +
		themesh->pdir2[i] * (u*u*C[1] + 2.0f*u*v*C[2] + v*v*C[3]) -
		2.0f * csctheta * tr * (rlen_viewdir * wperp +
					ndotv * (tr * w + kt * wperp));
	g *= (1.0f - sqr(ndotv));
	g -= 2.0f * kr * sintheta * ndotv * (kr * w + tr * wperp);
	return g;
}


// Find a zero crossing between val0 and val1 by linear interpolation
// Returns 0 if zero crossing is at val0, 1 if at val1, etc.
static inline float find_zero_linear(float val0, float val1)
{
	return val0 / (val0 - val1);
}


// Find a zero crossing using Hermite interpolation
float find_zero_hermite(int v0, int v1, float val0, float val1,
			const vec &grad0, const vec &grad1)
{
	if (unlikely(val0 == val1))
		return 0.5f;

	// Find derivatives along edge (of interpolation parameter in [0,1]
	// which means that e01 doesn't get normalized)
	vec e01 = themesh->vertices[v1] - themesh->vertices[v0];
	float d0 = e01 DOT grad0, d1 = e01 DOT grad1;

	// This next line would reduce val to linear interpolation
	//d0 = d1 = (val1 - val0);

	// Use hermite interpolation:
	//   val(s) = h1(s)*val0 + h2(s)*val1 + h3(s)*d0 + h4(s)*d1
	// where
	//  h1(s) = 2*s^3 - 3*s^2 + 1
	//  h2(s) = 3*s^2 - 2*s^3
	//  h3(s) = s^3 - 2*s^2 + s
	//  h4(s) = s^3 - s^2
	//
	//  val(s)  = [2(val0-val1) +d0+d1]*s^3 +
	//            [3(val1-val0)-2d0-d1]*s^2 + d0*s + val0
	// where
	//
	//  val(0) = val0; val(1) = val1; val'(0) = d0; val'(1) = d1
	//

	// Coeffs of cubic a*s^3 + b*s^2 + c*s + d
	float a = 2 * (val0 - val1) + d0 + d1;
	float b = 3 * (val1 - val0) - 2 * d0 - d1;
	float c = d0, d = val0;

	// -- Find a root by bisection
	// (as Newton can wander out of desired interval)

	// Start with entire [0,1] interval
	float sl = 0.0f, sr = 1.0f, valsl = val0, valsr = val1;

	// Check if we're in a (somewhat uncommon) 3-root situation, and pick
	// the middle root if it happens (given we aren't drawing curvy lines,
	// seems the best approach..)
	//
	// Find extrema of derivative (a -> 3a; b -> 2b, c -> c),
	// and check if they're both in [0,1] and have different signs
	float disc = 4 * b - 12 * a * c;
	if (disc > 0 && a != 0) {
		disc = sqrt(disc);
		float r1 = (-2 * b + disc) / (6 * a);
		float r2 = (-2 * b - disc) / (6 * a);
		if (r1 >= 0 && r1 <= 1 && r2 >= 0 && r2 <= 1) {
			float vr1 = (((a * r1 + b) * r1 + c) * r1) + d;
			float vr2 = (((a * r2 + b) * r2 + c) * r2) + d;
			// When extrema have different signs inside an
			// interval with endpoints with different signs,
			// the middle root is in between the two extrema
			if (vr1 < 0.0f && vr2 >= 0.0f ||
			    vr1 > 0.0f && vr2 <= 0.0f) {
				// 3 roots
				if (r1 < r2) {
					sl = r1;
					valsl = vr1;
					sr = r2;
					valsr = vr2;
				} else {
					sl = r2;
					valsl = vr2;
					sr = r1;
					valsr = vr1;
				}
			}
		}
	}

	// Bisection method (constant number of interations)
	for (int iter = 0; iter < 10; iter++) {
		float sbi = (sl + sr) / 2.0f;
		float valsbi = (((a * sbi + b) * sbi) + c) * sbi + d;

		// Keep the half which has different signs
		if (valsl < 0.0f && valsbi >= 0.0f ||
		    valsl > 0.0f && valsbi <= 0.0f) {
			sr = sbi;
			valsr = valsbi;
		} else {
			sl = sbi;
			valsl = valsbi;
		}
	}

	return 0.5f * (sl + sr);
}


// Draw part of a zero-crossing curve on one triangle face, but only if
// "test_num/test_den" is positive.  v0,v1,v2 are the indices of the 3
// vertices, "val" are the values of the scalar field whose zero
// crossings we are finding, and "test_*" are the values we are testing
// to make sure they are positive.  This function assumes that val0 has
// opposite sign from val1 and val2 - the following function is the
// general one that figures out which one actually has the different sign.
void draw_face_isoline2(int v0, int v1, int v2,
			const vector<float> &val,
			const vector<float> &test_num,
			const vector<float> &test_den,
			bool do_hermite, bool do_test, float fade)
{
	// How far along each edge?
	float w10 = do_hermite ?
		find_zero_hermite(v0, v1, val[v0], val[v1],
				  gradkr(v0), gradkr(v1)) :
		find_zero_linear(val[v0], val[v1]);
	float w01 = 1.0f - w10;
	float w20 = do_hermite ?
		find_zero_hermite(v0, v2, val[v0], val[v2],
				  gradkr(v0), gradkr(v2)) :
		find_zero_linear(val[v0], val[v2]);
	float w02 = 1.0f - w20;

	// Points along edges
	point p1 = w01 * themesh->vertices[v0] + w10 * themesh->vertices[v1];
	point p2 = w02 * themesh->vertices[v0] + w20 * themesh->vertices[v2];

	float test_num1 = 1.0f, test_num2 = 1.0f;
	float test_den1 = 1.0f, test_den2 = 1.0f;
	float z1 = 0.0f, z2 = 0.0f;
	bool valid1 = true;
	if (do_test) {
		// Interpolate to find value of test at p1, p2
		test_num1 = w01 * test_num[v0] + w10 * test_num[v1];
		test_num2 = w02 * test_num[v0] + w20 * test_num[v2];
		if (!test_den.empty()) {
			test_den1 = w01 * test_den[v0] + w10 * test_den[v1];
			test_den2 = w02 * test_den[v0] + w20 * test_den[v2];
		}
		// First point is valid iff num1/den1 is positive,
		// i.e. the num and den have the same sign
		valid1 = ((test_num1 >= 0.0f) == (test_den1 >= 0.0f));
		// There are two possible zero crossings of the test,
		// corresponding to zeros of the num and den
		if ((test_num1 >= 0.0f) != (test_num2 >= 0.0f))
			z1 = test_num1 / (test_num1 - test_num2);
		if ((test_den1 >= 0.0f) != (test_den2 >= 0.0f))
			z2 = test_den1 / (test_den1 - test_den2);
		// Sort and order the zero crossings
		if (z1 == 0.0f)
			z1 = z2, z2 = 0.0f;
		else if (z2 < z1)
			swap(z1, z2);
	}

	// If the beginning of the segment was not valid, and
	// no zero crossings, then whole segment invalid
	if (!valid1 && !z1 && !z2)
		return;

	// Draw the valid piece(s)
	int npts = 0;
	if (valid1) {
		//glColor4f(currcolor[0], currcolor[1], currcolor[2],
		//	  test_num1 / (test_den1 * fade + test_num1));
		//glVertex3fv(p1);
		gl_alpha.push_back(test_num1 / (test_den1 * fade + test_num1));
		gl_vertex.push_back(p1);
		npts++;
	}
	if (z1) {
		float num = (1.0f - z1) * test_num1 + z1 * test_num2;
		float den = (1.0f - z1) * test_den1 + z1 * test_den2;
		//glColor4f(currcolor[0], currcolor[1], currcolor[2],
		//	  num / (den * fade + num));
		//glVertex3fv((1.0f - z1) * p1 + z1 * p2);
		gl_alpha.push_back(num / (den * fade + num));
		gl_vertex.push_back((1.0f - z1) * p1 + z1 * p2);
		npts++;
	}
	if (z2) {
		float num = (1.0f - z2) * test_num1 + z2 * test_num2;
		float den = (1.0f - z2) * test_den1 + z2 * test_den2;
		//glColor4f(currcolor[0], currcolor[1], currcolor[2],
		//	  num / (den * fade + num));
		//glVertex3fv((1.0f - z2) * p1 + z2 * p2);
		gl_alpha.push_back(num / (den * fade + num));
		gl_vertex.push_back((1.0f - z2) * p1 + z2 * p2);
		npts++;
	}
	if (npts != 2) {
		//glColor4f(currcolor[0], currcolor[1], currcolor[2],
		//	  test_num2 / (test_den2 * fade + test_num2));
		//glVertex3fv(p2);
		gl_alpha.push_back(test_num2 / (test_den2 * fade + test_num2));
		gl_vertex.push_back(p2);
	}
}


// See above.  This is the driver function that figures out which of
// v0, v1, v2 has a different sign from the others.
void draw_face_isoline(int v0, int v1, int v2,
		       const vector<float> &val,
		       const vector<float> &test_num,
		       const vector<float> &test_den,
		       const vector<float> &ndotv,
		       bool do_bfcull, bool do_hermite,
		       bool do_test, float fade)
{
	// Backface culling
	if (likely(do_bfcull && ndotv[v0] <= 0.0f &&
		   ndotv[v1] <= 0.0f && ndotv[v2] <= 0.0f))
		return;

	// Quick reject if derivs are negative
	if (do_test) {
		if (test_den.empty()) {
			if (test_num[v0] <= 0.0f &&
			    test_num[v1] <= 0.0f &&
			    test_num[v2] <= 0.0f)
				return;
		} else {
			if (test_num[v0] <= 0.0f && test_den[v0] >= 0.0f &&
			    test_num[v1] <= 0.0f && test_den[v1] >= 0.0f &&
			    test_num[v2] <= 0.0f && test_den[v2] >= 0.0f)
				return;
			if (test_num[v0] >= 0.0f && test_den[v0] <= 0.0f &&
			    test_num[v1] >= 0.0f && test_den[v1] <= 0.0f &&
			    test_num[v2] >= 0.0f && test_den[v2] <= 0.0f)
				return;
		}
	}

	// Figure out which val has different sign, and draw
	if (val[v0] < 0.0f && val[v1] >= 0.0f && val[v2] >= 0.0f ||
	    val[v0] > 0.0f && val[v1] <= 0.0f && val[v2] <= 0.0f)
		draw_face_isoline2(v0, v1, v2,
				   val, test_num, test_den,
				   do_hermite, do_test, fade);
	else if (val[v1] < 0.0f && val[v2] >= 0.0f && val[v0] >= 0.0f ||
		 val[v1] > 0.0f && val[v2] <= 0.0f && val[v0] <= 0.0f)
		draw_face_isoline2(v1, v2, v0,
				   val, test_num, test_den,
				   do_hermite, do_test, fade);
	else if (val[v2] < 0.0f && val[v0] >= 0.0f && val[v1] >= 0.0f ||
		 val[v2] > 0.0f && val[v0] <= 0.0f && val[v1] <= 0.0f)
		draw_face_isoline2(v2, v0, v1,
				   val, test_num, test_den,
				   do_hermite, do_test, fade);
}


// Takes a scalar field and renders the zero crossings, but only where
// test_num/test_den is greater than 0.
void draw_isolines(const vector<float> &val,
		   const vector<float> &test_num,
		   const vector<float> &test_den,
		   const vector<float> &ndotv,
		   bool do_bfcull, bool do_hermite,
		   bool do_test, float fade)
{
	const int *t = &themesh->tstrips[0];
	const int *stripend = t;
	const int *end = t + themesh->tstrips.size();

	// Walk through triangle strips
	while (1) {
		if (unlikely(t >= stripend)) {
			if (unlikely(t >= end))
				return;
			// New strip: each strip is stored as
			// length followed by indices
			stripend = t + 1 + *t;
			// Skip over length plus first two indices of
			// first face
			t += 3;
		}
		// Draw a line if, among the values in this triangle,
		// at least one is positive and one is negative
		const float &v0 = val[*t], &v1 = val[*(t-1)], &v2 = val[*(t-2)];
		if (unlikely((v0 > 0.0f || v1 > 0.0f || v2 > 0.0f) &&
			     (v0 < 0.0f || v1 < 0.0f || v2 < 0.0f)))
			draw_face_isoline(*(t-2), *(t-1), *t,
					  val, test_num, test_den, ndotv,
					  do_bfcull, do_hermite, do_test, fade);
		t++;
	}
}


// Draw part of a ridge/valley curve on one triangle face.  v0,v1,v2
// are the indices of the 3 vertices; this function assumes that the
// curve connects points on the edges v0-v1 and v1-v2
// (or connects point on v0-v1 to center if to_center is true)
void draw_segment_ridge(int v0, int v1, int v2,
			float emax0, float emax1, float emax2,
			float kmax0, float kmax1, float kmax2,
			float thresh, bool to_center)
{
	// Interpolate to find ridge/valley line segment endpoints
	// in this triangle and the curvatures there
	float w10 = fabs(emax0) / (fabs(emax0) + fabs(emax1));
	float w01 = 1.0f - w10;
	point p01 = w01 * themesh->vertices[v0] + w10 * themesh->vertices[v1];
	float k01 = fabs(w01 * kmax0 + w10 * kmax1);

	point p12;
	float k12;
	if (to_center) {
		// Connect first point to center of triangle
		p12 = (themesh->vertices[v0] +
		       themesh->vertices[v1] +
		       themesh->vertices[v2]) / 3.0f;
		k12 = fabs(kmax0 + kmax1 + kmax2) / 3.0f;
	} else {
		// Connect first point to second one (on next edge)
		float w21 = fabs(emax1) / (fabs(emax1) + fabs(emax2));
		float w12 = 1.0f - w21;
		p12 = w12 * themesh->vertices[v1] + w21 * themesh->vertices[v2];
		k12 = fabs(w12 * kmax1 + w21 * kmax2);
	}

	// Don't draw below threshold
	k01 -= thresh;
	if (k01 < 0.0f)
		k01 = 0.0f;
	k12 -= thresh;
	if (k12 < 0.0f)
		k12 = 0.0f;

	// Skip lines that you can't see...
	if (k01 == 0.0f && k12 == 0.0f)
		return;

	// Fade lines
	if (draw_faded) {
		k01 /= (k01 + thresh);
		k12 /= (k12 + thresh);
	} else {
		k01 = k12 = 1.0f;
	}

	// Draw the line segment
	//glColor4f(currcolor[0], currcolor[1], currcolor[2], k01);
	//glVertex3fv(p01);
	//glColor4f(currcolor[0], currcolor[1], currcolor[2], k12);
	//glVertex3fv(p12);
	gl_alpha.push_back(k01);
	gl_vertex.push_back(p01);
	gl_alpha.push_back(k12);
	gl_vertex.push_back(p12);
}


// Draw ridges or valleys (depending on do_ridge) in a triangle v0,v1,v2
// - uses ndotv for backface culling (enabled with do_bfcull)
// - do_test checks for curvature maxima/minina for ridges/valleys
//   (when off, it draws positive minima and negative maxima)
// Note: this computes ridges/valleys every time, instead of once at the
//   start (given they aren't view dependent, this is wasteful)
// Algorithm based on formulas of Ohtake et al., 2004.
void draw_face_ridges(int v0, int v1, int v2,
		      bool do_ridge,
		      const vector<float> &ndotv,
		      bool do_bfcull, bool do_test, float thresh)
{
	// Backface culling
	if (likely(do_bfcull &&
		   ndotv[v0] <= 0.0f && ndotv[v1] <= 0.0f && ndotv[v2] <= 0.0f))
		return;

	// Check if ridge possible at vertices just based on curvatures
	if (do_ridge) {
		if ((themesh->curv1[v0] <= 0.0f) ||
		    (themesh->curv1[v1] <= 0.0f) ||
		    (themesh->curv1[v2] <= 0.0f))
			return;
	} else {
		if ((themesh->curv1[v0] >= 0.0f) ||
		    (themesh->curv1[v1] >= 0.0f) ||
		    (themesh->curv1[v2] >= 0.0f))
			return;
	}

	// Sign of curvature on ridge/valley
	float rv_sign = do_ridge ? 1.0f : -1.0f;

	// The "tmax" are the principal directions of maximal curvature,
	// flipped to point in the direction in which the curvature
	// is increasing (decreasing for valleys).  Note that this
	// is a bit different from the notation in Ohtake et al.,
	// but the tests below are equivalent.
	const float &emax0 = themesh->dcurv[v0][0];
	const float &emax1 = themesh->dcurv[v1][0];
	const float &emax2 = themesh->dcurv[v2][0];
	vec tmax0 = rv_sign * themesh->dcurv[v0][0] * themesh->pdir1[v0];
	vec tmax1 = rv_sign * themesh->dcurv[v1][0] * themesh->pdir1[v1];
	vec tmax2 = rv_sign * themesh->dcurv[v2][0] * themesh->pdir1[v2];

	// We have a "zero crossing" if the tmaxes along an edge
	// point in opposite directions
	bool z01 = ((tmax0 DOT tmax1) <= 0.0f);
	bool z12 = ((tmax1 DOT tmax2) <= 0.0f);
	bool z20 = ((tmax2 DOT tmax0) <= 0.0f);

	if (z01 + z12 + z20 < 2)
		return;

	if (do_test) {
		const point &p0 = themesh->vertices[v0],
			    &p1 = themesh->vertices[v1],
			    &p2 = themesh->vertices[v2];

		// Check whether we have the correct flavor of extremum:
		// Is the curvature increasing along the edge?
		z01 = z01 && ((tmax0 DOT (p1 - p0)) >= 0.0f ||
			      (tmax1 DOT (p1 - p0)) <= 0.0f);
		z12 = z12 && ((tmax1 DOT (p2 - p1)) >= 0.0f ||
			      (tmax2 DOT (p2 - p1)) <= 0.0f);
		z20 = z20 && ((tmax2 DOT (p0 - p2)) >= 0.0f ||
			      (tmax0 DOT (p0 - p2)) <= 0.0f);

		if (z01 + z12 + z20 < 2)
			return;
	}

	// Draw line segment
	const float &kmax0 = themesh->curv1[v0];
	const float &kmax1 = themesh->curv1[v1];
	const float &kmax2 = themesh->curv1[v2];
	if (!z01) {
		draw_segment_ridge(v1, v2, v0,
				   emax1, emax2, emax0,
				   kmax1, kmax2, kmax0,
				   thresh, false);
	} else if (!z12) {
		draw_segment_ridge(v2, v0, v1,
				   emax2, emax0, emax1,
				   kmax2, kmax0, kmax1,
				   thresh, false);
	} else if (!z20) {
		draw_segment_ridge(v0, v1, v2,
				   emax0, emax1, emax2,
				   kmax0, kmax1, kmax2,
				   thresh, false);
	} else {
		// All three edges have crossings -- connect all to center
		draw_segment_ridge(v1, v2, v0,
				   emax1, emax2, emax0,
				   kmax1, kmax2, kmax0,
				   thresh, true);
		draw_segment_ridge(v2, v0, v1,
				   emax2, emax0, emax1,
				   kmax2, kmax0, kmax1,
				   thresh, true);
		draw_segment_ridge(v0, v1, v2,
				   emax0, emax1, emax2,
				   kmax0, kmax1, kmax2,
				   thresh, true);
	}
}


// Draw the ridges (valleys) of the mesh
void draw_mesh_ridges(bool do_ridge, const vector<float> &ndotv,
		      bool do_bfcull, bool do_test, float thresh)
{
	const int *t = &themesh->tstrips[0];
	const int *stripend = t;
	const int *end = t + themesh->tstrips.size();

	// Walk through triangle strips
	while (1) {
		if (unlikely(t >= stripend)) {
			if (unlikely(t >= end))
				return;
			// New strip: each strip is stored as
			// length followed by indices
			stripend = t + 1 + *t;
			// Skip over length plus first two indices of
			// first face
			t += 3;
		}

		draw_face_ridges(*(t-2), *(t-1), *t,
				 do_ridge, ndotv, do_bfcull, do_test, thresh);
		t++;
	}
}


// Draw principal highlights on a face
void draw_face_ph(int v0, int v1, int v2, bool do_ridge,
		  const vector<float> &ndotv, bool do_bfcull,
		  bool do_test, float thresh)
{
	// Backface culling
	if (likely(do_bfcull &&
		   ndotv[v0] <= 0.0f && ndotv[v1] <= 0.0f && ndotv[v2] <= 0.0f))
		return;

	// Orient principal directions based on the largest principal curvature
	float k0 = themesh->curv1[v0];
	float k1 = themesh->curv1[v1];
	float k2 = themesh->curv1[v2];
	if (do_test && do_ridge && min(min(k0,k1),k2) < 0.0f)
		return;
	if (do_test && !do_ridge && max(max(k0,k1),k2) > 0.0f)
		return;

	vec d0 = themesh->pdir1[v0];
	vec d1 = themesh->pdir1[v1];
	vec d2 = themesh->pdir1[v2];
	float kmax = fabs(k0);
        // dref is the e1 vector with the largest |k1|
	vec dref = d0;
	if (fabs(k1) > kmax)
		kmax = fabs(k1), dref = d1;
	if (fabs(k2) > kmax)
		kmax = fabs(k2), dref = d2;
        
        // Flip all the e1 to agree with dref
	if ((d0 DOT dref) < 0.0f) d0 = -d0;
	if ((d1 DOT dref) < 0.0f) d1 = -d1;
	if ((d2 DOT dref) < 0.0f) d2 = -d2;

        // If directions have flipped (more than 45 degrees), then give up
        if ((d0 DOT dref) < M_SQRT1_2 ||
            (d1 DOT dref) < M_SQRT1_2 ||
            (d2 DOT dref) < M_SQRT1_2)
          return;

	// Compute view directions, dot products @ each vertex
	vec viewdir0 = viewpos - themesh->vertices[v0];
	vec viewdir1 = viewpos - themesh->vertices[v1];
	vec viewdir2 = viewpos - themesh->vertices[v2];

        // Normalize these for cos(theta) later...
        normalize(viewdir0);
        normalize(viewdir1);
        normalize(viewdir2);

        // e1 DOT w sin(theta) 
        // -- which is zero when looking down e2
	float dot0 = viewdir0 DOT d0;
	float dot1 = viewdir1 DOT d1;
	float dot2 = viewdir2 DOT d2;

	// We have a "zero crossing" if the dot products along an edge
	// have opposite signs
	int z01 = (dot0*dot1 <= 0.0f);
	int z12 = (dot1*dot2 <= 0.0f);
	int z20 = (dot2*dot0 <= 0.0f);

	if (z01 + z12 + z20 < 2)
		return;

	// Draw line segment
	float test0 = (sqr(themesh->curv1[v0]) - sqr(themesh->curv2[v0])) *
                      viewdir0 DOT themesh->normals[v0];
	float test1 = (sqr(themesh->curv1[v1]) - sqr(themesh->curv2[v1])) *
                      viewdir0 DOT themesh->normals[v1];
	float test2 = (sqr(themesh->curv1[v2]) - sqr(themesh->curv2[v2])) *
                      viewdir0 DOT themesh->normals[v2];

	if (!z01) {
		draw_segment_ridge(v1, v2, v0,
				   dot1, dot2, dot0,
				   test1, test2, test0,
				   thresh, false);
	} else if (!z12) {
		draw_segment_ridge(v2, v0, v1,
				   dot2, dot0, dot1,
				   test2, test0, test1,
				   thresh, false);
	} else if (!z20) {
		draw_segment_ridge(v0, v1, v2,
				   dot0, dot1, dot2,
				   test0, test1, test2,
				   thresh, false);
	}
}


// Draw principal highlights
void draw_mesh_ph(bool do_ridge, const vector<float> &ndotv, bool do_bfcull,
		  bool do_test, float thresh)
{
	const int *t = &themesh->tstrips[0];
	const int *stripend = t;
	const int *end = t + themesh->tstrips.size();

	// Walk through triangle strips
	while (1) {
		if (unlikely(t >= stripend)) {
			if (unlikely(t >= end))
				return;
			// New strip: each strip is stored as
			// length followed by indices
			stripend = t + 1 + *t;
			// Skip over length plus first two indices of
			// first face
			t += 3;
		}

		draw_face_ph(*(t-2), *(t-1), *t, do_ridge,
			     ndotv, do_bfcull, do_test, thresh);
		t++;
	}
}


// Draw exterior silhouette of the mesh: this just draws
// thick contours, which are partially hidden by the mesh.
// Note: this needs to happen *before* draw_base_mesh...
void draw_silhouette(const vector<float> &ndotv)
{
	glDepthMask(GL_FALSE);

	currcolor = vec(0.0, 0.0, 0.0);
	glLineWidth(6);
	glBegin(GL_LINES);
	draw_isolines(ndotv, vector<float>(), vector<float>(), ndotv,
		      false, false, false, 0.0f);
	glEnd();

	// Wide lines are gappy, so fill them in
	glEnable(GL_POINT_SMOOTH);
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glPointSize(6);
	glBegin(GL_POINTS);
	draw_isolines(ndotv, vector<float>(), vector<float>(), ndotv,
		      false, false, false, 0.0f);
	glEnd();

	glDisable(GL_POINT_SMOOTH);
	glDisable(GL_BLEND);
	glDepthMask(GL_TRUE);
}


// Draw the boundaries on the mesh
void draw_boundaries(bool do_hidden)
{
	themesh->need_faces();
	themesh->need_across_edge();
	if (do_hidden) {
		glColor3f(0.6, 0.6, 0.6);
		glLineWidth(1.5);
	} else {
		glColor3f(0.05, 0.05, 0.05);
		glLineWidth(2.5);
	}
	glBegin(GL_LINES);
	for (int i = 0; i < themesh->faces.size(); i++) {
		for (int j = 0; j < 3; j++) {
			if (themesh->across_edge[i][j] >= 0)
				continue;
			int v1 = themesh->faces[i][(j+1)%3];
			int v2 = themesh->faces[i][(j+2)%3];
			//glVertex3fv(themesh->vertices[v1]);
			//glVertex3fv(themesh->vertices[v2]);
			gl_alpha.push_back(1.0);
			gl_vertex.push_back(themesh->vertices[v1]);
			gl_alpha.push_back(1.0);
			gl_vertex.push_back(themesh->vertices[v2]);
		}
	}
	glEnd();
}


// Draw lines of n.l = const.
void draw_isophotes(const vector<float> &ndotv)
{
	// Light direction
	vec lightdir(&lightdir_matrix[8]);
	if (light_wrt_camera)
		lightdir = rot_only(inv(xf)) * lightdir;

	// Compute N dot L
	int nv = themesh->vertices.size();
	static vector<float> ndotl;
	ndotl.resize(nv);
	for (int i = 0; i < nv; i++)
		ndotl[i] = themesh->normals[i] DOT lightdir;

	if (draw_colors)
		currcolor = vec(0.4, 0.8, 0.4);
	else
		currcolor = vec(0.6, 0.6, 0.6);
	glColor3fv(currcolor);

	float dt = 1.0f / niso;
	for (int it = 0; it < niso; it++) {
		if (it == 0) {
			glLineWidth(2);
		} else {
			glLineWidth(1);
			for (int i = 0; i < nv; i++)
				ndotl[i] -= dt;
		}
		glBegin(GL_LINES);
		draw_isolines(ndotl, vector<float>(), vector<float>(),
			      ndotv, true, false, false, 0.0f);
		glEnd();
	}

        // Draw negative isophotes (useful when light is not at camera)
	if (draw_colors)
		currcolor = vec(0.6, 0.9, 0.6);
	else
		currcolor = vec(0.7, 0.7, 0.7);
	glColor3fv(currcolor);

	for (int i = 0; i < nv; i++)
		ndotl[i] += dt * (niso-1);
	for (int it = 1; it < niso; it++) {
		glLineWidth(1.0);
		for (int i = 0; i < nv; i++)
			ndotl[i] += dt;
		glBegin(GL_LINES);
		draw_isolines(ndotl, vector<float>(), vector<float>(),
			      ndotv, true, false, false, 0.0f);
		glEnd();
	}
}


// Draw lines of constant depth
void draw_topolines(const vector<float> &ndotv)
{
	// Camera direction and scale
	vec camdir(xf[2], xf[6], xf[10]);
	float depth_scale = 0.5f / themesh->bsphere.r * ntopo;
	float depth_offset = 0.5f * ntopo - topo_offset;

	// Compute depth
	static vector<float> depth;
	int nv = themesh->vertices.size();
	depth.resize(nv);
	for (int i = 0; i < nv; i++) {
		depth[i] = ((themesh->vertices[i] - themesh->bsphere.center)
			     DOT camdir) * depth_scale + depth_offset;
	}

	// Draw the topo lines
	glLineWidth(1);
	glColor3f(0.5, 0.5, 0.5);
	for (int it = 0; it < ntopo; it++) {
		glBegin(GL_LINES);
		draw_isolines(depth, vector<float>(), vector<float>(),
			      ndotv, true, false, false, 0.0f);
		glEnd();
		for (int i = 0; i < nv; i++)
			depth[i] -= 1.0f;
	}
}


// Draw K=0, H=0, and DwKr=thresh lines
void draw_misc(const vector<float> &ndotv, const vector<float> &DwKr,
	       bool do_hidden)
{
	if (do_hidden) {
		currcolor = vec(1, 0.5, 0.5);
		glLineWidth(1);
	} else {
		currcolor = vec(1, 0, 0);
		glLineWidth(2);
	}

	int nv = themesh->vertices.size();
	if (draw_K) {
		vector<float> K(nv);
		for (int i = 0; i < nv; i++)
			K[i] = themesh->curv1[i] * themesh->curv2[i];
		glBegin(GL_LINES);
		draw_isolines(K, vector<float>(), vector<float>(), ndotv,
			      !do_hidden, false, false, 0.0f);
		glEnd();
	}
	if (draw_H) {
		vector<float> H(nv);
		for (int i = 0; i < nv; i++)
			H[i] = 0.5f * (themesh->curv1[i] + themesh->curv2[i]);
		glBegin(GL_LINES);
		draw_isolines(H, vector<float>(), vector<float>(), ndotv,
			      !do_hidden, false, false, 0.0f);
		glEnd();
	}
	if (draw_DwKr) {
		glBegin(GL_LINES);
		draw_isolines(DwKr, vector<float>(), vector<float>(), ndotv,
			      !do_hidden, false, false, 0.0f);
		glEnd();
	}
}


// Draw the mesh, possibly including a bunch of lines
void draw_mesh()
{
	// These are static so the memory isn't reallocated on every frame
	static vector<float> ndotv, kr;
	static vector<float> sctest_num, sctest_den, shtest_num;
	static vector<float> q1, Dt1q1;
	static vector<vec2> t1;
	compute_perview(ndotv, kr, sctest_num, sctest_den, shtest_num,
		q1, t1, Dt1q1, use_texture);
	int nv = themesh->vertices.size();

	// Enable antialiased lines
	glEnable(GL_POINT_SMOOTH);
	glEnable(GL_LINE_SMOOTH);
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	// Exterior silhouette
	if (draw_extsil)
		draw_silhouette(ndotv);

	// The mesh itself, possibly colored and/or lit
	glDisable(GL_BLEND);
	draw_base_mesh();
	glEnable(GL_BLEND);

	// Draw the lines on top

	// First rendering pass (in light gray) if drawing hidden lines
	if (draw_hidden) {
		glDisable(GL_DEPTH_TEST);

		// K=0, H=0, DwKr=thresh
		draw_misc(ndotv, sctest_num, true);

		// Apparent ridges
		if (draw_apparent) {
			if (draw_colors) {
				currcolor = vec(0.8, 0.8, 0.4);
			} else {
				if (color_style == COLOR_GRAY ||
				    lighting_style != LIGHTING_NONE)
					currcolor = vec(0.75, 0.75, 0.75);
				else
					currcolor = vec(0.55, 0.55, 0.55);
			}
			if (draw_colors)
			glLineWidth(2);
			glBegin(GL_LINES);
			draw_mesh_app_ridges(ndotv, q1, t1, Dt1q1, true,
				test_ar, ar_thresh / sqr(feature_size));
			glEnd();
		}

		// Ridges and valleys
		currcolor = vec(0.55, 0.55, 0.55);
		if (draw_ridges) {
			if (draw_colors)
				currcolor = vec(0.72, 0.6, 0.72);
			glLineWidth(1);
			glBegin(GL_LINES);
			draw_mesh_ridges(true, ndotv, false, test_rv,
					 rv_thresh / feature_size);
			glEnd();
		}
		if (draw_valleys) {
			if (draw_colors)
				currcolor = vec(0.8, 0.72, 0.68);
			glLineWidth(1);
			glBegin(GL_LINES);
			draw_mesh_ridges(false, ndotv, false, test_rv,
					 rv_thresh / feature_size);
			glEnd();
		}

		// Principal highlights
		if (draw_phridges || draw_phvalleys) {
			if (draw_colors) {
				currcolor = vec(0.5,0,0);
			} else {
				if (color_style == COLOR_GRAY ||
				    lighting_style != LIGHTING_NONE)
					currcolor = vec(0.75, 0.75, 0.75);
				else
					currcolor = vec(0.55, 0.55, 0.55);
			}
			glLineWidth(2);
			glBegin(GL_LINES);
			float thresh = ph_thresh / sqr(feature_size);
			if (draw_phridges)
				draw_mesh_ph(true, ndotv, false, test_ph, thresh);
			if (draw_phvalleys)
				draw_mesh_ph(false, ndotv, false, test_ph, thresh);
			glEnd();
		}

		// Suggestive highlights
		if (draw_sh) {
			if (draw_colors) {
				currcolor = vec(0.5,0,0);
			} else {
				if (color_style == COLOR_GRAY ||
				    lighting_style != LIGHTING_NONE)
					currcolor = vec(0.75, 0.75, 0.75);
				else
					currcolor = vec(0.55,0.55,0.55);
			}
			float fade = draw_faded ? 0.03f / sqr(feature_size) : 0.0f;
			glLineWidth(2.5);
			glBegin(GL_LINES);
			draw_isolines(kr, shtest_num, sctest_den, ndotv,
				      false, use_hermite, test_sh, fade);
			glEnd();
		}

		// Suggestive contours and contours
		if (draw_sc) {
			float fade = (draw_faded && test_sc) ?
				     0.03f / sqr(feature_size) : 0.0f;
			if (draw_colors)
				currcolor = vec(0.5, 0.5, 1.0);
			glLineWidth(1.5);
			glBegin(GL_LINES);
			draw_isolines(kr, sctest_num, sctest_den, ndotv,
				      false, use_hermite, test_sc, fade);
			glEnd();
		}

		if (draw_c) {
			if (draw_colors)
				currcolor = vec(0.4, 0.8, 0.4);
			glLineWidth(1.5);
			glBegin(GL_LINES);
			draw_isolines(ndotv, kr, vector<float>(), ndotv,
				      false, false, test_c, 0.0f);
			glEnd();
		}

		// Boundaries
		if (draw_bdy)
			draw_boundaries(true);

		glEnable(GL_DEPTH_TEST);
	}


	// The main rendering pass

	// Isophotes
	if (draw_isoph)
		draw_isophotes(ndotv);

	// Topo lines
	if (draw_topo)
		draw_topolines(ndotv);

	// K=0, H=0, DwKr=thresh
	draw_misc(ndotv, sctest_num, false);

	// Apparent ridges
	currcolor = vec(0.0, 0.0, 0.0);
	if (draw_apparent) {
		if (draw_colors)
			currcolor = vec(0.4, 0.4, 0);
		glLineWidth(2.5);
		glBegin(GL_LINES);
		draw_mesh_app_ridges(ndotv, q1, t1, Dt1q1, true,
			test_ar, ar_thresh / sqr(feature_size));
		glEnd();
	}

	// Ridges and valleys
	currcolor = vec(0.0, 0.0, 0.0);
	float rvfade = draw_faded ? rv_thresh / feature_size : 0.0f;
	if (draw_ridges) {
		if (draw_colors)
			currcolor = vec(0.3, 0.0, 0.3);
		glLineWidth(2);
		glBegin(GL_LINES);
		draw_mesh_ridges(true, ndotv, true, test_rv,
				 rv_thresh / feature_size);
		glEnd();
	}
	if (draw_valleys) {
		if (draw_colors)
			currcolor = vec(0.5, 0.3, 0.2);
		glLineWidth(2);
		glBegin(GL_LINES);
		draw_mesh_ridges(false, ndotv, true, test_rv,
				 rv_thresh / feature_size);
		glEnd();
	}

	// Principal highlights
	if (draw_phridges || draw_phvalleys) {
		if (draw_colors) {
			currcolor = vec(0.5, 0, 0);
		} else {
			if (color_style == COLOR_GRAY ||
                            lighting_style != LIGHTING_NONE)
				currcolor = vec(1, 1, 1);
			else
				currcolor = vec(0, 0, 0);
		}
		glLineWidth(2);
		glBegin(GL_LINES);
		float thresh = ph_thresh / sqr(feature_size);
		if (draw_phridges)
			draw_mesh_ph(true, ndotv, true, test_ph, thresh);
		if (draw_phvalleys)
			draw_mesh_ph(false, ndotv, true, test_ph, thresh);
		glEnd();
		currcolor = vec(0.0, 0.0, 0.0);
	}

	// Suggestive highlights
    if (draw_sh) {
		if (draw_colors) {
			currcolor = vec(0.5,0,0);
		} else {
			if (color_style == COLOR_GRAY ||
                            lighting_style != LIGHTING_NONE)
				currcolor = vec(1.0, 1.0, 1.0);
			else
				currcolor = vec(0.3,0.3,0.3);
		}
		float fade = draw_faded ? 0.03f / sqr(feature_size) : 0.0f;
		glLineWidth(2.5);
		glBegin(GL_LINES);
		draw_isolines(kr, shtest_num, sctest_den, ndotv,
			      true, use_hermite, test_sh, fade);
		glEnd();
		currcolor = vec(0.0, 0.0, 0.0);
    }

	// Kr = 0 loops
	if (draw_sc && !test_sc && !draw_hidden) {
		if (draw_colors)
			currcolor = vec(0.5, 0.5, 1.0);
		else
			currcolor = vec(0.6, 0.6, 0.6);
		glLineWidth(1.5);
		glBegin(GL_LINES);
		draw_isolines(kr, sctest_num, sctest_den, ndotv,
			      true, use_hermite, false, 0.0f);
		glEnd();
		currcolor = vec(0.0, 0.0, 0.0);
	}

	// Suggestive contours and contours
	if (draw_sc && !use_texture) {
		float fade = draw_faded ? 0.03f / sqr(feature_size) : 0.0f;
		if (draw_colors)
			currcolor = vec(0.0, 0.0, 0.8);
		glLineWidth(2.5);
		glBegin(GL_LINES);
		draw_isolines(kr, sctest_num, sctest_den, ndotv,
			      true, use_hermite, true, fade);
		glEnd();
	}
	if (draw_c && !use_texture) {
		if (draw_colors)
			currcolor = vec(0.0, 0.6, 0.0);
		glLineWidth(2.5);
		glBegin(GL_LINES);
		draw_isolines(ndotv, kr, vector<float>(), ndotv,
			      false, false, true, 0.0f);
		glEnd();
	}
	if ((draw_sc || draw_c) && use_texture)
		draw_c_sc_texture(ndotv, kr, sctest_num, sctest_den);

	// Boundaries
	if (draw_bdy)
		draw_boundaries(false);

	glDisable(GL_LINE_SMOOTH);
	glDisable(GL_POINT_SMOOTH);
	glDisable(GL_BLEND);
	glDepthMask(GL_TRUE);
}


// Signal a redraw
void need_redraw()
{
	glutPostRedisplay();
}


// Clear the screen and reset OpenGL modes to something sane
void cls()
{
	glDisable(GL_DITHER);
	glDisable(GL_BLEND);
	glDisable(GL_DEPTH_TEST);
	glDisable(GL_NORMALIZE);
	glDisable(GL_LIGHTING);
	glDisable(GL_COLOR_MATERIAL);
	glClearColor(1,1,1,0);
	if (color_style == COLOR_GRAY)
		glClearColor(0.8, 0.8, 0.8, 0);
	glClearDepth(1);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
}


// Set up viewport and scissoring for the subwindow, and optionally draw
// a box around it (actually, just clears a rectangle one pixel bigger
// to black).  Assumes current viewport is set up for the whole window.
void set_subwindow_viewport(bool draw_box = false)
{
	GLint V[4];
	glGetIntegerv(GL_VIEWPORT, V);
	GLint x = V[0], y = V[1], w = V[2], h = V[3];
	int boxsize = min(w, h) / 3;

	x += w - boxsize*11/10;
	y += h - boxsize*11/10;
	w = h = boxsize;

	if (draw_box) {
		glViewport(x-1, y-1, w+2, h+2);
		glScissor(x-1, y-1, w+2, h+2);
		glClearColor(0,0,0,0);
		glEnable(GL_SCISSOR_TEST);
		glClear(GL_COLOR_BUFFER_BIT);
		glScissor(x, y, w, h);
	}

	glViewport(x, y, w, h);
}


// Draw the scene
void redraw()
{
	timestamp t = now();
	viewpos = inv(xf) * point(0,0,0);
	GLUI_Master.auto_set_viewport();

	// If dual viewports, first draw in window using camera_alt
	if (dual_vpmode) {
		// Set up camera and clear the screen
		glMatrixMode(GL_PROJECTION);
		glLoadMatrixd(alt_projmatrix);
		camera_alt.setupGL(xf_alt * themesh->bsphere.center,
				   themesh->bsphere.r);
		glGetDoublev(GL_PROJECTION_MATRIX, alt_projmatrix);
		cls();

		// Transform and draw
		glPushMatrix();
		glMultMatrixd((double *)xf_alt);
		draw_mesh();
		glPopMatrix();

		// Set viewport and draw a box for the subwindow
		set_subwindow_viewport(true);

		// Now we're ready to draw in the subwindow
	}

	camera.setupGL(xf * themesh->bsphere.center, themesh->bsphere.r);

	cls();

	// Transform and draw
	glPushMatrix();
	glMultMatrixd((double *)xf);
	draw_mesh();
	glPopMatrix();

	glDisable(GL_SCISSOR_TEST);
	glutSwapBuffers();
	printf("\rElapsed time: %.2f msec.", 1000.0f * (now() - t));
	fflush(stdout);

	// See if we need to autospin the camera(s)
	if (camera.autospin(xf))
		need_redraw();
	if (dual_vpmode) {
		if (camera_alt.autospin(xf_alt))
			need_redraw();
	} else {
		camera_alt = camera;
		xf_alt = xf;
	}
}


// Set the view to look at the middle of the mesh, from reasonably far away
void resetview()
{
	camera.stopspin();
	camera_alt.stopspin();

	if (!xf.read(xffilename))
		xf = xform::trans(0, 0, -3.5f / fov * themesh->bsphere.r) *
		     xform::trans(-themesh->bsphere.center);
	camera_alt = camera;
	xf_alt = xf;

	// Reset light position too
	lightdir->reset();
}


// Smooth the mesh
void filter_mesh(int dummy = 0)
{
	printf("\r");  fflush(stdout);
	smooth_mesh(themesh, currsmooth);

	if (use_dlists) {
	    glDeleteLists(1,1);
	}
	themesh->pointareas.clear();
	themesh->normals.clear();
	themesh->curv1.clear();
	themesh->dcurv.clear();
	themesh->need_normals();
	themesh->need_curvatures();
	themesh->need_dcurv();
	curv_colors.clear();
	gcurv_colors.clear();
	currsmooth *= 1.1f;
}


// Diffuse the normals across the mesh
void filter_normals(int dummy = 0)
{
	printf("\r");  fflush(stdout);
	diffuse_normals(themesh, currsmooth);
	themesh->curv1.clear();
	themesh->dcurv.clear();
	themesh->need_curvatures();
	themesh->need_dcurv();
	curv_colors.clear();
	gcurv_colors.clear();
	currsmooth *= 1.1f;
}


// Diffuse the curvatures across the mesh
void filter_curv(int dummy = 0)
{
	printf("\r");  fflush(stdout);
	diffuse_curv(themesh, currsmooth);
	themesh->dcurv.clear();
	themesh->need_dcurv();
	curv_colors.clear();
	gcurv_colors.clear();
	currsmooth *= 1.1f;
}


// Diffuse the curvature derivatives across the mesh
void filter_dcurv(int dummy = 0)
{
	printf("\r");  fflush(stdout);
	diffuse_dcurv(themesh, currsmooth);
	curv_colors.clear();
	gcurv_colors.clear();
	currsmooth *= 1.1f;
}


// Perform an iteration of subdivision
void subdivide_mesh(int dummy = 0)
{
	printf("\r");  fflush(stdout);
	subdiv(themesh);

	if (use_dlists) {
	    glDeleteLists(1,1);
	}
	themesh->need_tstrips();
	themesh->need_normals();
	themesh->need_pointareas();
	themesh->need_curvatures();
	themesh->need_dcurv();
	curv_colors.clear();
	gcurv_colors.clear();
}


// Save the current image to a PPM file
void dump_image(int dummy = 0)
{
	// Find first non-used filename
	const char filenamepattern[] = "img%d.ppm";
	int imgnum = 0;
	FILE *f;
	while (1) {
		char filename[1024];
		sprintf(filename, filenamepattern, imgnum++);
		f = fopen(filename, "rb");
		if (!f) {
			f = fopen(filename, "wb");
			printf("\n\nSaving image %s... ", filename);
			fflush(stdout);
			break;
		}
		fclose(f);
	}

	// Read pixels
	GLUI_Master.auto_set_viewport();
	GLint V[4];
	glGetIntegerv(GL_VIEWPORT, V);
	GLint width = V[2], height = V[3];
	char *buf = new char[width*height*3];
	glPixelStorei(GL_PACK_ALIGNMENT, 1);
	glReadPixels(V[0], V[1], width, height, GL_RGB, GL_UNSIGNED_BYTE, buf);

	// Flip top-to-bottom
	for (int i = 0; i < height/2; i++) {
		char *row1 = buf + 3 * width * i;
		char *row2 = buf + 3 * width * (height - 1 - i);
		for (int j = 0; j < 3 * width; j++)
			swap(row1[j], row2[j]);
	}

	// Write out file
	fprintf(f, "P6\n%d %d\n255\n", width, height);
	fwrite(buf, width*height*3, 1, f);
	fclose(f);
	delete [] buf;

	printf("Done.\n\n");
}


// Compute a "feature size" for the mesh: computed as 1% of
// the reciprocal of the 10-th percentile curvature
void compute_feature_size()
{
	int nv = themesh->curv1.size();
	int nsamp = min(nv, 500);

	vector<float> samples;
	samples.reserve(nsamp * 2);

	for (int i = 0; i < nsamp; i++) {
		// Quick 'n dirty portable random number generator
		static unsigned randq = 0;
		randq = unsigned(1664525) * randq + unsigned(1013904223);

		int ind = randq % nv;
		samples.push_back(fabs(themesh->curv1[ind]));
		samples.push_back(fabs(themesh->curv2[ind]));
	}

	const float frac = 0.1f;
	const float mult = 0.01f;
	themesh->need_bsphere();
	float max_feature_size = 0.05f * themesh->bsphere.r;

	int which = int(frac * samples.size());
	nth_element(samples.begin(), samples.begin() + which, samples.end());

	feature_size = min(mult / samples[which], max_feature_size);
}


// Handle mouse button and motion events
static unsigned buttonstate = 0;
static const unsigned ctrl_pressed = 1 << 30;

void mousemotionfunc(int x, int y)
{
	// Ctrl+mouse = relight
	if (buttonstate & ctrl_pressed) {
		GLUI_Master.auto_set_viewport();
		GLint V[4];
		glGetIntegerv(GL_VIEWPORT, V);
		y = V[1] + V[3] - 1 - y; // Adjust for top-left vs. bottom-left
		float xx = 2.0f * float(x - V[0]) / float(V[2]) - 1.0f;
		float yy = 2.0f * float(y - V[1]) / float(V[3]) - 1.0f;
		float theta = M_PI * min(sqrt(xx*xx+yy*yy), 1.0f);
		float phi = atan2(yy, xx);
		XForm<float> lightxf = lightxf.rot(phi, 0, 0, 1) *
				       lightxf.rot(theta, 0, 1, 0);
		lightdir->set_float_array_val((float *) lightxf);
		need_redraw();
		return;
	}

	static const Mouse::button physical_to_logical_map[] = {
		Mouse::NONE, Mouse::ROTATE, Mouse::MOVEXY, Mouse::MOVEZ,
		Mouse::MOVEZ, Mouse::MOVEXY, Mouse::MOVEXY, Mouse::MOVEXY,
	};
	Mouse::button b = Mouse::NONE;
	if (buttonstate & (1 << 3))
		b = Mouse::WHEELUP;
	else if (buttonstate & (1 << 4))
		b = Mouse::WHEELDOWN;
	else
		b = physical_to_logical_map[buttonstate & 7];

	if (dual_vpmode && mouse_moves_alt) {
		GLUI_Master.auto_set_viewport();
		glMatrixMode(GL_PROJECTION);
		glLoadMatrixd(alt_projmatrix);
		camera_alt.setupGL(xf_alt * themesh->bsphere.center,
				   themesh->bsphere.r);
		camera_alt.mouse(x, y, b,
				 xf_alt * themesh->bsphere.center,
				 themesh->bsphere.r, xf_alt);
	} else {
		camera.mouse(x, y, b,
			     xf * themesh->bsphere.center,
			     themesh->bsphere.r, xf);
	}

	need_redraw();
	GLUI_Master.sync_live_all();
}

void mousebuttonfunc(int button, int state, int x, int y)
{
	if (state == GLUT_DOWN)
		buttonstate |= (1 << button);
	else
		buttonstate &= ~(1 << button);

	if (glutGetModifiers() & GLUT_ACTIVE_CTRL)
		buttonstate |= ctrl_pressed;
	else
		buttonstate &= ~ctrl_pressed;

	// On a mouse click in dual_vpmode, check if we're in subwindow
	if (dual_vpmode && state == GLUT_DOWN && !(buttonstate & ctrl_pressed)) {
		GLUI_Master.auto_set_viewport();
		GLint V[4];
		glGetIntegerv(GL_VIEWPORT, V);
		int yy = V[1] + V[3] - 1 - y; // top-left vs. bottom-left
		set_subwindow_viewport();
		glGetIntegerv(GL_VIEWPORT, V);
		mouse_moves_alt = !(x > V[0] && yy > V[1] &&
				    x < V[0] + V[2] && yy < V[1] + V[3]);
	}

	mousemotionfunc(x, y);
}


#define Ctrl (1-'a')

// Keyboard callback
void keyboardfunc(unsigned char key, int x, int y)
{
	switch (key) {
		case ' ':
			resetview(); break;
		case 'a':
			draw_asymp = !draw_asymp; break;
		case 'A':
			draw_apparent = !draw_apparent; break;
		case 'b':
			draw_bdy = !draw_bdy; break;
		case 'B':
			test_c = !test_c; break;
		case 'c':
			draw_colors = !draw_colors; break;
		case 'C':
			filter_curv(); break;
		case 'd':
			draw_c = !draw_c; break;
		case 'D':
			draw_sc = !draw_sc; break;
		case 'e':
			draw_edges = !draw_edges; break;
		case 'E':
			draw_extsil = !draw_extsil; break;
		case 'f':
			draw_faded = !draw_faded; break;
                case 'F':                        
                        draw_sh = !draw_sh; break;
		case 'g':
			use_hermite = !use_hermite; break;
		case 'h':
			draw_hidden = !draw_hidden; break;
		case 'H':
			draw_H = !draw_H; break;
		case 'i':
			draw_isoph = !draw_isoph; break;
		case 'I':
			dump_image(); break;
		case 'K':
			draw_K = !draw_K; break;
		case 'l':
			lighting_style++;
			lighting_style %= nlighting_styles;
			break;
		case 'n':
			draw_norm = !draw_norm; break;
		case 'r':
			draw_phridges = !draw_phridges; break;
		case Ctrl+'r':
			draw_ridges = !draw_ridges; break;
		case 'R':
			test_rv = !test_rv; break;
		case 's':
			filter_normals(); break;
		case 'S':
			filter_mesh(); break;
		case 't':
			use_texture = !use_texture; break;
		case 'T':
			test_sc = !test_sc; break;
		case 'u':
			color_style++;
			color_style %= ncolor_styles;
			if (color_style == COLOR_MESH &&
			    themesh->colors.empty())
				color_style = COLOR_WHITE;
			break;
		case 'v':
			draw_phvalleys = !draw_phvalleys; break;
		case Ctrl+'v':
			draw_valleys = !draw_valleys; break;
		case 'V':
		case Ctrl+'s':
			subdivide_mesh(); break;
		case 'w':
			draw_w = !draw_w; break;
		case Ctrl+'w':
			draw_wperp = !draw_wperp; break;
		case 'W':
		case Ctrl+'d':
			draw_DwKr = !draw_DwKr; break;
		case 'x':
			xf.write(xffilename); break;
		case 'X':
			filter_dcurv(); break;
		case 'z':
			fov /= 1.1f; camera.set_fov(fov); break;
		case 'Z':
			fov *= 1.1f; camera.set_fov(fov); break;
		case '/':
			dual_vpmode = !dual_vpmode; break;
		case '+':
		case '=':
			niso++; break;
		case '-':
		case '_':
			if (niso > 1) niso--; break;
		case '1':
			draw_curv1 = !draw_curv1; break;
		case '2':
			draw_curv2 = !draw_curv2; break;
		case '7':
			rv_thresh /= 1.1f; break;
		case '8':
			rv_thresh *= 1.1f; break;
		case '9':
			sug_thresh /= 1.1f; break;
		case '0':
			sug_thresh *= 1.1f; break;
		case '\033': // Esc
		case '\021': // Ctrl-Q
		case 'Q':
		case 'q':
			exit(0);
	}
	need_redraw();
	GLUI_Master.sync_live_all();
}

void skeyboardfunc(int key, int x, int y)
{
	switch (key) {
		case GLUT_KEY_UP:
			sug_thresh *= 1.1f; break;
		case GLUT_KEY_DOWN:
			sug_thresh /= 1.1f; break;

		case GLUT_KEY_RIGHT:
			rv_thresh *= 1.1f; break;
		case GLUT_KEY_LEFT:
			rv_thresh /= 1.1f; break;
	}
	need_redraw();
	GLUI_Master.sync_live_all();
}


// Reshape the window.  We clear the window here to possibly avoid some
// weird problems.  Yuck.
void reshape(int x, int y)
{
	GLUI_Master.auto_set_viewport();
	cls();
	glutSwapBuffers();
	need_redraw();
}


void usage(const char *myname)
{
	fprintf(stderr, "Usage: %s [-options] infile\n", myname);
	exit(1);
}

int main(int argc, char *argv[])
{
    int wwid = 820, wht = 700;

    for (int j = 1; j < argc; j++) {
		if (argv[j][0] == '+') {
			sscanf(argv[j]+1, "%d,%d,%f,%f", &wwid, &wht,
					&sug_thresh, &ph_thresh);
		}
	}

	glutInitWindowSize(wwid, wht);
	glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGBA | GLUT_DEPTH);
	glutInit(&argc, argv);

	argc = 2;

	if (argc < 2)
		usage(argv[0]);

	// Skip over any parameter beginning with a '-' or '+'
	int i = 1;
	while (i < argc-1 && argv[i][0] == '-' && argv[i][0] == '+') {
		i++;
		if (!strcmp(argv[i-1], "--"))
			break;
	}
	

	const char *filename = argv[1];
	themesh = TriMesh::read(filename);

	if (!themesh)
		usage(argv[0]);

	xffilename = new char[strlen(filename) + 4];
	strcpy(xffilename, filename);
	char *dot = strrchr(xffilename, '.');
	if (!dot)
		dot = strrchr(xffilename, 0);
	strcpy(dot, ".xf");

	themesh->need_tstrips();
	themesh->need_bsphere();
	themesh->need_normals();
	themesh->need_curvatures();
	themesh->need_dcurv();
	compute_feature_size();
	currsmooth = 0.5f * themesh->feature_size();

	char windowname[255];
	sprintf(windowname, "RTSC - %s", filename);
	int main_win = glutCreateWindow(windowname);

	glutDisplayFunc(redraw);
	GLUI_Master.set_glutMouseFunc(mousebuttonfunc);
	glutMotionFunc(mousemotionfunc);
	GLUI_Master.set_glutKeyboardFunc(keyboardfunc);
	GLUI_Master.set_glutSpecialFunc(skeyboardfunc);
	GLUI_Master.set_glutReshapeFunc(reshape);

	GLUI *glui = GLUI_Master.create_glui_subwindow(main_win, GLUI_SUBWINDOW_BOTTOM);
	glui->set_main_gfx_window(main_win);
	GLUI_Rollout *g = glui->add_rollout("Options", false);
	glui->add_statictext_to_panel(g, "Lines:");
	glui->add_checkbox_to_panel(g, "Exterior silhouette", &draw_extsil);
	glui->add_checkbox_to_panel(g, "Occluding contours", &draw_c);
	glui->add_checkbox_to_panel(g, "Suggestive contours", &draw_sc);

	glui->add_checkbox_to_panel(g, "Suggestive hlt.", &draw_sh);
	glui->add_checkbox_to_panel(g, "Principal hlt. (R)", &draw_phridges);
	glui->add_checkbox_to_panel(g, "Principal hlt. (V)", &draw_phvalleys);

	glui->add_checkbox_to_panel(g, "Ridges", &draw_ridges);
	glui->add_checkbox_to_panel(g, "Valleys", &draw_valleys);

	glui->add_checkbox_to_panel(g, "Apparent ridges", &draw_apparent);

	glui->add_checkbox_to_panel(g, "K = 0", &draw_K);
	glui->add_checkbox_to_panel(g, "H = 0", &draw_H);
	glui->add_checkbox_to_panel(g, "DwKr = thresh.", &draw_DwKr);

	glui->add_checkbox_to_panel(g, "Boundaries", &draw_bdy);

	glui->add_checkbox_to_panel(g, "Isophotes", &draw_isoph);
	GLUI_Spinner *spinner = glui->add_spinner_to_panel(g, "# Isophotes ",
						  GLUI_SPINNER_INT, &niso);
	spinner->set_int_limits(1, 100);
	spinner->edittext->set_w(120);

	glui->add_checkbox_to_panel(g, "Topo lines", &draw_topo);
	spinner = glui->add_spinner_to_panel(g, "# Topo lines",
					     GLUI_SPINNER_INT, &ntopo);
	spinner->set_int_limits(1, 100);
	spinner->edittext->set_w(120);
	glui->add_slider_to_panel(g, "Topo offset", GLUI_SLIDER_FLOAT,
				  -1.0, 1.0, &topo_offset)->set_w(5);

	glui->add_column_to_panel(g, false);
	glui->add_statictext_to_panel(g, "Line tests:");
	glui->add_checkbox_to_panel(g, "Draw hidden lines", &draw_hidden);
	glui->add_checkbox_to_panel(g, "Trim \"inside\" contours", &test_c);
	glui->add_checkbox_to_panel(g, "Trim SC", &test_sc);
	glui->add_slider_to_panel(g, "SC thresh", GLUI_SLIDER_FLOAT,
				  0.0, 0.1, &sug_thresh);
	glui->add_checkbox_to_panel(g, "Trim SH", &test_sh);
	glui->add_slider_to_panel(g, "SH thresh", GLUI_SLIDER_FLOAT,
				  0.0, 0.1, &sh_thresh);
	glui->add_checkbox_to_panel(g, "Trim PH", &test_ph);
	glui->add_slider_to_panel(g, "PH thresh", GLUI_SLIDER_FLOAT,
				  0.0, 0.2, &ph_thresh);
	glui->add_checkbox_to_panel(g, "Trim RV", &test_rv);
	glui->add_slider_to_panel(g, "RV thresh", GLUI_SLIDER_FLOAT,
				  0.0, 0.5, &rv_thresh);
	glui->add_checkbox_to_panel(g, "Trim AR", &test_ar);
	glui->add_slider_to_panel(g, "AR thresh", GLUI_SLIDER_FLOAT,
				  0.0, 0.5, &ar_thresh);

	glui->add_column_to_panel(g, false);
	glui->add_statictext_to_panel(g, "Line style:");
	glui->add_checkbox_to_panel(g, "Texture mapping", &use_texture);
	glui->add_checkbox_to_panel(g, "Fade lines", &draw_faded);
	glui->add_checkbox_to_panel(g, "Draw in color", &draw_colors);
	glui->add_checkbox_to_panel(g, "Hermite interp", &use_hermite);

	glui->add_statictext_to_panel(g, " ");
	glui->add_statictext_to_panel(g, "Mesh style:");
	GLUI_RadioGroup *r = glui->add_radiogroup_to_panel(g, &color_style);
	glui->add_radiobutton_to_group(r, "White");
	glui->add_radiobutton_to_group(r, "Gray");
	glui->add_radiobutton_to_group(r, "Curvature (color)");
	glui->add_radiobutton_to_group(r, "Curvature (gray)");
	if (!themesh->colors.empty())
		glui->add_radiobutton_to_group(r, "Mesh colors");
	glui->add_checkbox_to_panel(g, "Draw edges", &draw_edges);

	glui->add_column_to_panel(g, false);
	glui->add_statictext_to_panel(g, "Lighting:");
	r = glui->add_radiogroup_to_panel(g, &lighting_style);
	glui->add_radiobutton_to_group(r, "None");
	glui->add_radiobutton_to_group(r, "Lambertian");
	glui->add_radiobutton_to_group(r, "Lambertian2");
	glui->add_radiobutton_to_group(r, "Hemisphere");
	glui->add_radiobutton_to_group(r, "Toon (gray/white)");
	glui->add_radiobutton_to_group(r, "Toon (black/white)");
	glui->add_radiobutton_to_group(r, "Gooch");
	lightdir = glui->add_rotation_to_panel(g, "Direction",
					       (float *)&lightdir_matrix);
	glui->add_checkbox_to_panel(g, "On camera", &light_wrt_camera);
	lightdir->reset();

	glui->add_column_to_panel(g, false);
	glui->add_statictext_to_panel(g, "Vectors:");
	glui->add_checkbox_to_panel(g, "Normal", &draw_norm);
	glui->add_checkbox_to_panel(g, "Principal 1", &draw_curv1);
	glui->add_checkbox_to_panel(g, "Principal 2", &draw_curv2);
	glui->add_checkbox_to_panel(g, "Asymptotic", &draw_asymp);
	glui->add_checkbox_to_panel(g, "Proj. View", &draw_w);

	glui->add_statictext_to_panel(g, " ");
	glui->add_statictext_to_panel(g, "Camera:");
	glui->add_checkbox_to_panel(g, "Dual viewport", &dual_vpmode);

	glui->add_column_to_panel(g, false);
	glui->add_button_to_panel(g, "Smooth Mesh", 0, filter_mesh);
	glui->add_button_to_panel(g, "Smooth Normals", 0, filter_normals);
	glui->add_button_to_panel(g, "Smooth Curv", 0, filter_curv);
	glui->add_button_to_panel(g, "Smooth DCurv", 0, filter_dcurv);
	glui->add_button_to_panel(g, "Subdivide Mesh", 0, subdivide_mesh);
	glui->add_button_to_panel(g, "Screencap", 0, dump_image);
	glui->add_button_to_panel(g, "Exit", 0, exit);

	// Go through command-line arguments and do what they say.
	// Any command line options are just interpreted as keyboard commands.

	/*
	for (i = 1; i < argc-1; i++) {
		if (argv[i][0] != '-' || !strcmp(argv[i], "--"))
			break;
		for (int j = 1; j < strlen(argv[i]); j++)
			keyboardfunc(argv[i][j], 0, 0);
	}
	*/
	resetview();

	glutMainLoop();
}

//-------------------------------------------------------------------------
//---------------------------------CLR-------------------------------------
//-------------------------------------------------------------------------

vector<float> ndotv, kr;
vector<float> sctest_num, sctest_den, shtest_num;
vector<float> q1, Dt1q1;
vector<vec2> t1;

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace GraphicResearchHuiZhao;

public ref class RtscSettings
{
public:
	// Toggles for drawing various lines
	int draw_extsil, draw_c, draw_sc;
	int draw_sh, draw_phridges, draw_phvalleys;
	int draw_ridges, draw_valleys, draw_apparent;
	int draw_K, draw_H, draw_DwKr;
	int draw_bdy, draw_isoph, draw_topo;
	
	// Toggles for tests we perform
	int test_c, test_sc, test_sh, test_ph, test_rv, test_ar;
	float sug_thresh, sh_thresh, ph_thresh;
	float rv_thresh, ar_thresh;
	
	// Toggles for style
	int use_texture;
	int draw_faded;
	int draw_colors;
	int use_hermite;
	int draw_hidden;

	RtscSettings()
	{
		// Toggles for drawing various lines
		draw_extsil = 0, draw_c = 1, draw_sc = 1;
		draw_sh = 0, draw_phridges = 0, draw_phvalleys = 0;
		draw_ridges = 0, draw_valleys = 0, draw_apparent = 0;
		draw_K = 0, draw_H = 0, draw_DwKr = 0;
		draw_bdy = 0, draw_isoph = 0, draw_topo = 0;
		
		// Toggles for tests we perform
		test_c = 1, test_sc = 1, test_sh = 1, test_ph = 1, test_rv = 1, test_ar = 1;
		sug_thresh = 0.01, sh_thresh = 0.02, ph_thresh = 0.04;
		rv_thresh = 0.1, ar_thresh = 0.1;
		
		// Toggles for style
		use_texture = 0;
		draw_faded = 1;
		draw_colors = 0;
		use_hermite = 0;
		draw_hidden = 0;
	}

	void DrawAll()
	{
		this -> draw_extsil = 1, this -> draw_c = 1, this -> draw_sc = 1;
		this -> draw_sh = 1, this -> draw_phridges = 1, this -> draw_phvalleys = 1;
		this -> draw_ridges = 1, this -> draw_valleys = 1, this -> draw_apparent = 1;
		this -> draw_K = 1, this -> draw_H = 1, this -> draw_DwKr = 1;
		this -> draw_bdy = 1, this -> draw_isoph = 1, this -> draw_topo = 1;
	}

	void SetUp()
	{
		::draw_extsil = this -> draw_extsil;
		::draw_c = this -> draw_c;
		::draw_sc = this -> draw_sc;
		::draw_sh = this -> draw_sh;
		::draw_phridges = this -> draw_phridges;
		::draw_phvalleys = this -> draw_phvalleys;
		::draw_ridges = this -> draw_ridges;
		::draw_valleys = this -> draw_valleys;
		::draw_apparent = this -> draw_apparent;
		::draw_K = this -> draw_K;
		::draw_H = this -> draw_H;
		::draw_DwKr = this -> draw_DwKr;
		::draw_bdy = this -> draw_bdy;
		::draw_isoph = this -> draw_isoph;
		::draw_topo = this -> draw_topo;

		::sug_thresh = this -> sug_thresh;
		::sh_thresh = this -> sh_thresh;
		::ph_thresh = this -> ph_thresh;
		::rv_thresh = this -> rv_thresh;
		::ar_thresh = this -> ar_thresh;

		::use_hermite = this -> use_hermite;
		::draw_hidden = this -> draw_hidden;
	}
};

public ref class Rtsc
{
public:
	int nv;
	BoundingSphere bsphere;
	BoundingBox bbox;
	array<Vector3D> ^normals;
	array<double> ^curv1, ^curv2;
	array<Vector3D> ^pdir1, ^pdir2;
	array<Vector4D> ^dcurv;
	
	double feature_size;
	array<Vector3D> ^curv_colors, ^gcurv_colors;

	array<double> ^ndotv, ^kr;
	array<double> ^sctest_num, ^sctest_den, ^shtest_num;
	array<double> ^q1, ^Dt1q1;
	array<Vector2D> ^t1;

	array<float> ^gl_alpha;
	array<Vector3D> ^gl_vertex;
	int front_index;

	Rtsc(System::String ^filename)
	{
		char *p = (char*)(void*)Marshal::StringToHGlobalAnsi(filename);
		::themesh = TriMesh::read(p);
		Marshal::FreeHGlobal(IntPtr(p));
	}

	void InitTriMesh()
	{
		::themesh->need_tstrips();
		::themesh->need_bsphere();
		::themesh->need_normals();
		::themesh->need_curvatures();
		::themesh->need_dcurv();
		
		this -> nv = ::themesh -> vertices.size();
		this -> bsphere = BoundingSphere(VecToVector(::themesh -> bsphere.center), ::themesh -> bsphere.r);
		this -> bbox = BoundingBox(VecToVector(::themesh -> bbox.min), VecToVector(::themesh -> bbox.max));

		this -> normals = gcnew array<Vector3D>(this -> nv);
		this -> curv1 = gcnew array<double>(this -> nv);
		this -> curv2 = gcnew array<double>(this -> nv);
		this -> pdir1 = gcnew array<Vector3D>(this -> nv);
		this -> pdir2 = gcnew array<Vector3D>(this -> nv);
		this -> dcurv = gcnew array<Vector4D>(this -> nv);

		for (int i = 0; i < this -> nv; i++)
		{
			this -> normals[i] = VecToVector(::themesh -> normals[i]);
			this -> curv1[i] = ::themesh -> curv1[i];
			this -> curv2[i] = ::themesh -> curv2[i];
			this -> pdir1[i] = VecToVector(::themesh -> pdir1[i]);
			this -> pdir2[i] = VecToVector(::themesh -> pdir2[i]);
			this -> dcurv[i] = VecToVector(::themesh -> dcurv[i]);
		}

		::compute_feature_size();
		this -> feature_size = ::feature_size;
		::compute_curv_colors();
		::compute_gcurv_colors();
		this -> curv_colors = gcnew array<Vector3D>(this -> nv);
		this -> gcurv_colors = gcnew array<Vector3D>(this -> nv);
		for (int i = 0; i < this -> nv; i++)
		{
			this -> curv_colors[i] = VecToVector(::curv_colors[i]);
			this -> gcurv_colors[i] = VecToVector(::gcurv_colors[i]);
		}
	}

	void InitRtsc(Vector3D viewpos)
	{
		::viewpos = point((float)viewpos.x, (float)viewpos.y, (float)viewpos.z);
		bool need_DwKr = (::draw_sc || ::draw_sh || ::draw_DwKr);

		::compute_perview(::ndotv, ::kr, ::sctest_num, ::sctest_den, ::shtest_num,
			::q1, ::t1, ::Dt1q1, ::use_texture);

		this -> ndotv = gcnew array<double>(this -> nv);
		this -> kr = gcnew array<double>(this -> nv);
		this -> sctest_num = gcnew array<double>(this -> nv);
		this -> sctest_den = gcnew array<double>(this -> nv);
		this -> shtest_num = gcnew array<double>(this -> nv);
		this -> q1 = gcnew array<double>(this -> nv);
		this -> Dt1q1 = gcnew array<double>(this -> nv);
		this -> t1 = gcnew array<Vector2D>(this -> nv);

		for (int i = 0; i < this -> nv; i++)
		{
			this -> ndotv[i] = ::ndotv[i];
			this -> kr[i] = ::kr[i];
			if(need_DwKr)
			{
				this -> sctest_num[i] = ::sctest_num[i];
				this -> sctest_den[i] = ::sctest_den[i];
				if(::draw_sh)
				{
					this -> shtest_num[i] = ::shtest_num[i];
				}
			}
			if(::draw_apparent)
			{
				this -> q1[i] = ::q1[i];
				this -> Dt1q1[i] = ::Dt1q1[i];
				this -> t1[i] = Vector2D(::t1[i][0], ::t1[i][1]);
			}
		}
	}

	Vector3D Gradkr(int i)
	{
		return VecToVector(gradkr(i));
	}

	void DrawExteriorSilhouette()
	{
		this -> DrawBegin();
		::draw_isolines(::ndotv, vector<float>(), vector<float>(), ::ndotv,
				      false, false, false, 0.0f);
		this -> DrawEnd();
	}

	void DrawOccludingContours()
	{
		this -> DrawBegin();
		if(::draw_hidden)
		{
			::draw_isolines(::ndotv, ::kr, vector<float>(), ::ndotv,
					      false, false, ::test_c, 0.0f);
		}
		this -> DrawFront();
		::draw_isolines(::ndotv, ::kr, vector<float>(), ::ndotv,
				true, false, true, 0.0f);
		this -> DrawEnd();
	}

	void DrawSuggestiveContours()
	{
		this -> DrawBegin();
		if(::draw_hidden)
		{
			float fade = (::draw_faded && ::test_sc) ?
					     0.03f / sqr(::feature_size) : 0.0f;
			::draw_isolines(::kr, ::sctest_num, ::sctest_den, ::ndotv,
					     false, ::use_hermite, ::test_sc, fade);
		}
		this -> DrawFront();
		float fade = ::draw_faded ? 0.03f / sqr(::feature_size) : 0.0f;
		::draw_isolines(::kr, ::sctest_num, ::sctest_den, ::ndotv,
				true, ::use_hermite, true, fade);
		this -> DrawEnd();
	}

	void DrawSuggestiveHighlights()
	{
		this -> DrawBegin();
		if(::draw_hidden)
		{
			float fade = ::draw_faded ? 0.03f / sqr(::feature_size) : 0.0f;
			::draw_isolines(::kr, ::shtest_num, ::sctest_den, ::ndotv,
					      false, ::use_hermite, ::test_sh, fade);
		}
		this -> DrawFront();
		float fade = ::draw_faded ? 0.03f / sqr(::feature_size) : 0.0f;
		::draw_isolines(::kr, ::shtest_num, ::sctest_den, ::ndotv,
					  true, ::use_hermite, ::test_sh, fade);
		this -> DrawEnd();
	}

	void DrawIsophotes(Vector3D lightdir, int niso)
	{
		vec dir((float)lightdir.x, (float)lightdir.y, (float)lightdir.z);

		// Compute N dot L
		int nv = ::themesh->vertices.size();
		vector<float> ndotl;
		ndotl.resize(nv);
		for (int i = 0; i < nv; i++)
			ndotl[i] = ::themesh->normals[i] DOT dir;

		this -> DrawBegin();
		this -> DrawFront();
		float dt = len(dir) / niso;
		for (int it = 0; it < niso; it++) {
			if (it > 0) {
				for (int i = 0; i < nv; i++)
					ndotl[i] -= dt;
			}
			::draw_isolines(ndotl, vector<float>(), vector<float>(),
				      ::ndotv, true, false, false, 0.0f);
		}

        // Draw negative isophotes (useful when light is not at camera)
		for (int i = 0; i < nv; i++)
			ndotl[i] += dt * (niso-1);
		for (int it = 1; it < niso; it++) {
			for (int i = 0; i < nv; i++)
				ndotl[i] += dt;
			::draw_isolines(ndotl, vector<float>(), vector<float>(),
				      ::ndotv, true, false, false, 0.0f);
		}
		this -> DrawEnd();
	}

	void DrawTopolines(int ntopo, double topo_offset)
	{
		// Camera direction and scale
		vec dir = ::viewpos;
		float depth_scale = 0.5f / ::themesh->bsphere.r * ntopo;
		float depth_offset = 0.5f * ntopo - topo_offset;

		// Compute depth
		vector<float> depth;
		int nv = ::themesh->vertices.size();
		depth.resize(nv);
		for (int i = 0; i < nv; i++) {
			depth[i] = ((::themesh->vertices[i] - ::themesh->bsphere.center)
				     DOT dir) * depth_scale + depth_offset;
		}

		// Draw the topo lines
		this -> DrawBegin();
		this -> DrawFront();
		float dt = len(dir);
		for (int it = 0; it < ntopo; it++) {
			::draw_isolines(depth, vector<float>(), vector<float>(),
				      ::ndotv, true, false, false, 0.0f);
			for (int i = 0; i < nv; i++)
				depth[i] -= dt;
		}
		this -> DrawEnd();
	}

	void DrawRidges()
	{
		this -> DrawBegin();
		if(::draw_hidden)
		{
			::draw_mesh_ridges(true, ::ndotv, false, test_rv,
					 rv_thresh / feature_size);
		}
		this -> DrawFront();
		::draw_mesh_ridges(true, ::ndotv, true, ::test_rv,
				 ::rv_thresh / ::feature_size);
		this -> DrawEnd();
	}

	void DrawValleys()
	{
		this -> DrawBegin();
		if(::draw_hidden)
		{
			::draw_mesh_ridges(false, ::ndotv, false, test_rv,
					 rv_thresh / feature_size);
		}
		this -> DrawFront();
		::draw_mesh_ridges(false, ::ndotv, true, ::test_rv,
				 ::rv_thresh / ::feature_size);
		this -> DrawEnd();
	}

	void DrawPrincipalHighlightRidges()
	{
		this -> DrawBegin();
		float thresh = ::ph_thresh / sqr(::feature_size);
		if(::draw_hidden)
		{
			::draw_mesh_ph(true, ::ndotv, false, ::test_ph, thresh);
		}
		this -> DrawFront();
		::draw_mesh_ph(true, ::ndotv, true, ::test_ph, thresh);
		this -> DrawEnd();
	}

	void DrawPrincipalHighlightValleys()
	{
		this -> DrawBegin();
		float thresh = ::ph_thresh / sqr(::feature_size);
		if(::draw_hidden)
		{
			::draw_mesh_ph(false, ::ndotv, false, ::test_ph, thresh);
		}
		this -> DrawFront();
		::draw_mesh_ph(false, ::ndotv, true, ::test_ph, thresh);
		this -> DrawEnd();
	}

	void DrawApparentRidges()
	{
		this -> DrawBegin();
		if(::draw_hidden)
		{
			::draw_mesh_app_ridges(::ndotv, ::q1, ::t1, ::Dt1q1, false,
				::test_ar, ::ar_thresh / sqr(::feature_size));
		}
		this -> DrawFront();
		::draw_mesh_app_ridges(::ndotv, ::q1, ::t1, ::Dt1q1, true,
			::test_ar, ::ar_thresh / sqr(::feature_size));
		this -> DrawEnd();
	}

	void DrawK()
	{
		vector<float> K(nv);
		for (int i = 0; i < nv; i++)
			K[i] = themesh->curv1[i] * themesh->curv2[i];
		this -> DrawBegin();
		if(::draw_hidden)
		{
			draw_isolines(K, vector<float>(), vector<float>(), ::ndotv,
					false, false, false, 0.0f);
		}
		this -> DrawFront();
		draw_isolines(K, vector<float>(), vector<float>(), ::ndotv,
				true, false, false, 0.0f);
		this -> DrawEnd();
	}

	void DrawH()
	{
		vector<float> H(nv);
		for (int i = 0; i < nv; i++)
			H[i] = 0.5f * (themesh->curv1[i] + themesh->curv2[i]);
		this -> DrawBegin();
		if(::draw_hidden)
		{
			draw_isolines(H, vector<float>(), vector<float>(), ::ndotv,
					false, false, false, 0.0f);
		}
		this -> DrawFront();
		draw_isolines(H, vector<float>(), vector<float>(), ::ndotv,
				true, false, false, 0.0f);
		this -> DrawEnd();
	}

	void DrawDwKr()
	{
		this -> DrawBegin();
		if(::draw_hidden)
		{
			draw_isolines(::sctest_num, vector<float>(), vector<float>(), ::ndotv,
					false, false, false, 0.0f);
		}
		this -> DrawFront();
		draw_isolines(::sctest_num, vector<float>(), vector<float>(), ::ndotv,
				true, false, false, 0.0f);
		this -> DrawEnd();
	}

	void DrawBoundaries()
	{
		this -> DrawBegin();
		if(::draw_hidden)
		{
			::draw_boundaries(true);
		}
		this -> DrawFront();
		::draw_boundaries(false);
		this -> DrawEnd();
	}

private:
	static inline Vector3D VecToVector(Vec<3, float> p)
	{
		return Vector3D(p[0], p[1], p[2]);
	}

	static inline Vector4D VecToVector(Vec<4, float> p)
	{
		return Vector4D(p[0], p[1], p[2], p[3]);
	}

	void DrawBegin()
	{
		::gl_alpha.clear();
		::gl_vertex.clear();
	}

	void DrawFront()
	{
		this -> front_index = ::gl_vertex.size();
	}

	void DrawEnd()
	{
		this -> gl_alpha = gcnew array<float>(::gl_alpha.size());
		this -> gl_vertex = gcnew array<Vector3D>(::gl_vertex.size());
		for (int i = 0; i < ::gl_alpha.size(); i++)
		{
			this -> gl_alpha[i] = ::gl_alpha[i];
			this -> gl_vertex[i] = VecToVector(::gl_vertex[i]);
		}
	}
};