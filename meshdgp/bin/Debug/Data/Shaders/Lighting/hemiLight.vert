//
// Vertex shader for hemispherical lighting
//
// Author: Randi Rost
//
// Copyright (C) 2005 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

uniform vec3 SkyColor;
uniform vec3 GroundColor;

varying vec3  DiffuseColor;

void main(void)
{
    vec3 ecPosition = vec3 (gl_ModelViewMatrix * gl_Vertex);
    vec3 tnorm      = normalize(gl_NormalMatrix * gl_Normal);
    vec3 lightVec   = normalize(vec3(gl_LightSource[0].position) - ecPosition);
    float costheta  = dot(tnorm, lightVec);
    float a         = 0.5 + 0.5 * costheta;

    DiffuseColor = mix(GroundColor, SkyColor, a);
    DiffuseColor += vec3(accessibility) * 0.125;
    DiffuseColor *= vec3(gl_Color);

    gl_TexCoord[0]  = gl_MultiTexCoord0;
    gl_Position     = ftransform();
    shadowSetup();
}