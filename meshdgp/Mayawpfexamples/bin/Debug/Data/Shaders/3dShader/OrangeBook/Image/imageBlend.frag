varying vec2 TexCoord;
uniform sampler2D BaseImage;
uniform sampler2D BlendImage;
uniform float Mode;
void main()
{
    vec4 baseColor=texture2D(BaseImage,gl_TexCoord[0].st);
    vec4 blendColor=texture2D(BlendImage,gl_TexCoord[0].st);
    vec4 white=vec4(1.0);
    vec4 lumCoeff=vec4(0.2125,0.7154,0.0721,1);
    if (Mode==0)
    {
    gl_FragColor =baseColor*0;
    }
    if (Mode==1)
    {
    gl_FragColor =baseColor;
    }
     if (Mode==2)
    {
    gl_FragColor =blendColor;
    }
    else if(Mode==18)
    gl_FragColor = (blendColor+baseColor)*0.5;
    else if(Mode==19)
    gl_FragColor =(baseColor.a==1.0)?blendColor:baseColor;
    else if(Mode==3)
    {
    vec3 color=blendColor.rgb;
    gl_FragColor=vec4(color,0.0);
    }
    else if(Mode==4)
    gl_FragColor =min(baseColor,blendColor);
    else if(Mode==5)
    gl_FragColor =max(baseColor,blendColor);
    else if(Mode==6)
    gl_FragColor =baseColor*blendColor;
    else if(Mode==7)
    gl_FragColor =vec4(white-(white-baseColor)*(white-blendColor));
    //Color Burn
    else if(Mode==8)
    gl_FragColor =white-(white-baseColor)/blendColor;
    //Color Dodge
    else if(Mode==9)
    gl_FragColor =baseColor/(white-blendColor);
    //Overlay
    else if(Mode==10)
    {
         float luminance=dot(baseColor,lumCoeff);
         if(luminance<0.45)
         gl_FragColor=2*baseColor*blendColor;
         else if(luminance>0.55)
         gl_FragColor =white-2*(white-baseColor)*(white-blendColor);
         else
         {
          vec4 result1=2*baseColor*blendColor;
          vec4 result2=white-2*(white-baseColor)*(white-blendColor);
          gl_FragColor=mix(result1,result2,(luminance-0.45)*10);
          
         }
    }
    // Soft Light
    else if(Mode==11)
    gl_FragColor =2*baseColor*blendColor+baseColor*baseColor-2*baseColor*baseColor*blendColor;
     // Hard Light
    else if(Mode==12)
    {
         float luminance=dot(blendColor,lumCoeff);
         if(luminance<0.45)
         gl_FragColor=2*baseColor*blendColor;
         else if(luminance>0.55)
         gl_FragColor =white-2*(white-baseColor)*(white-blendColor);
         else
         {
          vec4 result1=2*baseColor*blendColor;
          vec4 result2=white-2*(white-baseColor)*(white-blendColor);
          gl_FragColor=mix(result1,result2,(luminance-0.45)*10);
          
         }
    }
    //Add
    else if(Mode==13)
    gl_FragColor =blendColor+baseColor;
    //Subtract
    else if(Mode==14)
    gl_FragColor =baseColor-blendColor;
    //Diffrence
    else if(Mode==15)
    gl_FragColor =abs(baseColor-blendColor);
    //inverse Difference
    else if(Mode==16)
    gl_FragColor =white-abs(white-baseColor-blendColor);
    //Exclusion
     else if(Mode==17)
    gl_FragColor =blendColor+baseColor-2*baseColor*blendColor;

}