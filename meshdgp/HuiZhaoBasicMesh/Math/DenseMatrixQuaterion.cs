using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class DenseMatrixQuaternion
    {
        private Quaternion[,] datas;

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

        public DenseMatrixQuaternion()
        {
            rowCount = 0;
            columnCount = 0;
        }

        public DenseMatrixQuaternion(int rows, int columns)
        {
            datas = new Quaternion[rows, columns];

            this.rowCount = rows;
            this.columnCount = columns;
        }

        public Quaternion this[int row, int column]
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

        public DenseMatrixQuaternion Transpose()
        {
            DenseMatrixQuaternion tr = new DenseMatrixQuaternion(this.columnCount, this.rowCount);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    tr.datas[j, i] = datas[i, j];
                }
            }

            return tr;
        }

        public static DenseMatrixQuaternion Identity(int N)
        {
            DenseMatrixQuaternion identity = new DenseMatrixQuaternion(N, N);

            for (int i = 0; i < N; i++)
            {
                identity.datas[i, i] = Quaternion.Identity;
            }

            return identity;
        }

        #region Operators
        public static DenseMatrixQuaternion operator *(Quaternion left, DenseMatrixQuaternion right)
        {
            DenseMatrixQuaternion result = new DenseMatrixQuaternion(right.rowCount, right.columnCount);

            for (int i = 0; i < right.rowCount; i++)
            {
                for (int j = 0; j < right.columnCount; j++)
                {
                    result.datas[i, j] = right.datas[i, j] * left;
                }
            }

            return result;
        }

        public static DenseMatrixQuaternion operator *(DenseMatrixQuaternion left, Quaternion right)
        {
            return right * left;
        }

        public static DenseMatrixQuaternion operator *(DenseMatrixQuaternion left, DenseMatrixQuaternion right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.rowCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            DenseMatrixQuaternion result = new DenseMatrixQuaternion(left.rowCount, right.columnCount);

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

        public static Quaternion[] operator *(Quaternion[] left, DenseMatrixQuaternion right)
        {
            //Make sure matrix dimensions are equal
            if (left.Length != right.rowCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            Quaternion[] result = new Quaternion[right.columnCount];

            for (int j = 0; j < right.columnCount; j++)
            {
                for (int z = 0; z < left.Length; z++)
                {
                    result[j] += left[z] * right.datas[z, j];
                }
            }

            return result;
        }

        public static DenseMatrixQuaternion operator *(SparseMatrixQuaternion left, DenseMatrixQuaternion right)
        {
            //Make sure matrix dimensions are equal
            if (left.ColumnCount != right.RowCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            DenseMatrixQuaternion resultMatrix = new DenseMatrixQuaternion(left.RowCount, right.columnCount);

            Quaternion[,] result = resultMatrix.datas;

            for (int i = 0; i < right.columnCount; i++)
            {
                foreach (KeyValuePair<Pair, Quaternion> item in left.Datas)
                {
                    Pair pair = item.Key;
                    Quaternion value = item.Value;

                    int m = pair.Key;
                    int n = pair.Value;

                    //M: mutiply index N: vector store index
                    result[m, i] += right[n, i] * value;
                }
            }


            return resultMatrix;
        }

        public static Quaternion[] operator *(DenseMatrixQuaternion left, Quaternion[] right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.Length)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            Quaternion[] result = new Quaternion[left.columnCount];

            for (int i = 0; i < left.rowCount; i++)
            {
                for (int z = 0; z < left.columnCount; z++)
                {
                    result[i] += left.datas[i, z] * right[z];
                }
            }

            return result;
        }

        public static DenseMatrixQuaternion operator *(DenseMatrixQuaternion left, SparseMatrixQuaternion right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.RowCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }
            DenseMatrixQuaternion resultMatrix = new DenseMatrixQuaternion(left.RowCount, right.ColumnCount);
            Quaternion[,] result = resultMatrix.datas;

            for (int i = 0; i < left.rowCount; i++)
            {
                foreach (KeyValuePair<Pair, Quaternion> item in right.Datas)
                {
                    Pair pair = item.Key;
                    Quaternion value = item.Value;

                    int m = pair.Key;
                    int n = pair.Value;

                    //M: mutiply index N: vector store index
                    result[i, n] += left[i, m] * value;
                }
            }

            return resultMatrix;
        }

        public static DenseMatrixQuaternion operator -(DenseMatrixQuaternion left, DenseMatrixQuaternion right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.columnCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            DenseMatrixQuaternion matrix = new DenseMatrixQuaternion(left.RowCount, left.ColumnCount);

            for (int i = 0; i < left.RowCount; i++)
            {
                for (int j = 0; j < left.columnCount; j++)
                {
                    matrix.datas[i, j] = left.datas[i, j] - right.datas[i, j];
                }
            }

            return matrix;
        }

        public static DenseMatrixQuaternion operator /(DenseMatrixQuaternion left, Quaternion right)
        {
            //Make sure matrix dimensions are equal
            DenseMatrixQuaternion matrix = new DenseMatrixQuaternion(left.RowCount, left.ColumnCount);

            // Quaternion rightInv = right.Invert();

            for (int i = 0; i < left.RowCount; i++)
            {
                for (int j = 0; j < left.columnCount; j++)
                {
                    //  matrix.datas[i, j] = left.datas[i, j] * rightInv;
                }
            }

            return matrix;
        }

        public static DenseMatrixQuaternion operator +(DenseMatrixQuaternion left, DenseMatrixQuaternion right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.columnCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            DenseMatrixQuaternion matrix = new DenseMatrixQuaternion(left.RowCount, left.ColumnCount);

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
            datas = (Quaternion[,])ResizeArray(datas, new int[] { this.rowCount, this.columnCount });
        }

        public void AppendRows(int rows)
        {
            rowCount += rows;
            datas = (Quaternion[,])ResizeArray(datas, new int[] { this.rowCount, this.columnCount });
        }


        public void AppendColumn()
        {
            columnCount++;
            datas = (Quaternion[,])ResizeArray(datas, new int[] { this.rowCount, this.columnCount });
        }

        public void AppendColumns(int columns)
        {
            columnCount += columns;
            datas = (Quaternion[,])ResizeArray(datas, new int[] { this.rowCount, this.columnCount });
        }

        #endregion

        #region Util
        public void Normalize()
        {
            double norm = 0;
            for (int i = 0; i < this.rowCount; i++)
            {
                for (int j = 0; j < this.columnCount; j++)
                {
                    norm += datas[i, j].LengthSquared;
                }
            }
            norm = Math.Sqrt(norm);

            for (int i = 0; i < this.rowCount; i++)
            {
                for (int j = 0; j < this.columnCount; j++)
                {
                    datas[i, j] /= norm;
                }
            }

            //Quaternion norm = new Quaternion(F_Norm(), 0);
            //for (int i = 0; i < RowCount; i++)
            //{
            //    for (int j = 0; j < ColumnCount; j++)
            //    {
            //        this[i, j] /= norm;
            //    }
            //}
        }

        public static DenseMatrixQuaternion Copy(ref DenseMatrixQuaternion B)
        {
            DenseMatrixQuaternion newMatrix = new DenseMatrixQuaternion();
            newMatrix.rowCount = B.rowCount;
            newMatrix.columnCount = B.columnCount;
            Array.Copy(B.datas, newMatrix.datas, B.datas.Length);
            return newMatrix;
        }

        public void Fill(Quaternion value)
        {
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    this[i, j] = value;
                }
            }
        }

        public Quaternion Dot(DenseMatrixQuaternion right)
        {
            DenseMatrixQuaternion x = this.Transpose() * right;
            Quaternion result = x[0, 0];
            return result;
        }

        //Randomlize values from -1 to 1 to each item of the matrix
        public void Randomize()
        {
            //Random random = new Random();

            //for (int i = 0; i < rowCount; i++)
            //{
            //    for (int j = 0; j < columnCount; j++)
            //    {
            //        datas[i, j] = new Quaternion(2 * (random.NextDouble() - 0.5), 0);
            //    }
            //}

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
            DenseMatrixQuaternion tr = this.Transpose() * this;

            double sum = 0;
            for (int i = 0; i < tr.RowCount; i++)
            {
                sum += tr[i, i].Length;
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
                    double normValue = datas[i, j].Length;
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

        public void ToArray(out int rowCount, out int columnCount, out double[] values)
        {
            rowCount = 4 * this.rowCount;
            columnCount = 4 * this.columnCount;
            values = new double[rowCount * columnCount];

            int counts = rowCount * columnCount;

            int iter = 0;
            int columnItems = 4 * columnCount;
            for (int i = 0; i < this.rowCount; i++)
            {
                for (int j = 0; j < this.columnCount; j++)
                {
                    Quaternion q = this.datas[i, j];

                    Matrix4D qMatrix = q.ToMatrix();

                    int i4 = 4 * i;
                    int j4 = 4 * j;

                    int subIndex = 4 * i * columnItems + 4 * j;

                    for (int m = 0; m < 4; m++)
                    {
                        for (int n = 0; n < 4; n++)
                        {
                            values[subIndex + 4 * m + n] = qMatrix[m, n];
                        }
                    }


                    iter++;
                }
            }
        }

        #endregion



    }
}
