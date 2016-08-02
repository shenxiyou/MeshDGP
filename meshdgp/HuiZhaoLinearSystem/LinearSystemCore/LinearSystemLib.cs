using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
 

namespace GraphicResearchHuiZhao
{
   

    public  unsafe partial  class LinearSystemLib
    {
       
        
        
        public EnumSolver SolverType = EnumSolver.UmfpackLU;

        public LinearSystemLib()
        {
        }

        public LinearSystemLib(EnumSolver type)
        {
            this.SolverType = type;
        }

        public EnumSolver SolverTypeUsing
        {
            get
            {
                return SolverType;
            }
            set
            {
                FreeSolver();
                SolverType = value;
            }
        }

        private void* solver = null;

        public void Factorization(SparseMatrix A)
        {
            TripletArraryData data = ConvertToTripletArrayData(A);

            int rowCount = A.Rows.Count;
            int columnCount = A.Columns.Count;
            int nnz = data.nnz;
            
            fixed (int* ri = data.rowIndex, ci = data.colIndex)
            fixed (double* val = data.values)
            {
                switch (SolverType)
                {
                    
                    case EnumSolver.UmfpackLU:
                        solver = CreateSolverLUUMFPACK(rowCount,  nnz, ri, ci, val);
                        break;
                    case EnumSolver.SuperLULU:
                        solver = CreateSolverLUSuperLU(rowCount, rowCount,  nnz, ri, ci, val);
                        break;
                    case EnumSolver.CholmodCholesky:
                        solver = CreateSolverCholeskyCHOLMOD(rowCount, rowCount,  nnz, nnz, ri, ci, val);
                        break;
                    case EnumSolver.SPQRLeastNormal:
                        solver = CreateSolverQRSuiteSparseQR(rowCount, columnCount,  nnz,  nnz, ri, ci, val);
                        break;
                    case EnumSolver.SPQRLeastSqure:
                        solver = CreateSolverQRSuiteSparseQR(rowCount, columnCount,  nnz,  nnz, ri, ci, val);
                        break;
                }

            }
            if (solver == null) throw new Exception("Create Solver Fail");
        }

        public void SolveLinerSystem(ref double[] rightB, ref double[] unknownX)
        {

            if (solver == null)
                return;

            fixed (double* _x = unknownX, _rightB = rightB)
            {
                switch (SolverType)
                {
                    case EnumSolver.TaucsCholesky:
                        SolveCholeskyTAUCS(solver, _x, _rightB);
                        break;
                    case EnumSolver.TaucsCG:
                        SolveCGTAUCS(solver, _x, _rightB);
                        break;
                    case EnumSolver.TaucsLU:
                        SolveLUTAUCS(solver, _x, _rightB);
                        break;
                    case EnumSolver.UmfpackLU:
                        SolveLUUMFPACK(solver, _x, _rightB);
                        break;
                    case EnumSolver.SuperLULU:
                        SolveLUSuperLU(solver, _x, _rightB);
                        break;
                    case EnumSolver.CholmodCholesky:
                        SolveCholeskyCHOLMOD(solver, _x, _rightB);
                        break;
                    case EnumSolver.SPQRLeastNormal:
                        SolveLeastNormalByQR(solver, _x, _rightB);
                        break;
                    case EnumSolver.SPQRLeastSqure:
                        SolveLeastSqureByQR(solver, _x, _rightB);
                        break;
                }

            }
        }
        public void FreeSolver()
        {
            if (solver != null)
            {
                switch (SolverType)
                {
                    case EnumSolver.TaucsCholesky:
                        FreeSolverCholeskyTAUCS(solver);
                        break;
                    case EnumSolver.TaucsCG:
                        FreeSolverCGTAUCS(solver);
                        break;
                    case EnumSolver.TaucsLU:
                        FreeSolverLUTAUCS(solver);
                        break;
                    case EnumSolver.UmfpackLU:
                        FreeSolverLUUMFPACK(solver);
                        break;
                    case EnumSolver.SuperLULU:
                        FreeSolverLUSuperLU(solver);
                        break;
                    case EnumSolver.CholmodCholesky:
                        FreeSolverCholeskyCHOLMOD(solver);
                        break;
                    case EnumSolver.SPQRLeastNormal:
                        FreeSolverQRSuiteSparseQR(solver);
                        break;
                    case EnumSolver.SPQRLeastSqure:
                        //FreeSolverQRSuiteSparseQR(solver);
                        break;

                }

                solver = null;

                System.GC.Collect();
                
            }
        }


