   
   
void main()  
{  
    vec3 normal, lightDir;  
    vec4 diffuse, ambient,specular, globalAmbient;  
    float NdotL,NdotHV; 
    normal = normalize(gl_NormalMatrix * gl_Normal);  
    lightDir = normalize(vec3(gl_LightSource[0].position));  
    NdotL = max(dot(normal, lightDir), 0.0);  
    if (NdotL > 0.0)  
    {   
    NdotHV = max(dot(normal, gl_LightSource[0].halfVector.xyz),0.0);  
    specular = gl_FrontMaterial.specular * gl_LightSource[0].specular *  
            pow(NdotHV,gl_FrontMaterial.shininess);  
    }  
    diffuse = gl_FrontMaterial.diffuse * gl_LightSource[0].diffuse;   
    ambient = gl_FrontMaterial.ambient * gl_LightSource[0].ambient;      
    globalAmbient = gl_FrontMaterial.ambient * gl_LightModel.ambient;   
    gl_FrontColor =  NdotL * diffuse + globalAmbient + ambient;   
    gl_Position = ftransform();  
} 