using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class SVD2
    {
        public Matrix3D U = new Matrix3D();
        public Matrix3D V = new Matrix3D();
        public Vector3D E = new Vector3D();

        public SVD2(Matrix3D m)
        {
            double[,] uArray = new double[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    uArray[i, j] = m[i, j];
                }
            }
            double[,] vArray;
            double[] eArray;
            ASVD.svdcmp(uArray, out eArray, out vArray);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    U[i, j] = uArray[i, j];
                    V[i, j] = vArray[i, j];
                }
                E[i] = eArray[i];
            }
        }
    }
}
