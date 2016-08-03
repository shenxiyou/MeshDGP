

varying vec2 TexCoord;
void main()
{
  gl_Position = ftransform();
   TexCoord  = gl_MultiTexCoord0.st;
}