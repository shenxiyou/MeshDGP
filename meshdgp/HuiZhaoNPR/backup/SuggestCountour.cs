//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace GraphicResearchHuiZhao
//{
//    public class SuggestCountour : LineBase
//    {  
      

//        public SuggestCountour(TriMesh mesh)
//            : base(mesh)
//        {
//             viewPoint = ComputeViewPoint( );
//        }
//        TriMesh meshNew = new TriMesh();
//        //线条提取，阈值1最好设为300，阈值2最好设为45
//        public override TriMesh  Extract()
//        {
//            double thresh1=ConfigNPR.Instance.SuggestiveThreshold1;
//            double thresh2=ConfigNPR.Instance.SuggestiveThreshold2;

//            List<TriMesh.Vertex> vertexList = new List<TriMesh.Vertex>();

//            //重新构建模型，重建的模型为原模型相对于视点的正面
//            meshNew = LineCurvature.CreateMesh(mesh,viewPoint);

//            //计算每个点的曲率以及曲率导数，并将符合要求的点放入一个集合
//            CurvatureLib.Init(meshNew);
//            PrincipalCurvature[] curv = CurvatureLib.ComputeCurvature();
//            Vector4D[] dcurv = CurvatureLib.ComputeCurD();
//            double[] dknk = new double[meshNew.Vertices.Count * 2];
//            for (int i = 0; i < meshNew.Vertices.Count; i++)
//            {
//                double dkn =LineCurvature.ComputeDCurv(viewPoint,meshNew.Vertices[i], dcurv[i])
//                           * Math.Pow(Math.Sin(Math.Acos(LineCurvature.ComputeCos(viewPoint,meshNew.Vertices[i]))), 2);
//                dknk[i] = dkn + Math.Abs(LineCurvature.ComputeRadicalCurvature(viewPoint,meshNew.Vertices[i], curv[i].min, curv[i].max));
//                //if (dkn > thresh1 && Math.Abs(ComputeRadicalCurvature(meshNew.Vertices[i], curv[i].min, curv[i].max)) > thresh2)
//                //{
//                //    //mesh.Vertices[i].GetEdge(0).Traits.selectedFlag = 1;
//                //    vertexList.Add(meshNew.Vertices[i]);
//                //}
//                if (dknk[i] > thresh1)
//                {
//                    vertexList.Add(meshNew.Vertices[i]);
//                }
//            } 


//            //标记出所有符合要求的边
//            for (int i = 0; i < meshNew.Edges.Count; i++)
//            {
//                if (vertexList.Contains(meshNew.Edges[i].Vertex0) && vertexList.Contains(meshNew.Edges[i].Vertex1))
//                {
//                    meshNew.Edges[i].Traits.selectedFlag = 1;
//                }
//            }
//            return meshNew;
//        }

       
 

        
         

        
         








//    }
//}
