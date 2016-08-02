#include "ComputeEigen.h"
#include <stdlib.h>
#include "Util.h"
#include "areig.h"

//Dll output
int ComputeEigenNoSymmetricShiftModeCRS(
	int *index,
	int *pointer,
	double *values,
	int numberOfNoneZeros,
	int numberOfRows,
	int resultCount,
	double sigma,
    double *RealPart,
	double *ImagePart,
	double *EigenVectors
	)
{

	int n = numberOfRows;
	int nnz = numberOfNoneZeros;

	int nconv = AREig(RealPart, ImagePart, EigenVectors, n, nnz, values, index, pointer, sigma, resultCount);

	return nconv;
}

//None-Symmetric
void* ComputeNonSymmetricMatrixAllEigenValueAndEigenVector(int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows)
{
	 struct EigenResult *eigenStorage = (struct EigenResult*)malloc(sizeof(struct EigenResult));
	
	int n  = numberOfRows;
	int nnz = numberOfNoneZeros;
	int *rowIndex;
	int *pColumn;
	double *A;

	CoverTripletToCRS(Ti,Tj,Tx,n,nnz,A,rowIndex,pColumn);


	double* EigValR = (double*)malloc(sizeof(double)*numberOfRows);   // Real part of the eigenvalues.
	double* EigValI = (double*)malloc(sizeof(double)*numberOfRows);   // Imaginary part of the eigenvalues.
	double* EigVec  = (double*)malloc(sizeof(double)*numberOfRows*numberOfRows);  // Eigenvectors.

	int nconv = AREig(EigValR, EigValI, EigVec, n, nnz, A, rowIndex, pColumn,(numberOfRows-2),"LM");

	double *smallestPartR = EigValR+(numberOfRows-2);
	double *smallestPartI = EigValI+(numberOfRows-2);
	double *smallestVector = EigVec+(numberOfRows*(numberOfRows-2));
	int nconv1 = AREig(smallestPartR, smallestPartI, smallestVector, n, nnz, A, rowIndex, pColumn,2,"SM");

	eigenStorage->EigenValueImagePart = EigValI;
	eigenStorage->EigenValueRealPart = EigValR;
	eigenStorage->EigenVector = EigVec;
	eigenStorage->rows = numberOfRows;
	eigenStorage->resultCount = numberOfRows;

	return eigenStorage;
}

void* ComputeNonSymmetricMatrixEigenValueAndEigenVectorShiftMode(int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows,double sigma,int resultCount)
{
	 struct EigenResult *eigenStorage = (struct EigenResult*)malloc(sizeof(struct EigenResult));
	
	int n  = numberOfRows;
	int nnz = numberOfNoneZeros;
	int *rowIndex;
	int *pColumn;
	double *A;

	CoverTripletToCRS(Ti,Tj,Tx,n,nnz,A,rowIndex,pColumn);


	double* EigValR = (double*)malloc(sizeof(double)*resultCount);   // Real part of the eigenvalues.
	 double* EigValI = (double*)malloc(sizeof(double)*resultCount);   // Imaginary part of the eigenvalues.
	double* EigVec  = (double*)malloc(sizeof(double)*resultCount*numberOfRows);  // Eigenvectors.

	int nconv = AREig(EigValR, EigValI, EigVec, n, nnz, A, rowIndex, pColumn,sigma,resultCount);



	eigenStorage->EigenValueImagePart = EigValI;
	eigenStorage->EigenValueRealPart = EigValR;
	eigenStorage->EigenVector = EigVec;
	eigenStorage->rows = numberOfRows;
	eigenStorage->resultCount = resultCount;

	return eigenStorage;
}

