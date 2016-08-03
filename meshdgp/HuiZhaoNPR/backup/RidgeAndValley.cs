//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace GraphicResearchHuiZhao
//{
//    public class RidgeAndValley : LineBase
//    { 
//        public RidgeAndValley(TriMesh mesh): base(mesh)
//        {
//        }   

//        //通过主曲率的值来提取合适的线条,阈值1最好设为35，阈值2最好设为-7

//        public override TriMesh Extract()
//        {
//            double thresh1=ConfigNPR.Instance.RidgeValleythreshold1;
//            double thresh2 = ConfigNPR.Instance.RidgeValleythreshold2;
//            List<TriMesh.Vertex> vertexList = new List<TriMesh.Vertex>();

//            CurvatureLib.Init(mesh);
//            PrincipalCurvature[] curv = CurvatureLib.ComputeCurvature(); 
//            double[] dknk = new double[mesh.Vertices.Count * 2];
//            for (int i = 0; i < mesh.Vertices.Count; i++)
//            {
//                dknk[i] = curv[i].max + curv[i].min;
//                if (curv[i].max > thresh1 || curv[i].min < thresh2)
//                {
//                    vertexList.Add(mesh.Vertices[i]);
//                }
//            } 
//            for (int i = 0; i < mesh.Edges.Count; i++)
//            {
//                if (vertexList.Contains(mesh.Edges[i].Vertex0) && vertexList.Contains(mesh.Edges[i].Vertex1))
//                {
//                    vertexList.Remove(mesh.Edges[i].Vertex0);
//                    vertexList.Remove(mesh.Edges[i].Vertex1);
//                    mesh.Edges[i].Traits.selectedFlag = 1;
//                }
//            } 
//            return mesh;
//        }

        
         


//    }
//}
