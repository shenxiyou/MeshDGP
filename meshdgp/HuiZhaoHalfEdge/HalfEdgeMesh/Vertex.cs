

using System;
using System.Collections.Generic;

namespace GraphicResearchHuiZhao
{
    public partial class HalfEdgeMesh 
    {
        /// <summary>
        /// A vertex of the mesh.
        /// </summary>
        [Serializable]
        public class Vertex 
        {
            #region Fields
            /// <summary>
            /// The custom traits for the vertex.
            /// </summary>
            public VertexTraits Traits;

            private HalfEdge halfEdge;
            private HalfEdgeMesh  mesh;
            private int index;
            #endregion

            #region Constructors
            /// <summary>
            /// Creates a vertex with traits set to their default value.
            /// </summary>
            public Vertex() { }

            /// <summary>
            /// Creates a vertex with the given traits.
            /// </summary>
            /// <param name="vertexTraits">Traits for this vertex.</param>
            public Vertex(VertexTraits vertexTraits)
            {
                Traits = vertexTraits;
            }
            #endregion

            #region Properties
            /// <summary>
            /// The number of edges connected to the vertex.
            /// </summary>
            public int EdgeCount
            {
                get
                {
                    int count = 0;
                    foreach (Edge e in Edges)
                    {
                        ++count;
                    }
                    return count;
                }
            }

            /// <summary>
            /// The number of faces with the vertex.
            /// </summary>
            public int FaceCount
            {
                get
                {
                    int count = 0;
                    foreach (Face f in Faces)
                    {
                        ++count;
                    }
                    return count;
                }
            }

            /// <summary>
            /// A halfedge that originates from the vertex.
            /// </summary>
            public HalfEdge HalfEdge
            {
                get
                {
                    return halfEdge;
                }
                 set
                {
                    halfEdge = value;
                }
            }

            /// <summary>
            /// The number of halfedges from the vertex.
            /// </summary>
            public int HalfEdgeCount
            {
                get
                {
                    int count = 0;
                    foreach (HalfEdge h in HalfEdges)
                    {
                        ++count;
                    }
                    return count;
                }
            }

            /// <summary>
            /// The index of this in the mesh's interal vertex list.
            /// </summary>
            public int Index
            {
                get
                {
                    return index;
                }
                 set
                {
                    index = value;
                }
            }

            /// <summary>
            /// The mesh the vertex belongs to.
            /// </summary>
            public HalfEdgeMesh  Mesh
            {
                get
                {
                    return mesh;
                }
                set
                {
                    mesh = value;
                }
            }

