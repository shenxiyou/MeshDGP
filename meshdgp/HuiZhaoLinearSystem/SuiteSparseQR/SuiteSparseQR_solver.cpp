#include "SuiteSparseQR_solver.h"
#include <stdlib.h>

#define TRUE 1
#define FALSE 0

//[Checked! Working]
DllExport void* CreateSolverQRSuiteSparseQR_CCS(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int *rowIndex, int *colPtr, double *values)
{
	//Create Solve
	QRSolver *solver = (QRSolver*)malloc(sizeof(QRSolver));

	//Start solve
	cholmod_l_start(&(solver->c));

	cholmod_sparse *A = cholmod_l_allocate_sparse(numberOfRow, numberOfColumn, numberOfNoneZero, TRUE, TRUE, 0, CHOLMOD_REAL, &(solver->c));
	A->dtype = CHOLMOD_DOUBLE;

	int *ptr = (int*)A->p;
	int *idx = (int*)A->i;
	double *val = (double*)A->x;

	memcpy(ptr, colPtr, sizeof(int)*(numberOfColumn + 1));
	memcpy(idx, rowIndex, sizeof(int)*numberOfNoneZero);
	memcpy(val, values, sizeof(double)*numberOfNoneZero);

	//Factorize
	SuiteSparseQR_C_factorization *QR = SuiteSparseQR_C_factorize(SPQR_ORDERING_DEFAULT, SPQR_DEFAULT_TOL, A, &(solver->c));
	solver->QR = QR;
	solver->rowCount = numberOfRow;
	solver->columnCount = numberOfColumn;

	A->i = NULL;
	A->p = NULL;
	A->x = NULL;

	cholmod_l_free_sparse(&A, &(solver->c));

	return solver;
}

//[Checked! Working]
DllExport void* CreateSolverQRSuiteSparseQR_CRS(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int *rowPtr, int *colIndex, double *values)
{
	//Create Solve
	QRSolver *solver = (QRSolver*)malloc(sizeof(QRSolver));
	solver->rowCount = numberOfRow;
	solver->columnCount = numberOfColumn;

	//Start solve
	cholmod_l_start(&(solver->c));

	cholmod_sparse *At = cholmod_l_allocate_sparse(numberOfColumn, numberOfRow, numberOfNoneZero, TRUE, TRUE, 0, CHOLMOD_REAL, &(solver->c));
	int *ptr = (int*)At->p;
	int *idx = (int*)At->i;
	double *val = (double*)At->x;

	memcpy(ptr, rowPtr, sizeof(int)*(numberOfRow + 1));
	memcpy(idx, colIndex, sizeof(int)*numberOfNoneZero);
	memcpy(val, values, sizeof(double)*numberOfNoneZero);

	cholmod_sparse *A = cholmod_l_transpose(At, 1, &(solver->c));
	cholmod_l_free_sparse(&At, &(solver->c));

	//Factorize
	SuiteSparseQR_C_factorization *QR = SuiteSparseQR_C_factorize(SPQR_ORDERING_DEFAULT, SPQR_DEFAULT_TOL, A, &(solver->c));
	solver->QR = QR;

	cholmod_l_free_sparse(&A, &(solver->c));
	return solver;
}

//[Checked! Working]
DllExport void* CreateSolverQRSuiteSparseQR(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int numberOfEntries, int *Ti, int *Tj, double *Tx)
{
	//Create Solve
	QRSolver *solver = (QRSolver*)malloc(sizeof(QRSolver));

	//Start solve
	cholmod_l_start(&(solver->c));

	//Store matrix in triplet form
	cholmod_triplet *tempTriplet = cholmod_l_allocate_triplet(numberOfRow, numberOfColumn, numberOfEntries, 0, CHOLMOD_REAL, &(solver->c));
	tempTriplet->nzmax = numberOfEntries;
	tempTriplet->nnz = numberOfNoneZero;

	//Copy row and column index to triplet
	int *Iindex = (int*)tempTriplet->i;
	int *Jindex = (int*)tempTriplet->j;
	double *valueIndex = (double*)tempTriplet->x;

	int m = 0;
	for (m = 0; m<numberOfNoneZero; m++){
		Iindex[m] = Ti[m];
		Jindex[m] = Tj[m];
		valueIndex[m] = Tx[m];
	}

	//Create Matrix A by covert triplet form to sparse form
	cholmod_sparse *A = cholmod_l_triplet_to_sparse(tempTriplet, tempTriplet->nnz, &(solver->c));
	solver->rowCount = A->nrow;
	solver->columnCount = A->ncol;
	solver->nnz = A->nzmax;

	//Free created triplet object
	cholmod_l_free_triplet(&tempTriplet, &(solver->c));

	//Factorize
	SuiteSparseQR_C_factorization *QR = SuiteSparseQR_C_factorize(SPQR_ORDERING_DEFAULT, SPQR_DEFAULT_TOL, A, &(solver->c));
	solver->QR = QR;

	cholmod_l_free_sparse(&A, &(solver->c));

	//cholmod_dense *b = cholmod_l_ones(numberOfRow,1,CHOLMOD_REAL,&(solver->c));
	//solver->b = b;
	return solver;

}

