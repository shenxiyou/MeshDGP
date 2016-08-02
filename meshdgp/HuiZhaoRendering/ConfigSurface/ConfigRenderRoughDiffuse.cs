using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class ConfigRenderRoughDiffuse
    {
        private static ConfigRenderRoughDiffuse singleton = new ConfigRenderRoughDiffuse();


        public static ConfigRenderRoughDiffuse Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ConfigRenderRoughDiffuse();
                return singleton;
            }
        }


    }
}
