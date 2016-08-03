using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public partial class TriMeshUtil
    {
        public static void RetrieveSingle(TriMesh mesh, EnumPatchType input, EnumPatchType output)
        {
            switch (input)
            {
                case EnumPatchType.Vertex:
                    ShowOneRingOfVertex(mesh, output);
                    break;
                case EnumPatchType.Edge:
                    ShowOneRingOfEdge(mesh, output);
                    break;
                case EnumPatchType.Face:
                    ShowOneRingOfFace(mesh, output);
                    break;
                default:
                    break;
            }
        }
       

        public static void Retrieve(TriMesh mesh,EnumRetrieve enumSearch)
        {
            switch (enumSearch)
            {
                case EnumRetrieve.Clear:
                    ClearMeshColor(mesh);
                    break;
                case EnumRetrieve.OneRingVertexOfVertex:
                    ShowOneRingOfVertex(mesh,EnumPatchType.Vertex);
                    break;
                case EnumRetrieve.OneRingFaceOfVertex:
                    ShowOneRingOfVertex(mesh, EnumPatchType.Face);
                    break;

                case EnumRetrieve.OneRingEdgeOfVertex:
                    ShowOneRingOfVertex(mesh, EnumPatchType.Edge);
                    break;

                case EnumRetrieve.OneRingEdgeOfEdge:
                    ShowOneRingOfEdge(mesh, EnumPatchType.Edge);
                    break;

                case EnumRetrieve.OneRingFaceOfEdge:
                    ShowOneRingOfEdge(mesh, EnumPatchType.Face);
                    break;
                case EnumRetrieve.OneRingVertexOfEdge:
                    ShowOneRingOfEdge(mesh, EnumPatchType.Vertex);
                    break;

                case EnumRetrieve.OneRingFaceOfFace:
                    ShowOneRingFaceOfFace(mesh);
                    break;
                case EnumRetrieve.OneRingEdgeOfFace:
                   
                    break;

                case EnumRetrieve.OneRingVertexOfFace:
                    
                    break;


                case EnumRetrieve.NeighborFaceOfFace:
                    ShowNeighborFaceOfFace(mesh);
                    break;

                case EnumRetrieve.BoundaryEdgeOfFacePatch:
                    ShowBoundaryOfPatch(mesh,EnumPatchType.Face,EnumPatchType.Edge);
                    break;
                case EnumRetrieve.BoundaryVertexOfFacePatch:
                    ShowBoundaryOfPatch(mesh, EnumPatchType.Face, EnumPatchType.Vertex);
                    break;

                case EnumRetrieve.BoundaryFaceOfFacePatch:
                    ShowBoundaryOfPatch(mesh, EnumPatchType.Face, EnumPatchType.Face);
                    break;

                case EnumRetrieve.BoundaryEdgeOfEdgePatch:
                    ShowBoundaryOfPatch(mesh, EnumPatchType.Edge, EnumPatchType.Edge);
                    break;
                case EnumRetrieve.BoundaryFaceOfEdgePatch:
                    ShowBoundaryOfPatch(mesh, EnumPatchType.Edge, EnumPatchType.Face);
                    break;
                case EnumRetrieve.BoundaryVertexOfEdgePatch:
                    ShowBoundaryOfPatch(mesh, EnumPatchType.Edge, EnumPatchType.Vertex);
                    break;



                case EnumRetrieve.BoundaryEdgeOfVertexPatch:
                    ShowBoundaryOfPatch(mesh, EnumPatchType.Vertex, EnumPatchType.Edge);
                    break;
                case EnumRetrieve.BoundaryFaceOfVertexPatch:
                    ShowBoundaryOfPatch(mesh, EnumPatchType.Vertex, EnumPatchType.Face);
                    break;
                case EnumRetrieve.BoundaryVertexOfVertexPatch:
                    ShowBoundaryOfPatch(mesh, EnumPatchType.Vertex, EnumPatchType.Vertex);
                    break;


                case EnumRetrieve.OneRingFaceOfVertexPatch:
                    ShowOneRingOfPatch(mesh, EnumPatchType.Vertex, EnumPatchType.Face );
                    break;

                case EnumRetrieve.OneRingEdgeOfVertexPatch:
                    ShowOneRingOfPatch(mesh, EnumPatchType.Vertex, EnumPatchType.Edge);
                    break;

                case EnumRetrieve.OneRingVertexOfVertexPatch:
                    ShowOneRingOfPatch(mesh, EnumPatchType.Vertex, EnumPatchType.Vertex);
                    break;

                case EnumRetrieve.OneRingEdgeOfEdgePatch:
                    ShowOneRingOfPatch(mesh, EnumPatchType.Edge, EnumPatchType.Edge);
                    break;
                case EnumRetrieve.OneRingFaceOfEdgePatch:
                    ShowOneRingOfPatch(mesh, EnumPatchType.Edge, EnumPatchType.Face);
                    break;
                case EnumRetrieve.OneRingVertexOfEdgePatch:
                    ShowOneRingOfPatch(mesh, EnumPatchType.Edge, EnumPatchType.Vertex);
                    break;


                case EnumRetrieve.OneRingEdgeOfFacePatch:
                    ShowOneRingOfPatch(mesh, EnumPatchType.Face, EnumPatchType.Edge);
                    break;
                case EnumRetrieve.OneRingFaceOfFacePatch:
                    ShowOneRingOfPatch(mesh, EnumPatchType.Face, EnumPatchType.Face);
                    break;
                case EnumRetrieve.OneRingVertexOfFacePatch:
                    ShowOneRingOfPatch(mesh, EnumPatchType.Face, EnumPatchType.Vertex);
                    break;

                case EnumRetrieve.Boundary:
                    ShowBoundary(mesh);
                    break;
                case EnumRetrieve.TwoRingOfVertex:
                    ShowTwoRingOfVertex(mesh);
                    break;

                case EnumRetrieve.FacePatchByEdge:
                    ShowMeshPatchColor(mesh);
                    break;

              

            }
        }



         
    }
}
