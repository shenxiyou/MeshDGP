//
// Vertex shader for a particle fountain.
//
// Author: Philip Rideout
//
// Copyright (c) 2002-2006 3Dlabs Inc. Ltd.
//
// See 3Dlabs-License.txt for license information
//

uniform float Time;
uniform float RepeatFactor;
uniform float Radius;
uniform float Acceleration;

varying vec4 Color;

uniform vec3 DiffuseColor;

void main(void)
{
	vec4 vertex = vec4(0,0,0,1);

	float t = max(Time - gl_Vertex.y, 0.0);

	// modulo(a, b) = a - b * floor(a * (1 / b)).
	t = t - RepeatFactor * floor(t * (1.0 / RepeatFactor));

	vec3 velocity = Radius * (gl_Vertex.xyz - vec3(0.5));

	vertex += vec4(velocity * t, 0.0);
	vertex.y -= Acceleration * t * t;

	 Color = vec4(DiffuseColor, 1.0 - t);
	
	//Color = vec4(1.0,0.0,0.0, 1.0 - t);
	gl_Position = gl_ModelViewProjectionMatrix * vertex;
}
