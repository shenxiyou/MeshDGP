using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace GraphicResearchHuiZhao
{
    public class Fairing
    {
        private TriMesh mesh = null;
        public Fairing(TriMesh mesh)
        {
            this.mesh = mesh;

        }


        public void Run()
        {

            Stopwatch clock = new Stopwatch();

            clock.Start();

            double step = 0.01;

            DECMeshDouble decMesh = new DECMeshDouble(mesh);
            SparseMatrixDouble laplace = decMesh.Laplace;

            SparseMatrixDouble star0 = decMesh.HodgeStar0Form;

            SparseMatrixDouble star1 = decMesh.HodgeStar1Form;

            SparseMatrixDouble d0 = decMesh.ExteriorDerivative0Form;

            SparseMatrixDouble L = d0.Transpose() * star1 * d0;


            SparseMatrixDouble A = star0 + step * L;
            A.WriteToFile("A.ma");

            double[] xs = new double[mesh.Vertices.Count];
            double[] ys = new double[mesh.Vertices.Count];
            double[] zs = new double[mesh.Vertices.Count];

            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                xs[v.Index] = v.Traits.Position.x;
                ys[v.Index] = v.Traits.Position.y;
                zs[v.Index] = v.Traits.Position.z;
            }

            double[] rhs1 = star0 * xs;
            double[] rhs2 = star0 * ys;
            double[] rhs3 = star0 * zs;


            //SparseMatrix.WriteVectorToFile("xs.ve", rhs1);
            //SparseMatrix.WriteVectorToFile("ys.ve", rhs2);
            //SparseMatrix.WriteVectorToFile("zs.ve", rhs3);

            DenseMatrixDouble rhsx = new DenseMatrixDouble(mesh.Vertices.Count, 1);
            DenseMatrixDouble rhsy = new DenseMatrixDouble(mesh.Vertices.Count, 1);
            DenseMatrixDouble rhsz = new DenseMatrixDouble(mesh.Vertices.Count, 1);

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                rhsx[i, 0] = rhs1[i];
                rhsy[i, 0] = rhs2[i];
                rhsz[i, 0] = rhs3[i];
            }

            DenseMatrixDouble newX = LinearSystemGenericByLib.Instance.SolveLinerSystem(ref A, ref rhsx);
            DenseMatrixDouble newY = LinearSystemGenericByLib.Instance.SolveLinerSystem(ref A, ref rhsy);
            DenseMatrixDouble newZ = LinearSystemGenericByLib.Instance.SolveLinerSystem(ref A, ref rhsz);

            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                v.Traits.Position.x = newX[v.Index, 0];
                v.Traits.Position.y = newY[v.Index, 0];
                v.Traits.Position.z = newZ[v.Index, 0];
            }

            TriMeshUtil.ScaleToUnit(mesh,1.0f);
            TriMeshUtil.MoveToCenter(mesh);

            clock.Stop();

            decimal micro = clock.Elapsed.Ticks / 10m;
            Console.WriteLine("Total time cost:{0}", micro);

        }
    }
}
