/*
* Copyright (c) 2007-2010 SlimDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace GraphicResearchHuiZhao
{
    /*
     * This class is organized so that the least complex objects come first so that the least
     * complex objects will have the most methods in most cases. Note that not all shapes exist
     * at this time and not all shapes have a corresponding struct. Only the objects that have
     * a corresponding struct should come first in naming and in parameter order. The order of
     * complexity is as follows:
     * 
     * 1. Point
     * 2. Ray
     * 3. Segment
     * 4. Plane
     * 5. Triangle
     * 6. Polygon (polygon that lies on a single plane)
     * 7. Tetrahedron
     * 8. Box
     * 9. AABox
     * 10. Sphere
     * 11. Ellipsoid
     * 12. Cylinder
     * 13. Cone
     * 14. Capsule
     * 15. Torus
     * 16. Polyhedron
     * 17. Frustum
    */

    /// <summary>
    /// Contains static methods to help in determining intersections, containment, etc.
    /// </summary>
    public static class Collision
    {
        /// <summary>
        /// Determines the closest point between a point and a segment.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <param name="segment1">The starting point of the segment to test.</param>
        /// <param name="segment2">The ending point of the segment to test.</param>
        /// <param name="result">When the method completes, contains the closest point between the two objects.</param>
        public static void ClosestPointOnSegmentToPoint(ref Vector3D segment1, ref Vector3D segment2, ref Vector3D point, out Vector3D result)
        {
            Vector3D ab = segment2 - segment1;
            double t = Vector3D.Dot(point - segment1, ab) / Vector3D.Dot(ab, ab);

            if (t < 0.0f)
                t = 0.0f;

            if (t > 1.0f)
                t = 1.0f;

            result = segment1 + t * ab;
        }

        /// <summary>
        /// Determines the closest point between a <see cref="SlimMath.Plane"/> and a point.
        /// </summary>
        /// <param name="plane">The plane to test.</param>
        /// <param name="point">The point to test.</param>
        /// <param name="result">When the method completes, contains the closest point between the two objects.</param>
        public static void ClosestPointOnPlaneToPoint(ref Plane plane, ref Vector3D point, out Vector3D result)
        {
            //Source: Real-Time Collision Detection by Christer Ericson
            //Reference: Page 126

            double dot;
            Vector3D.Dot(ref plane.Normal, ref point, out dot);
            double t = dot - plane.D;

            result = point - (t * plane.Normal);
        }

        /// <summary>
        /// Determines the closest point between a point and a triangle.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <param name="vertex1">The first vertex to test.</param>
        /// <param name="vertex2">The second vertex to test.</param>
        /// <param name="vertex3">The third vertex to test.</param>
        /// <param name="result">When the method completes, contains the closest point between the two objects.</param>
        public static void ClosestPointOnTriangleToPoint(ref Vector3D vertex1, ref Vector3D vertex2, ref Vector3D vertex3, ref Vector3D point, out Vector3D result)
        {
            //Source: Real-Time Collision Detection by Christer Ericson
            //Reference: Page 136

            //Check if P in vertex region outside A
            Vector3D ab = vertex2 - vertex1;
            Vector3D ac = vertex3 - vertex1;
            Vector3D ap = point - vertex1;

            double d1 = Vector3D.Dot(ab, ap);
            double d2 = Vector3D.Dot(ac, ap);
            if (d1 <= 0.0f && d2 <= 0.0f)
            {
                result = vertex1; //Barycentric coordinates (1,0,0)
                return;
            }

            //Check if P in vertex region outside B
            Vector3D bp = point - vertex2;
            double d3 = Vector3D.Dot(ab, bp);
            double d4 = Vector3D.Dot(ac, bp);
            if (d3 >= 0.0f && d4 <= d3)
            {
                result = vertex2; // barycentric coordinates (0,1,0)
                return;
            }

            //Check if P in edge region of AB, if so return projection of P onto AB
            double vc = d1 * d4 - d3 * d2;
            if (vc <= 0.0f && d1 >= 0.0f && d3 <= 0.0f)
            {
                double v = d1 / (d1 - d3);
                result = vertex1 + v * ab; //Barycentric coordinates (1-v,v,0)
                return;
            }

            //Check if P in vertex region outside C
            Vector3D cp = point - vertex3;
            double d5 = Vector3D.Dot(ab, cp);
            double d6 = Vector3D.Dot(ac, cp);
            if (d6 >= 0.0f && d5 <= d6)
            {
                result = vertex3; //Barycentric coordinates (0,0,1)
                return;
            }

            //Check if P in edge region of AC, if so return projection of P onto AC
            double vb = d5 * d2 - d1 * d6;
            if (vb <= 0.0f && d2 >= 0.0f && d6 <= 0.0f)
            {
                double w = d2 / (d2 - d6);
                result = vertex1 + w * ac; //Barycentric coordinates (1-w,0,w)
                return;
            }

            //Check if P in edge region of BC, if so return projection of P onto BC
            double va = d3 * d6 - d5 * d4;
            if (va <= 0.0f && (d4 - d3) >= 0.0f && (d5 - d6) >= 0.0f)
            {
                double w = (d4 - d3) / ((d4 - d3) + (d5 - d6));
                result = vertex2 + w * (vertex3 - vertex2); //Barycentric coordinates (0,1-w,w)
                return;
            }

            //P inside face region. Compute Q through its barycentric coordinates (u,v,w)
            double denom = 1.0f / (va + vb + vc);
            double v2 = vb * denom;
            double w2 = vc * denom;
            result = vertex1 + ab * v2 + ac * w2; //= u*vertex1 + v*vertex2 + w*vertex3, u = va * denom = 1.0f - v - w
        }

        /// <summary>
        /// Determines the closest point between a <see cref="SlimMath.BoundingBox"/> and a point.
        /// </summary>
        /// <param name="box">The box to test.</param>
        /// <param name="point">The point to test.</param>
        /// <param name="result">When the method completes, contains the closest point between the two objects.</param>
        public static void ClosestPointOnBoxToPoint(ref BoundingBox box, ref Vector3D point, out Vector3D result)
        {
            //Source: Real-Time Collision Detection by Christer Ericson
            //Reference: Page 130

            Vector3D temp;
            Vector3D.Max(ref point, ref box.Minimum, out temp);
            Vector3D.Min(ref temp, ref box.Maximum, out result);
        }

        /// <summary>
        /// Determines the closest point between a <see cref="SlimMath.BoundingSphere"/> and a point.
        /// </summary>
        /// <param name="sphere"></param>
        /// <param name="point">The point to test.</param>
        /// <param name="result">When the method completes, contains the closest point between the two objects;
        /// or, if the point is directly in the center of the sphere, contains <see cref="SlimMath.Vector3D.Zero"/>.</param>
        public static void ClosestPointOnSphereToPoint(ref BoundingSphere sphere, ref Vector3D point, out Vector3D result)
        {
            //Source: Jorgy343
            //Reference: None

            //Get the unit direction from the sphere's center to the point.
            Vector3D.Subtract(ref point, ref sphere.Center, out result);
            result.Normalize();

            //Multiply the unit direction by the sphere's radius to get a vector
            //the length of the sphere.
            result *= sphere.Radius;

            //Add the sphere's center to the direction to get a point on the sphere.
            result += sphere.Center;
        }

        /// <summary>
        /// Determines the closest point between a <see cref="SlimMath.BoundingSphere"/> and a <see cref="SlimMath.BoundingSphere"/>.
        /// </summary>
        /// <param name="sphere1">The first sphere to test.</param>
        /// <param name="sphere2">The second sphere to test.</param>
        /// <param name="result">When the method completes, contains the closest point between the two objects;
        /// or, if the point is directly in the center of the sphere, contains <see cref="SlimMath.Vector3D.Zero"/>.</param>
        /// <remarks>
        /// If the two spheres are overlapping, but not directly ontop of each other, the closest point
        /// is the 'closest' point of intersection. This can also be considered is the deepest point of
        /// intersection.
        /// </remarks>
        public static void ClosestPointOnSphereToSphere(ref BoundingSphere sphere1, ref BoundingSphere sphere2, out Vector3D result)
        {
            //Source: Jorgy343
            //Reference: None

            //Get the unit direction from the first sphere's center to the second sphere's center.
            Vector3D.Subtract(ref sphere2.Center, ref sphere1.Center, out result);
            result.Normalize();

            //Multiply the unit direction by the first sphere's radius to get a vector
            //the length of the first sphere.
            result *= sphere1.Radius;

            //Add the first sphere's center to the direction to get a point on the first sphere.
            result += sphere1.Center;
        }

        /// <summary>
        /// Determines the distance between a <see cref="SlimMath.Plane"/> and a point.
        /// </summary>
        /// <param name="plane">The plane to test.</param>
        /// <param name="point">The point to test.</param>
        /// <returns>The distance between the two objects.</returns>
        public static double DistancePlanePoint(ref Plane plane, ref Vector3D point)
        {
            //Source: Real-Time Collision Detection by Christer Ericson
            //Reference: Page 127

            double dot;
            Vector3D.Dot(ref plane.Normal, ref point, out dot);
            return dot - plane.D;
        }

        /// <summary>
        /// Determines the distance between a <see cref="SlimMath.BoundingBox"/> and a point.
        /// </summary>
        /// <param name="box">The box to test.</param>
        /// <param name="point">The point to test.</param>
        /// <returns>The distance between the two objects.</returns>
        public static double DistanceBoxPoint(ref BoundingBox box, ref Vector3D point)
        {
            //Source: Real-Time Collision Detection by Christer Ericson
            //Reference: Page 131

            double distance = 0f;

            if (point.x < box.Minimum.x)
                distance += (box.Minimum.x - point.x) * (box.Minimum.x - point.x);
            if (point.x > box.Maximum.x)
                distance += (point.x - box.Maximum.x) * (point.x - box.Maximum.x);

            if (point.y < box.Minimum.y)
                distance += (box.Minimum.y - point.y) * (box.Minimum.y - point.y);
            if (point.y > box.Maximum.y)
                distance += (point.y - box.Maximum.y) * (point.y - box.Maximum.y);

            if (point.z < box.Minimum.z)
                distance += (box.Minimum.z - point.z) * (box.Minimum.z - point.z);
            if (point.z > box.Maximum.z)
                distance += (point.z - box.Maximum.z) * (point.z - box.Maximum.z);

            return (double)Math.Sqrt(distance);
        }

        /// <summary>
        /// Determines the distance between a <see cref="SlimMath.BoundingBox"/> and a <see cref="SlimMath.BoundingBox"/>.
        /// </summary>
        /// <param name="box1">The first box to test.</param>
        /// <param name="box2">The second box to test.</param>
        /// <returns>The distance between the two objects.</returns>
        public static double DistanceBoxBox(ref BoundingBox box1, ref BoundingBox box2)
        {
            double distance = 0f;

            //Distance for X.
            if (box1.Minimum.x > box2.Maximum.x)
            {
                double delta = box2.Maximum.x - box1.Minimum.x;
                distance += delta * delta;
            }
            else if (box2.Minimum.x > box1.Maximum.x)
            {
                double delta = box1.Maximum.x - box2.Minimum.x;
                distance += delta * delta;
            }

            //Distance for Y.
            if (box1.Minimum.y > box2.Maximum.y)
            {
                double delta = box2.Maximum.y - box1.Minimum.y;
                distance += delta * delta;
            }
            else if (box2.Minimum.y > box1.Maximum.y)
            {
                double delta = box1.Maximum.y - box2.Minimum.y;
                distance += delta * delta;
            }

            //Distance for Z.
            if (box1.Minimum.z > box2.Maximum.z)
            {
                double delta = box2.Maximum.z - box1.Minimum.z;
                distance += delta * delta;
            }
            else if (box2.Minimum.z > box1.Maximum.z)
            {
                double delta = box1.Maximum.z - box2.Minimum.z;
                distance += delta * delta;
            }

            return (double)Math.Sqrt(distance);
        }

        /// <summary>
        /// Determines the distance between a <see cref="SlimMath.BoundingSphere"/> and a point.
        /// </summary>
        /// <param name="sphere">The sphere to test.</param>
        /// <param name="point">The point to test.</param>
        /// <returns>The distance between the two objects.</returns>
        public static double DistanceSpherePoint(ref BoundingSphere sphere, ref Vector3D point)
        {
            //Source: Jorgy343
            //Reference: None

            double distance;
            Vector3D.Distance(ref sphere.Center, ref point, out distance);
            distance -= sphere.Radius;

            return Math.Max(distance, 0f);
        }

        /// <summary>
        /// Determines the distance between a <see cref="SlimMath.BoundingSphere"/> and a <see cref="SlimMath.BoundingSphere"/>.
        /// </summary>
        /// <param name="sphere1">The first sphere to test.</param>
        /// <param name="sphere2">The second sphere to test.</param>
        /// <returns>The distance between the two objects.</returns>
        public static double DistanceSphereSphere(ref BoundingSphere sphere1, ref BoundingSphere sphere2)
        {
            //Source: Jorgy343
            //Reference: None

            double distance;
            Vector3D.Distance(ref sphere1.Center, ref sphere2.Center, out distance);
            distance -= sphere1.Radius + sphere2.Radius;

            return Math.Max(distance, 0f);
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.Ray"/> and a point.
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="point">The point to test.</param>
        /// <returns>Whether the two objects intersect.</returns>
        public static bool RayIntersectsPoint(ref Ray ray, ref Vector3D point)
        {
            //Source: RayIntersectsSphere
            //Reference: None

            Vector3D m;
            Vector3D.Subtract(ref ray.Position, ref point, out m);

            //Same thing as RayIntersectsSphere except that the radius of the sphere (point)
            //is the epsilon for zero.
            double b = Vector3D.Dot(m, ray.Direction);
            double c = Vector3D.Dot(m, m) - Utilities.ZeroTolerance;

            if (c > 0f && b > 0f)
                return false;

            double discriminant = b * b - c;

            if (discriminant < 0f)
                return false;

            return true;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.Ray"/> and a <see cref="SlimMath.Ray"/>.
        /// </summary>
        /// <param name="ray1">The first ray to test.</param>
        /// <param name="ray2">The second ray to test.</param>
        /// <param name="point">When the method completes, contains the point of intersection,
        /// or <see cref="SlimMath.Vector3D.Zero"/> if there was no intersection.</param>
        /// <returns>Whether the two objects intersect.</returns>
        /// <remarks>
        /// This method performs a ray vs ray intersection test based on the following formula
        /// from Goldman.
        /// <code>s = det([o₂ − o₁, d₂, d₁ ⨯ d₂]) / ‖d₁ ⨯ d₂‖²</code>
        /// <code>t = det([o₂ − o₁, d₁, d₁ ⨯ d₂]) / ‖d₁ ⨯ d₂‖²</code>
        /// Where o₁ is the position of the first ray, o₂ is the position of the second ray,
        /// d₁ is the normalized direction of the first ray, d₂ is the normalized direction
        /// of the second ray, det denotes the determinant of a matrix, ⨯ denotes the cross
        /// product, [ ] denotes a matrix, and ‖ ‖ denotes the length or magnitude of a vector.
        /// </remarks>
        public static bool RayIntersectsRay(ref Ray ray1, ref Ray ray2, out Vector3D point)
        {
            //Source: Real-Time Rendering, Third Edition
            //Reference: Page 780
            
            Vector3D cross;

            Vector3D.Cross(ref ray1.Direction, ref ray2.Direction, out cross);
            double denominator = cross.Length();
            
            //Lines are parallel.
            if (Math.Abs(denominator) < Utilities.ZeroTolerance)
            {
                //Lines are parallel and on top of each other.
                if (Math.Abs(ray2.Position.x - ray1.Position.x) < Utilities.ZeroTolerance &&
                    Math.Abs(ray2.Position.y - ray1.Position.y) < Utilities.ZeroTolerance &&
                    Math.Abs(ray2.Position.z - ray1.Position.z) < Utilities.ZeroTolerance)
                {
                    point = Vector3D.Zero;
                    return true;
                }
            }

            denominator = denominator * denominator;

            //3x3 matrix for the first ray.
            double m11 = ray2.Position.x - ray1.Position.x;
            double m12 = ray2.Position.y - ray1.Position.y;
            double m13 = ray2.Position.z - ray1.Position.z;
            double m21 = ray2.Direction.x;
            double m22 = ray2.Direction.y;
            double m23 = ray2.Direction.z;
            double m31 = cross.x;
            double m32 = cross.y;
            double m33 = cross.z;

            //Determinant of first matrix.
            double dets =
                m11 * m22 * m33 +
                m12 * m23 * m31 +
                m13 * m21 * m32 -
                m11 * m23 * m32 -
                m12 * m21 * m33 -
                m13 * m22 * m31;

            //3x3 matrix for the second ray.
            m21 = ray1.Direction.x;
            m22 = ray1.Direction.y;
            m23 = ray1.Direction.z;

            //Determinant of the second matrix.
            double dett =
                m11 * m22 * m33 +
                m12 * m23 * m31 +
                m13 * m21 * m32 -
                m11 * m23 * m32 -
                m12 * m21 * m33 -
                m13 * m22 * m31;

            //t values of the point of intersection.
            double s = dets / denominator;
            double t = dett / denominator;

            //The points of intersection.
            Vector3D point1 = ray1.Position + (s * ray1.Direction);
            Vector3D point2 = ray2.Position + (t * ray2.Direction);

            //If the points are not equal, no intersection has occured.
            if (Math.Abs(point2.x - point1.x) > Utilities.ZeroTolerance ||
                Math.Abs(point2.y - point1.y) > Utilities.ZeroTolerance ||
                Math.Abs(point2.z - point1.z) > Utilities.ZeroTolerance)
            {
                point = Vector3D.Zero;
                return false;
            }

            point = point1;
            return true;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.Ray"/> and a <see cref="SlimMath.Plane"/>.
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="plane">The plane to test.</param>
        /// <param name="distance">When the method completes, contains the distance of the intersection,
        /// or 0 if there was no intersection.</param>
        /// <returns>Whether the two objects intersect.</returns>
        public static bool RayIntersectsPlane(ref Ray ray, ref Plane plane, out double distance)
        {
            //Source: Real-Time Collision Detection by Christer Ericson
            //Reference: Page 175

            double direction;
            Vector3D.Dot(ref plane.Normal, ref ray.Direction, out direction);

            if (Math.Abs(direction) < Utilities.ZeroTolerance)
            {
                distance = 0f;
                return false;
            }

            double position;
            Vector3D.Dot(ref plane.Normal, ref ray.Position, out position);
            distance = (-plane.D - position) / direction;

            if (distance < 0f)
            {
                if (distance < -Utilities.ZeroTolerance)
                {
                    distance = 0;
                    return false;
                }

                distance = 0f;
            }

            return true;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.Ray"/> and a <see cref="SlimMath.Plane"/>.
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="plane">The plane to test</param>
        /// <param name="point">When the method completes, contains the point of intersection,
        /// or <see cref="SlimMath.Vector3D.Zero"/> if there was no intersection.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public static bool RayIntersectsPlane(ref Ray ray, ref Plane plane, out Vector3D point)
        {
            //Source: Real-Time Collision Detection by Christer Ericson
            //Reference: Page 175

            double distance;
            if (!RayIntersectsPlane(ref ray, ref plane, out distance))
            {
                point = Vector3D.Zero;
                return false;
            }

            point = ray.Position + (ray.Direction * distance);
            return true;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.Ray"/> and a triangle.
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="vertex1">The first vertex of the triangle to test.</param>
        /// <param name="vertex2">The second vertex of the triagnle to test.</param>
        /// <param name="vertex3">The third vertex of the triangle to test.</param>
        /// <param name="distance">When the method completes, contains the distance of the intersection,
        /// or 0 if there was no intersection.</param>
        /// <returns>Whether the two objects intersected.</returns>
        /// <remarks>
        /// This method tests if the ray intersects either the front or back of the triangle.
        /// If the ray is parallel to the triangle's plane, no intersection is assumed to have
        /// happened. If the intersection of the ray and the triangle is behind the origin of
        /// the ray, no intersection is assumed to have happened. In both cases of assumptions,
        /// this method returns false.
        /// </remarks>
        public static bool RayIntersectsTriangle(ref Ray ray, ref Vector3D vertex1, ref Vector3D vertex2, ref Vector3D vertex3, out double distance)
        {
            //Source: Fast Minimum Storage Ray / Triangle Intersection
            //Reference: http://www.cs.virginia.edu/~gfx/Courses/2003/ImageSynthesis/papers/Acceleration/Fast%20MinimumStorage%20RayTriangle%20Intersection.pdf

            //Compute vectors along two edges of the triangle.
            Vector3D edge1, edge2;

            //Edge 1
            edge1.x = vertex2.x - vertex1.x;
            edge1.y = vertex2.y - vertex1.y;
            edge1.z = vertex2.z - vertex1.z;

            //Edge2
            edge2.x = vertex3.x - vertex1.x;
            edge2.y = vertex3.y - vertex1.y;
            edge2.z = vertex3.z - vertex1.z;

            //Cross product of ray direction and edge2 - first part of determinant.
            Vector3D directioncrossedge2;
            directioncrossedge2.x = (ray.Direction.y * edge2.z) - (ray.Direction.z * edge2.y);
            directioncrossedge2.y = (ray.Direction.z * edge2.x) - (ray.Direction.x * edge2.z);
            directioncrossedge2.z = (ray.Direction.x * edge2.y) - (ray.Direction.y * edge2.x);

            //Compute the determinant.
            double determinant;
            //Dot product of edge1 and the first part of determinant.
            determinant = (edge1.x * directioncrossedge2.x) + (edge1.y * directioncrossedge2.y) + (edge1.z * directioncrossedge2.z);

            //If the ray is parallel to the triangle plane, there is no collision.
            //This also means that we are not culling, the ray may hit both the
            //back and the front of the triangle.
            if (determinant > -Utilities.ZeroTolerance && determinant < Utilities.ZeroTolerance)
            {
                distance = 0f;
                return false;
            }

            double inversedeterminant = 1.0f / determinant;

            //Calculate the U parameter of the intersection point.
            Vector3D distanceVector = new Vector3D();
            distanceVector.x = ray.Position.x - vertex1.x;
            distanceVector.y = ray.Position.y - vertex1.y;
            distanceVector.z = ray.Position.z - vertex1.z;

            double triangleU;
            triangleU = (distanceVector.x * directioncrossedge2.x) + (distanceVector.y * directioncrossedge2.y) + (distanceVector.z * directioncrossedge2.z);
            triangleU *= inversedeterminant;

            //Make sure it is inside the triangle.
            if (triangleU < 0f || triangleU > 1f)
            {
                distance = 0f;
                return false;
            }

            //Calculate the V parameter of the intersection point.
            Vector3D distancecrossedge1;
            distancecrossedge1.x = (distanceVector.y * edge1.z) - (distanceVector.z * edge1.y);
            distancecrossedge1.y = (distanceVector.z * edge1.x) - (distanceVector.x * edge1.z);
            distancecrossedge1.z = (distanceVector.x * edge1.y) - (distanceVector.y * edge1.x);

            double triangleV;
            triangleV = ((ray.Direction.x * distancecrossedge1.x) + (ray.Direction.y * distancecrossedge1.y)) + (ray.Direction.z * distancecrossedge1.z);
            triangleV *= inversedeterminant;

            //Make sure it is inside the triangle.
            if (triangleV < 0f || triangleU + triangleV > 1f)
            {
                distance = 0f;
                return false;
            }

            //Compute the distance along the ray to the triangle.
            double raydistance;
            raydistance = (edge2.x * distancecrossedge1.x) + (edge2.y * distancecrossedge1.y) + (edge2.z * distancecrossedge1.z);
            raydistance *= inversedeterminant;

            //Is the triangle behind the ray origin?
            if (raydistance < 0f)
            {
                distance = 0f;
                return false;
            }

            distance = raydistance;
            return true;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.Ray"/> and a triangle.
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="vertex1">The first vertex of the triangle to test.</param>
        /// <param name="vertex2">The second vertex of the triagnle to test.</param>
        /// <param name="vertex3">The third vertex of the triangle to test.</param>
        /// <param name="point">When the method completes, contains the point of intersection,
        /// or <see cref="SlimMath.Vector3D.Zero"/> if there was no intersection.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public static bool RayIntersectsTriangle(ref Ray ray, ref Vector3D vertex1, ref Vector3D vertex2, ref Vector3D vertex3, out Vector3D point)
        {
            double distance;
            if (!RayIntersectsTriangle(ref ray, ref vertex1, ref vertex2, ref vertex3, out distance))
            {
                point = Vector3D.Zero;
                return false;
            }

            point = ray.Position + (ray.Direction * distance);
            return true;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.Ray"/> and a <see cref="SlimMath.BoundingBox"/>.
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="box">The box to test.</param>
        /// <param name="distance">When the method completes, contains the distance of the intersection,
        /// or 0 if there was no intersection.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public static bool RayIntersectsBox(ref Ray ray, ref BoundingBox box, out double distance)
        {
            //Source: Real-Time Collision Detection by Christer Ericson
            //Reference: Page 179

            distance = 0f;
            double tmax = double.MaxValue;

            if (Math.Abs(ray.Direction.x) < Utilities.ZeroTolerance)
            {
                if (ray.Position.x < box.Minimum.x || ray.Position.x > box.Maximum.x)
                {
                    distance = 0f;
                    return false;
                }
            }
            else
            {
                double inverse = 1.0f / ray.Direction.x;
                double t1 = (box.Minimum.x - ray.Position.x) * inverse;
                double t2 = (box.Maximum.x - ray.Position.x) * inverse;

                if (t1 > t2)
                {
                    double temp = t1;
                    t1 = t2;
                    t2 = temp;
                }

                distance = Math.Max(t1, distance);
                tmax = Math.Min(t2, tmax);

                if (distance > tmax)
                {
                    distance = 0f;
                    return false;
                }
            }

            if (Math.Abs(ray.Direction.y) < Utilities.ZeroTolerance)
            {
                if (ray.Position.y < box.Minimum.y || ray.Position.y > box.Maximum.y)
                {
                    distance = 0f;
                    return false;
                }
            }
            else
            {
                double inverse = 1.0f / ray.Direction.y;
                double t1 = (box.Minimum.y - ray.Position.y) * inverse;
                double t2 = (box.Maximum.y - ray.Position.y) * inverse;

                if (t1 > t2)
                {
                    double temp = t1;
                    t1 = t2;
                    t2 = temp;
                }

                distance = Math.Max(t1, distance);
                tmax = Math.Min(t2, tmax);

                if (distance > tmax)
                {
                    distance = 0f;
                    return false;
                }
            }

            if (Math.Abs(ray.Direction.z) < Utilities.ZeroTolerance)
            {
                if (ray.Position.z < box.Minimum.z || ray.Position.z > box.Maximum.z)
                {
                    distance = 0f;
                    return false;
                }
            }
            else
            {
                double inverse = 1.0f / ray.Direction.z;
                double t1 = (box.Minimum.z - ray.Position.z) * inverse;
                double t2 = (box.Maximum.z - ray.Position.z) * inverse;

                if (t1 > t2)
                {
                    double temp = t1;
                    t1 = t2;
                    t2 = temp;
                }

                distance = Math.Max(t1, distance);
                tmax = Math.Min(t2, tmax);

                if (distance > tmax)
                {
                    distance = 0f;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.Ray"/> and a <see cref="SlimMath.Plane"/>.
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="box">The box to test.</param>
        /// <param name="point">When the method completes, contains the point of intersection,
        /// or <see cref="SlimMath.Vector3D.Zero"/> if there was no intersection.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public static bool RayIntersectsBox(ref Ray ray, ref BoundingBox box, out Vector3D point)
        {
            double distance;
            if (!RayIntersectsBox(ref ray, ref box, out distance))
            {
                point = Vector3D.Zero;
                return false;
            }

            point = ray.Position + (ray.Direction * distance);
            return true;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.Ray"/> and a <see cref="SlimMath.BoundingSphere"/>.
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="sphere">The sphere to test.</param>
        /// <param name="distance">When the method completes, contains the distance of the intersection,
        /// or 0 if there was no intersection.</param>
        /// <returns>Whether the two objects intersected.</returns>
        /// <remarks>
        /// <para>
        /// This method uses the following math to compute the intersection:
        /// ‖x − c‖² = r²           Equation of sphere
        /// x = s + td              Equation of ray
        /// 
        /// Solve for t
        /// ‖s + td − c‖² = r²      Substitute equation of ray into equation of sphere
        /// v ≝ s − c
        /// ‖v + td‖² = r²
        /// v² + 2v⋅td + t²d² = r²
        /// d²t² + (2v⋅d)t + (v² − r²) = 0
        /// t² + (2v⋅d)t + (v² − r²) = 0    If d is a normalized vector
        /// 
        /// Quadratic equation gives us
        /// t = (−(2v⋅d) ± √((2v⋅d)² − 4(v² − r)²)) / 2
        /// t = −(v⋅d) ± √((v⋅d)² − (v² − r)²)
        /// </para>
        /// <para>
        /// Entrance of intersection is given by the smaller t
        /// t = −(v⋅d) − √((v⋅d)² − (v² − r)²)
        /// 
        /// Exit of intersection is given by the larger t
        /// t = −(v⋅d) + √((v⋅d)² − (v² − r)²)
        /// 
        /// If the smaller t value is &lt; 0 than the ray started inside of the sphere.
        /// 
        /// If the descriminant (v⋅d)² − (v² − r)² is &lt; 0 than no intersection occured. If the
        /// descriminant (v⋅d)² − (v² − r)² is = 0 than the ray is tangential to the sphere. If
        /// the descriminant (v⋅d)² − (v² − r)² is > 0 than the ray passes through the sphere.
        /// </para>
        /// </remarks>
        public static bool RayIntersectsSphere(ref Ray ray, ref BoundingSphere sphere, out double distance)
        {
            Vector3D v;
            Vector3D.Subtract(ref ray.Position, ref sphere.Center, out v);

            double b = Vector3D.Dot(v, ray.Direction);
            double c = Vector3D.Dot(v, v) - (sphere.Radius * sphere.Radius);

            if (c > 0f && b > 0f)
            {
                distance = 0f;
                return false;
            }

            double discriminant = b * b - c;

            if (discriminant < 0f)
            {
                distance = 0f;
                return false;
            }

            distance = -b - (double)Math.Sqrt(discriminant);

            if (distance < 0f)
                distance = 0f;

            return true;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.Ray"/> and a <see cref="SlimMath.BoundingSphere"/>. 
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="sphere">The sphere to test.</param>
        /// <param name="point">When the method completes, contains the point of intersection,
        /// or <see cref="SlimMath.Vector3D.Zero"/> if there was no intersection.</param>
        /// <returns>Whether the two objects intersected.</returns>
        /// <remarks>
        /// <para>
        /// This method uses the following math to compute the intersection:
        /// ‖x − c‖² = r²           Equation of sphere
        /// x = s + td              Equation of ray
        /// 
        /// Solve for t
        /// ‖s + td − c‖² = r²      Substitute equation of ray into equation of sphere
        /// v ≝ s − c
        /// ‖v + td‖² = r²
        /// v² + 2v⋅td + t²d² = r²
        /// d²t² + (2v⋅d)t + (v² − r²) = 0
        /// t² + (2v⋅d)t + (v² − r²) = 0    If d is a normalized vector
        /// 
        /// Quadratic equation gives us
        /// t = (−(2v⋅d) ± √((2v⋅d)² − 4(v² − r)²)) / 2
        /// t = −(v⋅d) ± √((v⋅d)² − (v² − r)²)
        /// </para>
        /// <para>
        /// Entrance of intersection is given by the smaller t
        /// t = −(v⋅d) − √((v⋅d)² − (v² − r)²)
        /// 
        /// Exit of intersection is given by the larger t
        /// t = −(v⋅d) + √((v⋅d)² − (v² − r)²)
        /// 
        /// If the smaller t value is &lt; 0 than the ray started inside of the sphere.
        /// 
        /// If the descriminant (v⋅d)² − (v² − r)² is &lt; 0 than no intersection occured. If the
        /// descriminant (v⋅d)² − (v² − r)² is = 0 than the ray is tangential to the sphere. If
        /// the descriminant (v⋅d)² − (v² − r)² is > 0 than the ray passes through the sphere.
        /// </para>
        /// </remarks>
        public static bool RayIntersectsSphere(ref Ray ray, ref BoundingSphere sphere, out Vector3D point)
        {
            double distance;
            if (!RayIntersectsSphere(ref ray, ref sphere, out distance))
            {
                point = Vector3D.Zero;
                return false;
            }

            point = ray.Position + (ray.Direction * distance);
            return true;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.Ray"/> and a <see cref="SlimMath.BoundingSphere"/>. 
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="sphere">The sphere to test.</param>
        /// <param name="point">When the method completes, contains the point of intersection,
        /// or <see cref="SlimMath.Vector3D.Zero"/> if there was no intersection.</param>
        /// <param name="normal">When the method completes, contains the normal vector on the
        /// sphere at the point of intersection.</param>
        /// <returns>Whether the two objects intersected.</returns>
        /// <remarks>
        /// <para>
        /// This method uses the following math to compute the intersection:
        /// ‖x − c‖² = r²           Equation of sphere
        /// x = s + td              Equation of ray
        /// 
        /// Solve for t
        /// ‖s + td − c‖² = r²      Substitute equation of ray into equation of sphere
        /// v ≝ s − c
        /// ‖v + td‖² = r²
        /// v² + 2v⋅td + t²d² = r²
        /// d²t² + (2v⋅d)t + (v² − r²) = 0
        /// t² + (2v⋅d)t + (v² − r²) = 0    If d is a normalized vector
        /// 
        /// Quadratic equation gives us
        /// t = (−(2v⋅d) ± √((2v⋅d)² − 4(v² − r)²)) / 2
        /// t = −(v⋅d) ± √((v⋅d)² − (v² − r)²)
        /// </para>
        /// <para>
        /// Entrance of intersection is given by the smaller t
        /// t = −(v⋅d) − √((v⋅d)² − (v² − r)²)
        /// 
        /// Exit of intersection is given by the larger t
        /// t = −(v⋅d) + √((v⋅d)² − (v² − r)²)
        /// 
        /// If the smaller t value is &lt; 0 than the ray started inside of the sphere.
        /// 
        /// If the descriminant (v⋅d)² − (v² − r)² is &lt; 0 than no intersection occured. If the
        /// descriminant (v⋅d)² − (v² − r)² is = 0 than the ray is tangential to the sphere. If
        /// the descriminant (v⋅d)² − (v² − r)² is > 0 than the ray passes through the sphere.
        /// </para>
        /// </remarks>
        public static bool RayIntersectsSphere(ref Ray ray, ref BoundingSphere sphere, out Vector3D point, out Vector3D normal)
        {
            double distance;
            if (!RayIntersectsSphere(ref ray, ref sphere, out distance))
            {
                point = Vector3D.Zero;
                normal = Vector3D.Zero;
                return false;
            }

            point = ray.Position + (ray.Direction * distance);
            normal = point - sphere.Center;
            normal.Normalize();
            return true;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.Ray"/> and a <see cref="SlimMath.BoundingSphere"/>. 
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="sphere">The sphere to test.</param>
        /// <param name="entrancePoint">When the method completes, contains the closest point of intersection,
        /// or <see cref="SlimMath.Vector3D.Zero"/> if there was no intersection.</param>
        /// <param name="entranceNormal">When the method completes, contains the normal vector on the
        /// sphere at the point of closest intersection.</param>
        /// <param name="exitPoint">When the method completes, contains the farthest point of intersection,
        /// or <see cref="SlimMath.Vector3D.Zero"/> if there was no intersection.</param>
        /// <param name="exitNormal">Whent he method completes, contains the normal vector on the
        /// sphere at the point of farthest intersection.</param>
        /// <returns>Whether the two objects intersected.</returns>
        /// <remarks>
        /// <para>
        /// This method uses the following math to compute the intersection:
        /// ‖x − c‖² = r²           Equation of sphere
        /// x = s + td              Equation of ray
        /// 
        /// Solve for t
        /// ‖s + td − c‖² = r²      Substitute equation of ray into equation of sphere
        /// v ≝ s − c
        /// ‖v + td‖² = r²
        /// v² + 2v⋅td + t²d² = r²
        /// d²t² + (2v⋅d)t + (v² − r²) = 0
        /// t² + (2v⋅d)t + (v² − r²) = 0    If d is a normalized vector
        /// 
        /// Quadratic equation gives us
        /// t = (−(2v⋅d) ± √((2v⋅d)² − 4(v² − r)²)) / 2
        /// t = −(v⋅d) ± √((v⋅d)² − (v² − r)²)
        /// </para>
        /// <para>
        /// Entrance of intersection is given by the smaller t
        /// t = −(v⋅d) − √((v⋅d)² − (v² − r)²)
        /// 
        /// Exit of intersection is given by the larger t
        /// t = −(v⋅d) + √((v⋅d)² − (v² − r)²)
        /// 
        /// If the smaller t value is &lt; 0 than the ray started inside of the sphere.
        /// 
        /// If the descriminant (v⋅d)² − (v² − r)² is &lt; 0 than no intersection occured. If the
        /// descriminant (v⋅d)² − (v² − r)² is = 0 than the ray is tangential to the sphere. If
        /// the descriminant (v⋅d)² − (v² − r)² is > 0 than the ray passes through the sphere.
        /// </para>
        /// </remarks>
        public static bool RayIntersectsSphere(ref Ray ray, ref BoundingSphere sphere, out Vector3D entrancePoint, out Vector3D entranceNormal, out Vector3D exitPoint, out Vector3D exitNormal)
        {
            Vector3D v;
            Vector3D.Subtract(ref ray.Position, ref sphere.Center, out v);

            double b = Vector3D.Dot(v, ray.Direction);
            double c = Vector3D.Dot(v, v) - (sphere.Radius * sphere.Radius);

            if (c > 0f && b > 0f)
            {
                entrancePoint = Vector3D.Zero;
                entranceNormal = Vector3D.Zero;
                exitPoint = Vector3D.Zero;
                exitNormal = Vector3D.Zero;
                return false;
            }

            double discriminant = b * b - c;

            if (discriminant < 0f)
            {
                entrancePoint = Vector3D.Zero;
                entranceNormal = Vector3D.Zero;
                exitPoint = Vector3D.Zero;
                exitNormal = Vector3D.Zero;
                return false;
            }

            double discriminantSquared = (double)Math.Sqrt(discriminant);
            double distance1 = -b - discriminantSquared;
            double distance2 = -b + discriminantSquared;

            if (distance1 < 0f)
            {
                distance1 = 0f;
                entrancePoint = Vector3D.Zero;
                entranceNormal = Vector3D.Zero;
            }
            else
            {
                entrancePoint = ray.Position + (ray.Direction * distance1);
                entranceNormal = entrancePoint - sphere.Center;
                entranceNormal.Normalize();
            }

            exitPoint = ray.Position + (ray.Direction * distance2);
            exitNormal = exitPoint - sphere.Center;
            exitNormal.Normalize();

            return true;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.Plane"/> and a point.
        /// </summary>
        /// <param name="plane">The plane to test.</param>
        /// <param name="point">The point to test.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public static PlaneIntersectionType PlaneIntersectsPoint(ref Plane plane, ref Vector3D point)
        {
            double distance;
            Vector3D.Dot(ref plane.Normal, ref point, out distance);
            distance += plane.D;

            if (distance > 0f)
                return PlaneIntersectionType.Front;

            if (distance < 0f)
                return PlaneIntersectionType.Back;

            return PlaneIntersectionType.Intersecting;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.Plane"/> and a <see cref="SlimMath.Plane"/>.
        /// </summary>
        /// <param name="plane1">The first plane to test.</param>
        /// <param name="plane2">The second plane to test.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public static bool PlaneIntersectsPlane(ref Plane plane1, ref Plane plane2)
        {
            Vector3D direction;
            Vector3D.Cross(ref plane1.Normal, ref plane2.Normal, out direction);

            //If direction is the zero vector, the planes are parallel and possibly
            //coincident. It is not an intersection. The dot product will tell us.
            double denominator;
            Vector3D.Dot(ref direction, ref direction, out denominator);

            if (Math.Abs(denominator) < Utilities.ZeroTolerance)
                return false;

            return true;
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
            double denominator;
            Vector3D.Dot(ref direction, ref direction, out denominator);

            //We assume the planes are normalized, therefore the denominator
            //only serves as a parallel and coincident check. Otherwise we need
            //to deivide the point by the denominator.
            if (Math.Abs(denominator) < Utilities.ZeroTolerance)
            {
                line = new Ray();
                return false;
            }

            Vector3D point;
            Vector3D temp = plane1.D * plane2.Normal - plane2.D * plane1.Normal;
            Vector3D.Cross(ref temp, ref direction, out point);

            line.Position = point;
            line.Direction = direction;
            line.Direction.Normalize();

            return true;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.Plane"/> and a triangle.
        /// </summary>
        /// <param name="plane">The plane to test.</param>
        /// <param name="vertex1">The first vertex of the triangle to test.</param>
        /// <param name="vertex2">The second vertex of the triagnle to test.</param>
        /// <param name="vertex3">The third vertex of the triangle to test.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public static PlaneIntersectionType PlaneIntersectsTriangle(ref Plane plane, ref Vector3D vertex1, ref Vector3D vertex2, ref Vector3D vertex3)
        {
            //Source: Real-Time Collision Detection by Christer Ericson
            //Reference: Page 207

            PlaneIntersectionType test1 = PlaneIntersectsPoint(ref plane, ref vertex1);
            PlaneIntersectionType test2 = PlaneIntersectsPoint(ref plane, ref vertex2);
            PlaneIntersectionType test3 = PlaneIntersectsPoint(ref plane, ref vertex3);

            if (test1 == PlaneIntersectionType.Front && test2 == PlaneIntersectionType.Front && test3 == PlaneIntersectionType.Front)
                return PlaneIntersectionType.Front;

            if (test1 == PlaneIntersectionType.Back && test2 == PlaneIntersectionType.Back && test3 == PlaneIntersectionType.Back)
                return PlaneIntersectionType.Back;

            return PlaneIntersectionType.Intersecting;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.Plane"/> and a <see cref="SlimMath.BoundingBox"/>.
        /// </summary>
        /// <param name="plane">The plane to test.</param>
        /// <param name="box">The box to test.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public static PlaneIntersectionType PlaneIntersectsBox(ref Plane plane, ref BoundingBox box)
        {
            //Source: Real-Time Collision Detection by Christer Ericson
            //Reference: Page 161

            Vector3D min;
            Vector3D max;

            max.x = (plane.Normal.x >= 0.0f) ? box.Minimum.x : box.Maximum.x;
            max.y = (plane.Normal.y >= 0.0f) ? box.Minimum.y : box.Maximum.y;
            max.z = (plane.Normal.z >= 0.0f) ? box.Minimum.z : box.Maximum.z;
            min.x = (plane.Normal.x >= 0.0f) ? box.Maximum.x : box.Minimum.x;
            min.y = (plane.Normal.y >= 0.0f) ? box.Maximum.y : box.Minimum.y;
            min.z = (plane.Normal.z >= 0.0f) ? box.Maximum.z : box.Minimum.z;

            double distance;
            Vector3D.Dot(ref plane.Normal, ref max, out distance);

            if (distance + plane.D > 0.0f)
                return PlaneIntersectionType.Front;

            distance = Vector3D.Dot(plane.Normal, min);

            if (distance + plane.D < 0.0f)
                return PlaneIntersectionType.Back;

            return PlaneIntersectionType.Intersecting;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.Plane"/> and a <see cref="SlimMath.BoundingSphere"/>.
        /// </summary>
        /// <param name="plane">The plane to test.</param>
        /// <param name="sphere">The sphere to test.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public static PlaneIntersectionType PlaneIntersectsSphere(ref Plane plane, ref BoundingSphere sphere)
        {
            //Source: Real-Time Collision Detection by Christer Ericson
            //Reference: Page 160

            double distance;
            Vector3D.Dot(ref plane.Normal, ref sphere.Center, out distance);
            distance += plane.D;

            if (distance > sphere.Radius)
                return PlaneIntersectionType.Front;

            if (distance < -sphere.Radius)
                return PlaneIntersectionType.Back;

            return PlaneIntersectionType.Intersecting;
        }

        //THIS IMPLEMENTATION IS INCOMPLETE!
        //NEEDS TO BE COMPLETED SOON
        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.BoundingBox"/> and a triangle.
        /// </summary>
        /// <param name="box">The box to test.</param>
        /// <param name="vertex1">The first vertex of the triangle to test.</param>
        /// <param name="vertex2">The second vertex of the triagnle to test.</param>
        /// <param name="vertex3">The third vertex of the triangle to test.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public static bool BoxIntersectsTriangle(ref BoundingBox box, ref Vector3D vertex1, ref Vector3D vertex2, ref Vector3D vertex3)
        {
            //Source: Real-Time Collision Detection by Christer Ericson
            //Reference: Page 169

            double p0, p1, p2, r;

            //Compute box center and extents (if not already given in that format)
            Vector3D center = (box.Minimum + box.Maximum) * 0.5f;
            double e0 = (box.Maximum.x - box.Minimum.x) * 0.5f;
            double e1 = (box.Maximum.y - box.Minimum.y) * 0.5f;
            double e2 = (box.Maximum.z - box.Minimum.z) * 0.5f;

            //Translate triangle as conceptually moving AABB to origin
            vertex1 = vertex1 - center;
            vertex2 = vertex2 - center;
            vertex3 = vertex3 - center;

            //Compute edge vectors for triangle
            Vector3D f0 = vertex2 - vertex1;
            Vector3D f1 = vertex3 - vertex2;
            Vector3D f2 = vertex1 - vertex3;

            //Test axes a00..a22 (category 3)
            //Test axis a00
            p0 = vertex1.z * vertex2.y - vertex1.y * vertex2.z;
            p2 = vertex3.z * (vertex2.y - vertex1.y) - vertex3.z * (vertex2.z - vertex1.z);
            r = e1 * Math.Abs(f0.z) + e2 * Math.Abs(f0.y);

            if (Math.Max(-Math.Max(p0, p2), Math.Min(p0, p2)) > r)
                return false; //Axis is a separating axis

            //Repeat similar tests for remaining axes a01..a22
            //...

            //Test the three axes corresponding to the face normals of AABB b (category 1).
            //Exit if...
            // ... [-e0, e0] and [Math.Min(vertex1.X,vertex2.X,vertex3.X), Math.Max(vertex1.X,vertex2.X,vertex3.X)] do not overlap
            if (Math.Max(Math.Max(vertex1.x, vertex2.x), vertex3.x) < -e0 || Math.Min(Math.Min(vertex1.x, vertex2.x), vertex3.x) > e0)
                return false;

            // ... [-e1, e1] and [Math.Min(vertex1.Y,vertex2.Y,vertex3.Y), Math.Max(vertex1.Y,vertex2.Y,vertex3.Y)] do not overlap
            if (Math.Max(Math.Max(vertex1.y, vertex2.y), vertex3.y) < -e1 || Math.Min(Math.Min(vertex1.y, vertex2.y), vertex3.y) > e1)
                return false;

            // ... [-e2, e2] and [Math.Min(vertex1.Z,vertex2.Z,vertex3.Z), Math.Max(vertex1.Z,vertex2.Z,vertex3.Z)] do not overlap
            if (Math.Max(Math.Max(vertex1.z, vertex2.z), vertex3.z) < -e2 || Math.Min(Math.Min(vertex1.z, vertex2.z), vertex3.z) > e2)
                return false;

            //Test separating axis corresponding to triangle face normal (category 2)
            Plane plane;
            plane.Normal = Vector3D.Cross(f0, f1);
            plane.D = Vector3D.Dot(plane.Normal, vertex1);

            return PlaneIntersectsBox(ref plane, ref box) == PlaneIntersectionType.Intersecting;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.BoundingBox"/> and a <see cref="SlimMath.BoundingBox"/>.
        /// </summary>
        /// <param name="box1">The first box to test.</param>
        /// <param name="box2">The second box to test.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public static bool BoxIntersectsBox(ref BoundingBox box1, ref BoundingBox box2)
        {
            if (box1.Minimum.x > box2.Maximum.x || box2.Minimum.x > box1.Maximum.x)
                return false;

            if (box1.Minimum.y > box2.Maximum.y || box2.Minimum.y > box1.Maximum.y)
                return false;

            if (box1.Minimum.z > box2.Maximum.z || box2.Minimum.z > box1.Maximum.z)
                return false;

            return true;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.BoundingBox"/> and a <see cref="SlimMath.BoundingSphere"/>.
        /// </summary>
        /// <param name="box">The box to test.</param>
        /// <param name="sphere">The sphere to test.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public static bool BoxIntersectsSphere(ref BoundingBox box, ref BoundingSphere sphere)
        {
            //Source: Real-Time Collision Detection by Christer Ericson
            //Reference: Page 166

            Vector3D vector;
            Vector3D.Clamp(ref sphere.Center, ref box.Minimum, ref box.Maximum, out vector);
            double distance = Vector3D.DistanceSquared(sphere.Center, vector);

            return distance <= sphere.Radius * sphere.Radius;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.BoundingSphere"/> and a triangle.
        /// </summary>
        /// <param name="sphere">The sphere to test.</param>
        /// <param name="vertex1">The first vertex of the triangle to test.</param>
        /// <param name="vertex2">The second vertex of the triagnle to test.</param>
        /// <param name="vertex3">The third vertex of the triangle to test.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public static bool SphereIntersectsTriangle(ref BoundingSphere sphere, ref Vector3D vertex1, ref Vector3D vertex2, ref Vector3D vertex3)
        {
            //Source: Real-Time Collision Detection by Christer Ericson
            //Reference: Page 167

            Vector3D point;
            ClosestPointOnTriangleToPoint(ref sphere.Center, ref vertex1, ref vertex2, ref vertex3, out point);
            Vector3D v = point - sphere.Center;

            double dot;
            Vector3D.Dot(ref v, ref v, out dot);

            return dot <= sphere.Radius * sphere.Radius;
        }

        /// <summary>
        /// Determines whether there is an intersection between a <see cref="SlimMath.BoundingSphere"/> and a <see cref="SlimMath.BoundingSphere"/>.
        /// </summary>
        /// <param name="sphere1">First sphere to test.</param>
        /// <param name="sphere2">Second sphere to test.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public static bool SphereIntersectsSphere(ref BoundingSphere sphere1, ref BoundingSphere sphere2)
        {
            double radiisum = sphere1.Radius + sphere2.Radius;
            return Vector3D.DistanceSquared(sphere1.Center, sphere2.Center) <= radiisum * radiisum;
        }

        /// <summary>
        /// Determines whether a <see cref="SlimMath.BoundingBox"/> contains a point.
        /// </summary>
        /// <param name="box">The box to test.</param>
        /// <param name="point">The point to test.</param>
        /// <returns>The type of containment the two objects have.</returns>
        public static ContainmentType BoxContainsPoint(ref BoundingBox box, ref Vector3D point)
        {
            if (box.Minimum.x <= point.x && box.Maximum.x >= point.x &&
                box.Minimum.y <= point.y && box.Maximum.y >= point.y &&
                box.Minimum.z <= point.z && box.Maximum.z >= point.z)
            {
                return ContainmentType.Contains;
            }

            return ContainmentType.Disjoint;
        }

        /// <summary>
        /// Determines whether a <see cref="SlimMath.BoundingBox"/> contains a triangle.
        /// </summary>
        /// <param name="box">The box to test.</param>
        /// <param name="vertex1">The first vertex of the triangle to test.</param>
        /// <param name="vertex2">The second vertex of the triagnle to test.</param>
        /// <param name="vertex3">The third vertex of the triangle to test.</param>
        /// <returns>The type of containment the two objects have.</returns>
        public static ContainmentType BoxContainsTriangle(ref BoundingBox box, ref Vector3D vertex1, ref Vector3D vertex2, ref Vector3D vertex3)
        {
            ContainmentType test1 = BoxContainsPoint(ref box, ref vertex1);
            ContainmentType test2 = BoxContainsPoint(ref box, ref vertex2);
            ContainmentType test3 = BoxContainsPoint(ref box, ref vertex3);

            if (test1 == ContainmentType.Contains && test2 == ContainmentType.Contains && test3 == ContainmentType.Contains)
                return ContainmentType.Contains;

            if (BoxIntersectsTriangle(ref box, ref vertex1, ref vertex2, ref vertex3))
                return ContainmentType.Intersects;

            return ContainmentType.Disjoint;
        }

        /// <summary>
        /// Determines whether a <see cref="SlimMath.BoundingBox"/> contains a <see cref="SlimMath.BoundingBox"/>.
        /// </summary>
        /// <param name="box1">The first box to test.</param>
        /// <param name="box2">The second box to test.</param>
        /// <returns>The type of containment the two objects have.</returns>
        public static ContainmentType BoxContainsBox(ref BoundingBox box1, ref BoundingBox box2)
        {
            if (box1.Maximum.x < box2.Minimum.x || box1.Minimum.x > box2.Maximum.x)
                return ContainmentType.Disjoint;

            if (box1.Maximum.y < box2.Minimum.y || box1.Minimum.y > box2.Maximum.y)
                return ContainmentType.Disjoint;

            if (box1.Maximum.z < box2.Minimum.z || box1.Minimum.z > box2.Maximum.z)
                return ContainmentType.Disjoint;

            if (box1.Minimum.x <= box2.Minimum.x && (box2.Maximum.x <= box1.Maximum.x &&
                box1.Minimum.y <= box2.Minimum.y && box2.Maximum.y <= box1.Maximum.y) &&
                box1.Minimum.z <= box2.Minimum.z && box2.Maximum.z <= box1.Maximum.z)
            {
                return ContainmentType.Contains;
            }

            return ContainmentType.Intersects;
        }

        /// <summary>
        /// Determines whether a <see cref="SlimMath.BoundingBox"/> contains a <see cref="SlimMath.BoundingSphere"/>.
        /// </summary>
        /// <param name="box">The box to test.</param>
        /// <param name="sphere">The sphere to test.</param>
        /// <returns>The type of containment the two objects have.</returns>
        public static ContainmentType BoxContainsSphere(ref BoundingBox box, ref BoundingSphere sphere)
        {
            Vector3D vector;
            Vector3D.Clamp(ref sphere.Center, ref box.Minimum, ref box.Maximum, out vector);
            double distance = Vector3D.DistanceSquared(sphere.Center, vector);

            if (distance > sphere.Radius * sphere.Radius)
                return ContainmentType.Disjoint;

            if ((((box.Minimum.x + sphere.Radius <= sphere.Center.x) && (sphere.Center.x <= box.Maximum.x - sphere.Radius)) && ((box.Maximum.x - box.Minimum.x > sphere.Radius) &&
                (box.Minimum.y + sphere.Radius <= sphere.Center.y))) && (((sphere.Center.y <= box.Maximum.y - sphere.Radius) && (box.Maximum.y - box.Minimum.y > sphere.Radius)) &&
                (((box.Minimum.z + sphere.Radius <= sphere.Center.z) && (sphere.Center.z <= box.Maximum.z - sphere.Radius)) && (box.Maximum.x - box.Minimum.x > sphere.Radius))))
            {
                return ContainmentType.Contains;
            }

            return ContainmentType.Intersects;
        }

        /// <summary>
        /// Determines whether a <see cref="SlimMath.BoundingSphere"/> contains a point.
        /// </summary>
        /// <param name="sphere">The sphere to test.</param>
        /// <param name="point">The point to test.</param>
        /// <returns>The type of containment the two objects have.</returns>
        public static ContainmentType SphereContainsPoint(ref BoundingSphere sphere, ref Vector3D point)
        {
            if (Vector3D.DistanceSquared(point, sphere.Center) <= sphere.Radius * sphere.Radius)
                return ContainmentType.Contains;

            return ContainmentType.Disjoint;
        }

        /// <summary>
        /// Determines whether a <see cref="SlimMath.BoundingSphere"/> contains a triangle.
        /// </summary>
        /// <param name="sphere">The sphere to test.</param>
        /// <param name="vertex1">The first vertex of the triangle to test.</param>
        /// <param name="vertex2">The second vertex of the triagnle to test.</param>
        /// <param name="vertex3">The third vertex of the triangle to test.</param>
        /// <returns>The type of containment the two objects have.</returns>
        public static ContainmentType SphereContainsTriangle(ref BoundingSphere sphere, ref Vector3D vertex1, ref Vector3D vertex2, ref Vector3D vertex3)
        {
            //Source: Jorgy343
            //Reference: None

            ContainmentType test1 = SphereContainsPoint(ref sphere, ref vertex1);
            ContainmentType test2 = SphereContainsPoint(ref sphere, ref vertex2);
            ContainmentType test3 = SphereContainsPoint(ref sphere, ref vertex3);

            if (test1 == ContainmentType.Contains && test2 == ContainmentType.Contains && test3 == ContainmentType.Contains)
                return ContainmentType.Contains;

            if (SphereIntersectsTriangle(ref sphere, ref vertex1, ref vertex2, ref vertex3))
                return ContainmentType.Intersects;

            return ContainmentType.Disjoint;
        }

        /// <summary>
        /// Determines whether a <see cref="SlimMath.BoundingSphere"/> contains a <see cref="SlimMath.BoundingBox"/>.
        /// </summary>
        /// <param name="sphere">The sphere to test.</param>
        /// <param name="box">The box to test.</param>
        /// <returns>The type of containment the two objects have.</returns>
        public static ContainmentType SphereContainsBox(ref BoundingSphere sphere, ref BoundingBox box)
        {
            Vector3D vector;

            if (!BoxIntersectsSphere(ref box, ref sphere))
                return ContainmentType.Disjoint;

            double radiussquared = sphere.Radius * sphere.Radius;
            vector.x = sphere.Center.x - box.Minimum.x;
            vector.y = sphere.Center.y - box.Maximum.y;
            vector.z = sphere.Center.z - box.Maximum.z;

            if (vector.LengthSquared > radiussquared)
                return ContainmentType.Intersects;

            vector.x = sphere.Center.x - box.Maximum.x;
            vector.y = sphere.Center.y - box.Maximum.y;
            vector.z = sphere.Center.z - box.Maximum.z;

            if (vector.LengthSquared > radiussquared)
                return ContainmentType.Intersects;

            vector.x = sphere.Center.x - box.Maximum.x;
            vector.y = sphere.Center.y - box.Minimum.y;
            vector.z = sphere.Center.z - box.Maximum.z;

            if (vector.LengthSquared > radiussquared)
                return ContainmentType.Intersects;

            vector.x = sphere.Center.x - box.Minimum.x;
            vector.y = sphere.Center.y - box.Minimum.y;
            vector.z = sphere.Center.z - box.Maximum.z;

            if (vector.LengthSquared > radiussquared)
                return ContainmentType.Intersects;

            vector.x = sphere.Center.x - box.Minimum.x;
            vector.y = sphere.Center.y - box.Maximum.y;
            vector.z = sphere.Center.z - box.Minimum.z;

            if (vector.LengthSquared > radiussquared)
                return ContainmentType.Intersects;

            vector.x = sphere.Center.x - box.Maximum.x;
            vector.y = sphere.Center.y - box.Maximum.y;
            vector.z = sphere.Center.z - box.Minimum.z;

            if (vector.LengthSquared > radiussquared)
                return ContainmentType.Intersects;

            vector.x = sphere.Center.x - box.Maximum.x;
            vector.y = sphere.Center.y - box.Minimum.y;
            vector.z = sphere.Center.z - box.Minimum.z;

            if (vector.LengthSquared > radiussquared)
                return ContainmentType.Intersects;

            vector.x = sphere.Center.x - box.Minimum.x;
            vector.y = sphere.Center.y - box.Minimum.y;
            vector.z = sphere.Center.z - box.Minimum.z;

            if (vector.LengthSquared > radiussquared)
                return ContainmentType.Intersects;

            return ContainmentType.Contains;
        }

        /// <summary>
        /// Determines whether a <see cref="SlimMath.BoundingSphere"/> contains a <see cref="SlimMath.BoundingSphere"/>.
        /// </summary>
        /// <param name="sphere1">The first sphere to test.</param>
        /// <param name="sphere2">The second sphere to test.</param>
        /// <returns>The type of containment the two objects have.</returns>
        public static ContainmentType SphereContainsSphere(ref BoundingSphere sphere1, ref BoundingSphere sphere2)
        {
            double distance = Vector3D.Distance(sphere1.Center, sphere2.Center);

            if (sphere1.Radius + sphere2.Radius < distance)
                return ContainmentType.Disjoint;

            if (sphere1.Radius - sphere2.Radius < distance)
                return ContainmentType.Intersects;

            return ContainmentType.Contains;
        }

        /// <summary>
        /// Generates a supporting point from a specific triangle.
        /// </summary>
        /// <param name="vertex1">The first vertex of the triangle.</param>
        /// <param name="vertex2">The second vertex of the triangle.</param>
        /// <param name="vertex3">The third vertex of the triangle</param>
        /// <param name="direction">The direction for which to build the supporting point.</param>
        /// <param name="result">When the method completes, contains the supporting point.</param>
        public static void SupportPoint(ref Vector3D vertex1, ref Vector3D vertex2, ref Vector3D vertex3, ref Vector3D direction, out Vector3D result)
        {
            double dot1 = Vector3D.Dot(vertex1, direction);
            double dot2 = Vector3D.Dot(vertex2, direction);
            double dot3 = Vector3D.Dot(vertex3, direction);

            if (dot1 > dot2 && dot1 > dot3)
                result = vertex1;
            else if (dot2 > dot1 && dot2 > dot3)
                result = vertex2;
            else
                result = vertex3;
        }

        /// <summary>
        /// Generates a supporting point from a specific <see cref="SlimMath.BoundingBox"/>.
        /// </summary>
        /// <param name="box">The box to generate the supporting point for.</param>
        /// <param name="direction">The direction for which to build the supporting point.</param>
        /// <param name="result">When the method completes, contains the supporting point.</param>
        public static void SupportPoint(ref BoundingBox box, ref Vector3D direction, out Vector3D result)
        {
            result.x = direction.x >= 0.0f ? box.Maximum.x : box.Minimum.x;
            result.y = direction.y >= 0.0f ? box.Maximum.y : box.Minimum.y;
            result.z = direction.z >= 0.0f ? box.Maximum.z : box.Minimum.z;
        }

        /// <summary>
        /// Generates a supporting point from a specific <see cref="SlimMath.BoundingSphere"/>.
        /// </summary>
        /// <param name="sphere">The sphere to generate the supporting point for.</param>
        /// <param name="direction">The direction for which to build the supporting point.</param>
        /// <param name="result">When the method completes, contains the supporting point.</param>
        public static void SupportPoint(ref BoundingSphere sphere, ref Vector3D direction, out Vector3D result)
        {
            result = (sphere.Radius / direction.Length()) * direction + sphere.Center;
        }

        /// <summary>
        /// Generates a supporting point from a polyhedra.
        /// </summary>
        /// <param name="points">The points that make up the polyhedra.</param>
        /// <param name="direction">The direction for which to build the supporting point.</param>
        /// <param name="result">When the method completes, contains the supporting point.</param>
        public static void SupportPoint(IEnumerable<Vector3D> points, ref Vector3D direction, out Vector3D result)
        {
            double maxdot = double.MinValue;
            result = Vector3D.Zero;

            foreach (Vector3D point in points)
            {
                double tempdot = Vector3D.Dot(direction, point);

                if (tempdot > maxdot)
                {
                    maxdot = tempdot;
                    result = point;
                }
            }
        }
    }
}
