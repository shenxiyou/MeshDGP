using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao 
{
    public partial class OpenGLManager     
    {
        public void SetFog()
        {
            if (GlobalSetting.FogSetting.FogEnable)
            {
                float[] fogColor = { GlobalSetting.FogSetting.FogColor.R, 
                                     GlobalSetting.FogSetting.FogColor.G,
                                     GlobalSetting.FogSetting.FogColor.B,
                                     GlobalSetting.FogSetting.FogColor.A};

                GL.Enable(EnableCap.Fog);
                GL.Fog(FogParameter.FogMode, (int)GlobalSetting.FogSetting.FogMode);
                GL.Fog(FogParameter.FogColor, fogColor);
                GL.Fog(FogParameter.FogDensity, (float)GlobalSetting.FogSetting.FogDenstity);
                GL.Fog(FogParameter.FogStart, (float)GlobalSetting.FogSetting.FogStart);
                GL.Fog(FogParameter.FogEnd, (float)GlobalSetting.FogSetting.FogEnd);

                GL.Hint(HintTarget.FogHint, GlobalSetting.FogSetting.HintMode );
            }
            else
            {

                GL.Disable(EnableCap.Fog);
            }
        }



        public void SetBlend()
        {
            if (GlobalSetting.BlendSetting.BlendEnable)
            {
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(GlobalSetting.BlendSetting.BlendingFactorSrc, GlobalSetting.BlendSetting.BlendingFactorDest);
                GL.BlendEquation(GlobalSetting.BlendSetting.BlendEquationMode);
                GL.BlendColor(GlobalSetting.BlendSetting.BlendColor);

                GL.BlendFuncSeparate(GlobalSetting.BlendSetting.BlendingFactorSrc, 
                                     GlobalSetting.BlendSetting.BlendingFactorDest,
                                     GlobalSetting.BlendSetting.BlendingFactorSrcTwo, 
                                     GlobalSetting.BlendSetting.BlendingFactorDestTwo);
                GL.BlendEquationSeparate(GlobalSetting.BlendSetting.BlendEquationMode,
                                         GlobalSetting.BlendSetting.BlendEquationModeTwo);

            }
            else
            {
                GL.Disable(EnableCap.Blend);
            }
        }

        public void SetColorMask()
        {
            GL.ColorMask(GlobalSetting.TestSetting.ColorMaskRed,
                GlobalSetting.TestSetting.ColorMaskGreen,
                GlobalSetting.TestSetting.ColorMaskBlue,
                GlobalSetting.TestSetting.ColorMaskAlpha);
        }

        public void SetScissor()
        {
            if (GlobalSetting.TestSetting.ScissorEnable)
            {
                GL.Enable(EnableCap.ScissorTest);
                Rectangle rect = GlobalSetting.TestSetting.ScissorRect;
                GL.Scissor(rect.X, rect.Y, rect.Width, rect.Height);
            }
            else
            {
                GL.Disable(EnableCap.ScissorTest);
            }
        }

        public void SetAlpha()
        {
            if (GlobalSetting.TestSetting.AlphaEnable)
            {
                GL.Enable(EnableCap.AlphaTest);
                GL.AlphaFunc(GlobalSetting.TestSetting.AlphaFunction, (float)GlobalSetting.TestSetting.AlphaValue);
            }
            else
            {
                GL.Disable(EnableCap.AlphaTest);
            }
        }

        public void SetDepth()
        {
            if (GlobalSetting.TestSetting.DepthEnable)
            {
                GL.Enable(EnableCap.DepthTest);  
                GL.Clear(ClearBufferMask.DepthBufferBit);
                GL.ClearDepth(GlobalSetting.TestSetting.DepthClearValue);
                GL.DepthRange(GlobalSetting.TestSetting.DepthNear, GlobalSetting.TestSetting.DepthFar);
                GL.DepthFunc(GlobalSetting.TestSetting.DepthFunction); 

            }
            else
            {
                GL.Disable(EnableCap.DepthTest);
            }
        }

        public void SetAccum()
        {
            if (GlobalSetting.TestSetting.AccumEnable)
            {
               

            }
            else
            {
            
            }
        }


        public void SetStencil()
        {
            
            if (GlobalSetting.TestSetting.StencilEnable)
            {
                GL.Enable(EnableCap.StencilTest);
                GL.ClearStencil(GlobalSetting.TestSetting.StencilClear);
                GL.StencilOp(GlobalSetting.TestSetting.StencilFail, 
                    GlobalSetting.TestSetting.StencilZFail, GlobalSetting.TestSetting.StencilZPass);
                GL.StencilFunc(GlobalSetting.TestSetting.StencilFunction, 
                    GlobalSetting.TestSetting.StencilRef, GlobalSetting.TestSetting.StencilMask);
                GL.Clear(ClearBufferMask.StencilBufferBit);
            }
            else
            {
                GL.Disable(EnableCap.StencilTest);
            }
        }

        public void SetEnableCaps()
        {
            if (GlobalSetting.EnalbeCapsSetting.CullFace)
            {
                GL.Enable(EnableCap.CullFace);
                GL.CullFace(GlobalSetting.EnalbeCapsSetting.CullFaceMode);
            }
            else
            {
                GL.Disable(EnableCap.CullFace);
            }

            GL.FrontFace(GlobalSetting.EnalbeCapsSetting.FrontFaceDirection);

            if (GlobalSetting.EnalbeCapsSetting.AntiAliasing)
            {
                GL.Enable(EnableCap.PointSmooth);
                GL.Enable(EnableCap.LineSmooth);
                GL.Enable(EnableCap.PolygonSmooth);
                GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
                GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
                GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
            }
            else
            {
                GL.Disable(EnableCap.PointSmooth);
                GL.Disable(EnableCap.LineSmooth);
                GL.Disable(EnableCap.PolygonSmooth);
            }
        }
    }
}