//[Checked! Working]
DllExport void SolveLeastNormalByQR(void *solver, double *X, double *b)
{
	QRSolver *cs = (QRSolver*)solver;
	cholmod_dense *bPart = cholmod_l_ones(cs->columnCount, 1, CHOLMOD_REAL, &(cs->c));

	//Create vector b
	double *bValue = (double*)bPart->x;
	for (int i = 0; i < bPart->nrow; i++){
		bValue[i] = b[i];
	}

	/*
	[Q,R] = spqr (A') ;
	x = Q*(R'\b) ;
	*/

	// solve y = R'\(E'*b)
	cholmod_dense *y = SuiteSparseQR_C_solve(SPQR_RTX_EQUALS_ETB, cs->QR, bPart, &(cs->c));

	// compute xln = Q*y
	cholmod_dense *x = SuiteSparseQR_C_qmult(SPQR_QX, cs->QR, y, &(cs->c));

	cholmod_l_free_dense(&y, &(cs->c));
	cholmod_l_free_dense(&bPart, &(cs->c));
	bPart = NULL;

	//Copy result to reference double array X
	double *xx = (double*)x->x;
	for (int i = 0; i<x->nzmax; i++){
		X[i] = xx[i];
	}

	cholmod_l_free_dense(&x, &(cs->c));
}

//[Checked! Working]
DllExport void SolveLeastSqureByQR(void *solver, double *X, double *b)
{
	QRSolver *cs = (QRSolver*)solver;
	cholmod_dense *bPart = cholmod_l_ones(cs->rowCount, 1, CHOLMOD_REAL, &(cs->c));

	//Create vector b
	double *bValue = (double*)bPart->x;
	for (int i = 0; i < bPart->nrow; i++){
		bValue[i] = b[i];
	}

	/*
	[Q,R] = spqr (A) ;
	x = R\(Q'*b) ;
	*/

	// Y = Q'*B
	cholmod_dense *y = SuiteSparseQR_C_qmult(SPQR_QTX, cs->QR, bPart, &(cs->c));
	// X = R\(Y)
	cholmod_dense *x = SuiteSparseQR_C_solve(SPQR_RETX_EQUALS_B, cs->QR, y, &(cs->c));

	cholmod_l_free_dense(&y, &(cs->c));
	cholmod_l_free_dense(&bPart, &(cs->c));
	bPart = NULL;

	//Copy result to reference double array X
	double *xx = (double*)x->x;
	for (int i = 0; i<x->nzmax; i++){
		X[i] = xx[i];
	}

	cholmod_l_free_dense(&x, &(cs->c));
}

//[Checked! Working]
DllExport void FreeSolverQRSuiteSparseQR(void *solver)
{
	QRSolver *cs = (QRSolver*)solver;

	if (cs->QR != NULL)
		SuiteSparseQR_C_free(&(cs->QR), &(cs->c));

	cholmod_l_finish(&(cs->c));

	if (cs != NULL)
		free(cs);

	cs->QR = NULL;
	cs = NULL;
	solver = NULL;
}

