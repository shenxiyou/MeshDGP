using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public partial class TriMeshModify 
    {
        public static void RemoveVertex(TriMesh.Vertex vertex)
        {
            TriMesh mesh = (TriMesh)vertex.Mesh;

            foreach (TriMesh.HalfEdge halfEdge in vertex.HalfEdges)
            {
                if (halfEdge.Next != null)
                {
                    halfEdge.Next.Face = null;
                    halfEdge.Opposite.Previous.Next = halfEdge.Next;
                    halfEdge.Next.Previous = halfEdge.Opposite.Previous;
                }
                if (halfEdge.ToVertex.HalfEdge == halfEdge.Opposite)
                {
                    halfEdge.ToVertex.HalfEdge = halfEdge.Next;
                }
                mesh.RemoveHalfedge(halfEdge.Opposite);
                mesh.RemoveHalfedge(halfEdge);
            }
            foreach (TriMesh.Face face in vertex.Faces)
            {
                mesh.RemoveFace(face);
            }
            foreach (TriMesh.Edge edge in vertex.Edges)
            {
                mesh.RemoveEdge(edge);
            }
            mesh.RemoveVertex(vertex);
        }


        public static void RemoveFace(TriMesh.Face face)
        {
            TriMesh mesh = (TriMesh)face.Mesh;

            foreach (TriMesh.HalfEdge halfedge in face.Halfedges)
            {
                halfedge.Face = null;
            }
            mesh.RemoveFace(face);
        }

        public static void RemoveEdge(TriMesh.Edge edge)
        {
            TriMesh mesh = (TriMesh)edge.Mesh;
            if (edge.HalfEdge0.Next == edge.HalfEdge1)
            {
                //TriMesh.HalfEdge hf = edge.Vertex0.HalfEdge;
                //hf.Previous.Next = hf.Opposite.Next;
                //hf.Next.Previous = hf.Previous;
                //edge.Vertex1.HalfEdge = hf.Previous;
                edge.HalfEdge0.Previous.Next = edge.HalfEdge1.Next;
                edge.HalfEdge1.Next.Previous = edge.HalfEdge0.Previous;
                edge.Vertex1.HalfEdge = edge.HalfEdge0.Previous;
                edge.Vertex0.HalfEdge = null;
            }
            else if (edge.HalfEdge1.Next == edge.HalfEdge0)
            {
                //TriMesh.HalfEdge hf = edge.Vertex1.HalfEdge;
                //hf.Previous.Next = hf.Opposite.Next;
                //hf.Next.Previous = hf.Previous;
                //edge.Vertex0.HalfEdge = hf.Previous;
                edge.HalfEdge1.Previous.Next = edge.HalfEdge0.Next;
                edge.HalfEdge0.Next.Previous = edge.HalfEdge1.Previous;
                edge.Vertex0.HalfEdge = edge.HalfEdge1.Previous;
                edge.Vertex1.HalfEdge = null;
            }
            else
            {
                edge.HalfEdge0.Next.Previous = edge.HalfEdge1.Previous;
                edge.HalfEdge0.Previous.Next = edge.HalfEdge1.Next;
                edge.HalfEdge1.Next.Previous = edge.HalfEdge0.Previous;
                edge.HalfEdge1.Previous.Next = edge.HalfEdge0.Next;
                edge.HalfEdge0.ToVertex.HalfEdge = edge.HalfEdge0.Next;
                edge.HalfEdge1.ToVertex.HalfEdge = edge.HalfEdge1.Next;
                edge.HalfEdge0.Next.Face = null;
                edge.HalfEdge0.Previous.Face = null;
                edge.HalfEdge1.Next.Face = null;
                edge.HalfEdge1.Previous.Face = null;
                if (edge.Face0 != null)
                {
                    mesh.RemoveFace(edge.Face0);
                }
                if (edge.Face1 != null)
                {
                    mesh.RemoveFace(edge.Face1);
                }
            }

            mesh.RemoveHalfedge(edge.HalfEdge1);
            mesh.RemoveHalfedge(edge.HalfEdge0);
            mesh.RemoveEdge(edge);
        }

        public static void RemoveOneRingOfEdge(TriMesh.Edge edge)
        {
            RemoveVertex(edge.Vertex0);
            RemoveVertex(edge.Vertex1);
        }

        public static void RemoveTwoRingOfVertex(TriMesh.Vertex vertex)
        {
            TriMesh mesh = (TriMesh)vertex.Mesh;
            List<TriMesh.Vertex> neighbors = new List<TriMesh.Vertex>();
            foreach (TriMesh.Vertex neighbor in vertex.Vertices)
            {
                neighbors.Add(neighbor);
            }
            RemoveVertex(vertex);
            for (int i = 0; i < neighbors.Count; i++)
            {
                RemoveVertex(neighbors[i]);
            }
        }

        #region 原来的

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
        private static TriMesh.Face CreateFace(TriMesh mesh, params TriMesh.Vertex[] faceVertices)
        {
            int n = faceVertices.Length; 
            // Require at least 3 vertices
            if (n < 3)
            {
                throw new BadTopologyException("Cannot create a polygon with fewer than three vertices.");
            }  
            TriMesh.Edge e;
            TriMesh.Face f;
            TriMesh.HalfEdge[] faceHalfedges = new TriMesh.HalfEdge[n];
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
            f = new TriMesh.Face(default(FaceTraits));
            mesh.AppendToFaceList(f); 
            // Create new edges
            for (int i = 0; i < n; ++i)
            {
                int j = (i + 1) % n; 
                if (isNewEdge[i])
                {
                    // Create new edge
                    e = new TriMesh.Edge();
                    mesh.AppendToEdgeList(e); 
                    // Create new halfedges
                    faceHalfedges[i] = new TriMesh.HalfEdge();
                    mesh.AppendToHalfedgeList(faceHalfedges[i]); 
                    faceHalfedges[i].Opposite = new TriMesh.HalfEdge();
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
            }

            // Connect next/previous halfedges
            for (int i = 0; i < n; ++i)
            {
                int j = (i + 1) % n;

                // Outer halfedges
                if (isNewEdge[i] && isNewEdge[j] && isUsedVertex[j])  // Both edges are new and vertex has faces connected already
                {
                    TriMesh.HalfEdge closeHalfedge = null; 
                    // Find the closing halfedge of the first available opening
                    foreach (TriMesh.HalfEdge h in faceVertices[j].HalfEdges)
                    {
                        if (h.Face == null)
                        {
                            closeHalfedge = h;
                            break;
                        }
                    } 
                    TriMesh.HalfEdge openHalfedge = closeHalfedge.Previous; 
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
                // Relink faces before adding new edges if they are in the way of a new face
                else if (!isNewEdge[i] && !isNewEdge[j] && faceHalfedges[i].Next != faceHalfedges[j]) 
                {
                    TriMesh.HalfEdge closeHalfedge = faceHalfedges[i].Opposite; 
                    // Find the closing halfedge of the opening opposite the opening halfedge i is on
                    do
                    {
                        closeHalfedge = closeHalfedge.Previous.Opposite;
                    } while (closeHalfedge.Face != null && closeHalfedge != faceHalfedges[j] 
                             && closeHalfedge != faceHalfedges[i].Opposite); 
                    if (closeHalfedge == faceHalfedges[j] || closeHalfedge == faceHalfedges[i].Opposite)
                    {
                        throw new BadTopologyException("Unable to find an opening to relink an existing face.");
                    } 
                    TriMesh.HalfEdge openHalfedge = closeHalfedge.Previous; 
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
        #endregion
    }
}
