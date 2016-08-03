#include "taucs_lu.h"

//LU Solver
DllExport void * CreateSolverLUTAUCS(int n, int nnz, int *rowIndex, int *colIndex, double *value)
{
	int rc;
	int currCol = -1;
	double mem = 0;
	int i = 0;
	struct LUSolver * s = (struct LUSolver*)malloc(sizeof(struct LUSolver));
	
	if (s == NULL) return NULL;
	s->rows = n;
	s->matrix = NULL;

	// 创建矩阵
	s->matrix = taucs_ccs_create(n, n, nnz, TAUCS_DOUBLE|TAUCS_LOWER|TAUCS_SYMMETRIC);

	

	s->matrix->rowind = (int*)malloc(sizeof(int)*nnz);
	s->matrix->colptr = (int*)malloc(sizeof(int)*(n+1));
	s->matrix->values.d = (double*)malloc(sizeof(int)*nnz);
	s->invperm = (int*)malloc(sizeof(int)*n);
	s->perm = (int*)malloc(sizeof(int)*n);
	
	for(i = 0;i<nnz;i++){
		s->matrix->rowind[i] = rowIndex[i];
		s->matrix->values.d[i] = value[i];
	}

	for(i = 0;i<n+1;i++){
		s->matrix->colptr[i] = colIndex[i];
	}

	if (s->matrix == NULL) return NULL;

	//分解矩阵
	taucs_ccs_order(s->matrix, &(s->perm), &(s->invperm), "colamd");
	s->L = taucs_io_create_multifile("tmp_lu_ooc");

	//获得可用内存大小
	mem = taucs_available_memory_size();
	rc = taucs_ooc_factor_lu(s->matrix, s->perm, s->L, mem);

	 if(rc == -1) {
		 return NULL;};

	return s;
}

DllExport void FreeSolverLUTAUCS(void *L){
	struct LUSolver *lu = (struct LUSolver*)L;
	taucs_io_delete(lu->L);
	taucs_dccs_free(lu->matrix);
}

DllExport int SolveLUTAUCS(void *L,double *b,double *x){
	struct LUSolver *Lu = (struct LUSolver*)L;
	taucs_dooc_solve_lu(Lu->L,x,b);
	taucs_dccs_free(Lu->matrix);
	return 0;
}
