using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class TriMeshEnhancement
    {
        TriMesh mesh;
        double capAngle = 150;

        public TriMeshEnhancement(TriMesh mesh)
        {
            this.mesh = mesh;
            
        }

        public void Show()
        {
            TriMeshUtil.ClearMeshColor(this.mesh);
            int count = 0;
            foreach (var hf in this.mesh.HalfEdges)
            {
                double angle = TriMeshUtil.ComputeAngle(hf) / Math.PI * 180;
                if (hf.Face != null && angle > this.capAngle)
                {
                    this.SelectAngle(hf);
                    count++;
                }
            }
            
        }

        public void SelectAngle(TriMesh.HalfEdge hf)
        {
            hf.Edge.Traits.SelectedFlag = 1;
            hf.Previous.Edge.Traits.SelectedFlag = 1;
            hf.Next.Edge.Traits.SelectedFlag = 1;
            hf.Next.ToVertex.Traits.SelectedFlag = 1;
        }

        public void RemoveCapWithSplit()
        {
            int count = 0;
            do
            {
                count = 0;
                for (int i = 0; i < this.mesh.HalfEdges.Count; i++)
                {
                    TriMesh.HalfEdge hf = this.mesh.HalfEdges[i];
                    double angle = TriMeshUtil.ComputeAngle(hf) / Math.PI * 180;
                    if (angle > this.capAngle)
                    {
                        Plane plane = this.GetPlane(hf);

                        Nullable<Vector3D> point = this.Intersect(plane, hf.Edge);
                        this.ShowIntersect(plane);

                        if (point != null)
                        {
                            this.Cut(hf, point.Value);
                            count++;
                        }
                        else
                        {
                            hf.Edge.Traits.SelectedFlag = 1;
                        }
                    }
                }
            } while (count != 0);
        }

        public void RemoveCapWithPlane()
        {
            int count = this.mesh.HalfEdges.Count;
            for (int i = 0; i < count; i++)
            {
                TriMesh.HalfEdge hf = this.mesh.HalfEdges[i];
                double angle = TriMeshUtil.ComputeAngle(hf) / Math.PI * 180;
                if (angle > this.capAngle)
                {
                    Plane plane = this.GetPlane(hf);
                    this.ShowIntersect(plane);
                    this.PlaneCut(hf, plane.Normal, this.capAngle - 90);
                }
            }
        }

        private void ShowIntersect(Plane plane)
        {
            foreach (var item in this.mesh.Edges)
            {
                if (this.Intersect(plane, item) != null)
                {
                    item.Traits.SelectedFlag = 1;
                }
            }
        }

        private Plane GetPlane(TriMesh.HalfEdge hf)
        {
            return this.GetProjectivePlane(hf);
        }

        private Plane GetBisectorPlane(TriMesh.HalfEdge hf)
        {
            Vector3D a = hf.Next.ToVertex.Traits.Position;
            Vector3D b = hf.FromVertex.Traits.Position;
            Vector3D c = hf.ToVertex.Traits.Position;
            Vector3D v1 = a + (b + c - a * 2);
            Vector3D v2 = a + new Plane(a, b, c).Normal;
            return new Plane(a, v1, v2);
        }

        private Plane GetProjectivePlane(TriMesh.HalfEdge hf)
        {
            Vector3D p = hf.Next.ToVertex.Traits.Position;
            Vector3D p1 = hf.FromVertex.Traits.Position;
            Vector3D p2 = hf.ToVertex.Traits.Position;
            return new Plane(p, p2 - p1);
        }
    }
}
