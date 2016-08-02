
#include <stdlib.h>
#include <stdio.h>
#include "cholmod.h"
#define DllExport  extern "C" __declspec( dllexport )

typedef struct cholmodsolver{
	cholmod_factor *L;
	cholmod_sparse *A;
	cholmod_common c;
}CholmodSolver;

DllExport void* CreateSolverCholeskyCHOLMOD(int numberOfRow,int numberOfColumn,int numberOfNoneZero,int numberOfEntries,int *Ti,int *Tj,double *Tx);
DllExport void SolveCholeskyCHOLMOD(void *solver,double *X,double *b);
DllExport void FreeSolverCholeskyCHOLMOD(void *solver);


DllExport void SolveRealByCholesky(int numberOfRow,int numberOfColumn,int numberOfNoneZero,int *Ti,int *Tj,double *Tx,double *X,double *b);
DllExport void SolveRealByCholesky_CRS(int numberOfRow,int numberOfColumn,int numberOfNoneZero ,int *rowIndex,int *colPtr,double *values,double *X,double *b);
DllExport void SolveRealByCholesky_CCS(int numberOfRow,int numberOfColumn,int numberOfNoneZero ,int *rowPtr,int *colIndex,double *values,double *X,double *b);