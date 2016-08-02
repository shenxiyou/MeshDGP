using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class ConfigAniamtion
    {
        private static ConfigAniamtion singleton = new ConfigAniamtion();

        public static ConfigAniamtion Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ConfigAniamtion();
                return singleton;
            }
        }


        private int time = 11;
        public int Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
            }
        }

    }
}
