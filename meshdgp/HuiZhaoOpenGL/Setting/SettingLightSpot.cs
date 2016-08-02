using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class SettingLightSpot:SettingLightPoint
    {
        public SettingLightSpot(float x, float y, float z, bool enable)
            : base(x, y, z, enable)
        {
        }

        Vector3D spotDirection = new Vector3D(0, 0, -1);

        public Vector3D SpotDirection
        {
            get
            {
                return spotDirection;
            }
            set
            {
                spotDirection = value;
            }
        }
       
        public float spotCutoff = 3.0f;
        public float SpotCutoff
        {
            get
            {
                return spotCutoff;
            }
            set
            {
                spotCutoff = value;
            }
        }
        public float spotExponent = 2.0f;
        public float SpotExponent
        {
            get
            {
                return spotExponent;
            }
            set
            {
                spotExponent = value;
            }
        }
    }
}
