using System;
using System.Collections.Generic;
using System.Text;
 
namespace GraphicResearchHuiZhao 
{
    public static class MeshOperators
    {

        public static SparseMatrix CreateMatrixATA(ref SparseMatrix A,ref NonManifoldMesh mesh)
        {
            // assume A is sorted
            // assume values in parameter adj is in order

            if (A == null)
                throw new Exception("A matrix is null");

            int[][] adj = MeshOperators.BuildTwoRingVV(ref mesh).GetRowIndex();
            int n = A.ColumnSize;
            SparseMatrix ATA = new SparseMatrix(n, n);

            for (int i = 0; i < n; i++)
            {
                List<SparseMatrix.Element> col1 = A.GetColumn(i);
                foreach (int j in adj[i])
                {
                    List<SparseMatrix.Element> col2 = A.GetColumn(j);
                    int c1 = 0, c2 = 0;
                    double sum = 0.0;
                    bool used = false;

                    while (c1 < col1.Count && c2 < col2.Count)
                    {
                        if (col1[c1].i < col2[c2].i) { c1++; continue; }
                        if (col1[c1].i > col2[c2].i) { c2++; continue; }
                        sum += col1[c1].value * col2[c2].value;
                        used = true;
                        c1++;
                        c2++;
                    }

                    if (used)
                        ATA.AddElement(i, j, sum);
                }
            }

            if (ATA.IsSymmetric() == false) throw new Exception("ATA is not symmetric!!");
            return ATA;
        }      

        private static void Copy(double[] from, double[] to)
        {
            for (int i = 0; i < from.Length; i++)
                to[i] = from[i];
        }

        public static void ResetVertexPosition(ref double[] backupPos,ref NonManifoldMesh mesh)
        {
            if (backupPos == null)
                return;

            Copy(backupPos, mesh.VertexPos);
            mesh.ComputeDualPosition();
            mesh.ComputeFaceNormal();
            mesh.ComputeVertexNormal();
        }

        public static double[][] ComputeUniformLap(ref NonManifoldMesh mesh)
        {
            SparseMatrix L = MeshOperators.BuildUniformMatrixL(ref mesh);
            if (L == null)
                throw new Exception("Laplacian matrix is null");

            int n = mesh.VertexCount;
            double[] v = new double[n];
            double[][] lap = new double[3][];

            for (int i = 0; i < 3; i++)
            {
                lap[i] = new double[n];
                for (int j = 0, k = 0; j < n; j++, k += 3)
                    v[j] = mesh.VertexPos[k + i];
                L.Multiply(v, 0, lap[i], 0);
            }

            return lap;
        }

        public static double[][] ComputeCotLap(ref NonManifoldMesh mesh)
        {
            SparseMatrix  L = MeshOperators.BuildCotMatrixL(ref mesh);
            if (L == null)
                throw new Exception("Laplacian matrix is null");

            int n = mesh.VertexCount;
            double[] v = new double[n];
            double[][] lap = new double[3][];

            for (int i = 0; i < 3; i++)
            {
                lap[i] = new double[n];
                for (int j = 0, k = 0; j < n; j++, k += 3)
                    v[j] = mesh.VertexPos[k + i];
                L.Multiply(v, 0, lap[i], 0);
            }

            return lap;
        }

        public static double[][] ComputeCurvatureLap(ref NonManifoldMesh mesh)
        {
            SparseMatrix L = MeshOperators.BuildCurvatureMatrixL(ref mesh);
            if (L == null)
                throw new Exception("Laplacian matrix is null");

            int n = mesh.VertexCount;
            double[] v = new double[n];
            double[][] lap = new double[3][];

            for (int i = 0; i < 3; i++)
            {
                lap[i] = new double[n];
                for (int j = 0, k = 0; j < n; j++, k += 3)
                    v[j] = mesh.VertexPos[k + i];
                L.Multiply(v, 0, lap[i], 0);
            }

            return lap;
        }

