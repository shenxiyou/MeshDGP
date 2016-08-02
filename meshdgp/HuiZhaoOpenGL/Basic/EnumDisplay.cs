 

using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public enum EnumDisplayMode
    {
        Basic, Vertex, WireFrame, DarkWireFrame, 
        Flat, Smooth, FlatLine, SmoothLine, FaceColor,
        Boundary,FaceNormal,VertexNormal,PricipalDirection,
  
        DualMeshB, DualMeshC, DualOnly,
        PointsWithLine,
        Textured,  
        FaceColored, 
        Laplacian,
        DualMesh,
        VertexColor, 
      
        Hole, 
        SelectedVertex, SelectedFace, SelectedEdge, SelectedSmooth,
        Dual, EdgesColorAndVertexColor,
        SegementationVertex,SegementationFace,
        Vector,
        TreeCotree,
        Generator,
    
        NPRLine, 
        ColorVis, 

        Accum,AccumAdd,Stencil,Transparent,TransparentTwo,Shadow,Mirror,
        OutSide,Axis,
        Color,
}


    public enum EnumMeshType
    {
        Tet,Manifold,NonManifold,Quad
    }


    public enum EnumTexture
    {
        Edge,Vertex,Multi,Double
    }
}
