varying vec3 lightDir,Normal;
varying vec4 positione;
void main() {

	
	vec3 n = -normalize(Normal);	
	vec3 e = -normalize(positione.xyz);
	

	
	float  edgeMask = dot(e, n);



	if(edgeMask < 0.2)
		gl_FragColor = vec4(0.0, 0.0, 0.0, 1.0 );	
	if(edgeMask > 0.2 && edgeMask < 0.225)
		gl_FragColor = vec4(0.02, 0.02, 0.02, 1.0 );	
	if(edgeMask > 0.225 && edgeMask < 0.25)
		gl_FragColor = vec4(0.1, 0.1, 0.1, 1.0 );	
	if(edgeMask > 0.25 && edgeMask < 0.275)
		gl_FragColor = vec4(0.3, 0.3, 0.3, 1.0 );



	
		
	if(edgeMask > 0.275 && edgeMask < 0.5)
		gl_FragColor = vec4(1.0, 1.0, 1.0, 1.0 );
		


		
	if(edgeMask > 0.5 && edgeMask < 0.6)		
		gl_FragColor = vec4(0.6, 0.6, 0.6, 1.0 );
	if(edgeMask > 0.6 && edgeMask < 0.7)		
		gl_FragColor = vec4(0.5, 0.5, 0.5, 1.0 );
	if(edgeMask > 0.7 && edgeMask < 0.75)		
		gl_FragColor = vec4(0.4, 0.4, 0.4, 1.0 );
	if(edgeMask > 0.75 && edgeMask < 0.8)		
		gl_FragColor = vec4(0.3, 0.3, 0.3, 1.0 );
		
	if(edgeMask > 0.8 && edgeMask < 0.85)
		gl_FragColor = vec4(0.2, 0.2, 0.2, 1.0 );
		
	if(edgeMask > 0.85 && edgeMask < 0.90)
		gl_FragColor = vec4(0.1, 0.1, 0.1, 1.0 );
	if(edgeMask > 0.9 && edgeMask < 0.95)
		gl_FragColor = vec4(0.05, 0.05, 0.05, 1.0 );
	if(edgeMask > 0.95 && edgeMask < 0.975)
		gl_FragColor = vec4(0.02, 0.02, 0.02, 1.0 );
	if(edgeMask > 0.975 && edgeMask < 1.0)
		gl_FragColor = vec4(0.01, 0.01, 0.01, 1.0 );
		

	 
	}