
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
 

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao 
{
    public class AdPlane:Shape
    {
        public AdPlane(int x_res, int y_res, float x_scale, float y_scale)
        {
            Vertices = new Vector3[x_res * y_res];
            Normals = new Vector3[x_res * y_res];
            Indices = new int[6 * x_res * y_res];
            Texcoords = new Vector2[x_res * y_res];

            int i = 0;
            for (int y = -y_res / 2; y < y_res / 2; y++)
            {
                for (int x = -x_res / 2; x < x_res / 2; x++)
                {
                    Vertices[i].X = x_scale * (float)x / (float)x_res;
                    Vertices[i].Z = y_scale * (float)y / (float)y_res;
                    Vertices[i].Y = -0.8f;
                    Normals[i].X = Normals[i].Z = 0;
                    Normals[i].Y = 1;
                    i++;
                }
            }

            i = 0;
            for (int y = 0; y < y_res - 1; y++)
            {
                for (int x = 0; x < x_res - 1; x++)
                {
                    Indices[i++] = (y + 0) * x_res + x;
                    Indices[i++] = (y + 1) * x_res + x;
                    Indices[i++] = (y + 0) * x_res + x + 1;

                    Indices[i++] = (y + 0) * x_res + x + 1;
                    Indices[i++] = (y + 1) * x_res + x;
                    Indices[i++] = (y + 1) * x_res + x + 1;
                }
            }
        }

        public override void Draw()
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Disable(EnableCap.CullFace);

            GL.Enable(EnableCap.Normalize);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, OpenGLManager.Instance.FirstTexture);

            SetUpTextureParameter();

            Color c = GlobalSetting.DisplaySetting.MeshColor;
            GL.Color3(c.R, c.G, c.B);

            GL.Begin(BeginMode.Triangles);
            for (int i = 0; i < Indices.Length; i++)
            {
               
                GL.TexCoord2(Texcoords[Indices[i]].X, Texcoords[Indices[i]].Y);
                GL.Normal3(Normals[Indices[i]].X, Normals[Indices[i]].Y, Normals[Indices[i]].Z);
                GL.Vertex3(Vertices[Indices[i]].X, Vertices[Indices[i]].Y, Vertices[Indices[i]].Z);
            }
             

            GL.End();
        }

        public void SetUpTextureParameter()
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (int)TextureWrapMode.Repeat);

        }



       

        }
    }
 
