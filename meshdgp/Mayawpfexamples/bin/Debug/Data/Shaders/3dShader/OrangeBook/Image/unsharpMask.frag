const int MaxKernelSize = 25; 

uniform vec2 Offset1[MaxKernelSize];

uniform int KernelSize1;

uniform vec4 KernelValue1[MaxKernelSize];

uniform vec2 Offset2[MaxKernelSize];

uniform int KernelSize2;

uniform vec4 KernelValue2[MaxKernelSize];
uniform float ScaleFactor;
uniform sampler2D BaseImage;
uniform float Mode;
vec4 one()
{
    int i;
    vec4 sum = vec4(0.0);

    for (i = 0; i < KernelSize1; i++)
    {
        vec4 tmp = texture2D(BaseImage, gl_TexCoord[0].st + Offset1[i]);
        sum += tmp * KernelValue1[i];
    }
 return sum;
   
}
vec4 two()
{
     int i;
    vec4 sum = vec4(0.0);

    for (i = 0; i < KernelSize2; i++)
    {
        vec4 tmp = texture2D(BaseImage, gl_TexCoord[0].st + Offset2[i]);
        sum += tmp * KernelValue2[i];
    }
 return sum;
}
void main()
{
     vec4 baseColor = texture2D(BaseImage, vec2(gl_TexCoord[0]));
    if(Mode==1)
    {
     vec4 rsum=one();
      gl_FragColor = ScaleFactor * rsum + baseColor;
    }
     else
     {
      vec4 rsum=two();
      gl_FragColor = ScaleFactor * rsum + baseColor;
      }
}