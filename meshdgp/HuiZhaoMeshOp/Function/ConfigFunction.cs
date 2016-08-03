using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class ConfigFunction
    {
        private static ConfigFunction singleton = new ConfigFunction();


        public static ConfigFunction Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ConfigFunction();
                return singleton;
            }
        }

        public double cos = 10;

        public double Cos
        {
            get
            {
                return cos;
            }

            set
            {
                cos = value;
            }
        }



        public double min = 0;

        public double Min 
        {
            get
            {
                return min ;
            }
            

        }

        public double max = 0;

        public double Max 
        {
            get
            {
                return max ;
            }
            

        }



        private double minClamp = 0;

        public double MinClamp
        {
            get
            {
                return minClamp;
            }
            set
            {
                minClamp = value;
            }

        }

        private double maxClamp = 0;

        public double MaxClamp
        {
            get
            {
                return maxClamp;
            }
            set
            {
                maxClamp = value;
            }

        }



        private double adf = 1.5;

        public double ADF
        {
            get
            {
                return adf;
            }
            set
            {
                adf = value;
            }
            
        }

        private double diffustion = 1.5;

        public double Diffustion
        {
            get
            {
                return diffustion;
            }
            set
            {
                diffustion = value;
            }

        }

        private int eigenCount = 50;

        public int EigenCount
        {
            get
            {
                return eigenCount;
            }
            set
            {
                eigenCount = value;
            }

        }


        private int eigenIndex = 0;

        public int EigenIndex
        {
            get
            {
                return eigenIndex;
            }
            set
            {
                eigenIndex = value;
            }

        }


        private double harmonicMin = 10;
        public double HarmonicMin
        {
            get
            {
                return harmonicMin;
            }
            set
            {
                harmonicMin = value;
            }
        }

        private double harmonicMax = 100;
        public double HarmonicMax
        {
            get
            {
                return harmonicMax;
            }
            set
            {
                harmonicMax = value;
            }
        }

        private bool nodal = false ;
        public bool Nodal
        {
            get
            {
                return nodal;
            }
            set
            {
                nodal = value;
            }
        }

        private int harmonicType = 1;
        public int HarmoincType
        {
            get
            {
                return harmonicType;
            }
            set
            {
                harmonicType = value;
            }
        }
       
    }
}
