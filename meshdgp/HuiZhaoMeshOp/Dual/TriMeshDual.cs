using System;
using System.Collections.Generic;
using System.Text;
using GraphicResearchHuiZhao;

namespace Temp
{
    public class TriMeshDual
    {
        public TriMesh mesh = null;

        public TriMeshDual(TriMesh mesh)
        {
            this.mesh = mesh;
        }

        public PolygonMesh TriMeshToDualB()
        {
            TriMeshModify.RepaireAllHole(mesh);
            PolygonMesh dualMesh = new PolygonMesh();
            ComputeGeometryB(dualMesh);
            MakeTheTopologyB(dualMesh);
            return dualMesh;
        }
        public void ComputeGeometryB(PolygonMesh dualMesh)
        {
            TriMesh.Vertex[] edgeVertices = new TriMesh.Vertex[mesh.Faces.Count * 3];
            foreach (TriMesh.Edge e in mesh.Edges)
            {    
                Vector3D midVertex0 = TriMeshUtil.GetMidPoint(e);  
                dualMesh.Vertices.Add(new VertexTraits(midVertex0.x, midVertex0.y, midVertex0.z));
            }
            foreach (TriMesh.Face f in mesh.Faces)
            {
                 
                Vector3D baryCenter = TriMeshUtil.GetMidPoint(f);
                dualMesh.Vertices.Add(new VertexTraits(baryCenter.x, baryCenter.y, baryCenter.z));
            }
        }
        
        public void MakeTheTopologyB(PolygonMesh dualMesh)
        {
            List<TriMesh.Vertex> boundaryVertices = new List<TriMesh.Vertex>();

            boundaryVertices = TriMeshUtil.RetrieveAllBoundaryVertex(mesh);
            foreach (TriMesh.Face face in mesh.Faces)
            {
                int index = face.Index;
                int baryVertexIndex = mesh.Edges.Count;
                int edgeVertexIndex;
                int faceIndex = index;
                TriMesh.Edge boundaryEdge = new TriMesh.Edge();
                TriMesh.HalfEdge tempHalfedge = new TriMesh.HalfEdge();
                TriMesh.Vertex vertex0 = mesh.Faces[index].GetVertex(0);
                TriMesh.Vertex vertex1 = mesh.Faces[index].GetVertex(1);
                TriMesh.Vertex vertex2 = mesh.Faces[index].GetVertex(2);
                TriMesh.HalfEdge halfedge0 = vertex0.FindHalfedgeTo(vertex1);
                TriMesh.HalfEdge halfedge1 = vertex1.FindHalfedgeTo(vertex2);
                TriMesh.HalfEdge halfedge2 = vertex2.FindHalfedgeTo(vertex0);
                TriMesh.HalfEdge boundaryHalfedge = new TriMesh.HalfEdge();
                tempHalfedge = halfedge0;
                
                edgeVertexIndex = halfedge0.Edge.Index;
                TriMesh.Vertex firstVertex = dualMesh.Vertices[baryVertexIndex + faceIndex];
                TriMesh.Vertex secondVertex = dualMesh.Vertices[edgeVertexIndex];
                if (firstVertex.FindHalfedgeTo(secondVertex) == null)
                {
                    List<TriMesh.Vertex> ringVertices = new List<TriMesh.Vertex>();
                    do
                    {
                        ringVertices.Add(dualMesh.Vertices[baryVertexIndex + faceIndex]);
                        ringVertices.Add(dualMesh.Vertices[edgeVertexIndex]);
                        if (tempHalfedge.Opposite.Next.Face != null)
                        {
                            tempHalfedge = tempHalfedge.Opposite.Next;
                            faceIndex = tempHalfedge.Face.Index;
                            edgeVertexIndex = tempHalfedge.Edge.Index;
                        }
                        else
                        {
                            tempHalfedge = tempHalfedge.Opposite.Next;
                            edgeVertexIndex = tempHalfedge.Edge.Index;
                            faceIndex = tempHalfedge.Opposite.Next.Face.Index;
                        }
                    } while (tempHalfedge != halfedge0);
                    TriMesh.Vertex[] newFaceVertices = new TriMesh.Vertex[ringVertices.Count];
                    for (int i = 0; i < ringVertices.Count; i++)
                    {
                        newFaceVertices[i] = ringVertices[i];
                    }
                    dualMesh.Faces.Add(newFaceVertices);
                }
                tempHalfedge = halfedge1;
                edgeVertexIndex = halfedge1.Edge.Index;
                firstVertex = dualMesh.Vertices[baryVertexIndex + faceIndex];
                secondVertex = dualMesh.Vertices[edgeVertexIndex];
                if (firstVertex.FindHalfedgeTo(secondVertex) == null)
                {
                    List<TriMesh.Vertex> ringVertices = new List<TriMesh.Vertex>();
                    do
                    {
                        ringVertices.Add(dualMesh.Vertices[baryVertexIndex + faceIndex]);
                        ringVertices.Add(dualMesh.Vertices[edgeVertexIndex]);
                        if (tempHalfedge.Opposite.Next != null)
                        {
                            tempHalfedge = tempHalfedge.Opposite.Next;
                            faceIndex = tempHalfedge.Face.Index;
                            edgeVertexIndex = tempHalfedge.Edge.Index;
                        }
                        else
                        {
                            continue;
                        }
                    } while (tempHalfedge != halfedge1);
                    TriMesh.Vertex[] newFaceVertices = new TriMesh.Vertex[ringVertices.Count];
                    for (int i = 0; i < ringVertices.Count; i++)
                    {
                        newFaceVertices[i] = ringVertices[i];

                    }
                    dualMesh.Faces.Add(newFaceVertices);
                }
                tempHalfedge = halfedge2;
                edgeVertexIndex = halfedge2.Edge.Index;
                firstVertex = dualMesh.Vertices[baryVertexIndex + faceIndex];
                secondVertex = dualMesh.Vertices[edgeVertexIndex];
                if (firstVertex.FindHalfedgeTo(secondVertex) == null)
                {
                    List<TriMesh.Vertex> ringVertices = new List<TriMesh.Vertex>();
                    do
                    {
                        ringVertices.Add(dualMesh.Vertices[baryVertexIndex + faceIndex]);
                        ringVertices.Add(dualMesh.Vertices[edgeVertexIndex]);
                        if (tempHalfedge.Opposite.Next != null)
                        {
                            tempHalfedge = tempHalfedge.Opposite.Next;
                            faceIndex = tempHalfedge.Face.Index;
                            edgeVertexIndex = tempHalfedge.Edge.Index;
                        }
                        else
                        {
                            continue;
                        }
                    } while (tempHalfedge != halfedge2);
                    TriMesh.Vertex[] newFaceVertices = new TriMesh.Vertex[ringVertices.Count];
                    for (int i = 0; i < ringVertices.Count; i++)
                    {
                        newFaceVertices[i] = ringVertices[i];

                    }
                    dualMesh.Faces.Add(newFaceVertices);
                }
            }
        }




