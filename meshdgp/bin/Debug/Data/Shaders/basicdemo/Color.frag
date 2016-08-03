varying vec4 Position;
uniform float L_pos;
uniform float R_pos;



void ColorOne()
{
 float dist = abs(L_pos) ;
  dist = dist / 10.0;
 //distance=0.1;

   	 if(Position.x<dist*10*R_pos)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);
   	 if(Position.x<dist*9*R_pos)
   gl_FragColor = vec4(0.0,0.0,0.0,0.0);
   	 if(Position.x<dist*8*R_pos)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);
   	 if(Position.x<dist*7*R_pos)
   gl_FragColor = vec4(0.0,0.0,0.0,0.0);
   	 if(Position.x<dist*6*R_pos)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);
   	 if(Position.x<dist*5*R_pos)
   gl_FragColor = vec4(0.0,0.0,0.0,0.0);
   	 if(Position.x<dist*4*R_pos)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);
   	 if(Position.x<dist*3*R_pos)
   gl_FragColor = vec4(0.0,0.0,0.0,0.0);
	 if(Position.x<dist*2*R_pos)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);
   if(Position.x<dist*R_pos)
   gl_FragColor = vec4(0.0,0.0,0.0,0.0);
   if(Position.x<dist*0)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);
   if(Position.x<dist*(-1)*R_pos)
   gl_FragColor = vec4(0.0,0.0,0.0,0.0);
   if(Position.x<dist*(-2)*R_pos)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);
   if(Position.x<dist*(-3)*R_pos)
   gl_FragColor = vec4(0.0,0.0,0.0,0.0);
   if(Position.x<dist*(-4)*R_pos)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);
   if(Position.x<dist*(-5)*R_pos)
   gl_FragColor = vec4(0.0,0.0,0.0,0.0);
   if(Position.x<dist*(-6)*R_pos)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);
   if(Position.x<dist*(-7)*R_pos)
   gl_FragColor = vec4(0.0,0.0,0.0,0.0);
   if(Position.x<dist*(-8)*R_pos)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);
   if(Position.x<dist*(-9)*R_pos)
   gl_FragColor = vec4(0.0,0.0,0.0,0.0);
   if(Position.x<dist*(-10)*R_pos)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);
  
















 /*  if(Position.x<L_pos)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);

   if(Position.x<L_pos - distance*1)
   gl_FragColor = vec4(0.1,0.1,0.1,0.0);

   if(Position.x<L_pos - distance*2)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);
      
   if(Position.x<L_pos - distance*3)
   gl_FragColor = vec4(0.1,0.1,0.1,0.0);
  
  if(Position.x<L_pos - distance*4)
    gl_FragColor = vec4(0.9,0.9,0.9,0.0);

  if(Position.x<L_pos - distance*5)
   gl_FragColor = vec4(0.1,0.1,0.1,0.0);


   if(Position.x<L_pos - distance*6)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);

   if(Position.x<L_pos - distance*7)
   gl_FragColor = vec4(0.1,0.1,0.1,0.0);

   if(Position.x<L_pos - distance*8)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);

   if(Position.x<L_pos - distance*9)
   gl_FragColor = vec4(0.1,0.1,0.1,0.0);


   if(Position.x<L_pos - distance*10)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);

   if(Position.x<L_pos - distance*11)
   gl_FragColor = vec4(0.1,0.1,0.1,0.0);

    if(Position.x<L_pos - distance*12)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);

   if(Position.x<L_pos - distance*13)
   gl_FragColor = vec4(0.1,0.1,0.1,0.0);

   if(Position.x<L_pos - distance*14)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);

   if(Position.x<L_pos - distance*15)
   gl_FragColor = vec4(0.1,0.1,0.1,0.0);

   if(Position.x<L_pos - distance*16)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);

   if(Position.x<L_pos - distance*17)
   gl_FragColor = vec4(0.1,0.1,0.1,0.0);

   if(Position.x<L_pos - distance*18)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);

   if(Position.x<L_pos - distance*19)
   gl_FragColor = vec4(0.1,0.1,0.1,0.0);
      
   if(Position.x<L_pos - distance*20)
   gl_FragColor = vec4(0.9,0.9,0.9,0.0);*/



} 








void ColorTwo() 
{
  float dist = abs(L_pos);
  dist = dist / 10.0;

   if(Position.x<dist*3.0)
   gl_FragColor = vec4(0.9,0.0,0.9,0.0);
  
  if(Position.x<dist*2.0)
    gl_FragColor = vec4(0.0,0.0,0.9,0.0);

   if(Position.x<dist*1.0)
   gl_FragColor = vec4(0.0,0.9,0.9,0.0);

    if(Position.x<dist*0.0)
   gl_FragColor = vec4(0.0,0.9,0.0,0.0);

   if(Position.x<dist*(-1.0))
   gl_FragColor = vec4(0.9,0.9,0.0,0.0);
     if(Position.x<dist*(-2.0))
   gl_FragColor = vec4(0.9,0.0,0.0,0.0);
  





}




void ColorThree() 
{
  float dist = abs(L_pos);
  dist = dist / 5.0;
     
     if(Position.x<dist*3.0*R_pos && Position.x>=dist*2.0*R_pos )
     gl_FragColor =  vec4(0.9-0.9*(dist*3.0*R_pos-Position.x)/dist ,0.0,0.9,1.0);
  
    if(Position.x<dist*2.0*R_pos&&Position.x>=dist*1.0*R_pos)
    gl_FragColor =  vec4(0.0,0.9*( dist*2.0*R_pos - Position.x )/dist ,0.9,1.0);

    if(Position.x<dist*1.0*R_pos&&Position.x>=dist*0.0*R_pos)
    gl_FragColor =  vec4(0.0,0.9,0.9-0.9*(dist*1.0*R_pos-Position.x)/dist,1.0);

    if(Position.x<dist*0.0*R_pos&&Position.x>=dist*(-1.0)*R_pos)
    gl_FragColor =  vec4(0.9*( dist*0.0*R_pos - Position.x )/dist,0.9,0.0,1.0);

    if(Position.x<dist*(-1.0)*R_pos&&Position.x>=dist*(-3.0)*R_pos)
    gl_FragColor =  vec4(0.9,0.9-0.9*(dist*(-1.0)*R_pos-Position.x)/dist,0.0,1.0);



}

 

uniform int type;

void main() 
{
  if(type==1);
  {
    ColorOne();
  }
  
  if(type==2);
  {
    ColorTwo();
  }
  
  if(type==3);
  {
    ColorThree();
  }
  
  
    
}
