using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class CholmodConverter
    {

        private static CholmodConverter singleton = new CholmodConverter();


        public static CholmodConverter Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new CholmodConverter();
                return singleton;
            }
        }

        private CholmodConverter()
        {

        }

        public static CholmodInfo ConverterDouble(ref SparseMatrixDouble A)
        {
            CholmodInfo info = new CholmodInfo();
            info.MatrixType = CholmodInfo.CholmodMatrixType.Sparse;
            info.RowCount = A.RowCount;
            info.ColumnCount = A.ColumnCount;

            A.ToTriplet(out info.colIndex, out info.rowIndex, out info.values, out info.nnz);

            return info;

        }

        public static CholmodInfo ConverterDouble(ref SparseMatrixDouble A, CholmodInfo.CholmodMatrixStorage storage)
        {
            CholmodInfo info = new CholmodInfo();
            info.MatrixType = CholmodInfo.CholmodMatrixType.Sparse;
            info.MatrixStorageType = storage;
            info.RowCount = A.RowCount;
            info.ColumnCount = A.ColumnCount;

            switch (storage)
            {
                case CholmodInfo.CholmodMatrixStorage.CRS:
                    A.ToCRS(out info.rowIndex, out info.colIndex, out info.values, out info.nnz);
                    break;
                case CholmodInfo.CholmodMatrixStorage.CCS:
                    A.ToCCS(out info.colIndex, out info.rowIndex, out info.values, out info.nnz);
                    break;
                case CholmodInfo.CholmodMatrixStorage.Triplet:
                    A.ToTriplet(out info.colIndex, out info.rowIndex, out info.values, out info.nnz);
                    break;
                default:
                    A.ToTriplet(out info.colIndex, out info.rowIndex, out info.values, out info.nnz);

                    break;
            }


            return info;

        }

        public static CholmodInfo ConvertDouble(ref DenseMatrixDouble b)
        {
            CholmodInfo info = new CholmodInfo();
            info.MatrixType = CholmodInfo.CholmodMatrixType.Dense;

            b.ToArray(out info.RowCount, out info.ColumnCount, out info.values);

            return info;
        }


        public static DenseMatrixDouble dConvertArrayToDenseMatrix(ref double[] x, int m, int n)
        {
            DenseMatrixDouble unknown = new DenseMatrixDouble(m, n);

            int count = 0;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    unknown[i, j] = x[count];
                    count++;
                }
            }

            return unknown;
        }


        public static CholmodInfo cConverter(ref SparseMatrixComplex A, CholmodInfo.CholmodMatrixStorage storage)
        {
            CholmodInfo info = new CholmodInfo();
            info.MatrixType = CholmodInfo.CholmodMatrixType.Sparse;
            info.MatrixStorageType = storage;
            info.MatrixStorageMethod = CholmodInfo.CholmodMatrixStoageMethod.Normal;
            info.MatrixItemType = CholmodInfo.CholmodMatrixItemType.Complex;
            info.RowCount = A.RowCount;
            info.ColumnCount = A.ColumnCount;

            switch (storage)
            {
                case CholmodInfo.CholmodMatrixStorage.CRS:
                    A.ToCRS(out info.colIndex, out info.rowIndex, out info.values, out info.nnz);
                    break;
                case CholmodInfo.CholmodMatrixStorage.CCS:
                    A.ToCCS(out info.colIndex, out info.rowIndex, out info.values, out info.nnz);
                    break;
                case CholmodInfo.CholmodMatrixStorage.Triplet:
                    A.ToTriplet(out info.colIndex, out info.rowIndex, out info.values, out info.z, out info.nnz);
                    break;
                default:
                    A.ToTriplet(out info.colIndex, out info.rowIndex, out info.values, out info.z, out info.nnz);

                    break;
            }
            return info;
        }

        public static CholmodInfo qConverter(ref SparseMatrixQuaternion A, CholmodInfo.CholmodMatrixStorage storage)
        {
            CholmodInfo info = new CholmodInfo();
            info.MatrixType = CholmodInfo.CholmodMatrixType.Sparse;
            info.MatrixStorageType = storage;
            info.MatrixStorageMethod = CholmodInfo.CholmodMatrixStoageMethod.Normal;
            info.MatrixItemType = CholmodInfo.CholmodMatrixItemType.Quaternion;
            info.RowCount = 4 * A.RowCount;
            info.ColumnCount = 4 * A.ColumnCount;

            switch (storage)
            {
                case CholmodInfo.CholmodMatrixStorage.CRS:
                    A.ToCRS(out info.colIndex, out info.rowIndex, out info.values, out info.nnz);
                    break;
                case CholmodInfo.CholmodMatrixStorage.CCS:
                    A.ToCCS(out info.colIndex, out info.rowIndex, out info.values, out info.nnz);
                    break;
                case CholmodInfo.CholmodMatrixStorage.Triplet:
                    A.ToTriplet(out info.colIndex, out info.rowIndex, out info.values, out info.nnz);
                    break;
                default:
                    A.ToTriplet(out info.colIndex, out info.rowIndex, out info.values, out info.nnz);
                    break;
            }
            return info;
        }

        public static CholmodInfo cConverter(ref SparseMatrixComplex A, CholmodInfo.CholmodMatrixStorage storage, CholmodInfo.CholmodMatrixStoageMethod method)
        {
            CholmodInfo info = new CholmodInfo();
            info.MatrixType = CholmodInfo.CholmodMatrixType.Sparse;
            info.MatrixStorageType = storage;
            info.MatrixStorageMethod = method;
            info.MatrixItemType = CholmodInfo.CholmodMatrixItemType.Complex;
            info.RowCount = A.RowCount;
            info.ColumnCount = A.ColumnCount;

            double[] values = null;

            switch (storage)
            {
                case CholmodInfo.CholmodMatrixStorage.CRS:
                    A.ToCRS(out info.colIndex, out info.rowIndex, out values, out info.nnz);
                    break;
                case CholmodInfo.CholmodMatrixStorage.CCS:
                    A.ToCCS(out info.colIndex, out info.rowIndex, out values, out info.nnz);
                    break;
                case CholmodInfo.CholmodMatrixStorage.Triplet:
                    A.ToTriplet(out info.colIndex, out info.rowIndex, out values, out info.z, out info.nnz);
                    break;
                default:
                    A.ToTriplet(out info.colIndex, out info.rowIndex, out values, out info.z, out info.nnz);

                    break;
            }

            switch (method)
            {
                case CholmodInfo.CholmodMatrixStoageMethod.Normal:
                    info.values = values;
                    break;
                case CholmodInfo.CholmodMatrixStoageMethod.Divided:
                    double[] realParts = new double[info.nnz];
                    double[] imgParts = new double[info.nnz];

                    for (int i = 0; i < info.nnz; i++)
                    {
                        realParts[i] = values[2 * i];
                        imgParts[i] = values[2 * i + 1];
                    }

                    info.values = realParts;
                    info.z = imgParts;

                    values = null;

                    break;
                default:
                    break;
            }

            GC.Collect();
            return info;

        }

        public static CholmodInfo cConverter(ref DenseMatrixComplex b)
        {
            CholmodInfo info = new CholmodInfo();
            info.MatrixType = CholmodInfo.CholmodMatrixType.Dense;

            b.ToArray(out info.RowCount, out info.ColumnCount, out info.values);    //Normal

            return info;
        }

        public static DenseMatrixComplex cConvertArrayToDenseMatrix(ref double[] x, ref double[] z, int m, int n)
        {
            DenseMatrixComplex unknown = new DenseMatrixComplex(m, n);

            int count = 0;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Complex value = new Complex(x[count], z[count]);
                    unknown[i, j] = value;
                    count++;
                }
            }

            return unknown;
        }

        public static DenseMatrixComplex cConvertArrayToDenseMatrix(ref double[] x, int m, int n)
        {
            DenseMatrixComplex unknown = new DenseMatrixComplex(m, n);

            int count = 0;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Complex value = new Complex(x[2 * count], x[2 * count + 1]);
                    unknown[i, j] = value;
                    count++;
                }
            }

            return unknown;
        }


    }
}
