//
// Fragment shader for refraction/reflection effect.
//
// Author: Randi Rost
//
// Copyright (C) 2005 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

varying vec3  Reflect;
varying vec3  Refract;
varying float Ratio;

uniform samplerCube cubemap;

void main(void)
{
    vec3 refractColor = vec3 (textureCube(cubemap, Refract));
    vec3 reflectColor = vec3 (textureCube(cubemap, Reflect));

    vec3 color   = mix(refractColor, reflectColor, Ratio);

    gl_FragColor = vec4(color, 1.0);
}
