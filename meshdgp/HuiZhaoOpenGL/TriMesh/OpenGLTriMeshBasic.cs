using System;
using System.Collections.Generic;
using System.Drawing;
 
 
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public partial class OpenGLTriMesh
    {

        public void DrawVertices(TriMesh mesh)
        {
            GL.Begin(BeginMode.Points);
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                GL.Normal3(mesh.Vertices[i].Traits.Normal.x,
                           mesh.Vertices[i].Traits.Normal.y,
                           mesh.Vertices[i].Traits.Normal.z);
                GL.Vertex3(mesh.Vertices[i].Traits.Position.x, 
                           mesh.Vertices[i].Traits.Position.y, 
                           mesh.Vertices[i].Traits.Position.z);
            }
            GL.End();
        }

        public void DrawTriangles(TriMesh mesh)
        {
            GL.Begin(BeginMode.Triangles);
            for (int i = 0; i < mesh.Faces.Count; i++)
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
            GL.End();
        }
        public void DrawFaceColorRandom(TriMesh mesh)
        {
            GL.ShadeModel(GlobalSetting.EnalbeCapsSetting.ShadingModel);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.Normalize);

            GL.Begin(BeginMode.Triangles);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                foreach (TriMesh.Vertex vertex in mesh.Faces[i].Vertices)
                {
                    int color = vertex.Index % 6 + 1;
                    GL.Color3((float)(color / 4),
                              (float)(color / 2 % 2),
                              (float)(color % 2));
                    GL.Normal3(vertex.Traits.Normal.ToArray());
                    GL.Vertex3(vertex.Traits.Position.ToArray());
                }
            }
            GL.End();
        }

        public void DrawPoints(TriMesh mesh)
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.CullFace);            
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Point);
            GL.PointSize(GlobalSetting.DisplaySetting.PointSize);
            GL.Enable(EnableCap.PointSmooth);
            GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
            OpenGLManager.Instance.SetColor(GlobalSetting.DisplaySetting.MeshColor);
            DrawVertices(mesh);
        }

        public void DrawWireFrame(TriMesh mesh)
        {    
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.LineWidth(GlobalSetting.DisplaySetting.LineWidth);
            GL.Enable(EnableCap.Normalize);
            GL.Enable(EnableCap.CullFace);
            OpenGLManager.Instance.SetColor(GlobalSetting.DisplaySetting.MeshColor);
            DrawTriangles(mesh); 
        }


        public void DrawWireFrameGray(TriMesh mesh)
        {
           
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.LineWidth(GlobalSetting.DisplaySetting.LineWidth);
            GL.Enable(EnableCap.CullFace);
            OpenGLManager.Instance.SetColor(GlobalSetting.DisplaySetting.WifeFrameColor);  
            DrawTriangles(mesh);
            
        }

        public void DrawWireFrameDark(TriMesh mesh)
        {
             
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.LineWidth(GlobalSetting.DisplaySetting.LineWidth);
            GL.Enable(EnableCap.CullFace);  
            OpenTK.Graphics.Color4 color = new OpenTK.Graphics.Color4(0.0f, 0.0f, 0.0f, 0.0f);
            OpenGLManager.Instance.SetColor(color); 
            DrawTriangles(mesh);
            
        }

        public void DrawCageWireFrame(TriMesh mesh)
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.LineWidth(GlobalSetting.DisplaySetting.LineWidth);
            GL.Enable(EnableCap.CullFace);

            OpenGLManager.Instance.SetColor(GlobalSetting.DisplaySetting.BoundaryColor); 
            DrawTriangles(mesh);


        }
       

        public void DrawFlatShaded(TriMesh mesh)
        {
            GL.ShadeModel(ShadingModel.Flat);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.Normalize);
            OpenGLManager.Instance.SetColor(GlobalSetting.DisplaySetting.MeshColor);
            DrawTriangles(mesh);
        }

        public void DrawFlatHiddenLine(TriMesh mesh)
        {
            GL.Enable(EnableCap.PolygonOffsetFill);
            DrawFlatShaded(mesh);
            DrawWireFrameDark(mesh);
            GL.Disable(EnableCap.PolygonOffsetFill);
        }

        public void DrawSmoothShaded(TriMesh mesh)
        { 
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill); 
            GL.Enable(EnableCap.Normalize); 
            Color c = GlobalSetting.DisplaySetting.MeshColor;
            OpenGLManager.Instance.SetColor(c);
            DrawTriangles(mesh);
        }

        public void DrawSmoothHiddenLine(TriMesh mesh)
        {
            GL.Enable(EnableCap.PolygonOffsetFill); 
            DrawSmoothShaded(mesh); 
            DrawWireFrameDark(mesh); 
            GL.Disable(EnableCap.PolygonOffsetFill);
        }
         

      

     
         
      

        

        public void DrawCurves(TriMesh m, List<List<int>> lines)
        {
            OpenGLManager.Instance.SetColor(GlobalSetting.DisplaySetting.BoundaryColor);
          
            GL.LineWidth(GlobalSetting.DisplaySetting.SelectionLineWidth);

            for (int i = 0; i < lines.Count; i++)
            {
                GL.Begin(BeginMode.LineStrip);
                
                for (int j = 0; j < lines[i].Count; j++)
                {
                    GL.Vertex3(m.Vertices[lines[i][j]].Traits.Position.x, 
                               m.Vertices[lines[i][j]].Traits.Position.y, 
                               m.Vertices[lines[i][j]].Traits.Position.z);
                }
                GL.End(); 
            } 
        }

    
       

        public void DrawVertexColor(TriMesh mesh)
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            GL.Enable(EnableCap.Normalize); 
            GL.Begin(BeginMode.Triangles); 
            Color4 color=Color4.White;
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                foreach (TriMesh.Vertex vertex in mesh.Faces[i].Vertices)
                {
                    color = vertex.Traits.Color;
                  
                    OpenGLManager.Instance.SetColor(color);
                    GL.Normal3(vertex.Traits.Normal.x, vertex.Traits.Normal.y, vertex.Traits.Normal.z);
                    GL.Vertex3(vertex.Traits.Position.x, vertex.Traits.Position.y, vertex.Traits.Position.z);

                }
            }
            GL.End(); 
            GL.Flush();

        }


     


        public void DrawSegementationVertex(TriMesh mesh)
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            GL.Enable(EnableCap.Normalize);

            GL.Begin(BeginMode.Triangles);

            OpenTK.Graphics.Color4 color = GlobalSetting.DisplaySetting.MeshColor;
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                foreach (TriMesh.Vertex vertex in mesh.Faces[i].Vertices)
                {

                    Color4 color4 = vertex.Traits.Color;

                    if (color4 != Color4.Black)
                    {
                        

                        OpenGLManager.Instance.SetColor(color4);
                    }

                    else
                    {
                        color=SetColorPallete(vertex.Traits.SelectedFlag);                        

                        OpenGLManager.Instance.SetColor(color);
                    }
                    GL.Normal3(vertex.Traits.Normal.x, vertex.Traits.Normal.y, vertex.Traits.Normal.z);
                    GL.Vertex3(vertex.Traits.Position.x, vertex.Traits.Position.y, vertex.Traits.Position.z);

                }
            }
            GL.End();

            GL.Flush();

        }



        public OpenTK.Graphics.Color4 SetColorPallete(int num)
        {
            OpenTK.Graphics.Color4 color = GlobalSetting.DisplaySetting.MeshColor;
            switch (num % 20)
            {
                case 0: color = GlobalSetting.DisplaySetting.MeshColor; break;
                // case 0: color = new OpenTK.Graphics.Color4(0.0f, 0.0f, 1.0f, 0.0f); break;
                case 1: color = new OpenTK.Graphics.Color4(1.0f, 0.0f, 0.0f, 0.0f); break;
                case 2: color = new OpenTK.Graphics.Color4(0.0f, 1.0f, 0.0f, 0.0f); break;
                case 3: color = new OpenTK.Graphics.Color4(1.0f, 1.0f, 0.0f, 0.0f); break;
                case 4: color = new OpenTK.Graphics.Color4(0.0f, 1.0f, 1.0f, 0.0f); break;
                case 5: color = new OpenTK.Graphics.Color4(1.0f, 0.0f, 1.0f, 0.0f); break;
                case 6: color = OpenTK.Graphics.Color4.Brown; break;
                case 7: color = OpenTK.Graphics.Color4.Tomato; break;
                case 8: color = OpenTK.Graphics.Color4.Turquoise; break;
                case 9: color = OpenTK.Graphics.Color4.Violet; break;
                case 10: color = OpenTK.Graphics.Color4.Goldenrod; break;


            }

            return color;
        }




        public void DrawSegementationBoundaryOnly(TriMesh mesh)
        {
            OpenTK.Graphics.Color4 color = GlobalSetting.DisplaySetting.MeshColor;
            OpenGLManager.Instance.SetColor(color); 
            GL.Begin(BeginMode.Lines);
            for (int i = 0; i < mesh.Edges.Count; i++)
            {
                if (mesh.Edges[i].Face0 != null && mesh.Edges[i].Face1 != null)
                {
                    if (mesh.Edges[i].Face0.Traits.SelectedFlag != mesh.Edges[i].Face1.Traits.SelectedFlag)
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
            }
            GL.End();
        }

        public void DrawSegementationWithBoundary(TriMesh mesh)
        {
            GL.Enable(EnableCap.PolygonOffsetFill);
            DrawSegementationFace(mesh);
            DrawSegementationBoundaryOnly(mesh);
            GL.Disable(EnableCap.PolygonOffsetFill);
        }

        public void DrawSegementationFace(TriMesh mesh)
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            GL.Enable(EnableCap.Normalize);

            GL.Begin(BeginMode.Triangles);

            OpenTK.Graphics.Color4 color = GlobalSetting.DisplaySetting.MeshColor;
            for (int i = 0; i < mesh.Faces.Count; i++)
            {

                Color4 color4 = mesh.Faces[i].Traits.Color;
                if (color4 != Color4.Black)
                {


                    OpenGLManager.Instance.SetColor(color4);
                }

                else
                {
                    color = SetColorPallete(mesh.Faces[i].Traits.SelectedFlag);         
                    

                    OpenGLManager.Instance.SetColor( color);
                }
                foreach (TriMesh.Vertex vertex in mesh.Faces[i].Vertices)
                {  
                    GL.Normal3(vertex.Traits.Normal.x, vertex.Traits.Normal.y, vertex.Traits.Normal.z);
                    GL.Vertex3(vertex.Traits.Position.x, vertex.Traits.Position.y, vertex.Traits.Position.z);

                }
            }
            GL.End();

            GL.Flush();

        }


 



        public void DrawPointsWithLine(TriMesh mesh)
        {
            GL.ShadeModel(ShadingModel.Smooth);

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.PointSize(GlobalSetting.DisplaySetting.PointSize);

            OpenGLManager.Instance.SetColor(Color.Red);

            GL.Begin(BeginMode.LineStrip);

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                GL.Vertex3(mesh.Vertices[i].Traits.Position.x, mesh.Vertices[i].Traits.Position.y, mesh.Vertices[i].Traits.Position.z);
            }
            GL.End();


            GL.Flush();

        }


        public void DrawCurve(TriMesh mesh, Color color)
        {
            GL.ShadeModel(ShadingModel.Smooth);

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.PointSize(GlobalSetting.DisplaySetting.PointSize);

            OpenGLManager.Instance.SetColor(color);

            GL.Begin(BeginMode.LineStrip);

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                GL.Vertex3(mesh.Vertices[i].Traits.Position.x, 
                           mesh.Vertices[i].Traits.Position.y, 
                           mesh.Vertices[i].Traits.Position.z);
            }
            GL.End(); 

            GL.Flush();

        }

      
    }
}
