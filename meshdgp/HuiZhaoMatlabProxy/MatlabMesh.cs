using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class MatlabProxy
    {

        public void OutPut(TriMesh mesh)
        { 
            double[,] V = CoverteVertice(mesh);
            double[,] F = CoverteFace(mesh);
            matlab.PutWorkspaceData("V", "base", V);
            matlab.PutWorkspaceData("F", "base", F);
          
        }

        public void ComputeLaplace(TriMesh mesh)
        {
            double[,] V = CoverteVertice(mesh);
            double[,] F = CoverteFace(mesh);
            matlab.PutWorkspaceData("V", "base", V);
            matlab.PutWorkspaceData("F", "base", F);
            CD(AppPath);
            matlab.Execute(@"L = cotmatrix(V,F)");
        }

        //public Eigen GetEign(TriMesh mesh, int num)
        //{
        //    SparseMatrix sparse = LaplaceBuilder.Instance.ComputeLaplaceCotHalf(mesh);
        //    return GetEigen(sparse, num);
        //}

        //public bool IsPSD(TriMesh mesh)
        //{
        //    SparseMatrix sparse = LaplaceBuilder.Instance.ComputeLaplaceCotHalf(mesh);
        //    return IsPSD(sparse);
        //}








    }
}
