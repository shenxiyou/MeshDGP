//
// Fragment shader for refraction/reflection/chromatic aberration effect.
//
// Author: Randi Rost
//
// Copyright (C) 2005 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

varying vec3  Reflect;
varying vec3  RefractR;
varying vec3  RefractG;
varying vec3  RefractB;
varying float Ratio;

uniform samplerCube cubemap;

void main(void)
{
    vec3 refractColor, reflectColor;

    refractColor.r = vec3 (textureCube(cubemap, RefractR)).r;
    refractColor.g = vec3 (textureCube(cubemap, RefractG)).g;
    refractColor.b = vec3 (textureCube(cubemap, RefractB)).b;

    reflectColor   = vec3 (textureCube(cubemap, Reflect));

    vec3 color = mix(refractColor, reflectColor, Ratio);

    gl_FragColor = vec4(color, 1.0);
}
