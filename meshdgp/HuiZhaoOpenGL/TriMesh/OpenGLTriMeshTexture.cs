using System;
using System.Collections.Generic;
using System.Drawing;
 
 
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public partial class OpenGLTriMesh
    {
        

        public void DrawTextureShaded(TriMesh mesh)
        {

            if (GlobalSetting.TextureSetting.EdgeUV == EnumTexture.Edge)
            {
                DrawEdgeUV(mesh);
            }
            else if (GlobalSetting.TextureSetting.EdgeUV == EnumTexture.Vertex)
            {
                DrawVertexUV(mesh);
            }
            else if (GlobalSetting.TextureSetting.EdgeUV == EnumTexture.Multi)
            {
                DrawMultiTextureShaded(mesh);
            }

            else if (GlobalSetting.TextureSetting.EdgeUV == EnumTexture.Double)
            {
                DrawDoubleSidedTexture(mesh);
            }
            
        }

       


        public void DrawMultiTextureShaded(TriMesh mesh)
        {


            OpenGLManager.Instance.EnableTwoTexture();

            DrawMultiTextureUV(mesh);

           
        }

       

        private static void DrawMultiTextureUV(TriMesh mesh)
        {
            GL.Begin(BeginMode.Triangles);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                foreach (TriMesh.Vertex vertex in mesh.Faces[i].Vertices)
                {
                    GL.Normal3(vertex.Traits.Normal.x, 
                               vertex.Traits.Normal.y, 
                               vertex.Traits.Normal.z); 

                    GL.MultiTexCoord2(TextureUnit.Texture0, 
                                      vertex.Traits.UV[0], 
                                      vertex.Traits.UV[1]);
                    GL.MultiTexCoord2(TextureUnit.Texture1, 
                                      vertex.Traits.UV[0], 
                                      vertex.Traits.UV[1]); 
                    
                    GL.Vertex3(vertex.Traits.Position.x, 
                               vertex.Traits.Position.y, 
                               vertex.Traits.Position.z);
                }
            }
            GL.End();
        }

 
         


        private   void DrawEdgeUV(TriMesh mesh)
        {
            GL.Begin(BeginMode.Triangles);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                foreach (TriMesh.Vertex vertex in mesh.Faces[i].Vertices)
                {
                    GL.Normal3(vertex.Traits.Normal.x, vertex.Traits.Normal.y, vertex.Traits.Normal.z);
                    TriMesh.HalfEdge halfedge = mesh.Faces[i].FindHalfedgeTo(vertex);
                    if (halfedge != null)
                    {
                        GL.TexCoord2(halfedge.Traits.TextureCoordinate.x, 1 - halfedge.Traits.TextureCoordinate.y);
                    }
                    GL.Vertex3(vertex.Traits.Position.x, vertex.Traits.Position.y, vertex.Traits.Position.z);
                }
            }
            GL.End();
        }

        public void DrawVertexUV(TriMesh mesh)
        {
            GL.Begin(BeginMode.Triangles);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                foreach (TriMesh.Vertex vertex in mesh.Faces[i].Vertices)
                {
                    GL.Normal3(vertex.Traits.Normal.x, 
                               vertex.Traits.Normal.y, 
                               vertex.Traits.Normal.z);
                    GL.TexCoord2(vertex.Traits.UV[0], vertex.Traits.UV[1]);
                    GL.Vertex3(vertex.Traits.Position.x, 
                               vertex.Traits.Position.y,
                               vertex.Traits.Position.z);
                }
            }
            GL.End();
        }


        public void DrawDoubleSidedTexture(TriMesh mesh)
        {
            if (OpenGLManager.Instance.TextureList.Count >= 2)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, OpenGLManager.Instance.TextureList[0].ID);
                GL.FrontFace(FrontFaceDirection.Cw);
                DrawVertexUV(mesh);

                GL.ActiveTexture(TextureUnit.Texture1);
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, OpenGLManager.Instance.TextureList[1].ID);
                GL.FrontFace(FrontFaceDirection.Ccw);
                DrawVertexUV(mesh);
            }
            
        }
    }
}
