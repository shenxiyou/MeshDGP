/**
 * \file OneSidedLighting.frag
 */
void calculateLighting(in int numLights, in vec3 N, in vec3 V, in float shininess,
                       inout vec4 ambient, inout vec4 diffuse, inout vec4 specular);

varying vec3 normal;
varying vec3 vertex;

void main()
{
    // Normalize the normal. A varying variable CANNOT
    // be modified by a fragment shader. So a new variable
    // needs to be created.
    vec3 n = normalize(normal);
   
    vec4 ambient  = vec4(0.0);
    vec4 diffuse  = vec4(0.0);
    vec4 specular = vec4(0.0);

    // In this case the built in uniform gl_MaxLights is used
    // to denote the number of lights. A better option may be passing
    // in the number of lights as a uniform or replacing the current
    // value with a smaller value.
    calculateLighting(gl_MaxLights, n, vertex, gl_FrontMaterial.shininess,
                      ambient, diffuse, specular);
   
    vec4 color = gl_FrontLightModelProduct.sceneColor  +
                 (ambient  * gl_FrontMaterial.ambient) +
                 (diffuse  * gl_FrontMaterial.diffuse) +
                 (specular * gl_FrontMaterial.specular);
                
    color = clamp(color, 0.0, 1.0);
   
    gl_FragColor = color;
}









/**
 * \file PhongLighting.frag
 */
const vec4 AMBIENT_BLACK = vec4(0.0, 0.0, 0.0, 1.0);
const vec4 DEFAULT_BLACK = vec4(0.0, 0.0, 0.0, 0.0);

bool isLightEnabled(in int i)
{
    // A separate variable is used to get
    // rid of a linker error.
    bool enabled = true;
   
    // If all the colors of the Light are set
    // to BLACK then we know we don't need to bother
    // doing a lighting calculation on it.
    if ((gl_LightSource[i].ambient  == AMBIENT_BLACK) &&
        (gl_LightSource[i].diffuse  == DEFAULT_BLACK) &&
        (gl_LightSource[i].specular == DEFAULT_BLACK))
        enabled = false;
       
    return(enabled);
}



float calculateAttenuation(in int i, in float dist)
{
    return(1.0 / (gl_LightSource[i].constantAttenuation +
                  gl_LightSource[i].linearAttenuation * dist +
                  gl_LightSource[i].quadraticAttenuation * dist * dist));
}

void directionalLight(in int i, in vec3 N, in float shininess,
                      inout vec4 ambient, inout vec4 diffuse, inout vec4 specular)
{
    vec3 L = normalize(gl_LightSource[i].position.xyz);
   
    float nDotL = dot(N, L);
   
    if (nDotL > 0.0)
    {   
        vec3 H = gl_LightSource[i].halfVector.xyz;
       
        float pf = pow(max(dot(N,H), 0.0), shininess);

        diffuse  += gl_LightSource[i].diffuse  * nDotL;
        specular += gl_LightSource[i].specular * pf;
    }
   
    ambient  += gl_LightSource[i].ambient;
}

void pointLight(in int i, in vec3 N, in vec3 V, in float shininess,
                inout vec4 ambient, inout vec4 diffuse, inout vec4 specular)
{
    vec3 D = gl_LightSource[i].position.xyz - V;
    vec3 L = normalize(D);

    float dist = length(D);
    float attenuation = calculateAttenuation(i, dist);

    float nDotL = dot(N,L);

    if (nDotL > 0.0)
    {   
        vec3 E = normalize(-V);
        vec3 R = reflect(-L, N);
       
        float pf = pow(max(dot(R,E), 0.0), shininess);

        diffuse  += gl_LightSource[i].diffuse  * attenuation * nDotL;
        specular += gl_LightSource[i].specular * attenuation * pf;
    }
   
    ambient  += gl_LightSource[i].ambient * attenuation;
}

void spotLight(in int i, in vec3 N, in vec3 V, in float shininess,
               inout vec4 ambient, inout vec4 diffuse, inout vec4 specular)
{
    vec3 D = gl_LightSource[i].position.xyz - V;
    vec3 L = normalize(D);

    float dist = length(D);
    float attenuation = calculateAttenuation(i, dist);

    float nDotL = dot(N,L);

    if (nDotL > 0.0)
    {   
        float spotEffect = dot(normalize(gl_LightSource[i].spotDirection), -L);
       
        if (spotEffect > gl_LightSource[i].spotCosCutoff)
        {
            attenuation *=  pow(spotEffect, gl_LightSource[i].spotExponent);

            vec3 E = normalize(-V);
            vec3 R = reflect(-L, N);
       
            float pf = pow(max(dot(R,E), 0.0), shininess);

            diffuse  += gl_LightSource[i].diffuse  * attenuation * nDotL;
            specular += gl_LightSource[i].specular * attenuation * pf;
        }
    }
   
    ambient  += gl_LightSource[i].ambient * attenuation;
}


void calculateLighting(in int numLights, in vec3 N, in vec3 V, in float shininess,
                       inout vec4 ambient, inout vec4 diffuse, inout vec4 specular)
{
    // Just loop through each light, and if its enabled add
    // its contributions to the color of the pixel.
    for (int i = 0; i < numLights; i++)
    {
        if (isLightEnabled(i))
        {
            if (gl_LightSource[i].position.w == 0.0)
                directionalLight(i, N, shininess, ambient, diffuse, specular);
            else if (gl_LightSource[i].spotCutoff == 180.0)
                pointLight(i, N, V, shininess, ambient, diffuse, specular);
            else
                 spotLight(i, N, V, shininess, ambient, diffuse, specular);
        }
    }
}

