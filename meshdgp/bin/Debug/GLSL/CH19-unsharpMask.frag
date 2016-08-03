//
// fragment shader for unsharp masking via convolution
//
// Author: Randi Rost
//
// Copyright (c) 2003-2005: 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

// maximum size supported by this shader
const int MaxKernelSize = 25; 

// array of offsets for accessing the base image
uniform vec2 Offset[MaxKernelSize];

// size of kernel (width * height) for this execution
uniform int KernelSize;

// value for each location in the convolution kernel
uniform vec4 KernelValue[MaxKernelSize];

// scaling factor for edge image
uniform vec4 ScaleFactor;

// image to be convolved
uniform sampler2D BaseImage;

void main()
{
    int i;
    vec4 sum = vec4(0.0);

    for (i = 0; i < KernelSize; i++)
    {
        vec4 tmp = texture2D(BaseImage, gl_TexCoord[0].st + Offset[i]);
        sum += tmp * KernelValue[i];
    }

    vec4 baseColor = texture2D(BaseImage, vec2(gl_TexCoord[0]));
    gl_FragColor = ScaleFactor * sum + baseColor;
}