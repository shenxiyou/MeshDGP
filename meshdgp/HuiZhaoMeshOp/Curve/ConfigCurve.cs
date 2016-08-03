using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class ConfigCurve
    {
        private static ConfigCurve singleton = new ConfigCurve();  
        public static ConfigCurve Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ConfigCurve();
                return singleton;
            }
        }

        private int controlPointNum = 10;
        public int ControlPointNum
        {
            get
            {
                return controlPointNum;
            }
            set
            {
                controlPointNum = value;
            }
        }


        private int curveNUM = 4000;
        public int CurveNUM
        {
            get
            {
                return curveNUM;
            }
            set
            {
                curveNUM = value;
            }
        }

    }
}
