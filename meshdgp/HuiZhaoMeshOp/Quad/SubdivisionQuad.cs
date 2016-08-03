using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class SubdivisionQuad
    {
        public QuadMesh Mesh = null;
        public SubdivisionQuad(QuadMesh Mesh)
        {
            this.Mesh = Mesh;
        }

        public QuadMesh CatMullClark()
        {
            QuadMesh subdivisionedMesh = ChangeTopologyCatMullClark(Mesh);

            ChangeGeometryCatMullClark(Mesh, subdivisionedMesh);

            return subdivisionedMesh;
        }


        public QuadMesh DooSabin()
        {
            QuadMesh subdivisionedMesh = ChangeTopologyDooSabin(Mesh);

            ChangeGeometryDooSabin(Mesh, subdivisionedMesh);

            return subdivisionedMesh;
        }



        private QuadMesh ChangeTopologyCatMullClark(QuadMesh sourceMesh)
        {
            QuadMesh newMesh = new QuadMesh();

            //获得初始的点
            for (int i = 0; i < sourceMesh.Vertices.Count; i++)
            {
                newMesh.Vertices.Add(new VertexTraits(sourceMesh.Vertices[i].Traits.Position.x, sourceMesh.Vertices[i].Traits.Position.y, sourceMesh.Vertices[i].Traits.Position.z));
            }

            //获得E点
            Vector3D midPosition;
            for (int i = 0; i < sourceMesh.Edges.Count; i++)
            {
                midPosition = (sourceMesh.Edges[i].Vertex0.Traits.Position +
                             sourceMesh.Edges[i].Vertex1.Traits.Position) / 2;
                newMesh.Vertices.Add(new VertexTraits(midPosition.x, midPosition.y, midPosition.z));
            }

            //获得F点
            foreach (QuadMesh.Face face in sourceMesh.Faces)
            {
                midPosition = (face.GetVertex(0).Traits.Position + face.GetVertex(1).Traits.Position + face.GetVertex(2).Traits.Position + face.GetVertex(3).Traits.Position) / 4;
                newMesh.Vertices.Add(new VertexTraits(midPosition.x, midPosition.y, midPosition.z));
            }


            //构造面
            foreach (QuadMesh.Face face in sourceMesh.Faces)
            {
                QuadMesh.Vertex vertex0 = newMesh.Vertices[face.GetVertex(3).Index];
                QuadMesh.Vertex vertex1 = newMesh.Vertices[face.GetVertex(0).Index];
                QuadMesh.Vertex vertex2 = newMesh.Vertices[face.GetVertex(1).Index];
                QuadMesh.Vertex vertex3 = newMesh.Vertices[face.GetVertex(2).Index];

                QuadMesh.Edge edge0 = face.GetVertex(0).FindEdgeTo(face.GetVertex(1));
                QuadMesh.Edge edge1 = face.GetVertex(1).FindEdgeTo(face.GetVertex(2));
                QuadMesh.Edge edge2 = face.GetVertex(2).FindEdgeTo(face.GetVertex(3));
                QuadMesh.Edge edge3 = face.GetVertex(3).FindEdgeTo(face.GetVertex(0));

                QuadMesh.Vertex midVertex0 = newMesh.Vertices[sourceMesh.Vertices.Count + edge0.Index];
                QuadMesh.Vertex midVertex1 = newMesh.Vertices[sourceMesh.Vertices.Count + edge1.Index];
                QuadMesh.Vertex midVertex2 = newMesh.Vertices[sourceMesh.Vertices.Count + edge2.Index];
                QuadMesh.Vertex midVertex3 = newMesh.Vertices[sourceMesh.Vertices.Count + edge3.Index];

                QuadMesh.Vertex FVertex = newMesh.Vertices[sourceMesh.Vertices.Count + sourceMesh.Edges.Count + face.Index];

                QuadMesh.Vertex[] newFace = new QuadMesh.Vertex[4];
                newFace[0] = vertex0;
                newFace[1] = midVertex3;
                newFace[2] = FVertex;
                newFace[3] = midVertex2;
                newMesh.Faces.AddQuads(newFace);
                newFace[0] = midVertex2;
                newFace[1] = FVertex;
                newFace[2] = midVertex1;
                newFace[3] = vertex3;
                newMesh.Faces.AddQuads(newFace);
                newFace[0] = midVertex3;
                newFace[1] = vertex1;
                newFace[2] = midVertex0;
                newFace[3] = FVertex;
                newMesh.Faces.AddQuads(newFace);
                newFace[0] = FVertex;
                newFace[1] = midVertex0;
                newFace[2] = vertex2;
                newFace[3] = midVertex1;
                newMesh.Faces.AddQuads(newFace);

            }

            return newMesh;

        }

        private void ChangeGeometryCatMullClark(QuadMesh sourceMesh, QuadMesh targetMesh)
        {
            for (int i = 0; i < sourceMesh.Vertices.Count; i++)
            {
                Vector3D position = new Vector3D(0, 0, 0);
                if (sourceMesh.Vertices[i].OnBoundary)
                {
                    int n = 0;
                    foreach (QuadMesh.Vertex neighbor in sourceMesh.Vertices[i].Vertices)
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

                    foreach (QuadMesh.Vertex neighbor in sourceMesh.Vertices[i].Vertices)
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
                QuadMesh.Vertex vertex0 = sourceMesh.Edges[i].Vertex0;
                QuadMesh.Vertex vertex1 = sourceMesh.Edges[i].Vertex1;
                if (sourceMesh.Edges[i].OnBoundary)
                {
                    position = (vertex0.Traits.Position + vertex1.Traits.Position) / 2;
                }
                else
                {

                    QuadMesh.Vertex vertex2 = sourceMesh.Edges[i].HalfEdge0.Next.ToVertex;
                    QuadMesh.Vertex vertex3 = sourceMesh.Edges[i].HalfEdge1.Next.ToVertex;
                    position = (vertex0.Traits.Position + vertex1.Traits.Position) * 3 / 8 +
                               (vertex2.Traits.Position + vertex3.Traits.Position) * 1 / 8;

                }
                targetMesh.Vertices[sourceMesh.Vertices.Count + i].Traits.Position = position;
            }

        }

        private double LoopComputeBeta(int n)//计算权值beta
        {
            double beta, middle;
            middle = 0.25f * (Math.Cos(Math.PI * 2 / n)) + 0.375;
            beta = (0.625f - (middle * middle)) / n;
            return beta;
        }
        private QuadMesh ChangeTopologyDooSabin(QuadMesh sourceMesh)
        {
            QuadMesh newMesh = new QuadMesh();




           

            return newMesh;

        }

        private void ChangeGeometryDooSabin(QuadMesh sourceMesh, QuadMesh targetMesh)
        {


        }






    }
}
