using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao 
{
    public class SettingEnableCaps
    {
        private bool cullFace = false;
        public bool CullFace
        {
            get
            {
                return cullFace;
            }
            set
            {
                cullFace = value;
            }
        }

        private CullFaceMode cullFaceMode = CullFaceMode.Front;
        public CullFaceMode CullFaceMode
        {
            get
            {
                return cullFaceMode;
            }
            set
            {
                cullFaceMode = value;
            }
        }

        private FrontFaceDirection frontFaceDirection = FrontFaceDirection.Ccw;
        public FrontFaceDirection FrontFaceDirection
        {
            get
            {
                return frontFaceDirection;
            }
            set
            {
                frontFaceDirection=value ;
            }
        }


        private bool antialiasing = false;
        public bool AntiAliasing
        {
            get
            {
                return antialiasing;
            }
            set
            {
                antialiasing = value;
            }
        }

        private MaterialFace polygonFace = MaterialFace.FrontAndBack;
        public MaterialFace PolygonFace
        {
            get { return this.polygonFace; }
            set { this.polygonFace = value; }
        }

        private ShadingModel shadingModel = ShadingModel.Smooth;
        public ShadingModel ShadingModel
        {
            get { return this.shadingModel; }
            set { this.shadingModel = value; }
        }
    }
}