        public static double[][] ComputeNormalizeCotLap(ref NonManifoldMesh mesh)
        {
            SparseMatrix L = MeshOperators.BuildCurvatureMatrixL(ref mesh);
            if (L == null)
                throw new Exception("Laplacian matrix is null");

            int n = mesh.VertexCount;
            double[] v = new double[n];
            double[][] lap = new double[3][];

            for (int i = 0; i < 3; i++)
            {
                lap[i] = new double[n];
                for (int j = 0, k = 0; j < n; j++, k += 3)
                    v[j] = mesh.VertexPos[k + i];
                L.Multiply(v, 0, lap[i], 0);
            }

            return lap;
        }


        public static SparseMatrix BuildUniformMatrixL(ref NonManifoldMesh mesh)
        {
            int n = mesh.VertexCount;
            SparseMatrix L = mesh.BuildAdjacentMatrix();

            for (int i = 0; i < n; i++)
            {
                double sum = L.Rows[i].Count;

                foreach (SparseMatrix.Element e in L.Rows[i])
                {
                    L.AddValueTo(i, e.j, -1 / sum - 1);
                }

                L.AddValueTo(i, i, 1);
            }

            L.SortElement();
            return L;
        }


        /// <summary>
        /// without divide by 2
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public static SparseMatrix BuildCommonCotMatrixL(ref NonManifoldMesh mesh)
        {
            if (mesh == null)
                throw new Exception("mesh is null");


            int n = mesh.VertexCount;
            SparseMatrix L = new SparseMatrix(n, n);

            for (int i = 0, j = 0; i < mesh.FaceCount; i++, j += 3)
            {

                int c1 = mesh.FaceIndex[j];
                int c2 = mesh.FaceIndex[j + 1];
                int c3 = mesh.FaceIndex[j + 2];
                Vector3D v1 = new Vector3D(mesh.VertexPos, c1 * 3);
                Vector3D v2 = new Vector3D(mesh.VertexPos, c2 * 3);
                Vector3D v3 = new Vector3D(mesh.VertexPos, c3 * 3);
                double cot1 = (v2 - v1).Dot(v3 - v1) / (v2 - v1).Cross(v3 - v1).Length();
                double cot2 = (v3 - v2).Dot(v1 - v2) / (v3 - v2).Cross(v1 - v2).Length();
                double cot3 = (v1 - v3).Dot(v2 - v3) / (v1 - v3).Cross(v2 - v3).Length();
                L.AddValueTo(c1, c2, -cot3 ); L.AddValueTo(c2, c1, -cot3);
                L.AddValueTo(c2, c3, -cot1); L.AddValueTo(c3, c2, -cot1);
                L.AddValueTo(c3, c1, -cot2); L.AddValueTo(c1, c3, -cot2 );
            }

            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                foreach (SparseMatrix.Element e in L.Rows[i])
                    sum += e.value;
                L.AddValueTo(i, i, -sum);
            }



            L.SortElement();
            return L;

        }
        /// <summary>
        /// divide by 2
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public static SparseMatrix BuildCotMatrixL(ref NonManifoldMesh mesh)
        {
            if (mesh == null)
                throw new Exception("mesh is null");


            int n = mesh.VertexCount;
            SparseMatrix L = new SparseMatrix(n, n);

            for (int i = 0, j = 0; i < mesh.FaceCount; i++, j += 3)
            {

                int c1 = mesh.FaceIndex[j];
                int c2 = mesh.FaceIndex[j + 1];
                int c3 = mesh.FaceIndex[j + 2];
                Vector3D v1 = new Vector3D(mesh.VertexPos, c1 * 3);
                Vector3D v2 = new Vector3D(mesh.VertexPos, c2 * 3);
                Vector3D v3 = new Vector3D(mesh.VertexPos, c3 * 3);
                double cot1 = (v2 - v1).Dot(v3 - v1) / (v2 - v1).Cross(v3 - v1).Length();
                double cot2 = (v3 - v2).Dot(v1 - v2) / (v3 - v2).Cross(v1 - v2).Length();
                double cot3 = (v1 - v3).Dot(v2 - v3) / (v1 - v3).Cross(v2 - v3).Length();
                L.AddValueTo(c1, c2, -cot3 / 2); L.AddValueTo(c2, c1, -cot3 / 2);
                L.AddValueTo(c2, c3, -cot1 / 2); L.AddValueTo(c3, c2, -cot1 / 2);
                L.AddValueTo(c3, c1, -cot2 / 2); L.AddValueTo(c1, c3, -cot2 / 2);
            }

            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                foreach (SparseMatrix.Element e in L.Rows[i])
                    sum += e.value;
                L.AddValueTo(i, i, -sum);
            }



