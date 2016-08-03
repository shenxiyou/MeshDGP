using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace GraphicResearchHuiZhao 
{
    public partial class OpenGLManager
    {

        public List<KeyValuePair<string, string>> GetOpenGLCardInfo()
        {
            List<KeyValuePair<string, string>> info = 
                new List<KeyValuePair<string, string>>();

            info.Add(new KeyValuePair<string, string>("Renderer", 
                GL.GetString(StringName.Renderer)));
            info.Add(new KeyValuePair<string, string>("ShadingLanguageVersion", 
                GL.GetString(StringName.ShadingLanguageVersion)));
            info.Add(new KeyValuePair<string, string>("Vendor", 
                GL.GetString(StringName.Vendor)));
            info.Add(new KeyValuePair<string, string>("Version", 
                GL.GetString(StringName.Version)));

            string openGLInfo = GL.GetString(StringName.Extensions);
            string[] infos = openGLInfo.Split(' ');

            for (int i = 0; i < infos.Length; i++)
            {
                info.Add(new KeyValuePair<string, string>(i.ToString(), infos[i]));
            } 
            return info;
        }

        public List<KeyValuePair<string, string>> GetOpenGLPNameInfo()
        {
            List<KeyValuePair<string, string>> info = 
                new List<KeyValuePair<string, string>>(); 
            foreach (GetPName pname in Enum.GetValues(typeof(GetPName)))
            {
                double[] buff = new double[32];
                GL.GetDouble(pname, buff);
                int last = 0;
                for (int i = 0; i < 32; i++)
                {
                    if (buff[i] != 0.0)
                    {
                        last = i + 1;
                    }
                }
                string str = null;
                switch (last)
                {
                    case 0:
                        str = "0";
                        break;
                    case 1:
                        str = buff[0].ToString();
                        break;
                    case 2:
                        Vector2d v2 = new Vector2d(buff[0], buff[1]);
                        str = v2.ToString();
                        break;
                    case 3:
                        Vector3d v3 = new Vector3d(buff[0], buff[1], buff[2]);
                        str = v3.ToString();
                        break;
                    case 4:
                        Vector4d v4 = new Vector4d(buff[0], buff[1], buff[2], buff[3]);
                        str = v4.ToString();
                        break;
                    case 16:
                        Matrix4d m4 = new Matrix4d(buff[0], buff[1], buff[2], buff[3],
                                                buff[4], buff[5], buff[6], buff[7],
                                                buff[8], buff[9], buff[10], buff[11],
                                                buff[12], buff[13], buff[14], buff[15]);
                        str = m4.ToString();
                        break;
                    default:
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < last; i++)
                        {
                            sb.Append(buff[i]);
                            sb.Append(',');
                        }
                        str = sb.ToString();
                        break;
                }
                info.Add(new KeyValuePair<string, string>(pname.ToString(), str));
            } 
            return info;
        }


        public List<KeyValuePair<string, string>> GetOpenGLPEnableCapsInfo()
        {
            List<KeyValuePair<string, string>> info =
                new List<KeyValuePair<string, string>>();
            foreach (EnableCap cap in Enum.GetValues(typeof(EnableCap)))
            {
             bool result = GL.IsEnabled(cap);
             info.Add(new KeyValuePair<string, string>(cap.ToString(), 
                                                   result.ToString()));
            } 
            return info;
        }
    }
}
