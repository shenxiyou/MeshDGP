using System;
using System.Collections.Generic;
using System.Text; 
using System.Drawing; 
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao 
{
    public class SettingFog
    {
        private bool fogEnable = false;

        public bool FogEnable
        {
            get
            {
                return fogEnable;
            }
            set
            {
                fogEnable = value;
            }
        }

        private double fogDenstity = 0;
        public double FogDenstity
        {
            get
            {
                return fogDenstity;
            }
            set
            {
                fogDenstity = value;
            }
        }

        private double fogStart = 0;
        public double FogStart
        {
            get
            {
                return fogStart;
            }
            set
            {
                fogStart = value;
            }
        }


        private double fogEnd = 1;
        public double FogEnd
        {
            get
            {
                return fogEnd;
            }
            set
            {
                fogEnd = value;
            }
        }

        private Color fogColor = Color.Black;
        public Color FogColor
        {
            get { return fogColor; }
            set { fogColor = value; }
        }

        private HintMode hintMode = HintMode.DontCare;
        public HintMode HintMode
        {
            get
            {
                return hintMode;
            }
            set
            {
                hintMode = value;
            }
        }

        private FogMode fogMode = FogMode.Linear;
        public FogMode FogMode
        {
            get
            {
                return fogMode;
            }
            set
            {
                fogMode = value;
            }
        }

    }
}
