using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public static partial class TetMeshUtil
        
    {
        public static Vector3D ComputeNormal(TetFace face)
        {
            if (face.Tetras.Count != 1)
            {
                return Vector3D.Zero;
            }
            Vector3D[] v = new Vector3D[3];
            for (int i = 0; i < 3; i++)
            {
                v[i] = face.Vertices[i].Pos;
            }
            Vector3D n = (v[1] - v[0]).Cross(v[2] - v[0]).Normalize();
            foreach (var vt in face.Tetras[0].Vertices)
            {
                bool top = true;
                foreach (var vf in face.Vertices)
                {
                    if (vt == vf)
                    {
                        top = false;
                        break;
                    }
                }
                if (top && n.Dot(vt.Pos - v[0]) > 0)
                {
                    n = -n;
                    TetVertex temp = face.Vertices[0];
                    face.Vertices[0] = face.Vertices[2];
                    face.Vertices[2] = temp;
                    break;
                }
            }

            return n;
        }

        public static Vector3D[] ComputeNormalUniformWeight(TetMesh mesh)
        {
            Vector3D[] vn = new Vector3D[mesh.Vertices.Count];
            foreach (var face in mesh.Faces)
            {
                Vector3D fn = ComputeNormal(face);
                if (fn != Vector3D.Zero)
                {
                    foreach (var v in face.Vertices)
                    {
                        vn[v.Index] += fn;
                    }
                }
            }

            for (int i = 0; i < vn.Length; i++)
            {
                vn[i] = vn[i].Normalize();
            }
            return vn;
        }

        public static double ComputeEdgeAvgLength(TetMesh mesh)
        {
            double sum = 0;
            foreach (var edge in mesh.Edges)
            {
                sum += Vector3D.Distance(edge.Vertices[0].Pos, edge.Vertices[1].Pos);
            }
            return sum / mesh.Edges.Count;
        }

        public static Vector3D GetMidPoint(TetFace face)
        {
            return (face.Vertices[0].Pos + face.Vertices[1].Pos + face.Vertices[2].Pos) / 3;
        }

        public static PlaneIntersectionType Intersect(Plane plane, TetVertex v)
        {
            return Collision.PlaneIntersectsPoint(ref plane, ref v.Pos);
        }

        public static PlaneIntersectionType Intersect(Plane plane, Tetrahedron tet)
        {
            int back = 0;
            int front = 0;
            foreach (var v in tet.Vertices)
            {
                switch (Intersect(plane,v))
                {
                    case PlaneIntersectionType.Back:
                        back++;
                        break;
                    case PlaneIntersectionType.Front:
                        front++;
                        break;
                    case PlaneIntersectionType.Intersecting:
                        break;
                    default:
                        break;
                }
            }
            if (back == 0)
            {
                return PlaneIntersectionType.Front;
            }
            else if (front == 0)
            {
                return PlaneIntersectionType.Back;
            }
            else
            {
                return PlaneIntersectionType.Intersecting;
            }
        }
    }
}
