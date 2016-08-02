using System;
using System.Collections.Generic;
using System.Drawing;
  
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public partial class OpenGLManager
    {
       
        public   void DrawCube()
        {
            PlaneShape plane = new PlaneShape();

            plane.Draw();


        }

        public  void DrawPlane()
        {
            //if ((GlobalData.Instance.shape != null) && (GlobalData.Instance.shape is Plane))
            //{
            //    GlobalData.Instance.shape.Draw();
            //}
            //else
            //{

            //    GlobalData.Instance.shape = new Plane();
            //    GlobalData.Instance.shape.Init();
            //    GlobalData.Instance.shape.Draw();
            //}

            //GL.Begin(BeginMode.Quads);
            //{
            //    GL.Vertex3(-1.0f, -1.0f, -1.0f);
            //    GL.Vertex3(1.0f, -1.0f, -1.0f);
            //    GL.Vertex3(1.0f, 1.0f, -1.0f);
            //    GL.Vertex3(-1.0f, 1.0f, -1.0f);
            //}
            //GL.End();

       
        }
        public void DrawQinghua()
        {
            //if ((GlobalData.Instance.shape != null) && (GlobalData.Instance.shape is Qihuaci))
            //{
            //    GlobalData.Instance.shape.Draw();
            //}
            //else
            //{

            //    GlobalData.Instance.shape = new Qihuaci();
        
            //    GlobalData.Instance.shape.Draw();
            //}

          


        }

        public void DrawEightFace()
        {
            //if ((GlobalData.Instance.shape != null) && (GlobalData.Instance.shape is Zhengbamianti))
            //{
            //    GlobalData.Instance.shape.Draw();
            //}
            //else
            //{

            //    GlobalData.Instance.shape = new Zhengbamianti();

            //    GlobalData.Instance.shape.Draw();
            //}




        }

        public   void DrawFloor()
        {
            //if ((GlobalData.Instance.shape != null) && (GlobalData.Instance.shape is Floor))
            //{
            //    GlobalData.Instance.shape.Draw();
            //}
            //else
            //{

            //    GlobalData.Instance.shape = new Floor(0.5f);
            //    GlobalData.Instance.shape.Init();
            //    GlobalData.Instance.shape.Draw();
            //}

            //GL.Begin(BeginMode.Quads);
            //{
            //    GL.Vertex2(-1.0f, -1.0f);
            //    GL.Vertex2(1.0f, -1.0f);
            //    GL.Vertex2(1.0f, 1.0f);
            //    GL.Vertex2(-1.0f, 1.0f);
            //}
            //GL.End();


        }
        public   void DrawSphere2()
        {
           
            //GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactorSrc.Zero, BlendingFactorDest.SrcColor); 

            //if ((GlobalData.Instance.drawableShape != null) && (GlobalData.Instance.drawableShape is SlicedSphere))
            //{
            //    GL.Color4(1.0f, 0.0f, 0.0f, 0.5f);
            //    GlobalData.Instance.drawableShape.Draw();
            //}
            //else
            //{

            //    GlobalData.Instance.drawableShape = new SlicedSphere(0.5f, OpenTK.Vector3d.Zero, SlicedSphere.eSubdivisions.Six, new SlicedSphere.eDir[] { SlicedSphere.eDir.All });

            //    GL.ShadeModel(ShadingModel.Smooth);
            //    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            //    //GL.Enable(EnableCap.Lighting);
            //    GL.Enable(EnableCap.Normalize);

            //    Color c = GlobalSetting.MeshDisplaySetting.MeshColor;
            //    GL.Color4(1.0f, 0.0f, 0.0f, 0.5f);
        
            //    GlobalData.Instance.drawableShape.Draw();
            //}
           
        }

        public void DrawStencilShape()
        {
            InitOpenGLState();
            #region Transform setup
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            //// Camera
            //GL.MatrixMode(MatrixMode.Modelview);
            //Matrix4 mv = Matrix4.LookAt(EyePosition, Vector3.Zero, Vector3.UnitY);
            //GL.LoadMatrix(ref mv);

            //GL.Translate(0f, 0f, CameraZoom);
            //GL.Rotate(CameraRotX, Vector3.UnitY);
            //GL.Rotate(CameraRotY, Vector3.UnitX);
            #endregion Transform setup

            RenderCsg();

            // ---------------------------------

            //if (ShowDebugWireFrame)
            //{
            //    GL.Color3(System.Drawing.Color.LightGray);
            //    GL.Disable(EnableCap.StencilTest);
            //    GL.Disable(EnableCap.Lighting);
            //    //GL.Disable( EnableCap.DepthTest );
            //    GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);
            //    DrawOperandB();
            //    GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
            //    GL.Enable(EnableCap.DepthTest);
            //    GL.Enable(EnableCap.Lighting);
            //    GL.Enable(EnableCap.StencilTest);
            //}
            


        }


        private void InitOpenGLState()
        {
            #region GL States
            GL.ClearColor(.08f, .12f, .16f, 1f);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.ClearDepth(1.0);

            GL.Enable(EnableCap.StencilTest);
            GL.ClearStencil(0);
            GL.StencilMask(0xFFFFFFFF); // read&write

            GL.Enable(EnableCap.CullFace);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.CullFace(CullFaceMode.Back);

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            GL.Color4(1f, 1f, 1f, 1f);

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.ShadeModel(ShadingModel.Smooth);

            #endregion GL States

        }


        public void DrawOperandB()
        {
            GL.PushMatrix();
           
            DrawSphere2();
            GL.PopMatrix();
        }

        public void DrawOperandA()
        {
            //GL.Disable(EnableCap.Texture2D);
            //OpenGLOperators.Instance.DrawSmoothShaded(GlobalData.Instance.CurrMeshRecord.Mesh);
            
        }

        public void RenderCsg()
        {
            // first pass
            GL.Disable(EnableCap.StencilTest);

            GL.ColorMask(false, false, false, false);
            GL.CullFace(CullFaceMode.Front);
            DrawOperandB();// draw front-faces into depth buffer

            // use stencil plane to find parts of b in a 
            GL.DepthMask(false);
            GL.Enable(EnableCap.StencilTest);
            GL.StencilFunc(StencilFunction.Always, 0, 0);

            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Incr);
            GL.CullFace(CullFaceMode.Back);
            DrawOperandA(); // increment the stencil where the front face of a is drawn

            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Decr);
            GL.CullFace(CullFaceMode.Front);
            DrawOperandA(); // decrement the stencil buffer where the back face of a is drawn

            GL.DepthMask(true);
            GL.Disable(EnableCap.DepthTest);

            GL.ColorMask(true, true, true, true);
            GL.StencilFunc(StencilFunction.Notequal, 0, 1);
            DrawOperandB(); // draw the part of b that's in a

            // fix depth
            GL.ColorMask(false, false, false, false);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.StencilTest);
            GL.DepthFunc(DepthFunction.Always);
            DrawOperandA();
            GL.DepthFunc(DepthFunction.Less);

            // second pass
            GL.CullFace(CullFaceMode.Back);
            DrawOperandA();

            GL.DepthMask(false);
            GL.Enable(EnableCap.StencilTest);

            GL.StencilFunc(StencilFunction.Always, 0, 0);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Incr);
            DrawOperandB(); // increment the stencil where the front face of b is drawn

            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Decr);
            GL.CullFace(CullFaceMode.Front);
            DrawOperandB(); // decrement the stencil buffer where the back face of b is drawn

            GL.DepthMask(true);
            GL.Disable(EnableCap.DepthTest);

            GL.ColorMask(true, true, true, true);
            GL.StencilFunc(StencilFunction.Equal, 0, 1);
            GL.CullFace(CullFaceMode.Back);
            DrawOperandA(); // draw the part of a that's in b

            GL.Enable(EnableCap.DepthTest);

        }
    }
}
