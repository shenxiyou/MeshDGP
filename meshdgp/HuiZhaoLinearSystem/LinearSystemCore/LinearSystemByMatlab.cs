using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GraphicResearchHuiZhao
{
    public partial class LinearSystem
    {


        public double[] SolveByMatLab(ref SparseMatrix A, ref double[] rightB, string modelName)
        {
            double[] unKnown = new double[rightB.Length];

            IOHuiZhao.Instance.WriteSystem(ref A, ref rightB, modelName);

            


            //
            //Must Put A breakpoint Above And Below Here!!!!!!!!!!!!!!!!!!!!!!!!!
            //

            unKnown = IOHuiZhao.Instance.ReadVector(modelName);


            return unKnown;
        }

        public SparseMatrix MultiplyByMatlab(SparseMatrix A, SparseMatrix B)
        {
            IOHuiZhao.Instance.WriteMatrix(ref A, "abf_A.matrix");
            IOHuiZhao.Instance.WriteMatrix(ref B, "abf_B.matrix");

            SparseMatrix C = IOHuiZhao.Instance.ReadMatrix("AB.matrix");

            return C;
        }

        public Eigen ComputeEigensByMatLab(SparseMatrix sparse, int count, string modelName)
        {


            IOHuiZhao.Instance.WriteMatrix(ref sparse, modelName);

            Eigen eigen = IOHuiZhao.Instance.ReadEigen(modelName);

            return eigen;
        }

       

    }
}
