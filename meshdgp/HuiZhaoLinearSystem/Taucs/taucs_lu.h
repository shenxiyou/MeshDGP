
#include <stdlib.h>
#include <stdio.h>
#include "taucs.h"
#define DllExport __declspec( dllexport )

struct LUSolver {
	int rows;
	taucs_ccs_matrix *matrix;
	taucs_io_handle *L;
	int* perm;
	int* invperm;
	double mem;
};

DllExport void* CreateSolverLUTAUCS(int numberOfRow,int nnz,int *Ti,int *Tj,double *Tx);
DllExport int SolveLUTAUCS(void * solver, double *x, double *b);
DllExport void FreeSolverLUTAUCS(void *a);

