const int MaxKernelSize = 25; 
uniform vec2 Offset1[MaxKernelSize];

uniform int KernelSize1;

uniform vec4 ScaleFactor1;

uniform vec2 Offset2[MaxKernelSize];

uniform int KernelSize2;

uniform vec4 ScaleFactor2;
uniform sampler2D BaseImage;
uniform float Mode;

void one()//3*3
{
     int i;
    vec4 sum = vec4(0.0);

    for (i = 0; i < KernelSize1; i++)
        sum += texture2D(BaseImage, gl_TexCoord[0].st + Offset1[i]);

    gl_FragColor = sum * ScaleFactor1;
}

void two()//5*5
{
     int i;
    vec4 sum = vec4(0.0);

    for (i = 0; i < KernelSize2; i++)
        sum += texture2D(BaseImage, gl_TexCoord[0].st + Offset2[i]);
    gl_FragColor = sum * ScaleFactor2;
}
void main()
{
    if(Mode==1)
    one();
    else
    two();
}