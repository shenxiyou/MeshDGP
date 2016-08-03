#ifndef UTIL_H
#define UTIL_H

#include <string>
#include <stdlib.h>
#include <iostream>
#include <vector>
#include <iterator>
#include <fstream>
#include <cstdlib>


using namespace std;

struct EigenResult{
	double *EigenValueRealPart;
	double *EigenValueImagePart;
	double *EigenVector;
	int rows;
	int resultCount;
	int lowFlag; // 1 is true,0 is false
	int upFlag; // 1 is true,0 is false;
};

void CoverTripletToCRS(int *Ti,int *Tj,double *Tx,int n, int nnz,
                       double* &A, int* &irow, int* &pcol);

void ReadFile(string filePath,int &nnz,int &m,int &n,int* &Ti,int* &Tj,double* &Tx,bool &isSymmetric,char &symmetricMark);
void WriteToFile(string filePath,void* solver);

vector<string> split(const string& s);

#endif