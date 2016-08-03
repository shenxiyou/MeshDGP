using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    /// <summary>
    /// 基于离散曲率的三角形折叠简化算法_石 坚
    /// </summary>
    public class SurfaceFittingSimplication : MergeTriangleSimplicationBase
    {
        public SurfaceFittingSimplication(TriMesh mesh)
            : base(mesh)
        {

        }

        protected override double GetValue(HalfEdgeMesh.Face target)
        {
            TriMesh.Vertex min = this.GetMinCvtVertex(target);
            return Math.Abs(this.traits.VertexDiscreteCurvature[min.Index]);
        }

        TriMesh.Vertex GetMinCvtVertex(TriMesh.Face face)
        {
            double minCvt = double.MaxValue;
            TriMesh.Vertex min = null;
            foreach (var v in face.Vertices)
            {
                double cvt = Math.Abs(this.traits.VertexDiscreteCurvature[v.Index]);
                if (cvt < minCvt)
                {
                    minCvt = cvt;
                    min = v;
                }
            }
            return min;
        }

        //protected override Vector3D GetPos(TriMesh.Face face)
        //{
        //    TriMesh.Vertex min = this.GetMinCvtVertex(face);
        //    double cvt = this.traits.VertexDiscreteCurvature[min.Index];
        //    double r = Math.Abs(1d / cvt);
        //    Vector3D normal = this.traits.FaceNormal[face.Index];

        //    Vector3D v0 = face.HalfEdge.FromVertex.Traits.Position;
        //    Vector3D v1 = face.HalfEdge.ToVertex.Traits.Position;
        //    Vector3D v2 = face.HalfEdge.Next.ToVertex.Traits.Position;

        //    Matrix4D m = new Matrix4D();
        //    m.Row1 = new Vector4D(v0 - v1, (v0 + v1).Dot(v0 - v1) / 2d);
        //    m.Row2 = new Vector4D(v1 - v2, (v1 + v2).Dot(v1 - v2) / 2d);
        //    m.Row3 = new Vector4D(v2 - v0, (v2 + v0).Dot(v2 - v0) / 2d);
        //    m.Row4 = new Vector4D(0d, 0d, 0d, 1d);
        //    //外心
        //    Vector3D outer = Vector3D.Zero;
        //    double det = Util.Solve(ref m, ref outer);
        //    Vector3D pos = Vector3D.Zero;
        //    if (det != 0)
        //    {
        //        double d1 = Vector3D.DistanceSquared(min.Traits.Position, outer);
        //        double d2 = Math.Sqrt(r * r - d1);
        //        //球心
        //        Vector3D c = outer - d2 * normal;
        //        //球面上
        //        pos = c + r * normal;
        //    }
        //    else
        //    {
        //        pos = TriMeshUtil.GetMidPoint(face);
        //    }

        //    return pos;
        //}
    }
}