            L.SortElement();
            return L;

        }

        public static SparseMatrix BuildNormalizeCotMatrixL(ref NonManifoldMesh mesh)
        {
            if (mesh == null)
                throw new Exception("mesh is null");


            int n = mesh.VertexCount;
            SparseMatrix L = new SparseMatrix(n, n);

            for (int i = 0, j = 0; i < mesh.FaceCount; i++, j += 3)
            {

                int c1 = mesh.FaceIndex[j];
                int c2 = mesh.FaceIndex[j + 1];
                int c3 = mesh.FaceIndex[j + 2];
                Vector3D v1 = new Vector3D(mesh.VertexPos, c1 * 3);
                Vector3D v2 = new Vector3D(mesh.VertexPos, c2 * 3);
                Vector3D v3 = new Vector3D(mesh.VertexPos, c3 * 3);
                double cot1 = (v2 - v1).Dot(v3 - v1) / (v2 - v1).Cross(v3 - v1).Length();
                double cot2 = (v3 - v2).Dot(v1 - v2) / (v3 - v2).Cross(v1 - v2).Length();
                double cot3 = (v1 - v3).Dot(v2 - v3) / (v1 - v3).Cross(v2 - v3).Length();
                L.AddValueTo(c1, c2, -cot3); L.AddValueTo(c2, c1, -cot3);
                L.AddValueTo(c2, c3, -cot1 ); L.AddValueTo(c3, c2, -cot1);
                L.AddValueTo(c3, c1, -cot2 ); L.AddValueTo(c1, c3, -cot2);
            }

            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                foreach (SparseMatrix.Element e in L.Rows[i])
                {
                    sum += e.value;
                }

                foreach (SparseMatrix.Element e in L.Rows[i])
                {
                    e.value = e.value / -sum;
                }

                L.AddValueTo(i, i, 1);
            }



            L.SortElement();
            return L;

        }

        public static double[] ComputeVoronoiArea(ref NonManifoldMesh mesh)
        {
            int n = mesh.VertexCount;
            double[] voronoiArea = new double[n];
            for (int i = 0; i < n; i++)
            {
                voronoiArea[i] = 0;
            }
            for (int i = 0, j = 0; i < mesh.FaceCount; i++, j += 3)
            {

                int c1 = mesh.FaceIndex[j];
                int c2 = mesh.FaceIndex[j + 1];
                int c3 = mesh.FaceIndex[j + 2];
                Vector3D v1 = new Vector3D(mesh.VertexPos, c1 * 3);
                Vector3D v2 = new Vector3D(mesh.VertexPos, c2 * 3);
                Vector3D v3 = new Vector3D(mesh.VertexPos, c3 * 3);

                double dis1=(v3-v2).Length()*(v3-v2).Length();
                double dis2=(v3-v1).Length()*(v3-v1).Length();
                double dis3=(v1-v2).Length()*(v1-v2).Length();


                double cot1 = (v2 - v1).Dot(v3 - v1) / (v2 - v1).Cross(v3 - v1).Length();
                double cot2 = (v3 - v2).Dot(v1 - v2) / (v3 - v2).Cross(v1 - v2).Length();
                double cot3 = (v1 - v3).Dot(v2 - v3) / (v1 - v3).Cross(v2 - v3).Length();

                bool obtuse = false;
                if (cot1 > 0 && cot2 > 0 && cot3 > 0)
                {
                    obtuse = true;
                }

                if (!obtuse)
                {
                    voronoiArea[c1] += (dis2 * cot2 + dis3 * cot3) / 8;
                    voronoiArea[c2] += (dis1 * cot1 + dis3 * cot3) / 8;
                    voronoiArea[c3] += (dis1 * cot1 + dis2 * cot2) / 8;
                }
                else
                {
                    double faceArea = mesh.ComputeFaceArea(i);
                    if (cot1 < 0)
                    {
                        voronoiArea[c1] += faceArea / 2;
                    }
                    else
                    {
                        voronoiArea[c1] += faceArea / 4;
                    }

                    if (cot2 < 0)
                    {
                        voronoiArea[c2] += faceArea / 2;
                    }
                    else
                    {
                        voronoiArea[c2] += faceArea / 4;
                    }

                    if (cot3 < 0)
                    {
                        voronoiArea[c3] += faceArea / 2;
                    }
                    else
                    {
                        voronoiArea[c3] += faceArea / 4;
                    }
                }
            }
            return voronoiArea ;
        }

      

 

        public static SparseMatrix BuildCurvatureMatrixL(ref NonManifoldMesh mesh)
        {
            if (mesh == null)
                throw new Exception("mesh is null");


            int n = mesh.VertexCount;
            SparseMatrix L = new SparseMatrix(n, n);

            for (int i = 0, j = 0; i < mesh.FaceCount; i++, j += 3)
            {

                int c1 = mesh.FaceIndex[j];
                int c2 = mesh.FaceIndex[j + 1];
                int c3 = mesh.FaceIndex[j + 2];
                Vector3D v1 = new Vector3D(mesh.VertexPos, c1 * 3);
                Vector3D v2 = new Vector3D(mesh.VertexPos, c2 * 3);
                Vector3D v3 = new Vector3D(mesh.VertexPos, c3 * 3);
                double cot1 = (v2 - v1).Dot(v3 - v1) / (v2 - v1).Cross(v3 - v1).Length();
                double cot2 = (v3 - v2).Dot(v1 - v2) / (v3 - v2).Cross(v1 - v2).Length();
                double cot3 = (v1 - v3).Dot(v2 - v3) / (v1 - v3).Cross(v2 - v3).Length();
                L.AddValueTo(c1, c2, -cot3); L.AddValueTo(c2, c1, -cot3);
                L.AddValueTo(c2, c3, -cot1); L.AddValueTo(c3, c2, -cot1);
                L.AddValueTo(c3, c1, -cot2); L.AddValueTo(c1, c3, -cot2);
            } 
           
                
            double[] voronoiArea=ComputeVoronoiArea(ref mesh);

               

            for (int i = 0; i < n; i++)
            {
                double sum = 0;


                foreach (SparseMatrix.Element e in L.Rows[i])
                {
                    e.value = e.value / (voronoiArea[e.i]*2);
                }

                foreach (SparseMatrix.Element e in L.Rows[i])
                {
                    sum += e.value;
                }

                L.AddValueTo(i, i, -sum);
            }




            L.SortElement();
            return L;

        }

        public static double[] ComputeGaussianCurvature(ref NonManifoldMesh mesh)
        {
            int n = mesh.VertexCount;
            double[] gaussianCurvature = new double[n];
            for (int i = 0; i < n; i++)
            {
                gaussianCurvature[i] = 2 * Math.PI; ;
            }

            for (int i = 0; i < n; i++)
            {
                foreach (int face in mesh.AdjVF[i])
                {
                    int a = mesh.FaceIndex[face * 3];
                    int b = mesh.FaceIndex[face * 3 + 1];
                    int c = mesh.FaceIndex[face * 3 + 2];

                    int x=0;
                    int y=0;
                    int z=0;
                    if (a == i)
                    {
                        x = i;
                        y = b;
                        z = c;
                    }
                    if (b == i)
                    {
                        x = b;
                        y = a;
                        z = c;
                    }
                    if (c == i)
                    {
                        x = c;
                        y = b;
                        z = a;
                    }


                    Vector3D v1 = new Vector3D(mesh.VertexPos, x * 3);
                    Vector3D v2 = new Vector3D(mesh.VertexPos, y * 3);
                    Vector3D v3 = new Vector3D(mesh.VertexPos, z * 3);

                    double area=((v2-v1).Cross(v3-v1)).Length() / ((v2-v1).Length()*(v3-v1).Length());
                    gaussianCurvature[i] -= Math.Asin(area);


                }
            }

            //for (int i = 0; i < n; i++)
            //{
            //    if (gaussianCurvature[i] < 0)
            //    {
            //        gaussianCurvature[i] = 0;
            //    }
            //}

            //double[] voronoiArea = ComputeVoronoiArea(ref mesh);
            //for (int i = 0; i < n; i++)
            //{
            //    gaussianCurvature[i] = (gaussianCurvature[i] / voronoiArea[i]) / 100000;
            //}

            

            return gaussianCurvature ;
        }


        public static double[][] ComputePricipalCurvature(ref NonManifoldMesh mesh)
        {
            int n = mesh.VertexCount;
            double[] mean = ComputeMeanCurvature(ref mesh);
            double[] gaussian = ComputeGaussianCurvature(ref mesh);

            double[][] pricipal  = new double[2][];
            pricipal[0]=new double[n];
            pricipal[1]=new double[n];


         
            for (int i = 0; i < n; i++)
            {
                pricipal[0][i] = mean[i] + Math.Sqrt(mean[i] * mean[i] - gaussian[i]);
                pricipal[1][i] = mean[i] - Math.Sqrt(mean[i] * mean[i] - gaussian[i]);
            }

            return pricipal;
        }

        public static double[] ComputeMeanCurvature(ref NonManifoldMesh mesh)
        {
            int n=mesh.VertexCount ;
            double[] meanCurvature = new double[n];
            for (int i = 0; i < n; i++)
            {
                meanCurvature[i] = 0;
            }

            SparseMatrix Lc = BuildUniformMatrixL(ref mesh);


            if (Lc == null)
                throw new Exception("Laplacian matrix is null");

        
            double[] v = new double[n];
            double[][] lap = new double[3][];

            for (int i = 0; i < 3; i++)
            {
                lap[i] = new double[n];
                for (int j = 0, k = 0; j < n; j++, k += 3)
                    v[j] = mesh.VertexPos[k + i];
                Lc.Multiply(v, 0, lap[i], 0);
            }

            for (int i = 0; i < n; i++)
            {
                meanCurvature[i] = Math.Sqrt(lap[0][i] * lap[0][i] + lap[1][i] * lap[1][i] + lap[2][i] * lap[2][i]);
            }

            lap= null;
            v = null;
            return meanCurvature;
            

        }



        

         


        public static SparseMatrix BuildTwoRingVV(ref NonManifoldMesh mesh)
        {
            int n = mesh.VertexCount;
            SparseMatrix L = new SparseMatrix(n, n, 6);

            for (int i = 0; i < n; i++)
            {
                foreach (int j in mesh.AdjVV[i])
                    foreach (int k in mesh.AdjVV[j])
                        L.AddElementIfNotExist(i, k, 0);
            }
            L.SortElement();
            return L;
        }

        public static int[][] BuildMatrixAAdjInfo(SparseMatrix A)
        {
            if (A == null)
                throw new Exception("A matrix is null");

            int[][] adj = new int[A.ColumnSize][];
            
            Set<int> s = new Set<int>();
            for (int i = 0; i < A.ColumnSize; i++)
            {
                s.Clear();

                List<SparseMatrix.Element> col = A.Columns[i];
                foreach (SparseMatrix.Element e in col)
                {
                    List<SparseMatrix.Element> row = A.Rows[e.i];
                    foreach (SparseMatrix.Element e2 in row)
                        s.Add(e2.j);
                }
                adj[i] = s.ToArray();
            }

            return adj;
        }



        public static SparseMatrix BuildMatrixDualL(ref NonManifoldMesh mesh)
        {
            // build dual Laplacian weight matrix L

            int vn = mesh.VertexCount;
            int fn = mesh.FaceCount;
            SparseMatrix L = new SparseMatrix(fn, vn, 6);

            for (int i = 0; i < fn; i++)
            {
                int f1 = mesh.AdjFF[i][0];
                int f2 = mesh.AdjFF[i][1];
                int f3 = mesh.AdjFF[i][2];
                Vector3D dv = mesh.GetDualPosition(i);
                Vector3D dv1 = mesh.GetDualPosition(f1);
                Vector3D dv2 = mesh.GetDualPosition(f2);
                Vector3D dv3 = mesh.GetDualPosition(f3);
                Vector3D u = dv - dv3;
                Vector3D v1 = dv1 - dv3;
                Vector3D v2 = dv2 - dv3;
                Vector3D normal = (v1.Cross(v2)).Normalize();
                Matrix3D M = new Matrix3D(v1, v2, normal);
                Vector3D coord = M.Inverse() * u;
                double alpha;

                alpha = 1.0 / 3.0;
                for (int j = 0, k = i * 3; j < 3; j++)
                    L.AddValueTo(i, mesh.FaceIndex[k++], alpha);

                alpha = coord[0] / 3.0;
                for (int j = 0, k = f1 * 3; j < 3; j++)
                    L.AddValueTo(i, mesh.FaceIndex[k++], -alpha);

                alpha = coord[1] / 3.0;
                for (int j = 0, k = f2 * 3; j < 3; j++)
                    L.AddValueTo(i, mesh.FaceIndex[k++], -alpha);

                alpha = (1.0 - coord[0] - coord[1]) / 3.0;
                for (int j = 0, k = f3 * 3; j < 3; j++)
                    L.AddValueTo(i, mesh.FaceIndex[k++], -alpha);

              
            }

            L.SortElement();
            return L;
        }


        public static NonManifoldMesh CreateDualMesh(NonManifoldMesh mesh)
        {
            NonManifoldMesh dualMesh = new NonManifoldMesh();
            dualMesh.VertexPos = mesh.CreateDualPosition();
            dualMesh.FaceIndex = mesh.CreateDualFaceIndex();
            dualMesh.VertexCount = mesh.FaceCount;
            dualMesh.FaceCount = dualMesh.FaceIndex.Length / 3;
            dualMesh.ScaleToUnitBox();
            dualMesh.MoveToCenter();
            dualMesh.ComputeFaceNormal();
            dualMesh.ComputeVertexNormal();
            //dualMesh.AdjVV = dualMesh.BuildAdjacentMatrix().GetRowIndex();
            //dualMesh.AdjVF = dualMesh.BuildAdjacentMatrixFV().GetColumnIndex();
            ////dualMesh.adjFF = dualMesh.BuildAdjacentMatrixFF().GetRowIndex();
            //dualMesh.FindBoundaryVertex();
            return dualMesh;

        }


        public static double[] ComputeDualLap(ref NonManifoldMesh mesh)
        {
            int vn = mesh.VertexCount;
            int fn = mesh.FaceCount;
            double[] dLap = new double[fn * 3];

            mesh.ComputeDualPosition();
            for (int i = 0, j = 0; i < fn; i++, j += 3)
            {
                Vector3D u = new Vector3D(mesh.DualVertexPos, j);
                Vector3D v1 = new Vector3D(mesh.DualVertexPos, mesh.AdjFF[i][0] * 3);
                Vector3D v2 = new Vector3D(mesh.DualVertexPos, mesh.AdjFF[i][1] * 3);
                Vector3D v3 = new Vector3D(mesh.DualVertexPos, mesh.AdjFF[i][2] * 3);
                Vector3D normal = ((v1 - v3).Cross(v2 - v3)).Normalize();
                Matrix3D m = new Matrix3D(v1 - v3, v2 - v3, normal);
                Vector3D coord = m.Inverse() * (u - v3);
                //dLap[j] = coord.x;
                //dLap[j + 1] = coord.y;
                //dLap[j + 2] = coord.z;

                dLap[j] = normal.x * coord[2];
                dLap[j + 1] = normal.x * coord[2];
                dLap[j + 2] = normal.x * coord[2];
            }

            return dLap;
        }

        public static void SetMeshColorToMeanCurvature( NonManifoldMesh mesh)
        {
            double[] meanCurvature=MeshOperators.ComputeMeanCurvature(ref mesh);
            UpdateColor(ref mesh, ref meanCurvature);
            
        }

        public static void SetMeshColorToPricipal1(NonManifoldMesh mesh)
        {
            double[][] pricipal =MeshOperators.ComputePricipalCurvature(ref mesh);
            UpdateColor(ref mesh, ref pricipal[0]);
        }

        public static void SetMeshColorToPricipal2(NonManifoldMesh mesh)
        {
            double[][] pricipal = MeshOperators.ComputePricipalCurvature(ref mesh);
            UpdateColor(ref mesh, ref pricipal[1]);
        }


        public static double[] GaussianCurvatureArray(NonManifoldMesh mesh) {
            return MeshOperators.ComputeGaussianCurvature(ref mesh);
        }

        public static void SetMeshColorToGaussianCurvature(NonManifoldMesh mesh)
        {
            double[] gaussianCurvature = MeshOperators.ComputeGaussianCurvature(ref mesh);
           
            UpdateColor(ref mesh, ref gaussianCurvature);

        }

        public static void UpdateColor(ref NonManifoldMesh mesh, ref double[] color)
        {
            int n = mesh.VertexCount;
            //double max = double.MinValue;
            //double min = double.MaxValue;
            //for (int i = 0; i < n; i++)
            //{
            //    if (max < mesh.Color[i])
            //    {
            //        max = mesh.Color[i];
            //    }
            //    if (min > mesh.Color[i])
            //    {
            //        min = mesh.Color[i];
            //    }
            //}

            for (int i = 0; i < n; i++)
                
                mesh.Color[i] = color[i];
        }

        public static void SetMeshColor(ref NonManifoldMesh mesh, ref double[] err)
        {
            double max = double.MinValue;
            foreach (double e in err)
                if (e > max) max = e;

            for (int i = 0, j = 0; i < mesh.FaceCount; i++, j += 3)
            {

                mesh.Color[j] = (float)(0.1 + (err[i] / max) * 0.9);
                mesh.Color[j + 1] = mesh.Color[j + 2] = 0.5f;
            }
        }


        public static double ComputeAverageArea(NonManifoldMesh mesh)
        {
            int n = mesh.FaceCount;
            double area = 0;
            for (int i = 0; i < n; i++)
            {
                area += mesh.ComputeFaceArea(i);
            }
            return area / n;
        }

        public static double ComputeOneRingArea(NonManifoldMesh mesh,int vertex)
        {
            int n = mesh.FaceCount;
            double area = 0;
            foreach (int k in mesh.AdjVF[vertex])
                area += mesh.ComputeFaceArea(k);
            return area;
        }

        public static double ComputeVolume(NonManifoldMesh mesh)
        {
            int n = mesh.FaceCount;
            double volume = 0;
            for(int i=0;i<n;i++)
            {
                int a=mesh.FaceIndex[i*3];
                int b=mesh.FaceIndex[i*3+1];
                int c=mesh.FaceIndex[i*3+2];

                Vector3D  vertexA=new Vector3D(mesh.VertexPos[a*3],mesh.VertexPos[a*3+1],mesh.VertexPos[a*3+2]);
                Vector3D  vertexB=new Vector3D(mesh.VertexPos[b*3],mesh.VertexPos[b*3+1],mesh.VertexPos[b*3+2]);
                Vector3D  vertexC=new Vector3D(mesh.VertexPos[c*3],mesh.VertexPos[c*3+1],mesh.VertexPos[c*3+2]);

                double v123 = vertexA.x * vertexB.y * vertexC.z;
                double v231 = vertexA.y * vertexB.z * vertexC.x;
                double v312 = vertexA.z * vertexB.x * vertexC.y;
                double v132 = vertexA.x * vertexB.z * vertexC.y;
                double v213 = vertexA.y * vertexB.x * vertexC.z;
                double v321 = vertexA.z * vertexB.y * vertexC.x;

                volume += (v123 + v231 + v312 - v132 - v213 - v321)/6;

            }
            return volume;
        }


        public static void TaubinSmooth(NonManifoldMesh mesh)
        {
            double l=-0.5;
            
            int n=mesh.VertexCount;
            double[][] lap= ComputeCotLap(ref mesh);

            for(int j=0;j<3;j++)
            {
            for (int i = 0; i < n; i++)
            {
                mesh.VertexPos[i * 3+j] += lap[j][i]*l;
            }
            }

        }

        



        

    }
}
