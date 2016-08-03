using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public static partial class TriMeshUtil
    {
        #region Normal

        #region Uniform

    

        public static Vector3D ComputeNormalUniformWeight(TriMesh.Vertex v)
        {
            Vector3D normal = Vector3D.Zero;
            foreach (var face in v.Faces)
            {
                normal += ComputeNormalFace(face);
            }
            return normal.Normalize();
        }

        public static Vector3D ComputeNormalUniformWeight(TriMesh.Vertex v, Vector3D[] faceNormal)
        {
            Vector3D normal = Vector3D.Zero;
            foreach (var face in v.Faces)
            {
                normal += faceNormal[face.Index];
            }
            return normal.Normalize();
        }


        public static Vector3D[] ComputeNormalVertex(TriMesh mesh)
        {
            
           
            return ComputeNormalUniformWeight(mesh); 
        }


        public static Vector3D[] ComputeNormalVertex(TriMesh mesh, EnumNormal type)
        {
            if (type == null)
            {
                type=EnumNormal.UniformWeight ;
            }

            Vector3D[] normal = null;
            switch (type)
            {
                case EnumNormal.AreaWeight:
                    normal= ComputeNormalAreaWeight(mesh);
                    break;
                case EnumNormal.SphereInscribed:
                    normal = ComputeNormalSphereInscribed(mesh);
                    break;
                case EnumNormal.TipAngleWeight:
                    normal = ComputeNormalTipAngleWeight(mesh);
                    break;
                case EnumNormal.UniformWeight:
                    normal = ComputeNormalUniformWeight(mesh);
                    break;

                    
            }
           
            return normal;
        }


        public static Vector3D[] ComputeNormalUniformWeight(TriMesh mesh)
        {
            Vector3D[] arr = new Vector3D[mesh.Vertices.Count];
            Vector3D[] faceNormal = ComputeNormalFace(mesh);
            foreach (var v in mesh.Vertices)
            {
                arr[v.Index] = ComputeNormalUniformWeight(v, faceNormal);
            }
            return arr;
        }


        #endregion

        #region AreaWeight

        public static Vector3D ComputeNormalAreaWeight(TriMesh.Vertex v)
        {
            Vector3D normal = Vector3D.Zero;
            foreach (var face in v.Faces)
            {
                double area = ComputeAreaFace(face);
                normal += area * ComputeNormalFace(face);
            }
            return normal.Normalize();
        }

        public static Vector3D ComputeNormalAreaWeight(TriMesh.Vertex v, double[] faceArea, Vector3D[] faceNormal)
        {
            Vector3D normal = Vector3D.Zero;
            foreach (var face in v.Faces)
            {
                double area = faceArea[face.Index];
                normal += area * faceNormal[face.Index];
            }
            return normal.Normalize();
        }

        public static Vector3D[] ComputeNormalAreaWeight(TriMesh mesh)
        {
            Vector3D[] arr = new Vector3D[mesh.Vertices.Count];
            double[] faceArea = ComputeAreaFace(mesh);
            Vector3D[] faceNormal = ComputeNormalFace(mesh);
            foreach (var v in mesh.Vertices)
            {
                arr[v.Index] = ComputeNormalAreaWeight(v, faceArea, faceNormal);
            }
            return arr;
        }

        #endregion

        #region TipAngleWeight

        public static Vector3D ComputeNormalTipAngleWeight(TriMesh.Vertex v)
        {
            Vector3D normal = Vector3D.Zero;
            foreach (var hf in v.HalfEdges)
            {
                if (hf.Face != null)
                {
                    double angle = ComputeAngle(hf.Next);
                    normal += angle * ComputeNormalFace(hf.Face);
                }
            }
            return normal.Normalize();
        }

        public static Vector3D ComputeNormalTipAngleWeight(TriMesh.Vertex v, Vector3D[] faceNormal)
        {
            Vector3D normal = Vector3D.Zero;
            foreach (var hf in v.HalfEdges)
            {
                if (hf.Face != null)
                {
                    double angle = TriMeshUtil.ComputeAngle(hf.Next);
                    normal += angle * faceNormal[hf.Face.Index];
                }
            }
            return normal.Normalize();
        }

        public static Vector3D[] ComputeNormalTipAngleWeight(TriMesh mesh)
        {
            Vector3D[] arr = new Vector3D[mesh.Vertices.Count];
            Vector3D[] faceNormal = ComputeNormalFace(mesh);
            foreach (var v in mesh.Vertices)
            {
                arr[v.Index] = ComputeNormalTipAngleWeight(v, faceNormal);
            }
            return arr;
        }

        #endregion

        public static Vector3D ComputeNormalSphereInscribed(TriMesh.Vertex v)
        {
            Vector3D normal = Vector3D.Zero;
            Vector3D[] vec = TriMeshUtil.GetHalfEdgesVector(v);
            double[] len = new double[vec.Length];
            for (int i = 0; i < vec.Length; i++)
            {
                len[i] = vec[i].Length();
            }
            for (int i = 0; i < vec.Length; i++)
            {
                int j = (i + 1) % vec.Length;
                Vector3D e1 = vec[i];
                Vector3D e2 = -vec[j];
                normal += e1.Cross(e2) / (len[i] * len[i] * len[j] * len[j]);
            }
            return normal.Normalize();
        }

        public static Vector3D[] ComputeNormalSphereInscribed(TriMesh mesh)
        {
            Vector3D[] arr = new Vector3D[mesh.Vertices.Count];
            foreach (var v in mesh.Vertices)
            {
                arr[v.Index] = ComputeNormalSphereInscribed(v);
            }
            return arr;
        }

        //public static Vector3D ComputeLengthWeightNormal(TriMesh.Vertex v, TriMeshBasic basic)
        //{
        //    Vector3D normal = Vector3D.Zero;
        //    foreach (var hf in v.Halfedges)
        //    {
        //        if (hf.Face != null)
        //        {
        //            double weight = basic.EdgeLength[hf.Next.Edge.Index] / basic.FacePerimeter[hf.Face.Index];
        //            normal += weight * basic.FaceNormal[hf.Face.Index];
        //        }
        //    }
        //    return normal.Normalize();
        //}

        public static Vector3D ComputeCurvNormal(Vector3D K)
        {
            return K.Normalize();
        }

        #endregion



        public static Vector3D ComputeNormalFace(TriMesh.Face face)
        {
            return Cross(face).Normalize();
        }

        public static Vector3D[] ComputeNormalFace(TriMesh mesh)
        {
            Vector3D[] normal = new Vector3D[mesh.Faces.Count];
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                normal[i] = ComputeNormalFace(mesh.Faces[i]);
            }
            return normal;
        }
    }
}
