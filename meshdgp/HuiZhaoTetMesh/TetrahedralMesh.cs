//using System;
//using System.Collections.Generic;
//using System.IO;
 

//namespace GraphicResearchHuiZhao
//{
//    public partial class TetMesh1
//    {
		

//        private int vertexCount;
//        private int tetrahedronCount;
//        private int triangleCount;
//        private int boundaryTriangleCount;

//        private double[] vertexPos = null;
//        private int[] tetrahedronIndex = null;
//        private int[] boundaryTriangleIndex = null;
//        private double[] boundaryTriangleNormal = null;
//        private byte[] flag = null;
//        private bool[] isBoundary = null;
//        private List<int>[] adjacentInfoVV = null;
//        private List<int>[] adjacentInfoVT = null;

//        public int VertexCount
//        {
//            get { return vertexCount; }
//            set { vertexCount = value; }
//        }
//        public int TetrahedronCount
//        {
//            get { return tetrahedronCount; }
//            set { tetrahedronCount = value; }
//        }
//        public int TriangleCount 
//        {
//            get { return triangleCount; } 
//        }
//        public int BoundaryTriangleCount
//        {
//            get { return boundaryTriangleCount; }
//        }
//        public double[] VertexPos 
//        {
//            get { return vertexPos; } 
//        }
//        public int[] TetrahedronIndex 
//        { 
//            get { return tetrahedronIndex; } 
//        }
//        public int[] BoundaryTriangleIndex 
//        { 
//            get { return boundaryTriangleIndex; } 
//        }
//        public double[] BoundaryTriangleNormal
//        {
//            get { return boundaryTriangleNormal; }
//        }
//        public byte[] Flag
//        {
//            get { return flag; }
//        }
//        public bool[] IsBoundary { get { return isBoundary; } }
//        public List<int>[] AdjacentInfoVV { get { return adjacentInfoVV; } }
//        public List<int>[] AdjacentInfoVT { get { return adjacentInfoVT; } }


//        public TetMesh1()
//        {
//        }

//        public TetMesh1(string file)
//        {
//            FileStream fs = File.OpenRead(file);
//            StreamReader sr = new StreamReader(fs);
//            char[] delimiters = { ' ', '\t' };
//            int num_materials = 0;

//            while (!sr.EndOfStream)
//            {
//                string line = sr.ReadLine();
//                string[] tokens = line.Split(delimiters);

//                switch (tokens[0])
//                {
//                    case "tet": break;
//                    case "num_materials":
//                        num_materials = int.Parse(tokens[1]);
//                        break;
//                    case "num_vertices":
//                        this.vertexCount = int.Parse(tokens[1]);
//                        this.vertexPos = new double[vertexCount * 3];
//                        this.flag = new byte[vertexCount];
//                        break;
//                    case "num_tetras":
//                        this.tetrahedronCount = int.Parse(tokens[1]);
//                        this.tetrahedronIndex = new int[tetrahedronCount * 4];
//                        break;
//                    case "num_triangles": 
//                        this.triangleCount = int.Parse(tokens[1]);
//                        break;
//                    case "MATERIALS":
//                        for (int i = 0; i < num_materials; i++)
//                            line = sr.ReadLine();
//                        break;
//                    case "VERTICES":
//                        for (int i=0, j=0; i<vertexCount; i++, j+=3)
//                        {
//                            string[] tokens2 = sr.ReadLine().Split(delimiters);
//                            this.vertexPos[j] = double.Parse(tokens2[0]);
//                            this.vertexPos[j + 1] = double.Parse(tokens2[1]);
//                            this.vertexPos[j + 2] = double.Parse(tokens2[2]);
//                        }
//                        break;
//                    case "TETRAS":
//                        for (int i = 0, j = 0; i < tetrahedronCount; i++, j += 4)
//                        {
//                            string[] tokens2 = sr.ReadLine().Split(delimiters);
//                            int orientation = int.Parse(tokens2[4]);
//                            this.tetrahedronIndex[j] = int.Parse(tokens2[0]);
//                            this.tetrahedronIndex[j + 1] = int.Parse(tokens2[1]);
//                            this.tetrahedronIndex[j + 2] = int.Parse(tokens2[2]);
//                            this.tetrahedronIndex[j + 3] = int.Parse(tokens2[3]);
//                        }
//                        break;
//                    case "TRIANGLES":
//                        break;
//                }
//            }

//            ScaleToUnitBox();
//            MoveToCenter();
//            ReorientateTetrahedrons();
//            BuildAdjacentInfo();
//            FindBoundary();
//            ComputeBoundaryNormal();
//        }

