


using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
 

namespace GraphicResearchHuiZhao
{
    public partial class QuadMesh
    {

        


        #region OffFileProcessorState class
        /// <summary>
        /// Stores state during processing of an OBJ file.
        /// </summary>
        private class OffFileProcessorState
        {
            public List<Vector3D> VertexNormals = new List<Vector3D>();
            public List<Vector2D> VertexTextureCoords = new List<Vector2D>();
        }
        #endregion
        /// <summary>
        /// Creates a new, identical mesh.
        /// </summary>
        /// <returns>A deep copy of the mesh.</returns>
        public QuadMesh Copy()
        {
            // Use serialization to create a deep copy
            using (MemoryStream ms = new MemoryStream(Vertices.Count * 300))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, this);
                ms.Seek(0, SeekOrigin.Begin);
                return (QuadMesh)bf.Deserialize(ms);
            }
        }

        
 


        public static QuadMesh FromOffFile(string fileName)
        {
            QuadMesh m = new QuadMesh();
            m.LoadOffFile(fileName);
            return m;
        }
     

        public static QuadMesh FromOffStream(Stream stream)
        {
            QuadMesh m = new QuadMesh();
            m.LoadOffStream(stream);
            return m;
        }
 
 
        private void LoadOffFile(string fileName)
        {
            using (Stream stream = File.OpenRead(fileName))
            {
                LoadOffStream(stream);
            }
            this.fileName = fileName;

        }
 

        private void LoadOffStream(Stream stream)
        {
            Traits.HasFaceVertexNormals = true;
            Traits.HasTextureCoordinates = true;

            StreamReader sr = new StreamReader(stream);
            OffFileProcessorState state = new OffFileProcessorState();
            string line;
            bool ignoreFirstThreeNum = true;

            while ((line = sr.ReadLine()) != null)
            {
                ProcessOffLine(line, state,ref ignoreFirstThreeNum);
            }

            TrimExcess();
        }


        

        private void ProcessOffLine(string line, OffFileProcessorState state, ref bool ignoreFirstThreeNum)
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
                Vertex[] faceVertices;
                int?[] vt, vn;

                if (tokens.Length == 3)
                {
                    if (!ignoreFirstThreeNum)
                    {
                        x = Single.Parse(tokens[0]);
                        y = Single.Parse(tokens[1]);
                        z = Single.Parse(tokens[2]);

                        Vertices.Add(new VertexTraits(x, y, z));
                    }

                    ignoreFirstThreeNum = false;
                }
                else if (tokens.Length == 4 || tokens.Length == 5)
                {
                    faceVertices = new Vertex[tokens.Length - 1];
                    vt = new int?[tokens.Length - 1];
                    vn = new int?[tokens.Length - 1];

                    // Parse vertex/texture coordinate/normal indices
                    for (int i = 0; i < faceVertices.Length; ++i)
                    {
                        string[] vertexTokens = tokens[i + 1].Split("/".ToCharArray());

                        v = Int32.Parse(tokens[i + 1]);

                        if (vertexTokens.Length > 1 && vertexTokens[1].Length > 0)
                        {
                            vt[i] = Int32.Parse(vertexTokens[1]);
                        }
                        else
                        {
                            Traits.HasTextureCoordinates = false;
                        }

                        if (vertexTokens.Length > 2 && vertexTokens[2].Length > 0)
                        {
                            vn[i] = Int32.Parse(vertexTokens[2]);
                        }
                        else
                        {
                            Traits.HasFaceVertexNormals = false;
                        }

                        faceVertices[i] = Vertices[v.Value];
                    }

                    Face[] addedFaces = Faces.AddQuads(faceVertices);

                    // Set texture coordinates and normals if any are given
                    for (int i = 0; i < faceVertices.Length; ++i)
                    {
                        HalfEdge faceVertex;
                        if (vt[i].HasValue || vn[i].HasValue)
                        {
                            foreach (Face f in addedFaces)
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


    }
}
