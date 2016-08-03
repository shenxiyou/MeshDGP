uniform sampler2D img;  
uniform sampler2D pos; 
uniform sampler2D nor;  

uniform int magnifier; 
uniform float centerX;
uniform float centerY;
uniform float R; 

varying vec2 vPos;  
 
void main()
{
	vec4 vcolor = texture2D(img, gl_TexCoord[0].st);      
	vec4 position = texture2D(pos, gl_TexCoord[0].st);	   
	vec3 normal = texture2D(nor, gl_TexCoord[0].st).xyz; 
	
	vec3 norm = normalize(normal);
	vec3 lightv = normalize(gl_LightSource[0].position.xyz - position.xyz);
	vec3 viewv = normalize(0-position.xyz);
	vec3 halfv = normalize(lightv + viewv);
	
	vec4 diffuse = max(0.0,dot(norm,lightv)) * vcolor * gl_LightSource[0].diffuse ;
	vec4 ambient = gl_FrontMaterial.ambient * gl_LightSource[0].ambient;

	vec4 specular = max(0.0,pow(dot(norm,halfv),gl_FrontMaterial.shininess)) * gl_FrontMaterial.specular * gl_LightSource[0].specular;
	vec3 color = diffuse.xyz + specular.xyz;
	  
	//if (magnifier == 0)
	//{
		 //gl_FragColor = vec4(color,1.0);  
		 //
	//}
	//else
	//{
		//vec2 center=vec2(centerX, centerY);  
		//float D=distance(vPos, center); 
		//if(D>R)
		//{
			 //
			//discard;
		//}
		//else if (D>=R-0.02)
		//{
			//gl_FragColor = vec4(0.2,0.2,0.2,1.0); 
		//}
		//else
		//{	
			//gl_FragColor = vec4(color,1.0);  
		//}
	//}
	
	gl_FragColor =vcolor;  
}