            /// <summary>
            /// Checks if the vertex is on the boundary of the mesh.
            /// </summary>
            public bool OnBoundary
            {
                get
                {
                    if (this.halfEdge == null)
                    {
                        return true;
                    }

                    // Search adjacent faces for any that are null
                    foreach (HalfEdge h in HalfEdges)
                    {
                        if (h.OnBoundary)
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }

            /// <summary>
            /// The number of vertices in the one ring neighborhood.
            /// </summary>
            public int VertexCount
            {
                get
                {
                    int count = 0;
                    foreach (Vertex v in Vertices)
                    {
                        ++count;
                    }
                    return count;
                }
            }
            #endregion

            #region Iterators
            /// <summary>
            /// An iterator for edges connected to the vertex.
            /// </summary>
            public IEnumerable<Edge> Edges
            {
                get
                {
                    foreach (HalfEdge h in HalfEdges)
                    {
                        yield return h.Edge;
                    }
                }
            }

            /// <summary>
            /// An iterator for the faces with the vertex.
            /// </summary>
            public IEnumerable<Face> Faces
            {
                get
                {
                    foreach (HalfEdge h in HalfEdges)
                    {
                        if (h.Face != null)
                        {
                            yield return h.Face;
                        }
                    }
                }
            }

            /// <summary>
            /// An iterator for the halfedges originating from the vertex.
            /// </summary>
            public IEnumerable<HalfEdge> HalfEdges
            {
                get
                {
                    HalfEdge h = this.halfEdge;

                    if (h != null)
                    {
                        do
                        {
                            yield return h;
                            h = h.Opposite.Next;
                        } while (h != this.halfEdge);
                    }
                }
            }

            /// <summary>
            /// An iterator for the vertices in the one ring neighborhood.
            /// </summary>
            public IEnumerable<Vertex> Vertices
            {
                get
                {
                    foreach (HalfEdge h in HalfEdges)
                    {
                        yield return h.ToVertex;
                    }
                }
            }
            #endregion

            #region Methods
            /// <summary>
            /// Searches for the edge associated with the specified vertex.
            /// </summary>
            /// <param name="vertex">A vertex sharing an edge with this vertex.</param>
            /// <returns>The edge if it is found, otherwise null.</returns>
            public Edge FindEdgeTo(Vertex vertex)
            {
                foreach (HalfEdge h in HalfEdges)
                {
                    if (h.ToVertex == vertex)
                    {
                        return h.Edge;
                    }
                }
                return null;
            }

            /// <summary>
            /// Searches for the halfedge pointing to the specified face from this vertex.
            /// </summary>
            /// <param name="face">The face the halfedge to find points to.</param>
            /// <returns>The halfedge if it is found, otherwise null.</returns>
            public HalfEdge FindHalfedgeTo(Face face)
            {
                foreach (HalfEdge h in HalfEdges)
                {
                    if (h.Face == face)
                    {
                        return h;
                    }
                }
                return null;
            }

            /// <summary>
            /// Searches for a halfedge pointing to a vertex from this vertex.
            /// </summary>
            /// <param name="vertex">A vertex pointed to by the halfedge to search for.</param>
            /// <returns>The halfedge from this vertex to the specified vertex. If none exists, returns null.</returns>
            public HalfEdge FindHalfedgeTo(Vertex vertex)
            {
                foreach (HalfEdge h in HalfEdges)
                {
                    if (h.ToVertex == vertex)
                    {
                        return h;
                    }
                }
                return null;
            }

            /// <summary>
            /// Searches for an indexed edge by iterating.
            /// </summary>
            /// <param name="index">The index of the edge to return.</param>
            /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is negative or too large.</exception>
            /// <returns>The specified edge.</returns>
            public Edge GetEdge(int index)
            {
                int count = 0;
                foreach (Edge e in Edges)
                {
                    if (count == index)
                    {
                        return e;
                    }
                    ++count;
                }
                throw new ArgumentOutOfRangeException("index");
            }

            /// <summary>
            /// Searches for an indexed face by iterating.
            /// </summary>
            /// <param name="index">The index of the face to return.</param>
            /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is negative or too large.</exception>
            /// <returns>The specified face.</returns>
            public Face GetFace(int index)
            {
                int count = 0;
                foreach (Face f in Faces)
                {
                    if (count == index)
                    {
                        return f;
                    }
                    ++count;
                }
                throw new ArgumentOutOfRangeException("index");
            }

            /// <summary>
            /// Searches for an indexed halfedge by iterating.
            /// </summary>
            /// <param name="index">The index of the halfedge to return.</param>
            /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is negative or too large.</exception>
            /// <returns>The specified halfedge.</returns>
            public HalfEdge GetHalfedge(int index)
            {
                int count = 0;
                foreach (HalfEdge h in HalfEdges)
                {
                    if (count == index)
                    {
                        return h;
                    }
                    ++count;
                }
                throw new ArgumentOutOfRangeException("index");
            }

            /// <summary>
            /// Searches for an indexed vertex by iterating.
            /// </summary>
            /// <param name="index">The index of the vertex to return.</param>
            /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is negative or too large.</exception>
            /// <returns>The specified vertex.</returns>
            public Vertex GetVertex(int index)
            {
                int count = 0;
                foreach (Vertex v in Vertices)
                {
                    if (count == index)
                    {
                        return v;
                    }
                    ++count;
                }
                throw new ArgumentOutOfRangeException("index");
            }
            #endregion

             
        }
    }
}
