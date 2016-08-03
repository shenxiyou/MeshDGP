using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public partial class OpenGLTetMesh
    {
        public void DrawSmoothShaded(TetMesh mesh)
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.Normalize);
            Color c = GlobalSetting.DisplaySetting.MeshColor;
            OpenGLManager.Instance.SetColorMesh(c);
            DrawTriangles(mesh);
        }

        public void DrawWireFrame(TetMesh mesh)
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.LineWidth(GlobalSetting.DisplaySetting.LineWidth);
            //GL.Enable(EnableCap.CullFace);
            OpenGLManager.Instance.SetColorMesh(GlobalSetting.DisplaySetting.WifeFrameColor);
            DrawTriangles(mesh);
        }

        public void DrawVertices(TetMesh mesh)
        {
            GL.Begin(BeginMode.Points);
            foreach (var v in mesh.Vertices)
            {
                if ((GlobalTetSetting.TetDisplayFlag & TetDisplayFlag.HasInner) == TetDisplayFlag.HasInner || v.OnBoundary)
                {
                    GL.Normal3(v.Normal.ToArray());
                    GL.Vertex3(v.Pos.ToArray());
                }
            }
            GL.End();
        }

        public void DrawTriangles(TetMesh mesh)
        {
            GL.Begin(BeginMode.Triangles);
            foreach (var face in mesh.Faces)
            {
                if ((GlobalTetSetting.TetDisplayFlag & TetDisplayFlag.HasInner) == TetDisplayFlag.HasInner || face.OnBoundary)
                {
                    foreach (var v in face.Vertices)
                    {
                        GL.Normal3(v.Normal.ToArray());
                        GL.Vertex3(v.Pos.ToArray());
                    }
                }
            }
            GL.End();
        }

        public void DrawPoints(TetMesh m)
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.CullFace);

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Point);
            GL.PointSize(GlobalSetting.DisplaySetting.PointSize);
            GL.Enable(EnableCap.PointSmooth);
            GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
            OpenGLManager.Instance.SetColorMesh(GlobalSetting.DisplaySetting.MeshColor);
            DrawVertices(m);
        }
    }
}
