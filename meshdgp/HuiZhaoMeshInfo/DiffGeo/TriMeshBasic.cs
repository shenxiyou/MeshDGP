


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace GraphicResearchHuiZhao 
{

    public partial class TriMeshUtil
    {
        public static Vector3D MinPosition(TriMesh mesh)
        {
            Vector3D position;
            position.x = double.MaxValue;
            position.y = double.MaxValue;
            position.z = double.MaxValue;

            foreach (TriMesh.Vertex item in mesh.Vertices)
            {
                if (position.x > item.Traits.Position.x)
                {
                    position.x = item.Traits.Position.x;
                }
                if (position.y > item.Traits.Position.y)
                {
                    position.y = item.Traits.Position.y;
                }
                if (position.z > item.Traits.Position.z)
                {
                    position.z = item.Traits.Position.z;
                }
            }

            return position;
        }

        public static  Vector3D MaxCoord(TriMesh mesh)
        {
            Vector3D maxCoord = new Vector3D(double.MinValue, double.MinValue, double.MinValue);
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                Vector3D v = mesh.Vertices[i].Traits.Position;
                maxCoord = Vector3D.Max(maxCoord, v);
            }
            return maxCoord;
        }
        public static Vector3D MinCoord(TriMesh mesh)
        {
            Vector3D minCoord = new Vector3D(double.MaxValue, double.MaxValue, double.MaxValue);
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                Vector3D v =  mesh.Vertices[i].Traits.Position;
                minCoord = Vector3D.Min(minCoord, v);
            }
            return minCoord;
        }

        public static  void ScaleToUnit(TriMesh mesh, double scale)
        {

            Vector3D max = new Vector3D(float.NegativeInfinity,
                                        float.NegativeInfinity,
                                        float.NegativeInfinity);
            Vector3D min = new Vector3D(float.PositiveInfinity,
                                        float.PositiveInfinity,
                                        float.PositiveInfinity);
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
            Vector3D d = max - min;
            double s = (d.x > d.y) ? d.x : d.y;
            s = (s > d.z) ? s : d.z;
            if (s <= 0) return;
            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                v.Traits.Position /= s;
                v.Traits.Position *= scale;
            }
        }

        public static  void MoveToCenter(TriMesh mesh)
        {
            Vector3D max = new Vector3D(float.NegativeInfinity,
                                        float.NegativeInfinity,
                                        float.NegativeInfinity);
            Vector3D min = new Vector3D(float.PositiveInfinity,
                                        float.PositiveInfinity,
                                        float.PositiveInfinity);
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
            Vector3D center = (max + min) / 2.0;
            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                v.Traits.Position -= center;
            } 
        }

        public static void MoveToCenter(List<TriMesh.Vertex> vertice)
        {
            Vector3D max = new Vector3D(float.NegativeInfinity,
                                        float.NegativeInfinity,
                                        float.NegativeInfinity);
            Vector3D min = new Vector3D(float.PositiveInfinity,
                                        float.PositiveInfinity,
                                        float.PositiveInfinity);
            foreach (TriMesh.Vertex v in vertice)
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
            Vector3D center = (max + min) / 2.0;
            foreach (TriMesh.Vertex v in vertice)
            {
                v.Traits.Position -= center;
            } 
        }


        public static Vector3D GetCenter(List<TriMesh.Vertex> vertice)
        {
            Vector3D max = new Vector3D(float.NegativeInfinity,
                                        float.NegativeInfinity,
                                        float.NegativeInfinity);
            Vector3D min = new Vector3D(float.PositiveInfinity,
                                        float.PositiveInfinity,
                                        float.PositiveInfinity);
            foreach (TriMesh.Vertex v in vertice)
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
            Vector3D center = (max + min) / 2.0;

            return center;
            
        }



        public static  int GroupVertice(TriMesh mesh)
        {
            foreach (TriMesh.Vertex vertex in mesh.Vertices)
            {
                if (vertex.Traits.SelectedFlag != 0)
                {
                    vertex.Traits.SelectedFlag = 255;
                }
            }
            byte id = 0;
            Queue queue = new Queue();
            for (int i = 0; i < mesh.Vertices.Count; i++)
                if (mesh.Vertices[i].Traits.SelectedFlag == 255)
                {
                    id++;
                    mesh.Vertices[i].Traits.SelectedFlag = id;
                    queue.Enqueue(i);
                    while (queue.Count > 0)
                    {
                        int curr = (int)queue.Dequeue();
                        foreach (TriMesh.Vertex neighbor in mesh.Vertices[curr].Vertices)
                        {
                            if (mesh.Vertices[neighbor.Index].Traits.SelectedFlag == 255)
                            {
                                mesh.Vertices[neighbor.Index].Traits.SelectedFlag = id;
                                queue.Enqueue(neighbor.Index);
                            }
                        }
                    }
                }

            return id - 1;
        }











        public static void FixIndex(TriMesh mesh)
        {
            int i = 0;
            foreach (TriMesh.Vertex vrtex in mesh.Vertices)
            {
                vrtex.Index = i;
                i++;
            }
            i = 0;
            foreach (TriMesh.HalfEdge hf in mesh.HalfEdges)
            {
                hf.Index = i;
                i++;
            }
            i = 0;
            foreach (TriMesh.Edge edge in mesh.Edges)
            {
                edge.Index = i;
                i++;
            }
            i = 0;
            foreach (TriMesh.Face face in mesh.Faces)
            {
                face.Index = i;
                i++;
            }
        }


        public delegate T Func<T>(TriMesh.Vertex v);

        public static T[]  GetFromVertices<T>(TriMesh.Face face, Func<T> func)
        {
            T[] arr = new T[3];
            int i = 0;
            foreach (var hf in face.Halfedges)
            {
                arr[i++] = func(hf.FromVertex);
            }
            return arr;
        }
    }
}
