using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    //public class EigenPair
    //{
    //    public List<double> EigenVector = new List<double>();
    //    public double EigenValue;


    //    public EigenPair(double eigenValue, List<double> EigenVector)
    //    {
    //        this.EigenValue = eigenValue;
    //        this.EigenVector = EigenVector;
    //    }
    //}


    public class EigenPair : IComparable<EigenPair>
    {
        public List<double> EigenVector = new List<double>();
        public double EigenValue;


        public EigenPair(double eigenValue, List<double> EigenVector)
        {
            this.EigenValue = eigenValue;
            this.EigenVector = EigenVector;
        }

        public int CompareTo(EigenPair other)
        {
            double left = Math.Abs(this.EigenValue);
            double right = Math.Abs(other.EigenValue);
            return left.CompareTo(right);
        }
    }
}
