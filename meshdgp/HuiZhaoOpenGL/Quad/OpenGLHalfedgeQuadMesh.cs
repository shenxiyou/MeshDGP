using System;
using System.Collections.Generic;
using System.Drawing;
 
 
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public partial class OpenGLTriMesh
    {
        public void DrawHalfEdgeQuadMesh(QuadMesh mesh)
        {
            switch (GlobalSetting.DisplaySetting.DisplayMode)
            {
                case EnumDisplayMode.Vertex:
                    DrawPoints(mesh);
                    break;
                case EnumDisplayMode.WireFrame:
                    DrawWireFrame(mesh);
                    break;

                case EnumDisplayMode.Flat:
                    DrawFlatShaded(mesh);
                    break;
                case EnumDisplayMode.FlatLine:

                    DrawFlatHiddenLine(mesh);
                    break;

                case EnumDisplayMode.Smooth:
                    DrawSmoothShaded(mesh);

                    break;
                case EnumDisplayMode.SmoothLine:
                    DrawSmoothHiddenLine(mesh);
                    break;

                case EnumDisplayMode.SelectedVertex:
                    DrawSmoothShaded(mesh);
                    DrawSelectedVerticeBySphere(mesh);
                    break;

                case EnumDisplayMode.SelectedFace:
                    DrawSmoothShaded(mesh);
                    DrawSelectedFace(mesh);
                    break;

                case EnumDisplayMode.SelectedEdge:
                    DrawSmoothShaded(mesh);
                    DrawSelectedEdges(mesh);
                    break;

                case EnumDisplayMode.Basic:
                    DrawBasic(mesh);
                    break;


            }
        }


        public void DrawFlatShaded(QuadMesh mesh)
        {

            GL.ShadeModel(ShadingModel.Flat);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            GL.Enable(EnableCap.Normalize);

            Color c = GlobalSetting.DisplaySetting.MeshColor;
            GL.Color3(c);

            GL.Begin(BeginMode.Triangles);

            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                foreach (QuadMesh.Vertex vertex in mesh.Faces[i].Vertices)
                {
                    GL.Normal3(vertex.Traits.Normal.x, vertex.Traits.Normal.y, vertex.Traits.Normal.z);
                    GL.Vertex3(vertex.Traits.Position.x, vertex.Traits.Position.y, vertex.Traits.Position.z);

                }
            }
            GL.End();

            GL.Flush();

        }


        public void DrawSmoothShaded(QuadMesh mesh)
        {
            GL.Enable(EnableCap.PolygonOffsetFill);

            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            GL.Enable(EnableCap.Normalize);

            Color c = GlobalSetting.DisplaySetting.MeshColor;
            GL.Color3(c);

            GL.Begin(BeginMode.Quads);

            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                foreach (QuadMesh.Vertex vertex in mesh.Faces[i].Vertices)
                {
                    GL.Normal3(vertex.Traits.Normal.x, vertex.Traits.Normal.y, vertex.Traits.Normal.z);
                    GL.Vertex3(vertex.Traits.Position.x, vertex.Traits.Position.y, vertex.Traits.Position.z);

                }
            }
            GL.End();

            GL.Flush();

        }



        public void DrawWireFrame(QuadMesh mesh)
        { 
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            GL.Enable(EnableCap.Normalize);

            Color c = GlobalSetting.DisplaySetting.WifeFrameColor;
            GL.Color3(c);

            DrawQuads(mesh);

            GL.Flush();

        }

        private void DrawQuads(QuadMesh mesh)
        {
            GL.Begin(BeginMode.Quads);

            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                foreach (QuadMesh.Vertex vertex in mesh.Faces[i].Vertices)
                {
                    GL.Normal3(vertex.Traits.Normal.x, vertex.Traits.Normal.y, vertex.Traits.Normal.z);
                    GL.Vertex3(vertex.Traits.Position.x, vertex.Traits.Position.y, vertex.Traits.Position.z);

                }
            }
            GL.End();
        }



        public void DrawSelectedVerticeBySphere(QuadMesh m)
        {

            for (int i = 0; i < m.Vertices.Count; i++)
            {
                if (m.Vertices[i].Traits.SelectedFlag == 0) continue;
                switch (m.Vertices[i].Traits.SelectedFlag % 6)
                {
                    case 0: GL.Color3(0.0f, 0.0f, 1.0f); break;
                    case 1: GL.Color3(1.0f, 0.0f, 0.0f); break;
                    case 2: GL.Color3(0.0f, 1.0f, 0.0f); break;
                    case 3: GL.Color3(1.0f, 1.0f, 0.0f); break;
                    case 4: GL.Color3(0.0f, 1.0f, 1.0f); break;
                    case 5: GL.Color3(1.0f, 0.0f, 1.0f); break;
                }
                GL.PushMatrix();
                GL.Translate(m.Vertices[i].Traits.Position.x, m.Vertices[i].Traits.Position.y, m.Vertices[i].Traits.Position.z);
                GL.Scale(0.01, 0.01, 0.01);
                OpenGLManager.Instance.DrawSphere();
                GL.PopMatrix();
            }
        }

        public void DrawDarkWireframe(QuadMesh mesh)
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Disable(EnableCap.CullFace);
            GL.Color3(0.0f, 0.0f, 0.0f);
            GL.LineWidth(GlobalSetting.DisplaySetting.LineWidth);

            DrawQuads(mesh);

            GL.Enable(EnableCap.CullFace);
        }



        public void DrawSmoothHiddenLine(QuadMesh m)
        {
            GL.Enable(EnableCap.PolygonOffsetFill);
            DrawSmoothShaded(m);
            DrawWireFrame(m);
            GL.Disable(EnableCap.PolygonOffsetFill);
        }

        public void DrawFlatHiddenLine(QuadMesh m)
        {
            GL.Enable(EnableCap.PolygonOffsetFill);
            DrawFlatShaded(m);
            DrawDarkWireframe(m);
            GL.Disable(EnableCap.PolygonOffsetFill);
        }

        public void DrawPoints(QuadMesh m)
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Point);
            GL.PointSize(GlobalSetting.DisplaySetting.PointSize);
            Color c = GlobalSetting.DisplaySetting.MeshColor;
            DrawQuads(m);
        }


        public void DrawCurves(QuadMesh m, List<List<int>> lines)
        {
            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.LineWidth(GlobalSetting.DisplaySetting.SelectionLineWidth);

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
                    GL.Vertex3(m.Vertices[lines[i][j]].Traits.Position.x, m.Vertices[lines[i][j]].Traits.Position.y, m.Vertices[lines[i][j]].Traits.Position.z);
                }
                GL.End();

            }



        }

        public void DrawSelectedEdges(QuadMesh m)
        {
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.LineWidth(GlobalSetting.DisplaySetting.SelectionLineWidth);
            GL.Begin(BeginMode.Lines);
            for (int i = 0; i < m.Edges.Count; i++)
            {
                if (m.Edges[i].Traits.SelectedFlag != 0)
                {
                    GL.Normal3(m.Edges[i].Vertex0.Traits.Normal.x, m.Edges[i].Vertex0.Traits.Normal.y, m.Edges[i].Vertex0.Traits.Normal.z);
                    GL.Vertex3(m.Edges[i].Vertex0.Traits.Position.x, m.Edges[i].Vertex0.Traits.Position.y, m.Edges[i].Vertex0.Traits.Position.z);
                    GL.Normal3(m.Edges[i].Vertex1.Traits.Normal.x, m.Edges[i].Vertex1.Traits.Normal.y, m.Edges[i].Vertex1.Traits.Normal.z);
                    GL.Vertex3(m.Edges[i].Vertex1.Traits.Position.x, m.Edges[i].Vertex1.Traits.Position.y, m.Edges[i].Vertex1.Traits.Position.z);
                }
            }
            GL.End();

        }
        public void DrawSelectedFace(QuadMesh m)
        {

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            GL.Color3(1.0f, 0.0f, 0.0f);

            GL.LineWidth(GlobalSetting.DisplaySetting.SelectionLineWidth);

            GL.Begin(BeginMode.Triangles);

            for (int i = 0; i < m.Faces.Count; i++)
            {
                if (m.Faces[i].Traits.SelectedFlag != 0)
                {
                    foreach (QuadMesh.Vertex vertex in m.Faces[i].Vertices)
                    {

                        GL.Normal3(vertex.Traits.Normal.x, vertex.Traits.Normal.y, vertex.Traits.Normal.z);
                        GL.Vertex3(vertex.Traits.Position.x, vertex.Traits.Position.y, vertex.Traits.Position.z);

                    }
                }
            }
            GL.End();


        }




        public void DrawBasic(QuadMesh mesh)
        {
            GL.Enable(EnableCap.PolygonOffsetFill);
            GL.PolygonOffset(1f, 1f);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.Normalize);
            //OpenGLManager.Instance.SetMeshColor(GlobalSetting.DisplaySetting.MeshColor);
            OpenGLManager.Instance.SetMaterialInfo();
            foreach (var face in mesh.Faces)
            {
                GL.Begin(BeginMode.Polygon);
                foreach (var v in face.Vertices)
                {
                    GL.Normal3(v.Traits.Normal.ToArray());
                    GL.Vertex3(v.Traits.Position.ToArray());
                }
                GL.End();
            }

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.LineWidth(GlobalSetting.DisplaySetting.LineWidth);
            GL.Enable(EnableCap.CullFace);
            OpenGLManager.Instance.SetColorMesh(GlobalSetting.DisplaySetting.WifeFrameColor);
            foreach (var face in mesh.Faces)
            {
                GL.Begin(BeginMode.Polygon);
                foreach (var v in face.Vertices)
                {
                    GL.Normal3(v.Traits.Normal.ToArray());
                    GL.Vertex3(v.Traits.Position.ToArray());
                }
                GL.End();
            }
            GL.Flush();
        }

        


    }
}
