 
uniform sampler2D uTexture;
 
in VertexData	{
	noperspective vec3 distance;
	vec4 color;
	vec2 texcoord;
} vVertexIn;
 
out vec4				oColor;
 
void main(void) {
	// determine frag distance to closest edge
	float fNearest = min(min(vVertexIn.distance[0],vVertexIn.distance[1]),vVertexIn.distance[2]);
	float fEdgeIntensity = exp2(-1.0*fNearest*fNearest);
 
	// blend between edge color and face color
	vec4 vFaceColor = texture( uTexture, vVertexIn.texcoord ) * vVertexIn.color; vFaceColor.a = 0.85;
	vec4 vEdgeColor = vec4(1.0, 1.0, 1.0, 0.85);
	oColor = mix(vFaceColor, vEdgeColor, fEdgeIntensity);
}
