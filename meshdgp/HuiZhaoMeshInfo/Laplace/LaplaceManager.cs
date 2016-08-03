using System;
using System.Collections.Generic;
using System.Text; 

namespace GraphicResearchHuiZhao 
{
    [Serializable]
    public partial class LaplaceManager
    {

        //Cot laplace is symmetric!
        //CotNormalize laplace is symmetric!
        //Graph laplace is symmetric!
        //Mass laplace is unsymmetric!
        //Dual laplace is unsymmetric!
        //MeanCurvature laplace is symmetric!
        //MeanCurvatreNormalize laplace is symmetric!
        //Rigid laplace is unsymmetric!
        //Stffness laplace is unsymmetric!
        //Uniform laplace is symmetric!

        private static LaplaceManager singleton = new LaplaceManager();


        public static LaplaceManager Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new LaplaceManager();
                return singleton;
            }
        }

       

        private LaplaceManager()
        {
        }

        public SparseMatrix CurrentMatrix = null;

        public EnumLaplaceMatrix CurrentMatrixType = EnumLaplaceMatrix.None;

        public TriMesh Mesh=null;

        

        public LaplaceManager(TriMesh mesh)
        {
            this.Mesh = mesh;
        }

        public void TESTGetAllLaplaceMatrixInfo(TriMesh mesh)
        {
            GenerateLaplaceMatrix(EnumLaplaceMatrix.Cot, mesh);
            if (CurrentMatrix != null && CurrentMatrix.IsSymmetric())
            {
                Console.WriteLine("Cot laplace is symmetric!");
            }
            else
            {
                Console.WriteLine("Cot laplace is unsymmetric!");
            }

            CurrentMatrix = null;

            GenerateLaplaceMatrix(EnumLaplaceMatrix.CotNormalize, mesh);
            if (CurrentMatrix != null && CurrentMatrix.IsSymmetric())
            {
                Console.WriteLine("CotNormalize laplace is symmetric!");
            }
            else
            {
                Console.WriteLine("CotNormalize laplace is unsymmetric!");
            }

            CurrentMatrix = null;

            GenerateLaplaceMatrix(EnumLaplaceMatrix.GraphTest, mesh);
            if (CurrentMatrix != null && CurrentMatrix.IsSymmetric())
            {
                Console.WriteLine("Graph laplace is symmetric!");
            }
            else
            {
                Console.WriteLine("Graph laplace is unsymmetric!");
            }

            CurrentMatrix = null;

            GenerateLaplaceMatrix(EnumLaplaceMatrix.Mass, mesh);
            if (CurrentMatrix != null && CurrentMatrix.IsSymmetric())
            {
                Console.WriteLine("Mass laplace is symmetric!");
            }
            else
            {
                Console.WriteLine("Mass laplace is unsymmetric!");
            }

            CurrentMatrix = null;

            GenerateLaplaceMatrix(EnumLaplaceMatrix.DualLaplace, mesh);
            if (CurrentMatrix != null && CurrentMatrix.IsSymmetric())
            {
                Console.WriteLine("Dual laplace is symmetric!");
            }
            else
            {
                Console.WriteLine("Dual laplace is unsymmetric!");
            }

            CurrentMatrix = null;

            GenerateLaplaceMatrix(EnumLaplaceMatrix.MeanCurvature, mesh);
            if (CurrentMatrix != null && CurrentMatrix.IsSymmetric())
            {
                Console.WriteLine("MeanCurvature laplace is symmetric!");
            }
            else
            {
                Console.WriteLine("MeanCurvature laplace is unsymmetric!");
            }

            CurrentMatrix = null;

            GenerateLaplaceMatrix(EnumLaplaceMatrix.MeanCurvatreNormalize, mesh);
            if (CurrentMatrix != null && CurrentMatrix.IsSymmetric())
            {
                Console.WriteLine("MeanCurvatreNormalize laplace is symmetric!");
            }
            else
            {
                Console.WriteLine("MeanCurvatreNormalize laplace is unsymmetric!");
            }

            CurrentMatrix = null;

            GenerateLaplaceMatrix(EnumLaplaceMatrix.Rigid, mesh);
            if (CurrentMatrix != null && CurrentMatrix.IsSymmetric())
            {
                Console.WriteLine("Rigid laplace is symmetric!");
            }
            else
            {
                Console.WriteLine("Rigid laplace is unsymmetric!");
            }

            CurrentMatrix = null;

            GenerateLaplaceMatrix(EnumLaplaceMatrix.Stffness, mesh);
            if (CurrentMatrix != null && CurrentMatrix.IsSymmetric())
            {
                Console.WriteLine("Stffness laplace is symmetric!");
            }
            else
            {
                Console.WriteLine("Stffness laplace is unsymmetric!");
            }

            CurrentMatrix = null;

            GenerateLaplaceMatrix(EnumLaplaceMatrix.CombinatorialGraphNormalized, mesh);
            if (CurrentMatrix != null && CurrentMatrix.IsSymmetric())
            {
                Console.WriteLine("Uniform laplace is symmetric!");
            }
            else
            {
                Console.WriteLine("Uniform laplace is unsymmetric!");
            }

            CurrentMatrix = null;
        }

        public SparseMatrix GenerateLaplaceMatrix(EnumLaplaceMatrix targetType, TriMesh mesh)
        {
           
            SparseMatrix currentMatrix = null;
            switch (targetType)
            {
                case EnumLaplaceMatrix.LapalceGraph:
                    currentMatrix = BuildLaplaceGraph(mesh); 
                    break;
                case EnumLaplaceMatrix.LaplaceTutte:
                    currentMatrix = BuildLaplaceTutte(mesh); 
                    break;
                case EnumLaplaceMatrix.LaplaceTutteSys:
                    currentMatrix = BuildLaplaceTutteSymmetrized(mesh); 
                    break;

                case EnumLaplaceMatrix.LaplaceGraphNomalized:
                    currentMatrix = BuildLaplaceGraphNomalized(mesh);

                    break;
                case EnumLaplaceMatrix.LapalceCot:
                    currentMatrix =BuildLaplaceCot(mesh); 
                    break;
                case EnumLaplaceMatrix.LapalceCotArea:
                    currentMatrix =BuildMatrixMeanCurvature(mesh);

                    break;

                case EnumLaplaceMatrix.None:
                    break;
                case EnumLaplaceMatrix.GraphTest:
                    currentMatrix = BuildMatrixGraphTest(mesh);
                   
                    break;
                case EnumLaplaceMatrix.Cot:
                    currentMatrix = BuildMatrixCot(mesh);
                    
                    break;
                case EnumLaplaceMatrix.CotNormalize:
                    currentMatrix = BuildMatrixCotNormalize(mesh);
                    
                    break;
                case EnumLaplaceMatrix.MeanCurvature:
                    currentMatrix = BuildMatrixMeanCurvature(mesh);
                    
                    break;
                case EnumLaplaceMatrix.MeanCurvatreNormalize:
                    currentMatrix = BuildMatrixMeanCurvatureNormalize(mesh);
                   
                    break;
                case EnumLaplaceMatrix.CombinatorialGraphNormalized:
                    currentMatrix = BuildMatrixCombinatorialGraphNormalized(mesh);
                   
                    break;

                case EnumLaplaceMatrix.Stffness:
                    currentMatrix = BuildMatrixStiffness(mesh);
                    break;

                case EnumLaplaceMatrix.Rigid:
                    currentMatrix = BuildMatrixRigid(mesh);
                    break;

                case EnumLaplaceMatrix.RigidPositive:
                    currentMatrix = BuildMatrixRigidPositive(mesh);
                    break;

                case EnumLaplaceMatrix.Mass:
                    currentMatrix =BuildMatrixMass(mesh);
                    break;

                case EnumLaplaceMatrix.CombinatorialGraph:
                    currentMatrix = BuildMatrixCombinatorialGraph(mesh);
                    break;
                case EnumLaplaceMatrix.MeanValue:
                    currentMatrix = BuildMatrixMeanValue(mesh);
                    break;

                case EnumLaplaceMatrix.Area :
                    currentMatrix = BuildMatrixArea(mesh);
                    break;

                case EnumLaplaceMatrix.TwoRingVV:
                    currentMatrix = BuildTwoRingVV(mesh);
                    break;

                case EnumLaplaceMatrix.AdjacentVV:
                    currentMatrix = BuildAdjacentMatrixVV(mesh);
                    break;

                case EnumLaplaceMatrix.AdjacentFV:
                    currentMatrix = BuildAdjacentMatrixFV(mesh);
                    break;

                case EnumLaplaceMatrix.AdjacentFF:
                    currentMatrix = BuildAdjacentMatrixFF(mesh);
                    break;

                
                default:
                    break;
            }


            this.CurrentMatrixType = targetType;
            this.CurrentMatrix = currentMatrix;
            return currentMatrix;
        }

        public SparseMatrix BuildMatrixRigid(TriMesh mesh)
        { 
            int n = mesh.Vertices.Count;
            SparseMatrix L = new SparseMatrix(n, n); 
            for (int i = 0; i < mesh.Faces.Count; i++)
            { 
                int c1 = mesh.Faces[i].GetVertex(0).Index;
                int c2 = mesh.Faces[i].GetVertex(1).Index;
                int c3 = mesh.Faces[i].GetVertex(2).Index; 
                Vector3D v1 = mesh.Faces[i].GetVertex(0).Traits.Position;
                Vector3D v2 = mesh.Faces[i].GetVertex(1).Traits.Position;
                Vector3D v3 = mesh.Faces[i].GetVertex(2).Traits.Position; 
                double cot1 = (v2 - v1).Dot(v3 - v1) 
                              / (v2 - v1).Cross(v3 - v1).Length();
                double cot2 = (v3 - v2).Dot(v1 - v2) 
                              / (v3 - v2).Cross(v1 - v2).Length();
                double cot3 = (v1 - v3).Dot(v2 - v3)
                              / (v1 - v3).Cross(v2 - v3).Length();
                L.AddValueTo(c1, c2, -cot3 / 2);
                L.AddValueTo(c2, c1, -cot3 / 2);
                L.AddValueTo(c2, c3, -cot1 / 2);
                L.AddValueTo(c3, c2, -cot1 / 2);
                L.AddValueTo(c3, c1, -cot2 / 2);
                L.AddValueTo(c1, c3, -cot2 / 2);
            } 
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                foreach (SparseMatrix.Element e in L.Rows[i])
                {
                    sum += e.value;
                }
                L.AddValueTo(i, i, -sum);
            } 
            L.SortElement();
            return L; 
        }

        public SparseMatrix BuildMatrixRigidPositive(TriMesh mesh)
        {

            int n = mesh.Vertices.Count;
            SparseMatrix L = new SparseMatrix(n, n);

            for (int i = 0; i < mesh.Faces.Count; i++)
            {

                int c1 = mesh.Faces[i].GetVertex(0).Index;
                int c2 = mesh.Faces[i].GetVertex(1).Index;
                int c3 = mesh.Faces[i].GetVertex(2).Index;

                Vector3D v1 = mesh.Faces[i].GetVertex(0).Traits.Position;
                Vector3D v2 = mesh.Faces[i].GetVertex(1).Traits.Position;
                Vector3D v3 = mesh.Faces[i].GetVertex(2).Traits.Position;

                double cot1 = (v2 - v1).Dot(v3 - v1) / (v2 - v1).Cross(v3 - v1).Length();
                double cot2 = (v3 - v2).Dot(v1 - v2) / (v3 - v2).Cross(v1 - v2).Length();
                double cot3 = (v1 - v3).Dot(v2 - v3) / (v1 - v3).Cross(v2 - v3).Length();
                L.AddValueTo(c1, c2, cot3 / 2); L.AddValueTo(c2, c1, cot3 / 2);
                L.AddValueTo(c2, c3, cot1 / 2); L.AddValueTo(c3, c2, cot1 / 2);
                L.AddValueTo(c3, c1, cot2 / 2); L.AddValueTo(c1, c3, cot2 / 2);
            }

            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                foreach (SparseMatrix.Element e in L.Rows[i])
                {
                    sum += e.value;
                }
                L.AddValueTo(i, i, -sum);
            }



            L.SortElement();
            return L;

        }


        public SparseMatrix BuildMatrixGraphTest(TriMesh mesh)
        {
            int n = mesh.Vertices.Count;
            SparseMatrix W =  BuildAdjacentMatrixVV(mesh);
            SparseMatrix D = new SparseMatrix(n, n);
            for (int i = 0; i < n; i++)
            {
                double sum = W.Rows[i].Count - 1;
                D.AddValueTo(i, i, sum);
            }
            SparseMatrix K = D.Minus(W);
            return K;
        }


        public SparseMatrix BuildMatrixCombinatorialGraph(TriMesh mesh)
        {
            int n = mesh.Vertices.Count;
            SparseMatrix W =  BuildAdjacentMatrixVV(mesh);
            SparseMatrix D = new SparseMatrix(n, n);
            for (int i = 0; i < n; i++)
            {
                double sum = W.Rows[i].Count;
                D.AddValueTo(i, i, sum);
            }
            SparseMatrix K = W.Minus(D);
            return K;
        }

        public SparseMatrix BuildMatrixCombinatorialGraphNormalized(TriMesh mesh)
        {
            int n = mesh.Vertices.Count;
            SparseMatrix L =  BuildAdjacentMatrixVV(mesh);
            for (int i = 0; i < n; i++)
            {
                double sum = L.Rows[i].Count;
                foreach (SparseMatrix.Element e in L.Rows[i])
                {
                    L.AddValueTo(i, e.j, 1 / sum - 1);
                }
                L.AddValueTo(i, i, -1);
            }
            L.SortElement();
            return L;
        }

        public SparseMatrix BuildMatrixCot(TriMesh mesh)
        { 
            int n = mesh.Vertices.Count;
            SparseMatrix L =  BuildLaplaceMatrixCotBasic(mesh); 
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                foreach (SparseMatrix.Element e in L.Rows[i])
                {
                    sum += e.value;
                }
                L.AddValueTo(i, i, -sum);
            } 
            L.SortElement(); 
            return L; 
        }


        public SparseMatrix BuildMatrixMeanValue(TriMesh mesh)
        {
            int n = mesh.Vertices.Count;
            SparseMatrix L =  BuildAdjacentMatrixVV(mesh);
            SparseMatrix D = new SparseMatrix(n, n); 
            foreach (TriMesh.HalfEdge halfedge in mesh.HalfEdges)
            {
                TriMesh.Vertex fromJ = halfedge.FromVertex;
                TriMesh.Vertex toI = halfedge.ToVertex; 
                Vector3D jtoI = (toI.Traits.Position - fromJ.Traits.Position).Normalize();
                Vector3D alphaToI = (halfedge.Next.ToVertex.Traits.Position 
                                     - toI.Traits.Position).Normalize();
                Vector3D betaToI = (halfedge.Opposite.Previous.FromVertex.Traits.Position 
                                    - toI.Traits.Position).Normalize();
                double cosGamaIJ = jtoI.Dot(alphaToI) / (jtoI.Length() * alphaToI.Length());
                double cosThetaIJ = jtoI.Dot(betaToI) / (jtoI.Length() * betaToI.Length());
                double angelGama = Math.Acos(cosGamaIJ);
                double angelTheta = Math.Acos(cosThetaIJ);
                double wij = (Math.Tan(angelGama / 2) + Math.Tan(angelTheta / 2))
                             / (toI.Traits.Position - fromJ.Traits.Position).Length();  
                int indexI = toI.Index;
                int indexJ = fromJ.Index;
                D.AddValueTo(indexI, indexJ, wij);
            }
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                foreach (SparseMatrix.Element item in D.Rows[i])
                {
                    sum += item.value;
                }
                D.AddValueTo(i, i, -sum);
            }
            return D;
        }

        public SparseMatrix BuildMatrixCotNormalize(TriMesh mesh)
        {
            int n = mesh.Vertices.Count;
            SparseMatrix L = BuildLaplaceMatrixCotBasic(mesh); 
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                foreach (SparseMatrix.Element e in L.Rows[i])
                {
                    sum += e.value;
                } 
                foreach (SparseMatrix.Element e in L.Rows[i])
                {
                    e.value = e.value / sum;
                } 
                L.AddValueTo(i, i, -1);
            }
            L.SortElement();
            return L; 
        }

        public SparseMatrix BuildMatrixMeanCurvature(TriMesh mesh)
        { 
            int n = mesh.Vertices.Count;
            SparseMatrix L = BuildLaplaceMatrixCotBasic(mesh); 
            double[] voronoiArea = TriMeshUtil.ComputeAreaVoronoi(mesh); 
            for (int i = 0; i < n; i++)
            {
                double sum = 0; 
                foreach (SparseMatrix.Element e in L.Rows[i])
                {
                    e.value = e.value / (voronoiArea[e.i] * 4);
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

        public SparseMatrix BuildMatrixMeanCurvatureNormalize(TriMesh mesh)
        {

            int n = mesh.Vertices.Count;
            SparseMatrix L = BuildLaplaceMatrixCotBasic(mesh);

            double[] voronoiArea = TriMeshUtil.ComputeAreaVoronoi(mesh);

            for (int i = 0; i < n; i++)
            {
                double sum = 0;

                foreach (SparseMatrix.Element e in L.Rows[i])
                {
                    e.value = e.value / (voronoiArea[e.i] * 4);
                }

                foreach (SparseMatrix.Element e in L.Rows[i])
                {
                    sum += e.value;
                }

                foreach (SparseMatrix.Element e in L.Rows[i])
                {
                    e.value = e.value / sum;
                }

                L.AddValueTo(i, i, -1);
            }

            L.SortElement();
            return L;

        }

        public SparseMatrix BuildMatrixMass(TriMesh mesh)
        { 
            int n = mesh.Vertices.Count;
            SparseMatrix L = new SparseMatrix(n, n); 
            for (int i = 0; i < mesh.Edges.Count; i++)
            {
                double face0area = 0;
                double face1area = 0;
                if (mesh.Edges[i].Face0 != null)
                {
                    face0area = TriMeshUtil.ComputeAreaFace(mesh.Edges[i].Face0); 
                } 
                if (mesh.Edges[i].Face1 != null)
                {
                    face1area =TriMeshUtil.ComputeAreaFace(mesh.Edges[i].Face1); 
                } 
                L.AddValueTo(mesh.Edges[i].Vertex0.Index, 
                             mesh.Edges[i].Vertex1.Index, 
                             (face0area + face1area) / 12);
                L.AddValueTo(mesh.Edges[i].Vertex1.Index, 
                             mesh.Edges[i].Vertex0.Index, 
                             (face0area + face1area) / 12); 
            } 
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                foreach (SparseMatrix.Element e in L.Rows[i])
                {
                    sum += e.value;
                }
                L.AddValueTo(i, i, sum);
            } 
            L.SortElement();
            return L; 
        }

        public SparseMatrix BuildMatrixStiffness(TriMesh mesh)
        { 
            int n = mesh.Vertices.Count;
            SparseMatrix L = BuildLaplaceMatrixCotBasic(mesh); 
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                foreach (SparseMatrix.Element e in L.Rows[i])
                {
                    e.value = e.value / 2;
                    sum += e.value;
                }
                L.AddValueTo(i, i, -sum);
            } 
            L.SortElement();
            return L;

        }

        public SparseMatrix BuildMatrixDual(TriMesh mesh)
        {
            int vn = mesh.Vertices.Count;
            int fn = mesh.Faces.Count;
            SparseMatrix L = new SparseMatrix(fn, vn, 6);
            for (int i = 0; i < fn; i++)
            {
                int f1 = mesh.Faces[i].GetFace(0).Index;
                int f2 = mesh.Faces[i].GetFace(1).Index;
                int f3 = mesh.Faces[i].GetFace(2).Index;
                Vector3D dv = mesh.DualGetVertexPosition(i);
                Vector3D dv1 = mesh.DualGetVertexPosition(f1);
                Vector3D dv2 = mesh.DualGetVertexPosition(f2);
                Vector3D dv3 = mesh.DualGetVertexPosition(f3);
                Vector3D u = dv - dv3;
                Vector3D v1 = dv1 - dv3;
                Vector3D v2 = dv2 - dv3;
                Vector3D normal = (v1.Cross(v2)).Normalize();
                Matrix3D M = new Matrix3D(v1, v2, normal);
                Vector3D coord = M.Inverse() * u;
                double alpha;
                alpha = 1.0 / 3.0;
                L.AddValueTo(i, mesh.Faces[i].GetVertex(0).Index, alpha);
                L.AddValueTo(i, mesh.Faces[i].GetVertex(1).Index, alpha);
                L.AddValueTo(i, mesh.Faces[i].GetVertex(2).Index, alpha);
                alpha = coord[0] / 3.0;
                L.AddValueTo(i, mesh.Faces[f1].GetVertex(0).Index, -alpha);
                L.AddValueTo(i, mesh.Faces[f1].GetVertex(1).Index, -alpha);
                L.AddValueTo(i, mesh.Faces[f1].GetVertex(2).Index, -alpha);
                alpha = coord[1] / 3.0;
                L.AddValueTo(i, mesh.Faces[f2].GetVertex(0).Index, -alpha);
                L.AddValueTo(i, mesh.Faces[f2].GetVertex(1).Index, -alpha);
                L.AddValueTo(i, mesh.Faces[f2].GetVertex(2).Index, -alpha);
                alpha = (1.0 - coord[0] - coord[1]) / 3.0;
                L.AddValueTo(i, mesh.Faces[f3].GetVertex(0).Index, -alpha);
                L.AddValueTo(i, mesh.Faces[f3].GetVertex(1).Index, -alpha);
                L.AddValueTo(i, mesh.Faces[f3].GetVertex(2).Index, -alpha);
            } 
            L.SortElement();
            return L;
        }

        public SparseMatrix BuildMatrixATA(ref SparseMatrix A,TriMesh mesh)
        {
            // assume A is sorted
            // assume values in parameter adj is in order

            if (A == null)
                throw new Exception("A matrix is null");

            int[][] adj = BuildTwoRingVV(mesh).GetRowIndex();
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


        public SparseMatrix BuildLaplaceMatrixCotBasic(TriMesh mesh)
        {  
            int n = mesh.Vertices.Count;
            SparseMatrix L = new SparseMatrix(n, n);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                int c1 = mesh.Faces[i].GetVertex(0).Index;
                int c2 = mesh.Faces[i].GetVertex(1).Index;
                int c3 = mesh.Faces[i].GetVertex(2).Index;
                Vector3D v1 = mesh.Faces[i].GetVertex(0).Traits.Position;
                Vector3D v2 = mesh.Faces[i].GetVertex(1).Traits.Position;
                Vector3D v3 = mesh.Faces[i].GetVertex(2).Traits.Position;
                double cot1 = (v2 - v1).Dot(v3 - v1) / (v2 - v1).Cross(v3 - v1).Length();
                double cot2 = (v3 - v2).Dot(v1 - v2) / (v3 - v2).Cross(v1 - v2).Length();
                double cot3 = (v1 - v3).Dot(v2 - v3) / (v1 - v3).Cross(v2 - v3).Length();
                L.AddValueTo(c1, c2, cot3); L.AddValueTo(c2, c1, cot3);
                L.AddValueTo(c2, c3, cot1); L.AddValueTo(c3, c2, cot1);
                L.AddValueTo(c3, c1, cot2); L.AddValueTo(c1, c3, cot2);
            }
            return L;
        }


        public  SparseMatrix BuildMatrixArea(TriMesh mesh)
        {
            List<List<TriMesh.HalfEdge>> bounds = 
                  TriMeshUtil.RetrieveBoundaryEdgeAll(mesh);
            int n = mesh.Vertices.Count;
            SparseMatrix A = new SparseMatrix(2 * n, 2 * n);
            foreach (List<TriMesh.HalfEdge> oneBoundary in bounds)
            {
                foreach (TriMesh.HalfEdge currentHF in oneBoundary)
                {
                    int Vi = currentHF.FromVertex.Index;
                    int Vj = currentHF.ToVertex.Index;                   
                    A[2 * Vi, 2 * Vj + 1] += -0.5;
                    A[2 * Vj + 1, 2 * Vi] += -0.5; 
                    A[2 * Vj, 2 * Vi + 1] += 0.5;
                    A[2 * Vi + 1, 2 * Vj] += 0.5;
                }
            }
            return A;
        }  

        //public SparseMatrix BuildMatrixArea(TriMesh mesh)
        //{
        //    List<List<TriMesh.Vertex>> bounds = TriMeshUtil.RetrieveBoundaryAllVertex(mesh);
        //    int n = mesh.Vertices.Count;
        //    SparseMatrix A = new SparseMatrix(2 * n + 4, 2 * n);
        //    foreach (List<TriMesh.Vertex> oneBoundary in bounds)
        //    {
        //        TriMesh.HalfEdge boundaryStartHF = oneBoundary[0].FindHalfedgeTo(oneBoundary[1]);
        //        TriMesh.HalfEdge currentHF = boundaryStartHF;
        //        do
        //        {
        //            TriMesh.Vertex Vi = currentHF.FromVertex;
        //            TriMesh.Vertex Vj = currentHF.ToVertex;

        //            //Mark UiVj
        //            A[2 * Vi.Index, 2 * Vj.Index + 1] += 0.5;
        //            A[2 * Vj.Index + 1, 2 * Vi.Index] += 0.5;

        //            //Mark VjUi
        //            A[2 * Vj.Index, 2 * Vi.Index + 1] += -0.5;
        //            A[2 * Vi.Index + 1, 2 * Vj.Index] += -0.5;

        //            currentHF = currentHF.Next;
        //        } while (currentHF != boundaryStartHF);
        //    }

        //    return A;
        //}

        public SparseMatrix BuildMatrix2N(TriMesh mesh)
        {
            SparseMatrix L = LaplaceManager.Instance.BuildMatrixRigid(mesh);

            int n = mesh.Vertices.Count;

            SparseMatrix Ld = new SparseMatrix(2 * n, 2 * n);

            foreach (List<SparseMatrix.Element> row in L.Rows)
            {
                foreach (SparseMatrix.Element rowItem in row)
                {
                    int i = rowItem.i;
                    int j = rowItem.j;
                    double value = rowItem.value;

                    Ld.AddValueTo(2 * i, 2 * j, value);
                    Ld.AddValueTo(2 * i + 1, 2 * j + 1, value);

                }
            }

            return Ld;
        }


        public SparseMatrix BuildMatrix2N(SparseMatrix L)
        { 
            int n = L.ColumnSize;

            SparseMatrix Ld = new SparseMatrix(2 * n, 2 * n);

            foreach (List<SparseMatrix.Element> row in L.Rows)
            {
                foreach (SparseMatrix.Element rowItem in row)
                {
                    int i = rowItem.i;
                    int j = rowItem.j;
                    double value = rowItem.value;

                    Ld.AddValueTo(2 * i, 2 * j, value);
                    Ld.AddValueTo(2 * i + 1, 2 * j + 1, value);

                }
            }

            return Ld;
        }


        public SparseMatrix BuildLaplaceCotArea(TriMesh mesh)
        {
            double[] areas = TriMeshUtil.ComputeAreaMixed(mesh);
            SparseMatrix cot = BuildLaplaceCot(mesh);
            int n = mesh.Vertices.Count;
            for (int i = 0; i < n; i++)
            {
                foreach (SparseMatrix.Element e in cot.Rows[i])
                {
                    e.value = e.value * (1 / areas[i]);
                }
            }
            return cot;
        }

    }
}
