using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class OpenGLNPRData
    {
        private static OpenGLNPRData singleton = new OpenGLNPRData();


        public static OpenGLNPRData Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new OpenGLNPRData();
                return singleton;
            }
        }


        public bool ExteriroSilhoutte = true;


        public bool HiddenLine = true;

    }
}
