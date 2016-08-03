using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class VertexPair
    {
        public TriMesh.Vertex Src;
        public TriMesh.Vertex Dst;

        public VertexPair(TriMesh.Vertex src, TriMesh.Vertex dst)
        {
            this.Src = src;
            this.Dst = dst;
        }
    }

    public class VertexMap : List<VertexPair>
    {
        TriMesh mesh;
        public VertexPair[] EdgeStore;

        public VertexMap(TriMesh mesh)
        {
            this.mesh = mesh;
            this.EdgeStore = new VertexPair[mesh.Edges.Count];
            for (int i = 0; i < mesh.Edges.Count; i++)
            {
                this.EdgeStore[i] = new VertexPair(mesh.Edges[i].Vertex0, mesh.Edges[i].Vertex1);
            }
        }

        public void Add(TriMesh.Vertex v1, TriMesh.Vertex v2)
        {
            this.Add(new VertexPair(v1, v2));
        }

        public TriMesh.Vertex[] GetOther(TriMesh.Vertex v, bool backward)
        {
            List<TriMesh.Vertex> list = new List<TriMesh.Vertex>();
            foreach (var item in this)
            {
                if (v == (backward ? item.Dst : item.Src))
                {
                    list.Add(backward ? item.Src : item.Dst);
                }
            }
            return list.ToArray();
        }

        public TriMesh.Vertex GetSrcVertex(TriMesh.Vertex v)
        {
            TriMesh.Vertex[] backward = this.GetOtherCascade(v, true);
            return backward.Length == 0 ? v : backward[backward.Length - 1];
        }

        public TriMesh.Vertex[] GetOtherCascade(TriMesh.Vertex v, bool backward)
        {
            List<TriMesh.Vertex> list = new List<TriMesh.Vertex>();
            Queue<TriMesh.Vertex> queue = new Queue<HalfEdgeMesh.Vertex>();
            queue.Enqueue(v);
            while (queue.Count > 0)
            {
                TriMesh.Vertex cur = queue.Dequeue();
                foreach (var item in this)
                {
                    if (cur == (backward ? item.Dst : item.Src))
                    {
                        list.Add(backward ? item.Src : item.Dst);
                        queue.Enqueue(backward ? item.Src : item.Dst);
                    }
                }
            }
            return list.ToArray();
        }

        public TriMesh.Vertex[] GetAllCascade(TriMesh.Vertex v)
        {
            TriMesh.Vertex src = this.GetSrcVertex(v);
            TriMesh.Vertex[] dst = this.GetOtherCascade(src, false);
            TriMesh.Vertex[] all = new HalfEdgeMesh.Vertex[dst.Length + 1];
            all[0] = src;
            for (int i = 0; i < dst.Length; i++)
            {
                all[i + 1] = dst[i];
            }
            return all;
        }

        /// <summary>
        /// 这里有时候有bug，原因不明
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public TriMesh.Edge GetOtherEdge(TriMesh.Vertex v1, TriMesh.Vertex v2)
        {
            TriMesh.Vertex[] arr1 = this.GetAllCascade(v1);
            TriMesh.Vertex[] arr2 = this.GetAllCascade(v2);
            foreach (var item1 in arr1)
            {
                foreach (var item2 in arr2)
                {
                    if (item1 != v1 || item2 != v2)
                    {
                        TriMesh.Edge edge = item1.FindEdgeTo(item2);
                        if (edge != null)
                        {
                            return edge;
                        }
                    }
                }
            }
            return null;
        }

        public double[] MapVertexValue(double[] src)
        {
            double[] dst = new double[this.mesh.Vertices.Count];
            foreach (var v in this.mesh.Vertices)
            {
                dst[v.Index] = src[this.GetSrcVertex(v).Index];
            }
            return dst;
        }

        /// <summary>
        /// cut后部分边的index改变，src是按照cut前边的index顺序，dst是按照cut后的
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public double[] MapEdgeValue(double[] src)
        {
            double[] dst = new double[this.mesh.Edges.Count];
            for (int i = 0; i < this.EdgeStore.Length; i++)
            {
                VertexPair pair=this.EdgeStore[i];
                TriMesh.Edge edge = pair.Src.FindEdgeTo(pair.Dst);
                TriMesh.Edge other = this.GetOtherEdge(pair.Src, pair.Dst);
                if (edge != null)
                {
                    dst[edge.Index] = src[i];
                }
                if (other != null)
                {
                    dst[other.Index] = src[i];
                }
            }
            return dst;
        }
    }
}
