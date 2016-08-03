using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    class DECComplex
    {
        private static DECComplex singleton = new DECComplex();

        public static DECComplex Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new DECComplex();
                return singleton;
            }
        } 

        public static double ComputeVertexDualArea(TriMesh.Vertex v)
        {
            double sum = 0;

            foreach (TriMesh.Face face in v.Faces)
            {
                sum += TriMeshUtil.ComputeAreaFaceTwo(face);
            }

            return sum / 3;
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

        public SparseMatrixComplex cBuildExteriorDerivative0Form(TriMesh mesh)
        {
            SparseMatrixComplex d0 = new SparseMatrixComplex(mesh.Edges.Count, mesh.Vertices.Count);

            foreach (TriMesh.Edge edge in mesh.Edges)
            {
                int ci = edge.HalfEdge0.FromVertex.Index;
                int cj = edge.HalfEdge0.ToVertex.Index;

                d0[edge.Index, ci] = new Complex(1, 0);
                d0[edge.Index, cj] = new Complex(-1, 0);

            }

            return d0;
        }

        public SparseMatrixComplex cBuildExteriorDerivative1Form(TriMesh mesh)
        {
            SparseMatrixComplex d1 = new SparseMatrixComplex(mesh.Faces.Count, mesh.Edges.Count);

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

                    d1[face.Index, hf.Edge.Index] = new Complex(s, 0);
                }
            }

            return d1;
        }


        public SparseMatrixComplex cBuildHodgeStar1Form(TriMesh mesh)
        {
            SparseMatrixComplex star1 = new SparseMatrixComplex(mesh.Edges.Count, mesh.Edges.Count);

            foreach (TriMesh.Edge edge in mesh.Edges)
            {
                double cotAlpha = ComputeTan(edge.HalfEdge0);

                double cotBeta = ComputeTan(edge.HalfEdge1);

                Complex value = new Complex((cotAlpha + cotBeta) / 2, 0);

                star1[edge.Index, edge.Index] = value;
            }


            return star1;
        }

        public SparseMatrixComplex cBuildHodgeStar2Form(TriMesh mesh)
        {
            SparseMatrixComplex star2 = new SparseMatrixComplex(mesh.Faces.Count, mesh.Faces.Count);

            foreach (TriMesh.Face face in mesh.Faces)
            {
                Complex value = new Complex(1 / TriMeshUtil.ComputeAreaFace(face), 0);


                star2[face.Index, face.Index] = value;
            }

            return star2;
        }
        public SparseMatrixComplex cBuildHodgeStar0Form(TriMesh mesh)
        {
            SparseMatrixComplex star0 = new SparseMatrixComplex(mesh.Vertices.Count, mesh.Vertices.Count);

            foreach (TriMesh.Vertex vertex in mesh.Vertices)
            {
                Complex value = new Complex(ComputeVertexDualArea(vertex), 0);

                star0[vertex.Index, vertex.Index] = value;
            }

            return star0;
        }
    }
}
