using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices; 

namespace GraphicResearchHuiZhao
{
    public static class CurvatureLib
    {
        public static readonly string FileName = "PInvoke.obj";

        static int nv;

        public static void Init(TriMesh mesh)
        {
            nv = mesh.Vertices.Count;
            TriMeshIO.WriteToObjFile(FileName, mesh);

            FromObjFile(mesh.FileName);
        }

        public static double[] ComputeGaussianCurvature()
        {

            PrincipalCurvature[] cur = ComputeCurvature();
            double[] gaussian=new double[cur.Length];
            for (int i = 0; i < gaussian.Length; i++)
            {
                gaussian[i] = cur[i].max * cur[i].min;
            }

            return gaussian;
        }

        public static double[] ComputePrincipalMax()
        {

            PrincipalCurvature[] cur = ComputeCurvature();
            double[] max = new double[cur.Length];
            for (int i = 0; i < cur.Length; i++)
            {
                max[i] = cur[i].max  ;
            }

            return max;
        }

        public static double[] ComputePrincipalMin()
        {

            PrincipalCurvature[] cur = ComputeCurvature();
            double[] min = new double[cur.Length];
            for (int i = 0; i < cur.Length; i++)
            {
                min[i] = cur[i].min;
            }

            return min;
        }

        public static double[] ComputePrincipalMaxAbs()
        {

            PrincipalCurvature[] cur = ComputeCurvature();
            double[] max = new double[cur.Length];
            for (int i = 0; i < cur.Length; i++)
            {
                max[i] = Math.Abs(cur[i].max);
            }

            return max;
        }

        public static double[] ComputePrincipalMinAbs()
        {

            PrincipalCurvature[] cur = ComputeCurvature();
            double[] min = new double[cur.Length];
            for (int i = 0; i < cur.Length; i++)
            {
                min[i] =Math.Abs(cur[i].min);
            }

            return min;
        }


        public static double[] ComputeMeanCurvature()
        {

            PrincipalCurvature[] cur = ComputeCurvature();
            double[] mean = new double[cur.Length];
            for (int i = 0; i < cur.Length; i++)
            {
                mean[i] = (cur[i].max + cur[i].min) / 2;
            }

            return mean;
        }

        public static Vector3D[] ComputeVertexNormal()
        {
            Vector3D[] normal = new Vector3D[nv];
            float[] buff = new float[nv * 3];
            GetVertexNormal(buff);

            for (int i = 0; i < nv; i++)
            {
                normal[i] = new Vector3D(buff[i * 3], buff[i * 3 + 1], buff[i * 3 + 2]);
            }
            return normal;
        }

        public static PrincipalCurvature[] ComputeCurvature()
        {
            PrincipalCurvature[] pc = new PrincipalCurvature[nv];
            float[] buff = new float[nv * 8];
            int n = GetCurv(buff);

            for (int i = 0; i < nv; i++)
            {
                pc[i] = new PrincipalCurvature
                {
                    max = buff[i * 8],
                    min = buff[i * 8 + 1],
                    maxDir = new Vector3D(buff[i * 8 + 2], buff[i * 8 + 3], buff[i * 8 + 4]),
                    minDir = new Vector3D(buff[i * 8 + 5], buff[i * 8 + 6], buff[i * 8 + 7])
                };
            }
            return pc;
        }

        public static Vector4D[] ComputeCurD()
        {
            Vector4D[] dcurv = new Vector4D[nv];
            float[] buff = new float[nv * 4];
            GetDCurv(buff);

            for (int i = 0; i < nv; i++)
            {
                dcurv[i] = new Vector4D(buff[i * 4], buff[i * 4 + 1], buff[i * 4 + 2], buff[i * 4 + 3]);
            }
            return dcurv;
        }

        [DllImport("trimeshccdll.dll", EntryPoint = "FromObjFile", CallingConvention = CallingConvention.Cdecl)]
        extern static void FromObjFile(string file);

        [DllImport("trimeshccdll.dll", EntryPoint = "GetVertexNormal", CallingConvention = CallingConvention.Cdecl)]
        extern static int GetVertexNormal(float[] buff);

        [DllImport("trimeshccdll.dll", EntryPoint = "GetCurv", CallingConvention = CallingConvention.Cdecl)]
        extern static int GetCurv(float[] buff);

        [DllImport("trimeshccdll.dll", EntryPoint = "GetDCurv", CallingConvention = CallingConvention.Cdecl)]
        extern static int GetDCurv(float[] buff);
    }
}
