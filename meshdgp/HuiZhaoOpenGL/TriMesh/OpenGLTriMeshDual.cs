using System;
using System.Collections.Generic;
using System.Drawing;
  
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public partial class OpenGLTriMesh
    {

        public void DrawDual(TriMesh mesh)
        {
            DrawSmoothHiddenLine(mesh);
            DrawDualPoint(mesh);

        }

        public void DrawDualPoint(TriMesh mesh)
        { 
            GL.PointSize(GlobalSetting.DisplaySetting.PointSize); 
            GL.Color3(Color.Red);
            GL.Begin(BeginMode.Points);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                Vector3D center = mesh.DualGetVertexPosition(i); 
                GL.Vertex3(center.x, center.y, center.z); 
            }
            GL.End(); 
            GL.Flush();

        }

        public void DrawDualLines(TriMesh mesh)
        {
            GL.ShadeModel(ShadingModel.Smooth); 
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.PointSize(GlobalSetting.DisplaySetting.PointSize); 
            GL.Color3(Color.Red); 
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                GL.Begin(BeginMode.LineLoop);
                foreach ( TriMesh.Face face in mesh.Vertices[i].Faces)
                { 
                    Vector3D center = new Vector3D(0, 0, 0);
                    foreach (TriMesh.Vertex vertex in face.Vertices)
                    {
                        center += vertex.Traits.Position; 
                    }
                    center = center / 3;
                    GL.Vertex3(center.x, center.y, center.z); 
                }
                GL.End(); 
            } 
            GL.Flush(); 
        }













        public void DrawDualMesh(TriMesh mesh)
        {
            
            DrawSelectedComplex(mesh);
            DrawDualMeshLine(GlobalData.Instance.DualMesh);
             

        }
        public void DrawDuaMeshPoint(TriMesh mesh)
        {
 
            GL.PointSize(GlobalSetting.DisplaySetting.PointSize);
            OpenGLManager.Instance.SetColor(GlobalSetting.DisplaySetting.DualColor);
            GL.Begin(BeginMode.Points);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                Vector3D center = mesh.DualGetVertexPosition(i);

                GL.Vertex3(center.x, center.y, center.z);

            }
            GL.End(); 
            GL.Flush();

        }
        public void DrawDualMeshLine(PolygonMesh mesh)
        {
            GL.ShadeModel(ShadingModel.Smooth);

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            OpenGLManager.Instance.SetColor(GlobalSetting.DisplaySetting.DualColor);
             
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                GL.Begin(BeginMode.LineLoop);
                foreach (TriMesh.Vertex v in mesh.Faces[i].Vertices)
                {
                    GL.Vertex3(v.Traits.Position.x, v.Traits.Position.y, v.Traits.Position.z);
                }  
                GL.End(); 
            }
            
            GL.Flush();

        }


        

    }
}
