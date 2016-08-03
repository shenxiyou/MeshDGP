using System;
using System.Collections.Generic;
using System.Text; 

namespace GraphicResearchHuiZhao 
{
    public class ProgressiveMesh
    {
        QEMProgressive method;
        int index;

        public TriMesh Mesh { get { return this.method.Mesh; } }

        public ProgressiveMesh(TriMesh mesh)
        {
            this.method = new QEMProgressive(mesh);
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
                TriMesh.Vertex left = null;
                TriMesh.Vertex right = null;
                foreach (var v in this.Mesh.Vertices)
                {
                    if (v.Index == ctx.Left)
                    {
                        left = v;
                    }
                    else if (v.Index == ctx.Right)
                    {
                        right = v;
                    }
                }
                TriMesh.Edge edge = left.FindEdgeTo(right);
                MergeEdge.Merge(edge, ctx.MidPos);
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
                SplitVertex.Split(this.Mesh, ctx);
                TriMesh.Vertex left = null;
                TriMesh.Vertex right = null;
                foreach (var v in this.Mesh.Vertices)
                {
                    if (v.Index == ctx.Left)
                    {
                        left = v;
                    }
                    else if (v.Index == ctx.Right)
                    {
                        right = v;
                    }
                }
                
                TriMesh.HalfEdge hf = left.FindHalfedgeTo(right);
                hf.Face.Traits.Normal = TriMeshUtil.ComputeNormalFace(hf.Face);
                hf.Opposite.Face.Traits.Normal = TriMeshUtil.ComputeNormalFace(hf.Opposite.Face);
                left.Traits.Normal =  TriMeshUtil.ComputeNormalAreaWeight(left);
                right.Traits.Normal = TriMeshUtil.ComputeNormalAreaWeight(right);
                return true;
            }
        }

        public void Show()
        {

        }
    }
}
