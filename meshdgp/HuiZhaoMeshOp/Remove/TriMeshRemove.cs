using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public partial class TriMeshModify 
    {
       

        public static void RemoveTwoRingOfVertex(TriMesh mesh)
        {
            List<TriMesh.Vertex> selected = TriMeshUtil.RetrieveSelectedVertex(mesh); 
            for (int i = 0; i < selected.Count; i++)
            {
                RemoveTwoRingOfVertex(selected[i]); 
            }
            TriMeshUtil.FixIndex(mesh);
            
        }

        public static void RemoveOneRingOfVertex(TriMesh mesh)
        {
            List<TriMesh.Vertex> selected = TriMeshUtil.RetrieveSelectedVertex(mesh); 
            for (int i = 0; i < selected.Count; i++)
            {
                RemoveVertex(selected[i]);

            }
            TriMeshUtil.FixIndex(mesh);
        }





        public static void RemoveSelectedFaces(TriMesh mesh)
        {
            List<TriMesh.Face> selected = TriMeshUtil.RetrieveSelectedFace(mesh);
            for (int i = 0; i < selected.Count; i++)
            {
                RemoveFace(selected[i]);
            }
            TriMeshUtil.FixIndex(mesh);
        }

        public static void RemoveSelectedEdges(TriMesh mesh)
        {
            List<TriMesh.Edge> selected = TriMeshUtil.RetrieveSelectedEdge(mesh);

            for (int i = 0; i < selected.Count; i++)
            {
                RemoveEdge(selected[i]);
            }
            TriMeshUtil.FixIndex(mesh);
        }

        public static void RemoveOneRingOfEdge(TriMesh mesh)
        {
            List<TriMesh.Edge> selected = TriMeshUtil.RetrieveSelectedEdge(mesh);

            for (int i = 0; i < selected.Count; i++)
            {
                RemoveOneRingOfEdge(selected[i]);
            }
            TriMeshUtil.FixIndex(mesh);
        }



        public static PolygonMesh AddOneTriangle()
        {
            PolygonMesh mesh = new PolygonMesh();

            return mesh;
        }


        public static TriMesh CreateSquare()
        {
            double length = 0.5;
            double width = 0.3;
            TriMesh Shape = new TriMesh();
            Shape.Vertices.Add(new VertexTraits(-width, -length, 0));
            Shape.Vertices.Add(new VertexTraits(width, -length, 0));
            Shape.Vertices.Add(new VertexTraits(-width, length, 0));
            Shape.Vertices.Add(new VertexTraits(width, length, 0));

            TriMesh.Vertex[] faceVertices = new TriMesh.Vertex[3];
            faceVertices[0] = Shape.Vertices[0];
            faceVertices[1] = Shape.Vertices[1];
            faceVertices[2] = Shape.Vertices[2];
            CreateFace(Shape,faceVertices);

            faceVertices = new TriMesh.Vertex[3];
            faceVertices[0] = Shape.Vertices[1];
            faceVertices[1] = Shape.Vertices[3];
            faceVertices[2] = Shape.Vertices[2];
            CreateFace(Shape, faceVertices);

            faceVertices = new TriMesh.Vertex[3];
            faceVertices[0] = Shape.Vertices[2];
            faceVertices[1] = Shape.Vertices[3];
            faceVertices[2] = Shape.Vertices[0];
            CreateFace(Shape, faceVertices);

            faceVertices = new TriMesh.Vertex[3];
            faceVertices[0] = Shape.Vertices[3];
            faceVertices[1] = Shape.Vertices[1];
            faceVertices[2] = Shape.Vertices[0];
            CreateFace(Shape, faceVertices);


            return Shape;

        }
    }
}
