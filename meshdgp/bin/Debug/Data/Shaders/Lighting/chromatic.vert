uniform vec3 MinCorner; // lower-left-near corner; in deLight's theater, it's (-6, -1, -6);
uniform vec3 MaxCorner; // upper-right-far corner; in deLight's theater, it's (+6, +6, +6);

vec3 localize(vec3 origin, vec3 direction)
{
    float distance = 100.0; // ie, infinity
    float t;

    if (direction.x != 0.0)
    {
        t = (MinCorner.x - origin.x) / direction.x;
        if (t > 0.0 && t < distance) distance = t;
        t = (MaxCorner.x - origin.x) / direction.x;
        if (t > 0.0 && t < distance) distance = t;
    }

    if (direction.y != 0.0)
    {
        t = (MinCorner.y - origin.y) / direction.y;
        if (t > 0.0 && t < distance) distance = t;
        t = (MaxCorner.y - origin.y) / direction.y;
        if (t > 0.0 && t < distance) distance = t;
    }

    if (direction.z != 0.0)
    {
        t = (MinCorner.z - origin.z) / direction.z;
        if (t > 0.0 && t < distance) distance = t;
        t = (MaxCorner.z - origin.z) / direction.z;
        if (t > 0.0 && t < distance) distance = t;
    }

    vec3 intersection = origin + direction * distance;

    // scale to a unit cube before returning
    return 2.0 * (intersection - MinCorner) / (MaxCorner - MinCorner) + vec3(-1);
}

//
// Vertex shader for refraction/reflection/chromatic aberration effect.
//
// Author: Randi Rost
//
// Copyright (C) 2005 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

uniform float Eta;              // Ratio of indices of refraction (air -> glass)
const float FresnelPower = 3.0; // Controls degree of reflectivity at grazing angles
float EtaR = Eta - 0.02;
float EtaG = Eta;
float EtaB = Eta + 0.02;
float F  = ((1.0 - EtaG) * (1.0 - EtaG)) / ((1.0 + EtaG) * (1.0 + EtaG));

varying vec3  Reflect;
varying vec3  RefractR;
varying vec3  RefractG;
varying vec3  RefractB;
varying float Ratio;

void main(void)
{
    vec4 ecPosition  = gl_ModelViewMatrix * gl_Vertex;
    vec3 ecPosition3 = ecPosition.xyz / ecPosition.w;

    vec3 i = normalize(ecPosition3);
    vec3 n = normalize(gl_NormalMatrix * gl_Normal);

    Ratio   = F + (1.0 - F) * pow((1.0 - dot(-i, n)), FresnelPower);

    RefractR = refract(i, n, EtaR);
    RefractR = vec3(gl_TextureMatrix[0] * vec4(RefractR, 1.0));
    RefractR = localize(vec3(gl_Vertex), RefractR);

    RefractG = refract(i, n, EtaG);
    RefractG = vec3(gl_TextureMatrix[0] * vec4(RefractG, 1.0));
    RefractG = localize(vec3(gl_Vertex), RefractG);

    RefractB = refract(i, n, EtaB);
    RefractB = vec3(gl_TextureMatrix[0] * vec4(RefractB, 1.0));
    RefractB = localize(vec3(gl_Vertex), RefractB);

    Reflect = reflect(i, n);
    Reflect = vec3(gl_TextureMatrix[0] * vec4(Reflect, 1.0));
    Reflect = localize(vec3(gl_Vertex), Reflect);

    gl_Position   = ftransform();
}