using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public struct UV
    {
        public Vector3D U;
        public Vector3D V;

        public override string ToString()
        {
            return string.Format("U: {0}, V: {1}", U, V);
        }
    }

    public struct KUV
    {
        public double U;
        public double V;
        public double UV;

        public override string ToString()
        {
            return string.Format("U: {0}, UV: {1}, V: {2}", U, UV, V);
        }
    }
}
