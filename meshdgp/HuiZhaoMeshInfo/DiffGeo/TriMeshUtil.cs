using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public static partial class TriMeshUtil
    {
        public static AxisAlignedBox ComputeBoundingBox(TriMesh mesh)
        {
            Vector3D max = Vector3D.MinValue;
            Vector3D min = Vector3D.MaxValue;
            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                for (int i = 0; i < 3; ++i)
                {
                    if (v.Traits.Position[i] > max[i])
                    {
                        max[i] = v.Traits.Position[i];
                    }
                    if (v.Traits.Position[i] < min[i])
                    {
                        min[i] = v.Traits.Position[i];
                    }
                }
            }

            return new AxisAlignedBox { Max = max, Min = min };
        }

        public static Sphere ComputeBoundingSphere(TriMesh mesh)
        {
            return ComputeBoundingSphere(mesh, ComputeBoundingBox(mesh));
        }

        public static Sphere ComputeBoundingSphere(TriMesh mesh, AxisAlignedBox box)
        {
            Vector3D mean = (box.Max + box.Min) * 0.5d;

            double distanceSquared, maxDistanceSquared = 0d;
            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                distanceSquared = Math.Pow((v.Traits.Position - mean).Length(), 2d);
                if (distanceSquared > maxDistanceSquared)
                {
                    maxDistanceSquared = distanceSquared;
                }
            }
            return new Sphere { Center = mean, Radius = (float)Math.Sqrt(maxDistanceSquared) };
        }

        

        public static double ComputeEdgeAvgLength(TriMesh mesh)
        {
            double sumLength = 0;
            foreach (TriMesh.Edge item in mesh.Edges)
            {

                sumLength += ComputeEdgeLength(item);
            }
            return sumLength / mesh.Edges.Count;
        }

        public static double ComputeInradius(TriMesh.Face face)
        {
            Vector3D a = face.GetVertex(0).Traits.Position;
            Vector3D b = face.GetVertex(1).Traits.Position;
            Vector3D c = face.GetVertex(2).Traits.Position;

            double u = (a - b).Length();
            double v = (b - c).Length();
            double w = (c - a).Length();

            return 0.5 * Math.Sqrt(((u + v - w) * (w + u - v) * (v + w - u)) / (u + v + w));
        }

      

        public static double Sum(double[] arr)
        {
            double sum = 0;
            foreach (var item in arr)
            {
                sum += item;
            }
            return sum;
        }

        public static T[] ToArray<T>(IEnumerable<T> c)
        {
            List<T> list = new List<T>();
            foreach (var item in c)
            {
                list.Add(item);
            }
            return list.ToArray();
        }
    }


    public struct Triple<T>
    {
        public T T0;
        public T T1;
        public T T2;
    }
}
