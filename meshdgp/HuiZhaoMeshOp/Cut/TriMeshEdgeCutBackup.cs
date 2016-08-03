//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace GraphicResearchHuiZhao
//{
//    public class TriMeshEdgeCutFromBoundary
//    {
//        TriMesh mesh;
//        Map<TriMesh.Vertex, TriMesh.Vertex> map;
//        Map<TriMesh.Vertex, TriMesh.Vertex> edges;
//        double move;

//        public TriMeshEdgeCutFromBoundary(TriMesh mesh)
//        {
//            this.mesh = mesh;
//        }

//        public Map<TriMesh.Vertex, TriMesh.Vertex> Cut(double move)
//        {
//            this.map = new Map<HalfEdgeMesh.Vertex, HalfEdgeMesh.Vertex>();
//            this.edges = new Map<HalfEdgeMesh.Vertex, HalfEdgeMesh.Vertex>();
//            this.move = move;
            
//            foreach (var edge in this.mesh.Edges)
//            {
//                if (edge.Traits.SelectedFlag != 0)
//                {
//                    this.edges.Add(edge.Vertex0, edge.Vertex1);
//                }
//            }

//            bool boundary = false;
//            foreach (var edge in this.edges)
//            {
//                if (edge.Key.OnBoundary || edge.Value.OnBoundary)
//                {
//                    boundary = true;
//                    break;
//                }
//            }

//            if (!boundary)
//            {
//                this.CutStart();
//            }

//            int count = 1;
//            while (this.edges.Count != 0 && count!=0)
//            {
//                count = 0;
//                for (int i = 0; i < this.edges.Count; i++)
//                {
//                    TriMesh.Edge edge = this.edges[i].Key.FindEdgeTo(this.edges[i].Value);
//                    if (edge != null)
//                    {
//                        if (this.Cut(edge))
//                        {
//                            this.edges.RemoveAt(i);
//                            count++;
//                        }
//                    }
//                    else
//                    {
//                        var pair = this.edges[i];
//                        TriMesh.Vertex[] map1 = this.map.GetValues(pair.Key);
//                        TriMesh.Vertex[] map2 = this.map.GetValues(pair.Value);

//                        for (int m = 0; m < map1.Length; m++)
//                        {
//                            this.edges.Add(map1[m], pair.Value);
//                        }

//                        for (int n = 0; n < map2.Length; n++)
//                        {
//                            this.edges.Add(pair.Key, map2[n]);
//                        }

//                        for (int m = 0; m < map1.Length; m++)
//                        {
//                            for (int n = 0; n < map2.Length; n++)
//                            {
//                                this.edges.Add(map1[m], map2[n]);
//                            }
//                        }
//                        count++;
//                        this.edges.RemoveAt(i);
//                    }
//                }
//            }
//            return this.map;
//        }

//        void CutStart()
//        {
//            TriMesh.Vertex[] arr1 = null;
//            TriMesh.Vertex[] arr2 = null;
//            for (int i = 0; i < this.edges.Count; i++)
//            {
//                for (int j = 0; j < this.edges.Count; j++)
//                {
//                    if (i != j)
//                    {
//                        arr1 = new HalfEdgeMesh.Vertex[]{
//                        this.edges[i].Key,
//                        this.edges[i].Value
//                        };
//                        arr2 = new HalfEdgeMesh.Vertex[]{
//                        this.edges[j].Key,
//                        this.edges[j].Value
//                        };
//                        for (int k = 0; k < 4; k++)
//                        {
//                            int m = k / 2;
//                            int n = k % 2;
//                            if (arr1[m] == arr2[n])
//                            {
//                                TriMesh.HalfEdge cur = arr1[(m + 1) % 2].FindHalfedgeTo(arr1[m]);
//                                TriMesh.HalfEdge next = arr2[n].FindHalfedgeTo(arr2[(n + 1) % 2]);
//                                this.CutInner(cur, next);
//                                return;
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        bool Cut(TriMesh.Edge edge)
//        {
//            if (edge.Face0 != null && edge.Face1 != null)
//            {
//                bool b1 = edge.Vertex0.OnBoundary;
//                bool b2 = edge.Vertex1.OnBoundary;
//                if (b1 && b2)
//                {
//                    this.CutBothBoundary(edge);
//                    return true;
//                }
//                else if (!b1 && b2)
//                {
//                    this.CutFromBoundary(edge.HalfEdge0);
//                    return true;
//                }
//                else if (b1 && !b2)
//                {
//                    this.CutFromBoundary(edge.HalfEdge1);
//                    return true;
//                }
//            }
//            return false;
//        }

//        void CutInner(TriMesh.HalfEdge cur, TriMesh.HalfEdge next)
//        {
//            TriMesh.Vertex share1 = cur.FromVertex;
//            TriMesh.Vertex share2 = next.ToVertex;
//            TriMesh.Vertex mid = cur.ToVertex;

