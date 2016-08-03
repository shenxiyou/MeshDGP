using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
 

namespace GraphicResearchHuiZhao
{
    public partial class QuadMeshUtil
    {
        public static void ScaleToUnit(QuadMesh mesh,double scale)
        {

            Vector3D max = new Vector3D(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
            Vector3D min = new Vector3D(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            foreach (QuadMesh.Vertex v in mesh.Vertices)
            {
                for (int i = 0; i < 3; ++i)
                {
                    if (v.Traits.Position[i] > max[i])
                    {
                        max[i] = v.Traits.Position[i];
                    }
                    if (v.Traits.Position[i] < min[i])
                    {
                        min[i] = v.Traits.Position[i];
                    }
                }
            }

            Vector3D d = max - min;
            double s = (d.x > d.y) ? d.x : d.y;
            s = (s > d.z) ? s : d.z;
            if (s <= 0) return;



            foreach (QuadMesh.Vertex v in mesh.Vertices)
            {
                v.Traits.Position /= s;
                v.Traits.Position *= scale;
            }




        }

        public static void MoveToCenter(QuadMesh mesh)
        {
            Vector3D max = new Vector3D(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
            Vector3D min = new Vector3D(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            foreach (QuadMesh.Vertex v in mesh.Vertices)
            {
                for (int i = 0; i < 3; ++i)
                {
                    if (v.Traits.Position[i] > max[i])
                    {
                        max[i] = v.Traits.Position[i];
                    }
                    if (v.Traits.Position[i] < min[i])
                    {
                        min[i] = v.Traits.Position[i];
                    }
                }
            }
            Vector3D center = (max + min) / 2.0;

            foreach (QuadMesh.Vertex v in mesh.Vertices)
            {
                v.Traits.Position -= center;

            }

        }




        public static TriMesh ToTriMesh(QuadMesh mesh)
        {
            TriMesh trimesh = new TriMesh();
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                Vector3D position = mesh.Vertices[i].Traits.Position;
                trimesh.Vertices.Add(new VertexTraits(position.x, position.y, position.z));
            }

            foreach (QuadMesh.Face face in mesh.Faces)
            {
                int v0 = face.GetVertex(0).Index;
                int v1 = face.GetVertex(1).Index;
                int v2 = face.GetVertex(2).Index;
                int v3 = face.GetVertex(3).Index;
                TriMesh.Vertex[] faceVertices = new TriMesh.Vertex[3];
                faceVertices[0] = trimesh.Vertices[v0];
                faceVertices[1] = trimesh.Vertices[v1];
                faceVertices[2] = trimesh.Vertices[v2];
                trimesh.Faces.AddTriangles(faceVertices);

                faceVertices[0] = trimesh.Vertices[v2];
                faceVertices[1] = trimesh.Vertices[v3];
                faceVertices[2] = trimesh.Vertices[v0];
                trimesh.Faces.AddTriangles(faceVertices);

            }

            return trimesh;
        }

        public static void ComputeNormal(QuadMesh mesh)
        {
            foreach (QuadMesh.Face face in mesh.Faces)
            {
                QuadMesh.Vertex v0 = face.GetVertex(0);
                QuadMesh.Vertex v1 = face.GetVertex(1);
                QuadMesh.Vertex v2 = face.GetVertex(2);
                QuadMesh.Vertex v3 = face.GetVertex(3);


                Vector3D normal1 = (v2.Traits.Position - v1.Traits.Position).Cross(v0.Traits.Position - v2.Traits.Position);
                normal1.Normalize();

                Vector3D normal2 = (v0.Traits.Position - v3.Traits.Position).Cross(v2.Traits.Position - v0.Traits.Position);
                normal2.Normalize();

                face.Traits.Normal = (normal1 + normal2) / 2;
            }
            foreach (QuadMesh.Vertex vertex in mesh.Vertices)
            {
                vertex.Traits.Normal = Vector3D.Zero;
                foreach (QuadMesh.Face face in vertex.Faces)
                {
                    vertex.Traits.Normal += face.Traits.Normal;
                }
                vertex.Traits.Normal /= vertex.FaceCount;
            }
        }

        public static double ComputeArea(QuadMesh mesh)
        {
            double area = 0;
            foreach (QuadMesh.Face face in mesh.Faces)
            {
                Vector3D v0 = face.GetVertex(0).Traits.Position;
                Vector3D v1 = face.GetVertex(1).Traits.Position;
                Vector3D v2 = face.GetVertex(2).Traits.Position;
                Vector3D v3 = face.GetVertex(3).Traits.Position;
                area += ((v1 - v0).Cross(v2 - v0)).Length() / 2.0;
                area += ((v0 - v2).Cross(v3 - v2)).Length() / 2.0;
            }
            return area;
        }
        public static double ComputeVolume(QuadMesh mesh)
        {

            int n = mesh.Faces.Count;
            double volume = 0;
            for (int i = 0; i < n; i++)
            {

                Vector3D vertexA = mesh.Faces[i].GetVertex(0).Traits.Position;
                Vector3D vertexB = mesh.Faces[i].GetVertex(1).Traits.Position;
                Vector3D vertexC = mesh.Faces[i].GetVertex(2).Traits.Position;
                Vector3D vertexD = mesh.Faces[i].GetVertex(3).Traits.Position;

                double v123 = vertexA.x * vertexB.y * vertexC.z;
                double v231 = vertexA.y * vertexB.z * vertexC.x;
                double v312 = vertexA.z * vertexB.x * vertexC.y;
                double v132 = vertexA.x * vertexB.z * vertexC.y;
                double v213 = vertexA.y * vertexB.x * vertexC.z;
                double v321 = vertexA.z * vertexB.y * vertexC.x;

                volume += (v123 + v231 + v312 - v132 - v213 - v321) / 6;

                double d123 = vertexC.x * vertexD.y * vertexA.z;
                double d231 = vertexC.y * vertexD.z * vertexA.x;
                double d312 = vertexC.z * vertexD.x * vertexA.y;
                double d132 = vertexC.x * vertexD.z * vertexA.y;
                double d213 = vertexC.y * vertexD.x * vertexA.z;
                double d321 = vertexC.z * vertexD.y * vertexA.x;
                volume += (d123 + d231 + d312 - d132 - d213 - d321) / 6;

            }
            return volume;
        }


    }
}
