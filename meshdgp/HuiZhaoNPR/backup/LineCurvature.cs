//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace GraphicResearchHuiZhao 
//{
//    public static class  LineCurvature
//    {
//        //计算曲率导数
//        public static double ComputeDCurv(Vector3D viewPoint, TriMesh.Vertex vertex, Vector4D dcurv)
//        {
//            double value = ComputeAngel(viewPoint,vertex);

//            double dkn = dcurv.x * Math.Pow(Math.Cos(value), 3)
//                       + 3 * dcurv.y * Math.Pow(Math.Cos(value), 2) * Math.Sin(ComputeAngel(viewPoint, vertex))
//                       + 3 * dcurv.z * Math.Cos(value) * Math.Pow(Math.Sin(value), 2)
//                       + dcurv.w* Math.Pow(Math.Cos(value), 3);
//            return dkn;
//        }

//        //计算模型的正前面
//        public static double ComputFont(Vector3D viewPoint, TriMesh.Face face)
//        {
//            Vector3D dir = viewPoint - face.GetVertex(0).Traits.Position;
//            Vector3D normal = TriMeshUtil.ComputeNormalFace(face);
//            double value = dir.Dot(normal);
//            return value;
//        }

//        //计算视线向量与投影向量的夹角的cos值
//        public static double ComputeAngel(Vector3D viewPoint, TriMesh.Vertex vertex)
//        {
            
//            Vector3D v = vertex.Traits.Position - viewPoint;
//            Vector3D w = ComputeProject(viewPoint,vertex);
//            double value1 = v.Dot(w);
//            double value2 = v.Length() * w.Length();
//            return value1 / value2;
//        }


//        //计算该点主曲率方向与切线方向的夹角
//        public static double ComputeAngel(TriMesh.Vertex vertex, Vector3D e)
//        {
//            double value1 = vertex.Traits.Position.Dot(e);
//            double value2 = vertex.Traits.Position.Length() * e.Length();
//            double angle = Math.Acos(value1 / value2) - Math.PI;
//            return angle;
//        }


//        //计算法向量与视线向量的cos值
//        public static double ComputeCos(Vector3D viewPoint, TriMesh.Vertex vertex)
//        {
//            Vector3D v = vertex.Traits.Position - viewPoint;
//            v = v / v.Length();
//            Vector3D normalVertex = TriMeshUtil.ComputeNormalAreaWeight(vertex);
//            Vector3D normal = normalVertex / normalVertex.Length();
//            return normal.Dot(v);
//        }

//        //计算视点向量的投影向量
//        public static Vector3D ComputeProject(Vector3D viewPoint, TriMesh.Vertex vertex)
//        {
//            Vector3D normal = TriMeshUtil.ComputeNormalAreaWeight(vertex);

//            Vector3D v = vertex.Traits.Position - viewPoint;
//            v = v / v.Length();
//            Vector3D w = (v - normal * ComputeCos(viewPoint,vertex)) / Math.Sin(Math.Acos(ComputeCos(viewPoint,vertex)));
//            return w;
//        }

//        //计算径向曲率
//        public static double ComputeRadicalCurvature(Vector3D viewPoint, TriMesh.Vertex vertex, double minCur, double maxCur)
//        {
//            double k1, k2, kn;
//            k1 = maxCur;
//            k2 = maxCur;
//            kn = k1 * Math.Pow(Math.Cos(ComputeAngel(viewPoint,vertex)), 2)
//               + k2 * Math.Pow(Math.Sin(ComputeAngel(viewPoint,vertex)), 2);
//            return kn;
//        }

//        public static double ComputeValue(Vector3D viewPoint, TriMesh.Edge edge)
//        {
//            double value1, value2;
//            //考虑到边界边的情况
//            if (edge.Face0 == null || edge.Face1 == null)
//            {
//                return 0;
//            }
//            //正常情况 
//            Vector3D normal = TriMeshUtil.ComputeNormalFace(edge.Face0);
//            Vector3D dir = TriMeshUtil.GetMidPoint(edge) - viewPoint;
//            value1 = dir.Dot(normal);
//            normal = TriMeshUtil.ComputeNormalFace(edge.Face1);
//            value2 = dir.Dot(normal);
//            return value1 * value2;
//        }



//        //构建新模型
//        public static TriMesh CreateMesh(TriMesh mesh,Vector3D viewPoint)
//        {
//            TriMesh.Vertex[] map = new HalfEdgeMesh.Vertex[mesh.Vertices.Count];
//            TriMesh newMesh = new TriMesh();
//            for (int i = 0; i < mesh.Faces.Count; i++)
//            {
//                if (ComputFont(viewPoint,mesh.Faces[i]) > 0)
//                {
//                    List<TriMesh.Vertex> list = new List<TriMesh.Vertex>();
//                    foreach (var v in mesh.Faces[i].Vertices)
//                    {
//                        if (map[v.Index] == null)
//                        {
//                            map[v.Index] = newMesh.Vertices.Add(new VertexTraits(v.Traits.Position));
//                        }
//                        list.Add(map[v.Index]);
//                    }
//                    newMesh.Faces.AddTriangles(list.ToArray());
//                }
//            }
//            TriMeshUtil.FixIndex(newMesh);
//            TriMeshUtil.SetUpNormalVertex(newMesh);
//            return newMesh;
//        }



//        public static double ComputeDkn(TriMesh.Vertex vertex, Vector4D dcurv, Vector3D e)
//        {
//            double value = LineCurvature.ComputeAngel(vertex, e);

//            double dkn = dcurv.x * Math.Pow(Math.Cos(value), 3)
//                       + 3 * dcurv.y * Math.Pow(Math.Cos(value), 2) * Math.Sin(value)
//                       + 3 * dcurv.z * Math.Cos(value) * Math.Pow(Math.Sin(value), 2)
//                       + dcurv.w * Math.Pow(Math.Cos(value), 3);
//            return dkn;
//        }

//    }
//}
