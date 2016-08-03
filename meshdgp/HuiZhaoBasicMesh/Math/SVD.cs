using System;

namespace GraphicResearchHuiZhao
{
	public class SVD
	{
		private int m, n;
		private bool state, fullRank;
		private double[]  w;
		private double[,] u, v;
		private double[,] inverse;
        private double[,] rotation;

		public int RowSize { get { return m; } }
		public int ColumnSize { get { return n; } }
		public bool State { get { return state; } }
		public bool FullRank { get { return fullRank; } }

		public double[,] Inverse
		{
			get
			{
				if (state == true && m==n && inverse == null) 
				{
					inverse = new double[n,n];
					for (int i=1; i<=n; i++)
						for (int j=1; j<=n; j++) {
							inverse[i-1,j-1] = 0.0;
							for (int k=1; k<=n; k++)
								if (w[k] != 0)
									inverse[i-1,j-1] += v[i,k] * u[j,k] * (1.0/w[k]);
						}
				}
				return inverse;
			}
		}

        public double[,] RotationMinus
        {
            get
            {
                if (state == true && m == n && inverse == null)
                {

                    int minus = -1;
                    double max = double.MaxValue;
                    for (int i = 1; i <= n; i++)
                    {
                        if (max > w[i])
                        {
                            max = w[i];
                            minus = i;
                        }
                    }

                    for (int i = 1; i <= n; i++)
                    {
                        u[i,minus] = -u[i,minus];
                    }


                    rotation = new double[n, n];
                    for (int i = 1; i <= n; i++)
                        for (int j = 1; j <= n; j++)
                        {
                            rotation[i - 1, j - 1] = 0.0;
                            for (int k = 1; k <= n; k++)
                                if (w[k] != 0)
                                    rotation[i - 1, j - 1] += v[i, k] * u[j, k];
                        }
                }
                return rotation;
            }
        }

        public double[,] Rotation
        {
            get
            {
                if (state == true && m == n && inverse == null)
                {
                    rotation = new double[n, n];
                    for (int i = 1; i <= n; i++)
                        for (int j = 1; j <= n; j++)
                        {
                            rotation[i - 1, j - 1] = 0.0;
                            for (int k = 1; k <= n; k++)
                                if (w[k] != 0)
                                    rotation[i - 1, j - 1] += v[i, k] * u[j, k];
                        }
                }
                return rotation;
            }
        }


		public SVD(double[,] A) 
		{
			int i, j;
			m = A.GetLength(0);
			n = A.GetLength(1);
			u = dmatrix(1,m,1,n);
			w = dvector(1,n);
			v = dmatrix(1,n,1,n);
			fullRank = true;

			for(i=0; i<m; i++)
				for (j=0; j<n; j++)
					u[i+1,j+1] = A[i,j];

			state = dsvdcmp(u, m, n, w, v);

			double max = 0.0;
			for (i=1; i<=n; i++) if (w[i]>max) max = w[i];

			double min = max * 1.0e-6;	// can be 1.0e-12 here
			for (i=1; i<=n; i++) 
				if (w[i]<min) 
				{
					w[i] = 0;
					fullRank = false;
				}
		}
		public SVD(double[] A, int m, int n)
		{
			int i, j;
			this.m = m;
			this.n = n;
			if (A.GetLength(0) < (m * n)) throw new ArgumentException();
			u = dmatrix(1,m,1,n);
			w = dvector(1,n);
			v = dmatrix(1,n,1,n);
			fullRank = true;

			for (i=0; i<m; i++)
				for (j=0; j<n; j++)
					u[i+1,j+1] = A[i*n + j];

			state = dsvdcmp(u, m, n, w, v);

			double max = 0.0;
			for (i=1; i<=n; i++) if (w[i]>max) max = w[i];

			double min = max * 1.0e-6;	// can be 1.0e-12 here
			for (i=1; i<=n; i++) 
				if (w[i]<min) 
				{
					w[i] = 0;
					fullRank = false;
				}
		}
		public bool SolveX(double[] B, double[] X) 
		{
			if (!state) return false;
			if (B.GetLength(0) < m) return false;
			if (X.GetLength(0) < n) return false;
			int i;
			double[] b2 = dvector(1,m);
			double[] x2 = dvector(1,n);
			for(i=0; i<m;i++) b2[i+1] = B[i];
			for(i=0; i<n;i++) x2[i+1] = X[i];
			dsvbksb(u, w, v, m, n, b2, x2);
			for(i=0; i<n;i++) X[i] = x2[i+1];
			return true;
		}