//            Vector3D normal = TriMeshUtil.ComputeNormalFace(cur.Face);
//            Vector3D vec = next.ToVertex.Traits.Position - cur.FromVertex.Traits.Position;
//            Vector3D dir = normal.Cross(vec).Normalize();


//            TriMesh.Vertex v2 = TriMeshModify.VertexSplit(mid, share1, share2,
//                mid.Traits.Position - dir * move, mid.Traits.Position + dir * move);

//            TriMesh.HalfEdge hf = mid.FindHalfedgeTo(v2);
//            TriMeshModify.RemoveEdge(hf.Edge);
//            this.map.Add(mid, v2);
//        }

//        /// <summary>
//        /// 源点在边界上
//        /// </summary>
//        /// <param name="hf"></param>
//        public void CutFromBoundary(TriMesh.HalfEdge hf)
//        {
//            TriMesh.HalfEdge[] arr = this.GetToBoundaryAntiClockWise(hf);

            
//            for (int i = 1; i < arr.Length; i++)
//            {
//                TriMeshModify.RemoveEdge(arr[i].Edge);
//            }

//            Vector3D vec = this.GetMoveVector(hf.Opposite);
//            TriMesh.Vertex v2 = this.Clone(hf.FromVertex, vec);

//            for (int i = 1; i < arr.Length; i++)
//            {
//                this.mesh.Faces.AddTriangles(v2, arr[i - 1].ToVertex, arr[i].ToVertex);
//            }
//        }

//        /// <summary>
//        /// 两端点都在边界上
//        /// </summary>
//        /// <param name="edge"></param>
//        void CutBothBoundary(TriMesh.Edge edge)
//        {
//            TriMesh.HalfEdge hf = edge.HalfEdge0;
//            TriMesh.HalfEdge[] leftArr = this.GetToBoundaryAntiClockWise(hf);
//            TriMesh.HalfEdge[] rightArr = this.GetToBoundaryClockWise(hf.Next);

          
//            for (int i = 1; i < leftArr.Length; i++)
//            {
//                TriMeshModify.RemoveEdge(leftArr[i].Edge);
//            }

//            for (int i = 0; i < rightArr.Length; i++)
//            {
//                TriMeshModify.RemoveEdge(rightArr[i].Edge);
//            }

//            Vector3D vec = this.GetMoveVector(hf.Opposite);
//            TriMesh.Vertex left = this.Clone(hf.FromVertex, vec);
//            TriMesh.Vertex right = this.Clone(hf.ToVertex, vec);

//            for (int i = 2; i < leftArr.Length; i++)
//            {
//                this.mesh.Faces.AddTriangles(left, leftArr[i - 1].ToVertex, leftArr[i].ToVertex);
//            }
//            for (int i = 1; i < rightArr.Length; i++)
//            {
//                this.mesh.Faces.AddTriangles(right, rightArr[i].ToVertex, rightArr[i - 1].ToVertex);
//            }
//            this.mesh.Faces.AddTriangles(left, right, leftArr[1].ToVertex);
//        }

//        TriMesh.HalfEdge[] GetToBoundaryClockWise(TriMesh.HalfEdge start)
//        {
//            List<TriMesh.HalfEdge> list = new List<HalfEdgeMesh.HalfEdge>();
//            while (start.Face != null)
//            {
//                list.Add(start);
//                start.Edge.Traits.SelectedFlag = 2;
//                start.Edge.Traits.Color = Color4.Red;
//                start = start.Opposite.Next;
//            }
//            return list.ToArray();
//        }

//        TriMesh.HalfEdge[] GetToBoundaryAntiClockWise(TriMesh.HalfEdge start)
//        {
//            List<TriMesh.HalfEdge> list = new List<HalfEdgeMesh.HalfEdge>();
//            list.Add(start);
//            while (start.Face != null)
//            {
//                start = start.Previous.Opposite;
//                list.Add(start);
//                start.Edge.Traits.SelectedFlag = 3;
//                start.Edge.Traits.Color = Color4.Purple;
//            }
//            return list.ToArray();
//        }

//        TriMesh.Vertex Clone(TriMesh.Vertex oldV, Vector3D moveVec)
//        {
//            TriMesh.Vertex newV = new HalfEdgeMesh.Vertex();
//            newV.Traits = new VertexTraits(oldV.Traits.Position);
//            this.map.Add(oldV, newV);
//            this.mesh.AppendToVertexList(newV);
//            oldV.Traits.Position += moveVec;
//            newV.Traits.Position -= moveVec;
//            return newV;
//        }

//        Vector3D GetMoveVector(TriMesh.HalfEdge hf)
//        {
//            Vector3D normal = TriMeshUtil.ComputeNormalFace(hf.Face);
//            Vector3D vec = hf.ToVertex.Traits.Position - hf.FromVertex.Traits.Position;
//            Vector3D dir = normal.Cross(vec).Normalize();
//            return dir * this.move;
//        }
//    }
//}
