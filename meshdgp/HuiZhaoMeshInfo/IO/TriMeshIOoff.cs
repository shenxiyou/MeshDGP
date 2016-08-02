using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;


namespace GraphicResearchHuiZhao 
{
    public partial class TriMeshIO
    {
      

  
        /// <summary>
        /// Stores state during processing of an OBJ file.
        /// </summary>
        private class OffFileProcessorState
        {
            public List<Vector3D> VertexNormals = new List<Vector3D>();
            public List<Vector2D> VertexTextureCoords = new List<Vector2D>();
        }
     

        public static TriMesh FromOffFile(string fileName)
        {
            TriMesh m = TriMeshIO.LoadOffFile(fileName);
            return m;
        }

        public static TriMesh FromOffStream(Stream stream)
        {
            TriMesh m = TriMeshIO.LoadOffStream(stream);
            return m;
        }

        private static TriMesh LoadOffStream(Stream stream)
        {
            TriMesh mesh = new TriMesh();
            mesh.Traits.HasFaceVertexNormals = true;
            mesh.Traits.HasTextureCoordinates = true;

            StreamReader sr = new StreamReader(stream);
            OffFileProcessorState state = new OffFileProcessorState();
            string line;
            bool ignoreFirstThreeNum = true;


            while ((line = sr.ReadLine()) != null)
            {
                ProcessOffLine(mesh, line, state, ref ignoreFirstThreeNum);
            }

            mesh.TrimExcess();
            return mesh;
        }

        private static TriMesh LoadOffFile(string fileName)
        {
            TriMesh mesh = null;
            using (Stream stream = File.OpenRead(fileName))
            {
                mesh = TriMeshIO.LoadOffStream(stream);
            }
            mesh.FileName = fileName;
            return mesh;

        }

        private static void ProcessOffLine(TriMesh mesh, string line, OffFileProcessorState state, ref bool ignoreFirstThreeNum)
        {
            // Trim out comments (allow comments trailing on a line)
            int commentStart = line.IndexOf("OFF");
            if (commentStart != -1)
            {
                line = line.Substring(0, commentStart);
            }

            // Tokenize line
            string[] tokens = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

            // Process line based on the keyword used
            if (tokens.Length > 0)
            {
                int? v;
                float x, y, z;
                TriMesh.Vertex[] faceVertices;
                int?[] vt, vn;

                if (tokens.Length == 3)
                {
                    if (!ignoreFirstThreeNum)
                    {
                        x = Single.Parse(tokens[0]);
                        y = Single.Parse(tokens[1]);
                        z = Single.Parse(tokens[2]);

                        mesh.Vertices.Add(new VertexTraits(x, y, z));
                    }

                    ignoreFirstThreeNum = false;
                }
                else if (tokens.Length == 4 || tokens.Length == 5)
                {
                    faceVertices = new TriMesh.Vertex[tokens.Length - 1];
                    vt = new int?[tokens.Length - 1];
                    vn = new int?[tokens.Length - 1];

                    // Parse vertex/texture coordinate/normal indices
                    for (int i = 0; i < faceVertices.Length; ++i)
                    {
                        //string[] vertexTokens = tokens[i + 1].Split("/".ToCharArray());
                        v = Int32.Parse(tokens[i + 1]);
                        faceVertices[i] = mesh.Vertices[v.Value];
                    }

                    TriMesh.Face[] addedFaces = mesh.Faces.AddTriangles(faceVertices);

                    // Set texture coordinates and normals if any are given
                    for (int i = 0; i < faceVertices.Length; ++i)
                    {
                        TriMesh.HalfEdge faceVertex;
                        if (vt[i].HasValue || vn[i].HasValue)
                        {
                            foreach (TriMesh.Face f in addedFaces)
                            {
                                faceVertex = f.FindHalfedgeTo(faceVertices[i]);
                                if (faceVertex != null) // Make sure vertex belongs to face if triangularization is on
                                {
                                    if (vt[i].HasValue)
                                    {
                                        faceVertex.Traits.TextureCoordinate = state.VertexTextureCoords[vt[i].Value - 1];
                                    }
                                    if (vn[i].HasValue)
                                    {
                                        faceVertex.Traits.Normal = state.VertexNormals[vn[i].Value - 1];
                                    }
                                }
                            }
                        }
                    }
                }


            }
        }


        public static void WriteToOffFile(string filePath, TriMesh mesh)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine("OFF");
            sw.WriteLine(mesh.Vertices.Count + " " + mesh.Faces.Count + " " + mesh.Edges.Count);

            foreach (TriMesh.Vertex item in mesh.Vertices)
            {
                sw.WriteLine("{0} {1} {2}", item.Traits.Position.x, item.Traits.Position.y, item.Traits.Position.z);
            }
            foreach (TriMesh.Face item in mesh.Faces)
            {
                TriMesh.Vertex v1 = item.GetVertex(0);
                TriMesh.Vertex v2 = item.GetVertex(1);
                TriMesh.Vertex v3 = item.GetVertex(2);

                sw.WriteLine("3 {0} {1} {2}", v1.Index, v2.Index, v3.Index);
            }

            sw.Close();
            fs.Close();
        }
    }
}
