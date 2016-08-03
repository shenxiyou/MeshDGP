
using System;
using System.Collections.Generic; 
using System.Text;
 

namespace GraphicResearchHuiZhao
{

    public partial class TriMeshModify
    {
        

        public static void SmoothTaubin(TriMesh Mesh)
        {
            double weight =  ConfigMeshOP.Instance.SmoothTaubinLamda;
            int iterative = ConfigMeshOP.Instance.SmoothTaubinIterative;
            bool cot=ConfigMeshOP.Instance.SmoothTaubinCot;
            int n = Mesh.Vertices.Count;  
            double[][] lap =null; 
            for (int j = 0; j < iterative; j++)
            {
                if (cot)
                {
                    lap = LaplaceManager.Instance.ComputeLaplacianCotNormalize(Mesh);
                }
                else
                {
                    lap = LaplaceManager.Instance.ComputeLaplacianUniform(Mesh);
                }
                for (int i = 0; i < n; i++)
                {
                   Mesh.Vertices[i].Traits.Position.x += lap[0][i] * weight;
                   Mesh.Vertices[i].Traits.Position.y += lap[1][i] * weight;
                   Mesh.Vertices[i].Traits.Position.z += lap[2][i] * weight;
                }
            } 
        }

        //public static void PreserveVolume(TriMesh mesh)
        //{
        //    double oldVolume = TriMeshUtil.ComputeVolume(Mesh);

        //    double newVolume = TriMeshUtil.ComputeVolume(Mesh);
        //    for (int i = 0; i < n; i++)
        //    {
        //        Mesh.Vertices[i].Traits.Position.x *= Math.Pow(oldVolume / newVolume, 1 / 3);
        //        Mesh.Vertices[i].Traits.Position.y *= Math.Pow(oldVolume / newVolume, 1 / 3);
        //        Mesh.Vertices[i].Traits.Position.z *= Math.Pow(oldVolume / newVolume, 1 / 3);
        //    }

        //}


        public static void PreModifiyGeometry(TriMesh mesh)
        {
            double ModifyVertexScale = 100; 
            int n = mesh.Vertices.Count;  
            for (int j = 0; j < 10; j++)
            {
                bool updated = false; 
                TriMeshUtil.SetUpNormalVertex(mesh); 
                for (int i = 0; i < n; i++)
                {
                    // build matrix U
                    Matrix3D U = new Matrix3D();
                    Vector3D u1 = mesh.Vertices[i].Traits.Position;
                    foreach (TriMesh.Vertex neighbor in mesh.Vertices[i].Vertices)
                    {
                        Vector3D u21 = neighbor.Traits.Position - u1;
                        U += u21.OuterCross(u21);
                    }

                    // Test for degenerated
                    Matrix3D invU = U.InverseSVD(); 
                    if (Matrix3D.lastSVDIsFullRank == false)
                    {
                        Vector3D normal = mesh.Vertices[i].Traits.Normal;
                        double area = 0;
                        foreach (TriMesh.Face face in mesh.Vertices[i].Faces)
                        {
                            area += TriMeshUtil.ComputeAreaFace(face);
                        }
                        Vector3D newU1 = u1 + normal.Normalize() * Math.Sqrt(area / 3.0) / ModifyVertexScale;
                        mesh.Vertices[i].Traits.Position = newU1; 
                        updated = true; 
                    }
                }
                if (!updated)
                    break;
            }
        }

    }
}
