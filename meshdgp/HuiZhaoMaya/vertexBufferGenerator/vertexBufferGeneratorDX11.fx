//**************************************************************************/
// Copyright (c) 2011 Autodesk, Inc.
// All rights reserved.
// 
// These coded instructions, statements, and computer programs contain
// unpublished proprietary information written by Autodesk, Inc., and are
// protected by Federal copyright law. They may not be disclosed to third
// parties or copied or duplicated in any form, in whole or in part, without
// the prior written consent of Autodesk, Inc.
//**************************************************************************/

// World-view-projection transformation.
float4x4 gWVPXf : WorldViewProjection < string UIWidget = "None"; >;


// Vertex shader input structure.
struct VS_INPUT
{
    float3 Pos : POSITION;

    // this is the custom stream
    // the vertex buffer provider will fill in the stream
    // when a matching custom semantic is found
    float2 CustomStream : myCustomStream;
};

// Vertex shader output structure.
struct VS_TO_PS
{
    // The vertex position in clip space.
    float4 HPos : SV_Position;

    // Color.
    float4 Color : COLOR0;
};

// Vertex shader.
VS_TO_PS VS_d3d11Example(VS_INPUT In)
{
    VS_TO_PS Out;
    
    float4 HPm = float4(In.Pos, 1.0f);

    // Transform the position from world space to clip space for output.
    Out.HPos = mul(HPm, gWVPXf);


    // here we use our custom stream data to affect the output.  
    // In this case an alternating, R, G, B pattern based on the vertex index.
    Out.Color = float4(0,0,0,1);

    int index = (int)In.CustomStream.y;
    if (index == 0)
        Out.Color.x = In.CustomStream.x;
    if (index == 1)
        Out.Color.y = In.CustomStream.x;
    if (index == 2)
        Out.Color.z = In.CustomStream.x;
 
    return Out;
}


float4 PS_d3d11Example(VS_TO_PS In) : SV_Target
{
    return In.Color;
}

// The main technique.
technique11 Main
{
    pass P0
    {
        SetVertexShader(CompileShader(vs_5_0, VS_d3d11Example()));
        SetGeometryShader(NULL);
        SetPixelShader(CompileShader(ps_5_0, PS_d3d11Example()));
    }
}
