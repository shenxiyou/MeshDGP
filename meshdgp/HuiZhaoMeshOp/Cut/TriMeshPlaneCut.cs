using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class TriMeshPlaneCut
    {
        TriMesh mesh;
        Plane plane;
        Dictionary<int,CutPoint> cutPoint;
        List<Triangle> list;

        public TriMeshPlaneCut(TriMesh mesh)
        {
            this.mesh = mesh;
        }

        public void Cut(Plane plane)
        {
            this.plane = plane;
            this.list = new List<Triangle>();
            this.cutPoint = new Dictionary<int, CutPoint>();

            foreach (var hf in this.mesh.HalfEdges)
            {
                this.CutHalfEdge(hf);
            }

            foreach (var face in this.mesh.Faces)
            {
                this.CutFace(face);
            }

            List<TriMesh.Edge> edges = new List<HalfEdgeMesh.Edge>();
            foreach (var hf in this.mesh.HalfEdges)
            {
                if (this.cutPoint.ContainsKey(hf.Index))
                {
                    edges.Add(hf.Edge);
                }
            }
            foreach (var item in edges)
            {
                TriMeshModify.RemoveEdge(item);
            }

            foreach (var item in this.list)
            {
                this.mesh.Faces.AddTriangles(item.V1, item.V2, item.V3);
            }
            TriMeshUtil.FixIndex(mesh);
            TriMeshUtil.SetUpNormalVertex(mesh);
        }

        public void CutHalfEdge(TriMesh.HalfEdge hf)
        {
            Vector3D p1 = hf.FromVertex.Traits.Position;
            Vector3D p2 = hf.ToVertex.Traits.Position;
            double d1 = TriMeshUtil.GetDistance(plane, p1);
            double d2 = TriMeshUtil.GetDistance(plane, p2);
            if (d1 < 0 && d2 > 0)
            {
                d1 = Math.Abs(d1);
                d2 = Math.Abs(d2);
                Vector3D pos = p1 + (p2 - p1) * d1 / (d1 + d2);
                TriMesh.Vertex v1 = new TriMesh.Vertex(new VertexTraits(pos));
                this.mesh.AppendToVertexList(v1);
                TriMesh.Vertex v2 = new TriMesh.Vertex(new VertexTraits(pos));
                this.mesh.AppendToVertexList(v2);
                this.cutPoint[hf.Index] = new CutPoint { Left = v1, Right = v2 };
            }
        }

        public void CutFace(TriMesh.Face face)
        {
            TriMesh.HalfEdge h = null;
            foreach (var hf in face.Halfedges)
            {
                if (this.cutPoint.ContainsKey(hf.Index))
                {
                    h = hf;
                }
            }
            if (h == null)
            {
                return;
            }
            if (this.cutPoint.ContainsKey(h.Next.Opposite.Index))
            {
                this.list.Add(new Triangle
                {
                    V1 = h.FromVertex,
                    V2 = this.cutPoint[h.Index].Left,
                    V3 = h.Next.ToVertex
                });
                this.list.Add(new Triangle
                {
                    V1 = this.cutPoint[h.Index].Right,
                    V2 = h.ToVertex,
                    V3 = this.cutPoint[h.Next.Opposite.Index].Right
                });
                this.list.Add(new Triangle
                {
                    V1 = this.cutPoint[h.Next.Opposite.Index].Left,
                    V2 = h.Next.ToVertex,
                    V3 = this.cutPoint[h.Index].Left
                });
                h.Previous.Edge.Traits.SelectedFlag = 1;
                h.ToVertex.Traits.SelectedFlag = 1;
            }
            else
            {
                this.list.Add(new Triangle
                {
                    V1 = h.FromVertex,
                    V2 = this.cutPoint[h.Index].Left,
                    V3 = this.cutPoint[h.Previous.Opposite.Index].Left
                });
                this.list.Add(new Triangle
                {
                    V1 = this.cutPoint[h.Index].Right,
                    V2 = h.ToVertex,
                    V3 = h.Next.ToVertex
                });
                this.list.Add(new Triangle
                {
                    V1 = this.cutPoint[h.Index].Right,
                    V2 = h.Next.ToVertex,
                    V3 = this.cutPoint[h.Previous.Opposite.Index].Right
                });
                h.Next.Edge.Traits.SelectedFlag = 1;
                h.FromVertex.Traits.SelectedFlag = 1;
            }
        }

        class CutPoint
        {
            public TriMesh.Vertex Left;
            public TriMesh.Vertex Right;
        }

        class Triangle
        {
            public TriMesh.Vertex V1;
            public TriMesh.Vertex V2;
            public TriMesh.Vertex V3;
        }
    }
}
