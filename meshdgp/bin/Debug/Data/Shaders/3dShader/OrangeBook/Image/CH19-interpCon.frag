//
// Fragment shader for modifying image contrast by
// interpolation and extrapolation
//
// Author: Randi Rost
//
// Copyright (c) 2002: 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

varying vec2      TexCoord;

uniform vec3      AvgLuminance;
uniform float     Alpha;
uniform sampler2D Image;

void main()
{
    vec3 color     = texture2D(Image, TexCoord).rgb;
    color          = mix(AvgLuminance, color, Alpha);
    gl_FragColor   = vec4(color, 1.0);
}