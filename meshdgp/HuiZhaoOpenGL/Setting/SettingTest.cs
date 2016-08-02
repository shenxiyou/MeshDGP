using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace GraphicResearchHuiZhao 
{
    public class SettingTest
    {
        private bool depthEnable = true;
        [Category("深度")]
        public bool DepthEnable
        {
            get { return depthEnable; }
            set { depthEnable = value; }
        }

        private double depthClearValue = 1;
        [Category("深度")]
        public double DepthClearValue
        {
            get { return depthClearValue; }
            set { depthClearValue = value; }
        }

        private DepthFunction depthFunction = DepthFunction.Lequal;
        [Category("深度")]
        public DepthFunction DepthFunction
        {
            get { return depthFunction; }
            set { depthFunction = value; }
        }

        private double depthNear = 0;
        [Category("深度")]
        public double DepthNear
        {
            get { return depthNear; }
            set { depthNear = value; }
        }

        private double depthFar = 1;
        [Category("深度")]
        public double DepthFar
        {
            get { return depthFar; }
            set { depthFar = value; }
        }

        private bool stencilEnable = false;
        [Category("模板")]
        public bool StencilEnable
        {
            get { return stencilEnable; }
            set { stencilEnable = value; }
        }

        private StencilFunction stencilFunction = StencilFunction.Less;
        [Category("模板")]
        public StencilFunction StencilFunction
        {
            get { return stencilFunction; }
            set { stencilFunction = value; }
        }

        private int stencilRef = 3;
        [Category("模板")]
        public int StencilRef
        {
            get { return stencilRef; }
            set { stencilRef = value; }
        }

        private uint stencilMask = 0xFFFFFFFF;
        [Category("模板")]
        public uint StencilMask
        {
            get { return stencilMask; }
            set { stencilMask = value; }
        }

        private int stencilClear = 0;
        [Category("模板")]
        public int StencilClear
        {
            get { return stencilClear; }
            set { stencilClear = value; }
        }

        private StencilOp stencilFail = StencilOp.Keep;
        [Category("模板")]
        public StencilOp StencilFail
        {
            get { return stencilFail; }
            set { stencilFail = value; }
        }

        private StencilOp stencilZFail = StencilOp.Keep;
        [Category("模板")]
        public StencilOp StencilZFail
        {
            get { return stencilZFail; }
            set { stencilZFail = value; }
        }

        private StencilOp stencilZPass = StencilOp.Keep;
        [Category("模板")]
        public StencilOp StencilZPass
        {
            get { return stencilZPass; }
            set { stencilZPass = value; }
        }

        private StencilFace stencilFace = StencilFace.FrontAndBack;
        [Category("模板")]
        public StencilFace StencilFace
        {
            get { return stencilFace; }
            set { stencilFace = value; }
        }

        private bool colorMaskRed = true;
        [Category("颜色")]
        public bool ColorMaskRed
        {
            get { return colorMaskRed; }
            set { colorMaskRed = value; }
        }

        private bool colorMaskGreen = true;
        [Category("颜色")]
        public bool ColorMaskGreen
        {
            get { return colorMaskGreen; }
            set { colorMaskGreen = value; }
        }

        private bool colorMaskBlue = true;
        [Category("颜色")]
        public bool ColorMaskBlue
        {
            get { return colorMaskBlue; }
            set { colorMaskBlue = value; }
        }

        private bool colorMaskAlpha = true;
        [Category("颜色")]
        public bool ColorMaskAlpha
        {
            get { return colorMaskAlpha; }
            set { colorMaskAlpha = value; }
        }

        private bool scissorEnable = false;
        [Category("裁剪")]
        public bool ScissorEnable
        {
            get { return scissorEnable; }
            set { scissorEnable = value; }
        }

        private Rectangle scissorRect;
        [Category("裁剪")]
        public Rectangle ScissorRect
        {
            get { return scissorRect; }
            set { scissorRect = value; }
        }

        private bool alphaEnable = false;
        [Category("Alpha")]
        public bool AlphaEnable
        {
            get { return alphaEnable; }
            set { alphaEnable = value; }
        }

        private double alphaValue = 1d;
        [Category("Alpha")]
        public double AlphaValue
        {
            get { return alphaValue; }
            set { alphaValue = value; }
        }

        private AlphaFunction alphaFunction = AlphaFunction.Less;
        [Category("Alpha")]
        public AlphaFunction AlphaFunction
        {
            get { return alphaFunction; }
            set { alphaFunction = value; }
        }

        private bool accumEnable = false;
        [Category("累计")]
        public bool AccumEnable
        {
            get { return accumEnable; }
            set { accumEnable = value; }
        }


    }
}
