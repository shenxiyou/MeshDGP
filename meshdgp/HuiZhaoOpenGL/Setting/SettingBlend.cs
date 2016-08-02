using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing ;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao 
{
    public class SettingBlend
    {
        private bool blendEnable = false;

        public bool BlendEnable
        {
            get
            {
                return blendEnable;
            }
            set
            {
                blendEnable = value;
            }
        }

        private BlendingFactorSrc blendingFactorSrc = BlendingFactorSrc.One;
        public BlendingFactorSrc BlendingFactorSrc
        {
            get
            {
                return blendingFactorSrc;
            }
            set
            {
                blendingFactorSrc = value;
            }
        }

        private BlendingFactorDest blendingFactorDest = BlendingFactorDest.Zero;
        public BlendingFactorDest BlendingFactorDest
        {
            get
            {
                return blendingFactorDest;
            }
            set
            {
                blendingFactorDest = value;
            }
        }

        private BlendEquationMode blendEquationMode = BlendEquationMode.FuncAdd;
        public BlendEquationMode BlendEquationMode
        {
            get
            {
                return blendEquationMode;
            }
            set
            {
                blendEquationMode = value;
            }
        }

        private Color blendColor = Color.White;
        public Color BlendColor
        {
            get
            {
                return blendColor;
            }
            set
            {
                blendColor = value;
            }
        }



        private BlendingFactorSrc blendingFactorSrcTwo = BlendingFactorSrc.One;
        public BlendingFactorSrc BlendingFactorSrcTwo
        {
            get
            {
                return blendingFactorSrcTwo;
            }
            set
            {
                blendingFactorSrcTwo = value;
            }
        }

        private BlendingFactorDest blendingFactorDestTwo = BlendingFactorDest.Zero;
        public BlendingFactorDest BlendingFactorDestTwo
        {
            get
            {
                return blendingFactorDestTwo;
            }
            set
            {
                blendingFactorDestTwo = value;
            }
        }

        private BlendEquationMode blendEquationModeTwo = BlendEquationMode.FuncAdd;
        public BlendEquationMode BlendEquationModeTwo
        {
            get
            {
                return blendEquationModeTwo;
            }
            set
            {
                blendEquationModeTwo = value;
            }
        }

    }
}
