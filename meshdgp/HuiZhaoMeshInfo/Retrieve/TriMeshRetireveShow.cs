using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public partial class TriMeshUtil
    {
        public static void ShowTwoRingOfVertex(TriMesh mesh)
        {
            List<TriMesh.Vertex> selected = RetrieveSelectedVertex(mesh);
            for (int i = 0; i < selected.Count; i++)
            {
                
                ShowTwoRingVertexOfVertex(selected[i]);
                      
            }

            for (int i = 0; i < selected.Count; i++)
            {

                selected[i].Traits.Color = Color4.Black;

            }
        }

        public static void ShowOneRingOfVertex(TriMesh mesh, EnumPatchType searchType)
        {
            List<TriMesh.Vertex> selected = RetrieveSelectedVertex(mesh);
            for (int i = 0; i < selected.Count; i++)
            {
                switch (searchType)
                {
                    case EnumPatchType.Vertex:
                        ShowOneRingVertexOfVertex(selected[i]);
                        break;
                    case EnumPatchType.Edge:
                        ShowOneRingEdgeOfVertex(selected[i]);
                        break;
                    case EnumPatchType.Face:
                        ShowOneRingFaceOfVertex(selected[i]);
                        break;
                }
            }
        }

        public static void ShowOneRingOfEdge(TriMesh mesh, EnumPatchType type)
        {
            List<TriMesh.Edge> selected = RetrieveSelectedEdge(mesh);
            for (int i = 0; i < selected.Count; i++)
            {
                switch (type)
                {
                    case EnumPatchType.Vertex:
                        ShowOneRingVertexOfEdge(selected[i]);
                        break;
                    case EnumPatchType.Edge:
                        ShowOneRingEdgeOfEdge(selected[i]);
                        break;
                    case EnumPatchType.Face:
                        ShowOneRingFaceOfEdge(selected[i]);
                        break;

                }
            }
        }

        public static void ShowOneRingFaceOfFace(TriMesh mesh)
        {
            List<TriMesh.Face> selected = RetrieveSelectedFace(mesh);
            for (int i = 0; i < selected.Count; i++)
            {
                ShowOneRingFaceOfFace(selected[i]);
            }
        }

        public static void ShowOneRingOfFace(TriMesh mesh, EnumPatchType type)
        {
            List<TriMesh.Face> selected = RetrieveSelectedFace(mesh);
            foreach (var face in selected)
            {
                switch (type)
                {
                    case EnumPatchType.Vertex:
                        ShowOneRingOfPatch(mesh, EnumPatchType.Face, type);
                        break;
                    case EnumPatchType.Edge:
                        ShowOneRingOfPatch(mesh, EnumPatchType.Face, type);
                        break;
                    case EnumPatchType.Face:
                        ShowOneRingFaceOfFace(face);
                        break;
                    default:
                        break;
                }
            }
        }

        public static void ShowNeighborFaceOfFace(TriMesh mesh)
        {
            List<TriMesh.Face> selected = RetrieveSelectedFace(mesh);
            for (int i = 0; i < selected.Count; i++)
            {
                ShowNeighborFaceOfFace(selected[i]);
            }
        }


        public static void ShowOneRingFaceOfVertex(TriMesh mesh)
        {
            List<TriMesh.Vertex> selected = RetrieveSelectedVertex(mesh);
            for (int i = 0; i < selected.Count; i++)
            {
                ShowOneRingFaceOfVertex(selected[i]);
            }


        }

        public static void ShowOneRingEdgeOfVertex(TriMesh mesh)
        {
            List<TriMesh.Vertex> selected = RetrieveSelectedVertex(mesh);
            for (int i = 0; i < selected.Count; i++)
            {
                ShowOneRingEdgeOfVertex(selected[i]);
            }
        }

        public static void ShowBoundaryOfPatch(TriMesh mesh, EnumPatchType patchType, EnumPatchType searchType)
        {
            bool[] selectedFlags;
            TriMesh.HalfEdge[] list = null;
            switch (patchType)
            {
                case EnumPatchType.Vertex:
                case EnumPatchType.Edge:
                    selectedFlags = CollectVertex(mesh, patchType);
                    list = RetrieveBoundaryHalfEdgeByPatch(mesh, selectedFlags);
                    break;
                case EnumPatchType.Face:
                    selectedFlags = CollectFace(mesh);
                    list = RetrieveRegionBoundaryHalfEdge(mesh, selectedFlags);
                    break;
                default:
                    break;
            }
            for (int i = 0; i < list.Length; i++)
            {
                switch (searchType)
                {
                    case EnumPatchType.Vertex:
                        list[i].ToVertex.Traits.SelectedFlag = 1;
                        list[i].ToVertex.Traits.Color = RetrieveResult.Instance.VertexResult;
                        break;
                    case EnumPatchType.Edge:
                        list[i].Edge.Traits.SelectedFlag = 1;
                        list[i].Edge.Traits.Color = RetrieveResult.Instance.EdgeResult;
                        break;
                    case EnumPatchType.Face:
                        if (list[i].Opposite.Face != null)
                        {
                            list[i].Opposite.Face.Traits.SelectedFlag = 1;
                            list[i].Opposite.Face.Traits.Color = RetrieveResult.Instance.FaceResult;
                        }
                        break;
                }
            }
        }

        public static void ShowOneRingOfPatch(TriMesh mesh, EnumPatchType patchType, EnumPatchType searchType)
        {
            bool[] selectedFlags = CollectVertex(mesh, patchType);
            TriMesh.HalfEdge[] list = RetrieveOneRingHalfEdgeByPatch(mesh, selectedFlags);
            for (int i = 0; i < list.Length; i++)
            {
                switch (searchType)
                {
                    case EnumPatchType.Vertex:
                        list[i].ToVertex.Traits.SelectedFlag = 1;
                        list[i].ToVertex.Traits.Color = RetrieveResult.Instance.VertexResult;
                        break;
                    case EnumPatchType.Edge:
                        list[i].Edge.Traits.SelectedFlag = 1;
                        list[i].Edge.Traits.Color = RetrieveResult.Instance.EdgeResult;
                        break;
                    case EnumPatchType.Face:
                        list[i].Face.Traits.SelectedFlag = 1;
                        list[i].Face.Traits.Color = RetrieveResult.Instance.FaceResult;
                        break;
                }

            }
        }


        public static void ShowBoundary(TriMesh mesh)
        {
            List<List<TriMesh.HalfEdge>> boundaries = RetrieveBoundaryEdgeAll(mesh);
            for (int i = 0; i < boundaries.Count; i++)
            {
                for (int j = 0; j < boundaries[i].Count; j++)
                {
                    boundaries[i][j].Edge.Traits.SelectedFlag = 1;
                    boundaries[i][j].Edge.Traits.Color = Color4.Green;
                }
            }
        }
    }
}
