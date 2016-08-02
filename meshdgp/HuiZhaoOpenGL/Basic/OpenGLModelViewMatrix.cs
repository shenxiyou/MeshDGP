using System;
using System.Collections.Generic;
using System.Drawing;  
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao 
{
    public partial class OpenGLManager
    {  
        public void SetModelViewMatrix(Matrix4D modelViewMatrix)
        {  
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(modelViewMatrix.ToArray());

            
         
        }

        

        public void SetProjectionMatrix(Matrix4D projectionMatrix)
        {
            GL.MatrixMode(MatrixMode.Projection);
            Matrix4D m= projectionMatrix.Transpose();
            GL.LoadMatrix(m.ToArray());
        }

        public void SetViewPort(int x,int y,int width, int height)
        {
            if (height == 0)
            {
                GL.Viewport(x, y, width, 1);
            }
            else
            {
                GL.Viewport(x, y, width, height);
            } 
        }

        public void SetViewPortFirst()
        {
            SetViewPort(0, SettingTransform.Instance.ViewportHeight/2,
                        SettingTransform.Instance.ViewportWidth/2, SettingTransform.Instance.ViewportHeight/2);
            GL.Scissor(0, SettingTransform.Instance.ViewportHeight / 2,
                        SettingTransform.Instance.ViewportWidth / 2, SettingTransform.Instance.ViewportHeight / 2);
        }

        public void SetViewPortSecond()
        {
            SetViewPort(SettingTransform.Instance.ViewportWidth / 2, SettingTransform.Instance.ViewportHeight/2,
                        SettingTransform.Instance.ViewportWidth / 2, SettingTransform.Instance.ViewportHeight/2);
            GL.Scissor(SettingTransform.Instance.ViewportWidth / 2, SettingTransform.Instance.ViewportHeight / 2,
                        SettingTransform.Instance.ViewportWidth / 2, SettingTransform.Instance.ViewportHeight / 2);

        }

        public void SetViewPortThird()
        {
            SetViewPort(0, 0,
                        SettingTransform.Instance.ViewportWidth / 2, SettingTransform.Instance.ViewportHeight/2);
            GL.Scissor(0, 0,
                        SettingTransform.Instance.ViewportWidth / 2, SettingTransform.Instance.ViewportHeight / 2);
        }

        public void SetViewPortForth()
        {
            SetViewPort(SettingTransform.Instance.ViewportWidth / 2, 0,
                        SettingTransform.Instance.ViewportWidth / 2, SettingTransform.Instance.ViewportHeight/2);
            GL.Scissor(SettingTransform.Instance.ViewportWidth / 2, 0,
                        SettingTransform.Instance.ViewportWidth / 2, SettingTransform.Instance.ViewportHeight / 2);
        }

        public void SetViewPort( )
        {
            SetViewPort(SettingTransform.Instance.ViewportX, SettingTransform.Instance.ViewportY,
                        SettingTransform.Instance.ViewportWidth, SettingTransform.Instance.ViewportHeight);
        }

        public void SetProjectionMatrix( )
        {
            if (SettingTransform.Instance.UseFovY)
            {
                TransformController.Instance.SetFrustum(SettingTransform.Instance.FovY, 
                                                 SettingTransform.Instance.Aspect,
                                                 SettingTransform.Instance.Near, SettingTransform.Instance.Far);
            }
            else
            {
                TransformController.Instance.SetProjection(SettingTransform.Instance.Left,
                                                    SettingTransform.Instance.Right,
                                                    SettingTransform.Instance.Bottom,
                                                    SettingTransform.Instance.Top,
                                                    SettingTransform.Instance.Near,
                                                    SettingTransform.Instance.Far,
                                                    SettingTransform.Instance.ViewportWidth,
                                                    SettingTransform.Instance.ViewportHeight,
                                                    SettingTransform.Instance.Ortho,
                                                    SettingTransform.Instance.UseRatio);
            }
            SetProjectionMatrix(TransformController.Instance.ProjectionMatrix);
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.Ortho(SettingOpenGL.Instance.Left,
            //                                        SettingOpenGL.Instance.Right,
            //                                        SettingOpenGL.Instance.Bottom,
            //                                        SettingOpenGL.Instance.Top,
            //                                        SettingOpenGL.Instance.Near,
            //                                        SettingOpenGL.Instance.Far);
        }

    }
}
