//
// Vertex shader for drawing the earth with one texture
//
// Author: Randi Rost
//
// Copyright (c) 2002-2006 3Dlabs Inc. Ltd. 
//
// See 3Dlabs-License.txt for license information
//

varying float LightIntensity;
uniform vec3 LightPosition;

//Calculate the intensity of light
float intensity(float diffuse,float spec)
{   
   const float SpecularContribution = 0.1;
   const float DiffuseContribution  = 1.0 - SpecularContribution;

   float lightIntensity  = DiffuseContribution * diffuse +
                      SpecularContribution * spec;
   return lightIntensity;
   
}
void main()
{
    vec3 ecPosition = vec3(gl_ModelViewMatrix * gl_Vertex);
    vec3 tnorm      = normalize(gl_NormalMatrix * gl_Normal);
    vec3 lightVec   = normalize(LightPosition - ecPosition);
    vec3 reflectVec = reflect(-lightVec, tnorm);
    vec3 viewVec    = normalize(-ecPosition);
    float diffuse   = max(dot(lightVec, tnorm), 0.0);
    float spec      = clamp(dot(reflectVec, viewVec), 0.0, 1.0);
    spec            = pow(spec, 16.0);

    LightIntensity  = intensity(diffuse,spec);

    if(gl_MultiTexCoord0.s==0&&gl_MultiTexCoord0.t==0)
	     gl_TexCoord[0] = gl_Vertex;
	else
         gl_TexCoord[0]  = gl_MultiTexCoord0;
    gl_Position     = ftransform();
}