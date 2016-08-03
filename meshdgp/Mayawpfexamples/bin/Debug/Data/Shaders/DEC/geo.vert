varying vec3 position;
varying vec3 normal;

void main()
{	
   gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
   gl_FrontColor = gl_Color;

   position = vec3(gl_ModelViewMatrix * gl_Vertex);
   normal = normalize(gl_NormalMatrix * gl_Normal);
}
