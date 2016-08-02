using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class PrincipalCurvature
    {
        public Vector3D maxDir;
        public Vector3D minDir;
        public double max;
        public double min;

        public override string ToString()
        {
            return string.Format("Max: {0}, Dir: {1}\nMin: {2}, Dir: {3}", max, maxDir, min, minDir);
        }
    }
}
