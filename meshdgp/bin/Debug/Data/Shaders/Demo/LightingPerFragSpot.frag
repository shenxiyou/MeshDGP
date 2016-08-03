   
   
    varying vec4 diffuse,ambientGlobal, ambient;  
    varying vec3 normal,lightDir,halfVector;  
    varying float dist;   
    void main()  
    {  
        vec3 n,halfV,viewV,ldir;  
        float NdotL,NdotHV;  
        vec4 color = ambientGlobal;  
        float att,spotEffect;  
        n = normalize(normal);  
        NdotL = max(dot(n,normalize(lightDir)),0.0);  
        n = normalize(normal);  
		if (NdotL > 0.0)  
		{  
			spotEffect = dot(normalize(gl_LightSource[6].spotDirection),  
					     normalize(-lightDir));  
			if (spotEffect > gl_LightSource[6].spotCosCutoff)  
			{  
			        att = 1.0 / (gl_LightSource[6].constantAttenuation +  
							gl_LightSource[6].linearAttenuation * dist +  
							gl_LightSource[6].quadraticAttenuation 
							* dist * dist);  
					color += att * (diffuse * NdotL + ambient);  
					halfV = normalize(halfVector);  
					NdotHV = max(dot(n,halfV),0.0);  
					color += att * gl_FrontMaterial.specular 
					* gl_LightSource[6].specular *  
									pow(NdotHV,gl_FrontMaterial.shininess);  
			}  
		}  
        gl_FragColor = color;  
    }  
    
  