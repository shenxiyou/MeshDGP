using System;
using System.Collections.Generic;
 
namespace GraphicResearchHuiZhao
{
    public class Matrix3D
    {
        public static bool lastSVDIsFullRank = false;
        private const int len = 9;
        private const int row_size = 3;
        private double[] e = new double[len];

        public Matrix3D() { }
        public Matrix3D(double[] arr)
        {
            for (int i = 0; i < len; i++) e[i] = arr[i];
        }
        public Matrix3D(double[,] arr)
        {
            for (int i = 0; i < row_size; i++)
                for (int j = 0; j < row_size; j++)
                    this[i, j] = arr[i, j];
        }
        public Matrix3D(Matrix3D m) : this(m.e) { }
        public Matrix3D(Vector3D v1, Vector3D v2, Vector3D v3)
        {
            for (int i = 0; i < row_size; i++)
            {
                this[i, 0] = v1[i];
                this[i, 1] = v2[i];
                this[i, 2] = v3[i];
            }
        }

        public void Clear()
        {
            for (int i = 0; i < len; i++) e[i] = 0;
        }
        public double this[int index]
        {
            get { return e[index]; }
            set { e[index] = value; }
        }
        public double this[int row, int column]
        {
            get { return e[row * row_size + column]; }
            set { e[row * row_size + column] = value; }
        }
        public double[] ToArray()
        {
            return (double[])e.Clone();
        }
        public double Trace()
        {
            return e[0] + e[4] + e[8];
        }
        public double SqNorm()
        {
            double sq = 0;
            for (int i = 0; i < len; i++) sq += e[i] * e[i];
            return sq;
        }
        public Matrix3D Transpose()
        {
            Matrix3D m = new Matrix3D();
            for (int i = 0; i < row_size; i++)
                for (int j = 0; j < row_size; j++)
                    m[j, i] = this[i, j];
            return m;
        }
        public Matrix3D Inverse()
        {
            double a = e[0];
            double b = e[1];
            double c = e[2];
            double d = e[3];
            double E = e[4];
            double f = e[5];
            double g = e[6];
            double h = e[7];
            double i = e[8];
            double det = a * (E * i - f * h) - b * (d * i - f * g) + c * (d * h - E * g);
            if (det == 0) throw new ArithmeticException();

            Matrix3D inv = new Matrix3D();
            inv[0] = (E * i - f * h) / det;
            inv[1] = (c * h - b * i) / det;
            inv[2] = (b * f - c * E) / det;
            inv[3] = (f * g - d * i) / det;
            inv[4] = (a * i - c * g) / det;
            inv[5] = (c * d - a * f) / det;
            inv[6] = (d * h - E * g) / det;
            inv[7] = (b * g - a * h) / det;
            inv[8] = (a * E - b * d) / det;
            return inv;
        }
        public Matrix3D InverseSVD()
        {
            SVD svd = new SVD(e, 3, 3);
            Matrix3D inv = new Matrix3D(svd.Inverse);
            lastSVDIsFullRank = svd.FullRank;
            return inv;
        }

        public double Det()
        {
            return e[0] * e[4] * e[8] + e[1] * e[5] * e[6] + e[2] * e[3] * e[7] - e[0] * e[5] * e[7] - e[1] * e[3] * e[8] - e[2] * e[4] * e[6];
        }

        public Matrix3D  SVDRotation()
        {
            SVD svd = new SVD(e, 3, 3);
            Matrix3D rot = new Matrix3D(svd.Rotation);
             
            lastSVDIsFullRank = svd.FullRank;
            return rot;
        }

        public Matrix3D SVDRotationMinus()
        {
            SVD svd = new SVD(e, 3, 3);
            Matrix3D rot = new Matrix3D(svd.RotationMinus);

            lastSVDIsFullRank = svd.FullRank;
            return rot;
        }

        public Matrix3D InverseTranspose()
        {
            double a = e[0];
            double b = e[1];
            double c = e[2];
            double d = e[3];
            double E = e[4];
            double f = e[5];
            double g = e[6];
            double h = e[7];
            double i = e[8];
            double det = a * (E * i - f * h) - b * (d * i - f * g) + c * (d * h - E * g);
            if (det == 0) throw new ArithmeticException();

            Matrix3D inv = new Matrix3D();
            inv[0] = (E * i - f * h) / det;
            inv[3] = (c * h - b * i) / det;
            inv[6] = (b * f - c * E) / det;
            inv[1] = (f * g - d * i) / det;
            inv[4] = (a * i - c * g) / det;
            inv[7] = (c * d - a * f) / det;
            inv[2] = (d * h - E * g) / det;
            inv[5] = (b * g - a * h) / det;
            inv[8] = (a * E - b * d) / det;
            return inv;
        }
        public Matrix3D OrthogonalFactor(double eps)
        {
            Matrix3D Q = new Matrix3D(this);
            Matrix3D Q2 = new Matrix3D();
            double err = 0;
            do
            {
                Q2 = (Q + Q.InverseTranspose()) / 2.0;
                err = (Q2 - Q).SqNorm();
                Q = Q2;
            } while (err > eps);

            return Q2;
        }
        public Matrix3D OrthogonalFactorIter()
        {
            return (this + this.InverseTranspose()) / 2;
        }
        public static Matrix3D IdentityMatrix()
        {
            Matrix3D m = new Matrix3D();
            m[0] = m[4] = m[8] = 1.0;
            return m;
        }
        public static Vector3D operator *(Matrix3D m, Vector3D v)
        {
            Vector3D ret = new Vector3D();
            ret.x = m[0] * v.x + m[1] * v.y + m[2] * v.z;
            ret.y = m[3] * v.x + m[4] * v.y + m[5] * v.z;
            ret.z = m[6] * v.x + m[7] * v.y + m[8] * v.z;
            return ret;
        }
        public static Matrix3D operator *(Matrix3D m1, Matrix3D m2)
        {
            Matrix3D ret = new Matrix3D();
            for (int i = 0; i < row_size; i++)
                for (int j = 0; j < row_size; j++)
                {
                    ret[i, j] = 0.0;
                    for (int k = 0; k < row_size; k++)
                        ret[i, j] += m1[i, k] * m2[k, j];
                }
            return ret;
        }
        public static Matrix3D operator +(Matrix3D m1, Matrix3D m2)
        {
            Matrix3D ret = new Matrix3D();
            for (int i = 0; i < len; i++) ret[i] = m1[i] + m2[i];
            return ret;
        }
        public static Matrix3D operator -(Matrix3D m1, Matrix3D m2)
        {
            Matrix3D ret = new Matrix3D();
            for (int i = 0; i < len; i++) ret[i] = m1[i] - m2[i];
            return ret;
        }
        public static Matrix3D operator *(Matrix3D m, double d)
        {
            Matrix3D ret = new Matrix3D();
            for (int i = 0; i < len; i++) ret[i] = m[i] * d;
            return ret;
        }
        public static Matrix3D operator /(Matrix3D m, double d)
        {
            Matrix3D ret = new Matrix3D();
            for (int i = 0; i < len; i++) ret[i] = m[i] / d;
            return ret;
        }

        public static Matrix3D Rotate(double angle)
        {
            Matrix3D rotate = new Matrix3D();
            double cosX = Math.Cos(angle);
            double sinX = Math.Sin(angle);

            rotate[0, 0] = cosX; rotate[0, 1] = -sinX;
            rotate[1, 0] = sinX; rotate[1, 1] = cosX;
            rotate[2, 2] = 1;

            return rotate;
        }
    }
}
