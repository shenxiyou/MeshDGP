using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public abstract class TetElement
    {
        public int Index;
        public int Flag;
        public Color4 Color;

        public List<TetVertex> Vertices = new List<TetVertex>();
        public List<TetEdge> Edges = new List<TetEdge>();
        public List<TetFace> Faces = new List<TetFace>();
        public List<Tetrahedron> Tetras = new List<Tetrahedron>();

        public abstract bool OnBoundary { get; }
    }

    public class TetVertex : TetElement
    {
        public Vector3D Pos;
        public Vector3D Normal;
        public Vector3D SelectedNormal;

        public override bool OnBoundary
        {
            get
            {
                foreach (var tet in this.Tetras)
                {
                    if (tet.OnBoundary)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }

    public class TetEdge : TetElement
    {
        public bool B;

        public override bool OnBoundary
        {
            get
            {
                foreach (var face in this.Faces)
                {
                    if (face.OnBoundary)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }

    public class TetFace : TetElement
    {
        public Vector3D Normal;
        public Vector3D SelectedNormal;

        public bool B;

        public override bool OnBoundary
        {
            get { return this.Tetras.Count != 2; }
        }
    }

    public class Tetrahedron : TetElement
    {
        public override bool OnBoundary
        {
            get
            {
                foreach (var face in this.Faces)
                {
                    if (face.OnBoundary)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