void* ComputeNonSymmetricMatrixEigenValueAndEigenVectorWithLargestMagnitude(int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows,int resultCount)
{
	struct EigenResult *eigenStorage = (struct EigenResult*)malloc(sizeof(struct EigenResult));
	
	int n  = numberOfRows;
	int nnz = numberOfNoneZeros;
	int *rowIndex;
	int *pColumn;
	double *A;

	CoverTripletToCRS(Ti,Tj,Tx,n,nnz,A,rowIndex,pColumn);


	double* EigValR = (double*)malloc(sizeof(double)*resultCount);   // Real part of the eigenvalues.
	 double* EigValI = (double*)malloc(sizeof(double)*resultCount);   // Imaginary part of the eigenvalues.
	double* EigVec  = (double*)malloc(sizeof(double)*resultCount*numberOfRows);  // Eigenvectors.

	int nconv = AREig(EigValR, EigValI, EigVec, n, nnz, A, rowIndex, pColumn,resultCount);

	eigenStorage->EigenValueImagePart = EigValI;
	eigenStorage->EigenValueRealPart = EigValR;
	eigenStorage->EigenVector = EigVec;
	eigenStorage->rows = numberOfRows;
	eigenStorage->resultCount = resultCount;

	return eigenStorage;
}

void* ComputeNonSymmetricMatrixEigenValueAndEigenVectorWithSmallestMagnitude(int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows,int resultCount)
{
	struct EigenResult *eigenStorage = (struct EigenResult*)malloc(sizeof(struct EigenResult));
	
	int n  = numberOfRows;
	int nnz = numberOfNoneZeros;
	int *rowIndex;
	int *pColumn;
	double *A;

	CoverTripletToCRS(Ti,Tj,Tx,n,nnz,A,rowIndex,pColumn);


	double* EigValR = (double*)malloc(sizeof(double)*resultCount);   // Real part of the eigenvalues.
	 double* EigValI = (double*)malloc(sizeof(double)*resultCount);   // Imaginary part of the eigenvalues.
	double* EigVec  = (double*)malloc(sizeof(double)*resultCount*numberOfRows);  // Eigenvectors.

	int nconv = AREig(EigValR, EigValI, EigVec, n, nnz, A, rowIndex, pColumn,resultCount,"SM");

	eigenStorage->EigenValueImagePart = EigValI;
	eigenStorage->EigenValueRealPart = EigValR;
	eigenStorage->EigenVector = EigVec;
	eigenStorage->rows = numberOfRows;
	eigenStorage->resultCount = resultCount;

	return eigenStorage;
}

void* ComputeNonSymmetricMatrixEigenValueAndEigenVectorWithLargestRealPart(int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows,int resultCount)
{
	struct EigenResult *eigenStorage = (struct EigenResult*)malloc(sizeof(struct EigenResult));
	
	int n  = numberOfRows;
	int nnz = numberOfNoneZeros;
	int *rowIndex;
	int *pColumn;
	double *A;

	CoverTripletToCRS(Ti,Tj,Tx,n,nnz,A,rowIndex,pColumn);

	double* EigValR = (double*)malloc(sizeof(double)*resultCount);   // Real part of the eigenvalues.
	 double* EigValI = (double*)malloc(sizeof(double)*resultCount);   // Imaginary part of the eigenvalues.
	double* EigVec  = (double*)malloc(sizeof(double)*resultCount*numberOfRows);  // Eigenvectors.

	int nconv = AREig(EigValR, EigValI, EigVec, n, nnz, A, rowIndex, pColumn,resultCount,"LR");

	eigenStorage->EigenValueImagePart = EigValI;
	eigenStorage->EigenValueRealPart = EigValR;
	eigenStorage->EigenVector = EigVec;
	eigenStorage->rows = numberOfRows;
	eigenStorage->resultCount = resultCount;

	return eigenStorage;
}

