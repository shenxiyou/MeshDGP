//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace GraphicResearchHuiZhao
//{
//    public class DemarcatingCurve : LineBase
//    {  

//        public DemarcatingCurve(TriMesh mesh)
//            : base(mesh)
//        {
//        }
         
//        public override TriMesh Extract()
//        {
//            double threshold=ConfigNPR.Instance.Demarcatingthreshold; 

//            List<TriMesh.Vertex> vertexList = new List<TriMesh.Vertex>();
           
//            CurvatureLib.Init(mesh);
//            PrincipalCurvature[] curv = CurvatureLib.ComputeCurvature();
//            Vector4D[] dcurv = CurvatureLib.ComputeCurD(); 

//            double[] dknk = new double[mesh.Vertices.Count * 2];
//            for (int i = 0; i < mesh.Vertices.Count; i++)
//            {
//                dknk[i] =LineCurvature.ComputeDkn(mesh.Vertices[i], dcurv[i], curv[i].maxDir);
//                if (dknk[i] > threshold)
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
 
        

       

//        //
       
    
    
//    }
//}
