//
// Vertex shader for simple accessibility rendering.
//
// Author: Philip Rideout
//
// Copyright (C) 2005 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

void main(void)
{
    gl_FrontColor  = vec4(accessibility * vec3(gl_Color), 1.0);
    gl_Position    = ftransform();
    shadowSetup();
}