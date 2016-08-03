//
// Vertex shader for neighborhood averaging via convolution
//
// Author: Randi Rost
//
// Copyright (c) 2003-2005: 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

varying vec2 TexCoord;

void main()

{
    gl_Position    = ftransform();
     gl_TexCoord[0]  = gl_MultiTexCoord0;
}
