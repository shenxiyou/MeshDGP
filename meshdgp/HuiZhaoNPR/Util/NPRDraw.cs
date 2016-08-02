using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing; 
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public static class NPRDraw
    { 

        private static void DrawBaseMesh(TriMesh mesh)
        {
            GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.PolygonOffset(1f, 1f);
            GL.Enable(EnableCap.PolygonOffsetFill);
            GL.Disable(EnableCap.ColorMaterial);

            switch (ConfigNPR.Instance.CurvatureColor)
            {
                case ConfigNPR.MeshStyle.None:
                    GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Emission, GlobalSetting.DisplaySetting.BackGroundColor);
                    GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, Color.Black);
                    DrawBasic(mesh);
                    OpenGLManager.Instance.SetMaterial();
                    break;
                case ConfigNPR.MeshStyle.Basic:
                    OpenGLManager.Instance.SetMaterial();
                    DrawBasic(mesh);
                    break;
                case ConfigNPR.MeshStyle.CurvColor:
                case ConfigNPR.MeshStyle.CurvGray:
                    DrawCurvColor(mesh);
                    break;
                default:
                    break;
            }
            GL.Enable(EnableCap.ColorMaterial);
        }

        private static void DrawBasic(TriMesh mesh)
        {
            GL.Disable(EnableCap.Blend);
            
            GL.Begin(BeginMode.Triangles);
            foreach (var f in mesh.Faces)
            {
                foreach (var v in f.Vertices)
                {
                    GL.Normal3(v.Traits.Normal.ToArray());
                    GL.Vertex3(v.Traits.Position.ToArray());
                }
            }
            GL.End();
            
            GL.Enable(EnableCap.Blend);
        }

        private static void DrawCurvColor(TriMesh mesh)
        {
            GL.Enable(EnableCap.ColorMaterial);
            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.Diffuse);
            GL.Begin(BeginMode.Triangles);
            foreach (var f in mesh.Faces)
            {
                foreach (var v in f.Vertices)
                {
                    switch (ConfigNPR.Instance.CurvatureColor)
                    {
                        case ConfigNPR.MeshStyle.CurvColor:
                           // GL.Color3(rtsc.curv_colors[v.Index].ToArray());
                            break;
                        case ConfigNPR.MeshStyle.CurvGray:
                          //  GL.Color3(rtsc.gcurv_colors[v.Index].ToArray());
                            break;
                        default:
                            break;
                    }
                    //GL.Normal3(v.Traits.Normal.ToArray());
                    GL.Vertex3(v.Traits.Position.ToArray());
                }
            }
            GL.End();
        }

        private static void DrawLine(LineBase line)
        {
            if (ConfigNPR.Instance.RTSC)
            {
                LineGLInfo info = RTSC.Instance.GetLine(line.Type);

                DrawLineInfo(line.Color, info);
            }
        }

        public static void DrawLineInfo(Color c, LineGLInfo info)
        { 

            GL.Enable(EnableCap.ColorMaterial);
            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.Diffuse);  
          
            float[] color = new float[4] { c.R / 256f, c.G / 256f, c.B / 256f, 1f };

            if (ConfigNPR.Instance.DrawHiddenLine)
            {
                GL.LineWidth((float)ConfigNPR.Instance.HiddenLineWidth);
                GL.Begin(BeginMode.Lines);
                for (int i = 0; i < info.Front; i++)
                {
                    color[3] = info.Alpha[i];
                    GL.Color4(color);
                    GL.Vertex3(info.Vertex[i].ToArray());
                }
                GL.End();
            }

            GL.LineWidth((float)ConfigNPR.Instance.LineWidth);
            GL.Begin(BeginMode.Lines);
            for (int i = info.Front; i < info.Vertex.Length; i++)
            {
                color[3] = i < info.Alpha.Length ? info.Alpha[i] : 1f;
                GL.Color4(color);
                GL.Vertex3(info.Vertex[i].ToArray());
            }
            GL.End();
            GL.Disable(EnableCap.ColorMaterial);
        }

        public static void DrawAllLines(object sender, TriMeshDrawEventArgs e)
        {
           
            GL.Enable(EnableCap.PointSmooth);
            GL.Enable(EnableCap.LineSmooth);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
            GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
            GL.Enable(EnableCap.ColorMaterial);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            for (int i = 0; i < ConfigNPR.Instance.Enable.Length; i++)
            {
                if (ConfigNPR.Instance.Enable[i])
                {
                    if (NPRLines.All[i] is Silluhoute)
                    {
                        GL.Disable(EnableCap.DepthTest);
                        DrawLine(NPRLines.All[i]);
                        GL.Enable(EnableCap.DepthTest);
                    }
                }
            }

            DrawBaseMesh(e.Mesh);

            if (ConfigNPR.Instance.DrawHiddenLine)
            {
                GL.Disable(EnableCap.DepthTest);
            }

            for (int i = 0; i < ConfigNPR.Instance.Enable.Length; i++)
            {
                if (ConfigNPR.Instance.Enable[i])
                {
                    if (!(NPRLines.All[i] is Silluhoute))
                    {
                        DrawLine(NPRLines.All[i]);
                    }
                }
            }
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.ColorMaterial);
        }
    }
}
