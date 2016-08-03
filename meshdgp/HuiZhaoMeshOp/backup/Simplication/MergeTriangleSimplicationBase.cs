using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public abstract class MergeTriangleSimplicationBase : MergeSimplicationBase<TriMesh.Face>
    {
        protected MinHeapTwo<TriMesh.Face> heap;
        protected HeapNode<TriMesh.Face>[] handle;
        protected TriMeshTraits traits;
        protected TriMesh.Face[] removed = new HalfEdgeMesh.Face[4];

        public MergeTriangleSimplicationBase(TriMesh mesh)
            : base(mesh)
        {

        }

        protected override void Init()
        {
            this.heap = new MinHeapTwo<TriMesh.Face>(this.Mesh.Faces.Count);
            this.handle = new HeapNode<TriMesh.Face>[this.Mesh.Faces.Count];
            this.traits = new TriMeshTraits(this.Mesh);
            this.traits.Init();

            foreach (TriMesh.Face face in this.Mesh.Faces)
            {
                double value = this.GetValue(face);
                this.handle[face.Index] = heap.Add(value, face);
            }
        }

        protected override MergeArgs GetMin()
        {
            List<HeapNode<TriMesh.Face>> unMergable = new List<HeapNode<TriMesh.Face>>();
            HeapNode<TriMesh.Face> node = this.heap.Pull();
            while (node != null && !TriMeshModify.IsMergeable(node.Item))
            {
                unMergable.Add(node);
                node = heap.Pull();
            }

            foreach (var item in unMergable)
            {
                this.heap.Add(item);
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

        protected virtual Vector3D GetPos(TriMesh.Face target)
        {
            return TriMeshUtil.GetMidPoint(target);
        }

        protected override TriMesh.Vertex Merge(MergeArgs args)
        {
            return TriMeshModify.Merge(args.Target, args.Pos);
        }

        protected override void BeforeMerge(TriMesh.Face target)
        {
            this.removed[0] = target;
            int i = 1;
            foreach (var face in target.Faces)
            {
                this.removed[i++] = face;
            }
        }

        protected override void AfterMerge(TriMesh.Vertex v)
        {
            foreach (var face in this.removed)
            {
                this.heap.Del(handle[face.Index]);
            }

            this.traits.MergeUpdate(v);
            foreach (var face in v.Faces)
            {
                double value = this.GetValue(face);
                this.heap.Update(handle[face.Index], value);
            }
        }

        protected abstract double GetValue(TriMesh.Face target);
    }
}
