//
// Fragment shader for image-based lighting
//
// Author: Philip Rideout
//
// Copyright (C) 2005 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

varying vec3 Normal;
varying vec3 Position;

uniform samplerCube diffuseEnvmap;
uniform samplerCube specularEnvmap;

void main(void)
{
   vec3 normal = normalize(gl_NormalMatrix * Normal);

    vec3 tCoord = reflect(normalize(Position), normal);
    tCoord = vec3(gl_TextureMatrix[0] * vec4(tCoord, 1.0));
    vec3 specularColor = vec3(textureCube(specularEnvmap, tCoord));

    tCoord = normal;
    tCoord = vec3(gl_TextureMatrix[0] * vec4(tCoord, 1.0));
    vec3 diffuseColor = vec3(textureCube(diffuseEnvmap, tCoord));

    vec3 color = diffuseColor + specularColor;
  //  color = computeShadow() * color * gl_Color.rgb;
    color = color * gl_Color.rgb;
    gl_FragColor = vec4(color, gl_Color.a);
}