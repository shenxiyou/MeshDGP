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
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Globalization;

namespace GraphicResearchHuiZhao
{
    /// <summary>
    /// Represents a four dimensional mathematical quaternion.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    [TypeConverter(typeof(GraphicResearchHuiZhao.QuaternionConverter))]
    public struct Quaternion : IEquatable<Quaternion>, IFormattable
    {
        /// <summary>
        /// The size of the <see cref="SlimMath.Quaternion"/> type, in bytes.
        /// </summary>
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(Quaternion));

        /// <summary>
        /// A <see cref="SlimMath.Quaternion"/> with all of its components set to zero.
        /// </summary>
        public static readonly Quaternion Zero = new Quaternion();

        /// <summary>
        /// A <see cref="SlimMath.Quaternion"/> with all of its components set to one.
        /// </summary>
        public static readonly Quaternion One = new Quaternion(1.0f, 1.0f, 1.0f, 1.0f);

        /// <summary>
        /// The identity <see cref="SlimMath.Quaternion"/> (0, 0, 0, 1).
        /// </summary>
        public static readonly Quaternion Identity = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

        /// <summary>
        /// The X component of the quaternion.
        /// </summary>
        public double x;

        /// <summary>
        /// The Y component of the quaternion.
        /// </summary>
        public double y;

        /// <summary>
        /// The Z component of the quaternion.
        /// </summary>
        public double z;

        /// <summary>
        /// The W component of the quaternion.
        /// </summary>
        public double w;

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Quaternion"/> struct.
        /// </summary>
        /// <param name="value">The value that will be assigned to all components.</param>
        public Quaternion(double value)
        {
            x = value;
            y = value;
            z = value;
            w = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Quaternion"/> struct.
        /// </summary>
        /// <param name="value">A vector containing the values with which to initialize the components.</param>
        public Quaternion(Vector4D value)
        {
            x = value.x;
            y = value.y;
            z = value.z;
            w = value.w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Quaternion"/> struct.
        /// </summary>
        /// <param name="value">A vector containing the values with which to initialize the X, Y, and Z components.</param>
        /// <param name="w">Initial value for the W component of the quaternion.</param>
        public Quaternion(Vector3D value, double w)
        {
            x = value.x;
            y = value.y;
            z = value.z;
            this.w = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Quaternion"/> struct.
        /// </summary>
        /// <param name="value">A vector containing the values with which to initialize the X and Y components.</param>
        /// <param name="z">Initial value for the Z component of the quaternion.</param>
        /// <param name="w">Initial value for the W component of the quaternion.</param>
        public Quaternion(Vector2D value, double z, double w)
        {
            x = value.x;
            y = value.y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Quaternion"/> struct.
        /// </summary>
        /// <param name="x">Initial value for the X component of the quaternion.</param>
        /// <param name="y">Initial value for the Y component of the quaternion.</param>
        /// <param name="z">Initial value for the Z component of the quaternion.</param>
        /// <param name="w">Initial value for the W component of the quaternion.</param>
        public Quaternion(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Quaternion"/> struct.
        /// </summary>
        /// <param name="values">The values to assign to the X, Y, Z, and W components of the quaternion. This must be an array with four elements.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than four elements.</exception>
        public Quaternion(double[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length != 4)
                throw new ArgumentOutOfRangeException("values", "There must be four and only four input values for Quaternion.");

            x = values[0];
            y = values[1];
            z = values[2];
            w = values[3];
        }

        /// <summary>
        /// Gets a value indicating whether this instance is equivalent to the identity quaternion.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is an identity quaternion; otherwise, <c>false</c>.
        /// </value>
        public bool IsIdentity
        {
            get { return this.Equals(Identity); }
        }

        /// <summary>
        /// Gets a value indicting whether this instance is normalized.
        /// </summary>
        public bool IsNormalized
        {
            get { return Math.Abs((x * x) + (y * y) + (z * z) + (w * w) - 1f) < Utilities.ZeroTolerance; }
        }

        /// <summary>
        /// Gets the angle of the quaternion.
        /// </summary>
        /// <value>The quaternion's angle.</value>
        /// 

        public double Angle
        {
            get
            {
                double length = (x * x) + (y * y) + (z * z);
                if (length < Utilities.ZeroTolerance)
                    return 0.0f;

                return (double)(2.0 * Math.Acos(w));
            }
        }

        /// <summary>
        /// Gets the axis components of the quaternion.
        /// </summary>
        /// <value>The axis components of the quaternion.</value>
        /// 

        public Vector3D Axis
        {
            get
            {
                double length = (x * x) + (y * y) + (z * z);
                if (length < Utilities.ZeroTolerance)
                    return Vector3D.UnitX;

                double inv = 1.0f / length;
                return new Vector3D(x * inv, y * inv, z * inv);
            }
        }

        /// <summary>
        /// Calculates the length of the quaternion.
        /// </summary>
        /// <remarks>
        /// <see cref="SlimMath.Quaternion.LengthSquared"/> may be preferred when only the relative length is needed
        /// and speed is of the essence.
        /// </remarks>
        public double Length
        {
            get { return (double)Math.Sqrt((x * x) + (y * y) + (z * z) + (w * w)); }
        }

        /// <summary>
        /// Calculates the squared length of the quaternion.
        /// </summary>
        /// <remarks>
        /// This property may be preferred to <see cref="SlimMath.Quaternion.Length"/> when only a relative length is needed
        /// and speed is of the essence.
        /// </remarks>
        public double LengthSquared
        {
            get { return (x * x) + (y * y) + (z * z) + (w * w); }
        }

        /// <summary>
        /// Gets or sets the component at the specified index.
        /// </summary>
        /// <value>The value of the X, Y, Z, or W component, depending on the index.</value>
        /// <param name="index">The index of the component to access. Use 0 for the X component, 1 for the Y component, 2 for the Z component, and 3 for the W component.</param>
        /// <returns>The value of the component at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="index"/> is out of the range [0, 3].</exception>
        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    case 3: return w;
                }

                throw new ArgumentOutOfRangeException("index", "Indices for Quaternion run from 0 to 3, inclusive.");
            }

            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    case 3: w = value; break;
                    default: throw new ArgumentOutOfRangeException("index", "Indices for Quaternion run from 0 to 3, inclusive.");
                }
            }
        }

