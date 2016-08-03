//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace GraphicResearchHuiZhao
//{
//    public partial class TriMeshSimpification
//    {
//        #region QEM

//        private TriMesh.VertexDynamicTrait<Matrix4D> vertexMatrix = null;
//        private TriMesh.EdgeDynamicTrait<ErrorPair> edgeError;

//        private void SimplifyComputeInitQ()
//        {
//            foreach (TriMesh.Vertex vertex in mesh.Vertices)
//            {
//                Matrix4D q = SimplifyComputeVertexQ(vertex);
//                vertexMatrix[vertex] = q;
//            }
//        }

//        private static Matrix4D SimplifyComputeVertexQ(TriMesh.Vertex vertex)
//        {
//            Matrix4D q = Matrix4D.ZeroMatrix;

//            foreach (TriMesh.Face face in vertex.Faces)
//            {
//                TriMesh.Vertex v1 = face.GetVertex(0);
//                TriMesh.Vertex v2 = face.GetVertex(1);
//                TriMesh.Vertex v3 = face.GetVertex(2);

//                double[] plane = SimplifyComputePlane(v1, v2, v3);
//                Matrix4D k = SimplifyComputeK(plane);
//                q = q + k;

//            }
//            return q;
//        }

//        private static double[] SimplifyComputePlane(TriMesh.Vertex vertexA,
//            TriMesh.Vertex vertexB, TriMesh.Vertex vertexC)
//        {
//            double x1 = vertexA.Traits.Position.x;
//            double x2 = vertexB.Traits.Position.x;
//            double x3 = vertexC.Traits.Position.x;

//            double y1 = vertexA.Traits.Position.y;
//            double y2 = vertexB.Traits.Position.y;
//            double y3 = vertexC.Traits.Position.y;

//            double z1 = vertexA.Traits.Position.z;
//            double z2 = vertexB.Traits.Position.z;
//            double z3 = vertexC.Traits.Position.z;

//            double[] plane = new double[4];

//            double a = (y2 - y1) * (z3 - z1) - (z2 - z1) * (y3 - y1);
//            double b = (z2 - z1) * (x3 - x1) - (x2 - x1) * (z3 - z1);
//            double c = (x2 - x1) * (y3 - y1) - (y2 - y1) * (x3 - x1);
//            double M = Math.Sqrt(a * a + b * b + c * c);

//            a = a / M;
//            b = b / M;
//            c = c / M;

//            double d = -1 * (a * x1 + b * y1 + c * z1);

//            plane[0] = a;
//            plane[1] = b;
//            plane[2] = c;
//            plane[3] = d;

//            return plane;

//        }

//        private static Matrix4D SimplifyComputeK(double[] plane)
//        {
//            Matrix4D k = Matrix4D.ZeroMatrix;
//            double a = plane[0];
//            double b = plane[1];
//            double c = plane[2];
//            double d = plane[3];

//            k[0] = a * a; k[1] = a * b; k[2] = a * c; k[3] = a * d;
//            k[4] = a * b; k[5] = b * b; k[6] = b * c; k[7] = b * d;
//            k[8] = a * c; k[9] = b * c; k[10] = c * c; k[11] = c * d;
//            k[12] = a * d; k[13] = b * d; k[14] = c * d; k[15] = d * d;

//            return k;
//        }

//        private ErrorPair CalculateError(TriMesh.Edge edge)
//        {
//            TriMesh.Vertex v1 = edge.Vertex0;
//            TriMesh.Vertex v2 = edge.Vertex1;

//            Matrix4D Qbar = Matrix4D.ZeroMatrix;
//            Qbar = vertexMatrix[v1] + vertexMatrix[v2];

//            //if q_bar is symmetric
//            if (Qbar[1] != Qbar[4] || Qbar[2] != Qbar[8] || Qbar[6] != Qbar[9] ||
//               Qbar[3] != Qbar[12] || Qbar[7] != Qbar[13] || Qbar[11] != Qbar[14]
//                ) throw new Exception("Matrix Qbar is not symmetric");

