using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public partial class TriMeshSimplicationCluster
    {
        private TriMesh mesh;
        public TriMesh Mesh
        {
            set
            {
                mesh = value;
            }
            get
            {
                return mesh;
            }
        } 

        public TriMeshSimplicationCluster(TriMesh mesh)
        {
            this.mesh = mesh;
        } 
      

       

        double cubeWeight = 0; 
        private double startX;
        private double startY;
        private double startZ;

        private int Xcount;
        private int Ycount;
        private int Zcount;
  

        #region Process Control

        public void ClusterProcess()
        {
            for (int x = 0; x < Xcount; x++)
            {
                for (int y = 0; y < Ycount; y++)
                {
                    for (int z = 0; z < Zcount; z++)
                    {
                        TriMesh.Vertex[] group = FindVerticesInRange(x * cubeWeight + startX, 
                            y * cubeWeight + startY, z * cubeWeight + startZ);
                        if (group.Length != 0)
                        {
                            Vector3D avg = AvgOfCluster(group);
                            ClusterVertexs(group, avg);
                        }
                    }
                }
            }

            TriMeshUtil.FixIndex(mesh);
            
        }


        #endregion

        #region Basic Methods

        public void ClusterVertexs(TriMesh.Vertex[] vertices, 
                                   Vector3D position)
        {
            Queue<TriMesh.Edge> connectedEdge = 
                                 new Queue<TriMesh.Edge>();
            Dictionary<TriMesh.Edge, bool> processedFlag =
                new Dictionary<HalfEdgeMesh.Edge, bool>();
            foreach (TriMesh.Vertex v in vertices)
            {
                foreach (TriMesh.HalfEdge hf in v.HalfEdges)
                {
                    foreach (TriMesh.Vertex vx in vertices)
                    {
                        if (hf.ToVertex == vx && 
                            !processedFlag.ContainsKey(hf.Edge))
                        {
                            connectedEdge.Enqueue(hf.Edge);
                            processedFlag[hf.Edge] = true;
                        }
                    }
                }
            }
            int n = 0;
            while (connectedEdge.Count > 0)
            {
                TriMesh.Edge e = connectedEdge.Dequeue();
                if (e.HalfEdge0 == null || e.HalfEdge1 == null)
                    continue;
                if (TriMeshModify.IsMergeable(e))
                {
                    TriMeshModify.MergeEdge(e, position);
                    n = 0;
                }
                else
                {
                    connectedEdge.Enqueue(e);
                    n++;
                    if (n > connectedEdge.Count)
                    {
                        break;
                    };
                }
            }
        }

        public TriMesh.Vertex[] FindVerticesInRange(double startX, double startY, double startZ)
        {
            double endX = startX + cubeWeight;
            double endY = startY + cubeWeight;
            double endZ = startZ + cubeWeight;
            List<TriMesh.Vertex> group = new List<TriMesh.Vertex>();

            foreach (TriMesh.Vertex item in mesh.Vertices)
            {
                Vector3D pos = item.Traits.Position;
                if (pos.x >= startX && pos.x < endX &&
                    pos.y >= startY && pos.y < endY &&
                    pos.z >= startZ && pos.z < endZ
                    )
                {
                    group.Add(item);
                }
            }

            return group.ToArray();
        }

        public Vector3D AvgOfCluster(TriMesh.Vertex[] vertices)
        {
            Vector3D avg = new Vector3D();

            foreach (TriMesh.Vertex item in vertices)
            {
                Vector3D pos = item.Traits.Position;
                avg += pos;
            }

            avg /= vertices.Length;

            return avg;
        }

        public void BuildCubes(int lessCount)
        {
            //Analize obj
            double[] x = new double[2];
            double[] y = new double[2];
            double[] z = new double[2];

            x[0] = double.MaxValue; x[1] = double.MinValue;
            y[0] = double.MaxValue; y[1] = double.MinValue;
            z[0] = double.MaxValue; z[1] = double.MinValue;

            foreach (TriMesh.Vertex item in mesh.Vertices)
            {
                Vector3D position = item.Traits.Position;
                if (position.x < x[0]) x[0] = position.x;
                if (position.x > x[1]) x[1] = position.x;
                if (position.y < y[0]) y[0] = position.y;
                if (position.y > y[1]) y[1] = position.y;
                if (position.z < z[0]) z[0] = position.z;
                if (position.z > z[1]) z[1] = position.z;
            }

            double xRange = x[1] - x[0];
            double yRange = y[1] - y[0];
            double zRange = z[1] - z[0];

            startX = x[0];
            startY = y[0];
            startZ = z[0];

            double minRange = Math.Min(xRange, Math.Min(yRange, zRange));
            cubeWeight = minRange / lessCount;

            Xcount = (int)Math.Ceiling(xRange / cubeWeight);
            Ycount = (int)Math.Ceiling(yRange / cubeWeight);
            Zcount = (int)Math.Ceiling(zRange / cubeWeight);
        }
        #endregion

       
    }
}
