using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class TriMeshModify 
    {
        /// <summary>
        /// Adds a face to the mesh with the specified face traits.
        /// </summary>
        /// <param name="faceVertices">The vertices of the face in counterclockwise order.</param>
        /// <returns>The face created by this method.</returns>
        /// <exception cref="BadTopologyException">
        /// Thrown when fewer than three vertices are given or the vertices cannot form a valid face.
        /// </exception>
        /// <exception cref="ArgumentNullException">Thrown when a null vertex is given.</exception>
        public TriMesh.Face CreateFace(params TriMesh.Vertex[] faceVertices)
        {
            int n = faceVertices.Length;
            // Require at least 3 vertices
            if (n < 3)
            {
                throw new BadTopologyException("Cannot create a polygon with fewer than three vertices.");
            }
            TriMesh mesh = (TriMesh)faceVertices[0].Mesh;
            TriMesh.HalfEdge[] faceHalfedges = new TriMesh.HalfEdge[n];
            bool[] isUsedVertex = new bool[n]; 

            // Make sure input is (mostly) acceptable before making any changes to the mesh
            for (int i = 0; i < n; ++i)
            {
                int j = (i + 1) % n;
                faceHalfedges[i] = this.Validate(faceVertices[i], faceVertices[j]);
                isUsedVertex[i] = (faceVertices[i].HalfEdge != null); 
            }
            // Create face
            TriMesh.Face f = new TriMesh.Face(default(FaceTraits));
            mesh.AppendToFaceList(f);
            // Create new edges
            for (int i = 0; i < n; ++i)
            {
                int j = (i + 1) % n;
                if (faceHalfedges[i] == null)
                {
                    TriMesh.Edge newEdge = this.CreateNewEdge(faceVertices[i], faceVertices[j]);
                    faceHalfedges[i] = newEdge.HalfEdge0;
                }
                faceHalfedges[i].Face = f;
            }

            // Connect next/previous halfedges
            for (int i = 0; i < n; ++i)
            {
                int j = (i + 1) % n;
                this.ConnectHalfedge(faceHalfedges[i], faceHalfedges[j], isUsedVertex[j]);
            }
            // Connect face to an inner halfedge
            f.HalfEdge = faceHalfedges[0];
            return f;
        }

        TriMesh.HalfEdge Validate(TriMesh.Vertex from, TriMesh.Vertex to)
        {
            TriMesh mesh = (TriMesh)from.Mesh;
            if (from == null)
            {
                throw new ArgumentNullException("Can't add a null vertex to a face.");
            }
            if (!from.OnBoundary)
            {
                throw new BadTopologyException("Can't add an edge to a vertex on the interior of a mesh.");
            }
            // Find existing halfedges for this face
            TriMesh.HalfEdge hf = from.FindHalfedgeTo(to);
            if (hf != null && !hf.OnBoundary)
            {
                throw new BadTopologyException("Can't add more than two faces to an edge.");
            }
            return hf;
        }

        TriMesh.Edge CreateNewEdge(TriMesh.Vertex from, TriMesh.Vertex to)
        {
            TriMesh mesh = (TriMesh)from.Mesh;
            // Create new edge
            TriMesh.Edge edge = new TriMesh.Edge();
            mesh.AppendToEdgeList(edge);
            // Create new halfedges
            TriMesh.HalfEdge hf0 = new TriMesh.HalfEdge();
            mesh.AppendToHalfedgeList(hf0);
            hf0.Opposite = new TriMesh.HalfEdge();
            mesh.AppendToHalfedgeList(hf0.Opposite);
            // Connect opposite halfedge to inner halfedge
            hf0.Opposite.Opposite = hf0;
            // Connect edge to halfedges
            edge.HalfEdge0 = hf0;
            // Connect halfedges to edge
            hf0.Edge = edge;
            hf0.Opposite.Edge = edge;
            // Connect halfedges to vertices
            hf0.ToVertex = to;
            hf0.Opposite.ToVertex = from;
            // Connect vertex to outgoing halfedge if it doesn't have one yet
            if (from.HalfEdge == null)
            {
                from.HalfEdge = hf0;
            }
            return edge;
        }

        void ConnectHalfedge(TriMesh.HalfEdge cur, TriMesh.HalfEdge next, bool vertexIsUsed)
        {
            TriMesh mesh = (TriMesh)cur.Mesh;
            bool curIsNew = cur.Next == null;
            bool nextIsNew = next.Previous == null;

            // Outer halfedges
            if (curIsNew && nextIsNew)
            {
                this.OuterHalfedgeBothNew(cur, next, vertexIsUsed);
            }
            else if (curIsNew && !nextIsNew)  // This is new, next is old
            {
                cur.Opposite.Previous = next.Previous;
                next.Previous.Next = cur.Opposite;
            }
            else if (!curIsNew && nextIsNew)  // This is old, next is new
            {
                cur.Next.Previous = next.Opposite;
                next.Opposite.Next = cur.Next;
            }
            else
            {
                this.OuterHalfedgeBothOld(cur, next);
            }
            // Inner halfedges
            cur.Next = next;
            next.Previous = cur;
        }

        void OuterHalfedgeBothNew(TriMesh.HalfEdge cur, TriMesh.HalfEdge next, bool vertexIsUsed)
        {
            if (vertexIsUsed)  // Both edges are new and vertex has faces connected already
            {
                TriMesh.Vertex vertex = cur.ToVertex;
                TriMesh.HalfEdge closeHalfedge = null;
                // Find the closing halfedge of the first available opening
                foreach (TriMesh.HalfEdge h in vertex.HalfEdges)
                {
                    if (h.Face == null)
                    {
                        closeHalfedge = h;
                        break;
                    }
                }
                TriMesh.HalfEdge openHalfedge = closeHalfedge.Previous;
                // Link new outer halfedges into this opening
                cur.Opposite.Previous = openHalfedge;
                openHalfedge.Next = cur.Opposite;
                next.Opposite.Next = closeHalfedge;
                closeHalfedge.Previous = next.Opposite;
            }
            else
            {
                cur.Opposite.Previous = next.Opposite;
                next.Opposite.Next = cur.Opposite;
            }
        }

        void OuterHalfedgeBothOld(TriMesh.HalfEdge cur, TriMesh.HalfEdge next)
        {
            // Relink faces before adding new edges if they are in the way of a new face
            if (cur.Next != next)
            {
                TriMesh.HalfEdge closeHalfedge = cur.Opposite;
                // Find the closing halfedge of the opening opposite the opening halfedge i is on
                do
                {
                    closeHalfedge = closeHalfedge.Previous.Opposite;
                } while (closeHalfedge.Face != null && closeHalfedge != next
                         && closeHalfedge != cur.Opposite);
                if (closeHalfedge == next || closeHalfedge == cur.Opposite)
                {
                    throw new BadTopologyException("Unable to find an opening to relink an existing face.");
                }
                TriMesh.HalfEdge openHalfedge = closeHalfedge.Previous;
                // Remove group of faces between two openings, close up gap to form one opening
                openHalfedge.Next = cur.Next;
                cur.Next.Previous = openHalfedge;
                // Insert group of faces into target opening
                next.Previous.Next = closeHalfedge;
                closeHalfedge.Previous = next.Previous;
            }
        }
    }
}
