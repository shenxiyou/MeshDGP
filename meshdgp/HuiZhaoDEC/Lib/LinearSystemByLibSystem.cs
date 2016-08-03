using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GraphicResearchHuiZhao
{
    public unsafe partial class LinearSystemGenericByLib
    {
        private static LinearSystemGenericByLib singleton = new LinearSystemGenericByLib();

        public static LinearSystemGenericByLib Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new LinearSystemGenericByLib();
                return singleton;
            }
        }

        #region import SuperLU functions

        [DllImport("SuperLU.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void* CreateSolverLUSuperLU(int numberOfRows, int numberOfColumns, int numberOfNoneZero, int* rowIndex, int* columnIndex, double* values);
        [DllImport("SuperLU.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void SolveLUSuperLU(void* solver, double* x, double* b);
        [DllImport("SuperLU.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void FreeSolverLUSuperLU(void* solver);

        #endregion

        #region import CHOMOLD functions
        [DllImport("CHOLMOD.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void* CreateSolverCholeskyCHOLMOD(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int numberOfEntries, int* Ti, int* Tj, double* Tx);
        [DllImport("CHOLMOD.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void SolveCholeskyCHOLMOD(void* solver, double* X, double* b);
        [DllImport("CHOLMOD.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void FreeSolverCholeskyCHOLMOD(void* solver);
        [DllImport("CHOLMOD.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void SolveRealByCholesky(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int* Ti, int* Tj, double* Tx, double* X, double* b);
        [DllImport("CHOLMOD.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void SolveRealByCholesky_CRS(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int* rowIndex, int* colPtr, double* values, double* X, double* b);
        [DllImport("CHOLMOD.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void SolveRealByCholesky_CCS(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int* rowPtr, int* colIndex, double* values, double* X, double* b);

        //For Complex:
        [DllImport("CHOLMOD.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void* CreateSolverCholesky_CCS_Complex(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int* rowPtr, int* colIndex, double* values);
        [DllImport("CHOLMOD.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void SolveCholeskyCHOLMOD_Complex(void* solver, double* X, double* B);

        #endregion

        #region import UMFPACK functions
        [DllImport("UMFPack.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void FreeSolverLUUMFPACK(void* a);
        [DllImport("UMFPack.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void* CreateSolverLUUMFPACK(int numberOfRows, int numberOfNoneZero, int* RowIndex, int* ColumnIndex, double* Value);
        [DllImport("UMFPack.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe int SolveLUUMFPACK(void* solver, double* x, double* b);
        [DllImport("UMFPack.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void SolveRealByLU(int numberOfRow, int numberOfColumn, int nnz, int* Ti, int* Tj, double* Tx, double* X, double* b);
        [DllImport("UMFPack.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void SolveRealByLU_CCS(int numberOfRow, int numberOfColumn, int nnz, int* rowIndices, int* colPtr, double* values, double* X, double* b);
        [DllImport("UMFPack.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void* CreateSolverLUUMFPACK_CCS_Complex(int numberOfRow, int numberOfColumn, int nnz, int* rowIndices, int* colPtr, double* Values);
        [DllImport("UMFPack.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void* CreateSolverLUUMFPACK_CCS(int numberOfRow, int numberOfColumn, int nnz, int* rowIndices, int* columnPtr, double* Values);
        [DllImport("UMFPack.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe int SolveLUUMFPACK_Complex(void* solver, double* x, double* b);
        [DllImport("UMFPack.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void FreeSolverLUUMFPACK_Complex(void* a);

        #endregion

        #region import SuiteSparseQR functions
        [DllImport("SuiteSparseQR.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void* CreateSolverQRSuiteSparseQR(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int numberOfEntries, int* Ti, int* Tj, double* Tx);
        [DllImport("SuiteSparseQR.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void* CreateSolverQRSuiteSparseQR_CCS(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int* rowIndex, int* colPtr, double* values);
        [DllImport("SuiteSparseQR.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void* CreateSolverQRSuiteSparseQR_CRS(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int* rowIndex, int* colPtr, double* values);
        [DllImport("SuiteSparseQR.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void SolveQRSuiteSparseQR(void* solver, double* X, double* b);
        [DllImport("SuiteSparseQR.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void SolveLeastNormalByQR(void* solver, double* X, double* b);
        [DllImport("SuiteSparseQR.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void SolveLeastSqureByQR(void* solver, double* X, double* b);
        [DllImport("SuiteSparseQR.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void FreeSolverQRSuiteSparseQR(void* solver);
        [DllImport("SuiteSparseQR.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void SolveRealByQR(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int* Ti, int* Tj, double* Tx, double* X, double* b);
        [DllImport("SuiteSparseQR.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void SolveRealByQR_CCS(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int* rowIndex, int* colPtr, double* values, double* X, double* b);
        [DllImport("SuiteSparseQR.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void SolveRealByQR_CRS(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int* rowPtr, int* colIndex, double* values, double* X, double* b);

        //For complex
        [DllImport("SuiteSparseQR.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void SolveRealByQR_CCS_Complex(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int* rowPtr, int* colIndex, double* values, double* X, double* b);
        [DllImport("SuiteSparseQR.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void SolveRealByQR_CRS_Complex(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int* rowIndex, int* colPtr, double* values, double* X, double* b);


        #endregion

        private void* solver = null;
        private int m = 0;
        private int n = 0;

        public DenseMatrixDouble SolveLinerSystem(ref SparseMatrixDouble A, ref DenseMatrixDouble b)
        {
            if (A.RowCount != b.RowCount)
            {
                throw new Exception("The dimension of A and b must be agree");
            }


            CholmodInfo cholmodb = CholmodConverter.ConvertDouble(ref b);
            CholmodInfo cholmodA = CholmodConverter.ConverterDouble(ref A, CholmodInfo.CholmodMatrixStorage.CCS);

            double[] x = new double[A.ColumnCount];

            fixed (int* Index = cholmodA.rowIndex, Pt = cholmodA.colIndex)
            fixed (double* val = cholmodA.values, bp = cholmodb.values, xx = x)
            {
                SolveRealByQR_CCS(cholmodA.RowCount,
                                  cholmodA.ColumnCount,
                                  cholmodA.nnz,
                                  Index,
                                  Pt,   //Column Pointer
                                  val,
                                  xx,
                                  bp);
            }

            DenseMatrixDouble unknown = CholmodConverter.dConvertArrayToDenseMatrix(ref x, x.Length, 1);


            cholmodA = null;
            cholmodb = null;
            GC.Collect();
            return unknown;
        }

        public DenseMatrixComplex SolveLinerSystem(ref SparseMatrixComplex A, ref DenseMatrixComplex b)
        {
            if (A.RowCount != b.RowCount)
            {
                throw new Exception("The dimension of A and b must be agree");
            }


            CholmodInfo cholmodb = CholmodConverter.cConverter(ref b);
            CholmodInfo cholmodA = CholmodConverter.cConverter(ref A, CholmodInfo.CholmodMatrixStorage.CCS);

            double[] x = new double[2 * A.ColumnCount];

            fixed (int* Index = cholmodA.rowIndex, Pt = cholmodA.colIndex)
            fixed (double* val = cholmodA.values, bp = cholmodb.values, xx = x)
            {
                SolveRealByQR_CCS_Complex(cholmodA.RowCount,
                                  cholmodA.ColumnCount,
                                  cholmodA.nnz,
                                  Pt,   //Column Pointer
                                  Index,    //Row Index
                                  val,
                                  xx,
                                  bp);
            }

            DenseMatrixComplex unknown = CholmodConverter.cConvertArrayToDenseMatrix(ref x, x.Length, 1);


            cholmodA = null;
            cholmodb = null;
            GC.Collect();
            return unknown;
        }

        public DenseMatrixDouble SolveSymmetricSystem(ref SparseMatrixDouble A, ref DenseMatrixDouble b)
        {
            if (A.RowCount != b.RowCount)
            {
                throw new Exception("The dimension of A and b must be agree");
            }

            if (!A.IsSymmetric())
            {
                throw new Exception("The matrix is asymmetric");
            }

            CholmodInfo cholmodb = CholmodConverter.ConvertDouble(ref b);
            CholmodInfo cholmodA = CholmodConverter.ConverterDouble(ref A, CholmodInfo.CholmodMatrixStorage.CCS);

            double[] x = new double[A.ColumnCount];

            fixed (int* Index = cholmodA.rowIndex, Pt = cholmodA.colIndex)
            fixed (double* val = cholmodA.values, bp = cholmodb.values, xx = x)
            {
                SolveRealByLU_CCS(cholmodA.RowCount,
                                  cholmodA.ColumnCount,
                                  cholmodA.nnz,
                                  Index,    //Row Index
                                  Pt,   //Column Pointer
                                  val,
                                  xx,
                                  bp);
            }

            DenseMatrixDouble unknown = CholmodConverter.dConvertArrayToDenseMatrix(ref x, x.Length, 1);

            cholmodA = null;
            cholmodb = null;
            GC.Collect();

            return unknown;
        }

        public DenseMatrixDouble SolveLinearSystemByLU(ref SparseMatrixDouble A, ref DenseMatrixDouble b)
        {
            if (A.RowCount != b.RowCount)
            {
                throw new Exception("The dimension of A and b must be agree");
            }

            CholmodInfo cholmodb = CholmodConverter.ConvertDouble(ref b);
            CholmodInfo cholmodA = CholmodConverter.ConverterDouble(ref A, CholmodInfo.CholmodMatrixStorage.CCS);

            double[] x = new double[A.ColumnCount];

            fixed (int* Index = cholmodA.rowIndex, Pt = cholmodA.colIndex)
            fixed (double* val = cholmodA.values, bp = cholmodb.values, xx = x)
            {
                SolveRealByLU_CCS(cholmodA.RowCount,
                                  cholmodA.ColumnCount,
                                  cholmodA.nnz,
                                  Index,    //Row Index
                                  Pt,   //Column Pointer
                                  val,
                                  xx,
                                  bp);
            }

            DenseMatrixDouble unknown = CholmodConverter.dConvertArrayToDenseMatrix(ref x, x.Length, 1);

            cholmodA = null;
            cholmodb = null;
            GC.Collect();

            return unknown;
        }


        #region Cholesky

        public void FactorizationCholesky(ref SparseMatrixDouble A)
        {
            // CholmodInfo cholmodA = CholmodConverter.Converter(ref A);
            int rowCount = A.RowCount;
            int columnCount = A.ColumnCount;
            int nnz = 0;


            int[] Ri = null;
            int[] Ci = null;
            double[] values = null;

            A.ToTriplet(out Ci, out Ri, out values, out nnz);

            fixed (int* ri = Ri, ci = Ci)
            fixed (double* val = values)
            {
                solver = CreateSolverCholeskyCHOLMOD(rowCount, rowCount, nnz, nnz, ri, ci, val);

            }
            if (solver == null) throw new Exception("Create Solver Fail");

        }

        /*
         * For Complex Calculation
         */
        public void FactorizationCholesky(ref SparseMatrixComplex A)
        {
            CholmodInfo cholmodA = CholmodConverter.cConverter(ref A, CholmodInfo.CholmodMatrixStorage.CCS);

            m = A.RowCount;
            n = A.ColumnCount;

            fixed (int* Index = cholmodA.rowIndex, Pt = cholmodA.colIndex)
            fixed (double* val = cholmodA.values)
            {
                solver = CreateSolverCholesky_CCS_Complex(cholmodA.RowCount, cholmodA.ColumnCount, cholmodA.nnz, Pt, Index, val);

            }

            if (solver == null) throw new Exception("Create Solver Fail");

        }

        private void SolveCholeskyComplex(ref double[] rightB, ref double[] unknownX)
        {
            if (solver == null)
                return;

            fixed (double* _x = unknownX, _rightB = rightB)
            {
                SolveCholeskyCHOLMOD_Complex(solver, _x, _rightB);
            }
        }

        public DenseMatrixComplex SolveByFactorizedCholesky(ref DenseMatrixComplex b)
        {
            CholmodInfo cholmodb = CholmodConverter.cConverter(ref b);

            DenseMatrixComplex result = new DenseMatrixComplex(n, 1);

            double[] x = new double[2 * n];

            SolveCholeskyComplex(ref cholmodb.values, ref x);

            for (int i = 0; i < n; i++)
            {
                result[i, 0] = new Complex(x[2 * i], x[2 * i + 1]);
            }

            x = null;
            GC.Collect();

            return result;
        }

        public void FreeCholeskySolver()
        {
            if (solver != null)
            {
                FreeSolverCholeskyCHOLMOD(solver);
            }
        }

        #endregion

        #region LU Factorize
        /*
         * For Complex Calculation
         */
        public void FactorizationLU(ref SparseMatrixComplex A)
        {
            CholmodInfo cholmodA = CholmodConverter.cConverter(ref A,
                                                              CholmodInfo.CholmodMatrixStorage.CCS);

            m = A.RowCount;
            n = A.ColumnCount;

            fixed (int* Index = cholmodA.rowIndex, Pt = cholmodA.colIndex)
            fixed (double* val = cholmodA.values)
            {
                solver = CreateSolverLUUMFPACK_CCS_Complex(cholmodA.RowCount,
                                                           cholmodA.ColumnCount,
                                                           cholmodA.nnz,
                                                           Index,
                                                           Pt,
                                                           val
                                                           );
            }


            if (solver == null) throw new Exception("Create Solver Fail");
        }

        public void FactorizationLU(ref SparseMatrixDouble A)
        {
            CholmodInfo cholmodA = CholmodConverter.ConverterDouble(ref A,
                                                              CholmodInfo.CholmodMatrixStorage.CCS);

            m = A.RowCount;
            n = A.ColumnCount;

            fixed (int* Index = cholmodA.rowIndex, Pt = cholmodA.colIndex)
            fixed (double* val = cholmodA.values)
            {
                solver = CreateSolverLUUMFPACK_CCS(cholmodA.RowCount,
                                                           cholmodA.ColumnCount,
                                                           cholmodA.nnz,
                                                           Index,
                                                           Pt,
                                                           val
                                                           );
            }


            if (solver == null) throw new Exception("Create Solver Fail");
        }

        public void FactorizationLU(ref SparseMatrixQuaternion A)
        {
            CholmodInfo cholmodA = CholmodConverter.qConverter(ref A,
                                                  CholmodInfo.CholmodMatrixStorage.CCS);

            m = A.RowCount;
            n = A.ColumnCount;

            fixed (int* Index = cholmodA.rowIndex, Pt = cholmodA.colIndex)
            fixed (double* val = cholmodA.values)
            {
                solver = CreateSolverLUUMFPACK_CCS(cholmodA.RowCount,
                                                           cholmodA.ColumnCount,
                                                           cholmodA.nnz,
                                                           Index,
                                                           Pt,
                                                           val
                                                           );
            }


            if (solver == null) throw new Exception("Create Solver Fail");

        }

        protected void SolveLU(ref double[] rightB, ref double[] unknownX)  //in and output are in Normal Complex format
        {
            if (solver == null)
                return;

            fixed (double* _x = unknownX, _rightB = rightB)
            {
                SolveLUUMFPACK(solver, _x, _rightB);
            }
        }

        public DenseMatrixQuaternion SolveByFactorizedLU(ref DenseMatrixQuaternion b)
        {
            if (b.ColumnCount > 1)
            {
                return null;
            }

            double[][] rhs = new double[4][];   //Arranage By Column
            for (int i = 0; i < 4; i++)
            {
                rhs[i] = new double[4 * b.RowCount];
            }

            double[][] x = new double[4][];
            for (int i = 0; i < 4; i++)
            {
                x[i] = new double[4 * this.n];
            }

            for (int i = 0; i < b.RowCount; i++)
            {
                Quaternion item = b[i, 0];

                Matrix4D mat = item.ToMatrix();

                rhs[0][4 * i] = mat[0, 0]; rhs[1][4 * i] = mat[0, 1]; rhs[2][4 * i] = mat[0, 2]; rhs[3][4 * i] = mat[0, 3];
                rhs[0][4 * i + 1] = mat[1, 0]; rhs[1][4 * i + 1] = mat[1, 1]; rhs[2][4 * i + 1] = mat[1, 2]; rhs[3][4 * i + 1] = mat[1, 3];
                rhs[0][4 * i + 2] = mat[2, 0]; rhs[1][4 * i + 2] = mat[2, 1]; rhs[2][4 * i + 2] = mat[2, 2]; rhs[3][4 * i + 2] = mat[2, 3];
                rhs[0][4 * i + 3] = mat[3, 0]; rhs[1][4 * i + 3] = mat[3, 1]; rhs[2][4 * i + 3] = mat[3, 2]; rhs[3][4 * i + 3] = mat[3, 3];
            }

            DenseMatrixQuaternion result = new DenseMatrixQuaternion(n, 1);

            //Solve
            for (int i = 0; i < 4; i++)
            {
                SolveLU(ref rhs[i], ref x[i]);
            }

            for (int i = 0; i < this.n; i++)
            {
                result[i, 0] = new Quaternion(x[0][4 * i + 1], x[0][4 * i + 2], x[0][4 * i + 3], x[0][4 * i]);
            }

            x = null;
            GC.Collect();

            return result;
        }

        protected void SolveLUComplex(ref double[] rightB, ref double[] unknownX)  //in and output are in Normal Complex format
        {
            if (solver == null)
                return;

            fixed (double* _x = unknownX, _rightB = rightB)
            {
                SolveLUUMFPACK_Complex(solver, _x, _rightB);
            }
        }

        public DenseMatrixComplex SolveByFactorizedLU(ref DenseMatrixComplex b)
        {
            CholmodInfo cholmodb = CholmodConverter.cConverter(ref b);

            DenseMatrixComplex result = new DenseMatrixComplex(n, 1);

            double[] x = new double[2 * n];

            SolveLUComplex(ref cholmodb.values, ref x);

            for (int i = 0; i < n; i++)
            {
                result[i, 0] = new Complex(x[2 * i], x[2 * i + 1]);
            }

            x = null;
            GC.Collect();

            return result;
        }

        public DenseMatrixDouble SolveByFactorizedLU(ref DenseMatrixDouble b)
        {
            CholmodInfo cholmodb = CholmodConverter.ConvertDouble(ref b);

            DenseMatrixDouble result = new DenseMatrixDouble(n, 1);

            double[] x = new double[n];

            SolveLU(ref cholmodb.values, ref x);

            for (int i = 0; i < n; i++)
            {
                result[i, 0] = x[i];
            }

            x = null;
            GC.Collect();

            return result;
        }

        public void FreeSolverLU()
        {
            if (solver != null)
            {
                FreeSolverLUUMFPACK(solver);
            }
        }
        public void FreeSolverLUComplex()
        {
            if (solver != null)
            {
                FreeSolverLUUMFPACK_Complex(solver);
            }
        }

        #endregion

        #region QR

        public void FactorizationQR(ref SparseMatrixDouble A)
        {
            // CholmodInfo cholmodA = CholmodConverter.Converter(ref A);
            int rowCount = A.RowCount;
            int columnCount = A.ColumnCount;
            int nnz = 0;
            this.m = rowCount;
            this.n = columnCount;

            int[] Ri = null;
            int[] Ci = null;
            double[] values = null;

            A.ToCCS(out Ci, out Ri, out values, out nnz);

            fixed (int* ri = Ri, ci = Ci)
            fixed (double* val = values)
            {
                solver = CreateSolverQRSuiteSparseQR_CCS(rowCount, columnCount, nnz, ri, ci, val);

            }
            if (solver == null) throw new Exception("Create Solver Fail");
        }

        protected void SolveLeastNormByFractorizedQR(ref double[] rightB, ref double[] unknownX)
        {
            if (solver == null)
                return;

            fixed (double* _x = unknownX, _rightB = rightB)
            {
                SolveLeastNormalByQR(solver, _x, _rightB);
            }
        }

        protected void SolveLeastSqureByFractorizedQR(ref double[] rightB, ref double[] unknownX)
        {
            if (solver == null)
                return;

            fixed (double* _x = unknownX, _rightB = rightB)
            {
                SolveLeastSqureByQR(solver, _x, _rightB);
            }
        }

        public DenseMatrixDouble SolveByFractorizedQRLeastNorm(ref DenseMatrixDouble b)
        {
            CholmodInfo cholmodb = CholmodConverter.ConvertDouble(ref b);

            DenseMatrixDouble result = new DenseMatrixDouble(m, 1);

            double[] x = new double[m];

            SolveLeastNormByFractorizedQR(ref cholmodb.values, ref x);

            for (int i = 0; i < m; i++)
            {
                result[i, 0] = x[i];
            }

            x = null;
            GC.Collect();

            return result;
        }

        public DenseMatrixDouble SolveByFractorizedQRLeastSqure(ref DenseMatrixDouble b)
        {
            CholmodInfo cholmodb = CholmodConverter.ConvertDouble(ref b);

            DenseMatrixDouble result = new DenseMatrixDouble(n, 1);

            double[] x = new double[n];

            SolveLeastSqureByFractorizedQR(ref cholmodb.values, ref x);

            for (int i = 0; i < n; i++)
            {
                result[i, 0] = x[i];
            }

            x = null;
            GC.Collect();

            return result;
        }

        public void FreeFractorizedQR()
        {
            if (solver != null)
            {
                FreeSolverQRSuiteSparseQR(solver);
            }
        }


        #endregion

        #region Util



        #endregion

        public void FreeSolver()
        {
            if (solver != null)
            {

            }
        }


    }
}
