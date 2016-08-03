#version 130

/* Accurate lighting with Linear Blend Skinning.
 *
 * Copyright (c) 2013 Marco Tarini <marco.tarini@isti.cnr.it>
 * See "about" for license info!
 */

attribute vec3 pos;
attribute vec2 uv;
attribute vec3 tang;
attribute vec3 bitang;
attribute float isTextureFlipped;

attribute vec4 boneWeight;
attribute ivec4 boneIndex;

attribute vec3 deformFactorsTang;
attribute vec3 deformFactorsBitang;

uniform mat4x4 boneMatrix[32];

varying vec3 v_color;
varying vec2 v_uv;
varying vec3 lightDir;

/* only for illustratin purposes */
varying vec3 v_norm;
varying vec3 v_norm_orig;


// reduces the length of a vector to X=0.9, if it is bigger
vec3 capped(vec3 p){
    float div = inversesqrt( dot( p, p) );
    return p * min(1.0,div*0.9);
}

void main(){

    vec4 pos4 = vec4(pos,1);

    vec3 q0 = vec3(boneMatrix[ boneIndex[0] ] * pos4);
    vec3 q1 = vec3(boneMatrix[ boneIndex[1] ] * pos4) - q0;
    vec3 q2 = vec3(boneMatrix[ boneIndex[2] ] * pos4) - q0;
    vec3 q3 = vec3(boneMatrix[ boneIndex[3] ] * pos4) - q0;

    vec3 _pos = q0
              + q1*boneWeight[1]
              + q2*boneWeight[2]
              + q3*boneWeight[3];

    gl_Position = gl_ModelViewProjectionMatrix * vec4( _pos,1);

    mat3 T = mat3(boneMatrix[ boneIndex[0] ]) * boneWeight[0]
           + mat3(boneMatrix[ boneIndex[1] ]) * boneWeight[1]
           + mat3(boneMatrix[ boneIndex[2] ]) * boneWeight[2]
           + mat3(boneMatrix[ boneIndex[3] ]) * boneWeight[3];

    vec3 _tang = T*tang;
    vec3 _bitang = T*bitang;

    // only for illustration purposes:
    v_norm_orig = normalize( cross( _tang,_bitang) ) * isTextureFlipped;


    /* correction of tangent and bitangent direction */

    _tang += capped(
          deformFactorsTang[0]*q1+
          deformFactorsTang[1]*q2+
          deformFactorsTang[2]*q3
    );

    _bitang += capped(
          deformFactorsBitang[0]*q1+
          deformFactorsBitang[1]*q2+
          deformFactorsBitang[2]*q3
    );

    // vectors are "capped" is because of Sec 4.5

    normalize( _tang );
    normalize( _bitang );
    vec3 _norm = normalize( cross( _tang,_bitang) ) * isTextureFlipped;


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

    // only for illustration purposes:
    v_norm = _norm;
}