//            //Generate Q_delta
//            Matrix4D Qdelta = Matrix4D.ZeroMatrix;
//            for (int i = 0; i <= 11; i++)
//            {
//                Qdelta[i] = Qbar[i];
//            }
//            Qdelta[12] = 0;
//            Qdelta[13] = 0;
//            Qdelta[14] = 0;
//            Qdelta[15] = 1;

//            //check q_delta if is invertible
//            Vector3D newVertex = new Vector3D();
//            double min_error = double.MaxValue;

//            double det = Qdelta.SubDeterminate(0, 1, 2, 4, 5, 6, 8, 9, 10);
//            if (det != 0)
//            {
//                newVertex.x = -1 / det * (Qdelta.SubDeterminate(1, 2, 3, 5, 6, 7, 9, 10, 11));
//                newVertex.y = 1 / det * (Qdelta.SubDeterminate(0, 2, 3, 4, 6, 7, 8, 10, 11));
//                newVertex.z = -1 / det * (Qdelta.SubDeterminate(0, 1, 3, 4, 5, 7, 8, 9, 11));
//                min_error = this.VertexError(Qbar, newVertex);
//            }
//            else
//            {
//                Vector3D[] vecArr = new Vector3D[3]{
//                    v1.Traits.Position,
//                    v2.Traits.Position,
//                    (v1.Traits.Position + v2.Traits.Position) / 2
//                };

//                for (int i = 0; i < 3; i++)
//                {
//                    double cur_error = this.VertexError(Qbar, vecArr[i]);
//                    if (cur_error < min_error)
//                    {
//                        min_error = cur_error;
//                        newVertex = vecArr[i];
//                    }
//                }
//            }
//            return new ErrorPair() { Pos = newVertex, Error = min_error };
//        }

//        private double VertexError(Matrix4D Q, Vector3D v)
//        {
//            return this.VertexError(Q, v.x, v.y, v.z);
//        }

//        private double VertexError(Matrix4D Q, double x, double y, double z)
//        {
//            return Q[0] * x * x + 2 * Q[1] * x * y + 2 * Q[2] * x * z + 2 * Q[3] * x +
//                Q[5] * y * y + 2 * Q[6] * y * z + 2 * Q[7] * y +
//                Q[10] * z * z + 2 * Q[11] * z + Q[15];
//        }

//        private TriMesh.Edge GetMinErrorEdge()
//        {
//            TriMesh.Edge target = null;
//            double min = double.MaxValue;
//            foreach (var item in this.mesh.Edges)
//            {
//                double error = this.edgeError[item].Error;
//                if (error < min && item.HalfEdge0 != null && item.HalfEdge1 != null
//                    && TriMeshModify.IsMergeable(item))
//                {
//                    target = item;
//                    min = error;
//                }
//            }
//            return target;
//        }

//        public int SimplifyQEM(int faceToPreserveCount)
//        {
//            this.vertexMatrix = new TriMesh.VertexDynamicTrait<Matrix4D>(this.mesh);
//            this.edgeError = new TriMesh.EdgeDynamicTrait<ErrorPair>(this.mesh);

//            this.SimplifyComputeInitQ();

//            foreach (TriMesh.Edge edge in mesh.Edges)
//            {
//                edgeError[edge] = CalculateError(edge);
//            }

//            int n = 0;

//            while (faceToPreserveCount < mesh.Faces.Count)
//            {
//                TriMesh.Edge mergeTarget = this.GetMinErrorEdge();
//                if (mergeTarget == null)
//                {
//                    break;
//                }

//                //V1 Update
//                TriMesh.Vertex v1 = mergeTarget.Vertex0;
//                TriMesh.Vertex v2 = mergeTarget.Vertex1;

//                Vector3D newPos = CalculateError(mergeTarget).Pos;
//                TriMesh.Vertex v3 = TriMeshModify.MergeEdge(mergeTarget, newPos);
//                vertexMatrix[v3] = vertexMatrix[v1] + vertexMatrix[v2];

//                //Update Each
//                foreach (TriMesh.Edge edge in v1.Edges)
//                {
//                    edgeError[edge] = CalculateError(edge);
//                }

//                n++;
//            }
//            TriMeshUtil.FixIndex(mesh);

//            return n;
//        }

//        struct ErrorPair
//        {
//            public Vector3D Pos;
//            public double Error;
//        }


//        #endregion
//    }
//}
