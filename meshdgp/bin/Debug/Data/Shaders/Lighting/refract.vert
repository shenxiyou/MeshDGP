//
// Vertex shader for refraction effect.
//
// Author: Randi Rost
//
// Copyright (C) 2005 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

uniform float Eta;
varying vec3 Vertex;
varying vec3 Refract;

void main(void)
{
    vec4 ecPosition  = gl_ModelViewMatrix * gl_Vertex;
    vec3 ecPosition3 = ecPosition.xyz / ecPosition.w;
    Vertex = vec3(gl_Vertex);
    vec3 incidence   = normalize(ecPosition3);
    vec3 tnorm       = gl_NormalMatrix * gl_Normal;

    Refract = refract(incidence, tnorm, Eta);
    Refract = vec3(gl_TextureMatrix[0] * vec4(Refract, 1.0));

    gl_Position   = ftransform();
}