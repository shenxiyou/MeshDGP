using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class Morse
    {
        public TriMesh mesh;
        public double[] function;
        public EnumMorseVertexType[] morseType;

        public bool WithoutSaddle = true; 
       


        public MorseVertice morseVertice = new MorseVertice();

        public Morse(TriMesh mesh, double[] function)
        {
            this.mesh = mesh;
            this.function = function;
            this.Init();
        }

        public int ComputeSaddleType(TriMesh.Vertex saddle,double[] function)
        {
            int m =  CountChange(saddle,function) / 2 - 1; 
            return m;
        }

        public void ColorMorseVertice(TriMesh mesh, EnumMorseVertexType[] morseType)
        {
            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                switch (morseType[v.Index])
                {
                    case EnumMorseVertexType.Regular:
                        break;
                    case EnumMorseVertexType.Minum:
                        v.Traits.SelectedFlag = 1;
                        v.Traits.Color = Color4.Yellow;
                        break;
                    case EnumMorseVertexType.Maxium:
                        v.Traits.SelectedFlag = 2;
                        v.Traits.Color = Color4.Red;
                        break;
                    case EnumMorseVertexType.Saddle:
                        int change = ComputeSaddleType(v,function);
                        v.Traits.SelectedFlag =(byte)(3 + change);
                        v.Traits.Color = Color4.Blue;
                        break;
                    default:
                        break;
                }
            }
            //int m = 0;
            //foreach (TriMesh.Vertex s in this.saddleList)
            //{
            //    m +=  CountChange(s,function) / 2 - 1;
            //}
            //Console.WriteLine("Max: {0}, Min: {1}, Saddle: {2}, Saddle m: {3}", 
            //    this.maxList.Count, this.minList.Count, this.saddleList.Count, m);
            //Console.WriteLine("euler: {0}", TriMeshUtil.GetEulerCharacteristic(this.mesh));
        }

        public void ColorMorseVertice(TriMesh mesh,double[] function)
        {
            EnumMorseVertexType[] morseType = ComputeMorse(mesh, function);

            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                switch (morseType[v.Index])
                {
                    case EnumMorseVertexType.Regular:
                        break;
                    case EnumMorseVertexType.Minum:
                        v.Traits.SelectedFlag = 1;
                        v.Traits.Color = Color4.Yellow;
                        break;
                    case EnumMorseVertexType.Maxium:
                        v.Traits.SelectedFlag = 2;
                        v.Traits.Color = Color4.Red;
                        break;
                    case EnumMorseVertexType.Saddle:
                        int change = ComputeSaddleType(v,function);
                        v.Traits.SelectedFlag = (byte)(3 + change);
                        v.Traits.Color = Color4.Blue;
                        break;
                    default:
                        break;
                }
            }
            //int m = 0;
            //foreach (TriMesh.Vertex s in this.saddleList)
            //{
            //    m +=  CountChange(s,function) / 2 - 1;
            //}
            //Console.WriteLine("Max: {0}, Min: {1}, Saddle: {2}, Saddle m: {3}", 
            //    this.maxList.Count, this.minList.Count, this.saddleList.Count, m);
            //Console.WriteLine("euler: {0}", TriMeshUtil.GetEulerCharacteristic(this.mesh));
        }

        private void Init()
        {
            TriMeshModify.RepairSimpleHoles(this.mesh);
            AdjustFunction(mesh,ref function);


            morseType=ComputeMorse(mesh, function); 
            morseVertice = ComputeMorseVertice(morseType);
        }

        public void AdjustFunction(TriMesh mesh,ref double[] function)
        {

            LinkedList<TriMesh.Edge> link = new LinkedList<HalfEdgeMesh.Edge>();
            foreach (TriMesh.Edge edge in  mesh.Edges)
            {
                if ( function[edge.Vertex0.Index] ==  function[edge.Vertex1.Index])
                {
                    link.AddLast(edge);
                }
            }

            while (link.Count != 0)
            {
                TriMesh.HalfEdge[] path = TriMeshModify.Sort(link);
                //if (path[0].FromVertex != path[path.Length - 1].ToVertex)
                {
                    double adj = 0.00000001;
                    if (IsMax(path[0].FromVertex,ref function))
                    {
                        adj = -adj;
                    }
                    foreach (TriMesh.HalfEdge  hf in path)
                    {
                         function[hf.ToVertex.Index] = function[hf.FromVertex.Index] + adj;
                    }
                }
            }
        }

        public void CheckFunction(TriMesh mesh,ref double[] function)
        {
            foreach (TriMesh.Edge edge in  mesh.Edges)
            {
                if ( function[edge.Vertex0.Index] ==  function[edge.Vertex1.Index])
                {
                    edge.Traits.SelectedFlag = 9;
                    edge.Traits.Color = Color4.Yellow;
                }
            }
        }

        public bool IsMax(TriMesh.Vertex v,ref double[] function)
        {
            foreach (TriMesh.Vertex round in v.Vertices)
            {
                if (this.function[round.Index] > this.function[v.Index])
                {
                    return false;
                }
            }
            return true;
        }


        public MorseVertice ComputeMorseVertice(TriMesh mesh, double[] function)
        {

            EnumMorseVertexType[] morseType = ComputeMorse(mesh,function);

            MorseVertice morseVertice= new MorseVertice();
            for(int i=0;i<morseType.Length ;i++)
            {
                switch (morseType[i])
                {
                    case EnumMorseVertexType.Saddle: 
                    morseVertice.SaddleList.Add(mesh.Vertices[i]);
                    break;

                    case EnumMorseVertexType.Maxium:
                    morseVertice.MaxList.Add(mesh.Vertices[i]);
                    break;
                  
                    case EnumMorseVertexType.Minum:
                    morseVertice.MinList.Add(mesh.Vertices[i]);
                    break;

                   
                }
                
            }
           
            return morseVertice;
        }

        public MorseVertice ComputeMorseVertice(EnumMorseVertexType[] morseType)
        {
 

            MorseVertice morseVertice = new MorseVertice();
            for (int i = 0; i < morseType.Length; i++)
            {
                switch (morseType[i])
                {
                    case EnumMorseVertexType.Saddle:
                        morseVertice.SaddleList.Add(mesh.Vertices[i]);
                        break;

                    case EnumMorseVertexType.Maxium:
                        morseVertice.MaxList.Add(mesh.Vertices[i]);
                        break;

                    case EnumMorseVertexType.Minum:
                        morseVertice.MinList.Add(mesh.Vertices[i]);
                        break;


                }

            }
            return morseVertice;
        }


        public EnumMorseVertexType[] ComputeMorse(TriMesh mesh,double[] function)
        {
            EnumMorseVertexType[] morseType = new EnumMorseVertexType[ mesh.Vertices.Count];
            foreach (TriMesh.Vertex v in  mesh.Vertices)
            {
                double mid =  function[v.Index];
                int change =  CountChange(v,function); 
                if (change == 2)
                {
                     morseType[v.Index] = EnumMorseVertexType.Regular;
                }
                else if (change > 2 && change % 2 == 0)
                {
                     morseType[v.Index] = EnumMorseVertexType.Saddle;
                      
                }
                else if (change == 0)
                {
                    double round =  function[v.HalfEdge.ToVertex.Index];
                    if (round > mid)
                    {
                        morseType[v.Index] = EnumMorseVertexType.Minum;
                        
                    }
                    else
                    {
                        morseType[v.Index] = EnumMorseVertexType.Maxium; 
                    }
                }
                else
                {
                    throw new Exception("不应该为奇数");
                }
            }
            return  morseType;
        }

        public int[] ComputeMorseChange(TriMesh mesh, double[] function)
        {
            EnumMorseVertexType[] type = ComputeMorse(mesh, function);
            int[] m = new int[type.Length];
            for (int i = 0; i < type.Length; i++)
            {
                m[i] = 0;
                if (type[i] == EnumMorseVertexType.Saddle)
                {
                    m[i] = ComputeSaddleType(mesh.Vertices[i],function);
                }
                if (type[i] == EnumMorseVertexType.Minum)
                {
                    m[i] = 1;
                }
                if (type[i] == EnumMorseVertexType.Maxium)
                {
                    m[i] = 1;
                }
                if (type[i] == EnumMorseVertexType.Regular)
                {
                    m[i] = 0;
                }
            }

            return m;

        }

        public int CountChange(TriMesh.Vertex v, double[] function)
        {
            double mid =  function[v.Index];
            int count = 0;
            foreach (TriMesh.HalfEdge hf in v.HalfEdges)
            {
                TriMesh.HalfEdge lk = hf.Next;
                double from =  function[lk.FromVertex.Index];
                double to =  function[lk.ToVertex.Index];
                if ((from > mid) != (to > mid))
                {
                    count++;
                }
            }
            return count;
        }

        public int CountChange(TriMesh.Vertex v)
        {
            double mid = function[v.Index];
            int count = 0;
            foreach (TriMesh.HalfEdge hf in v.HalfEdges)
            {
                TriMesh.HalfEdge lk = hf.Next;
                double from = function[lk.FromVertex.Index];
                double to = function[lk.ToVertex.Index];
                if ((from > mid) != (to > mid))
                {
                    count++;
                }
            }
            return count;
        }

        private TriMesh.HalfEdge FindN2P(TriMesh.Vertex v)
        {
            double mid = this.function[v.Index];
            foreach (TriMesh.HalfEdge hf in v.HalfEdges)
            {
                TriMesh.HalfEdge lk = hf.Next;
                double from = this.function[lk.FromVertex.Index];
                double to = this.function[lk.ToVertex.Index];
                if (from < mid && to > mid)
                {
                    return hf;
                }
            }
            return null;
        }

        private TriMesh.HalfEdge[] FindExtreme(TriMesh.Vertex v, bool maxOrMin)
        {
            switch (this.morseType[v.Index])
            {
                case EnumMorseVertexType.Regular:
                case EnumMorseVertexType.Minum:
                case EnumMorseVertexType.Maxium:
                    TriMesh.HalfEdge hf = this.FindExtreme(v.HalfEdges, maxOrMin);
                    if (hf != null)
                    {
                        return new HalfEdgeMesh.HalfEdge[] { hf };
                    }
                    break;
                case EnumMorseVertexType.Saddle:
                    return this.FindSaddleExtreme(v, maxOrMin);
                default:
                    break;
            }
            return new HalfEdgeMesh.HalfEdge[0];
        }

        private TriMesh.HalfEdge FindExtreme(IEnumerable<TriMesh.HalfEdge> hfGroup, bool maxOrMin)
        {
            double value = maxOrMin ? double.MinValue : double.MaxValue;
            TriMesh.HalfEdge tmp = null;

            foreach (TriMesh.HalfEdge hf in hfGroup)
            {
                if (this.Compare(this.function[hf.ToVertex.Index], value, maxOrMin) &&
                    this.Compare(this.function[hf.ToVertex.Index], this.function[hf.FromVertex.Index], maxOrMin))
                {
                    value = this.function[hf.ToVertex.Index];
                    tmp = hf;
                }
            }
            return tmp;
        }

        private bool Compare(double value1, double value2, bool greateOrLess)
        {
            return (value1 > value2) == greateOrLess;
        }

        private TriMesh.HalfEdge[] FindSaddleExtreme(TriMesh.Vertex v, bool maxOrMin)
        {
            List<TriMesh.HalfEdge> all = new List<HalfEdgeMesh.HalfEdge>();
            TriMesh.HalfEdge n2p = this.FindN2P(v);
            v.HalfEdge = n2p;
            List<TriMesh.HalfEdge> part = new List<HalfEdgeMesh.HalfEdge>();
            double mid = this.function[v.Index];
            foreach (TriMesh.HalfEdge hf in v.HalfEdges)
            {
                double round = this.function[hf.ToVertex.Index];
                if (this.Compare(round, mid, maxOrMin))
                {
                    part.Add(hf);
                }
                else
                {
                    if (part.Count != 0)
                    {
                        all.Add(this.FindExtreme(part, maxOrMin));
                    }
                    part.Clear();
                }
            }
            if (part.Count != 0)
            {
                all.Add(this.FindExtreme(part, maxOrMin));
            }
            return all.ToArray();
        }

        public TriMesh.HalfEdge[][] FindPath(TriMesh.Vertex v, bool ascOrDesc)
        {
            List<TriMesh.HalfEdge[]> all = new List<HalfEdgeMesh.HalfEdge[]>();
            List<TriMesh.HalfEdge> path = new List<HalfEdgeMesh.HalfEdge>();
            Stack<TriMesh.HalfEdge> stack = new Stack<HalfEdgeMesh.HalfEdge>();
            stack.Push(v.HalfEdge.Opposite);
            while (stack.Count != 0)
            {
                if (stack.Count > 100000)
                {
                    throw new Exception("估计死循环了");
                }
                TriMesh.HalfEdge cur = stack.Pop();
                if (cur.ToVertex != v)
                {
                    path.Add(cur);
                }
                TriMesh.HalfEdge[] extreme = this.FindExtreme(cur.ToVertex, ascOrDesc);
                if (extreme.Length == 0)
                {
                    all.Add(path.ToArray());
                    path.Clear();
                }
                foreach (TriMesh.HalfEdge hf in extreme)
                {
                    if (!this.WithoutSaddle || this.morseType[hf.ToVertex.Index] != EnumMorseVertexType.Saddle)
                    {
                        stack.Push(hf);
                    }
                    else
                    {
                        TriMesh.HalfEdge start = hf.Opposite;
                        double saddleValue = this.function[hf.ToVertex.Index];
                        double roundValue = this.function[start.Next.ToVertex.Index];
                        while (!this.Compare(saddleValue, roundValue, ascOrDesc))
                        {
                            start = start.Previous.Opposite;
                            roundValue = this.function[start.Next.ToVertex.Index];
                            path.Add(start.Next);
                        }
                        stack.Push(start.Next);
                    }
                }
            }
            return all.ToArray();
        }



        public void DrawSaddleToMin(TriMesh mesh, double[] function)
        {
            MorseVertice morseVertice = ComputeMorseVertice(mesh, function);

            foreach (TriMesh.Vertex saddle in morseVertice.SaddleList)
            {
                foreach (TriMesh.HalfEdge[] path in FindPath(saddle, false))
                {
                    foreach (TriMesh.HalfEdge hf in path)
                    {
                        hf.Edge.Traits.SelectedFlag = 1;
                        hf.Edge.Traits.Color = Color4.Green;
                    }
                }
            }
           
        }

        public void DrawSaddleToMax(TriMesh mesh,double[] function)
        {
            MorseVertice morseVertice = ComputeMorseVertice(mesh, function);

            foreach (TriMesh.Vertex saddle in morseVertice.SaddleList)
            {
                foreach (TriMesh.HalfEdge[] path in FindPath(saddle, true))
                {
                    foreach (TriMesh.HalfEdge hf in path)
                    {
                        hf.Edge.Traits.SelectedFlag = 1;
                        hf.Edge.Traits.Color = Color4.Purple;
                    }
                }
            }

        }


        public void DrawSaddleToMin( )
        {
           

            foreach (TriMesh.Vertex saddle in morseVertice.SaddleList)
            {
                foreach (TriMesh.HalfEdge[] path in FindPath(saddle, false))
                {
                    foreach (TriMesh.HalfEdge hf in path)
                    {
                        hf.Edge.Traits.SelectedFlag = 1;
                        hf.Edge.Traits.Color = Color4.Green;
                    }
                }
            }

        }

        public void DrawSaddleToMax()
        {
            

            foreach (TriMesh.Vertex saddle in morseVertice.SaddleList)
            {
                foreach (TriMesh.HalfEdge[] path in FindPath(saddle, true))
                {
                    foreach (TriMesh.HalfEdge hf in path)
                    {
                        hf.Edge.Traits.SelectedFlag = 1;
                        hf.Edge.Traits.Color = Color4.Purple;
                    }
                }
            }

        }

        
        public string BuildMorseTheory(TriMesh mesh, double[] function)
        {
            morseVertice = ComputeMorseVertice(mesh, function);
            string morseinfo = "   Vertices - Edges + Faces= ";
            morseinfo += TriMeshUtil.CountEulerCharacteristic(mesh)+"\r\n";
            
            morseinfo += "   Saddle: " + morseVertice.SaddleList.Count+"\r\n";
            morseinfo += "   Maxima: " + morseVertice.MaxList.Count + "\r\n";
            morseinfo += "   Minima: " + morseVertice.MinList.Count + "\r\n";
            morseinfo += "   Minima - Saddle + Maxima = ";
            morseinfo += morseVertice.MinList.Count - morseVertice.SaddleList.Count + morseVertice.MaxList.Count + "\r\n";

            return morseinfo;

        }


    }
}
