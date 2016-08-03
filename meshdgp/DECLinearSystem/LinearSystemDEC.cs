using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GraphicResearchHuiZhao
{
    public class LinearSystemDEC
    {

         #region import SuiteSparseQR functions
        [DllImport("SuiteSparseQR.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void* solveSymmetric(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int numberOfEntries, int* Ti, int* Tj, double* Tx);

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



        private LinearSystemDEC()
        {
             
        }


        // solves the sparse linear system Ax = b using sparse QR factorization
        public    void solve(ref SparseMatrix<T>  A, DenseMatrix<T>  x, ref DenseMatrix<T> b ) 
   
        {
        }
 
         // solves the sparse linear system Ax = b using sparse LU factorization
        public void solveSymmetric( ref SparseMatrix<T>  A, ref DenseMatrix<T>  x, ref DenseMatrix<T> b )       
        {
        }

   
        // solves the positive definite sparse linear system Ax = b using sparse Cholesky factorization
        public void solvePositiveDefinite(ref SparseMatrix<T>  A,ref DenseMatrix<T>  x,ref DenseMatrix<T>  b ) 
        {
        }
   

         // backsolves the prefactored positive definite sparse linear system LL'x = b
        public void backsolvePositiveDefinite(ref SparseFactor<T>  L, ref DenseMatrix<T>  x,ref DenseMatrix<T>  b ) 
        {
        }
  

         // solves A x = lambda x for the smallest nonzero eigenvalue lambda
        // A must be symmetric; x is used as an initial guess
        public  void smallestEig( ref SparseMatrix<T> A,ref  DenseMatrix<T> x,  bool ignoreConstantVector = true )
        {
        }
  

        // solves A x = lambda B x for the smallest nonzero generalized eigenvalue lambda
        // A and B must be symmetric; x is used as an initial guess
        public  void smallestEig(ref SparseMatrix<T>  A, SparseMatrix<T>  B,ref DenseMatrix<T>  x )
        {
        }


        // solves A x = lambda x for the smallest nonzero eigenvalue lambda
        // A must be positive (semi-)definite; x is used as an initial guess
        public  void smallestEigPositiveDefinite(ref SparseMatrix<T>  A, ref DenseMatrix<T> x, bool ignoreConstantVector = true )    
        {
        }


        // solves A x = lambda B x for the smallest nonzero eigenvalue lambda
        // A must be positive (semi-)definite, B must be symmetric; x is used as an initial guess
        public void smallestEigPositiveDefinite(ref SparseMatrix<T>  A,ref SparseMatrix<T> B,ref DenseMatrix<T> x )
        {
        }


        // solves A x = lambda (B - EE^T) x for the smallest nonzero eigenvalue lambda
       // A must be positive (semi-)definite, B must be symmetric; EE^T is a low-rank matrix, and
       // x is used as an initial guess
        public  void smallestEigPositiveDefinite( ref SparseMatrix<T>  A,ref SparseMatrix<T>  B,ref DenseMatrix<T>  E,ref DenseMatrix<T>  x )
        {
        }
 
         // returns the max residual of the linear problem A x = b relative to the largest entry of the solution
        public  double residual(ref   SparseMatrix<T> A,ref    DenseMatrix<T>  x, ref DenseMatrix<T> b )
        {
            return 0;
        }
  
        // returns the max residual of the eigenvalue problem A x = lambda x relative to the largest entry of the solution

        public double residual(ref   SparseMatrix<T> A,ref   DenseMatrix<T>x )
        {
            return 0;
        }
   
        // returns the max residual of the generalized eigenvalue problem A x = lambda B x relative to the largest entry of the solution

        public double residual( ref  SparseMatrix<T>   A,ref  SparseMatrix<T>  B, ref   DenseMatrix<T>  x )
        {
            return 0;
        }
   

          // returns the max residual of the generalized eigenvalue problem A x = lambda (B - EE^T) x relative to the largest entry of the solution

        public double residual( ref SparseMatrix<T>  A,ref   SparseMatrix<T>  B, ref DenseMatrix<T>  E, ref  DenseMatrix<T>  x )
        {
            return 0;
        }
 

        // returns <Ax,x>/<x,x>
        public T rayleighQuotient( ref SparseMatrix<T>  A, ref  DenseMatrix<T>  x ) 
        {
            return 0;
        }
   
        // returns <Ax,x>/<Bx,x>
        public T rayleighQuotient( ref SparseMatrix<T> A, ref SparseMatrix<T>  B, ref  DenseMatrix<T>  x )
        {
            return 0;
        }
   
        // returns <Ax,x>/<(B-EE^T)x,x>
        public T rayleighQuotient( ref SparseMatrix<T>  A,ref SparseMatrix<T>  B, ref  DenseMatrix<T>  E, ref  DenseMatrix<T>  x )
        {
            return 0;
        }
   

    }
}
