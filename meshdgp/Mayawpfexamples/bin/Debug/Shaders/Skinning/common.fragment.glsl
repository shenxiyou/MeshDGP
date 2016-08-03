#version 120

/* common fragment shader:
 * basic x fragment lighting
 * uses: tangent space normal-map, and shininess map
 *
 * Copyright (c) 2013 Marco Tarini <marco.tarini@isti.cnr.it>
 * See "about" for license info!
 */

varying vec2 v_uv;
varying vec3 lightDir; // also serves as half-way dir

uniform sampler2D samplerBump;
uniform sampler2D samplerSpec;

void main(){
   gl_FragColor.a = 1.0;

   vec3 bump = texture2D( samplerBump, v_uv ).rgb*2.0 - vec3(1.0);
   vec3 specCol = texture2D( samplerSpec, v_uv ).rgb;

   /* uncomment to remove normalmap and spec map */
   //bump = vec3(0,0,1);
   //specCol = vec3(0.2);

   float diffuse =  dot( lightDir , bump) ;
   const vec3 baseCol = vec3(0.80,0.80,1.0);

   gl_FragColor.rgb =
     baseCol * (0.25 + 0.75*diffuse)
     +
     specCol* pow( diffuse , 24.0) ;

}
