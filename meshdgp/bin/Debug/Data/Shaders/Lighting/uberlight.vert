const float as = 1.0 / 1.5;   // ambient scale factor
const float ds = 1.0 / 3.0;   // diffuse scale factor

uniform mat4 MCtoLC;          // Model to light coordinate system transform
varying vec3 LCpos;           // Vertex position in light coordinate system 

void main()
{
    gl_Position = ftransform();

    // Compute vertex position in light coordinates
    LCpos    = (MCtoLC * gl_Vertex).xyz;

    // Diffuse factor
    vec4 ecPosition = gl_ModelViewMatrix * gl_Vertex;
    vec3 ecPosition3 = (vec3 (ecPosition)) / ecPosition.w;
    vec3 VP = vec3(gl_LightSource[0].position) - ecPosition3;
    VP = normalize(VP);
    vec3 normal = gl_NormalMatrix * gl_Normal;
    float df = max(0.0, dot(normal, VP));

    // Ambient factor
  //  float af = accessibility;
    float af = 0.5f;
    // Final color
    float scale = min(1.0, af * as + df * ds);
    vec3 color = vec3(scale * gl_Color);

    gl_FrontColor  = vec4(color, 1.0);
    gl_Position    = ftransform();
   // shadowSetup();
}
