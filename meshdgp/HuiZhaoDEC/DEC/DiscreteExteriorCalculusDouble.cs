using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class DECDouble
    {

        private static DECDouble singleton = new DECDouble();


        public static DECDouble Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new DECDouble();
                return singleton;
            }
        }

        private DECDouble()
        {

        }

        public SparseMatrixDouble Star0;

        public SparseMatrixDouble Star1;

        public SparseMatrixDouble D0;

        public SparseMatrixDouble D1;

        public SparseMatrixDouble Star2;

        public SparseMatrixDouble Laplace;

        public static double ComputeVertexDualArea(TriMesh.Vertex v)
        {
            double sum = 0;

            foreach (TriMesh.Face face in v.Faces)
            {
                sum += TriMeshUtil.ComputeAreaFaceTwo(face);
            }

            return sum / 3;
        }

        public SparseMatrixDouble BuildHodgeStar0Form(TriMesh mesh)
        {
            SparseMatrixDouble star0 = new SparseMatrixDouble(mesh.Vertices.Count, mesh.Vertices.Count);


            foreach (TriMesh.Vertex vertex in mesh.Vertices)
            {
                star0[vertex.Index, vertex.Index] = ComputeVertexDualArea(vertex);
            }

            this.Star0 = star0;
            return star0;
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

        public SparseMatrixDouble BuildHodgeStar1Form(TriMesh mesh)
        {
            SparseMatrixDouble star1 = new SparseMatrixDouble(mesh.Edges.Count, mesh.Edges.Count);

            foreach (TriMesh.Edge edge in mesh.Edges)
            {
                double cotAlpha = ComputeTan(edge.HalfEdge0);

                double cotBeta = ComputeTan(edge.HalfEdge1);

                star1[edge.Index, edge.Index] = (cotAlpha + cotBeta) / 2;
            }

            this.Star1 = star1;
            return star1;
        }

        public SparseMatrixDouble BuildHodgeStar2Form(TriMesh mesh)
        {
            SparseMatrixDouble star2 = new SparseMatrixDouble(mesh.Faces.Count, mesh.Faces.Count);

            foreach (TriMesh.Face face in mesh.Faces)
            {
                star2[face.Index, face.Index] = 1 / TriMeshUtil.ComputeAreaFace(face);
            }

            Star2 = star2;
            return star2;
        }

        public SparseMatrixDouble BuildExteriorDerivative0Form(TriMesh mesh)
        {
            SparseMatrixDouble d0 = new SparseMatrixDouble(mesh.Edges.Count, mesh.Vertices.Count);

            foreach (TriMesh.Edge edge in mesh.Edges)
            {
                int ci = edge.HalfEdge0.FromVertex.Index;
                int cj = edge.HalfEdge0.ToVertex.Index;

                d0[edge.Index, ci] = 1;
                d0[edge.Index, cj] = -1;

            }

            D0 = d0;
            return d0;
        }

        public SparseMatrixDouble BuildExteriorDerivative1Form(TriMesh mesh)
        {
            SparseMatrixDouble d1 = new SparseMatrixDouble(mesh.Faces.Count, mesh.Edges.Count);

            foreach (TriMesh.Face face in mesh.Faces)
            {
                foreach (TriMesh.HalfEdge hf in face.Halfedges)
                {
                    double s = 0;

                    if (hf.Edge.HalfEdge0 == hf)
                    {
                        s = -1;
                    }
                    else
                    {
                        s = 1;
                    }

                    d1[face.Index, hf.Edge.Index] = s;
                }
            }

            D1 = d1;
            return d1;
        }

        public SparseMatrixDouble BuildLaplaceWithNeumannBoundary(TriMesh mesh)
        {
            SparseMatrixDouble d0 = this.BuildExteriorDerivative0Form(mesh);
            D0 = d0;

            SparseMatrixDouble star1 = this.BuildHodgeStar1Form(mesh);
            Star1 = star1;

            SparseMatrixDouble star0 = this.BuildHodgeStar0Form(mesh);
            Star0 = star0;

            SparseMatrixDouble delta = d0.Transpose() * star1 * d0;
            delta += (1.0e-8) * star0;

            Laplace = delta;
            return delta;
        }

        public SparseMatrixDouble BuildLaplace(TriMesh mesh)
        {
            SparseMatrixDouble sparse = new SparseMatrixDouble();

            SparseMatrixDouble d0 = this.BuildExteriorDerivative0Form(mesh);


            return sparse;
        }
    }
}
