using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace GraphicResearchHuiZhao
{
    public unsafe partial class LinearSystem
    {
        #region import SuperLU functions

        [DllImport("EigenArpackUtil.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe
        int ComputeEigenNoSymmetricShiftModeCRS(
            int* index,
            int* pointer,
            double* values,
            int numberOfNoneZeros,
            int numberOfRows,
            int resultCount,
            double sigma,
            double* RealPart,
            double* ImagePart,
            double* EigenVectors
            );

        #endregion

        public Eigen ComputeEigen(SparseMatrix sparse, int count, string modelName)
        {
            if (LinearSystemInfo.Matlab)
            {
                return  ComputeEigensByMatLab(sparse, count, modelName);
            }

            else
            {
                return ComputeEigensByLib(sparse, count);
            }
        }

        public Eigen ComputeEigensByLib(SparseMatrix sparse, int count)
        {
            SparseMatrixDouble ds = new SparseMatrixDouble(sparse);
             

            Eigen eigen = ComputeEigensByLib(ds, 0.0, count);

            return eigen;
        }

        public Eigen ComputeEigensByLib(SparseMatrixDouble sparse, double sigma, int count)
        {
            int[] pCol;
            int[] iRow;
            double[] Values;
            int NNZ;

            int m = sparse.RowCount;

            sparse.ToCCS(out pCol, out iRow, out Values, out NNZ);

            double[] ImagePart = new double[count];
            double[] RealPart = new double[count];
            double[] Vectors = new double[count * m];

            fixed (int* ri = iRow, cp = pCol)
            fixed (double* val = Values, vets = Vectors, imgPart = ImagePart, relPart = RealPart)
            {
                int result = ComputeEigenNoSymmetricShiftModeCRS(ri, cp, val, NNZ, m, count, sigma, relPart, imgPart, vets);
            }

           

            List<EigenPair> list = new List<EigenPair>();

            for (int i = 0; i < count; i++)
            {
                double realPart = RealPart[i];

                 

                List<double> vector = new List<double>();

                int startIndex = i * m;
                int endIndex = i * m + m;

                for (int j = startIndex; j < endIndex; j++)
                {
                    double value = Vectors[j];
                    vector.Add(value);
                }

                EigenPair newPair = new EigenPair(realPart, vector);


                list.Add(newPair);
            }

            list.Sort();

            Eigen eigen = new Eigen();
            eigen.SortedEigens = list.ToArray();
            return eigen;
        }


        //public Eigen ComputeEigensByLib(SparseMatrixDouble sparse, double sigma, int count)
        //{
        //    int[] pCol;
        //    int[] iRow;
        //    double[] Values;
        //    int NNZ;

        //    int m = sparse.RowCount;

        //    sparse.ToCCS(out pCol, out iRow, out Values, out NNZ);

        //    double[] ImagePart = new double[count];
        //    double[] RealPart = new double[count];
        //    double[] Vectors = new double[count * m];

        //    fixed (int* ri = iRow, cp = pCol)
        //    fixed (double* val = Values, vets = Vectors, imgPart = ImagePart, relPart = RealPart)
        //    {
        //        int result = ComputeEigenNoSymmetricShiftModeCRS(ri, cp, val, NNZ, m, count, sigma, relPart, imgPart, vets);
        //    }

        //    Eigen eigen = new Eigen();

        //    for (int i = 0; i < count; i++)
        //    {
        //        double realPart = RealPart[i];

        //        if (Math.Abs(realPart) < 1e-8)
        //        {
        //            continue;
        //        }

        //        if (eigen.SortedEigens.ContainsKey(realPart))
        //        {
        //            continue;
        //        }

        //        List<double> vector = new List<double>();

        //        int startIndex = i * m;
        //        int endIndex = i * m + m;

        //        for (int j = startIndex; j < endIndex; j++)
        //        {
        //            double value = Vectors[j];
        //            vector.Add(value);
        //        }

        //        EigenPair newPair = new EigenPair(realPart, vector);


        //        eigen.SortedEigens.Add(realPart, newPair);
        //    }


        //    return eigen;
        //}



    }
}
