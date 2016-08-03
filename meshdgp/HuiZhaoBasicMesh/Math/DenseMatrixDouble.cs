using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class DenseMatrixDouble
    {
        private double[,] datas;

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

        public DenseMatrixDouble()
        {
            rowCount = 0;
            columnCount = 0;
        }

        public DenseMatrixDouble(int rows, int columns)
        {
            datas = new double[rows, columns];

            this.rowCount = rows;
            this.columnCount = columns;
        }

        public DenseMatrixDouble(int rows, int columns, double[] array)
        {
            datas = new double[rows, columns];

            this.rowCount = rows;
            this.columnCount = columns;
        }

        public DenseMatrixDouble(double[,] array)
        {
            datas = array;

            this.rowCount = array.GetLength(0);
            this.columnCount = array.GetLength(1);
        }


        public DenseMatrixDouble(double[] array)
        {
            datas = new double[array.Length, 1];

            this.rowCount = array.Length;
            this.columnCount = 1;

            for (int i = 0; i < array.Length; i++)
            {
                datas[i, 0] = array[i];
            }
        }


        public double F_Norm()
        {
            DenseMatrixDouble tr = this.Transpose() * this;

            double sum = 0;
            for (int i = 0; i < tr.RowCount; i++)
            {
                sum += tr[i, i];
            }

            return Math.Sqrt(sum);
        }


        public double this[int row, int column]
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
                    if (datas[i, j] != datas[j, i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public DenseMatrixDouble Transpose()
        {
            DenseMatrixDouble tr = new DenseMatrixDouble(this.columnCount, this.rowCount);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    tr.datas[j, i] = datas[i, j];
                }
            }

            return tr;
        }

        public static DenseMatrixDouble Identity(int N)
        {
            DenseMatrixDouble identity = new DenseMatrixDouble(N, N);

            for (int i = 0; i < N; i++)
            {
                identity.datas[i, i] = 1;
            }

            return identity;
        }

        public static DenseMatrixDouble operator *(DenseMatrixDouble left, SparseMatrixDouble right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.RowCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }
            DenseMatrixDouble resultMatrix = new DenseMatrixDouble(left.RowCount, right.ColumnCount);
            double[,] result = resultMatrix.datas;

            for (int i = 0; i < left.rowCount; i++)
            {
                foreach (KeyValuePair<Pair, double> item in right.Datas)
                {
                    Pair pair = item.Key;
                    double value = item.Value;

                    int m = pair.Key;
                    int n = pair.Value;

                    //M: mutiply index N: vector store index
                    result[i, n] += left[i, m] * value;
                }
            }

            return resultMatrix;
        }

        public static DenseMatrixDouble operator *(SparseMatrixDouble left, DenseMatrixDouble right)
        {
            //Make sure matrix dimensions are equal
            if (left.ColumnCount != right.RowCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            DenseMatrixDouble resultMatrix = new DenseMatrixDouble(left.RowCount, right.columnCount);

            double[,] result = resultMatrix.datas;

            for (int i = 0; i < right.columnCount; i++)
            {
                foreach (KeyValuePair<Pair, double> item in left.Datas)
                {
                    Pair pair = item.Key;
                    double value = item.Value;

                    int m = pair.Key;
                    int n = pair.Value;

                    //M: mutiply index N: vector store index
                    result[m, i] += right[n, i] * value;
                }
            }


            return resultMatrix;
        }


        public static DenseMatrixDouble operator *(double left, DenseMatrixDouble right)
        {
            DenseMatrixDouble result = new DenseMatrixDouble(right.rowCount, right.columnCount);

            for (int i = 0; i < right.rowCount; i++)
            {
                for (int j = 0; j < right.columnCount; j++)
                {
                    result.datas[i, j] = right.datas[i, j] * left;
                }
            }

            return result;
        }

        public static DenseMatrixDouble operator *(DenseMatrixDouble left, double right)
        {
            return right * left;
        }

        public static DenseMatrixDouble operator *(DenseMatrixDouble left, DenseMatrixDouble right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.rowCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            DenseMatrixDouble result = new DenseMatrixDouble(left.rowCount, right.columnCount);

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

        public static double[] operator *(double[] left, DenseMatrixDouble right)
        {
            //Make sure matrix dimensions are equal
            if (left.Length != right.rowCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            double[] result = new double[right.columnCount];

            for (int j = 0; j < right.columnCount; j++)
            {
                for (int z = 0; z < left.Length; z++)
                {
                    result[j] += left[z] * right.datas[z, j];
                }
            }

            return result;
        }

        public static double[] operator *(DenseMatrixDouble left, double[] right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.Length)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            double[] result = new double[left.columnCount];

            for (int i = 0; i < left.rowCount; i++)
            {
                for (int z = 0; z < left.columnCount; z++)
                {
                    result[i] += left.datas[i, z] * right[z];
                }
            }

            return result;
        }


        public static DenseMatrixDouble operator -(DenseMatrixDouble left, DenseMatrixDouble right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.columnCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            DenseMatrixDouble matrix = new DenseMatrixDouble(left.RowCount, left.ColumnCount);

            for (int i = 0; i < left.RowCount; i++)
            {
                for (int j = 0; j < left.columnCount; j++)
                {
                    matrix.datas[i, j] = left.datas[i, j] - right.datas[i, j];
                }
            }

            return matrix;
        }

        public static DenseMatrixDouble operator +(DenseMatrixDouble left, DenseMatrixDouble right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.columnCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            DenseMatrixDouble matrix = new DenseMatrixDouble(left.RowCount, left.ColumnCount);

            for (int i = 0; i < left.RowCount; i++)
            {
                for (int j = 0; j < left.columnCount; j++)
                {
                    matrix.datas[i, j] = left.datas[i, j] + right.datas[i, j];
                }
            }

            return matrix;
        }

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
            datas = (double[,])ResizeArray(datas, new int[] { this.rowCount, this.columnCount });
        }

        public void AppendRows(int rows)
        {
            rowCount += rows;
            datas = (double[,])ResizeArray(datas, new int[] { this.rowCount, this.columnCount });
        }


        public void AppendColumn()
        {
            columnCount++;
            datas = (double[,])ResizeArray(datas, new int[] { this.rowCount, this.columnCount });
        }

        public void AppendColumns(int columns)
        {
            columnCount += columns;
            datas = (double[,])ResizeArray(datas, new int[] { this.rowCount, this.columnCount });
        }

        public static DenseMatrixDouble Copy(ref DenseMatrixDouble B)
        {
            DenseMatrixDouble newMatrix = new DenseMatrixDouble();
            newMatrix.rowCount = B.rowCount;
            newMatrix.columnCount = B.columnCount;
            Array.Copy(B.datas, newMatrix.datas, B.datas.Length);
            return newMatrix;
        }

        public void ToArray(out int rowCount, out int columnCount, out double[] values)
        {
            rowCount = this.rowCount;
            columnCount = this.columnCount;
            values = new double[this.rowCount * this.columnCount];

            int counts = rowCount * columnCount;

            int iter = 0;
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    values[iter] = datas[i, j];
                    iter++;
                }
            }
        }


    }
}
