using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class HarmonicBasis
    {
        public TriMesh mesh = null;
        public HarmonicBasis(TriMesh mesh)
        {
            this.mesh = mesh;
            treeCoTree = new TreeCoTree(mesh);
        }

        public TreeCoTree treeCoTree = null;
       
        public DenseMatrixDouble BuildClosedPrimalOneForm(TriMesh mesh, List<TriMesh.HalfEdge> cycle)
        {
            DenseMatrixDouble w = new DenseMatrixDouble(mesh.Edges.Count, 1);

            foreach (TriMesh.HalfEdge hf in cycle)
            {
                double value = 1.0f;

                if (hf.Edge.HalfEdge0 != hf)
                {
                    value = -value;
                }

                w[hf.Edge.Index, 0] = value;
            }

            return w;
        }

        public List<double>[] BuildHarmonicBasis(List<List<TriMesh.HalfEdge>> generators)
        {
            List<double>[] HarmonicBasis;

            HarmonicBasis = new List<double>[mesh.HalfEdges.Count];

            SparseMatrixDouble laplace = DECDouble.Instance.BuildLaplaceWithNeumannBoundary(mesh);
            SparseMatrixDouble star1 = DECDouble.Instance.Star1;
            if (star1 == null)
            {
                star1 = DECDouble.Instance.BuildHodgeStar1Form(mesh);
            }

            SparseMatrixDouble d0 = DECDouble.Instance.D0;
            if (d0 == null)
            {
                d0 = DECDouble.Instance.BuildExteriorDerivative0Form(mesh);
            }

            SparseMatrixDouble d1 = DECDouble.Instance.D1;
            if (d1 == null)
            {
                d1 = DECDouble.Instance.BuildExteriorDerivative1Form(mesh);
            }


            SparseMatrixDouble div = d0.Transpose() * star1;

            bool skipBoundary = true;
            foreach (List<TriMesh.HalfEdge> loopItem in generators)
            {
                if (skipBoundary && treeCoTree.IsBoundaryGenerator(loopItem))
                {
                    skipBoundary = false;
                    continue;
                }

                DenseMatrixDouble W = BuildClosedPrimalOneForm(mesh, loopItem);
                DenseMatrixDouble divW = div * W;

                DenseMatrixDouble u = LinearSystemGenericByLib.Instance.SolveLinerSystem(ref laplace, ref divW);

                DenseMatrixDouble h = star1 * (W - (d0 * u));

                //Store correspond edge
                foreach (TriMesh.Edge edge in mesh.Edges)
                {
                    double value = h[edge.Index, 0];
                    if (HarmonicBasis[edge.HalfEdge0.Index] == null)
                    {
                        HarmonicBasis[edge.HalfEdge0.Index] = new List<double>();
                    }
                    HarmonicBasis[edge.HalfEdge0.Index].Add(value);


                    if (HarmonicBasis[edge.HalfEdge1.Index] == null)
                    {
                        HarmonicBasis[edge.HalfEdge1.Index] = new List<double>();
                    }
                    HarmonicBasis[edge.HalfEdge1.Index].Add(-value);

                }
            }

            return HarmonicBasis;
        }

        public int NumberOfHarmonicBases(List<List<TriMesh.HalfEdge>> generators)
        {
            int nb = 0;
            bool skipBoundaryLoop = true;
            for (int i = 0; i < generators.Count; i++)
            {
                List<TriMesh.HalfEdge> cycle = generators[i];

                if (skipBoundaryLoop && treeCoTree.IsBoundaryGenerator(cycle))
                {
                    skipBoundaryLoop = false;
                    continue;
                }
                nb++;
            }

            return nb;
        }
    }
}
