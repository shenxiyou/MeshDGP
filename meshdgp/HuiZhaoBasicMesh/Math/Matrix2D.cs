using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public struct Matrix2D
    {
        private const int len = 4;
        private const int row_size = 2;
        double a, b, c, d;

        public Matrix2D(double[] arr)
        {
            a = arr[0];
            b = arr[1];
            c = arr[2];
            d = arr[3];
        }
        public Matrix2D(double[,] arr)
        {
            a = arr[0, 0];
            b = arr[0, 1];
            c = arr[1, 0];
            d = arr[1, 1];
        }

        //public static Vector2D operator *(Matrix2D matrix, Vector2D vector)
        //{
        //    Vector2D result = new Vector2D();

        //    result.x = matrix[0, 0] * vector.x + matrix[0, 1] * vector.y;
        //    result.y = matrix[1, 0] * vector.x + matrix[1, 1] * vector.y;

        //    return result;
        //}

        public double Det()
        {
            return a * d - c * b;
        }

        public double this[int m,int n]
        {
            get {
                if (m == 0)
                {
                    if (n == 0)
                    {
                        return a;
                    }
                    else if(n == 1)
                    {
                        return b;
                    }
                }
                else
                {
                    if (n == 0)
                    {
                        return c;
                    }
                    else if (n == 1)
                    {
                        return d;
                    }
                   
                }

                throw new Exception("Out of index");
            }
            set {
                if (m == 0)
                {
                    if (n == 0)
                    {
                        a = value;
                    }
                    else if (n == 1)
                    {
                        b = value;
                    }
                    else
                    {
                        throw new Exception("Out of index");   
                    }
                }
                else if (m == 1)
                {
                    if (n == 0)
                    {
                        c = value;
                    }
                    else if (n == 1)
                    {
                        d = value;
                    }
                    else
                    {
                        throw new Exception("Out of index");   
                    }

                }
                else
                {
                    throw new Exception("Out of index");   
                }

                      
            }
        }

        // using column vectors
        public Matrix2D(Vector2D v1, Vector2D v2)
        {
            a = v1.x;
            b = v2.x;
            c = v1.y;
            d = v2.y;
        }

        public Matrix2D(double a, double b, double c, double d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }


        public static Matrix2D operator *(Matrix2D m1, Matrix2D m2)
        {
            Matrix2D ret = new Matrix2D(
                m1.a * m2.a + m1.b * m2.c,
                m1.a * m2.b + m1.b * m2.d,
                m1.c * m2.a + m1.d * m2.c,
                m1.c * m2.b + m1.d * m2.d
                );
            return ret;
        }
        public static Vector2D operator *(Matrix2D m, Vector2D v)
        {
            return new Vector2D(m.A * v.x + m.B * v.y, m.C * v.x + m.D * v.y);
        }
        public Matrix2D Inverse()
        {
            double det = (a * d - b * c);
            if (double.IsNaN(det)) throw new ArithmeticException();
            return new Matrix2D(d / det, -b / det, -c / det, a / det);
        }
        public Matrix2D Transpose()
        {
            return new Matrix2D(a, c, b, d);
        }
        public double Trace()
        {
            return a + d;
        }

        public double A
        {
            get { return a; }
            set { a = value; }
        }
        public double B
        {
            get { return b; }
            set { b = value; }
        }
        public double C
        {
            get { return c; }
            set { c = value; }
        }
        public double D
        {
            get { return d; }
            set { d = value; }
        }
        public override string ToString()
        {
            return
                a.ToString("F5") + " " + b.ToString("F5") + " " +
                c.ToString("F5") + " " + d.ToString("F5");
        }
    }	
}
