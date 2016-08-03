uniform vec3 eye;
uniform vec3 light;
varying vec3 position;
varying vec3 normal;

 

float diffuse( vec3 N, vec3 L )
{
   return max( 0., dot( N, L ));
}

float specular( vec3 N, vec3 L, vec3 E )
{
   const float shininess = 8.;
   vec3 R = 2.*dot(L,N)*N - L;
   return pow( max( 0., dot( R, E )), shininess );
}

float fresnel( vec3 N, vec3 E )
{
   const float sharpness = 10.;
   float NE = max( 0., dot( N, E ));
   return pow( sqrt( 1. - NE*NE ), sharpness );
}

void main()
{

   vec3 color= vec3(0,0,0); 
   
   float h = gl_Color.r; 
   if(h==0.0001)
   {  
   color= vec3(0,0,0);
   }
   else
   {
     
   // color
   float d = 1. - gl_Color.r;
   float r = (1. - d*d) * .8;
   float g = (1. - (2. * (d - .5)) * (2. * (d - .5))) * .7;
   float b = (1. - (1. - d) * (1. - d));
   color = vec3(r, g, b);

   }
    
   
  
   
   
   vec3 N = normalize( normal );
   vec3 L = normalize( light - position );
   vec3 E = normalize( eye - position );
   vec3 R = 2.*dot(L,N)*N - L;
   vec3 one = vec3( 1., 1., 1. );

 
   gl_FragColor.rgb = diffuse(N,L)*color + .5*specular(N,L,E)*one + .5*fresnel(N,E)*one;
   
   gl_FragColor.a = 1.;
}

