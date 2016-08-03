using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class DenseMatrixComplex
    {
        private Complex[,] datas;

        private int rowCount;

        public int RowCount
        {
            get { return rowCount; }
        }

        private int columnCount;

        public int ColumnCount
        {
            get { return columnCount; }
        }

        public DenseMatrixComplex()
        {
            rowCount = 0;
            columnCount = 0;
        }

        public DenseMatrixComplex(int rows, int columns)
        {
            datas = new Complex[rows, columns];

            this.rowCount = rows;
            this.columnCount = columns;
        }

        public Complex this[int row, int column]
        {
            set
            {
                if (row > rowCount - 1 || column > columnCount - 1)
                {
                    throw new IndexOutOfRangeException();
                }

                datas[row, column] = value;

            }
            get
            {
                if (row > rowCount - 1 || column > columnCount - 1)
                {
                    throw new IndexOutOfRangeException();
                }

                return datas[row, column];
            }

        }

        public DenseMatrixComplex Transpose()
        {
            DenseMatrixComplex tr = new DenseMatrixComplex(this.columnCount, this.rowCount);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    tr.datas[j, i] = datas[i, j];
                }
            }

            return tr;
        }

        public static DenseMatrixComplex Identity(int N)
        {
            DenseMatrixComplex identity = new DenseMatrixComplex(N, N);

            for (int i = 0; i < N; i++)
            {
                identity.datas[i, i] = Complex.Identity;
            }

            return identity;
        }

        #region Operators
        public static DenseMatrixComplex operator *(Complex left, DenseMatrixComplex right)
        {
            DenseMatrixComplex result = new DenseMatrixComplex(right.rowCount, right.columnCount);

            for (int i = 0; i < right.rowCount; i++)
            {
                for (int j = 0; j < right.columnCount; j++)
                {
                    result.datas[i, j] = right.datas[i, j] * left;
                }
            }

            return result;
        }

        public static DenseMatrixComplex operator *(DenseMatrixComplex left, Complex right)
        {
            return right * left;
        }

        public static DenseMatrixComplex operator *(DenseMatrixComplex left, DenseMatrixComplex right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.rowCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            DenseMatrixComplex result = new DenseMatrixComplex(left.rowCount, right.columnCount);

            for (int i = 0; i < left.rowCount; i++)
            {
                for (int j = 0; j < right.columnCount; j++)
                {
                    for (int z = 0; z < left.columnCount; z++)
                    {
                        result.datas[i, j] += left.datas[i, z] * right.datas[z, j];
                    }
                }
            }

            return result;
        }

        public static Complex[] operator *(Complex[] left, DenseMatrixComplex right)
        {
            //Make sure matrix dimensions are equal
            if (left.Length != right.rowCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            Complex[] result = new Complex[right.columnCount];

            for (int j = 0; j < right.columnCount; j++)
            {
                for (int z = 0; z < left.Length; z++)
                {
                    result[j] += left[z] * right.datas[z, j];
                }
            }

            return result;
        }

        public static DenseMatrixComplex operator *(SparseMatrixComplex left, DenseMatrixComplex right)
        {
            //Make sure matrix dimensions are equal
            if (left.ColumnCount != right.RowCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            DenseMatrixComplex resultMatrix = new DenseMatrixComplex(left.RowCount, right.columnCount);

            Complex[,] result = resultMatrix.datas;

            for (int i = 0; i < right.columnCount; i++)
            {
                foreach (KeyValuePair<Pair, Complex> item in left.Datas)
                {
                    Pair pair = item.Key;
                    Complex value = item.Value;

                    int m = pair.Key;
                    int n = pair.Value;

                    //M: mutiply index N: vector store index
                    result[m, i] += right[n, i] * value;
                }
            }


            return resultMatrix;
        }

        public static Complex[] operator *(DenseMatrixComplex left, Complex[] right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.Length)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            Complex[] result = new Complex[left.columnCount];

            for (int i = 0; i < left.rowCount; i++)
            {
                for (int z = 0; z < left.columnCount; z++)
                {
                    result[i] += left.datas[i, z] * right[z];
                }
            }

            return result;
        }

        public static DenseMatrixComplex operator *(DenseMatrixComplex left, SparseMatrixComplex right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.RowCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }
            DenseMatrixComplex resultMatrix = new DenseMatrixComplex(left.RowCount, right.ColumnCount);
            Complex[,] result = resultMatrix.datas;

            for (int i = 0; i < left.rowCount; i++)
            {
                foreach (KeyValuePair<Pair, Complex> item in right.Datas)
                {
                    Pair pair = item.Key;
                    Complex value = item.Value;

                    int m = pair.Key;
                    int n = pair.Value;

                    //M: mutiply index N: vector store index
                    result[i, n] += left[i, m] * value;
                }
            }

            return resultMatrix;
        }

        public static DenseMatrixComplex operator -(DenseMatrixComplex left, DenseMatrixComplex right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.columnCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            DenseMatrixComplex matrix = new DenseMatrixComplex(left.RowCount, left.ColumnCount);

            for (int i = 0; i < left.RowCount; i++)
            {
                for (int j = 0; j < left.columnCount; j++)
                {
                    matrix.datas[i, j] = left.datas[i, j] - right.datas[i, j];
                }
            }

            return matrix;
        }

        public static DenseMatrixComplex operator /(DenseMatrixComplex left, Complex right)
        {
            //Make sure matrix dimensions are equal
            DenseMatrixComplex matrix = new DenseMatrixComplex(left.RowCount, left.ColumnCount);

            Complex rightInv = right.Inv();

            for (int i = 0; i < left.RowCount; i++)
            {
                for (int j = 0; j < left.columnCount; j++)
                {
                    matrix.datas[i, j] = left.datas[i, j] * rightInv;
                }
            }

            return matrix;
        }

        public static DenseMatrixComplex operator +(DenseMatrixComplex left, DenseMatrixComplex right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.columnCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            DenseMatrixComplex matrix = new DenseMatrixComplex(left.RowCount, left.ColumnCount);

            for (int i = 0; i < left.RowCount; i++)
            {
                for (int j = 0; j < left.columnCount; j++)
                {
                    matrix.datas[i, j] = left.datas[i, j] + right.datas[i, j];
                }
            }

            return matrix;
        }

        #endregion

        #region Dimesion Resize
        private static Array ResizeArray(Array arr, int[] newSizes)
        {
            if (newSizes.Length != arr.Rank)
                throw new ArgumentException("arr must have the same number of dimensions " +
                                            "as there are elements in newSizes", "newSizes");

            var temp = Array.CreateInstance(arr.GetType().GetElementType(), newSizes);
            int length = arr.Length <= temp.Length ? arr.Length : temp.Length;
            Array.ConstrainedCopy(arr, 0, temp, 0, length);
            return temp;
        }

        public void AppendRow()
        {
            rowCount++;
            datas = (Complex[,])ResizeArray(datas, new int[] { this.rowCount, this.columnCount });
        }

        public void AppendRows(int rows)
        {
            rowCount += rows;
            datas = (Complex[,])ResizeArray(datas, new int[] { this.rowCount, this.columnCount });
        }


        public void AppendColumn()
        {
            columnCount++;
            datas = (Complex[,])ResizeArray(datas, new int[] { this.rowCount, this.columnCount });
        }

        public void AppendColumns(int columns)
        {
            columnCount += columns;
            datas = (Complex[,])ResizeArray(datas, new int[] { this.rowCount, this.columnCount });
        }

        #endregion

        #region Util
        public void Normalize()
        {
            Complex norm = new Complex(F_Norm(), 0);
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    this[i, j] /= norm;
                }
            }
        }

        public static DenseMatrixComplex Copy(ref DenseMatrixComplex B)
        {
            DenseMatrixComplex newMatrix = new DenseMatrixComplex();
            newMatrix.rowCount = B.rowCount;
            newMatrix.columnCount = B.columnCount;
            Array.Copy(B.datas, newMatrix.datas, B.datas.Length);
            return newMatrix;
        }

        public void Fill(Complex value)
        {
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    this[i, j] = value;
                }
            }
        }

        public Complex Dot(DenseMatrixComplex right)
        {
            DenseMatrixComplex x = this.Transpose() * right;
            Complex result = x[0, 0];
            return result;
        }

        //Randomlize values from -1 to 1 to each item of the matrix
        public void Randomize()
        {
            Random random = new Random();

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    datas[i, j] = new Complex(2 * (random.NextDouble() - 0.5), 0);
                }
            }

        }
        #endregion

        #region Self Attributes

        public bool IsSymmetric()
        {
            if (this.rowCount != this.columnCount)
            {
                return false;
            }

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = i; j < columnCount; j++)
                {
                    if (datas[i, j].Equals(datas[j, i]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public double F_Norm()
        {
            DenseMatrixComplex tr = this.Transpose() * this;

            double sum = 0;
            for (int i = 0; i < tr.RowCount; i++)
            {
                sum += tr[i, i].Norm();
            }

            return Math.Sqrt(sum);
        }

        public double L2Norm()
        {
            return 0;
        }

        public double InifnityNorm()
        {
            double max = double.MinValue;
            for (int i = 0; i < this.RowCount; i++)
            {
                for (int j = 0; j < this.ColumnCount; j++)
                {
                    double normValue = datas[i, j].Norm();
                    if (normValue > max)
                    {
                        max = normValue;
                    }
                }
            }
            return max;
        }

        #endregion

        #region Output


        public void ToArray(out int rowCount, out int columnCount, out double[] values, out double[] z)
        {
            rowCount = this.rowCount;
            columnCount = this.columnCount;
            values = new double[this.rowCount * this.columnCount];
            z = new double[this.rowCount * this.columnCount];

            int counts = rowCount * columnCount;

            int iter = 0;
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    values[iter] = datas[i, j].RealPart;
                    z[iter] = datas[i, j].ImagePart;
                    iter++;
                }
            }
        }

        public void ToArray(out int rowCount, out int columnCount, out double[] values) //Coherence
        {
            rowCount = this.rowCount;
            columnCount = this.columnCount;
            values = new double[2 * this.rowCount * this.columnCount];

            int counts = rowCount * columnCount;

            int iter = 0;
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    values[2 * iter] = datas[i, j].RealPart;
                    values[2 * iter + 1] = datas[i, j].ImagePart;
                    iter++;
                }
            }
        }
        #endregion



    }
}
