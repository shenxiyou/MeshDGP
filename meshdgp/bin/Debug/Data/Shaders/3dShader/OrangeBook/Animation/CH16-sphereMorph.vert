//
// Vertex shader for morphing a sphere
//
// Author: Philip Rideout
//
// Copyright (c) 2005 3Dlabs Inc. Ltd. 
//
// See 3Dlabs-License.txt for license information
//

varying vec4 Color;

uniform vec3 LightPosition;
uniform vec3 SurfaceColor;

const float PI = 3.14159;
const float TWO_PI = PI * 2.0;

uniform float Radius;
uniform float Blend;

vec3 sphere(vec2 domain)
{
    vec3 range;
    range.x = Radius * cos(domain.y) * sin(domain.x);
    range.y = Radius * sin(domain.y) * sin(domain.x);
    range.z = Radius * cos(domain.x);
    return range;
}

void main()
{
    vec2 p0 = gl_Vertex.xy * TWO_PI;
    vec3 normal = sphere(p0);;
    vec3 r0 = Radius * normal;
    vec3 vertex = r0;

    normal = mix(gl_Normal, normal, Blend);
    vertex = mix(gl_Vertex.xyz, vertex, Blend);

    normal = normalize(gl_NormalMatrix * normal);
    vec3 position = vec3(gl_ModelViewMatrix * vec4(vertex, 1.0));

    vec3 lightVec = normalize(LightPosition - position);
    float diffuse = max(dot(lightVec, normal), 0.0);

    if (diffuse < 0.125)
         diffuse = 0.125;

    Color = vec4(SurfaceColor * diffuse, 1.0);
    gl_Position = gl_ModelViewProjectionMatrix * vec4(vertex,1.0);
}
