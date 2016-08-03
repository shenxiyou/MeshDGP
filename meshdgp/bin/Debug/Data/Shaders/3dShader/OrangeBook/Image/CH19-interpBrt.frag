//
// Fragment shader for modifying image brightness by
// interpolation and extrapolation
//
// Author: Randi Rost
//
// Copyright (c) 2003-2005: 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

varying vec2      TexCoord;
uniform float     Alpha;
uniform sampler2D Image;

void main()
{
    gl_FragColor  = texture2D(Image, TexCoord) * Alpha;
}