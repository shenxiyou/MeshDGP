using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class MathUtil
    {
        public static double ClampWithRange(double value, double floor, double ceiling)
        {
            return ((value < floor) ? floor : (value > ceiling) ? ceiling : value);
        }

    }
}
