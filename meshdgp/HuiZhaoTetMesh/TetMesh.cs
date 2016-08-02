using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class TetMesh
    {
        public double AvgLength;

        List<TetVertex> vertices = new List<TetVertex>();
        List<TetEdge> edges = new List<TetEdge>();
        List<TetFace> faces = new List<TetFace>();
        List<Tetrahedron> tetras = new List<Tetrahedron>();

        public virtual IList<TetVertex> Vertices
        {
            get { return vertices; }
        }

        public virtual IList<TetEdge> Edges
        {
            get { return edges; }
        }

        public virtual IList<TetFace> Faces
        {
            get { return faces; }
        }

        public virtual IList<Tetrahedron> Tetras
        {
            get { return tetras; }
        }

        public TetMesh()
        {

        }

        public void ComputeTraits()
        {
            AvgLength = TetMeshUtil.ComputeEdgeAvgLength(this);
        }

        public void ComputeNormal()
        {
            foreach (var face in this.faces)
            {
                face.Normal = TetMeshUtil.ComputeNormal(face);
            }

            Vector3D[] vn = TetMeshUtil.ComputeNormalUniformWeight(this);
            foreach (var v in this.vertices)
            {
                v.Normal = vn[v.Index];
            }
        }

        public void ComputeSelectedNormal()
        {
            foreach (var face in this.faces)
            {
                face.SelectedNormal = TetMeshFlag.ComputeNormal(face);
            }

            Vector3D[] vn = TetMeshFlag.ComputeNormalUniformWeight(this);
            foreach (var v in this.vertices)
            {
                v.SelectedNormal = vn[v.Index];
            }
        }

        public void Check()
        {
            foreach (var edge in this.edges)
            {
                if (edge.B != edge.OnBoundary)
                {
                    Console.WriteLine("Edge: " + edge.Index);
                }
            }

            foreach (var face in this.faces)
            {
                if (face.B != face.OnBoundary)
                {
                    Console.WriteLine("Face: " + face.Index);
                }
            }
        }
    }
}
