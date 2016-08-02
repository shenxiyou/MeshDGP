using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.ComponentModel;

namespace GraphicResearchHuiZhao 
{
    public class SettingTexture
    {
        private bool enableTexture = true;

        private TextureEnvMode envMode = TextureEnvMode.Modulate;

        private Color envColor = Color.White;

        private bool autoTexture = false;

        private TextureGenMode autoGenMode = TextureGenMode.EyeLinear;

        private TextureGenParameter autoGenPara = TextureGenParameter.EyePlane;

        private bool cubeMap = false;

        private Plane planeS = new Plane(1, 0, 0, 0);
        private Plane planeT = new Plane(0, 1, 0, 0);
        private Plane planeR = new Plane(0, 0, 1, 0);

        private bool animationTexture = false;

        private bool multiTexture = false;

        private TextureMinFilter minFilter = TextureMinFilter.Linear;
        private TextureMagFilter magFilter = TextureMagFilter.Linear;
        private TextureWrapMode wrapModeS = TextureWrapMode.Repeat ;
        private TextureWrapMode wrapModeT = TextureWrapMode.Repeat;
        private Color borderColor = Color.Red;
        private HintMode hint = HintMode.Nicest;


        private HintTarget hintTarget = HintTarget.PerspectiveCorrectionHint;


        private EnumTexture edgeUV = EnumTexture.Vertex;

        public EnumTexture EdgeUV
        {
            get
            {
                return edgeUV;
            }
            set
            {
                edgeUV = value;
            }
        }
            

        public bool EnableTexture
        {
            get
            {
                return enableTexture;
            }
            set
            {
                enableTexture = value;
                
            }
        }

        [Category("Env")]
        public TextureEnvMode EnvMode
        {
            get
            {
                return envMode;
            }
            set
            {
                envMode = value;
           
            }
        }
        [Category("Env")]
        public Color EnvColor
        {
            get { return envColor; }
            set { envColor = value;  }
        }
        [Category("AutoTexture")]
        public bool AutoTexture
        {
            get
            {
                return autoTexture;
            }
            set
            {
                autoTexture = value;
               
            }
        }
        [Category("AutoTexture")]
        public TextureGenMode AutoGenMode
        {
            get
            {
                return autoGenMode;
            }
            set
            {
                autoGenMode = value;
                
            }
        }
        [Category("AutoTexture")]
        public TextureGenParameter AutoGenPara
        {
            get
            {
                return autoGenPara;
            }
            set
            {
                autoGenPara = value;
                
            }
        }

        [Category("AutoTexture")]
        public Plane PlaneS
        {
            get
            {
                return planeS;
            }
            set
            {
                planeS = value;
                
            }
        }

        [Category("AutoTexture")]
        public Plane PlaneT
        {
            get
            {
                return planeT;
            }
            set
            {
                planeT = value;
                
            }
        }

        public bool CubeMap
        {
            get { return cubeMap; }
            set { cubeMap = value; }
        }

        [Category("AutoTexture")]
        public Plane PlaneR
        {
            get
            {
                return planeR;
            }
            set
            {
                planeR = value;
                
            }
        }

        [Category("Animation")]
        public bool AnimationTexture
        {
            get
            {
                return animationTexture;
            }
            set
            {
                animationTexture = value;
               
            }
        }

        public bool MultiTexture
        {
            get
            {
                return multiTexture;
            }
            set
            {
                multiTexture = value;
               
            }
        }

        [Category("Parameters")]
        public TextureMinFilter MinFilter
        {
            get
            {
                return minFilter;
            }
            set
            {
                minFilter = value;
                
            }
        }

        [Category("Parameters")]
        public TextureMagFilter MagFilter
        {
            get
            {
                return magFilter;
            }
            set
            {
                magFilter = value;
                
            }
        }

        [Category("Parameters")]
        public TextureWrapMode WrapModeS
        {
            get
            {
                return wrapModeS;
            }
            set
            {
                wrapModeS = value;
                
            }
        }

        [Category("Parameters")]
        public TextureWrapMode WrapModeT
        {
            get
            {
                return wrapModeT;
            }
            set
            {
                wrapModeT = value;
                
            }
        }

        [Category("Parameters")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                
            }
        }

        public HintMode Hint  
        {
            get
            {
                return hint;
            }
            set
            {
                hint = value;
                
            }
        }

        public HintTarget HintTarget
        {
            get
            {
                return hintTarget;
            }
            set
            {
                hintTarget = value;
                
            }
        }

        private Vector3D transform = new Vector3D(0.1, 0.1, 0);

        private Vector3D rotation = Vector3D.Zero;

        private Vector3D scale = Vector3D.One;
        
        private double rotationAngle = 0.0f;
        [Category("Animation")]
        public Vector3D Transform
        {
            get { return transform; }
            set { transform = value;  }
        }
        [Category("Animation")]
        public Vector3D Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        [Category("Animation")]
        public Vector3D Scale
        {
            get { return scale; }
            set { scale = value;  }
        }
        [Category("Animation")]
        public double RotationAngle
        {
            get { return rotationAngle; }
            set { rotationAngle = value;   }
        }

    }
}
