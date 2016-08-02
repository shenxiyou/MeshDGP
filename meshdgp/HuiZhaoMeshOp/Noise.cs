using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public partial  class TriMeshModify
    {
        

        public static void AddNoise(TriMesh Mesh,double threshold)
        {
            Random random = new Random();
            double avglength =  TriMeshUtil.ComputeEdgeAvgLength(Mesh);
            threshold *= avglength;
            foreach (TriMesh.Vertex item in Mesh.Vertices)
            {
                Vector3D normal = item.Traits.Normal.Normalize();
                double scale = threshold * (random.NextDouble() - 0.5f);
                item.Traits.Position.x += normal.x * scale;
                item.Traits.Position.y += normal.y * scale;
                item.Traits.Position.z += normal.z * scale;
            }
        }

        public static void AddNoiseTwo(TriMesh Mesh, double rate, int iter)
        {
            Random random = new Random();
            double[] scale = new double[Mesh.Vertices.Count];
            double max = TriMeshUtil.ComputeBoundingSphere(Mesh).Radius * rate; 
            double avglength =  TriMeshUtil.ComputeEdgeAvgLength(Mesh); 
            foreach (var v in Mesh.Vertices)
            {
                scale[v.Index] = max * (random.NextDouble() - 0.5f) * 2;
            }

            for (int i = 0; i < max / avglength * iter; i++)
            {
                foreach (var v in Mesh.Vertices)
                {
                    double sum = 0;
                    int count = 0;
                    foreach (var r in v.Vertices)
                    {
                        sum += scale[r.Index];
                        count++;
                    }
                    double weight = 0.5;
                    scale[v.Index] = scale[v.Index] * weight + sum / count * (1 - weight);
                }
            } 
            foreach (var v in Mesh.Vertices)
            {
                Vector3D normal = v.Traits.Normal.Normalize();
                v.Traits.Position += normal * scale[v.Index];
            }
        }

    }

}
