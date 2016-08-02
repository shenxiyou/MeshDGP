using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
 
using System.Diagnostics;

namespace GraphicResearchHuiZhao
{
    public partial class LinearSystem
    {
        //Single instance
        private static LinearSystem singleton = new LinearSystem();

        public static LinearSystem Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new LinearSystem();
                return singleton;
            }
        }

       

        

        public LinearSystemInfo LinearSystemInfo = new LinearSystemInfo();

        
       

        public double[] SolveSystem(ref SparseMatrix A, ref double[] rightB,string modelname)
        {
            if (LinearSystemInfo.Matlab)
            {
                return SolveByMatLab(ref A, ref rightB,modelname);
            }

            else
            {
                return SolveByLib(ref A, ref rightB);
            }
        }

        public double[] SolveSystem(ref SparseMatrix A, ref double[] rightB)
        {
            return SolveSystem(ref A, ref rightB, "HuiZhao");
        }

        private double[] SolveByLib(ref  SparseMatrix A, ref double[] rightB)
        {
            double[] result = new double[rightB.Length];

            if (A.RowSize > rightB.Length)
            {
                result = SloveLeastSquare(ref A, ref rightB);
            }
            if (A.RowSize == rightB.Length)
            {
                result = SloveSquare(ref A, ref rightB);
            }
            if (A.RowSize < rightB.Length)
            {
                result = SloveLeastNormal(ref A, ref rightB);
            }

            return result;
        }

      


       

        public double[] SloveSquare(ref SparseMatrix A, ref double[] rightB)
        {
            double[] unknown = new double[rightB.Length];

            LinearSystemLib linearSolver = new LinearSystemLib(EnumSolver.UmfpackLU);
            linearSolver.Factorization(A);
            linearSolver.SolveLinerSystem(ref rightB, ref unknown);
            linearSolver.FreeSolver();
            return unknown;
        }

        public double[] SlovePositiveDefinite(ref SparseMatrix A, ref double[] rightB)
        {
            double[] unknown = new double[rightB.Length];

            LinearSystemLib linearSolver = new LinearSystemLib(EnumSolver.CholmodCholesky);
            linearSolver.Factorization(A);
            linearSolver.SolveLinerSystem(ref rightB, ref unknown);
            linearSolver.FreeSolver();
            return unknown;
        }

        public double[] SloveSymmetric(ref SparseMatrix A, ref double[] rightB)
        {
            double[] unknown = new double[rightB.Length];

            LinearSystemLib linearSolver = new LinearSystemLib(EnumSolver.UmfpackLU);
            linearSolver.Factorization(A);
            linearSolver.SolveLinerSystem(ref rightB, ref unknown);
            linearSolver.FreeSolver();
            return unknown;
        }

        public double[] SloveLeastSquare(ref SparseMatrix L, ref double[] b)
        {
            double[] result = new double[L.ColumnSize];

            LinearSystemLib linearSolver = new LinearSystemLib();
            linearSolver.SolverType = EnumSolver.SPQRLeastSqure;
            linearSolver.Factorization(L);
            linearSolver.SolveLinerSystem(ref b, ref result);
            linearSolver.FreeSolver();

            return result;
        }

        public double[] SloveLeastNormal(ref SparseMatrix L, ref double[] b)
        {
            double[] result = new double[L.ColumnSize];


            LinearSystemLib linearSolver = new LinearSystemLib();
            linearSolver.SolverType = EnumSolver.SPQRLeastNormal;
            linearSolver.Factorization(L.Transpose());
            linearSolver.SolveLinerSystem(ref b,ref result);
            linearSolver.FreeSolver();
         
            return result;
        }




      
         

    }
}
