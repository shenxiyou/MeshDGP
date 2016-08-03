using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class NoneTrivalConnection
    {
        private TriMesh mesh = null;

        public NoneTrivalConnection(TriMesh mesh)
        {
            this.mesh = mesh;
        }

        List<List<TriMesh.HalfEdge>> generatorLoops;

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

        public static double ComputeTan(TriMesh.HalfEdge hf)
        {
            if (hf.OnBoundary)
            {
                return 0;
            }

            Vector3D p0 = hf.Next.ToVertex.Traits.Position;
            Vector3D p1 = hf.FromVertex.Traits.Position;
            Vector3D p2 = hf.Next.FromVertex.Traits.Position;

            Vector3D u = p1 - p0;
            Vector3D v = p2 - p0;

            return u.Dot(v) / (u.Cross(v).Length());
        }

        protected double ComputeParallTransport(TriMesh.HalfEdge hf)
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

            return (deltaL - deltaR);
        }

        protected double ComputeConnectionOneForm(TriMesh.HalfEdge hf, List<double>[] HarmonicBasis, double[] HarmonicCoeffition, DenseMatrixDouble u)
        {
            double angle = 0.0f;

            double star1 = 0.5 * (ComputeTan(hf) + ComputeTan(hf.Opposite));
            double u0 = u[hf.Opposite.FromVertex.Index, 0];
            double u1 = u[hf.FromVertex.Index, 0];

            angle += star1 * (u1 - u0);

            List<double> harmonis = HarmonicBasis[hf.Index];

            if (harmonis != null)
            {
                for (int k = 0; k < harmonis.Count; k++)
                {
                    angle += HarmonicCoeffition[k] * harmonis[k];
                }
            }

            //double star0 = 0.5 * (Comput)

            return angle;
        }

        private List<double>[] HarmonicBasis;

        private double[] HarmonicCoffition;

        private SparseMatrixDouble Laplace;

        public List<KeyValuePair<TriMesh.Vertex, double>> Singularities;

        public double ComputeTheta(TriMesh.Vertex vertex)
        {
            double sum = 0;

            foreach (TriMesh.HalfEdge hf in vertex.HalfEdges)
            {
                sum += Math.Atan(ComputeTan(hf.Next));
            }

            return sum;
        }

        public DenseMatrixDouble InitWithTrivalHolonmy(SparseMatrixDouble Laplace, TriMesh mesh)
        {
            DenseMatrixDouble b = new DenseMatrixDouble(mesh.Vertices.Count, 1);
            double[] tempSingularities = new double[mesh.Vertices.Count];
            for (int i = 0; i < tempSingularities.Length; i++)
            {
                tempSingularities[i] = 0;
            }

            foreach (KeyValuePair<TriMesh.Vertex, double> pair in Singularities)
            {
                int index = pair.Key.Index;
                double value = pair.Value;

                tempSingularities[index] = value;
            }

            double[] GuassianCurvs = TriMeshUtil.ComputeGaussianCurvatureIntegrated(mesh);

            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                double value = 0;
                if (!v.OnBoundary)
                {
                    value -= GuassianCurvs[v.Index];
                    value += 2 * Math.PI * tempSingularities[v.Index];
                }

                b[v.Index, 0] = value;
            }

            DenseMatrixDouble u = LinearSystemGenericByLib.Instance.SolveLinerSystem(ref Laplace, ref b);

            return u;
        }

        DenseMatrixDouble u;

        public void InitProcess(TriMesh mesh)
        {

            TreeCoTree treeCotree = new TreeCoTree(mesh);

            generatorLoops = treeCotree.ExtractHonologyGenerator(mesh);

            HarmonicBasis basis = new HarmonicBasis(mesh);
            HarmonicBasis = basis.BuildHarmonicBasis(generatorLoops);
            int numberOfHarmBases = basis.NumberOfHarmonicBases(generatorLoops);

            //Still need to built
            u = InitWithTrivalHolonmy(Laplace, mesh);

            HarmonicCoffition = new double[numberOfHarmBases];


            if (numberOfHarmBases == 0)
            {
                return;
            }

            DenseMatrixDouble b = new DenseMatrixDouble(numberOfHarmBases, 1);
            SparseMatrixDouble H = new SparseMatrixDouble(numberOfHarmBases, numberOfHarmBases);

            int row = 0;

            bool skipBoundaryLoop = true;
            for (int i = 0; i < generatorLoops.Count; i++)
            {
                List<TriMesh.HalfEdge> cycle = generatorLoops[i];

                if (skipBoundaryLoop && treeCotree.IsBoundaryGenerator(cycle))
                {
                    skipBoundaryLoop = false;
                    continue;
                }

                foreach (TriMesh.HalfEdge hf in cycle)
                {
                    for (int col = 0; col < numberOfHarmBases; col++)
                    {
                        H[row, col] += HarmonicBasis[hf.Index][col];
                    }
                }

                double value = -GeneratorHolomy(cycle, HarmonicBasis, HarmonicCoffition, u);

                b[row, 0] = value;
                row++;
            }

            DenseMatrixDouble x = null;

            if (b.F_Norm() > 1.0e-8)
            {
                x = LinearSystemGenericByLib.Instance.SolveLinerSystem(ref H, ref b);
            }
            else
            {
                x = new DenseMatrixDouble(numberOfHarmBases, 1);
            }

            for (int i = 0; i < numberOfHarmBases; i++)
            {
                HarmonicCoffition[i] = x[i, 0];
            }

        }

        public double GeneratorHolomy(List<TriMesh.HalfEdge> cycle, List<double>[] HarmonicBasis, double[] HarmonicCoeffition, DenseMatrixDouble u)
        {
            double sum = 0;
            if (cycle.Count == 0)
            {
                return sum;
            }

            foreach (TriMesh.HalfEdge hf in cycle)
            {
                sum += ComputeParallTransport(hf);
                sum += ComputeConnectionOneForm(hf, HarmonicBasis, HarmonicCoeffition, u);
            }

            while (sum < 0)
            {
                sum += 2.0 * Math.PI;
            }

            while (sum > 2.0 * Math.PI)
            {
                sum -= 2.0 * Math.PI;
            }

            return sum;
        }

        public Vector3D[] Process()
        {
            //Build laplace
            SparseMatrixDouble star0 = DECDouble.Instance.Star0;
            if (star0 == null)
            {
                star0 = DECDouble.Instance.BuildHodgeStar0Form(mesh);
            }

            SparseMatrixDouble star1 = DECDouble.Instance.Star1;
            if (star1 == null)
            {
                star1 = DECDouble.Instance.BuildHodgeStar1Form(mesh);
            }

            SparseMatrixDouble d0 = DECDouble.Instance.D0;
            if (d0 == null)
            {
                d0 = DECDouble.Instance.BuildExteriorDerivative0Form(mesh);
            }

            SparseMatrixDouble d1 = DECDouble.Instance.D1;
            if (d1 == null)
            {
                d1 = DECDouble.Instance.BuildExteriorDerivative1Form(mesh);
            }

            if (Singularities == null)
            {
                Singularities = new List<KeyValuePair<HalfEdgeMesh.Vertex, double>>();
            }

            if (Singularities.Count == 0)
            {
                int count = 0;
                foreach (TriMesh.Vertex v in mesh.Vertices)
                {
                    if (v.Traits.selectedFlag > 0)
                    {
                        count++;
                    }
                }

                double avg = 2 / (double)count;

                int item = 0;
                foreach (TriMesh.Vertex V in mesh.Vertices)
                {
                    if (V.Traits.selectedFlag > 0)
                    {
                        if (item % 2 == 0) //2
                        {
                            Singularities.Add(new KeyValuePair<HalfEdgeMesh.Vertex, double>(V, -1));
                        }

                        if (item % 2 == 1) //2
                        {
                            Singularities.Add(new KeyValuePair<HalfEdgeMesh.Vertex, double>(V, 1));
                        }

                    }

                }

            }

            Laplace = d0.Transpose() * star1 * d0;

            Laplace = Laplace + (1.0e-8) * star0;


            InitProcess(this.mesh);
            GenerateFaceNormals();
            Vector3D[] VectorFields = ComputeVectorField(0);

            return VectorFields;
        }


        //Plot surface
        #region Plot to Surface

        public Vector3D[] FaceNormals;

        public Vector3D ComputeFaceNormals(TriMesh.Face f)
        {
            Vector3D p0 = f.GetVertex(0).Traits.Position;
            Vector3D p1 = f.GetVertex(1).Traits.Position;
            Vector3D p2 = f.GetVertex(2).Traits.Position;

            return (p1 - p0).Cross(p2 - p0).Normalize();
        }

        public void GenerateFaceNormals()
        {
            FaceNormals = new Vector3D[mesh.Faces.Count];

            foreach (TriMesh.Face f in mesh.Faces)
            {
                FaceNormals[f.Index] = ComputeFaceNormals(f);
            }

        }

        public static Matrix3D Orthogonalize(Vector3D u, Vector3D v)
        {
            Vector3D a = u.Normalize();

            Vector3D temp = v - (v.Dot(a)) * a;
            Vector3D b = temp.Normalize();
            Vector3D c = a.Cross(b).Normalize();

            Matrix3D e = new Matrix3D(a, b, c);

            return e;
        }

        protected Vector3D Transport(Vector3D wI, TriMesh.Face faceI, TriMesh.Face faceJ)
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

            double angle = ComputeConnectionOneForm(sharedEdgeI, HarmonicBasis, HarmonicCoffition, u);

            //Compute the basis 
            Matrix3D Ei = Orthogonalize(bv - av, cv - av);
            Matrix3D Ej = Orthogonalize(bv - av, bv - dv);

            //Build Rotate Matrix between two Faces
            Matrix3D rotateMatrix = Matrix3D.Rotate(angle);

            Vector3D wj = (Ej * rotateMatrix * Ei.Inverse() * wI);

            return wj;
        }

        protected Vector3D Transport2(Vector3D wI, TriMesh.Face faceI, TriMesh.Face faceJ)
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

            Vector3D p1 = sharedEdgeI.ToVertex.Traits.Position;
            Vector3D p0 = sharedEdgeI.FromVertex.Traits.Position;
            Vector3D e = (p1 - p0).Normalize();

            Vector3D nR = FaceNormals[sharedEdgeI.Face.Index];
            Vector3D nL = FaceNormals[sharedEdgeJ.Face.Index];

            Vector3D nLxnR = nR.Cross(nL);

            double dihdral = Math.Atan2(nLxnR.Length(), nL.Dot(nR));
            if (e.Dot(nLxnR) < 0)
            {
                dihdral = -dihdral;
            }

            double angle = -ComputeConnectionOneForm(sharedEdgeI, HarmonicBasis, HarmonicCoffition, u);

            Vector3D me = Rotate(wI, nR, angle);
            Vector3D wj = Rotate(me, e, dihdral);

            return wj;
        }


        public Vector3D Rotate(Vector3D v, Vector3D axis, double angle)
        {
            angle *= 0.5;
            //Quaternion q = new Quaternion(Math.Cos(angle), Math.Sin(angle) * axis
            Quaternion q = new Quaternion();

            double sinA = Math.Sin(angle);
            q.w = Math.Cos(angle);
            q.x = sinA * axis.x;
            q.y = sinA * axis.y;
            q.z = sinA * axis.z;

            Quaternion qc = Quaternion.Zero;
            Quaternion.Conjugate(ref q, out qc);
            Quaternion vq = new Quaternion(v.x, v.y, v.z, 0);
            Quaternion res = qc * vq * q;

            Vector3D imageOfRes = new Vector3D();
            imageOfRes.x = res.x;
            imageOfRes.y = res.y;
            imageOfRes.z = res.z;

            return imageOfRes;
        }


        public Vector3D[] ComputeVectorField(double initAngle)
        {
            Vector3D[] vectorFields = new Vector3D[mesh.Faces.Count];
            bool[] visitedFlags = new bool[mesh.Faces.Count];
            for (int i = 0; i < visitedFlags.Length; i++)
            {
                visitedFlags[i] = false;
            }

            //Find a initial root to expend
            TriMesh.Face rootFace = mesh.Faces[0];
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

                foreach (TriMesh.Face neighbor in currentFace.Faces)
                {
                    if (visitedFlags[neighbor.Index] == false)
                    {
                        Vector3D wj = Transport2(wI, currentFace, neighbor);
                        vectorFields[neighbor.Index] = wj;
                        queue.Enqueue(neighbor);
                        visitedFlags[neighbor.Index] = true;
                    }
                }

                ii++;
            }

            return vectorFields;
        }

        #endregion

    }
}
