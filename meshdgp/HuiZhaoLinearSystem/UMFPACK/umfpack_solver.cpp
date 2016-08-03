#include "umfpack_solver.h"
#include "umfpack.h"
#include <stdlib.h>
#include <stdio.h>
#include <string.h>

DllExport void* CreateSolverLUUMFPACK(int numberOfRows, int numberOfNoneZero, int *Ti, int *Tj, double *Tx)
{
	//创建 Compressed Row Storage 存储结构
	int *Ai = (int*)malloc(sizeof(int)*(numberOfNoneZero));
	int *Ap = (int*)malloc(sizeof(int)*(numberOfRows + 1));
	double *Ax = (double*)malloc(sizeof(double)*(numberOfNoneZero));

	//转换 triplet 到 CRS
	int staus = umfpack_di_triplet_to_col(numberOfRows, numberOfRows, numberOfNoneZero, Ti, Tj, Tx, Ap, Ai, Ax, NULL);
	if (staus != UMFPACK_OK){
		return NULL;
	}

	//创建 solver
	UmfpackSolver *umpsolver = (UmfpackSolver*)malloc(sizeof(UmfpackSolver));

	//设置 solver
	umpsolver->Ai = Ai;
	umpsolver->Ap = Ap;
	umpsolver->Ax = Ax;
	umpsolver->AiSize = umpsolver->AxSize = numberOfNoneZero;
	umpsolver->ApSize = numberOfRows + 1;
	umpsolver->n = numberOfRows;
	umpsolver->m = numberOfRows;

	//因子化
	void *Symbolic, *Numeric;
	(void)umfpack_di_symbolic(numberOfRows, numberOfRows, Ap, Ai, Ax, &Symbolic, NULL, NULL);
	(void)umfpack_di_numeric(Ap, Ai, Ax, Symbolic, &Numeric, NULL, NULL);
	umfpack_di_free_symbolic(&Symbolic);
	umpsolver->Numeric = Numeric;

	return umpsolver;
};

DllExport void FreeSolverLUUMFPACK(void *a)
{
	UmfpackSolver *item = (UmfpackSolver*)a;
	void *Numeric = item->Numeric;
	umfpack_di_free_numeric(&Numeric);
	item->Numeric = NULL;
	free(item->Ai);
	item->Ai = NULL;
	free(item->Ap);
	item->Ap = NULL;
	free(item->Ax);
	item->Ax = NULL;
	free(item);
	a = NULL;
}

DllExport int SolveLUUMFPACK(void * sp, double *x, double *b)
{
	UmfpackSolver *umfSolver = (UmfpackSolver*)sp;

	if (umfSolver->Ai == NULL || umfSolver->Ap == NULL || umfSolver->Ax == NULL) return -1;

	int *Ap = umfSolver->Ap;
	int *Ai = umfSolver->Ai;
	double *Ax = umfSolver->Ax;
	int n = umfSolver->n;
	int m = umfSolver->m;
	double *null = (double *)NULL;

	//解方程 Ax = b
	void *Numeric = umfSolver->Numeric;
	(void)umfpack_di_solve(UMFPACK_A, Ap, Ai, Ax, x, b, Numeric, null, null);

	return 0;
}

void SolveRealByLU(int numberOfRows, int numberOfColumn, int nnz, int *Ti, int *Tj, double *Tx, double *X, double *b)
{
	//创建 Compressed Row Storage 存储结构
	int *Ai = (int*)malloc(sizeof(int)*(nnz));
	int *Ap = (int*)malloc(sizeof(int)*(numberOfRows + 1));
	double *Ax = (double*)malloc(sizeof(double)*(nnz));

	//转换 triplet 到 CRS
	int staus = umfpack_di_triplet_to_col(numberOfRows, numberOfColumn, nnz, Ti, Tj, Tx, Ap, Ai, Ax, NULL);
	if (staus != UMFPACK_OK){
		return;
	}

	//因子化
	void *Symbolic, *Numeric;
	umfpack_di_symbolic(numberOfRows, numberOfColumn, Ap, Ai, Ax, &Symbolic, NULL, NULL);
	umfpack_di_numeric(Ap, Ai, Ax, Symbolic, &Numeric, NULL, NULL);
	umfpack_di_solve(UMFPACK_A, Ap, Ai, Ax, X, b, Numeric, NULL, NULL);

	umfpack_di_free_symbolic(&Symbolic);
	umfpack_di_free_numeric(&Numeric);

	if (Ai != NULL)
		free(Ai);
	if (Ap != NULL)
		free(Ap);
	if (Ax != NULL)
		free(Ax);
}

void SolveRealByLU_CCS(int numberOfRow, int numberOfColumn, int nnz, int *rowIndices, int *colPtr, double *values, double *X, double *b)
{

	int *Ai = (int*)malloc(sizeof(int)*(nnz));
	int *Ap = (int*)malloc(sizeof(int)*(numberOfColumn + 1));
	double *Ax = (double*)malloc(sizeof(double)*(nnz));

	memcpy(Ax, values, sizeof(double)*nnz);
	memcpy(Ap, colPtr, sizeof(int)*(numberOfColumn + 1));
	memcpy(Ai, rowIndices, sizeof(int)*nnz);

	void *Symbolic, *Numeric;
	(void)umfpack_di_symbolic(numberOfRow, numberOfColumn, Ap, Ai, Ax, &Symbolic, NULL, NULL);
	(void)umfpack_di_numeric(Ap, Ai, Ax, Symbolic, &Numeric, NULL, NULL);
	umfpack_di_solve(UMFPACK_A, Ap, Ai, Ax, X, b, Numeric, NULL, NULL);

	umfpack_di_free_numeric(&Numeric);
	umfpack_di_free_symbolic(&Symbolic);

	free(Ai);
	free(Ap);
	free(Ax);
}

