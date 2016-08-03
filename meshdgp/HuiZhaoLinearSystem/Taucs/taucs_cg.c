#include <memory.h>
#include "taucs_cg.h"
//
// 共轭梯度迭代解法
//

DllExport void * CreateSolverCGTAUCS
(int n, int nnz, int *rowIndex, int *colIndex, double *value)
{
	int currCol = -1;
	char* options[] = {"taucs.factor.LLT=true", NULL};
	struct CGSolver * s = (struct CGSolver*) malloc(sizeof(struct CGSolver));
	if (s == NULL) return NULL;
	s->n = n;
	s->matrix = NULL;
	

	//创建矩阵
	s->matrix = taucs_ccs_create(n, n, nnz, TAUCS_DOUBLE|TAUCS_LOWER|TAUCS_SYMMETRIC);
	if (s->matrix == NULL) return NULL;

	//拷贝矩阵数据到矩阵中
	memcpy(s->matrix->rowind, rowIndex, sizeof(int) * nnz);
	memcpy(s->matrix->values.d, value, sizeof(double) * nnz);
	memcpy(s->matrix->colptr, colIndex, sizeof(int) * (n+1));


	return s;
}


DllExport int SolveCGTAUCS(void * sp, double *x, double *b) 
{
	int rc;
	char* options [] = {"taucs.solve.cg=true", NULL};
	struct CGSolver * s = (struct CGSolver *) sp;

	//设置迭代次数
	int itermax = 100;
	double max_error = 0;


	if (s->matrix == NULL) return -1;

	//用共轭梯度的方法迭代的解方程
	rc = taucs_conjugate_gradients(s->matrix,0,0,x,b,itermax,max_error);
	if (rc != TAUCS_SUCCESS) return rc;

	return 0;
}

DllExport int FreeSolverCGTAUCS(void * sp) {
	struct CGSolver * s = (struct CGSolver *)sp;
	int rc = 0;

	if (sp == NULL)
	{
		return 0;
	}

	if (s->matrix != NULL) {
		taucs_ccs_free(s->matrix);
		s->matrix = NULL;
	}
	return rc;
}

