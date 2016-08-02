#region --- License ---
/* Copyright (c) 2006, 2007 Stefanos Apostolopoulos
 * See license.txt for license info
 */
#endregion


using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using System.Runtime.InteropServices;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public class Floor : Shape
    {

        public Floor(float size)
        {
            Vertices = new Vector3[]
            {
                new Vector3(-size,-1.3f, -size),
                new Vector3(-size, -1.3f, size),
                new Vector3( size, -1.3f, size),
                new Vector3( size,-1.3f, -size),
               
            };


            Texcoords = new Vector2[]
            {
                new Vector2(0.0f, 1.0f ),
                new Vector2(0.0f,  0.0f ),                
                new Vector2( 1.0f, 0.0f ),
                new Vector2( 1.0f, 1.0f ),
                
            };

            Normals = new Vector3[]
            {
                new Vector3(0.0f, 1.0f,  0.0f),
                new Vector3( 0.0f,1.0f,  0.0f),
                new Vector3( 0.0f,  1.0f,  0.0f),
                new Vector3(0.0f, 1.0f,  0.0f),
               
            };


        }

        public void Init()
        {
           

        }

        public override void Draw()
        {

            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Disable(EnableCap.CullFace);
            // GL.Disable(EnableCap.Normalize);
            GL.Enable(EnableCap.Normalize);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, OpenGLManager.Instance.FirstTexture);

            SetUpTextureParameter();

            Color c = GlobalSetting.DisplaySetting.MeshColor;
            GL.Color3(c.R, c.G, c.B);

            GL.Begin(BeginMode.Quads);


            GL.TexCoord2(Texcoords[0].X, Texcoords[0].Y);
            GL.Normal3(Normals[0].X, Normals[0].Y, Normals[0].Z);
            GL.Vertex3(Vertices[0].X, Vertices[0].Y, Vertices[0].Z);

            GL.TexCoord2(Texcoords[1].X, Texcoords[1].Y);
            GL.Normal3(Normals[0].X, Normals[0].Y, Normals[0].Z);
            GL.Vertex3(Vertices[1].X, Vertices[1].Y, Vertices[1].Z);

            GL.TexCoord2(Texcoords[2].X, Texcoords[2].Y);
            GL.Normal3(Normals[0].X, Normals[0].Y, Normals[0].Z);
            GL.Vertex3(Vertices[2].X, Vertices[2].Y, Vertices[2].Z);

            GL.TexCoord2(Texcoords[3].X, Texcoords[3].Y);
            GL.Normal3(Normals[0].X, Normals[0].Y, Normals[0].Z);
            GL.Vertex3(Vertices[3].X, Vertices[3].Y, Vertices[3].Z);



            GL.End();
        }

        public void Draw2()
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Disable(EnableCap.CullFace);

            GL.Enable(EnableCap.Normalize);
            GL.Enable(EnableCap.Texture2D);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, OpenGLManager.Instance.FirstTexture);

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, OpenGLManager.Instance.SecondTexture);

            Color c = GlobalSetting.DisplaySetting.MeshColor;
            GL.Color3(c.R, c.G, c.B);



            GL.Begin(BeginMode.Quads);


            GL.MultiTexCoord2(TextureUnit.Texture0, Texcoords[0].X, Texcoords[0].Y);
            GL.MultiTexCoord2(TextureUnit.Texture1, Texcoords[0].X, Texcoords[0].Y);
            GL.Normal3(Normals[0].X, Normals[0].Y, Normals[0].Z);
            GL.Vertex3(Vertices[0].X, Vertices[0].Y, Vertices[0].Z);

            GL.MultiTexCoord2(TextureUnit.Texture0, Texcoords[1].X, Texcoords[1].Y);
            GL.MultiTexCoord2(TextureUnit.Texture1, Texcoords[1].X, Texcoords[1].Y);
            GL.Normal3(Normals[0].X, Normals[0].Y, Normals[0].Z);
            GL.Vertex3(Vertices[1].X, Vertices[1].Y, Vertices[1].Z);

            GL.MultiTexCoord2(TextureUnit.Texture0, Texcoords[2].X, Texcoords[2].Y);
            GL.MultiTexCoord2(TextureUnit.Texture1, Texcoords[2].X, Texcoords[2].Y);
            GL.Normal3(Normals[0].X, Normals[0].Y, Normals[0].Z);
            GL.Vertex3(Vertices[2].X, Vertices[2].Y, Vertices[2].Z);

            GL.MultiTexCoord2(TextureUnit.Texture0, Texcoords[3].X, Texcoords[3].Y);
            GL.MultiTexCoord2(TextureUnit.Texture1, Texcoords[3].X, Texcoords[3].Y);
            GL.Normal3(Normals[0].X, Normals[0].Y, Normals[0].Z);
            GL.Vertex3(Vertices[3].X, Vertices[3].Y, Vertices[3].Z);



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