        public TripletArraryData ConvertToTripletArrayData(SparseMatrix A)
        {
            TripletArraryData data = new TripletArraryData();
            switch (SolverType)
            {

                case EnumSolver.UmfpackLU:
                    data = ConvertToDataUnSym(A);
                    break;
                case EnumSolver.SuperLULU:
                    data = ConvertToDataUnSym(A);
                    break;
                case EnumSolver.CholmodCholesky:
                    data = ConvertToDataSymU(A);
                    break;
                case EnumSolver.SPQRLeastNormal:
                    data = ConvertToDataUnSym(A);
                    break;
                case EnumSolver.SPQRLeastSqure:
                    data = ConvertToDataUnSym(A);
                    break;
            }

            return data;
        }

        public TripletArraryData ConvertToDataUnSym(SparseMatrix A)
        {

            TripletArraryData data = new TripletArraryData();
            int unSym_nnz = 0;
      

            foreach (List<SparseMatrix.Element> col in A.Columns)
            {
                foreach (SparseMatrix.Element e in col)
                { 
                    if (e.value != 0)
                    {
                        unSym_nnz++;
                    }
                }
            }


           

            data.rowIndex = new int[unSym_nnz];
            data.colIndex = new int[unSym_nnz];
            data.values = new double[unSym_nnz];
            data.nnz = unSym_nnz;
            // copy values to arrays
            int rowCur = 0;
            data.colIndex[0] = 0;
            foreach (List<SparseMatrix.Element> col in A.Columns)
            {
                foreach (SparseMatrix.Element e in col)
                {
                    if (e.value != 0)
                    {
                        data.rowIndex[rowCur] = e.i;
                        data.colIndex[rowCur] = e.j;
                        data.values[rowCur] = e.value;
                        rowCur++;
                    }
                }

            }



            return data;
        }


        public TripletArraryData ConvertToDataSymU(SparseMatrix A)
        {
            TripletArraryData data = new TripletArraryData();
            // get number of non-zero elements
            int rowCount = A.Rows.Count;
            int columnCount = A.Columns.Count;
            
           
            int sym_nnz = 0;

            foreach (List<SparseMatrix.Element> col in A.Columns)
            {
                foreach (SparseMatrix.Element e in col)
                {
                    if (e.i >= e.j)
                    {
                        
                        if (e.value != 0)
                        {
                            sym_nnz++;
                        }
                    }
                    
                }
            }

            #region Chomold
            data.values = new double[sym_nnz];
            data.rowIndex = new int[sym_nnz];
            data.colIndex = new int[sym_nnz];
            data.nnz = sym_nnz;

            int rowCur = 0;

            foreach (List<SparseMatrix.Element> col in A.Columns)
            {
                foreach (SparseMatrix.Element e in col)
                {
                    if (e.i < e.j || e.value == 0) continue;

                    data.rowIndex[rowCur] = e.i;
                    data.colIndex[rowCur] = e.j;
                    data.values[rowCur] = e.value;
                    rowCur++;
                }

            }
            #endregion

            return data;
        }










        //public void Factorization(SparseMatrix A)
        //{
        //    // get number of non-zero elements
        //    int rowCount = A.Rows.Count;
        //    int columnCount = A.Columns.Count;
        //    int nnz = 0;
        //    int unSym_nnz = 0;
        //    int sym_nnz = 0;

        //    foreach (List<SparseMatrix.Element> col in A.Columns)
        //    {
        //        foreach (SparseMatrix.Element e in col)
        //        {
        //            if (e.i >= e.j)
        //            {
        //                nnz++;
        //                if (e.value != 0)
        //                {
        //                    sym_nnz++;
        //                }
        //            }
        //            if (e.value != 0)
        //            {
        //                unSym_nnz++;
        //            }
        //        }
        //    }



        //    double[] values = null;
        //    int[] rowIndex = null;
        //    int[] colIndex = null;

        //    if (SolverType == EnumSolver.TaucsCG ||
        //        SolverType == EnumSolver.TaucsCholesky ||
        //        SolverType == EnumSolver.TaucsLU)
        //    {
        //        #region Tacus Field
        //        // create temp arrays

