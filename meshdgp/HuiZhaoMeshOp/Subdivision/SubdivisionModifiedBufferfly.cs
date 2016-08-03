using System;
using System.Collections.Generic;
using System.Text; 

namespace GraphicResearchHuiZhao
{
     public partial class TriMeshSubdivision
    {
        private Vector3D ComputeSingularPoint(List<TriMesh.HalfEdge> hfs)
        {
            if (hfs.Count == 3)
            {
                double[] weight = new double[4]{
                    3f / 4f, 5f / 12f, -1f / 12f, -1f / 12f};
                Vector3D sum = weight[0] * hfs[0].FromVertex.Traits.Position;
                for (int i = 0; i < 3; i++)
                {
                    sum += weight[i + 1] * hfs[i].ToVertex.Traits.Position;
                }
                return sum;
            }
            else if (hfs.Count == 4)
            {
                double[] weight = new double[5]{
                    3f / 4f, 3f / 8f, 0f, -1f / 8f, 0f};
                Vector3D sum = weight[0] * hfs[0].FromVertex.Traits.Position;
                for (int i = 0; i < 4; i++)
                {
                    sum += weight[i + 1] * hfs[i].ToVertex.Traits.Position;
                }
                return sum;
            }
            else if (hfs.Count >= 5)
            {
                double[] weight = new double[hfs.Count + 1];
                weight[0] = 3f / 4f;
                Vector3D sum = weight[0] * hfs[0].FromVertex.Traits.Position;
                double weightSum = weight[0];
                for (int i = 0; i < hfs.Count; i++)
                {
                    weight[i + 1] = (1f / hfs.Count) *
                        (1f / 4f + Math.Cos((2f * i * Math.PI) / (hfs.Count)) +
                        (1f / 2f) * Math.Cos((4f * i * Math.PI) / (hfs.Count)));
                    sum += weight[i + 1] * hfs[i].ToVertex.Traits.Position;
                    weightSum += weight[i + 1];
                }
                return sum / weightSum;
            }
            else
            {
                throw new Exception();
            }
        }

        private Vector3D ComputeModifiedButterflyTraits(double tension, TriMesh.Edge edge)
        {
            List<TriMesh.HalfEdge> left = new List<HalfEdgeMesh.HalfEdge>();
            List<TriMesh.HalfEdge> right = new List<HalfEdgeMesh.HalfEdge>();
            edge.Vertex0.HalfEdge = edge.HalfEdge1;
            edge.Vertex1.HalfEdge = edge.HalfEdge0;
            foreach (var hf in edge.Vertex0.HalfEdges)
	        {
		        left.Add(hf);
            }
            foreach (var hf in edge.Vertex1.HalfEdges)
	        {
		        right.Add(hf);
            }

            if (!edge.OnBoundary)
            {
                if (left.Count == 6 && right.Count == 6)
                {
                    double[] weight = new double[6];
                    weight[0] = (1f / 2f) - (double)tension;
                    weight[1] = weight[5] = (1f / 8f) + 2 * (double)tension;
                    weight[2] = weight[4] = -(1f / 16f) - (double)tension;
                    weight[3] = (double)tension;

                    Vector3D sum = Vector3D.Zero;
                    for (int i = 0; i < 6; i++)
                    {
                        sum += weight[i] * left[i].ToVertex.Traits.Position;
                        sum += weight[i] * right[i].ToVertex.Traits.Position;
                    }

                    double weightSum = weight[0] + 2 * weight[1] + 2 * weight[2] + weight[3];
                    return sum / weightSum / 2;
                }
                else if (left.Count == 6)
                {
                    return ComputeSingularPoint(right);
                }
                else if (right.Count == 6)
                {
                    return ComputeSingularPoint(left);
                }
                else
                {
                    return (ComputeSingularPoint(left) + ComputeSingularPoint(right)) / 2;
                }
            }
            else
            {
                double[] weight = new double[]{9f / 16f,-1f / 16f};
                Vector3D sum = Vector3D.Zero;
                for (int i = 0; i < 2; i++)
                {
                    sum += weight[i] * left[i].ToVertex.Traits.Position;
                    sum += weight[i] * right[i].ToVertex.Traits.Position;
                }
                return sum;
            }
        }

        private void ChangeGeometryWithModifiedButterfly(TriMesh sourceMesh, TriMesh targetMesh, double tension)
        {
            foreach (TriMesh.Face face in sourceMesh.Faces)
            {
                foreach (var edge in face.Edges)
                {
                    eMap[edge.Index].Traits.Position = ComputeModifiedButterflyTraits(tension, edge);
                }
            }
        }


        /// <param name="tension"> tensile force to control vertex skewness, number should be from 0 to 1, 0 is recommanded</param>
        /// <returns></returns>
        public TriMesh SubdivitionModifiedButtefly(double tension)
        {
            TriMesh modifiedButterfly = ChangeTopologyLoop(Mesh);
            ChangeGeometryWithModifiedButterfly(Mesh, modifiedButterfly, tension);
            return modifiedButterfly;
        }
    }
}
