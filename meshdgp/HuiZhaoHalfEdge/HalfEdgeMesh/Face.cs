using System;
using System.Collections.Generic;
 

namespace GraphicResearchHuiZhao
{
    public partial class HalfEdgeMesh
    {
        /// <summary>
        /// A face of the mesh.
        /// </summary>
        [Serializable]
        public class Face
        { 
            public FaceTraits Traits=new FaceTraits(); 
            private HalfEdge halfedge;
            private int index;  
            public Face() 
            { 
                Traits = new FaceTraits(); 
            } 
            public Face(FaceTraits faceTraits)
            {
                if (faceTraits == null)
                {
                    Traits = new FaceTraits();
                }
                else
                {
                    Traits = faceTraits;
                }
            }
            public IEnumerable<Edge> Edges
            {
                get
                {
                    foreach (HalfEdge h in Halfedges)
                    {
                        yield return h.Edge;
                    }
                }
            }
            public IEnumerable<Face> Faces
            {
                get
                {
                    foreach (HalfEdge h in Halfedges)
                    {
                        if (h.Opposite.Face != null)
                            yield return h.Opposite.Face;
                    }
                }
            }
            public IEnumerable<HalfEdge> Halfedges
            {
                get
                {
                    HalfEdge h = this.halfedge;

                    do
                    {
                        yield return h;
                        h = h.Next;
                    } while (h != this.halfedge);
                }
            }
            public IEnumerable<Vertex> Vertices
            {
                get
                {
                    foreach (HalfEdge h in Halfedges)
                    {
                        yield return h.ToVertex;
                    }
                }
            }
            public bool OnBoundary
            {
                get
                {
                    foreach (HalfEdge h in Halfedges)
                    {
                        if (h.Opposite.OnBoundary)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            } 



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
            public HalfEdge HalfEdge
            {
                get
                {
                    return halfedge;
                }
                set
                {
                    halfedge = value;
                }
            } 
            public int HalfedgeCount
            {
                get
                {
                    int count = 0;
                    foreach (HalfEdge h in Halfedges)
                    {
                        ++count;
                    }
                    return count;
                }
            }
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
            public HalfEdgeMesh Mesh
            {
                get
                {
                    return halfedge.Mesh;
                }
            } 

            #region Methods
            /// <summary>
            /// Searches for the edge associated with the specified face.
            /// </summary>
            /// <param name="face">A face sharing an edge with this face.</param>
            /// <returns>The edge if it is found, otherwise null.</returns>
            public Edge FindEdgeTo(Face face)
            {
                foreach (HalfEdge h in Halfedges)
                {
                    if (h.Opposite.Face == face)
                    {
                        return h.Edge;
                    }
                }
                return null;
            }

            /// <summary>
            /// Searches for the halfedge pointing to the specified vertex from this face.
            /// </summary>
            /// <param name="vertex">The vertex the halfedge to find points to.</param>
            /// <returns>The halfedge if it is found, otherwise null.</returns>
            public HalfEdge FindHalfedgeTo(Vertex vertex)
            {
                foreach (HalfEdge h in Halfedges)
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
                foreach (HalfEdge h in Halfedges)
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
