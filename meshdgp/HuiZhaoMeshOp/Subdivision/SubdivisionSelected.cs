using System;
using System.Collections.Generic;
using System.Text; 

namespace GraphicResearchHuiZhao
{
    public partial class TriMeshSubdivision
    {
        public TriMesh SubdivisionSelectedLoop()
        {
            TriMesh loopedMesh = ChangeTopologyLoopSelected(Mesh);
            ChangeGeometryLoopSelected(Mesh, loopedMesh);
            TriMeshUtil.FixIndex(loopedMesh);
            return loopedMesh;
        }

        private TriMesh ChangeTopologyLoopSelected(TriMesh sourceMesh)
        {
            TriMesh newMesh = new TriMesh();
            newMesh.Clear();

            vMap = new HalfEdgeMesh.Vertex[sourceMesh.Vertices.Count];
            eMap = new HalfEdgeMesh.Vertex[Mesh.Edges.Count];

            foreach (var v in sourceMesh.Vertices)
            {
                vMap[v.Index] = newMesh.Vertices.Add(new VertexTraits(v.Traits.Position));
            }

            foreach (var edge in sourceMesh.Edges)
            {
                if (edge.Vertex0.Traits.SelectedFlag != 0 && edge.Vertex1.Traits.SelectedFlag != 0)
                {
                    eMap[edge.Index] = newMesh.Vertices.Add(new VertexTraits(TriMeshUtil.GetMidPoint(edge)));
                }
            }

            foreach (TriMesh.Face face in sourceMesh.Faces)
            {
                List<TriMesh.HalfEdge> list = new List<HalfEdgeMesh.HalfEdge>();
                foreach (var hf in face.Halfedges)
                {
                    if (hf.ToVertex.Traits.SelectedFlag != 0 && hf.FromVertex.Traits.SelectedFlag != 0)
                    {
                        list.Add(hf);
                    }
                }

                switch (list.Count)
                {
                    case 0:
                        newMesh.Faces.AddTriangles(
                            vMap[face.GetVertex(0).Index],
                            vMap[face.GetVertex(1).Index],
                            vMap[face.GetVertex(2).Index]);
                        break;
                    case 1:
                        TriMesh.HalfEdge h = list[0];
                        newMesh.Faces.AddTriangles(
                            eMap[h.Edge.Index],
                            vMap[h.ToVertex.Index],
                            vMap[h.Next.ToVertex.Index]);
                        newMesh.Faces.AddTriangles(
                            eMap[h.Edge.Index],
                            vMap[h.Next.ToVertex.Index],
                            vMap[h.FromVertex.Index]);
                        break;
                    case 3:
                        foreach (var hf in face.Halfedges)
                        {
                            newMesh.Faces.AddTriangles(eMap[hf.Edge.Index], vMap[hf.ToVertex.Index], eMap[hf.Next.Edge.Index]);
                        }
                        newMesh.Faces.AddTriangles(
                            eMap[face.HalfEdge.Previous.Edge.Index],
                            eMap[face.HalfEdge.Edge.Index],
                            eMap[face.HalfEdge.Next.Edge.Index]);
                        break;
                    default:
                        break;
                }
            }
            return newMesh;
        }

        private void ChangeGeometryLoopSelected(TriMesh sourceMesh, TriMesh targetMesh)
        {
             
                for (int i = 0; i < sourceMesh.Vertices.Count; i++)
                {
                    if (sourceMesh.Vertices[i].Traits.SelectedFlag == 0)
                    {
                        continue;
                    }

                    Vector3D position = new Vector3D(0, 0, 0);

                    if (sourceMesh.Vertices[i].OnBoundary)
                    {
                        int n = 0;
                        foreach (TriMesh.Vertex neighbor in sourceMesh.Vertices[i].Vertices)
                        {
                            if (neighbor.OnBoundary)
                            {
                                position += neighbor.Traits.Position;
                                n++;
                            }
                        }
                        targetMesh.Vertices[i].Traits.Position = sourceMesh.Vertices[i].Traits.Position * 3 / 4 + position * 1 / (4 * n);

                    }
                    else
                    {

                        foreach (TriMesh.Vertex neighbor in sourceMesh.Vertices[i].Vertices)
                        {
                            position += neighbor.Traits.Position;
                        }

                        int n = Mesh.Vertices[i].VertexCount;
                        double beta = LoopComputeBeta(n);

                        targetMesh.Vertices[i].Traits.Position = (1 - n * beta) * Mesh.Vertices[i].Traits.Position + beta * position;
                        targetMesh.Vertices[i].Traits.SelectedFlag = 1;
                    }
                }

                //update new added vertex position
                int j = 0;
                for (int i = 0; i < sourceMesh.Edges.Count; i++)
                {
                    TriMesh.Vertex vertex0 = sourceMesh.Edges[i].Vertex0;
                    TriMesh.Vertex vertex1 = sourceMesh.Edges[i].Vertex1;

                    if (vertex0.Traits.SelectedFlag == 0 ||
                        vertex1.Traits.SelectedFlag == 0
                        )
                    {
                        continue;
                    }

                    Vector3D position = new Vector3D(0, 0, 0);

                    if (sourceMesh.Edges[i].OnBoundary)
                    {
                        position = (vertex0.Traits.Position + vertex1.Traits.Position) / 2;
                    }
                    else
                    {

                        TriMesh.Vertex vertex2 = sourceMesh.Edges[i].HalfEdge0.Next.ToVertex;
                        TriMesh.Vertex vertex3 = sourceMesh.Edges[i].HalfEdge1.Next.ToVertex;

                        position = (vertex0.Traits.Position + vertex1.Traits.Position) * 3 / 8 + (vertex2.Traits.Position + vertex3.Traits.Position) * 1 / 8;

                    }

                    eMap[sourceMesh.Edges[i].Index].Traits.Position = position;
                    eMap[sourceMesh.Edges[i].Index].Traits.SelectedFlag = 1;
                    j++;
                }
            }




        
    }
}
