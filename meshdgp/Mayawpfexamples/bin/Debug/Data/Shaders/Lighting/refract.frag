//
// Vertex shader for refraction effect.
//
// Author: Randi Rost
//
// Copyright (C) 2005 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

varying vec3 Refract;
varying vec3 Vertex;
uniform samplerCube cubemap;

void main(void)
{
    vec3 refractColor;

    refractColor = vec3 (textureCube(cubemap, localize(Vertex, Refract)));

    gl_FragColor = vec4(refractColor, 1.0);
}
