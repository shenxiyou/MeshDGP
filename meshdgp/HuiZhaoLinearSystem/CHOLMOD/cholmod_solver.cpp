#include "cholmod_solver.h"
#define TRUE 1
#define FALSE 0

DllExport void* CreateSolverCholeskyCHOLMOD(int numberOfRow,int numberOfColumn,int numberOfNoneZero,int numberOfEntries,int *Ti,int *Tj,double *Tx)
{
	//创建 Solve
	CholmodSolver *solver = (CholmodSolver*)malloc(sizeof(CholmodSolver));

	//开始 solve
	cholmod_start(&(solver->c));

	//将矩阵存储为 triplet 形式
	cholmod_triplet *tempTriplet = cholmod_allocate_triplet(numberOfRow,numberOfColumn,numberOfEntries,-1,CHOLMOD_REAL,&(solver->c));
	tempTriplet->nzmax = numberOfEntries;
	tempTriplet->nnz = numberOfNoneZero;

	//拷贝行索引和列索引到 triplet
	int *Iindex = (int*)tempTriplet->i;
	int *Jindex = (int*)tempTriplet->j;
	double *valueIndex = (double*)tempTriplet->x;

	int m = 0;
	for(m = 0;m<numberOfNoneZero;m++){
		Iindex[m] = Ti[m];
		Jindex[m] = Tj[m];
		valueIndex[m] = Tx[m];
	}

	//创建矩阵 A 可以根据 triplet 中数据转换
	cholmod_sparse *A = cholmod_triplet_to_sparse(tempTriplet,numberOfNoneZero,&(solver->c));
	solver->A = A;

	//清空 triplet 内容
	cholmod_free_triplet(&tempTriplet,&(solver->c));

	//因子化
	cholmod_factor *L = cholmod_analyze (A, &(solver->c));
	cholmod_factorize(A, L,&(solver->c));
	solver->L = L;

	return solver;

}

DllExport void SolveCholeskyCHOLMOD(void *solver,double *X,double *B)
{
	
	CholmodSolver *cs = (CholmodSolver*)solver;

	//创建向量 b
	cholmod_dense *b = cholmod_allocate_dense(cs->A->nrow,1,cs->A->nrow,CHOLMOD_REAL,&(cs->c));
	
	double *bValue = (double*)b->x;
	for(int i = 0 ;i < b->nrow;i++){
		bValue[i] = B[i];
	}

	//解方程，并获得结果向量 X
	cholmod_dense *x = cholmod_solve (CHOLMOD_A,cs->L,b,&(cs->c));
	cholmod_free_dense(&b,&(cs->c));

	//将结果 X 项目拷贝至外部数组引用
	double *xx = (double*)x->x;
	for(int i = 0;i<x->nzmax;i++){
		X[i] = xx[i];
	}

	//释放掉资源
	cholmod_free_dense(&x,&(cs->c));
}

DllExport void FreeSolverCholeskyCHOLMOD(void *solver)
{
	CholmodSolver *cs = (CholmodSolver*)solver;
	cholmod_free_factor(&(cs->L),&(cs->c));
	cholmod_free_sparse(&(cs->A),&(cs->c));
	cholmod_finish (&(cs->c));
	free(cs);
	cs->A = NULL;
	cs->L = NULL;
	cs = NULL;
	solver = NULL;
}


