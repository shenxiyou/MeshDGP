using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace GraphicResearchHuiZhao
{
    public class SparseMatrixQuaternion
    {
        private Dictionary<Pair, Quaternion> mapData;

        public Dictionary<Pair, Quaternion> Datas
        {
            get { return mapData; }
        }

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

        public int NNZ
        {
            get { return mapData.Count; }
        }

        #region Constructors
        public SparseMatrixQuaternion(int rows, int columns)
        {
            rowCount = rows;
            columnCount = columns;
            mapData = new Dictionary<Pair, Quaternion>();
        }

        public SparseMatrixQuaternion()
        {
            rowCount = 0;
            columnCount = 0;
            mapData = new Dictionary<Pair, Quaternion>();
        }

        public SparseMatrixQuaternion(SparseMatrixQuaternion matrix)
        {
            mapData = new Dictionary<Pair, Quaternion>(matrix.mapData);
            this.rowCount = matrix.rowCount;
            this.columnCount = matrix.columnCount;
        }

        #endregion

        #region Operators

        public static SparseMatrixQuaternion operator *(double left, SparseMatrixQuaternion right)
        {
            SparseMatrixQuaternion result = new SparseMatrixQuaternion(right.rowCount, right.columnCount);

            foreach (KeyValuePair<Pair, Quaternion> item in right.mapData)
            {
                Pair pair = item.Key;
                Quaternion value = left * item.Value;

                result.mapData.Add(pair, value);
            }

            return result;
        }

        public static SparseMatrixQuaternion operator *(SparseMatrixQuaternion left, double right)
        {
            return right * left;
        }

        public static SparseMatrixQuaternion operator *(SparseMatrixQuaternion left, SparseMatrixQuaternion right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.rowCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            SparseMatrixQuaternion result = new SparseMatrixQuaternion(left.rowCount, right.columnCount);

            int leftNNZ = left.mapData.Count;
            int rightNNZ = right.mapData.Count;

            #region Left < Right
            //We use right as stardand sight
            //if (leftNNZ < rightNNZ)
            //{
            //Connection nonezero for each row of matrix a 
            List<KeyValuePair<int, Quaternion>>[] bRows = new List<KeyValuePair<int, Quaternion>>[right.rowCount];
            for (int i = 0; i < bRows.Length; i++)
            {
                bRows[i] = new List<KeyValuePair<int, Quaternion>>();
            }


            foreach (KeyValuePair<Pair, Quaternion> item in right.mapData)
            {
                Pair pair = item.Key;
                Quaternion value = item.Value;

                bRows[pair.Key].Add(new KeyValuePair<int, Quaternion>(pair.Value, value));
            }

            //Compute C = A*B
            foreach (KeyValuePair<Pair, Quaternion> item in left.mapData)
            {
                Pair pair = item.Key;
                int mA = pair.Key;
                int nA = pair.Value;
                Quaternion value = item.Value;

                List<KeyValuePair<int, Quaternion>> bRow = bRows[nA];

                for (int i = 0; i < bRow.Count; i++)
                {
                    int k = bRow[i].Key;

                    Pair pair2 = new Pair(mA, k);

                    if (result.mapData.ContainsKey(pair2))
                    {
                        result.mapData[pair2] += value * bRow[i].Value;
                    }
                    else
                    {
                        result.mapData.Add(pair2, value * bRow[i].Value);
                    }


                }
            }

            //}
            #endregion
            #region Right < Left

            //else if (leftNNZ > rightNNZ)
            //{
            //    //Connection nonezero for each row of matrix a 
            //    List<KeyValuePair<int, double>>[] aCols = new List<KeyValuePair<int, double>>[left.columnCount];
            //    for (int i = 0; i < aCols.Length; i++)
            //    {
            //        aCols[i] = new List<KeyValuePair<int, double>>();
            //    }

            //    foreach (KeyValuePair<Pair, double> item in left.mapData)
            //    {
            //        Pair pair = item.Key;
            //        double value = item.Value;

            //        aCols[pair.Value].Add(new KeyValuePair<int, double>(pair.Key, value));
            //    }

            //    //Compute C = A*B
            //    foreach (KeyValuePair<Pair, double> item in right.mapData)
            //    {
            //        Pair pair = item.Key;
            //        int mA = pair.Key;
            //        int nA = pair.Value;
            //        double value = item.Value;

            //        List<KeyValuePair<int, double>> aCol = aCols[mA];

            //        for (int i = 0; i < aCol.Count; i++)
            //        {
            //            int k = aCol[i].Key;

            //            Pair pair2 = new Pair(k, nA);

            //            if (result.mapData.ContainsKey(pair2))
            //            {
            //                result.mapData[pair2] += value * aCol[i].Value;
            //            }
            //            else
            //            {
            //                result.mapData.Add(pair2, value * aCol[i].Value);
            //            }

            //        }
            //    }

            //}


            #endregion

            return result;
        }

        public static Quaternion[] operator *(double[] left, SparseMatrixQuaternion right)
        {
            //Make sure matrix dimensions are equal
            if (left.Length != right.rowCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            Quaternion[] result = new Quaternion[right.columnCount];

            foreach (KeyValuePair<Pair, Quaternion> item in right.mapData)
            {
                Pair pair = item.Key;
                Quaternion value = item.Value;

                int m = pair.Key;
                int n = pair.Value;

                //M: mutiply index N: vector store index
                result[n] += left[m] * value;
            }

            return result;
        }

        public static Quaternion[] operator *(SparseMatrixQuaternion left, Quaternion[] right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.Length)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            Quaternion[] result = new Quaternion[left.columnCount];

            foreach (KeyValuePair<Pair, Quaternion> item in left.mapData)
            {
                Pair pair = item.Key;
                Quaternion value = item.Value;

                int m = pair.Key;
                int n = pair.Value;

                //M: mutiply index N: vector store index
                result[m] += right[n] * value;
            }

            return result;
        }

        public static SparseMatrixQuaternion operator +(SparseMatrixQuaternion left, SparseMatrixQuaternion right)
        {
            SparseMatrixQuaternion result = new SparseMatrixQuaternion(right.rowCount, right.columnCount);

            foreach (KeyValuePair<Pair, Quaternion> item in left.mapData)
            {
                Pair pair = item.Key;
                Quaternion value = item.Value;

                result.mapData.Add(pair, value);
            }

            foreach (KeyValuePair<Pair, Quaternion> item in right.mapData)
            {
                Pair pair = item.Key;
                Quaternion value = item.Value;

                if (result.mapData.ContainsKey(pair))
                {
                    Quaternion temp = result.mapData[pair] += item.Value;
                    if (temp.Equals(Quaternion.Zero))
                    {
                        result.mapData.Remove(pair);
                    }
                }
                else
                {
                    result.mapData.Add(pair, value);
                }

            }

            return result;
        }

        public static SparseMatrixQuaternion operator -(SparseMatrixQuaternion left, SparseMatrixQuaternion right)
        {
            SparseMatrixQuaternion result = new SparseMatrixQuaternion(right.rowCount, right.columnCount);

            foreach (KeyValuePair<Pair, Quaternion> item in left.mapData)
            {
                Pair pair = item.Key;
                Quaternion value = item.Value;

                result.mapData.Add(pair, value);
            }

            foreach (KeyValuePair<Pair, Quaternion> item in right.mapData)
            {
                Pair pair = item.Key;
                Quaternion value = item.Value;

                if (result.mapData.ContainsKey(pair))
                {
                    Quaternion temp = result.mapData[pair] -= item.Value;
                    if (temp.Equals(Quaternion.Zero))
                    {
                        result.mapData.Remove(pair);
                    }
                }
                else
                {
                    result.mapData.Add(pair, value);
                }
            }

            return result;
        }
        #endregion

        #region Attributes
        public Quaternion this[int row, int column]
        {
            set
            {
                if (row > rowCount - 1 || column > columnCount - 1)
                {
                    throw new IndexOutOfRangeException();
                }

                Pair pair = new Pair(row, column);

                if (value.Equals(Quaternion.Zero))
                {
                    if (mapData.ContainsKey(pair))
                    {
                        mapData.Remove(pair);
                    }
                    else
                    {
                        return;
                    }
                }

                mapData[pair] = value;
            }
            get
            {
                if (row > rowCount - 1 || column > columnCount - 1)
                {
                    throw new IndexOutOfRangeException();
                }

                Pair pair = new Pair(row, column);

                if (mapData.ContainsKey(pair))
                {
                    return mapData[pair];
                }
                else
                {
                    return Quaternion.Zero;
                }
            }
        }

        /// <summary>
        /// Return the lagest dimension of the matrix
        /// </summary>
        public int Length()
        {
            return rowCount >= columnCount ? rowCount : columnCount;
        }

        public bool IsSymmetric()
        {
            if (this.rowCount != this.columnCount)
            {
                return false;
            }

            foreach (KeyValuePair<Pair, Quaternion> item in mapData)
            {
                Pair pair = item.Key;
                if (pair.Key >= pair.Value)
                {
                    continue;
                }

                Quaternion value = item.Value;

                int m = pair.Key;
                int n = pair.Value;

                if (!mapData[new Pair(n, m)].Equals(value))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Elemental Transf.

        public SparseMatrixQuaternion Transpose()
        {

            SparseMatrixQuaternion trMatrix = new SparseMatrixQuaternion(this.columnCount, this.rowCount);

            foreach (KeyValuePair<Pair, Quaternion> item in mapData)
            {
                Pair pair = item.Key;
                Quaternion value = item.Value;

                trMatrix.mapData[new Pair(pair.Value, pair.Key)] = value;
            }

            return trMatrix;
        }

        public void AppendRow()
        {
            rowCount++;
        }

        public void AppendRows(int rows)
        {
            rowCount += rows;
        }

        public void AppendColumn()
        {
            columnCount++;
        }

        public void AppendColumns(int columns)
        {
            columnCount += columns;
        }

        public SparseMatrixQuaternion SubMatrix(int rowStart, int rowEnd, int columnStart, int columnEnd)
        {
            if (rowEnd < rowStart || columnEnd < columnStart)
            {
                throw new ArgumentOutOfRangeException();
            }

            int rows = rowEnd - rowStart + 1;
            int columns = columnEnd - columnStart + 1;

            SparseMatrixQuaternion subMatrix = new SparseMatrixQuaternion(rows, columns);

            int subMatrixEntiries = rows * columns;

            if (subMatrixEntiries < mapData.Count)
            {
                for (int i = 0; i < rows; i++)
                {
                    int rowInx = i + rowStart;
                    for (int j = 0; j < columns; j++)
                    {
                        int columnInx = j + columnStart;
                        Pair pair = new Pair(rowInx, columnInx);
                        if (!mapData.ContainsKey(pair))
                        {
                            continue;
                        }

                        Pair subPair = new Pair(i, j);
                        subMatrix.mapData[subPair] = mapData[pair];

                    }
                }
            }
            else if (subMatrixEntiries >= mapData.Count)
            {
                foreach (KeyValuePair<Pair, Quaternion> item in mapData)
                {
                    Pair pair = item.Key;
                    Quaternion value = item.Value;

                    int m = pair.Key;
                    int n = pair.Value;

                    if ((m >= rowStart && m <= rowEnd) && (n >= columnStart && m <= columnEnd))
                    {
                        int i = m - rowStart;
                        int j = n - columnStart;

                        subMatrix.mapData[new Pair(i, j)] = value;
                    }

                }
            }

            return subMatrix;
        }

        public void ExchangeRow(int row1, int row2)
        {

        }


        #endregion

        #region Calculation

        public SparseMatrixQuaternion Inverse()
        {
            return null;
        }

        #endregion

        #region Utils

        public static SparseMatrixQuaternion Identity(int N)
        {
            SparseMatrixQuaternion identity = new SparseMatrixQuaternion(N, N);

            for (int i = 0; i < N; i++)
            {
                identity.mapData.Add(new Pair(i, i), Quaternion.Identity);
            }

            return identity;
        }

        public static SparseMatrixQuaternion Copy(ref SparseMatrixQuaternion B)
        {
            Dictionary<Pair, Quaternion> newData = new Dictionary<Pair, Quaternion>(B.mapData);

            SparseMatrixQuaternion newMatrix = new SparseMatrixQuaternion(B.rowCount, B.columnCount);
            newMatrix.mapData = newData;

            return newMatrix;
        }

        public void PrintMatrix()
        {
            for (int i = 0; i < rowCount; i++)
            {
                Console.Write("|");
                for (int j = 0; j < columnCount; j++)
                {
                    Console.Write(" " + this[i, j] + " ");
                }
                Console.WriteLine("|");
            }
        }

        public static int CompareKey(KeyValuePair<int, Quaternion> a, KeyValuePair<int, Quaternion> b)
        {
            return a.Key.CompareTo(b.Key);
        }

        public void WriteToFile(String path)
        {
            StreamWriter sw = sw = File.CreateText(path);

            //Output matrix attributes
            sw.WriteLine("#Row: " + 4 * this.rowCount + " Columns: " + 4 * this.columnCount + " nnz: " + 16 * this.NNZ);

            sw.WriteLine("#Symmetric: false N");

            foreach (KeyValuePair<Pair, Quaternion> e in mapData)
            {
                Pair pair = e.Key;

                double q0 = e.Value.x;
                double q1 = e.Value.y;
                double q2 = e.Value.z;
                double q3 = e.Value.w;

                //Q0
                sw.WriteLine(4 * pair.Key + " " + 4 * pair.Value + " " + q0);
                sw.WriteLine((4 * pair.Key + 1) + " " + (4 * pair.Value + 1) + " " + q0);
                sw.WriteLine((4 * pair.Key + 2) + " " + (4 * pair.Value + 2) + " " + q0);
                sw.WriteLine((4 * pair.Key + 3) + " " + (4 * pair.Value + 3) + " " + q0);

                //Q1
                sw.WriteLine((4 * pair.Key + 1) + " " + (4 * pair.Value) + " " + q1);
                sw.WriteLine((4 * pair.Key) + " " + (4 * pair.Value + 1) + " " + q1);
                sw.WriteLine((4 * pair.Key + 3) + " " + (4 * pair.Value + 2) + " " + q1);
                sw.WriteLine((4 * pair.Key + 2) + " " + (4 * pair.Value + 3) + " " + q1);

                //Q2
                sw.WriteLine((4 * pair.Key + 2) + " " + (4 * pair.Value) + " " + q2);
                sw.WriteLine((4 * pair.Key) + " " + (4 * pair.Value + 2) + " " + q2);
                sw.WriteLine((4 * pair.Key + 3) + " " + (4 * pair.Value + 1) + " " + q2);
                sw.WriteLine((4 * pair.Key + 1) + " " + (4 * pair.Value + 3) + " " + q2);

                //Q3
                sw.WriteLine((4 * pair.Key + 3) + " " + (4 * pair.Value) + " " + q3);
                sw.WriteLine((4 * pair.Key) + " " + (4 * pair.Value + 3) + " " + q3);
                sw.WriteLine((4 * pair.Key + 2) + " " + (4 * pair.Value + 1) + " " + q3);
                sw.WriteLine((4 * pair.Key + 1) + " " + (4 * pair.Value + 2) + " " + q3);
            }

            sw.Close();
        }


        public void ToCRS(out int[] rowPtr, out int[] colIndices, out double[] values, out int nnz)
        {
            SparseMatrixDouble sparse = new SparseMatrixDouble(4 * this.rowCount, 4 * this.columnCount);


            foreach (KeyValuePair<Pair, Quaternion> e in mapData)
            {
                Pair pair = e.Key;

                double q0 = e.Value.w;
                double q1 = e.Value.x;
                double q2 = e.Value.y;
                double q3 = e.Value.z;

                if (q0 != 0)
                {
                    sparse.Datas.Add(new Pair(4 * pair.Key, 4 * pair.Value), q0);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 1, 4 * pair.Value + 1), q0);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 2, 4 * pair.Value + 2), q0);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 3, 4 * pair.Value + 3), q0);
                }

                if (q1 != 0)
                {
                    sparse.Datas.Add(new Pair(4 * pair.Key + 1, 4 * pair.Value), q1);
                    sparse.Datas.Add(new Pair(4 * pair.Key, 4 * pair.Value + 1), -q1);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 3, 4 * pair.Value + 2), q1);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 2, 4 * pair.Value + 3), -q1);
                }

                if (q2 != 0)
                {
                    sparse.Datas.Add(new Pair(4 * pair.Key + 2, 4 * pair.Value), q2);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 3, 4 * pair.Value + 1), -q2);
                    sparse.Datas.Add(new Pair(4 * pair.Key, 4 * pair.Value + 2), -q2);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 1, 4 * pair.Value + 3), q2);
                }

                if (q3 != 0)
                {
                    sparse.Datas.Add(new Pair(4 * pair.Key + 3, 4 * pair.Value), q3);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 2, 4 * pair.Value + 1), q3);
                    sparse.Datas.Add(new Pair(4 * pair.Key, 4 * pair.Value + 3), -q3);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 1, 4 * pair.Value + 2), -q3);
                }


            }

            sparse.ToCRS(out rowPtr, out colIndices, out values, out nnz);

        }

        public void ToCCS(out int[] colPtr, out int[] rowIndices, out double[] values, out int nnz)
        {
            SparseMatrixDouble sparse = new SparseMatrixDouble(4 * this.rowCount, 4 * this.columnCount);

            foreach (KeyValuePair<Pair, Quaternion> e in mapData)
            {
                Pair pair = e.Key;

                double q0 = e.Value.w;
                double q1 = e.Value.x;
                double q2 = e.Value.y;
                double q3 = e.Value.z;

                if (q0 != 0)
                {
                    sparse.Datas.Add(new Pair(4 * pair.Key, 4 * pair.Value), q0);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 1, 4 * pair.Value + 1), q0);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 2, 4 * pair.Value + 2), q0);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 3, 4 * pair.Value + 3), q0);
                }

                if (q1 != 0)
                {
                    sparse.Datas.Add(new Pair(4 * pair.Key + 1, 4 * pair.Value), q1);
                    sparse.Datas.Add(new Pair(4 * pair.Key, 4 * pair.Value + 1), -q1);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 3, 4 * pair.Value + 2), q1);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 2, 4 * pair.Value + 3), -q1);
                }

                if (q2 != 0)
                {
                    sparse.Datas.Add(new Pair(4 * pair.Key + 2, 4 * pair.Value), q2);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 3, 4 * pair.Value + 1), -q2);
                    sparse.Datas.Add(new Pair(4 * pair.Key, 4 * pair.Value + 2), -q2);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 1, 4 * pair.Value + 3), q2);
                }

                if (q3 != 0)
                {
                    sparse.Datas.Add(new Pair(4 * pair.Key + 3, 4 * pair.Value), q3);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 2, 4 * pair.Value + 1), q3);
                    sparse.Datas.Add(new Pair(4 * pair.Key, 4 * pair.Value + 3), -q3);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 1, 4 * pair.Value + 2), -q3);
                }

            }

            sparse.ToCCS(out colPtr, out rowIndices, out values, out nnz);

        }

        public void ToTriplet(out int[] colIndices, out int[] rowIndices, out double[] values, out int nnz)
        {
            SparseMatrixDouble sparse = new SparseMatrixDouble(4 * this.rowCount, 4 * this.columnCount);

            foreach (KeyValuePair<Pair, Quaternion> e in mapData)
            {
                Pair pair = e.Key;

                double q0 = e.Value.w;
                double q1 = e.Value.x;
                double q2 = e.Value.y;
                double q3 = e.Value.z;

                if (q0 != 0)
                {
                    sparse.Datas.Add(new Pair(4 * pair.Key, 4 * pair.Value), q0);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 1, 4 * pair.Value + 1), q0);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 2, 4 * pair.Value + 2), q0);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 3, 4 * pair.Value + 3), q0);
                }

                if (q1 != 0)
                {
                    sparse.Datas.Add(new Pair(4 * pair.Key + 1, 4 * pair.Value), q1);
                    sparse.Datas.Add(new Pair(4 * pair.Key, 4 * pair.Value + 1), -q1);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 3, 4 * pair.Value + 2), q1);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 2, 4 * pair.Value + 3), -q1);
                }

                if (q2 != 0)
                {
                    sparse.Datas.Add(new Pair(4 * pair.Key + 2, 4 * pair.Value), q2);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 3, 4 * pair.Value + 1), -q2);
                    sparse.Datas.Add(new Pair(4 * pair.Key, 4 * pair.Value + 2), -q2);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 1, 4 * pair.Value + 3), q2);
                }

                if (q3 != 0)
                {
                    sparse.Datas.Add(new Pair(4 * pair.Key + 3, 4 * pair.Value), q3);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 2, 4 * pair.Value + 1), q3);
                    sparse.Datas.Add(new Pair(4 * pair.Key, 4 * pair.Value + 3), -q3);
                    sparse.Datas.Add(new Pair(4 * pair.Key + 1, 4 * pair.Value + 2), -q3);
                }

            }

            sparse.ToTriplet(out colIndices, out rowIndices, out values, out nnz);

        }

        #endregion
    }

}
