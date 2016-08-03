varying vec4 Position;

void main()
{
   gl_Position = ftransform();
   Position = gl_Vertex;
   
}