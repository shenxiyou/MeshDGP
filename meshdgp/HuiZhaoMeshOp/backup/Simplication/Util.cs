using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public static class Util
    {
        public static double Solve(ref Matrix4D m, ref Vector3D v)
        {
            double det = m.SubDeterminate(0, 1, 2, 4, 5, 6, 8, 9, 10);
            if (det != 0)
            {
                v.x = (-1d / det) * (m.SubDeterminate(1, 2, 3, 5, 6, 7, 9, 10, 11));
                v.y = (1d / det) * (m.SubDeterminate(0, 2, 3, 4, 6, 7, 8, 10, 11));
                v.z = (-1d / det) * (m.SubDeterminate(0, 1, 3, 4, 5, 7, 8, 9, 11));
            }
            return det;
        }

        public static bool Contains(TriMesh.Edge edge, Vector3D v)
        {
            Vector3D from = edge.Vertex0.Traits.Position;
            Vector3D to = edge.Vertex1.Traits.Position;
            Vector3D dir = (from - to).Normalize();
            double length = (from - to).Length();
            return ((v - to).Normalize() - dir).Length() < 0.0001f;
            //return (v - to).Normalize() == dir &&
            //    (v - from).Length() < length &&
            //    (v - to).Length() < length;
        }
    }
}
