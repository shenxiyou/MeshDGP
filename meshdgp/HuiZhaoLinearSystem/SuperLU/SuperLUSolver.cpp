#include "SuperLUSolver.h"
#include <stdlib.h>

DllExport void* CreateSolverLUSuperLU(int numberOfRows,int numberOfColumns,int numberOfNoneZero,int *rowIndex,int *columnIndex,double *values)
{
	//分配 Compress Row Store 存储空间 
	int *xa = intCalloc(numberOfColumns+1);
	int *asub = intCalloc(numberOfNoneZero);
	double *a = doubleCalloc(numberOfNoneZero);

	//将矩阵存储为 Compress Row Store 形式
	int i = 0;
	for(i = 0; i < numberOfColumns+1; i++){
		xa[i] = 0;
	}

	for(i = 0; i < numberOfNoneZero; i++){
		xa[columnIndex[i]+1]++;
		asub[i] = rowIndex[i];
		a[i] = values[i];
	}

	for(i = 1; i < numberOfColumns+1;i++){
		xa[i] += xa[i-1];
	}

	//分配矩阵 A
	SuperMatrix A;
	dCreate_CompCol_Matrix(&A,numberOfRows,numberOfColumns,numberOfNoneZero,a,asub,xa,SLU_NC,SLU_D,SLU_GE);

	//创建 Solver
	SuperLUSolver *lus = (SuperLUSolver*)malloc(sizeof(SuperLUSolver));

	lus->a = (void*)a;
	lus->asub = (int_t*)asub;
	lus->xa = (int_t*)xa;

	lus->m = numberOfRows;
	lus->n = numberOfColumns;
	lus->nnz = numberOfNoneZero;
	lus->A = A;

	//对因子化进行设置
	superlu_options_t *options = (superlu_options_t*)SUPERLU_MALLOC(sizeof(superlu_options_t));
	set_default_options(options);
	options->ColPerm = COLAMD;
	options->SymmetricMode = NO;
	lus->opinion = options;

	//LU 分解
	int info = 0;

	int *perm_r = intMalloc(lus->m);
	int *perm_c = intMalloc(lus->n);


	SuperMatrix L;
	SuperMatrix U;
	trans_t trans;

	//开始分解
	StatInit(&(lus->state));
	Factorization(lus->opinion,&(lus->A),perm_c,perm_r,&L,&U,&(lus->state),&info,trans);		

	lus->L = L;
	lus->U = U;
	lus->info = info;
	lus->transt = trans;
	lus->perc = perm_c;
	lus->perr = perm_r;

	//若发生错误返回 NULL
	if(info != 0){
		return NULL;
	}

		return lus;
}

