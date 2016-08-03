#version 130

/* Standard lighting with Linear Blend Skinning
 *
 * Copyright (c) 2013 Marco Tarini <marco.tarini@isti.cnr.it>
 * See "about" for license info!
 *
 */

attribute vec3 pos;
attribute vec3 norm;
attribute vec2 uv;
attribute vec3 tang;
attribute vec3 bitang;

attribute vec4 boneWeight;
attribute ivec4 boneIndex;

uniform mat4 boneMatrix[32];

varying vec3 v_color;
varying vec3 v_norm;
varying vec2 v_uv;
varying vec3 lightDir;

void main(){

    mat4 T = boneMatrix[ boneIndex[0] ] * boneWeight[0] +
             boneMatrix[ boneIndex[1] ] * boneWeight[1] +
             boneMatrix[ boneIndex[2] ] * boneWeight[2] +
             boneMatrix[ boneIndex[3] ] * boneWeight[3] ;

    gl_Position = gl_ModelViewProjectionMatrix * (T * vec4(pos,1) );

    lightDir = vec3(0.0,0.0,1.0);

    vec3 _tang = mat3(T)*tang;
    vec3 _bitang = mat3(T)*bitang;
    vec3 _norm = mat3(T)*norm;


    lightDir = vec3(0.0,0.0,1.0); // in VIEW space

    // take lightDir to OBJECT space
    lightDir = lightDir*gl_NormalMatrix;

    // take lightDir to TANGENT space
    lightDir = vec3(
       dot(lightDir , _tang ),
       dot(lightDir , _bitang),
       dot(lightDir , _norm)
    );

    lightDir = normalize( lightDir );

    v_uv = uv; /* pass down texture coordintaes */

}
