using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GraphicResearchHuiZhao
{
    public partial class LinearSystemDEC
    {

        private LinearSystemDEC()
        {
        }

        // solves the sparse linear system Ax = b using sparse LU factorization
        public DenseMatrixDouble solveLU(ref SparseMatrixDouble A, ref DenseMatrixDouble b)
        {

            LinearSystemGenericByLib linearSolver = new LinearSystemGenericByLib();
            //linearSolver.FactorizationLU(ref A);
            DenseMatrixDouble x = null;
            linearSolver.FreeSolver();

            return x;
        }

        // solves the positive definite sparse linear system Ax = b using sparse Cholesky factorization
        public DenseMatrixDouble solvePositiveDefinite(ref SparseMatrixDouble A, ref DenseMatrixDouble b)
        {
            LinearSystemGenericByLib linearSolver = new LinearSystemGenericByLib();
            linearSolver.FactorizationCholesky(ref A);
            DenseMatrixDouble x = null;
            linearSolver.FreeSolver();
            return x;
        }

        // solves the positive definite sparse linear system Ax = b using sparse QR factorization
        public DenseMatrixDouble solveLeastSquare(ref SparseMatrixDouble A, ref DenseMatrixDouble b)
        {
            LinearSystemGenericByLib linearSolver = new LinearSystemGenericByLib();
            linearSolver.FactorizationQR(ref A);
            DenseMatrixDouble x = null;
            linearSolver.FreeSolver();
            return x;
        }


        // solves the positive definite sparse linear system Ax = b using sparse QR factorization
        public DenseMatrixDouble solveLeastNormal(ref SparseMatrixDouble A, ref DenseMatrixDouble b)
        {
            LinearSystemGenericByLib linearSolver = new LinearSystemGenericByLib();
            linearSolver.FactorizationQR(ref A);
            DenseMatrixDouble x = null;
            linearSolver.FreeSolver();
            return x;
        }



    }
}
