using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GraphicResearchHuiZhao
{
    public unsafe partial class LinearSystemLib
    {
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

        #endregion

        #region import UMFPACK functions
        [DllImport("UMFPack.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void FreeSolverLUUMFPACK(void* a);
        [DllImport("UMFPack.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void* CreateSolverLUUMFPACK(int numberOfRows, int numberOfNoneZero, int* RowIndex, int* ColumnIndex, double* Value);
        [DllImport("UMFPack.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe int SolveLUUMFPACK(void* solver, double* x, double* b);

        #endregion

        #region import taucs functions

        [DllImport("taucs.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void* CreateSolverCholeskyTAUCS(int numberOfRows, int numberOfNoneZeroEntries, int* rowIndex, int* colIndex, double* value);
        [DllImport("taucs.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe int SolveCholeskyTAUCS(void* solver, double* x, double* b);
        [DllImport("taucs.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe int FreeSolverCholeskyTAUCS(void* solver);

        [DllImport("taucs.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        static extern unsafe void* CreateSolverCGTAUCS(int numberOfRows, int numberOfNoneZeroEntries, int* rowIndex, int* colIndex, double* value);
        [DllImport("taucs.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        static extern unsafe int SolveCGTAUCS(void* solver, double* x, double* b);
        [DllImport("taucs.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        static extern unsafe int FreeSolverCGTAUCS(void* solver);

        [DllImport("taucs.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void* CreateSolverLUTAUCS(int numberOfRows, int numberOfNoneZeroEntries, int* rowIndex, int* colIndex, double* value);
        [DllImport("taucs.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void FreeSolverLUTAUCS(void* solver);
        [DllImport("taucs.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe void SolveLUTAUCS(void* solver, double* x, double* b);

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
        #endregion
    }
}
