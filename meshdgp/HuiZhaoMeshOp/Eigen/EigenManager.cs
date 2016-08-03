using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
 
 


namespace GraphicResearchHuiZhao
{
  
    public class EigenManager
    {
        private static EigenManager singleton = new EigenManager();


        public static EigenManager Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new EigenManager();
                return singleton;
            }
        } 

       

        private  EigenManager( )
        {
             
        }


        private int num = 50;

        public int Num
        {
            get
            {
                return num;
            }
            set
            {
                num = value;
            }
        }


        public Eigen ComputeEigen(TriMesh mesh, EnumLaplaceMatrix type,int count)
        {
            SparseMatrix matrix = LaplaceManager.Instance.GenerateLaplaceMatrix(type, mesh);
            Eigen eigen=ComputeEigen(matrix,mesh.FileName,count);
            return eigen ;

        }

        public Eigen  ComputeEigen(SparseMatrix matrix, string modelName,int count)
        {
            Eigen eigen = LinearSystem.Instance.ComputeEigen(matrix, count, modelName);  
           
            return eigen;
        } 


        public Eigen ComputeEigen(TriMesh mesh,int count)
        {
            SparseMatrix sparse = LaplaceManager.Instance.BuildMatrixCot(mesh);
            Eigen eigen = LinearSystem.Instance.ComputeEigen(sparse,count, mesh.FileName);
            return eigen;
        }


    }
}
