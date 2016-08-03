using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public abstract class MergeEdgeSimplicationBase : MergeSimplicationBase<TriMesh.Edge>
    {
        public List<EdgeContext> Logs = new List<EdgeContext>();

        protected MinHeapTwo<TriMesh.Edge> heap;
        protected HeapNode<TriMesh.Edge>[] handle;
        protected TriMeshTraits traits;

        protected TriMesh.Edge[] removed = new HalfEdgeMesh.Edge[3];

        public MergeEdgeSimplicationBase(TriMesh mesh)
            : base(mesh)
        {

        }

        protected override void Init()
        {
            this.heap = new MinHeapTwo<TriMesh.Edge>(this.Mesh.Edges.Count);
            this.handle = new HeapNode<TriMesh.Edge>[this.Mesh.Edges.Count];
            this.traits = new TriMeshTraits(this.Mesh);
            this.traits.Init();

            foreach (var item in this.Mesh.Edges)
            {
                double value = this.GetValue(item);
                this.handle[item.Index] = this.heap.Add(value, item);
            }
        }

        protected override MergeArgs GetMin()
        {
            List<HeapNode<TriMesh.Edge>> unMergable = new List<HeapNode<TriMesh.Edge>>();
            HeapNode<TriMesh.Edge> node = heap.Pull();
            while (node != null && !TriMeshModify.IsMergeable(node.Item))
            {
                unMergable.Add(node);
                node = heap.Pull();
            }

            foreach (var item in unMergable)
            {
                heap.Add(item);
            }

            if (node == null)
            {
                return null;
            }
            else
            {
                Vector3D pos = this.GetPos(node.Item);
                return new MergeArgs { Target = node.Item, Pos = pos };
            }
        }

        protected virtual Vector3D GetPos(TriMesh.Edge target)
        {
            return TriMeshUtil.GetMidPoint(target);
        }

        protected override TriMesh.Vertex Merge(MergeArgs args)
        {
            EdgeContext ctx = TriMeshModify.Merge(args.Target, args.Pos);
            this.Logs.Add(ctx);
            return ctx.Left;
        }

        protected override void BeforeMerge(TriMesh.Edge target)
        {
            this.removed[0] = target;
            this.removed[1] = target.HalfEdge0.Previous.Edge;
            this.removed[2] = target.HalfEdge1.Previous.Edge;
        }

        protected override void AfterMerge(TriMesh.Vertex v)
        {
            foreach (var edge in this.removed)
            {
                this.heap.Del(this.handle[edge.Index]);
            }

            this.traits.MergeUpdate(v);
            foreach (var edge in v.Edges)
            {
                double value = this.GetValue(edge);
                this.heap.Update(handle[edge.Index], value);
            }
        }

        protected abstract double GetValue(TriMesh.Edge target);
    }
}
