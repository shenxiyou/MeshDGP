 
uniform float Time;            // updated each frame by the application
 uniform vec4  Background;      // constant color equal to background
 attribute  vec3  Velocity;      // initial velocity
 attribute  float StartTime;     // time at which particle is activated
varying vec4  Color;
void main()
{
     vec4  vert;
     float t = Time - StartTime;
    if (t >= 0.0)
     {
         vert    = gl_Vertex + vec4(Velocity * t, 0.0);
         vert.y -= 4.9 * t * t;
         Color   = gl_Color;
   }
    else
   {
         vert  = gl_Vertex;       // Initial position
         Color = Background;     // "pre-birth" color
    }
    gl_Position  = gl_ModelViewProjectionMatrix * vert;
}