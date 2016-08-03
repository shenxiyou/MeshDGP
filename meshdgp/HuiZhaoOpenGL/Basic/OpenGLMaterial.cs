using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao 
{
    public partial class OpenGLManager
    {

        
        public void SetMaterialInfo()
        {
            if (GlobalSetting.MaterialSetting.Enable)
            {
                GL.Disable(EnableCap.ColorMaterial);
             
            }
            else
            {
                GL.Enable(EnableCap.ColorMaterial);
                GL.ColorMaterial(MaterialFace.FrontAndBack, 
                            ColorMaterialParameter.Diffuse);
            }
            SetMaterial();
            
        }

        public void SetMaterial()
        { 
            GL.Material(MaterialFace.Front, MaterialParameter.Ambient, 
                        GlobalSetting.MaterialSetting.MaterialAmbient);
            GL.Material(MaterialFace.Front, MaterialParameter.Emission, 
                        GlobalSetting.MaterialSetting.MaterialEmission);
            GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, 
                        GlobalSetting.MaterialSetting.MaterialDiffuse);
            GL.Material(MaterialFace.Front, MaterialParameter.Specular, 
                        GlobalSetting.MaterialSetting.MaterialSpecular);
            GL.Material(MaterialFace.Front, MaterialParameter.Shininess,
                        GlobalSetting.MaterialSetting.MaterialShininess);

            GL.Material(MaterialFace.Back, MaterialParameter.Ambient, 
                        GlobalSetting.MaterialSetting.BackMaterialAmbient);
            GL.Material(MaterialFace.Back, MaterialParameter.Emission, 
                        GlobalSetting.MaterialSetting.BackMaterialEmission);
            GL.Material(MaterialFace.Back, MaterialParameter.Diffuse, 
                        GlobalSetting.MaterialSetting.BackMaterialDiffuse);
            GL.Material(MaterialFace.Back, MaterialParameter.Specular, 
                        GlobalSetting.MaterialSetting.BackMaterialSpecular);
            GL.Material(MaterialFace.Back, MaterialParameter.Shininess, 
                        GlobalSetting.MaterialSetting.BackMaterialShiness);
        }

        public void SetMaterial(OpenTK.Graphics.Color4 color)
        {   
            
            SetMaterial();
            GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, color);
        }
         
    }
}
