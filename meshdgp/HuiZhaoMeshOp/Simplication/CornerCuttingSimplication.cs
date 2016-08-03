using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    /// <summary>
    /// 基于割角的保特征网格简化算法_计忠平
    /// </summary>
    public class CornerCuttingSimplication : MergeSimplicationBase<TriMesh.Edge>
    {
        MinHeap<TriMesh.HalfEdge> heap;
        HeapNode<TriMesh.HalfEdge>[] handle;
        TriMesh.HalfEdge[] removed = new HalfEdgeMesh.HalfEdge[6];

        public CornerCuttingSimplication(TriMesh mesh)
            : base(mesh)
        {

        }

        protected override void Init()
        {
            this.heap = new MinHeap<HalfEdgeMesh.HalfEdge>(this.Mesh.HalfEdges.Count);
            this.handle = new HeapNode<HalfEdgeMesh.HalfEdge>[this.Mesh.HalfEdges.Count];

            foreach (var hf in this.Mesh.HalfEdges)
            {
                double value = this.GetValue(hf);
                this.handle[hf.Index] = this.heap.Add(value, hf);
            }
        }

        protected override MergeArgs GetMin()
        {
            List<HeapNode<TriMesh.HalfEdge>> unMergable = new List<HeapNode<TriMesh.HalfEdge>>();
            HeapNode<TriMesh.HalfEdge> node = heap.Pull();
            while (node != null && !TriMeshModify.IsMergeable(node.Item.Edge))
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
                return new MergeArgs { Target = node.Item.Edge, Pos = pos };
            }
        }

        protected Vector3D GetPos(TriMesh.HalfEdge hf)
        {
            return TriMeshUtil.GetMidPoint(hf.Edge);
        }

        protected Vector3D GetPos1(TriMesh.HalfEdge hf)
        {
            Vector3D from = hf.FromVertex.Traits.Position;
            Vector3D to = hf.ToVertex.Traits.Position;
            Vector3D l = to - from;
            Vector3D[] vi = this.GetV(hf);
            Vector3D[] vj = this.GetV(hf.Opposite);
            Vector3D c1 = Vector3D.Zero;
            Vector3D c2 = Vector3D.Zero;
            for (int i = 0; i < vi.Length - 1; i++)
            {
                c1 += (vi[i] - from).Cross(vi[i + 1] - from);
            }
            for (int i = 0; i < vj.Length - 1; i++)
            {
                c2 += (vj[i + 1] - to).Cross(vj[i] - to);
            }
            Matrix4D m = Matrix4D.ZeroMatrix;
            m.Row1 = new Vector4D(c1 - c2, to.Dot(c2)-from.Dot(c1));
            m.Row2 = new Vector4D(l.y, -l.x, 0d, to.y * l.x - to.x * l.y);
            m.Row3 = new Vector4D(l.z, 0d, -l.x, to.z * l.x - to.x * l.z);
            m.Row4 = new Vector4D(0d, 0d, 0d, 1d);

            Vector3D pos = Vector3D.Zero;
            double det = Util.Solve(ref m, ref pos);
            if (det == 0)
            {
                pos = TriMeshUtil.GetMidPoint(hf.Edge);
            }
            else
            {
                double vo1 = c1.Dot(pos - from);
                double vo2 = c2.Dot(pos - to);
            }
            return pos;
        }

        protected override HalfEdgeMesh.Vertex Merge(MergeArgs args)
        {
            return TriMeshModify.MergeEdge(args.Target, args.Pos);
        }

        protected override void BeforeMerge(TriMesh.Edge target)
        {
            this.removed[0] = target.HalfEdge0;
            this.removed[1] = target.HalfEdge1;
            this.removed[2] = target.HalfEdge0.Next;
            this.removed[3] = target.HalfEdge0.Previous;
            this.removed[4] = target.HalfEdge1.Next;
            this.removed[5] = target.HalfEdge1.Previous;
        }

        protected override void AfterMerge(TriMesh.Vertex v)
        {
            foreach (var hf in this.removed)
            {
                this.heap.Del(handle[hf.Index]);
            }

            foreach (var hf in v.HalfEdges)
            {
                double value1 = this.GetValue(hf);
                double value2 = this.GetValue(hf.Opposite);
                this.heap.Update(handle[hf.Index], value1);
                this.heap.Update(handle[hf.Opposite.Index], value2);
            }
        }

        protected double GetValue(TriMesh.HalfEdge hf)
        {
            Vector3D top = hf.FromVertex.Traits.Position;
            Vector3D right = hf.ToVertex.Traits.Position;
            Vector3D[] vi = this.GetV(hf);
            double volume = 0;
            for (int i = 0; i < vi.Length - 1; i++)
            {
                volume += TriMeshUtil.ComputeVolume(top, right, vi[i], vi[i + 1]);
            }
            return Math.Abs(volume);
        }

        Vector3D[] GetV(TriMesh.HalfEdge hf)
        {
            List<Vector3D> list = new List<Vector3D>();
            TriMesh.HalfEdge cur = hf.FromVertex.HalfEdge;
            while (cur != hf)
            {
                cur = cur.Opposite.Next;
            }
            cur = cur.Opposite.Next;
            while (cur != hf)
            {
                list.Add(cur.ToVertex.Traits.Position);
                cur = cur.Opposite.Next;
            }
            return list.ToArray();
        }
    }
}
