#version 130

/* Standard lighting with Dual Quaternion Skinning
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

uniform mat2x4 boneDualQuaternion[32];

varying vec3 v_color;
varying vec3 v_norm;
varying vec2 v_uv;
varying vec3 lightDir;

varying float v_norm_err;


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
    vec3 _norm   = norm   + applyQuatToVec( iDualQuat[0]  , norm );

    gl_Position = gl_ModelViewProjectionMatrix * vec4(_pos,1) ;


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
