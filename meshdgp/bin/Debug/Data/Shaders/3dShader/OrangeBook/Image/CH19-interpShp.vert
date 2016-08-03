//
// Vertex shader for modifying image sharpness by
// interpolation and extrapolation
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
    gl_Position = ftransform();
   TexCoord  = gl_MultiTexCoord0.st;
}
