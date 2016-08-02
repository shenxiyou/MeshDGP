using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing; 
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao 
{
    public partial class OpenGLManager
    {
        public void LoadBitmap(string fileName)
        {
            Bitmap bitmap = new Bitmap(fileName);  

            BitmapData data = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, 
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, 
                PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, 
                PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data); 
        }

        public void LoadCubeMap(string fileName, int index)
        {
            LoadCubeMap(fileName, TextureTarget.TextureCubeMapPositiveX + index);
        }

        public void LoadCubeMap(string fileName, TextureTarget target)
        {
            Bitmap bitmap = new Bitmap(fileName);

            BitmapData data = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(target, 0,
                PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data); 
        }

        private uint LoadDDS(string fileName)
        {
            uint texture = 0;
            //TextureLoaderParameters.FlipImages = false;
            //TextureLoaderParameters.MagnificationFilter = TextureMagFilter.Linear;
            //TextureLoaderParameters.MinificationFilter = TextureMinFilter.Linear;
            //TextureLoaderParameters.WrapModeS = TextureWrapMode.ClampToEdge;
            //TextureLoaderParameters.WrapModeT = TextureWrapMode.ClampToEdge;
            //TextureLoaderParameters.EnvMode = TextureEnvMode.Modulate;


            TextureLoaderParameters.MagnificationFilter = TextureMagFilter.Linear;
            TextureLoaderParameters.MinificationFilter = TextureMinFilter.LinearMipmapLinear;
            TextureLoaderParameters.WrapModeS = TextureWrapMode.ClampToBorder;
            TextureLoaderParameters.WrapModeT = TextureWrapMode.ClampToBorder;
        

            TextureTarget textureTarget;
            ImageDDS.LoadFromDisk(fileName, out texture, out textureTarget);



            return texture;
        }

    }
}
