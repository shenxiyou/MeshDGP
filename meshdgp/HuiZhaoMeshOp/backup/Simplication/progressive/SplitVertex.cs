using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class SplitVertex
    {
        public void Split(EdgeContext ctx)
        {
            TriMesh.HalfEdge[] hfs = this.FindGroup(ctx.Left, ctx.Top, ctx.Buttom);

            TriMesh mesh = (TriMesh)ctx.Left.Mesh;
            ctx.Left.Traits.Position = ctx.LeftPos;
            
            ctx.Left.HalfEdge = hfs[0];

            ctx.Right.HalfEdge = hfs[1];
            mesh.Add(ctx.Right);

            for (int i = 0; i < hfs.Length - 1; i++)
            {
                hfs[i].Opposite.ToVertex = ctx.Right;
            }

            TriMesh.HalfEdge[] triangle1 = this.AddInnerTriangle(mesh, ctx.AboveFace, ctx.Left, ctx.Right, ctx.Top);
            this.InsertEdge(mesh, ctx.TopLeftEdge, triangle1[1], hfs[0]);

            TriMesh.HalfEdge[] triangle2 = this.AddInnerTriangle(mesh, ctx.UnderFace, ctx.Right, ctx.Left, ctx.Buttom);
            this.InsertEdge(mesh, ctx.ButtomRightEdge, triangle2[1], hfs[hfs.Length - 1]);

            ctx.MidEdge.HalfEdge0 = triangle2[0];
            triangle1[0].Edge = ctx.MidEdge;
            triangle2[0].Edge = ctx.MidEdge;
            triangle1[0].Opposite = triangle2[0];
            triangle2[0].Opposite = triangle1[0];
            mesh.Add(ctx.MidEdge);
        }

        private TriMesh.HalfEdge[] FindGroup(TriMesh.Vertex v, TriMesh.Vertex begin, TriMesh.Vertex end)
        {
            List<TriMesh.HalfEdge> group = new List<TriMesh.HalfEdge>();

            TriMesh.HalfEdge start = v.FindHalfedgeTo(begin);
            group.Add(start);
            TriMesh.HalfEdge current = start;

            while (current.ToVertex != end)
            {
                current = current.Opposite.Next;
                group.Add(current);
            }

            return group.ToArray();
        }

        private void ConnectHalfEdge(params TriMesh.HalfEdge[] hfs)
        {
            for (int i = 0; i < hfs.Length; i++)
            {
                hfs[i].Next = hfs[(i + 1) % 3];
                hfs[i].Previous = hfs[(i + 2) % 3];
            }
        }

        private TriMesh.HalfEdge[] AddInnerTriangle(TriMesh mesh, TriMesh.Face face, params TriMesh.Vertex[] verteces)
        {
            mesh.Add(face);

            TriMesh.HalfEdge[] hfs = new TriMesh.HalfEdge[3];
            for (int i = 0; i < hfs.Length; i++)
            {
                hfs[i] = new TriMesh.HalfEdge();
                hfs[i].ToVertex = verteces[(i + 1) % hfs.Length];
                hfs[i].Face = face;
                mesh.AppendToHalfedgeList(hfs[i]);
            }
            face.HalfEdge = hfs[0];
            this.ConnectHalfEdge(hfs);
            return hfs;
        }

        private void InsertEdge(TriMesh mesh, TriMesh.Edge edge, TriMesh.HalfEdge inner, TriMesh.HalfEdge outer)
        {
            TriMesh.Edge left = edge;
            left.HalfEdge0 = outer;
            outer.Edge = left;
            inner.Next.Edge = left;

            TriMesh.Edge right = outer.Edge;
            right.HalfEdge0 = inner;
            inner.Edge = right;
            outer.Opposite.Edge = right;

            inner.Opposite = outer.Opposite;
            inner.Next.Opposite = outer;
            outer.Opposite.Opposite = inner;
            outer.Opposite = inner.Next;

            mesh.Add(edge);
        }
    }
}
