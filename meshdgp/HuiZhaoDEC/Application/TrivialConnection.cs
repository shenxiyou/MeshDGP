using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class TrivialConnection
    {

        private TriMesh mesh = null;

        public TrivialConnection(TriMesh mesh)
        {
            this.mesh = mesh;

        }

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

        protected Vector3D Transport(Vector3D wI, TriMesh.Face faceI, TriMesh.Face faceJ, DenseMatrixDouble x)
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
            double angle = x[sharedEdge.Index, 0];


            if (sharedEdge.HalfEdge0 == sharedEdgeI)
            {
                angle = -angle;
            }

            //Compute the basis 
            Matrix3D Ei = Orthogonalize(bv - av, cv - av);
            Matrix3D Ej = Orthogonalize(bv - av, bv - dv);

            //Build Rotate Matrix between two Faces
            Matrix3D rotateMatrix = Matrix3D.Rotate(angle);

            Vector3D wj = (Ej * rotateMatrix * Ei.Inverse() * wI);

            return wj;
        }

        #endregion


        public Vector3D[] ComputeVectorField(DenseMatrixDouble x,double initAngle)
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
                        Vector3D wj = Transport(wI, currentFace, neighbor, x);
                        vectorFields[neighbor.Index] = wj;
                        queue.Enqueue(neighbor);
                        visitedFlags[neighbor.Index] = true;
                    }
                }

                ii++;
            }

            return vectorFields;
        }

        public List<KeyValuePair<TriMesh.Vertex,double>> Singularities;

        protected DenseMatrixDouble ComputeTrivaialConnection(SparseMatrixDouble d0, SparseMatrixDouble d1, double[] Guassian)
        {
            DenseMatrixDouble b = CholmodConverter.dConvertArrayToDenseMatrix(ref Guassian, Guassian.Length, 1);

            double[] singularValues = new double[Singularities.Count];

            //Init with avg of 2
            double avgValue = 2.0f / (double)Singularities.Count;

            int j = 0;
            foreach (KeyValuePair<TriMesh.Vertex, double> vItem in Singularities)
            {
                int index = vItem.Key.Index;
                double value = vItem.Value;

                b[index, 0] = Guassian[index] - 2 * Math.PI * value;
                j++;
            }

            for (int i = 0; i < Guassian.Length; i++)
            {
                b[i, 0] = -b[i, 0];
            }

            SparseMatrixDouble A = d0.Transpose();

            DenseMatrixDouble x = LinearSystemGenericByLib.Instance.SolveLinerSystem(ref A, ref b);

            SparseMatrixDouble d1T = d1.Transpose();
            SparseMatrixDouble Laplace = d1 * d1T;
            DenseMatrixDouble rhs = d1 * x;

            DenseMatrixDouble y = LinearSystemGenericByLib.Instance.SolveLinerSystem(ref Laplace, ref rhs);
            x = x - d1T * y;

            return x;
        }


        DenseMatrixDouble x;

        public void InitProcess()
        {
            SparseMatrixDouble d0 = DECDouble.Instance.BuildExteriorDerivative0Form(mesh);
            SparseMatrixDouble d1 = DECDouble.Instance.BuildExteriorDerivative1Form(mesh);
            //SparseMatrixDouble d1 = SparseMatrixDouble.ReadFromFile("d1.mat");

            double[] guassianCurvatures = TriMeshUtil.ComputeGaussianCurvatureIntegrated(mesh);

            //Seize all singularities
            if (Singularities.Count == 0)
            {
                Singularities.Add(new KeyValuePair<TriMesh.Vertex, double>(mesh.Vertices[0], 2.0f));
            }

            x = ComputeTrivaialConnection(d0, d1, guassianCurvatures);
        }

        public Vector3D[] GetTrivalFields(double initAngle)
        {
            if (x == null)
            {
                throw new Exception("Error");
            }

            Vector3D[] vectorFields2 = ComputeVectorField(x, initAngle);

            return vectorFields2;
        }



    }
}
