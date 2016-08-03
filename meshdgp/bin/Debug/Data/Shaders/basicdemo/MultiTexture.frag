/**
 * \file MultiTexture.frag
 */
 
 
uniform sampler2D TexUnit0;
uniform int TexturingType0;
uniform sampler2D TexUnit1;
uniform int TexturingType1;

void main()
{
    vec4 color = gl_Color;
   
    applyTexture2D(TexUnit0, TexturingType0, 0, color);
    applyTexture2D(TexUnit1, TexturingType1, 0, color);
   
    gl_FragColor = color;
}