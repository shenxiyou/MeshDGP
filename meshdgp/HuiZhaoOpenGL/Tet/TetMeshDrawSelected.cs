using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace GraphicResearchHuiZhao 
{
    public partial class OpenGLTetMesh
    {
        public void DrawSelectedVertices(TetMesh mesh)
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Point);
            GL.PointSize(GlobalSetting.DisplaySetting.PointSize);
            GL.Enable(EnableCap.PointSmooth);
            GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
            OpenGLManager.Instance.SetColorMesh(GlobalSetting.DisplaySetting.SelectedVertexColor);
            GL.Begin(BeginMode.Points);
            foreach (var v in mesh.Vertices)
            {
                if (v.Flag != 0)
                {
                    Color4 color = v.Color;
                    if (color != Color4.Black)
                    {
                        OpenTK.Graphics.Color4 colorTwo = new OpenTK.Graphics.Color4((float)color.R, (float)color.G, (float)color.B, 0.0f);
                        OpenGLManager.Instance.SetColorMesh(colorTwo);
                    }
                    GL.Normal3(v.Normal.ToArray());
                    GL.Vertex3(v.Pos.ToArray());
                }
            }
            GL.End();
        }

        private void DrawSelectedEdges(TetMesh mesh)
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.LineWidth(GlobalSetting.DisplaySetting.SelectionLineWidth);
            GL.Enable(EnableCap.LineSmooth);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
            GL.Begin(BeginMode.Lines);
            foreach (var edge in mesh.Edges)
            {
                if (edge.Flag != 0)
                {
                    Color4 color = edge.Color;
                    if (color != Color4.Black)
                    {
                        OpenTK.Graphics.Color4 colorTwo = new OpenTK.Graphics.Color4((float)color.R, (float)color.G, (float)color.B, 0.0f);
                        OpenGLManager.Instance.SetColorMesh(colorTwo);
                    }
                    else
                    {
                        OpenGLManager.Instance.SetColorMesh(GlobalSetting.DisplaySetting.SelectedEdgeColor);
                    }
                    foreach (var v in edge.Vertices)
                    {
                        GL.Normal3(v.Normal.ToArray());
                        GL.Vertex3(v.Pos.ToArray());
                    }
                }
            }
            GL.End();
        }

        public void DrawSelectedFaces(TetMesh mesh)
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.Normalize);
            GL.Begin(BeginMode.Triangles);
            foreach (var face in mesh.Faces)
            {
                if (face.Flag != 0)
                {
                    Color4 color = face.Color;
                    if (color != Color4.Black)
                    {
                        OpenTK.Graphics.Color4 colorTwo = new OpenTK.Graphics.Color4((float)color.R, (float)color.G, (float)color.B, 0.0f);
                        OpenGLManager.Instance.SetColorMesh(colorTwo);
                    }
                    else
                    {
                        OpenGLManager.Instance.SetColorMesh(GlobalSetting.DisplaySetting.SelectedFaceColor);
                    }
                    foreach (var v in face.Vertices)
                    {
                        GL.Normal3(v.Normal.ToArray());
                        GL.Vertex3(v.Pos.ToArray());
                    }
                }
            }
            GL.End();
        }

        public void DrawSelectedTetrahedron(TetMesh mesh)
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.LineWidth(GlobalSetting.DisplaySetting.LineWidth);
            OpenGLManager.Instance.SetColorMesh(GlobalSetting.DisplaySetting.WifeFrameColor);
            GL.Begin(BeginMode.Triangles);
            foreach (var tet in mesh.Tetras)
            {
                if (tet.Flag != 0)
                {
                    foreach (var face in tet.Faces)
                    {
                        if (TetMeshFlag.CheckBoundary(face) != null)
                        {
                            foreach (var v in face.Vertices)
                            {
                                GL.Normal3(v.SelectedNormal.ToArray());
                                GL.Vertex3(v.Pos.ToArray());
                            }
                        }
                    }
                }
            }
            GL.End();

            GL.LightModel(LightModelParameter.LightModelTwoSide, 1f);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.Normalize);
            //Color c = GlobalSetting.MeshDisplaySetting.TetrahedronColor;
            //OpenGLManager.Instance.SetMeshColor(c);
            GL.Disable(EnableCap.ColorMaterial);
            OpenGLManager.Instance.SetMaterialInfo();

            GL.Begin(BeginMode.Triangles);
            foreach (var tet in mesh.Tetras)
            {
                if (tet.Flag != 0)
                {
                    foreach (var face in tet.Faces)
                    {
                        if (TetMeshFlag.CheckBoundary(face) != null)
                        {
                            foreach (var v in face.Vertices)
                            {
                                GL.Normal3(v.SelectedNormal.ToArray());
                                GL.Vertex3(v.Pos.ToArray());
                            }
                        }
                    }
                }
            }
            GL.End();
            GL.Enable(EnableCap.ColorMaterial);
        }

        public void DrawSelectedFaceNormal(TetMesh mesh)
        {
            OpenGLManager.Instance.SetColorMesh(GlobalSetting.DisplaySetting.NormalColor);
            double l = mesh.AvgLength * GlobalSetting.DisplaySetting.NormalLength;
            GL.Begin(BeginMode.Lines);
            foreach (var face in mesh.Faces)
            {
                if (face.SelectedNormal != Vector3D.Zero)
                {
                    Vector3D mid = TetMeshUtil.GetMidPoint(face);
                    Vector3D normal = face.SelectedNormal * l;
                    GL.Vertex3(mid.ToArray());
                    GL.Vertex3((mid + normal).ToArray());
                }
            }
            GL.End();
        }

        public void DrawSelectedVertexNormal(TetMesh mesh)
        {
            OpenGLManager.Instance.SetColorMesh(GlobalSetting.DisplaySetting.NormalColor);
            double l = mesh.AvgLength * GlobalSetting.DisplaySetting.NormalLength;
            GL.Begin(BeginMode.Lines);
            foreach (var v in mesh.Vertices)
            {
                if (v.SelectedNormal != Vector3D.Zero)
                {
                    Vector3D normal = v.SelectedNormal * l;
                    GL.Vertex3(v.Pos.ToArray());
                    GL.Vertex3((v.Pos + normal).ToArray());
                }
            }
            GL.End();
        }

        //public void DrawSelectedVerticeBySphere(TetMesh m)
        //{

        //    for (int i = 0; i < m.Vertices.Count; i++)
        //    {
        //        if (m.Vertices[i].Traits.SelectedFlag == 0)
        //            continue;
        //        OpenTK.Graphics.Color4 color = new OpenTK.Graphics.Color4(0, 0, 0, 0);
        //        switch (m.Vertices[i].Traits.SelectedFlag % 6)
        //        {
        //            case 0: color = new OpenTK.Graphics.Color4(0.0f, 0.0f, 1.0f, 0.0f); break;
        //            case 1: color = new OpenTK.Graphics.Color4(1.0f, 0.0f, 0.0f, 0.0f); break;
        //            case 2: color = new OpenTK.Graphics.Color4(0.0f, 1.0f, 0.0f, 0.0f); break;
        //            case 3: color = new OpenTK.Graphics.Color4(1.0f, 1.0f, 0.0f, 0.0f); break;
        //            case 4: color = new OpenTK.Graphics.Color4(0.0f, 1.0f, 1.0f, 0.0f); break;
        //            case 5: color = new OpenTK.Graphics.Color4(1.0f, 0.0f, 1.0f, 0.0f); break;

        //        }

        //        OpenGLManager.Instance.SetMeshColor( color);

        //        GL.PushMatrix();
        //        GL.Translate(m.Vertices[i].Traits.Position.x, m.Vertices[i].Traits.Position.y, m.Vertices[i].Traits.Position.z);
        //        GL.Scale(0.01, 0.01, 0.01);
        //        OpenGLManager.Instance.DrawSphere();
        //        GL.PopMatrix();
        //    }
        //}


    }
}
