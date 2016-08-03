using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GraphicResearchHuiZhao
{
    public class SpinForm
    {

        private TriMesh mesh = null;
        public SpinForm(TriMesh mesh)
        {
            this.mesh = mesh;
        }

        public double[] SetCurvatureChange(TGAImage image, double scale)
        {
            double[] rho = new double[mesh.Faces.Count];
            double w = (double)image.Image.Width;
            double h = (double)image.Image.Height;

            foreach (TriMesh.Face face in mesh.Faces)
            {
                rho[face.Index] = 0;

                for (int i = 0; i < 3; i++)
                {
                    TriMesh.HalfEdge faceVertex = face.FindHalfedgeTo(face.GetVertex(i));
                    Vector2D uv = faceVertex.Traits.TextureCoordinate;
                    rho[face.Index] += image.Sample(uv.x * w, uv.y * h) / 3;
                }

                //Shrink to [-scale,scale]
                rho[face.Index] = (2 * (rho[face.Index] - 0.5)) * scale;
            }
            return rho;
        }

        public void UpdateDeformation(String imagePath, double scale)
        {

            TGAImage image = new TGAImage(imagePath);

            //double[] rho = IOHuiZhao.Instance.ReadVectorFromMatlab("A.vector");
            double[] rho = SetCurvatureChange(image, 10);

            SparseMatrixQuaternion E = BuildEigenValueProblem(rho);
            DenseMatrixQuaternion lamda = LinearSystemDEC.Instance.SolveEigen(ref E);

            SparseMatrixQuaternion Laplace = BuildCotLaplace();
            DenseMatrixQuaternion omega = BuildOmega(lamda);

            LinearSystemGenericByLib.Instance.FactorizationLU(ref Laplace);
            DenseMatrixQuaternion newV = LinearSystemGenericByLib.Instance.SolveByFactorizedLU(ref omega);
            NormalizeSolution(ref newV);

            UpdatePosition(ref newV);
        }

        SparseMatrixQuaternion BuildEigenValueProblem(double[] rho)
        {
            int nV = mesh.Vertices.Count;
            SparseMatrixQuaternion E = new SparseMatrixQuaternion(nV, nV);

            int[] Indices = new int[3];

            foreach (TriMesh.Face face in mesh.Faces)
            {
                double A = TriMeshUtil.ComputeAreaFace(face);
                double a = -1 / (4 * A);
                double b = rho[face.Index] / 6;
                double c = A * rho[face.Index] * rho[face.Index] / 9;

                Indices[0] = face.GetVertex(0).Index;
                Indices[1] = face.GetVertex(1).Index;
                Indices[2] = face.GetVertex(2).Index;

                Quaternion[] e = new Quaternion[3];
                for (int i = 0; i < 3; i++)
                {
                    Vector3D eV = mesh.Vertices[Indices[(i + 2) % 3]].Traits.Position -
                                  mesh.Vertices[Indices[(i + 1) % 3]].Traits.Position;
                    e[i] = new Quaternion(eV, 0);
                }

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Quaternion q = a * e[i] * e[j] + b * (e[j] - e[i]) + c;
                        E[Indices[i], Indices[j]] += q;
                    }
                }

            }

            return E;
        }

        SparseMatrixQuaternion BuildCotLaplace()
        {
            SparseMatrixQuaternion Laplace = new SparseMatrixQuaternion(mesh.Vertices.Count, mesh.Vertices.Count);

            foreach (TriMesh.Face face in mesh.Faces)
            {
                foreach (TriMesh.HalfEdge hf in face.Halfedges)
                {
                    Vector3D a = hf.FromVertex.Traits.Position;
                    Vector3D b = hf.Next.FromVertex.Traits.Position;
                    Vector3D c = hf.Next.Next.FromVertex.Traits.Position;

                    int aIndex = hf.FromVertex.Index;
                    int bIndex = hf.Next.FromVertex.Index;
                    int cIndex = hf.Next.Next.FromVertex.Index;

                    Vector3D u1 = b - a;
                    Vector3D u2 = c - a;
                    double cotAlpha = u1.Dot(u2) / u1.Cross(u2).Length();

                    Laplace[bIndex, cIndex] -= cotAlpha / 2;
                    Laplace[cIndex, bIndex] -= cotAlpha / 2;

                    Laplace[cIndex, cIndex] += cotAlpha / 2;
                    Laplace[bIndex, bIndex] += cotAlpha / 2;

                }
            }

            return Laplace;
        }

        DenseMatrixQuaternion BuildOmega(DenseMatrixQuaternion lamda)
        {
            DenseMatrixQuaternion Omega = new DenseMatrixQuaternion(mesh.Vertices.Count, 1);

            TriMesh.Vertex[] index = new TriMesh.Vertex[3];

            foreach (TriMesh.Face face in mesh.Faces)
            {
                index[0] = face.GetVertex(2);
                index[1] = face.GetVertex(0);
                index[2] = face.GetVertex(1);

                for (int i = 0; i < 3; i++)
                {
                    Quaternion f0 = new Quaternion(index[(i + 0) % 3].Traits.Position, 0);
                    Quaternion f1 = new Quaternion(index[(i + 1) % 3].Traits.Position, 0);
                    Quaternion f2 = new Quaternion(index[(i + 2) % 3].Traits.Position, 0);

                    //Setting Orientation Swap it
                    TriMesh.Vertex aI = index[(i + 1) % 3];
                    TriMesh.Vertex bI = index[(i + 2) % 3];
                    if (aI.Index > bI.Index)
                    {
                        aI = index[(i + 2) % 3];
                        bI = index[(i + 1) % 3];
                    }

                    Quaternion aLamda = lamda[aI.Index, 0];
                    Quaternion bLamda = lamda[bI.Index, 0];
                    Quaternion e = new Quaternion(bI.Traits.Position, 0) - new Quaternion(aI.Traits.Position, 0);
                    Quaternion conj = aLamda.Conjugate();
                    Quaternion bconj = bLamda.Conjugate();
                    Quaternion partA = conj * e * aLamda;
                    Quaternion eTilde = (1f / 3f) * partA +
                                        (1f / 6f) * aLamda.Conjugate() * e * bLamda +
                                        (1f / 6f) * bLamda.Conjugate() * e * aLamda +
                                        (1f / 3f) * bLamda.Conjugate() * e * bLamda;

                    Vector3D u1 = index[(i + 1) % 3].Traits.Position - index[(i + 0) % 3].Traits.Position;
                    Vector3D u2 = index[(i + 2) % 3].Traits.Position - index[(i + 0) % 3].Traits.Position;

                    double cotAlpha = u1.Dot(u2) / u1.Cross(u2).Length();

                    Quaternion q = cotAlpha * eTilde / 2;

                    Omega[aI.Index, 0] -= q;
                    Omega[bI.Index, 0] += q;
                }

            }

            RemoveMean(ref Omega);
            return Omega;
        }

        DenseMatrixQuaternion ReadLambda(String path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            String line = null;

            DenseMatrixQuaternion qt = new DenseMatrixQuaternion(mesh.Vertices.Count, 1);
            char[] splitChar = new char[] { ' ' };
            int row = 0;

            while ((line = sr.ReadLine()) != null)
            {
                String[] token = line.Split(splitChar);

                double w = double.Parse(token[0]);
                double x = double.Parse(token[1]);
                double y = double.Parse(token[2]);
                double z = double.Parse(token[3]);

                Quaternion q = new Quaternion(x, y, z, w);

                qt[row, 0] = q;

                row++;
            }

            return qt;
        }

        #region Util
        void RemoveMean(ref DenseMatrixQuaternion x)
        {
            Quaternion mean = Quaternion.Zero;
            for (int i = 0; i < x.RowCount; i++)
            {
                mean += x[i, 0];
            }

            mean /= x.RowCount;

            for (int i = 0; i < x.RowCount; i++)
            {
                x[i, 0] -= mean;
            }

        }

        void NormalizeSolution(ref DenseMatrixQuaternion newVertices)
        {
            RemoveMean(ref newVertices);

            // Infinite Norm
            double r = 0;
            for (int i = 0; i < newVertices.RowCount; i++)
            {
                r = Math.Max(r, newVertices[i, 0].LengthSquared);
            }
            r = Math.Sqrt(r);

            for (int i = 0; i < newVertices.RowCount; i++)
            {
                newVertices[i, 0] /= r;
            }

        }

        void UpdatePosition(ref DenseMatrixQuaternion newVertices)
        {
            for (int i = 0; i < newVertices.RowCount; i++)
            {
                Quaternion v = newVertices[i, 0];
                Vector3D newPosition = new Vector3D(v.x, v.y, v.z);
                mesh.Vertices[i].Traits.Position = newPosition;
            }

        }

        #endregion

    }
}
