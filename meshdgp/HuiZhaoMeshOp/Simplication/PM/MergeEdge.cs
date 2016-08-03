using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public static class MergeEdge
    {
        public static EdgeContext Merge(TriMesh.Edge edge)
        {
            Vector3D position = TriMeshUtil.GetMidPoint(edge);
            return Merge(edge, position);
        }

        public static EdgeContext Merge(TriMesh.Edge edge, Vector3D position)
        {
            TriMesh mesh = (TriMesh)edge.Mesh;

            EdgeContext context = new EdgeContext()
            {
                LeftPos = edge.Vertex0.Traits.Position,
                MidPos = position,
                RightPos = edge.Vertex1.Traits.Position,
                Left = edge.Vertex0.Index,
                Right = edge.Vertex1.Index,
                Top = edge.HalfEdge1.Next.ToVertex.Index,
                Bottom = edge.HalfEdge0.Next.ToVertex.Index,
            };

            TriMeshModify.MergeEdge(edge);

            return context;
        }

        ///// <summary>
        ///// 保留Vertex0
        ///// </summary>
        ///// <param name="edge"></param>
        ///// <param name="position"></param>
        ///// <returns></returns>
        //public static EdgeContext Merge1(TriMesh.Edge edge, Vector3D position)
        //{
        //    TriMesh mesh = (TriMesh)edge.Mesh;
        //    TriMesh.Vertex v0 = edge.Vertex0;
        //    TriMesh.Vertex v1 = edge.Vertex1;
        //    TriMesh.HalfEdge hf0 = edge.HalfEdge0;
        //    TriMesh.HalfEdge hf1 = edge.HalfEdge1;

        //    EdgeContext context = new EdgeContext()
        //    {
        //        LeftPos = v0.Traits.Position,
        //        MidPos = position,
        //        Left = v0,
        //        Right = v1,
        //        Top = hf1.Next.ToVertex,
        //        Buttom = hf0.Next.ToVertex,
        //        MidEdge = edge,
        //        TopLeftEdge = hf1.Previous.Edge,
        //        AboveFace = edge.Face0,
        //        ButtomRightEdge = hf0.Previous.Edge,
        //        UnderFace = edge.Face1
        //    };

        //    if (context.Top == context.Buttom)
        //    {
        //        context.Top.Traits.selectedFlag = 1;
        //    }

        //    v0.Traits.Position = position;

        //    foreach (var item in v1.Halfedges)
        //    {
        //        //if (item.ToVertex != v0)
        //        {
        //            item.Opposite.ToVertex = v0;
        //        }
        //    }

        //    MergeOneSide(hf0);
        //    MergeOneSide(hf1);

        //    mesh.RemoveVertex(v1);
        //    mesh.RemoveEdge(edge);
        //    //v1.HalfEdge = null;
        //    //edge.HalfEdge0 = null;

        //    return context;
        //}
    }
}
