


using System;
using System.IO;
using System.Diagnostics;

using OpenTK;
using OpenTK.Graphics;

namespace GraphicResearchHuiZhao
{
    public abstract class AbstractShader : IDisposable
    {
        public readonly string file;
        public readonly int handle;

        public AbstractShader(string file)
        {
            this.file = file;

            string sourceCode;
            using (StreamReader reader = File.OpenText(this.file))
            {
                sourceCode = reader.ReadToEnd();
            }

            this.handle = GL.CreateShader(this.ShaderType);

            GL.ShaderSource(this.handle, sourceCode);
            GL.CompileShader(this.handle);

            int[] results = new int[1];
            GL.GetShader(this.handle, ShaderParameter.CompileStatus, results);

            if (results[0] == 0)
            {
                GL.GetShader(this.handle, ShaderParameter.InfoLogLength, results);

                string info;
                GL.GetShaderInfoLog(this.handle, out info);

                throw new Exception(string.Format("Shader error: {0}", info));
            }
        }

        public void Dispose()
        {
            if (this.handle != 0)
            {
                GL.DeleteShader(this.handle);
            }
        }

        protected abstract ShaderType ShaderType
        {
            get;
        }
    }
}
