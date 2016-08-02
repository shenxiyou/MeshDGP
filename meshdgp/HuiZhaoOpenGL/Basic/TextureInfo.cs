using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class TextureInfo
    {
        public uint ID = 0;
        public String FileName = string.Empty;

        public TextureInfo(uint id, string fileName)
        {
            this.ID = id;
            this.FileName = fileName;
        }
    }
}
