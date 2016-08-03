//
// Vertex shader for rendering contact points.
//
// Author: Philip Rideout
//
// Copyright (C) 2005 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

void main(void)
{
    float alpha = (accessibility > 0.75) ? 0.0 : (0.75 - accessibility) * 1.33;
    gl_FrontColor  = vec4(0, 0, 0, alpha);
    gl_Position    = ftransform();
}