#include <stdlib.h>
#include <stdio.h>


#define DllExport  extern "C" __declspec( dllexport )

enum{
	MartixAllZeroError
};


typedef struct umfpacksolver
{
	int *Ap;
	long ApSize;

	int *Ai;
	long AiSize;

	double *Ax;
	double *Az;
	long AxSize;

	double *b;
	long n;
	long m;
	void *Numeric;
}UmfpackSolver;


DllExport void* CreateSolverLUUMFPACK(int numberOfRow, int nnz, int *Ti, int *Tj, double *Tx);
DllExport int SolveLUUMFPACK(void * solver, double *x, double *b);

DllExport void SolveRealByLU(int numberOfRow, int numberOfColumn, int nnz, int *Ti, int *Tj, double *Tx, double *X, double *b);
DllExport void SolveRealByLU_CCS(int numberOfRow, int numberOfColumn, int nnz, int *rowIndices, int *colPtr, double *values, double *X, double *b);

DllExport void* CreateSolverLUUMFPACK_CCS_Complex(int numberOfRow, int numberOfColumn, int nnz, int *rowIndices, int *colPtr, double *Values);
DllExport void* CreateSolverLUUMFPACK_CCS(int numberOfRow, int numberOfColumn, int nnz, int *rowIndices, int *colPtr, double *Values);
DllExport int SolveLUUMFPACK_Complex(void * solver, double *x, double *b);
DllExport void FreeSolverLUUMFPACK_Complex(void *a);



