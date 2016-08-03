using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class ConfigMatlab
    {

        private static ConfigMatlab singleton = new ConfigMatlab();

        public static ConfigMatlab Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ConfigMatlab();
                return singleton;
            }
        }



        public string APPPath
        {
            get
            {
              return  MatlabProxy.Instance.AppPath;
            }

            set
            {
                MatlabProxy.Instance.AppPath = value;
            }
        }

        public int EigenNum
        {
            get
            {
                return MatlabProxy.Instance.EigenNum;
            }

            set
            {
                MatlabProxy.Instance.EigenNum = value;
            }
        }

        private bool outPutInfo = true;
        public bool OutPutInfo
        {
            get
            {
                return outPutInfo;
            }
            set
            {
                outPutInfo = value;
            }
        }
    }
}
