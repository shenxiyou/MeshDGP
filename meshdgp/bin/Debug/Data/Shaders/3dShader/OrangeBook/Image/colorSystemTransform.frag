
uniform sampler2D BaseImage;
uniform float Mode;
varying vec2 TexCoord;
void rgbtoCIE(vec3 color)
{
     float c=color.r*0.412453+color.g*0.357580+color.b*0.180423;
     float i=color.r*0.212671+color.g*0.715160+color.b*0.072169;
     float e=color.r*0.019334+color.g*0.119193+color.b*0.950227;
     vec3 CieColor=vec3(c,i,e);
     gl_FragColor = vec4(CieColor,1);
}
void rgbtoCMY(vec3 color)
{
     float c=1-color.r;
     float m=1-color.g;
     float y=1-color.b;
     vec3 Cmycolor=vec3(c,m,y);
     gl_FragColor = vec4(Cmycolor,1);
}
void main()
{ 
     vec3 rgbcolor = texture2D(BaseImage,TexCoord).rgb;
     float c=rgbcolor.r*0.412453+rgbcolor.g*0.357580+rgbcolor.b*0.180423;
     float i=rgbcolor.r*0.212671+rgbcolor.g*0.715160+rgbcolor.b*0.072169;
     float e=rgbcolor.r*0.019334+rgbcolor.g*0.119193+rgbcolor.b*0.950227;
     float C=1-rgbcolor.r;
     float m=1-rgbcolor.g;
     float y=1-rgbcolor.b;
     if(Mode==0)
     {
      rgbtoCIE(rgbcolor);
      }
      else if(Mode==1)
      {
      rgbtoCMY(rgbcolor);
      }
      else if(Mode==2)
      {
      float r=1-C;
      float g=1-m;
      float b=1-y;
      vec3 RgbColor=vec3(r,g,b);
      gl_FragColor = vec4(RgbColor,1);
      }
      
}