        /// <summary>
        /// Conjugates the quaternion.
        /// </summary>
        public Quaternion Conjugate()
        {
            Quaternion con = new Quaternion(-this.x, -this.y, -this.z, this.w);
            return con;
        }

        public Vector3D ImagePart
        {
            get
            {
                Vector3D image = new Vector3D(this.x, this.y, this.z);
                return image;
            }

        }

        /// <summary>
        /// Reverses the direction of a given quaternion.
        /// </summary>
        public void Negate()
        {
            this.x = -x;
            this.y = -y;
            this.z = -z;
            this.w = -w;
        }

        /// <summary>
        /// Conjugates and renormalizes the quaternion.
        /// </summary>
        public void Invert()
        {
            double lengthSq = LengthSquared;
            if (lengthSq > Utilities.ZeroTolerance)
            {
                lengthSq = 1.0f / lengthSq;

                x = -x * lengthSq;
                y = -y * lengthSq;
                z = -z * lengthSq;
                w = w * lengthSq;
            }
        }

        /// <summary>
        /// Converts the quaternion into a unit quaternion.
        /// </summary>
        public void Normalize()
        {
            double length = Length;
            if (length > Utilities.ZeroTolerance)
            {
                double inverse = 1.0f / length;
                x *= inverse;
                y *= inverse;
                z *= inverse;
                w *= inverse;
            }
        }

        /// <summary>
        /// Exponentiates a quaternion.
        /// </summary>
        public void Exponential()
        {
            Exponential(ref this, out this);
        }

        /// <summary>
        /// Calculates the natural logarithm of the specified quaternion.
        /// </summary>
        public void Logarithm()
        {
            Logarithm(ref this, out this);
        }

        /// <summary>
        /// Creates an array containing the elements of the quaternion.
        /// </summary>
        /// <returns>A four-element array containing the components of the quaternion.</returns>
        public double[] ToArray()
        {
            return new double[] { x, y, z, w };
        }

        /// <summary>
        /// Adds two quaternions.
        /// </summary>
        /// <param name="left">The first quaternion to add.</param>
        /// <param name="right">The second quaternion to add.</param>
        /// <param name="result">When the method completes, contains the sum of the two quaternions.</param>
        public static void Add(ref Quaternion left, ref Quaternion right, out Quaternion result)
        {
            result.x = left.x + right.x;
            result.y = left.y + right.y;
            result.z = left.z + right.z;
            result.w = left.w + right.w;
        }