DllExport void* CreateSolverLUUMFPACK_CCS_Complex(int numberOfRow, int numberOfColumn, int nnz, int *rowIndices, int *columnPtr, double *Values)
{
	int *Ai = (int*)malloc(sizeof(int)*(nnz));
	int *Ap = (int*)malloc(sizeof(int)*(numberOfColumn + 1));
	double *Ax = (double*)malloc(sizeof(double)*(nnz));
	double *Az = (double*)malloc(sizeof(double)*(nnz));

	for (size_t i = 0; i < nnz; i++)
	{
		Ax[i] = Values[2 * i];
		Az[i] = Values[2 * i + 1];
	}

	memcpy(Ap, columnPtr, sizeof(int)*(numberOfColumn + 1));
	memcpy(Ai, rowIndices, sizeof(int)*nnz);

	void *Symbolic, *Numeric;
	(void)umfpack_zi_symbolic(numberOfRow, numberOfColumn, Ap, Ai, Ax, Az, &Symbolic, NULL, NULL);
	(void)umfpack_zi_numeric(Ap, Ai, Ax, Az, Symbolic, &Numeric, NULL, NULL);


	//创建 solver
	UmfpackSolver *umpsolver = (UmfpackSolver*)malloc(sizeof(UmfpackSolver));

	//设置 solver
	umpsolver->Ai = Ai;
	umpsolver->Ap = Ap;
	umpsolver->Ax = Ax;
	umpsolver->Az = Az;
	umpsolver->AiSize = umpsolver->AxSize = nnz;
	umpsolver->ApSize = numberOfRow + 1;
	umpsolver->n = numberOfColumn;
	umpsolver->m = numberOfRow;
	umpsolver->Numeric = Numeric;

	return umpsolver;
}

DllExport void* CreateSolverLUUMFPACK_CCS(int numberOfRow, int numberOfColumn, int nnz, int *rowIndices, int *columnPtr, double *Values)
{

	int *Ai = (int*)malloc(sizeof(int)*(nnz));
	int *Ap = (int*)malloc(sizeof(int)*(numberOfColumn + 1));
	double *Ax = (double*)malloc(sizeof(double)*(nnz));

	memcpy(Ax, Values, sizeof(double)*nnz);
	memcpy(Ap, columnPtr, sizeof(int)*(numberOfColumn + 1));
	memcpy(Ai, rowIndices, sizeof(int)*nnz);

	void *Symbolic, *Numeric;
	(void)umfpack_di_symbolic(numberOfRow, numberOfColumn, Ap, Ai, Ax, &Symbolic, NULL, NULL);
	(void)umfpack_di_numeric(Ap, Ai, Ax, Symbolic, &Numeric, NULL, NULL);

	//创建 solver
	UmfpackSolver *umpsolver = (UmfpackSolver*)malloc(sizeof(UmfpackSolver));

	//设置 solver
	umpsolver->Ai = Ai;
	umpsolver->Ap = Ap;
	umpsolver->Ax = Ax;
	umpsolver->Az = NULL;
	umpsolver->AiSize = umpsolver->AxSize = nnz;
	umpsolver->ApSize = numberOfRow + 1;
	umpsolver->n = numberOfColumn;
	umpsolver->m = numberOfRow;
	umpsolver->Numeric = Numeric;

	return umpsolver;
}


DllExport int SolveLUUMFPACK_Complex(void * solver, double *x, double *b)
{
	UmfpackSolver *umfSolver = (UmfpackSolver*)solver;

	if (umfSolver->Ai == NULL || umfSolver->Ap == NULL || umfSolver->Ax == NULL) return -1;

	int *Ap = umfSolver->Ap;
	int *Ai = umfSolver->Ai;
	double *Ax = umfSolver->Ax;
	double *Az = umfSolver->Az;
	int n = umfSolver->n;
	int m = umfSolver->m;
	double *null = (double *)NULL;
	void *Numeric = umfSolver->Numeric;

	double *xx = (double*)malloc(sizeof(double)*m);
	double *xz = (double*)malloc(sizeof(double)*m);

	double *bb = (double*)malloc(sizeof(double)*m);
	double *bz = (double*)malloc(sizeof(double)*m);
	for (size_t i = 0; i < m; i++)
	{
		bb[i] = b[2 * i];
		bz[i] = b[2 * i + 1];
	}

	umfpack_zi_solve(UMFPACK_A, Ap, Ai, Ax, Az, xx, xz, bb, bz, Numeric, NULL, NULL);
	free(bb);
	free(bz);

	for (size_t i = 0; i < m; i++)
	{
		x[i * 2] = xx[i];
		x[i * 2 + 1] = xz[i];
	}
	free(xx);
	free(xz);
}

DllExport void FreeSolverLUUMFPACK_Complex(void *a)
{
	UmfpackSolver *item = (UmfpackSolver*)a;
	void *Numeric = item->Numeric;
	umfpack_di_free_numeric(&Numeric);
	item->Numeric = NULL;
	free(item->Ai);
	item->Ai = NULL;
	free(item->Ap);
	item->Ap = NULL;
	if (item->Az != NULL)
	{
		free(item->Az);
		item->Az = NULL;
	}
	free(item->Ax);
	item->Ax = NULL;
	free(item);



	a = NULL;
}