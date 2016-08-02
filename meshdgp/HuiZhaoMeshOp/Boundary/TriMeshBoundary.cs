using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class TriMeshModify 
    {
        public static void RepairSimpleHoles(TriMesh mesh)
        {
            
            List<List<TriMesh.HalfEdge>> holes = TriMeshUtil.RetrieveBoundaryEdgeAll(mesh);

            foreach (List<TriMesh.HalfEdge> hole in holes)
            {
                for (int i = 2; i < hole.Count; i++)
                {
                    mesh.Faces.AddTriangles(hole[0].ToVertex, hole[i - 1].ToVertex, hole[i].ToVertex);
                }
            }

            TriMeshUtil.FixIndex(mesh);
        }

        public static void RepaireHoleTwo(List<TriMesh.Vertex> hole)
        {
            TriMesh mesh = (TriMesh)hole[0].Mesh;
            Vector3D center = Vector3D.Zero;

            for (int i = 0; i < hole.Count; i++)
            {
                center += hole[i].Traits.Position;
            }
            center /= hole.Count;

            TriMesh.Vertex vertex = mesh.Vertices.Add(new VertexTraits(center));

            if (hole.Count >= 3)
            {
                TriMesh.Vertex[] faceVertices = new TriMesh.Vertex[3];

                for (int i = 0; i < hole.Count; i++)
                {
                    faceVertices[0] = vertex;
                    faceVertices[1] = hole[i];
                    faceVertices[2] = hole[(i + 1) % hole.Count];
                    mesh.Faces.AddTriangles(faceVertices);

                }


            }

            TriMeshUtil.FixIndex(mesh);
        }
        public static void RepaireHole(List<TriMesh.Vertex> hole)
        {
            TriMesh mesh = (TriMesh)hole[0].Mesh;
            if (hole.Count >= 3)
            {
                mesh.Faces.AddTriangles(hole.ToArray());
            }

            TriMeshUtil.FixIndex(mesh);
        }


        public static void RepaireOneHole(TriMesh mesh)
        {
            List<TriMesh.Vertex> hole = TriMeshUtil.RetrieveBoundarySingle(mesh);
            RepaireHole(hole);
            TriMeshUtil.FixIndex(mesh);
        }



        public static void RepaireAllHole(TriMesh mesh)
        {
            List<List<TriMesh.Vertex>> holes = TriMeshUtil.RetrieveBoundaryAllVertex(mesh);
            for (int i = 0; i < holes.Count; i++)
            {
                RepaireHoleTwo(holes[i]);
            }

            TriMeshUtil.FixIndex(mesh);
        }

        public static void RepairHole(TriMesh mesh)
        {
            List<TriMesh.Vertex> hole = new List<TriMesh.Vertex>();
            hole = TriMeshUtil.RetrieveBoundarySingle(mesh);
            TriMesh.Vertex[] faceVertices = new TriMesh.Vertex[3];
            for (int i = 0; i < hole.Count - 2; i++)
            {
                faceVertices[0] = mesh.Vertices[hole[0].Index];
                faceVertices[1] = mesh.Vertices[hole[i + 1].Index];
                faceVertices[2] = mesh.Vertices[hole[i + 2].Index];

                mesh.Faces.AddTriangles(faceVertices);
            }

            TriMeshUtil.FixIndex(mesh);

        }

        public static void RepairComplexHole(TriMesh mesh)
        {
            List<TriMesh.Vertex> hole = new List<TriMesh.Vertex>();
            TriMesh.Vertex[] faceVertices = new TriMesh.Vertex[3];




            while (true)//此循环只限单洞修补
            {
                //计算边界边的平均值
                #region 计算边界边的平均值
                hole = TriMeshUtil.RetrieveBoundarySingle(mesh);
                double aver_l = 0;
                double[] edgeLength = new double[hole.Count];
                for (int i = 0; i < hole.Count; i++)
                {
                    if (i == hole.Count - 1)
                    {
                        edgeLength[i] = Math.Sqrt(Math.Pow(hole[i].Traits.Position.x - hole[0].Traits.Position.x, 2) + Math.Pow(hole[i].Traits.Position.y - hole[0].Traits.Position.y, 2) + Math.Pow(hole[i].Traits.Position.z - hole[0].Traits.Position.z, 2));
                        break;
                    }
                    edgeLength[i] = Math.Sqrt(Math.Pow(hole[i].Traits.Position.x - hole[i + 1].Traits.Position.x, 2) + Math.Pow(hole[i].Traits.Position.y - hole[i + 1].Traits.Position.y, 2) + Math.Pow(hole[i].Traits.Position.z - hole[i + 1].Traits.Position.z, 2));
                }
                for (int i = 0; i < hole.Count; i++)
                {
                    aver_l += edgeLength[i];
                }
                aver_l = aver_l / hole.Count;
                #endregion

                hole = TriMeshUtil.RetrieveBoundarySingle(mesh);

                //计算角度值
                #region 计算角度值
                double[] angle = new double[hole.Count];
                double a = 0, b = 0, c = 0;
                for (int i = 0; i < hole.Count; i++)
                {
                    if (i == 0)
                    {
                        a = Math.Sqrt(Math.Pow(hole[hole.Count - 1].Traits.Position.x - hole[i + 1].Traits.Position.x, 2) + Math.Pow(hole[hole.Count - 1].Traits.Position.y - hole[i + 1].Traits.Position.y, 2) + Math.Pow(hole[hole.Count - 1].Traits.Position.z - hole[i + 1].Traits.Position.z, 2));
                        b = Math.Sqrt(Math.Pow(hole[i].Traits.Position.x - hole[i + 1].Traits.Position.x, 2) + Math.Pow(hole[i].Traits.Position.y - hole[i + 1].Traits.Position.y, 2) + Math.Pow(hole[i].Traits.Position.z - hole[i + 1].Traits.Position.z, 2));
                        c = Math.Sqrt(Math.Pow(hole[hole.Count - 1].Traits.Position.x - hole[i].Traits.Position.x, 2) + Math.Pow(hole[hole.Count - 1].Traits.Position.y - hole[i].Traits.Position.y, 2) + Math.Pow(hole[hole.Count - 1].Traits.Position.z - hole[i].Traits.Position.z, 2));
                        angle[i] = Math.Acos((b * b + c * c - a * a) / (2 * b * c));
                    }
                    else if (i == hole.Count - 1)
                    {
                        a = Math.Sqrt(Math.Pow(hole[i - 1].Traits.Position.x - hole[0].Traits.Position.x, 2) + Math.Pow(hole[i - 1].Traits.Position.y - hole[0].Traits.Position.y, 2) + Math.Pow(hole[i - 1].Traits.Position.z - hole[0].Traits.Position.z, 2));
                        b = Math.Sqrt(Math.Pow(hole[i].Traits.Position.x - hole[0].Traits.Position.x, 2) + Math.Pow(hole[i].Traits.Position.y - hole[0].Traits.Position.y, 2) + Math.Pow(hole[i].Traits.Position.z - hole[0].Traits.Position.z, 2));
                        c = Math.Sqrt(Math.Pow(hole[i - 1].Traits.Position.x - hole[i].Traits.Position.x, 2) + Math.Pow(hole[i - 1].Traits.Position.y - hole[i].Traits.Position.y, 2) + Math.Pow(hole[i - 1].Traits.Position.z - hole[i].Traits.Position.z, 2));
                        angle[i] = Math.Acos((b * b + c * c - a * a) / (2 * b * c));
                    }
                    else
                    {
                        a = Math.Sqrt(Math.Pow(hole[i - 1].Traits.Position.x - hole[i + 1].Traits.Position.x, 2) + Math.Pow(hole[i - 1].Traits.Position.y - hole[i + 1].Traits.Position.y, 2) + Math.Pow(hole[i - 1].Traits.Position.z - hole[i + 1].Traits.Position.z, 2));
                        b = Math.Sqrt(Math.Pow(hole[i].Traits.Position.x - hole[i + 1].Traits.Position.x, 2) + Math.Pow(hole[i].Traits.Position.y - hole[i + 1].Traits.Position.y, 2) + Math.Pow(hole[i].Traits.Position.z - hole[i + 1].Traits.Position.z, 2));
                        c = Math.Sqrt(Math.Pow(hole[i - 1].Traits.Position.x - hole[i].Traits.Position.x, 2) + Math.Pow(hole[i - 1].Traits.Position.y - hole[i].Traits.Position.y, 2) + Math.Pow(hole[i - 1].Traits.Position.z - hole[i].Traits.Position.z, 2));
                        angle[i] = Math.Acos((b * b + c * c - a * a) / (2 * b * c));
                    }
                }
                #endregion


                //取得最小的角度
                #region 取得最小的角度
                int min = 0;
                for (int i = 0; i < hole.Count - 1; i++)
                {
                    if (angle[i] < angle[i + 1])
                        min = i;
                    else
                        min = i + 1;
                }
                #endregion


                //求最小角度对应的边长
                #region 求最小角度对应的边长
                double temp = 0;
                if (min == hole.Count - 1)
                {
                    temp = Math.Sqrt(Math.Pow(hole[min - 1].Traits.Position.x - hole[0].Traits.Position.x, 2) + Math.Pow(hole[min - 1].Traits.Position.y - hole[0].Traits.Position.y, 2) + Math.Pow(hole[min - 1].Traits.Position.z - hole[0].Traits.Position.z, 2));
                }
                else if (min == 0)
                {
                    temp = Math.Sqrt(Math.Pow(hole[hole.Count - 1].Traits.Position.x - hole[min + 1].Traits.Position.x, 2) + Math.Pow(hole[hole.Count - 1].Traits.Position.y - hole[min + 1].Traits.Position.y, 2) + Math.Pow(hole[hole.Count - 1].Traits.Position.z - hole[min + 1].Traits.Position.z, 2));

                }
                else
                {
                    temp = Math.Sqrt(Math.Pow(hole[min - 1].Traits.Position.x - hole[min + 1].Traits.Position.x, 2) + Math.Pow(hole[min - 1].Traits.Position.y - hole[min + 1].Traits.Position.y, 2) + Math.Pow(hole[min - 1].Traits.Position.z - hole[min + 1].Traits.Position.z, 2));
                }
                #endregion


                //判断最小角对应的边与平均边界边长，并进行修补
                #region 判断最小角对应的边与平均边界边长，并进行修补
                if (temp > aver_l)
                {
                    if (min == hole.Count - 1)
                    {
                        faceVertices[0] = mesh.Vertices[hole[min].Index];
                        faceVertices[1] = mesh.Vertices[hole[0].Index];
                        faceVertices[2] = mesh.Vertices[hole[min - 1].Index];
                        mesh.Faces.AddTriangles(faceVertices);
                    }
                    else if (min == 0)
                    {
                        faceVertices[0] = mesh.Vertices[hole[min].Index];
                        faceVertices[1] = mesh.Vertices[hole[min + 1].Index];
                        faceVertices[2] = mesh.Vertices[hole[hole.Count - 1].Index];
                        mesh.Faces.AddTriangles(faceVertices);
                    }
                    else
                    {

                        faceVertices[0] = mesh.Vertices[hole[min].Index];
                        faceVertices[1] = mesh.Vertices[hole[min + 1].Index];
                        faceVertices[2] = mesh.Vertices[hole[min - 1].Index];
                        mesh.Faces.AddTriangles(faceVertices);
                    }
                }

                else
                {
                    if (min == hole.Count - 1)
                    {
                        Vector3D midPosition;
                        midPosition = (hole[min - 1].Traits.Position + hole[0].Traits.Position) / 2;
                        mesh.Vertices.Add(new VertexTraits(midPosition.x, midPosition.y, midPosition.z));

                        faceVertices[0] = mesh.Vertices[hole[min].Index];
                        faceVertices[1] = mesh.Vertices[mesh.Vertices.Count - 1];
                        faceVertices[2] = mesh.Vertices[hole[min - 1].Index];
                        mesh.Faces.AddTriangles(faceVertices);

                        faceVertices[0] = mesh.Vertices[hole[min].Index];
                        faceVertices[1] = mesh.Vertices[hole[0].Index];
                        faceVertices[2] = mesh.Vertices[mesh.Vertices.Count - 1];
                        mesh.Faces.AddTriangles(faceVertices);
                    }
                    else if (min == 0)
                    {
                        Vector3D midPosition;
                        midPosition = (hole[hole.Count - 1].Traits.Position + hole[min + 1].Traits.Position) / 2;
                        mesh.Vertices.Add(new VertexTraits(midPosition.x, midPosition.y, midPosition.z));

                        faceVertices[0] = mesh.Vertices[hole[min].Index];
                        faceVertices[1] = mesh.Vertices[mesh.Vertices.Count - 1];
                        faceVertices[2] = mesh.Vertices[hole[hole.Count - 1].Index];
                        mesh.Faces.AddTriangles(faceVertices);

                        faceVertices[0] = mesh.Vertices[hole[min].Index];
                        faceVertices[1] = mesh.Vertices[hole[min + 1].Index];
                        faceVertices[2] = mesh.Vertices[mesh.Vertices.Count - 1];
                        mesh.Faces.AddTriangles(faceVertices);
                    }
                    else
                    {
                        Vector3D midPosition;
                        midPosition = (hole[min - 1].Traits.Position + hole[min + 1].Traits.Position) / 2;
                        mesh.Vertices.Add(new VertexTraits(midPosition.x, midPosition.y, midPosition.z));

                        faceVertices[0] = mesh.Vertices[hole[min].Index];
                        faceVertices[1] = mesh.Vertices[mesh.Vertices.Count - 1];
                        faceVertices[2] = mesh.Vertices[hole[min - 1].Index];
                        mesh.Faces.AddTriangles(faceVertices);

                        faceVertices[0] = mesh.Vertices[hole[min].Index];
                        faceVertices[1] = mesh.Vertices[hole[min + 1].Index];
                        faceVertices[2] = mesh.Vertices[mesh.Vertices.Count - 1];
                        mesh.Faces.AddTriangles(faceVertices);
                    }
                }
                #endregion


                //更新边界点，并判断空洞是否完整
                #region 更新边界点，并判断空洞是否完整

                TriMeshUtil.FixIndex(mesh);
                hole = TriMeshUtil.RetrieveBoundarySingle(mesh);
                if (hole.Count < 3)
                    break;
                #endregion
            } 

        }


        public static void BoundaryExpand(TriMesh mesh)
        {
            double length = TriMeshUtil.ComputeEdgeAvgLength(mesh);
            List<List<TriMesh.HalfEdge>> holes = TriMeshUtil.RetrieveBoundaryEdgeAll(mesh);
            foreach (var hole in holes)
            {
            TriMesh.Vertex[] arr = new HalfEdgeMesh.Vertex[hole.Count];
            for (int i = 0; i < hole.Count; i++)
            {
                Vector3D normal = Vector3D.UnitX;
                if (hole[i].Opposite.Face != null)
                {
                    normal = TriMeshUtil.ComputeNormalFace(hole[i].Opposite.Face);
                }
                Vector3D toPos = hole[i].ToVertex.Traits.Position;
                Vector3D fromPos = hole[i].FromVertex.Traits.Position;
                Vector3D hfDir = toPos - fromPos;
                Vector3D hfMid = (toPos + fromPos) / 2;
                Vector3D pos = hfMid + normal.Cross(hfDir).Normalize() * length;
                arr[i] = mesh.Vertices.Add(new VertexTraits(pos));
            }
            for (int i = 0; i < hole.Count; i++)
            {
                int next = (i + 1) % hole.Count;
                TriMesh.Face face= mesh.Faces.Add(arr[i], 
                                                  hole[i].ToVertex, 
                                                  hole[next].ToVertex);
                face.Traits.SelectedFlag = 1;
                face = mesh.Faces.Add(arr[i], 
                                      hole[next].ToVertex, 
                                      arr[next]);
                face.Traits.SelectedFlag = 1;
            }
            }
        }

        public static void BoundaryShrink(TriMesh mesh)
        {
            List<List<TriMesh.HalfEdge>> holes = TriMeshUtil.RetrieveBoundaryEdgeAll(mesh);
            foreach (var hole in holes)
            {
                foreach (var hf in hole)
                {
                    TriMeshModify.RemoveVertex(hf.FromVertex);
                }
            }
        }

    }
}