void* ComputeNonSymmetricMatrixEigenValueAndEigenVectorWithSmallestRealPart(int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows,int resultCount)
{
	struct EigenResult *eigenStorage = (struct EigenResult*)malloc(sizeof(struct EigenResult));
	
	int n  = numberOfRows;
	int nnz = numberOfNoneZeros;
	int *rowIndex;
	int *pColumn;
	double *A;

	CoverTripletToCRS(Ti,Tj,Tx,n,nnz,A,rowIndex,pColumn);


	double* EigValR = (double*)malloc(sizeof(double)*resultCount);   // Real part of the eigenvalues.
	double* EigValI = (double*)malloc(sizeof(double)*resultCount);   // Imaginary part of the eigenvalues.
	double* EigVec  = (double*)malloc(sizeof(double)*resultCount*numberOfRows);  // Eigenvectors.

	int nconv = AREig(EigValR, EigValI, EigVec, n, nnz, A, rowIndex, pColumn,resultCount,"SR");

	eigenStorage->EigenValueImagePart = EigValI;
	eigenStorage->EigenValueRealPart = EigValR;
	eigenStorage->EigenVector = EigVec;
	eigenStorage->rows = numberOfRows;
	eigenStorage->resultCount = resultCount;

	return eigenStorage;
}

#
//Symmetric
void* ComputeSymmetricMatrixEigenValueAndEigenVectorShiftMode(char matrixTrais,int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows,double sigma,int resultCount,int maxIteration)
{
	struct EigenResult *eigenStorage = (struct EigenResult*)malloc(sizeof(struct EigenResult));

	int n  = numberOfRows;
	int nnz = numberOfNoneZeros;
	int *rowIndex;
	int *pColumn;
	double *A;

	CoverTripletToCRS(Ti,Tj,Tx,n,nnz,A,rowIndex,pColumn);

	double* EigValR = (double*)malloc(sizeof(double)*resultCount);   // Real part of the eigenvalues.
	//There is no image part in Symmetric mode
	double* EigVec  = (double*)malloc(sizeof(double)*resultCount*numberOfRows);  // Eigenvectors.

	int nconv = AREig(EigValR, EigVec, n, nnz, A, rowIndex, pColumn,matrixTrais,sigma,resultCount);

	eigenStorage->EigenValueRealPart = EigValR;
	eigenStorage->EigenVector = EigVec;
	eigenStorage->rows = numberOfRows;
	eigenStorage->resultCount = resultCount;

	switch (matrixTrais)
	{
	case 'U':
		eigenStorage->upFlag = 1;
		eigenStorage->lowFlag = 0;
		break;
	case 'L':
		eigenStorage->upFlag = 0;
		eigenStorage->lowFlag = 1;
	default:
		break;
	}

	return eigenStorage;
}

void* ComputeSymmetricMatrixEigenValueAndEigenVectorWithLargestMagnitude(char matrixTrais,int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows,int resultCount)
{
	struct EigenResult *eigenStorage = (struct EigenResult*)malloc(sizeof(struct EigenResult));
	
	int n  = numberOfRows;
	int nnz = numberOfNoneZeros;
	int *rowIndex;
	int *pColumn;
	double *A;

	CoverTripletToCRS(Ti,Tj,Tx,n,nnz,A,rowIndex,pColumn);


	double* EigValR = (double*)malloc(sizeof(double)*resultCount);   // Real part of the eigenvalues.
	double* EigVec  = (double*)malloc(sizeof(double)*resultCount*numberOfRows);  // Eigenvectors.

	int nconv = AREig(EigValR, EigVec, n, nnz, A, rowIndex, pColumn,matrixTrais,resultCount);

	eigenStorage->EigenValueRealPart = EigValR;
	eigenStorage->EigenVector = EigVec;
	eigenStorage->rows = numberOfRows;
	eigenStorage->resultCount = resultCount;
	switch (matrixTrais)
	{
	case 'U':
		eigenStorage->upFlag = 1;
		eigenStorage->lowFlag = 0;
		break;
	case 'L':
		eigenStorage->upFlag = 0;
		eigenStorage->lowFlag = 1;
	default:
		break;
	}


	return eigenStorage;
}

