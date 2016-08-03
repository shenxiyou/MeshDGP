//
// Fragment shader for environment mapping with an
// equirectangular 2D texture and refraction mapping
// with a background texture blended together using
// the fresnel terms
//
// Author: Jon Kennedy, based on the envmap shader by John Kessenich, Randi Rost
//
// Copyright (c) 2002-2006 3Dlabs Inc. Ltd.
//
// See 3Dlabs-License.txt for license information
//

const vec3 Xunitvec = vec3 (1.0, 0.0, 0.0);
const vec3 Yunitvec = vec3 (0.0, 1.0, 0.0);

uniform vec3  BaseColor;
uniform float Depth;
uniform float MixRatio;

// need to scale our framebuffer - it has a fixed width/height of 2048
uniform float FrameWidth;
uniform float FrameHeight;

uniform sampler2D EnvMap;
uniform sampler2D RefractionMap;

varying vec3  Normal;
varying vec3  EyeDir;
varying vec4  EyePos;
varying float LightIntensity;

varying vec3 Position;

uniform float Offset;
uniform vec3 FireColor1;
uniform vec3 FireColor2;
uniform bool Teapot;
uniform float Extent;
uniform sampler3D sampler3d;

uniform sampler2D permTexture;


#define ONE 0.00390625
#define ONEHALF 0.001953125

/*
 * The interpolation function. This could be a 1D texture lookup
 * to get some more speed, but it's not the main part of the algorithm.
 */
float fade(float t) {
  // return t*t*(3.0-2.0*t); // Old fade, yields discontinuous second derivative
  return t*t*t*(t*(t*6.0-15.0)+10.0); // Improved fade, yields C2-continuous noise
}

/*
 * 3D classic noise. Slower, but a lot more useful than 2D noise.
 */
float noise(vec3 P)
{
  vec3 Pi = ONE*floor(P)+ONEHALF; // Integer part, scaled so +1 moves one texel
                                  // and offset 1/2 texel to sample texel centers
  vec3 Pf = fract(P);     // Fractional part for interpolation

  // Noise contributions from (x=0, y=0), z=0 and z=1
  float perm00 = texture2D(permTexture, Pi.xy).a ;
  vec3  grad000 = texture2D(permTexture, vec2(perm00, Pi.z)).rgb * 4.0 - 1.0;
  float n000 = dot(grad000, Pf);
  vec3  grad001 = texture2D(permTexture, vec2(perm00, Pi.z + ONE)).rgb * 4.0 - 1.0;
  float n001 = dot(grad001, Pf - vec3(0.0, 0.0, 1.0));

  // Noise contributions from (x=0, y=1), z=0 and z=1
  float perm01 = texture2D(permTexture, Pi.xy + vec2(0.0, ONE)).a ;
  vec3  grad010 = texture2D(permTexture, vec2(perm01, Pi.z)).rgb * 4.0 - 1.0;
  float n010 = dot(grad010, Pf - vec3(0.0, 1.0, 0.0));
  vec3  grad011 = texture2D(permTexture, vec2(perm01, Pi.z + ONE)).rgb * 4.0 - 1.0;
  float n011 = dot(grad011, Pf - vec3(0.0, 1.0, 1.0));

  // Noise contributions from (x=1, y=0), z=0 and z=1
  float perm10 = texture2D(permTexture, Pi.xy + vec2(ONE, 0.0)).a ;
  vec3  grad100 = texture2D(permTexture, vec2(perm10, Pi.z)).rgb * 4.0 - 1.0;
  float n100 = dot(grad100, Pf - vec3(1.0, 0.0, 0.0));
  vec3  grad101 = texture2D(permTexture, vec2(perm10, Pi.z + ONE)).rgb * 4.0 - 1.0;
  float n101 = dot(grad101, Pf - vec3(1.0, 0.0, 1.0));

  // Noise contributions from (x=1, y=1), z=0 and z=1
  float perm11 = texture2D(permTexture, Pi.xy + vec2(ONE, ONE)).a ;
  vec3  grad110 = texture2D(permTexture, vec2(perm11, Pi.z)).rgb * 4.0 - 1.0;
  float n110 = dot(grad110, Pf - vec3(1.0, 1.0, 0.0));
  vec3  grad111 = texture2D(permTexture, vec2(perm11, Pi.z + ONE)).rgb * 4.0 - 1.0;
  float n111 = dot(grad111, Pf - vec3(1.0, 1.0, 1.0));

  // Blend contributions along x
  vec4 n_x = mix(vec4(n000, n001, n010, n011),
                 vec4(n100, n101, n110, n111), fade(Pf.x));

  // Blend contributions along y
  vec2 n_xy = mix(n_x.xy, n_x.zw, fade(Pf.y));

  // Blend contributions along z
  float n_xyz = mix(n_xy.x, n_xy.y, fade(Pf.z));

  // We're done, return the final noise value.
  return n_xyz;
}

