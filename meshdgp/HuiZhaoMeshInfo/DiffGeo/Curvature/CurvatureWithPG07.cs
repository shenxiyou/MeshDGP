using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class CurvaturePG07
    {
        TriMesh mesh;
   

        public Vector3D[] K;
        public double[] Mean;
        public Vector3D[] Normal;
        public PrincipalCurvature[] PrincipalCurv;

        public CurvaturePG07(TriMesh mesh)
        {
            this.mesh = mesh; 

            this.K = new Vector3D[this.mesh.Vertices.Count];
            this.Mean = new double[this.mesh.Vertices.Count];
            this.Normal = new Vector3D[this.mesh.Vertices.Count];
            this.PrincipalCurv = new PrincipalCurvature[this.mesh.Vertices.Count];

            foreach (var v in this.mesh.Vertices)
            {
                double mixedArea =TriMeshUtil.ComputeAreaMixed(v);
                this.K[v.Index] = ComputeK(v, mixedArea);
                this.Mean[v.Index] = ComputeMeanCurvature(this.K[v.Index]);
                this.Normal[v.Index] = ComputeNormal(this.K[v.Index]);
            }

            foreach (var v in this.mesh.Vertices)
            {
                this.PrincipalCurv[v.Index] = this.ComputePrincipalCurvature(v);
            }
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
            return (K / 2).Length();
        }

        public static Vector3D ComputeNormal(Vector3D K)
        {
            return K.Normalize();
        }

        public PrincipalCurvature ComputePrincipalCurvature(TriMesh.Vertex v)
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
                sum += (cota + cotb) * (this.Normal[v.Index] - this.Normal[hf.ToVertex.Index]);
            }
            double mixedArea = TriMeshUtil.ComputeAreaMixed(v);
            Vector3D laplace = sum / mixedArea / 2d;
            double square = -laplace.Dot(this.Normal[v.Index]);
            double k = this.K[v.Index].Length();
            double delta = -k * k + 2d * square;
            if (delta < 0d)
            {
                delta = 0d;
            }
            PrincipalCurvature pc = new PrincipalCurvature();
            pc.max = (k + Math.Pow(delta, 0.5)) / 2d;
            pc.min = (k - Math.Pow(delta, 0.5)) / 2d;
            return pc;
        }

        public PrincipalCurvature ComputePrincipalDir(TriMesh.Vertex v)
        {
            Vector3D normal = this.Normal[v.Index];
            Vector3D s = v.Traits.Position;
            Vector3D fMax = s + normal / this.PrincipalCurv[v.Index].max;
            Vector3D fMin = s + normal / this.PrincipalCurv[v.Index].min;

            return null;
        }
    }
}
