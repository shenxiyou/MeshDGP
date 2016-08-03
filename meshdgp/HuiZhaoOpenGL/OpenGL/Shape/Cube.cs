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
    public class Cube : Shape
    {
        public Cube()
        {
            Vertices = new Vector3[]
            {
                new Vector3(-1.0f, -1.0f,  1.0f),
                new Vector3( 1.0f, -1.0f,  1.0f),
                new Vector3( 1.0f,  1.0f,  1.0f),
                new Vector3(-1.0f,  1.0f,  1.0f),
                new Vector3(-1.0f, -1.0f, -1.0f),
                new Vector3( 1.0f, -1.0f, -1.0f), 
                new Vector3( 1.0f,  1.0f, -1.0f),
                new Vector3(-1.0f,  1.0f, -1.0f)
            };

            Indices = new int[]
            {
                // front face
                0, 1, 2, 2, 3, 0,
                // top face
                3, 2, 6, 6, 7, 3,
                // back face
                7, 6, 5, 5, 4, 7,
                // left face
                4, 0, 3, 3, 7, 4,
                // bottom face
                0, 1, 5, 5, 4, 0,
                // right face
                1, 5, 6, 6, 2, 1,
            };

            Normals = new Vector3[]
            {
                new Vector3(-1.0f, -1.0f,  1.0f),
                new Vector3( 1.0f, -1.0f,  1.0f),
                new Vector3( 1.0f,  1.0f,  1.0f),
                new Vector3(-1.0f,  1.0f,  1.0f),
                new Vector3(-1.0f, -1.0f, -1.0f),
                new Vector3( 1.0f, -1.0f, -1.0f),
                new Vector3( 1.0f,  1.0f, -1.0f),
                new Vector3(-1.0f,  1.0f, -1.0f),
            };

            //Colors = new int[]
            //{
            //    Utilities.ColorToRgba32(Color.DarkRed),
            //    Utilities.ColorToRgba32(Color.DarkRed),
            //    Utilities.ColorToRgba32(Color.Gold),
            //    Utilities.ColorToRgba32(Color.Gold),
            //    Utilities.ColorToRgba32(Color.DarkRed),
            //    Utilities.ColorToRgba32(Color.DarkRed),
            //    Utilities.ColorToRgba32(Color.Gold),
            //    Utilities.ColorToRgba32(Color.Gold),
            //};


            Colors = new Color[]
            {
                 Color.DarkRed ,
                 Color.DarkRed ,
                 Color.Gold ,
                 Color.Gold,
                 Color.DarkRed,
                 Color.DarkRed,
                 Color.Gold,
                 Color.Gold,
            };
        }

            public override   void Draw()
            {
                GL.ShadeModel(ShadingModel.Smooth);
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                GL.Disable(EnableCap.CullFace);
         
                GL.Enable(EnableCap.Normalize);

                Color c = GlobalSetting.DisplaySetting.MeshColor;
                GL.Color3(c.R, c.G, c.B);

                GL.Begin(BeginMode.Triangles);
                for (int i = 0; i < Indices.Length; i++)
                {
                    GL.Color3( Colors[Indices[i]]);
                    GL.Normal3(Normals[Indices[i]].X, Normals[Indices[i]].Y, Normals[Indices[i]].Z);
                    GL.Vertex3(Vertices[Indices[i]].X, Vertices[Indices[i]].Y, Vertices[Indices[i]].Z);
                }

                GL.End();
            }
        
    }
}
