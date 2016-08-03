varying vec3 Normal;
varying vec4 positione;
varying vec3 lightDir;
void main()
{
	/* first transform the normal into eye space and normalize the result */
  Normal = normalize(gl_NormalMatrix*gl_Normal);
	/* transform vertex coordinates into eye space */
  positione = gl_ModelViewMatrix*gl_Vertex;
  gl_Position = ftransform(); 
  //lightDir = vec3(1.0,1.0,1.0);
  
   lightDir = normalize(vec3(gl_LightSource[0].position)); 
}
