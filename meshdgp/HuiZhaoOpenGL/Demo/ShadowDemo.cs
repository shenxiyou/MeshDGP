using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public class ShadowDemo
    {
        public readonly static ShadowDemo Instance;

        static ShadowDemo()
        {
            Instance = new ShadowDemo();
        }

        //TriMesh cube;
        TriMesh sphere;
        //TriMesh bunny;

        ShadowDemo()
        {
            //this.cube = TriMeshIO.ReadFile("cube.obj");
            //foreach (var v in this.cube.Vertices)
            //{
            //    Vector3D p = v.Traits.Position;
            //    v.Traits.Position = new Vector3D(p.x, p.y, p.z);
            //}
            this.sphere = TriMeshIO.ReadFile("sphere.obj");
            TriMeshUtil.ScaleToUnit(this.sphere, 0.1);
            //this.bunny = TriMeshIO.ReadFile("bunny.obj");
            //TriMeshUtil.ScaleToUnit(this.bunny, 0.3);
        }

        public void Draw(TriMesh front, TriMesh back, Vector3D light)
        {
            Vector4d light4d = new Vector4d(light.x, light.y, light.z, 0);
            Vector3D frontPos = new Vector3D(0.5, 0.5, 0.8);
            Vector3D backPos = new Vector3D(0, 0, 0);            

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Light(LightName.Light0, LightParameter.Diffuse, Color.White);
            GL.Light(LightName.Light0, LightParameter.Position, (Vector4)light4d);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.StencilTest);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
            GL.StencilFunc(StencilFunction.Always, 1, 1);
            GL.Clear(ClearBufferMask.StencilBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            Matrix4d m = Matrix4d.LookAt(light4d.Xyz, Vector3d.Zero, Vector3d.UnitY);
            GL.MultMatrix(ref m);

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, Color.Red);
            GL.ColorMask(false, false, false, false);
            DrawMesh(front, frontPos);
            GL.ColorMask(true, true, true, true);
            GL.PopMatrix();
            
            GL.StencilFunc(StencilFunction.Equal, 1, 1);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, Color.Black);
            DrawMesh(back, backPos);

            GL.StencilFunc(StencilFunction.Equal, 0, 1);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, Color.CornflowerBlue);
            DrawMesh(back, backPos);
            GL.Disable(EnableCap.StencilTest);
            
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, Color.CornflowerBlue);
            DrawMesh(front, frontPos);

            GL.Material(MaterialFace.FrontAndBack, 
                MaterialParameter.Diffuse, Color.Red);
            DrawMesh(this.sphere, light);

            GL.Disable(EnableCap.DepthTest);
        }

        public static void DrawMesh(TriMesh mesh, Vector3D move)
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

        void Void()
        {
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Greater, 0.5f);
        }

        //public static Vector3d ComputeViewPoint()
        //{
        //    Matrix4D m = ToolPool.Instance.Tool.MVP.ViewMatrix
        //              * ToolPool.Instance.Tool.Ball.GetMatrix()
        //              * ToolPool.Instance.Tool.MVP.ModelMatrix;
        //    Vector4D view = new Vector4D(0, 0, 4, 1);
        //    view = m * view;
        //    return new Vector3d(view.x, view.y, view.z);
        //}

    }
}
