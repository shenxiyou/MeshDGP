using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public struct Complex
    {
        public double RealPart;


        public double ImagePart;

        public static Complex Zero
        {
            get { return new Complex(0f, 0f); }
        }

        public static Complex Identity
        {
            get { return new Complex(1f, 1f); }
        }

        public override bool Equals(object obj)
        {
            Complex cobj = (Complex)obj;

            if (cobj.RealPart == this.RealPart && cobj.ImagePart == this.ImagePart)
            {
                return true;
            }

            return false;
        }

        public Complex(double realPart, double imagePart)
        {
            this.RealPart = realPart;
            this.ImagePart = imagePart;
        }

        public static Complex operator +(Complex left, Complex right)
        {
            return new Complex(left.RealPart + right.RealPart, left.ImagePart + right.ImagePart);
        }

        public static Complex operator -(Complex left, Complex right)
        {
            return new Complex(left.RealPart - right.RealPart, left.ImagePart - right.ImagePart);
        }

        public static Complex operator *(Complex left, Complex right)
        {
            double a = left.RealPart;
            double b = left.ImagePart;
            double c = right.RealPart;
            double d = right.ImagePart;

            return new Complex((a * c - b * d), (b * c + a * d));
        }

        public static Complex operator *(double left, Complex right)
        {
            return new Complex(right.RealPart * left, right.ImagePart * left);
        }

        public static Complex operator *(Complex left, double right)
        {
            return new Complex(left.RealPart * right, left.ImagePart * right);
        }

        public static Complex operator /(Complex left, Complex right)
        {
            double a = left.RealPart;
            double b = left.ImagePart;
            double c = right.RealPart;
            double d = right.ImagePart;

            return new Complex((a * c + b * d) / (c * c + d * d), (b * c - a * d) / (c * c + d * d));
        }

        public static Complex operator /(Complex left, double right)
        {

            return new Complex(left.RealPart / right, left.ImagePart / right);
        }

        public double Norm()
        {
            return Math.Sqrt(RealPart * RealPart + ImagePart * ImagePart);
        }

        public double Norm2()
        {
            return RealPart * RealPart + ImagePart * ImagePart;
        }

        public Complex Inv()
        {
            return this.Conj() / this.Norm2();
        }

        public Complex Conj()
        {
            return new Complex(RealPart, -ImagePart);
        }

        public override string ToString()
        {
            return RealPart + " + " + ImagePart + "i";
        }
    }
}
