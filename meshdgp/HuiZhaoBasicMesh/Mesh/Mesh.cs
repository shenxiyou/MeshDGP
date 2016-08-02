using System;
using System.Collections;
using System.Collections.Generic;
using System.IO; 

namespace GraphicResearchHuiZhao
{
	public class NonManifoldMesh:IDisposable
	{




        private NonManifoldMesh dualMesh = null;
        public NonManifoldMesh DualMesh
        {
            get
            {
                if (dualMesh == null)
                {
                    dualMesh =MeshOperators.CreateDualMesh(this);
                }
                return dualMesh;
            }
            set
            {
                dualMesh = value;
            }
        }


        protected int vertexCount = -1;
        protected int faceCount = -1;

        protected double[] vertexPos = null;
        protected double[] vertexNormal = null;
        protected double[] textCoordinate = null;
        protected int[] faceIndex = null;
        protected int[] faceTexIndex = null;
        protected double[] faceNormal = null;
        protected double[] dualVertexPos = null;
        protected byte[] vertexflag = null;
        protected byte[] faceFlag = null;
        protected bool[] isBoundary = null;
        protected double[] color = null;
        protected int[][] adjVV = null;
        protected int[][] adjVF = null;
        protected int[][] adjFF = null;




        protected bool textureExist = false;
        public bool TextureExist
        {
            get
            {
                return textureExist;
            }
        }

        public string TextureFile
        {
            get
            {
                return textureFile;
            }
            set
            {
                textureFile = value;
            }
        }
        protected string textureFile = string.Empty;

		public int VertexCount
		{
			get { return vertexCount; }
            set { vertexCount = value; }
		}
		public int FaceCount
		{
			get { return faceCount; }
            set { faceCount = value; }
		}
        public double[] VertexPos { get { return vertexPos; } set { vertexPos = value; } }
        public double[] VertexNormal { get { return vertexNormal; } set { vertexNormal = value; } }
        public double[] TextextCoordinate { get { return textCoordinate; } set { textCoordinate = value; } }
        public int[] FaceIndex { get { return faceIndex; } set { faceIndex = value; } }
        public int[] FaceTexIndex { get { return faceTexIndex; } }
        public double[] FaceNormal { get { return faceNormal; } set { FaceNormal = value; } }
		public double [] DualVertexPos { get { return dualVertexPos; } }
		public byte [] VertexFlag
		{
			get 
            {
                if (vertexflag == null)
                    vertexflag = new byte[vertexCount];
                return vertexflag; 
            }
		}

        public byte[] FaceFlag
        {
            get
            {
                if (faceFlag == null)
                    faceFlag = new byte[faceCount];
                return faceFlag;
            }
        }


		public bool [] IsBoundary
		{
			get 
            {
                if (isBoundary == null)
                    isBoundary = new bool[vertexCount];
                return isBoundary; 
            }
		}
		public double [] Color
		{
			get 
            {
                if (color == null)
                    color = new double[faceCount * 3];
                return color; 
            }
		}
		public int[][] AdjVV
		{
			get { return adjVV; }
             
		}
		public int [][] AdjVF
		{
			get { return adjVF; }
            
		}
		public int [][] AdjFF
		{
			get { return adjFF; }
		}

        public NonManifoldMesh(string filename)
        {
             string lowcase = filename.ToLower();
             if (lowcase.EndsWith(".obj"))
             {
                 ReadObj(filename);
             }
              
             
             
        }

 

        public NonManifoldMesh()
        {
        }

        

        public void PostInit()
        {
            ScaleToUnitBox();
            MoveToCenter();
            ComputeFaceNormal();
            ComputeVertexNormal();
            this.adjVV = BuildAdjacentMatrix().GetRowIndex();
            this.adjVF = BuildAdjacentMatrixFV().GetColumnIndex();
            this.adjFF = BuildAdjacentMatrixFF().GetRowIndex();
            FindBoundaryVertex();
            ComputeDualPosition();
        }

