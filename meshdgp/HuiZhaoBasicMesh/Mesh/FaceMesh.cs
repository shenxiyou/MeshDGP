using System;
using System.Collections.Generic;
using System.IO;

namespace GraphicResearchHuiZhao 
{
    public class FaceMesh:NonManifoldMesh 
    {
         

        public FaceMesh()
        {
        }

        

        public FaceMesh(string filename)
        {
            ReadHoe(filename);
        }

        public void ReadHoe(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            List<double> vlist = new List<double>();
            List<int> flist = new List<int>();
            List<double> vtlist = new List<double>();
            List<int> ftexlist = new List<int>();

            char[] delimiters = { ' ', '\t' };
            string s = "";

            while (sr.Peek() > -1)
            {
                s = sr.ReadLine();
                string[] tokens = s.Split(delimiters);
                switch (tokens[0].ToLower())
                {
                    case "feature":
                        for (int i = 1; i < tokens.Length; i++)
                        {
                            if (tokens[i].Equals("")) continue;
                            
                        }
                        break;
                    case "handle":
                        for (int i = 1; i < tokens.Length; i++)
                        {
                            if (tokens[i].Equals("")) continue;
                           
                        }
                        break;
                    case "v":
                        for (int i = 1; i < tokens.Length; i++)
                        {
                            if (tokens[i].Equals("")) continue;
                            vlist.Add(Double.Parse(tokens[i]));
                        }
                        break;
                    case "f":
                        for (int i = 1; i < tokens.Length; i++)
                        {
                            if (tokens[i].Equals("")) continue;
                            string[] tokens2 = tokens[i].Split('/');
                            flist.Add(Int32.Parse(tokens2[0]) - 1);

                        }
                        break;
                    case "ft":
                        for (int i = 1; i < tokens.Length; i++)
                        {
                            if (tokens[i].Equals("")) continue;
                            string[] tokens2 = tokens[i].Split('/');
                            ftexlist.Add(Int32.Parse(tokens2[0]) - 1);

                        }
                        break;
                    case "vt":
                        for (int i = 1; i < tokens.Length; i++)
                        {
                            if (tokens[i].Equals("")) continue;
                            vtlist.Add(Double.Parse(tokens[i]));
                        }
                        break;

                    case "texture":
                        textureFile = tokens[1];

                        break;
                }
            }

            this.vertexCount = vlist.Count / 3;
            this.faceCount = flist.Count / 3;
            this.vertexPos = new double[vertexCount * 3];
            this.faceIndex = new int[faceCount * 3];


            for (int i = 0; i < vlist.Count; i++) vertexPos[i] = (double)vlist[i];
            for (int i = 0; i < flist.Count; i++) faceIndex[i] = (int)flist[i];

            if (vtlist.Count != 0)
            {
                this.textCoordinate = new double[vtlist.Count];
                for (int i = 0; i < vtlist.Count; i++) textCoordinate[i] = (double)vtlist[i];

                textureExist = true;
            }

            if (ftexlist.Count != 0)
            {
                this.faceTexIndex = new int[faceCount * 3];
                for (int i = 0; i < ftexlist.Count; i++) faceTexIndex[i] = (int)ftexlist[i];
            }



            sr.Close();

            PostInit();
        }

        public void WriteHoe(string filename)
        {
            StreamWriter sw = new StreamWriter(filename);
            sw.Write("texture ");
            {
                sw.Write(textureFile);
            }
            sw.WriteLine();
            sw.Write("feature ");
             
            sw.WriteLine();


            sw.Write("handle ");
             
            sw.WriteLine();

            for (int i = 0, j = 0; i < vertexCount; i++, j += 3)
            {
                sw.Write("v ");
                sw.Write(vertexPos[j].ToString() + " ");
                sw.Write(vertexPos[j + 1].ToString() + " ");
                sw.Write(vertexPos[j + 2].ToString());
                sw.WriteLine();
            }

            for (int i = 0, j = 0; i < faceCount; i++, j += 3)
            {
                sw.Write("f ");
                sw.Write((faceIndex[j] + 1).ToString() + " ");
                sw.Write((faceIndex[j + 1] + 1).ToString() + " ");
                sw.Write((faceIndex[j + 2] + 1).ToString());
                sw.WriteLine();
            }

            for (int i = 0, j = 0; i < textCoordinate.Length / 2; i++, j += 2)
            {
                sw.Write("vt ");
                sw.Write((textCoordinate[j] + 1).ToString() + " ");
                sw.Write((textCoordinate[j + 1] + 1).ToString() + " ");
                sw.WriteLine();
            }

            for (int i = 0, j = 0; i < faceCount; i++, j += 3)
            {
                sw.Write("ft ");
                sw.Write((faceTexIndex[j] + 1).ToString() + " ");
                sw.Write((faceTexIndex[j + 1] + 1).ToString() + " ");
                sw.Write((faceTexIndex[j + 2] + 1).ToString());
                sw.WriteLine();
            }

            sw.Close();
        }
    }
}
