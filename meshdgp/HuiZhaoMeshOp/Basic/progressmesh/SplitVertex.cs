using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public partial class TriMeshModify 
    {
        public static void Split(TriMesh mesh, EdgeContext ctx)
        {
            TriMesh.Vertex left = null;
            TriMesh.Vertex top = null;
            TriMesh.Vertex bottom = null;

            foreach (var v in mesh.Vertices)
            {
                if (v.Index == ctx.Left)
                {
                    left = v;
                }
                else if (v.Index == ctx.Top)
                {
                    top = v;
                }
                else if (v.Index == ctx.Bottom)
                {
                    bottom = v;
                }
            }

            TriMesh.Vertex right = TriMeshModify.VertexSplit(left, top, bottom, ctx.LeftPos, ctx.RightPos);
            TriMesh.HalfEdge hf = left.FindHalfedgeTo(right);
            right.Index = ctx.Right;
            hf.Next.ToVertex.Index = ctx.Top;
            hf.Opposite.Next.ToVertex.Index = ctx.Bottom;
        }
    }
}
