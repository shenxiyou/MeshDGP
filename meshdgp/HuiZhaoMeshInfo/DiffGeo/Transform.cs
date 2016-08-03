using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public static class Transform
    {
        /// <summary>
        /// Performs the LDL^T decomposition of a symmetric positive definite matrix.
        /// </summary>
        /// <param name="A">An N by N SPD matrix, which has its lower triangle modified.</param>
        /// <param name="rDiag">A vector of size N, which is modified.</param>
        /// <returns>True if successful; otherwise, false.</returns>
        public static bool LdlTransposeDecomp(double[,] A, double[] rDiag)
        {
            //if (A == null)
            //{
            //    throw new ArgumentNullException("A");
            //}
            //if (rDiag == null)
            //{
            //    throw new ArgumentNullException("rDiag");
            //}

            int N = rDiag.Length;

            if (A.GetLength(0) != N || A.GetLength(1) != N)
            {
                throw new ArgumentException("A and rDiag dimensions must match.");
            }

            double[] v = new double[N - 1];

            for (int i = 0; i < N; ++i)
            {
                for (int k = 0; k < i; ++k)
                {
                    v[k] = A[i, k] * rDiag[k];
                }

                for (int j = i; j < N; ++j)
                {
                    double sum = A[i, j];

                    for (int k = 0; k < i; ++k)
                    {
                        sum -= v[k] * A[j, k];
                    }

                    if (i == j)
                    {
                        if (sum <= 0.0f)
                        {
                            return false;
                        }

                        rDiag[i] = 1.0f / sum;
                    }
                    else
                    {
                        A[j, i] = sum;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Solves the linear system Ax = B after LdlTransposeDecomp.
        /// </summary>
        /// <param name="A">A square N by N matrix.</param>
        /// <param name="rDiag">A vector of size N.</param>
        /// <param name="B">A vector of size N.</param>
        /// <returns>A vector of size N.</returns>
        public static double[] LdlTransposeSolve(double[,] A, double[] rDiag, double[] B)
        {
            //if (A == null)
            //{
            //    throw new ArgumentNullException("A");
            //}
            //if (rDiag == null)
            //{
            //    throw new ArgumentNullException("rDiag");
            //}
            //if (B == null)
            //{
            //    throw new ArgumentNullException("B");
            //}

            int N = rDiag.Length;

            if (A.GetLength(0) != N || A.GetLength(1) != N || B.Length != N)
            {
                throw new ArgumentException("A, rDiag, and B dimensions must match.");
            }

            double[] x = new double[N];

            for (int i = 0; i < N; ++i)
            {
                double sum = B[i];

                for (int k = 0; k < i; ++k)
                {
                    sum -= A[i, k] * x[k];
                }

                x[i] = sum * rDiag[i];
            }

            for (int i = N - 1; i >= 0; --i)
            {
                double sum = 0.0f;

                for (int k = i + 1; k < N; ++k)
                {
                    sum += A[k, i] * x[k];
                }

                x[i] -= sum * rDiag[i];
            }

            return x;
        }

        /// <summary>
        /// Solves the linear system Ax = B after LdlTransposeDecomp, but stores x in B.
        /// </summary>
        /// <param name="A">A square N by N matrix.</param>
        /// <param name="rDiag">A vector of size N.</param>
        /// <param name="B">A vector of size N, which will be replaced by the solution.</param>
        public static void LdlTransposeSolveInPlace(double[,] A, double[] rDiag, double[] B)
        {
            //if (A == null)
            //{
            //    throw new ArgumentNullException("A");
            //}
            //if (rDiag == null)
            //{
            //    throw new ArgumentNullException("rDiag");
            //}
            //if (B == null)
            //{
            //    throw new ArgumentNullException("B");
            //}

            int N = rDiag.Length;

            if (A.GetLength(0) != N || A.GetLength(1) != N || B.Length != N)
            {
                throw new ArgumentException("A, rDiag, and B dimensions must match.");
            }

            for (int i = 0; i < N; ++i)
            {
                double sum = B[i];

                for (int k = 0; k < i; ++k)
                {
                    sum -= A[i, k] * B[k];
                }

                B[i] = sum * rDiag[i];
            }

            for (int i = N - 1; i >= 0; --i)
            {
                double sum = 0.0f;

                for (int k = i + 1; k < N; ++k)
                {
                    sum += A[k, i] * B[k];
                }

                B[i] -= sum * rDiag[i];
            }
        }

        /// <summary>
        /// A class containing static methods for computing curvature on a mesh.
        /// </summary>
        public static PrincipalCurvature DiagonalizeCurvature(UV src, KUV srcK, Vector3D dstNormal)
        {
            UV rotated = Transform.RotateCoordinateSystem(src, dstNormal);

            double c = 1.0;
            double s = 0.0;
            double t = 0.0;

            // Jacobi rotation to diagonalize
            if (srcK.UV != 0.0f)
            {
                double h = 0.5f * (srcK.V - srcK.U) / srcK.UV;

                if (h < 0.0f)
                {
                    t = 1.0f / (h - Math.Sqrt(1.0f + h * h));
                }
                else
                {
                    t = 1.0f / (h + Math.Sqrt(1.0f + h * h));
                }

                c = 1.0f / Math.Sqrt(1.0f + t * t);
                s = t * c;
            }

            PrincipalCurvature pc = new PrincipalCurvature();

            pc.max = srcK.U - t * srcK.UV;
            pc.min = srcK.V + t * srcK.UV;

            if (Math.Abs(pc.max) >= Math.Abs(pc.min))
            {
                pc.maxDir = c * rotated.U - s * rotated.V;
            }
            else
            {
                double temp = pc.min;
                pc.min = pc.max;
                pc.max = temp;
                pc.maxDir = s * rotated.U + c * rotated.V;
            }

            pc.minDir = dstNormal.Cross(pc.maxDir);

            return pc;
        }

        /// <summary>
        /// Projects a curvature tensor from an old basis to a new one.
        /// </summary>
        public static KUV ProjectCurvature(UV src, KUV srcK, UV dst)
        {
            UV rotated = Transform.RotateCoordinateSystem(dst, src.U.Cross(src.V));

            double u1 = rotated.U.Dot(src.U);
            double v1 = rotated.U.Dot(src.V);
            double u2 = rotated.V.Dot(src.U);
            double v2 = rotated.V.Dot(src.V);

            KUV dstK = new KUV()
            {
                U = srcK.U * u1 * u1 + srcK.UV * (2.0f * u1 * v1) + srcK.V * v1 * v1,
                UV = srcK.U * u1 * u2 + srcK.UV * (u1 * v2 + u2 * v1) + srcK.V * v1 * v2,
                V = srcK.U * u2 * u2 + srcK.UV * (2.0f * u2 * v2) + srcK.V * v2 * v2
            };
            return dstK;
        }

        public static Vector4D ProjectDCurvature(UV src, Vector4D srcDCurv, UV dst)
        {
            UV rotated = Transform.RotateCoordinateSystem(dst, src.U.Cross(src.V));

            double u1 = rotated.U.Dot(src.U);
            double v1 = rotated.U.Dot(src.V);
            double u2 = rotated.V.Dot(src.U);
            double v2 = rotated.V.Dot(src.V);

            Vector4D dstDCurv = Vector4D.Zero;
            dstDCurv[0] = srcDCurv[0] * u1 * u1 * u1 +
                       srcDCurv[1] * 3.0f * u1 * u1 * v1 +
                       srcDCurv[2] * 3.0f * u1 * v1 * v1 +
                       srcDCurv[3] * v1 * v1 * v1;
            dstDCurv[1] = srcDCurv[0] * u1 * u1 * u2 +
                       srcDCurv[1] * (u1 * u1 * v2 + 2.0f * u2 * u1 * v1) +
                       srcDCurv[2] * (u2 * v1 * v1 + 2.0f * u1 * v1 * v2) +
                       srcDCurv[3] * v1 * v1 * v2;
            dstDCurv[2] = srcDCurv[0] * u1 * u2 * u2 +
                       srcDCurv[1] * (u2 * u2 * v1 + 2.0f * u1 * u2 * v2) +
                       srcDCurv[2] * (u1 * v2 * v2 + 2.0f * u2 * v2 * v1) +
                       srcDCurv[3] * v1 * v2 * v2;
            dstDCurv[3] = srcDCurv[0] * u2 * u2 * u2 +
                       srcDCurv[1] * 3.0f * u2 * u2 * v2 +
                       srcDCurv[2] * 3.0f * u2 * v2 * v2 +
                       srcDCurv[3] * v2 * v2 * v2;
            return dstDCurv;
        }

        /// <summary>
        /// Rotates a 3D coordinate system to match a specified normal.
        /// </summary>
        public static UV RotateCoordinateSystem(UV src, Vector3D dstNormal)
        {
            Vector3D srcNormal = src.U.Cross(src.V);
            double cos = srcNormal.Dot(dstNormal);

            UV dst;
            if (cos <= -1.0f)
            {
                dst.U = -src.U;
                dst.V = -src.V;
            }
            else
            {
                Vector3D perpOld = dstNormal - cos * srcNormal;
                Vector3D dPerp = 1.0f / (1.0f + cos) * (srcNormal + dstNormal);

                dst.U = src.U - dPerp * src.U.Dot(perpOld);
                dst.V = src.V - dPerp * src.V.Dot(perpOld);
            }
            return dst;
        }
    }
}
