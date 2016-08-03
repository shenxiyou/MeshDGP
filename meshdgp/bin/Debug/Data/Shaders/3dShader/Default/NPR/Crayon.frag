varying vec2 TexCoord;
uniform sampler2D Noise;
uniform sampler2D EdgeMap;
uniform sampler2D ColorMap; 
varying vec3 MCposition ;
void main (void)
{


       float Disturb = texture2D(Noise, TexCoord).r;
       vec2 Offset= Disturb.rr*0.05f;
       vec4 FillColor=texture2D(ColorMap,TexCoord+Offset); 
       float Edge=texture2D(EdgeMap,TexCoord+Offset).r;
 
    
       vec3 Color = vec3(FillColor);
	if (Edge>0)
		Color = vec3(0);
	else
		Color = 1-(1-Color)*Disturb ; 
	gl_FragColor = vec4(Color, 1);


 
}

