uniform samplerCube tex;
 
void main(void)
{
    vec3 color = textureCube(tex, gl_TexCoord[0].stp).rgb;
    gl_FragColor = vec4(computeShadow() * color, gl_Color.a);
}
