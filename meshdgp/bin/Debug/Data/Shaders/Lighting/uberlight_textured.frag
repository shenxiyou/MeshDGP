uniform samplerCube tex;

// Superellipse shaping parameters
uniform float SeWidth;
uniform float SeHeight;
uniform float SeWidthEdge;
uniform float SeHeightEdge;
uniform float SeRoundness;

// Distance shaping parameters
uniform float DsNear;
uniform float DsFar;
uniform float DsNearEdge;
uniform float DsFarEdge;

// Vertex position in light coordinate system 
varying vec3 LCpos;

float superEllipseShape(vec3 pos) 
{
    // Project the point onto the z = 1.0 plane
    vec2 ppos   = pos.xy / pos.z;
    vec2 abspos = abs(ppos);

    float w = SeWidth;
    float W = SeWidthEdge;
    float h = SeHeight;
    float H = SeHeightEdge;

    if (SeRoundness < 0.0001)
        return (1.0 - smoothstep(w, W, abspos.x)) * (1.0 - smoothstep(h, H, abspos.y));

    float exp1 = 2.0 / SeRoundness;
    float exp2 = -SeRoundness / 2.0;

    float inner = w * h * pow(pow(h * abspos.x, exp1) + pow(w * abspos.y, exp1), exp2);
    float outer = W * H * pow(pow(H * abspos.x, exp1) + pow(W * abspos.y, exp1), exp2);

    return 1.0 - smoothstep(inner, outer, 1.0);
}

float distanceShape(vec3 pos)
{
   float depth = abs(pos.z);
   return smoothstep(DsNearEdge, DsNear, depth) *
          (1.0 - smoothstep(DsFar, DsFarEdge, depth));
}

void main()
{
    float attenuation = superEllipseShape(LCpos) * distanceShape(LCpos);
    attenuation = max(attenuation, 0.5);
    vec3 texcolor = textureCube(tex, gl_TexCoord[0].stp).rgb;
    gl_FragColor = vec4(computeShadow() * texcolor * attenuation, gl_Color.a);
}
