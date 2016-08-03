#include <memory.h>
#include "taucs_cholesky.h"

//
//Cholesky 分解
//
DllExport void * CreateSolverCholeskyTAUCS
	(int n, int nnz, int *rowIndex, int *colIndex, double *value)
{
	int rc;
	int currCol = -1;
	char* options[] = {"taucs.factor.LLT=true", NULL};

	struct Solver * s = (struct Solver*) malloc(sizeof(struct Solver));
	if (s == NULL) return NULL;
	s->n = n;
	s->matrix = NULL;
	s->factorization = NULL;

	 //打开记录文件
	taucs_logfile("c:/log.txt");
	
	//创建矩阵
	s->matrix = taucs_ccs_create(n, n, nnz, TAUCS_DOUBLE|TAUCS_LOWER|TAUCS_SYMMETRIC);
	if (s->matrix == NULL) return NULL;
	
	//拷贝项目到矩阵
	memcpy(s->matrix->rowind, rowIndex, sizeof(int) * nnz);
	memcpy(s->matrix->values.d, value, sizeof(double) * nnz);
	memcpy(s->matrix->colptr, colIndex, sizeof(int) * (n+1));
	
	//分解矩阵	
	rc = taucs_linsolve(s->matrix, &(s->factorization) , 0, NULL, NULL, options, NULL);
	if (rc != TAUCS_SUCCESS) { FreeSolverCholeskyTAUCS(s); return NULL;};
	
	taucs_logfile("none");

	return s;
}

DllExport int SolveCholeskyTAUCS(void * sp, double *x, double *b) {
	int rc;
	char* options [] = {"taucs.factor=false", NULL};

	struct Solver * s = (struct Solver *) sp;
	void * F = NULL;

	if (s->matrix == NULL || s->factorization == NULL) return -1;

	//解方程
	rc = taucs_linsolve(s->matrix, &s->factorization, 1, x, b, options, NULL);
	if (rc != TAUCS_SUCCESS) return rc;

	return 0;
}

DllExport double SolveEx(void * sp, double *x, int xIndex, double *b, int bIndex) {
	int rc = -1;
	char* options [] = {"taucs.factor=false", NULL};
	struct Solver * s = (struct Solver *) sp;
	double time = taucs_ctime();

	rc = taucs_linsolve(s->matrix, &s->factorization, 1, x + xIndex, b + bIndex, options, NULL);
	time = taucs_ctime() - time;

	//return time;
	if (rc != TAUCS_SUCCESS) return rc;
	return 0;
}

DllExport int FreeSolverCholeskyTAUCS(void * sp) {
	struct Solver * s = (struct Solver *)sp;
	int rc = 0;

	if (sp == NULL)
	{
		return 0;
	}

	if (s->matrix != NULL) {
		taucs_ccs_free(s->matrix);
		s->matrix = NULL;
	}
	if (s->factorization != NULL) {
		rc = taucs_linsolve(NULL, &s->factorization, 0, NULL, NULL, NULL, NULL);
		s->factorization = NULL;
	}
	return rc;
}

