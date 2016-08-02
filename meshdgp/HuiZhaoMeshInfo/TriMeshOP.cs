using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class TriMeshUtil
    {
        public static void SetUpNormalFace(TriMesh mesh)
        {
            Vector3D[] normal = TriMeshUtil.ComputeNormalFace(mesh);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                mesh.Faces[i].Traits.Normal = normal[i];
            }
        }

        public static void SetUpNormalVertex(TriMesh mesh)
        {
           Vector3D[] normal=  TriMeshUtil.ComputeNormalVertex(mesh);
           for (int i = 0; i < mesh.Vertices.Count; i++)
           {
               mesh.Vertices[i].Traits.Normal = normal[i];
           }
        }

        public static void SetUpVertexNormal(TriMesh mesh,EnumNormal type)
        {
            Vector3D[] normal = TriMeshUtil.ComputeNormalVertex(mesh,type);
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                mesh.Vertices[i].Traits.Normal = normal[i];
            }
        }

        public static void SetUpCurvature(TriMesh mesh)
        {

            

            PrincipalCurvature[] PrincipalCurv = TriMeshUtil.ComputePricipalCurvature(mesh);

            foreach (var v in  mesh.Vertices)
            {
                PrincipalCurvature pc =  PrincipalCurv[v.Index];
                v.Traits.MaxCurvature = pc.max;
                v.Traits.MaxCurvatureDirection = pc.maxDir;
                v.Traits.MinCurvature = pc.min;
                v.Traits.MinCurvatureDirection = pc.minDir;
                v.Traits.MeanCurvature = (pc.max + pc.min) / 2d;
                v.Traits.GaussianCurvature = pc.max * pc.min;
            }
        }

        public static void SetUpCurvature(TriMesh mesh, PrincipalCurvature[] PrincipalCurv)
        {
 

            foreach (var v in mesh.Vertices)
            {
                PrincipalCurvature pc = PrincipalCurv[v.Index];
                v.Traits.MaxCurvature = pc.max;
                v.Traits.MaxCurvatureDirection = pc.maxDir;
                v.Traits.MinCurvature = pc.min;
                v.Traits.MinCurvatureDirection = pc.minDir;
                v.Traits.MeanCurvature = (pc.max + pc.min) / 2d;
                v.Traits.GaussianCurvature = pc.max * pc.min;
            }
        }

        public static Color4 SetRandomColor(int i)
        {
          
            return new Color4((70 * i) % 256, (80 + 50 * i) % 256, (160 + 30 * i) % 256);
        }

        public static void SetRandomColor(TriMesh mesh)
        {
            foreach (TriMesh.Face face in mesh.Faces)
            {
                int i = face.Index;
                face.Traits.SelectedFlag = (byte)i;
                face.Traits.Color = TriMeshUtil.SetRandomColor(i);
            } 
        }

        public static void SetVertexColor(TriMesh mesh, double[] arr)
        {
            Color4[] color = ColorJet.Jet(arr, true);
            for (int i = 0; i < color.Length; i++)
            {
                mesh.Vertices[i].Traits.Color = color[i];
            }
        }

        public static void SetVertexColorNoNormalize(TriMesh mesh, double[] arr)
        {
            Color4[] color = ColorJet.Jet(arr, false);
            for (int i = 0; i < color.Length; i++)
            {
                mesh.Vertices[i].Traits.Color = color[i];
            }
        }

        public static void SetVertexColor(TriMesh mesh, Color4[] color)
        { 
            for (int i = 0; i < color.Length; i++)
            {
                mesh.Vertices[i].Traits.Color = color[i];
            }
        }

        public static Color4 GradientColor(float d)
        {
            return new Color4(Color4.HSB2RGB(d * 240, 1, 1), 1);
        }

    }
}
