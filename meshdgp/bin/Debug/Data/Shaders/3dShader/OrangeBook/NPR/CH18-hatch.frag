//
// Fragment shader for procedurally generated hatching or "woodcut" appearance.
//
// This is an OpenGL 2.0 implementation of Scott F. Johnston's "Mock Media"
// (from "Advanced RenderMan: Beyond the Companion" SIGGRAPH 98 Course Notes)
//
// Author: Bert Freudenberg <bert@isg.cs.uni-magdeburg.de>
//
// Copyright (c) 2002-2003 3Dlabs, Inc. 
//
// See 3Dlabs-License.txt for license information
//

const float frequency = 1.0;

varying vec3  ObjPos;               
varying float V;                    
varying float LightIntensity;

uniform sampler3D Noise;           
uniform float Swidth;             

void main()
{
    float dp = length(vec2(dFdx(V * Swidth), dFdy(V * Swidth)));
    float logdp    = -log2(dp);
    float ilogdp   = floor(logdp);
    float stripes  = exp2(ilogdp); 
    float noise    = texture3D(Noise, ObjPos).x; 
    
    float sawtooth = fract((V + noise * 0.1) * frequency * stripes);
    
    
    float triangle = abs(2.0 * sawtooth - 1.0); 
    float transition = logdp - ilogdp; 
    triangle = abs((1.0 + transition) * triangle - transition);

    const float edgew = 0.2;    
    float edge0  = clamp(LightIntensity - edgew, 0.0, 1.0);
    float edge1  = clamp(LightIntensity, 0.0, 1.0);
    float square = 1.0 - smoothstep(edge0, edge1, triangle);

    gl_FragColor = vec4(vec3(square), 1.0);
}