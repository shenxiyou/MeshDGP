//
// Fragment shader for cartoon-style shading
//
// Author: Philip Rideout
//
// Copyright (c) 2005-2006 3Dlabs Inc. Ltd.
//
// See 3Dlabs-License.txt for license information
//

uniform vec3 DiffuseColor;
uniform vec3 PhongColor;
uniform float Edge;
uniform float Phong;
varying vec3 Normal;
uniform vec3 LightPosition;

void main (void)
{
	vec3 color = DiffuseColor;
	float intensity = dot(LightPosition,Normal);
	if (abs(intensity) < Edge)
		color = vec3(0);
	if (intensity > Phong)
		color = PhongColor; 
	gl_FragColor = vec4(color, 1);
}