//Addcitional Method 
DllExport void SolveRealByQR(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int *Ti, int *Tj, double *Tx, double *X, double *b)
{
	cholmod_common c;
	cholmod_l_start(&c);

	//Allocate triplet
	cholmod_triplet *tempTriplet = cholmod_l_allocate_triplet(numberOfRow, numberOfColumn, numberOfNoneZero, 0, CHOLMOD_REAL, &c);
	tempTriplet->nzmax = numberOfNoneZero;
	tempTriplet->nnz = numberOfNoneZero;

	//Copy row and column index to triplet
	int *Iindex = (int*)tempTriplet->i;
	int *Jindex = (int*)tempTriplet->j;
	double *valueIndex = (double*)tempTriplet->x;

	int m = 0;
	for (m = 0; m<numberOfNoneZero; m++){
		Iindex[m] = Ti[m];
		Jindex[m] = Tj[m];
		valueIndex[m] = Tx[m];
	}

	//Create Matrix A by covert triplet form to sparse form
	cholmod_sparse *A = cholmod_l_triplet_to_sparse(tempTriplet, tempTriplet->nnz, &c);

	int *ptr = (int*)A->p;
	int *idx = (int*)A->i;
	double *val = (double*)A->x;

	printf("Row index:\n");
	for (size_t i = 0; i < numberOfNoneZero + 1; i++)
	{
		printf("%d ", idx[i]);
	}

	printf("\nValues Pointer:\n");
	for (size_t i = 0; i < numberOfNoneZero + 1; i++)
	{
		printf("%lf ", val[i]);
	}

	printf("\nCol Pointer:\n");
	for (size_t i = 0; i < numberOfColumn + 2; i++)
	{
		printf("%d ", ptr[i]);
	}


	//Free created triplet object
	cholmod_l_free_triplet(&tempTriplet, &c);

	//Create B part 
	cholmod_dense *bPart = cholmod_l_ones(numberOfRow, 1, CHOLMOD_REAL, &c);
	double *bValue = (double*)bPart->x;
	for (int i = 0; i < bPart->nrow; i++){
		bValue[i] = b[i];
	}

	cholmod_dense *result = SuiteSparseQR_C_backslash(SPQR_ORDERING_DEFAULT, SPQR_DEFAULT_TOL, A, bPart, &c);
	cholmod_l_free_sparse(&A, &c);
	cholmod_l_free_dense(&bPart, &c);
	bPart = NULL;

	double *xx = (double*)result->x;
	for (int i = 0; i<result->nzmax; i++){
		X[i] = xx[i];
	}

	cholmod_l_free_dense(&result, &c);

	cholmod_l_finish(&c);

}

DllExport void SolveRealByQR_CRS(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int *rowPtr, int *colIndex, double *values, double *X, double *b)
{
	cholmod_common c;
	cholmod_l_start(&c);

	cholmod_sparse *At = cholmod_l_allocate_sparse(numberOfColumn, numberOfRow, numberOfNoneZero, TRUE, TRUE, 0, CHOLMOD_REAL, &c);
	int *ptr = (int*)At->p;
	int *idx = (int*)At->i;
	double *val = (double*)At->x;

	memcpy(ptr, rowPtr, sizeof(int)*(numberOfRow + 1));
	memcpy(idx, colIndex, sizeof(int)*numberOfNoneZero);
	memcpy(val, values, sizeof(double)*numberOfNoneZero);

	cholmod_sparse *A = cholmod_l_transpose(At, 1, &c);
	cholmod_l_free_sparse(&At, &c);

	//Create B part 
	cholmod_dense *bPart = cholmod_l_ones(numberOfRow, 1, CHOLMOD_REAL, &c);
	double *bValue = (double*)bPart->x;
	for (int i = 0; i < bPart->nrow; i++){
		bValue[i] = b[i];
	}

	cholmod_dense *result = SuiteSparseQR_C_backslash(SPQR_ORDERING_DEFAULT, SPQR_DEFAULT_TOL, A, bPart, &c);
	cholmod_l_free_sparse(&A, &c);
	cholmod_l_free_dense(&bPart, &c);
	bPart = NULL;

	double *xx = (double*)result->x;
	for (int i = 0; i<result->nzmax; i++){
		X[i] = xx[i];
	}

	cholmod_l_free_dense(&result, &c);

	cholmod_l_finish(&c);

	cholmod_l_finish(&c);

}

DllExport void SolveRealByQR_CCS(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int *rowIndex, int *colPtr, double *values, double *X, double *b)
{
	cholmod_common c;
	cholmod_l_start(&c);

	//Create Matrix A by covert triplet form to sparse form
	cholmod_sparse *A = cholmod_l_allocate_sparse(numberOfRow, numberOfColumn, numberOfNoneZero, TRUE, TRUE, 0, CHOLMOD_REAL, &c);
	A->dtype = CHOLMOD_DOUBLE;

	int *ptr = (int*)A->p;
	int *idx = (int*)A->i;
	double *val = (double*)A->x;

	memcpy(ptr, colPtr, sizeof(int)*(numberOfColumn + 1));
	memcpy(idx, rowIndex, sizeof(int)*numberOfNoneZero);
	memcpy(val, values, sizeof(double)*numberOfNoneZero);

	//Create B part 
	cholmod_dense *bPart = cholmod_l_ones(numberOfRow, 1, CHOLMOD_REAL, &c);
	double *bValue = (double*)bPart->x;
	for (int i = 0; i < bPart->nrow; i++){
		bValue[i] = b[i];
	}

	cholmod_dense *result = SuiteSparseQR_C_backslash(SPQR_ORDERING_DEFAULT, SPQR_DEFAULT_TOL, A, bPart, &c);
	cholmod_l_free_sparse(&A, &c);
	cholmod_l_free_dense(&bPart, &c);
	bPart = NULL;

	double *xx = (double*)result->x;
	for (int i = 0; i<result->nzmax; i++){
		X[i] = xx[i];
	}

	cholmod_l_free_dense(&result, &c);

	cholmod_l_finish(&c);

	cholmod_l_finish(&c);
}

