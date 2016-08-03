using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class TriMeshEnhancement
    {
        private double IntersectThreshold = 0.00001;

        public void PlaneCutTest()
        {
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                TriMesh.Vertex v = mesh.Vertices[i];
                if (v.Traits.SelectedFlag != 0)
                {
                    Plane plane = new Plane(v.Traits.Position, new Vector3D(1, 0, 0));

                    foreach (var hf in v.HalfEdges)
                    {
                        this.PlaneCut(hf.Next, plane.Normal);
                    }
                    break;
                }
            }
        }

        public void PlaneCut(TriMesh.HalfEdge above, Vector3D normal)
        {
            this.PlaneCut(above, normal, 90);
        }

        public void PlaneCut(TriMesh.HalfEdge above, Vector3D normal, double maxAngle)
        {
            Plane plane = new Plane(above.Next.ToVertex.Traits.Position, normal);
            Nullable<Vector3D> point = this.Intersect(plane, above.Edge);
            double angle = this.GetAngle(plane, above);

            while (point != null && angle > maxAngle)
            {
                TriMesh.Vertex left = above.FromVertex;
                TriMesh.Vertex right = above.ToVertex;
                TriMesh.Vertex buttom = above.Opposite.Next.ToVertex;

                this.Cut(above, point.Value);

                if (above.Opposite.OnBoundary)
                {
                    break;
                }

                TriMesh.HalfEdge[] below = new[] { left.FindHalfedgeTo(buttom), buttom.FindHalfedgeTo(right) };
                foreach (var hf in below)
                {
                    point = this.Intersect(plane, hf.Edge);
                    if (point != null)
                    {
                        angle = this.GetAngle(plane, hf);
                        above = hf;
                        break;
                    }
                }
            }
        }

        private double GetAngle(Plane plane, TriMesh.HalfEdge hf)
        {
            Vector3D v = hf.ToVertex.Traits.Position - hf.FromVertex.Traits.Position;
            double cos = Math.Abs(plane.Normal.Dot(v.Normalize()));
            return Math.Acos(cos) * 180 / Math.PI;
        }

        private void Cut(TriMesh.HalfEdge above, Vector3D pos)
        {
            TriMesh.Vertex v1 = above.FromVertex;
            TriMesh.Vertex top = above.Next.ToVertex;
            TriMesh.Vertex buttom = above.Opposite.Next.ToVertex;
            TriMesh.Vertex v2 = TriMeshModify.VertexSplit(v1, 
                top, buttom, v1.Traits.Position, pos);
            v2.Traits.SelectedFlag = 1;
        }

        private Nullable<Vector3D> Intersect(Plane plane, TriMesh.Edge edge)
        {
            return TriMeshUtil.Intersect(plane, edge, IntersectThreshold);
        }

        
    }
}