        public void ReadObj(string filename)
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
                            if (tokens2.Length > 1)
                            {
                                if (tokens2[1].Equals("")) continue;
                                ftexlist.Add(Int32.Parse(tokens2[1]) - 1);
                            }
                        }
                        break;
                    case "vt":
                        for (int i = 1; i < tokens.Length; i++)
                        {
                            if (tokens[i].Equals("")) continue;
                            vtlist.Add(Double.Parse(tokens[i]));
                        }
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


       

     
		public void Writeobj(string filename)
		{
            StreamWriter sw = new StreamWriter(filename);
			for (int i=0,j=0; i<vertexCount; i++,j+=3)
			{
				sw.Write("v ");
				sw.Write(vertexPos[j].ToString() + " ");
				sw.Write(vertexPos[j+1].ToString() + " ");
				sw.Write(vertexPos[j+2].ToString());
				sw.WriteLine();
			}

			for (int i=0,j=0; i<faceCount; i++,j+=3)
			{
				sw.Write("f ");
				sw.Write((faceIndex[j]+1).ToString() + " ");
				sw.Write((faceIndex[j+1]+1).ToString() + " ");
				sw.Write((faceIndex[j+2]+1).ToString());
				sw.WriteLine();
			}
            sw.Close();
		}

        

		public Vector3D MaxCoord()
		{
			Vector3D maxCoord = new Vector3D(double.MinValue, double.MinValue, double.MinValue);
			for (int i=0,j=0; i<vertexCount; i++,j+=3) 
			{
				Vector3D v = new Vector3D(vertexPos, j);
				maxCoord = Vector3D.Max(maxCoord, v);
			}
			return maxCoord;
		}
		public Vector3D MinCoord()
		{
			Vector3D minCoord = new Vector3D(double.MaxValue, double.MaxValue, double.MaxValue);
			for (int i=0,j=0; i<vertexCount; i++,j+=3) 
			{
				Vector3D v = new Vector3D(vertexPos, j);
				minCoord = Vector3D.Min(minCoord, v);
			}
			return minCoord;
		}
		public void MoveToCenter()
		{
			Vector3D center = (MaxCoord() + MinCoord()) / 2.0;

			for (int i=0,j=0; i<vertexCount; i++,j+=3) 
			{
				vertexPos[j] -= center.x;
				vertexPos[j+1] -= center.y;
				vertexPos[j+2] -= center.z;
			}
		}
		public void ScaleToUnitBox()
		{
			Vector3D d = MaxCoord() - MinCoord();
			double s = (d.x>d.y)? d.x:d.y;
			s = (s>d.z)? s: d.z;
			if (s<=0) return;
			for (int i=0; i<vertexPos.Length; i++)
				vertexPos[i] /= s;
		}
		public void ComputeFaceNormal() 
		{
            if (this.faceNormal == null)
            this.faceNormal = new double[faceCount * 3];


			for (int i=0,j=0; i<faceCount; i++,j+=3)
			{
				int c1 = faceIndex[j] * 3;
				int c2 = faceIndex[j+1] * 3;
				int c3 = faceIndex[j+2] * 3;
				Vector3D v1 = new Vector3D(vertexPos, c1);
				Vector3D v2 = new Vector3D(vertexPos, c2);
				Vector3D v3 = new Vector3D(vertexPos, c3);
				Vector3D normal = (v2-v1).Cross(v3-v1).Normalize();
				faceNormal[j] = normal.x;
				faceNormal[j+1] = normal.y;
				faceNormal[j+2] = normal.z;
			}
		}
		public void ComputeVertexNormal() 
		{
            if (this.vertexNormal == null)
            this.vertexNormal = new double[vertexCount * 3];

			Array.Clear(vertexNormal, 0, vertexNormal.Length);
			for (int i=0,j=0; i<faceCount; i++,j+=3) 
			{
				int c1 = faceIndex[j] * 3;
				int c2 = faceIndex[j+1] * 3;
				int c3 = faceIndex[j+2] * 3;
				vertexNormal[c1] += faceNormal[j];
				vertexNormal[c2] += faceNormal[j];
				vertexNormal[c3] += faceNormal[j];
				vertexNormal[c1+1] += faceNormal[j+1];
				vertexNormal[c2+1] += faceNormal[j+1];
				vertexNormal[c3+1] += faceNormal[j+1];
				vertexNormal[c1+2] += faceNormal[j+2];
				vertexNormal[c2+2] += faceNormal[j+2];
				vertexNormal[c3+2] += faceNormal[j+2];
			}
			for (int i=0,j=0; i<vertexCount; i++,j+=3)
			{
				Vector3D n = new Vector3D(vertexNormal, j);
				n = n.Normalize();
				vertexNormal[j] = n.x;
				vertexNormal[j+1] = n.y;
				vertexNormal[j+2] = n.z;
			}
		}

