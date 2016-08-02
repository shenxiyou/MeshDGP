using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class QEMSimplication : MergeEdgeSimplicationBase
    {
        TriMesh.VertexDynamicTrait<Matrix4D> vertexMatrix;
        ErrorPair[] edgeError;

        public QEMSimplication(TriMesh mesh)
            : base(mesh)
        {

        }

        protected override void Init()
        {
            this.heap = new MinHeap<TriMesh.Edge>(this.Mesh.Edges.Count);
            this.handle = new HeapNode<TriMesh.Edge>[this.Mesh.Edges.Count];
            this.vertexMatrix = new TriMesh.VertexDynamicTrait<Matrix4D>(this.Mesh);
            this.edgeError = new ErrorPair[this.Mesh.Edges.Count];
            this.traits = new TriMeshTraits(this.Mesh);
            this.traits.Init();

            this.SimplifyComputeInitQ();

            foreach (TriMesh.Edge edge in this.Mesh.Edges)
            {
                ErrorPair pair = CalculateError(edge);
                this.edgeError[edge.Index] = pair;
                this.handle[edge.Index] = this.heap.Add(pair.Error, edge);
            }
        }

        protected override Vector3D GetPos(HalfEdgeMesh.Edge target)
        {
            return this.edgeError[target.Index].Pos;
        }

        protected override double GetValue(HalfEdgeMesh.Edge target)
        {
            throw new NotImplementedException();
        }

        protected override void BeforeMerge(TriMesh.Edge edge)
        {
            base.BeforeMerge(edge);
            TriMesh.Vertex v1 = edge.Vertex0;
            TriMesh.Vertex v2 = edge.Vertex1;
            vertexMatrix[v1] += vertexMatrix[v2];
        }

        protected override void AfterMerge(TriMesh.Vertex v)
        {
            //foreach (var edge in this.Mesh.RemoveEdges)
            //{
            //    this.heap.Del(handle[edge.Index]);
            //}
            foreach (var edge in this.removed)
            {
                this.heap.Del(this.handle[edge.Index]);
            }

            foreach (TriMesh.Edge edge in v.Edges)
            {
                ErrorPair pair = CalculateError(edge);
                this.edgeError[edge.Index] = pair;
                this.heap.Update(this.handle[edge.Index], pair.Error);
            }
        }

        struct ErrorPair
        {
            public Vector3D Pos;
            public double Error;
        }

        private void SimplifyComputeInitQ()
        {
            foreach (TriMesh.Vertex vertex in this.Mesh.Vertices)
            {
                Matrix4D q = SimplifyComputeVertexQ(vertex);
                vertexMatrix[vertex] = q;
            }
        }

        private static Matrix4D SimplifyComputeVertexQ(TriMesh.Vertex vertex)
        {
            Matrix4D q = Matrix4D.ZeroMatrix;

            foreach (TriMesh.Face face in vertex.Faces)
            {
                TriMesh.Vertex v1 = face.GetVertex(0);
                TriMesh.Vertex v2 = face.GetVertex(1);
                TriMesh.Vertex v3 = face.GetVertex(2);

                //double[] plane = SimplifyComputePlane(v1, v2, v3);
                //Matrix4D k = SimplifyComputeK(plane);
                Plane plane = new Plane(v1.Traits.Position, v2.Traits.Position, v3.Traits.Position);
                Matrix4D k = Square(plane);
                q = q + k;

            }
            return q;
        }

        private static Matrix4D Square(Plane plane)
        {
            Matrix4D k = Matrix4D.ZeroMatrix;
            double a = plane.Normal.x;
            double b = plane.Normal.y;
            double c = plane.Normal.z;
            double d = plane.D;

            k[0] = a * a; k[1] = a * b; k[2] = a * c; k[3] = a * d;
            k[4] = a * b; k[5] = b * b; k[6] = b * c; k[7] = b * d;
            k[8] = a * c; k[9] = b * c; k[10] = c * c; k[11] = c * d;
            k[12] = a * d; k[13] = b * d; k[14] = c * d; k[15] = d * d;

            return k;
        }

        private ErrorPair CalculateError(TriMesh.Edge edge)
        {
            TriMesh.Vertex v1 = edge.Vertex0;
            TriMesh.Vertex v2 = edge.Vertex1;

            Matrix4D Qbar = Matrix4D.ZeroMatrix;
            Qbar = vertexMatrix[v1] + vertexMatrix[v2];

            //if q_bar is symmetric
            if (Qbar[1] != Qbar[4] || Qbar[2] != Qbar[8] || Qbar[6] != Qbar[9] ||
               Qbar[3] != Qbar[12] || Qbar[7] != Qbar[13] || Qbar[11] != Qbar[14]
                ) throw new Exception("Matrix Qbar is not symmetric");

            //Generate Q_delta
            Matrix4D Qdelta = Matrix4D.ZeroMatrix;
            for (int i = 0; i <= 11; i++)
            {
                Qdelta[i] = Qbar[i];
            }
            Qdelta[12] = 0;
            Qdelta[13] = 0;
            Qdelta[14] = 0;
            Qdelta[15] = 1;

            //check q_delta if is invertible
            Vector3D newVertex = new Vector3D();
            double min_error = double.MaxValue;

            //double det = Qdelta.SubDeterminate(0, 1, 2, 4, 5, 6, 8, 9, 10);
            //if (det != 0)
            //{
            //    newVertex.x = -1 / det * (Qdelta.SubDeterminate(1, 2, 3, 5, 6, 7, 9, 10, 11));
            //    newVertex.y = 1 / det * (Qdelta.SubDeterminate(0, 2, 3, 4, 6, 7, 8, 10, 11));
            //    newVertex.z = -1 / det * (Qdelta.SubDeterminate(0, 1, 3, 4, 5, 7, 8, 9, 11));
            //    min_error = this.VertexError(Qbar, newVertex);
            //}
            double det = Util.Solve(ref Qdelta, ref newVertex);
            if (det != 0)
            {
                min_error = this.VertexError(Qbar, newVertex);
            }
            else
            {
                Vector3D[] vecArr = new Vector3D[3]{
                    v1.Traits.Position,
                    v2.Traits.Position,
                    (v1.Traits.Position + v2.Traits.Position) / 2
                };

                for (int i = 0; i < 3; i++)
                {
                    double cur_error = this.VertexError(Qbar, vecArr[i]);
                    if (cur_error < min_error)
                    {
                        min_error = cur_error;
                        newVertex = vecArr[i];
                    }
                }
            }
            return new ErrorPair() { Pos = newVertex, Error = min_error };
        }

        private double VertexError(Matrix4D Q, Vector3D v)
        {
            return this.VertexError(Q, v.x, v.y, v.z);
        }

        private double VertexError(Matrix4D Q, double x, double y, double z)
        {
            return Q[0] * x * x + 2 * Q[1] * x * y + 2 * Q[2] * x * z + 2 * Q[3] * x +
                Q[5] * y * y + 2 * Q[6] * y * z + 2 * Q[7] * y +
                Q[10] * z * z + 2 * Q[11] * z + Q[15];
        }
    }
}
