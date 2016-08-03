using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class DistanceDijkstra 
    {
        public TriMesh mesh=null; 

        public DistanceDijkstra(TriMesh mesh) 
        {
            this.mesh = mesh;  
        } 

        public double Run()
        {  
           List<TriMesh.Vertex> two = TriMeshUtil.RetrieveTwoVertexMust(mesh); 
           TriMesh.Vertex source = two[0];
           TriMesh.Vertex destination = two[1]; 
           double minDistance = Dijkstra(source, destination);  
           return minDistance;
        } 
        
        
        public double  Dijkstra(TriMesh.Vertex source, TriMesh.Vertex destination)
        {
            List<TriMesh.Vertex> S = new List<TriMesh.Vertex>();//以求出最短路径的点的集合
            List<TriMesh.Vertex> T = new List<TriMesh.Vertex>();//未求出最短路径的点的集合


            double minDistance=0;
            PathD[] minPath = new PathD[mesh.Vertices.Count];//每个点的最短路径过程
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                minPath[i] = new PathD();
            }

            #region STEP1 初始化两组点的集合
            S.Add(source);
            minPath[source.Index].path=0;
            minPath[source.Index].vertex = source;
            minPath[source.Index].pathNumber.Add(source.Index);
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                if (mesh.Vertices[i] != source)
                {
                    T.Add(mesh.Vertices[i]);
                    minPath[mesh.Vertices[i].Index].path = double.MaxValue;
                    minPath[mesh.Vertices[i].Index].vertex = mesh.Vertices[i]; 
                }
            }
            int count = 0;
            foreach (TriMesh.Vertex vertex in source.Vertices)
            {
                if (vertex == destination)
                { 
                    return minDistance =TriMeshUtil.ComputeEdgeLength(source.FindEdgeTo(vertex));
                }
                count++;
                if (count == source.VertexCount)
                {
                    minDistance = double.MaxValue;
                }
                
            }
            #endregion

            #region STEP2 遍历所有点，计算source到destination的最短路径的点的集合
            while (true)
            {
                for (int i = 0; i < S.Count; i++)
                {
                    foreach (TriMesh.Vertex v in S[i].Vertices)
                    {
                        for (int j = 0; j < T.Count; j++)
                        {
                            if (T[j] == v)
                            {
                                if (S.Contains(T[j]) == true)
                                    continue;
                                if (TriMeshUtil.ComputeEdgeLength(S[i].FindEdgeTo(T[j])) < minPath[T[j].Index].path)
                                {
                                    minPath[T[j].Index].path = TriMeshUtil.ComputeEdgeLength(S[i].FindEdgeTo(T[j]));
                                    
                                    minPath[T[j].Index].pathNumber.Clear();
                                    for (int k = 0; k < minPath[S[i].Index].pathNumber.Count; k++)
                                    {
                                        minPath[T[j].Index].pathNumber.Add(minPath[S[i].Index].pathNumber[k]);
                                    }
                                    minPath[T[j].Index].pathNumber.Add(T[j].Index);
                                    S.Add(T[j]);
                                   
                                }
                            }
                        }
                    }
                }


                if (S.Count == mesh.Vertices.Count)
                    break;
            }
            #endregion

            #region 计算出最短路径以及为最短路径染色
            minDistance = 0;
            for (int i = 0; i < minPath[destination.Index].pathNumber.Count-1; i++)
            {
                minDistance += TriMeshUtil.ComputeEdgeLength(mesh.Vertices[minPath[destination.Index].pathNumber[i]].FindEdgeTo(
                    mesh.Vertices[minPath[destination.Index].pathNumber[i + 1]]));
            }
            for (int i = 0; i < minPath[destination.Index].pathNumber.Count-1; i++)
            {
              
                mesh.Vertices[minPath[destination.Index].pathNumber[i]].FindEdgeTo(mesh.Vertices[minPath[destination.Index].pathNumber[i + 1]]).Traits.SelectedFlag = 1;
            }
            #endregion

            return minDistance;
        }


       
    }
}
