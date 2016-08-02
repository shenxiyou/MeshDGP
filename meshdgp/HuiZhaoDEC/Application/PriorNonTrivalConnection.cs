using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public struct TransportData
    {
        public double delta;
        public double sign;
        public double omega;
        public double alphaI;
        public double alphaJ;
    }

    public class PriorNonTrivalConnection
    {
        public TriMesh Mesh;

        public int[] MapRow;

        public TreeCoTree treeCotree;

        public double[] EdgeTheta; //Connection betwenn incident faces

        public double[] EdgeHodgeStar1;

        public bool[] GeneratorOnBoundary;

        public List<KeyValuePair<TriMesh.Vertex, double>> Singularities;

        public List<KeyValuePair<List<TriMesh.HalfEdge>, double>> GeneratorValues;

        //Output 
        public Vector3D[] VectorFields;

        List<List<TriMesh.HalfEdge>> basisCycles;
        int nContractibleCycles;

        List<List<TriMesh.HalfEdge>> dualCycles;

        DenseMatrixDouble b;

        DenseMatrixDouble K;

        SparseMatrixDouble A;


        #region Util

        private double TipAngle(Vector3D x, Vector3D a, Vector3D b)
        {
            Vector3D u = (a - x).Normalize();
            Vector3D v = (b - x).Normalize();

            return Math.Atan2(u.Cross(v).Length(), u.Dot(v));
        }

        public void UpdateHodgeStar(TriMesh mesh)
        {
            foreach (TriMesh.Edge edge in mesh.Edges)
            {
                double sum = 0;

                TriMesh.HalfEdge currentHe = edge.HalfEdge0;
                do
                {
                    Vector3D a = currentHe.FromVertex.Traits.Position;
                    Vector3D b = currentHe.Next.FromVertex.Traits.Position;
                    Vector3D c = currentHe.Next.Next.FromVertex.Traits.Position;

                    Vector3D u = a - c;
                    Vector3D v = b - c;

                    double cotTheta = (u.Dot(v)) / u.Cross(v).Length();

                    sum += 0.5 * cotTheta;

                } while (currentHe != edge.HalfEdge0);

                EdgeHodgeStar1[edge.Index] = Math.Max(sum, 0);

            }


        }

        public double ComputeGeneratorDefect(List<TriMesh.HalfEdge> cycle)
        {
            double theta = 0;
            foreach (TriMesh.HalfEdge hf in cycle)
            {
                theta = ComputeParallTransport(theta, hf);
            }

            while (theta >= Math.PI)
            {
                theta -= 2 * Math.PI;
            }

            while (theta < -Math.PI)
            {
                theta += 2 * Math.PI;
            }

            return -theta;
        }

        public double ComputeAngleDefect(TriMesh.Vertex vertex)
        {
            double sum = 0;

            foreach (TriMesh.HalfEdge hf in vertex.HalfEdges)
            {
                Vector3D p1 = hf.FromVertex.Traits.Position;
                Vector3D p2 = hf.Next.FromVertex.Traits.Position;
                Vector3D p3 = hf.Next.Next.FromVertex.Traits.Position;

                Vector3D u1 = p2 - p1;
                Vector3D u2 = p3 - p1;

                sum += Math.Atan2(u1.Cross(u2).Length(), u1.Dot(u2));
            }

            return 2 * Math.PI - sum;
        }

        protected double ComputeParallTransport(double phi, TriMesh.HalfEdge hf)
        {
            if (hf.OnBoundary || hf.Opposite.OnBoundary)
            {
                return 0;
            }

            Vector3D e = hf.FromVertex.Traits.Position - hf.ToVertex.Traits.Position;

            Vector3D al, bl;
            FaceFrame(hf.Face.HalfEdge, out al, out bl);

            Vector3D ar, br;
            FaceFrame(hf.Opposite.Face.HalfEdge, out ar, out br);

            double deltaL = Math.Atan2(e.Dot(bl), e.Dot(al));
            double deltaR = Math.Atan2(e.Dot(br), e.Dot(ar));

            return (phi - deltaL) + deltaR;
        }

        public void FaceFrame(TriMesh.HalfEdge hf, out Vector3D a, out Vector3D b)
        {
            a = Vector3D.Zero;
            b = Vector3D.Zero;

            if (hf.OnBoundary)
            {
                return;
            }

            TriMesh.Vertex v0 = hf.FromVertex;
            TriMesh.Vertex v1 = hf.ToVertex;

            a = (v0.Traits.Position - v1.Traits.Position).Normalize();
            b = hf.Face.Traits.Normal.Cross(a);
        }

        #endregion

        #region Singularities

        public List<List<TriMesh.HalfEdge>> Generators
        {
            get { return dualCycles; }
        }

        #endregion


        public PriorNonTrivalConnection(TriMesh mesh)
        {
            this.Mesh = mesh;
            Init();
            UpdateHodgeStar(mesh);
            Build(mesh);
        }



        public void Init()
        {
            if (Singularities == null)
            {
                Singularities = new List<KeyValuePair<HalfEdgeMesh.Vertex, double>>();
            }
            else
            {
                Singularities.Clear();
            }

            if (GeneratorValues == null)
            {
                GeneratorValues = new List<KeyValuePair<List<HalfEdgeMesh.HalfEdge>, double>>();
            }
            else
            {
                GeneratorValues.Clear();
            }

            if (treeCotree == null)
            {
                treeCotree = new TreeCoTree(Mesh);
            }

            foreach (KeyValuePair<HalfEdgeMesh.Vertex, double> selectedV in Singularities)
            {
                selectedV.Key.Traits.SelectedFlag = 1;
            }

            EdgeTheta = new double[Mesh.Edges.Count];
            EdgeHodgeStar1 = new double[Mesh.Edges.Count];

            basisCycles = Append1RingBases(Mesh);
            nContractibleCycles = basisCycles.Count;

            dualCycles = treeCotree.ExtractHonologyGenerator(Mesh);
            GeneratorOnBoundary = new bool[dualCycles.Count];


            //Append to basisCycle
            foreach (List<TriMesh.HalfEdge> dualCycle in dualCycles)
            {
                basisCycles.Add(dualCycle);
            }

            //basisCycles.RemoveAt(basisCycles.Count - 1);
        }

        public void Build(TriMesh mesh)
        {

            int nBasisCycles = basisCycles.Count;

            //Build Matrix A
            A = BuildCycleMatrix(mesh, basisCycles);
            ApplyCotanWeights(ref A);

            //Factorize
            LinearSystemGenericByLib.Instance.FactorizationQR(ref A);

            K = new DenseMatrixDouble(basisCycles.Count, 1);
            b = new DenseMatrixDouble(basisCycles.Count, 1);

            //Add constraint of angle defect
            for (int i = 0; i < nContractibleCycles; i++)
            {
                K[i, 0] = -ComputeGeneratorDefect(basisCycles[i]);
            }

            //Add constraint of Generator
            int nGenerators = dualCycles.Count;
            for (int i = nContractibleCycles; i < nGenerators + nContractibleCycles; i++)
            {
                List<TriMesh.HalfEdge> generatorCycle = basisCycles[i];

                //Boundary condition
                if (treeCotree.IsBoundaryGenerator(generatorCycle))
                {
                    K[i, 0] = -BoundaryLoopCurvature(generatorCycle);
                    GeneratorOnBoundary[i - nContractibleCycles] = true;
                }
                //None-Boundary condition
                else
                {
                    K[i, 0] = -ComputeGeneratorDefect(generatorCycle);
                    GeneratorOnBoundary[i - nContractibleCycles] = false;
                }
            }

            //Copy to b
            for (int i = 0; i < nBasisCycles; i++)
            {
                b[i, 0] = K[i, 0];
            }
        }


        public void SetupRHS(TriMesh mesh)
        {
            //K will not change,while b will change with different singularity index.
            double indexSum = 0;
            foreach (TriMesh.Vertex vertex in mesh.Vertices)
            {
                if (vertex.OnBoundary)
                {
                    continue;
                }

                int i = MapRow[vertex.Index];
                b[i, 0] = K[i, 0];
            }

            //Add constraint of singularity
            foreach (KeyValuePair<TriMesh.Vertex, double> singularity in Singularities)
            {
                int i = MapRow[singularity.Key.Index];

                b[i, 0] = K[i, 0] + 2 * Math.PI * singularity.Value;
                indexSum += singularity.Value;
            }

            //Add constraint of index to Generator
            int nContractibleCycles = b.RowCount - dualCycles.Count;
            for (int i = 0; i < dualCycles.Count; i++)
            {
                int j = nContractibleCycles + i;
                b[j, 0] = K[j, 0] + 2 * Math.PI * GeneratorValues[i].Value;

                if (GeneratorOnBoundary[i])
                {
                    indexSum += GeneratorValues[i].Value;
                }
            }

            Console.WriteLine("Total Index Sum:" + indexSum);
        }

        public void Update()
        {

            SetupRHS(Mesh);

            DenseMatrixDouble x = LinearSystemGenericByLib.Instance.SolveByFractorizedQRLeastNorm(ref b);

            ApplyCotanWeights(Mesh, ref x);

            foreach (TriMesh.Edge edge in Mesh.Edges)
            {
                EdgeTheta[edge.Index] = x[edge.Index, 0];
            }

            //Reset
            for (int i = 0; i < b.RowCount; i++)
            {
                b[i, 0] = K[i, 0];
            }

        }

        #region Trival

        SparseMatrixDouble BuildCycleMatrix(TriMesh mesh, List<List<TriMesh.HalfEdge>> cycles)
        {
            SparseMatrixDouble A = new SparseMatrixDouble(mesh.Edges.Count, cycles.Count);

            int l = 0;
            foreach (List<TriMesh.HalfEdge> cycle in cycles)
            {
                foreach (TriMesh.HalfEdge hf in cycle)
                {
                    int k = hf.Edge.Index;
                    int i = hf.FromVertex.Index;
                    int j = hf.ToVertex.Index;

                    if (i > j)
                    {
                        A[k, l] = 1;
                    }
                    else
                    {
                        A[k, l] = -1;
                    }

                }
                l++;
            }

            return A;
        }

        void ApplyCotanWeights(ref SparseMatrixDouble A)
        {
            List<Pair> pairs = new List<Pair>();
            foreach (KeyValuePair<Pair, double> e in A.Datas)
            {
                Pair pair = e.Key;
                pairs.Add(pair);
            }

            foreach (Pair item in pairs)
            {
                int row = item.Key;
                int column = item.Value;

                double value = A.Datas[item] * Math.Sqrt(EdgeHodgeStar1[row]);

                A.Datas[item] = value;
            }

            pairs.Clear();
            pairs = null;
        }

        void ApplyCotanWeights(TriMesh mesh, ref DenseMatrixDouble x)
        {
            foreach (TriMesh.Edge edge in mesh.Edges)
            {
                x[edge.Index, 0] = x[edge.Index, 0] * Math.Sqrt(EdgeHodgeStar1[edge.Index]);
            }

        }

        public List<List<TriMesh.HalfEdge>> Append1RingBases(TriMesh mesh)
        {
            MapRow = new int[mesh.Vertices.Count];
            List<List<TriMesh.HalfEdge>> cycles = new List<List<HalfEdgeMesh.HalfEdge>>();

            foreach (TriMesh.Vertex vertex in mesh.Vertices)
            {
                if (vertex.OnBoundary)
                {
                    MapRow[vertex.Index] = -1;
                    continue;
                }

                List<TriMesh.HalfEdge> cycle = new List<HalfEdgeMesh.HalfEdge>();

                TriMesh.HalfEdge currentHf = vertex.HalfEdge;
                do
                {
                    cycle.Add(currentHf);

                    currentHf = currentHf.Opposite.Next;
                } while (currentHf != vertex.HalfEdge);

                MapRow[vertex.Index] = cycles.Count;

                cycles.Add(cycle);
            }

            return cycles;
        }

        public double BoundaryLoopCurvature(List<TriMesh.HalfEdge> cycle)
        {
            double totalK = 0;

            //Get the virtual face Boundary.
            TriMesh.Vertex v0 = cycle[0].Opposite.Next.FromVertex;
            if (!v0.OnBoundary)
            {
                v0 = cycle[0].Next.FromVertex;
            }

            TriMesh.HalfEdge he0 = v0.HalfEdge;
            do
            {
                he0 = he0.Opposite.Next;

                if (he0.OnBoundary)
                {
                    int a = 0;
                }
            } while (!he0.OnBoundary);

            Vector3D c = new Vector3D(0, 0, 0);
            TriMesh.HalfEdge he = he0;

            int boundaryLength = 0;
            do
            {
                c += he.FromVertex.Traits.Position;
                boundaryLength++;
                he = he.Next;
            } while (he != he0);

            c /= (double)boundaryLength;

            double K = 2 * Math.PI;
            he = he0;
            do
            {
                Vector3D a = he.FromVertex.Traits.Position;
                Vector3D b = he.Next.FromVertex.Traits.Position;
                K -= TipAngle(c, a, b);
                he = he.Next;
            } while (he != he0);
            totalK += K;

            // add the curvature around each of the boundary vertices, using
            // the following labels:
            //    c - virtual center vertex of boundary loop (computed above)
            //    d - current boundary vertex (we walk around the 1-ring of this vertex)
            //    a,b - consecutive interior vertices in 1-ring of d
            //    e,f - boundary vertices adjacent to d
            he = he0;
            do
            {
                TriMesh.Vertex v = he.FromVertex;
                Vector3D d = v.Traits.Position;

                K = 2 * Math.PI;

                TriMesh.HalfEdge he2 = v.HalfEdge;

                do
                {
                    if (he2.OnBoundary)
                    {
                        Vector3D f = he2.Next.FromVertex.Traits.Position;
                        K -= TipAngle(d, f, c);
                    }
                    else
                    {
                        Vector3D a = he2.Next.FromVertex.Traits.Position;
                        Vector3D b = he2.Next.Next.FromVertex.Traits.Position;
                        K -= TipAngle(d, a, b);


                        if (he2.Opposite.OnBoundary)
                        {
                            Vector3D e = he2.Opposite.FromVertex.Traits.Position;
                            K -= TipAngle(d, c, e);
                        }

                    }

                    he2 = he2.Opposite.Next;
                } while (he2 != v.HalfEdge);

                totalK += K;
                he = he.Next;

            } while (he != he0);

            return totalK;
        }


        #endregion


        #region Plot to Surface

        public static Matrix3D Orthogonalize(Vector3D u, Vector3D v)
        {
            Vector3D a = u.Normalize();

            Vector3D temp = v - (v.Dot(a)) * a;
            Vector3D b = temp.Normalize();
            Vector3D c = a.Cross(b).Normalize();

            Matrix3D e = new Matrix3D(a, b, c);

            return e;
        }

        protected Vector3D Transport(Vector3D wI, TriMesh.Face faceI, TriMesh.Face faceJ, double angle)
        {
            /*
             * triangles i and j according to the following labels:
                     
                            b
                           /|\
                          / | \
                         /  |  \
                        /   |   \
                       c  i | j  d
                        \   |   /
                         \  |  /
                          \ | /
                           \|/
                            a
         
             */

            //Find Shared edge IJ
            TriMesh.HalfEdge sharedEdgeI = null;
            TriMesh.HalfEdge sharedEdgeJ = null;
            TriMesh.Edge sharedEdge = null;

            foreach (TriMesh.HalfEdge edgeI in faceI.Halfedges)
            {
                foreach (TriMesh.HalfEdge edgeJ in faceJ.Halfedges)
                {
                    if (edgeI.Opposite == edgeJ)
                    {
                        sharedEdge = edgeI.Edge;
                        sharedEdgeI = edgeI;
                        sharedEdgeJ = edgeJ;
                        break;
                    }
                }
            }

            if (sharedEdge == null)
                throw new Exception("Error");

            //Find vertex correspondent to figure above
            Vector3D av = sharedEdgeI.FromVertex.Traits.Position;
            Vector3D bv = sharedEdgeJ.FromVertex.Traits.Position;
            Vector3D cv = sharedEdgeI.Next.ToVertex.Traits.Position;
            Vector3D dv = sharedEdgeJ.Next.ToVertex.Traits.Position;

            //Compute the basis 
            Matrix3D Ei = Orthogonalize(bv - av, cv - av);
            Matrix3D Ej = Orthogonalize(bv - av, bv - dv);

            //Build Rotate Matrix between two Faces
            Matrix3D rotateMatrix = Matrix3D.Rotate(angle);

            Vector3D wj = (Ej * rotateMatrix * Ei.Inverse() * wI);

            return wj;
        }

        #endregion


        //public void ComputeFrameAngles(double initialAngle)
        //{
        //    TreeNode<TriMesh.Face> Root = transportTree.Root;

        //    Queue<TreeNode<HalfEdgeMesh.Face>> queue = new Queue<TreeNode<HalfEdgeMesh.Face>>();
        //    queue.Enqueue(transportTree.Root);

        //    bool[] processedFlag = new bool[Mesh.Faces.Count];

        //    while (queue.Count != 0)
        //    {
        //        TreeNode<HalfEdgeMesh.Face> currentFaceNode = queue.Dequeue();

        //        TriMesh.HalfEdge startHf = currentFaceNode.Attribute.HalfEdge;
        //        TriMesh.HalfEdge currentHf = startHf;
        //        do
        //        {
        //            TriMesh.Face neighborFace = currentHf.Opposite.Face;

        //            TransportData td = transDatas[currentHf.Index];

        //            td.alphaJ = td.alphaI + td.delta - td.sign * td.omega;

        //            currentHf = currentHf.Next;
        //        } while (currentHf != startHf);


        //    }


        //}


        //public void UpdateAngles(int initAngle)
        //{
        //    TriMesh.Face faceTransport = Mesh.Faces[3];
        //    FaceAngle[faceTransport.Index] = initAngle;
        //    Queue<TriMesh.Face> queue = new Queue<TriMesh.Face>();
        //    queue.Enqueue(faceTransport);

        //    bool[] processedFlag = new bool[Mesh.Faces.Count];

        //    while (queue.Count != 0)
        //    {
        //        TriMesh.Face currentFace = queue.Dequeue();

        //        TriMesh.HalfEdge startHf = currentFace.HalfEdge;
        //        TriMesh.HalfEdge currentHf = startHf;
        //        double currentDelta = Deltas[currentFace.Index];

        //        do
        //        {
        //            TriMesh.Face neighborFace = currentHf.Opposite.Face;


        //            if (processedFlag[neighborFace.Index] == false &&
        //                neighborFace != faceTransport &&
        //                !neighborFace.OnBoundary
        //                )
        //            {
        //                processedFlag[neighborFace.Index] = true;
        //                queue.Enqueue(neighborFace);

        //                double delta = ComputeParallTransport(currentDelta, currentHf);
        //                Deltas[neighborFace.Index] = delta;

        //                double omega = EdgeTheta[currentHf.Edge.Index];
        //                double sign = currentHf.FromVertex.Index > currentHf.ToVertex.Index ? -1 : 1;
        //                FaceAngle[neighborFace.Index] = FaceAngle[currentFace.Index] + delta - sign * omega;

        //            }
        //            currentHf = currentHf.Next;
        //        } while (currentHf != startHf);


        //    }
        //}


        //public void AppendDirectionalConstraints(TriMesh mesh, List<List<TriMesh.HalfEdge>> cycles)
        //{
        //    transDatas = new TransportData[mesh.HalfEdges.Count];
        //    TriMesh.Face faceTransport = mesh.Faces[3];
        //    transportTree = new DynamicTree<HalfEdgeMesh.Face>();
        //    transportTree.Root = new TreeNode<HalfEdgeMesh.Face>(faceTransport);

        //    Queue<TreeNode<HalfEdgeMesh.Face>> queue = new Queue<TreeNode<HalfEdgeMesh.Face>>();
        //    queue.Enqueue(transportTree.Root);

        //    bool[] processedFlag = new bool[mesh.Faces.Count];

        //    while (queue.Count != 0)
        //    {
        //        TreeNode<HalfEdgeMesh.Face> currentFaceNode = queue.Dequeue();

        //        TriMesh.HalfEdge startHf = currentFaceNode.Attribute.HalfEdge;
        //        TriMesh.HalfEdge currentHf = startHf;
        //        do
        //        {
        //            TriMesh.Face neighborFace = currentHf.Opposite.Face;

        //            if (processedFlag[neighborFace.Index] == false &&
        //                neighborFace != faceTransport &&
        //                !neighborFace.OnBoundary
        //                )
        //            {
        //                TreeNode<HalfEdgeMesh.Face> neighNode = new TreeNode<HalfEdgeMesh.Face>(neighborFace);
        //                processedFlag[neighborFace.Index] = true;
        //                currentFaceNode.AddChild(neighNode);
        //                queue.Enqueue(neighNode);


        //                TransportData td = new TransportData();
        //                td.delta = ComputeParallTransport(0, currentHf);
        //                td.sign = currentHf.FromVertex.Index > currentHf.ToVertex.Index ? 1 : -1;
        //                td.omega = EdgeTheta[currentHf.Edge.Index];
        //                td.alphaI = FaceAngle[currentHf.Face.Index];
        //                td.alphaJ = FaceAngle[currentHf.Opposite.Face.Index];
        //                transDatas[currentHf.Index] = td;
        //                Deltas[currentHf.Face.Index] = td.delta;

        //            }
        //            currentHf = currentHf.Next;
        //        } while (currentHf != startHf);


        //    }


        //}

        public Vector3D[] ComputeVectorField(double initAngle)
        {
            Vector3D[] vectorFields = new Vector3D[Mesh.Faces.Count];
            bool[] visitedFlags = new bool[Mesh.Faces.Count];
            for (int i = 0; i < visitedFlags.Length; i++)
            {
                visitedFlags[i] = false;
            }

            //Find a initial root to expend
            TriMesh.Face rootFace = Mesh.Faces[0];
            var v1 = rootFace.GetVertex(0);
            var v3 = rootFace.GetVertex(2);
            var v2 = rootFace.GetVertex(1);

            Vector3D w0 = (rootFace.GetVertex(2).Traits.Position - rootFace.GetVertex(0).Traits.Position).Normalize();

            //Init transpot
            Vector3D av = v1.Traits.Position;
            Vector3D bv = v2.Traits.Position;
            Vector3D cv = v3.Traits.Position;
            Matrix3D Ei = Orthogonalize(bv - av, cv - av);

            Vector3D w0i = (Ei * Matrix3D.Rotate(initAngle) * Ei.Inverse() * w0);

            vectorFields[rootFace.Index] = w0i;
            visitedFlags[rootFace.Index] = true;

            //Recurse all faces
            Queue<TriMesh.Face> queue = new Queue<HalfEdgeMesh.Face>();
            queue.Enqueue(rootFace);

            int ii = 0;
            while (queue.Count > 0)
            {
                TriMesh.Face currentFace = queue.Dequeue();
                Vector3D wI = vectorFields[currentFace.Index];

                TriMesh.HalfEdge cuHe = currentFace.HalfEdge;

                do
                {
                    TriMesh.Face neighbor = cuHe.Opposite.Face;

                    if (neighbor == null)
                    {
                        cuHe = cuHe.Next;
                        continue;
                    }

                    if (visitedFlags[neighbor.Index] == false)
                    {
                        double angle = EdgeTheta[cuHe.Edge.Index];
                        int i = cuHe.FromVertex.Index;
                        int j = cuHe.ToVertex.Index;

                        if (i > j)
                        {
                            angle = -angle;
                        }

                        Vector3D wj = Transport(wI, currentFace, neighbor, angle);
                        vectorFields[neighbor.Index] = wj;
                        queue.Enqueue(neighbor);
                        visitedFlags[neighbor.Index] = true;
                    }
                    cuHe = cuHe.Next;

                } while (cuHe != currentFace.HalfEdge);


                ii++;
            }

            return vectorFields;
        }

    }
}
