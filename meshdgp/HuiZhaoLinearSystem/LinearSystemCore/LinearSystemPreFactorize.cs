using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class LinearSystem
    {

        private LinearSystemLib linearSolverPreFactorize = new LinearSystemLib();
        public void Factorize(ref  SparseMatrix A)
        {

            linearSolverPreFactorize = new LinearSystemLib(LinearSystemInfo.SolverType);
            linearSolverPreFactorize.Factorization(A);
        }

        public void Factorize(ref  SparseMatrix A, EnumSolver solver)
        {
            linearSolverPreFactorize = new LinearSystemLib(solver);
            linearSolverPreFactorize.Factorization(A);
        }

        public double[] SolveByPreFactorize(ref double[] rightB)
        {
            if (linearSolverPreFactorize == null)
            {
                throw (new Exception("Please Prefactorize Matrix!!!"));
            }
            double[] unknow = new double[rightB.Length];
            linearSolverPreFactorize.SolveLinerSystem(ref rightB, ref unknow);
            return unknow;
        }

        public void Free()
        {
            linearSolverPreFactorize.FreeSolver();
        }

    }
}
