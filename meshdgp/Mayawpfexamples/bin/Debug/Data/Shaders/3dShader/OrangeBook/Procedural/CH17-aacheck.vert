//
// Fragment shader for an antialiased checkerboard
//
// Authors: Dave Baldwin, Randi Rost
//          based on a RenderMan shader by Larry Gritz
//
// Copyright (c) 2002-2005 3Dlabs Inc. Ltd. 
//
// See 3Dlabs-License.txt for license information
//

varying vec2 TexCoord;

void main()
{
    gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;

    if(gl_MultiTexCoord0.s==0&&gl_MultiTexCoord0.t==0)
	     TexCoord = gl_Vertex.xy;
	else
         TexCoord    = gl_MultiTexCoord0.st;
    
}