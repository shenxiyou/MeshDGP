using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class QEMProgressive : QEMSimplication
    {
        public List<EdgeContext> Logs = new List<EdgeContext>();

        public QEMProgressive(TriMesh mesh)
            : base(mesh)
        {

        }

        protected override HalfEdgeMesh.Vertex Merge(MergeSimplicationBase<HalfEdgeMesh.Edge>.MergeArgs args)
        {
            EdgeContext ctx = MergeEdge.Merge(args.Target, args.Pos);
            this.Logs.Add(ctx);

            TriMesh.Vertex left = null;
            foreach (var v in this.Mesh.Vertices)
            {
                if (v.Index == ctx.Left)
                {
                    left = v;
                }
            }
            return left;
        }
    }
}
