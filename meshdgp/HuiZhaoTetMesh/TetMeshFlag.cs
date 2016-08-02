using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public static class TetMeshFlag
    {
        public static Tetrahedron CheckBoundary(TetFace face)
        {
            Tetrahedron t0 = face.Tetras.Count > 0 && face.Tetras[0].Flag > 0 ? face.Tetras[0] : null;
            Tetrahedron t1 = face.Tetras.Count > 1 && face.Tetras[1].Flag > 0 ? face.Tetras[1] : null;
            if (t0 != null && t1 == null)
            {
                return t0;
            }
            else if (t0 == null && t1 != null)
            {
                return t1;
            }
            else
            {
                return null;
            }
        }

        public static Vector3D ComputeNormal(TetFace face)
        {
            if (face.OnBoundary)
            {
                return TetMeshUtil.ComputeNormal(face);
            }
            Tetrahedron selected = CheckBoundary(face);
            if (selected == null)
            {
                return Vector3D.Zero;
            }

            Vector3D[] v = new Vector3D[3];
            for (int i = 0; i < 3; i++)
            {
                v[i] = face.Vertices[i].Pos;
            }
            Vector3D n = (v[1] - v[0]).Cross(v[0] - v[2]).Normalize();
            foreach (var vt in selected.Vertices)
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
    }
}
