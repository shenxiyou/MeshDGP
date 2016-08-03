using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class ConfigRenderSmoothDiffuse
    {
        private static ConfigRenderSmoothDiffuse singleton = new ConfigRenderSmoothDiffuse();


        public static ConfigRenderSmoothDiffuse Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ConfigRenderSmoothDiffuse();
                return singleton;
            }
        }


    }
}
