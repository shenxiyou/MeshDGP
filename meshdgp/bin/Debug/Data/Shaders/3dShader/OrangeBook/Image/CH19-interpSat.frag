//
// Fragment shader for modifying image saturation by
// interpolation and extrapolation
//
// Author: Randi Rost
//
// Copyright (c) 2003-2005: 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

const vec3 LumCoeff = vec3 (0.2125, 0.7154, 0.0721);

varying vec2      TexCoord;
uniform float     Alpha;
uniform sampler2D Image;

void main()
{
    vec3 texColor  = texture2D(Image, TexCoord).rgb;
    vec3 intensity = vec3(dot(texColor, LumCoeff));
    vec3 color     = mix(intensity, texColor, Alpha);
    gl_FragColor   = vec4(color, 1.0);
}