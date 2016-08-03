

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GraphicResearchHuiZhao
{
    public partial class HalfEdgeMesh 
    {
        #region EdgeCollection class
        /// <summary>
        /// Type allowing edges to be accessed like an array.
        /// </summary>
        [Serializable]
        public class EdgeCollection : IEnumerable<Edge>
        {
            #region Fields
            readonly HalfEdgeMesh  mesh;
            #endregion

            #region Constructors
            internal EdgeCollection(HalfEdgeMesh  m)
            {
                mesh = m;
            }
            #endregion

            #region Properties
            /// <summary>
            /// Accesses the edges in a mesh.
            /// </summary>
            /// <param name="index">The index of the edge.</param>
            /// <returns>The indexed <see cref="Edge"/>.</returns>
            public Edge this[int index]
            {
                get
                {
                    return mesh.edges[index];
                }
            }

            /// <summary>
            /// The number of edges in the mesh.
            /// </summary>
            public int Count
            {
                get
                {
                    return mesh.edges.Count;
                }
            }
            #endregion

            #region Iterators
            /// <summary>
            /// Provides an enumerator for the edges of the mesh.
            /// </summary>
            /// <returns>An edge of the mesh.</returns>
            public IEnumerator<Edge> GetEnumerator()
            {
                foreach (Edge e in mesh.edges)
                {
                    yield return e;
                }
            }

            /// <summary>
            /// Useless IEnumerable.GetEnumerator() implementation.
            /// </summary>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            #endregion

            //public void RemoveFormList(Edge e) 
            //{
            //    this.mesh.edges.Remove(e);
            //}

            //public void Remove(Edge edge)
            //{
            //    edge.HalfEdge0.Next.Previous = edge.HalfEdge1.Previous;
            //    edge.HalfEdge0.Previous.Next = edge.HalfEdge1.Next;

            //    edge.HalfEdge1.Next.Previous = edge.HalfEdge0.Previous;
            //    edge.HalfEdge1.Previous.Next = edge.HalfEdge0.Next;

            //    edge.HalfEdge0.ToVertex.HalfEdge = edge.HalfEdge0.Next;
            //    edge.HalfEdge1.ToVertex.HalfEdge = edge.HalfEdge1.Next;

            //    edge.HalfEdge0.Next.Face = null;
            //    edge.HalfEdge0.Previous.Face = null;

            //    edge.HalfEdge1.Next.Face = null;
            //    edge.HalfEdge1.Previous.Face = null; 

            //    if (edge.Face0 != null)
            //    {
            //        this.mesh.faces.Remove(edge.Face0);
            //    }

            //    if (edge.Face1 != null)
            //    {
            //        this.mesh.faces.Remove(edge.Face1);
            //    }
            //    this.mesh.halfEdges.Remove(edge.HalfEdge1);
            //    this.mesh.halfEdges.Remove(edge.HalfEdge0);

            //    this.mesh.edges.Remove(edge);
               
            //}
        }
        #endregion

       
    }
}