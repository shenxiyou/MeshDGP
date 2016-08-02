using System;
using System.Collections.Generic;
using System.Text; 

namespace GraphicResearchHuiZhao
{
    public partial class TriMeshSubdivision
    {

        #region LoopSubdivision

        public TriMesh  SubdivisionLoop()
        {
            TriMesh loopedMesh = ChangeTopologyLoop(Mesh);
            ChangeGeometryLoop(Mesh,loopedMesh);
            return loopedMesh;
        }

        private TriMesh ChangeTopologyLoop(TriMesh sourceMesh)
        {
            TriMesh newMesh = new TriMesh();
            newMesh.Clear();
            vMap = new HalfEdgeMesh.Vertex[sourceMesh.Vertices.Count];
            eMap = new HalfEdgeMesh.Vertex[sourceMesh.Edges.Count];

            foreach (var v in sourceMesh.Vertices)
            {
                vMap[v.Index] = newMesh.Vertices.Add(new VertexTraits(v.Traits.Position));
            }

            foreach (var edge in sourceMesh.Edges)
            {
                eMap[edge.Index] = newMesh.Vertices.Add(
                    new VertexTraits(TriMeshUtil.GetMidPoint(edge)));
            }

            foreach(TriMesh.Face face in sourceMesh.Faces)
            {
                foreach (var hf in face.Halfedges)
                {
                    newMesh.Faces.AddTriangles(eMap[hf.Edge.Index], 
                        vMap[hf.ToVertex.Index], eMap[hf.Next.Edge.Index]);
                }
                newMesh.Faces.AddTriangles(
                    eMap[face.HalfEdge.Previous.Edge.Index],
                    eMap[face.HalfEdge.Edge.Index],
                    eMap[face.HalfEdge.Next.Edge.Index]);
            }
            return newMesh;
        }

        private void ChangeGeometryLoop(TriMesh sourceMesh,TriMesh targetMesh)
        { 
            for (int i = 0; i < sourceMesh.Vertices.Count; i++)
            {
                Vector3D position = new Vector3D(0, 0, 0);
                if (sourceMesh.Vertices[i].OnBoundary)
                {
                    int n = 0;
                    foreach(TriMesh.Vertex neighbor in sourceMesh.Vertices[i].Vertices)
                    {
                        if (neighbor.OnBoundary)
                        {
                            position += neighbor.Traits.Position;
                            n++;
                        }
                    }
                    targetMesh.Vertices[i].Traits.Position = 
                        sourceMesh.Vertices[i].Traits.Position * 3 / 4
                        + position * 1 / (4 * n);

                }
                else
                {

                    foreach (TriMesh.Vertex neighbor in sourceMesh.Vertices[i].Vertices)
                    {
                        position += neighbor.Traits.Position;
                    }
                    int n = Mesh.Vertices[i].VertexCount;
                    double beta = LoopComputeBeta(n);
                    targetMesh.Vertices[i].Traits.Position = 
                                      (1 - n * beta) * 
                                      Mesh.Vertices[i].Traits.Position + 
                                      beta * position;
                }
            }
            //update new added vertex position
            for (int i = 0; i < sourceMesh.Edges.Count; i++)
            {
                Vector3D position = new Vector3D(0, 0, 0);
                TriMesh.Vertex vertex0 = sourceMesh.Edges[i].Vertex0;
                TriMesh.Vertex vertex1 = sourceMesh.Edges[i].Vertex1;
                if (sourceMesh.Edges[i].OnBoundary)
                {
                    position = (vertex0.Traits.Position + vertex1.Traits.Position) / 2;
                }
                else
                {

                    TriMesh.Vertex vertex2 = sourceMesh.Edges[i].HalfEdge0.Next.ToVertex;
                    TriMesh.Vertex vertex3 = sourceMesh.Edges[i].HalfEdge1.Next.ToVertex;
                    position = (vertex0.Traits.Position + vertex1.Traits.Position) * 3 / 8 +
                               (vertex2.Traits.Position + vertex3.Traits.Position) * 1 / 8;

                }
                targetMesh.Vertices[sourceMesh.Vertices.Count + i].Traits.Position = position;
            }
        }
 
        private double LoopComputeBeta(int n)//计算权值beta
        {
            double beta, middle;
            middle = 0.25f * (Math.Cos(6.2831853 / n)) + 0.375;
            beta = (0.625f - (middle * middle)) / n;
            return beta;

            //double beta=0;
            //if (n > 3)
            //{
            //    beta = 3 / (8 * n);
            //}
            //else
            //{
            //    beta = 3 / 16;
            //}
            //return beta;
        }
      

        #endregion
    }
}
