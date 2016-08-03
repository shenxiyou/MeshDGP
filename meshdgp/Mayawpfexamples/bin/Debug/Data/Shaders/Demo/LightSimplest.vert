varying vec3 Normal;
void main(void)
{
gl_Position=gl_ModelViewProjectionMatrix*gl_Vertex;
Normal=normalize(gl_NormalMatrix*gl_Normal);
gl_FrontColor=gl_Color;
}