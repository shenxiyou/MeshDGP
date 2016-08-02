using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace GraphicResearchHuiZhao
{

    public class SparseMatrixDouble
    {
        private Dictionary<Pair, double> mapData;

        public Dictionary<Pair, double> Datas
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
        public SparseMatrixDouble(int rows, int columns)
        {
            rowCount = rows;
            columnCount = columns;
            mapData = new Dictionary<Pair, double>();
        }

        public SparseMatrixDouble(double[,] arrays)
        {
            int m = arrays.GetLength(0);
            int n = arrays.GetLength(1);
            mapData = new Dictionary<Pair, double>();
            rowCount = m;
            columnCount = n;

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (arrays[i, j] != 0)
                    {
                        mapData.Add(new Pair(i, j), arrays[i, j]);
                    }
                }
            }

        }

        public SparseMatrixDouble()
        {
            rowCount = 0;
            columnCount = 0;
            mapData = new Dictionary<Pair, double>();
        }

        public SparseMatrixDouble(SparseMatrixDouble matrix)
        {
            mapData = new Dictionary<Pair, double>(matrix.mapData);
            this.rowCount = matrix.rowCount;
            this.columnCount = matrix.columnCount;
        }

        public SparseMatrixDouble(SparseMatrix matrix)
        {
            rowCount = matrix.Rows.Count;
            columnCount = matrix.Columns.Count;
            mapData = new Dictionary<Pair, double>();

            foreach (List<SparseMatrix.Element> rows in matrix.Rows)
            {
                foreach (SparseMatrix.Element item in rows)
                {
                    int m = item.i;
                    int n = item.j;
                    double value = item.value;
                    mapData.Add(new Pair(m, n), value);
                }
            }

        }


        public SparseMatrix Convert()
        {
            SparseMatrix result = new SparseMatrix(this.RowCount, this.ColumnCount);
            foreach (KeyValuePair<Pair, double> item in this.mapData)
            {
                Pair pair = item.Key;
                double value = item.Value;

                result.AddElement(pair.Key,pair.Value, value);
            }

            return result;
        }



        #endregion

        #region Operators

        public static SparseMatrixDouble operator *(double left, SparseMatrixDouble right)
        {
            SparseMatrixDouble result = new SparseMatrixDouble(right.rowCount, right.columnCount);

            foreach (KeyValuePair<Pair, double> item in right.mapData)
            {
                Pair pair = item.Key;
                double value = left * item.Value;

                result.mapData.Add(pair, value);
            }

            return result;
        }

        public static SparseMatrixDouble operator *(SparseMatrixDouble left, double right)
        {
            return right * left;
        }

        public static SparseMatrixDouble operator *(SparseMatrixDouble left, SparseMatrixDouble right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.rowCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            SparseMatrixDouble result = new SparseMatrixDouble(left.rowCount, right.columnCount);

            int leftNNZ = left.mapData.Count;
            int rightNNZ = right.mapData.Count;

            #region Left < Right
            //We use right as stardand sight
            //if (leftNNZ < rightNNZ)
            //{
            //Connection nonezero for each row of matrix a 
            List<KeyValuePair<int, double>>[] bRows = new List<KeyValuePair<int, double>>[right.rowCount];
            for (int i = 0; i < bRows.Length; i++)
            {
                bRows[i] = new List<KeyValuePair<int, double>>();
            }


            foreach (KeyValuePair<Pair, double> item in right.mapData)
            {
                Pair pair = item.Key;
                double value = item.Value;

                bRows[pair.Key].Add(new KeyValuePair<int, double>(pair.Value, value));
            }

            //Compute C = A*B
            foreach (KeyValuePair<Pair, double> item in left.mapData)
            {
                Pair pair = item.Key;
                int mA = pair.Key;
                int nA = pair.Value;
                double value = item.Value;

                List<KeyValuePair<int, double>> bRow = bRows[nA];

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

        public static double[] operator *(double[] left, SparseMatrixDouble right)
        {
            //Make sure matrix dimensions are equal
            if (left.Length != right.rowCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            double[] result = new double[right.columnCount];

            foreach (KeyValuePair<Pair, double> item in right.mapData)
            {
                Pair pair = item.Key;
                double value = item.Value;

                int m = pair.Key;
                int n = pair.Value;

                //M: mutiply index N: vector store index
                result[n] += left[m] * value;
            }

            return result;
        }

        public static double[] operator *(SparseMatrixDouble left, double[] right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.Length)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            double[] result = new double[left.columnCount];

            foreach (KeyValuePair<Pair, double> item in left.mapData)
            {
                Pair pair = item.Key;
                double value = item.Value;

                int m = pair.Key;
                int n = pair.Value;

                //M: mutiply index N: vector store index
                result[m] += right[n] * value;
            }

            return result;
        }

        public static SparseMatrixDouble operator +(SparseMatrixDouble left, SparseMatrixDouble right)
        {
            SparseMatrixDouble result = new SparseMatrixDouble(right.rowCount, right.columnCount);

            foreach (KeyValuePair<Pair, double> item in left.mapData)
            {
                Pair pair = item.Key;
                double value = item.Value;

                result.mapData.Add(pair, value);
            }

            foreach (KeyValuePair<Pair, double> item in right.mapData)
            {
                Pair pair = item.Key;
                double value = item.Value;

                if (result.mapData.ContainsKey(pair))
                {
                    double temp = result.mapData[pair] += item.Value;
                    if (temp == 0)
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

        public static SparseMatrixDouble operator -(SparseMatrixDouble left, SparseMatrixDouble right)
        {
            SparseMatrixDouble result = new SparseMatrixDouble(right.rowCount, right.columnCount);

            foreach (KeyValuePair<Pair, double> item in left.mapData)
            {
                Pair pair = item.Key;
                double value = item.Value;

                result.mapData.Add(pair, value);
            }

            foreach (KeyValuePair<Pair, double> item in right.mapData)
            {
                Pair pair = item.Key;
                double value = item.Value;

                if (result.mapData.ContainsKey(pair))
                {
                    double temp = result.mapData[pair] -= item.Value;
                    if (temp == 0)
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
        public double this[int row, int column]
        {
            set
            {
                if (row > rowCount - 1 || column > columnCount - 1)
                {
                    throw new IndexOutOfRangeException();
                }

                Pair pair = new Pair(row, column);

                if (value == 0)
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
                    return 0;
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

            foreach (KeyValuePair<Pair, double> item in mapData)
            {
                Pair pair = item.Key;
                if (pair.Key >= pair.Value)
                {
                    continue;
                }

                double value = item.Value;

                int m = pair.Key;
                int n = pair.Value;

                if (mapData[new Pair(n, m)] != value)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Elemental Transf.

        public SparseMatrixDouble Transpose()
        {

            SparseMatrixDouble trMatrix = new SparseMatrixDouble(this.columnCount, this.rowCount);

            foreach (KeyValuePair<Pair, double> item in mapData)
            {
                Pair pair = item.Key;
                double value = item.Value;

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

        public SparseMatrixDouble SubMatrix(int rowStart, int rowEnd, int columnStart, int columnEnd)
        {
            if (rowEnd < rowStart || columnEnd < columnStart)
            {
                throw new ArgumentOutOfRangeException();
            }

            int rows = rowEnd - rowStart + 1;
            int columns = columnEnd - columnStart + 1;

            SparseMatrixDouble subMatrix = new SparseMatrixDouble(rows, columns);

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
                foreach (KeyValuePair<Pair, double> item in mapData)
                {
                    Pair pair = item.Key;
                    double value = item.Value;

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

        public SparseMatrixDouble Inverse()
        {
            return null;
        }

        #endregion

        #region Utils

        public static SparseMatrixDouble Identity(int N)
        {
            SparseMatrixDouble identity = new SparseMatrixDouble(N, N);

            for (int i = 0; i < N; i++)
            {
                identity.mapData.Add(new Pair(i, i), 1);
            }

            return identity;
        }

        public static SparseMatrixDouble Copy(ref SparseMatrixDouble B)
        {
            Dictionary<Pair, double> newData = new Dictionary<Pair, double>(B.mapData);

            SparseMatrixDouble newMatrix = new SparseMatrixDouble(B.rowCount, B.columnCount);
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

        public static int CompareKey(KeyValuePair<int, double> a, KeyValuePair<int, double> b)
        {
            return a.Key.CompareTo(b.Key);
        }

        public void WriteToFile(String path)
        {
            StreamWriter sw = sw = File.CreateText(path);

            //Output matrix attributes
            sw.WriteLine("#Row: " + this.rowCount + " Columns: " + this.columnCount + " nnz: " + this.NNZ);

            sw.WriteLine("#Symmetric: false N");

            foreach (KeyValuePair<Pair, double> e in mapData)
            {
                Pair pair = e.Key;
                double value = e.Value;

                sw.WriteLine(pair.Key + " " + pair.Value + " " + value);
            }

            sw.Close();
        }

        public static SparseMatrixDouble ReadFromFile(String path)
        {
            StreamReader sr = new StreamReader(path);
            String line = null;

            int m = int.MinValue;
            int n = int.MinValue;

            int count = 0;
            SparseMatrixDouble sm = null;

            while ((line = sr.ReadLine()) != null)
            {
                String[] token = line.Split(' ');

                if (count == 0)
                {
                    m = int.Parse(token[1]);
                    n = int.Parse(token[3]);
                    int nnz = int.Parse(token[5]);
                    sm = new SparseMatrixDouble(m, n);

                    count++;
                    continue;
                }

                int index_i = int.Parse(token[0]);
                int index_j = int.Parse(token[1]);
                double value = double.Parse(token[2]);
                if (value != 0)
                {
                    Pair newPair = new Pair(index_i, index_j);
                    sm.Datas.Add(newPair, value);
                }

            }

            sr.Close();

            GC.Collect();
            return sm;
        }

        public void ToCRS(out int[] rowPtr, out int[] colIndices, out double[] values, out int nnz)
        {
            rowPtr = new int[this.RowCount + 1];
            colIndices = new int[this.NNZ];
            values = new double[this.NNZ];
            nnz = this.NNZ;

            //Connection nonezero for each row of matrix a 
            List<KeyValuePair<int, double>>[] bRows = new List<KeyValuePair<int, double>>[rowCount];
            for (int i = 0; i < bRows.Length; i++)
            {
                bRows[i] = new List<KeyValuePair<int, double>>();
            }

            foreach (KeyValuePair<Pair, double> e in mapData)
            {
                Pair pair = e.Key;
                double value = e.Value;

                bRows[pair.Key].Add(new KeyValuePair<int, double>(pair.Value, value));
            }


            for (int i = 0; i < bRows.Length; i++)
            {
                bRows[i].Sort(CompareKey);
            }

            int item = 0;
            int rowSum = 0;
            for (int i = 0; i < rowCount; i++)
            {
                List<KeyValuePair<int, double>> bRow = bRows[i];

                foreach (KeyValuePair<int, double> pair in bRow)
                {
                    colIndices[item] = pair.Key;
                    values[item] = pair.Value;
                    item++;
                }

                rowSum += bRow.Count;
                rowPtr[i + 1] = rowSum;
            }

        }

        public void ToCCS(out int[] colPtr, out int[] rowIndices, out double[] values, out int nnz)
        {
            colPtr = new int[this.columnCount + 1];
            rowIndices = new int[this.NNZ];
            values = new double[this.NNZ];
            nnz = this.NNZ;

            List<KeyValuePair<int, double>>[] aCols = new List<KeyValuePair<int, double>>[columnCount];
            for (int i = 0; i < aCols.Length; i++)
            {
                aCols[i] = new List<KeyValuePair<int, double>>();
            }

            foreach (KeyValuePair<Pair, double> item in mapData)
            {
                Pair pair = item.Key;
                double value = item.Value;

                //Add column to each correlate col list with key of row index and value of entity value
                aCols[pair.Value].Add(new KeyValuePair<int, double>(pair.Key, value));
            }

            for (int i = 0; i < aCols.Length; i++)
            {
                aCols[i].Sort(CompareKey);
            }

            int aitem = 0;
            int colSum = 0;
            for (int i = 0; i < columnCount; i++)
            {
                List<KeyValuePair<int, double>> aCol = aCols[i];

                foreach (KeyValuePair<int, double> pair in aCol)
                {
                    rowIndices[aitem] = pair.Key;
                    values[aitem] = pair.Value;
                    aitem++;
                }
                /*
                 * 
                 */

                colSum += aCol.Count;
                colPtr[i + 1] = colSum;
            }


        }

        public void ToTriplet(out int[] colIndices, out int[] rowIndices, out double[] values, out int nnz)
        {
            colIndices = new int[this.NNZ];
            rowIndices = new int[this.NNZ];
            values = new double[this.NNZ];
            nnz = this.NNZ;

            int count = 0;
            foreach (KeyValuePair<Pair, double> item in mapData)
            {
                Pair pair = item.Key;
                double value = item.Value;

                colIndices[count] = pair.Key;
                rowIndices[count] = pair.Value;
                values[count] = value;
                count++;
            }


        }

        #endregion
    }

}
