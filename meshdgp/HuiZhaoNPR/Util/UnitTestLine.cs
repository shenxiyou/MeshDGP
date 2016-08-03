using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class UnitTestLine
    {
        private static ConfigNPR singleton = new ConfigNPR();
        public static ConfigNPR Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ConfigNPR();
                return singleton;
            }
        }

        public void TestEqual()
        {
            
        }
    }
}
