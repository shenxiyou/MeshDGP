using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class SettingLight
    {
        private bool enabled = true;
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }

        private bool doubleSideLight = true;
        public bool DoubleSideLight
        {
            get
            {
                return doubleSideLight;
            }
            set
            {
                doubleSideLight = value;
            }
        }

    }
}
