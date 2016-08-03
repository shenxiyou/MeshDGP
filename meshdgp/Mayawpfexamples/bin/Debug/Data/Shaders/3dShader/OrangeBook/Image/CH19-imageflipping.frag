uniform sampler2D BaseImage;
uniform float VerticalFlipping;
void main()
{
    vec2 vst=gl_TexCoord[0].st;
    vec3 tmp=texture2D(BaseImage, vst.st).rgb;
    if(VerticalFlipping==0)
    {
     vst.t=1-vst.t;  
    }
    else
   {
     vst.s=1-vst.s; 
   }
    tmp = texture2D(BaseImage, vst.st).rgb;
    gl_FragColor = vec4(tmp,1);
}