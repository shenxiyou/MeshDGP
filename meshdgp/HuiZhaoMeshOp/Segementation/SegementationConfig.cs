using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class SegementationConfig
    {
        private static SegementationConfig singleton = new SegementationConfig();

        public static SegementationConfig Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new SegementationConfig();
                return singleton;
            }
        }

        private double k = 0;
        public double K
        {
            get
            {
                return k;
            }
            set
            {
                k = value;
            }
        }
    }
}
