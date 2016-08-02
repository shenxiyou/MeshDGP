using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public partial class LaplaceManager
    {
        public double[][] ComputeLaplacianBasic(SparseMatrix L, TriMesh mesh)
        {
            if (L == null)
                throw new Exception("Laplacian matrix is null");

            int n = mesh.Vertices.Count; 
            double[] coordinate = new double[n];
            double[][] lap = new double[3][];  
            coordinate = TriMeshUtil.GetX(mesh).ToArray();
            lap[0]=L.Multiply(coordinate); 
            coordinate = TriMeshUtil.GetY(mesh).ToArray();
            lap[1] = L.Multiply(coordinate); 
            coordinate = TriMeshUtil.GetZ(mesh).ToArray();
            lap[2] = L.Multiply(coordinate);
            return lap;
        }




        public double[][] ComputeLaplacianUniform(TriMesh mesh)
        {
            SparseMatrix L = this.BuildMatrixCombinatorialGraphNormalized(mesh);

            double[][] lap = ComputeLaplacianBasic(L,mesh);


            return lap;
        }

        public double[][] ComputeLaplacianCotNormalize(TriMesh mesh)
        {
            SparseMatrix L = this.BuildMatrixCotNormalize(mesh);

            double[][] lap = ComputeLaplacianBasic(L, mesh);

            return lap;
        }

        public double[][] ComputeLaplacianMeanCurvature(TriMesh mesh)
        {
            SparseMatrix L = this.BuildMatrixMeanCurvature(mesh);

            double[][] lap = ComputeLaplacianBasic(L, mesh);

            return lap;
        }

        public double[][] ComputeLaplacianCot(TriMesh mesh)
        {
            SparseMatrix L = this.BuildMatrixCot(mesh);

            double[][] lap = ComputeLaplacianBasic(L, mesh);

            return lap;
        }

        public double[][] ComputeLaplacianRigid(TriMesh mesh)
        {
            SparseMatrix L = this.BuildMatrixRigid(mesh);

            double[][] lap = ComputeLaplacianBasic(L, mesh);

            return lap;
        }

        public double[][] ComputeLaplacianMeanCurvatureNormalize(TriMesh mesh)
        {
            SparseMatrix L = this.BuildMatrixMeanCurvatureNormalize(mesh);

            double[][] lap = ComputeLaplacianBasic(L, mesh);

            return lap;
        }

        public double[][] ComputeLaplacianCMCF(TriMesh mesh)
        {
            SparseMatrix L = this.BuildMatrixMass(mesh);  
            double[][] lap = ComputeLaplacianBasic(L, mesh); 
            return lap;
        }

        public double[][] ComputeLaplacianDual(TriMesh mesh)
        { 
            int fn = mesh.Faces.Count;
            double[][] laplacian = new double[3][];
            laplacian[0] = new double[fn];
            laplacian[1] = new double[fn];
            laplacian[2] = new double[fn]; 
            for (int i = 0; i < fn; i++)
            {
                int f1 = mesh.Faces[i].GetFace(0).Index;
                int f2 = mesh.Faces[i].GetFace(1).Index;
                int f3 = mesh.Faces[i].GetFace(2).Index; 
                Vector3D u = mesh.DualGetVertexPosition(i);
                Vector3D v1 = mesh.DualGetVertexPosition(f1);
                Vector3D v2 = mesh.DualGetVertexPosition(f2);
                Vector3D v3 = mesh.DualGetVertexPosition(f3); 
                Vector3D normal = ((v1 - v3).Cross(v2 - v3)).Normalize();
                Matrix3D m = new Matrix3D(v1 - v3, v2 - v3, normal);
                Vector3D coord = m.Inverse() * (u - v3); 
                laplacian[0][i] = normal.x * coord[2];
                laplacian[1][i] = normal.y * coord[2];
                laplacian[2][i] = normal.z * coord[2];
            } 
            return laplacian;
        }

    }
}
