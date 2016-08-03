using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public static partial class TriMeshUtil
    {

        public static double ComputeTotalGaussianCurvarture(TriMesh mesh)
        {
            double[] gaussian = ComputeGaussianCurvature(mesh);
            double[] mixedArea = ComputeAreaMixed(mesh);
            double sum = 0;

            for (int i = 0; i < gaussian.Length; i++)
            {
               
                sum += gaussian[i]* mixedArea[i];
            }

            return sum;
        }

        public static double ComputeGaussianCurvature(TriMesh.Vertex v)
        {
            double mixedArea = ComputeAreaMixed(v);
            return ComputeGaussianCurvature(v, mixedArea);
        }

         

        public static double ComputeGaussianCurvature(TriMesh.Vertex v, double mixedArea)
        {
            double curvature = ComputeGaussianCurvatureIntegrated(v);
            return curvature / mixedArea;
        }

        public static double ComputeGaussianCurvatureIntegrated(TriMesh.Vertex v)
        {
            bool onBoundary = false;
            double curvature = 0;
            foreach (var hf in v.HalfEdges)
            {
                if (hf.OnBoundary)
                {
                    onBoundary = true;
                }
                else
                {
                    Vector3D v1 = (hf.Previous.FromVertex.Traits.Position 
                                   - v.Traits.Position).Normalize();
                    Vector3D v2 = (hf.ToVertex.Traits.Position 
                                   - v.Traits.Position).Normalize();
                    curvature -= ComputeAngle(ref v1, ref v2);
                }
            }
            curvature += onBoundary ? Math.PI : 2 * Math.PI;
            return curvature;
        }

        private static double ComputeAngle(ref Vector3D v1, ref Vector3D v2)
        {
            double cosa = v1.Dot(v2);
            double angle = Math.Acos(MathUtil.ClampWithRange(cosa, -1, 1));
            return angle; ;
        }

        public static double[] ComputeGaussianCurvature(TriMesh mesh)
        {
            double[] gauss = new double[mesh.Vertices.Count];
            double[] mixedArea = ComputeAreaMixed(mesh);
            foreach (var v in mesh.Vertices)
            {
                gauss[v.Index] = ComputeGaussianCurvature(v, mixedArea[v.Index]);
            }
            return gauss;
        }

        public static double[] ComputeGaussianCurvatureIntegrated(TriMesh mesh)
        {
            double[] gauss = new double[mesh.Vertices.Count];
 
            foreach (var v in mesh.Vertices)
            {
                gauss[v.Index] =ComputeGaussianCurvatureIntegrated(v );
            }
            return gauss;
        }



        public static Vector3D ComputeK(TriMesh.Vertex v)
        {
            double mixedArea = ComputeAreaMixed(v);
            return ComputeK(v, mixedArea);
        }

        public static Vector3D ComputeK(TriMesh.Vertex v, double mixedArea)
        {
            Vector3D sum = Vector3D.Zero;
            Vector3D mid = v.Traits.Position;
            foreach (var hf in v.HalfEdges)
            {
                Vector3D buttom = hf.ToVertex.Traits.Position;
                Vector3D left = hf.Opposite.Next.ToVertex.Traits.Position;
                Vector3D right = hf.Next.ToVertex.Traits.Position;
                double cota = (mid - left).Dot(buttom - left) / (mid - left).Cross(buttom - left).Length();
                double cotb = (mid - right).Dot(buttom - right) / (mid - right).Cross(buttom - right).Length();
                sum += (cota + cotb) * (buttom - mid);
            }
            return sum / mixedArea / 2d;
        }

        public static double ComputeMeanCurvature(Vector3D K)
        {
            return (K / 2d).Length();
        }

        public static double ComputeMeanCurvature(TriMesh.Vertex v)
        {
            return (ComputeK(v) / 2d).Length();
        }

        public static double[] ComputeMeanCurvature(TriMesh mesh)
        {
            int n = mesh.Vertices.Count;
            double[] meanCurvature = new double[n];
            for (int i = 0; i < n; i++)
            {
                meanCurvature[i] = 0;
            }

            double[][] lap = LaplaceManager.Instance.ComputeLaplacianMeanCurvature(mesh);

            for (int i = 0; i < n; i++)
            {
                meanCurvature[i] = Math.Sqrt(lap[0][i] * lap[0][i] + lap[1][i] * lap[1][i] + lap[2][i] * lap[2][i]);
            }

            return meanCurvature;
        }

        public static PrincipalCurvature ComputePricipalCurvature(TriMesh.Vertex v)
        {
            double mean = ComputeMeanCurvature(v);
            double gauss = ComputeGaussianCurvature(v);
            return ComputePricipalCurvature(mean, gauss);
        }

        public static PrincipalCurvature ComputePricipalCurvature(double mean, double gauss)
        {
            double rightTemp = mean * mean - gauss;
            double rightPart;
            if (rightTemp <= 0)
            {
                rightPart = 0;
            }
            else
            {
                rightPart = Math.Sqrt(rightTemp);
            }
            PrincipalCurvature pc = new PrincipalCurvature();
            pc.max = mean + rightPart;
            pc.min = mean - rightPart;
            return pc;
        }

         

        public static PrincipalCurvature[] ComputePricipalCurvature(TriMesh mesh)
        {
            double[] mean = ComputeMeanCurvature(mesh);
            double[] gauss = ComputeGaussianCurvatureIntegrated(mesh);

            int n = mesh.Vertices.Count;

            PrincipalCurvature[] pricipal = new PrincipalCurvature[n];

            for (int i = 0; i < n; i++)
            {  
                pricipal[i] = ComputePricipalCurvature(mean[i], gauss[i]);
            }

            return pricipal;
        }

        public static PrincipalCurvature[] ComputePricipalCurvaturePG07(TriMesh mesh)
        {
            CurvaturePG07 pg = new CurvaturePG07(mesh);
            return pg.PrincipalCurv; 
            
        }


       

       
    }
}
