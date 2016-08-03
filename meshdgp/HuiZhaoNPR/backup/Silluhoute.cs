//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace GraphicResearchHuiZhao
//{
//    public class Silluhoute:LineBase
//    {  
//        public Silluhoute(TriMesh mesh)
//            : base(mesh)
//        {
//            viewPoint = ComputeViewPoint( );
//        }

//        //线条提取
//        public override TriMesh Extract()
//        {
//            for (int i = 0; i < mesh.Edges.Count; i++)
//            {
//                if (ComputeValueOuter(mesh.Edges[i]) <= ConfigNPR.Instance.Contourthreshold 
//                    && ComputeValueInner(mesh.Edges[i]) <= ConfigNPR.Instance.Silluhoutethreshold)
//                {
//                    mesh.Edges[i].Traits.selectedFlag = 1;
//                }

//                double angle = TriMeshUtil.ComputeAngle(mesh.Edges[i].HalfEdge0);
//                if (angle > 3)
//                {
//                   mesh.Edges[i].Traits.selectedFlag = 1;
//                }
//            }
//            return mesh;
//        }

       

//        //计算视线向量与面的法向量的夹角
//        public double ComputeValueOuter(TriMesh.Edge edge)
//        { 
//            //考虑到边界边的情况
//            if (edge.Face0 == null || edge.Face1 == null)
//            {
//                return 0;
//            }
//            //正常情况
//            Vector3D normal = TriMeshUtil.ComputeNormalFace(edge.Face0);
//            Vector3D dir=edge.Vertex0.Traits.Position-viewPoint;
//            double value1 = dir.Dot(normal);  
//            normal = TriMeshUtil.ComputeNormalFace(edge.Face1);
//            double value2 = dir.Dot(normal);
//            return value1 * value2;
//        }

       

//        public double ComputeValueInner(TriMesh.Edge edge)
//        {
//            //考虑到边界边的情况
//            if (edge.Face0 == null || edge.Face1 == null)
//            {
//                return 0;
//            }
//            //正常情况
//            Vector3D normal = TriMeshUtil.ComputeNormalFace(edge.Face0);
//            Vector3D dir = edge.Vertex1.Traits.Position - viewPoint;
//            double value1 = dir.Dot(normal);
//            normal = TriMeshUtil.ComputeNormalFace(edge.Face1);
//            double value2 = dir.Dot(normal);
//            return value1 * value2;
//        }

//        public double ComputeValue4(TriMesh.Edge edge)
//        {   
//            //考虑到边界边的情况
//            if (edge.Face0 == null || edge.Face1 == null)
//            {
//                return 0;
//            }
//            Vector3D normal = TriMeshUtil.ComputeNormalUniformWeight(edge.Vertex0);
//            Vector3D dir = edge.Vertex0.Traits.Position - viewPoint;
//            double value3 = dir.Dot(normal);
//            normal = TriMeshUtil.ComputeNormalUniformWeight(edge.Vertex1);
//            dir = edge.Vertex1.Traits.Position - viewPoint;
//            double value4 = dir.Dot(normal);
//            return value3 * value4;
//        }

//        public double ComputeValue2(TriMesh.Edge edge)
//        {
//            //考虑到边界边的情况
//            if (edge.Face0 == null || edge.Face1 == null)
//            {
//                return 0;
//            }

//            Vector3D normal = TriMeshUtil.ComputeNormalFace(edge.Face0);
//            double value1 = viewPoint.Dot(normal);
//            normal = TriMeshUtil.ComputeNormalFace(edge.Face1);
//            double value2 = viewPoint.Dot(normal);
//            return value1 * value2;
//        }

//    }
//}