//        public Vector3D MaxCoord()
//        {
//            Vector3D maxCoord = new Vector3D(double.MinValue, double.MinValue, double.MinValue);
//            for (int i = 0, j = 0; i < vertexCount; i++, j += 3)
//            {
//                Vector3D v = new Vector3D(vertexPos, j);
//                maxCoord = Vector3D.Max(maxCoord, v);
//            }
//            return maxCoord;
//        }
//        public Vector3D MinCoord()
//        {
//            Vector3D minCoord = new Vector3D(double.MaxValue, double.MaxValue, double.MaxValue);
//            for (int i = 0, j = 0; i < vertexCount; i++, j += 3)
//            {
//                Vector3D v = new Vector3D(vertexPos, j);
//                minCoord = Vector3D.Min(minCoord, v);
//            }
//            return minCoord;
//        }
//        public void MoveToCenter()
//        {
//            Vector3D center = (MaxCoord() + MinCoord()) / 2.0;

//            for (int i = 0, j = 0; i < vertexCount; i++, j += 3)
//            {
//                vertexPos[j] -= center.x;
//                vertexPos[j + 1] -= center.y;
//                vertexPos[j + 2] -= center.z;
//            }
//        }
//        public void ScaleToUnitBox()
//        {
//            Vector3D d = MaxCoord() - MinCoord();
//            double s = (d.x > d.y) ? d.x : d.y;
//            s = (s > d.z) ? s : d.z;
//            if (s <= 0) return;
//            for (int i = 0; i < vertexPos.Length; i++)
//                vertexPos[i] /= s;
//        }
//        public void ReorientateTetrahedrons()
//        {
////			double eps = 10e-6;
//            int count = 0;

//            for (int i=0,j=0; i<tetrahedronCount; i++,j+=4)
//            {
//                Vector3D v1 = new Vector3D(vertexPos, tetrahedronIndex[j] * 3);
//                Vector3D v2 = new Vector3D(vertexPos, tetrahedronIndex[j+1] * 3);
//                Vector3D v3 = new Vector3D(vertexPos, tetrahedronIndex[j+2] * 3);
//                Vector3D v4 = new Vector3D(vertexPos, tetrahedronIndex[j+3] * 3);
//                Vector3D normal = ((v2-v1).Cross(v3-v1));
//                double dot = normal.Dot(v4-v1);
//// 				if (normal.Length()<eps || Math.Abs(dot)<eps)
//// 					throw new Exception("degenerated mesh! " + normal.ToString());
//                if (dot > 0) // not well orientated
//                {
//                    int tmp = tetrahedronIndex[j];
//                    tetrahedronIndex[j] = tetrahedronIndex[j+1];
//                    tetrahedronIndex[j+1] = tmp;
//                    count++;
//                }
//            }
//            OutputText("Reorientation: " + count + " flips");
//        }
//        public void BuildAdjacentInfo()
//        {
//            this.adjacentInfoVV = new List<int>[this.vertexCount];
//            this.adjacentInfoVT = new List<int>[this.vertexCount];
//            for (int i = 0; i < vertexCount; i++)
//            {
//                adjacentInfoVV[i] = new List<int>(8);
//                adjacentInfoVT[i] = new List<int>(8);
//            }

//            for (int i = 0, j = 0; i < tetrahedronCount; i++, j += 4)
//            {
//                int c1 = tetrahedronIndex[j];
//                int c2 = tetrahedronIndex[j + 1];
//                int c3 = tetrahedronIndex[j + 2];
//                int c4 = tetrahedronIndex[j + 3];
//                List<int> adj;

//                adj = adjacentInfoVV[c1];
//                if (adj.IndexOf(c2) == -1) adj.Add(c2);
//                if (adj.IndexOf(c3) == -1) adj.Add(c3);
//                if (adj.IndexOf(c4) == -1) adj.Add(c4);

//                adj = adjacentInfoVV[c2];
//                if (adj.IndexOf(c1) == -1) adj.Add(c1);
//                if (adj.IndexOf(c3) == -1) adj.Add(c3);
//                if (adj.IndexOf(c4) == -1) adj.Add(c4);

//                adj = adjacentInfoVV[c3];
//                if (adj.IndexOf(c1) == -1) adj.Add(c1);
//                if (adj.IndexOf(c2) == -1) adj.Add(c2);
//                if (adj.IndexOf(c4) == -1) adj.Add(c4);

//                adj = adjacentInfoVV[c4];
//                if (adj.IndexOf(c1) == -1) adj.Add(c1);
//                if (adj.IndexOf(c2) == -1) adj.Add(c2);
//                if (adj.IndexOf(c3) == -1) adj.Add(c3);

//                if (adjacentInfoVT[c1].IndexOf(i) == -1) adjacentInfoVT[c1].Add(i);
//                if (adjacentInfoVT[c2].IndexOf(i) == -1) adjacentInfoVT[c2].Add(i);
//                if (adjacentInfoVT[c3].IndexOf(i) == -1) adjacentInfoVT[c3].Add(i);
//                if (adjacentInfoVT[c4].IndexOf(i) == -1) adjacentInfoVT[c4].Add(i);
//            }

