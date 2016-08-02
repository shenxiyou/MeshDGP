using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class MatlabProxy
    {

        public string AppPath = "C:\\Develop\\gptoolbox-master\\mesh";

        public int EigenNum = 10;

      

        public void Clear()
        {
            matlab.Execute(@"clear");
        }
       

        public void CD(string path)
        {
            matlab.Execute(@"cd " +path);
        }


        public void OutPut(double[] data, string name)
        {
            matlab.PutWorkspaceData(name, "base", data);
        }
        public void OutPut(int[] data,string name)
        {
            matlab.PutWorkspaceData(name, "base", data);
        }

        public void OutPut(double[,] data, string name)
        {
            matlab.PutWorkspaceData(name, "base", data);
        }

        public void OutPut(int[,] data, string name)
        {
            matlab.PutWorkspaceData(name, "base", data);
        }

        //public void OutPut(double[][] data, string name)
        //{
        //    matlab.PutWorkspaceData(name, "base", data);
        //}

        public double[,] CoverteVertice(TriMesh mesh)
        {
            int nv = mesh.Vertices.Count;
            double[,] data = new double[nv, 3];
            for (int i = 0; i < nv; i++)
            {
                data[i, 0] = mesh.Vertices[i].Traits.Position.x;
                data[i, 1] = mesh.Vertices[i].Traits.Position.y;
                data[i, 2] = mesh.Vertices[i].Traits.Position.z;

            } 
            return data;
        }

        public float[,] CoverteVerticeV1(TriMesh mesh)
        {
            int nv = mesh.Vertices.Count;
            float[,] data = new float[nv, 3];
            for (int i = 0; i < nv; i++)
            {
                data[i, 0] =(float) mesh.Vertices[i].Traits.Position.x;
                data[i, 1] = (float)mesh.Vertices[i].Traits.Position.y;
                data[i, 2] = (float)mesh.Vertices[i].Traits.Position.z;

            }
            return data;
        }
        public double[,] CoverteFace(TriMesh mesh)
        {
            int nv = mesh.Faces.Count;
            double[,] data = new double[nv, 3];
            for (int i = 0; i < nv; i++)
            {
                int index=0;
                foreach(TriMesh.HalfEdge  hf in mesh.Faces[i].Halfedges)
                {
                    data[i, index] = hf.ToVertex.Index+1 ;
                    index++;
                }
                

            }
            return data;
        }


        public void OutPut(SparseMatrix sparse, String name)
        {
            
            double r ;
            double c;  
            double[] i ;
            double[] j ;
            double[] value;

          

            ConvertToSparse(sparse, out i, out  j, out value, out r, out c);
            matlab.PutWorkspaceData("i", "base", i);
            matlab.PutWorkspaceData("j", "base", j);
            matlab.PutWorkspaceData("value", "base", value);
            matlab.PutWorkspaceData("r", "base", r);
            matlab.PutWorkspaceData("c", "base", c);

            matlab.Execute(@" "+name+"=sparse(i,j,value,r,c)");

          
        }

        public SparseMatrix GetSparseMatrix(String name)
        {

            matlab.Execute(@"[row,col]=size(" + name + ")");
            double r = matlab.GetVariable("row", "base");
            double c = matlab.GetVariable("col", "base");

            matlab.Execute(@"[i,j value]=find(" + name + ")");
            double[,] i = matlab.GetVariable("i", "base");
            double[,] j= matlab.GetVariable("j", "base");
            double[,] value = matlab.GetVariable("value", "base"); 
            

            SparseMatrix sparse = ConvertToSparse(i, j, value, (int)r, (int)c);
            return sparse;
        }

        public SparseMatrix ConvertToSparse(double[,] i, double[,] j,double[,] value,int row,int col)
        {
            SparseMatrix sparse = new SparseMatrix(row, col);
            for(int k=0;k<i.Length;k++)
            {
                int r = (int)i[k, 0] - 1;
                int c = (int)j[k, 0] - 1;
                sparse.AddElement(r,c, value[k,0]);
               // Console.WriteLine("{0}--{1}--{2}", r,c, value[k, 0]);
                     
            }
            return sparse;
        }


        public void ConvertToSparse(SparseMatrix sparse, out double[] i, out double[] j, out double[] value, out double row, out double col)
        {
            row = sparse.RowSize;
            col = sparse.ColumnSize; 


            int length = sparse.NumOfElements();
            i = new double[length];
            j = new double[length];
            value = new double[length];

            int index = 0;
            for (int k = 0; k < (int)row; k++)
            { 
                foreach (SparseMatrix.Element e in sparse.Rows[k])
                {
                    i[index] = e.i + 1;
                    j[index] = e.j + 1;
                    value[index] = e.value;
                    index++;
                } 
            }


           
            
        }

        public double[] GetDoubleArray(string name)
        { 
            double[] data= matlab.GetVariable(name, "base");
           
            return data;
        }


        public int[] GetIntArray(string name)
        {
            int[] data = matlab.GetVariable(name, "base");

            return data;
        }

        public double[,] GetMatrix(string name)
        {
            //matlab.Execute(@"[row,col]=size(" + name + ")");
            //double r = matlab.GetVariable("row", "base");
            //double c = matlab.GetVariable("col", "base");
            double[,] array = matlab.GetVariable(name, "base");
           // PrintMatrix(array, (int)r, (int)c);
            //row = array.GetLength(0);
            //col = array.GetLength(1);
            return array;
        }

        public double[,] GetMatrixV1(string name)
        {
            //matlab.Execute(@"[row,col]=size(" + name + ")");
            //double r = matlab.GetVariable("row", "base");
            //double c = matlab.GetVariable("col", "base");
            object output = null;
            matlab.GetWorkspaceData(name, "base", out output);
            double[,] array = output as double[,];
        //    PrintMatrix(array, (int)r, (int)c);
            //row = array.GetLength(0);
            //col = array.GetLength(1);
            return array;
        }


        public void PrintArray(double[] input)
        {
            var str = "";
            for (int index = 0; index < input.Length; index++)
            {
                var item = input[index];
                str += item.ToString();
                if (index < input.Length - 1)
                    str += ", ";
            }
            Console.WriteLine("[" + str + "]");
        }


        public void PrintMatrix(double[,] input, int row, int col)
        {
            string str = "";
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    var item = input[i, j];
                    str += item.ToString();
                    if (j < col - 1)
                        str += ", ";
                }

                Console.WriteLine("[" + str + "]");
                str = "";
            }

        }




      

        public Eigen GetEigen(SparseMatrix sparse, int num)
        {
            OutPut(sparse, "L");
            matlab.Execute(@"[EV,ED] = eigs(L," + num.ToString() + ",'sm') ");

            double[,] eigVector = GetMatrix("EV");
            double[,] eigValue = GetMatrix("ED");
             
            int len=eigVector.GetLength(0);
            List<EigenPair> list = new List<EigenPair>();

            for (int i = 0; i < num; i++)
            {
                double realPart = eigValue[i,i];  

                List<double> vector = new List<double>();

                for (int j = 0; j < len; j++)
                {
                    double value = eigVector[j,i];
                    vector.Add(value);
                }

                EigenPair newPair = new EigenPair(realPart, vector);


                list.Add(newPair);
            }

            list.Sort();

            Eigen eigen = new Eigen();
            eigen.SortedEigens = list.ToArray();  
             
            return eigen;

        }



        public bool IsPSD(SparseMatrix sparse)
        { 
            MatlabProxy.Instance.OutPut(sparse, "psd");

            matlab.Execute(@"[~,p] = chol(psd)");

            double p = 0;
            p=matlab.GetVariable("p", "base");

            bool result = false;
            if (p == 0)
                result = true;
            else
                result = false;


            return result;

        }

     
        public void SVD(ref double[,] X, out double[,]S,out double[,]V,out double[,] U)
        {
            OutPut(X, "X");
            matlab.Execute(@"[U,S,V] = svd(X)");
            U= GetMatrix("U");
            S = GetMatrix("S");
            V = GetMatrix("V");
        }


        public double[,] SVDRotation(ref double[,] X )
        {
            OutPut(X, "X");
            matlab.Execute(@"[U,S,V] = svd(X)");
            matlab.Execute(@"R=U*transpose(V)");
            double[,] R = GetMatrix("R");
            return R;
        }

        public Matrix3D SVDRotation(ref Matrix3D matrix)
        {

            double[,] X = new double[3, 3];

            for (int i = 0; i < 3;i++ )
            { 
                for(int j=0;j<3;j++)
                {
                    X[i, j] = matrix[i, j];
                }
            }

            double[,] R = SVDRotation(ref X);

            Matrix3D result = new Matrix3D();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    result[i, j] = R[i, j];
                }
            } 
            return result;
        }


        

        public SVDInfo SVDInfo(ref Matrix3D matrix)
        {

            double[,] X = new double[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    X[i, j] = matrix[i, j];
                }
            }

            OutPut(X, "A");
            matlab.Execute(@"[U,S,V] = svd(A)");
             matlab.Execute(@"R = V*U';  if( det(R) < 0 ) U(:,end) = -U(:,end); R = V*U'; end"); 
            // matlab.Execute(@"R = V*U'");

            matlab.Execute(@"DetU=det(U)");
            matlab.Execute(@"DetS=det(S)");
            matlab.Execute(@"DetV=det(V)");
            matlab.Execute(@"DetR=det(R)");
            matlab.Execute(@"DetA=det(A)");

            double[,] A=  GetMatrix("A");
            double[,] R = GetMatrix("R");
            double[,] U = GetMatrix("U");
            double[,] S=  GetMatrix("S");
            double[,] V = GetMatrix("V");

            SVDInfo svdInfo = new SVDInfo();

            svdInfo.DetU = matlab.GetVariable("DetU", "base");
            svdInfo.DetS = matlab.GetVariable("DetS", "base");
            svdInfo.DetV = matlab.GetVariable("DetV", "base");
            svdInfo.DetR = matlab.GetVariable("DetR", "base");
            svdInfo.DetA = matlab.GetVariable("DetA", "base");


            svdInfo.A = new Matrix3D();
            svdInfo.R = new Matrix3D();
            svdInfo.U = new Matrix3D();
            svdInfo.S = new Matrix3D();
            svdInfo.V = new Matrix3D();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    svdInfo.A[i, j] = A[i, j];
                    svdInfo.R[i, j] = R[i, j];
                    svdInfo.U[i, j] = U[i, j];
                    svdInfo.S[i, j] = S[i, j];
                    svdInfo.V[i, j] = V[i, j];
                }
            }


            


            return svdInfo;
        }
    }
}
