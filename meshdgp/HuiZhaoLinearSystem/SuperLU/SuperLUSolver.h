#include <stdio.h>
#include "slu_ddefs.h"
#define DllExport  extern "C" __declspec( dllexport )

typedef struct superLUsolver{
	int_t *xa;
	int_t *asub;
	void *a;

	int m;
	int n;
	int nnz; //非 0 项个数
	SuperMatrix A;
	SuperMatrix B;
	SuperMatrix L;
	SuperMatrix U;
	SuperLUStat_t state;
	superlu_options_t *opinion;

	int *perc;
	int *perr;
	int info;
	trans_t transt;

}SuperLUSolver;


DllExport void* CreateSolverLUSuperLU(int numberOfRows,int numberOfColumns,int numberOfNoneZero,int *rowIndex,int *columnIndex,double *values);

DllExport void SolveLUSuperLU(void *solver,double *x,double *b);

DllExport void FreeSolverLUSuperLU(void *solver);

void Factorization(superlu_options_t *options, SuperMatrix *A, int *perm_c, int *perm_r,
      SuperMatrix *L, SuperMatrix *U, 
	  SuperLUStat_t *stat, int *info,trans_t &trans);