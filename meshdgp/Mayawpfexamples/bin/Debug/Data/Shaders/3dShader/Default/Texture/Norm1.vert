#version 120


varying vec3 lightVec; 
varying vec3 halfVector;
varying vec2 texCoord;

uniform vec3 LightPosition;  // eye space position of light

void main(void)
{
	gl_Position = ftransform();
	//texCoord = gl_MultiTexCoord0.xy;
	if(gl_MultiTexCoord0.s==0&&gl_MultiTexCoord0.t==0)
	     texCoord = gl_Vertex.xy;
	else
        texCoord = gl_MultiTexCoord0.st;
	vec3 normal = normalize(gl_NormalMatrix * gl_Normal);
	
	mat3 trans = transpose(gl_NormalMatrix);
        vec3 tangent = normalize(gl_NormalMatrix * trans[0]); 
	vec3 binormal = normalize(gl_NormalMatrix * trans[1]);
	
	vec3 vVertex = vec3(gl_ModelViewMatrix * gl_Vertex);
	//vec3 tmpVec = gl_LightSource[0].position.xyz - vVertex;
	vec3 tmpVec = LightPosition.xyz - vVertex;
	
	lightVec.x = dot(tmpVec, tangent);
	lightVec.y = dot(tmpVec, binormal);
	lightVec.z = dot(tmpVec, normal);
	halfVector = reflect(-tmpVec, gl_Normal);
        halfVector.x = dot(tmpVec, tangent);
	halfVector.y = dot(tmpVec, binormal);
	halfVector.z = dot(tmpVec, normal);
}