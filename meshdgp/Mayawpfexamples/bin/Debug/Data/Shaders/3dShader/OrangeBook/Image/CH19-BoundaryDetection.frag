


const int MaxKernelSize = 25; 


uniform vec2 Offset1[MaxKernelSize];

uniform int KernelSize1;

uniform vec4 KernelValue1[MaxKernelSize];



uniform vec2 Offset2[MaxKernelSize];


uniform int KernelSize2;


uniform vec4 KernelValue2[MaxKernelSize];

// image to be convolved
uniform sampler2D BaseImage;

uniform float Mode;
void one()//3*3
{
    int i;
    vec4 sum = vec4(1.0);
    for (i = 0; i < KernelSize1; i++)
    {
        vec4 tmp = texture2D(BaseImage, gl_TexCoord[0].st + Offset1[i]);
        sum -= tmp * KernelValue1[i];
    }

    gl_FragColor = sum;
}

void two()//5*5
{
    int i;
    vec4 sum = vec4(1.0);
    for (i = 0; i < KernelSize2; i++)
    {
        vec4 tmp = texture2D(BaseImage, gl_TexCoord[0].st + Offset2[i]);
        sum -= tmp * KernelValue2[i];
    }

    gl_FragColor = sum;
}
void main()
{
   if(Mode==1)
    one();
   else
    two();
}