//        }
//        public void FindBoundary()
//        {
//            Dictionary<TriIndex, int> triRepeatCount
//                = new Dictionary<TriIndex, int>(tetrahedronCount * 3, new TriIndexComparer());
//            for (int i = 0, j = 0; i < tetrahedronCount; i++, j += 4)
//            {
//                int c1 = tetrahedronIndex[j];
//                int c2 = tetrahedronIndex[j + 1];
//                int c3 = tetrahedronIndex[j + 2];
//                int c4 = tetrahedronIndex[j + 3];
//                TriIndex t1 = new TriIndex(c1, c2, c3);
//                TriIndex t2 = new TriIndex(c2, c4, c3);
//                TriIndex t3 = new TriIndex(c3, c4, c1);
//                TriIndex t4 = new TriIndex(c1, c4, c2);
//                if (triRepeatCount.ContainsKey(t1)) triRepeatCount[t1]++;
//                else triRepeatCount.Add(t1, 1);
//                if (triRepeatCount.ContainsKey(t2)) triRepeatCount[t2]++;
//                else triRepeatCount.Add(t2, 1);
//                if (triRepeatCount.ContainsKey(t3)) triRepeatCount[t3]++;
//                else triRepeatCount.Add(t3, 1);
//                if (triRepeatCount.ContainsKey(t4)) triRepeatCount[t4]++;
//                else triRepeatCount.Add(t4, 1);
//            }

//            this.isBoundary = new bool[vertexCount];
//            List<int> boundaryTriangle = new List<int>(tetrahedronCount);
//            for (int i = 0; i < vertexCount; i++) isBoundary[i] = false;
//            foreach (KeyValuePair<TriIndex, int> pair in triRepeatCount)
//                if (pair.Value == 1)
//                {
//                    this.isBoundary[pair.Key.i] = true;
//                    this.isBoundary[pair.Key.j] = true;
//                    this.isBoundary[pair.Key.k] = true;
//                    boundaryTriangle.Add(pair.Key.i2);
//                    boundaryTriangle.Add(pair.Key.j2);
//                    boundaryTriangle.Add(pair.Key.k2);
//                }
//            this.boundaryTriangleIndex = boundaryTriangle.ToArray();
//            this.boundaryTriangleCount = boundaryTriangleIndex.Length / 3;
//        }
//        public void ComputeBoundaryNormal()
//        {
//            this.boundaryTriangleNormal = new double[boundaryTriangleIndex.Length];
//            for (int i=0,j=0; i<boundaryTriangleIndex.Length/3; i++,j+=3)
//            {
//                Vector3D v1 = new Vector3D(vertexPos, boundaryTriangleIndex[j] * 3);
//                Vector3D v2 = new Vector3D(vertexPos, boundaryTriangleIndex[j+1] * 3);
//                Vector3D v3 = new Vector3D(vertexPos, boundaryTriangleIndex[j+2] * 3);
//                Vector3D normal = ((v2-v1).Cross(v3-v1)).Normalize();
//                boundaryTriangleNormal[j] = normal.x;
//                boundaryTriangleNormal[j+1] = normal.y;
//                boundaryTriangleNormal[j+2] = normal.z;
//            }
//        }
//        public void GroupingFlags()
//        {
//            for (int i = 0; i < flag.Length; i++)
//                if (flag[i] != 0) flag[i] = 255;

//            byte id = 0;
//            Queue<int> queue = new Queue<int>();
//            for (int i = 0; i < vertexCount; i++)
//                if (flag[i] == 255)
//                {
//                    id++;
//                    flag[i] = id;
//                    queue.Enqueue(i);
//                    while (queue.Count > 0)
//                    {
//                        int curr = (int)queue.Dequeue(); 
//                        foreach (int j in adjacentInfoVV[curr])
//                            if (flag[j] == 255)
//                            {
//                                flag[j] = id;
//                                queue.Enqueue(j);
//                            }
//                    }
//                }
//        }
//        public double TetrahedronVolume(int index)
//        {
//            int b = index * 4;
//            int c1 = tetrahedronIndex[b];
//            int c2 = tetrahedronIndex[b+1];
//            int c3 = tetrahedronIndex[b+2];
//            int c4 = tetrahedronIndex[b+3];
//            Vector3D u = new Vector3D(VertexPos, c1 * 3);
//            Vector3D v1 = new Vector3D(VertexPos, c2 * 3) - u;
//            Vector3D v2 = new Vector3D(VertexPos, c3 * 3) - u;
//            Vector3D v3 = new Vector3D(VertexPos, c4 * 3) - u;

//            return Math.Abs(v1.Dot(v2.Cross(v3)) / 6.0);
//        }

//        private void OutputText(string s)
//        {
			 
//        }
//    }
//}
