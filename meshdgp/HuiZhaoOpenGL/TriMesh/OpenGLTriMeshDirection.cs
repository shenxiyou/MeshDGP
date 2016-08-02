using System;
using System.Collections.Generic;
using System.Drawing; 

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public partial class OpenGLTriMesh
    {
        public void DrawVertexPrincipleDirection(TriMesh mesh)
        {
         OpenGLManager.Instance.SetColor(GlobalSetting.DisplaySetting.NormalColor);
            double avgLength = TriMeshUtil.ComputeEdgeAvgLength(mesh);
            avgLength *= GlobalSetting.DisplaySetting.NormalLength;
            foreach (TriMesh.Vertex item in mesh.Vertices)
            {
                Vector3D maxnormal = 
                    item.Traits.MaxCurvatureDirection.Normalize() * avgLength;
                Vector3D minnormal =  
                    item.Traits.MinCurvatureDirection.Normalize() * avgLength;
                double x = item.Traits.Position.x;
                double y = item.Traits.Position.y;
                double z = item.Traits.Position.z;
                GL.Material(MaterialFace.Front,MaterialParameter.Diffuse,
                            GlobalSetting.DisplaySetting.FirstPricipalColor);
                GL.Begin(BeginMode.Lines);
                
                GL.Vertex3(x, y, z);
                GL.Vertex3(x + maxnormal.x, y + maxnormal.y, z + maxnormal.z);
                GL.End();

                GL.Material(MaterialFace.Front, MaterialParameter.Diffuse,
                            GlobalSetting.DisplaySetting.SecondPricipalColor);
                GL.Begin(BeginMode.Lines);
                
                GL.Vertex3(x, y, z);
                GL.Vertex3(x + minnormal.x, y + minnormal.y, z + minnormal.z);
                GL.End(); 
            } 
        }

        public void DrawFaceNormal(TriMesh mesh)
        {
         OpenGLManager.Instance.SetColor(GlobalSetting.DisplaySetting.NormalColor);
            double avgLength = TriMeshUtil.ComputeEdgeAvgLength(mesh);
            avgLength *= GlobalSetting.DisplaySetting.NormalLength; 
            foreach (TriMesh.Face item in mesh.Faces)
            {
                double x = (item.GetVertex(0).Traits.Position.x +
                           item.GetVertex(1).Traits.Position.x +
                           item.GetVertex(2).Traits.Position.x) / 3; 
                double y = (item.GetVertex(0).Traits.Position.y +
                           item.GetVertex(1).Traits.Position.y +
                           item.GetVertex(2).Traits.Position.y) / 3; 
                double z = (item.GetVertex(0).Traits.Position.z +
                           item.GetVertex(1).Traits.Position.z +
                           item.GetVertex(2).Traits.Position.z) / 3; 
                Vector3D normal = item.Traits.Normal.Normalize() * avgLength; 
                GL.Begin(BeginMode.Lines);
                GL.Vertex3(x, y, z);
                GL.Vertex3(x + normal.x, y + normal.y, z + normal.z);
                GL.End(); 
            } 
        }

        public void DrawVertexNormal(TriMesh mesh)
        {
            OpenGLManager.Instance.SetColor(GlobalSetting.DisplaySetting.NormalColor);
            double avgLength = TriMeshUtil.ComputeEdgeAvgLength(mesh);
            avgLength *= GlobalSetting.DisplaySetting.NormalLength;
            foreach (TriMesh.Vertex item in mesh.Vertices)
            {
                Vector3D normal = item.Traits.Normal.Normalize() * avgLength;
                double x = item.Traits.Position.x;
                double y = item.Traits.Position.y;
                double z = item.Traits.Position.z;               
                GL.Begin(BeginMode.Lines);
                GL.Vertex3(x, y, z);
                GL.Vertex3(x + normal.x, y + normal.y, z + normal.z);
                GL.End();
            }
        }

        

    }
}
