using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public static partial class TriMeshUtil
    {
       
        //vertex area
        public static double ComputeAreaOneRing(TriMesh.Vertex vertex)
        {
            double area = 0;
            foreach (TriMesh.Face face in vertex.Faces)
            {
                area += TriMeshUtil.ComputeAreaFace(face);
            }
            return area;
        }

        public static double ComputeAreaVoronoi(TriMesh.Vertex v)
        {
            double sum = 0;
            Vector3D mid = v.Traits.Position;
            foreach (var hf in v.HalfEdges)
            {
                Vector3D buttom = hf.ToVertex.Traits.Position;
                Vector3D left = hf.Opposite.Next.ToVertex.Traits.Position;
                Vector3D right = hf.Next.ToVertex.Traits.Position;
                double cota = (mid - left).Dot(buttom - left)
                    / (mid - left).Cross(buttom - left).Length();
                double cotb = (mid - right).Dot(buttom - right) 
                    / (mid - right).Cross(buttom - right).Length();
                double d = Vector3D.DistanceSquared(mid, buttom);
                sum += (cota + cotb) * d;
            }
            double area= sum / 8d;;
            return area;
        }

        public static double[] ComputeAreaVoronoi(TriMesh mesh)
        {
            double[] area = new double[mesh.Vertices.Count];
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                area[i] = ComputeAreaVoronoi(mesh.Vertices[i]);
            }
            return area;
        }

        public static double ComputeAreaMixed(TriMesh.Vertex v)
        {
            double area = 0;
            foreach (var hf in v.HalfEdges)
            {
                TriMesh.Vertex p1 = hf.FromVertex;
                TriMesh.Vertex p2 = hf.ToVertex;
                TriMesh.Vertex p3 = hf.Next.ToVertex;
                Vector3D v1 = p1.Traits.Position;
                Vector3D v2 = p2.Traits.Position;
                Vector3D v3 = p3.Traits.Position;
                double dis1 = (v3 - v2).Length() * (v3 - v2).Length();
                double dis2 = (v3 - v1).Length() * (v3 - v1).Length();
                double dis3 = (v1 - v2).Length() * (v1 - v2).Length();
                double cot1 = (v2 - v1).Dot(v3 - v1) / 
                              (v2 - v1).Cross(v3 - v1).Length();
                double cot2 = (v3 - v2).Dot(v1 - v2) / 
                              (v3 - v2).Cross(v1 - v2).Length();
                double cot3 = (v1 - v3).Dot(v2 - v3) / 
                              (v1 - v3).Cross(v2 - v3).Length();
                if (cot1 > 0 && cot2 > 0 && cot3 > 0)
                {
                    area += (dis2 * cot2 + dis3 * cot3) / 8;
                }
                else
                {
                    if (hf.Face == null)
                        continue;
                    double faceArea = ComputeAreaFace(hf.Face);
                    if (cot1 < 0)
                    {
                        area += faceArea / 2;
                    }
                    else
                    {
                        area += faceArea / 4;
                    }
                }
            }
            return area;
        }

        public static double[] ComputeAreaMixed(TriMesh mesh)
        {
            int n = mesh.Vertices.Count;
            double[] voronoiArea = new double[n];
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                TriMesh.Face face = mesh.Faces[i];
                TriMesh.Vertex p1 = face.HalfEdge.ToVertex;
                TriMesh.Vertex p2 = face.HalfEdge.Next.ToVertex;
                TriMesh.Vertex p3 = face.HalfEdge.FromVertex;
                int c1 = p1.Index;
                int c2 = p2.Index;
                int c3 = p3.Index;
                Vector3D v1 = p1.Traits.Position;
                Vector3D v2 = p2.Traits.Position;
                Vector3D v3 = p3.Traits.Position;
                double dis1 = (v3 - v2).Length() * (v3 - v2).Length();
                double dis2 = (v3 - v1).Length() * (v3 - v1).Length();
                double dis3 = (v1 - v2).Length() * (v1 - v2).Length();
                double cot1 = (v2 - v1).Dot(v3 - v1) / (v2 - v1).Cross(v3 - v1).Length();
                double cot2 = (v3 - v2).Dot(v1 - v2) / (v3 - v2).Cross(v1 - v2).Length();
                double cot3 = (v1 - v3).Dot(v2 - v3) / (v1 - v3).Cross(v2 - v3).Length();
                bool obtuse = true;
                if (cot1 > 0 && cot2 > 0 && cot3 > 0)
                {
                    obtuse = false;
                }
                if (!obtuse)
                {
                    voronoiArea[c1] += (dis2 * cot2 + dis3 * cot3) / 8;
                    voronoiArea[c2] += (dis1 * cot1 + dis3 * cot3) / 8;
                    voronoiArea[c3] += (dis1 * cot1 + dis2 * cot2) / 8;
                }
                else
                {
                    double faceArea = ComputeAreaFace(mesh.Faces[i]);
                    if (cot1 < 0)
                    {
                        voronoiArea[c1] += faceArea / 2;
                    }
                    else
                    {
                        voronoiArea[c1] += faceArea / 4;
                    }

                    if (cot2 < 0)
                    {
                        voronoiArea[c2] += faceArea / 2;
                    }
                    else
                    {
                        voronoiArea[c2] += faceArea / 4;
                    }

                    if (cot3 < 0)
                    {
                        voronoiArea[c3] += faceArea / 2;
                    }
                    else
                    {
                        voronoiArea[c3] += faceArea / 4;
                    }
                }
            }
            return voronoiArea;
        }


        //face area


        public static double ComputeAreaFaceTwo(TriMesh.Face face)
        {

            Vector3D v1 = new Vector3D(face.GetVertex(0).Traits.Position.x,
                                       face.GetVertex(0).Traits.Position.y,
                                       face.GetVertex(0).Traits.Position.z);
            Vector3D v2 = new Vector3D(face.GetVertex(1).Traits.Position.x,
                                       face.GetVertex(1).Traits.Position.y,
                                       face.GetVertex(1).Traits.Position.z);
            Vector3D v3 = new Vector3D(face.GetVertex(2).Traits.Position.x,
                                       face.GetVertex(2).Traits.Position.y,
                                       face.GetVertex(2).Traits.Position.z);

            double a = Math.Sqrt((v1.x - v2.x) * (v1.x - v2.x) 
                                +(v1.y - v2.y) * (v1.y - v2.y) 
                                +(v1.z - v2.z) * (v1.z - v2.z));
            double b = Math.Sqrt((v3.x - v2.x) * (v3.x - v2.x) 
                                +(v3.y - v2.y) * (v3.y - v2.y) 
                                +(v3.z - v2.z) * (v3.z - v2.z));
            double c = Math.Sqrt((v1.x - v3.x) * (v1.x - v3.x) 
                                +(v1.y - v3.y) * (v1.y - v3.y) 
                                +(v1.z - v3.z) * (v1.z - v3.z));
            double p = (a + b + c) / 2;
            double area=Math.Sqrt(p * (p - a) * (p - b) * (p - c));

            return area;
        }

        public static double ComputeAreaFace(TriMesh.Face face)
        {
            return Cross(face).Length() / 2d;
        }

        public static double[] ComputeAreaFace(TriMesh mesh)
        {
            double[] area = new double[mesh.Faces.Count];
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                area[i] = ComputeAreaFaceTwo(mesh.Faces[i]);
            }
            return area;
        }

        public static double ComputeAreaAverage(TriMesh mesh)
        {
            int n = mesh.Faces.Count;
            double area = 0;
            for (int i = 0; i < n; i++)
            {
                area += ComputeAreaFace(mesh.Faces[i]);
            }
            return area / n;
        }

        public static double ComputeAreaTotal(TriMesh mesh)
        {
            int n = mesh.Faces.Count;
            double area = 0;
            for (int i = 0; i < n; i++)
            {
                area += ComputeAreaFace(mesh.Faces[i]);
            }
            return area;
        }


        public static Vector3D ComputeFaceCenter(TriMesh.Face face)
        {
            Vector3D center= Vector3D.Zero;
            foreach(TriMesh.Vertex v in face.Vertices)
            {
                center += v.Traits.Position;
            }
            center = center / 3d;
            return center;
        }

        public static List<Vector3D> ComputeFaceCenter(TriMesh mesh)
        {
            List<Vector3D> centers = new List<Vector3D>();
            for(int i=0;i<mesh.Faces.Count;i++)
            {
                Vector3D c =  ComputeFaceCenter(mesh.Faces[i]) ;
                centers.Add(c);
                
            }
            return centers;
        }


        /// <summary>
        /// 4点内体积
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns></returns>
        public static double ComputeVolume(params TriMesh.Vertex[] vertices)
        {
            Vector3D[] v = new Vector3D[4];
            for (int i = 0; i < 4; i++)
            {
                v[i] = vertices[i].Traits.Position;
            }
            return ComputeVolume(v);
        }

        public static double ComputeVolume(params Vector3D[] v)
        {
            return Math.Abs((v[1] - v[0]).Cross(v[2] - v[0]).Dot(v[3] - v[0]) / 6d);
        }

        public static double ComputeVolume(TriMesh mesh)
        {
            int n = mesh.Faces.Count;
            double volume = 0;
            for (int i = 0; i < n; i++)
            {
                Vector3D vertexA = mesh.Faces[i].GetVertex(0).Traits.Position;
                Vector3D vertexB = mesh.Faces[i].GetVertex(1).Traits.Position;
                Vector3D vertexC = mesh.Faces[i].GetVertex(2).Traits.Position;
                double v123 = vertexA.x * vertexB.y * vertexC.z;
                double v231 = vertexA.y * vertexB.z * vertexC.x;
                double v312 = vertexA.z * vertexB.x * vertexC.y;
                double v132 = vertexA.x * vertexB.z * vertexC.y;
                double v213 = vertexA.y * vertexB.x * vertexC.z;
                double v321 = vertexA.z * vertexB.y * vertexC.x;
                volume += (v123 + v231 + v312 - v132 - v213 - v321) / 6;
            }
            return volume;
        }

    }
}
