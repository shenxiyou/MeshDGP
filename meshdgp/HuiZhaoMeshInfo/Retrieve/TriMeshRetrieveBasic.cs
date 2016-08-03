using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public partial class TriMeshUtil
    {
        public static void ShowOneRingVertexOfVertex(TriMesh.Vertex vertex)
        {
            foreach (TriMesh.Vertex neighbors in vertex.Vertices)
            {
                neighbors.Traits.SelectedFlag = 8;
                neighbors.Traits.Color = RetrieveResult.Instance.VertexResult;
            }
        }

        public static void ShowTwoRingVertexOfVertex(TriMesh.Vertex vertex)
        {
            foreach (TriMesh.Vertex neighbors in vertex.Vertices)
            {
                neighbors.Traits.SelectedFlag = 8;
                neighbors.Traits.Color =RetrieveResult.Instance.VertexResult;

                foreach (TriMesh.Vertex item in neighbors.Vertices)
                {
                    item.Traits.SelectedFlag = 8;
                    item.Traits.Color = RetrieveResult.Instance.VertexResult;
                }
            }

            vertex.Traits.Color = Color4.Black;
        }

        public static void ShowOneRingFaceOfVertex(TriMesh.Vertex vertex)
        {
            foreach (TriMesh.Face neighbors in vertex.Faces)
            {
                neighbors.Traits.SelectedFlag = 9;
                neighbors.Traits.Color = RetrieveResult.Instance.FaceResult;
            }
        }

        public static void ShowOneRingEdgeOfVertex(TriMesh.Vertex vertex)
        {
            foreach (TriMesh.HalfEdge neighbors in vertex.HalfEdges)
            {
                neighbors.Next.Edge.Traits.SelectedFlag = 10;
                neighbors.Next.Edge.Traits.Color = RetrieveResult.Instance.EdgeResult;
            }
        }

        public static void ShowOneRingVertexOfEdge(TriMesh.Edge edge)
        {
            foreach (var item in  RetrieveOneRingVertexOfEdge(edge))
            {
                item.Traits.SelectedFlag = 8;
                item.Traits.Color = RetrieveResult.Instance.VertexResult;
            }
        }

        public static void ShowOneRingEdgeOfEdge(TriMesh.Edge edge)
        {
            foreach (var item in  RetrieveOneRingEdgeOfEdge(edge))
            {
                item.Edge.Traits.SelectedFlag = 8;
                item.Edge.Traits.Color = RetrieveResult.Instance.EdgeResult;
            }
            edge.Traits.SelectedFlag = 10;
        }

        public static void ShowOneRingFaceOfEdge(TriMesh.Edge edge)
        {
            ShowOneRingFaceOfVertex(edge.Vertex0);
            ShowOneRingFaceOfVertex(edge.Vertex1);
        }

        public static void ShowNeighborFaceOfFace(TriMesh.Face face)
        {
            foreach (TriMesh.Face neighbors in face.Faces)
            {
                neighbors.Traits.SelectedFlag = 9;
                neighbors.Traits.Color = RetrieveResult.Instance.FaceResult;
            }
            face.Traits.Color = Color4.Black;
        }

        public static void ShowOneRingFaceOfFace(TriMesh.Face face)
        {
            foreach (var item in face.Vertices)
            {
                ShowOneRingFaceOfVertex(item);
            }
            face.Traits.Color = Color4.Black;
        }
    }
}
