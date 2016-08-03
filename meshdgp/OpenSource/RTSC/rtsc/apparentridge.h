#ifndef APPARENTRIDGE_H
#define APPARENTRIDGE_H
/*
Original by Tilke Judd
Tweaks by Szymon Rusinkiewicz

apparentridge.h
Compute apparent ridges.

Implements method of
  Judd, T., Durand, F, and Adelson, E.
  Apparent Ridges for Line Drawing,
  ACM Trans. Graphics (Proc. SIGGRAPH), vol. 26, no. 3, 2007.
*/
//zdd++ Email:zddmail@gmail.com
#include <vector>
using namespace std;


// Compute principal view dependent curvatures and directions at vertex i.
void compute_viewdep_curv(const TriMesh *mesh, int i, float ndotv,
			  float u2, float uv, float v2,
			  float &q1, vec2 &t1);

// Compute D_{t_1} q_1 - the derivative of max view-dependent curvature
// in the principal max view-dependent curvature direction.
void compute_Dt1q1(const TriMesh *mesh, int i, float ndotv,
		   const vector<float> &q1, const vector<vec2> &t1,
		   float &Dt1q1);

// Draw apparent ridges of the mesh
void draw_mesh_app_ridges(const vector<float> &ndotv, const vector<float> &q1,
			  const vector<vec2> &t1, const vector<float> &Dt1q1,
			  bool do_bfcull, bool do_test, float thresh);

#endif
