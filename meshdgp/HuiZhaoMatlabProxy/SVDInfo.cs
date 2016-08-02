using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class SVDInfo
    {
        public Matrix3D A = null;
        public Matrix3D U = null;
        public Matrix3D S = null;
        public Matrix3D V = null;
        public Matrix3D R = null;
        public double DetA;
        public double DetU;
        public double DetS;
        public double DetV;
        public double DetR;
    }
}
