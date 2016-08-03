#include "Util.h"



void CoverTripletToCRS(int *Ti,int *Tj,double *Tx,int n, int nnz,
                       double* &A, int* &irow, int* &pcol)

{
	  A    = (double*)malloc(sizeof(double)*nnz);
	  irow = (int*)malloc(sizeof(int)*nnz);
	  pcol = (int*)malloc(sizeof(int)*(n+1));

	for(int j = 0;j < n+1;j++){
		pcol[j] = 0;
	}

	int i = 0;
	for(i = 0; i < nnz; i++){
		int index = Tj[i]+1;
		pcol[index]++;
		irow[i] = Ti[i];
		A[i] = Tx[i];
	}

	for(i = 1; i < n+1;i++){
		pcol[i] += pcol[i-1];

	}

}


vector<string> split(const string& s) {

	vector<string> ret;
	typedef string::size_type string_size;
	string_size i = 0;

	// invariant: we have processed characters [original value of i, i)
	while (i != s.size()) {

		// ignore leading blanks
		// invariant: chartacters in range [original i, current i) are all spaces
		while (i !=s.size() && isspace(s[i]))
			++i;

		// find end of next word
		string_size j = i;
		// invariant: none of the characters in rang [original j, current j) is a space
		while (j != s.size() && !isspace(s[j]))
			++j;

		// if we found some nonewithspace characters
		if (i != j) {
			// copy from s starting at i and taking j - i chars
			ret.push_back(s.substr(i, j - i));
			i = j;
		}
	}
		return ret;
};

void ReadFile(string filePath,int &nnz,int &m,int &n,int* &Ti,int* &Tj,double* &Tx,bool &isSymmetric,char &symmetricMark){

	ifstream out;
	out.open(filePath.c_str(),ios::in);
	string line;
	int count = 0;
	while(getline(out,line)){

		if(count == 0){
			vector<string> vs = split(line);
			string rowString = vs[1];
			string columnString = vs[3];
			string nnzString = vs[5];

			m = atoi(rowString.c_str());
			n = atoi(columnString.c_str());
			nnz = atoi(nnzString.c_str());

			Ti = (int*)malloc(sizeof(int)*nnz);
			Tj = (int*)malloc(sizeof(int)*nnz);
			Tx = (double*)malloc(sizeof(double)*nnz);

		}
		else if(count == 1)	  //Symmetric Situaction
		{	 
		   vector<string> vs = split(line);
		   string symmetricMark = vs[1];

		   if(symmetricMark == "true")
			   isSymmetric = true;
		   else
			   isSymmetric = false;

		   string symmMark = vs[2];
		   symmetricMark = symmetricMark.c_str()[0];
		}	
		else if(count > 1){

			vector<string> vs = split(line);
			string ti = vs[0];
			string tj = vs[1];
			string tx = vs[2];

			int tiIndex = atoi(ti.c_str());
			int tjIndex = atoi(tj.c_str());
			double txValue = atof(tx.c_str());

			int *currentTi = Ti+(count-2);
			int *currentTj = Tj+(count-2);
			double *currentTx = Tx + (count-2);

			*currentTi = tiIndex;
			*currentTj = tjIndex;
			*currentTx = txValue;

		}

		count++;
	}

	out.close();
}

void WriteToFile(string filePath,void* solver){
	struct EigenResult *so = (struct EigenResult*)solver;
	double *EigValR = so->EigenValueRealPart;
	double *EigValI = so->EigenValueImagePart;
	double *EigVec = so->EigenVector;
	int nn = so->resultCount;
	int nRow = so->rows;

	
	ofstream ofs(filePath.c_str());

   
  int j = 0;
  ofs << "#lambda["<< (j+1) <<"]: " << EigValR[j]<<endl;
  int size = nn*nRow;
  for(int i = 0;i<size;i++){
	  

	  double value = EigVec[i];

	  ofs<<value;

	  if(i != size-1){
	  	 ofs<<endl;
	  }

	  if((i+1)%nRow == 0 && j != nn-1){
	   ofs<<endl;
	   j++;
	   ofs << "#lambda["<< (j+1) <<"]: " << EigValR[j]<<endl;
	  }
  }

  ofs.close();
}
