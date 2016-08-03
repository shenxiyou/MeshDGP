using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class Silluhoute:LineBase
    {
        //Vector3D viewPoint;

        ////public override bool[] Extract(TriMesh mesh)
        ////{
        ////    viewPoint = ToolPool.Instance.Tool.ComputeViewPoint();
        ////    bool[] flags = new bool[mesh.Edges.Count];
        ////    for (int i = 0; i < mesh.Edges.Count; i++)
        ////    {
        ////        if (ComputeValueOuter(mesh.Edges[i]) <= ConfigNPR.Instance.Contourthreshold
        ////            && ComputeValueInner(mesh.Edges[i]) <= ConfigNPR.Instance.Silluhoutethreshold)
        ////        {
        ////            flags[i] = true;
        ////        }

        ////        double angle = Math.PI - TriMeshUtil.ComputeDihedralAngle(mesh.Edges[i]);
        ////        if (angle < ConfigNPR.Instance.SilhouetteAngle)
        ////        {
        ////            flags[i] = true;
        ////        }
        ////    }
        ////    return flags;
        ////}

        ////计算视线向量与面的法向量的夹角
        //public double ComputeValueOuter(TriMesh.Edge edge)
        //{
        //    //考虑到边界边的情况
        //    if (edge.Face0 == null || edge.Face1 == null)
        //    {
        //        return 0;
        //    }
        //    //正常情况
        //    Vector3D normal = TriMeshUtil.ComputeNormalFace(edge.Face0);
        //    Vector3D dir = edge.Vertex0.Traits.Position - viewPoint;
        //    double value1 = dir.Dot(normal);
        //    normal = TriMeshUtil.ComputeNormalFace(edge.Face1);
        //    double value2 = dir.Dot(normal);
        //    return value1 * value2;
        //}

       

        //public double ComputeValueInner(TriMesh.Edge edge)
        //{
        //    //考虑到边界边的情况
        //    if (edge.Face0 == null || edge.Face1 == null)
        //    {
        //        return 0;
        //    }
        //    //正常情况
        //    Vector3D normal = TriMeshUtil.ComputeNormalFace(edge.Face0);
        //    Vector3D dir = edge.Vertex1.Traits.Position - viewPoint;
        //    double value1 = dir.Dot(normal);
        //    normal = TriMeshUtil.ComputeNormalFace(edge.Face1);
        //    double value2 = dir.Dot(normal);
        //    return value1 * value2;
        //}

        //public double ComputeValue4(TriMesh.Edge edge)
        //{   
        //    //考虑到边界边的情况
        //    if (edge.Face0 == null || edge.Face1 == null)
        //    {
        //        return 0;
        //    }
        //    Vector3D normal = TriMeshUtil.ComputeNormalUniformWeight(edge.Vertex0);
        //    Vector3D dir = edge.Vertex0.Traits.Position - viewPoint;
        //    double value3 = dir.Dot(normal);
        //    normal = TriMeshUtil.ComputeNormalUniformWeight(edge.Vertex1);
        //    dir = edge.Vertex1.Traits.Position - viewPoint;
        //    double value4 = dir.Dot(normal);
        //    return value3 * value4;
        //}

        //public double ComputeValue2(TriMesh.Edge edge)
        //{
        //    //考虑到边界边的情况
        //    if (edge.Face0 == null || edge.Face1 == null)
        //    {
        //        return 0;
        //    }

        //    Vector3D normal = TriMeshUtil.ComputeNormalFace(edge.Face0);
        //    double value1 = viewPoint.Dot(normal);
        //    normal = TriMeshUtil.ComputeNormalFace(edge.Face1);
        //    double value2 = viewPoint.Dot(normal);
        //    return value1 * value2;
        //}

        public Silluhoute()
        {
            this.Type = EnumLine.Silluhoute;
        }
    }
}
