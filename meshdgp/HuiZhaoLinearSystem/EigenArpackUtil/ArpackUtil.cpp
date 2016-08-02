#include <fstream>
#include <cstdlib>

#include "ComputeEigen.h"
#include "Util.h"
#include <iostream>
#include <iterator>
#include <string>
#include <vector>

using namespace std;

int main(int argc,char* argv[])
{
	int nev = atoi(argv[1]);
	char* readPathC = argv[2];
	char* writePathC = argv[3];

	string readPath(readPathC);
	string writePath(writePathC);

	int nnz = 0;
	int *Ti = NULL;
	int *Tj = NULL;
	double *Tx = NULL;

	int n = 0;
	int m = 0;
	
	bool isSymmetric = false;
	char symmetricMark;
	ReadFile(readPath,nnz,m,n,Ti,Tj,Tx,isSymmetric,symmetricMark);

	void *solver = ComputeSymmetricMatrixEigenValueAndEigenVectorShiftMode('L',Ti,Tj,Tx,nnz,n,0.0,nev,3000);

	WriteToFile(writePath,solver);
  
  return 0;
}

