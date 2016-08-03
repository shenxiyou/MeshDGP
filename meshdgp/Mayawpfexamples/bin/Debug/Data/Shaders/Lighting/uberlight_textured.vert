uniform mat4 MCtoLC;          // Model to light coordinate system transform
varying vec3 LCpos;           // Vertex position in light coordinate system 

void main()
{
    gl_Position = ftransform();
    LCpos    = (MCtoLC * gl_Vertex).xyz;
    gl_TexCoord[0] = gl_TextureMatrix[0] * gl_MultiTexCoord0;
    gl_Position    = ftransform();
    shadowSetup();
}
