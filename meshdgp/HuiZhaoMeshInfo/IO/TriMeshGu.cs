using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public partial class TriMeshIO
    {




        public static TriMesh FromGuFile(string fileName)
        {
            TriMesh m = TriMeshIO.LoadGuFile(fileName);
            return m;
        }

        public static TriMesh FromGuStream(Stream stream)
        {
            TriMesh m = TriMeshIO.LoadGuStream(stream);
            return m;
        }

        private static TriMesh LoadGuFile(string fileName)
        {
            TriMesh mesh = null;
            using (Stream stream = File.OpenRead(fileName))
            {
                mesh = LoadGuStream(stream);
            }
            mesh.FileName = fileName;
            return mesh;
        }

        private static TriMesh LoadGuStream(Stream stream)
        {
            TriMesh mesh = new TriMesh();
            mesh.Traits.HasFaceVertexNormals = true;
            mesh.Traits.HasTextureCoordinates = true;

            StreamReader sr = new StreamReader(stream);
            PlyFileProcessorState state = new PlyFileProcessorState();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                ProcessGuLine(mesh, line, state);
            }

            mesh.TrimExcess();
            return mesh;
        }

        private static void ProcessGuLine(TriMesh mesh, string line, PlyFileProcessorState state)
        {
            // Trim out comments (allow comments trailing on a line)
            int commentStart = line.IndexOf("m");
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
                if (tokens[0] == "element" && tokens[1] == "vertex")
                {
                    state.Vnum = Int32.Parse(tokens[2]);
                }
                else if (tokens[0] == "element" && tokens[1] == "face")
                {
                    state.Fnum = Int32.Parse(tokens[2]);
                }
                else if (tokens[0] == "end_header")
                {
                    state.startv = true;
                }
                else if (state.startv == true)
                {
                    x = Single.Parse(tokens[0]);
                    y = Single.Parse(tokens[1]);
                    z = Single.Parse(tokens[2]);

                    double r = Single.Parse(tokens[3]);
                    double g = Single.Parse(tokens[4]);
                    double b = Single.Parse(tokens[5]);

                    double nx = Single.Parse(tokens[6]);
                    double ny = Single.Parse(tokens[7]);
                    double nz = Single.Parse(tokens[8]);

                    int sel = int.Parse(tokens[9]);

                    double tx = Single.Parse(tokens[10]);
                    double ty = Single.Parse(tokens[11]);

                    TriMesh.Vertex vertex = mesh.Vertices.Add(new VertexTraits(x, y, z));
                    vertex.Traits.Color = new Color4(r, g, b);
                    vertex.Traits.SelectedFlag = (byte)sel;

                    vertex.Traits.UV.x = tx;
                    vertex.Traits.UV.y = ty;
                    state.countv += 1;
                    if (state.countv == state.Vnum)
                    {
                        state.startf = true;
                        state.startv = false;
                    }
                }
                else if (state.startf == true)
                {
                    int vCount = int.Parse(tokens[0]);

                    faceVertices = new TriMesh.Vertex[vCount];


                    // Parse vertex/texture coordinate/normal indices
                    for (int i = 0; i < faceVertices.Length; ++i)
                    {
                        //string[] vertexTokens = tokens[i + 1].Split("/".ToCharArray());
                        v = Int32.Parse(tokens[i + 1]);
                        faceVertices[i] = mesh.Vertices[v.Value];
                    }

                    TriMesh.Face[] addedFaces = mesh.Faces.AddTriangles(faceVertices);

                    double r = Single.Parse(tokens[vCount + 1]);
                    double g = Single.Parse(tokens[vCount + 2]);
                    double b = Single.Parse(tokens[vCount + 3]);
                    int sel = int.Parse(tokens[vCount + 4]);

                    //double vx = Single.Parse(tokens[vCount + 5]);
                    //double vy = Single.Parse(tokens[vCount + 6]);

                    addedFaces[0].Traits.SelectedFlag = (byte)sel;
                    addedFaces[0].Traits.Color = new Color4(r, g, b);



                    state.countf += 1;
                    if (state.countf == state.Fnum)
                    {
                        state.starte = true;
                        state.startf = false;
                    }
                }

                else if (state.starte == true)
                {

                    int sel = int.Parse(tokens[0]);
                    mesh.Edges[state.counte].Traits.SelectedFlag = (byte)sel;
                    state.counte += 1;
                }

            }
        }







        public static void WriteToGuFile(string filePath, TriMesh mesh)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            //sw.WriteLine("end_header");
            //sw.WriteLine("ply");
            //sw.WriteLine("format ascii 1.0 ");
            //sw.WriteLine("comment made by Hui Zhao");
            //sw.WriteLine("comment this file is a " + mesh.ModelName);
            //sw.WriteLine("element vertex  " + mesh.Vertices.Count.ToString());
            //sw.WriteLine("property double x");
            //sw.WriteLine("property double y");
            //sw.WriteLine("property double z");
            //sw.WriteLine("property double red");
            //sw.WriteLine("property double green");
            //sw.WriteLine("property double blue");
            //sw.WriteLine("property double nx");
            //sw.WriteLine("property double ny");
            //sw.WriteLine("property double nz");
            //sw.WriteLine("property int sel");
            //sw.WriteLine("property double tx");
            //sw.WriteLine("property double ty");
            //sw.WriteLine("element face " + mesh.Faces.Count.ToString());
            //sw.WriteLine("property list uchar int vertex_indices");
            //sw.WriteLine("property double red");
            //sw.WriteLine("property double green");
            //sw.WriteLine("property double blue");
            //sw.WriteLine("property int sel");
            ////sw.WriteLine("property double vx");
            ////sw.WriteLine("property double vy");
            //sw.WriteLine("element edge " + mesh.Edges.Count.ToString());
            //sw.WriteLine("property int sel");
            //sw.WriteLine("end_header");



            foreach (TriMesh.Vertex item in mesh.Vertices)
            {
                int i = 1;
                sw.Write("Vertex   {0} {1} {2} {3}", i, item.Traits.Position.x, item.Traits.Position.y, item.Traits.Position.z);
        
                sw.Write("{ uv=({0} {1}) rgb=({2} {3} {4})} ", item.Traits.UV.x.ToString(), item.Traits.UV.y.ToString(), item.Traits.Color.R.ToString(), item.Traits.Color.G.ToString(), item.Traits.Color.B.ToString());
                sw.WriteLine();
                i++;
            }

            //  sw.WriteLine();

            foreach (TriMesh.Face item in mesh.Faces)
            {
                TriMesh.Vertex v1 = item.GetVertex(0);
                TriMesh.Vertex v2 = item.GetVertex(1);
                TriMesh.Vertex v3 = item.GetVertex(2);

                int i = 1;
                sw.Write("Face {0} {1} {2} {3}",i, v1.Index, v2.Index, v3.Index); 
                sw.WriteLine();
                i++;
            }
       

            sw.Close();
            fs.Close();
        }



    }
}
