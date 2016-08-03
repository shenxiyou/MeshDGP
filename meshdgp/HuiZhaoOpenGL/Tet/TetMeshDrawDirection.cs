using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public partial class OpenGLTetMesh
    {
        public void DrawFaceNormal(TetMesh mesh)
        {
            OpenGLManager.Instance.SetColorMesh(GlobalSetting.DisplaySetting.NormalColor);
            double l = mesh.AvgLength * GlobalSetting.DisplaySetting.NormalLength;
            GL.Begin(BeginMode.Lines);
            foreach (var face in mesh.Faces)
            {
                if (face.Normal != Vector3D.Zero)
                {
                    Vector3D mid = TetMeshUtil.GetMidPoint(face);
                    Vector3D normal = face.Normal * l;
                    GL.Vertex3(mid.ToArray());
                    GL.Vertex3((mid + normal).ToArray());
                }
            }
            GL.End();
        }

        public void DrawVertexNormal(TetMesh mesh)
        {
            OpenGLManager.Instance.SetColorMesh(GlobalSetting.DisplaySetting.NormalColor);
            double l = mesh.AvgLength * GlobalSetting.DisplaySetting.NormalLength;
            GL.Begin(BeginMode.Lines);
            foreach (var v in mesh.Vertices)
            {
                if (v.OnBoundary)
                {
                    Vector3D normal = v.Normal * l;
                    double x = v.Pos.x;
                    double y = v.Pos.y;
                    double z = v.Pos.z;
                    GL.Vertex3(x, y, z);
                    GL.Vertex3(x + normal.x, y + normal.y, z + normal.z);
                    
                }
            }
            GL.End();
        }
    }
}