        public PolygonMesh TriMeshToDualC()
        {

            TriMeshModify.RepaireAllHole(mesh);
            PolygonMesh dualMesh = new PolygonMesh();
            ComputeGeometryC(dualMesh);
            MakeTheTopologyC(dualMesh);
            return dualMesh;

        }
        public void ComputeGeometryC(PolygonMesh dualMesh)
        {
            //get the mid-Vertex of each edges
            TriMesh.Vertex[] edgeVertices = new TriMesh.Vertex[mesh.Faces.Count * 3];
            foreach (TriMesh.Edge e in mesh.Edges)
            {
                Vector3D midVertex0 = TriMeshUtil.GetMidPoint(e);  
                dualMesh.Vertices.Add(new VertexTraits(midVertex0.x,
                                                       midVertex0.y,
                                                       midVertex0.z));
            }
            //get circumcentre of each faces
            foreach (TriMesh.Face f in mesh.Faces)
            {
                TriMesh.Vertex vertex0 = f.GetVertex(0);
                TriMesh.Vertex vertex1 = f.GetVertex(1);
                TriMesh.Vertex vertex2 = f.GetVertex(2);
                Triangle triangle = new Triangle(vertex0.Traits.Position, vertex1.Traits.Position, vertex2.Traits.Position);
                Vector3D circumCenter = triangle.ComputeCircumCenter();
                dualMesh.Vertices.Add(new VertexTraits(circumCenter.x,
                                                       circumCenter.y,
                                                       circumCenter.z));

            }
        }
        
        
        public void MakeTheTopologyC(PolygonMesh dualMesh)
        {
            foreach (TriMesh.Face face in mesh.Faces)
            {
                int index = face.Index;
                int circumVertexIndex = mesh.Edges.Count;
                int edgeVertexIndex;
                int faceIndex = index;
                TriMesh.HalfEdge tempHalfedge = new TriMesh.HalfEdge();
                TriMesh.Vertex vertex0 = mesh.Faces[index].GetVertex(0);
                TriMesh.Vertex vertex1 = mesh.Faces[index].GetVertex(1);
                TriMesh.Vertex vertex2 = mesh.Faces[index].GetVertex(2);
                TriMesh.HalfEdge halfedge0 = vertex0.FindHalfedgeTo(vertex1);
                TriMesh.HalfEdge halfedge1 = vertex1.FindHalfedgeTo(vertex2);
                TriMesh.HalfEdge halfedge2 = vertex2.FindHalfedgeTo(vertex0);
                tempHalfedge = halfedge0;
                edgeVertexIndex = halfedge0.Edge.Index;
                TriMesh.Vertex firstVertex = dualMesh.Vertices[circumVertexIndex + faceIndex];
                TriMesh.Vertex secondVertex = dualMesh.Vertices[edgeVertexIndex];
                if (firstVertex.FindHalfedgeTo(secondVertex) == null)
                {
                    List<TriMesh.Vertex> ringVertices = new List<TriMesh.Vertex>();
                    do
                    {
                        ringVertices.Add(dualMesh.Vertices[circumVertexIndex + faceIndex]);
                        ringVertices.Add(dualMesh.Vertices[edgeVertexIndex]);
                        if (tempHalfedge.Opposite.Next != null)
                        {
                            tempHalfedge = tempHalfedge.Opposite.Next;
                            faceIndex = tempHalfedge.Face.Index;
                            edgeVertexIndex = tempHalfedge.Edge.Index;
                        }
                        else
                        {
                            continue;
                        }
                    } while (tempHalfedge != halfedge0);
                    TriMesh.Vertex[] newFaceVertices = new TriMesh.Vertex[ringVertices.Count];
                    for (int i = 0; i < ringVertices.Count; i++)
                    {
                        newFaceVertices[i] = ringVertices[i];
                    }
                    dualMesh.Faces.Add(newFaceVertices);
                }
                tempHalfedge = halfedge1;
                edgeVertexIndex = halfedge1.Edge.Index;
                firstVertex = dualMesh.Vertices[circumVertexIndex + faceIndex];
                secondVertex = dualMesh.Vertices[edgeVertexIndex];
                if (firstVertex.FindHalfedgeTo(secondVertex) == null)
                {
                    List<TriMesh.Vertex> ringVertices = new List<TriMesh.Vertex>();
                    do
                    {
                        ringVertices.Add(dualMesh.Vertices[circumVertexIndex + faceIndex]);
                        ringVertices.Add(dualMesh.Vertices[edgeVertexIndex]);
                        if (tempHalfedge.Opposite.Next != null)
                        {
                            tempHalfedge = tempHalfedge.Opposite.Next;
                            faceIndex = tempHalfedge.Face.Index;
                            edgeVertexIndex = tempHalfedge.Edge.Index;
                        }
                        else
                        {
                            continue;
                        }
                    } while (tempHalfedge != halfedge1);
                    TriMesh.Vertex[] newFaceVertices = new TriMesh.Vertex[ringVertices.Count];
                    for (int i = 0; i < ringVertices.Count; i++)
                    {
                        newFaceVertices[i] = ringVertices[i];
                    }
                    dualMesh.Faces.Add(newFaceVertices);
                }
                tempHalfedge = halfedge2;
                edgeVertexIndex = halfedge2.Edge.Index;
                firstVertex = dualMesh.Vertices[circumVertexIndex + faceIndex];
                secondVertex = dualMesh.Vertices[edgeVertexIndex];
                if (firstVertex.FindHalfedgeTo(secondVertex) == null)
                {
                    List<TriMesh.Vertex> ringVertices = new List<TriMesh.Vertex>();
                    do
                    {
                        ringVertices.Add(dualMesh.Vertices[circumVertexIndex + faceIndex]);
                        ringVertices.Add(dualMesh.Vertices[edgeVertexIndex]);
                        if (tempHalfedge.Opposite.Next != null)
                        {
                            tempHalfedge = tempHalfedge.Opposite.Next;
                            faceIndex = tempHalfedge.Face.Index;
                            edgeVertexIndex = tempHalfedge.Edge.Index;
                        }
                        else
                        {
                            continue;
                        }
                    } while (tempHalfedge != halfedge2);
                    TriMesh.Vertex[] newFaceVertices = new TriMesh.Vertex[ringVertices.Count];
                    for (int i = 0; i < ringVertices.Count; i++)
                    {
                        newFaceVertices[i] = ringVertices[i];
                    }
                    dualMesh.Faces.Add(newFaceVertices);
                }
            }
        }
    }
}
