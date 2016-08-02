using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class Test567
    {
        TriMesh mesh;

        public TriMesh Mesh { get { return this.mesh; } }

        public Test567(TriMesh mesh)
        {
            this.mesh = mesh;
        }

        public void Test()
        {
            // 1红2绿3黄4青5紫6蓝7红
            foreach (var item in this.mesh.Edges)
            {
                if (this.EdgeCollapse(item))
                {
                    //item.HalfEdge0.FromVertex.Traits.SelectedFlag = 1;
                    break;
                }
            }
        }

        public void SetColor()
        {
            foreach (var v in this.mesh.Vertices)
            {
                int count = 0;
                foreach (var hf in v.HalfEdges)
                {
                    count++;
                }
                if (count == 3)
                {
                    v.Traits.SelectedFlag = 2;
                }
                else if (count == 4)
                {
                    v.Traits.SelectedFlag = 3;
                }
                else if (count > 7)
                {
                    v.Traits.SelectedFlag = 1;
                }
                else
                {
                    v.Traits.SelectedFlag = 0;
                }
            }
        }

        public void Run()
        {
            //this.Step3();
            //this.Step4();
            //this.Subdivision();
            //this.StepGT7();
            //this.CollapseAll();
        }

        public void Step3()
        {
            for (int z = 0; z < mesh.Vertices.Count; z++)
            {
                if (mesh.Vertices[z].VertexCount == 3)
                {
                    this.RemoveVertex3(mesh.Vertices[z]);
                }
            }
        }

        bool RemoveVertex3(TriMesh.Vertex top)
        {
            /*
             *       top
             *       / \
             *      /   \
             *     /face \
             *  left-----right
             *     \     /
             *      \   /
             *       \ /
             *      buttom
             */
            TriMesh.HalfEdge leftToRight = null;

            //找到一个可用的面，top顶点的对边不在边界上
            foreach (var item in top.HalfEdges)
            {
                if (!item.Next.OnBoundary)
                {
                    leftToRight = item.Next;
                    break;
                }
            }
            if (leftToRight == null)
            {
                return false;
            }

            TriMesh.Vertex left, buttom, right, mid;

            left = leftToRight.FromVertex;
            right = leftToRight.ToVertex;
            mid = this.AddMid(left, right);
            buttom = leftToRight.Opposite.Next.ToVertex;

            TriMeshModify.RemoveEdge(leftToRight.Edge);
            //连接中点，把2个三角形拆成4个
            this.CreateFace(top, left, mid);
            this.CreateFace(top, mid, right);
            this.CreateFace(buttom, right, mid);
            this.CreateFace(buttom, mid, left);

            return true;
        }

        public void Step4()
        {
            for (int z = 0; z < mesh.Vertices.Count; z++)
            {
                if (mesh.Vertices[z].VertexCount == 4)
                {
                    this.RemoveVertex4(mesh.Vertices[z]);
                }
            }
        }

        public  bool RemoveVertex4(TriMesh.Vertex cur)
        {
            /*                                        cur
             *      vl     vr                       /\
             *      |\     /|                      /  \
             *      | \   / |                     /    \
             *      |  \ /  |                    /      \
             *      |  cur  |                   /        \
             *      |  / \  |                  /          \
             *      | /   \ |                lm-----sm-----rm
             *      |/face \|                / \    /\    / \
             *    left-----right            /   \  /  \  /   \
             *       \     /               /     sl____sr     \
             *        \   /               /       \    /       \
             *         \ /               /         \  /         \
             *         vm               /           \/           \
             *                        left----------mm----------right
             *                        
             *                               中部三角形放大图
             */

            TriMesh.HalfEdge curToLeft = null;

            //找到一个可用的面，这个面的3个邻面必须都存在
            foreach (var item in cur.HalfEdges)
            {
                if (item.Face != null && item.Face.FaceCount == 3)
                {
                    curToLeft = item;
                    break;
                }
            }
            if (curToLeft == null)
            {
                return false;
            }

            TriMesh.Vertex left, right, vl, vr, vm,
                           lm, sm, rm, sl, sr, mm;
            TriMesh.HalfEdge leftToRight = curToLeft.Next;
            TriMesh.HalfEdge rightToCur = leftToRight.Next;

            left = curToLeft.ToVertex;
            right = leftToRight.ToVertex;
            vl = curToLeft.Opposite.Next.ToVertex;
            vr = rightToCur.Opposite.Next.ToVertex;
            vm = leftToRight.Opposite.Next.ToVertex;

            lm = this.AddMid(left, cur);
            rm = this.AddMid(right, cur);
            mm = this.AddMid(left, right);

            sm = this.AddMid(lm, rm);
            sl = this.AddMid(lm, mm);
            sr = this.AddMid(mm, rm);

            TriMeshModify.RemoveEdge(curToLeft.Edge);
            TriMeshModify.RemoveEdge(leftToRight.Edge);
            TriMeshModify.RemoveEdge(rightToCur.Edge);

            //图1外圈三角形
            this.CreateFace(vl, left, lm);
            this.CreateFace(vl, lm, cur);
            this.CreateFace(vr, cur, rm);
            this.CreateFace(vr, rm, right);
            this.CreateFace(mm, left, vm);
            this.CreateFace(mm, vm, right);

            //图2外圈三角形
            this.CreateFace(cur, lm, sm);
            this.CreateFace(cur, sm, rm);
            this.CreateFace(lm, left, sl);
            this.CreateFace(sl, left, mm);
            this.CreateFace(rm, sr, right);
            this.CreateFace(sr, mm, right);

            //图2内圈三角形
            this.CreateFace(sm, lm, sl);
            this.CreateFace(sm, sr, rm);
            this.CreateFace(sm, sl, sr);
            this.CreateFace(sl, mm, sr);

            return true;
        }

        public void Subdivision()
        {
            /*        /\                  /\
             *       /  \                /__\
             *      /    \              /\  /\
             *     /      \            /__\/__\
             *    /        \          /\  /\  /\
             *   /__________\        /__\/__\/__\
             *   
             *   为了让每个顶点周围都有它专属的一圈价为6的点，把每个三角形拆成9个
             */
            TriMesh copy = TriMeshIO.Clone(this.mesh);
            this.mesh.Clear();

            TriMesh.Vertex[] faceMap = new HalfEdgeMesh.Vertex[copy.Faces.Count];
            TriMesh.Vertex[] hfMap = new HalfEdgeMesh.Vertex[copy.HalfEdges.Count];

            foreach (var v in copy.Vertices)
            {
                this.mesh.Vertices.Add(new VertexTraits(v.Traits.Position));
            }

            foreach (var face in copy.Faces)
            {
                faceMap[face.Index] = this.mesh.Vertices.Add(new VertexTraits(TriMeshUtil.GetMidPoint(face)));
            }

            foreach (var hf in copy.HalfEdges)
            {
                Vector3D pos = hf.FromVertex.Traits.Position * 2 / 3 + hf.ToVertex.Traits.Position * 1 / 3;
                hfMap[hf.Index] = this.mesh.Vertices.Add(new VertexTraits(pos));
            }

            foreach (var face in copy.Faces)
            {
                foreach (var hf in face.Halfedges)
                {
                    this.mesh.Faces.AddTriangles(faceMap[face.Index], hfMap[hf.Index], hfMap[hf.Opposite.Index]);
                    this.mesh.Faces.AddTriangles(faceMap[face.Index], hfMap[hf.Opposite.Index], hfMap[hf.Next.Index]);
                }
            }

            foreach (var v in copy.Vertices)
            {
                foreach (var hf in v.HalfEdges)
                {
                    if (hf.Face != null)
                    {
                        this.mesh.Faces.AddTriangles(this.mesh.Vertices[v.Index], hfMap[hf.Index], hfMap[hf.Previous.Opposite.Index]);
                    }
                }
            }
        }

        public void StepGT7()
        {
            for (int z = 0; z < mesh.Vertices.Count; z++)
            {
                TriMesh.Vertex cur = mesh.Vertices[z];
                if (cur.VertexCount > 7)
                {
                    if (cur.OnBoundary)
                    {
                        this.GT7Bound(cur);
                    }
                    else
                    {
                        this.GT7Inner(cur);
                    }
                }
            }
        }

        public void GT7Bound(TriMesh.Vertex mid)
        {
            TriMesh.HalfEdge[] halfedges = Extensions.ToArray(mid.HalfEdges);
            int left = 0;
            //找到从边界起的第一条边
            for (int i = 0; i < halfedges.Length; i++)
            {
                if (halfedges[i].Face == null)
                {
                    left = i;
                }
            }
            //从倒数第三条边开始，这样保留的6条边是两边靠近边界的各3条边
            int begin = (halfedges.Length + left - 3) % halfedges.Length;
            mid.HalfEdge = halfedges[begin];
            this.GT7Inner(mid);
        }

        public void GT7Inner(TriMesh.Vertex mid)
        {
            //begin到begin+5保留，begin和begin+5共享给新顶点
            TriMesh.HalfEdge[] hfs = Extensions.ToArray(mid.HalfEdges);

            TriMeshModify.VertexSplit(mid, hfs[0].ToVertex, hfs[5].ToVertex);

            if (mid.VertexCount > 7)
            {
                mid.HalfEdge = hfs[hfs.Length - 1];
                this.GT7Inner(mid);
            }
        }

        TriMesh.Vertex AddMid(TriMesh.Vertex v1, TriMesh.Vertex v2)
        {
            return this.AddMid(v1, v2, 0.5);
        }

        TriMesh.Vertex AddMid(TriMesh.Vertex v1, TriMesh.Vertex v2, double rate)
        {
            VertexTraits trait = new VertexTraits(v1.Traits.Position * (1 - rate) + v2.Traits.Position * rate);
            return this.mesh.Vertices.Add(trait);
        }

        public void CollapseAll(int faceCount)
        {
            int count = 0;
            do
            {
                count = 0;
                for (int i = 0; i < this.mesh.Edges.Count; i++)
                {
                    if (this.mesh.Faces.Count > faceCount)
                    {
                        if (this.EdgeCollapse(this.mesh.Edges[i]))
                        {
                            count++;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                
                if (count == 0)
                {
                    this.FlipOne();
                }
            } while (true);
            
        }

        public bool EdgeCanCollapse(TriMesh.Edge edge)
        {
            if (edge.Vertex0.OnBoundary || edge.Vertex1.OnBoundary)
            {
                return false;
            }
            int left = edge.Vertex1.HalfEdgeCount;
            int right = edge.Vertex0.HalfEdgeCount;
            int top = edge.HalfEdge0.Next.ToVertex.HalfEdgeCount;
            int bottom = edge.HalfEdge1.Next.ToVertex.HalfEdgeCount;

            return left + right < 12 && top > 5 && bottom > 5;
        }

        public  bool EdgeCollapse(TriMesh.Edge edge)
        {
            if (this.EdgeCanCollapse(edge) && TriMeshModify.IsMergeable(edge))
            {
                TriMeshModify.MergeEdge(edge);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void FlipOne()
        {
            for (int i = 0; i < this.mesh.Edges.Count; i++)
            {
                if (this.EdgeFlip(this.mesh.Edges[i]))
                {
                    break;
                }
            }
        }

        public bool EdgeCanFlip(TriMesh.Edge edge)
        {
            if (edge.OnBoundary)
            {
                return false;
            }
            int left = edge.Vertex1.HalfEdgeCount;
            int right = edge.Vertex0.HalfEdgeCount;
            int top = edge.HalfEdge0.Next.ToVertex.HalfEdgeCount;
            int bottom = edge.HalfEdge1.Next.ToVertex.HalfEdgeCount;
            double dot = this.GetNormal(edge.Face0).Dot(this.GetNormal(edge.Face1));

            return left > 5 && right > 5 && top < 7 && bottom < 7 && dot > 0.95;
        }

        public  bool EdgeFlip(TriMesh.Edge edge)
        {
            if (this.EdgeCanFlip(edge))
            {
                TriMeshModify.EdgeSwap(edge);
                return true;
            }
            else
            {
                return false;
            }
        }

        Vector3D GetNormal(TriMesh.Face face)
        {
            List<Vector3D> list = new List<Vector3D>();
            foreach (var item in face.Vertices)
            {
                list.Add(item.Traits.Position);
            }
            return (list[2] - list[1]).Cross(list[2] - list[0]).Normalize();
        }

        public void Enhance()
        {

        }

        TriMesh.Face CreateFace(params TriMesh.Vertex[] faceVerteces)
        {
            return new TriMeshModify().CreateFace(faceVerteces);
        }
    }

    
}