        public void ComputeWeightVertexNormal()
        {
            if (this.vertexNormal == null)
                this.vertexNormal = new double[vertexCount * 3];

            Array.Clear(vertexNormal, 0, vertexNormal.Length);
            for (int i = 0, j = 0; i < faceCount; i++, j += 3)
            {
                double area = ComputeFaceArea(i);
                int c1 = faceIndex[j] * 3;
                int c2 = faceIndex[j + 1] * 3;
                int c3 = faceIndex[j + 2] * 3;
                vertexNormal[c1] += faceNormal[j]*area;
                vertexNormal[c2] += faceNormal[j] * area;
                vertexNormal[c3] += faceNormal[j] * area;
                vertexNormal[c1 + 1] += faceNormal[j + 1] * area;
                vertexNormal[c2 + 1] += faceNormal[j + 1] * area;
                vertexNormal[c3 + 1] += faceNormal[j + 1] * area;
                vertexNormal[c1 + 2] += faceNormal[j + 2] * area;
                vertexNormal[c2 + 2] += faceNormal[j + 2] * area;
                vertexNormal[c3 + 2] += faceNormal[j + 2] * area;
            }
            for (int i = 0, j = 0; i < vertexCount; i++, j += 3)
            {
                Vector3D n = new Vector3D(vertexNormal, j);
                n = n.Normalize();
                vertexNormal[j] = n.x;
                vertexNormal[j + 1] = n.y;
                vertexNormal[j + 2] = n.z;
            }
        }


