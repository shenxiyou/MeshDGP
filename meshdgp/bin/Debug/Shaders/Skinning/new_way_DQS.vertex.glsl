#version 130

/* Accurate lighting with Dual Quaterion Skinning
 * See Sec. 4.6 of the paper (see "about")
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

uniform mat2x4 boneDualQuaternion[32];

varying vec3 v_color;
varying vec2 v_uv;
varying vec3 lightDir;

/* only for illustratin purposes */
varying vec3 v_norm;
varying vec3 v_norm_orig;

/* rotation of vector by quaternion */
// NB: result must be ADDED to vector being rotated!
vec3 applyQuatToVec( vec4 quat , vec3 vec  )
{
    return 2.0 * cross( quat.xyz, cross( quat.xyz, vec ) +  quat.w * vec );
}

/* roto-translation of position by dual quaternion */
// NB: result must be ADDED to pos being roto-translated!
vec3 applyDualQuatToPos( mat2x4 dq , vec3 pos )
{
    return 2.0 * (
      + cross( dq[0].xyz, cross(dq[0].xyz, pos) + pos*dq[0].w + dq[1].xyz )
      + dq[1].xyz * dq[0].w
      - dq[0].xyz * dq[1].w
    );
}

mat2x4 normalizedQuaternion( mat2x4 dq ) {
    dq /= length(dq[0]);
    dq[1] -= dot(dq[1],dq[0]) * dq[0];
    return dq;
}


// reduces the length of a vector to X=0.9, if it is bigger
vec3 capped(vec3 p){
    float div = inversesqrt( dot( p, p) );
    return p * min(1.0,div*0.9);
}


void main(){

    mat2x4 dq0 = boneDualQuaternion[ boneIndex[0] ];
    mat2x4 dq1 = boneDualQuaternion[ boneIndex[1] ];
    mat2x4 dq2 = boneDualQuaternion[ boneIndex[2] ];
    mat2x4 dq3 = boneDualQuaternion[ boneIndex[3] ];

    // find "shortest route" before quaternion interplation!
    dq1 = ( dot( dq0[0], dq1[0]) < 0 )? -dq1 : dq1;
    dq2 = ( dot( dq0[0], dq2[0]) < 0 )? -dq2 : dq2;
    dq3 = ( dot( dq0[0], dq3[0]) < 0 )? -dq3 : dq3;

    mat2x4 iDualQuat = dq0 * boneWeight[0] +
                       dq1 * boneWeight[1] +
                       dq2 * boneWeight[2] +
                       dq3 * boneWeight[3] ;

    iDualQuat = normalizedQuaternion(iDualQuat);

    vec3 _pos    = pos    + applyDualQuatToPos( iDualQuat , pos );
    vec3 _tang   = tang   + applyQuatToVec( iDualQuat[0]  , tang   );
    vec3 _bitang = bitang + applyQuatToVec( iDualQuat[0]  , bitang );

    vec3 q0 = applyDualQuatToPos( dq0 , pos );
    vec3 q1 = applyDualQuatToPos( dq1 , pos );
    vec3 q2 = applyDualQuatToPos( dq2 , pos );
    vec3 q3 = applyDualQuatToPos( dq3 , pos );

    gl_Position = gl_ModelViewProjectionMatrix * vec4(_pos,1) ;

    // only for illustration purposes:
    v_norm_orig = normalize( cross( _tang,_bitang) ) * isTextureFlipped;


    /* correction of tangent and bitangent direction */

    _tang += capped(
          deformFactorsTang[0]*(q1-q0)+
          deformFactorsTang[1]*(q2-q0)+
          deformFactorsTang[2]*(q3-q0)
    );

    _bitang += capped(
          deformFactorsBitang[0]*(q1-q0)+
          deformFactorsBitang[1]*(q2-q0)+
          deformFactorsBitang[2]*(q3-q0)
    );

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

    //v_color = vec3(iDualQuat[1].x);
    v_uv = uv; /* pass down texture coordintaes */

    // only for illustration purposes:
    v_norm = _norm;

}
