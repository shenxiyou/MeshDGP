using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class SVDInfoCPlus
    {
        public Matrix3D A;
        public Matrix3D U;
        public Vector3D S;
        public Matrix3D V;
        public Matrix3D R;
        public Matrix3D T;

        public double DetA;
        public double DetU;

        public double DetV;
        public double DetR;
        public double DetT;



        public SVDInfoCPlus()
        {
            A = new Matrix3D();
            U = new Matrix3D();
            V = new Matrix3D();
            R = new Matrix3D();
            T = new Matrix3D();
            S = new Vector3D();
        }
    }

}
     
