using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
	public class RowBasedMatrix
	{
		public struct Element : IComparable<Element>
		{
			public int colIndex;
			public double value;
			public Element(int c)
			{
				this.colIndex = c;
				this.value = 0;
			}
			public Element(int c, double value)
			{
				this.colIndex = c;
				this.value = value;
			}


			#region IComparable<Element> Members

			public int CompareTo(Element other)
			{
				return colIndex - other.colIndex;
			}

			#endregion
		};

		private List<Element>[] rows;

		public List<Element>[] Rows { get { return rows; } }
		public int RowSize { get { return rows.Length; } }

		public RowBasedMatrix(int rowSize)
		{
			this.rows = new List<Element>[rowSize];
			for(int i=0; i<rowSize; i++)
				rows[i] = new List<Element>();
		}
		public RowBasedMatrix(int rowSize, int capacity)
		{
			this.rows = new List<Element>[rowSize];
			for (int i = 0; i < rowSize; i++)
				rows[i] = new List<Element>(capacity);
		}
		public RowBasedMatrix(SparseMatrix M) : this(M.RowSize)
		{
			foreach(List<SparseMatrix.Element> row in M.Rows)
				foreach(SparseMatrix.Element e in row)
					AddElement(e.i, e.j, e.value);
		}
		public void AddElement(int r, int c, double value)
		{
			rows[r].Add(new Element(c, value));
		}
		public int FindElement(int r, int c)
		{
			for(int i=0; i<rows[r].Count; i++)
				if (rows[r][i].colIndex == c) return i;
			return -1;
		}
		public int BinarySearchElement(int r, int c)
		{
			return rows[r].BinarySearch(new Element(c));
		}
		public void Sort()
		{
			foreach(List<Element> row in rows) row.Sort();
		}
		public void Multiply(double[] xIn, double[] xOut)
		{
			for (int i = 0; i < RowSize; i++)
			{
				double sum = 0;
				foreach (Element e in rows[i])
					sum += e.value * xIn[e.colIndex];
				xOut[i] = sum;
			}
		}
		public void PreMultiply(double[] xIn, double[] xOut)
		{
			for (int i=0; i<xOut.Length; i++) xOut[i] = 0;

			for (int i=0; i<RowSize; i++)
			{
				foreach (Element e in rows[i])
					xOut[e.colIndex] += e.value * xIn[i];
			}
		}


		public RowBasedMatrix IncompleteCholesky()
		{
			// suppose this matrix is symmetric, square and sorted

			int n = this.RowSize;
			RowBasedMatrix L = new RowBasedMatrix(n);


			for (int i=0; i<n; i++)
			{
				List<Element> r = rows[i];
				List<Element> rr1 = L.rows[i];
				for (int j=0; j<r.Count; j++)
				{
					Element e = r[j];

					if (e.colIndex == i)
					{
						double sum = e.value;
						for (int c=0; c<rr1.Count && rr1[c].colIndex<i; c++)
							sum -= rr1[c].value * rr1[c].value;
						if (sum > 0.0) 
							rr1.Add(new Element(i, Math.Sqrt(sum)));
						else
							rr1.Add(new Element(i, 10e-12));
					}

					else if (e.colIndex < i)
					{
						List<Element> rr2 = L.rows[e.colIndex];
						double sum = e.value;
						int c1=0, c2=0;
						while (c1<rr1.Count && c2<rr2.Count)
						{
							Element e1 = rr1[c1];
							Element e2 = rr2[c2];
							if (e1.colIndex >= e.colIndex) break;
							if (e2.colIndex >= e.colIndex) break;

							if (e1.colIndex > e2.colIndex) c2++;
							else if (e1.colIndex < e2.colIndex) c1++;
							else 
							{
								sum -= e1.value * e2.value;
								c1++;
								c2++;
							}
						}
						while (rr2[c2].colIndex < e.colIndex ) c2++;
						rr1.Add(new Element(e.colIndex, sum/rr2[c2].value));
					}
				}
			}

			return L;
		}
		public RowBasedMatrix IncompleteCholesky(double droptol)
		{
			// suppose this matrix is symmetric, square and sorted

			int n = this.RowSize;
			int nAdd = 0;
			RowBasedMatrix L = new RowBasedMatrix(n);


			for (int i = 0; i < n; i++)
			{
				List<Element> r = rows[i];
				List<Element> rr1 = L.rows[i];
				double drop = 0;
 				double norm = 0;
 				foreach(Element e in r) norm += e.value * e.value;
 				norm  = Math.Sqrt(norm) * droptol;

				int k = 0;
				for (int j=0; j<i; j++)
				{
					double sum = 0;
					if (k < r.Count && r[k].colIndex == j) sum = r[k++].value;

					int c1 = 0, c2 = 0;
					List<Element> rr2 = L.rows[j];
					while (c1 < rr1.Count && c2 < rr2.Count)
					{
						Element e1 = rr1[c1];
						Element e2 = rr2[c2];
						if (e1.colIndex >= j) break;
						if (e2.colIndex >= j) break;

						if (e1.colIndex > e2.colIndex) c2++;
						else if (e1.colIndex < e2.colIndex) c1++;
						else
						{
							sum -= e1.value * e2.value;
							c1++;
							c2++;
						}
					}
					while (rr2[c2].colIndex < j) c2++;
					sum /= rr2[c2].value;
					if (Math.Abs(sum) > norm)
						{ rr1.Add(new Element(j, sum)); nAdd++; }
					else
						{ drop += sum; }
				}

				if (r[k].colIndex != i) throw new Exception();

				double diag = r[k].value;// + drop;
				for (int c = 0; c < rr1.Count && rr1[c].colIndex < i; c++)
					diag -= rr1[c].value * rr1[c].value;
				if (diag > 0.0)
					rr1.Add(new Element(i, Math.Sqrt(diag)));
				else
					throw new Exception();
			}
			OutputText("drops:" + nAdd.ToString());

			return L;
		}
		public void Cholesky_Solve(double[] x, double[] b)
		{
			int n = RowSize;
			double[] t = new double[n];

			for (int i=0; i<n; i++)
			{
				List<Element> r = rows[i];
				double sum = b[i];
				int j;
				for (j=0; j<r.Count && r[j].colIndex<i; j++)
					sum -= r[j].value * x[r[j].colIndex];
				x[i] = sum / r[j].value;
			}

			for (int i=n-1; i>=0; i--)
			{
				List<Element> r = rows[i];

				x[i] /= r[r.Count-1].value;
				for (int j=0; j<r.Count && r[j].colIndex<i; j++)
					x[r[j].colIndex] -= r[j].value * x[i];
			}
		}
		public void CG_Solve(double[] x, double[] b, int maxIter, double eps, int recompute)
		{
			int n = RowSize;
			int iter = 0;
			double[] r = new double[n];
			double[] d = new double[n];
			double[] q = new double[n];
			double errNew=0, errold=0;

			for(int i=0; i<n; i++)
			{
				double Ax = 0;
				foreach (Element e in rows[i]) 
					Ax += e.value * x[e.colIndex];
				d[i] = r[i] = b[i] - Ax;
				errNew += r[i] * r[i];
			}
			double tolerance = eps * errNew;

			OutputText("0:" + errNew.ToString());

			while (iter<maxIter && errNew>tolerance)
			{
				iter++;

				double dAd = 0;
				for (int i=0; i<n; i++)
				{
					double Ad = 0;
					foreach (Element e in rows[i])
						Ad += e.value * d[e.colIndex];
					q[i] = Ad;
					dAd += d[i] * Ad;
				}
				double alpha = errNew / dAd;

				for (int i=0; i<n; i++) 
					x[i] += alpha * d[i];

				if (iter%recompute == 0)
				{
					for (int i=0; i<n; i++)
					{
						double Ax = 0;
						foreach (Element e in rows[i])
							Ax += e.value * x[e.colIndex];
						r[i] = b[i] - Ax;
					}
					 OutputText(iter + ":" + errNew.ToString());
				}
				else
				{
					for (int i=0; i<n; i++)
						r[i] -= alpha * q[i];
				}

				errold = errNew;
				errNew = 0;
				for (int i=0; i<n; i++) errNew += r[i] * r[i];

				double beta = errNew / errold;

				for (int i=0; i<n; i++)
					d[i] = r[i] + beta * d[i];
			}
			 OutputText(iter + ":" + errNew.ToString());
		}
		public void CG_Solve(double[] x, double[] b, int maxIter, double eps, int recompute, double[] inv)
		{
			int n = RowSize;
			int iter = 0;
			double[] r = new double[n];
			double[] d = new double[n];
			double[] q = new double[n];
			double[] s = new double[n];
			double errNew = 0, errold = 0;

			for (int i = 0; i < n; i++)
			{
				double Ax = 0;
				foreach (Element e in rows[i])
					Ax += e.value * x[e.colIndex];
				r[i] = b[i] - Ax;
				d[i] = inv[i] * r[i];
				errNew += r[i] * d[i];
			}
			double tolerance = eps * errNew;

			OutputText("0:" + errNew.ToString());

			while (iter < maxIter && errNew > tolerance)
			{
				iter++;

				double dAd = 0;
				for (int i = 0; i < n; i++)
				{
					double Ad = 0;
					foreach (Element e in rows[i])
						Ad += e.value * d[e.colIndex];
					q[i] = Ad;
					dAd += d[i] * Ad;
				}
				double alpha = errNew / dAd;

				for (int i = 0; i < n; i++)
					x[i] += alpha * d[i];

				if (iter % recompute == 0)
				{
					for (int i = 0; i < n; i++)
					{
						double Ax = 0;
						foreach (Element e in rows[i])
							Ax += e.value * x[e.colIndex];
						r[i] = b[i] - Ax;
					}
					 OutputText(iter + ":" + errNew.ToString());
				}
				else
				{
					for (int i = 0; i < n; i++)
						r[i] -= alpha * q[i];
				}

				for (int i = 0; i < n; i++) s[i] = inv[i] * r[i];
				errold = errNew;
				errNew = 0;
				for (int i = 0; i < n; i++) errNew += r[i] * s[i];

				double beta = errNew / errold;

				for (int i = 0; i < n; i++)
					d[i] = s[i] + beta * d[i];
			}
			 OutputText(iter + ":" + errNew.ToString());
		}
		public void CG_SolveATA(double[] x, double[] b, int maxIter, double eps, int recompute, double[] inv)
		{
			int n = x.Length;
			int iter = 0;
			double[] r = new double[n];
			double[] d = new double[n];
			double[] q = new double[n];
			double[] s = new double[n];
			double[] t = new double[RowSize];
			double errNew = 0, errold = 0;


// 			Multiply(x, t);
// 			PreMultiply(t, r);

			for (int i=0; i<n; i++) r[i] = b[i];
			for (int i=0; i<RowSize; i++)
			{
				double sum = 0;
				foreach (Element e in rows[i])
					sum += e.value * x[e.colIndex];
				foreach (Element e in rows[i])
					r[e.colIndex] -= e.value * sum;
			}
			for (int i = 0; i < n; i++)
			{
// 				r[i] = b[i] - r[i];
				d[i] = inv[i] * r[i];
				errNew += r[i] * d[i];
			}
			double tolerance = eps * errNew;

		    OutputText("0:" + errNew.ToString());

			while (iter < maxIter && errNew > tolerance)
			{
				iter++;

				double dAd = 0;

				for (int i=0; i<n; i++) q[i] = 0;
				for (int i=0; i<RowSize; i++)
				{
					double sum = 0;
					foreach (Element e in rows[i])
						sum += e.value * d[e.colIndex];
					foreach (Element e in rows[i])
						q[e.colIndex] += e.value * sum;
				}
				for (int i = 0; i < n; i++)
					dAd += d[i] * q[i];

				double alpha = errNew / dAd;

				for (int i = 0; i < n; i++)
					x[i] += alpha * d[i];

				if (iter % recompute == 0)
				{
					for (int i=0; i<n; i++) r[i] = b[i];
					for (int i=0; i<RowSize; i++)
					{
						double sum = 0;
						foreach (Element e in rows[i])
							sum += e.value * x[e.colIndex];
						foreach (Element e in rows[i])
							r[e.colIndex] -= e.value * sum;
					}
					OutputText(iter + ":" + errNew.ToString());
				}
				else
				{
					for (int i = 0; i < n; i++)
						r[i] -= alpha * q[i];
				}

				for (int i = 0; i < n; i++) s[i] = inv[i] * r[i];
				errold = errNew;
				errNew = 0;
				for (int i = 0; i < n; i++) errNew += r[i] * s[i];

				double beta = errNew / errold;

				for (int i = 0; i < n; i++)
					d[i] = s[i] + beta * d[i];
			}

			double err = 0;
			for (int i = 0; i < n; i++) err += r[i] * r[i];
			OutputText(iter + ":" + err.ToString());
		}
		public void CG_SolveATA(double[] x, double[] b, int maxIter, double eps, int recompute, RowBasedMatrix inv)
		{
			int n = x.Length;
			int iter = 0;
			double[] r = new double[n];
			double[] d = new double[n];
			double[] q = new double[n];
			double[] s = new double[n];
			double[] t = new double[n];
			double errNew = 0, errold = 0;

			for (int i=0; i<n; i++) r[i] = b[i];
			for (int i=0; i<RowSize; i++)
			{
				double sum = 0;
				foreach (Element e in rows[i])
					sum += e.value * x[e.colIndex];
				foreach (Element e in rows[i])
					r[e.colIndex] -= e.value * sum;
			}
			inv.Cholesky_Solve(d, r);
			for (int i = 0; i < n; i++)
			{
				errNew += r[i] * d[i];
			}
			double tolerance = eps * errNew;

			OutputText("0:" + errNew.ToString());

			while (iter < maxIter && errNew > tolerance)
			{
				iter++;

				double dAd = 0;

				for (int i=0; i<n; i++) q[i] = 0;
				for (int i=0; i<RowSize; i++)
				{
					double sum = 0;
					foreach (Element e in rows[i])
						sum += e.value * d[e.colIndex];
					foreach (Element e in rows[i])
						q[e.colIndex] += e.value * sum;
				}
				for (int i = 0; i < n; i++)
					dAd += d[i] * q[i];

				double alpha = errNew / dAd;

				for (int i = 0; i < n; i++)
					x[i] += alpha * d[i];

				if (iter % recompute == 0)
				{
					for (int i=0; i<n; i++) r[i] = b[i];
					for (int i=0; i<RowSize; i++)
					{
						double sum = 0;
						foreach (Element e in rows[i])
							sum += e.value * x[e.colIndex];
						foreach (Element e in rows[i])
							r[e.colIndex] -= e.value * sum;
					}
					OutputText(iter + ":" + errNew.ToString());
				}
				else
				{
					for (int i = 0; i < n; i++)
						r[i] -= alpha * q[i];
				}

				inv.Cholesky_Solve(s, r);
				errold = errNew;
				errNew = 0;
				for (int i = 0; i < n; i++) errNew += r[i] * s[i];

				double beta = errNew / errold;

				for (int i = 0; i < n; i++)
					d[i] = s[i] + beta * d[i];
			}

			double err=0;
			for (int i=0; i<n; i++) err += r[i] * r[i];
		    OutputText(iter + ":" + err.ToString());
		}


        public void OutputText(string text)
        {

        }
	}
}
