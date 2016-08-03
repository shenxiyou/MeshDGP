using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao 
{
    public partial class OpenGLManager
    {
        public   uint FirstTexture = 0;

        public   uint SecondTexture = 0;

        public   void ReleaseTexture(uint texture)
        {
            GL.DeleteTextures(1, ref texture);
        }


        public List<TextureInfo> TextureList = new List<TextureInfo>();

        public TextureInfo CurrentTexture = null;
        public TextureInfo[] CubeMapTexture = new TextureInfo[6];

        public void LoadTexture(string textureFile)
        {
            TextureInfo info = new TextureInfo(0, textureFile);
            //CurrentTexture = info;
            TextureList.Add(info);
        }


        public void LoadFirstTexture(string textureFile)
        {

            if (textureFile.EndsWith("dds"))
            {
                FirstTexture = LoadDDS(textureFile);
            }
            else
            {
                LoadBitmap(textureFile);
            }


        }

        public void LoadSecondTexture(string textureFile)
        {
            // GL.ActiveTexture(TextureUnit.Texture1);
            if (textureFile.EndsWith("dds"))
            {
                SecondTexture = LoadDDS(textureFile);
            }
            else
            {
                LoadBitmap(textureFile);
            }

        }

        //public void LoadCubeMapTexture(string textureFile)
        //{
        //    uint texture = 0;
        //    Bitmap bitmap = new Bitmap(fileName);

        //    //int texture;
        //    GL.Enable(EnableCap.Texture2D);

        //    GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

        //    GL.GenTextures(1, out texture);

        //    GL.BindTexture(TextureTarget.TextureCubeMap, texture);

        //    BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
        //        ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        //    GL.TexImage2D(TextureTarget.TextureCubeMap, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
        //        OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

        //    bitmap.UnlockBits(data);
        //    SetUpTextureParameter();

        //    return texture;
        //}

        public uint InitTexture(string textureFile)
        {
            uint textureID = 0;
            if (textureFile.EndsWith("dds"))
            { 
                LoadDDS(textureFile);  
            }
            else
            {
                textureID = GenerateTextureID();
                GL.BindTexture(TextureTarget.Texture2D, textureID);
                LoadBitmap(textureFile);
                
            }
            return textureID;
        }

        public uint InitCubeMapTexture(string textureFile, int index)
        {
            uint textureID = 0;
            if (textureFile.EndsWith("dds"))
            {
                throw new Exception("不支持");
            }
            else
            {
                textureID = GenerateTextureID();
                GL.BindTexture(TextureTarget.TextureCubeMap, textureID);
                LoadCubeMap(textureFile, index);
            }
            return textureID;
        }

        public  uint GenerateTextureID()
        { 
            uint textureID = 0;
            GL.GenTextures(1, out textureID);       
            return textureID;
        }

        public void SetTextureState()
        {
            if (GlobalSetting.TextureSetting.EnableTexture)
            {
                EnableTextureOne();
                SetTextureEnvMode();
                SetUpTextureParameter();
                SetAnimationTexture();
                SetAutoTexture(); 
            }
            else
            {
                DisableTexture();
            } 
        }

        public void SetCubeMapState()
        {
            if (GlobalSetting.TextureSetting.CubeMap)
            {
                GL.Disable(EnableCap.Texture2D);

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.Enable(EnableCap.TextureCubeMap);

                SetTextureEnvMode();
                SetUpCubeMapParameter();
            }
            else
            {
                GL.Disable(EnableCap.TextureCubeMap);
            }
        }

        public void DisableTexture()
        {
            GL.Disable(EnableCap.Texture2D);
            //GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void EnableTextureOne()
        { 
          GL.ActiveTexture(TextureUnit.Texture0);
          GL.Enable(EnableCap.Texture2D); 
        }

        public void EnableTwoTexture()
        {
            if (OpenGLManager.Instance.TextureList.Count >= 2)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, TextureList[0].ID);

                GL.ActiveTexture(TextureUnit.Texture1);
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, TextureList[1].ID);
            }
        }

        public void EnableMulitTexture()
        {
            if (GlobalSetting.TextureSetting.MultiTexture)
            {
                EnableTwoTexture();
            }
            else
            {
                 
            }
        }


        public void SetTextureEnvMode()
        {
            GL.TexEnv(TextureEnvTarget.TextureEnv, 
                      TextureEnvParameter.TextureEnvMode, 
                      (float)GlobalSetting.TextureSetting.EnvMode);
            GL.TexEnv(TextureEnvTarget.TextureEnv, 
                      TextureEnvParameter.TextureEnvColor, 
                      GlobalSetting.TextureSetting.EnvColor);
        }

        public void SetAnimationTexture()
        {
            if (GlobalSetting.TextureSetting.AnimationTexture)
            {
                GL.MatrixMode(MatrixMode.Texture);
                GL.Translate(GlobalSetting.TextureSetting.Transform.x,
                             GlobalSetting.TextureSetting.Transform.y,
                             GlobalSetting.TextureSetting.Transform.z);
                GL.Scale(GlobalSetting.TextureSetting.Scale.x,
                         GlobalSetting.TextureSetting.Scale.y,
                         GlobalSetting.TextureSetting.Scale.z);
                GL.Rotate(GlobalSetting.TextureSetting.Rotation.x,
                          GlobalSetting.TextureSetting.Rotation.y,
                          GlobalSetting.TextureSetting.Rotation.z,
                          GlobalSetting.TextureSetting.RotationAngle);
            }
        }



        public   void SetUpTextureParameter()
        {
            GL.Hint(GlobalSetting.TextureSetting.HintTarget, 
                    GlobalSetting.TextureSetting.Hint);
            GL.TexParameter(TextureTarget.Texture2D, 
                            TextureParameterName.TextureMinFilter, 
                            (int)GlobalSetting.TextureSetting.MinFilter);
            GL.TexParameter(TextureTarget.Texture2D, 
                            TextureParameterName.TextureMagFilter, 
                            (int)GlobalSetting.TextureSetting.MagFilter);
            GL.TexParameter(TextureTarget.Texture2D, 
                            TextureParameterName.TextureWrapS, 
                            (int)GlobalSetting.TextureSetting.WrapModeS);
            GL.TexParameter(TextureTarget.Texture2D, 
                            TextureParameterName.TextureWrapT, 
                            (int)GlobalSetting.TextureSetting.WrapModeT);
            GL.TexParameter(TextureTarget.Texture2D, 
                            TextureParameterName.TextureBorderColor,
                new float[]{GlobalSetting.TextureSetting.BorderColor.R/256f,
                    GlobalSetting.TextureSetting.BorderColor.G/256f,
                    GlobalSetting.TextureSetting.BorderColor.B/256f,
                    GlobalSetting.TextureSetting.BorderColor.A/256f});
        }

        public void SetUpCubeMapParameter()
        {
            GL.Hint(GlobalSetting.TextureSetting.HintTarget, GlobalSetting.TextureSetting.Hint);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)GlobalSetting.TextureSetting.MinFilter);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)GlobalSetting.TextureSetting.MagFilter);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)GlobalSetting.TextureSetting.WrapModeS);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)GlobalSetting.TextureSetting.WrapModeT);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureBorderColor,
                new float[]{GlobalSetting.TextureSetting.BorderColor.R/256f,
                    GlobalSetting.TextureSetting.BorderColor.G/256f,
                    GlobalSetting.TextureSetting.BorderColor.B/256f,
                    GlobalSetting.TextureSetting.BorderColor.A/256f});
        }

        


        public void SetAutoTexture()
        {
            if (GlobalSetting.TextureSetting.AutoTexture)
            {
                EnableAutoTexture();
            }
            else
            {
                DisableAutoTexture();
            }
        }

        public void EnableAutoTexture()
        {
            GL.Enable(EnableCap.TextureGenS);
            GL.Enable(EnableCap.TextureGenT);
            GL.Enable(EnableCap.TextureGenR);  
            
            GL.TexGen(TextureCoordName.S, TextureGenParameter.TextureGenMode, 
                (float)GlobalSetting.TextureSetting.AutoGenMode);
            GL.TexGen(TextureCoordName.T, TextureGenParameter.TextureGenMode, 
                (float)GlobalSetting.TextureSetting.AutoGenMode);
            GL.TexGen(TextureCoordName.R, TextureGenParameter.TextureGenMode, 
                (float)GlobalSetting.TextureSetting.AutoGenMode);
            GL.TexGen(TextureCoordName.S, GlobalSetting.TextureSetting.AutoGenPara, 
                GlobalSetting.TextureSetting.PlaneS.ToArray());
            GL.TexGen(TextureCoordName.T, GlobalSetting.TextureSetting.AutoGenPara, 
                GlobalSetting.TextureSetting.PlaneT.ToArray());
            GL.TexGen(TextureCoordName.R, GlobalSetting.TextureSetting.AutoGenPara, 
                GlobalSetting.TextureSetting.PlaneR.ToArray());
        }

        public void DisableAutoTexture()
        {    

            GL.Disable(EnableCap.TextureGenS);
            GL.Disable(EnableCap.TextureGenT);
            GL.Disable(EnableCap.TextureGenQ);
            GL.Disable(EnableCap.TextureGenR);
             
        }

       


        public void UseFirstTexture()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, OpenGLManager.Instance.FirstTexture);
        }

        public void UseSecondTexture()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, OpenGLManager.Instance.SecondTexture);
        }

        public void UseColorTexture()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, OpenGLManager.Instance.ColorTexture);
        }

        public void UseDeptheTexture()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, OpenGLManager.Instance.DepthTexture);

        }

        public void DisableTextureTwo()
        {
            GL.Disable(EnableCap.Texture2D);
        }

        //public int GetNumOfTextureUnit()
        //{
        //    return GL.GetInteger(GetPName.MaxTextureUnits);
        //}
    }
}
