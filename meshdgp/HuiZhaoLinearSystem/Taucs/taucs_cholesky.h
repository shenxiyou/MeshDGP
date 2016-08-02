#include <stdlib.h>
#include <stdio.h>
#include "taucs.h"

#define DllExport __declspec( dllexport )


struct Solver {
	int n;
	taucs_ccs_matrix * matrix;
	void * factorization;
};

DllExport void * CreateSolverCholeskyTAUCS(int n, int nnz, int *rowIndex, int *colIndex, double *value);
DllExport int FreeSolverCholeskyTAUCS(void * sp);
DllExport int SolveCholeskyTAUCS(void * sp, double *x, double *b);
DllExport double SolveEx(void * sp, double *x, int xIndex, double *b, int bIndex);

