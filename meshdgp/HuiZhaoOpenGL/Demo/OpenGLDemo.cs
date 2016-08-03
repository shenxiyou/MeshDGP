using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao 
{
    public class OpenGLDemo
    {
        private static OpenGLDemo instance = null;
        public static OpenGLDemo Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OpenGLDemo();
                }
                return instance;
            }
        }

        public BeginMode beginMode;
        public ShadingModel shadingModel;
        public MaterialFace polygonMode;

        public void Draw(TriMesh mesh)
        {
            switch (GlobalSetting.DisplaySetting.OpenGLDemo)
            {
                
               
              
                case EnumOpenGLDemo.Demo3:
                    ShadowDemo.Instance.Draw(
                        GlobalData.Instance.MeshOne, 
                        GlobalData.Instance.MeshTwo,
                        GlobalSetting.Light0Setting.LightPosition);
                    break;
             
                    break;
                case EnumOpenGLDemo.Light:
                    LightDemo.Instance.Draw();
                    break;
                
         
            }
        }

        TriMesh bunny;
        TriMesh box;
        TriMesh[] sphere;

        public  OpenGLDemo()
        {
            //this.bunny = TriMeshIO.ReadFile("bunny.obj");
            //TriMeshUtil.SetUpNormalVertex(this.bunny);
            //TriMeshUtil.ScaleToUnit(this.bunny, 0.3);

            //this.box = TriMeshIO.ReadFile("teapot.obj");
            //TriMeshUtil.SetUpNormalVertex(this.box);

            //this.sphere = new TriMesh[]{
            //    TriMeshIO.ReadFile("sphere2048.obj"),
            //    TriMeshIO.ReadFile("sphere2048.obj"),
            //    TriMeshIO.ReadFile("sphere2048.obj")};
            //foreach (var item in this.sphere)
            //{
            //    TriMeshUtil.SetUpNormalVertex(item);
            //}
            //TriMeshUtil.ScaleToUnit(sphere[1], 0.5);
            //TriMeshUtil.ScaleToUnit(sphere[2], 1.2);
        }

        //public void DemoOne()
        //{
        //    GL.Enable(EnableCap.ColorMaterial);
        //    GL.ColorMaterial(MaterialFace.Front, ColorMaterialParameter.Diffuse);
        //    GL.Begin(BeginMode.Points);
        //    GL.Color3(0.0, 1.0, 0.0);
        //    GL.Vertex3(0.0, 0.0, 0.0);
        //    GL.Color3(1.0, 1.0, 0.0);
        //    GL.Vertex3(0.0, 0.3, 0.0);
        //    GL.Color3(0.0, 1.0, 1.0);
        //    GL.Vertex3(0.3, 0.3, 0.0);
        //    GL.Vertex3(0.3, 0.0, 0.0);
        //    GL.End();
        //}

        public void DemoOne()
        { 
            GL.ShadeModel(this.shadingModel);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.ColorMaterial);

            GL.Begin(this.beginMode);
            double r = 0.3;
            int n = 6;
            for (int i = 0; i < n; i++)
            {
                double x = r * Math.Cos(Math.PI * 2 / n * i);
                double y = r * Math.Sin(Math.PI * 2 / n * i);
                int color = i + 1;
                GL.Color3((float)(color / 4), (float)(color / 2 % 2), (float)(color % 2));
                GL.Vertex3(x, y, 0);
            }
            GL.End();
        }

        public void Demo()
        {
            GL.ShadeModel(this.shadingModel);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.ColorMaterial);

            GL.Begin(this.beginMode);
            double r = 0.3;
            GL.Color3(0f, 0f, 1f);
            GL.Vertex3(-r, r, 0);
            GL.Color3(0f, 1f, 0f);
            GL.Vertex3(-r, -r, 0);
            GL.Color3(0f, 1f, 1f);
            GL.Vertex3(0, r, 0);
            GL.Color3(1f, 0f, 0f);
            GL.Vertex3(0, -r, 0);
            GL.Color3(1f, 0f, 1f);
            GL.Vertex3(r, r, 0);
            GL.Color3(1f, 1f, 0f);
            GL.Vertex3(r, -r, 0);
            GL.End();
        }

       

        public void DemoTwo()
        {
            GL.Enable(EnableCap.ColorMaterial);
            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.Diffuse);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(false);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
             
            this.DrawSphere();

            GL.Disable(EnableCap.Blend);
            GL.DepthMask(true);
            GL.Disable(EnableCap.DepthTest);
        }

        public void DemoThree()
        {
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            this.DrawBunny();
            GL.DepthMask(false);
            TeapotDemo.Instance.Draw();
            GL.Disable(EnableCap.Blend);
            GL.DepthMask(true);
            GL.Disable(EnableCap.DepthTest);
        }

        void DrawBunny()
        {
            OpenTK.Graphics.Color4 color = GlobalSetting.MaterialSetting.BackMaterialDiffuse;
            GL.Color4(color);

            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.Normalize);

            OpenGLTriMesh.Instance.DrawTriangles(this.bunny);
        }

        void DrawBox()
        {
            OpenTK.Graphics.Color4 color = GlobalSetting.MaterialSetting.MaterialDiffuse;
            GL.Color4(color);

            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.Normalize);

            OpenGLTriMesh.Instance.DrawTriangles(this.box);
            OpenGLTriMesh.Instance.DrawWireFrameGray(this.box);
        }

        void DrawSphere()
        {
            Vector3D[] move = new Vector3D[]{
                new Vector3D(0,0,1),
                new Vector3D(0.3,0,0),
                new Vector3D(-0.3,0,-1)};

            OpenTK.Graphics.Color4[] colors = new OpenTK.Graphics.Color4[]{
                OpenTK.Graphics.Color4.Blue,
                OpenTK.Graphics.Color4.Yellow,
                OpenTK.Graphics.Color4.Green};

            GL.Begin(BeginMode.Triangles);

            for (int i = 0; i < 3; i++)
            {
                colors[i].A = 0.6f;
                GL.Color4(colors[i]);
                if (sphere[i] != null)
                {
                    foreach (var face in sphere[i].Faces)
                    {
                        foreach (var v in face.Vertices)
                        {
                            GL.Normal3(v.Traits.Normal.ToArray());
                            GL.Vertex3((v.Traits.Position + move[i]).ToArray());
                        }
                    }
                }
            }

            GL.End();
        }

        

        

       
    }
}
