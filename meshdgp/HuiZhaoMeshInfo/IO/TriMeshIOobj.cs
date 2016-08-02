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
        private class ObjFileProcessorState
        {
            public List<Vector3D> VertexNormals = new List<Vector3D>();
            public List<Vector2D> VertexTextureCoords = new List<Vector2D>();
        }
 


        #region Static methods
        /// <summary>
        /// Returns a <see cref="TriMesh"/> object loaded from the specified OBJ file.
        /// </summary>
        /// <param name="fileName">The name of the OBJ file to load.</param>
        /// <returns>The mesh loaded from the OBJ file.</returns>
        public static TriMesh FromObjFile(string fileName)
        {
            TriMesh m = TriMeshIO.LoadObjFile(fileName);
            return m;
        }

        /// <summary>
        /// Returns a <see cref="TriMesh"/> object loaded from the specified OBJ file stream.
        /// </summary>
        /// <param name="stream">The stream for the OBJ to load.</param>
        /// <returns>The mesh loaded from the OBJ file stream.</returns>
        public static TriMesh FromObjStream(Stream stream)
        {
            TriMesh m = TriMeshIO.LoadObjStream(stream);
            return m;
        }
        #endregion

        /// <summary>
        /// Loads an OBJ file.
        /// </summary>
        /// <param name="fileName">The name of the file to load.</param>
        private static TriMesh LoadObjFile(string fileName)
        {
            TriMesh mesh = null;
            using (Stream stream = File.OpenRead(fileName))
            {
                mesh = LoadObjStream(stream);
            }
            mesh.FileName = fileName;
            return mesh;

        }

        /// <summary>
        /// Loads an OBJ file from a stream.
        /// </summary>
        /// <param name="stream">A stream with OBJ file data.</param>
        private static TriMesh LoadObjStream(Stream stream)
        {
            TriMesh mesh = new TriMesh();
            mesh.Traits.HasFaceVertexNormals = true;
            mesh.Traits.HasTextureCoordinates = true;

            StreamReader sr = new StreamReader(stream);
            ObjFileProcessorState state = new ObjFileProcessorState();
            string line;

            while ((line = sr.ReadLine()) != null)
            {

                ProcessObjLine(mesh, line, state);

            }

            if (state.VertexTextureCoords.Count == mesh.Vertices.Count)
            {
                for (int i = 0; i < mesh.Vertices.Count; i++)
                {
                    mesh.Vertices[i].Traits.UV = state.VertexTextureCoords[i];
                }
            }
            mesh.TrimExcess();
            return mesh;
        }

        /// <summary>
        /// Processes a line from an OBJ file.
        /// </summary>
        /// <param name="line">A line from an OBJ file.</param>
        /// <param name="state">An object that manages state between calls.</param>
        private static void ProcessObjLine(TriMesh mesh, string line, ObjFileProcessorState state)
        {
            // Trim out comments (allow comments trailing on a line)
            int commentStart = line.IndexOf('#');
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

                switch (tokens[0])
                {
                    case "v":   // Vertex
                        if (tokens.Length != 4)
                        {
                            throw new IOException("Vertices in the OBJ file must have 3 coordinates.");
                        }

                        x = Single.Parse(tokens[1]);
                        y = Single.Parse(tokens[2]);
                        z = Single.Parse(tokens[3]);

                        mesh.Vertices.Add(new VertexTraits(x, y, z));
                        break;

                    case "vt":   // Vertex texture
                        if (tokens.Length != 3 && tokens.Length != 4)
                        {
                            throw new IOException("Texture coordinates in the OBJ file must have 2 or 3 coordinates.");
                        }

                        x = Single.Parse(tokens[1]);
                        y = Single.Parse(tokens[2]);


                        state.VertexTextureCoords.Add(new Vector2D(x, y));
                        break;

                    case "vn":   // Vertex normal
                        if (tokens.Length != 4)
                        {
                            throw new IOException("Vertex normals in the OBJ file must have 3 coordinates.");
                        }

                        x = Single.Parse(tokens[1]);
                        y = Single.Parse(tokens[2]);
                        z = Single.Parse(tokens[3]);

                        state.VertexNormals.Add(new Vector3D(x, y, z));
                        break;

                    case "f":   // Face
                        faceVertices = new TriMesh.Vertex[tokens.Length - 1];
                        vt = new int?[tokens.Length - 1];
                        vn = new int?[tokens.Length - 1];

                        // Parse vertex/texture coordinate/normal indices
                        for (int i = 0; i < faceVertices.Length; ++i)
                        {
                            string[] vertexTokens = tokens[i + 1].Split("/".ToCharArray());

                            v = Int32.Parse(vertexTokens[0]);

                            if (vertexTokens.Length > 1 && vertexTokens[1].Length > 0)
                            {
                                vt[i] = Int32.Parse(vertexTokens[1]);
                            }
                            else
                            {
                                mesh.Traits.HasTextureCoordinates = false;
                            }

                            if (vertexTokens.Length > 2 && vertexTokens[2].Length > 0)
                            {
                                vn[i] = Int32.Parse(vertexTokens[2]);
                            }
                            else
                            {
                                mesh.Traits.HasFaceVertexNormals = false;
                            }

                            faceVertices[i] = mesh.Vertices[v.Value - 1];
                        }

                        try
                        {
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
                        catch { }

                        break;
                }
            }
        }



        public static void WriteToObjFile(string filePath, TriMesh mesh)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine("#Vertices:" + mesh.Vertices.Count);
            sw.WriteLine("#Faces:" + mesh.Faces.Count);

            foreach (TriMesh.Vertex item in mesh.Vertices)
            {
                sw.WriteLine("v {0} {1} {2}", item.Traits.Position.x, item.Traits.Position.y, item.Traits.Position.z);
            }

            sw.WriteLine();

            foreach (TriMesh.Face item in mesh.Faces)
            {
                TriMesh.Vertex v1 = item.GetVertex(0);
                TriMesh.Vertex v2 = item.GetVertex(1);
                TriMesh.Vertex v3 = item.GetVertex(2);

                sw.WriteLine("f {0} {1} {2}", v1.Index + 1, v2.Index + 1, v3.Index + 1);
            }

            sw.Close();
            fs.Close();
        }



        public static void WriteSelection(string filePath, TriMesh mesh)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            //sw.WriteLine("#Vertices:" + mesh.Vertices.Count);
            //sw.WriteLine("#Edges:" + mesh.Faces.Count);
            //sw.WriteLine("#Faces:" + mesh.Faces.Count);

            foreach (TriMesh.Vertex item in mesh.Vertices)
            {
                if (item.Traits.SelectedFlag > 0)
                {
                    sw.WriteLine("{0} {1} {2}", item.Traits.Position.x, item.Traits.Position.y, item.Traits.Position.z);
                }
            }

            sw.WriteLine();

            foreach (TriMesh.Edge item in mesh.Edges)
            {
                if (item.Traits.SelectedFlag > 0)
                {
                    Vector3D v1 = item.Vertex0.Traits.Position;
                    Vector3D v2 = item.Vertex1.Traits.Position;
                    sw.WriteLine("{0} {1} {2} {3} {4} {5} ", v1.x, v1.y, v1.z, v2.x, v2.y, v2.z);
                }
            }

            sw.WriteLine();

            foreach (TriMesh.Face item in mesh.Faces)
            {
                if (item.Traits.SelectedFlag > 0)
                {
                    Vector3D v1 = item.GetVertex(0).Traits.Position;
                    Vector3D v2 = item.GetVertex(1).Traits.Position;
                    Vector3D v3 = item.GetVertex(2).Traits.Position;

                    sw.WriteLine("{0} {1} {2} {3} {4} {5} {6} {7} {8}", v1.x, v1.y, v1.z, v2.x, v2.y, v2.z, v3.x, v3.y, v3.z);
                }
            }

            sw.WriteLine();

            sw.Close();
            fs.Close();
        }


        public static void WriteToObjFileWithTexture(string filePath, TriMesh mesh)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine("#Vertices:" + mesh.Vertices.Count);
            sw.WriteLine("#Faces:" + mesh.Faces.Count);

            foreach (TriMesh.Vertex item in mesh.Vertices)
            {
                sw.WriteLine("v {0} {1} {2}", item.Traits.Position.x, item.Traits.Position.y, item.Traits.Position.z);
            }

            sw.WriteLine();

            foreach (TriMesh.Vertex item in mesh.Vertices)
            {
                sw.WriteLine("vt {0} {1}  ", item.Traits.UV.x, item.Traits.UV.y);
            }



            foreach (TriMesh.Face item in mesh.Faces)
            {
                TriMesh.Vertex v1 = item.GetVertex(0);
                TriMesh.Vertex v2 = item.GetVertex(1);
                TriMesh.Vertex v3 = item.GetVertex(2);

                int index1 = v1.Index + 1;
                int index2 = v2.Index + 1;
                int index3 = v3.Index + 1;

                sw.WriteLine("f {0} {1} {2}", index1 + "/" + index1, index2 + "/" + index2, index3 + "/" + index3);
            }



            sw.Close();
            fs.Close();
        }


    }
}
