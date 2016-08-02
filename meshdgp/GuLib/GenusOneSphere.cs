using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class GenusOneSphere:GenusBaseGu
    {
        private int iterCount = 1000;
        private double step = 0.0001;

    


        public GenusOneSphere(TriMesh mesh)
        {
            iterCount = ConfigGu.Instance.IterCount;
            step = ConfigGu.Instance.Step;

        }

        public void MappingGauss(TriMesh mesh)
        {
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                Vector3D[] normals = TriMeshUtil.ComputeNormalVertex(mesh, EnumNormal.SphereInscribed);
                mesh.Vertices[i].Traits.Position = normals[i];
            }
            TriMeshUtil.SetUpNormalVertex(mesh);
        }

        public void MappingSphericalTuette(TriMesh mesh)
        {
            

            MappingGauss(mesh);
            for (int i = 0; i < iterCount; i++)
            {
                MappingSphericalTuetteOneStep(mesh);
            }

        }

        public void MappingSphericalTuetteOneStep(TriMesh mesh)
        {

            
            SparseMatrix sparse = LaplaceManager.Instance.BuildLaplaceGraph(mesh);

            double[][] lap = LaplaceManager.Instance.ComputeLaplacianBasic(sparse, mesh);

            Vector3D[] lapVec = new Vector3D[mesh.Vertices.Count];
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {

                lapVec[i] = new Vector3D(lap[0][i], lap[1][i], lap[2][i]);
            }

            Vector3D[] derivative = new Vector3D[mesh.Vertices.Count];
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                Vector3D normal = mesh.Vertices[i].Traits.Position.Normalize();
                normal = normal.Normalize();
                derivative[i] = lapVec[i] - lapVec[i].Dot(normal) * normal;
                mesh.Vertices[i].Traits.Position += derivative[i] * step;
            }


            TriMeshUtil.SetUpNormalVertex(mesh);
            OnChanged(EventArgs.Empty);
        }

        public double ComputeEnergyTuette(TriMesh mesh)
        {
            double energy = 0;

            //for (int i = 0; i < mesh.Vertices.Count; i++)
            //{
            //    foreach(TriMesh.Vertex v in mesh.Vertices[i].Vertices)
            //    {
            //        energy+= (mesh.Vertices[i].Traits.Position - v.Traits.Position).LengthSquared;
            //    }
            //}

            for (int i = 0; i < mesh.Edges.Count; i++)
            {
                energy += (mesh.Edges[i].Vertex0.Traits.Position - mesh.Edges[i].Vertex1.Traits.Position).LengthSquared;
               // Console.WriteLine("the step {0} :: {1}", i,energy);
            }
           // Console.WriteLine("total enery is: {0}", energy);
            return energy;
        }

        public double ComputeEnergyHarmonic(TriMesh mesh)
        {
            double energy = 0;

            // double[] cot=LaplaceManager.Instance.ComputeLaplacianCot
            List<double> cot = LaplaceBuilder.Instance.ComputeCotEdge(mesh);
            //for (int i = 0; i < mesh.Vertices.Count; i++)
            //{
            //    foreach (TriMesh.Vertex v in mesh.Vertices[i].Vertices)
            //    {
            //        energy += (mesh.Vertices[i].Traits.Position - v.Traits.Position).LengthSquared;
            //    }
            //}

            for (int i = 0; i < mesh.Edges.Count; i++)
            {
                energy += cot[i] * (mesh.Edges[i].Vertex0.Traits.Position - mesh.Edges[i].Vertex1.Traits.Position).LengthSquared;
            }

            return energy;
        }
    }
}
