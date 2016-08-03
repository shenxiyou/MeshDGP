//
// Vertex shader for mixing diffuse and ambient occlusion lighting.
//
// Author: Philip Rideout
//
// Copyright (C) 2005 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

// Ambient and Diffuse scale factors.
const float as = 1.0 / 1.5;
const float ds = 1.0 / 3.0;

void main(void)
{
    // Compute diffuse factor for infinite lighting.  shadowSetup() does it for us if shadowing is on.
#ifndef SHADOW
    vec3 VP = vec3(gl_LightSource[0].position);
    VP = normalize(VP);
    vec3 normal = normalize(gl_NormalMatrix * gl_Normal);
    float DF = max(0.0, dot(normal, VP));
#endif

    shadowSetup();

    // Ambient factor
    float af = accessibility;

    // Final color
    float scale = min(1.0, af * as + DF * ds);
    vec3 color = vec3(scale * gl_Color);
    gl_FrontColor  = vec4(color, 1.0);

    gl_Position    = ftransform();
}