        public Matrix2D S
        {
            get {
                Matrix2D matrix = new Matrix2D();

                matrix[0, 0] = u[1, 2];
                matrix[0, 1] = u[1, 1];
                matrix[1, 0] = u[2, 2];
                matrix[1, 1] = u[2, 1];

                return matrix;
            }
        }

        public Matrix2D D
        {
            get
            {
                Matrix2D matrix = new Matrix2D();

                matrix[0, 0] = v[1, 2];
                matrix[0, 1] = v[1, 1];
                matrix[1, 0] = v[2, 2];
                matrix[1, 1] = v[2, 1];

                return matrix;
            }
        }

        public Matrix2D V
        {
            get
            {
                Matrix2D matrix = new Matrix2D();

                matrix[0, 0] = w[2];
                matrix[1, 1] = w[1];

                return matrix;
            }
        }

        //public Matrix3D S
        //{
        //    get {
        //        Matrix3D matrix = new Matrix3D();

        //        for (int i = 1; i < 4; i++)
        //        {
        //            for (int j = 1; j < 4; j++)
        //            {
        //                if (j == 2)
        //                {
        //                    matrix[i-1, j-1] = -u[i, j];
        //                }
        //                else {
        //                    matrix[i-1, j-1] = u[i, j];
        //                }
        //            }
        //        }

        //        return matrix;
        //    }
        //}

        //public Matrix3D V
        //{
        //    get
        //    {
        //        Matrix3D matrix = new Matrix3D();

        //        for (int i = 1; i < 4; i++)
        //        {
        //            matrix[i - 1, i - 1] = w[i];
        //        }

        //        return matrix;
        //    }
        //}


        //public Matrix3D D
        //{
        //    get
        //    {
        //        Matrix3D matrix = new Matrix3D();

        //        for (int i = 1; i < 4; i++)
        //        {
        //            for (int j = 1; j < 4; j++)
        //            {
        //                if (j == 2)
        //                {
        //                    matrix[i - 1, j - 1] = -v[i, j];
        //                }
        //                else
        //                {
        //                    matrix[i - 1, j - 1] = v[i, j];
        //                }
        //            }
        //        }

        //        return matrix;
        //    }
        //}