        //        rowIndex = new int[nnz];
        //        colIndex = new int[rowCount + 1];
        //        values = new double[nnz];

        //        // copy values to arrays
        //        int rowCur = 0;
        //        int colCur = 0;
        //        colIndex[0] = 0;
        //        foreach (List<SparseMatrix.Element> col in A.Columns)
        //        {
        //            foreach (SparseMatrix.Element e in col)
        //            {
        //                if (e.i < e.j) continue;
        //                rowIndex[rowCur] = e.i;
        //                values[rowCur] = e.value;
        //                rowCur++;
        //            }
        //            colIndex[++colCur] = rowCur;
        //        }



        //        #endregion
        //    }
        //    else if (SolverType == EnumSolver.UmfpackLU ||
        //             SolverType == EnumSolver.SuperLULU ||
        //             SolverType == EnumSolver.SPQRLeastNormal ||
        //             SolverType == EnumSolver.SPQRLeastSqure
        //            )
        //    {
        //        #region UMPACK Field
        //        rowIndex = new int[unSym_nnz];
        //        colIndex = new int[unSym_nnz];
        //        values = new double[unSym_nnz];

        //        // copy values to arrays
        //        int rowCur = 0;
        //        colIndex[0] = 0;
        //        foreach (List<SparseMatrix.Element> col in A.Columns)
        //        {
        //            foreach (SparseMatrix.Element e in col)
        //            {
        //                if (e.value != 0)
        //                {
        //                    rowIndex[rowCur] = e.i;
        //                    colIndex[rowCur] = e.j;
        //                    values[rowCur] = e.value;
        //                    rowCur++;
        //                }
        //            }

        //        }

        //        #endregion
        //    }
        //    else if (SolverType == EnumSolver.CholmodCholesky)
        //    {
        //        #region Chomold
        //        values = new double[sym_nnz];
        //        rowIndex = new int[sym_nnz];
        //        colIndex = new int[sym_nnz];

        //        int rowCur = 0;

        //        foreach (List<SparseMatrix.Element> col in A.Columns)
        //        {
        //            foreach (SparseMatrix.Element e in col)
        //            {
        //                if (e.i < e.j || e.value == 0) continue;

        //                rowIndex[rowCur] = e.i;
        //                colIndex[rowCur] = e.j;
        //                values[rowCur] = e.value;
        //                rowCur++;
        //            }

        //        }
        //        #endregion
        //    }


        //    fixed (int* ri = rowIndex, ci = colIndex)
        //    fixed (double* val = values)
        //    {
        //        switch (SolverType)
        //        {
        //            case EnumSolver.TaucsCholesky:
        //                solver = CreateSolverCholeskyTAUCS(rowCount, nnz, ri, ci, val);
        //                break;
        //            case EnumSolver.TaucsCG:
        //                solver = CreateSolverCGTAUCS(rowCount, nnz, ri, ci, val);
        //                break;
        //            case EnumSolver.TaucsLU:
        //                solver = CreateSolverLUTAUCS(rowCount, nnz, ri, ci, val);
        //                break;
        //            case EnumSolver.UmfpackLU:
        //                solver = CreateSolverLUUMFPACK(rowCount, unSym_nnz, ri, ci, val);
        //                break;
        //            case EnumSolver.SuperLULU:
        //                solver = CreateSolverLUSuperLU(rowCount, rowCount, unSym_nnz, ri, ci, val);
        //                break;
        //            case EnumSolver.CholmodCholesky:
        //                solver = CreateSolverCholeskyCHOLMOD(rowCount, rowCount, sym_nnz, nnz, ri, ci, val);
        //                break;
        //            case EnumSolver.SPQRLeastNormal:
        //                solver = CreateSolverQRSuiteSparseQR(rowCount, columnCount, unSym_nnz, unSym_nnz, ri, ci, val);
        //                break;
        //            case EnumSolver.SPQRLeastSqure:
        //                solver = CreateSolverQRSuiteSparseQR(rowCount, columnCount, unSym_nnz, unSym_nnz, ri, ci, val);
        //                break;
        //        }

        //    }
        //    if (solver == null) throw new Exception("Create Solver Fail");
        //}
    }
}
