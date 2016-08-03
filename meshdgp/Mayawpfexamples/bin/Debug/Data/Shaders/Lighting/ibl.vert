varying vec3 Normal;
varying vec3 Position;

void main(void)
{
    vec4 ecPosition = gl_ModelViewMatrix * gl_Vertex;
    Position = (vec3 (ecPosition)) / ecPosition.w;
    Normal = gl_Normal;

    //vec3 color = vec3(accessibility * gl_Color);
    
     vec3 color = vec3(gl_Color);
    gl_FrontColor  = vec4(color, 1.0);
    gl_Position    = ftransform();
   // shadowSetup();
}
