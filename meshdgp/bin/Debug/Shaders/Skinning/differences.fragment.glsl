#version 120

/* a fragment shader used to color the object with the color-ramp
 * shown in Fig.1 of the paper (see "about")
 *
 * Copyright (c) 2013 Marco Tarini <marco.tarini@isti.cnr.it>
 * See "about" for license info!
 */

varying vec3 v_norm;
varying vec2 v_uv;

varying vec3 lightDir;

varying vec3 v_norm_orig;

vec3 colorRamp( float err){
    err = (err+1.0)*0.5;
    err*=err*err;
    return 0.8*vec3( 1.0,err,err);
}

void main(){
   gl_FragColor.rgb = colorRamp( dot( normalize(v_norm), normalize(v_norm_orig) ) );
   gl_FragColor.a = 1.0;
}
