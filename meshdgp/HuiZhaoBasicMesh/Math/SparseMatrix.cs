using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace GraphicResearchHuiZhao
{
    [Serializable]
    public class SparseMatrix
    {
        #region Helper Classes
        public class Element
        {
            public int i, j;
            public double value;

            public Element(int i, int j, double value)
            {
                this.i = i;
                this.j = j;
                this.value = value;
            }
        }

        private class RowComparer : IComparer<Element>
        {
            #region IComparer Members
            public int Compare(Element e1, Element e2)
            {
                return e1.j - e2.j;
            }
            #endregion
        }

        private class ColumnComparer : IComparer<Element>
        {
            #region IComparer Members
            public int Compare(Element e1, Element e2)
            {
                return e1.i - e2.i;
            }
            #endregion
        }

        #endregion

        private int m, n;
        private List<List<Element>> rows, columns;

        public int RowSize { get { return m; } }

        public int WholeSize { get { return m*n; } }
        public int ZeroSize { get { return m * n-NumOfElements(); } }
        public int ColumnSize { get { return n; } }
        public int NumOfElements()
        {
            int count = 0;
            if (m < n)
                foreach (List<Element> r in rows) count += r.Count;
            else
                foreach (List<Element> c in columns) count += c.Count;
            return count;
        }
        public List<List<Element>> Rows { get { return rows; } }
        public List<List<Element>> Columns { get { return columns; } }
        public List<Element> GetRow(int index) { return (List<Element>)rows[index]; }
        public List<Element> GetColumn(int index) { return (List<Element>)columns[index]; }

        public double this[int row, int column]
        {
            get
            {
                List<Element> r = this.rows[row];
                Element e = null;
                foreach (Element item in r)
                {
                    if (item.j == column)
                    {
                        e = item;
                        break;
                    }
                }

                if (e == null)
                {
                    return 0;
                }
                else
                {
                    return e.value;
                }
            }
            set
            {
                List<Element> r = this.rows[row];
                Element e = null;
                foreach (Element item in r)
                {
                    if (item.j == column)
                    {
                        e = item;
                        break;
                    }
                }

                if (e == null)
                {
                    this.AddValueTo(row, column, value);
                }
                else
                {
                    e.value = value;
                }
            }
        }

        public SparseMatrix(int m, int n)
        {
            this.m = m;
            this.n = n;
            rows = new List<List<Element>>(m);
            columns = new List<List<Element>>(n);

            for (int i = 0; i < m; i++)
                rows.Add(new List<Element>());
            for (int i = 0; i < n; i++)
                columns.Add(new List<Element>());
        }
        public SparseMatrix(int m, int n, int nElements)
        {
            this.m = m;
            this.n = n;
            rows = new List<List<Element>>(m);
            columns = new List<List<Element>>(n);

            for (int i = 0; i < m; i++)
                rows.Add(new List<Element>(nElements));
            for (int i = 0; i < n; i++)
                columns.Add(new List<Element>(nElements));
        }

        public static SparseMatrix Identity(int n)
        {
            SparseMatrix identity = new SparseMatrix(n, n);
            for (int i = 0; i < n; i++)
            {
                identity.AddElement(i, i, 1);
            }
            return identity;
        }

        public SparseMatrix(SparseMatrix right)
        {
            m = right.m;
            n = right.n;
            rows = new List<List<Element>>(m);
            columns = new List<List<Element>>(n);
            for (int i = 0; i < m; i++)
                rows.Add(new List<Element>());
            for (int i = 0; i < n; i++)
                columns.Add(new List<Element>());
            foreach (List<Element> list in right.Rows)
                foreach (Element e in list)
                    AddElement(e.i, e.j, e.value);
        }
        public override bool Equals(object obj)
        {
            SparseMatrix right = obj as SparseMatrix;
            if (obj == null) return false;
            if (right.m != m) return false;
            if (right.n != n) return false;

            for (int i = 0; i < n; i++)
            {
                List<Element> c1 = columns[i] as List<Element>;
                List<Element> c2 = right.columns[i] as List<Element>;
                if (c1.Count != c2.Count) return false;
                for (int j = 0; j < c1.Count; j++)
                {
                    Element e1 = c1[j] as Element;
                    Element e2 = c2[j] as Element;
                    if (e1.j != e2.j) return false;
                    if (e1.value != e2.value) return false;
                }
            }
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() + this.NumOfElements();
        }
        public bool CheckElements()
        {
            foreach (List<Element> r in rows)
                foreach (Element e in r)
                    if (Double.IsInfinity(e.value) ||
                        Double.IsNaN(e.value) ||
                        e.value == 0.0)
                        return false;
            return true;
        }
        public bool IsSymmetric()
        {
            if (m != n) return false;

            for (int i = 0; i < m; i++)
            {
                List<Element> row = GetRow(i);
                List<Element> col = GetColumn(i);

                if (row.Count != col.Count)
                    return false;

                for (int j = 0; j < row.Count; j++)
                {
                    SparseMatrix.Element e1 = row[j] as SparseMatrix.Element;
                    SparseMatrix.Element e2 = col[j] as SparseMatrix.Element;
                    if (e1.i != e2.j)
                        return false;
                    if (e1.j != e2.i)
                        return false;
                    //if (e1.value != e2.value) return false;
                }
            }

            return true;

        }
        public Element AddElement(int i, int j, double value)
        {
            List<Element> r = rows[i] as List<Element>;
            List<Element> c = columns[j] as List<Element>;
            Element e = new Element(i, j, value);
            r.Add(e);
            c.Add(e);
            return e;
        }
        public Element AddElement(Element e)
        {
            List<Element> r = rows[e.i] as List<Element>;
            List<Element> c = columns[e.j] as List<Element>;
            r.Add(e);
            c.Add(e);
            return e;
        }
        public Element FindElement(int i, int j)
        {
            List<Element> rr = rows[i] as List<Element>;
            foreach (Element e in rr)
                if (e.j == j) return e;
            return null;
        }
        public Element AddElementIfNotExist(int i, int j, double value)
        {
            Element e = FindElement(i, j);
            if (e == null)
                return AddElement(i, j, value);
            else
                return null;
        }
        public Element AddValueTo(int i, int j, double value)
        {
            Element e = FindElement(i, j);
            if (e == null)
            {
                e = new Element(i, j, 0);
                AddElement(e);
            }

            e.value += value;
            return e;
        }
        public void SortElement()
        {
            RowComparer rComparer = new RowComparer();
            ColumnComparer cComparer = new ColumnComparer();
            foreach (List<Element> r in rows) r.Sort(rComparer);
            foreach (List<Element> c in columns) c.Sort(cComparer);
        }

        public void AddRow()
        {
            rows.Add(new List<Element>());
            m++;
        }
        public void AddColumn()
        {
            columns.Add(new List<Element>());
            n++;
        }
        public double[] Multiply(double[] xIn )
        {
 
            double[] xOut=new double[m] ;
            for (int i = 0; i < m; i++)
            {
                List<Element> r = rows[i] as List<Element>;
                double sum = 0.0;
                foreach (Element e in r) sum += e.value * xIn[e.j];
                xOut[i] = sum;
            }

            return xOut;
        }

        public void Multiply(double[] xIn, double[] xOut)
        {
            if (xIn.Length < n || xOut.Length < m) throw new ArgumentException();

            for (int i = 0; i < m; i++)
            {
                List<Element> r = rows[i] as List<Element>;
                double sum = 0.0;
                foreach (Element e in r) sum += e.value * xIn[e.j];
                xOut[i] = sum;
            }
        }
        public void Multiply(double[] xIn, int indexIn, double[] xOut, int indexOut)
        {
            if (xIn.Length - indexIn < n || xOut.Length - indexOut < m) throw new ArgumentException();

            for (int i = 0; i < m; i++)
            {
                List<Element> r = rows[i] as List<Element>;
                double sum = 0.0;
                foreach (Element e in r) sum += e.value * xIn[e.j + indexIn];
                xOut[i + indexOut] = sum;
            }
        }
        public void PreMultiply(double[] xIn, double[] xOut)
        {
            if (xIn.Length < m || xOut.Length < n) throw new ArgumentException();

            for (int j = 0; j < n; j++)
            {
                List<Element> c = columns[j] as List<Element>;
                double sum = 0.0;
                foreach (Element e in c) sum += e.value * xIn[e.i];
                xOut[j] = sum;
            }
        }
        public void Scale(double s)
        {
            foreach (List<Element> list in rows)
                foreach (Element e in list)
                    e.value *= s;
        }

        public SparseMatrix Multiply(SparseMatrix right)
        {
            if (n != right.m) throw new ArgumentException();

            SparseMatrix ret = new SparseMatrix(m, right.n);

            for (int i = 0; i < Rows.Count; i++)
            {
                List<Element> rr = Rows[i] as List<Element>;

                for (int j = 0; j < right.Columns.Count; j++)
                {
                    List<Element> cc = right.Columns[j] as List<Element>;
                    int c1 = 0;
                    int c2 = 0;
                    double sum = 0;
                    bool used = false;

                    while (c1 < rr.Count && c2 < cc.Count)
                    {
                        Element e1 = rr[c1] as Element;
                        Element e2 = cc[c2] as Element;
                        //if (e1.j < e2.i) { c1++; continue; }
                        //if (e1.j > e2.i) { c2++; continue; }
                        sum += e1.value * e2.value;
                        c1++;
                        c2++;
                        used = true;
                    }
                    if (used) ret[i, j] = sum;
                }
            }

            return ret;
        }

        public SparseMatrix Add(SparseMatrix right)
        {
            if (m != right.m || n != right.n)
                throw new ArgumentException();

            SparseMatrix ret = new SparseMatrix(m, n);

            for (int i = 0; i < m; i++)
            {
                List<Element> r1 = Rows[i] as List<Element>;
                List<Element> r2 = right.Rows[i] as List<Element>;
                int c1 = 0;
                int c2 = 0;
                while (c1 < r1.Count && c2 < r2.Count)
                {
                    Element e1 = r1[c1] as Element;
                    Element e2 = r2[c2] as Element;
                    if (e1.j < e2.j)
                    {
                        c1++;
                        ret.AddElement(i, e1.j, e1.value);
                        continue;
                    }
                    if (e1.j > e2.j)
                    {
                        c2++;
                        ret.AddElement(i, e2.j, e2.value);
                        continue;
                    }
                    ret.AddElement(i, e1.j, e1.value + e2.value);
                    c1++;
                    c2++;
                }
                while (c1 < r1.Count)
                {
                    Element e = r1[c1] as Element;
                    ret.AddElement(e.i, e.j, e.value);
                    c1++;
                }
                while (c2 < r2.Count)
                {
                    Element e = r2[c2] as Element;
                    ret.AddElement(e.i, e.j, e.value);
                    c2++;
                }
            }
            return ret;
        }
        public SparseMatrix Transpose()
        {
            SparseMatrix ret = new SparseMatrix(this);
            int t = ret.m;
            ret.m = ret.n;
            ret.n = t;
            List<List<Element>> tmp = ret.rows;
            ret.rows = ret.columns;
            ret.columns = tmp;
            foreach (List<Element> r in ret.rows)
                foreach (Element e in r)
                {
                    t = e.i;
                    e.i = e.j;
                    e.j = t;
                }
            return ret;
        }
        public double[] GetDiagonalPreconditionor()
        {
            if (m != n) return null;

            double[] ret = new double[n];
            for (int i = 0; i < n; i++)
            {
                Element d = FindElement(i, i);
                if (d == null) ret[i] = 1.0;
                else if (d.value == 0.0) ret[i] = 1.0;
                else ret[i] = 1.0 / d.value;
            }
            return ret;
        }
        public SparseMatrix ConcatRows(SparseMatrix right)
        {
            if (this.ColumnSize != right.ColumnSize) throw new ArgumentException();

            SparseMatrix m = new SparseMatrix(this.RowSize + right.RowSize, this.ColumnSize);

            foreach (List<Element> r in this.rows)
                foreach (Element e in r)
                    m.AddElement(e.i, e.j, e.value);

            int r_base = this.RowSize;
            foreach (List<Element> r in right.rows)
                foreach (Element e in r)
                    m.AddElement(r_base + e.i, e.j, e.value);

            return m;
        }
        public int[][] GetRowIndex()
        {
            int[][] arr = new int[m][];

            for (int i = 0; i < m; i++)
            {
                arr[i] = new int[rows[i].Count];
                int j = 0;
                foreach (Element e in rows[i])
                    arr[i][j++] = e.j;
            }
            return arr;
        }
        public int[][] GetColumnIndex()
        {
            int[][] arr = new int[n][];

            for (int i = 0; i < n; i++)
            {
                arr[i] = new int[columns[i].Count];
                int j = 0;
                foreach (Element e in columns[i])
                    arr[i][j++] = e.i;
            }
            return arr;
        }


        public static void ConjugateGradientsMethod
            (SparseMatrix A, double[] b, double[] x, int iter, double tolerance)
        {
            int n = A.ColumnSize;
            int rn = (int)Math.Sqrt(n);
            if (A.RowSize != A.ColumnSize) throw new ArgumentException();
            if (b.Length != n || x.Length != n) throw new ArgumentException();
            double[] r = new double[n];
            double[] d = new double[n];
            double[] q = new double[n];
            double[] t = new double[n];

            int i = 0;						// i<= 0
            A.Multiply(x, r);				// r <= b - Ax
            Subtract(r, b, r);
            Assign(d, r);					// d <= r
            double newError = Dot(r, r);	// newError <= rTr
            double oError = newError;		// oError <= newError
            double oldError;
            //tolerance = tolerance * tolerance * oError;

            // While i<iMax and newError > tolerance^2*oldError do
            while ((i < iter) && (newError > tolerance))
            {
                A.Multiply(d, q);			// q <= Ad
                double alpha =				// alpha <= newError/(dTq)
                    newError / Dot(d, q);
                Scale(t, d, alpha);			// x <= x + aplha*d
                Add(x, x, t);
                if (i % rn == 0)				// If i is divisible by 50
                {
                    A.Multiply(x, r);		// r <= b - Ax
                    Subtract(r, b, r);
                }
                else
                {
                    Scale(t, q, alpha);		// r <= r - aplha * q
                    Subtract(r, r, t);
                }
                oldError = newError;		// oldError <= newError
                newError = Dot(r, r);		// newError = rTr
                if (newError < tolerance)	// prevents roundoff error
                {
                    A.Multiply(x, r);
                    Subtract(r, b, r);
                    newError = Dot(r, r);
                }
                double beta =				// beta = newError/oldError
                    newError / oldError;
                Scale(d, d, beta);			// d <= r + beta * d
                Add(d, d, r);
                i++;						// i <= i + 1

                //if (i%100 == 0)
                //	MyDebug.WriteLine(i.ToString() + ": " + newError.ToString());
                //for(int kk=0; kk<n; kk++) MyDebug.Write(" " + x[kk].ToString());
                //MyDebug.WriteLine("");
            }
            //MyDebug.WriteLine(i.ToString() + ": " + newError.ToString());
        }
        public static void ConjugateGradientsMethod2
            (SparseMatrix A, double[] b, double[] x, int iter, double tolerance)
        {
            int n = A.ColumnSize;
            int rn = (int)Math.Sqrt(n);
            if (A.RowSize != A.ColumnSize) throw new ArgumentException();
            if (b.Length != n || x.Length != n) throw new ArgumentException();
            double[] r1 = new double[n];
            double[] r2 = new double[n];
            double[] d1 = new double[n];
            double[] d2 = new double[n];
            double[] q1 = new double[n];
            double[] q2 = new double[n];
            double[] t = new double[n];

            int i = 0;						// i<= 0
            A.Multiply(x, r1);				// r1 <= b - Ax
            Subtract(r1, b, r1);
            Assign(r2, r1);					// r2 <= r1
            Assign(d1, r1);					// d1 <= r1
            Assign(d2, r2);					// d2 <= r2
            double newError = Dot(r2, r1);	// newError <= rTr
            double oError = newError;		// oError <= newError
            double oldError;
            //tolerance = tolerance * tolerance * oError;

            // While i<iMax and newError > tolerance^2*oldError do
            while ((i < iter) && (newError > tolerance))
            {
                A.Multiply(d1, q1);			// q1 <= Ad1
                A.PreMultiply(d2, q2);		// q2 <= d2A
                double alpha =				// alpha <= newError/(d2Tq1)
                    newError / Dot(d2, q1);
                Scale(t, d1, alpha);			// x <= x + aplha*d1
                Add(x, x, t);
                if (i % rn == 0)				// If i is divisible by 50
                {
                    A.Multiply(x, r1);		// r <= b - Ax
                    Subtract(r1, b, r1);
                    Assign(r2, r1);
                }
                else
                {
                    Scale(t, q1, alpha);		// r <= r - aplha * q1
                    Subtract(r1, r1, t);
                    Scale(t, q2, alpha);
                    Subtract(r2, r2, t);
                }
                oldError = newError;		// oldError <= newError
                newError = Dot(r2, r1);		// newError = rTr
                if (newError < tolerance)	// prevents roundoff error
                {
                    A.Multiply(x, r1);
                    Subtract(r1, b, r1);
                    Assign(r2, r1);
                    newError = Dot(r2, r1);
                }
                double beta =				// beta = newError/oldError
                    newError / oldError;
                Scale(d1, d1, beta);		// d1 <= r1 + beta * d1
                Add(d1, d1, r1);
                Scale(d2, d2, beta);		// d2 <= r2 + beta * d2
                Add(d2, d2, r2);
                i++;						// i <= i + 1

                //if (i%100 == 0)
                //	MyDebug.WriteLine(i.ToString() + ": " + newError.ToString());
                //for(int kk=0; kk<n; kk++) MyDebug.Write(" " + x[kk].ToString());
                //MyDebug.WriteLine("");
            }
            //MyDebug.WriteLine(i.ToString() + ": " + newError.ToString());
        }
        public static void ConjugateGradientsMethod3
            (SparseMatrix A, double[] b, double[] x, bool[] boundary, int iter, double tolerance)
        {
            int n = A.ColumnSize;
            int rn = (int)Math.Sqrt(n);
            if (A.RowSize != A.ColumnSize) throw new ArgumentException();
            if (b.Length != n || x.Length != n) throw new ArgumentException();
            double[] r = new double[n];
            double[] d = new double[n];
            double[] q = new double[n];
            double[] t = new double[n];

            int i = 0;						// i<= 0
            A.Multiply(x, r);				// r <= b - Ax
            Subtract(r, b, r);
            Assign(d, r);					// d <= r
            double newError = Dot(r, r);	// newError <= rTr
            double oError = newError;		// oError <= newError
            double oldError;
            //tolerance = tolerance * tolerance * oError;

            // While i<iMax and newError > tolerance^2*oldError do
            while ((i < iter) && (newError > tolerance))
            {
                A.Multiply(d, q);			// q <= Ad
                double alpha =				// alpha <= newError/(dTq)
                    newError / Dot(d, q);
                Scale(t, d, alpha);			// x <= x + aplha*d (if x is not boundary)
                Add(x, x, t);

                if (i % rn == 0)				// If i is divisible by 50
                {
                    A.Multiply(x, t);
                    //for (int kk=0; kk<n; kk++)
                    //	if (boundary[kk] == false)
                    //		b[kk] = t[kk];
                    Subtract(r, b, t);		// r <= b - Ax
                }
                else
                {
                    Scale(t, q, alpha);		// r <= r - aplha * q
                    Subtract(r, r, t);
                }
                oldError = newError;		// oldError <= newError
                newError = Dot(r, r);		// newError = rTr
                if (newError < tolerance)	// prevents roundoff error
                {
                    A.Multiply(x, r);
                    Subtract(r, b, r);
                    newError = Dot(r, r);
                }
                double beta =				// beta = newError/oldError
                    newError / oldError;
                Scale(d, d, beta);			// d <= r + beta * d
                Add(d, d, r);
                i++;						// i <= i + 1

                //if (i%100 == 0)
                //	MyDebug.WriteLine(i.ToString() + ": " + newError.ToString());
                //for(int kk=0; kk<n; kk++) MyDebug.Write(" " + x[kk].ToString());
                //MyDebug.WriteLine("");
            }
            //MyDebug.WriteLine(i.ToString() + ": " + newError.ToString());
        }
        public static void JacobiMethod
            (SparseMatrix A, double[] b, double[] x, int iter, double tolerance)
        {
            int n = A.ColumnSize;
            if (A.ColumnSize != A.RowSize) throw new ArgumentException();
            if (b.Length != n || x.Length != n) throw new ArgumentException();
            double[] r = new double[n];
            double[] d = new double[n];
            double[] t = new double[n];
            double error;

            for (int i = 0; i < n; i++)
            {
                Element e = A.FindElement(i, i);
                if (e != null) d[i] = (3.0 / 4.0) / e.value;
                else d[i] = 0;
            }

            A.Multiply(x, t);
            Subtract(r, b, t);
            error = Dot(r, r);
            int count = 0;
            while (count < iter && (error > tolerance))
            {
                for (int i = 0; i < n; i++)
                    t[i] = r[i] * d[i];
                Add(x, x, t);
                A.Multiply(x, t);
                Subtract(r, b, t);
                error = Dot(r, r);
                count++;
                //if (count % 100 == 0)
                //	MyDebug.WriteLine(count.ToString() + ": " + error.ToString());
            }
            //MyDebug.WriteLine(count.ToString() + ": " + error.ToString());
        }
        // u <= v;
        private static void Assign(double[] u, double[] v)
        {
            if (u.Length != v.Length) throw new ArgumentException();
            for (int i = 0; i < u.Length; i++)
                u[i] = v[i];
        }
        // w = u-v
        private static void Subtract(double[] w, double[] u, double[] v)
        {
            if (u.Length != v.Length || v.Length != w.Length)
                throw new ArgumentException();

            for (int i = 0; i < u.Length; i++)
                w[i] = u[i] - v[i];
        }
        // w = u+v
        private static void Add(double[] w, double[] u, double[] v)
        {
            if (u.Length != v.Length || v.Length != w.Length)
                throw new ArgumentException();

            for (int i = 0; i < u.Length; i++)
                w[i] = u[i] + v[i];
        }
        private static void Scale(double[] w, double[] u, double s)
        {
            if (u.Length != w.Length) throw new ArgumentException();
            for (int i = 0; i < u.Length; i++)
                w[i] = u[i] * s;
        }
        private static double Dot(double[] u, double[] v)
        {
            if (u.Length != v.Length) throw new ArgumentException();
            double sum = 0.0;
            for (int i = 0; i < u.Length; i++)
                sum += u[i] * v[i];
            return sum;
        }

        public SparseMatrix Minus(SparseMatrix right)
        {
            SparseMatrix ret = new SparseMatrix(this);

            foreach (List<Element> r in right.rows)
            {
                foreach (Element e in r)
                {
                    int i = e.i;
                    int j = e.j;

                    Element temp = FindElement(i, j);

                    double leftPartValue;
                    if (temp == null)
                    {
                        leftPartValue = 0;
                    }
                    else
                    {
                        leftPartValue = temp.value;
                    }

                    double rightPartValue = e.value;
                    double result = leftPartValue - rightPartValue;

                    Element temp2 = ret.FindElement(i, j);

                    if (temp2 == null)
                    {
                        ret.AddValueTo(i, j, result);
                    }
                    else
                    {
                        temp2.value = result;
                    }

                }
            }

            return ret;
        }

        public SparseMatrix AddT(SparseMatrix right)
        {
            SparseMatrix ret = new SparseMatrix(this);

            foreach (List<Element> r in right.rows)
            {
                foreach (Element e in r)
                {
                    int i = e.i;
                    int j = e.j;

                    Element temp = FindElement(i, j);

                    double leftPartValue;
                    if (temp == null)
                    {
                        leftPartValue = 0;
                    }
                    else
                    {
                        leftPartValue = temp.value;
                    }

                    double rightPartValue = e.value;
                    double result = leftPartValue + rightPartValue;

                    Element temp2 = ret.FindElement(i, j);

                    if (temp2 == null)
                    {
                        ret.AddValueTo(i, j, result);
                    }
                    else
                    {
                        temp2.value = result;
                    }

                }
            }

            return ret;
        }

        public void Multiply(double factor)
        {
            foreach (List<Element> row in this.rows)
            {
                foreach (Element item in row)
                {
                    item.value *= factor;
                }
            }
        }


        public void ClearRow(int i)
        {

            for (int j = 0; j < this.columns.Count; j++)
            {
                this[i, j] = 0;
            }

        }



        public static SparseMatrix Mult(SparseMatrix left, SparseMatrix right)
        {
            if (left.ColumnSize != right.RowSize) throw new ArgumentException();

            SparseMatrix ret = new SparseMatrix(left.RowSize, right.ColumnSize);

            for (int i = 0; i < left.Rows.Count; i++)
            {
                List<SparseMatrix.Element> rr = left.Rows[i] as List<SparseMatrix.Element>;

                for (int j = 0; j < right.Columns.Count; j++)
                {
                    List<SparseMatrix.Element> cc = right.Columns[j] as List<SparseMatrix.Element>;
                    int c1 = 0;
                    int c2 = 0;
                    double sum = 0;
                    bool used = false;

                    while (c1 < rr.Count && c2 < cc.Count)
                    {
                        int i1 = rr[c1].j;
                        int i2 = cc[c2].i;
                        if (i1 > i2)
                        {
                            c2++;
                        }
                        else if (i1 < i2)
                        {
                            c1++;
                        }
                        else
                        {
                            sum += rr[c1++].value * cc[c2++].value;
                            used = true;
                        }
                    }
                    if (used) ret[i, j] = sum;
                }
            }

            return ret;
        }


        //public static SparseMatrix ReadMatrixFromMatlab(String path)
        //{
        //    StreamReader sr = new StreamReader(path);
        //    String line = null;

        //    int m = int.MinValue;
        //    int n = int.MinValue;

        //    int count = 0;
        //    SparseMatrix sm = null;

        //    while ((line = sr.ReadLine()) != null)
        //    {
        //        String[] token = line.Split(' ');

        //        if (count == 0)
        //        {
        //            m = int.Parse(token[1]);
        //            n = int.Parse(token[3]);
        //            int nnz = int.Parse(token[5]);
        //            sm = new SparseMatrix(m, n);

        //            count++;
        //            continue;
        //        }

        //        int index_i = int.Parse(token[0]);
        //        int index_j = int.Parse(token[1]);
        //        double value = double.Parse(token[2]);
        //        if (value != 0)
        //        {
        //            sm.AddValueTo(index_i, index_j, value);
        //        }

        //    }

        //    sr.Close();

        //    GC.Collect();
        //    return sm;
        //}

        //public static double[] ReadVectorFromMatlab(String path)
        //{
        //    StreamReader sr = new StreamReader(path);
        //    String line = sr.ReadLine();

        //    int count = int.Parse(line.Split(' ')[2]);
        //    double[] values = new double[count];

        //    int i = 0;
        //    while ((line = sr.ReadLine()) != null)
        //    {
        //        double value = double.Parse(line);
        //        values[i] = value;
        //        i++;
        //    }

        //    sr.Close();
        //    return values;
        //}

        //public void WriteMatrixToMatlab(String path)
        //{
        //    StreamWriter sw;
        //    sw = File.CreateText(path);

        //    foreach (List<SparseMatrix.Element> col in this.Columns)
        //    {
        //        foreach (SparseMatrix.Element e in col)
        //        {
        //            if (e.value != 0)
        //            {
        //                sw.WriteLine((e.i + 1) + " " + (e.j + 1) + " " + e.value);
        //            }
        //        }
        //    }

        //    sw.Close();
        //}

        //public static void WriteVectorToMatlab(String path, double[] vector)
        //{

        //    StreamWriter sw;
        //    sw = File.CreateText(path);

        //    int count = 1;
        //    foreach (double i in vector)
        //    {
        //        sw.WriteLine(count + " " + 1 + " " + i);
        //        count++;
        //    }

        //    sw.Close();
        //}

        //public void WriteMatrixToFile(String path)
        //{
        //    StreamWriter sw;
        //    sw = File.CreateText(path);


        //    int lineCount = 0;
        //    bool symmetric = this.IsSymmetric();

        //    //Get NoneOfZeroCount
        //    foreach (List<SparseMatrix.Element> col in this.Columns)
        //    {
        //        foreach (SparseMatrix.Element e in col)
        //        {
        //            if (!symmetric && e.value != 0)
        //            {
        //                lineCount++;
        //            }

        //            if (symmetric && e.i >= e.j && e.value != 0)
        //            {
        //                lineCount++;
        //            }
        //        }
        //    }


        //    //Output matrix attributes
        //    sw.WriteLine("#Row: " + this.RowSize + " Columns: " + this.ColumnSize + " nnz: " + lineCount);

        //    if (symmetric)
        //    {
        //        sw.WriteLine("#Symmetric: true L");
        //    }
        //    else
        //    {
        //        sw.WriteLine("#Symmetric: false N");
        //    }

        //    int z = 0;
        //    foreach (List<SparseMatrix.Element> col in this.Columns)
        //    {
        //        foreach (SparseMatrix.Element e in col)
        //        {
        //            if (!symmetric && e.value != 0)
        //            {
        //                z++;
        //                if (z != lineCount)
        //                    sw.WriteLine(e.i + " " + e.j + " " + e.value);
        //                else
        //                    sw.Write(e.i + " " + e.j + " " + e.value);
        //            }

        //            if (symmetric && e.i >= e.j && e.value != 0)
        //            {
        //                z++;
        //                if (z != lineCount)
        //                    sw.WriteLine(e.i + " " + e.j + " " + e.value);
        //                else
        //                    sw.Write(e.i + " " + e.j + " " + e.value);
        //            }
        //        }
        //    }

        //    sw.Close();

        //}

        

        //public static void WriteVectorToFile(String path, double[] vector)
        //{
        //    StreamWriter sw;
        //    sw = File.CreateText(path);

        //    sw.WriteLine("# Vector is " + vector.Length);

        //    foreach (double i in vector)
        //    {
        //        sw.WriteLine(i);
        //    }

        //    sw.Close();
        //}

    }


}
