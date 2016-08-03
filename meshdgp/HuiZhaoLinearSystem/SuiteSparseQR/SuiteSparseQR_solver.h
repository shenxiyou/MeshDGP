#include <stdlib.h>
#include <stdio.h>
#include "cholmod.h"
#include "SuiteSparseQR_C.h"
#define DllExport  extern "C" __declspec( dllexport )

typedef struct QRsolver{
	SuiteSparseQR_C_factorization *QR;

	int rowCount;
	int columnCount;
	int nnz;

	cholmod_common c;

}QRSolver;

DllExport void* CreateSolverQRSuiteSparseQR(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int numberOfEntries, int *Ti, int *Tj, double *Tx);
DllExport void FreeSolverQRSuiteSparseQR(void *solver);
DllExport void* CreateSolverQRSuiteSparseQR_CCS(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int *rowIndex, int *colPtr, double *values);
DllExport void* CreateSolverQRSuiteSparseQR_CRS(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int *rowPtr, int *colIndex, double *values);
DllExport void SolveLeastNormalByQR(void *solver, double *X, double *b);
DllExport void SolveLeastSqureByQR(void *solver, double *X, double *b);

DllExport void SolveRealByQR(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int *Ti, int *Tj, double *Tx, double *X, double *b);
DllExport void SolveRealByQR_CRS(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int *rowIndex, int *colPtr, double *values, double *X, double *b);
DllExport void SolveRealByQR_CCS(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int *rowPtr, int *colIndex, double *values, double *X, double *b);

//Complex
DllExport void SolveRealByQR_CCS_Complex(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int *rowPtr, int *colIndex, double *values, double *X, double *b);
DllExport void SolveRealByQR_CRS_Complex(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int *rowIndex, int *colPtr, double *values, double *X, double *b);
