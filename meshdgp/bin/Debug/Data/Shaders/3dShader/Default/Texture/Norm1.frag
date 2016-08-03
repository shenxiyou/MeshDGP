#version 120


    uniform sampler2D colorMap;
    uniform sampler2D normalMap;
 
    uniform float strength;
   // uniform float scale;
    //uniform float bias;
   // uniform int parallax;
    //uniform int isTexUnit0Used;
    
varying vec3 lightVec;
varying vec3 halfVector;
varying vec2 texCoord;

void main (void)
{	

        vec3 h = normalize(halfVector);
	
        vec3 n = normalize(texture2D(normalMap, texCoord).rgb * 2.0 - 1.0);
        vec3 l = normalize(lightVec);
        
        float nDotL = max(0.0, dot(n, l));
        float nDotH = max(0.0, dot(n, h));
        float power = (nDotL == 0.0) ? 0.0 : pow(nDotH, gl_FrontMaterial.shininess);
        
        vec4 ambient = gl_LightSource[0].ambient;
        vec4 diffuse = gl_LightSource[0].diffuse * gl_FrontMaterial.diffuse * nDotL;
        vec4 specular = gl_LightSource[0].specular * gl_FrontMaterial.specular * power * strength*0.5;
        vec4 color = gl_FrontLightModelProduct.sceneColor  + diffuse + specular;
        
    
        gl_FragColor = color;
        
}