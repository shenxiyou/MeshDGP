using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class TriMeshModify
    {
        public static void InverseFace(TriMesh mesh)
        {
            List<TriMesh.Vertex[]> faces = new List<HalfEdgeMesh.Vertex[]>();
            foreach (TriMesh.Face face in mesh.Faces)
            {
                TriMesh.HalfEdge hf = face.HalfEdge;
                TriMesh.Vertex[] arr = new TriMesh.Vertex[]{
                    hf.Next.ToVertex,
                    hf.ToVertex,
                    hf.FromVertex
                };
                faces.Add(arr);
            }
            TriMesh.Vertex[] vertices = new TriMesh.Vertex[mesh.Vertices.Count];
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                vertices[i] = mesh.Vertices[i];
                vertices[i].HalfEdge = null;
            }
            mesh.Clear(); 
            foreach (var v in vertices)
            {
                mesh.AppendToVertexList(v);
            } 
            foreach (var face in faces)
            {
                mesh.Faces.AddTriangles(face);
            }
        }



        public static void Move(TriMesh mesh, Vector3D vec)
        {
            foreach (var v in mesh.Vertices)
            {
                v.Traits.Position += vec;
            }
        }
         
    }
}
