using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class Geodistance
    {

        private TriMesh mesh = null;
        public Geodistance(TriMesh mesh)
        {
            this.mesh = mesh;


        }


        public double[] Process(double dt, TriMesh mesh, out Vector3D[] vectorFields, out double maxDistance)
        {
            DenseMatrixDouble x = null;
            int nb = BuildImpulseSingal(mesh, out x);
            vectorFields = null;
            maxDistance = 0;
            if (nb == 0)
            {
                return null;
            }

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


            SparseMatrixDouble L = d0.Transpose() * star1 * d0;

            L += 1.0e-8 * star0;

            //Heat flow:
            //[TO DO] edgeLength

            double edgelength = MeanEdgeLength(mesh);

            dt *= Math.Sqrt(edgelength);
            SparseMatrixDouble A = star0 + dt * L;

            DenseMatrixDouble u = LinearSystemGenericByLib.Instance.SolveLinerSystem(ref A, ref x);

            //Extract geodesic
            vectorFields = ComputeVectorField(u, mesh);

            DenseMatrixDouble div = ComputeDivergence(mesh, vectorFields);

            DenseMatrixDouble phi = LinearSystemGenericByLib.Instance.SolveLinerSystem(ref L, ref div);

            SetMinToZero(phi);


            double[] VertexDistances = AssignDistance(phi);

            double max = double.MinValue;
            for (int i = 0; i < VertexDistances.Length; i++)
            {
                double value = Math.Abs(VertexDistances[i]);
                if (max < value)
                {
                    max = value;
                }
            }

            maxDistance = max;

            return VertexDistances;
        }

        protected double MeanEdgeLength(TriMesh mesh)
        {
            double sum = 0;
            foreach (TriMesh.Edge edge in mesh.Edges)
            {
                TriMesh.Vertex v0 = edge.Vertex0;
                TriMesh.Vertex v1 = edge.Vertex1;

                sum += (v0.Traits.Position - v1.Traits.Position).Length();

            }

            return sum / mesh.Edges.Count;
        }

        protected void SetMinToZero(DenseMatrixDouble phi)
        {
            double min = double.MaxValue;
            for (int i = 0; i < phi.RowCount; i++)
            {
                for (int j = 0; j < phi.ColumnCount; j++)
                {
                    if (min > phi[i, j])
                    {
                        min = phi[i, j];
                    }
                }
            }

            for (int i = 0; i < phi.RowCount; i++)
            {
                for (int j = 0; j < phi.ColumnCount; j++)
                {

                    phi[i, j] -= min;

                }
            }

        }

        protected double[] AssignDistance(DenseMatrixDouble phi)
        {
            double[] vertexDistance = new double[mesh.Vertices.Count];

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                vertexDistance[i] = phi[i, 0];
                mesh.Vertices[i].Traits.Distance = phi[i, 0];
            }

            return vertexDistance;
        }


        protected DenseMatrixDouble ComputeDivergence(TriMesh mesh, Vector3D[] vectorFields)
        {
            DenseMatrixDouble div = new DenseMatrixDouble(mesh.Vertices.Count, 1);

            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                double sum = 0;

                foreach (TriMesh.HalfEdge hf in v.HalfEdges)
                {
                    if (hf.OnBoundary)
                    {
                        continue;
                    }

                    Vector3D n = RotatedEdge(hf.Next);
                    Vector3D vf = vectorFields[hf.Face.Index];

                    sum += n.Dot(vf);

                }

                div[v.Index, 0] = sum;
            }

            return div;
        }

        protected Vector3D[] ComputeVectorField(DenseMatrixDouble u, TriMesh mesh)
        {

            Vector3D[] vectors = new Vector3D[mesh.Faces.Count];

            foreach (TriMesh.Face f in mesh.Faces)
            {
                if (f.OnBoundary)
                {
                    continue;
                }

                TriMesh.HalfEdge hij = f.HalfEdge;
                TriMesh.HalfEdge hjk = hij.Next;
                TriMesh.HalfEdge hki = hjk.Next;

                TriMesh.Vertex vi = hij.FromVertex;
                TriMesh.Vertex vj = hjk.FromVertex;
                TriMesh.Vertex vk = hki.FromVertex;

                double ui = u[vi.Index, 0];
                double uj = u[vj.Index, 0];
                double uk = u[vk.Index, 0];

                Vector3D eijL = RotatedEdge(hij);
                Vector3D ejkL = RotatedEdge(hjk);
                Vector3D ekiL = RotatedEdge(hki);

                double area = TriMeshUtil.ComputeAreaFaceTwo(f);

                Vector3D x = 0.5 * (ui * ejkL + uj * ekiL + uk * eijL) / area;

                vectors[f.Index] = -x.Normalize();
            }

            return vectors;
        }

        protected Vector3D RotatedEdge(TriMesh.HalfEdge hf)
        {
            if (hf.OnBoundary)
            {
                return Vector3D.Zero;
            }

            Vector3D n = ComputeFaceNormals(hf.Face);
            Vector3D p0 = hf.FromVertex.Traits.Position;
            Vector3D p1 = hf.ToVertex.Traits.Position;

            return n.Cross(p1 - p0);
        }

        public Vector3D ComputeFaceNormals(TriMesh.Face f)
        {
            Vector3D p0 = f.GetVertex(0).Traits.Position;
            Vector3D p1 = f.GetVertex(1).Traits.Position;
            Vector3D p2 = f.GetVertex(2).Traits.Position; 

            return (p1 - p0).Cross(p2 - p0).Normalize();
        }

        protected int BuildImpulseSingal(TriMesh mesh, out DenseMatrixDouble x)
        {
            int nb = 0;
            x = new DenseMatrixDouble(mesh.Vertices.Count, 1);

            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                if (v.Traits.selectedFlag > 0)
                {
                    x[v.Index, 0] = 1;
                    nb++;
                }
            }

            return nb;
        }
    }
}
