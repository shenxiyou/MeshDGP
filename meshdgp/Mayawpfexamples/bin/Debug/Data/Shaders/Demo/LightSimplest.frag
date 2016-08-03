varying vec3 Normal;    
vec3 lightColor = vec3(1.0, 0.0, 0.0);
vec3 lightDir = vec3(0.0, 0.0,4.0);	
void main(void)
{
    vec3 color = clamp( dot(normalize(Normal), lightDir), 
                       0.0, 1.0) * lightColor; 
    gl_FragColor = vec4(color, 1.0);
}