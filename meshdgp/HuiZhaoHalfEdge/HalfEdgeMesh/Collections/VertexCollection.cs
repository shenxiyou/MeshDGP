

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GraphicResearchHuiZhao
{
    public partial class HalfEdgeMesh 
    {
         

        #region VertexCollection class
        /// <summary>
        /// Type allowing vertices to be accessed like an array.
        /// </summary>
        [Serializable]
        public class VertexCollection : IEnumerable<Vertex>
        {
            #region Fields
            readonly HalfEdgeMesh  mesh;
            #endregion

            #region Constructors
            internal VertexCollection(HalfEdgeMesh  m)
            {
                mesh = m;
            }
            #endregion

            #region Properties
            /// <summary>
            /// Accesses the vertices in a mesh.
            /// </summary>
            /// <param name="index">The index of the vertex.</param>
            /// <returns>The indexed <see cref="Vertex"/>.</returns>
            public Vertex this[int index]
            {
                get
                {
                    return mesh.vertices[index];
                }
            }

            /// <summary>
            /// The number of vertices in the mesh.
            /// </summary>
            public int Count
            {
                get
                {
                    return mesh.vertices.Count;
                }
            }
            #endregion

            #region Iterators
            /// <summary>
            /// Provides an enumerator for the vertices of the mesh.
            /// </summary>
            /// <returns>A vertex of the mesh.</returns>
            public IEnumerator<Vertex> GetEnumerator()
            {
                foreach (Vertex v in mesh.vertices)
                {
                    yield return v;
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

            #region Methods
            /// <summary>
            /// Adds a vertex to the mesh.
            /// </summary>
            /// <returns>The vertex created by this method.</returns>
            public Vertex Add()
            {
                Vertex v = new Vertex();
                mesh.AppendToVertexList(v);
                return v;
            }

            /// <summary>
            /// Adds a vertex to the mesh with the specified vertex traits.
            /// </summary>
            /// <param name="vertexTraits">The custom traits for the vertex to add to the mesh.</param>
            /// <returns>The vertex created by this method.</returns>
            public Vertex Add(VertexTraits vertexTraits)
            {
                Vertex v = new Vertex(vertexTraits);
                mesh.AppendToVertexList(v);
                return v;
            }

            //public void RemoveFromList(Vertex vertex) 
            //{
            //    this.mesh.vertices.Remove(vertex);
            //}

            //public void Remove(Vertex vertex)
            //{ 
            //    foreach (HalfEdge halfEdge in vertex.Halfedges)
            //    {

            //        if (halfEdge.Next != null)
            //        {
            //            halfEdge.Next.Face = null;
            //            halfEdge.Opposite.Previous.Next = halfEdge.Next;
            //            halfEdge.Next.Previous = halfEdge.Opposite.Previous;
            //        }

            //        if (halfEdge.ToVertex.HalfEdge == halfEdge.Opposite)
            //        {
            //            halfEdge.ToVertex.HalfEdge = halfEdge.Next;
            //        }

            //        this.mesh.halfEdges.Remove(halfEdge.Opposite);
            //        this.mesh.halfEdges.Remove(halfEdge);
            //    }

            //    foreach (Face face in vertex.Faces)
            //    {
            //        this.mesh.faces.Remove(face);
            //    }

            //    foreach (Edge edge in vertex.Edges)
            //    {
            //        this.mesh.edges.Remove(edge);
            //    }


            //    this.mesh.vertices.Remove(vertex);
            //}
            #endregion
        }
        #endregion
    }
}