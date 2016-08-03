#version 130

/* Simple fragment shader: no skinning
 *
 * Copyright (c) 2013 Marco Tarini <marco.tarini@isti.cnr.it>
 * See "about" for license info!
 */

attribute vec3 pos;
attribute vec2 uv;
attribute vec3 norm;
attribute vec3 tang;
attribute vec3 bitang;
attribute float isTextureFlipped;


varying vec3 v_color;
varying vec2 v_uv;
varying vec3 lightDir;


void main(){

    gl_Position = gl_ModelViewProjectionMatrix * vec4(pos,1) ;
    vec3 _norm = normalize( cross( tang,bitang) ) * isTextureFlipped;

    lightDir = vec3(0.0,0.0,1.0); // in VIEW space

    // take lightDir to OBJECT space
    lightDir = lightDir*gl_NormalMatrix;

    // take lightDir to TANGENT space
    lightDir = vec3(
       dot(lightDir , tang ),
       dot(lightDir , bitang),
       dot(lightDir , _norm )
    );

    lightDir = normalize( lightDir );

    v_uv = uv; /* pass down texture coordintaes */

}
