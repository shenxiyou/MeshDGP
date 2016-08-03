varying vec2 TexCoord;
uniform sampler2D Image;

void main()
{
    
    float grey = dot(texture2D(Image, TexCoord).rgb, vec3(0.299, 0.587, 0.114)); 
    float r=grey;
    float g=grey;
    float b=grey;
    if(grey<127.5)
   {
        if(grey>=63.75)
        {
         r=0;
         g=grey;
         b=0.25*grey;
         }
        else
        {
         r=0;
         g=0.25*grey;
         b=grey;
        }
   }
   else
      {
        if(grey>=191.25)
        {
         r=grey;
         g=0.25*grey;
         b=0;
         }
      else 
        {
         r=0.25*grey;
         g=grey;
         b=0;
        }
      }
 vec3 tmp=vec3(r,g,b);
 gl_FragColor = vec4 (tmp,1.0);
}