DllExport void SolveRealByCholesky(int numberOfRow,int numberOfColumn,int numberOfNoneZero,int *Ti,int *Tj,double *Tx,double *X,double *b)
{
	cholmod_common c;
	cholmod_l_start(&c);

	//Allocate triplet
	cholmod_triplet *tempTriplet = cholmod_l_allocate_triplet(numberOfRow,numberOfColumn,numberOfNoneZero,0,CHOLMOD_REAL,&c);
	tempTriplet->nzmax = numberOfNoneZero;
	tempTriplet->nnz = numberOfNoneZero;

	//Copy row and column index to triplet
	int *Iindex = (int*)tempTriplet->i;
	int *Jindex = (int*)tempTriplet->j;
	double *valueIndex = (double*)tempTriplet->x;

	int m = 0;
	for(m = 0;m<numberOfNoneZero;m++){
		Iindex[m] = Ti[m];
		Jindex[m] = Tj[m];
		valueIndex[m] = Tx[m];
	}

	//Create Matrix A by covert triplet form to sparse form
	cholmod_sparse *A = cholmod_l_triplet_to_sparse(tempTriplet,tempTriplet->nnz,&c);
	cholmod_factor *L = cholmod_analyze (A, &c);
	cholmod_factorize(A, L,&c);

	//Free created triplet object
	cholmod_l_free_triplet(&tempTriplet,&c);

	//Create B part 
	cholmod_dense *bPart = cholmod_l_ones(numberOfRow,1,CHOLMOD_REAL,&c);
	double *bValue = (double*)bPart->x;
	for(int i = 0 ;i < bPart->nrow;i++){
		bValue[i] = b[i];
	}

	cholmod_dense *result = cholmod_solve (CHOLMOD_A,L,bPart,&c);

	cholmod_l_free_dense(&bPart,&c);
	bPart = NULL;
	

	double *xx = (double*)result->x;
	for(int i = 0;i<result->nzmax;i++){
		X[i] = xx[i];
	}

	cholmod_l_free_dense(&result,&c);
	cholmod_l_free_sparse (&A, &c) ;
	cholmod_l_finish(&c);
}


DllExport void SolveRealByCholesky_CRS(int numberOfRow,int numberOfColumn,int numberOfNoneZero ,int *rowIndex,int *colPtr,double *values,double *X,double *b)
{
	cholmod_common c;
	cholmod_l_start(&c);

	cholmod_sparse *At = cholmod_l_allocate_sparse(numberOfRow,numberOfColumn,numberOfNoneZero,TRUE,FALSE,0,CHOLMOD_REAL,&c);
	At->i = rowIndex;
	At->p = colPtr;
	At->x = values;

    cholmod_sparse *A = cholmod_l_transpose(At,1,&c);
	cholmod_l_free_sparse(&At,&c);
    cholmod_factor *L = cholmod_analyze (A, &c);
	cholmod_factorize(A, L,&c);

	//Create B part 
	cholmod_dense *bPart = cholmod_l_ones(numberOfRow,1,CHOLMOD_REAL,&c);
	double *bValue = (double*)bPart->x;
	for(int i = 0 ;i < bPart->nrow;i++){
		bValue[i] = b[i];
	}

	cholmod_dense *result = cholmod_solve (CHOLMOD_A,L,bPart,&c);
	cholmod_l_free_dense(&bPart,&c);
	bPart = NULL;

	double *xx = (double*)result->x;
	for(int i = 0;i<result->nzmax;i++){
		X[i] = xx[i];
	}

	cholmod_l_free_dense(&result,&c);
	cholmod_l_free_sparse (&A, &c) ;
	cholmod_l_finish(&c);

}


DllExport void SolveRealByCholesky_CCS(int numberOfRow,int numberOfColumn,int numberOfNoneZero ,int *rowPtr,int *colIndex,double *values,double *X,double *b)
{
	cholmod_common c;
	cholmod_l_start(&c);

	//Create Matrix A by covert triplet form to sparse form
	cholmod_sparse *A = cholmod_l_allocate_sparse(numberOfRow,numberOfColumn,numberOfNoneZero,TRUE,TRUE,0,CHOLMOD_REAL,&c);
	A->i = colIndex;
	A->p = rowPtr;
	A->x = values;
    cholmod_factor *L = cholmod_analyze (A, &c);
	cholmod_factorize(A, L,&c);
	
	//Create B part 
	cholmod_dense *bPart = cholmod_l_ones(numberOfRow,1,CHOLMOD_REAL,&c);
	double *bValue = (double*)bPart->x;
	for(int i = 0 ;i < bPart->nrow;i++){
		bValue[i] = b[i];
	}

	cholmod_dense *result = cholmod_solve (CHOLMOD_A,L,bPart,&c);
	cholmod_l_free_dense(&bPart,&c);
	bPart = NULL;

	double *xx = (double*)result->x;
	for(int i = 0;i<result->nzmax;i++){
		X[i] = xx[i];
	}

	cholmod_l_free_dense(&result,&c);
	cholmod_l_free_sparse(&A, &c) ;
	cholmod_l_finish(&c);
}