		public SparseMatrix BuildAdjacentMatrix()
		{
			SparseMatrix m = new SparseMatrix(vertexCount, vertexCount, 6);

			for (int i=0,j=0; i<faceCount; i++,j+=3)
			{
				int c1 = faceIndex[j];
				int c2 = faceIndex[j+1];
				int c3 = faceIndex[j+2];
				m.AddElementIfNotExist(c1, c2, 1.0);
				m.AddElementIfNotExist(c2, c3, 1.0);
				m.AddElementIfNotExist(c3, c1, 1.0);
				m.AddElementIfNotExist(c2, c1, 1.0);
				m.AddElementIfNotExist(c3, c2, 1.0);
				m.AddElementIfNotExist(c1, c3, 1.0);
			}

			m.SortElement();
			return m;
		}
		public SparseMatrix BuildAdjacentMatrixFV()
		{
			SparseMatrix m = new SparseMatrix(faceCount, vertexCount, 6);

			for (int i=0,j=0; i<faceCount; i++,j+=3)
			{
				m.AddElementIfNotExist(i, faceIndex[j], 1.0);
				m.AddElementIfNotExist(i, faceIndex[j+1], 1.0);
				m.AddElementIfNotExist(i, faceIndex[j+2], 1.0);
			}

			m.SortElement();
			return m;
		}
		public SparseMatrix BuildAdjacentMatrixFF()
		{
			SparseMatrix m = new SparseMatrix(faceCount, faceCount, 3);

			for (int i = 0; i < faceCount; i++)
			{
				int v1 = faceIndex[i * 3];
				int v2 = faceIndex[i * 3 + 1];
				int v3 = faceIndex[i * 3 + 2];

				foreach (int j in adjVF[v1])
					if (j != i && IsContainVertex(j, v2))
						m.AddElementIfNotExist(i, j, 1.0);

				foreach (int j in adjVF[v2])
					if (j != i && IsContainVertex(j, v3))
						m.AddElementIfNotExist(i, j, 1.0);

				foreach (int j in adjVF[v3])
					if (j != i && IsContainVertex(j, v1))
						m.AddElementIfNotExist(i, j, 1.0);
			}

			return m;
		}
		public void FindBoundaryVertex()
		{
            if(this.isBoundary==null)
            this.isBoundary = new bool[vertexCount];

			for (int i=0; i<vertexCount; i++)
			{
				int nAdjV = adjVV[i].Length;
				int nAdjF = adjVF[i].Length;
				this.isBoundary[i] = (nAdjV != nAdjF);
			}
		}
		public void GroupingFlags()
		{
            if (vertexflag == null)
            {
                vertexflag = new byte[vertexCount];
            }
			for (int i=0; i<vertexflag.Length; i++)
				if (vertexflag[i] != 0) vertexflag[i] = 255;

			byte id = 0;
			Queue queue = new Queue();
			for (int i=0; i<vertexCount; i++)
				if (vertexflag[i] == 255)
				{
					id++;
					vertexflag[i] = id;
					queue.Enqueue(i);
					while (queue.Count > 0)
					{
						int curr = (int)queue.Dequeue();
						foreach (int j in adjVV[curr])
						{
							if (vertexflag[j] == 255)
							{
								vertexflag[j] = id;
								queue.Enqueue(j);
							}
						}
					}
				}
		}
		public void RemoveVertex(int index)
		{
			RemoveOneVertex(index);

			this.vertexNormal = new double[vertexCount*3];
			this.faceNormal = new double[faceCount*3];
			this.vertexflag = new byte[vertexCount];
			this.isBoundary = new bool[vertexCount];

			ComputeFaceNormal();
			ComputeVertexNormal();
			SparseMatrix adjMatrix = BuildAdjacentMatrix();
			SparseMatrix adjMatrixFV = BuildAdjacentMatrixFV();
			this.adjVV = adjMatrix.GetRowIndex();
			this.adjVF = adjMatrixFV.GetColumnIndex();
			FindBoundaryVertex();
			ComputeDualPosition();
		}

		public void RemoveVertex(ArrayList indice)
		{
			for (int i=0; i<indice.Count; i++)
				RemoveOneVertex(((int)indice[i])-i); 

			ComputeFaceNormal();
			ComputeVertexNormal();
			SparseMatrix adjMatrix = BuildAdjacentMatrix();
			SparseMatrix adjMatrixFV = BuildAdjacentMatrixFV();
			this.adjVV = adjMatrix.GetRowIndex();
			this.adjVF = adjMatrixFV.GetColumnIndex();
			FindBoundaryVertex();
			 
		}
		public bool IsContainVertex(int fIndex, int vIndex)
		{
			int b = fIndex * 3;
			int v1 = faceIndex[b];
			int v2 = faceIndex[b + 1];
			int v3 = faceIndex[b + 2];
			return (v1 == vIndex) || (v2 == vIndex) || (v3 == vIndex);
		}

