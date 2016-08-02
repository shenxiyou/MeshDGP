using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class Flattern
    {
        private TriMesh mesh = null;

        public Flattern(TriMesh mesh)
        {
            this.mesh = mesh;

        }

        public void FlattenProcess()
        {

            SparseMatrixComplex Lc = BuildEnergy();

            SparseMatrixComplex star0 = DECComplex.Instance.cBuildHodgeStar0Form(mesh);
            Lc += new Complex(1.0e-8, 0) * star0; //[Reconsider: Complex(1.0e-8)]

            //Compute parameterization
            DenseMatrixComplex x = new DenseMatrixComplex(Lc.RowCount, 1);
            x.Randomize();  //Initial guesses

            DenseMatrixComplex result = LinearSystemDEC.Instance.smallestEigPositiveDefinite(ref Lc, ref star0, ref x);

            //Assign sultion
            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                Complex value = result[v.Index, 0];

                v.Traits.Position.x = value.RealPart;
                v.Traits.Position.y = value.ImagePart;
                v.Traits.Position.z = 0;
            }

            TriMeshUtil.ScaleToUnit(mesh,1.0);
            TriMeshUtil.MoveToCenter(mesh);

        }

        public static Complex DDGConstant = new Complex(0, 1);

        public SparseMatrixComplex BuildEnergy()
        {
            //Build Laplace matrix
            SparseMatrixDouble d0 = DECDouble.Instance.BuildExteriorDerivative0Form(mesh); ;
            SparseMatrixDouble star1 = DECDouble.Instance.BuildHodgeStar1Form(mesh);
            SparseMatrixDouble L = d0.Transpose() * star1 * d0;

            SparseMatrixComplex A = SparseMatrixComplex.Copy(ref L);

            //Iterate faces
            foreach (TriMesh.Face face in mesh.Faces)
            {
                //Iterate each halfedge in face
                foreach (TriMesh.HalfEdge hf in face.Halfedges)
                {
                    int i = hf.ToVertex.Index;
                    int j = hf.FromVertex.Index;

                    Complex value = 0.5 * DDGConstant;

                    A[i, j] -= value;
                    A[j, i] += value;

                }
            }


            return A;
        }

    }
}