void* ComputeSymmetricMatrixEigenValueAndEigenVectorWithSmallestMagnitude(char matrixTrais,int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows,int resultCount)
{
	struct EigenResult *eigenStorage = (struct EigenResult*)malloc(sizeof(struct EigenResult));
	
	int n  = numberOfRows;
	int nnz = numberOfNoneZeros;
	int *rowIndex;
	int *pColumn;
	double *A;

	CoverTripletToCRS(Ti,Tj,Tx,n,nnz,A,rowIndex,pColumn);


	double* EigValR = (double*)malloc(sizeof(double)*resultCount);   // Real part of the eigenvalues.
	double* EigVec  = (double*)malloc(sizeof(double)*resultCount*numberOfRows);  // Eigenvectors.

	int nconv = AREig(EigValR, EigVec, n, nnz, A, rowIndex, pColumn,matrixTrais,resultCount);

	eigenStorage->EigenValueRealPart = EigValR;
	eigenStorage->EigenVector = EigVec;
	eigenStorage->rows = numberOfRows;
	eigenStorage->resultCount = resultCount;
	switch (matrixTrais)
	{
	case 'U':
		eigenStorage->upFlag = 1;
		eigenStorage->lowFlag = 0;
		break;
	case 'L':
		eigenStorage->upFlag = 0;
		eigenStorage->lowFlag = 1;
	default:
		break;
	}


	return eigenStorage;
}

void* ComputeSymmetricMatrixEigenValueAndEigenVectorWithLargestRealPart(char matrixTrais,int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows,int resultCount)
{
	struct EigenResult *eigenStorage = (struct EigenResult*)malloc(sizeof(struct EigenResult));
	
	int n  = numberOfRows;
	int nnz = numberOfNoneZeros;
	int *rowIndex;
	int *pColumn;
	double *A;

	CoverTripletToCRS(Ti,Tj,Tx,n,nnz,A,rowIndex,pColumn);


	double* EigValR = (double*)malloc(sizeof(double)*resultCount);   // Real part of the eigenvalues.
	double* EigVec  = (double*)malloc(sizeof(double)*resultCount*numberOfRows);  // Eigenvectors.

	int nconv = AREig(EigValR, EigVec, n, nnz, A, rowIndex, pColumn,matrixTrais,resultCount,"LA");

	eigenStorage->EigenValueRealPart = EigValR;
	eigenStorage->EigenVector = EigVec;
	eigenStorage->rows = numberOfRows;
	eigenStorage->resultCount = resultCount;
	switch (matrixTrais)
	{
	case 'U':
		eigenStorage->upFlag = 1;
		eigenStorage->lowFlag = 0;
		break;
	case 'L':
		eigenStorage->upFlag = 0;
		eigenStorage->lowFlag = 1;
	default:
		break;
	}


	return eigenStorage;
}

void* ComputeSymmetricMatrixEigenValueAndEigenVectorWithSmallestRealPart(char matrixTrais,int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows,int resultCount)
{
	struct EigenResult *eigenStorage = (struct EigenResult*)malloc(sizeof(struct EigenResult));
	
	int n  = numberOfRows;
	int nnz = numberOfNoneZeros;
	int *rowIndex;
	int *pColumn;
	double *A;

	CoverTripletToCRS(Ti,Tj,Tx,n,nnz,A,rowIndex,pColumn);


	double* EigValR = (double*)malloc(sizeof(double)*resultCount);   // Real part of the eigenvalues.
	double* EigVec  = (double*)malloc(sizeof(double)*resultCount*numberOfRows);  // Eigenvectors.

	int nconv = AREig(EigValR, EigVec, n, nnz, A, rowIndex, pColumn,matrixTrais,resultCount,"SA");

	eigenStorage->EigenValueRealPart = EigValR;
	eigenStorage->EigenVector = EigVec;
	eigenStorage->rows = numberOfRows;
	eigenStorage->resultCount = resultCount;

	switch (matrixTrais)
	{
	case 'U':
		eigenStorage->upFlag = 1;
		eigenStorage->lowFlag = 0;
		break;
	case 'L':
		eigenStorage->upFlag = 0;
		eigenStorage->lowFlag = 1;
	default:
		break;
	}


	return eigenStorage;
}
