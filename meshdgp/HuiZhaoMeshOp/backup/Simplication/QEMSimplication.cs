using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class QEMSimplication : MergeEdgeSimplicationBase
    {
        private struct ErrorPair
        {
            public Vector3D Pos;
            public double Error;
        }


        private TriMesh.VertexDynamicTrait<Matrix4D> vertexMatrix;
        private ErrorPair[] edgeError;

        public QEMSimplication(TriMesh mesh)
            : base(mesh)
        {

        }

        protected override void Init()
        {
            this.heap = new MinHeapTwo<TriMesh.Edge>(this.Mesh.Edges.Count);
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

        

        private void SimplifyComputeInitQ()
        {
            foreach (TriMesh.Vertex vertex in this.Mesh.Vertices)
            {
                Matrix4D q = ComputeVertexQ(vertex);
                vertexMatrix[vertex] = q;
            }
        }

        private static Matrix4D ComputeVertexQ(TriMesh.Vertex vertex)
        {
            Matrix4D q = Matrix4D.ZeroMatrix;
            foreach (TriMesh.Face face in vertex.Faces)
            {
                TriMesh.Vertex v1 = face.GetVertex(0);
                TriMesh.Vertex v2 = face.GetVertex(1);
                TriMesh.Vertex v3 = face.GetVertex(2);
                Plane plane = new Plane(v1.Traits.Position, 
                                        v2.Traits.Position, 
                                        v3.Traits.Position);
                Matrix4D k = ComputePlane(plane);
                q = q + k;
            }
            return q;
        }

        private static double[] SimplifyComputePlane(TriMesh.Vertex vertexA,
            TriMesh.Vertex vertexB, TriMesh.Vertex vertexC)
        {
            double x1 = vertexA.Traits.Position.x;
            double x2 = vertexB.Traits.Position.x;
            double x3 = vertexC.Traits.Position.x;

            double y1 = vertexA.Traits.Position.y;
            double y2 = vertexB.Traits.Position.y;
            double y3 = vertexC.Traits.Position.y;

            double z1 = vertexA.Traits.Position.z;
            double z2 = vertexB.Traits.Position.z;
            double z3 = vertexC.Traits.Position.z;

            double[] plane = new double[4];

            double a = (y2 - y1) * (z3 - z1) - (z2 - z1) * (y3 - y1);
            double b = (z2 - z1) * (x3 - x1) - (x2 - x1) * (z3 - z1);
            double c = (x2 - x1) * (y3 - y1) - (y2 - y1) * (x3 - x1);
            double M = Math.Sqrt(a * a + b * b + c * c);

            a = a / M;
            b = b / M;
            c = c / M;

            double d = -1 * (a * x1 + b * y1 + c * z1);

            plane[0] = a;
            plane[1] = b;
            plane[2] = c;
            plane[3] = d;

            return plane;

        }

        private static Matrix4D SimplifyComputeK(double[] plane)
        {
            Matrix4D k = Matrix4D.ZeroMatrix;
            double a = plane[0];
            double b = plane[1];
            double c = plane[2];
            double d = plane[3];

            k[0] = a * a; k[1] = a * b; k[2] = a * c; k[3] = a * d;
            k[4] = a * b; k[5] = b * b; k[6] = b * c; k[7] = b * d;
            k[8] = a * c; k[9] = b * c; k[10] = c * c; k[11] = c * d;
            k[12] = a * d; k[13] = b * d; k[14] = c * d; k[15] = d * d;

            return k;
        }

        private static Matrix4D ComputePlane(Plane plane)
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

            Matrix4D QEdge = Matrix4D.ZeroMatrix;
            QEdge = vertexMatrix[v1] + vertexMatrix[v2];

            //if q_bar is symmetric
            if (QEdge[1] != QEdge[4] || QEdge[2] != QEdge[8] || QEdge[6] != QEdge[9] ||
               QEdge[3] != QEdge[12] || QEdge[7] != QEdge[13] || QEdge[11] != QEdge[14]
                ) throw new Exception("Matrix Qbar is not symmetric");

            //Generate Q_delta
            Matrix4D Qdelta = Matrix4D.ZeroMatrix;
            for (int i = 0; i <= 11; i++)
            {
                Qdelta[i] = QEdge[i];
            }
            Qdelta[12] = 0;
            Qdelta[13] = 0;
            Qdelta[14] = 0;
            Qdelta[15] = 1;

            //check q_delta if is invertible
            Vector3D newVertex = new Vector3D();
            double min_error = double.MaxValue;
            double det = Util.Solve(ref Qdelta, ref newVertex);
            if (det != 0)
            {
                min_error = this.VertexError(QEdge, newVertex);
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
                    double cur_error = this.VertexError(QEdge, vecArr[i]);
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
