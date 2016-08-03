using System;
using System.Collections.Generic;
using System.Text; 

namespace GraphicResearchHuiZhao
{
    public partial class TriMeshSubdivision
    {
        #region SubdivisionSqrt3EdgeFlip
        public TriMesh SubdivisonSqrt3()
        {
            TriMesh sqrt = ChangeTopologyInit(Mesh);
            ChangeGeometrySqrt3(Mesh, sqrt);
            TriMeshUtil.FixIndex(sqrt);

            return sqrt;
        }

        private TriMesh ChangeTopologyInit(TriMesh sourceMesh)
        {
            TriMesh mesh = new TriMesh();

            vMap = new HalfEdgeMesh.Vertex[sourceMesh.Vertices.Count];
            faceMap = new HalfEdgeMesh.Vertex[sourceMesh.Faces.Count];
            foreach (var face in sourceMesh.Faces)
            {
                faceMap[face.Index] = mesh.Vertices.Add(
                    new VertexTraits(TriMeshUtil.GetMidPoint(face)));
            }

            foreach (var v in sourceMesh.Vertices)
            {
                vMap[v.Index] = mesh.Vertices.Add(new VertexTraits(v.Traits.Position));
                foreach (var hf in v.HalfEdges)
                {
                    if (hf.Face != null && hf.Opposite.Face != null)
                    {
                        mesh.Faces.AddTriangles(faceMap[hf.Face.Index], 
                            vMap[v.Index], faceMap[hf.Opposite.Face.Index]);
                    }
                }
            }

            foreach (var hf in sourceMesh.HalfEdges)
            {
                if (hf.Face == null)
                {
                    mesh.Faces.AddTriangles(vMap[hf.ToVertex.Index], 
                        vMap[hf.FromVertex.Index], faceMap[hf.Opposite.Face.Index]);
                }
            }
            return mesh;
        }

        private void ChangeGeometrySqrt3(TriMesh sourceMesh, TriMesh targetMesh)
        {
            for (int i = 0; i < sourceMesh.Vertices.Count; i++)
            {
                int n = sourceMesh.Vertices[i].VertexCount;
                Vector3D position = sourceMesh.Vertices[i].Traits.Position;
                Vector3D neighborsum = new Vector3D(0, 0, 0);
                double alpha = Sqrt3ComputeAlpha(n);
                foreach (TriMesh.Vertex neighbor in sourceMesh.Vertices[i].Vertices)
                {
                    neighborsum += neighbor.Traits.Position;
                }
                vMap[i].Traits.Position = (1 - alpha) * position
                                                       + (alpha / n) * neighborsum;
            }
        }

        private double Sqrt3ComputeAlpha(int n)//计算权值beta
        {
            double alpha, middle;
            middle = 2 * (Math.Cos(6.2831853 / n));
            alpha = (4 - middle) / 9;
            return alpha;
        }
        #endregion
    }
}
