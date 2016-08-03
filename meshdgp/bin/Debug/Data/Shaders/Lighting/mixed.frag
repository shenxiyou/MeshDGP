//
// Fragment shader for mixing diffuse and ambient occlusion lighting.
//
// Author: Philip Rideout
//
// Copyright (C) 2005 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

void main(void)
{
    gl_FragColor = vec4(computeShadow() * gl_Color.rgb, gl_Color.a);
}