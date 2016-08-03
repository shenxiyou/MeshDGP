using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public class TeapotDemo
    {
        public static readonly TeapotDemo Instance;

        static TeapotDemo()
        {
            Instance = new TeapotDemo();
        }

        TriMesh mesh;

        TeapotDemo()
        {
            this.mesh = TriMeshIO.ReadFile("teapot.obj");
            TriMeshUtil.SetUpNormalVertex(this.mesh);
        }

       

        public void Draw()
        {
            GlobalSetting.SettingLight.DoubleSideLight = true;
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, 
                BlendingFactorDest.OneMinusSrcAlpha);
            GL.Disable(EnableCap.ColorMaterial);

            float f = 1 - GlobalSetting.MaterialSetting.MaterialDiffuse.A / 256f;
            float alpha = GlobalSetting.MaterialSetting.MaterialDiffuse.A / 256f;

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);
            GL.DepthFunc(DepthFunction.Always);
            this.DrawTeapot(f * alpha);
            GL.DepthFunc(DepthFunction.Lequal);
            this.DrawTeapot((alpha - f * alpha) / (1f - f * alpha));

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.DepthFunc(DepthFunction.Always);
            this.DrawTeapot(f * alpha);
            GL.DepthFunc(DepthFunction.Lequal);
            this.DrawTeapot((alpha - f * alpha) / (1f - f * alpha));

            GL.Disable(EnableCap.Blend);
            GL.DepthMask(true);
            GL.Disable(EnableCap.DepthTest);
        }

        public void DrawTeapot(float alpha)
        {
            OpenTK.Graphics.Color4 front = 
                GlobalSetting.MaterialSetting.MaterialDiffuse;
            front.A = alpha;
            OpenTK.Graphics.Color4 back = 
                GlobalSetting.MaterialSetting.BackMaterialDiffuse;
            back.A = alpha;
            GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, front);
            GL.Material(MaterialFace.Back, MaterialParameter.Diffuse, back);
            DrawMesh(this.mesh);
        }

        static void DrawMesh(TriMesh mesh)
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.Normalize);

            GL.Begin(BeginMode.Triangles);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                foreach (TriMesh.Vertex vertex in mesh.Faces[i].Vertices)
                {
                    GL.Normal3(vertex.Traits.Normal.ToArray());
                    GL.Vertex3(vertex.Traits.Position.ToArray());
                }
            }
            GL.End();
        }
    }
}
