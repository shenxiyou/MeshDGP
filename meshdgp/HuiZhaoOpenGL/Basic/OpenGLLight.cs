using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public partial class OpenGLManager
    {
        

        public void SetLighting()
        {
            if (GlobalSetting.SettingLight.Enabled)
            {
                GL.Enable(EnableCap.Lighting);

                SetDoubleSideLight(); 

                SetLight0Direct();
                SetLight1Direct();
                SetLight2Direct();
                SetLight3Direct();
                SetLight4Direct();
                SetLight5Direct(); 
                SetLight6Spot();
                SetLight7Point();                
            }
            else
            {
                GL.Disable(EnableCap.Lighting); 
            }
           
        }


        public void SetLight6Spot()
        {
            if (GlobalSetting.Light6Setting.Enabled)
            {

                GL.Enable(EnableCap.Light6);
                Vector4 pos =
                    new Vector4((float)GlobalSetting.Light6Setting.LightPosition.x,
                                (float)GlobalSetting.Light6Setting.LightPosition.y,
                                (float)GlobalSetting.Light6Setting.LightPosition.z,
                                1.0f);
                GL.Light(LightName.Light6, LightParameter.Position, pos);
                GL.Light(LightName.Light6, LightParameter.Diffuse, 
                         GlobalSetting.Light6Setting.DiffuseColor);
                GL.Light(LightName.Light6, LightParameter.Specular, 
                         GlobalSetting.Light6Setting.SpecularColor);
                GL.Light(LightName.Light6, LightParameter.Ambient, 
                         GlobalSetting.Light6Setting.AmbientColor);

                GL.Light(LightName.Light6, LightParameter.LinearAttenuation, 
                         GlobalSetting.Light6Setting.LinearAttenuation);
                GL.Light(LightName.Light6, LightParameter.ConstantAttenuation, 
                         GlobalSetting.Light6Setting.ConstantAttenuation);
                GL.Light(LightName.Light6, LightParameter.QuadraticAttenuation, 
                         GlobalSetting.Light6Setting.QuadraticAttenuation);

                Vector4 spotDirection = 
                    new Vector4((float)GlobalSetting.Light6Setting.SpotDirection.x,
                                (float)GlobalSetting.Light6Setting.SpotDirection.y,
                                (float)GlobalSetting.Light6Setting.SpotDirection.z,
                                 0.0f);
                GL.Light(LightName.Light6, LightParameter.SpotDirection, 
                         spotDirection);
                GL.Light(LightName.Light6, LightParameter.SpotExponent, 
                         GlobalSetting.Light6Setting.SpotExponent);
                GL.Light(LightName.Light6, LightParameter.SpotCutoff, 
                        GlobalSetting.Light6Setting.SpotCutoff);
            }
            else
            {
                GL.Disable(EnableCap.Light6);
            }
        }


        public void SetLight7Point()
        {
            if (GlobalSetting.Light7Setting.Enabled)
            {

                GL.Enable(EnableCap.Light7);               
                GL.Light(LightName.Light7, LightParameter.Diffuse, 
                         GlobalSetting.Light7Setting.DiffuseColor);
                GL.Light(LightName.Light7, LightParameter.Specular, 
                         GlobalSetting.Light7Setting.SpecularColor);
                GL.Light(LightName.Light7, LightParameter.Ambient, 
                         GlobalSetting.Light7Setting.AmbientColor);

                Vector4 pos =
                  new Vector4((float)GlobalSetting.Light7Setting.LightPosition.x,
                                  (float)GlobalSetting.Light7Setting.LightPosition.y,
                                  (float)GlobalSetting.Light7Setting.LightPosition.z,
                                  1.0f);
                GL.Light(LightName.Light7, LightParameter.Position,
                         pos);


                GL.Light(LightName.Light7, LightParameter.LinearAttenuation, 
                         GlobalSetting.Light7Setting.LinearAttenuation);
                GL.Light(LightName.Light7, LightParameter.ConstantAttenuation, 
                         GlobalSetting.Light7Setting.ConstantAttenuation);
                GL.Light(LightName.Light7, LightParameter.QuadraticAttenuation, 
                         GlobalSetting.Light7Setting.QuadraticAttenuation);
            }
            else
            {
                GL.Disable(EnableCap.Light7);
            }
        }



        public void SetLight0Direct()
        {
            if (GlobalSetting.Light0Setting.Enabled)
            {
                GL.Enable(EnableCap.Light0);
                Vector4 pos =
                    new Vector4((float)GlobalSetting.Light0Setting.LightPosition.x,
                               (float)GlobalSetting.Light0Setting.LightPosition.y,
                               (float)GlobalSetting.Light0Setting.LightPosition.z,
                               0.0f);

                GL.Light(LightName.Light0, LightParameter.Position, pos);

                GL.Light(LightName.Light0, LightParameter.Diffuse, 
                         GlobalSetting.Light0Setting.DiffuseColor);
                GL.Light(LightName.Light0, LightParameter.Specular, 
                         GlobalSetting.Light0Setting.SpecularColor);
                GL.Light(LightName.Light0, LightParameter.Ambient, 
                         GlobalSetting.Light0Setting.AmbientColor);

            }
            else
            {
                GL.Disable(EnableCap.Light0);
            }
        }


        public void SetLight1Direct()
        {
            if (GlobalSetting.Light1Setting.Enabled)
            {
                GL.Enable(EnableCap.Light1);
                Vector4 pos = new Vector4((float)GlobalSetting.Light1Setting.LightPosition.x,
                                   (float)GlobalSetting.Light1Setting.LightPosition.y,
                                   (float)GlobalSetting.Light1Setting.LightPosition.z,
                                   0.0f);
                GL.Light(LightName.Light1, LightParameter.Position, pos);
                GL.Light(LightName.Light1, LightParameter.Diffuse, GlobalSetting.Light1Setting.DiffuseColor);
                GL.Light(LightName.Light1, LightParameter.Specular, GlobalSetting.Light1Setting.SpecularColor);
                GL.Light(LightName.Light1, LightParameter.Ambient, GlobalSetting.Light1Setting.AmbientColor);
            }
            else
            {
                GL.Disable(EnableCap.Light1);
            }
        }

        public void SetLight2Direct()
        {
            if (GlobalSetting.Light2Setting.Enabled)
            {
                GL.Enable(EnableCap.Light2);
                Vector4 pos = new Vector4((float)GlobalSetting.Light2Setting.LightPosition.x,
                                  (float)GlobalSetting.Light2Setting.LightPosition.y,
                                  (float)GlobalSetting.Light2Setting.LightPosition.z,
                                  0.0f);
                GL.Light(LightName.Light2, LightParameter.Position, pos);
                GL.Light(LightName.Light2, LightParameter.Diffuse, GlobalSetting.Light2Setting.DiffuseColor);
                GL.Light(LightName.Light2, LightParameter.Specular, GlobalSetting.Light2Setting.SpecularColor);
                GL.Light(LightName.Light2, LightParameter.Ambient, GlobalSetting.Light2Setting.AmbientColor);
            }
            else
            {
                GL.Disable(EnableCap.Light2);
            }
        }


        public void SetLight3Direct()
        {
            if (GlobalSetting.Light3Setting.Enabled)
            {
                GL.Enable(EnableCap.Light3);
                Vector4 pos = new Vector4((float)GlobalSetting.Light3Setting.LightPosition.x,
                                (float)GlobalSetting.Light3Setting.LightPosition.y,
                                (float)GlobalSetting.Light3Setting.LightPosition.z,
                                0.0f);
                GL.Light(LightName.Light3, LightParameter.Position, pos);
                GL.Light(LightName.Light3, LightParameter.Diffuse, GlobalSetting.Light3Setting.DiffuseColor);
                GL.Light(LightName.Light3, LightParameter.Specular, GlobalSetting.Light3Setting.SpecularColor);
                GL.Light(LightName.Light3, LightParameter.Ambient, GlobalSetting.Light3Setting.AmbientColor);
            }
            else
            {
                GL.Disable(EnableCap.Light3);
            }
        }


        public void SetLight4Direct()
        {
            if (GlobalSetting.Light4Setting.Enabled)
            {
                GL.Enable(EnableCap.Light4);
                Vector4 pos = new Vector4((float)GlobalSetting.Light4Setting.LightPosition.x,
                                (float)GlobalSetting.Light4Setting.LightPosition.y,
                                (float)GlobalSetting.Light4Setting.LightPosition.z,
                                0.0f);
                GL.Light(LightName.Light4, LightParameter.Position, pos);
                GL.Light(LightName.Light4, LightParameter.Diffuse, GlobalSetting.Light4Setting.DiffuseColor);
                GL.Light(LightName.Light4, LightParameter.Specular, GlobalSetting.Light4Setting.SpecularColor);
                GL.Light(LightName.Light4, LightParameter.Ambient, GlobalSetting.Light4Setting.AmbientColor);
            }
            else
            {
                GL.Disable(EnableCap.Light4);
            }
        }


        public void SetLight5Direct()
        {
            if (GlobalSetting.Light5Setting.Enabled)
            {
                GL.Enable(EnableCap.Light5);
                Vector4 pos =new Vector4((float)GlobalSetting.Light5Setting.LightPosition.x,
                                (float)GlobalSetting.Light5Setting.LightPosition.y,
                                (float)GlobalSetting.Light5Setting.LightPosition.z,
                                0.0f);
                GL.Light(LightName.Light5, LightParameter.Position, pos);
                GL.Light(LightName.Light5, LightParameter.Diffuse, GlobalSetting.Light5Setting.DiffuseColor);
                GL.Light(LightName.Light5, LightParameter.Specular, GlobalSetting.Light5Setting.SpecularColor);
                GL.Light(LightName.Light5, LightParameter.Ambient, GlobalSetting.Light5Setting.AmbientColor);
            }
            else
            {
                GL.Disable(EnableCap.Light5);
            }
        }



        public void SetDoubleSideLight()
        {
            if (GlobalSetting.SettingLight.DoubleSideLight)
            {
                GL.LightModel(LightModelParameter.LightModelTwoSide, 1.0f);
                GL.Disable(EnableCap.CullFace);
            }
            else
            {
                GL.LightModel(LightModelParameter.LightModelTwoSide, 0.0f);
                GL.Enable(EnableCap.CullFace);
            }
        }



        public void DrawLight()
        {   
            if (GlobalSetting.Light0Setting.EnabledDraw)
            { 
                DrawBall(0,  GlobalSetting.Light0Setting.LightPosition);
            }
            if (GlobalSetting.Light1Setting.EnabledDraw)
            {
                DrawBall(1, GlobalSetting.Light1Setting.LightPosition);
            }
            if (GlobalSetting.Light2Setting.EnabledDraw)
            {
                DrawBall(2, GlobalSetting.Light2Setting.LightPosition);
            }
            if (GlobalSetting.Light3Setting.EnabledDraw)
            {
                DrawBall(3, GlobalSetting.Light3Setting.LightPosition);
            }
            if (GlobalSetting.Light4Setting.EnabledDraw)
            {
                DrawBall(4, GlobalSetting.Light4Setting.LightPosition);
            }
            if (GlobalSetting.Light5Setting.EnabledDraw)
            {
                DrawBall(5, GlobalSetting.Light5Setting.LightPosition);
            }
            if (GlobalSetting.Light6Setting.EnabledDraw)
            {
                DrawBall(6, GlobalSetting.Light6Setting.LightPosition);
            }
            if (GlobalSetting.Light7Setting.EnabledDraw)
            {
                DrawBall(7, GlobalSetting.Light7Setting.LightPosition);
            } 
        }

        public  void DrawBall(int colorIndex,Vector3D pos)
        {
            OpenTK.Graphics.Color4 color = 
                OpenGLTriMesh.Instance.SetRandomColor(colorIndex);
            OpenGLManager.Instance.SetColor(color);
            GL.PushMatrix();
            GL.Translate(ConvertPosition(pos)); 
            double scale = GlobalSetting.DisplaySetting.ScaleSphere;
            GL.Scale(scale, scale, scale);
            OpenGLManager.Instance.DrawSphere();
            GL.PopMatrix();
        }


    }
}
