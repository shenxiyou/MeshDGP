using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class TriMeshTraits
    {
        TriMesh mesh;

        public double[] EdgeLength;
        public double[] FaceArea;
        public Vector3D[] FaceNormal;
        public double[] VertexAreaSum;
        public Vector3D[] VertexAreaWeightNormal;
        public double[] VertexDiscreteCurvature;

        public TriMeshTraits(TriMesh mesh)
        {
            this.mesh = mesh;
        }

        public void Init()
        {
            
            TriMeshUtil.FixIndex(mesh);

            int vc = mesh.Vertices.Count;
            int ec = mesh.Edges.Count;
            int fc = mesh.Faces.Count;

            this.EdgeLength = new double[ec];
            this.FaceArea = new double[fc];
            this.FaceNormal = new Vector3D[fc];
            this.VertexAreaSum = new double[ec];
            this.VertexAreaWeightNormal = new Vector3D[vc];
            this.VertexDiscreteCurvature = new double[vc];

            foreach (var edge in this.mesh.Edges)
            {
                this.UpdateLength(edge);
            }

            foreach (var face in this.mesh.Faces)
            {
                this.UpdateAreaAndNormal(face);
            }

            foreach (var v in this.mesh.Vertices)
            {
                this.UpdateAreaWeightNormal(v);
                this.UpdateVertexDiscreteCurvature(v);
            }
        }

        public void MergeUpdate(TriMesh.Vertex v)
        {
            foreach (var item in v.Edges)
            {
                this.UpdateLength(item);
            }

            foreach (var face in v.Faces)
            {
                this.UpdateAreaAndNormal(face);
            }
            this.UpdateAreaWeightNormal(v);
            foreach (var hf in v.HalfEdges)
            {
                this.UpdateAreaWeightNormal(hf.ToVertex);
                this.UpdateVertexDiscreteCurvature(v);
            }
        }

        void UpdateLength(TriMesh.Edge edge)
        {
            this.EdgeLength[edge.Index] = TriMeshUtil.ComputeEdgeLength(edge);
        }

        void UpdateAreaAndNormal(TriMesh.Face face)
        {
            Vector3D cross = this.Cross(face);
            this.FaceArea[face.Index] = cross.Length() / 2d;
            this.FaceNormal[face.Index] = cross.Normalize();
        }

        void UpdateAreaWeightNormal(TriMesh.Vertex v)
        {
            this.VertexAreaSum[v.Index] = 0;
            this.VertexAreaWeightNormal[v.Index] = Vector3D.Zero;
            foreach (var face in v.Faces)
            {
                this.VertexAreaWeightNormal[v.Index] += this.FaceNormal[face.Index] * this.FaceArea[face.Index];
                this.VertexAreaSum[v.Index] += this.FaceArea[face.Index];
            }
            this.VertexAreaWeightNormal[v.Index] /= this.VertexAreaSum[v.Index];
        }

        void UpdateVertexDiscreteCurvature(TriMesh.Vertex v)
        {
            Vector3D delta = Vector3D.Zero;
            foreach (var hf in v.HalfEdges)
            {
                delta += hf.ToVertex.Traits.Position - v.Traits.Position;
            }
            Vector3D normal = this.VertexAreaWeightNormal[v.Index];
            this.VertexDiscreteCurvature[v.Index] = (delta / 2).Dot(normal);
        }

        Vector3D Cross(TriMesh.Face face)
        {
            Vector3D[] arr = new Vector3D[3];
            int i = 0;
            foreach (var v in face.Vertices)
            {
                arr[i++] = v.Traits.Position;
            }

            return ((arr[1] - arr[0]).Cross(arr[2] - arr[0]));
        }
    }
}
