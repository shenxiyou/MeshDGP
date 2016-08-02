using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GraphicResearchHuiZhao
{
    public class TetGenParser
    {
        string nodeFile;
        string edgeFile;
        string faceFile;
        string eleFile;
        string neighFile;
        TetMesh mesh;

        public TetGenParser(string fileWithoutExt)
        {
            nodeFile = fileWithoutExt + ".node";
            edgeFile = fileWithoutExt + ".edge";
            faceFile = fileWithoutExt + ".face";
            eleFile = fileWithoutExt + ".ele";
            //neighFile = fileWithoutExt + ".neigh";

            if (!File.Exists(nodeFile) ||
                !File.Exists(edgeFile) ||
                !File.Exists(faceFile) ||
                !File.Exists(eleFile) )
                //!File.Exists(neighFile))
            {
                throw new FileNotFoundException("tetgen文件不全");
            }

            mesh = new TetMesh();
        }

        /// <summary>
        /// tetgen参数-fe才能读
        /// </summary>
        /// <param name="fileWithoutExt"></param>
        /// <returns></returns>
        public TetMesh ReadAll()
        {

            ReadNodeFile();
            ReadEdgeFile();
            ReadFaceFile();
            ReadEleFile();
            //ReadNeighbour();
            return mesh;
        }

        void ReadNodeFile()
        {
            string[] all = File.ReadAllLines(nodeFile);
            int[] arr = ReadIntLine(GetFirstLine(all));

            foreach (var line in GetRemainingLine(all))
            {
                double[] dArr = ReadDoubleLine(line);
                TetVertex v = new TetVertex();
                v.Index = mesh.Vertices.Count;
                v.Pos = new Vector3D(dArr, 1);
                mesh.Vertices.Add(v);
            }
        }

        void ReadEdgeFile()
        {
            string[] all = File.ReadAllLines(edgeFile);
            int[] arr = ReadIntLine(GetFirstLine(all));

            foreach (var line in GetRemainingLine(all))
            {
                arr = ReadIntLine(line);
                TetEdge edge = new TetEdge();
                edge.Vertices.Add(mesh.Vertices[arr[1]]);
                edge.Vertices.Add(mesh.Vertices[arr[2]]);
                edge.Vertices.TrimExcess();
                edge.Index = mesh.Edges.Count;
                edge.B = arr[3] == 1;
                mesh.Edges.Add(edge);
            }
        }

        void ReadFaceFile()
        {
            string[] all = File.ReadAllLines(faceFile);
            int[] arr = ReadIntLine(GetFirstLine(all));

            foreach (var line in GetRemainingLine(all))
            {
                arr = ReadIntLine(line);
                TetFace face = new TetFace();
                face.Vertices.Add(mesh.Vertices[arr[1]]);
                face.Vertices.Add(mesh.Vertices[arr[2]]);
                face.Vertices.Add(mesh.Vertices[arr[3]]);
                face.Vertices.TrimExcess();
                face.Index = mesh.Faces.Count;
                face.B = arr[4] == 1;
                mesh.Faces.Add(face);
            }
        }

        void ReadEleFile()
        {
            string[] all = File.ReadAllLines(eleFile);
            int[] arr = ReadIntLine(GetFirstLine(all));

            foreach (var line in GetRemainingLine(all))
            {
                arr = ReadIntLine(line);
                Tetrahedron tetra = new Tetrahedron();
                for (int i = 0; i < 4; i++)
                {
                    tetra.Vertices.Add(mesh.Vertices[arr[i + 1]]);
                }
                tetra.Vertices.TrimExcess();
                tetra.Index = mesh.Tetras.Count;
                mesh.Tetras.Add(tetra);
            }
        }

        //void ReadNeighbour()
        //{
        //    string[] all = File.ReadAllLines(neighFile);

        //    foreach (var line in GetRemainingLine(all))
        //    {
        //        int[] arr = ReadIntLine(line);
        //        Tetrahedron center = mesh.Tetras[arr[0]];
        //        for (int i = 0; i < 4; i++)
        //        {
        //            if (arr[i] != -1)
        //            {
        //                center.Tetras.Add(mesh.Tetras[arr[i]]);
        //            }
        //        }
        //    }
        //}

        static string GetFirstLine(string[] all)
        {
            foreach (var line in all)
            {
                if (line.Length != 0 && line[0] != '#')
                {
                    return line;
                }
            }
            return null;
        }

        static IEnumerable<string> GetRemainingLine(string[] all)
        {
            bool r = false;
            foreach (var line in all)
            {
                if (line.Length != 0 && line[0] != '#')
                {
                    if (r)
                    {
                        yield return line;
                    }
                    else
                    {
                        r = true;
                    }
                }
            }
        }

        static int[] ReadIntLine(string line)
        {
            string[] strs = line.Split(new[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int[] arr = new int[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                if (!int.TryParse(strs[i], out arr[i]))
                {
                    return null;
                }
            }
            return arr;
        }

        static double[] ReadDoubleLine(string line)
        {
            string[] strs = line.Split(new[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            double[] arr = new double[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                if (!double.TryParse(strs[i], out arr[i]))
                {
                    return null;
                }
            }
            return arr;
        }
    }

}
