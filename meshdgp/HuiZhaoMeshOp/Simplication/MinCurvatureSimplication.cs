using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class MinCurvatureSimplication : MergeSimplicationBase<TriMesh.Edge>
    {
        MinHeap<TriMesh.Vertex> heap;
        HeapNode<TriMesh.Vertex>[] handle;
        TriMesh.Vertex removed;

        public MinCurvatureSimplication(TriMesh mesh)
            : base(mesh)
        {

        }

        protected override void Init()
        {
            this.heap = new MinHeap<TriMesh.Vertex>(this.Mesh.Vertices.Count);
            this.handle = new HeapNode<TriMesh.Vertex>[this.Mesh.Vertices.Count];

            foreach (TriMesh.Vertex v in this.Mesh.Vertices)
            {
                double curvature = this.GetValue(v);
                this.handle[v.Index] = heap.Add(Math.Abs(curvature), v);
            }
        }

        protected override MergeArgs GetMin()
        {
            List<HeapNode<TriMesh.Vertex>> unMergable = new List<HeapNode<TriMesh.Vertex>>();
            HeapNode<TriMesh.Vertex> node = heap.Pull();
            while (node != null)
            {
                foreach (var hf in node.Item.HalfEdges)
                {
                    if (TriMeshModify.IsMergeable(hf.Edge))
                    {
                        foreach (var item in unMergable)
                        {
                            heap.Add(item);
                        }
                        Vector3D pos = hf.ToVertex.Traits.Position;
                        return new MergeArgs() { Target = hf.Edge, Pos = pos };
                    }
                }
                unMergable.Add(node);
                node = heap.Pull();
            }
            return null;
        }

        protected override TriMesh.Vertex Merge(MergeArgs args)
        {
            return TriMeshModify.MergeEdge(args.Target, args.Pos);
        }

        protected override void BeforeMerge(TriMesh.Edge target)
        {
            this.removed = target.Vertex1;
        }

        protected override void AfterMerge(TriMesh.Vertex v)
        {
            heap.Del(this.handle[this.removed.Index]);

            double v1Curvature = this.GetValue(v);
            this.heap.Update(this.handle[v.Index], Math.Abs(v1Curvature));
            foreach (var round in v.Vertices)
            {
                double curvature = this.GetValue(round);
                this.heap.Update(handle[round.Index], Math.Abs(curvature));
            }
        }

        protected double GetValue(TriMesh.Vertex v)
        {
            return TriMeshUtil.ComputeGaussianCurvature(v);
        }
    }
}
