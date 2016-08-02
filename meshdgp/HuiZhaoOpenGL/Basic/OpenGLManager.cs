using System;
using System.Collections.Generic;
using System.Drawing;
   
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao 
{
    public partial class OpenGLManager
    {
        //public static int texture = -1;

        private static OpenGLManager instance = null;
        public static OpenGLManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OpenGLManager();
                }
                return instance;
            }
        }

        private OpenGLManager()
        {

        }


        public void Init()
        {
            OpenGLManager.Instance.InitDefaultState();
            OpenGLManager.Instance.SetLighting();
            OpenGLManager.Instance.SetMaterialInfo();

            OpenGLManager.Instance.SetClipPlane();
            OpenGLManager.Instance.SetFog();
            OpenGLManager.Instance.SetBlend();

            OpenGLManager.Instance.SetColorMask();
            OpenGLManager.Instance.SetScissor();
            OpenGLManager.Instance.SetAlpha();
            OpenGLManager.Instance.SetDepth();
            OpenGLManager.Instance.SetStencil();
            OpenGLManager.Instance.SetEnableCaps();
            OpenGLManager.Instance.SetTextureState();

            OpenGLManager.Instance.ClearColor(GlobalSetting.DisplaySetting.BackGroundColor);
            OpenGLManager.instance.DrawLight();

        }
        

        public   void InitDefaultState()
        {
            
            GL.Clear(ClearBufferMask.ColorBufferBit 
                     | ClearBufferMask.DepthBufferBit 
                     | ClearBufferMask.AccumBufferBit 
                     | ClearBufferMask.StencilBufferBit);

            GL.ShadeModel(ShadingModel.Smooth);            

            GL.ClearColor(GlobalSetting.DisplaySetting.BackGroundColor);   
   
            GL.ClearDepth(1.0f);                   
            GL.Enable(EnableCap.DepthTest);                
            GL.DepthFunc(DepthFunction.Lequal);             
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);      
            GL.Enable(EnableCap.CullFace);
            GL.PolygonOffset(1f, 1f);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, 
                BlendingFactorDest.OneMinusSrcAlpha);
            GL.Disable(EnableCap.PointSmooth);
            GL.Disable(EnableCap.LineSmooth);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Dither);  
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            
        }


        

        public   void SetTransparentState()
        {
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One); 
         
        }


       

        //public static void InitTexture(string textFile)
        //{
        //    if (texture != -1)
        //        DrawOperators.ReleaseTexture();

        //    Bitmap bitmap = new Bitmap(textFile);
        //    //int texture;
        //    //GL.Enable(EnableCap.Texture2D);

        //    GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            
        //    GL.GenTextures(1, out texture);
        //    GL.BindTexture(TextureTarget.Texture2D, texture);

        //    BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
        //        ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        //    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
        //        OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            
        //    bitmap.UnlockBits(data);

        //    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        //    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

              
        //}

       

       




      




        public  void DrawSphere()
        {  
            int lats = 20;
            int longs = 20;
            int i, j;
            for (i = 0; i <= lats; i++)
            {
                double lat0 = Math.PI * (-0.5 + (double)(i - 1) / lats);
                double z0 = Math.Sin(lat0);
                double zr0 = Math.Cos(lat0);

                double lat1 = Math.PI * (-0.5 + (double)i / lats);
                double z1 = Math.Sin(lat1);
                double zr1 = Math.Cos(lat1);

                GL.Begin(BeginMode.QuadStrip); 
                for (j = 0; j <= longs; j++)
                {
                    double lng = 2 * Math.PI * (double)(j - 1) / longs;
                    double x = Math.Sin(lng);
                    double y = Math.Cos(lng);

                    GL.Normal3(x * zr0, y * zr0, z0);
                    GL.Vertex3(x * zr0, y * zr0, z0);
                    GL.Normal3(x * zr1, y * zr1, z1);
                    GL.Vertex3(x * zr1, y * zr1, z1);
                }
                GL.End();
            }
        }
 

       


        
       

       



   

        public   void ClearColor(Color color)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(color);

        }

        public  void ClearColor(float r, float g, float b, float alpha)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(r, g, b, alpha);

        }

       
        

        // Returns a System.Drawing.Bitmap with the contents of the current framebuffer
        public   Bitmap GrabScreen(Rectangle rectangle)
        {
            if (OpenTK.Graphics.GraphicsContext.CurrentContext == null)
                throw new OpenTK.Graphics.GraphicsContextMissingException();

            Bitmap bmp = new Bitmap(rectangle.Width,rectangle.Height);
            System.Drawing.Imaging.BitmapData data =
                bmp.LockBits(rectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.ReadPixels(0, 0, rectangle.Width,rectangle.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);

            //Bitmap bmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            //System.Drawing.Imaging.BitmapData data =
            //    bmp.LockBits(this.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //GL.ReadPixels(0, 0, this.ClientSize.Width, this.ClientSize.Height, PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bmp;
        }

      
        public   void SetAntialising(bool antialiasing)
        {
            if (antialiasing)
            {
                GL.Enable(EnableCap.PointSmooth);
                GL.Enable(EnableCap.LineSmooth);
                GL.Enable(EnableCap.Blend);
            }
            else
            {
                GL.Disable(EnableCap.PointSmooth);
                GL.Disable(EnableCap.LineSmooth);
                GL.Disable(EnableCap.Blend);
            }
        }




        public void DisableGPUProgram()
        {
            GL.UseProgram(0);
        }

        public void DrawFlatShape(double mouseDownPosX, double mouseDownPosY, 
                         double mouseCurrPosX, double mouseCurrPosY,
                         OpenGLFlatShape shape)
        {
            switch (shape)
            {
                case OpenGLFlatShape.Circle:
                    DrawCircle((short)mouseDownPosX, (short)mouseDownPosY,
                      (short)mouseCurrPosX, (short)mouseCurrPosY);
                    break;
                case OpenGLFlatShape.Rectangle:

                    OpenTK.Graphics.Color4 color =
                        new OpenTK.Graphics.Color4(1.0f, 1.0f, 1.0f, 0.0f);
                    SetColor(color);
                    GL.Rects((short)mouseDownPosX, (short)mouseDownPosY, 
                             (short)mouseCurrPosX, (short)mouseCurrPosY);
                    break;
            }
        }

        public void DrawSelectionInterface(double width, double height, 
                         double mouseDownPosX, double mouseDownPosY, 
                         double mouseCurrPosX, double mouseCurrPosY,
                         OpenGLFlatShape shape)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Ortho(0, width, 0, height, 0, 1000);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);

            DrawFlatShape((short)mouseDownPosX, (short)mouseDownPosY,
                       (short)mouseCurrPosX, (short)mouseCurrPosY,
                       shape);
           

            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);

            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PopMatrix();
        }

        public void DrawCircle(double mouseDownPosX, double mouseDownPosY, 
                               double mouseCurrPosX, double mouseCurrPosY)
        {
            double angle = 0;
            double x2 = 0;
            double y2 = 0;

            double centerx = mouseDownPosX + (mouseCurrPosX - mouseDownPosX) / 2;
            double centery = mouseDownPosY + (mouseCurrPosY - mouseDownPosY) / 2;
            double radius = Math.Sqrt((mouseDownPosX - mouseCurrPosX) 
                                       * (mouseDownPosX - mouseCurrPosX) 
                                   + (mouseDownPosY - mouseCurrPosY) 
                                   * (mouseDownPosY - mouseCurrPosY)) / 2;

            SetColor(GlobalSetting.DisplaySetting.BoundaryColor);

            GL.LineWidth(GlobalSetting.DisplaySetting.SelectionLineWidth);

            GL.Begin(BeginMode.LineStrip);
            for (angle = 1.0f; angle < 361.0f; angle += 0.2)
            {
                x2 = centerx + Math.Sin(angle) * radius;
                y2 = centery + Math.Cos(angle) * radius;
                GL.Vertex2(x2, y2);
            }
            GL.End();
        }

        public void SetColor(Color4 color4)
        {

            OpenTK.Graphics.Color4 color = ConvertColor(color4);
            SetColor(color);
        }


        public void SetColor(OpenTK.Graphics.Color4 color)
        {
            //bool result=false ;
            //GL.GetBoolean(GetPName.ColorMaterial,out result) ;
            //if(!result)
            //{
            //    SetMaterialColor(color);
              
            //}
            //else
            //{
            //    SetMeshColor(color);
            //}

            if (GlobalSetting.MaterialSetting.Enable)
            {
                SetColorMaterial(color);
            }
            else
            {
                SetColorMesh(color);
            }
        }


        



        public void SetColorMaterial(OpenTK.Graphics.Color4 color)
        {
            GL.Disable(EnableCap.ColorMaterial);
            OpenGLManager.Instance.SetMaterial(color);

        }

        public void SetColorMesh(OpenTK.Graphics.Color4 color)
        {
            GL.Enable(EnableCap.ColorMaterial);
            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.Diffuse);
            GL.Color4(color);
             
        }

        public Vector3 ConvertColor2(Color color)
        {
            Vector3 result = new Vector3((float)color.R/255, (float)color.G/255, (float)color.B/255); 

            return result;

        }


        public OpenTK.Graphics.Color4 ConvertColor(Color4 color)
        {
            OpenTK.Graphics.Color4 colorTwo = new OpenTK.Graphics.Color4((float)color.R, (float)color.G, (float)color.B, (float)color.Alpha); 

            return colorTwo;

        }

        public Vector3 ConvertPosition(Vector3D pos)
        {

            Vector3 result = new Vector3((float)pos.x, (float)pos.y, (float)pos.z);

            return result;

        }


        
    }
}
