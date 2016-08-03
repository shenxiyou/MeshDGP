using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{

     

    public class FunctionHarmonic
    {
        public int type =2; 
        public double Maximum = 100; 
        public double Minimum = 10; 
        public int MinFlag = 1;
        public int MaxFlag = 2;

        private SparseMatrix BuildMatrixA(TriMesh Mesh)
        {
            int n = Mesh.Vertices.Count;
            SparseMatrix sparse = new SparseMatrix(n, n); 
            switch (type)
            {
                case 1:
                    sparse = LaplaceManager.Instance.BuildMatrixCombinatorialGraphNormalized(Mesh);
                    break;
                case 2:
                    sparse = LaplaceManager.Instance.BuildMatrixMeanValue(Mesh);
                    break;
                case 3:
                    sparse = LaplaceManager.Instance.BuildMatrixCot(Mesh);
                    break;
                default:
                    break;
            } 
            return sparse;
        }

        private double[] BuildRightB(TriMesh Mesh)
        {
            int n = Mesh.Vertices.Count;
            double[] rightB = new double[n];

            for (int i = 0; i < n; i++)
            {
                rightB[i] = 0;
            }


            return rightB;
        }


        private void AddConstraints(SparseMatrix sparse, double[] rightB, TriMesh mesh)
        {
            List<int> min = new List<int>();
            List<int> max = new List<int>();  
            foreach (TriMesh.Vertex vertex in mesh.Vertices)
            {
                if (vertex.Traits.SelectedFlag == MinFlag)
                {
                    min.Add(vertex.Index);
                } 
                if (vertex.Traits.SelectedFlag == MaxFlag)
                {
                    max.Add(vertex.Index);
                }
            } 
            if (min.Count == 0)
            {
                min.Add(0);
            }
            if (max.Count == 0)
            {
                max.Add(mesh.Vertices.Count - 10);
            } 
            for (int i = 0; i < min.Count; i++)
            { 
                sparse.ClearRow(min[i]); 
                sparse[min[i], min[i]] = 1; 
                rightB[min[i]] = Minimum;
            } 
            for (int i = 0; i < max.Count; i++)
            { 
                sparse.ClearRow(max[i]); 
                sparse[max[i], max[i]] = 1; 
                rightB[max[i]] = Maximum;
            } 
        }


        public double[] ComputeHarmonicFunction(TriMesh mesh)
        {
            SparseMatrix sparse = BuildMatrixA(mesh);
            double[] rightB = BuildRightB(mesh); 
            AddConstraints(sparse, rightB, mesh); 
            double[] unknownX = LinearSystem.Instance.SolveSystem(ref sparse, ref rightB, mesh.FileName); 
            return unknownX;
        }


        
    }
}
