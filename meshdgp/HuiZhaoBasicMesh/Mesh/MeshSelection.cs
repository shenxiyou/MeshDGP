using System;
using System.Collections;
using System.IO;

namespace GraphicResearchHuiZhao
{
	public class MeshSelection
	{
		byte [] data = null;

		public int Size
		{
			get { return Data.Length; }
		}
		public byte [] Data
		{
			get { return data; }
		}


		public MeshSelection(int size)
		{
			this.data = new byte[size];
		}
		public MeshSelection(NonManifoldMesh m)
		{
			this.data = (byte[])m.VertexFlag.Clone();
		}
		public MeshSelection(StreamReader sr)
		{
			int n = Int32.Parse(sr.ReadLine());
			this.data = new byte[n];
			
			for (int i=0; i<n; i++)
			{
				this.data[i] = Byte.Parse(sr.ReadLine());
			}
		}

		public void Write(StreamWriter sw)
		{
			int n = data.Length;
			sw.WriteLine(n.ToString());

			for (int i=0; i<n; i++)
				sw.WriteLine(data[i].ToString());
		}
		public void Apply(NonManifoldMesh m)
		{
			this.data.CopyTo(m.VertexFlag, 0);
		}
	}
}
