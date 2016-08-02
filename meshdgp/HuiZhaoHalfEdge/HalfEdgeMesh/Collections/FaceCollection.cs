

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GraphicResearchHuiZhao
{
    public partial class HalfEdgeMesh 
    {
       

        #region FaceCollection class
        /// <summary>
        /// Type allowing faces to be accessed like an array.
        /// </summary>
        [Serializable]
        public class FaceCollection : IEnumerable<Face>
        {
            #region Fields
            readonly HalfEdgeMesh  mesh;
            #endregion

            #region Constructors
            internal FaceCollection(HalfEdgeMesh  m)
            {
                mesh = m;
            }
            #endregion

            #region Properties
            /// <summary>
            /// Accesses the faces in a mesh.
            /// </summary>
            /// <param name="index">The index of the face.</param>
            /// <returns>The indexed <see cref="Face"/>.</returns>
            public Face this[int index]
            {
                get
                {
                    return mesh.faces[index];
                }
            }

            /// <summary>
            /// The number of faces in the mesh.
            /// </summary>
            public int Count
            {
                get
                {
                    return mesh.faces.Count;
                }
            }
            #endregion

            #region Iterators
            /// <summary>
            /// Provides an enumerator for the faces of the mesh.
            /// </summary>
            /// <returns>A face of the mesh.</returns>
            public IEnumerator<Face> GetEnumerator()
            {
                foreach (Face f in mesh.faces)
                {
                    yield return f;
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
            /// Adds a face to the mesh with default face traits.
            /// </summary>
            /// <param name="faceVertices">The vertices of the face in counterclockwise order.</param>
            /// <returns>The face created by this method.</returns>
            /// <exception cref="BadTopologyException">
            /// Thrown when fewer than three vertices are given or the vertices cannot form a valid face.
            /// </exception>
            /// <exception cref="ArgumentNullException">Thrown when a null vertex is given.</exception>
            public Face Add(params Vertex[] faceVertices)
            {
                return Add(default(FaceTraits), faceVertices);
            }

            /// <summary>
            /// Adds a face to the mesh with the specified face traits.
            /// </summary>
            /// <param name="faceTraits">The custom traits for the face to add to the mesh.</param>
            /// <param name="faceVertices">The vertices of the face in counterclockwise order.</param>
            /// <returns>The face created by this method.</returns>
            /// <exception cref="BadTopologyException">
            /// Thrown when fewer than three vertices are given or the vertices cannot form a valid face.
            /// </exception>
            /// <exception cref="ArgumentNullException">Thrown when a null vertex is given.</exception>
            public Face Add(FaceTraits faceTraits, params Vertex[] faceVertices)
            {
                if (mesh.trianglesOnly)
                {
                    return AddTriangles(faceTraits, faceVertices)[0];
                }
                else
                {
                    return CreateFace(faceTraits, faceVertices);
                }
            }

            /// <summary>
            /// Adds triangular faces to the mesh with default face traits.
            /// </summary>
            /// <param name="faceVertices">The vertices of the faces in counterclockwise order.</param>
            /// <returns>An array of faces created by this method.</returns>
            /// <exception cref="BadTopologyException">
            /// Thrown when fewer than three vertices are given or the vertices cannot form a valid face.
            /// </exception>
            /// <exception cref="ArgumentNullException">Thrown when a null vertex is given.</exception>
            public Face[] AddTriangles(params Vertex[] faceVertices)
            {
                return AddTriangles(default(FaceTraits), faceVertices);
            }

            /// <summary>
            /// Adds triangular faces to the mesh with the specified face traits.
            /// </summary>
            /// <param name="faceTraits">The custom traits for the faces to add to the mesh.</param>
            /// <param name="faceVertices">The vertices of the faces in counterclockwise order.</param>
            /// <returns>An array of faces created by this method.</returns>
            /// <exception cref="BadTopologyException">
            /// Thrown when fewer than three vertices are given or the vertices cannot form a valid face.
            /// </exception>
            /// <exception cref="ArgumentNullException">Thrown when a null vertex is given.</exception>
            public Face[] AddTriangles(FaceTraits faceTraits, params Vertex[] faceVertices)
            {
                int n = faceVertices.Length;

                // Require at least 3 vertices
                if (n < 3)
                {
                    throw new BadTopologyException("Cannot create a polygon with fewer than three vertices.");
                }

                Face[] addedFaces = new Face[n - 2];

                try
                {
                    // Triangulate the face
                    for (int i = 0; i < n - 2; ++i)
                    {
                        addedFaces[i] = CreateFace(faceTraits, faceVertices[0], faceVertices[i + 1], faceVertices[i + 2]);
                    }
                }
                catch
                {
                }

                return addedFaces;
            }

            //For quad situation
            public Face[] AddQuads(params Vertex[] faceVertices)
            {
                return AddQuads(default(FaceTraits), faceVertices);
            }

            public Face[] AddQuads(FaceTraits faceTraits, params Vertex[] faceVertices)
            {
                int n = faceVertices.Length;

                // Require at least 4 vertices
                if (n < 4)
                {
                    throw new BadTopologyException("Cannot create a polygon with fewer than three vertices.");
                }

                Face[] addedFaces = new Face[n - 3];

                // Quadalate the face
                for (int i = 0; i < n - 3; ++i)
                {
                    addedFaces[i] = CreateFace(faceTraits, faceVertices[0], faceVertices[i + 1], faceVertices[i + 2], faceVertices[i + 3]);
                }

                return addedFaces;
            }

            /// <summary>
            /// Adds a face to the mesh with the specified face traits.
            /// </summary>
            /// <param name="faceTraits">The custom traits for the face to add to the mesh.</param>
            /// <param name="faceVertices">The vertices of the face in counterclockwise order.</param>
            /// <returns>The face created by this method.</returns>
            /// <exception cref="BadTopologyException">
            /// Thrown when fewer than three vertices are given or the vertices cannot form a valid face.
            /// </exception>
            /// <exception cref="ArgumentNullException">Thrown when a null vertex is given.</exception>
            private Face CreateFace(FaceTraits faceTraits, params Vertex[] faceVertices)
            {
                int n = faceVertices.Length;

                // Require at least 3 vertices
                if (n < 3)
                {
                    throw new BadTopologyException("Cannot create a polygon with fewer than three vertices.");
                }

                Edge e;
                Face f;
                HalfEdge[] faceHalfedges = new HalfEdge[n];
                bool[] isNewEdge = new bool[n], isUsedVertex = new bool[n];

                for (int i = 0; i < n; i++)
                {
                    int j = (i + 1) % n;

                    faceHalfedges[i] = faceVertices[i].FindHalfedgeTo(faceVertices[j]);

                }

                // Make sure input is (mostly) acceptable before making any changes to the mesh
                for (int i = 0; i < n; ++i)
                {
                    int j = (i + 1) % n;

                    if (faceVertices[i] == null)
                    {
                        throw new ArgumentNullException("Can't add a null vertex to a face.");
                    }
                    if (!faceVertices[i].OnBoundary)
                    {
                        
                        throw new BadTopologyException("Can't add an edge to a vertex on the interior of a mesh.");
                    }

                    // Find existing halfedges for this face
                    faceHalfedges[i] = faceVertices[i].FindHalfedgeTo(faceVertices[j]);
                    isNewEdge[i] = (faceHalfedges[i] == null);
                    isUsedVertex[i] = (faceVertices[i].HalfEdge != null);

                    if (!isNewEdge[i] && !faceHalfedges[i].OnBoundary)
                    {
                        
                        throw new BadTopologyException("Can't add more than two faces to an edge.");
                    }
                }

                // Create face
                f = new Face(faceTraits);
                mesh.AppendToFaceList(f);

                // Create new edges
                for (int i = 0; i < n; ++i)
                {
                    int j = (i + 1) % n;

                    if (isNewEdge[i])
                    {
                        // Create new edge
                        e = new Edge();
                        mesh.AppendToEdgeList(e);

                        // Create new halfedges
                        faceHalfedges[i] = new HalfEdge();
                        mesh.AppendToHalfedgeList(faceHalfedges[i]);

                        faceHalfedges[i].Opposite = new HalfEdge();
                        mesh.AppendToHalfedgeList(faceHalfedges[i].Opposite);

                        // Connect opposite halfedge to inner halfedge
                        faceHalfedges[i].Opposite.Opposite = faceHalfedges[i];

                        // Connect edge to halfedges
                        e.HalfEdge0 = faceHalfedges[i];

                        // Connect halfedges to edge
                        faceHalfedges[i].Edge = e;
                        faceHalfedges[i].Opposite.Edge = e;

                        // Connect halfedges to vertices
                        faceHalfedges[i].ToVertex = faceVertices[j];
                        faceHalfedges[i].Opposite.ToVertex = faceVertices[i];

                        // Connect vertex to outgoing halfedge if it doesn't have one yet
                        if (faceVertices[i].HalfEdge == null)
                        {
                            faceVertices[i].HalfEdge = faceHalfedges[i];
                        }
                    }

                    if (faceHalfedges[i].Face != null)
                    {
                        throw new BadTopologyException("An inner halfedge already has a face assigned to it.");
                    }

                    // Connect inner halfedge to face
                    faceHalfedges[i].Face = f;

                    Debug.Assert(faceHalfedges[i].FromVertex == faceVertices[i] && faceHalfedges[i].ToVertex == faceVertices[j]);
                }

                // Connect next/previous halfedges
                for (int i = 0; i < n; ++i)
                {
                    int j = (i + 1) % n;

                    // Outer halfedges
                    if (isNewEdge[i] && isNewEdge[j] && isUsedVertex[j])  // Both edges are new and vertex has faces connected already
                    {
                        HalfEdge closeHalfedge = null;

                        // Find the closing halfedge of the first available opening
                        foreach (HalfEdge h in faceVertices[j].HalfEdges)
                        {
                            if (h.Face == null)
                            {
                                closeHalfedge = h;
                                break;
                            }
                        }

                        Debug.Assert(closeHalfedge != null);

                        HalfEdge openHalfedge = closeHalfedge.Previous;

                        // Link new outer halfedges into this opening
                        faceHalfedges[i].Opposite.Previous = openHalfedge;
                        openHalfedge.Next = faceHalfedges[i].Opposite;
                        faceHalfedges[j].Opposite.Next = closeHalfedge;
                        closeHalfedge.Previous = faceHalfedges[j].Opposite;
                    }
                    else if (isNewEdge[i] && isNewEdge[j])  // Both edges are new
                    {
                        faceHalfedges[i].Opposite.Previous = faceHalfedges[j].Opposite;
                        faceHalfedges[j].Opposite.Next = faceHalfedges[i].Opposite;
                    }
                    else if (isNewEdge[i] && !isNewEdge[j])  // This is new, next is old
                    {
                        faceHalfedges[i].Opposite.Previous = faceHalfedges[j].Previous;
                        faceHalfedges[j].Previous.Next = faceHalfedges[i].Opposite;
                    }
                    else if (!isNewEdge[i] && isNewEdge[j])  // This is old, next is new
                    {
                        faceHalfedges[i].Next.Previous = faceHalfedges[j].Opposite;
                        faceHalfedges[j].Opposite.Next = faceHalfedges[i].Next;
                    }
                    else if (!isNewEdge[i] && !isNewEdge[j] && faceHalfedges[i].Next != faceHalfedges[j])  // Relink faces before adding new edges if they are in the way of a new face
                    {
                        HalfEdge closeHalfedge = faceHalfedges[i].Opposite;

                        // Find the closing halfedge of the opening opposite the opening halfedge i is on
                        do
                        {
                            closeHalfedge = closeHalfedge.Previous.Opposite;
                        } while (closeHalfedge.Face != null && closeHalfedge != faceHalfedges[j] && closeHalfedge != faceHalfedges[i].Opposite);

                        if (closeHalfedge == faceHalfedges[j] || closeHalfedge == faceHalfedges[i].Opposite)
                        {
                            throw new BadTopologyException("Unable to find an opening to relink an existing face.");
                        }

                        HalfEdge openHalfedge = closeHalfedge.Previous;

                        // Remove group of faces between two openings, close up gap to form one opening
                        openHalfedge.Next = faceHalfedges[i].Next;
                        faceHalfedges[i].Next.Previous = openHalfedge;

                        // Insert group of faces into target opening
                        faceHalfedges[j].Previous.Next = closeHalfedge;
                        closeHalfedge.Previous = faceHalfedges[j].Previous;
                    }

                    // Inner halfedges
                    faceHalfedges[i].Next = faceHalfedges[j];
                    faceHalfedges[j].Previous = faceHalfedges[i];
                }

                // Connect face to an inner halfedge
                f.HalfEdge = faceHalfedges[0];

                return f;
            }

            //public void RemoveFromList(Face face) 
            //{
            //    this.mesh.faces.Remove(face);
            //}

            //public void Remove(Face face)
            //{
            //    foreach (HalfEdge halfedge in face.Halfedges)
            //    {
            //        halfedge.Face = null; 
            //    }  
            //    this.mesh.faces.Remove(face);
            //}

            #endregion
        }
        #endregion

        
    }
}