using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class LapalceRecon
    {
        public TriMesh Recon(TriMesh mesh,int reconNum)
        {
            

            Eigen eigen = EigenManager.Instance.ComputeEigen(mesh, 
                          EnumLaplaceMatrix.LapalceCot, reconNum+10);

            List<double> X = TriMeshUtil.GetX(mesh);
            List<double> Y = TriMeshUtil.GetY(mesh);
            List<double> Z = TriMeshUtil.GetZ(mesh);

            List<double> factorX = new List<double>(reconNum);
            List<double> factorY = new List<double>(reconNum);
            List<double> factorZ = new List<double>(reconNum);
            for (int i = 0; i < reconNum; i++)
            {
                List<double> eigenVector = eigen.GetEigenVector(i) ;
                double valueX = TriMeshUtil.Multiply(eigenVector, X);
                double valueY = TriMeshUtil.Multiply(eigenVector, Y);
                double valueZ = TriMeshUtil.Multiply(eigenVector, Z);
                factorX.Add(valueX);
                factorY.Add(valueY);
                factorZ.Add(valueZ);
            }


            double[] reconX = new double[ mesh.Vertices.Count];
            double[] reconY = new double[mesh.Vertices.Count];
            double[] reconZ = new double[mesh.Vertices.Count];
            for (int i = 0; i < reconNum; i++)
            { 
                List<double> eigenVector = eigen.GetEigenVector(i);
                List<double> tempX=TriMeshUtil.Multiply(eigenVector, factorX[i]);
                reconX = TriMeshUtil.Add(reconX,tempX);

                List<double> tempY = TriMeshUtil.Multiply(eigenVector, factorY[i]);
                reconY = TriMeshUtil.Add(reconY, tempY);
                List<double> tempZ = TriMeshUtil.Multiply(eigenVector, factorZ[i]);
                reconZ = TriMeshUtil.Add(reconZ, tempZ); 
            }

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                mesh.Vertices[i].Traits.Position.x = reconX[i];
                mesh.Vertices[i].Traits.Position.y = reconY[i];
                mesh.Vertices[i].Traits.Position.z = reconZ[i];
            }


            return mesh;
        }


      
    }
}
