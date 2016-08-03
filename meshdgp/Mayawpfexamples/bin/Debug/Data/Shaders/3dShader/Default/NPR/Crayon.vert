//
// Vertex shader for cartoon-style shading
//
// Author: Philip Rideout
//
// Copyright (c) 2005-2006 3Dlabs Inc. Ltd.
//
// See 3Dlabs-License.txt for license information
//
varying vec2 TexCoord;
 varying vec3 MCposition ;
void main(void)
{
 
   TexCoord = gl_MultiTexCoord0.st;
   MCposition= vec3(gl_Vertex);
   gl_Position = ftransform();
}
