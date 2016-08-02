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
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace GraphicResearchHuiZhao
{
    /// <summary>
    /// Represents a plane in three dimensional space.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Plane : IEquatable<Plane>, IFormattable
    {
        /// <summary>
        /// The normal vector of the plane.
        /// </summary>
        public Vector3D Normal;

        /// <summary>
        /// The distance of the plane along its normal from the origin.
        /// </summary>
        public double D;

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Plane"/> struct.
        /// </summary>
        /// <param name="value">The value that will be assigned to all components.</param>
        public Plane(double value)
        {
            Normal.x = Normal.y = Normal.z = D = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Plane"/> struct.
        /// </summary>
        /// <param name="a">The X component of the normal.</param>
        /// <param name="b">The Y component of the normal.</param>
        /// <param name="c">The Z component of the normal.</param>
        /// <param name="d">The distance of the plane along its normal from the origin.</param>
        public Plane(double a, double b, double c, double d)
        {
            Normal.x = a;
            Normal.y = b;
            Normal.z = c;
            D = d;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Plane"/> struct.
        /// </summary>
        /// <param name="value">The normal of the plane.</param>
        /// <param name="d">The distance of the plane along its normal from the origin</param>
        public Plane(Vector3D value, double d)
        {
            Normal = value;
            D = d;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Plane"/> struct.
        /// </summary>
        /// <param name="point">Any point that lies along the plane.</param>
        /// <param name="normal">The normal of the plane.</param>
        public Plane(Vector3D point, Vector3D normal)
        {
            Normal = normal;
            D = -Vector3D.Dot(normal, point);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Plane"/> struct.
        /// </summary>
        /// <param name="point1">First point of a triangle defining the plane.</param>
        /// <param name="point2">Second point of a triangle defining the plane.</param>
        /// <param name="point3">Third point of a triangle defining the plane.</param>
        public Plane(Vector3D point1, Vector3D point2, Vector3D point3)
        {
            double x1 = point2.x - point1.x;
            double y1 = point2.y - point1.y;
            double z1 = point2.z - point1.z;
            double x2 = point3.x - point1.x;
            double y2 = point3.y - point1.y;
            double z2 = point3.z - point1.z;
            double yz = (y1 * z2) - (z1 * y2);
            double xz = (z1 * x2) - (x1 * z2);
            double xy = (x1 * y2) - (y1 * x2);
            double invPyth = 1.0f / (double)(Math.Sqrt((yz * yz) + (xz * xz) + (xy * xy)));

            Normal.x = yz * invPyth;
            Normal.y = xz * invPyth;
            Normal.z = xy * invPyth;
            D = -((Normal.x * point1.x) + (Normal.y * point1.y) + (Normal.z * point1.z));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Plane"/> struct.
        /// </summary>
        /// <param name="values">The values to assign to the A, B, C, and D components of the plane. This must be an array with four elements.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than four elements.</exception>
        public Plane(double[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length != 4)
                throw new ArgumentOutOfRangeException("values", "There must be four and only four input values for Plane.");

            Normal.x = values[0];
            Normal.y = values[1];
            Normal.z = values[2];
            D = values[3];
        }

        /// <summary>
        /// Gets or sets the component at the specified index.
        /// </summary>
        /// <value>The value of the A, B, C, or D component, depending on the index.</value>
        /// <param name="index">The index of the component to access. Use 0 for the A component, 1 for the B component, 2 for the C component, and 3 for the D component.</param>
        /// <returns>The value of the component at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="index"/> is out of the range [0, 3].</exception>
        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return Normal.x;
                    case 1: return Normal.y;
                    case 2: return Normal.z;
                    case 3: return D;
                }

                throw new ArgumentOutOfRangeException("index", "Indices for Plane run from 0 to 3, inclusive.");
            }

            set
            {
                switch (index)
                {
                    case 0: Normal.x = value; break;
                    case 1: Normal.y = value; break;
                    case 2: Normal.z = value; break;
                    case 3: D = value; break;
                    default: throw new ArgumentOutOfRangeException("index", "Indices for Plane run from 0 to 3, inclusive.");
                }
            }
        }

        /// <summary>
        /// Changes the coefficients of the normal vector of the plane to make it of unit length.
        /// </summary>
        public void Normalize()
        {
            double magnitude = 1.0f / (double)(Math.Sqrt((Normal.x * Normal.x) + (Normal.y * Normal.y) + (Normal.z * Normal.z)));

            Normal.x *= magnitude;
            Normal.y *= magnitude;
            Normal.z *= magnitude;
            D *= magnitude;
        }

        /// <summary>
        /// Creates an array containing the elements of the plane.
        /// </summary>
        /// <returns>A four-element array containing the components of the plane.</returns>
        public double[] ToArray()
        {
            return new double[] { Normal.x, Normal.y, Normal.z, D };
        }

        /// <summary>
        /// Determines if there is an intersection between the current object and a point.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public PlaneIntersectionType Intersects(ref Vector3D point)
        {
            return Collision.PlaneIntersectsPoint(ref this, ref point);
        }

        /// <summary>
        /// Determines if there is an intersection between the current object and a <see cref="SlimMath.Ray"/>.
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public bool Intersects(ref Ray ray)
        {
            double distance;
            return Collision.RayIntersectsPlane(ref ray, ref this, out distance);
        }

        /// <summary>
        /// Determines if there is an intersection between the current object and a <see cref="SlimMath.Ray"/>.
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="distance">When the method completes, contains the distance of the intersection,
        /// or 0 if there was no intersection.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public bool Intersects(ref Ray ray, out double distance)
        {
            return Collision.RayIntersectsPlane(ref ray, ref this, out distance);
        }

        /// <summary>
        /// Determines if there is an intersection between the current object and a <see cref="SlimMath.Ray"/>.
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="point">When the method completes, contains the point of intersection,
        /// or <see cref="SlimMath.Vector3D.zero"/> if there was no intersection.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public bool Intersects(ref Ray ray, out Vector3D point)
        {
            return Collision.RayIntersectsPlane(ref ray, ref this, out point);
        }

        /// <summary>
        /// Determines if there is an intersection between the current object and a <see cref="SlimMath.Plane"/>.
        /// </summary>
        /// <param name="plane">The plane to test.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public bool Intersects(ref Plane plane)
        {
            return Collision.PlaneIntersectsPlane(ref this, ref plane);
        }

        /// <summary>
        /// Determines if there is an intersection between the current object and a <see cref="SlimMath.Plane"/>.
        /// </summary>
        /// <param name="plane">The plane to test.</param>
        /// <param name="line">When the method completes, contains the line of intersection
        /// as a <see cref="SlimMath.Ray"/>, or a zero ray if there was no intersection.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public bool Intersects(ref Plane plane, out Ray line)
        {
            return Collision.PlaneIntersectsPlane(ref this, ref plane, out line);
        }

        /// <summary>
        /// Determines if there is an intersection between the current object and a triangle.
        /// </summary>
        /// <param name="vertex1">The first vertex of the triangle to test.</param>
        /// <param name="vertex2">The second vertex of the triagnle to test.</param>
        /// <param name="vertex3">The third vertex of the triangle to test.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public PlaneIntersectionType Intersects(ref Vector3D vertex1, ref Vector3D vertex2, ref Vector3D vertex3)
        {
            return Collision.PlaneIntersectsTriangle(ref this, ref vertex1, ref vertex2, ref vertex3);
        }

        /// <summary>
        /// Determines if there is an intersection between the current object and a <see cref="SlimMath.BoundingBox"/>.
        /// </summary>
        /// <param name="box">The box to test.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public PlaneIntersectionType Intersects(ref BoundingBox box)
        {
            return Collision.PlaneIntersectsBox(ref this, ref box);
        }

        /// <summary>
        /// Determines if there is an intersection between the current object and a <see cref="SlimMath.BoundingSphere"/>.
        /// </summary>
        /// <param name="sphere">The sphere to test.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public PlaneIntersectionType Intersects(ref BoundingSphere sphere)
        {
            return Collision.PlaneIntersectsSphere(ref this, ref sphere);
        }

        /// <summary>
        /// Scales each component of the plane by the given scaling factor.
        /// </summary>
        /// <param name="value">The plane to scale.</param>
        /// <param name="scale">The amount by which to scale the plane.</param>
        /// <param name="result">When the method completes, contains the scaled plane.</param>
        public static void Multiply(ref Plane value, double scale, out Plane result)
        {
            result.Normal.x = value.Normal.x * scale;
            result.Normal.y = value.Normal.y * scale;
            result.Normal.z = value.Normal.z * scale;
            result.D = value.D * scale;
        }

        /// <summary>
        /// Scales each component of the plane by the given scaling factor.
        /// </summary>
        /// <param name="value">The plane to scale.</param>
        /// <param name="scale">The amount by which to scale the plane.</param>
        /// <returns>The scaled plane.</returns>
        public static Plane Multiply(Plane value, double scale)
        {
            return new Plane(value.Normal.x * scale, value.Normal.y * scale, value.Normal.z * scale, value.D * scale);
        }

        /// <summary>
        /// Scales the distance component of the plane by the given scaling factor.
        /// </summary>
        /// <param name="value">The plane to scale.</param>
        /// <param name="scale">The amount by which to scale the plane.</param>
        /// <param name="result">When the method completes, contains the scaled plane.</param>
        public static void Scale(ref Plane value, double scale, out Plane result)
        {
            result.Normal = value.Normal;
            result.D = value.D * scale;
        }

        /// <summary>
        /// Scales the distance component of the plane by the given scaling factor.
        /// </summary>
        /// <param name="value">The plane to scale.</param>
        /// <param name="scale">The amount by which to scale the plane.</param>
        /// <returns>The scaled plane.</returns>
        public static Plane Scale(Plane value, double scale)
        {
            Plane result;
            Scale(ref value, scale, out result);
            return result;
        }

        /// <summary>
        /// Calculates the dot product of the specified vector and plane.
        /// </summary>
        /// <param name="left">The source plane.</param>
        /// <param name="right">The source vector.</param>
        /// <param name="result">When the method completes, contains the dot product of the specified plane and vector.</param>
        public static void Dot(ref Plane left, ref Vector4D right, out double result)
        {
            result = (left.Normal.x * right.x) + (left.Normal.y * right.y) + (left.Normal.z * right.z) + (left.D * right.w);
        }

        /// <summary>
        /// Calculates the dot product of the specified vector and plane.
        /// </summary>
        /// <param name="left">The source plane.</param>
        /// <param name="right">The source vector.</param>
        /// <returns>The dot product of the specified plane and vector.</returns>
        public static double Dot(Plane left, Vector4D right)
        {
            return (left.Normal.x * right.x) + (left.Normal.y * right.y) + (left.Normal.z * right.z) + (left.D * right.w);
        }

        /// <summary>
        /// Calculates the dot product of a specified vector and the normal of the plane plus the distance value of the plane.
        /// </summary>
        /// <param name="left">The source plane.</param>
        /// <param name="right">The source vector.</param>
        /// <param name="result">When the method completes, contains the dot product of a specified vector and the normal of the Plane plus the distance value of the plane.</param>
        public static void DotCoordinate(ref Plane left, ref Vector3D right, out double result)
        {
            result = (left.Normal.x * right.x) + (left.Normal.y * right.y) + (left.Normal.z * right.z) + left.D;
        }

        /// <summary>
        /// Calculates the dot product of a specified vector and the normal of the plane plus the distance value of the plane.
        /// </summary>
        /// <param name="left">The source plane.</param>
        /// <param name="right">The source vector.</param>
        /// <returns>The dot product of a specified vector and the normal of the Plane plus the distance value of the plane.</returns>
        public static double DotCoordinate(Plane left, Vector3D right)
        {
            return (left.Normal.x * right.x) + (left.Normal.y * right.y) + (left.Normal.z * right.z) + left.D;
        }

        /// <summary>
        /// Calculates the dot product of the specified vector and the normal of the plane.
        /// </summary>
        /// <param name="left">The source plane.</param>
        /// <param name="right">The source vector.</param>
        /// <param name="result">When the method completes, contains the dot product of the specified vector and the normal of the plane.</param>
        public static void DotNormal(ref Plane left, ref Vector3D right, out double result)
        {
            result = (left.Normal.x * right.x) + (left.Normal.y * right.y) + (left.Normal.z * right.z);
        }

        /// <summary>
        /// Calculates the dot product of the specified vector and the normal of the plane.
        /// </summary>
        /// <param name="left">The source plane.</param>
        /// <param name="right">The source vector.</param>
        /// <returns>The dot product of the specified vector and the normal of the plane.</returns>
        public static double DotNormal(Plane left, Vector3D right)
        {
            return (left.Normal.x * right.x) + (left.Normal.y * right.y) + (left.Normal.z * right.z);
        }

        /// <summary>
        /// Changes the coefficients of the normal vector of the plane to make it of unit length.
        /// </summary>
        /// <param name="plane">The source plane.</param>
        /// <param name="result">When the method completes, contains the normalized plane.</param>
        public static void Normalize(ref Plane plane, out Plane result)
        {
            double magnitude = 1.0f / (double)(Math.Sqrt((plane.Normal.x * plane.Normal.x) + (plane.Normal.y * plane.Normal.y) + (plane.Normal.z * plane.Normal.z)));

            result.Normal.x = plane.Normal.x * magnitude;
            result.Normal.y = plane.Normal.y * magnitude;
            result.Normal.z = plane.Normal.z * magnitude;
            result.D = plane.D * magnitude;
        }

        /// <summary>
        /// Changes the coefficients of the normal vector of the plane to make it of unit length.
        /// </summary>
        /// <param name="plane">The source plane.</param>
        /// <returns>The normalized plane.</returns>
        public static Plane Normalize(Plane plane)
        {
            double magnitude = 1.0f / (double)(Math.Sqrt((plane.Normal.x * plane.Normal.x) + (plane.Normal.y * plane.Normal.y) + (plane.Normal.z * plane.Normal.z)));
            return new Plane(plane.Normal.x * magnitude, plane.Normal.y * magnitude, plane.Normal.z * magnitude, plane.D * magnitude);
        }

        /// <summary>
        /// Transforms a normalized plane by a quaternion rotation.
        /// </summary>
        /// <param name="plane">The normalized source plane.</param>
        /// <param name="rotation">The quaternion rotation.</param>
        /// <param name="result">When the method completes, contains the transformed plane.</param>
        public static void Transform(ref Plane plane, ref Quaternion rotation, out Plane result)
        {
            double x2 = rotation.x + rotation.x;
            double y2 = rotation.y + rotation.y;
            double z2 = rotation.z + rotation.z;
            double wx = rotation.w * x2;
            double wy = rotation.w * y2;
            double wz = rotation.w * z2;
            double xx = rotation.x * x2;
            double xy = rotation.x * y2;
            double xz = rotation.x * z2;
            double yy = rotation.y * y2;
            double yz = rotation.y * z2;
            double zz = rotation.z * z2;

            double x = plane.Normal.x;
            double y = plane.Normal.y;
            double z = plane.Normal.z;

            /*
             * Note:
             * Factor common arithmetic out of loop.
            */
            result.Normal.x = ((x * ((1.0f - yy) - zz)) + (y * (xy - wz))) + (z * (xz + wy));
            result.Normal.y = ((x * (xy + wz)) + (y * ((1.0f - xx) - zz))) + (z * (yz - wx));
            result.Normal.z = ((x * (xz - wy)) + (y * (yz + wx))) + (z * ((1.0f - xx) - yy));
            result.D = plane.D;
        }

        /// <summary>
        /// Transforms a normalized plane by a quaternion rotation.
        /// </summary>
        /// <param name="plane">The normalized source plane.</param>
        /// <param name="rotation">The quaternion rotation.</param>
        /// <returns>The transformed plane.</returns>
        public static Plane Transform(Plane plane, Quaternion rotation)
        {
            Plane result;
            double x2 = rotation.x + rotation.x;
            double y2 = rotation.y + rotation.y;
            double z2 = rotation.z + rotation.z;
            double wx = rotation.w * x2;
            double wy = rotation.w * y2;
            double wz = rotation.w * z2;
            double xx = rotation.x * x2;
            double xy = rotation.x * y2;
            double xz = rotation.x * z2;
            double yy = rotation.y * y2;
            double yz = rotation.y * z2;
            double zz = rotation.z * z2;

            double x = plane.Normal.x;
            double y = plane.Normal.y;
            double z = plane.Normal.z;

            /*
             * Note:
             * Factor common arithmetic out of loop.
            */
            result.Normal.x = ((x * ((1.0f - yy) - zz)) + (y * (xy - wz))) + (z * (xz + wy));
            result.Normal.y = ((x * (xy + wz)) + (y * ((1.0f - xx) - zz))) + (z * (yz - wx));
            result.Normal.z = ((x * (xz - wy)) + (y * (yz + wx))) + (z * ((1.0f - xx) - yy));
            result.D = plane.D;

            return result;
        }

        /// <summary>
        /// Transforms an array of normalized planes by a quaternion rotation.
        /// </summary>
        /// <param name="planes">The array of normalized planes to transform.</param>
        /// <param name="rotation">The quaternion rotation.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="planes"/> is <c>null</c>.</exception>
        public static void Transform(Plane[] planes, ref Quaternion rotation)
        {
            if (planes == null)
                throw new ArgumentNullException("planes");

            double x2 = rotation.x + rotation.x;
            double y2 = rotation.y + rotation.y;
            double z2 = rotation.z + rotation.z;
            double wx = rotation.w * x2;
            double wy = rotation.w * y2;
            double wz = rotation.w * z2;
            double xx = rotation.x * x2;
            double xy = rotation.x * y2;
            double xz = rotation.x * z2;
            double yy = rotation.y * y2;
            double yz = rotation.y * z2;
            double zz = rotation.z * z2;

            for (int i = 0; i < planes.Length; ++i)
            {
                double x = planes[i].Normal.x;
                double y = planes[i].Normal.y;
                double z = planes[i].Normal.z;

                /*
                 * Note:
                 * Factor common arithmetic out of loop.
                */
                planes[i].Normal.x = ((x * ((1.0f - yy) - zz)) + (y * (xy - wz))) + (z * (xz + wy));
                planes[i].Normal.y = ((x * (xy + wz)) + (y * ((1.0f - xx) - zz))) + (z * (yz - wx));
                planes[i].Normal.z = ((x * (xz - wy)) + (y * (yz + wx))) + (z * ((1.0f - xx) - yy));
            }
        }

        /// <summary>
        /// Transforms a normalized plane by a matrix.
        /// </summary>
        /// <param name="plane">The normalized source plane.</param>
        /// <param name="transformation">The transformation matrix.</param>
        /// <param name="result">When the method completes, contains the transformed plane.</param>
        public static void Transform(ref Plane plane, ref Matrix4D transformation, out Plane result)
        {
            double x = plane.Normal.x;
            double y = plane.Normal.y;
            double z = plane.Normal.z;
            double d = plane.D;

            Matrix4D inverse;
            Matrix4D.Inverse(ref transformation, out inverse);

            result.Normal.x = (((x * inverse.M11) + (y * inverse.M12)) + (z * inverse.M13)) + (d * inverse.M14);
            result.Normal.y = (((x * inverse.M21) + (y * inverse.M22)) + (z * inverse.M23)) + (d * inverse.M24);
            result.Normal.z = (((x * inverse.M31) + (y * inverse.M32)) + (z * inverse.M33)) + (d * inverse.M34);
            result.D = (((x * inverse.M41) + (y * inverse.M42)) + (z * inverse.M43)) + (d * inverse.M44);
        }

        /// <summary>
        /// Transforms a normalized plane by a matrix.
        /// </summary>
        /// <param name="plane">The normalized source plane.</param>
        /// <param name="transformation">The transformation matrix.</param>
        /// <returns>When the method completes, contains the transformed plane.</returns>
        public static Plane Transform(Plane plane, Matrix4D transformation)
        {
            Plane result;
            double x = plane.Normal.x;
            double y = plane.Normal.y;
            double z = plane.Normal.z;
            double d = plane.D;

            transformation.Inverse();
            result.Normal.x = (((x * transformation.M11) + (y * transformation.M12)) + (z * transformation.M13)) + (d * transformation.M14);
            result.Normal.y = (((x * transformation.M21) + (y * transformation.M22)) + (z * transformation.M23)) + (d * transformation.M24);
            result.Normal.z = (((x * transformation.M31) + (y * transformation.M32)) + (z * transformation.M33)) + (d * transformation.M34);
            result.D = (((x * transformation.M41) + (y * transformation.M42)) + (z * transformation.M43)) + (d * transformation.M44);

            return result;
        }

        /// <summary>
        /// Transforms an array of normalized planes by a matrix.
        /// </summary>
        /// <param name="planes">The array of normalized planes to transform.</param>
        /// <param name="transformation">The transformation matrix.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="planes"/> is <c>null</c>.</exception>
        public static void Transform(Plane[] planes, ref Matrix4D transformation)
        {
            if (planes == null)
                throw new ArgumentNullException("planes");

            Matrix4D inverse;
            Matrix4D.Inverse(ref transformation, out inverse);

            for (int i = 0; i < planes.Length; ++i)
            {
                Transform(ref planes[i], ref transformation, out planes[i]);
            }
        }

        /// <summary>
        /// Scales each component of the plane by the given value.
        /// </summary>
        /// <param name="scale">The amount by which to scale the plane.</param>
        /// <param name="plane">The plane to scale.</param>
        /// <returns>The scaled plane.</returns>
        public static Plane operator *(double scale, Plane plane)
        {
            return new Plane(plane.Normal.x * scale, plane.Normal.y * scale, plane.Normal.z * scale, plane.D * scale);
        }

        /// <summary>
        /// Scales each component of the plane by the given value.
        /// </summary>
        /// <param name="plane">The plane to scale.</param>
        /// <param name="scale">The amount by which to scale the plane.</param>
        /// <returns>The scaled plane.</returns>
        public static Plane operator *(Plane plane, double scale)
        {
            return new Plane(plane.Normal.x * scale, plane.Normal.y * scale, plane.Normal.z * scale, plane.D * scale);
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Plane left, Plane right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Plane left, Plane right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "A:{0} B:{1} C:{2} D:{3}", Normal.x, Normal.y, Normal.z, D);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            return string.Format(CultureInfo.CurrentCulture, "A:{0} B:{1} C:{2} D:{3}", Normal.x.ToString(format, CultureInfo.CurrentCulture),
                Normal.y.ToString(format, CultureInfo.CurrentCulture), Normal.z.ToString(format, CultureInfo.CurrentCulture), D.ToString(format, CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(IFormatProvider formatProvider)
        {
            return string.Format(formatProvider, "A:{0} B:{1} C:{2} D:{3}", Normal.x, Normal.y, Normal.z, D);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format(formatProvider, "A:{0} B:{1} C:{2} D:{3}", Normal.x.ToString(format, formatProvider),
                Normal.y.ToString(format, formatProvider), Normal.z.ToString(format, formatProvider), D.ToString(format, formatProvider));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Normal.GetHashCode() + D.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="SlimMath.Vector4d"/> is equal to this instance.
        /// </summary>
        /// <param name="value">The <see cref="SlimMath.Vector4d"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="SlimMath.Vector4d"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Plane value)
        {
            return Normal == value.Normal && D == value.D;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Plane)obj);
        }

#if SlimDX1xInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimMath.Plane"/> to <see cref="SlimDX.Plane"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SlimDX.Plane(Plane value)
        {
            return new SlimDX.Plane(value.Normal, value.D);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimDX.Plane"/> to <see cref="SlimMath.Plane"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Plane(SlimDX.Plane value)
        {
            return new Plane(value.Normal, value.D);
        }
#endif

#if XnaInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimMath.Plane"/> to <see cref="Microsoft.xna.Framework.Plane"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Microsoft.xna.Framework.Plane(Plane value)
        {
            return new Microsoft.xna.Framework.Plane(value.Normal, value.D);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Microsoft.xna.Framework.Plane"/> to <see cref="SlimMath.Plane"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Plane(Microsoft.xna.Framework.Plane value)
        {
            return new Plane(value.Normal, value.D);
        }
#endif
    }
}
