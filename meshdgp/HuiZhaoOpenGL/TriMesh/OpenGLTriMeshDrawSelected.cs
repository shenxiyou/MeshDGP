using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace GraphicResearchHuiZhao 
{
    public partial class OpenGLTriMesh
    {

      

        public void DrawSelectedComplex(TriMesh mesh)
        {
            GL.Enable(EnableCap.PolygonOffsetFill);
          
            DrawSmoothShaded(mesh);
           // GL.Disable(EnableCap.DepthTest);
            DrawWireFrameGray(mesh); 
            DrawSelectedEdgeColor(mesh);  
            DrawSelectedFaceColor(mesh);  
            DrawSelectedVerticeColor(mesh); 
            GL.Disable(EnableCap.PolygonOffsetFill);
        }

        public void DrawSelectedSmooth(TriMesh mesh)
        {
            GL.Enable(EnableCap.PolygonOffsetFill);
            DrawSmoothShaded(mesh); 
            DrawSelectedEdgeColor(mesh);
            DrawSelectedFaceColor(mesh);
            DrawSelectedVerticeColor(mesh); 
            GL.Disable(EnableCap.PolygonOffsetFill);
        }

        public void DrawSelectedEdgeColor(TriMesh mesh)
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line); 
            GL.LineWidth(GlobalSetting.DisplaySetting.SelectionLineWidth);
            GL.Enable(EnableCap.LineSmooth);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
            OpenGLManager.Instance.SetColor(
                GlobalSetting.DisplaySetting.SelectedEdgeColor);
            GL.Begin(BeginMode.Lines);
            for (int i = 0; i < mesh.Edges.Count; i++)
            {
                if (mesh.Edges[i].Traits.SelectedFlag != 0)
                {
                    Color4 color = mesh.Edges[i].Traits.Color;
                    OpenTK.Graphics.Color4 colorTwo = 
                        GlobalSetting.DisplaySetting.SelectedEdgeColor;
                    if (color != Color4.Black)
                    {
                        colorTwo = OpenGLManager.Instance.ConvertColor(color);   
                    } 
                    else if (mesh.Edges[i].Traits.SelectedFlag > 1)
                    {
                        colorTwo = SetRandomColor(
                            (byte)mesh.Edges[i].Traits.SelectedFlag);
                    } 
                    OpenGLManager.Instance.SetColor(colorTwo); 
                    GL.Normal3(mesh.Edges[i].Vertex0.Traits.Normal.x,
                               mesh.Edges[i].Vertex0.Traits.Normal.y,
                               mesh.Edges[i].Vertex0.Traits.Normal.z);
                    GL.Vertex3(mesh.Edges[i].Vertex0.Traits.Position.x,
                               mesh.Edges[i].Vertex0.Traits.Position.y,
                               mesh.Edges[i].Vertex0.Traits.Position.z);
                    GL.Normal3(mesh.Edges[i].Vertex1.Traits.Normal.x,
                               mesh.Edges[i].Vertex1.Traits.Normal.y,
                               mesh.Edges[i].Vertex1.Traits.Normal.z);
                    GL.Vertex3(mesh.Edges[i].Vertex1.Traits.Position.x,
                               mesh.Edges[i].Vertex1.Traits.Position.y,
                               mesh.Edges[i].Vertex1.Traits.Position.z);
                }
            }
            GL.End();
        }

        public void DrawFaceColor(TriMesh mesh)
        {


            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.Normalize);


            OpenGLManager.Instance.SetColor(GlobalSetting.DisplaySetting.MeshColor);
            GL.Begin(BeginMode.Triangles);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                
                Color4 color = mesh.Faces[i].Traits.Color; 
                OpenTK.Graphics.Color4 colorTwo = GlobalSetting.DisplaySetting.MeshColor;
                if (color != Color4.Black)
                {
                    colorTwo = OpenGLManager.Instance.ConvertColor(color);

                }
               // colorTwo = SetRandomColor(i);
               
                   
                

                OpenGLManager.Instance.SetColor(colorTwo);

                foreach (TriMesh.Vertex vertex in mesh.Faces[i].Vertices)
                {
                    GL.Normal3(vertex.Traits.Normal.x, vertex.Traits.Normal.y, vertex.Traits.Normal.z);
                    GL.Vertex3(vertex.Traits.Position.x, vertex.Traits.Position.y, vertex.Traits.Position.z);

                }
               
            }
            GL.End();
        }


        public void DrawSelectedFaceColor(TriMesh mesh)
        { 
            

            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.Normalize);


            OpenGLManager.Instance.SetColor(GlobalSetting.DisplaySetting.SelectedFaceColor);
            GL.Begin(BeginMode.Triangles);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                if (mesh.Faces[i].Traits.SelectedFlag != 0)
                {
                    Color4 color = mesh.Faces[i].Traits.Color;

                    OpenTK.Graphics.Color4 colorTwo = GlobalSetting.DisplaySetting.SelectedFaceColor;
                    if (color != Color4.Black)
                    {
                        colorTwo = OpenGLManager.Instance.ConvertColor(color);  

                    }

                    else if (mesh.Faces[i].Traits.SelectedFlag > 1)
                    {
                        colorTwo = SetRandomColor((byte)mesh.Faces[i].Traits.SelectedFlag);
                    }

                    OpenGLManager.Instance.SetColor(colorTwo);
 
                    foreach (TriMesh.Vertex vertex in mesh.Faces[i].Vertices)
                    {
                        GL.Normal3(vertex.Traits.Normal.x, vertex.Traits.Normal.y, vertex.Traits.Normal.z);
                        GL.Vertex3(vertex.Traits.Position.x, vertex.Traits.Position.y, vertex.Traits.Position.z);

                    }
                }
            }
            GL.End();
        }


        public void DrawSelectedVerticeColor(TriMesh mesh)
        {
            OpenGLManager.Instance.SetColor(
                       GlobalSetting.DisplaySetting.SelectedVertexColor);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Point); 
            GL.PointSize(GlobalSetting.DisplaySetting.PointSize); 
            GL.Enable(EnableCap.PointSmooth);
            GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest); 
            GL.Begin(BeginMode.Points);
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                if (mesh.Vertices[i].Traits.SelectedFlag != 0)
                {
                    Color4 color = mesh.Vertices[i].Traits.Color; 
                    OpenTK.Graphics.Color4 colorTwo = OpenTK.Graphics.Color4.Red ;
                    if (color != Color4.Black)
                    {
                        colorTwo = OpenGLManager.Instance.ConvertColor(color);   
                    } 
                    else if (mesh.Vertices[i].Traits.SelectedFlag > 1)
                    {
                      colorTwo = 
                          SetRandomColor((byte)mesh.Vertices[i].Traits.SelectedFlag);
                    } 
                    OpenGLManager.Instance.SetColor(colorTwo); 
                    GL.Normal3(mesh.Vertices[i].Traits.Normal.x,
                               mesh.Vertices[i].Traits.Normal.y,
                               mesh.Vertices[i].Traits.Normal.z);
                    GL.Vertex3(mesh.Vertices[i].Traits.Position.x,
                               mesh.Vertices[i].Traits.Position.y,
                               mesh.Vertices[i].Traits.Position.z);
                }
            }
            GL.End();
        }




        public void DrawSelectedEdges(TriMesh m)
        {
            GL.Enable(EnableCap.PolygonOffsetFill);
            DrawSmoothShaded(m);
            DrawSelectedEdgeOnly(m);
            GL.Disable(EnableCap.PolygonOffsetFill);

        }

        private void DrawSelectedEdgeOnly(TriMesh mesh)
        {
         OpenGLManager.Instance.SetColor(GlobalSetting.DisplaySetting.SelectedEdgeColor);
            GL.LineWidth(GlobalSetting.DisplaySetting.SelectionLineWidth);
            GL.Begin(BeginMode.Lines);
            for (int i = 0; i < mesh.Edges.Count; i++)
            {
                if (mesh.Edges[i].Traits.SelectedFlag != 0)
                {
                    GL.Normal3(mesh.Edges[i].Vertex0.Traits.Normal.x,
                               mesh.Edges[i].Vertex0.Traits.Normal.y,
                               mesh.Edges[i].Vertex0.Traits.Normal.z);
                    GL.Vertex3(mesh.Edges[i].Vertex0.Traits.Position.x,
                               mesh.Edges[i].Vertex0.Traits.Position.y,
                               mesh.Edges[i].Vertex0.Traits.Position.z);
                    GL.Normal3(mesh.Edges[i].Vertex1.Traits.Normal.x,
                               mesh.Edges[i].Vertex1.Traits.Normal.y,
                               mesh.Edges[i].Vertex1.Traits.Normal.z);
                    GL.Vertex3(mesh.Edges[i].Vertex1.Traits.Position.x,
                               mesh.Edges[i].Vertex1.Traits.Position.y,
                               mesh.Edges[i].Vertex1.Traits.Position.z);
                }
            }
            GL.End();
        }

        public void DrawSelectedFace(TriMesh mesh)
        {
            GL.Enable(EnableCap.PolygonOffsetFill);
            DrawSmoothShaded(mesh);
            DrawSelectedFaceOnly(mesh);
            GL.Disable(EnableCap.PolygonOffsetFill);


        }

        public void DrawSelectedFaceOnly(TriMesh mesh)
        {
            OpenGLManager.Instance.SetColor(
                GlobalSetting.DisplaySetting.SelectedFaceColor);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);            
            GL.LineWidth(GlobalSetting.DisplaySetting.SelectionLineWidth);
            GL.Begin(BeginMode.Triangles);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                if (mesh.Faces[i].Traits.SelectedFlag != 0)
                {
                    foreach (TriMesh.Vertex vertex in mesh.Faces[i].Vertices)
                    {
                        GL.Normal3(vertex.Traits.Normal.x, 
                                   vertex.Traits.Normal.y, 
                                   vertex.Traits.Normal.z);
                        GL.Vertex3(vertex.Traits.Position.x, 
                                   vertex.Traits.Position.y, 
                                   vertex.Traits.Position.z);

                    }
                }
            }
            GL.End();
        }


        public void DrawSelectedVerticeBySphere(TriMesh mesh)
        { 
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                if (mesh.Vertices[i].Traits.SelectedFlag == 0)
                    continue;

                OpenGLManager.Instance.DrawBall(mesh.Vertices[i].Traits.SelectedFlag,  mesh.Vertices[i].Traits.Position );

               
            }
        }

        public OpenTK.Graphics.Color4 SetRandomColor(int i)
        { 
            Color4 color = TriMeshUtil.SetRandomColor(i);
            OpenTK.Graphics.Color4 colorTwo = OpenGLManager.Instance.ConvertColor(color);   
            switch (i % 6)
            {
            case 0: colorTwo = new OpenTK.Graphics.Color4(0.0f, 0.0f, 1.0f, 0.0f); break;
            case 1: colorTwo = new OpenTK.Graphics.Color4(1.0f, 0.0f, 0.0f, 0.0f); break;
            case 2: colorTwo = new OpenTK.Graphics.Color4(0.0f, 1.0f, 0.0f, 0.0f); break;
            case 3: colorTwo = new OpenTK.Graphics.Color4(1.0f, 1.0f, 0.0f, 0.0f); break;
            case 4: colorTwo = new OpenTK.Graphics.Color4(0.0f, 1.0f, 1.0f, 0.0f); break;
            case 5: colorTwo = new OpenTK.Graphics.Color4(1.0f, 0.0f, 1.0f, 0.0f); break; 
            } 
            return colorTwo;
        }
        


    }
}
