using System;
using System.Collections.Generic;
using System.Drawing;
 
 
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public partial class OpenGLTriMesh
    {


        public void DrawConnectionVector(TriMesh m)
        {
            for (int i = 0; i < m.Faces.Count; i++)
            {
                GL.Color3(0.0f, 0.0f, 1.0f);

                Vector3D faceCenter = new Vector3D();

                //Get Face center point
                faceCenter.x = (m.Faces[i].GetVertex(0).Traits.Position.x +
                                  m.Faces[i].GetVertex(1).Traits.Position.x +
                                  m.Faces[i].GetVertex(2).Traits.Position.x) / 3;

                faceCenter.y = (m.Faces[i].GetVertex(0).Traits.Position.y +
                                  m.Faces[i].GetVertex(1).Traits.Position.y +
                                  m.Faces[i].GetVertex(2).Traits.Position.y) / 3;

                faceCenter.z = (m.Faces[i].GetVertex(0).Traits.Position.z +
                                  m.Faces[i].GetVertex(1).Traits.Position.z +
                                  m.Faces[i].GetVertex(2).Traits.Position.z) / 3;

                //Face Normal Vector
               // Vector3D vector = m.Faces[i].faceVector.Normalize();

                GL.Begin(BeginMode.Lines);

                GL.Color3(Color.Black);

                GL.Vertex3(faceCenter.x, faceCenter.y, faceCenter.z);
               // GL.Vertex3(faceCenter.x - 0.2 * vector.x, faceCenter.y - 0.2 * vector.y, faceCenter.z - 0.2 * vector.z);

                GL.End();

            }
        }


        public void DrawDeformationBox(TriMesh box)
        {
            DrawSelectedVerticeBySphere(box);
            DrawCageWireFrame(box);            
        }


        public void DrawFFDGrid(TriMesh m, List<List<int>> lines)
        {
            DrawSelectedVerticeBySphere(m);


            GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, GlobalSetting.DisplaySetting.BoundaryColor);
            GL.LineWidth(0.1f);

            for (int i = 0; i < lines.Count; i++)
            {
                GL.Begin(BeginMode.LineStrip);
                OpenTK.Graphics.Color4 color = new OpenTK.Graphics.Color4(0, 0, 0, 0); 
                GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, color);
                for (int j = 0; j < lines[i].Count; j++)
                {
                    GL.Vertex3(m.Vertices[lines[i][j]].Traits.Position.x,
                               m.Vertices[lines[i][j]].Traits.Position.y,
                               m.Vertices[lines[i][j]].Traits.Position.z);
                }
                GL.End();

            }

        }
     

        public void DrawSkeleton(List<TriMesh> mesh)
        {
            for (int i = 0; i < mesh.Count; i++)
            {
                DrawTriMesh(mesh[i]);
            }

        }

        public void DrawSmoothShadedOne(TriMesh mesh)
        {


            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            GL.Enable(EnableCap.Normalize);

            Color c = GlobalSetting.DisplaySetting.MeshColor;
            GL.Color3(c);

            GL.Begin(BeginMode.Triangles);

            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                foreach (TriMesh.Vertex vertex in mesh.Faces[i].Vertices)
                {
                    GL.Normal3(vertex.Traits.Normal.x, vertex.Traits.Normal.y, vertex.Traits.Normal.z);
                    GL.Vertex3(vertex.Traits.Position.x, vertex.Traits.Position.y, vertex.Traits.Position.z);

                }
            }
            GL.End();

            GL.Flush();

        }

    }
}
