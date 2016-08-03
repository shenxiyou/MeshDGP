/**
 * SingleTexture.frag
 */

//void applyTexture2D(in sampler2D texUnit, in int type, in int index, inout vec4 color);

/**
 * \file Texturing.frag
 */
 
const int REPLACE  = 0;
const int MODULATE = 1;
const int DECAL    = 2;
const int BLEND    = 3;
const int ADD      = 4;
const int COMBINE  = 5;

void applyTexture2D(in sampler2D texUnit, in int type, 
                    in int index, inout vec4 color)
{ 
    vec4 texture = texture2D(texUnit, gl_TexCoord[index].st); 
    if (type == REPLACE)
    {
        color = texture;
    }
    else if (type == MODULATE)
    {
        color *= texture;
    }
    else if (type == DECAL)
    {
        vec3 temp = mix(color.rgb, texture.rgb, texture.a);
       
        color = vec4(temp, color.a);
    }
    else if (type == BLEND)
    {
        vec3 temp = mix(color.rgb, 
               gl_TextureEnvColor[index].rgb, texture.rgb);
       
        color = vec4(temp, color.a * texture.a);
    }
    else if (type == ADD)
    {
        color.rgb += texture.rgb;
        color.a   *= texture.a;
       
        color = clamp(color, 0.0, 1.0);
    }
    else
    {
        color = clamp(texture * color, 0.0, 1.0);
    }
}



//uniform sampler2D TexUnit0;
//uniform int TexturingType;

uniform int Single;

uniform sampler2D TexUnit0;
uniform int TexturingType0;

uniform sampler2D TexUnit1;
uniform int TexturingType1;

void MultiTexture()
{
    vec4 color = gl_Color; 
    applyTexture2D(TexUnit0, TexturingType0, 0, color);
    applyTexture2D(TexUnit1, TexturingType1, 0, color); 
    gl_FragColor = color;
}


void SingleTexture()
{
    vec4 color = gl_Color; 
    applyTexture2D(TexUnit0, TexturingType0, 0, color); 
    gl_FragColor = color;
}




void main()
{
    if(Single==1)
    {
       SingleTexture();
    }
    else
    {
      MultiTexture();
    }
     
}


