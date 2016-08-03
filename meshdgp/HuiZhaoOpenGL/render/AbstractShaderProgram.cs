
using System; 
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using OpenTK;

using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public class AbstractGPURender : IDisposable
    {
        protected   int handle;
        private   VertexShader vertexShader=null;
        private   FragmentShader fragmentShader=null;

       
        protected  string path ;

         

        public  string VSFileName
        {
            get;
            set;
        }
        public  string FSFileName
        {
            get;
            set;
        }

        public AbstractGPURender()
        {
           // path = Application.StartupPath + "/";
            VSFileName = string.Empty;
            FSFileName = string.Empty;
        }
        

        public void Use()
        {
            GL.UseProgram(this.handle);
            // TODO check for errors
        }

        public void Dispose()
        {
            if (this.vertexShader != null)
            {
                GL.DetachShader(this.handle, this.vertexShader.handle);
                this.vertexShader.Dispose();
            }
         

            if (this.fragmentShader != null)
            {
                GL.DetachShader(this.handle, this.fragmentShader.handle);
                this.fragmentShader.Dispose();
            }
          

            GL.DeleteProgram(this.handle);
        }

        private Dictionary<string, int> uniformLocations = new Dictionary<string, int>();
        private int FindUniformLocation(string name)
        {
            int location;
            if (uniformLocations.TryGetValue(name, out location)) return location;

            location = GL.GetUniformLocation(this.handle, name);
            if (location == -1)
            {
                throw new Exception(string.Format("Unable to find uniform variable {0}", name));
            }

            uniformLocations[name] = location;

            return location;
        }

        private Dictionary<string, int> attribIndecies = new Dictionary<string, int>();
        public int FindAttribIndex(string name)
        {
            int index;
            if (attribIndecies.TryGetValue(name, out index)) return index;

            index = GL.GetAttribLocation(this.handle, name);

            attribIndecies[name] = index;

            return index;
        }

        public void SetSamplerUniform(string name, int sampler)
        {
            GL.Uniform1(FindUniformLocation(name), sampler);
        }

        public void SetSamplerUniform(string name, uint sampler)
        {
            GL.Uniform1(FindUniformLocation(name), sampler);
        }

        [Obsolete]
        public void SetMatrix(string name, Matrix4 value)
        {
            float[] matrix = new float[]
            {
            	value.Column0.X, value.Column1.X, value.Column2.X, value.Column3.X,
            	value.Column0.Y, value.Column1.Y, value.Column2.Y, value.Column3.Y,
            	value.Column0.Z, value.Column1.Z, value.Column2.Z, value.Column3.Z,
            	value.Column0.W, value.Column1.W, value.Column2.W, value.Column3.W,
            };

            GL.UniformMatrix4(FindUniformLocation(name), 1, false, matrix);
           
        }

        public void SetMatrix4(string name, float[] values)
        {
            int count = values.Length / 16;
            if (count <= 0) return;

            GL.UniformMatrix4(FindUniformLocation(name), count, false, values);
        }

        public void SetMatrix3(string name, float[] values)
        {
            int count = values.Length / 9;
            if (count <= 0) return;

            GL.UniformMatrix3(FindUniformLocation(name), count, false, values);
        }

        public void SetVector3(string name, Vector3 value)
        {
            GL.Uniform3(FindUniformLocation(name), value.X, value.Y, value.Z);
        }

        public void SetFloat(string name, float value)
        {
            GL.Uniform1(FindUniformLocation(name), value);
        }




        private void LoadCompileVSFile()
        {
            if (this.VSFileName != string.Empty)
            {
                Trace.WriteLine(string.Format("Loading vertex shader {0}", VSFileName));
                this.vertexShader = new VertexShader(path + VSFileName);

                
            }
        }

        private void LoadCompileFSFile()
        {
            // Load&Compile Fragment Shader
            if (this.FSFileName != string.Empty)
            {
                Trace.WriteLine(string.Format("Loading fragment shader {0}", FSFileName));
                this.fragmentShader = new FragmentShader(path + FSFileName);

            }

           
        }

        private void CheckGPUAbility()
        {

            // Check for necessary capabilities:
            string extensions = GL.GetString(StringName.Extensions);
            if (!GL.GetString(StringName.Extensions).Contains("GL_ARB_shading_language"))
            {
                throw new NotSupportedException(String.Format("This example requires OpenGL 2.0. Found {0}. Aborting.",
                    GL.GetString(StringName.Version).Substring(0, 3)));
            }

            if (!extensions.Contains("GL_ARB_texture_compression") ||
                 !extensions.Contains("GL_EXT_texture_compression_s3tc"))
            {
                throw new NotSupportedException("This example requires support for texture compression. Aborting.");
            }
            Trace.WriteLine(string.Format("Extension: {0}", extensions));

        }


        private void LinkProgram()
        {
            // Link the Shaders to a usable Program
            this.handle = GL.CreateProgram();
            GL.AttachShader(this.handle,this.vertexShader.handle);
            if (FSFileName != string.Empty)
            {
                GL.AttachShader(this.handle,this.fragmentShader.handle);
            }
            // link it all together
            GL.LinkProgram(this.handle);

            // flag ShaderObjects for delete when not used anymore
            vertexShader.Dispose();

           // GL.DeleteShader(VertexShaderObject);
            if (FSFileName != string.Empty)
            {
                fragmentShader.Dispose();
               // GL.DeleteShader(FragmentShaderObject);
            }


            CheckAfterLink();

        }

        private void CheckAfterLink()
        {
            int[] results = new int[1];
            GL.GetProgram(this.handle, ProgramParameter.LinkStatus, results);
            Trace.WriteLine("Linking Program (" + this.handle + ") " + ((results[0] == 1) ? "succeeded." : "FAILED!"));
            if (results[0] == 0)
            {
                GL.GetProgram(this.handle, ProgramParameter.InfoLogLength, results);

                System.Text.StringBuilder infoLog = new System.Text.StringBuilder(results[0]);
                int length;
                GL.GetProgramInfoLog(this.handle, results[0], out length, infoLog);
                Trace.WriteLine("Program Log:\n" + infoLog);

                throw new Exception(string.Format("Shader program link failed: {0}", infoLog));
            }



            GL.ValidateProgram(this.handle);
            GL.GetProgram(this.handle, ProgramParameter.ValidateStatus, results);
            if (results[0] == 0)
            {
                GL.GetProgram(this.handle, ProgramParameter.InfoLogLength, results);

                System.Text.StringBuilder infoLog = new System.Text.StringBuilder(results[0]);
                int length;
                GL.GetProgramInfoLog(this.handle, results[0], out length, infoLog);
                Trace.WriteLine("Program Log:\n" + infoLog);
                throw new Exception(string.Format("Shader program validate failed: {0}", infoLog));
            }



            GL.GetProgram(this.handle, ProgramParameter.ActiveAttributes, out results[0]);
            Trace.WriteLine("Program registered " + results[0] + " Attributes. (Should be 4: Pos, UV, Normal, Tangent)");

            Trace.WriteLine("Tangent attribute bind location: " + GL.GetAttribLocation(this.handle, "Tangent"));

            Trace.WriteLine("End of Shader build. GL Error: " + GL.GetError());




            GL.GetProgram(this.handle, ProgramParameter.ActiveAttributeMaxLength, out results[0]);
            Trace.WriteLine("ActiveAttributeMaxLength: " + results[0]);

            GL.GetProgram(this.handle, ProgramParameter.ActiveUniformMaxLength, out results[0]);
            Trace.WriteLine("ActiveUniformMaxLength: " + results[0]);

            GL.GetProgram(this.handle, ProgramParameter.ActiveUniforms, out results[0]);
            Trace.WriteLine("ActiveUniforms: " + results[0]);

   

        }

        /// <summary>Setup OpenGL and load resources here.</summary>
        /// <param name="e">Not used.</param>
        private void LoadCompileLinkGPUFile()
        {
            CheckGPUAbility();
            LoadCompileVSFile();
            LoadCompileFSFile();
            BindAttribute();
            LinkProgram();


        }

        public void Init()
        {
            this.LoadCompileLinkGPUFile();
            this.SetUpGPU();
        }

        protected virtual void BindAttribute()
        {

        }


        public void SetUpGPU()
        {

            this.Use();
            PassParameter();
       

        }


        protected virtual void RenderShape()
        {
        }

        public virtual void PassParameter()
        {
        }

        public virtual void SetupParameter()
        {
            this.SetUpGPU();
        }


        public void SendUniformli(string parameter, uint value)
        {
            GL.Uniform1(GL.GetUniformLocation(this.handle, parameter), value);
        }

        public void SendUniformlf(string parameter, float value)
        {
            GL.Uniform1(GL.GetUniformLocation(this.handle, parameter), value);
        }

    }
}
