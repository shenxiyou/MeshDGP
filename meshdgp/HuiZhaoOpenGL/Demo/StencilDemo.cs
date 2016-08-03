using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public class StencilDemo
    {
        public readonly static StencilDemo Instance;

        static StencilDemo()
        {
            Instance = new StencilDemo();
        }

        TriMesh cube;
        TriMesh sphere;
        TriMesh bunny;

        StencilDemo()
        {
            this.cube = TriMeshIO.ReadFile("cube.obj");
            foreach (var v in this.cube.Vertices)
            {
                Vector3D p = v.Traits.Position;
                v.Traits.Position = new Vector3D(p.x * 2, p.y * 0.01, p.z * 2);
            }
            this.sphere = TriMeshIO.ReadFile("sphere.obj");
            this.bunny = TriMeshIO.ReadFile("bunny.obj");
        }

       
      

        public void Draw(TriMesh mesh)
        {
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.StencilTest);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
            GL.StencilFunc(StencilFunction.Always, 1, 1);
            GL.Clear(ClearBufferMask.StencilBufferBit);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.OneMinusDstAlpha, BlendingFactorDest.DstAlpha);
            Vector4 color = new Vector4(0.3f, 0.3f, 0.3f, 0.8f);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, color);
            DrawMesh(this.cube, new Vector3D(0, 0, 0));

            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.StencilFunc(StencilFunction.Equal, 1, 1);

            GL.PushMatrix();
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Scale(1, -1, 1);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, Color.CornflowerBlue);
            DrawMesh(mesh, new Vector3D(0, 0.5, 0));
            GL.PopMatrix();

            GL.Disable(EnableCap.StencilTest);
            GL.Disable(EnableCap.Blend);

            GL.FrontFace(FrontFaceDirection.Ccw);
            DrawMesh(mesh, new Vector3D(0, 0.5, 0));

            GL.Disable(EnableCap.DepthTest);
        }

        private void DrawMesh(TriMesh mesh, Vector3D move)
        {
            GL.Begin(BeginMode.Triangles);

            for (int i = 0; i < 3; i++)
            {
                foreach (var face in mesh.Faces)
                {
                    foreach (var v in face.Vertices)
                    {
                        GL.Normal3(v.Traits.Normal.ToArray());
                        GL.Vertex3((v.Traits.Position + move).ToArray());
                    }
                }
            }

            GL.End();
        }

    }
}
