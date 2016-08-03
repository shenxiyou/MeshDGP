using System;
using System.Collections.Generic;
using System.Text;



namespace GraphicResearchHuiZhao
{
    public partial class TriMeshFunction
    {

        public static double[] ComputeDistanceADF(TriMesh mesh, double parameterT)
        {
            Eigen eigen = EigenManager.Instance.ComputeEigen(mesh, EnumLaplaceMatrix.LapalceCot, 50);
            int sourceVertex = TriMeshUtil.RetrieveSelectedVertexFirst(mesh).Index;

            double[] distance = ComputeDistanceADF(mesh,parameterT,eigen); ;
            return distance;
        }

        public static double[] ComputeDistanceADF(TriMesh mesh, double parameterT, Eigen eigens)
        {
            double[] adfValues = new double[mesh.Vertices.Count];

            for (int j = 1; j < eigens.Count; j++)
            {
                List<double> group = eigens.SortedEigens[j].EigenVector;
                for (int i = 0; i < group.Count; i++)
                {
                    adfValues[i] += Math.Exp(-parameterT * (eigens.SortedEigens[j].EigenValue / eigens.SortedEigens[1].EigenValue)) *
                        (group[i] * group[i]);
                }
            }

            return adfValues;


        }




        public static double[] ComputeDistanceCommuteTime(int originVertexIndex, 
                                                          Eigen eigens)
        {
            double[] diffusionDistance = new double[eigens.EigenVectorSize];
            for (int i = 0; i < diffusionDistance.Length; i++)
            {
                diffusionDistance[i] = 0;
            } 
            for (int i = 1; i < eigens.Count; i++)
            {
                EigenPair pair = eigens.SortedEigens[i];
                double eigenValue = pair.EigenValue;
                List<double> eigenVector = pair.EigenVector;
                for (int j = 0; j < eigenVector.Count; j++)
                {
                    double orig = eigenVector[originVertexIndex];
                    double y = eigenVector[j];
                    double res = orig - y;
                    diffusionDistance[j] +=  (1/eigenValue) * (res * res);
                }

            }
            for (int i = 0; i < diffusionDistance.Length; i++)
            {
                diffusionDistance[i] = Math.Sqrt(diffusionDistance[i]);
            }
            return diffusionDistance;
        }



  

        public static double[] ComputeDistanceDiffusion(int originVertexIndex, 
                                                        double parameterT, 
                                                        Eigen eigens)
        {
            double[] diffusionDistance = new double[eigens.EigenVectorSize];
            for (int i = 0; i < diffusionDistance.Length; i++)
            {
                diffusionDistance[i] = 0;
            }
            double optParamterT =parameterT / (2 * eigens.GetEigenValue(1));
            for (int i = 1; i < eigens.Count; i++)
            {
                EigenPair pair = eigens.SortedEigens[i];
                double eigenValue = pair.EigenValue;
                List<double> eigenVector = pair.EigenVector;                          
                for (int j = 0; j < eigenVector.Count; j++)
                {
                    double orig = eigenVector[originVertexIndex];
                    double y = eigenVector[j];
                    double res = orig - y;
                    diffusionDistance[j] += Math.Exp(-2 * optParamterT * eigenValue) 
                                            * (res * res);                  
                }
                          
            }
            for (int i = 0; i < diffusionDistance.Length; i++)
            {
                diffusionDistance[i] = Math.Sqrt(diffusionDistance[i]);    
            }
            return diffusionDistance;
        }

        public static double[] ComputeDistanceBiharmonic(int sourceVertex, Eigen eigens)
        { 
            double[] biharmonicDistance = new double[eigens.EigenVectorSize];
            for (int i = 0; i < biharmonicDistance.Length; i++)
            {
                biharmonicDistance[i] = 0;
            } 
            for (int i = 1; i < eigens.Count; i++)
            {
                EigenPair pair = eigens.SortedEigens[i]; 
                double eigenValue = pair.EigenValue; 
                List<double> eigenVector = pair.EigenVector;  
                for (int j = 0; j < eigenVector.Count; j++)
                {
                    double orig = eigenVector[sourceVertex];
                    double y = eigenVector[j];
                    double res = orig - y;
                    biharmonicDistance[j] += (res * res)/((eigenValue) * (eigenValue)); 
                }
            } 
            for (int i = 0; i < biharmonicDistance.Length; i++)
            {
                biharmonicDistance[i] = Math.Sqrt(biharmonicDistance[i]);
            } 
            return biharmonicDistance;
        }
  
        public static double[] ComputeDistanceDiffusion(TriMesh mesh, double parameterT)
        {
            int sourceVertex = TriMeshUtil.RetrieveSelectedVertexFirst(mesh).Index;

            Eigen eigen = EigenManager.Instance.ComputeEigen(mesh, EnumLaplaceMatrix.LapalceCot, 50);

            double[] distance = ComputeDistanceDiffusion(sourceVertex, parameterT, eigen);
            return distance;

        }

        public static double[] ComputeDistanceCommuteTime(TriMesh mesh)
        {
            int sourceVertex = TriMeshUtil.RetrieveSelectedVertexFirst(mesh).Index;
            Eigen eigen = EigenManager.Instance.ComputeEigen(mesh, EnumLaplaceMatrix.LapalceCot, 50);
            double[] distance = ComputeDistanceCommuteTime(sourceVertex, eigen);
            return distance;
        }
 

        public static double[] ComputeDistanceBiharmonic(TriMesh mesh)
        {
            Eigen eigen = EigenManager.Instance.ComputeEigen(mesh, EnumLaplaceMatrix.LapalceCot, ConfigFunction.Instance.EigenCount);
            int sourceVertex = TriMeshUtil.RetrieveSelectedVertexFirst(mesh).Index;  

            double[] distance = ComputeDistanceBiharmonic(sourceVertex,eigen); ;
            return distance;
        }

        public static double[] ComputeEigenVector(TriMesh mesh, int i)
        {
            SparseMatrix matrix = null;
            if (LaplaceManager.Instance.CurrentMatrix == null)
            {
                 matrix = LaplaceManager.Instance.BuildMatrixCot(mesh);
            }
            else
            {
                 matrix = LaplaceManager.Instance.CurrentMatrix;
            }
           
            Eigen eigen = EigenManager.Instance.ComputeEigen(matrix, "test", i + 2);

            return eigen.GetEigenVector(i).ToArray(); ;
        }

        public static double[] ComputeEigenVector(SparseMatrix matrix, int i)
        {
            Eigen eigen = EigenManager.Instance.ComputeEigen(matrix,"test", i+20);

            return eigen.GetEigenVector(i).ToArray(); ;
        }

        public static double[] ComputeEigenVector(Eigen eigen, int i)
        {  
            return eigen.GetEigenVector(i).ToArray(); ;
        }




    }


}
