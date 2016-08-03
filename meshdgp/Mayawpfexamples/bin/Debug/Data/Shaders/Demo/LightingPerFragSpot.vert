  
  
    varying vec4 diffuse,ambientGlobal,ambient;  
    varying vec3 normal,lightDir,halfVector;  
    varying float dist;  
    void main()  
    {  
        vec4 ecPos;  
        vec3 aux;  
        normal = normalize(gl_NormalMatrix * gl_Normal);   
        
        ecPos = gl_ModelViewMatrix * gl_Vertex;         
        aux = vec3(gl_LightSource[6].position-ecPos);  
        lightDir = normalize(aux);  
        
        dist = length(aux);  
        halfVector = normalize(gl_LightSource[6].halfVector.xyz);   
        diffuse = gl_FrontMaterial.diffuse * gl_LightSource[6].diffuse;   
        ambient = gl_FrontMaterial.ambient * gl_LightSource[6].ambient;  
        ambientGlobal = gl_FrontMaterial.ambient * gl_LightModel.ambient;  
        gl_Position = ftransform();  
    }  