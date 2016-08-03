varying vec3 normal;
varying vec4 position;
varying vec4 vcolor;
void main()
{
	gl_Position	   = ftransform();
	position	   = gl_ModelViewMatrix * gl_Vertex;
	normal		   = gl_NormalMatrix * gl_Normal;
	vcolor	       = gl_Color;
	gl_TexCoord[0] = gl_MultiTexCoord0;
}
	