		public void ComputeDualPosition()
		{
			if (dualVertexPos == null) 
				dualVertexPos = new double[faceCount * 3];

			for (int i=0; i<dualVertexPos.Length; i++)
				dualVertexPos[i] = 0.0;

			for (int i=0,j=0; i<vertexCount; i++,j+=3)
				foreach (int k in adjVF[i])
				{
					int b = k * 3;
					dualVertexPos[b] += vertexPos[j];
					dualVertexPos[b+1] += vertexPos[j+1];
					dualVertexPos[b+2] += vertexPos[j+2];
				}

			for (int i = 0; i < dualVertexPos.Length; i++)
				dualVertexPos[i] /= 3.0;
		}
		public Vector3D GetDualPosition(int fIndex)
		{
			return new Vector3D(dualVertexPos, fIndex * 3);
		}
		public double ComputeFaceArea(int fIndex)
		{
			int b = fIndex * 3;
			Vector3D v1 = new Vector3D(VertexPos, faceIndex[b] * 3);
			Vector3D v2 = new Vector3D(VertexPos, faceIndex[b+1] * 3);
			Vector3D v3 = new Vector3D(VertexPos, faceIndex[b+2] * 3);
			return ((v2-v1).Cross(v3-v1)).Length() / 2.0;
		}
		private void RemoveOneVertex(int index)
		{
			double [] vp = new double[(vertexCount-1)*3];
			for (int i=0,j=0,k=0; i<vertexCount; i++,j+=3) 
			{
				if (i == index) continue;
				vp[k++] = vertexPos[j];
				vp[k++] = vertexPos[j+1];
				vp[k++] = vertexPos[j+2];
			}

			ArrayList flist = new ArrayList(faceCount*3);
			for (int i=0,j=0; i<faceCount; i++,j+=3)
			{
				int c1 = faceIndex[j];
				int c2 = faceIndex[j+1];
				int c3 = faceIndex[j+2];
				if (c1==index || c2==index || c3==index) continue;
				if (c1>index) c1--; flist.Add(c1);
				if (c2>index) c2--; flist.Add(c2);
				if (c3>index) c3--; flist.Add(c3);
			}

			this.vertexCount--;
			this.vertexPos = vp;
			this.faceCount = flist.Count / 3;
			this.faceIndex = new int[flist.Count];

			for (int i=0; i<flist.Count; i++) faceIndex[i] = (int)flist[i];
		}

        

        


        public double[] CreateDualPosition()
        {

            double[] dualVertexPos = new double[faceCount * 3];

            for (int i = 0; i < dualVertexPos.Length; i++)
                dualVertexPos[i] = 0.0;

            for (int i = 0, j = 0; i < vertexCount; i++, j += 3)
                foreach (int k in adjVF[i])
                {
                    int b = k * 3;
                    dualVertexPos[b] += vertexPos[j];
                    dualVertexPos[b + 1] += vertexPos[j + 1];
                    dualVertexPos[b + 2] += vertexPos[j + 2];
                }

            for (int i = 0; i < dualVertexPos.Length; i++)
                dualVertexPos[i] /= 3.0;

            return dualVertexPos;
        }

        public int[] CreateDualFaceIndex()
        {
            int fn=FaceCount;
            int[] dualFaceIndex = new int[fn * 3*3];

            int cur = 0;
            for (int i = 0; i < faceCount; i++)
            {
                int neighbornum = AdjFF[i].Length;


                int f1;
                int f2;
                int f3;
                
                if (neighbornum == 3)
                {
                    f1 = AdjFF[i][0];
                    f2 = AdjFF[i][1];
                    f3 = AdjFF[i][2];
 

                    dualFaceIndex[cur++] = f1;
                    dualFaceIndex[cur++] = f2;
                    dualFaceIndex[cur++] = f3;
                }
                
            }

            return dualFaceIndex;
        }



        #region IDisposable Members

        public void Dispose()
        {
           if(vertexPos!= null)
               vertexPos =null;
		   if(vertexNormal!= null)
               vertexNormal=null;
           if(textCoordinate != null)
               textCoordinate =null;
           if(faceIndex != null)
               faceIndex = null;
           if(faceTexIndex != null)
               faceTexIndex =null;
            if(faceNormal  != null)
               faceNormal  =null;
            if(dualVertexPos!= null)
               dualVertexPos =null;
            if(vertexflag  != null)
                vertexflag = null;
            if (faceFlag != null)
                faceFlag = null;
            if (faceTexIndex != null)
               faceTexIndex =null;
            if(isBoundary != null)
               isBoundary =null;
            if(color   != null)
               color   =null;
            if(adjVV!= null)
               adjVV =null;
            if(adjVF  != null)
               adjVF =null;
            if(adjFF  != null)
               adjFF =null;
		 
        }

        #endregion
    }
}
