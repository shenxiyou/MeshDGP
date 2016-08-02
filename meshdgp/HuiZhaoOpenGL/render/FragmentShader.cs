


using System;

using OpenTK.Graphics;

namespace GraphicResearchHuiZhao
{
    public class FragmentShader : AbstractShader
    {
        public FragmentShader(string file)
            : base(file)
        {
        }

        protected override ShaderType ShaderType
        {
            get
            {
                return ShaderType.FragmentShader;
            }
        }
    }
}
