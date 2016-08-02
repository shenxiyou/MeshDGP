#include <stdlib.h>
#include <stdio.h>
#include "taucs.h"
#define DllExport __declspec( dllexport )

struct SymbolicSolver {
	int n;
	taucs_ccs_matrix * matrix;
	void * factorization;
	int  * perm;
	int  * invperm;
	double *tmp_b;
	double *tmp_x;
};

DllExport void * CreateSolverSymbolicTAUCS(int n, int nnz, int *rowIndex, int *colIndex, double *value);
DllExport void FreeSolverSymbolicTAUCS(void * sp);
DllExport int NumericFactor(void *sp);
DllExport void FreeNumericFactor(void *sp);
DllExport int NumericSolve(void * sp, double *x, double *b);