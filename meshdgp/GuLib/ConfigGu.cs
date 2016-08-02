using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class ConfigGu
    {
        private static ConfigGu singleton = new ConfigGu();

        public static ConfigGu Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ConfigGu();
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


        private int iterCount = 1000;
        public int IterCount
        {
            get
            {
                return iterCount;
            }
            set
            {
                iterCount = value;
            }
        }


        private double step = 0.0001;
        public double Step
        {
            get
            {
                return step;
            }
            set
            {
                step = value;
            }
        }

    }
}
