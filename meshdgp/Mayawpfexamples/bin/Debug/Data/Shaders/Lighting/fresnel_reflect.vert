vec3 localize(vec3 origin, vec3 direction) { return direction; }

//
// Vertex shader for refraction/reflection/chromatic aberration effect.
//
// Author: Randi Rost
//
// Copyright (C) 2005 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

const float Eta = 0.67;         // Ratio of indices of refraction (air -> glass)
const float FresnelPower = 10.0; // Controls degree of reflectivity at grazing angles

const float F  = ((1.0 - Eta) * (1.0 - Eta)) / ((1.0 + Eta) * (1.0 + Eta));

varying vec3  Reflect;
varying vec3  Refract;
varying float Ratio;

void main(void)
{
    vec4 ecPosition  = gl_ModelViewMatrix * gl_Vertex;
    vec3 ecPosition3 = ecPosition.xyz / ecPosition.w;

    vec3 i = normalize(ecPosition3);
    vec3 n = normalize(gl_NormalMatrix * gl_Normal);

    Ratio = F + (1.0 - F) * pow((1.0 - dot(-i, n)), FresnelPower);
    Ratio = 1.0 - Ratio;

    Refract = refract(i, n, Eta);
    Refract = vec3(gl_TextureMatrix[0] * vec4(Refract, 1.0));
    Refract = localize(vec3(gl_Vertex), Refract);

    Reflect = reflect(i, n);
    Reflect = vec3(gl_TextureMatrix[0] * vec4(Reflect, 1.0));
    Reflect = localize(vec3(gl_Vertex), Reflect);

    gl_Position   = ftransform();
}