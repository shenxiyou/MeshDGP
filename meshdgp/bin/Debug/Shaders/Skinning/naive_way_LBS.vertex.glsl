#version 150

/* Accurate lighting with LBS, done naively.
 * Provided for comparison.
 *
 * Computes Jackobian and inverts it.
 * Prone to numerical error (sec. 4.4 of the paper, see "about").
 * Requires a vec3 per weight (here, four of them).
 *
 * Copyright (c) 2013 Marco Tarini <marco.tarini@isti.cnr.it>
 * See "about" for license info!
 */

attribute vec3 pos;
attribute vec3 norm;
attribute vec2 uv;
attribute vec3 tang;
attribute vec3 bitang;

attribute vec4 boneWeight;
attribute ivec4 boneIndex;

attribute vec3 weightGradient0;
attribute vec3 weightGradient1;
attribute vec3 weightGradient2;
attribute vec3 weightGradient3;

uniform mat4x4 boneMatrix[32];

varying vec3 v_color;
varying vec3 v_norm;
varying vec2 v_uv;
varying vec3 lightDir;

varying vec3 v_norm_orig;

/* just a test */
mat3 outerProductManual( vec3 a, vec3 b){
    mat3 res;
    res[0] = a*b.x;
    res[1] = a*b.y;
    res[2] = a*b.z;
    return res;
}

void main(){

    vec4 pos4 = vec4(pos,1);

    vec3 q0 = vec3(boneMatrix[ boneIndex[0] ] * pos4);
    vec3 q1 = vec3(boneMatrix[ boneIndex[1] ] * pos4);
    vec3 q2 = vec3(boneMatrix[ boneIndex[2] ] * pos4);
    vec3 q3 = vec3(boneMatrix[ boneIndex[3] ] * pos4);

    vec3 _pos = q0*boneWeight[0]
              + q1*boneWeight[1]
              + q2*boneWeight[2]
              + q3*boneWeight[3];

    gl_Position = gl_ModelViewProjectionMatrix * vec4( _pos,1);

    mat3 T = mat3(boneMatrix[ boneIndex[0] ]) *boneWeight[0]
           + mat3(boneMatrix[ boneIndex[1] ]) *boneWeight[1]
           + mat3(boneMatrix[ boneIndex[2] ]) *boneWeight[2]
           + mat3(boneMatrix[ boneIndex[3] ]) *boneWeight[3];

    T += outerProduct(  q0.xyz , weightGradient0 );
    T += outerProduct(  q1.xyz , weightGradient1 );
    T += outerProduct(  q2.xyz , weightGradient2 );
    T += outerProduct(  q3.xyz , weightGradient3 );


    vec3 _tang = T*tang;
    vec3 _bitang = T*bitang;

    T = inverse( T ); // use inverse for the normal
    vec3 _norm = norm*T ; // transposed (note the order of the product terms)

    // lightDir, expressed in VIEW space
    lightDir = vec3(0.0,0.0,1.0);

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
