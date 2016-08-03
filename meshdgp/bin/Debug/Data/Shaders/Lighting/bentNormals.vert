//
// Vertex shader for ambient occlusion rendering.
//
// Author: Philip Rideout
//
// Copyright (C) 2005 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

varying vec3 BentNormal;
varying vec3 Vertex;
varying float Accessibility;

void main(void)
{
    BentNormal = gl_NormalMatrix * bent_normal;
    Vertex = vec3(gl_Vertex);
    Accessibility = accessibility;

    gl_FrontColor  = gl_Color;
    gl_Position    = ftransform();
    shadowSetup();
}
