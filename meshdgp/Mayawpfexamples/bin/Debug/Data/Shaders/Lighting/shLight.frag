//
// Fragment shader for spherical harmonic lighting
//
// Author: Randi Rost
//
// Copyright (C) 2005 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

//uniform vec3  BaseColor;

varying vec3  DiffuseColor;
//varying vec3  SpecularColor;

void main(void)
{
    gl_FragColor = vec4 (computeShadow() * DiffuseColor, 1.0);
}