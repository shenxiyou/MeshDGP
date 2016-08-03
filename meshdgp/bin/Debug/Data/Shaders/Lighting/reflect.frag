//
// Fragment shader for simple cube mapping.
//
// Author: Philip Rideout
//
// Copyright (C) 2005 3Dlabs, Inc.
//
// See 3Dlabs-License.txt for license information
//

varying vec3 Normal;
varying vec3 Position;
varying vec3 Vertex;
uniform samplerCube cubemap;

void main(void)
{
    vec3 tCoord = reflect(normalize(Position), Normal);
    tCoord = vec3(gl_TextureMatrix[0] * vec4(tCoord, 1.0));
    tCoord = localize(Vertex, tCoord);
    vec3 color = vec3(textureCube(cubemap, tCoord));
    gl_FragColor = vec4(color, gl_Color.a);
}
