using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class SettingLightPoint:SettingLightDirect
    {

        public SettingLightPoint(float x, float y, float z, bool enable)
            : base(x, y, z, enable)
        {
        }
        public float linearAttenuation = 0.2f;
        public float LinearAttenuation
        {
            get
            {
                return linearAttenuation;
            }
            set
            {
                linearAttenuation = value;
            }
        }
        public float constantAttenuation = 0.2f;
        public float ConstantAttenuation
        {
            get
            {
                return constantAttenuation;
            }
            set
            {
                constantAttenuation = value;
            }
        }
        public float quadraticAttenuation = 0.2f;
        public float QuadraticAttenuation
        {
            get
            {
                return quadraticAttenuation;
            }
            set
            {
                quadraticAttenuation = value;
            }
        }
    }
}
