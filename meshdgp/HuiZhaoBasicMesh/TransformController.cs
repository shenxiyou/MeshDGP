using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{


    public enum MVPActionType
    {
        None,
        RollClockWise,
        RollCounterClockWise,
        PitchUp,
        PitchDown,
        YawLeft,
        YawRight,
        ZoomIn,
        ZoomOut,

        UP,
        Down,
        ClockWise,
        CounterClockWise,
        Left,
        Right,
        Near,
        Far
    }


    public class TransformController
    {

        private static TransformController instance = null;
        public static TransformController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TransformController();
                }
                return instance;
            }
        }

        private TransformController()
        {

        }


        public bool Ortho = true  ;
       
        public Matrix4D ModelMatrix = Matrix4D.IdentityMatrix;  
        public Matrix4D ViewMatrix = Matrix4D.IdentityMatrix;
        public Matrix4D ProjectionMatrix = Matrix4D.IdentityMatrix;

 




        public Vector3D eyePosition=new Vector3D(0,0,1);
        public Vector3D orientation=Vector3D.Zero;
        public Vector3D target=Vector3D.UnitY;

        

        public Vector3D EyePosition
        {
            get
            {
                return eyePosition;
            }
            set
            {
                eyePosition = value;

                LookAtFrom(eyePosition, target, orientation);
            }
        }

        public Vector3D Orientation
        {
            get
            {
                return orientation;
            }
            set
            {
                orientation = value;

                LookAtFrom(eyePosition, target, orientation);
            }
        }

        public Vector3D Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;

                LookAtFrom(eyePosition, target, orientation);
            }
        }

       
        public Matrix4D TransformInverse
        {
            get 
            {
                return ModelMatrix.Inverse();
            }
        }

       

        public void MoveToCenter()
        {
            ModelMatrix[0, 3] = 0;
            ModelMatrix[1, 3] = 0;
            ModelMatrix[2, 3] = 0;

        }

        public MVPActionType CurrentActionType = MVPActionType.None;


        

        public double Angle = Math.PI * 5 / 180.0;
        public double TransValue = 0.1f;
        public double ScaleRatio = 1.1f;

        public void Roll(double angle)
        {
            ModelMatrix = ModelMatrix* Matrix4D.RotationYawPitchRoll(0, 0, angle) ;
        }

        public void Yaw(double angle)
        {
            ModelMatrix =ModelMatrix* Matrix4D.RotationYawPitchRoll(0, angle, 0)  ;
        }

        public void Pitch(double angle)
        {
            ModelMatrix = ModelMatrix *Matrix4D.RotationYawPitchRoll(angle, 0, 0)   ;
        }

       




        public void Transform()
        {
            switch (CurrentActionType)
            {
                case MVPActionType.RollClockWise:
                    this.Roll(Angle);
                    break;
                case MVPActionType.RollCounterClockWise:
                    this.Roll(-Angle);
                    break;
                case MVPActionType.PitchUp:
                    this.Pitch(Angle);
                    break;
                case MVPActionType.PitchDown:
                    this.Pitch(-Angle);
                    break;
                case MVPActionType.YawLeft:
                    this.Yaw(Angle);
                    break;
                case MVPActionType.YawRight:
                    this.Yaw(-Angle);
                    break;
                case MVPActionType.ZoomIn:
                    this.Scale(ScaleRatio); 
                    break;
                case MVPActionType.ZoomOut:
                    this.Scale(1/ScaleRatio); 
                    break;


                case MVPActionType.ClockWise:
                    this.RotateCamera(Angle, 0, 0);
                    break;
                case MVPActionType.CounterClockWise:
                    this.RotateCamera(-Angle, 0, 0);
                    break;
                case MVPActionType.UP:
                    this.TranslateCamera(0, TransValue, 0);
                    break;
                case MVPActionType.Down:
                    this.TranslateCamera(0, -TransValue, 0);
                    break;
                case MVPActionType.Left:
                    this.TranslateCamera(-TransValue,0, 0);
                    break;
                case MVPActionType.Right:
                    this.TranslateCamera(TransValue,0, 0);
                    break;
                case MVPActionType.Near:
                    this.TranslateCamera(0, 0,TransValue);
                    break;
                case MVPActionType.Far:
                    this.TranslateCamera(0, 0,-TransValue);
                    break;
                default:
                    break;
            }
        }


        public void ModelRotate(double x, double y, double z)
        {
           // ModelMatrix =ModelMatrix* Matrix4D.RotationYawPitchRoll(y, x, z);

            ModelMatrix = ModelMatrix * Matrix4D.Rotation(x, y, z);
            
        }

        public void ModelRotate(Vector3D rotation)
        {
            // ModelMatrix =ModelMatrix* Matrix4D.RotationYawPitchRoll(y, x, z);

            ModelMatrix = ModelMatrix * Matrix4D.Rotation(rotation.x, rotation.y, rotation.z);

        }

        public void ModelRotate(Vector3D axis,double degree)
        {
            Vector3D v= axis.Normalize();
            ModelMatrix = ModelMatrix * Matrix4D.RotationAxis(v,degree);

        }

        public void ModelQuaternion(Vector3D axis, double degree)
        {
            Quaternion rotate = Quaternion.RotationAxis(axis, degree);

            ModelMatrix = ModelMatrix * Matrix4D.RotationQuaternion(rotate);

        }

        public void ModelTranslate(double x, double y, double z)
        {
            ModelMatrix = ModelMatrix * Matrix4D.Translation(x, y, z);
        }

        public void ModelTranslate(Vector3D trans)
        {
            ModelMatrix = ModelMatrix * Matrix4D.Translation(trans.x, trans.y, trans.z);
        }

        public void ModelScale(double x, double y, double z)
        {
            ModelMatrix = ModelMatrix * Matrix4D.Scaling(x, y, z);
        }

        public void ModelScale(Vector3D scale)
        {
            ModelMatrix = ModelMatrix * Matrix4D.Scaling(scale.x, scale.y, scale.z);
        }

        public void ModelMirror(double x, double y, double z)
        {
            ModelMatrix = ModelMatrix * Matrix4D.Mirror(x, y, z);
        }

        public void ModelMirror(Vector3D mirror)
        {
            ModelMatrix = ModelMatrix * Matrix4D.Mirror(mirror.x, mirror.y, mirror.z);
        }

        public void ModelShear(double x, double y, double z)
        {
            ModelMatrix = ModelMatrix * Matrix4D.Shearing(x, y, z);
        }

        public void ModelShear(Vector3D shear)
        {
            ModelMatrix = ModelMatrix * Matrix4D.Shearing(shear.x, shear.y, shear.z);
        }

        public void Scale(double ratio)
        {
            ModelMatrix = Matrix4D.Scaling(ratio) * ModelMatrix;
        }
       
        public void LookFromX()
        {   
            ModelMatrix = Matrix4D.RotationYawPitchRoll(Math.PI/2, 0, 0) ; 
        }
        public void LookFromY()
        { 
            ModelMatrix = Matrix4D.RotationYawPitchRoll(0, Math.PI / 2, 0); 
        } 
        public void LookFromZ()
        { 
            ModelMatrix = Matrix4D.RotationYawPitchRoll(0, 0, Math.PI / 2); 
        }
        public void LookFromXMinus()
        { 
            ModelMatrix = Matrix4D.RotationYawPitchRoll(-Math.PI / 2, 0, 0); 
        }
        public void LookFromYMinus()
        { 
            ModelMatrix = Matrix4D.RotationYawPitchRoll(0, -Math.PI / 2, 0); 
        } 
        public void LookFromZMinus()
        { 
            ModelMatrix = Matrix4D.RotationYawPitchRoll(0, 0, -Math.PI / 2); 
        }

        public void LookAtFrom(Vector3D eye,Vector3D target,Vector3D up)
        {
            ViewMatrix = Matrix4D.LookAtRH(eye, target, up);  
        }

        public void TranslateCamera(double x, double y, double z)
        {
            ViewMatrix = ViewMatrix * Matrix4D.Translation(-x, -y, -z);
              
        }

        public void TranslateCamera(Vector3D trans)
        {
            TranslateCamera(-trans.x, -trans.y, -trans.z);

        }

        public void RotateCamera(double x, double y, double z)
        {
            ViewMatrix = ViewMatrix * Matrix4D.RotationYawPitchRoll(-y, -x, -z);
        }
        public void RotateCamera(Vector3D rotation)
        {
            RotateCamera(-rotation.x, -rotation.y, -rotation.z);
        }

        public void SetCamearPosition(double x, double y, double z)
        {
            ViewMatrix = Matrix4D.LookAtRH(new Vector3D(x,y,z), Vector3D.Zero, Vector3D.UnitY);  
        }

        

        public void SetProjection(int width, int height)
        {   

            double ratio;
            if (width > height)
            {
                ratio = (width / height) / 2.0;
            }
            else
            {
                ratio = (height / width) / 2.0;
            }

            if (Ortho)
            {
                if (width > height)
                {
                    ProjectionMatrix = Matrix4D.SetOrthoFrustum(-ratio, ratio, -0.5, 0.5, -100, 100);
                }
                else
                {
                    ProjectionMatrix = Matrix4D.SetOrthoFrustum(-0.5, 0.5, -ratio, ratio, -100, 100);
                }
            }
            else
            {
                ProjectionMatrix = Matrix4D.SetFrustum(-1, 1, -1, 1, -100, 100.0);
            }
        }


       

       
        public void SetFrustumPespective(double left, double right, double bottom, double top, double near, double far)
        {
            ProjectionMatrix = Matrix4D.SetFrustum(left, right, bottom, top, near, far);
        }


        public void SetFrustumOrtho(double left, double right, double bottom, double top, double near, double far)
        {
            ProjectionMatrix = Matrix4D.SetOrthoFrustum(left, right, bottom, top, near, far);
        }
        
        ///////////////////////////////////////////////////////////////////////////////
        // set a symmetric perspective frustum with 4 params similar to gluPerspective
        // (vertical field of view, aspect ratio, near, far)
        ///////////////////////////////////////////////////////////////////////////////
        public void SetFrustum(double fovY, double aspectRatio, double front, double back)
        {
            double DEG2RAD = 3.141593f / 180;
            double tangent =  Math.Tan((fovY / 2 * DEG2RAD));   // tangent of half fovY
            double height = front * tangent;           // half height of near plane
            double width = height * aspectRatio;       // half width of near plane

            // params: left, right, bottom, top, near, far
            SetFrustumPespective(-width, width, -height, height, front, back);
        }



        ///////////////////////////////////////////////////////////////////////////////
        // set 6 params of frustum
        ///////////////////////////////////////////////////////////////////////////////
        public void SetProjection(double left, double right, double bottom, double top, double near, double far,int width,int height,bool ortho,bool useRatio)
        {
            double ratio;
            if (!ortho)
            {
                if (!useRatio)
                {
                    SetFrustumPespective(left, right, bottom, top, near, far);
                }
                else
                {
                    if (width > height)
                    {
                        ratio = width / height;
                        SetFrustumPespective(left * ratio, right * ratio, 
                                             bottom, top, near, far);
                    }
                    else
                    {
                        ratio = height / width;
                        SetFrustumPespective(left, right, bottom * ratio, 
                                             top * ratio, near, far);
                    }
                }

                
              
            }
            else
            {
                if (!useRatio)
                {

                    SetFrustumOrtho(left, right, bottom, top, near, far);
                }
                else
                {
                    if (width > height)
                    {
                        ratio = width / height;
                        SetFrustumOrtho(left * ratio, right * ratio, bottom, top, near, far);
                    }
                    else
                    {
                        ratio = height / width;
                        SetFrustumOrtho(left, right, bottom * ratio, top * ratio, near, far);
                    }
                }

            }
        }


                ///////////////////////////////////////////////////////////////////////////////
        // compute 8 vertices of frustum
        ///////////////////////////////////////////////////////////////////////////////
        public Vector3D[] ComputeFrustumVertices(double l, double r, double b, double t, double n, double f)
        {

            Vector3D[] frustumVertices = new Vector3D[8];
            double ratio;
            double farLeft;
            double farRight;
            double farBottom;
            double farTop;

            // perspective mode
            if (!Ortho)
                ratio = f / n;
            // orthographic mode
            else
                ratio = 1;
            farLeft   = l * ratio;
            farRight  = r * ratio;
            farBottom = b * ratio;
            farTop    = t * ratio;

            // compute 8 vertices of the frustum
            // near top right
            frustumVertices[0].x = r;
            frustumVertices[0].y = t;
            frustumVertices[0].z = -n;

            // near top left
            frustumVertices[1].x = l;
            frustumVertices[1].y = t;
            frustumVertices[1].z = -n;

            // near bottom left
            frustumVertices[2].x = l;
            frustumVertices[2].y = b;
            frustumVertices[2].z = -n;

            // near bottom right
            frustumVertices[3].x = r;
            frustumVertices[3].y = b;
            frustumVertices[3].z = -n;

            // far top right
            frustumVertices[4].x = farRight;
            frustumVertices[4].y = farTop;
            frustumVertices[4].z = -f;

            // far top left
            frustumVertices[5].x = farLeft;
            frustumVertices[5].y = farTop;
            frustumVertices[5].z = -f;

            // far bottom left
            frustumVertices[6].x = farLeft;
            frustumVertices[6].y = farBottom;
            frustumVertices[6].z = -f;

            // far bottom right
            frustumVertices[7].x = farRight;
            frustumVertices[7].y = farBottom;
            frustumVertices[7].z = -f;

            return frustumVertices;
        }


    }
}
