using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class ProgressiveMesh
    {
        MergeEdgeSimplicationBase method;
        int index;

        public TriMesh Mesh { get { return this.method.Mesh; } }

        public ProgressiveMesh(MergeEdgeSimplicationBase method)
        {
            this.method = method;
        }

        public void Run(int preserveFace)
        {
            if (preserveFace > this.Mesh.Faces.Count)
            {
                this.Backward(preserveFace);
            }
            else
            {
                this.Forward(preserveFace);
            }
        }

        public void Forward(int preserveFace)
        {
            while (preserveFace < this.Mesh.Faces.Count)
            {
                this.Forward();
            }
        }

        public void Backward(int preserveFace)
        {
            while (preserveFace > this.Mesh.Faces.Count && this.Backward()) ;
        }

        public bool Forward()
        {
            if (this.index < this.method.Logs.Count)
            {
                EdgeContext ctx = this.method.Logs[this.index];
                TriMeshModify.Merge(ctx.MidEdge, ctx.MidPos);
                index++;
                return true;
            }
            else
            {
                int r = this.method.Run(this.Mesh.Faces.Count - 1);
                index += r;
                return false;
            }
        }

        public bool Backward()
        {
            if (this.index == 0)
            {
                return false;
            }
            else
            {
                index--;
                EdgeContext ctx = this.method.Logs[this.index];
                new SplitVertex().Split(ctx);
                return true;
            }
        }

        public void Show()
        {
            if (this.index > 0)
            {
                EdgeContext ctx = this.method.Logs[index - 1];
                ctx.Top.Traits.selectedFlag = 1;
                ctx.TopLeftEdge.Traits.selectedFlag = 1;
                ctx.Left.Traits.selectedFlag = 1;
                ctx.MidEdge.Traits.selectedFlag = 1;
                ctx.Right.Traits.selectedFlag = 1;
                ctx.ButtomRightEdge.Traits.selectedFlag = 1;
                ctx.Buttom.Traits.selectedFlag = 1;
            }
            if (this.index < this.method.Logs.Count)
            {
                EdgeContext ctx = this.method.Logs[index];
                ctx.Top.Traits.selectedFlag = 1;
                ctx.TopLeftEdge.Traits.selectedFlag = 1;
                ctx.Left.Traits.selectedFlag = 1;
                ctx.MidEdge.Traits.selectedFlag = 1;
                ctx.Right.Traits.selectedFlag = 1;
                ctx.ButtomRightEdge.Traits.selectedFlag = 1;
                ctx.Buttom.Traits.selectedFlag = 1;
            }
        }
    }
}
