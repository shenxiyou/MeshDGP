using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public enum  EnumRetrieve
    {
        OneRingVertexOfVertex,OneRingEdgeOfVertex,OneRingFaceOfVertex,
        OneRingVertexOfEdge, OneRingEdgeOfEdge, OneRingFaceOfEdge,
        OneRingVertexOfFace, OneRingEdgeOfFace, OneRingFaceOfFace,
        NeighborFaceOfFace,
        TwoRingOfVertex,        
       
        BoundaryVertexOfVertexPatch,BoundaryVertexOfFacePatch,BoundaryVertexOfEdgePatch,
        BoundaryEdgeOfVertexPatch,BoundaryEdgeOfFacePatch,BoundaryEdgeOfEdgePatch,
        BoundaryFaceOfVertexPatch,BoundaryFaceOfFacePatch,BoundaryFaceOfEdgePatch,
        OneRingVertexOfVertexPatch, OneRingEdgeOfVertexPatch, OneRingFaceOfVertexPatch,
        OneRingVertexOfEdgePatch, OneRingEdgeOfEdgePatch, OneRingFaceOfEdgePatch,
        OneRingVertexOfFacePatch,OneRingEdgeOfFacePatch,OneRingFaceOfFacePatch,
        BreadFirstSearch,DepthFirstSearch,
        Clear,
        Boundary,
        FacePatchByEdge,
        
    }

    public enum EnumRetrieveSimple
    {
        OneRingVertexOfVertex, OneRingEdgeOfVertex, OneRingFaceOfVertex,
        OneRingVertexOfEdge, OneRingEdgeOfEdge, OneRingFaceOfEdge,
        OneRingVertexOfFace, OneRingEdgeOfFace, OneRingFaceOfFace,
        NeighborFaceOfFace,
        TwoRingOfVertex,
        Boundary,
    }
   

    //Add-ons
    public enum EnumPatchType
    {
        Vertex,
        Edge,
        Face
    }
}
