#include "taucs_symbolic.h"
#include <memory.h>
//
// functions for symbolic solver
//

DllExport void * CreateSolverSymbolicTAUCS(int n, int nnz, int *rowIndex, int *colIndex, double *value)
{
	struct SymbolicSolver * s = (struct SymbolicSolver*) malloc(sizeof(struct SymbolicSolver));
	s->n = n;
	s->matrix = taucs_ccs_create(n, n, nnz, TAUCS_DOUBLE|TAUCS_LOWER|TAUCS_SYMMETRIC);
	s->factorization = NULL;
	s->perm    = (int*) malloc(sizeof(int) * n);
	s->invperm = (int*) malloc(sizeof(int) * n);
	s->tmp_b = (double*) malloc(sizeof(double) * n);
	s->tmp_x = (double*) malloc(sizeof(double) * n);

	if (s->matrix == NULL) return NULL;
	if (s->perm == NULL) return NULL;

	memcpy(s->matrix->colptr, colIndex, sizeof(int)*(n+1));
	memcpy(s->matrix->rowind, rowIndex, sizeof(int)*nnz);
	memcpy(s->matrix->values.d, value, sizeof(double)*nnz);

	taucs_ccs_order(s->matrix, &s->perm, &s->invperm, "metis");
	s->matrix = taucs_ccs_permute_symmetrically(s->matrix, s->perm, s->invperm);
	s->factorization = taucs_ccs_factor_llt_symbolic(s->matrix);

	return s;
}

DllExport void FreeSolverSymbolicTAUCS(void *sp)
{
	struct SymbolicSolver * s = (struct SymbolicSolver *) sp;
	if (s == NULL) return;
	if (s->matrix) taucs_ccs_free(s->matrix);
	if (s->factorization) taucs_linsolve(NULL, &s->factorization, 0, NULL, NULL, NULL, NULL);
	if (s->perm) free(s->perm);
	if (s->invperm) free(s->invperm);
	if (s->tmp_b) free (s->tmp_b);
	if (s->tmp_x) free (s->tmp_x);
	s->matrix = NULL;
	s->perm = s->invperm = NULL;
	s->tmp_b = s->tmp_x = NULL;
}

DllExport int NumericFactor(void *sp)
{
	struct SymbolicSolver * s = (struct SymbolicSolver *) sp;
	return taucs_ccs_factor_llt_numeric(s->matrix, s->factorization);
}

DllExport void FreeNumericFactor(void *sp)
{
	struct SymbolicSolver * s = (struct SymbolicSolver *) sp;
	taucs_supernodal_factor_free_numeric(s->factorization);
}

DllExport int NumericSolve(void * sp, double *x, double *b)
{
	int rc = -1;
	struct SymbolicSolver * s = (struct SymbolicSolver *) sp;
	taucs_vec_permute(s->n, TAUCS_DOUBLE, b, s->tmp_b, s->perm);
	rc = taucs_supernodal_solve_llt(s->factorization, s->tmp_x, s->tmp_b);
	taucs_vec_permute(s->n, TAUCS_DOUBLE, s->tmp_x, x, s->invperm);
	return rc;
}
