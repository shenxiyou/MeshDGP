using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace GraphicResearchHuiZhao
{
    public class SettingTransform
    {

        private static SettingTransform instance = null;
        public static SettingTransform Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SettingTransform();
                }
                return instance;
            }
        }

        private SettingTransform()
        {

        }

        private double scaleUnit = 0.9;

        public double ScaleUnit
        {
            get
            {
                return scaleUnit;
            }
            set
            {
                scaleUnit = value;
            }
        }



        private Vector3D quaternion = Vector3D.UnitX;


        [Category("Quaternion")]
        public Vector3D Quaternion
        {
            get
            {
                return quaternion;
            }
            set
            {
                quaternion = value;
            }

        }

        private double quaternionDegree = 0.1;

        [Category("Quaternion")]
        public double QuaternionDegree
        {
            get
            {
                return quaternionDegree;
            }
            set
            {
                quaternionDegree = value;
            }
        }

        private Vector3D rotationAxis = Vector3D.UnitX;


        [Category("Model")]
        public Vector3D RotationAxis
        {
            get
            {
                return rotationAxis;
            }
            set
            {
                rotationAxis = value;
            }

        }

        private double rotationV = 0.1;

        [Category("Model")]
        public double RotationV
        {
            get
            {
                return rotationV;
            }
            set
            {
                rotationV = value;
            }
        }


        private Vector3D modelTranslate = Vector3D.Zero;


        [Category("Model")]
        public Vector3D ModelTranslate
        {
            get
            {
                return modelTranslate;
            }
            set
            {
                modelTranslate = value;
            }

        }

        private bool useRotationAxis = false;

        [Category("Model")]
        public bool UseRotationAxis
        {
            get
            {
                return useRotationAxis;
            }
            set
            {
                useRotationAxis = value;
            }
        }

        private Vector3D modelRotation = Vector3D.Zero;


        [Category("Model")]
        public Vector3D ModelRotation
        {
            get
            {
                return modelRotation;
            }
            set
            {
                modelRotation = value;
            }

        }

        private Vector3D modelScale =new  Vector3D(1,1,1);


        [Category("Model")]
        public Vector3D ModelScale
        {
            get
            {
                return modelScale;
            }
            set
            {
                modelScale = value;
            }

        }
        private Vector3D modelShearing = Vector3D.Zero;


        [Category("Model")]
        public Vector3D ModelShearing
        {
            get
            {
                return modelShearing;
            }
            set
            {
                modelShearing = value;
            }

        }


        private Vector3D modelMirror = Vector3D.Zero;


        [Category("Model")]
        public Vector3D ModelMirror
        {
            get
            {
                return modelMirror;
            }
            set
            {
                modelMirror = value;
            }
        }


       





        private Vector3D eyePos = Vector3D.Zero;

        [Category("View")]
        public Vector3D EyePos
        {
            get
            {
                return eyePos;
            }
            set
            {
                eyePos = value;
            }
        }


        private Vector3D eyeDirection = Vector3D.Zero;


        [Category("View")]
        public Vector3D EyeDirection
        {
            get
            {
                return eyeDirection;
            }
            set
            {
                eyeDirection = value;
            }
        }

        
        private bool useRatio = true;

        [Category("Projection")]
        public bool UseRatio
        {
            get
            {
                return useRatio;
            }
            set
            {
                useRatio = value;
            }
        }

        private bool ortho = true ;

        [Category("Projection")]
        public bool Ortho
        {
            get
            {
                return ortho;
            }
            set
            {
                ortho = value;
            }
        }

        private bool useFovY = false;

        [Category("Projection")]
        public bool UseFovY
        {
            get
            {
                return useFovY;
            }
            set
            {
                useFovY = value;
            }
        }

        private int viewportX = 0;
        private int viewportY = 0;

        private int viewportWidth = 0;

        private int viewportHeight = 0;

        [Category("Viewport")]
        public int ViewportWidth
        {
            get
            {
                return viewportWidth;
            }
            set
            {
                viewportWidth = value;
            }
        }

        [Category("Viewport")]
        public int ViewportHeight
        {
            get
            {
                return viewportHeight;
            }
            set
            {
                viewportHeight = value;
            }
        }

        [Category("Viewport")]
        public int ViewportX
        {
            get
            {
                return viewportX;
            }
            set
            {
                viewportX = value;
            }
        }

        [Category("Viewport")]
        public int ViewportY
        {
            get
            {
                return viewportY;
            }
            set
            {
                viewportY = value;
            }
        }


        private double left = -0.5;
        private double right = 0.5;
        private double top = 0.5;
        private double bottom = -0.5;
        private double near = -10;
        private double far = 10;

        private double fovY = 60;
        private double aspect = 2;

        [Category("Projection")]
        public double FovY
        {
            set
            {
                fovY = value;
            }
            get
            {
                return fovY;
            }
        }

        [Category("Projection")]
        public double Aspect
        {
            set
            {
                aspect = value;
            }
            get
            {
                return aspect;
            }
        }


        [Category("Projection")]
        public  double Left
        {
            set
            { 
                left = value;
            }
            get
            {
                return left;
            }
        }

        [Category("Projection")]
        public double Right
        {
            set
            {
                right = value;
            }
            get
            {
                return right;
            }
        }

        [Category("Projection")]
        public double Top
        {
            set
            {
                top = value;
            }
            get
            {
                return top;
            }
        }

        [Category("Projection")]
        public double Bottom
        {
            set
            {
                bottom = value;
            }
            get
            {
                return bottom;
            }
        }

        [Category("Projection")]
        public double Near
        {
            set
            {
                near = value;
            }
            get
            {
                return near;
            }

        }

        [Category("Projection")]
        public double Far
        {
            set
            {
                far = value;
            }
            get
            {
                return far;
            }

        }

      


    }
}
