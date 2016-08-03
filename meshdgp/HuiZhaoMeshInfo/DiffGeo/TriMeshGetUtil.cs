using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public static partial class TriMeshUtil
    {

        public static double[] Add(double[] vector1, List<double> vector2)
        {
            double[] result =new  double[vector1.Length];
            for (int i = 0; i < vector1.Length; i++)
            {
               result[i]=(vector1[i]+ vector2[i]);
            }
            return result;
        }
        public static double Multiply(List<double> vector1, List<double> vector2)
        {
            double result = 0;
            for (int i = 0; i < vector1.Count; i++)
            {
                result += vector1[i] * vector2[i];
            }
            return result;
        }

        public static List<double> Multiply(List<double> vector, double factor)
        {
            List<double> result = new List<double>(vector.Count);
            for (int i = 0; i < vector.Count; i++)
            {
                result.Add(vector[i] * factor);
            }
            return result;
        }

        public static List<double> GetX(TriMesh mesh)
        {
            List<double> X = new List<double>(mesh.Vertices.Count);

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
               X.Add(mesh.Vertices[i].Traits.Position.x);
            }

            return X;
        }

        public static List<double> GetY(TriMesh mesh)
        {
            List<double> X = new List<double>(mesh.Vertices.Count);

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                X.Add(mesh.Vertices[i].Traits.Position.y);
            }

            return X;
        }

        public static List<double> GetZ(TriMesh mesh)
        {
            List<double> X = new List<double>(mesh.Vertices.Count);

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
               X.Add(mesh.Vertices[i].Traits.Position.z);
            }

            return X;
        }

        public static Vector3D[] GetHalfEdgesVector(TriMesh.Vertex v)
        {
            List<Vector3D> list = new List<Vector3D>(12);
            foreach (var hf in v.HalfEdges)
            {
                list.Add(GetHalfEdgeVector(hf));
            }
            return list.ToArray();
        }


        public static Triple<Vector3D> GetHalfEdgesVector(TriMesh.Face face)
        {
            TriMesh.HalfEdge hf = face.HalfEdge;
            return new Triple<Vector3D>()
            {
                T0 = GetHalfEdgeVector(hf),
                T1 = GetHalfEdgeVector(hf.Next),
                T2 = GetHalfEdgeVector(hf.Previous)
            };
        }

        public static Triple<Vector3D> GetVerticesPosition(TriMesh.Face face)
        {
            TriMesh.HalfEdge hf = face.HalfEdge;
            return new Triple<Vector3D>()
            {
                T0 = hf.ToVertex.Traits.Position,
                T1 = hf.Next.ToVertex.Traits.Position,
                T2 = hf.FromVertex.Traits.Position
            };
        }

        public static Vector3D GetHalfEdgeVector(TriMesh.HalfEdge hf)
        {
            return hf.ToVertex.Traits.Position - hf.FromVertex.Traits.Position;
        }



        public static Vector3D GetMidPoint(TriMesh.Face face)
        {
            Vector3D sum = new Vector3D();
            foreach (var v in face.Vertices)
            {
                sum += v.Traits.Position;
            }
            return sum / 3;
        }

        public static Vector3D GetMidPoint(TriMesh.Edge edge)
        {
            return (edge.Vertex0.Traits.Position + edge.Vertex1.Traits.Position) / 2;
        }

        

        public static double ComputeEdgeLength(TriMesh.Edge edge)
        {
            return (edge.Vertex0.Traits.Position -
                             edge.Vertex1.Traits.Position).Length();
        }

        public static double[] ComputeEdgeLength(TriMesh mesh)
        {
            double[] edgeLength = new double[mesh.Edges.Count];
            for(int i=0;i<mesh.Edges.Count;i++)
            {
                edgeLength[i] = TriMeshUtil.ComputeEdgeLength(mesh.Edges[i]);

            }
            return edgeLength;

        }

        public static double ComputeEdgeLength(TriMesh.Vertex vertex1, TriMesh.Vertex vertex2)
        {
            double x1 = vertex1.Traits.Position.x;
            double y1 = vertex1.Traits.Position.y;
            double z1 = vertex1.Traits.Position.z;

            double x2 = vertex2.Traits.Position.x;
            double y2 = vertex2.Traits.Position.y;
            double z2 = vertex2.Traits.Position.z;


            double lengthOftheEdge = Math.Sqrt(Math.Pow(x1 - x2, 2)
                    + Math.Pow(y1 - y2, 2)
                    + Math.Pow(z1 - z2, 2));

            return lengthOftheEdge;
        }

        public static Vector3D Cross(TriMesh.Face face)
        {
            Triple<Vector3D> t = GetVerticesPosition(face);

            return (t.T1 - t.T0).Cross(t.T2 - t.T0);
        }






        public static double ComputePerimeter(TriMesh.Face face)
        {
            Triple<Vector3D> t = TriMeshUtil.GetHalfEdgesVector(face);
            return t.T0.Length() + t.T1.Length() + t.T2.Length();
        }

        public static TriMesh.Face FindFace(TriMesh.Vertex v1, TriMesh.Vertex v2)
        {
            TriMesh.HalfEdge hf = v1.FindHalfedgeTo(v2);
            return hf == null ? null : hf.Face;
        }

        public static TriMesh.Edge  FindEdge(TriMesh.Vertex v1, TriMesh.Vertex v2)
        {
            TriMesh.HalfEdge hf = v1.FindHalfedgeTo(v2);
            return hf == null ? null : hf.Edge;
        }

       
    }
}
