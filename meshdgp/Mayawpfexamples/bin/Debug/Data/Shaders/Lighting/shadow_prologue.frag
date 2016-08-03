#ifdef SHADOW

uniform sampler2DShadow shadow;
varying vec4 ShadowCoord;
uniform float epsilon;
uniform float ShadowLightness;
varying float DF;

float computeShadow()
{
    vec4 tc = ShadowCoord;
    float depth = shadow2DProj(shadow, tc).x;

    float brightness = 1.0;

    // create a falloff if the surface normal points away from the light;
    // this helps to reduce boundary artifacts.
    if (DF < 0.125)
        brightness = (DF * 8.0) * (1.0 - ShadowLightness) + ShadowLightness;

    return depth < 1.0 ? ShadowLightness : brightness;
}

#else

float computeShadow() { return 1.0; }

#endif
