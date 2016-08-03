varying vec3 normal;
varying vec4 position;
varying vec4 vcolor;

uniform sampler2D g_texture0;
void main()
{
	//vec4 texture_color = texture2D(g_texture0, gl_TexCoord[0].st);
	
	vec4 texture_color =vec4(1.0,0.0,1.0,0.5);
    if (vcolor.x + vcolor.y + vcolor.z>=3.0)
        gl_FragData[0] = texture_color;
    else
		gl_FragData[0] = vcolor;
		
	gl_FragData[1] = position;
	gl_FragData[2] = vec4(normal,1.0);
	
    //gl_FragData[1] =vec4(1.0,1.0,0.0,1.0);
	//gl_FragData[2] = vec4(1.0,0.0,0.0,1.0);
}	