DllExport void SolveLUSuperLU(void *solver,double *x,double *b)
{
		SuperLUSolver *lus = (SuperLUSolver*)solver;
	
		//获得矩阵行数与列数
		int mb = lus->m;
		int nb = 1;

		//创建向量 b
		SuperMatrix B;
		dCreate_Dense_Matrix(&B,mb,nb,b,mb,SLU_DN,SLU_D,SLU_GE);
		lus->B = B;

		 if ( (lus->info) == 0 ) {
			/* 解决 A*X=B, 并将 X 写入 B 里 */
			dgstrs (lus->transt, &(lus->L), &(lus->U), lus->perc, lus->perr, &B,&lus->state, &(lus->info));
	   }

		if(lus->info != 0){
			printf("Error!");
		}

		//拷贝结果到外部数组引用
		DNformat *Astore= (DNformat *)B.Store;  
		double *dp = (double *) Astore->nzval;  
		for(int i = 0;i<lus->m;i++){
			x[i] = dp[i];
		}
}

 DllExport void FreeSolverLUSuperLU(void *solver)
 {
	SuperLUSolver *lus = (SuperLUSolver*)solver;

	SUPERLU_FREE(lus->perc);
	SUPERLU_FREE(lus->perr);
	SUPERLU_FREE(lus->opinion);

	//停止计算流程
	StatFree(&(lus->state));

	//销毁创建的空间
	Destroy_SuperMatrix_Store(&lus->A);
	Destroy_SuperMatrix_Store(&lus->B);
    Destroy_SuperNode_Matrix(&(lus->L));
	Destroy_CompCol_Matrix(&(lus->U));

}

 
//因子化具体实现较为复杂
void
	Factorization(superlu_options_t *options, SuperMatrix *A, int *perm_c, int *perm_r,
      SuperMatrix *L, SuperMatrix *U, 
      SuperLUStat_t *stat, int *info ,trans_t &transt)
{

    SuperMatrix *AA;/* A in SLU_NC format used by the factorization routine.*/
    SuperMatrix AC; /* Matrix postmultiplied by Pc */
    int      lwork = 0, *etree, i;
    
    /* Set default values for some parameters */
    int      panel_size;     /* panel size */
    int      relax;          /* no of columns in a relaxed snodes */
    int      permc_spec;
    trans_t  trans = NOTRANS;
    double   *utime;
    double   t;	/* Temporary time */

    /* Test the input parameters ... */
    *info = 0;

    if ( options->Fact != DOFACT ) *info = -1;
    else if ( A->nrow != A->ncol || A->nrow < 0 ||
	 (A->Stype != SLU_NC && A->Stype != SLU_NR) ||
	 A->Dtype != SLU_D || A->Mtype != SLU_GE )
	*info = -2;
    
    if ( *info != 0 ) {
	i = -(*info);
	xerbla_("dgssv", &i);
	return;
    }

    utime = stat->utime;

    /* Convert A to SLU_NC format when necessary. */
    if ( A->Stype == SLU_NR ) {
	NRformat *Astore = (NRformat*)A->Store;
	AA = (SuperMatrix *) SUPERLU_MALLOC( sizeof(SuperMatrix) );
	dCreate_CompCol_Matrix(AA, A->ncol, A->nrow, Astore->nnz, 
			       (double*)Astore->nzval, Astore->colind, Astore->rowptr,
			       SLU_NC, A->Dtype, A->Mtype);
	trans = TRANS;
    } else {
        if ( A->Stype == SLU_NC ) AA = A;
    }

    t = SuperLU_timer_();
    /*
     * Get column permutation vector perm_c[], according to permc_spec:
     *   permc_spec = NATURAL:  natural ordering 
     *   permc_spec = MMD_AT_PLUS_A: minimum degree on structure of A'+A
     *   permc_spec = MMD_ATA:  minimum degree on structure of A'*A
     *   permc_spec = COLAMD:   approximate minimum degree column ordering
     *   permc_spec = MY_PERMC: the ordering already supplied in perm_c[]
     */
    permc_spec = options->ColPerm;
    if ( permc_spec != MY_PERMC && options->Fact == DOFACT )
      get_perm_c(permc_spec, AA, perm_c);
    utime[COLPERM] = SuperLU_timer_() - t;

    etree = intMalloc(A->ncol);

    t = SuperLU_timer_();
    sp_preorder(options, AA, perm_c, etree, &AC);
    utime[ETREE] = SuperLU_timer_() - t;

    panel_size = sp_ienv(1);
    relax = sp_ienv(2);

    /*printf("Factor PA = LU ... relax %d\tw %d\tmaxsuper %d\trowblk %d\n", 
	  relax, panel_size, sp_ienv(3), sp_ienv(4));*/
    t = SuperLU_timer_(); 
    /* Compute the LU factorization of A. */
    dgstrf(options, &AC, relax, panel_size, etree,
            NULL, lwork, perm_c, perm_r, L, U, stat, info);
    utime[FACT] = SuperLU_timer_() - t;



    SUPERLU_FREE (etree);
    Destroy_CompCol_Permuted(&AC);

    if ( A->Stype == SLU_NR ) {
	Destroy_SuperMatrix_Store(AA);
	SUPERLU_FREE(AA);
	}

	transt = trans;
}

