//
// Fragment shader for simple accessibility rendering.
//
// Author: Philip Rideout
//
// Copyright (C) 2005 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

void main(void)
{
    gl_FragColor = vec4(gl_Color.rgb * computeShadow(), gl_Color.a);
}