        /// <summary>
        /// Adds two quaternions.
        /// </summary>
        /// <param name="left">The first quaternion to add.</param>
        /// <param name="right">The second quaternion to add.</param>
        /// <returns>The sum of the two quaternions.</returns>
        public static Quaternion Add(Quaternion left, Quaternion right)
        {
            Quaternion result;
            Add(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Subtracts two quaternions.
        /// </summary>
        /// <param name="left">The first quaternion to subtract.</param>
        /// <param name="right">The second quaternion to subtract.</param>
        /// <param name="result">When the method completes, contains the difference of the two quaternions.</param>
        public static void Subtract(ref Quaternion left, ref Quaternion right, out Quaternion result)
        {
            result.x = left.x - right.x;
            result.y = left.y - right.y;
            result.z = left.z - right.z;
            result.w = left.w - right.w;
        }

        /// <summary>
        /// Subtracts two quaternions.
        /// </summary>
        /// <param name="left">The first quaternion to subtract.</param>
        /// <param name="right">The second quaternion to subtract.</param>
        /// <returns>The difference of the two quaternions.</returns>
        public static Quaternion Subtract(Quaternion left, Quaternion right)
        {
            Quaternion result;
            Subtract(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Scales a quaternion by the given value.
        /// </summary>
        /// <param name="value">The quaternion to scale.</param>
        /// <param name="scalar">The amount by which to scale the quaternion.</param>
        /// <param name="result">When the method completes, contains the scaled quaternion.</param>
        public static void Multiply(ref Quaternion value, double scalar, out Quaternion result)
        {
            result.x = value.x * scalar;
            result.y = value.y * scalar;
            result.z = value.z * scalar;
            result.w = value.w * scalar;
        }

        /// <summary>
        /// Scales a quaternion by the given value.
        /// </summary>
        /// <param name="value">The quaternion to scale.</param>
        /// <param name="scalar">The amount by which to scale the quaternion.</param>
        /// <returns>The scaled quaternion.</returns>
        public static Quaternion Multiply(Quaternion value, double scalar)
        {
            Quaternion result;
            Multiply(ref value, scalar, out result);
            return result;
        }

        /// <summary>
        /// Modulates a quaternion by another.
        /// </summary>
        /// <param name="left">The first quaternion to modulate.</param>
        /// <param name="right">The second quaternion to modulate.</param>
        /// <param name="result">When the moethod completes, contains the modulated quaternion.</param>
        public static void Multiply(ref Quaternion left, ref Quaternion right, out Quaternion result)
        {
            double lx = left.x;
            double ly = left.y;
            double lz = left.z;
            double lw = left.w;
            double rx = right.x;
            double ry = right.y;
            double rz = right.z;
            double rw = right.w;

            result.x = (rx * lw + lx * rw + ry * lz) - (rz * ly);
            result.y = (ry * lw + ly * rw + rz * lx) - (rx * lz);
            result.z = (rz * lw + lz * rw + rx * ly) - (ry * lx);
            result.w = (rw * lw) - (rx * lx + ry * ly + rz * lz);
        }

        /// <summary>
        /// Modulates a quaternion by another.
        /// </summary>
        /// <param name="left">The first quaternion to modulate.</param>
        /// <param name="right">The second quaternion to modulate.</param>
        /// <returns>The modulated quaternion.</returns>
        public static Quaternion Multiply(Quaternion left, Quaternion right)
        {
            Quaternion result;
            Multiply(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <param name="result">When the method completes, contains the scaled vector.</param>
        public static void Divide(ref Quaternion value, double scalar, out Quaternion result)
        {
            result = new Quaternion(value.x / scalar, value.y / scalar, value.z / scalar, value.w / scalar);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Quaternion Divide(Quaternion value, double scalar)
        {
            return new Quaternion(value.x / scalar, value.y / scalar, value.z / scalar, value.w / scalar);
        }

        /// <summary>
        /// Reverses the direction of a given quaternion.
        /// </summary>
        /// <param name="value">The quaternion to negate.</param>
        /// <param name="result">When the method completes, contains a quaternion facing in the opposite direction.</param>
        public static void Negate(ref Quaternion value, out Quaternion result)
        {
            result.x = -value.x;
            result.y = -value.y;
            result.z = -value.z;
            result.w = -value.w;
        }

        /// <summary>
        /// Reverses the direction of a given quaternion.
        /// </summary>
        /// <param name="value">The quaternion to negate.</param>
        /// <returns>A quaternion facing in the opposite direction.</returns>
        public static Quaternion Negate(Quaternion value)
        {
            Quaternion result;
            Negate(ref value, out result);
            return result;
        }

        /// <summary>
        /// Returns a <see cref="SlimMath.Quaternion"/> containing the 4D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 2D triangle.
        /// </summary>
        /// <param name="value1">A <see cref="SlimMath.Quaternion"/> containing the 4D Cartesian coordinates of vertex 1 of the triangle.</param>
        /// <param name="value2">A <see cref="SlimMath.Quaternion"/> containing the 4D Cartesian coordinates of vertex 2 of the triangle.</param>
        /// <param name="value3">A <see cref="SlimMath.Quaternion"/> containing the 4D Cartesian coordinates of vertex 3 of the triangle.</param>
        /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="value2"/>).</param>
        /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="value3"/>).</param>
        /// <param name="result">When the method completes, contains a new <see cref="SlimMath.Quaternion"/> containing the 4D Cartesian coordinates of the specified point.</param>
        public static void Barycentric(ref Quaternion value1, ref Quaternion value2, ref Quaternion value3, double amount1, double amount2, out Quaternion result)
        {
            Quaternion start, end;
            Slerp(ref value1, ref value2, amount1 + amount2, out start);
            Slerp(ref value1, ref value3, amount1 + amount2, out end);
            Slerp(ref start, ref end, amount2 / (amount1 + amount2), out result);
        }

        /// <summary>
        /// Returns a <see cref="SlimMath.Quaternion"/> containing the 4D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 2D triangle.
        /// </summary>
        /// <param name="value1">A <see cref="SlimMath.Quaternion"/> containing the 4D Cartesian coordinates of vertex 1 of the triangle.</param>
        /// <param name="value2">A <see cref="SlimMath.Quaternion"/> containing the 4D Cartesian coordinates of vertex 2 of the triangle.</param>
        /// <param name="value3">A <see cref="SlimMath.Quaternion"/> containing the 4D Cartesian coordinates of vertex 3 of the triangle.</param>
        /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="value2"/>).</param>
        /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="value3"/>).</param>
        /// <returns>A new <see cref="SlimMath.Quaternion"/> containing the 4D Cartesian coordinates of the specified point.</returns>
        public static Quaternion Barycentric(Quaternion value1, Quaternion value2, Quaternion value3, double amount1, double amount2)
        {
            Quaternion result;
            Barycentric(ref value1, ref value2, ref value3, amount1, amount2, out result);
            return result;
        }

        /// <summary>
        /// Conjugates a quaternion.
        /// </summary>
        /// <param name="value">The quaternion to conjugate.</param>
        /// <param name="result">When the method completes, contains the conjugated quaternion.</param>
        public static void Conjugate(ref Quaternion value, out Quaternion result)
        {
            result.x = -value.x;
            result.y = -value.y;
            result.z = -value.z;
            result.w = value.w;
        }

        /// <summary>
        /// Conjugates a quaternion.
        /// </summary>
        /// <param name="value">The quaternion to conjugate.</param>
        /// <returns>The conjugated quaternion.</returns>
        public static Quaternion Conjugate(Quaternion value)
        {
            Quaternion result;
            Conjugate(ref value, out result);
            return result;
        }

        /// <summary>
        /// Calculates the dot product of two quaternions.
        /// </summary>
        /// <param name="left">First source quaternion.</param>
        /// <param name="right">Second source quaternion.</param>
        /// <param name="result">When the method completes, contains the dot product of the two quaternions.</param>
        public static void Dot(ref Quaternion left, ref Quaternion right, out double result)
        {
            result = (left.x * right.x) + (left.y * right.y) + (left.z * right.z) + (left.w * right.w);
        }

        /// <summary>
        /// Calculates the dot product of two quaternions.
        /// </summary>
        /// <param name="left">First source quaternion.</param>
        /// <param name="right">Second source quaternion.</param>
        /// <returns>The dot product of the two quaternions.</returns>
        public static double Dot(Quaternion left, Quaternion right)
        {
            return (left.x * right.x) + (left.y * right.y) + (left.z * right.z) + (left.w * right.w);
        }

        /// <summary>
        /// Exponentiates a quaternion.
        /// </summary>
        /// <param name="value">The quaternion to exponentiate.</param>
        /// <param name="result">When the method completes, contains the exponentiated quaternion.</param>
        public static void Exponential(ref Quaternion value, out Quaternion result)
        {
            double angle = (double)Math.Sqrt((value.x * value.x) + (value.y * value.y) + (value.z * value.z));
            double sin = (double)Math.Sin(angle);

            if (Math.Abs(sin) >= Utilities.ZeroTolerance)
            {
                double coeff = sin / angle;
                result.x = coeff * value.x;
                result.y = coeff * value.y;
                result.z = coeff * value.z;
            }
            else
            {
                result = value;
            }

            result.w = (double)Math.Cos(angle);
        }

        /// <summary>
        /// Exponentiates a quaternion.
        /// </summary>
        /// <param name="value">The quaternion to exponentiate.</param>
        /// <returns>The exponentiated quaternion.</returns>
        public static Quaternion Exponential(Quaternion value)
        {
            Quaternion result;
            Exponential(ref value, out result);
            return result;
        }

        /// <summary>
        /// Conjugates and renormalizes the quaternion.
        /// </summary>
        /// <param name="value">The quaternion to conjugate and renormalize.</param>
        /// <param name="result">When the method completes, contains the conjugated and renormalized quaternion.</param>
        public static void Invert(ref Quaternion value, out Quaternion result)
        {
            result = value;
            result.Invert();
        }

        /// <summary>
        /// Conjugates and renormalizes the quaternion.
        /// </summary>
        /// <param name="value">The quaternion to conjugate and renormalize.</param>
        /// <returns>The conjugated and renormalized quaternion.</returns>
        public static Quaternion Invert(Quaternion value)
        {
            Quaternion result;
            Invert(ref value, out result);
            return result;
        }

        /// <summary>
        /// Performs a linear interpolation between two quaternions.
        /// </summary>
        /// <param name="start">Start quaternion.</param>
        /// <param name="end">End quaternion.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <param name="result">When the method completes, contains the linear interpolation of the two quaternions.</param>
        /// <remarks>
        /// This method performs the linear interpolation based on the following formula.
        /// <code>start + (end - start) * amount</code>
        /// Passing <paramref name="amount"/> a value of 0 will cause <paramref name="start"/> to be returned; a value of 1 will cause <paramref name="end"/> to be returned. 
        /// </remarks>
        public static void Lerp(ref Quaternion start, ref Quaternion end, double amount, out Quaternion result)
        {
            double inverse = 1.0f - amount;

            if (Dot(start, end) >= 0.0f)
            {
                result.x = (inverse * start.x) + (amount * end.x);
                result.y = (inverse * start.y) + (amount * end.y);
                result.z = (inverse * start.z) + (amount * end.z);
                result.w = (inverse * start.w) + (amount * end.w);
            }
            else
            {
                result.x = (inverse * start.x) - (amount * end.x);
                result.y = (inverse * start.y) - (amount * end.y);
                result.z = (inverse * start.z) - (amount * end.z);
                result.w = (inverse * start.w) - (amount * end.w);
            }

            result.Normalize();
        }

        /// <summary>
        /// Performs a linear interpolation between two quaternion.
        /// </summary>
        /// <param name="start">Start quaternion.</param>
        /// <param name="end">End quaternion.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <returns>The linear interpolation of the two quaternions.</returns>
        /// <remarks>
        /// This method performs the linear interpolation based on the following formula.
        /// <code>start + (end - start) * amount</code>
        /// Passing <paramref name="amount"/> a value of 0 will cause <paramref name="start"/> to be returned; a value of 1 will cause <paramref name="end"/> to be returned. 
        /// </remarks>
        public static Quaternion Lerp(Quaternion start, Quaternion end, double amount)
        {
            Quaternion result;
            Lerp(ref start, ref end, amount, out result);
            return result;
        }

        /// <summary>
        /// Calculates the natural logarithm of the specified quaternion.
        /// </summary>
        /// <param name="value">The quaternion whose logarithm will be calculated.</param>
        /// <param name="result">When the method completes, contains the natural logarithm of the quaternion.</param>
        public static void Logarithm(ref Quaternion value, out Quaternion result)
        {
            if (Math.Abs(value.w) < 1.0f)
            {
                double angle = (double)Math.Acos(value.w);
                double sin = (double)Math.Sin(angle);

                if (Math.Abs(sin) >= Utilities.ZeroTolerance)
                {
                    double coeff = angle / sin;
                    result.x = value.x * coeff;
                    result.y = value.y * coeff;
                    result.z = value.z * coeff;
                }
                else
                {
                    result = value;
                }
            }
            else
            {
                result = value;
            }

            result.w = 0.0f;
        }

        /// <summary>
        /// Calculates the natural logarithm of the specified quaternion.
        /// </summary>
        /// <param name="value">The quaternion whose logarithm will be calculated.</param>
        /// <returns>The natural logarithm of the quaternion.</returns>
        public static Quaternion Logarithm(Quaternion value)
        {
            Quaternion result;
            Logarithm(ref value, out result);
            return result;
        }

        /// <summary>
        /// Converts the quaternion into a unit quaternion.
        /// </summary>
        /// <param name="value">The quaternion to normalize.</param>
        /// <param name="result">When the method completes, contains the normalized quaternion.</param>
        public static void Normalize(ref Quaternion value, out Quaternion result)
        {
            Quaternion temp = value;
            result = temp;
            result.Normalize();
        }

        /// <summary>
        /// Converts the quaternion into a unit quaternion.
        /// </summary>
        /// <param name="value">The quaternion to normalize.</param>
        /// <returns>The normalized quaternion.</returns>
        public static Quaternion Normalize(Quaternion value)
        {
            value.Normalize();
            return value;
        }

        /// <summary>
        /// Creates a quaternion given a rotation and an axis.
        /// </summary>
        /// <param name="axis">The axis of rotation.</param>
        /// <param name="angle">The angle of rotation.</param>
        /// <param name="result">When the method completes, contains the newly created quaternion.</param>
        /// 

        public static void RotationAxis(ref Vector3D axis, double angle, 
            out Quaternion result)
        {
            Vector3D normalized;
            Vector3D.Normalize(ref axis, out normalized);

            double half = angle * 0.5f;
            double sin = (double)Math.Sin(half);
            double cos = (double)Math.Cos(half);

            result.x = normalized.x * sin;
            result.y = normalized.y * sin;
            result.z = normalized.z * sin;
            result.w = cos;
        }

        /// <summary>
        /// Creates a quaternion given a rotation and an axis.
        /// </summary>
        /// <param name="axis">The axis of rotation.</param>
        /// <param name="angle">The angle of rotation.</param>
        /// <returns>The newly created quaternion.</returns>
        public static Quaternion RotationAxis(Vector3D axis, double angle)
        {
            Quaternion result;
            RotationAxis(ref axis, angle, out result);
            return result;
        }

        /// <summary>
        /// Creates a quaternion given a rotation matrix.
        /// </summary>
        /// <param name="matrix">The rotation matrix.</param>
        /// <param name="result">When the method completes, contains the newly created quaternion.</param>
        /// 

        public static void RotationMatrix(ref Matrix4D matrix, out Quaternion result)
        {
            double sqrt;
            double half;
            double scale = matrix.M11 + matrix.M22 + matrix.M33;

            if (scale > 0.0f)
            {
                sqrt = (double)Math.Sqrt(scale + 1.0f);
                result.w = sqrt * 0.5f;
                sqrt = 0.5f / sqrt;

                result.x = (matrix.M23 - matrix.M32) * sqrt;
                result.y = (matrix.M31 - matrix.M13) * sqrt;
                result.z = (matrix.M12 - matrix.M21) * sqrt;
            }
            else if ((matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33))
            {
                sqrt = (double)Math.Sqrt(1.0f + matrix.M11 - matrix.M22 - matrix.M33);
                half = 0.5f / sqrt;

                result.x = 0.5f * sqrt;
                result.y = (matrix.M12 + matrix.M21) * half;
                result.z = (matrix.M13 + matrix.M31) * half;
                result.w = (matrix.M23 - matrix.M32) * half;
            }
            else if (matrix.M22 > matrix.M33)
            {
                sqrt = (double)Math.Sqrt(1.0f + matrix.M22 - matrix.M11 - matrix.M33);
                half = 0.5f / sqrt;

                result.x = (matrix.M21 + matrix.M12) * half;
                result.y = 0.5f * sqrt;
                result.z = (matrix.M32 + matrix.M23) * half;
                result.w = (matrix.M31 - matrix.M13) * half;
            }
            else
            {
                sqrt = (double)Math.Sqrt(1.0f + matrix.M33 - matrix.M11 - matrix.M22);
                half = 0.5f / sqrt;

                result.x = (matrix.M31 + matrix.M13) * half;
                result.y = (matrix.M32 + matrix.M23) * half;
                result.z = 0.5f * sqrt;
                result.w = (matrix.M12 - matrix.M21) * half;
            }
        }

        /// <summary>
        /// Creates a quaternion given a rotation matrix.
        /// </summary>
        /// <param name="matrix">The rotation matrix.</param>
        /// <returns>The newly created quaternion.</returns>
        public static Quaternion RotationMatrix(Matrix4D matrix)
        {
            Quaternion result;
            RotationMatrix(ref matrix, out result);
            return result;
        }

        /// <summary>
        /// Creates a quaternion given a yaw, pitch, and roll value.
        /// </summary>
        /// <param name="yaw">The yaw of rotation.</param>
        /// <param name="pitch">The pitch of rotation.</param>
        /// <param name="roll">The roll of rotation.</param>
        /// <param name="result">When the method completes, contains the newly created quaternion.</param>
        /// 

        public static void RotationYawPitchRoll(double yaw, double pitch, 
                                      double roll, out Quaternion result)
        {
            double halfRoll = roll * 0.5f;
            double halfPitch = pitch * 0.5f;
            double halfYaw = yaw * 0.5f;

            double sinRoll = (double)Math.Sin(halfRoll);
            double cosRoll = (double)Math.Cos(halfRoll);
            double sinPitch = (double)Math.Sin(halfPitch);
            double cosPitch = (double)Math.Cos(halfPitch);
            double sinYaw = (double)Math.Sin(halfYaw);
            double cosYaw = (double)Math.Cos(halfYaw);

            result.x = (cosYaw * sinPitch * cosRoll) + (sinYaw * cosPitch * sinRoll);
            result.y = (sinYaw * cosPitch * cosRoll) - (cosYaw * sinPitch * sinRoll);
            result.z = (cosYaw * cosPitch * sinRoll) - (sinYaw * sinPitch * cosRoll);
            result.w = (cosYaw * cosPitch * cosRoll) + (sinYaw * sinPitch * sinRoll);
        }


        public static Quaternion RotationYawPitchRollRight(Vector3D ypr)
        {
            Quaternion quaternion = new Quaternion();
            Quaternion.RotationYawPitchRollRight(ypr.x, ypr.y,ypr.z, out quaternion);
            return quaternion;
        }

        public static Quaternion RotationYawPitchRoll(Vector3D ypr)
        {
            Quaternion quaternion = new Quaternion();
            Quaternion.RotationYawPitchRoll(ypr.x, ypr.y, ypr.z, out quaternion);
            return quaternion;
        }

        public static void RotationYawPitchRollRight(double yaw, double pitch,
                                     double roll, out Quaternion result)
        {
            double halfRoll = roll * 0.5f;
            double halfPitch = pitch * 0.5f;
            double halfYaw = yaw * 0.5f;

            double sinRoll = (double)Math.Sin(halfRoll);
            double cosRoll = (double)Math.Cos(halfRoll);
            double sinPitch = (double)Math.Sin(halfPitch);
            double cosPitch = (double)Math.Cos(halfPitch);
            double sinYaw = (double)Math.Sin(halfYaw);
            double cosYaw = (double)Math.Cos(halfYaw);

            result.x = (sinYaw * cosPitch * cosRoll) - (cosYaw * sinPitch * sinRoll);
            result.y = (cosYaw * sinPitch * cosRoll) + (sinYaw * cosPitch * sinRoll);            
            result.z = (cosYaw * cosPitch * sinRoll) - (sinYaw * sinPitch * cosRoll);
            result.w = (cosYaw * cosPitch * cosRoll) + (sinYaw * sinPitch * sinRoll);
        }

        /// <summary>
        /// Creates a quaternion given a yaw, pitch, and roll value.
        /// </summary>
        /// <param name="yaw">The yaw of rotation.</param>
        /// <param name="pitch">The pitch of rotation.</param>
        /// <param name="roll">The roll of rotation.</param>
        /// <returns>The newly created quaternion.</returns>
        public static Quaternion RotationYawPitchRoll(double yaw, double pitch, double roll)
        {
            Quaternion result;
            RotationYawPitchRoll(yaw, pitch, roll, out result);
            return result;
        }

        /// <summary>
        /// Interpolates between two quaternions, using spherical linear interpolation.
        /// </summary>
        /// <param name="start">Start quaternion.</param>
        /// <param name="end">End quaternion.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <param name="result">When the method completes, contains the spherical linear interpolation of the two quaternions.</param>
        /// 

        public static void Slerp(ref Quaternion start, ref Quaternion end, 
                                 double amount, out Quaternion result)
        {
            double opposite;
            double inverse;
            double dot = Dot(start, end);

            if (Math.Abs(dot) > 1.0f - Utilities.ZeroTolerance)
            {
                inverse = 1.0f - amount;
                opposite = amount * Math.Sign(dot);
            }
            else
            {
                double acos = (double)Math.Acos(Math.Abs(dot));
                double invSin = (double)(1.0f / Math.Sin(acos));

                inverse = (double)Math.Sin((1.0f - amount) * acos) * invSin;
                opposite = (double)Math.Sin(amount * acos) * invSin * Math.Sign(dot);
            }

            result.x = (inverse * start.x) + (opposite * end.x);
            result.y = (inverse * start.y) + (opposite * end.y);
            result.z = (inverse * start.z) + (opposite * end.z);
            result.w = (inverse * start.w) + (opposite * end.w);
        }

        /// <summary>
        /// Interpolates between two quaternions, using spherical linear interpolation.
        /// </summary>
        /// <param name="start">Start quaternion.</param>
        /// <param name="end">End quaternion.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <returns>The spherical linear interpolation of the two quaternions.</returns>
        public static Quaternion Slerp(Quaternion start, Quaternion end, double amount)
        {
            Quaternion result;
            Slerp(ref start, ref end, amount, out result);
            return result;
        }

        /// <summary>
        /// Interpolates between quaternions, using spherical quadrangle interpolation.
        /// </summary>
        /// <param name="value1">First source quaternion.</param>
        /// <param name="value2">Second source quaternion.</param>
        /// <param name="value3">Thrid source quaternion.</param>
        /// <param name="value4">Fourth source quaternion.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of interpolation.</param>
        /// <param name="result">When the method completes, contains the spherical quadrangle interpolation of the quaternions.</param>
        public static void Squad(ref Quaternion value1, ref Quaternion value2, ref Quaternion value3, ref Quaternion value4, double amount, out Quaternion result)
        {
            Quaternion start, end;
            Slerp(ref value1, ref value4, amount, out start);
            Slerp(ref value2, ref value3, amount, out end);
            Slerp(ref start, ref end, 2.0f * amount * (1.0f - amount), out result);
        }

        /// <summary>
        /// Interpolates between quaternions, using spherical quadrangle interpolation.
        /// </summary>
        /// <param name="value1">First source quaternion.</param>
        /// <param name="value2">Second source quaternion.</param>
        /// <param name="value3">Thrid source quaternion.</param>
        /// <param name="value4">Fourth source quaternion.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of interpolation.</param>
        /// <returns>The spherical quadrangle interpolation of the quaternions.</returns>
        public static Quaternion Squad(Quaternion value1, Quaternion value2, Quaternion value3, Quaternion value4, double amount)
        {
            Quaternion result;
            Squad(ref value1, ref value2, ref value3, ref value4, amount, out result);
            return result;
        }

        /// <summary>
        /// Sets up control points for spherical quadrangle interpolation.
        /// </summary>
        /// <param name="value1">First source quaternion.</param>
        /// <param name="value2">Second source quaternion.</param>
        /// <param name="value3">Third source quaternion.</param>
        /// <param name="value4">Fourth source quaternion.</param>
        /// <param name="result1">When the method completes, contains the first control point for spherical quadrangle interpolation.</param>
        /// <param name="result2">When the method completes, contains the second control point for spherical quadrangle interpolation.</param>
        /// <param name="result3">When the method completes, contains the third control point for spherical quadrangle interpolation.</param>
        public static void SquadSetup(ref Quaternion value1, ref Quaternion value2, ref Quaternion value3, ref Quaternion value4, out Quaternion result1, out Quaternion result2, out Quaternion result3)
        {
            Quaternion q0 = (value1 + value2).LengthSquared < (value1 - value2).LengthSquared ? -value1 : value1;
            Quaternion q2 = (value2 + value3).LengthSquared < (value2 - value3).LengthSquared ? -value3 : value3;
            Quaternion q3 = (value3 + value4).LengthSquared < (value3 - value4).LengthSquared ? -value4 : value4;
            Quaternion q1 = value2;

            Quaternion q1Exp, q2Exp;
            Exponential(ref q1, out q1Exp);
            Exponential(ref q2, out q2Exp);

            result1 = q1 * Exponential(-0.25f * (Logarithm(q1Exp * q2) + Logarithm(q1Exp * q0)));
            result2 = q2 * Exponential(-0.25f * (Logarithm(q2Exp * q3) + Logarithm(q2Exp * q1)));
            result3 = q2;
        }

        /// <summary>
        /// Sets up control points for spherical quadrangle interpolation.
        /// </summary>
        /// <param name="value1">First source quaternion.</param>
        /// <param name="value2">Second source quaternion.</param>
        /// <param name="value3">Third source quaternion.</param>
        /// <param name="value4">Fourth source quaternion.</param>
        /// <returns>An array of three quaternions that represent control points for spherical quadrangle interpolation.</returns>
        public static Quaternion[] SquadSetup(Quaternion value1, Quaternion value2, Quaternion value3, Quaternion value4)
        {
            Quaternion[] results = new Quaternion[3];
            SquadSetup(ref value1, ref value2, ref value3, ref value4, out results[0], out results[1], out results[2]);

            return results;
        }

        /// <summary>
        /// Adds two quaternions.
        /// </summary>
        /// <param name="left">The first quaternion to add.</param>
        /// <param name="right">The second quaternion to add.</param>
        /// <returns>The sum of the two quaternions.</returns>
        public static Quaternion operator +(Quaternion left, Quaternion right)
        {
            Quaternion result;
            Add(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Subtracts two quaternions.
        /// </summary>
        /// <param name="left">The first quaternion to subtract.</param>
        /// <param name="right">The second quaternion to subtract.</param>
        /// <returns>The difference of the two quaternions.</returns>
        public static Quaternion operator -(Quaternion left, Quaternion right)
        {
            Quaternion result;
            Subtract(ref left, ref right, out result);
            return result;
        }

        public static Quaternion operator -(Quaternion left, double right)
        {
            Quaternion result = Quaternion.Zero;
            Quaternion rightQ = new Quaternion(0, 0, 0, right);
            result = left - rightQ;
            return result;
        }

        public static Quaternion operator +(Quaternion left, double right)
        {
            Quaternion result = Quaternion.Zero;
            Quaternion rightQ = new Quaternion(0, 0, 0, right);
            result = left + rightQ;
            return result;
        }

        /// <summary>
        /// Reverses the direction of a given quaternion.
        /// </summary>
        /// <param name="value">The quaternion to negate.</param>
        /// <returns>A quaternion facing in the opposite direction.</returns>
        public static Quaternion operator -(Quaternion value)
        {
            Quaternion result;
            Negate(ref value, out result);
            return result;
        }

        /// <summary>
        /// Scales a quaternion by the given value.
        /// </summary>
        /// <param name="value">The quaternion to scale.</param>
        /// <param name="scalar">The amount by which to scale the quaternion.</param>
        /// <returns>The scaled quaternion.</returns>
        public static Quaternion operator *(double scalar, Quaternion value)
        {
            Quaternion result;
            Multiply(ref value, scalar, out result);
            return result;
        }

        /// <summary>
        /// Scales a quaternion by the given value.
        /// </summary>
        /// <param name="value">The quaternion to scale.</param>
        /// <param name="scalar">The amount by which to scale the quaternion.</param>
        /// <returns>The scaled quaternion.</returns>
        public static Quaternion operator *(Quaternion value, double scalar)
        {
            Quaternion result;
            Multiply(ref value, scalar, out result);
            return result;
        }

        /// <summary>
        /// Multiplies a quaternion by another.
        /// </summary>
        /// <param name="left">The first quaternion to multiply.</param>
        /// <param name="right">The second quaternion to multiply.</param>
        /// <returns>The multiplied quaternion.</returns>
        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            Quaternion result = Quaternion.Zero;
            //Multiply(ref left, ref right, out result);

            double s1 = left.w;
            double s2 = right.w;
            Vector3D v1 = new Vector3D(left.x, left.y, left.z);
            Vector3D v2 = new Vector3D(right.x, right.y, right.z);

            result.w = s1 * s2 - v1.Dot(v2);
            Vector3D newV = s1 * v2 + s2 * v1 + v1.Cross(v2);
            result.x = newV.x;
            result.y = newV.y;
            result.z = newV.z;

            return result;
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Quaternion operator /(Quaternion value, double scalar)
        {
            return new Quaternion(value.x / scalar, value.y / scalar, value.z / scalar, value.w / scalar);
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Quaternion left, Quaternion right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Quaternion left, Quaternion right)
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
            return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2} W:{3}", x, y, z, w);
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
            if (format == null)
                return ToString();

            return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2} W:{3}", x.ToString(format, CultureInfo.CurrentCulture),
                y.ToString(format, CultureInfo.CurrentCulture), z.ToString(format, CultureInfo.CurrentCulture), w.ToString(format, CultureInfo.CurrentCulture));
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
            return string.Format(formatProvider, "X:{0} Y:{1} Z:{2} W:{3}", x, y, z, w);
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
            if (format == null)
                return ToString(formatProvider);

            return string.Format(formatProvider, "X:{0} Y:{1} Z:{2} W:{3}", x.ToString(format, formatProvider),
                y.ToString(format, formatProvider), z.ToString(format, formatProvider), w.ToString(format, formatProvider));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return x.GetHashCode() + y.GetHashCode() + z.GetHashCode() + w.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="SlimMath.Quaternion"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="SlimMath.Quaternion"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="SlimMath.Quaternion"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Quaternion other)
        {
            return (this.x == other.x) && (this.y == other.y) && (this.z == other.z) && (this.w == other.w);
        }

        /// <summary>
        /// Determines whether the specified <see cref="SlimMath.Quaternion"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="SlimMath.Quaternion"/> to compare with this instance.</param>
        /// <param name="epsilon">The amount of error allowed.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="SlimMath.Quaternion"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Quaternion other, double epsilon)
        {
            return ((double)Math.Abs(other.x - x) < epsilon &&
                (double)Math.Abs(other.y - y) < epsilon &&
                (double)Math.Abs(other.z - z) < epsilon &&
                (double)Math.Abs(other.w - w) < epsilon);
        }

        public Matrix4D ToMatrix()
        {
            Matrix4D Q = new Matrix4D();
            Q[0, 0] = w; Q[0, 1] = -x; Q[0, 2] = -y; Q[0, 3] = -z;
            Q[1, 0] = x; Q[1, 1] = w; Q[1, 2] = -z; Q[1, 3] = y;
            Q[2, 0] = y; Q[2, 1] = z; Q[2, 2] = w; Q[2, 3] = -x;
            Q[3, 0] = z; Q[3, 1] = -y; Q[3, 2] = x; Q[3, 3] = w;
            return Q;
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

            return Equals((Quaternion)obj);
        }

#if SlimDX1xInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimMath.Quaternion"/> to <see cref="SlimDX.Quaternion"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SlimDX.Quaternion(Quaternion value)
        {
            return new SlimDX.Quaternion(value.X, value.Y, value.Z, value.W);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimDX.Quaternion"/> to <see cref="SlimMath.Quaternion"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Quaternion(SlimDX.Quaternion value)
        {
            return new Quaternion(value.X, value.Y, value.Z, value.W);
        }
#endif

#if WPFInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimMath.Quaternion"/> to <see cref="System.Windows.Media.Media3D.Quaternion"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator System.Windows.Media.Media3D.Quaternion(Quaternion value)
        {
            return new System.Windows.Media.Media3D.Quaternion(value.X, value.Y, value.Z, value.W);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Windows.Media.Media3D.Quaternion"/> to <see cref="SlimMath.Quaternion"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Quaternion(System.Windows.Media.Media3D.Quaternion value)
        {
            return new Quaternion((double)value.X, (double)value.Y, (double)value.Z, (double)value.W);
        }
#endif

#if XnaInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimMath.Quaternion"/> to <see cref="Microsoft.Xna.Framework.Quaternion"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Microsoft.Xna.Framework.Quaternion(Quaternion value)
        {
            return new Microsoft.Xna.Framework.Quaternion(value.X, value.Y, value.Z, value.W);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Microsoft.Xna.Framework.Quaternion"/> to <see cref="SlimMath.Quaternion"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Quaternion(Microsoft.Xna.Framework.Quaternion value)
        {
            return new Quaternion(value.X, value.Y, value.Z, value.W);
        }
#endif
    }
}
