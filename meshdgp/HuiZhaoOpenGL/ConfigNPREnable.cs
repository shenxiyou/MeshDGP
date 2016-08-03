using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class ConfigNPREnable
    {
        private static ConfigNPREnable singleton = new ConfigNPREnable(); 

        public static ConfigNPREnable Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ConfigNPREnable();
                return singleton;
            }
        }

        private bool exteriorCountour = false;
        public bool ExteriorCountour
        {
            get
            {
                return exteriorCountour;
            }
            set
            {
                exteriorCountour = value;
            }
        }
    }
}
