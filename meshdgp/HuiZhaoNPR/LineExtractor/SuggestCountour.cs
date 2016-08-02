using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class SuggestCountour : LineBase
    {
        //public override bool[] Extract(TriMesh mesh)
        //{
        //    Vector3D viewPoint = ToolPool.Instance.Tool.ComputeViewPoint();
        //    double thresh1 = ConfigNPR.Instance.SuggestiveContours;
        //    bool[] vFlags = new bool[mesh.Vertices.Count];
        //    bool[] eFlags = new bool[mesh.Edges.Count];

        //    CurvatureLib.Init(mesh);
        //    var curv = CurvatureLib.ComputeCurvature();
        //    var dcurv = CurvatureLib.ComputeCurD();
        //    double[] dknk = new double[mesh.Vertices.Count];
        //    for (int i = 0; i < mesh.Vertices.Count; i++)
        //    {
        //        double dkn = LineCurvature.ComputeDCurv(viewPoint, mesh.Vertices[i], dcurv[i])
        //                   * Math.Pow(Math.Sin(Math.Acos(LineCurvature.ComputeCos(viewPoint, mesh.Vertices[i]))), 2);
        //        dknk[i] = dkn + Math.Abs(LineCurvature.ComputeRadicalCurvature(viewPoint, mesh.Vertices[i], curv[i].min, curv[i].max));
        //        //if (dkn > thresh1 && Math.Abs(ComputeRadicalCurvature(meshNew.Vertices[i], curv[i].min, curv[i].max)) > thresh2)
        //        //{
        //        //    //mesh.Vertices[i].GetEdge(0).Traits.selectedFlag = 1;
        //        //    vertexList.Add(meshNew.Vertices[i]);
        //        //}
        //        if (dknk[i] > thresh1)
        //        {
        //            vFlags[i] = true;
        //        }
        //    }
        //    //标记出所有符合要求的边
        //    for (int i = 0; i < mesh.Edges.Count; i++)
        //    {
        //        TriMesh.Edge edge = mesh.Edges[i];
        //        if (vFlags[edge.Vertex0.Index] && vFlags[edge.Vertex1.Index])
        //        {
        //            eFlags[i] = true;
        //        }
        //    }
        //    return eFlags;
        //}


        ////线条提取，阈值1最好设为300，阈值2最好设为45
        //public override TriMesh Extract2(TriMesh mesh)
        //{
        //    Vector3D viewPoint = ToolPool.Instance.Tool.ComputeViewPoint();
        //    double thresh1 = ConfigNPR.Instance.SuggestiveContours;
        //    //double thresh2=ConfigNPR.Instance.SuggestiveThreshold2;

        //    List<TriMesh.Vertex> vertexList = new List<TriMesh.Vertex>();

        //    //重新构建模型，重建的模型为原模型相对于视点的正面
        //    TriMesh meshNew = LineCurvature.CreateMesh(mesh, viewPoint);

        //    //计算每个点的曲率以及曲率导数，并将符合要求的点放入一个集合
        //    CurvatureLib.Init(meshNew);
        //    PrincipalCurvature[] curv = CurvatureLib.ComputeCurvature();
        //    Vector4D[] dcurv = CurvatureLib.ComputeCurD();
        //    double[] dknk = new double[meshNew.Vertices.Count * 2];
        //    for (int i = 0; i < meshNew.Vertices.Count; i++)
        //    {
        //        double dkn = LineCurvature.ComputeDCurv(viewPoint, meshNew.Vertices[i], dcurv[i])
        //                   * Math.Pow(Math.Sin(Math.Acos(LineCurvature.ComputeCos(viewPoint, meshNew.Vertices[i]))), 2);
        //        dknk[i] = dkn + Math.Abs(LineCurvature.ComputeRadicalCurvature(viewPoint, meshNew.Vertices[i], curv[i].min, curv[i].max));
        //        //if (dkn > thresh1 && Math.Abs(ComputeRadicalCurvature(meshNew.Vertices[i], curv[i].min, curv[i].max)) > thresh2)
        //        //{
        //        //    //mesh.Vertices[i].GetEdge(0).Traits.selectedFlag = 1;
        //        //    vertexList.Add(meshNew.Vertices[i]);
        //        //}
        //        if (dknk[i] > thresh1)
        //        {
        //            vertexList.Add(meshNew.Vertices[i]);
        //        }
        //    }


        //    //标记出所有符合要求的边
        //    for (int i = 0; i < meshNew.Edges.Count; i++)
        //    {
        //        if (vertexList.Contains(meshNew.Edges[i].Vertex0) && vertexList.Contains(meshNew.Edges[i].Vertex1))
        //        {
        //            meshNew.Edges[i].Traits.selectedFlag = 1;
        //        }
        //    }
        //    return meshNew;
        //}

        public SuggestCountour()
        {
            this.Type = EnumLine.SuggestCountour;
        }
    }
}
