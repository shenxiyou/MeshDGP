


using System;

using OpenTK.Graphics;

namespace GraphicResearchHuiZhao
{
    class VertexShader : AbstractShader
    {
        public VertexShader(string file)
            : base(file)
        {
        }

        protected override ShaderType ShaderType
        {
            get
            {
                return ShaderType.VertexShader;
            }
        }
    }
}
