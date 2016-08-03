﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class CCSMatrix
    {
        private int m;
        private int n;
        private int[] rowIndex;
        private int[] colIndex;
        private double[] values;

        public int RowSize
        {
            get { return m; }
        }
        public int ColumnSize
        {
            get { return n; }
        }
        public int[] RowIndex
        {
            get { return rowIndex; }
        }
        public int[] ColIndex
        {
            get { return colIndex; }
        }
        public double[] Values
        {
            get { return values; }
        }


        public CCSMatrix(SparseMatrix matrix)
        {
            // get number of non-zero elements
            m = matrix.RowSize;
            n = matrix.RowSize;
            int nnz = 0;
            foreach (List<SparseMatrix.Element> col in matrix.Columns) nnz += col.Count;

            // create temp arrays
            rowIndex = new int[nnz];
            colIndex = new int[n + 1];
            values = new double[nnz];

            // copy values to arrays
            int index = 0;
            int index2 = 0;
            colIndex[0] = 0;
            foreach (List<SparseMatrix.Element> col in matrix.Columns)
            {
                foreach (SparseMatrix.Element e in col)
                {
                    rowIndex[index] = e.i;
                    values[index] = e.value;
                    index++;
                }
                colIndex[++index2] = index;
            }
        }
        public CCSMatrix(SparseMatrix matrix, bool transponse)
        {
            // get number of non-zero elements
            m = matrix.ColumnSize;
            n = matrix.RowSize;
            int nnz = 0;
            foreach (List<SparseMatrix.Element> col in matrix.Columns) nnz += col.Count;

            // create temp arrays
            rowIndex = new int[nnz];
            colIndex = new int[n + 1];
            values = new double[nnz];

            // copy values to arrays
            int index = 0;
            int index2 = 0;
            colIndex[0] = 0;
            foreach (List<SparseMatrix.Element> row in matrix.Rows)
            {
                foreach (SparseMatrix.Element e in row)
                {
                    rowIndex[index] = e.j;
                    values[index] = e.value;
                    index++;
                }
                colIndex[++index2] = index;
            }
        }

        public void Multiply(double[] xIn, double[] xOut)
        {
            if (xIn.Length < n || xOut.Length < m) throw new ArgumentException();

            for (int i = 0; i < m; i++) xOut[i] = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = colIndex[i]; j < colIndex[i + 1]; j++)
                {
                    int r = rowIndex[j];
                    xOut[r] += values[j] * xIn[i];
                }
            }
        }
        public void PreMultiply(double[] xIn, double[] xOut)
        {
            if (xIn.Length < m || xOut.Length < n) throw new ArgumentException();

            for (int i = 0; i < n; i++) xOut[i] = 0;

            for (int i = 0; i < n; i++)
            {
                double sum = 0.0;
                for (int j = colIndex[i]; j < colIndex[i + 1]; j++)
                {
                    int r = rowIndex[j];
                    sum += values[j] * xIn[r];
                }
                xOut[i] = sum;
            }
        }

        public void CG(double[] x, double[] b, double[] inv, double eps, int maxIter)
        {
            double[] r = new double[m];
            double[] d = new double[m];
            double[] q = new double[m];
            double[] s = new double[m];
            double err, new_err, old_err, tmp;

            for (int i = 0; i < m; i++) r[i] = b[i];
            for (int i = 0; i < n; i++)
                for (int j = colIndex[i]; j < colIndex[i + 1]; j++)
                    r[rowIndex[j]] -= values[j] * x[i];

            new_err = 0;
            for (int i = 0; i < m; i++)
            {
                d[i] = inv[i] * r[i];
                new_err += d[i] * r[i];
            }
            err = new_err;

            OutputText("start: " + new_err.ToString());
            OutputText("err: " + (eps * eps * err).ToString());

            int iter = 0;
            while (iter < maxIter && new_err > eps * eps * err)
            {
                Multiply(d, q);

                tmp = 0;
                for (int i = 0; i < m; i++) tmp += d[i] * q[i];
                double alpha = new_err / tmp;
                for (int i = 0; i < m; i++) x[i] += alpha * d[i];
                for (int i = 0; i < m; i++) if (double.IsNaN(x[i])) throw new Exception();

                if (iter % 50 == 0)
                {
                    for (int i = 0; i < m; i++) r[i] = b[i];
                    for (int i = 0; i < n; i++)
                        for (int j = colIndex[i]; j < colIndex[i + 1]; j++)
                            r[rowIndex[j]] -= values[j] * x[i];
                            OutputText(iter.ToString() + ": " + new_err.ToString());
                }
                else
                {
                    for (int i = 0; i < m; i++) r[i] -= alpha * q[i];
                }

                for (int i = 0; i < m; i++) s[i] = inv[i] * r[i];

                old_err = new_err;
                new_err = 0;
                for (int i = 0; i < m; i++) new_err += r[i] * s[i];

                double beta = new_err / old_err;

                for (int i = 0; i < m; i++) d[i] = s[i] + beta * d[i];

                iter++;

            }
        }


        public void OutputText(string text)
        {

        }
    }
}
