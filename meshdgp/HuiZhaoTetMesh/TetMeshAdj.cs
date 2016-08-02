using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class TetMesh
    {
        public void BuildAdj()
        {
            this.BuildVE();
            this.BuildVF();
            this.BuildVT();
            this.BuildVV();

            this.BuildEF();
            this.BuildET();

            this.BuildFE();
            this.BuildFT();

            this.BuildTE();
            this.BuildTF();
            this.BuildTT();
        }

        void BuildVE()
        {
            foreach (var edge in this.edges)
            {
                foreach (var v in edge.Vertices)
                {
                    v.Edges.Add(edge);
                }
            }
        }

        void BuildVF()
        {
            foreach (var face in this.faces)
            {
                foreach (var v in face.Vertices)
                {
                    v.Faces.Add(face);
                }
            }
        }

        void BuildVT()
        {
            foreach (var tet in this.tetras)
            {
                foreach (var v in tet.Vertices)
                {
                    v.Tetras.Add(tet);
                }
            }
        }

        /// <summary>
        /// 需要VE
        /// </summary>
        void BuildVV()
        {
            foreach (var edge in this.edges)
            {
                edge.Vertices[0].Vertices.Add(edge.Vertices[1]);
                edge.Vertices[1].Vertices.Add(edge.Vertices[0]);
            }
        }

        /// <summary>
        /// 需要VF
        /// </summary>
        void BuildEF()
        {
            foreach (var edge in this.edges)
            {
                foreach (var f0 in edge.Vertices[0].Faces)
                {
                    foreach (var f1 in edge.Vertices[1].Faces)
                    {
                        if (f0 == f1)
                        {
                            edge.Faces.Add(f0);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 需要VT
        /// </summary>
        void BuildET()
        {
            foreach (var edge in this.edges)
            {
                foreach (var t0 in edge.Vertices[0].Tetras)
                {
                    foreach (var t1 in edge.Vertices[1].Tetras)
                    {
                        if (t0 == t1)
                        {
                            edge.Tetras.Add(t0);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 需要EF
        /// </summary>
        void BuildFE()
        {
            foreach (var edge in this.edges)
            {
                foreach (var face in edge.Faces)
                {
                    face.Edges.Add(edge);
                }
            }
        }

        /// <summary>
        /// 需要FE、ET
        /// </summary>
        void BuildFT()
        {
            foreach (var face in this.faces)
            {
                foreach (var tet0 in face.Edges[0].Tetras)
                {
                    foreach (var tet1 in face.Edges[1].Tetras)
                    {
                        if (tet0 == tet1)
                        {
                            face.Tetras.Add(tet0);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 需要ET
        /// </summary>
        void BuildTE()
        {
            foreach (var edge in this.edges)
            {
                foreach (var tet in edge.Tetras)
                {
                    tet.Edges.Add(edge);
                }
            }
        }

        /// <summary>
        /// 需要FT
        /// </summary>
        void BuildTF()
        {
            foreach (var face in this.faces)
            {
                foreach (var tet in face.Tetras)
                {
                    tet.Faces.Add(face);
                }
            }
        }

        /// <summary>
        /// 需要FT
        /// </summary>
        void BuildTT()
        {
            foreach (var face in this.faces)
            {
                if (face.Tetras.Count == 2)
                {
                    face.Tetras[0].Tetras.Add(face.Tetras[1]);
                    face.Tetras[1].Tetras.Add(face.Tetras[0]);
                }
            }
        }
    }
}