		static public bool Solve(double[,] a, double[] b, double[] x) 
		{
			int i, j;
			int m = a.GetLength(0);
			int n = a.GetLength(1);
			double[,] u = dmatrix(1,m,1,n);
			double[] w = dvector(1,n);
			double[,] v = dmatrix(1,n,1,n);
			double[] b2 = dvector(1,m);
			double[] x2 = dvector(1,n);

			for(i=0; i<m; i++)
				for (j=0; j<n; j++)
					u[i+1,j+1] = a[i,j];

			for(i=0; i<m;i++) b2[i+1] = b[i];
			for(i=0; i<n;i++) x2[i+1] = x[i];

			bool ok = dsvdcmp(u, m, n, w, v);
			if (ok) 
			{
				double max = 0.0;
				for (i=1; i<=n; i++)
					if (w[i]>max) max = w[i];

				double min = max * 1.0e-12;	// can be 1.0e-12 here
				for (i=1; i<=n; i++)
					if (w[i]<min) w[i] = 0;
		
				//dsvbksb(u, w, v, m, n, b-1, x-1);
				dsvbksb(u, w, v, m, n, b2, x2);
			}
			for(i=0; i<n;i++) x[i] = x2[i+1];

			return ok;
		}
		static private double dpythag(double a, double b)
		{
			double absa, absb, tmp;
			absa = Math.Abs(a);
			absb = Math.Abs(b);
			if (absa > absb) 
			{
				tmp = absb / absa;
				return absa * Math.Sqrt(1.0 + (tmp * tmp));
			}
			else 
			{
				tmp = absa/absb;
				return (absb == 0.0 ? 0.0 : absb * Math.Sqrt(1.0 + (tmp * tmp)));
			}
		}
		static private void dsvbksb(double[,] u, double[] w, double[,] v, int m, int n, double[] b, double[] x)
		{
			int jj,j,i;
			double s;
			double[] tmp;

			tmp=dvector(1,n);
			for (j=1;j<=n;j++) 
			{
				s=0.0;
				if (w[j] != 0.0) 
				{
					for (i=1;i<=m;i++) s += u[i,j]*b[i];
					s /= w[j];
				}
				tmp[j]=s;
			}
			for (j=1;j<=n;j++) 
			{
				s=0.0;
				for (jj=1;jj<=n;jj++) s += v[j,jj]*tmp[jj];
				x[j]=s;
			}
		}
		static private bool dsvdcmp(double[,] a, int m, int n, double[] w, double[,] v)
		{
			int flag, i, its, j, jj, k, l, nm;
			double anorm, c, f, g, h, s, scale, x, y, z;
			double[] rv1;

			l = nm = 0;
			rv1 = dvector(1,n);
			g = scale = anorm = 0.0;
			for (i=1;i<=n;i++) 
			{
				l=i+1;
				rv1[i]=scale*g;
				g=s=scale=0.0;
				if (i <= m) 
				{
					for (k=i;k<=m;k++) scale += Math.Abs(a[k,i]);
					if (scale != 0.0) 
					{
						for (k=i;k<=m;k++) 
						{
							a[k,i] /= scale;
							s += a[k,i]*a[k,i];
						}
						f=a[i,i];
						g = -Sign(Math.Sqrt(s),f);
						h=f*g-s;
						a[i,i]=f-g;
						for (j=l;j<=n;j++) 
						{
							for (s=0.0,k=i;k<=m;k++) s += a[k,i]*a[k,j];
							f=s/h;
							for (k=i;k<=m;k++) a[k,j] += f*a[k,i];
						}
						for (k=i;k<=m;k++) a[k,i] *= scale;
					}
				}
				w[i]=scale *g;
				g=s=scale=0.0;
				if (i <= m && i != n) 
				{
					for (k=l;k<=n;k++) scale += Math.Abs(a[i,k]);
					if (scale != 0.0) 
					{
						for (k=l;k<=n;k++) 
						{
							a[i,k] /= scale;
							s += a[i,k]*a[i,k];
						}
						f=a[i,l];
						g = -Sign(Math.Sqrt(s),f);
						h=f*g-s;
						a[i,l]=f-g;
						for (k=l;k<=n;k++) rv1[k]=a[i,k]/h;
						for (j=l;j<=m;j++) 
						{
							for (s=0.0,k=l;k<=n;k++) s += a[j,k]*a[i,k];
							for (k=l;k<=n;k++) a[j,k] += s*rv1[k];
						}
						for (k=l;k<=n;k++) a[i,k] *= scale;
					}
				}
				anorm=Math.Max(anorm,(Math.Abs(w[i])+Math.Abs(rv1[i])));
			}
			for (i=n;i>=1;i--) 
			{
				if (i < n) 
				{
					if (g != 0.0) 
					{
						for (j=l;j<=n;j++) v[j,i]=(a[i,j]/a[i,l])/g;
						for (j=l;j<=n;j++) 
						{
							for (s=0.0,k=l;k<=n;k++) s += a[i,k]*v[k,j];
							for (k=l;k<=n;k++) v[k,j] += s*v[k,i];
						}
					}
					for (j=l;j<=n;j++) v[i,j]=v[j,i]=0.0;
				}
				v[i,i]=1.0;
				g=rv1[i];
				l=i;
			}
			for (i=Math.Min(m,n);i>=1;i--) 
			{
				l=i+1;
				g=w[i];
				for (j=l;j<=n;j++) a[i,j]=0.0;
				if (g != 0.0) 
				{
					g=1.0/g;
					for (j=l;j<=n;j++) 
					{
						for (s=0.0,k=l;k<=m;k++) s += a[k,i]*a[k,j];
						f=(s/a[i,i])*g;
						for (k=i;k<=m;k++) a[k,j] += f*a[k,i];
					}
					for (j=i;j<=m;j++) a[j,i] *= g;
				} 
				else for (j=i;j<=m;j++) a[j,i]=0.0;
				++a[i,i];
			}
			for (k=n;k>=1;k--) 
			{
				for (its=1;its<=30;its++) 
				{
					flag=1;
					for (l=k;l>=1;l--) 
					{
						nm=l-1;
						if ((double)(Math.Abs(rv1[l])+anorm) == anorm) 
						{
							flag=0;
							break;
						}
						if ((double)(Math.Abs(w[nm])+anorm) == anorm) break;
					}
					if (flag != 0) 
					{
						c=0.0;
						s=1.0;
						for (i=l;i<=k;i++) 
						{
							f=s*rv1[i];
							rv1[i]=c*rv1[i];
							if ((double)(Math.Abs(f)+anorm) == anorm) break;
							g=w[i];
							h=dpythag(f,g);
							w[i]=h;
							h=1.0/h;
							c=g*h;
							s = -f*h;
							for (j=1;j<=m;j++) 
							{
								y=a[j,nm];
								z=a[j,i];
								a[j,nm]=y*c+z*s;
								a[j,i]=z*c-y*s;
							}
						}
					}
					z=w[k];
					if (l == k) 
					{
						if (z < 0.0) 
						{
							w[k] = -z;
							for (j=1;j<=n;j++) v[j,k] = -v[j,k];
						}
						break;
					}
					if (its == 30) return false; //nrerror("no convergence in 30 dsvdcmp iterations");
					x=w[l];
					nm=k-1;
					y=w[nm];
					g=rv1[nm];
					h=rv1[k];
					f=((y-z)*(y+z)+(g-h)*(g+h))/(2.0*h*y);
					g=dpythag(f,1.0);
					f=((x-z)*(x+z)+h*((y/(f+Sign(g,f)))-h))/x;
					c=s=1.0;
					for (j=l;j<=nm;j++) 
					{
						i=j+1;
						g=rv1[i];
						y=w[i];
						h=s*g;
						g=c*g;
						z=dpythag(f,h);
						rv1[j]=z;
						c=f/z;
						s=h/z;
						f=x*c+g*s;
						g = g*c-x*s;
						h=y*s;
						y *= c;
						for (jj=1;jj<=n;jj++) 
						{
							x=v[jj,j];
							z=v[jj,i];
							v[jj,j]=x*c+z*s;
							v[jj,i]=z*c-x*s;
						}
						z=dpythag(f,h);
						w[j]=z;
						if (z != 0.0) 
						{
							z=1.0/z;
							c=f*z;
							s=h*z;
						}
						f=c*g+s*y;
						x=c*y-s*g;
						for (jj=1;jj<=m;jj++) 
						{
							y=a[jj,j];
							z=a[jj,i];
							a[jj,j]=y*c+z*s;
							a[jj,i]=z*c-y*s;
						}
					}
					rv1[l]=0.0;
					rv1[k]=f;
					w[k]=x;
				}
			}
			return true;
		}
		static private double[] dvector(int nl, int nh)
		{
			return new double[nh+1];
		}
		static private double[,] dmatrix(int nrl, int nrh, int ncl, int nch) 
		{
			return new double[nrh+1,nch+1];
		}
		static private double Sign(double a, double b) 
		{
			return (b >= 0.0) ? Math.Abs(a) : -Math.Abs(a);
		}
	}
}
