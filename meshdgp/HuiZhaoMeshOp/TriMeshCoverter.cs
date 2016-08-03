

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
 

namespace GraphicResearchHuiZhao
{
     
    public class TriMeshConverter 
    {

        public static NonManifoldMesh ConvertToMesh(TriMesh triMesh)
        {
            NonManifoldMesh mesh = new NonManifoldMesh();

            mesh.VertexCount = triMesh.Vertices.Count;
            mesh.FaceCount = triMesh.Faces.Count;
            mesh.VertexPos = new double[mesh.VertexCount * 3];
            mesh.FaceIndex = new int[mesh.FaceCount * 3];

            int n = mesh.VertexCount;

            for (int i = 0; i < n; i++)
            {
                mesh.VertexPos[i * 3] = (double)triMesh.Vertices[i].Traits.Position.x;
                mesh.VertexPos[i * 3 + 1] = (double)triMesh.Vertices[i].Traits.Position.y;
                mesh.VertexPos[i * 3 + 2] = (double)triMesh.Vertices[i].Traits.Position.z;
            }

            int cur = 0;
            foreach (TriMesh.Face f in triMesh.Faces)
            {
                foreach (TriMesh.Vertex v in f.Vertices)
                {
                    mesh.FaceIndex[cur++] = v.Index;
                }

            }

            //if (triMesh.Traits.HasTextureCoordinates)
            //{
            //    double[] textCoodinate=new double[mesh.VertexCount*2];
            //    cur=0;
            //    foreach (HalfEdgeMesh.TriMesh.Vertex v in triMesh.Vertices)
            //    {
            //        textCoodinate[cur++]=v.Traits.TextureCoordinate.x;
            //        textCoodinate[cur++]=v.Traits.TextureCoordinate.y;

            //    }
            //    mesh.TextextCoordinate = textCoodinate;
            //}

            //if (triMesh.Traits.HasFaceVertexNormals)
            //{
            //    double[] normal = new double[mesh.VertexCount * 3];
            //    cur = 0;
            //    foreach (HalfEdgeMesh.TriMesh.Vertex v in triMesh.Vertices)
            //    {
            //        normal[cur++] = v.Traits.Normal.x;
            //        normal[cur++] = v.Traits.Normal.y;
            //        normal[cur++] = v.Traits.Normal.z;

            //    }
            //    mesh.VertexNormal = normal;

            //}

            mesh.PostInit();
            return mesh;
        }

        public static TriMesh ConvertToTriMesh(NonManifoldMesh mesh)
        {
            TriMesh triMesh = new GraphicResearchHuiZhao.TriMesh();
            triMesh.Traits.HasFaceVertexNormals = true;
            triMesh.Traits.HasTextureCoordinates = true;

            if (mesh.VertexNormal == null)
            {
                triMesh.Traits.HasFaceVertexNormals = false;

            }
            else
            {
                triMesh.Traits.HasFaceVertexNormals = true;
            }

            if (mesh.TextextCoordinate == null)
            {
                triMesh.Traits.HasTextureCoordinates = false;
            }
            else
            {
                triMesh.Traits.HasTextureCoordinates = true;
            }

            int vertexCount = mesh.VertexCount;
            int faceCount = mesh.FaceCount;
            for (int i = 0; i < vertexCount; i++)
            {
                VertexTraits traits = new VertexTraits((float)mesh.VertexPos[i * 3], (float)mesh.VertexPos[i * 3 + 1], (float)mesh.VertexPos[i * 3 + 2]);
                triMesh.Vertices.Add(traits);

            }

            TriMesh.Vertex[] vertices = new TriMesh.Vertex[3];

            for (int i = 0; i < faceCount; i++)
            {
                vertices[0] = triMesh.Vertices[mesh.FaceIndex[i * 3]];
                vertices[1] = triMesh.Vertices[mesh.FaceIndex[i * 3 + 1]];
                vertices[2] = triMesh.Vertices[mesh.FaceIndex[i * 3 + 2]];


                triMesh.Faces.AddTriangles(vertices);
            }

            if (triMesh.Traits.HasTextureCoordinates)
            {
                TriMesh.HalfEdge faceVertex;
                foreach (TriMesh.Face face in triMesh.Faces)
                {
                    foreach (TriMesh.Vertex vertex in face.Vertices)
                    {
                        faceVertex = face.FindHalfedgeTo(vertex);
                        if (faceVertex != null) // Make sure vertex belongs to face if triangularization is on
                        {

                            faceVertex.Traits.TextureCoordinate = new Vector2D(mesh.TextextCoordinate[vertex.Index * 2], mesh.TextextCoordinate[vertex.Index * 2 + 1]);

                        }
                    }
                }
            }

            if (triMesh.Traits.HasFaceVertexNormals)
            {
                TriMesh.HalfEdge faceVertex;
                foreach (TriMesh.Face face in triMesh.Faces)
                {
                    foreach (TriMesh.Vertex vertex in face.Vertices)
                    {
                        faceVertex = face.FindHalfedgeTo(vertex);
                        if (faceVertex != null) // Make sure vertex belongs to face if triangularization is on
                        {

                            faceVertex.Traits.Normal = new Vector3D(mesh.FaceNormal, vertex.Index);

                        }
                    }
                }
            }



            //if (triMesh.Traits.HasTextureCoordinates)
            //{

            //    foreach (HalfEdgeMesh.TriMesh.Vertex vertex in triMesh.Vertices )
            //    {

            //       vertex.Traits.TextureCoordinate = new Vector2d(mesh.TextextCoordinate[vertex.Index * 2], mesh.TextextCoordinate[vertex.Index * 2 + 1]);


            //    }
            //}

            //if (triMesh.Traits.HasFaceVertexNormals)
            //{
            //   foreach (HalfEdgeMesh.TriMesh.Vertex vertex in triMesh.Vertices)
            //   { 
            //       vertex.Traits.Normal = new Vector3d(mesh.FaceNormal, vertex.Index);

            //    }
            //}


            return triMesh;
        }


    }
}
