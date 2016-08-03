using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public static class SegementationGrow
    {
        public static void GrowByVertexArea(TriMesh mesh)
        {
            Queue<TriMesh.Vertex> queue = new Queue<HalfEdgeMesh.Vertex>();
            double[] avgArea = new double[mesh.Vertices.Count];
            foreach (var v in mesh.Vertices)
            {
                avgArea[v.Index] = TriMeshUtil.ComputeAreaOneRing(v);
                if (v.Traits.SelectedFlag != 0)
                {
                    queue.Enqueue(v);
                }
            }
            
            double k = 0.896;
            while (queue.Count != 0)
            {
                TriMesh.Vertex center = queue.Dequeue();
                foreach (var round in center.Vertices)
                {
                    if (round.Traits.SelectedFlag == 0)
                    {
                        if (Math.Abs(avgArea[center.Index] -
                            avgArea[round.Index]) < k * avgArea[center.Index])
                        {
                            round.Traits.SelectedFlag = center.Traits.SelectedFlag;
                            queue.Enqueue(round);
                        }
                    }
                }
            }
        }

        public static void Vertex2Face(TriMesh mesh)
        {
            foreach (var v in mesh.Vertices)
            {
                if (v.Traits.SelectedFlag != 0)
                {
                    v.HalfEdge.Face.Traits.SelectedFlag = v.Traits.SelectedFlag;
                }
            }
        }


        public static void GrowByFaceAngle(TriMesh mesh)
        {
            Queue<TriMesh.Face> queue = new Queue<HalfEdgeMesh.Face>();
            Vector3D[] normal = TriMeshUtil.ComputeNormalFace(mesh);
            foreach (var face in mesh.Faces)
            {
                if (face.Traits.SelectedFlag != 0)
                {
                    queue.Enqueue(face);
                }
            }

            double k = 0.449;
            while (queue.Count != 0)
            {
                TriMesh.Face center = queue.Dequeue();
                foreach (var round in center.Faces)
                {
                    if (round.Traits.SelectedFlag == 0)
                    {
                        if (normal[center.Index].Dot(normal[round.Index]) > k)
                        {
                            round.Traits.SelectedFlag = center.Traits.SelectedFlag;
                            queue.Enqueue(round);
                        }
                    }
                }
            }
        }

        public static void KMean(TriMesh mesh)
        {
            Dictionary<int, Cluster> dict = new Dictionary<int, Cluster>();
            Queue<TriMesh.Face> queue = new Queue<HalfEdgeMesh.Face>();
            foreach (var face in mesh.Faces)
            {
                if (face.Traits.SelectedFlag != 0)
                {
                    dict[face.Traits.SelectedFlag] = new Cluster();
                    dict[face.Traits.SelectedFlag].Add(TriMeshUtil.GetMidPoint(face));
                    queue.Enqueue(face);
                }
            }

            while (queue.Count != 0)
            {
                TriMesh.Face center = queue.Dequeue();
                foreach (var round in center.Faces)
                {
                    if (round.Traits.SelectedFlag == 0)
                    {
                        int index = GetNearest(round, dict);
                        dict[index].Add(TriMeshUtil.GetMidPoint(round));
                        round.Traits.SelectedFlag = (byte)index;
                        queue.Enqueue(round);
                    }
                }
            }
        }

        static int GetNearest(TriMesh.Face face, Dictionary<int, Cluster> dict)
        {
            Vector3D center = TriMeshUtil.GetMidPoint(face);
            int nearestIndex = -1;
            double nearest = double.MaxValue;
            foreach (var item in dict)
            {
                double dist = Vector3D.Distance(center, item.Value.Center);
                if (dist < nearest)
                {
                    nearestIndex = item.Key;
                    nearest = dist;
                }
            }
            return nearestIndex;
        }

        public static void RegionGrow(TriMesh mesh)
        {
            Vector3D[] normal = TriMeshUtil.ComputeNormalFace(mesh);
            double[] area = TriMeshUtil.ComputeAreaFace(mesh);
            SortedList<double, TriMesh.Face> sort = new SortedList<double, HalfEdgeMesh.Face>();
            foreach (var center in mesh.Faces)
            {
                List<double> list = new List<double>();
                double sum = 0;
                foreach (var round in center.Faces)
                {
                    double l = (normal[round.Index] - normal[center.Index]).Length();// * area[round.Index]);
                    sum += l;
                    list.Add(l);
                }
                sum /= list.Count;
                double d = 0;
                foreach (var item in list)
                {
                    d += Math.Pow(item - sum, 2);
                }
                d /= list.Count;
                if (sum < 1 && d < 0.1)
                {
                    sort.Add(sum, center);
                }
            }

            Stack<TriMesh.Face> stack = new Stack<HalfEdgeMesh.Face>();
            Dictionary<int, Cluster> dict = new Dictionary<int, Cluster>();
            double k = 1;
            int flag = 0;

            do
            {
                if (stack.Count == 0)
                {
                    bool run = false;
                    if (flag > 100) return;
                    for (int i = flag; i < sort.Count; i++)
                    {
                        TriMesh.Face next = sort.Values[i];
                        if (next.Traits.SelectedFlag == 0)
                        {
                            flag++;
                            next.Traits.SelectedFlag = (byte)flag;
                            stack.Push(next);
                            dict[flag] = new Cluster();
                            dict[flag].Add(normal[sort.Values[i].Index]);
                            run = true;
                            break;
                        }
                    }
                    if (!run) return;
                }
                TriMesh.Face center = stack.Pop();
                
                foreach (var round in center.Faces)
                {
                    if (round.Traits.SelectedFlag == 0 &&
                        (normal[round.Index] - normal[center.Index]).Length() < k &&
                        (normal[round.Index] - dict[flag].Center).Length() < k)
                    {
                        dict[flag].Add(normal[round.Index]);
                        round.Traits.SelectedFlag = (byte)flag;
                        stack.Push(round);
                    }
                }
            } while (true) ;
        }

        class Cluster
        {
            public Vector3D Center;
            public int Count;

            public void Add(Vector3D v)
            {
                Center = (v + Center * Count) / (Count + 1);
                if (Center.Length() > 1.02)
                {

                }
                Count++;
            }
        }
    }
}
