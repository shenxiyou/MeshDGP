using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao 
{
    public partial class OpenGLNonManifold
    {
        
        private static OpenGLNonManifold instance = null;
        public static OpenGLNonManifold Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OpenGLNonManifold();
                }
                return instance;
            }
        }

        private OpenGLNonManifold()
        {

        }

        public void DrawCurves(NonManifoldMesh m, List<List<int>> lines)
        {
            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.PointSize(GlobalSetting.DisplaySetting.PointSize);
            GL.LineWidth(GlobalSetting.DisplaySetting.SelectionLineWidth);
            GL.EnableClientState(ArrayCap.VertexArray);

            GL.VertexPointer<double>(3, VertexPointerType.Double, 0, m.VertexPos);

            for (int i = 0; i < lines.Count; i++)
            {
                GL.Begin(BeginMode.LineStrip);
                switch (i % 6)
                {
                    case 0: GL.Color3(0.0f, 0.0f, 1.0f); break;
                    case 1: GL.Color3(1.0f, 0.0f, 0.0f); break;
                    case 2: GL.Color3(0.0f, 1.0f, 0.0f); break;
                    case 3: GL.Color3(1.0f, 1.0f, 0.0f); break;
                    case 4: GL.Color3(0.0f, 1.0f, 1.0f); break;
                    case 5: GL.Color3(1.0f, 0.0f, 1.0f); break;
                }
                for (int j = 0; j < lines[i].Count; j++)
                {
                    GL.ArrayElement(lines[i][j]);
                }
                GL.End();

            }


            GL.DisableClientState(ArrayCap.VertexArray);
        }

        public void DrawStrokes(NonManifoldMesh m, List<List<int>> lines)
        {
            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.PointSize(GlobalSetting.DisplaySetting.PointSize);
            GL.LineWidth(GlobalSetting.DisplaySetting.SelectionLineWidth);
            GL.EnableClientState(ArrayCap.VertexArray);

            GL.VertexPointer<double>(3, VertexPointerType.Double, 0, m.VertexPos);

            for (int i = 0; i < lines.Count; i++)
            {
                switch (i % 6)
                {
                    case 0: GL.Color3(0.0f, 0.0f, 1.0f); break;
                    case 1: GL.Color3(1.0f, 0.0f, 0.0f); break;
                    case 2: GL.Color3(0.0f, 1.0f, 0.0f); break;
                    case 3: GL.Color3(1.0f, 1.0f, 0.0f); break;
                    case 4: GL.Color3(0.0f, 1.0f, 1.0f); break;
                    case 5: GL.Color3(1.0f, 0.0f, 1.0f); break;
                }
                GL.Begin(BeginMode.Lines);
                if (lines[i].Count == 2)
                {
                    GL.ArrayElement(lines[i][0]);
                    GL.ArrayElement(lines[i][1]);
                }
                GL.End();

            }


            GL.DisableClientState(ArrayCap.VertexArray);
        }






        public void DrawFlatShaded(NonManifoldMesh m)
        {
            GL.ShadeModel(ShadingModel.Flat);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            //GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Normalize);


            Color c = GlobalSetting.DisplaySetting.MeshColor;
            GL.Color3(c.R, c.G, c.B);
            GL.EnableClientState(ArrayCap.VertexArray);

            GL.VertexPointer<double>(3, VertexPointerType.Double, 0, m.VertexPos);
            GL.Begin(BeginMode.Triangles);
            for (int i = 0, j = 0; i < m.FaceCount; i++, j += 3)
            {
                GL.Normal3(ref m.FaceNormal[j]);
                GL.ArrayElement(m.FaceIndex[j]);
                GL.ArrayElement(m.FaceIndex[j + 1]);
                GL.ArrayElement(m.FaceIndex[j + 2]);
            }
            GL.End();

            GL.DisableClientState(ArrayCap.VertexArray);
            //GL.Disable(EnableCap.Lighting);
        }
        public void DrawWireframe(NonManifoldMesh m)
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Disable(EnableCap.CullFace);
            GL.Color3(1.0f, 1.0f, 1.0f);



            GL.LineWidth(GlobalSetting.DisplaySetting.LineWidth);
            GL.EnableClientState(ArrayCap.VertexArray);

            GL.VertexPointer<double>(3, VertexPointerType.Double, 0, m.VertexPos);

            GL.DrawElements(BeginMode.Triangles, m.FaceCount * 3, DrawElementsType.UnsignedInt, m.FaceIndex);

            GL.DisableClientState(ArrayCap.VertexArray);
            GL.Enable(EnableCap.CullFace);


        }
        public void DrawDarkWireframe(NonManifoldMesh m)
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Disable(EnableCap.CullFace);
            GL.Color3(0.2f, 0.2f, 0.2f);



            GL.LineWidth(GlobalSetting.DisplaySetting.LineWidth);
            GL.EnableClientState(ArrayCap.VertexArray);

            GL.VertexPointer<double>(3, VertexPointerType.Double, 0, m.VertexPos);
            GL.DrawElements(BeginMode.Triangles, m.FaceCount * 3, DrawElementsType.UnsignedInt, m.FaceIndex);

            GL.DisableClientState(ArrayCap.VertexArray);
            GL.Enable(EnableCap.CullFace);
        }
        public void DrawSmoothHiddenLine(NonManifoldMesh m)
        {
            GL.Enable(EnableCap.PolygonOffsetFill);
            DrawSmoothShaded(m);
            DrawDarkWireframe(m);
            GL.Disable(EnableCap.PolygonOffsetFill);
        }
        public void DrawFlatHiddenLine(NonManifoldMesh m)
        {
            GL.Enable(EnableCap.PolygonOffsetFill);
            DrawFlatShaded(m);
            DrawDarkWireframe(m);
            GL.Disable(EnableCap.PolygonOffsetFill);
        }

        public void DrawSelectedFaceByRectangle(NonManifoldMesh m)
        {
            GL.Enable(EnableCap.PolygonOffsetFill);
            DrawSmoothShaded(m);
            //GL.Disable(EnableCap.PolygonOffsetFill);

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Disable(EnableCap.CullFace);



            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.LineWidth(GlobalSetting.DisplaySetting.LineWidth);
            GL.EnableClientState(ArrayCap.VertexArray);




            GL.VertexPointer<double>(3, VertexPointerType.Double, 0, m.VertexPos);
            GL.Begin(BeginMode.Triangles);

            int n = m.FaceCount;
            for (int i = 0; i < n; i++)
            {
                if (m.FaceFlag[i] == 0) continue;

                GL.ArrayElement(m.FaceIndex[i * 3]);
                GL.ArrayElement(m.FaceIndex[i * 3 + 1]);
                GL.ArrayElement(m.FaceIndex[i * 3 + 2]);
            }
            GL.End();

            GL.DisableClientState(ArrayCap.VertexArray);
            GL.Enable(EnableCap.CullFace);


        }


        public void DrawSelectedFace(NonManifoldMesh m, int face)
        {
            //GL.Enable(EnableCap.PolygonOffsetFill);
            //DrawSmoothShaded(m);

            GL.Color3(1.0f, 1.0f, 0.0f);
            GL.LineWidth(GlobalSetting.DisplaySetting.LineWidth);
            GL.EnableClientState(ArrayCap.VertexArray);




            GL.VertexPointer<double>(3, VertexPointerType.Double, 0, m.VertexPos);
            GL.Begin(BeginMode.Triangles);

            GL.ArrayElement(m.FaceIndex[face * 3]);
            GL.ArrayElement(m.FaceIndex[face * 3 + 1]);
            GL.ArrayElement(m.FaceIndex[face * 3 + 2]);

            GL.End();

            GL.DisableClientState(ArrayCap.VertexArray);
            GL.Enable(EnableCap.CullFace);
            //GL.Disable(EnableCap.PolygonOffsetFill);
        }


        public void DrawSelectedVerticeBySphere(NonManifoldMesh m)
        {



            //SlicedSphere sphere = new SlicedSphere(0.005f, OpenTK.Vector3d.Zero,
            //                               SlicedSphere.eSubdivisions.Three,
            //                               new SlicedSphere.eDir[] { SlicedSphere.eDir.All },
            //                               true);

            for (int i = 0; i < m.VertexCount; i++)
            {
                if (m.VertexFlag[i] == 0) continue;
                switch (m.VertexFlag[i] % 6)
                {
                    case 0: GL.Color3(0.0f, 0.0f, 1.0f); break;
                    case 1: GL.Color3(1.0f, 0.0f, 0.0f); break;
                    case 2: GL.Color3(0.0f, 1.0f, 0.0f); break;
                    case 3: GL.Color3(1.0f, 1.0f, 0.0f); break;
                    case 4: GL.Color3(0.0f, 1.0f, 1.0f); break;
                    case 5: GL.Color3(1.0f, 0.0f, 1.0f); break;
                }
                GL.PushMatrix();
                GL.Translate(m.VertexPos[i * 3], m.VertexPos[i * 3 + 1], m.VertexPos[i * 3 + 2]);
                GL.Scale(0.005, 0.005, 0.005);
                OpenGLManager.Instance.DrawSphere();
                GL.PopMatrix();
            }
        }

        public void DrawSelectedVerticeByPoint(NonManifoldMesh m)
        {
            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.PointSize(GlobalSetting.DisplaySetting.PointSize);
            GL.EnableClientState(ArrayCap.VertexArray);

            GL.VertexPointer<double>(3, VertexPointerType.Double, 0, m.VertexPos);
            GL.Begin(BeginMode.Points);
            int n = m.VertexCount;
            for (int i = 0; i < n; i++)
            {
                if (m.VertexFlag[i] == 0) continue;
                switch (m.VertexFlag[i] % 6)
                {
                    case 0: GL.Color3(0.0f, 0.0f, 1.0f); break;
                    case 1: GL.Color3(1.0f, 0.0f, 0.0f); break;
                    case 2: GL.Color3(0.0f, 1.0f, 0.0f); break;
                    case 3: GL.Color3(1.0f, 1.0f, 0.0f); break;
                    case 4: GL.Color3(0.0f, 1.0f, 1.0f); break;
                    case 5: GL.Color3(1.0f, 0.0f, 1.0f); break;
                }

                GL.ArrayElement(i);
            }
            GL.End();

            GL.DisableClientState(ArrayCap.VertexArray);



        }


        public void DrawPoints(NonManifoldMesh m)
        {

            GL.PointSize(GlobalSetting.DisplaySetting.PointSize);
            Color c = GlobalSetting.DisplaySetting.MeshColor;
            GL.Color3(c.R, c.G, c.B);
            GL.EnableClientState(ArrayCap.VertexArray);

            GL.VertexPointer<double>(3, VertexPointerType.Double, 0, m.VertexPos);
            GL.DrawArrays(BeginMode.Points, 0, m.VertexCount);

            GL.DisableClientState(ArrayCap.VertexArray);
        }

        public void DrawTransparentShaded(NonManifoldMesh m)
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            GL.Enable(EnableCap.Normalize);

            GL.Disable(EnableCap.CullFace);
            // GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);


            Color c = GlobalSetting.DisplaySetting.MeshColor;

            GL.Color4(c.R, c.G, c.B, (byte)250);



            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);

            GL.VertexPointer<double>(3, VertexPointerType.Double, 0, m.VertexPos);
            GL.NormalPointer<double>(NormalPointerType.Double, 0, m.VertexNormal);
            GL.DrawElements(BeginMode.Triangles, m.FaceCount * 3, DrawElementsType.UnsignedInt, m.FaceIndex);

            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.NormalArray);
            //GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);



        }

        public void DrawSilhouetteShaded(NonManifoldMesh m)
        {


            GL.LineWidth(5.0f);
            GL.PolygonMode(MaterialFace.Back, PolygonMode.Line);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.CullFace(CullFaceMode.Front);

            Color c = GlobalSetting.DisplaySetting.MeshColor;

            GL.Color4(c.R, c.G, c.B, (byte)250);

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);

            GL.VertexPointer<double>(3, VertexPointerType.Double, 0, m.VertexPos);
            GL.NormalPointer<double>(NormalPointerType.Double, 0, m.VertexNormal);

            GL.DrawElements(BeginMode.Triangles, m.FaceCount * 3, DrawElementsType.UnsignedInt, m.FaceIndex);

            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.NormalArray);
            //GL.Disable(EnableCap.Lighting);

        }
        public void DrawSmoothShaded(NonManifoldMesh m)
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            //GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Normalize);
            GL.Disable(EnableCap.Texture2D);

            Color c = GlobalSetting.DisplaySetting.MeshColor;

            GL.Color4(c.R, c.G, c.B, (byte)250);

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);

            GL.VertexPointer<double>(3, VertexPointerType.Double, 0, m.VertexPos);
            GL.NormalPointer<double>(NormalPointerType.Double, 0, m.VertexNormal);

            GL.DrawElements(BeginMode.Triangles, m.FaceCount * 3, DrawElementsType.UnsignedInt, m.FaceIndex);

            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.NormalArray);
            //GL.Disable(EnableCap.Lighting);

        }

        public void DrawTangentShaded(NonManifoldMesh m)
        {
            //if (OpenGLState.Instance.Tangentindex == -1)
            //    return;

            //GL.ShadeModel(ShadingModel.Smooth);
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            ////GL.Enable(EnableCap.Lighting);
            //GL.Enable(EnableCap.Normalize);

            //Color c = GlobalSetting.MeshDisplaySetting.MeshColor;
            //GL.Color3(c.R, c.G, c.B);

            //GL.EnableClientState(ArrayCap.VertexArray);
            //GL.EnableClientState(ArrayCap.NormalArray);
            //GL.EnableVertexAttribArray(OpenGLState.Instance.Tangentindex);		

            //GL.VertexPointer<double>(3, VertexPointerType.Double, 0, m.VertexPos);
            //GL.NormalPointer<double>(NormalPointerType.Double, 0, m.VertexNormal);
            //GL.VertexAttribPointer<double>(OpenGLState.Instance.Tangentindex, 1, VertexAttribPointerType.Double, false,1, m.VertexNormal);


            //GL.DrawElements(BeginMode.Triangles, m.FaceCount * 3, DrawElementsType.UnsignedInt, m.FaceIndex);

            //GL.DisableClientState(ArrayCap.VertexArray);
            //GL.DisableClientState(ArrayCap.NormalArray);
            //GL.Disable(EnableCap.Lighting);

        }




        public void DrawBoundaryVertice(NonManifoldMesh m)
        {


            GL.Color3(1.0f, 0.54f, 0.0f);
            GL.PointSize(GlobalSetting.DisplaySetting.PointSize);
            GL.EnableClientState(ArrayCap.VertexArray);

            GL.VertexPointer<double>(3, VertexPointerType.Double, 0, m.VertexPos);
            GL.Begin(BeginMode.Points);
            for (int i = 0; i < m.VertexCount; i++)
            {
                if (m.IsBoundary[i])
                    GL.ArrayElement(i);
            }
            GL.End();

            GL.DisableClientState(ArrayCap.VertexArray);
        }
        public void DrawVertexColorShaded(NonManifoldMesh m)
        {


            double max = double.MinValue;
            double min = double.MaxValue;
            for (int i = 0; i < m.VertexCount; i++)
            {
                if (max < m.Color[i])
                {
                    max = m.Color[i];
                }
                if (min > m.Color[i])
                {
                    min = m.Color[i];
                }
            }

            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.Normalize);

            GL.EnableClientState(ArrayCap.VertexArray);

            GL.VertexPointer(3, VertexPointerType.Double, 0, m.VertexPos);
            GL.Begin(BeginMode.Triangles);

            int n = m.FaceCount;
            for (int i = 0, j = 0; i < m.FaceCount; i++, j += 3)
            {
                GL.Normal3(ref m.FaceNormal[j]);

                GL.Color3(m.Color[m.FaceIndex[j]] / max, 1 - m.Color[m.FaceIndex[j]] / max, 0);
                GL.ArrayElement(m.FaceIndex[j]);
                GL.Color3(m.Color[m.FaceIndex[j + 1]] / max, 1 - m.Color[m.FaceIndex[j + 1]] / max, 0);
                GL.ArrayElement(m.FaceIndex[j + 1]);
                GL.Color3(m.Color[m.FaceIndex[j + 2]] / max, 1 - m.Color[m.FaceIndex[j + 2]] / max, 0);
                GL.ArrayElement(m.FaceIndex[j + 2]);
            }
            GL.End();



            GL.DisableClientState(ArrayCap.VertexArray);

        }


        public void DrawPatch(ref NonManifoldMesh m)
        {
            int N = 12;


            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.Normalize);

            GL.EnableClientState(ArrayCap.VertexArray);

            GL.VertexPointer(3, VertexPointerType.Double, 0, m.VertexPos);
            GL.Begin(BeginMode.Triangles);

            int n = m.FaceCount;
            for (int i = 0, j = 0; i < m.FaceCount; i++, j += 3)
            {
                GL.Normal3(ref m.FaceNormal[j]);


                if (m.VertexFlag[m.FaceIndex[j]] == 1)
                {
                    GL.Color3(Color.Yellow);
                }
                else if (m.VertexFlag[m.FaceIndex[j]] == 2)
                {
                    GL.Color3(Color.CornflowerBlue);
                }
                else
                {
                    GL.Color3(Color.Crimson);
                }
                GL.ArrayElement(m.FaceIndex[j]);

                GL.ArrayElement(m.FaceIndex[j + 1]);

                GL.ArrayElement(m.FaceIndex[j + 2]);
            }
            GL.End();


            //GL.Begin(BeginMode.Points);


            n = m.VertexCount;
            for (int i = 0; i < n; i++)
            {

                double value = (double)(N / 2) / (double)(N + 1);
                if (Math.Abs(m.Color[i] - value) < 0.01)
                {
                    GL.Color3(1.0, 0.0, 0.0);
                    GL.PushMatrix();
                    GL.Translate(m.VertexPos[i * 3], m.VertexPos[i * 3 + 1], m.VertexPos[i * 3 + 2]);
                    GL.Scale(0.002, 0.002, 0.002);
                    OpenGLManager.Instance.DrawSphere();
                    GL.PopMatrix();
                    //GL.ArrayElement(i);
                }

            }
            //GL.End();

            GL.DisableClientState(ArrayCap.VertexArray);
        }

        public void DrawIsoPart(ref NonManifoldMesh m)
        {
            int N = 12;


            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.Normalize);

            GL.EnableClientState(ArrayCap.VertexArray);

            GL.VertexPointer(3, VertexPointerType.Double, 0, m.VertexPos);
            GL.Begin(BeginMode.Triangles);

            int n = m.FaceCount;
            for (int i = 0, j = 0; i < m.FaceCount; i++, j += 3)
            {
                GL.Normal3(ref m.FaceNormal[j]);

                if ((m.Color[m.FaceIndex[j]] + m.Color[m.FaceIndex[j]] + m.Color[m.FaceIndex[j]]) / 3 < (double)(N / 2) / (double)(N + 1))
                {
                    GL.Color3(Color.Yellow);
                }
                else
                {
                    GL.Color3(Color.CornflowerBlue);
                }
                GL.ArrayElement(m.FaceIndex[j]);

                GL.ArrayElement(m.FaceIndex[j + 1]);

                GL.ArrayElement(m.FaceIndex[j + 2]);
            }
            GL.End();


            //GL.Begin(BeginMode.Points);


            n = m.VertexCount;
            for (int i = 0; i < n; i++)
            {

                double value = (double)(N / 2) / (double)(N + 1);
                if (Math.Abs(m.Color[i] - value) < 0.01)
                {
                    GL.Color3(1.0, 0.0, 0.0);
                    GL.PushMatrix();
                    GL.Translate(m.VertexPos[i * 3], m.VertexPos[i * 3 + 1], m.VertexPos[i * 3 + 2]);
                    GL.Scale(0.002, 0.002, 0.002);
                    OpenGLManager.Instance.DrawSphere();
                    GL.PopMatrix();
                    //GL.ArrayElement(i);
                }

            }
            //GL.End();

            GL.DisableClientState(ArrayCap.VertexArray);
        }

        public void DrawIsoLine(ref NonManifoldMesh m)
        {
            double max = double.MinValue;
            double min = double.MaxValue;
            for (int i = 0; i < m.VertexCount; i++)
            {
                if (max < m.Color[i])
                {
                    max = m.Color[i];
                }
                if (min > m.Color[i])
                {
                    min = m.Color[i];
                }
            }

            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.Normalize);

            GL.EnableClientState(ArrayCap.VertexArray);

            GL.VertexPointer(3, VertexPointerType.Double, 0, m.VertexPos);
            GL.Begin(BeginMode.Triangles);

            int n = m.FaceCount;
            for (int i = 0, j = 0; i < m.FaceCount; i++, j += 3)
            {
                GL.Normal3(ref m.FaceNormal[j]);

                GL.Color3(m.Color[m.FaceIndex[j]] / max, 1 - m.Color[m.FaceIndex[j]] / max, 0);
                GL.ArrayElement(m.FaceIndex[j]);
                GL.Color3(m.Color[m.FaceIndex[j + 1]] / max, 1 - m.Color[m.FaceIndex[j + 1]] / max, 0);
                GL.ArrayElement(m.FaceIndex[j + 1]);
                GL.Color3(m.Color[m.FaceIndex[j + 2]] / max, 1 - m.Color[m.FaceIndex[j + 2]] / max, 0);
                GL.ArrayElement(m.FaceIndex[j + 2]);
            }
            GL.End();


            //GL.Begin(BeginMode.Points);


            n = m.VertexCount;
            for (int i = 0; i < n; i++)
            {
                int N = 10;
                for (int k = 1; k < N + 1; k++)
                {
                    double value = (double)k / (double)(N + 1);
                    if (Math.Abs(m.Color[i] - value) < 0.01)
                    {
                        GL.Color3(1.0, 0.0, 0.0);
                        GL.PushMatrix();
                        GL.Translate(m.VertexPos[i * 3], m.VertexPos[i * 3 + 1], m.VertexPos[i * 3 + 2]);
                        GL.Scale(0.002, 0.002, 0.002);
                        OpenGLManager.Instance.DrawSphere();
                        GL.PopMatrix();
                        //GL.ArrayElement(i);
                    }
                }
            }
            //GL.End();

            GL.DisableClientState(ArrayCap.VertexArray);
        }


        public void DrawMeanCurvatureShaded(NonManifoldMesh m)
        {

            DrawGaussianCurvatureShaded(m);

        }

        public void DrawGaussianCurvatureShaded(NonManifoldMesh mesh)
        {
            double max = double.MinValue;
            double min = double.MaxValue;
            int n = mesh.VertexCount;
            for (int i = 0; i < n; i++)
            {
                if (max < mesh.Color[i])
                {
                    max = mesh.Color[i];
                }
                if (min > mesh.Color[i])
                {
                    min = mesh.Color[i];
                }
            }

            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.Normalize);

            GL.EnableClientState(ArrayCap.VertexArray);

            GL.VertexPointer(3, VertexPointerType.Double, 0, mesh.VertexPos);
            GL.Begin(BeginMode.Triangles);

            int m = mesh.FaceCount;
            for (int i = 0, j = 0; i < m; i++, j += 3)
            {
                GL.Normal3(ref mesh.FaceNormal[j]);

                GL.Color3(mesh.Color[mesh.FaceIndex[j]] / max, 1, 0);
                GL.ArrayElement(mesh.FaceIndex[j]);
                GL.Color3(mesh.Color[mesh.FaceIndex[j + 1]] / max, 1, 0);
                GL.ArrayElement(mesh.FaceIndex[j + 1]);
                GL.Color3(mesh.Color[mesh.FaceIndex[j + 2]] / max, 1, 0);
                GL.ArrayElement(mesh.FaceIndex[j + 2]);
            }
            GL.End();


            GL.DisableClientState(ArrayCap.VertexArray);

        }



        public void DrawFaceColorFlatShaded(NonManifoldMesh m)
        {
            GL.ShadeModel(ShadingModel.Flat);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            //GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Normalize);



            GL.EnableClientState(ArrayCap.VertexArray);

            GL.VertexPointer(3, VertexPointerType.Double, 0, m.VertexPos);
            GL.Begin(BeginMode.Triangles);
            for (int i = 0, j = 0; i < m.FaceCount; i++, j += 3)
            {
                GL.Normal3(ref m.FaceNormal[j]);
                GL.Color3(ref m.Color[j]);
                GL.ArrayElement(m.FaceIndex[j]);
                GL.ArrayElement(m.FaceIndex[j + 1]);
                GL.ArrayElement(m.FaceIndex[j + 2]);
            }
            GL.End();

            GL.DisableClientState(ArrayCap.VertexArray);
            //GL.Disable(EnableCap.Lighting);
        }


        public void DrawLapCoord(NonManifoldMesh mesh)
        {
            GL.LineWidth(2.0f);
            GL.Color3(1.0, 0.0, 0.0);
            GL.Begin(BeginMode.Lines);
            double[] lapCoordiante = MeshOperators.ComputeDualLap(ref mesh);
            for (int i = 0, j = 0; i < mesh.FaceCount; i++, j += 3)
            {
                GraphicResearchHuiZhao.Vector3D u = mesh.GetDualPosition(i);
                GraphicResearchHuiZhao.Vector3D lap = new GraphicResearchHuiZhao.Vector3D(lapCoordiante[j], lapCoordiante[j + 1], lapCoordiante[j + 2]);
                GraphicResearchHuiZhao.Vector3D v = u + 3.0 * lap;
                GL.Vertex3(u.x, u.y, u.z);
                GL.Vertex3(v.x, v.y, v.z);
            }
            GL.End();
        }

        public void DrawNotFullRankVertex(NonManifoldMesh mesh, bool[] fullRank)
        {
            GL.Color3(1.0f, 0.4f, 0.4f);
            GL.PointSize(6.0f);
            GL.EnableClientState(ArrayCap.VertexArray);

            GL.VertexPointer(3, VertexPointerType.Double, 0, mesh.VertexPos);
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Begin(BeginMode.Points);
            for (int i = 0; i < mesh.VertexCount; i++)
            {
                if (fullRank[i] == true) continue;
                GL.ArrayElement(i);
            }
            GL.End();

            GL.DisableClientState(ArrayCap.VertexArray);
        }

        public void SphereEnvShaded(ref NonManifoldMesh mesh)
        {

        }

        public void DrawTextureShaded(NonManifoldMesh m)
        {


            if (OpenGLManager.Instance.FirstTexture == 0)
                return;

            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            //GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Normalize);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, OpenGLManager.Instance.FirstTexture);

            Color c = GlobalSetting.DisplaySetting.MeshColor;
            GL.Color3(c.R, c.G, c.B);

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);
            //GL.EnableClientState(ArrayCap.TextureCoordArray);

            GL.VertexPointer<double>(3, VertexPointerType.Double, 0, m.VertexPos);
            GL.NormalPointer<double>(NormalPointerType.Double, 0, m.VertexNormal);
            //GL.TexCoordPointer<double>(2, TexCoordPointerType.Double, 0, m.VertextextCoordinate);

            //int[,] face = new int[2, m.FaceCount * 3];
            //for (int i = 0; i < m.FaceCount * 3; i++)
            //{
            //    face[0, i] = m.FaceIndex[i];
            //    face[1, i] = m.FaceTexIndex[i];
            //}

            //GL.DrawElements<int>(BeginMode.Triangles, m.FaceCount * 3, DrawElementsType.UnsignedInt, face);
            //GL.DrawElements(BeginMode.Triangles, m.FaceCount * 3, DrawElementsType.UnsignedInt, m.FaceIndex);




            GL.Begin(BeginMode.Triangles);
            for (int i = 0, j = 0; i < m.FaceCount; i++, j += 3)
            {
                int s = 0;
                int t = 0;
                s = m.FaceTexIndex[j] * 2;
                t = m.FaceTexIndex[j] * 2 + 1;
                GL.TexCoord2(m.TextextCoordinate[s], 1 - m.TextextCoordinate[t]);
                GL.ArrayElement(m.FaceIndex[j]);
                s = m.FaceTexIndex[j + 1] * 2;
                t = m.FaceTexIndex[j + 1] * 2 + 1;
                GL.TexCoord2(m.TextextCoordinate[s], 1 - m.TextextCoordinate[t]);
                GL.ArrayElement(m.FaceIndex[j + 1]);
                s = m.FaceTexIndex[j + 2] * 2;
                t = m.FaceTexIndex[j + 2] * 2 + 1;
                GL.TexCoord2(m.TextextCoordinate[s], 1 - m.TextextCoordinate[t]);
                GL.ArrayElement(m.FaceIndex[j + 2]);
            }
            GL.End();



            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.NormalArray);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
            //GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.Normalize);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);

        }


        public void DrawTextureAnimationShaded(NonManifoldMesh m)
        {

            GL.MatrixMode(MatrixMode.Texture);
            GL.Translate(0.01f, 0.01f, 0f);

            DrawTextureShaded(m);

        }


        public void DrawMultiTextureShaded(NonManifoldMesh m)
        {


            if (OpenGLManager.Instance.FirstTexture == 0)
                return;
            if (OpenGLManager.Instance.SecondTexture == 0)
                return;


            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            //GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Normalize);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, OpenGLManager.Instance.FirstTexture);

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, OpenGLManager.Instance.SecondTexture);

            Color c = GlobalSetting.DisplaySetting.MeshColor;
            GL.Color3(c.R, c.G, c.B);

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);
            //GL.EnableClientState(ArrayCap.TextureCoordArray);

            GL.VertexPointer<double>(3, VertexPointerType.Double, 0, m.VertexPos);
            GL.NormalPointer<double>(NormalPointerType.Double, 0, m.VertexNormal);
            //GL.TexCoordPointer<double>(2, TexCoordPointerType.Double, 0, m.VertextextCoordinate);

            //int[,] face = new int[2, m.FaceCount * 3];
            //for (int i = 0; i < m.FaceCount * 3; i++)
            //{
            //    face[0, i] = m.FaceIndex[i];
            //    face[1, i] = m.FaceTexIndex[i];
            //}

            //GL.DrawElements<int>(BeginMode.Triangles, m.FaceCount * 3, DrawElementsType.UnsignedInt, face);
            //GL.DrawElements(BeginMode.Triangles, m.FaceCount * 3, DrawElementsType.UnsignedInt, m.FaceIndex);

            GL.Begin(BeginMode.Triangles);
            for (int i = 0, j = 0; i < m.FaceCount; i++, j += 3)
            {
                int s = 0;
                int t = 0;
                s = m.FaceTexIndex[j] * 2;
                t = m.FaceTexIndex[j] * 2 + 1;
                GL.MultiTexCoord2(TextureUnit.Texture0, m.TextextCoordinate[s], 1 - m.TextextCoordinate[t]);
                GL.MultiTexCoord2(TextureUnit.Texture1, m.TextextCoordinate[s], 1 - m.TextextCoordinate[t]);

                GL.ArrayElement(m.FaceIndex[j]);
                s = m.FaceTexIndex[j + 1] * 2;
                t = m.FaceTexIndex[j + 1] * 2 + 1;

                GL.MultiTexCoord2(TextureUnit.Texture0, m.TextextCoordinate[s], 1 - m.TextextCoordinate[t]);
                GL.MultiTexCoord2(TextureUnit.Texture1, m.TextextCoordinate[s], 1 - m.TextextCoordinate[t]);

                GL.ArrayElement(m.FaceIndex[j + 1]);
                s = m.FaceTexIndex[j + 2] * 2;
                t = m.FaceTexIndex[j + 2] * 2 + 1;

                GL.MultiTexCoord2(TextureUnit.Texture0, m.TextextCoordinate[s], 1 - m.TextextCoordinate[t]);
                GL.MultiTexCoord2(TextureUnit.Texture1, m.TextextCoordinate[s], 1 - m.TextextCoordinate[t]);

                GL.ArrayElement(m.FaceIndex[j + 2]);
            }
            GL.End();



            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.NormalArray);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
            //GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.Normalize);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);

        }

        public void DrawAutoTextureShaded(NonManifoldMesh m)
        {


            if (OpenGLManager.Instance.FirstTexture == 0)
                return;

            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            //GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Normalize);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, OpenGLManager.Instance.FirstTexture);

            Color c = GlobalSetting.DisplaySetting.MeshColor;
            GL.Color3(c.R, c.G, c.B);

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);

            GL.VertexPointer<double>(3, VertexPointerType.Double, 0, m.VertexPos);
            GL.NormalPointer<double>(NormalPointerType.Double, 0, m.VertexNormal);


            OpenGLManager.Instance.EnableAutoTexture();

            GL.Begin(BeginMode.Triangles);
            for (int i = 0, j = 0; i < m.FaceCount; i++, j += 3)
            {

                GL.ArrayElement(m.FaceIndex[j]);

                GL.ArrayElement(m.FaceIndex[j + 1]);

                GL.ArrayElement(m.FaceIndex[j + 2]);
            }
            GL.End();



            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.NormalArray);

            GL.Disable(EnableCap.Normalize);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);

        }
    }
}