void main (void)
{
    // Compute reflection vector
    vec3 reflectDir = reflect(EyeDir, Normal);

    // Compute altitude and azimuth angles

    vec2 index;

    index.y = dot(normalize(reflectDir), Yunitvec);
    reflectDir.y = 0.0;
    index.x = dot(normalize(reflectDir), Xunitvec) * 0.5;

    // Translate index values into proper range

    if (reflectDir.z >= 0.0)
        index = (index + 1.0) * 0.5;
    else
    {
        index.t = (index.t + 1.0) * 0.5;
        index.s = (-index.s) * 0.5 + 1.0;
    }
    
    // if reflectDir.z >= 0.0, s will go from 0.25 to 0.75
    // if reflectDir.z <  0.0, s will go from 0.75 to 1.25, and
    // that's OK, because we've set the texture to wrap.
  
    // Do a lookup into the environment map.

    vec3 envColor = vec3 (texture2D(EnvMap, index));
    
    // calc fresnels term.  This allows a view dependant blend of reflection/refraction
    float fresnel = abs(dot(normalize(EyeDir), Normal));
    fresnel *= MixRatio;
    fresnel = clamp(fresnel, 0.1, 0.9);

	// calc refraction
	vec3 refractionDir = normalize(EyeDir) - normalize(Normal);

	// Scale the refraction so the z element is equal to depth
	float depthVal = Depth / -refractionDir.z;
	
	// perform the div by w
	float recipW = 1.0 / EyePos.w;
	vec2 eye = EyePos.xy * vec2(recipW);

	// calc the refraction lookup
	index.s = (eye.x + refractionDir.x * depthVal);
	index.t = (eye.y + refractionDir.y * depthVal);
	
	// scale and shift so we're in the range 0-1
	index.s = index.s / 2.0 + 0.5;
	index.t = index.t / 2.0 + 0.5;
	
	// as we're looking at the framebuffer, we want it clamping at the edge of the rendered scene, not the edge of the texture,
	// so we clamp before scaling to fit
	float recip1k = 1.0 / 2048.0;
	index.s = clamp(index.s, 0.0, 1.0 - recip1k);
	index.t = clamp(index.t, 0.0, 1.0 - recip1k);
	
	// scale the texture so we just see the rendered framebuffer
	index.s = index.s * FrameWidth * recip1k;
	index.t = index.t * FrameHeight * recip1k;
	
    vec3 RefractionColor = vec3 (texture2D(RefractionMap, index));
    
    // Add lighting to base color and mix
    vec3 base = LightIntensity * BaseColor;
    envColor = mix(envColor, RefractionColor, fresnel);
    envColor = mix(envColor, base, 0.2);

    gl_FragColor = vec4 (envColor, 1.0);


	 //vec4 noisevec;
    vec3 color;
    float intensity;
    float alpha;
	
	float noisevec = noise(Position + Offset);
	
	
    //noisevec = texture3D(sampler3d, Position);
    //noisevec = texture3D(sampler3d, vec3 (Position.x+noisevec[1],
      //         Position.y-noisevec[3]+Offset,
       //        Position.z+noisevec[1]));

    //intensity = 0.75 * (noisevec[0] + noisevec[1] + noisevec[2] + noisevec[3]);

    //intensity = 1.95 * abs(2.0 * intensity - 0.35);
    
    intensity = noisevec * 1.7;
    intensity = clamp(intensity, 0.0, 1.0);
	
    alpha = fract((Position.y+Extent)*0.65);

    color = mix(FireColor1, FireColor2, intensity) * (1.0 - alpha) * 2.0;
    color = clamp(color, 0.0, 1.0);
    alpha = 1.0 - alpha  * intensity;
    alpha *= alpha;

    //gl_FragColor = vec4(color, alpha);
}