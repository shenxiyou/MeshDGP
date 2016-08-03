
 
using System;
 

namespace GraphicResearchHuiZhao
{
//#if CURVATURE
    /// <summary>
    /// A class containing static methods for computing curvature on a mesh.
    /// </summary>
    /// <remarks>
    /// The methods in this class are based on the C++ trimesh2 library
    /// (from TriMesh_curvature.cc and lineqn.h).
    /// </remarks>
    public static class Curvature
    {
        /// <summary>
        /// Given a curvature tensor, finds principal directions and curvatures.
        /// </summary>
        /// <param name="u">The u vector.</param>
        /// <param name="v">The v vector.</param>
        /// <param name="ku">ku.</param>
        /// <param name="kuv">kuv.</param>
        /// <param name="kv">kv.</param>
        /// <param name="normalNew">The new normal.</param>
        /// <param name="pDirMax">The maximum principle curvature direction.</param>
        /// <param name="pDirMin">The minimum principle curvature direction.</param>
        /// <param name="kMax">The maximum principle curvature.</param>
        /// <param name="kMin">The minimum principle curvature.</param>
        public static void DiagonalizeCurvature(Vector3D u, Vector3D v, double ku, double kuv, double kv, Vector3D normalNew,
            out Vector3D pDirMax, out Vector3D pDirMin, out double kMax, out double kMin)
        {
            Vector3D uRotated, vRotated;

            RotateCoordinateSystem(u, v, normalNew, out uRotated, out vRotated);

            double c = 1.0 ;
            double s = 0.0 ;
            double t = 0.0 ;

            // Jacobi rotation to diagonalize
            if (kuv != 0.0f)
            {
                double h = 0.5f * (kv - ku) / kuv;

                if (h < 0.0f)
                {
                    t = 1.0f / (h - (float)Math.Sqrt(1.0f + h * h));
                }
                else
                {
                    t = 1.0f / (h + (float)Math.Sqrt(1.0f + h * h));
                }

                c = 1.0f / (float)Math.Sqrt(1.0f + t * t);
                s = t * c;
            }

            kMax = ku - t * kuv;
            kMin = kv + t * kuv;

            if (Math.Abs(kMax) >= Math.Abs(kMin))
            {
                pDirMax = c * uRotated - s * vRotated;
            }
            else
            {
                double temp = kMin;
                kMin = kMax;
                kMax = temp;
                pDirMax = s * uRotated + c * vRotated;
            }

            pDirMin =  normalNew.Cross(pDirMax);
        }

        /// <summary>
        /// Performs the LDL^T decomposition of a symmetric positive definite matrix.
        /// </summary>
        /// <param name="A">An N by N SPD matrix, which has its lower triangle modified.</param>
        /// <param name="rDiag">A vector of size N, which is modified.</param>
        /// <returns>True if successful; otherwise, false.</returns>
        public static bool LdlTransposeDecomp(float[,] A, float[] rDiag)
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

            float[] v = new float[N - 1];

            for (int i = 0; i < N; ++i)
            {
                for (int k = 0; k < i; ++k)
                {
                    v[k] = A[i, k] * rDiag[k];
                }

                for (int j = i; j < N; ++j)
                {
                    float sum = A[i, j];

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
        public static float[] LdlTransposeSolve(float[,] A, float[] rDiag, float[] B)
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

            float[] x = new float[N];

            for (int i = 0; i < N; ++i)
            {
                float sum = B[i];

                for (int k = 0; k < i; ++k)
                {
                    sum -= A[i, k] * x[k];
                }

                x[i] = sum * rDiag[i];
            }

            for (int i = N - 1; i >= 0; --i)
            {
                float sum = 0.0f;

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
        public static void LdlTransposeSolveInPlace(float[,] A, float[] rDiag, float[] B)
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
                float sum = B[i];

                for (int k = 0; k < i; ++k)
                {
                    sum -= A[i, k] * B[k];
                }

                B[i] = sum * rDiag[i];
            }

            for (int i = N - 1; i >= 0; --i)
            {
                float sum = 0.0f;

                for (int k = i + 1; k < N; ++k)
                {
                    sum += A[k, i] * B[k];
                }

                B[i] -= sum * rDiag[i];
            }
        }

        /// <summary>
        /// Projects a curvature tensor from an old basis to a new one.
        /// </summary>
        /// <param name="uOld">Old u.</param>
        /// <param name="vOld">Old v.</param>
        /// <param name="kuOld">Old ku.</param>
        /// <param name="kuvOld">Old kuv.</param>
        /// <param name="kvOld">Old kv.</param>
        /// <param name="uNew">New u</param>
        /// <param name="vNew">New v.</param>
        /// <param name="kuNew">New ku.</param>
        /// <param name="kuvNew">New kuv.</param>
        /// <param name="kvNew">New kv.</param>
        public static void ProjectCurvature(Vector3D uOld, Vector3D vOld, float kuOld, float kuvOld, float kvOld, Vector3D uNew, Vector3D vNew,
            out float kuNew, out float kuvNew, out float kvNew)
        {
            Vector3D uNewRotated, vNewRotated;

            RotateCoordinateSystem(uNew, vNew,  uOld.Cross(vOld), out uNewRotated, out vNewRotated);

            float u1 =(float) uNewRotated.Dot(uOld);
            float v1 = (float)uNewRotated.Dot(vOld);
            float u2 = (float)vNewRotated.Dot(uOld);
            float v2 = (float)vNewRotated.Dot(vOld);

            kuNew  = kuOld * u1 * u1 + kuvOld * (2.0f    * u1 * v1) + kvOld * v1 * v1;
            kuvNew = kuOld * u1 * u2 + kuvOld * (u1 * v2 + u2 * v1) + kvOld * v1 * v2;
            kvNew  = kuOld * u2 * u2 + kuvOld * (2.0f    * u2 * v2) + kvOld * v2 * v2;
        }

        /// <summary>
        /// Rotates a 3D coordinate system to match a specified normal.
        /// </summary>
        /// <param name="uOld">Old u.</param>
        /// <param name="vOld">Old v.</param>
        /// <param name="normalNew">The normal to match.</param>
        /// <param name="uNew">New u.</param>
        /// <param name="vNew">New v.</param>
        public static void RotateCoordinateSystem(Vector3D uOld, Vector3D vOld, Vector3D normalNew,
            out Vector3D uNew, out Vector3D vNew)
        {
            Vector3D normalOld =  uOld.Cross(vOld);
            float normalsDot =(float)  normalOld.Dot(normalNew);

            if (normalsDot <= -1.0f)
            {
                uNew = uOld * -1;
                vNew = vOld * -1; ;
            }
            else
            {
                Vector3D perpOld = normalNew - normalsDot * normalOld;
                Vector3D dPerp = 1.0f / (1.0f + normalsDot) * (normalOld + normalNew);

                uNew = uOld - dPerp *  uOld.Dot(perpOld);
                vNew = vOld - dPerp *  vOld.Dot(perpOld);
            }
        }
    }
//#endif
}
