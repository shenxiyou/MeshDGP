
#include <stdlib.h>
#include <stdio.h>
#include "taucs.h"
#define DllExport __declspec( dllexport )


struct CGSolver {
	int n;
	taucs_ccs_matrix * matrix;

};

DllExport void * CreateSolverCGTAUCS(int n, int nnz, int *rowIndex, int *colIndex, double *value);
DllExport int FreeSolverCGTAUCS(void * sp);
DllExport int SolveCGTAUCS(void * sp, double *x, double *b);