//For Complex
DllExport void SolveRealByQR_CCS_Complex(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int *rowIndex, int *colPtr, double *values, double *X, double *b)
{
	cholmod_common c;
	cholmod_l_start(&c);

	//Create Matrix A by covert triplet form to sparse form
	cholmod_sparse *A = cholmod_l_allocate_sparse(numberOfRow, numberOfColumn, numberOfNoneZero, TRUE, TRUE, 0, CHOLMOD_COMPLEX, &c);
	A->dtype = CHOLMOD_DOUBLE;

	int *ptr = (int*)A->p;
	int *idx = (int*)A->i;
	double *val = (double*)A->x;

	memcpy(ptr, colPtr, sizeof(int)*(numberOfColumn + 1));
	memcpy(idx, rowIndex, sizeof(int)*numberOfNoneZero);
	memcpy(val, values, sizeof(double)* 2 * numberOfNoneZero);

	//Create B part 
	cholmod_dense *bPart = cholmod_l_ones(numberOfRow, 1, CHOLMOD_COMPLEX, &c);
	double *bValue = (double*)bPart->x;
	for (int i = 0; i < 2 * bPart->nrow; i++){
		bValue[i] = b[i];
	}

	cholmod_dense *result = SuiteSparseQR_C_backslash(SPQR_ORDERING_DEFAULT, SPQR_DEFAULT_TOL, A, bPart, &c);
	cholmod_l_free_sparse(&A, &c);
	cholmod_l_free_dense(&bPart, &c);
	bPart = NULL;

	double *xx = (double*)result->x;
	for (int i = 0; i<2 * result->nzmax; i++){
		X[i] = xx[i];
	}

	cholmod_l_free_dense(&result, &c);

	cholmod_l_finish(&c);

	cholmod_l_finish(&c);
}

DllExport void SolveRealByQR_CRS_Complex(int numberOfRow, int numberOfColumn, int numberOfNoneZero, int *rowPtr, int *colIndex, double *values, double *X, double *b)
{
	cholmod_common c;
	cholmod_l_start(&c);

	cholmod_sparse *At = cholmod_l_allocate_sparse(numberOfColumn, numberOfRow, numberOfNoneZero, TRUE, FALSE, 0, CHOLMOD_COMPLEX, &c);
	At->dtype = CHOLMOD_DOUBLE;
	int *ptr = (int*)At->p;
	int *idx = (int*)At->i;
	double *val = (double*)At->x;

	memcpy(ptr, rowPtr, sizeof(int)*(numberOfRow + 1));
	memcpy(idx, colIndex, sizeof(int)*numberOfNoneZero);
	memcpy(val, values, sizeof(double)*2*numberOfNoneZero);

	cholmod_sparse *A = cholmod_l_transpose(At, 1, &c);
	cholmod_l_free_sparse(&At, &c);

	//Create B part 
	cholmod_dense *bPart = cholmod_l_ones(numberOfRow, 1, CHOLMOD_COMPLEX, &c);
	double *bValue = (double*)bPart->x;
	for (int i = 0; i < 2 * bPart->nrow; i++){
		bValue[i] = b[i];
	}

	cholmod_dense *result = SuiteSparseQR_C_backslash(SPQR_ORDERING_DEFAULT, SPQR_DEFAULT_TOL, A, bPart, &c);
	cholmod_l_free_sparse(&A, &c);
	cholmod_l_free_dense(&bPart, &c);
	bPart = NULL;

	double *xx = (double*)result->x;
	for (int i = 0; i<2 * result->nzmax; i++){
		X[i] = xx[i];
	}

	cholmod_l_free_dense(&result, &c);

	cholmod_l_finish(&c);

	cholmod_l_finish(&c);
}

