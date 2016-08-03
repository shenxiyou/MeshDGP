using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class LinearSystemDEC
    {

        LinearSystemGenericByLib linearSolver = null;
        // solves the positive definite sparse linear system Ax = b using sparse Cholesky factorization
        public void solveBackPositiveDefiniteInit(ref SparseMatrixDouble A)
        {
            linearSolver = new LinearSystemGenericByLib();
            linearSolver.FactorizationCholesky(ref A);

        }


        // solves the positive definite sparse linear system Ax = b using sparse Cholesky factorization
        public DenseMatrixDouble solveBackPositiveDefiniteIterate(ref DenseMatrixDouble b)
        {
            DenseMatrixDouble x = null;
            if (linearSolver != null)
            {
                //  x= linearSolver.SolveLinerSystem(ref b);
            }
            return x;

        }

        // solves the positive definite sparse linear system Ax = b using sparse Cholesky factorization
        public void solveBackPositiveDefiniteFree()
        {

            if (linearSolver != null)
            {
                linearSolver.FreeSolver();
            }
        }

    }
}
