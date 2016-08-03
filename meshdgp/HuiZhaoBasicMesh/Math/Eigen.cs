using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    //public class Eigen
    //{
    

    //    public SortedList<double, EigenPair> SortedEigens = new SortedList<double, EigenPair>();

    //    public int Count
    //    {
    //        get
    //        {
    //            return SortedEigens.Count;
    //        }
    //    }

    //    public List<double> Eigens
    //    {
    //        get
    //        {
    //            return (List<double>)SortedEigens.Keys;
    //        }
    //    }


    //    public List<EigenPair> EigenVectors
    //    {
    //        get
    //        {
    //            return (List<EigenPair>)SortedEigens.Values;
    //        }
    //    }


    //    public List<double> GetEigenVector(int i)
    //    {
    //        return SortedEigens.Values[i].EigenVector;
    //    }

    //    public int EigenVectorSize 
    //    {
    //        get
    //        {
    //            return SortedEigens.Values[0].EigenVector.Count;
    //        }
    //    }


    //    public double GetEigenValue(int i)
    //    {
    //        return SortedEigens.Values[i].EigenValue;
    //    }


    //    public EigenPair GetEigen(int i)
    //    {
    //        return SortedEigens.Values[i];
    //    }

       

    //    public Eigen()
    //    { 
            
    //    }
    //}



    public class Eigen
    {
        public EigenPair[] SortedEigens;

        public int Count
        {
            get
            {
                return this.SortedEigens.Length;
            }
        }

        public List<double> GetEigenVector(int i)
        {
            return SortedEigens[i].EigenVector;
        }

        public double GetEigenValue(int i)
        {
            return SortedEigens[i].EigenValue;
        }
        public int EigenVectorSize
        {
            get
            {
                return SortedEigens[0].EigenVector.Count;
            }
        }


        //public static Eigen ReadEigenVectorsFromFile(string File)
        //{
        //    Eigen eigen = new Eigen();
        //    List<EigenPair> list = new List<EigenPair>();

        //    //Read File
        //    using (System.IO.StreamReader sr = new System.IO.StreamReader(File))
        //    {
        //        String line = null;
        //        List<double> currentVector = null;
        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            if (line != "" && line[0] == '#')
        //            {
        //                String[] tokens = line.Split(' ');
        //                List<double> aVector = new List<double>();
        //                currentVector = aVector;

        //                EigenPair pair = new EigenPair(double.Parse(tokens[1]), aVector);
        //                list.Add(pair);
        //            }
        //            else if (line != "")
        //            {
        //                double value = double.Parse(line);
        //                currentVector.Add(value);
        //            }
        //        }
        //    }
        //    list.Sort();
        //    eigen.SortedEigens = list.ToArray();
        //    return eigen;
        //}

        public Eigen()
        {

        }
    }
}
