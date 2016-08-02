using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public class TransparentDemo
    {
        public static readonly TransparentDemo Instance;

        static TransparentDemo()
        {
            Instance = new TransparentDemo();
        }

        TriMesh mesh;

        TransparentDemo()
        {
            this.mesh = TriMeshIO.ReadFile("cube.obj");
            foreach (var item in this.mesh.Vertices)
            {
                item.Traits.Position.z /= 3;
            }
        }

        public void Draw(AlphaFunction func, float value)
        {
            //GL.Disable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.CullFace);
            //GL.CullFace(CullFaceMode.Back);
            //GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, 
            //    BlendingFactorDest.OneMinusSrcAlpha);
            //GL.Enable(EnableCap.AlphaTest);
            //GL.AlphaFunc(func, value);

            float alpha = 0.25f;
            //this.DrawCube(alpha * 4, Color.Red, Vector3D.Zero);
            //this.DrawCube(alpha * 3, Color.Blue, new Vector3D(0.1, 0.1, 0.2));
            //this.DrawCube(alpha * 2, Color.Green, new Vector3D(0.2, 0.2, 0.4));
            //this.DrawCube(alpha * 1, Color.Yellow, new Vector3D(0.3, 0.3, 0.6));

            //GL.Disable(EnableCap.AlphaTest);
            //GL.Disable(EnableCap.Blend);
        }

       

       
    }
}
