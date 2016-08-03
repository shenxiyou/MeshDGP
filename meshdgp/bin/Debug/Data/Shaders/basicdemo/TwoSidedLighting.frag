/**
 * TwoSidedLighting.frag
 */
void main()
{
    // Normalize the normal. A varying variable CANNOT
    // be modified by a fragment shader. So a new variable
    // needs to be created.
    vec3 n = normalize(normal);
   
    vec4 ambient, diffuse, specular, color;

    // Initialize the contributions.
    ambient  = vec4(0.0);
    diffuse  = vec4(0.0);
    specular = vec4(0.0);
   
    // In this case the built in uniform gl_MaxLights is used
    // to denote the number of lights. A better option may be passing
    // in the number of lights as a uniform or replacing the current
    // value with a smaller value.
    calculateLighting(gl_MaxLights, n, vertex, gl_FrontMaterial.shininess,
                      ambient, diffuse, specular);
   
    color  = gl_FrontLightModelProduct.sceneColor  +
             (ambient  * gl_FrontMaterial.ambient) +
             (diffuse  * gl_FrontMaterial.diffuse) +
             (specular * gl_FrontMaterial.specular);

    // Re-initialize the contributions for the back
    // pass over the lights
    ambient  = vec4(0.0);
    diffuse  = vec4(0.0);
    specular = vec4(0.0);
          
    // Now caculate the back contribution. All that needs to be
    // done is to flip the normal.
    calculateLighting(gl_MaxLights, -n, vertex, gl_BackMaterial.shininess,
                      ambient, diffuse, specular);

    color += gl_BackLightModelProduct.sceneColor  +
             (ambient  * gl_BackMaterial.ambient) +
             (diffuse  * gl_BackMaterial.diffuse) +
             (specular * gl_BackMaterial.specular);

    color = clamp(color, 0.0, 1.0);
   
    gl_FragColor = color;
}