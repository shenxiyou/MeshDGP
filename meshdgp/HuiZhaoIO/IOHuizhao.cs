using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;



namespace GraphicResearchHuiZhao
{
    public partial class IOHuiZhao
    {
        //Single instance
        private static IOHuiZhao singleton = new IOHuiZhao();

        public static IOHuiZhao Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new IOHuiZhao();
                return singleton;
            }
        }

        public SparseMatrix ReadMatrix(String modelName)
        {

            string fileName = Path.GetFileNameWithoutExtension(modelName);
            string path = GetPath() + fileName + ".matrix";

            return ReadMatrixBasic(path);
        }

        private static SparseMatrix ReadMatrixBasic(string path)
        {
            StreamReader sr = new StreamReader(path);
            String line = null;

            int m = int.MinValue;
            int n = int.MinValue;

            int count = 0;
            SparseMatrix sm = null;

            while ((line = sr.ReadLine()) != null)
            {
                String[] token = line.Split(' ');

                if (count == 0)
                {
                    m = int.Parse(token[1]);
                    n = int.Parse(token[3]);
                    int nnz = int.Parse(token[5]);
                    sm = new SparseMatrix(m, n);

                    count++;
                    continue;
                }
                if (line[0] == '#') continue;

                int index_i = int.Parse(token[0]);
                int index_j = int.Parse(token[1]);
                double value = double.Parse(token[2]);
                if (value != 0)
                {
                    sm.AddValueTo(index_i, index_j, value);
                }

            }

            sr.Close();

            GC.Collect();
            return sm;
        }

        public void WriteMatrix(ref SparseMatrix sparseMatrix, string modelName)
        {
            string fileName = Path.GetFileNameWithoutExtension(modelName);
            string path = GetPath() + fileName + ".matrix";

            WriteMatrix(sparseMatrix, path);
        }

        private static void WriteMatrix(SparseMatrix sparseMatrix, string path)
        {
            StreamWriter sw;
            sw = File.CreateText(path);


            int lineCount = 0;
            bool symmetric = sparseMatrix.IsSymmetric();

            //Get NoneOfZeroCount
            foreach (List<SparseMatrix.Element> col in sparseMatrix.Columns)
            {
                foreach (SparseMatrix.Element e in col)
                {
                    if (!symmetric && e.value != 0)
                    {
                        lineCount++;
                    }

                    if (symmetric && e.i >= e.j && e.value != 0)
                    {
                        lineCount++;
                    }
                }
            }


            //Output matrix attributes
            sw.WriteLine("#Row: " + sparseMatrix.RowSize + " Columns: " + sparseMatrix.ColumnSize + " nnz: " + lineCount);

            if (symmetric)
            {
                sw.WriteLine("#Symmetric: true L");
            }
            else
            {
                sw.WriteLine("#Symmetric: false N");
            }

            int z = 0;
            foreach (List<SparseMatrix.Element> col in sparseMatrix.Columns)
            {
                foreach (SparseMatrix.Element e in col)
                {
                    if (!symmetric && e.value != 0)
                    {
                        z++;
                        if (z != lineCount)
                            sw.WriteLine(e.i + " " + e.j + " " + e.value);
                        else
                            sw.Write(e.i + " " + e.j + " " + e.value);
                    }

                    if (symmetric && e.i >= e.j && e.value != 0)
                    {
                        z++;
                        if (z != lineCount)
                            sw.WriteLine(e.i + " " + e.j + " " + e.value);
                        else
                            sw.Write(e.i + " " + e.j + " " + e.value);
                    }
                }
            }

            sw.Close();
        }

        public double[] ReadVector(string modelName)
        {
            string fileName = Path.GetFileNameWithoutExtension(modelName);
            string path = GetPath() + fileName + "_solution.vector";

            StreamReader sr = new StreamReader(path);
            String line = sr.ReadLine();

            //string[] strs = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //int count = int.Parse(strs[strs.Length - 1]);
            //double[] values = new double[count];
            List<double> list = new List<double>();

            //int i = 0;
            while ((line = sr.ReadLine()) != null)
            {
                double value = double.Parse(line);
                list.Add(value);
                //values[i] = value;
                //i++;
            }

            sr.Close();
            return list.ToArray();
        }

        public void WriteVector(ref double[] vector, string modelName)
        {
            string fileName = Path.GetFileNameWithoutExtension(modelName);
            string path = GetPath() + fileName + ".vector";

            StreamWriter sw;
            sw = File.CreateText(path);

            sw.WriteLine("# Vector is " + vector.Length);

            foreach (double i in vector)
            {
                sw.WriteLine(i);
            }

            sw.Close();
        }

        public double[] ReadSystem(string modelName)
        {
            double[] result = null;

            string fileName = Path.GetFileNameWithoutExtension(modelName);
            string path = GetPath() + fileName + "_solution.vector";

            result = ReadVector(path);

            return result;
        }

        public void WriteSystem(ref SparseMatrix A, ref double[] rightB, string modelName)
        {
            WriteMatrix(ref A, modelName);
            WriteVector(ref rightB, modelName);
        }

        public Eigen ReadEigen(string modelName)
        {
            string fileName = Path.GetFileNameWithoutExtension(modelName);
            string path = GetPath() + fileName + ".eigens";

            Eigen eigen = new Eigen();
            List<EigenPair> list = new List<EigenPair>();

            //Read File
            using (StreamReader sr = new StreamReader(path))
            {
                String line = null;
                List<double> currentVector = null;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != "" && line[0] == '#')
                    {
                        String[] tokens = line.Split(' ');
                        List<double> aVector = new List<double>();
                        currentVector = aVector;

                        EigenPair pair = new EigenPair(double.Parse(tokens[1]), aVector);
                        list.Add(pair);
                    }
                    else if (line != "")
                    {
                        double value = double.Parse(line);
                        currentVector.Add(value);
                    }
                }
            }
            list.Sort();
            eigen.SortedEigens = list.ToArray();
            return eigen;
        }
    }
}