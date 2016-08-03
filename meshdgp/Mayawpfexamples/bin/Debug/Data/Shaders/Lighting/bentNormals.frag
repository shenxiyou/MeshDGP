//
// Fragment shader for ambient occlusion rendering.
//
// Author: Philip Rideout
//
// Copyright (C) 2005 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

const float PI = 3.1415926535;
uniform samplerCube cubemap;

varying vec3 Vertex;
varying vec3 BentNormal;
varying float Accessibility;

void main(void)
{
    vec3 tCoord = vec3(gl_TextureMatrix[0] * vec4(BentNormal, 1.0));
    float blur = sqrt(Accessibility);

    // the bigger the bias, the blurrier the image
    // the max value is implementation-dependent and should be passed in via a uniform
    // values that are too high result in an ugly flat-shaded look...I'm not sure why.
    float bias = 3.0 * blur;

/*
    // GLSL doesn't let you explicitly set LOD from the FSU; you must bias it instead.
    // We work around this by computing the original LOD value and subtracting it.
    // TODO this appears to be 0 always -- file a compiler bug!
    float u = tCoord.x;
    float v = tCoord.y;
    float dudx = dFdx(u);
    float dudy = dFdy(u);
    float dvdx = dFdx(v);
    float dvdy = dFdy(v);
    float lod = max(sqrt(dudx * dudx + dvdx * dvdx), sqrt(dudy * dudy + dvdy * dvdy));
    bias -= lod;
*/

    tCoord = localize(vec3(Vertex), tCoord);
    vec3 color = Accessibility * mix(gl_Color.rgb, vec3(textureCube(cubemap, tCoord, bias)), 0.25);
    gl_FragColor = vec4(computeShadow() * color, gl_Color.a);
}
