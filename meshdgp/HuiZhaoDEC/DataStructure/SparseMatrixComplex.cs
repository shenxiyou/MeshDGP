using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace GraphicResearchHuiZhao
{
    public class SparseMatrixComplex
    {
        private Dictionary<Pair, Complex> mapData;

        public Dictionary<Pair, Complex> Datas
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
        public SparseMatrixComplex(int rows, int columns)
        {
            rowCount = rows;
            columnCount = columns;
            mapData = new Dictionary<Pair, Complex>();
        }

        public SparseMatrixComplex()
        {
            rowCount = 0;
            columnCount = 0;
            mapData = new Dictionary<Pair, Complex>();
        }

        public SparseMatrixComplex(SparseMatrixComplex matrix)
        {
            mapData = new Dictionary<Pair, Complex>(matrix.mapData);
            this.rowCount = matrix.rowCount;
            this.columnCount = matrix.columnCount;
        }

        #endregion

        #region Operators

        public static SparseMatrixComplex operator *(double left, SparseMatrixComplex right)
        {
            SparseMatrixComplex result = new SparseMatrixComplex(right.rowCount, right.columnCount);

            foreach (KeyValuePair<Pair, Complex> item in right.mapData)
            {
                Pair pair = item.Key;
                Complex value = left * item.Value;

                result.mapData.Add(pair, value);
            }

            return result;
        }

        public static SparseMatrixComplex operator *(Complex left, SparseMatrixComplex right)
        {
            SparseMatrixComplex result = new SparseMatrixComplex(right.rowCount, right.columnCount);

            foreach (KeyValuePair<Pair, Complex> item in right.mapData)
            {
                Pair pair = item.Key;
                Complex value = left * item.Value;

                result.mapData.Add(pair, value);
            }

            return result;
        }

        public static SparseMatrixComplex operator *(SparseMatrixComplex left, double right)
        {
            return right * left;
        }

        public static SparseMatrixComplex operator *(SparseMatrixComplex left, Complex right)
        {
            return right * left;
        }

        public static SparseMatrixComplex operator *(SparseMatrixComplex left, SparseMatrixComplex right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.rowCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            SparseMatrixComplex result = new SparseMatrixComplex(left.rowCount, right.columnCount);

            int leftNNZ = left.mapData.Count;
            int rightNNZ = right.mapData.Count;

            #region Left < Right
            //We use right as stardand sight
            //if (leftNNZ < rightNNZ)
            //{
            //Connection nonezero for each row of matrix a 
            List<KeyValuePair<int, Complex>>[] bRows = new List<KeyValuePair<int, Complex>>[right.rowCount];
            for (int i = 0; i < bRows.Length; i++)
            {
                bRows[i] = new List<KeyValuePair<int, Complex>>();
            }


            foreach (KeyValuePair<Pair, Complex> item in right.mapData)
            {
                Pair pair = item.Key;
                Complex value = item.Value;

                bRows[pair.Key].Add(new KeyValuePair<int, Complex>(pair.Value, value));
            }

            //Compute C = A*B
            foreach (KeyValuePair<Pair, Complex> item in left.mapData)
            {
                Pair pair = item.Key;
                int mA = pair.Key;
                int nA = pair.Value;
                Complex value = item.Value;

                List<KeyValuePair<int, Complex>> bRow = bRows[nA];

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

        public static Complex[] operator *(double[] left, SparseMatrixComplex right)
        {
            //Make sure matrix dimensions are equal
            if (left.Length != right.rowCount)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            Complex[] result = new Complex[right.columnCount];

            foreach (KeyValuePair<Pair, Complex> item in right.mapData)
            {
                Pair pair = item.Key;
                Complex value = item.Value;

                int m = pair.Key;
                int n = pair.Value;

                //M: mutiply index N: vector store index
                result[n] += left[m] * value;
            }

            return result;
        }

        public static Complex[] operator *(SparseMatrixComplex left, Complex[] right)
        {
            //Make sure matrix dimensions are equal
            if (left.columnCount != right.Length)
            {
                throw new Exception("The dimension of two matrix must be equal");
            }

            Complex[] result = new Complex[left.columnCount];

            foreach (KeyValuePair<Pair, Complex> item in left.mapData)
            {
                Pair pair = item.Key;
                Complex value = item.Value;

                int m = pair.Key;
                int n = pair.Value;

                //M: mutiply index N: vector store index
                result[m] += right[n] * value;
            }

            return result;
        }

        public static SparseMatrixComplex operator +(SparseMatrixComplex left, SparseMatrixComplex right)
        {
            SparseMatrixComplex result = new SparseMatrixComplex(right.rowCount, right.columnCount);

            foreach (KeyValuePair<Pair, Complex> item in left.mapData)
            {
                Pair pair = item.Key;
                Complex value = item.Value;

                result.mapData.Add(pair, value);
            }

            foreach (KeyValuePair<Pair, Complex> item in right.mapData)
            {
                Pair pair = item.Key;
                Complex value = item.Value;

                if (result.mapData.ContainsKey(pair))
                {
                    Complex temp = result.mapData[pair] += item.Value;
                    if (temp.Equals(Complex.Zero))
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

        public static SparseMatrixComplex operator -(SparseMatrixComplex left, SparseMatrixComplex right)
        {
            SparseMatrixComplex result = new SparseMatrixComplex(right.rowCount, right.columnCount);

            foreach (KeyValuePair<Pair, Complex> item in left.mapData)
            {
                Pair pair = item.Key;
                Complex value = item.Value;

                result.mapData.Add(pair, value);
            }

            foreach (KeyValuePair<Pair, Complex> item in right.mapData)
            {
                Pair pair = item.Key;
                Complex value = item.Value;

                if (result.mapData.ContainsKey(pair))
                {
                    Complex temp = result.mapData[pair] -= item.Value;
                    if (temp.Equals(Complex.Zero))
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
        public Complex this[int row, int column]
        {
            set
            {
                if (row > rowCount - 1 || column > columnCount - 1)
                {
                    throw new IndexOutOfRangeException();
                }

                Pair pair = new Pair(row, column);

                if (value.Equals(Complex.Zero))
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
                    return Complex.Zero;
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

            foreach (KeyValuePair<Pair, Complex> item in mapData)
            {
                Pair pair = item.Key;
                if (pair.Key >= pair.Value)
                {
                    continue;
                }

                Complex value = item.Value;

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

        public SparseMatrixComplex Transpose()
        {

            SparseMatrixComplex trMatrix = new SparseMatrixComplex(this.columnCount, this.rowCount);

            foreach (KeyValuePair<Pair, Complex> item in mapData)
            {
                Pair pair = item.Key;
                Complex value = item.Value;

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

        public SparseMatrixComplex SubMatrix(int rowStart, int rowEnd, int columnStart, int columnEnd)
        {
            if (rowEnd < rowStart || columnEnd < columnStart)
            {
                throw new ArgumentOutOfRangeException();
            }

            int rows = rowEnd - rowStart + 1;
            int columns = columnEnd - columnStart + 1;

            SparseMatrixComplex subMatrix = new SparseMatrixComplex(rows, columns);

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
                foreach (KeyValuePair<Pair, Complex> item in mapData)
                {
                    Pair pair = item.Key;
                    Complex value = item.Value;

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

        public SparseMatrixComplex Inverse()
        {
            return null;
        }

        #endregion

        #region Utils

        public static SparseMatrixComplex Identity(int N)
        {
            SparseMatrixComplex identity = new SparseMatrixComplex(N, N);

            for (int i = 0; i < N; i++)
            {
                identity.mapData.Add(new Pair(i, i), Complex.Identity);
            }

            return identity;
        }

        public static SparseMatrixComplex Copy(ref SparseMatrixDouble B)
        {
            //We only copy of realpart
            Dictionary<Pair, Complex> newData = new Dictionary<Pair, Complex>();

            foreach (KeyValuePair<Pair, double> e in B.Datas)
            {
                Pair pair = e.Key;
                double value = e.Value;

                newData.Add(pair, new Complex(value, 0));
            }


            SparseMatrixComplex newMatrix = new SparseMatrixComplex(B.RowCount, B.ColumnCount);
            newMatrix.mapData = newData;

            return newMatrix;
        }


        public static SparseMatrixComplex Copy(ref SparseMatrixComplex B)
        {
            Dictionary<Pair, Complex> newData = new Dictionary<Pair, Complex>(B.mapData);

            SparseMatrixComplex newMatrix = new SparseMatrixComplex(B.rowCount, B.columnCount);
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

        public static int CompareKey(KeyValuePair<int, Complex> a, KeyValuePair<int, Complex> b)
        {
            return a.Key.CompareTo(b.Key);
        }

        public void WriteToFile(String path)
        {
            StreamWriter sw = sw = File.CreateText(path);

            //Output matrix attributes
            sw.WriteLine("#Row: " + 4 * this.rowCount + " Columns: " + 4 * this.columnCount + " nnz: " + 16 * this.NNZ);

            sw.WriteLine("#Symmetric: false N");

            foreach (KeyValuePair<Pair, Complex> e in mapData)
            {
                Pair pair = e.Key;

                Complex v = e.Value;

                sw.WriteLine(pair.Key + " " + pair.Value + " " + v.RealPart + "+" + v.ImagePart + "i");
            }

            sw.Close();
        }


        public void ToCRS(out int[] rowPtr, out int[] colIndices, out double[] values, out int nnz)   //z is the image part of compelx
        {
            rowPtr = new int[this.RowCount + 1];
            colIndices = new int[this.NNZ];
            values = new double[2 * this.NNZ];

            nnz = this.NNZ;

            //Connection nonezero for each row of matrix a 
            List<KeyValuePair<int, Complex>>[] bRows = new List<KeyValuePair<int, Complex>>[rowCount];
            for (int i = 0; i < bRows.Length; i++)
            {
                bRows[i] = new List<KeyValuePair<int, Complex>>();
            }

            foreach (KeyValuePair<Pair, Complex> e in mapData)
            {
                Pair pair = e.Key;
                Complex value = e.Value;

                bRows[pair.Key].Add(new KeyValuePair<int, Complex>(pair.Value, value));
            }


            for (int i = 0; i < bRows.Length; i++)
            {
                bRows[i].Sort(CompareKey);
            }

            int item = 0;
            int rowSum = 0;
            for (int i = 0; i < rowCount; i++)
            {
                List<KeyValuePair<int, Complex>> bRow = bRows[i];

                foreach (KeyValuePair<int, Complex> pair in bRow)
                {
                    colIndices[item] = pair.Key;
                    values[2 * item] = pair.Value.RealPart;
                    values[2 * item + 1] = pair.Value.ImagePart;

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
            values = new double[2 * this.NNZ];
            nnz = this.NNZ;

            List<KeyValuePair<int, Complex>>[] aCols = new List<KeyValuePair<int, Complex>>[columnCount];
            for (int i = 0; i < aCols.Length; i++)
            {
                aCols[i] = new List<KeyValuePair<int, Complex>>();
            }

            foreach (KeyValuePair<Pair, Complex> item in mapData)
            {
                Pair pair = item.Key;
                Complex value = item.Value;

                //Add column to each correlate col list with key of row index and value of entity value
                aCols[pair.Value].Add(new KeyValuePair<int, Complex>(pair.Key, value));
            }

            for (int i = 0; i < aCols.Length; i++)
            {
                aCols[i].Sort(CompareKey);
            }

            int aitem = 0;
            int colSum = 0;
            for (int i = 0; i < columnCount; i++)
            {
                List<KeyValuePair<int, Complex>> aCol = aCols[i];

                foreach (KeyValuePair<int, Complex> pair in aCol)
                {
                    rowIndices[aitem] = pair.Key;
                    values[2 * aitem] = pair.Value.RealPart;
                    values[2 * aitem + 1] = pair.Value.ImagePart;
                    aitem++;
                }

                colSum += aCol.Count;
                colPtr[i + 1] = colSum;
            }

        }

        public void ToTriplet(out int[] colIndices, out int[] rowIndices, out double[] values, out double[] z, out int nnz)
        {
            colIndices = new int[this.columnCount + 1];
            rowIndices = new int[this.NNZ];
            values = new double[this.NNZ];
            z = new double[this.NNZ];

            nnz = this.NNZ;

            int count = 0;
            foreach (KeyValuePair<Pair, Complex> item in mapData)
            {
                Pair pair = item.Key;
                Complex value = item.Value;

                colIndices[count] = pair.Key;
                rowIndices[count] = pair.Value;
                values[2 * count] = value.RealPart;
                values[2 * count + 1] = value.ImagePart;

                count++;
            }


        }

        #endregion
    }

}
