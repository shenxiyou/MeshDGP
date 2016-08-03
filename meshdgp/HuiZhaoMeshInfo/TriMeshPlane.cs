using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class TriMeshUtil
    {
        public static Nullable<Vector3D> Intersect(Plane plane, TriMesh.Edge edge, double threshold)
        {
            Vector3D p1 = edge.Vertex0.Traits.Position;
            Vector3D p2 = edge.Vertex1.Traits.Position;
            double d1 = GetDistance(plane, p1);
            double d2 = GetDistance(plane, p2);
            if (Math.Abs(d1) > threshold && Math.Abs(d2) > threshold && d1 * d2 < 0)
            {
                d1 = Math.Abs(d1);
                d2 = Math.Abs(d2);
                return p1 + (p2 - p1) * d1 / (d1 + d2);
            }
            else
            {
                return null;
            }
        }

        public static double GetDistance(Plane plane, Vector3D point)
        {
            return plane.Normal.Dot(point) + plane.D;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.Plane"/> and a <see cref="SlimMath.Plane"/>.
        /// </summary>
        /// <param name="plane1">The first plane to test.</param>
        /// <param name="plane2">The second plane to test.</param>
        /// <param name="line">When the method completes, contains the line of intersection
        /// as a <see cref="SlimMath.Ray"/>, or a zero ray if there was no intersection.</param>
        /// <returns>Whether the two objects intersected.</returns>
        /// <remarks>
        /// Although a ray is set to have an origin, the ray returned by this method is really
        /// a line in three dimensions which has no real origin. The ray is considered valid when
        /// both the positive direction is used and when the negative direction is used.
        /// </remarks>
        public static bool PlaneIntersectsPlane(ref Plane plane1, ref Plane plane2, out Ray line)
        {
            //Source: Real-Time Collision Detection by Christer Ericson
            //Reference: Page 207

            Vector3D direction;
            Vector3D.Cross(ref plane1.Normal, ref plane2.Normal, out direction);

            //If direction is the zero vector, the planes are parallel and possibly
            //coincident. It is not an intersection. The dot product will tell us.
            //double denominator;
            //Vector3D.Dot(ref direction, ref direction, out denominator);

            ////We assume the planes are normalized, therefore the denominator
            ////only serves as a parallel and coincident check. Otherwise we need
            ////to deivide the point by the denominator.
            //if (Math.Abs(denominator) < Utilities.ZeroTolerance)
            //{
            //    line = new Ray();
            //    return false;
            //}

            Vector3D point;
            Vector3D temp = plane1.D * plane2.Normal - plane2.D * plane1.Normal;
            Vector3D.Cross(ref temp, ref direction, out point);

            line.Position = point;
            line.Direction = direction;
            line.Direction.Normalize();

            return true;
        }
    }
}
