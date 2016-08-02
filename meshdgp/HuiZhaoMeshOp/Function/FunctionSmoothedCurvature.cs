using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{



    public class FunctionSmoothedCurvature
    {
        private double weightSmooth = -60.1;

        private int type = 2;

        private SparseMatrix BuildMatrixA(TriMesh Mesh)
        {
            int n = Mesh.Vertices.Count;
            SparseMatrix sparse = null;
            switch (type)
            {
                case 1:
                    sparse = LaplaceManager.Instance.BuildLaplaceTutte(Mesh);
                    break;
                case 2:
                    sparse = LaplaceManager.Instance.BuildMatrixCotNormalize(Mesh);
                    break;
                case 3:
                    sparse = LaplaceManager.Instance.BuildMatrixMeanCurvature(Mesh); 
                    break;
                default:
                    break;
            }

            sparse.Scale(weightSmooth);
            SparseMatrix identity = SparseMatrix.Identity(sparse.RowSize);
            sparse = sparse.Add(identity); 
            return sparse;
        }
        

        private double[] BuildRightB(TriMesh Mesh)
        {
            double[] b = TriMeshUtil.ComputeGaussianCurvatureIntegrated(Mesh);

            for (int i = 0; i < b.Length; i++)
            {
                b[i] = -(b[i]*b[i]);
            }
            return b; 
        }


       


        public double[] ComputeFunction(TriMesh mesh)
        {
            SparseMatrix sparse = BuildMatrixA(mesh);
            double[] rightB = BuildRightB(mesh); 
            double[] unknownX = LinearSystem.Instance.SolveSystem(ref sparse, ref rightB, mesh.FileName);
            return unknownX;
        }



    }
}
