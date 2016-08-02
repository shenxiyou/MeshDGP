using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{

    //Use Power iteration  to compute smallest or largest eigenvalue and eigenvector
    public partial class LinearSystemDEC
    {

        private static LinearSystemDEC singleton = new LinearSystemDEC();

        public static LinearSystemDEC Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new LinearSystemDEC();
                return singleton;
            }
        }

        public int maxEigIter = 10;

        // solves A x = lambda x for the smallest nonzero eigenvalue lambda
        // A must be symmetric; x is used as an initial guess
        public DenseMatrixDouble smallestEig(ref SparseMatrixDouble A, bool ignoreConstantVector)
        {
            ignoreConstantVector = true;

            DenseMatrixDouble x = new DenseMatrixDouble();

            for (int iter = 0; iter < maxEigIter; iter++)
            {
                x = solveLU(ref A, ref x);
                if (ignoreConstantVector)
                {
                    //  x.removeMean();
                }
                //x.normalize();
            }

            return x;
        }


        // solves A x = lambda B x for the smallest nonzero generalized eigenvalue lambda
        // A and B must be symmetric; x is used as an initial guess
        public DenseMatrixDouble smallestEig<T>(ref SparseMatrixDouble A, SparseMatrixDouble B)
        {
            DenseMatrixDouble x = new DenseMatrixDouble();

            return x;
        }


        // solves A x = lambda x for the smallest nonzero eigenvalue lambda
        // A must be positive (semi-)definite; x is used as an initial guess
        public DenseMatrixDouble smallestEigPositiveDefinite(ref SparseMatrixDouble A, bool ignoreConstantVector)
        {
            ignoreConstantVector = true;

            DenseMatrixDouble x = new DenseMatrixDouble();

            return x;
        }

        public DenseMatrixComplex smallestEigPositiveDefinite(ref SparseMatrixComplex A, bool ignoreConstantVector)
        {
            ignoreConstantVector = true;

            DenseMatrixComplex x = new DenseMatrixComplex();

            return x;
        }


        // solves A x = lambda B x for the smallest nonzero eigenvalue lambda
        // A must be positive (semi-)definite, B must be symmetric; x is used as an initial guess
        public DenseMatrixDouble smallestEigPositiveDefinite<T>(ref SparseMatrixDouble A, ref SparseMatrixDouble B)
        {

            DenseMatrixDouble x = new DenseMatrixDouble();

            return x;
        }


        // solves A x = lambda (B - EE^T) x for the smallest nonzero eigenvalue lambda
        // A must be positive (semi-)definite, B must be symmetric; EE^T is a low-rank matrix, and
        // x is used as an initial guess
        public DenseMatrixDouble smallestEigPositiveDefinite(ref SparseMatrixDouble A, ref SparseMatrixDouble B, ref SparseMatrixDouble E)
        {
            DenseMatrixDouble x = new DenseMatrixDouble();

            return x;
        }

        public int MaxEigIter = 20;

        public DenseMatrixComplex smallestEigPositiveDefinite(ref SparseMatrixComplex A, ref SparseMatrixComplex B, ref DenseMatrixComplex x)
        {
            LinearSystemGenericByLib.Instance.FactorizationLU(ref A);
            int n = A.Length();

            DenseMatrixComplex e = new DenseMatrixComplex(n, 1);
            e.Fill(new Complex(1, 0));

            Complex dot = e.Dot(B * e);
            double norm = Math.Sqrt(dot.Norm());
            e /= new Complex(norm, 0);

            //Iteratation
            for (int i = 0; i < MaxEigIter; i++)
            {
                x = B * x;
                x = LinearSystemGenericByLib.Instance.SolveByFactorizedLU(ref x);
                x -= x.Dot(B * e) * e;

                double newNorm = Math.Sqrt(x.Dot(B * x).Norm());
                x /= new Complex(newNorm, 0);
            }

            LinearSystemGenericByLib.Instance.FreeSolverLUComplex();
            return x;
        }

        public DenseMatrixQuaternion SolveEigen(ref SparseMatrixQuaternion A)
        {
            LinearSystemGenericByLib.Instance.FactorizationLU(ref A);

            DenseMatrixQuaternion b = new DenseMatrixQuaternion(A.ColumnCount, 1);
            b.Fill(Quaternion.Identity);

            for (int i = 0; i < 20; i++)
            {
                b.Normalize();
                DenseMatrixQuaternion x = LinearSystemGenericByLib.Instance.SolveByFactorizedLU(ref b);
                b = null;
                b = x;
            }

            GC.Collect();
            b.Normalize();
            LinearSystemGenericByLib.Instance.FreeSolverLU();
            return b;
        }

        public DenseMatrixDouble smallestEigPositiveDefinite<T>(ref SparseMatrixDouble A, ref SparseMatrixDouble B, ref SparseMatrixDouble E)
        {
            DenseMatrixDouble x = new DenseMatrixDouble();

            return x;
        }

        // returns the max residual of the linear problem A x = b relative to the largest entry of the solution
        public double residual(ref   SparseMatrixDouble A, ref    SparseMatrixDouble x, ref SparseMatrixDouble b)
        {
            return 0;
        }

        public double cResidual(ref SparseMatrixComplex A, ref DenseMatrixComplex b, ref DenseMatrixComplex x)
        {
            DenseMatrixComplex errors = (A * x - b);
            double norm = errors.InifnityNorm() / x.InifnityNorm();
            return norm;
        }


        // returns the max residual of the eigenvalue problem A x = lambda x relative to the largest entry of the solution

        public double residual(ref   SparseMatrixDouble A, ref   DenseMatrixDouble x)
        {
            return 0;
        }

        // returns the max residual of the generalized eigenvalue problem A x = lambda B x relative to the largest entry of the solution

        public double residual(ref  SparseMatrixDouble A, ref  SparseMatrixDouble B, ref   DenseMatrixDouble x)
        {
            return 0;
        }


        // returns the max residual of the generalized eigenvalue problem A x = lambda (B - EE^T) x relative to the largest entry of the solution

        public double residual(ref SparseMatrixDouble A, ref   SparseMatrixDouble B, ref DenseMatrixDouble E, ref  DenseMatrixDouble x)
        {
            return 0;
        }


        // returns <Ax,x>/<x,x>
        public double rayleighQuotient(ref SparseMatrixDouble A, ref  DenseMatrixDouble x)
        {

            return default(double);
        }

        // returns <Ax,x>/<Bx,x>
        public double rayleighQuotient(ref SparseMatrixDouble A, ref SparseMatrixDouble B, ref  DenseMatrixDouble x)
        {
            return default(double);
        }

        // returns <Ax,x>/<(B-EE^T)x,x>
        public double rayleighQuotient(ref SparseMatrixDouble A, ref SparseMatrixDouble B, ref  DenseMatrixDouble E, ref  DenseMatrixDouble x)
        {
            return default(double);
        }
    }
}
