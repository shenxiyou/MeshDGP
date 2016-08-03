using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class TriMeshEdgeCutFromBoundary
    {
        TriMesh mesh;
        VertexMap vertexMap;
        Queue<VertexPair> edgeQueue;
        double move;

        public TriMeshEdgeCutFromBoundary(TriMesh mesh)
        {
            this.mesh = mesh;
        }

        public VertexMap Cut(double move)
        {
            this.vertexMap = new VertexMap(this.mesh);
            this.edgeQueue = new Queue<VertexPair>();
            this.move = move;
            
            foreach (var edge in this.mesh.Edges)
            {
                if (edge.Traits.SelectedFlag != 0)
                {
                    this.edgeQueue.Enqueue(new VertexPair(edge.Vertex0, edge.Vertex1));
                }
            }

            bool boundary = false;
            foreach (var edge in this.edgeQueue)
            {
                if (edge.Src.OnBoundary || edge.Dst.OnBoundary)
                {
                    boundary = true;
                    break;
                }
            }

            if (!boundary)
            {
                this.CutStart();
            }

            int count = 1;
            while (this.edgeQueue.Count != 0 && count < this.edgeQueue.Count)
            {
                VertexPair pair = this.edgeQueue.Dequeue();
                TriMesh.Edge edge = pair.Src.FindEdgeTo(pair.Dst);
                if (edge != null)
                {
                    if (this.Cut(edge))
                    {
                        count = 0;
                    }
                    else
                    {
                        this.edgeQueue.Enqueue(pair);
                        count++;
                    }
                }
                else
                {
                    TriMesh.Vertex[] map1 = this.vertexMap.GetOther(pair.Src, false);
                    TriMesh.Vertex[] map2 = this.vertexMap.GetOther(pair.Dst, false);

                    for (int m = 0; m < map1.Length; m++)
                    {
                        this.edgeQueue.Enqueue(new VertexPair(map1[m], pair.Dst));
                    }

                    for (int n = 0; n < map2.Length; n++)
                    {
                        this.edgeQueue.Enqueue(new VertexPair(pair.Src, map2[n]));
                    }

                    for (int m = 0; m < map1.Length; m++)
                    {
                        for (int n = 0; n < map2.Length; n++)
                        {
                            this.edgeQueue.Enqueue(new VertexPair(map1[m], map2[n]));
                        }
                    }
                    count = 0;
                }
            }
            TriMeshUtil.FixIndex(mesh);
          
            return this.vertexMap;
        }

        void CutStart()
        {
            for (int i = 0; i < this.edgeQueue.Count; i++)
            {
                VertexPair edge1 = this.edgeQueue.Dequeue();
                for (int j = 0; j < this.edgeQueue.Count; j++)
                {
                    VertexPair edge2 = this.edgeQueue.Dequeue();
                    TriMesh.HalfEdge hf1 = null;
                    TriMesh.HalfEdge hf2 = null;
                    if (edge1.Src == edge2.Src)
                    {
                        hf1 = edge1.Dst.FindHalfedgeTo(edge1.Src);
                        hf2 = edge2.Src.FindHalfedgeTo(edge2.Dst);
                    }
                    else if (edge1.Src == edge2.Dst)
                    {
                        hf1 = edge1.Dst.FindHalfedgeTo(edge1.Src);
                        hf2 = edge2.Dst.FindHalfedgeTo(edge2.Src);
                    }
                    else if (edge1.Dst == edge2.Src)
                    {
                        hf1 = edge1.Src.FindHalfedgeTo(edge1.Dst);
                        hf2 = edge2.Src.FindHalfedgeTo(edge2.Dst);
                    }
                    else if (edge1.Dst == edge2.Dst)
                    {
                        hf1 = edge1.Src.FindHalfedgeTo(edge1.Dst);
                        hf2 = edge2.Dst.FindHalfedgeTo(edge2.Src);
                    }
                    if (hf1 != null && hf2 != null)
                    {
                        this.CutInner(hf1, hf2);
                        return;
                    }
                    else
                    {
                        this.edgeQueue.Enqueue(edge2);
                    }
                }
                this.edgeQueue.Enqueue(edge1);
            }
        }

        bool Cut(TriMesh.Edge edge)
        {
            if (edge.Face0 != null && edge.Face1 != null)
            {
                bool b1 = edge.Vertex0.OnBoundary;
                bool b2 = edge.Vertex1.OnBoundary;
                if (b1 && b2)
                {
                    this.CutBothBoundary(edge);
                    return true;
                }
                else if (!b1 && b2)
                {
                    this.CutFromBoundary(edge.HalfEdge0);
                    return true;
                }
                else if (b1 && !b2)
                {
                    this.CutFromBoundary(edge.HalfEdge1);
                    return true;
                }
            }
            return false;
        }

        void CutInner(TriMesh.HalfEdge cur, TriMesh.HalfEdge next)
        {
            TriMesh.Vertex share1 = cur.FromVertex;
            TriMesh.Vertex share2 = next.ToVertex;
            TriMesh.Vertex mid = cur.ToVertex;

            Vector3D normal = TriMeshUtil.ComputeNormalFace(cur.Face);
            Vector3D vec = next.ToVertex.Traits.Position - cur.FromVertex.Traits.Position;
            Vector3D dir = normal.Cross(vec).Normalize();

 
            TriMesh.Vertex v2 = TriMeshModify.VertexSplit(mid, share1, share2,
                mid.Traits.Position - dir * move, mid.Traits.Position + dir * move);

            TriMesh.HalfEdge hf = mid.FindHalfedgeTo(v2);
            TriMeshModify.RemoveEdge(hf.Edge);
            this.vertexMap.Add(mid, v2);
        }

        /// <summary>
        /// 源点在边界上
        /// </summary>
        /// <param name="hf"></param>
        void CutFromBoundary(TriMesh.HalfEdge hf)
        {
            TriMesh.HalfEdge[] arr = this.GetToBoundaryAntiClockWise(hf);

     
            for (int i = 1; i < arr.Length; i++)
            {
                TriMeshModify.RemoveEdge(arr[i].Edge);
            }

            Vector3D vec = this.GetMoveVector(hf.Opposite);
            TriMesh.Vertex v2 = this.Clone(hf.FromVertex, vec);

            for (int i = 1; i < arr.Length; i++)
            {
                this.mesh.Faces.AddTriangles(v2, arr[i - 1].ToVertex, arr[i].ToVertex);
            }
        }

        /// <summary>
        /// 两端点都在边界上
        /// </summary>
        /// <param name="edge"></param>
        void CutBothBoundary(TriMesh.Edge edge)
        {
            TriMesh.HalfEdge hf = edge.HalfEdge0;
            TriMesh.HalfEdge[] leftArr = this.GetToBoundaryAntiClockWise(hf);
            TriMesh.HalfEdge[] rightArr = this.GetToBoundaryClockWise(hf.Next);

        
            for (int i = 1; i < leftArr.Length; i++)
            {
                TriMeshModify.RemoveEdge(leftArr[i].Edge);
            }

            for (int i = 0; i < rightArr.Length; i++)
            {
                TriMeshModify.RemoveEdge(rightArr[i].Edge);
            }

            Vector3D vec = this.GetMoveVector(hf.Opposite);
            TriMesh.Vertex left = this.Clone(hf.FromVertex, vec);
            TriMesh.Vertex right = this.Clone(hf.ToVertex, vec);

            for (int i = 2; i < leftArr.Length; i++)
            {
                this.mesh.Faces.AddTriangles(left, leftArr[i - 1].ToVertex, leftArr[i].ToVertex);
            }
            for (int i = 1; i < rightArr.Length; i++)
            {
                this.mesh.Faces.AddTriangles(right, rightArr[i].ToVertex, rightArr[i - 1].ToVertex);
            }
            this.mesh.Faces.AddTriangles(left, right, leftArr[1].ToVertex);
        }

        TriMesh.HalfEdge[] GetToBoundaryClockWise(TriMesh.HalfEdge start)
        {
            List<TriMesh.HalfEdge> list = new List<HalfEdgeMesh.HalfEdge>();
            while (start.Face != null)
            {
                list.Add(start);
                start.Edge.Traits.SelectedFlag = 2;
                start.Edge.Traits.Color = Color4.Red;
                start = start.Opposite.Next;
            }
            return list.ToArray();
        }

        TriMesh.HalfEdge[] GetToBoundaryAntiClockWise(TriMesh.HalfEdge start)
        {
            List<TriMesh.HalfEdge> list = new List<HalfEdgeMesh.HalfEdge>();
            list.Add(start);
            while (start.Face != null)
            {
                start = start.Previous.Opposite;
                list.Add(start);
                start.Edge.Traits.SelectedFlag = 3;
                start.Edge.Traits.Color = Color4.Purple;
            }
            return list.ToArray();
        }

        TriMesh.Vertex Clone(TriMesh.Vertex oldV, Vector3D moveVec)
        {
            TriMesh.Vertex newV = new HalfEdgeMesh.Vertex();
            newV.Traits = new VertexTraits(oldV.Traits.Position);
            this.vertexMap.Add(oldV, newV);
            this.mesh.AppendToVertexList(newV);
            oldV.Traits.Position += moveVec;
            newV.Traits.Position -= moveVec;
            return newV;
        }

        Vector3D GetMoveVector(TriMesh.HalfEdge hf)
        {
            Vector3D normal = TriMeshUtil.ComputeNormalFace(hf.Face);
            Vector3D vec = hf.ToVertex.Traits.Position - hf.FromVertex.Traits.Position;
            Vector3D dir = normal.Cross(vec).Normalize();
            return dir * this.move;
        }
    }
}
