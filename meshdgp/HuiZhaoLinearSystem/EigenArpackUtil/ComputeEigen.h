#ifndef COMPUTEEIGEN_H
#define COMPUTEEIGEN_H

#define DllExport  extern "C" __declspec( dllexport )

DllExport int ComputeEigenNoSymmetricShiftModeCRS(
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
	);

//None-Symmetic
void* ComputeNonSymmetricMatrixAllEigenValueAndEigenVector(int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows);

void* ComputeNonSymmetricMatrixEigenValueAndEigenVectorShiftMode(int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows,double sigma,int resultCount);

void* ComputeNonSymmetricMatrixEigenValueAndEigenVectorWithLargestMagnitude(int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows,int resultCount);

void* ComputeNonSymmetricMatrixEigenValueAndEigenVectorWithSmallestMagnitude(int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows,int resultCount);

void* ComputeNonSymmetricMatrixEigenValueAndEigenVectorWithLargestRealPart(int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows,int resultCount);

void* ComputeNonSymmetricMatrixEigenValueAndEigenVectorWithSmallestRealPart(int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows,int resultCount);

//Symmetric

void* ComputeSymmetricMatrixEigenValueAndEigenVectorShiftMode(char matrixTraits,int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows,double sigma,int resultCount,int maxiteration);

void* ComputeSymmetricMatrixEigenValueAndEigenVectorWithLargestMagnitude(char matrixTraits,int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows,int resultCount);

void* ComputeSymmetricMatrixEigenValueAndEigenVectorWithSmallestMagnitude(char matrixTraits,int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows,int resultCount);

void* ComputeSymmetricMatrixEigenValueAndEigenVectorWithLargestRealPart(char matrixTraits,int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows,int resultCount);

void* ComputeSymmetricMatrixEigenValueAndEigenVectorWithSmallestRealPart(char matrixTraits,int *Ti,int *Tj,double *Tx,int numberOfNoneZeros,int numberOfRows,int resultCount);

#endif