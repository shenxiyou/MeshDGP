using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class TriMeshModify
    {
        public TriMesh.HalfEdge[][] Sort(TriMesh mesh)
        {
            List<TriMesh.HalfEdge[]> list = new List<HalfEdgeMesh.HalfEdge[]>();
            LinkedList<TriMesh.Edge> src = new LinkedList<HalfEdgeMesh.Edge>();
            foreach (var edge in mesh.Edges)
            {
                if (edge.Traits.SelectedFlag != 0)
                {
                    src.AddLast(edge);
                }
            }
            while (src.Count != 0)
            {
                list.Add(Sort(src));
            }
            return list.ToArray();
        }

        public static TriMesh.HalfEdge[] Sort(LinkedList<TriMesh.Edge> src)
        {
            LinkedList<TriMesh.HalfEdge> dst = new LinkedList<HalfEdgeMesh.HalfEdge>();
            dst.AddLast(src.First.Value.HalfEdge0);
            src.RemoveFirst();
            LinkedListNode<TriMesh.Edge> node = src.First;
            int count = 0;
            while (node != null)
            {
                TriMesh.Vertex first = dst.First.Value.FromVertex;
                TriMesh.Vertex last = dst.Last.Value.ToVertex;

                if (node.Value.Vertex0 == first)
                {
                    dst.AddFirst(node.Value.HalfEdge0);
                    src.Remove(node);
                    count++;
                }
                else if (node.Value.Vertex1 == first)
                {
                    dst.AddFirst(node.Value.HalfEdge1);
                    src.Remove(node);
                    count++;
                }
                else if (node.Value.Vertex0 == last)
                {
                    dst.AddLast(node.Value.HalfEdge1);
                    src.Remove(node);
                    count++;
                }
                else if (node.Value.Vertex1 == last)
                {
                    dst.AddLast(node.Value.HalfEdge0);
                    src.Remove(node);
                    count++;
                }
                node = node.Next;
                if (node == null && count != 0)
                {
                    node = src.First;
                    count = 0;
                }
            }
            TriMesh.HalfEdge[] arr = new HalfEdgeMesh.HalfEdge[dst.Count];
            dst.CopyTo(arr, 0);
            return arr;
        }
    }
}
