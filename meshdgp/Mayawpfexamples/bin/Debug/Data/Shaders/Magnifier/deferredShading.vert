
varying vec2 vPos;  

void main()
{
	gl_TexCoord[0] = gl_MultiTexCoord0;	
	
	gl_Position = ftransform();
	vPos=vec2(gl_Position.x/gl_Position.w, gl_Position.y/gl_Position.w);
}
