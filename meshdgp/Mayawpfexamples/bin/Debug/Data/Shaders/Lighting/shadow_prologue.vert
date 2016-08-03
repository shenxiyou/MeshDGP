#ifdef SHADOW

varying vec4 ShadowCoord;
varying float DF;

void shadowSetup()
{
    // Diffuse factor for infinite lighting.
    vec3 VP = vec3(gl_LightSource[0].position);
    VP = normalize(VP);
    vec3 normal = normalize(gl_NormalMatrix * gl_Normal);
    DF = max(0.0, dot(normal, VP));

    // Shadow coordinate.
    ShadowCoord = gl_TextureMatrix[1] * gl_Vertex;
}

#else

void shadowSetup() {}

#endif
