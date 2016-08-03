using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace GraphicResearchHuiZhao 
{
    public partial class OpenGLTriMesh
    {

        public  void DrawAntiAliasPonit(TriMesh mesh)
        {
            GL.Enable(EnableCap.PointSmooth);
            GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Point);
            GL.PointSize(GlobalSetting.DisplaySetting.PointSize);
            GL.Color4(GlobalSetting.DisplaySetting.MeshColor);

            GL.Begin(BeginMode.Points);
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                GL.Normal3(mesh.Vertices[i].Traits.Normal.x,
                           mesh.Vertices[i].Traits.Normal.y,
                           mesh.Vertices[i].Traits.Normal.z);
                GL.Vertex3(mesh.Vertices[i].Traits.Position.x,
                           mesh.Vertices[i].Traits.Position.y,
                           mesh.Vertices[i].Traits.Position.z);
            }
            GL.End();
         
        }

        public void DrawAntiAliasLine(TriMesh mesh)
        {
            GL.Enable(EnableCap.LineSmooth);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.LineWidth(GlobalSetting.DisplaySetting.LineWidth);
            GL.Enable(EnableCap.CullFace);
            GL.Color4(GlobalSetting.DisplaySetting.MeshColor);

            GL.Begin(BeginMode.Triangles);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                foreach (TriMesh.Vertex vertex in mesh.Faces[i].Vertices)
                {
                    GL.Normal3(vertex.Traits.Normal.x,
                               vertex.Traits.Normal.y,
                               vertex.Traits.Normal.z);
                    GL.Vertex3(vertex.Traits.Position.x,
                               vertex.Traits.Position.y,
                               vertex.Traits.Position.z);

                }
            }
            GL.End();
        }
   



        public void DrawAccum(TriMesh mesh)
        {
            GL.Accum(AccumOp.Load, 0);
            for (int i = 0; i < 20; i++)
            {
                DrawAccum(mesh, new Vector3D(i / 20d, 0, 0));
                GL.Accum(AccumOp.Accum, 1f / 20);
            }
            GL.Accum(AccumOp.Return, 1f);
        }

      

        public void DrawAccumAdd(TriMesh mesh)
        {
         
            DrawAccumAdd(mesh,Color.Red, Vector3D.Zero);
            GL.Accum(AccumOp.Load, -0.2f);
            DrawAccumAdd(mesh, Color.Blue, new Vector3D(0.1, 0.1, 0.2));
            GL.Accum(AccumOp.Accum, 0.6f);
            DrawAccumAdd(mesh, Color.Green, new Vector3D(0.2, 0.2, 0.4));
            GL.Accum(AccumOp.Accum, -0.2f);
            DrawAccumAdd(mesh, Color.Yellow, new Vector3D(0.3, 0.3, 0.6));
            GL.Accum(AccumOp.Accum, 0.6f);
            GL.Accum(AccumOp.Return, 1f);
        }

        private void DrawAccumAdd(TriMesh mesh, Color color, Vector3D offset)
        {
            DrawAccumAdd(mesh, 1, color, offset);
        }

        private  void DrawAccumAdd(TriMesh mesh,float alpha, Color color, Vector3D offset)
        {
            GL.Disable(EnableCap.ColorMaterial);
            OpenTK.Graphics.Color4 c = color;
            c.A = alpha;
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, c);

            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.Normalize);

            GL.Begin(BeginMode.Triangles);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                foreach (TriMesh.Vertex vertex in mesh.Faces[i].Vertices)
                {
                    GL.Normal3(vertex.Traits.Normal.ToArray());
                    GL.Vertex3((vertex.Traits.Position + offset).ToArray());
                }
            }
            GL.End();
        }


        public void DrawStencil(TriMesh mesh)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.StencilTest);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
            GL.StencilFunc(StencilFunction.Always, 1, 1);
            GL.Clear(ClearBufferMask.StencilBufferBit);

            GL.ColorMask(false, false, false, false);
            DrawStencilMesh(GlobalData.Instance.MeshTwo, new Vector3D(-0.2, 0, 0));

            GL.StencilFunc(StencilFunction.Equal, 1, 1);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            GL.ColorMask(true, true, true, true);
            DrawStencilMesh(mesh, new Vector3D(0.2, 0, 0));

            GL.Disable(EnableCap.StencilTest);
            GL.Disable(EnableCap.DepthTest);
        }

        private void DrawStencilMesh(TriMesh mesh, Vector3D move)
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

 


        public void DrawAccum(TriMesh mesh, Vector3D move)
        {
            GL.Begin(BeginMode.Triangles);

            foreach (var face in mesh.Faces)
            {
                foreach (var v in face.Vertices)
                {
                    GL.Normal3(v.Traits.Normal.ToArray());
                    GL.Vertex3((v.Traits.Position + move).ToArray());
                }
            }

            GL.End();
        }


        public void DrawStencilNO(TriMesh mesh)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            DrawStencilMesh(GlobalData.Instance.MeshTwo, new Vector3D(-0.2, 0, 0));
            DrawStencilMesh(mesh, new Vector3D(0.2, 0, 0));

            GL.Disable(EnableCap.DepthTest);
        }

        public void DrawTransparentBasic(TriMesh mesh)
        {
            GlobalSetting.SettingLight.DoubleSideLight = true;
            float alpha = GlobalSetting.MaterialSetting.MaterialDiffuse.A / 256f;
            //GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            DrawTransparent(mesh,alpha);
        }

        public void DrawTransparentTwo(TriMesh mesh)
        {            
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, 
                BlendingFactorDest.OneMinusSrcAlpha);
            DrawSmoothShaded(mesh);
            GL.DepthMask(false);
            OpenGLManager.Instance.SetColor(
                GlobalSetting.DisplaySetting.BoundaryColor);
            DrawSmoothHiddenLine(GlobalData.Instance.MeshTwo);            
            GL.Disable(EnableCap.Blend);
            GL.DepthMask(true);
            GL.Disable(EnableCap.DepthTest);     
        }


        public void DrawTransparent(TriMesh mesh)
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
            DrawTransparent(mesh, f * alpha);
            GL.DepthFunc(DepthFunction.Lequal);
            DrawTransparent(mesh, (alpha - f * alpha) / (1f - f * alpha));

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.DepthFunc(DepthFunction.Always);
            DrawTransparent(mesh, f * alpha);
            GL.DepthFunc(DepthFunction.Lequal);
            DrawTransparent(mesh, (alpha - f * alpha) / (1f - f * alpha));

            GL.Disable(EnableCap.Blend);
            GL.DepthMask(true);
            GL.Disable(EnableCap.DepthTest);
        }

        private void DrawTransparent(TriMesh mesh, float alpha)
        {
            OpenTK.Graphics.Color4 front =
                GlobalSetting.MaterialSetting.MaterialDiffuse;
            front.A = alpha;
            OpenTK.Graphics.Color4 back =
                GlobalSetting.MaterialSetting.BackMaterialDiffuse;
            back.A = alpha;
            GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, front);
            GL.Material(MaterialFace.Back, MaterialParameter.Diffuse, back);
            DrawTransparentMesh(mesh);
        }

        private void DrawTransparentMesh(TriMesh mesh)
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


        public void DrawMirror(TriMesh mesh)
        {
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.StencilTest);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, 
                         StencilOp.Replace);
            GL.StencilFunc(StencilFunction.Always, 1, 1);
            GL.Clear(ClearBufferMask.StencilBufferBit);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.OneMinusDstAlpha, 
                         BlendingFactorDest.DstAlpha); 
            OpenGLManager.Instance.SetColorMaterial(
                GlobalSetting.DisplaySetting.WifeFrameColor);
         
            DrawStencilMesh(GlobalData.Instance.MeshTwo, new Vector3D(0, 0, 0));

            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.StencilFunc(StencilFunction.Equal, 1, 1);

            GL.PushMatrix();
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Scale(1, -1, 1);     
            OpenGLManager.Instance.SetColorMaterial(
                GlobalSetting.MaterialSetting.MaterialDiffuse);
            DrawStencilMesh(mesh, new Vector3D(0, 0.5, 0));
            GL.PopMatrix();

            GL.Disable(EnableCap.StencilTest);
            GL.Disable(EnableCap.Blend);
            GL.FrontFace(FrontFaceDirection.Ccw);
            DrawStencilMesh(mesh, new Vector3D(0, 0.5, 0));
            GL.Disable(EnableCap.DepthTest);
        }


        public void DrawShadow(TriMesh mesh)
        {
            DrawShadow(mesh, GlobalData.Instance.MeshTwo, GlobalSetting.Light0Setting.LightPosition);
        }

        public void DrawShadow(TriMesh front, TriMesh back, Vector3D light)
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
            DrawStencilMesh(front, frontPos);
            GL.ColorMask(true, true, true, true);
            GL.PopMatrix();

            GL.StencilFunc(StencilFunction.Equal, 1, 1);
            OpenGLManager.Instance.SetColorMaterial(
                GlobalSetting.DisplaySetting.WifeFrameColor);
            DrawStencilMesh(back, backPos);

            GL.StencilFunc(StencilFunction.Equal, 0, 1);
            OpenGLManager.Instance.SetColorMaterial(
                 GlobalSetting.MaterialSetting.BackMaterialDiffuse);
            DrawStencilMesh(back, backPos);
            GL.Disable(EnableCap.StencilTest);

            OpenGLManager.Instance.SetColorMaterial(
                GlobalSetting.MaterialSetting.MaterialDiffuse);
            DrawStencilMesh(front, frontPos);
          
            GL.Disable(EnableCap.DepthTest);
        }


     

    }
}
