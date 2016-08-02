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
    /// Represents a four dimensional mathematical vector.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    [TypeConverter(typeof(GraphicResearchHuiZhao.Vector4Converter))]
    public struct Vector4D : IEquatable<Vector4D>, IFormattable
    {
        /// <summary>
        /// The size of the <see cref="SlimMath.Vector4d"/> type, in bytes.
        /// </summary>
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(Vector4D));

        /// <summary>
        /// A <see cref="SlimMath.Vector4d"/> with all of its components set to zero.
        /// </summary>
        public static readonly Vector4D Zero = new Vector4D();

        /// <summary>
        /// The X unit <see cref="SlimMath.Vector4d"/> (1, 0, 0, 0).
        /// </summary>
        public static readonly Vector4D UnitX = new Vector4D(1.0f, 0.0f, 0.0f, 0.0f);

        /// <summary>
        /// The Y unit <see cref="SlimMath.Vector4d"/> (0, 1, 0, 0).
        /// </summary>
        public static readonly Vector4D UnitY = new Vector4D(0.0f, 1.0f, 0.0f, 0.0f);

        /// <summary>
        /// The Z unit <see cref="SlimMath.Vector4d"/> (0, 0, 1, 0).
        /// </summary>
        public static readonly Vector4D UnitZ = new Vector4D(0.0f, 0.0f, 1.0f, 0.0f);

        /// <summary>
        /// The W unit <see cref="SlimMath.Vector4d"/> (0, 0, 0, 1).
        /// </summary>
        public static readonly Vector4D UnitW = new Vector4D(0.0f, 0.0f, 0.0f, 1.0f);

        /// <summary>
        /// A <see cref="SlimMath.Vector4d"/> with all of its components set to one.
        /// </summary>
        public static readonly Vector4D One = new Vector4D(1.0f, 1.0f, 1.0f, 1.0f);

        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public double x;

        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public double y;

        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        public double z;

        /// <summary>
        /// The W component of the vector.
        /// </summary>
        public double w;

        public static Vector3D MinValue = new Vector3D(double.MinValue, double.MinValue, double.MinValue);
        public static Vector3D MaxValue = new Vector3D(double.MaxValue, double.MaxValue, double.MaxValue);


        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Vector4d"/> struct.
        /// </summary>
        /// <param name="value">The value that will be assigned to all components.</param>
        public Vector4D(double value)
        {
            x = value;
            y = value;
            z = value;
            w = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Vector4d"/> struct.
        /// </summary>
        /// <param name="x">Initial value for the X component of the vector.</param>
        /// <param name="y">Initial value for the Y component of the vector.</param>
        /// <param name="z">Initial value for the Z component of the vector.</param>
        /// <param name="w">Initial value for the W component of the vector.</param>
        public Vector4D(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector4D(Vector3D v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
            this.w = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Vector4d"/> struct.
        /// </summary>
        /// <param name="value">A vector containing the values with which to initialize the X, Y, and Z components.</param>
        /// <param name="w">Initial value for the W component of the vector.</param>
        public Vector4D(Vector3D value, double w)
        {
            this.x = value.x;
            this.y = value.y;
            this.z = value.z;
            this.w = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Vector4d"/> struct.
        /// </summary>
        /// <param name="value">A vector containing the values with which to initialize the X and Y components.</param>
        /// <param name="z">Initial value for the Z component of the vector.</param>
        /// <param name="w">Initial value for the W component of the vector.</param>
        public Vector4D(Vector2D value, double z, double w)
        {
            x = value.x;
            y = value.y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Vector4d"/> struct.
        /// </summary>
        /// <param name="values">The values to assign to the X, Y, Z, and W components of the vector. This must be an array with four elements.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than four elements.</exception>
        public Vector4D(double[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length != 4)
                throw new ArgumentOutOfRangeException("values", "There must be four and only four input values for Vector4.");

            x = values[0];
            y = values[1];
            z = values[2];
            w = values[3];
        }

        public Vector4D(double[] arr, int index)
        {
            x = arr[index];
            y = arr[index + 1];
            z = arr[index + 2];
            w = arr[index + 3];
        }

        /// <summary>
        /// Gets a value indicting whether this instance is normalized.
        /// </summary>
        public bool IsNormalized
        {
            get { return Math.Abs((x * x) + (y * y) + (z * z) + (w * w) - 1f) < Utilities.ZeroTolerance; }
        }

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        /// <remarks>
        /// <see cref="SlimMath.Vector4d.LengthSquared"/> may be preferred when only the relative length is needed
        /// and speed is of the essence.
        /// </remarks>
        public double Length()
        {
           return (double)Math.Sqrt((x * x) + (y * y) + (z * z) + (w * w));
        }

        /// <summary>
        /// Calculates the squared length of the vector.
        /// </summary>
        /// <remarks>
        /// This property may be preferred to <see cref="SlimMath.Vector4d.Length"/> when only a relative length is needed
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

                throw new ArgumentOutOfRangeException("index", "Indices for Vector4 run from 0 to 3, inclusive.");
            }

            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    case 3: w = value; break;
                    default: throw new ArgumentOutOfRangeException("index", "Indices for Vector4 run from 0 to 3, inclusive.");
                }
            }
        }

        /// <summary>
        /// Converts the vector into a unit vector.
        /// </summary>
        public Vector4D Normalize()
        {
            Vector4D v = new Vector4D(this.x, this.y, this.z, this.w);
            double length = Length();
            if (length > Utilities.ZeroTolerance)
            {
                double inverse = 1.0f / length;
                v.x *= inverse;
                v.y *= inverse;
                v.z *= inverse;
                v.w *= inverse;
            }
            return v;
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        public void Negate()
        {
            x = -x;
            y = -y;
            z = -z;
            w = -w;
        }

        /// <summary>
        /// Takes the absolute value of each component.
        /// </summary>
        public void Abs()
        {
            this.x = Math.Abs(x);
            this.y = Math.Abs(y);
            this.z = Math.Abs(z);
            this.w = Math.Abs(w);
        }

        /// <summary>
        /// Creates an array containing the elements of the vector.
        /// </summary>
        /// <returns>A four-element array containing the components of the vector.</returns>
        public double[] ToArray()
        {
            return new double[] { x, y, z, w };
        }

        #region Transcendentals
        /// <summary>
        /// Takes the square root of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the square root of.</param>
        /// <param name="result">When the method completes, contains a vector that is the square root of the input vector.</param>
        public static void Sqrt(ref Vector4D value, out Vector4D result)
        {
            result.x = (double)Math.Sqrt(value.x);
            result.y = (double)Math.Sqrt(value.y);
            result.z = (double)Math.Sqrt(value.z);
            result.w = (double)Math.Sqrt(value.w);
        }

        /// <summary>
        /// Takes the square root of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the square root of.</param>
        /// <returns>A vector that is the square root of the input vector.</returns>
        public static Vector4D Sqrt(Vector4D value)
        {
            Vector4D temp;
            Sqrt(ref value, out temp);
            return temp;
        }

        /// <summary>
        /// Takes the reciprocal of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the reciprocal of.</param>
        /// <param name="result">When the method completes, contains a vector that is the reciprocal of the input vector.</param>
        public static void Reciprocal(ref Vector4D value, out Vector4D result)
        {
            result.x = 1.0f / value.x;
            result.y = 1.0f / value.y;
            result.z = 1.0f / value.z;
            result.w = 1.0f / value.w;
        }

        /// <summary>
        /// Takes the reciprocal of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the reciprocal of.</param>
        /// <returns>A vector that is the reciprocal of the input vector.</returns>
        public static Vector4D Reciprocal(Vector4D value)
        {
            Vector4D temp;
            Reciprocal(ref value, out temp);
            return temp;
        }

        /// <summary>
        /// Takes the square root of each component in the vector and than takes the reciprocal of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the square root and recpirocal of.</param>
        /// <param name="result">When the method completes, contains a vector that is the square root and reciprocal of the input vector.</param>
        public static void ReciprocalSqrt(ref Vector4D value, out Vector4D result)
        {
            result.x = 1.0f / (double)Math.Sqrt(value.x);
            result.y = 1.0f / (double)Math.Sqrt(value.y);
            result.z = 1.0f / (double)Math.Sqrt(value.z);
            result.w = 1.0f / (double)Math.Sqrt(value.w);
        }

        /// <summary>
        /// Takes the square root of each component in the vector and than takes the reciprocal of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the square root and recpirocal of.</param>
        /// <returns>A vector that is the square root and reciprocal of the input vector.</returns>
        public static Vector4D ReciprocalSqrt(Vector4D value)
        {
            Vector4D temp;
            ReciprocalSqrt(ref value, out temp);
            return temp;
        }

        /// <summary>
        /// Takes e raised to the component in the vector.
        /// </summary>
        /// <param name="value">The value to take e raised to each component of.</param>
        /// <param name="result">When the method completes, contains a vector that has e raised to each of the components in the input vector.</param>
        public static void Exp(ref Vector4D value, out Vector4D result)
        {
            result.x = (double)Math.Exp(value.x);
            result.y = (double)Math.Exp(value.y);
            result.z = (double)Math.Exp(value.z);
            result.w = (double)Math.Exp(value.w);
        }

        /// <summary>
        /// Takes e raised to the component in the vector.
        /// </summary>
        /// <param name="value">The value to take e raised to each component of.</param>
        /// <returns>A vector that has e raised to each of the components in the input vector.</returns>
        public static Vector4D Exp(Vector4D value)
        {
            Vector4D temp;
            Exp(ref value, out temp);
            return temp;
        }

        /// <summary>
        /// Takes the sine and than the cosine of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the sine and cosine of.</param>
        /// <param name="sinResult">When the method completes, contains the sine of each component in the input vector.</param>
        /// <param name="cosResult">When the method completes, contains the cpsome pf each component in the input vector.</param>
        public static void SinCos(ref Vector4D value, out Vector4D sinResult, out Vector4D cosResult)
        {
            sinResult.x = (double)Math.Sin(value.x);
            sinResult.y = (double)Math.Sin(value.y);
            sinResult.z = (double)Math.Sin(value.z);
            sinResult.w = (double)Math.Sin(value.w);

            cosResult.x = (double)Math.Cos(value.x);
            cosResult.y = (double)Math.Cos(value.y);
            cosResult.z = (double)Math.Cos(value.z);
            cosResult.w = (double)Math.Cos(value.w);
        }

        /// <summary>
        /// Takes the sine of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the sine of.</param>
        /// <param name="result">When the method completes, a vector that contains the sine of each component in the input vector.</param>
        public static void Sin(ref Vector4D value, out Vector4D result)
        {
            result.x = (double)Math.Sin(value.x);
            result.y = (double)Math.Sin(value.y);
            result.z = (double)Math.Sin(value.z);
            result.w = (double)Math.Sin(value.w);
        }

        /// <summary>
        /// Takes the sine of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the sine of.</param>
        /// <returns>A vector that contains the sine of each component in the input vector.</returns>
        public static Vector4D Sin(Vector4D value)
        {
            Vector4D temp;
            Sin(ref value, out temp);
            return temp;
        }

        /// <summary>
        /// Takes the cosine of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the cosine of.</param>
        /// <param name="result">When the method completes, contains a vector that contains the cosine of each component in the input vector.</param>
        public static void Cos(ref Vector4D value, out Vector4D result)
        {
            result.x = (double)Math.Cos(value.x);
            result.y = (double)Math.Cos(value.y);
            result.z = (double)Math.Cos(value.z);
            result.w = (double)Math.Cos(value.w);
        }

        /// <summary>
        /// Takes the cosine of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the cosine of.</param>
        /// <returns>A vector that contains the cosine of each component in the input vector.</returns>
        public static Vector4D Cos(Vector4D value)
        {
            Vector4D temp;
            Cos(ref value, out temp);
            return temp;
        }

        /// <summary>
        /// Takes the tangent of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the tangent of.</param>
        /// <param name="result">When the method completes, contains a vector that contains the tangent of each component in the input vector.</param>
        public static void Tan(ref Vector4D value, out Vector4D result)
        {
            result.x = (double)Math.Tan(value.x);
            result.y = (double)Math.Tan(value.y);
            result.z = (double)Math.Tan(value.z);
            result.w = (double)Math.Tan(value.w);
        }

        /// <summary>
        /// Takes the tangent of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the tangent of.</param>
        /// <returns>A vector that contains the tangent of each component in the input vector.</returns>
        public static Vector4D Tan(Vector4D value)
        {
            Vector4D temp;
            Tan(ref value, out temp);
            return temp;
        }
        #endregion

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <param name="result">When the method completes, contains the sum of the two vectors.</param>
        public static void Add(ref Vector4D left, ref Vector4D right, out Vector4D result)
        {
            result = new Vector4D(left.x + right.x, left.y + right.y, left.z + right.z, left.w + right.w);
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The sum of the two vectors.</returns>
        public static Vector4D Add(Vector4D left, Vector4D right)
        {
            return new Vector4D(left.x + right.x, left.y + right.y, left.z + right.z, left.w + right.w);
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <param name="result">When the method completes, contains the difference of the two vectors.</param>
        public static void Subtract(ref Vector4D left, ref Vector4D right, out Vector4D result)
        {
            result = new Vector4D(left.x - right.x, left.y - right.y, left.z - right.z, left.w - right.w);
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <returns>The difference of the two vectors.</returns>
        public static Vector4D Subtract(Vector4D left, Vector4D right)
        {
            return new Vector4D(left.x - right.x, left.y - right.y, left.z - right.z, left.w - right.w);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <param name="result">When the method completes, contains the scaled vector.</param>
        public static void Multiply(ref Vector4D value, double scalar, out Vector4D result)
        {
            result = new Vector4D(value.x * scalar, value.y * scalar, value.z * scalar, value.w * scalar);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector4D Multiply(Vector4D value, double scalar)
        {
            return new Vector4D(value.x * scalar, value.y * scalar, value.z * scalar, value.w * scalar);
        }

        /// <summary>
        /// Modulates a vector with another by performing component-wise multiplication.
        /// </summary>
        /// <param name="left">The first vector to modulate.</param>
        /// <param name="right">The second vector to modulate.</param>
        /// <param name="result">When the method completes, contains the modulated vector.</param>
        public static void Modulate(ref Vector4D left, ref Vector4D right, out Vector4D result)
        {
            result = new Vector4D(left.x * right.x, left.y * right.y, left.z * right.z, left.w * right.w);
        }

        /// <summary>
        /// Modulates a vector with another by performing component-wise multiplication.
        /// </summary>
        /// <param name="left">The first vector to modulate.</param>
        /// <param name="right">The second vector to modulate.</param>
        /// <returns>The modulated vector.</returns>
        public static Vector4D Modulate(Vector4D left, Vector4D right)
        {
            return new Vector4D(left.x * right.x, left.y * right.y, left.z * right.z, left.w * right.w);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <param name="result">When the method completes, contains the scaled vector.</param>
        public static void Divide(ref Vector4D value, double scalar, out Vector4D result)
        {
            result = new Vector4D(value.x / scalar, value.y / scalar, value.z / scalar, value.w / scalar);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector4D Divide(Vector4D value, double scalar)
        {
            return new Vector4D(value.x / scalar, value.y / scalar, value.z / scalar, value.w / scalar);
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <param name="result">When the method completes, contains a vector facing in the opposite direction.</param>
        public static void Negate(ref Vector4D value, out Vector4D result)
        {
            result = new Vector4D(-value.x, -value.y, -value.z, -value.w);
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>A vector facing in the opposite direction.</returns>
        public static Vector4D Negate(Vector4D value)
        {
            return new Vector4D(-value.x, -value.y, -value.z, -value.w);
        }

        /// <summary>
        /// Takes the absolute value of each component.
        /// </summary>
        /// <param name="value">The vector to take the absolute value of.</param>
        /// <param name="result">When the method completes, contains a vector that has all positive components.</param>
        public static void Abs(ref Vector4D value, out Vector4D result)
        {
            result = new Vector4D(Math.Abs(value.x), Math.Abs(value.y), Math.Abs(value.z), Math.Abs(value.w));
        }

        /// <summary>
        /// Takes the absolute value of each component.
        /// </summary>
        /// <param name="value">The vector to take the absolute value of.</param>
        /// <returns>A vector that has all positive components.</returns>
        public static Vector4D Abs(Vector4D value)
        {
            return new Vector4D(Math.Abs(value.x), Math.Abs(value.y), Math.Abs(value.z), Math.Abs(value.w));
        }

        /// <summary>
        /// Returns a <see cref="SlimMath.Vector4d"/> containing the 4D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 4D triangle.
        /// </summary>
        /// <param name="value1">A <see cref="SlimMath.Vector4d"/> containing the 4D Cartesian coordinates of vertex 1 of the triangle.</param>
        /// <param name="value2">A <see cref="SlimMath.Vector4d"/> containing the 4D Cartesian coordinates of vertex 2 of the triangle.</param>
        /// <param name="value3">A <see cref="SlimMath.Vector4d"/> containing the 4D Cartesian coordinates of vertex 3 of the triangle.</param>
        /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="value2"/>).</param>
        /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="value3"/>).</param>
        /// <param name="result">When the method completes, contains the 4D Cartesian coordinates of the specified point.</param>
        public static void Barycentric(ref Vector4D value1, ref Vector4D value2, ref Vector4D value3, double amount1, double amount2, out Vector4D result)
        {
            result = new Vector4D(
                (value1.x + (amount1 * (value2.x - value1.x))) + (amount2 * (value3.x - value1.x)),
                (value1.y + (amount1 * (value2.y - value1.y))) + (amount2 * (value3.y - value1.y)),
                (value1.z + (amount1 * (value2.z - value1.z))) + (amount2 * (value3.z - value1.z)),
                (value1.w + (amount1 * (value2.w - value1.w))) + (amount2 * (value3.w - value1.w)));
        }

        /// <summary>
        /// Returns a <see cref="SlimMath.Vector4d"/> containing the 4D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 4D triangle.
        /// </summary>
        /// <param name="value1">A <see cref="SlimMath.Vector4d"/> containing the 4D Cartesian coordinates of vertex 1 of the triangle.</param>
        /// <param name="value2">A <see cref="SlimMath.Vector4d"/> containing the 4D Cartesian coordinates of vertex 2 of the triangle.</param>
        /// <param name="value3">A <see cref="SlimMath.Vector4d"/> containing the 4D Cartesian coordinates of vertex 3 of the triangle.</param>
        /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="value2"/>).</param>
        /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="value3"/>).</param>
        /// <returns>A new <see cref="SlimMath.Vector4d"/> containing the 4D Cartesian coordinates of the specified point.</returns>
        public static Vector4D Barycentric(Vector4D value1, Vector4D value2, Vector4D value3, double amount1, double amount2)
        {
            Vector4D result;
            Barycentric(ref value1, ref value2, ref value3, amount1, amount2, out result);
            return result;
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="result">When the method completes, contains the clamped value.</param>
        public static void Clamp(ref Vector4D value, ref Vector4D min, ref Vector4D max, out Vector4D result)
        {
            double x = value.x;
            x = (x > max.x) ? max.x : x;
            x = (x < min.x) ? min.x : x;

            double y = value.y;
            y = (y > max.y) ? max.y : y;
            y = (y < min.y) ? min.y : y;

            double z = value.z;
            z = (z > max.z) ? max.z : z;
            z = (z < min.z) ? min.z : z;

            double w = value.w;
            w = (w > max.w) ? max.w : w;
            w = (w < min.w) ? min.w : w;

            result = new Vector4D(x, y, z, w);
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static Vector4D Clamp(Vector4D value, Vector4D min, Vector4D max)
        {
            Vector4D result;
            Clamp(ref value, ref min, ref max, out result);
            return result;
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">When the method completes, contains the distance between the two vectors.</param>
        /// <remarks>
        /// <see cref="SlimMath.Vector4d.DistanceSquared(ref Vector4d, ref Vector4d, out double)"/> may be preferred when only the relative distance is needed
        /// and speed is of the essence.
        /// </remarks>
        public static void Distance(ref Vector4D value1, ref Vector4D value2, out double result)
        {
            double x = value1.x - value2.x;
            double y = value1.y - value2.y;
            double z = value1.z - value2.z;
            double w = value1.w - value2.w;

            result = (double)Math.Sqrt((x * x) + (y * y) + (z * z) + (w * w));
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        /// <remarks>
        /// <see cref="SlimMath.Vector4d.DistanceSquared(Vector4d, Vector4d)"/> may be preferred when only the relative distance is needed
        /// and speed is of the essence.
        /// </remarks>
        public static double Distance(Vector4D value1, Vector4D value2)
        {
            double x = value1.x - value2.x;
            double y = value1.y - value2.y;
            double z = value1.z - value2.z;
            double w = value1.w - value2.w;

            return (double)Math.Sqrt((x * x) + (y * y) + (z * z) + (w * w));
        }

        /// <summary>
        /// Calculates the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">When the method completes, contains the squared distance between the two vectors.</param>
        /// <remarks>Distance squared is the value before taking the square root. 
        /// Distance squared can often be used in place of distance if relative comparisons are being made. 
        /// For example, consider three points A, B, and C. To determine whether B or C is further from A, 
        /// compare the distance between A and B to the distance between A and C. Calculating the two distances 
        /// involves two square roots, which are computationally expensive. However, using distance squared 
        /// provides the same information and avoids calculating two square roots.
        /// </remarks>
        public static void DistanceSquared(ref Vector4D value1, ref Vector4D value2, out double result)
        {
            double x = value1.x - value2.x;
            double y = value1.y - value2.y;
            double z = value1.z - value2.z;
            double w = value1.w - value2.w;

            result = (x * x) + (y * y) + (z * z) + (w * w);
        }

        /// <summary>
        /// Calculates the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The squared distance between the two vectors.</returns>
        /// <remarks>Distance squared is the value before taking the square root. 
        /// Distance squared can often be used in place of distance if relative comparisons are being made. 
        /// For example, consider three points A, B, and C. To determine whether B or C is further from A, 
        /// compare the distance between A and B to the distance between A and C. Calculating the two distances 
        /// involves two square roots, which are computationally expensive. However, using distance squared 
        /// provides the same information and avoids calculating two square roots.
        /// </remarks>
        public static double DistanceSquared(Vector4D value1, Vector4D value2)
        {
            double x = value1.x - value2.x;
            double y = value1.y - value2.y;
            double z = value1.z - value2.z;
            double w = value1.w - value2.w;

            return (x * x) + (y * y) + (z * z) + (w * w);
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="left">First source vector</param>
        /// <param name="right">Second source vector.</param>
        /// <param name="result">When the method completes, contains the dot product of the two vectors.</param>
        public static void Dot(ref Vector4D left, ref Vector4D right, out double result)
        {
            result = (left.x * right.x) + (left.y * right.y) + (left.z * right.z) + (left.w * right.w);
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="left">First source vector.</param>
        /// <param name="right">Second source vector.</param>
        /// <returns>The dot product of the two vectors.</returns>
        public static double Dot(Vector4D left, Vector4D right)
        {
            return (left.x * right.x) + (left.y * right.y) + (left.z * right.z) + (left.w * right.w);
        }

        public double Dot(Vector4D right) {
            return Dot(this, right); 
        }

        /// <summary>
        /// Converts the vector into a unit vector.
        /// </summary>
        /// <param name="value">The vector to normalize.</param>
        /// <param name="result">When the method completes, contains the normalized vector.</param>
        public static void Normalize(ref Vector4D value, out Vector4D result)
        {
            Vector4D temp = value;
            result = temp;
            result.Normalize();
        }

        /// <summary>
        /// Converts the vector into a unit vector.
        /// </summary>
        /// <param name="value">The vector to normalize.</param>
        /// <returns>The normalized vector.</returns>
        public static Vector4D Normalize(Vector4D value)
        {
            value.Normalize();
            return value;
        }

        /// <summary>
        /// Performs a linear interpolation between two vectors.
        /// </summary>
        /// <param name="start">Start vector.</param>
        /// <param name="end">End vector.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <param name="result">When the method completes, contains the linear interpolation of the two vectors.</param>
        /// <remarks>
        /// This method performs the linear interpolation based on the following formula.
        /// <code>start + (end - start) * amount</code>
        /// Passing <paramref name="amount"/> a value of 0 will cause <paramref name="start"/> to be returned; a value of 1 will cause <paramref name="end"/> to be returned. 
        /// </remarks>
        public static void Lerp(ref Vector4D start, ref Vector4D end, double amount, out Vector4D result)
        {
            result.x = start.x + ((end.x - start.x) * amount);
            result.y = start.y + ((end.y - start.y) * amount);
            result.z = start.z + ((end.z - start.z) * amount);
            result.w = start.w + ((end.w - start.w) * amount);
        }

        /// <summary>
        /// Performs a linear interpolation between two vectors.
        /// </summary>
        /// <param name="start">Start vector.</param>
        /// <param name="end">End vector.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <returns>The linear interpolation of the two vectors.</returns>
        /// <remarks>
        /// This method performs the linear interpolation based on the following formula.
        /// <code>start + (end - start) * amount</code>
        /// Passing <paramref name="amount"/> a value of 0 will cause <paramref name="start"/> to be returned; a value of 1 will cause <paramref name="end"/> to be returned. 
        /// </remarks>
        public static Vector4D Lerp(Vector4D start, Vector4D end, double amount)
        {
            Vector4D result;
            Lerp(ref start, ref end, amount, out result);
            return result;
        }

        /// <summary>
        /// Performs a cubic interpolation between two vectors.
        /// </summary>
        /// <param name="start">Start vector.</param>
        /// <param name="end">End vector.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <param name="result">When the method completes, contains the cubic interpolation of the two vectors.</param>
        public static void SmoothStep(ref Vector4D start, ref Vector4D end, double amount, out Vector4D result)
        {
            amount = (amount > 1.0f) ? 1.0f : ((amount < 0.0f) ? 0.0f : amount);
            amount = (amount * amount) * (3.0f - (2.0f * amount));

            result.x = start.x + ((end.x - start.x) * amount);
            result.y = start.y + ((end.y - start.y) * amount);
            result.z = start.z + ((end.z - start.z) * amount);
            result.w = start.w + ((end.w - start.w) * amount);
        }

        /// <summary>
        /// Performs a cubic interpolation between two vectors.
        /// </summary>
        /// <param name="start">Start vector.</param>
        /// <param name="end">End vector.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <returns>The cubic interpolation of the two vectors.</returns>
        public static Vector4D SmoothStep(Vector4D start, Vector4D end, double amount)
        {
            Vector4D result;
            SmoothStep(ref start, ref end, amount, out result);
            return result;
        }

        /// <summary>
        /// Performs a Hermite spline interpolation.
        /// </summary>
        /// <param name="value1">First source position vector.</param>
        /// <param name="tangent1">First source tangent vector.</param>
        /// <param name="value2">Second source position vector.</param>
        /// <param name="tangent2">Second source tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">When the method completes, contains the result of the Hermite spline interpolation.</param>
        public static void Hermite(ref Vector4D value1, ref Vector4D tangent1, ref Vector4D value2, ref Vector4D tangent2, double amount, out Vector4D result)
        {
            double squared = amount * amount;
            double cubed = amount * squared;
            double part1 = ((2.0f * cubed) - (3.0f * squared)) + 1.0f;
            double part2 = (-2.0f * cubed) + (3.0f * squared);
            double part3 = (cubed - (2.0f * squared)) + amount;
            double part4 = cubed - squared;

            result = new Vector4D(
                (((value1.x * part1) + (value2.x * part2)) + (tangent1.x * part3)) + (tangent2.x * part4),
                (((value1.y * part1) + (value2.y * part2)) + (tangent1.y * part3)) + (tangent2.y * part4),
                (((value1.z * part1) + (value2.z * part2)) + (tangent1.z * part3)) + (tangent2.z * part4),
                (((value1.w * part1) + (value2.w * part2)) + (tangent1.w * part3)) + (tangent2.w * part4));
        }

        /// <summary>
        /// Performs a Hermite spline interpolation.
        /// </summary>
        /// <param name="value1">First source position vector.</param>
        /// <param name="tangent1">First source tangent vector.</param>
        /// <param name="value2">Second source position vector.</param>
        /// <param name="tangent2">Second source tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The result of the Hermite spline interpolation.</returns>
        public static Vector4D Hermite(Vector4D value1, Vector4D tangent1, Vector4D value2, Vector4D tangent2, double amount)
        {
            Vector4D result;
            Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
            return result;
        }

        /// <summary>
        /// Performs a Catmull-Rom interpolation using the specified positions.
        /// </summary>
        /// <param name="value1">The first position in the interpolation.</param>
        /// <param name="value2">The second position in the interpolation.</param>
        /// <param name="value3">The third position in the interpolation.</param>
        /// <param name="value4">The fourth position in the interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">When the method completes, contains the result of the Catmull-Rom interpolation.</param>
        public static void CatmullRom(ref Vector4D value1, ref Vector4D value2, ref Vector4D value3, ref Vector4D value4, double amount, out Vector4D result)
        {
            double squared = amount * amount;
            double cubed = amount * squared;

            result.x = 0.5f * ((((2.0f * value2.x) + ((-value1.x + value3.x) * amount)) +
                (((((2.0f * value1.x) - (5.0f * value2.x)) + (4.0f * value3.x)) - value4.x) * squared)) +
                ((((-value1.x + (3.0f * value2.x)) - (3.0f * value3.x)) + value4.x) * cubed));

            result.y = 0.5f * ((((2.0f * value2.y) + ((-value1.y + value3.y) * amount)) +
                (((((2.0f * value1.y) - (5.0f * value2.y)) + (4.0f * value3.y)) - value4.y) * squared)) +
                ((((-value1.y + (3.0f * value2.y)) - (3.0f * value3.y)) + value4.y) * cubed));

            result.z = 0.5f * ((((2.0f * value2.z) + ((-value1.z + value3.z) * amount)) +
                (((((2.0f * value1.z) - (5.0f * value2.z)) + (4.0f * value3.z)) - value4.z) * squared)) +
                ((((-value1.z + (3.0f * value2.z)) - (3.0f * value3.z)) + value4.z) * cubed));

            result.w = 0.5f * ((((2.0f * value2.w) + ((-value1.w + value3.w) * amount)) +
                (((((2.0f * value1.w) - (5.0f * value2.w)) + (4.0f * value3.w)) - value4.w) * squared)) +
                ((((-value1.w + (3.0f * value2.w)) - (3.0f * value3.w)) + value4.w) * cubed));
        }

        /// <summary>
        /// Performs a Catmull-Rom interpolation using the specified positions.
        /// </summary>
        /// <param name="value1">The first position in the interpolation.</param>
        /// <param name="value2">The second position in the interpolation.</param>
        /// <param name="value3">The third position in the interpolation.</param>
        /// <param name="value4">The fourth position in the interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>A vector that is the result of the Catmull-Rom interpolation.</returns>
        public static Vector4D CatmullRom(Vector4D value1, Vector4D value2, Vector4D value3, Vector4D value4, double amount)
        {
            Vector4D result;
            CatmullRom(ref value1, ref value2, ref value3, ref value4, amount, out result);
            return result;
        }

        /// <summary>
        /// Returns a vector containing the largest components of the specified vectors.
        /// </summary>
        /// <param name="value1">The first source vector.</param>
        /// <param name="value2">The second source vector.</param>
        /// <param name="result">When the method completes, contains an new vector composed of the largest components of the source vectors.</param>
        public static void Max(ref Vector4D value1, ref Vector4D value2, out Vector4D result)
        {
            result.x = (value1.x > value2.x) ? value1.x : value2.x;
            result.y = (value1.y > value2.y) ? value1.y : value2.y;
            result.z = (value1.z > value2.z) ? value1.z : value2.z;
            result.w = (value1.w > value2.w) ? value1.w : value2.w;
        }

        /// <summary>
        /// Returns a vector containing the largest components of the specified vectors.
        /// </summary>
        /// <param name="value1">The first source vector.</param>
        /// <param name="value2">The second source vector.</param>
        /// <returns>A vector containing the largest components of the source vectors.</returns>
        public static Vector4D Max(Vector4D value1, Vector4D value2)
        {
            Vector4D result;
            Max(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Returns a vector containing the smallest components of the specified vectors.
        /// </summary>
        /// <param name="value1">The first source vector.</param>
        /// <param name="value2">The second source vector.</param>
        /// <param name="result">When the method completes, contains an new vector composed of the smallest components of the source vectors.</param>
        public static void Min(ref Vector4D value1, ref Vector4D value2, out Vector4D result)
        {
            result.x = (value1.x < value2.x) ? value1.x : value2.x;
            result.y = (value1.y < value2.y) ? value1.y : value2.y;
            result.z = (value1.z < value2.z) ? value1.z : value2.z;
            result.w = (value1.w < value2.w) ? value1.w : value2.w;
        }

        /// <summary>
        /// Returns a vector containing the smallest components of the specified vectors.
        /// </summary>
        /// <param name="value1">The first source vector.</param>
        /// <param name="value2">The second source vector.</param>
        /// <returns>A vector containing the smallest components of the source vectors.</returns>
        public static Vector4D Min(Vector4D value1, Vector4D value2)
        {
            Vector4D result;
            Min(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Orthogonalizes a list of vectors.
        /// </summary>
        /// <param name="destination">The list of orthogonalized vectors.</param>
        /// <param name="source">The list of vectors to orthogonalize.</param>
        /// <remarks>
        /// <para>Orthogonalization is the process of making all vectors orthogonal to each other. This
        /// means that any given vector in the list will be orthogonal to any other given vector in the
        /// list.</para>
        /// <para>Because this method uses the modified Gram-Schmidt process, the resulting vectors
        /// tend to be numerically unstable. The numeric stability decreases according to the vectors
        /// position in the list so that the first vector is the most stable and the last vector is the
        /// least stable.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="destination"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="destination"/> is shorter in length than <paramref name="source"/>.</exception>
        public static void Orthogonalize(Vector4D[] destination, params Vector4D[] source)
        {
            //Uses the modified Gram-Schmidt process.
            //q1 = m1
            //q2 = m2 - ((q1 ⋅ m2) / (q1 ⋅ q1)) * q1
            //q3 = m3 - ((q1 ⋅ m3) / (q1 ⋅ q1)) * q1 - ((q2 ⋅ m3) / (q2 ⋅ q2)) * q2
            //q4 = m4 - ((q1 ⋅ m4) / (q1 ⋅ q1)) * q1 - ((q2 ⋅ m4) / (q2 ⋅ q2)) * q2 - ((q3 ⋅ m4) / (q3 ⋅ q3)) * q3
            //q5 = ...

            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");
            if (destination.Length < source.Length)
                throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");

            for (int i = 0; i < source.Length; ++i)
            {
                Vector4D newvector = source[i];

                for (int r = 0; r < i; ++r)
                {
                    newvector -= (Vector4D.Dot(destination[r], newvector) / Vector4D.Dot(destination[r], destination[r])) * destination[r];
                }

                destination[i] = newvector;
            }
        }

        /// <summary>
        /// Orthonormalizes a list of vectors.
        /// </summary>
        /// <param name="destination">The list of orthonormalized vectors.</param>
        /// <param name="source">The list of vectors to orthonormalize.</param>
        /// <remarks>
        /// <para>Orthonormalization is the process of making all vectors orthogonal to each
        /// other and making all vectors of unit length. This means that any given vector will
        /// be orthogonal to any other given vector in the list.</para>
        /// <para>Because this method uses the modified Gram-Schmidt process, the resulting vectors
        /// tend to be numerically unstable. The numeric stability decreases according to the vectors
        /// position in the list so that the first vector is the most stable and the last vector is the
        /// least stable.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="destination"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="destination"/> is shorter in length than <paramref name="source"/>.</exception>
        public static void Orthonormalize(Vector4D[] destination, params Vector4D[] source)
        {
            //Uses the modified Gram-Schmidt process.
            //Because we are making unit vectors, we can optimize the math for orthogonalization
            //and simplify the projection operation to remove the division.
            //q1 = m1 / |m1|
            //q2 = (m2 - (q1 ⋅ m2) * q1) / |m2 - (q1 ⋅ m2) * q1|
            //q3 = (m3 - (q1 ⋅ m3) * q1 - (q2 ⋅ m3) * q2) / |m3 - (q1 ⋅ m3) * q1 - (q2 ⋅ m3) * q2|
            //q4 = (m4 - (q1 ⋅ m4) * q1 - (q2 ⋅ m4) * q2 - (q3 ⋅ m4) * q3) / |m4 - (q1 ⋅ m4) * q1 - (q2 ⋅ m4) * q2 - (q3 ⋅ m4) * q3|
            //q5 = ...

            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");
            if (destination.Length < source.Length)
                throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");

            for (int i = 0; i < source.Length; ++i)
            {
                Vector4D newvector = source[i];

                for (int r = 0; r < i; ++r)
                {
                    newvector -= Vector4D.Dot(destination[r], newvector) * destination[r];
                }

                newvector.Normalize();
                destination[i] = newvector;
            }
        }

        /// <summary>
        /// Transforms a 4D vector by the given <see cref="SlimMath.Quaternion"/> rotation.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="rotation">The <see cref="SlimMath.Quaternion"/> rotation to apply.</param>
        /// <param name="result">When the method completes, contains the transformed <see cref="SlimMath.Vector4d"/>.</param>
        public static void Transform(ref Vector4D vector, ref Quaternion rotation, out Vector4D result)
        {
            double x = rotation.x + rotation.x;
            double y = rotation.y + rotation.y;
            double z = rotation.z + rotation.z;
            double wx = rotation.w * x;
            double wy = rotation.w * y;
            double wz = rotation.w * z;
            double xx = rotation.x * x;
            double xy = rotation.x * y;
            double xz = rotation.x * z;
            double yy = rotation.y * y;
            double yz = rotation.y * z;
            double zz = rotation.z * z;

            double num1 = ((1.0f - yy) - zz);
            double num2 = (xy - wz);
            double num3 = (xz + wy);
            double num4 = (xy + wz);
            double num5 = ((1.0f - xx) - zz);
            double num6 = (yz - wx);
            double num7 = (xz - wy);
            double num8 = (yz + wx);
            double num9 = ((1.0f - xx) - yy);

            result = new Vector4D(
                ((vector.x * num1) + (vector.y * num2)) + (vector.z * num3),
                ((vector.x * num4) + (vector.y * num5)) + (vector.z * num6),
                ((vector.x * num7) + (vector.y * num8)) + (vector.z * num9),
                vector.w);
        }

        /// <summary>
        /// Transforms a 4D vector by the given <see cref="SlimMath.Quaternion"/> rotation.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="rotation">The <see cref="SlimMath.Quaternion"/> rotation to apply.</param>
        /// <returns>The transformed <see cref="SlimMath.Vector4d"/>.</returns>
        public static Vector4D Transform(Vector4D vector, Quaternion rotation)
        {
            Vector4D result;
            Transform(ref vector, ref rotation, out result);
            return result;
        }

        /// <summary>
        /// Transforms an array of vectors by the given <see cref="SlimMath.Quaternion"/> rotation.
        /// </summary>
        /// <param name="source">The array of vectors to transform.</param>
        /// <param name="rotation">The <see cref="SlimMath.Quaternion"/> rotation to apply.</param>
        /// <param name="destination">The array for which the transformed vectors are stored.
        /// This array may be the same array as <paramref name="source"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="destination"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="destination"/> is shorter in length than <paramref name="source"/>.</exception>
        public static void Transform(Vector4D[] source, ref Quaternion rotation, Vector4D[] destination)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");
            if (destination.Length < source.Length)
                throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");

            double x = rotation.x + rotation.x;
            double y = rotation.y + rotation.y;
            double z = rotation.z + rotation.z;
            double wx = rotation.w * x;
            double wy = rotation.w * y;
            double wz = rotation.w * z;
            double xx = rotation.x * x;
            double xy = rotation.x * y;
            double xz = rotation.x * z;
            double yy = rotation.y * y;
            double yz = rotation.y * z;
            double zz = rotation.z * z;

            double num1 = ((1.0f - yy) - zz);
            double num2 = (xy - wz);
            double num3 = (xz + wy);
            double num4 = (xy + wz);
            double num5 = ((1.0f - xx) - zz);
            double num6 = (yz - wx);
            double num7 = (xz - wy);
            double num8 = (yz + wx);
            double num9 = ((1.0f - xx) - yy);

            for (int i = 0; i < source.Length; ++i)
            {
                destination[i] = new Vector4D(
                    ((source[i].x * num1) + (source[i].y * num2)) + (source[i].z * num3),
                    ((source[i].x * num4) + (source[i].y * num5)) + (source[i].z * num6),
                    ((source[i].x * num7) + (source[i].y * num8)) + (source[i].z * num9),
                    source[i].w);
            }
        }

        /// <summary>
        /// Transforms a 4D vector by the given <see cref="SlimMath.Matrix4d"/>.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="transform">The transformation <see cref="SlimMath.Matrix4d"/>.</param>
        /// <param name="result">When the method completes, contains the transformed <see cref="SlimMath.Vector4d"/>.</param>
        public static void Transform(ref Vector4D vector, ref Matrix4D transform, out Vector4D result)
        {
            result = new Vector4D(
                (vector.x * transform.M11) + (vector.y * transform.M21) + (vector.z * transform.M31) + (vector.w * transform.M41),
                (vector.x * transform.M12) + (vector.y * transform.M22) + (vector.z * transform.M32) + (vector.w * transform.M42),
                (vector.x * transform.M13) + (vector.y * transform.M23) + (vector.z * transform.M33) + (vector.w * transform.M43),
                (vector.x * transform.M14) + (vector.y * transform.M24) + (vector.z * transform.M34) + (vector.w * transform.M44));
        }

        /// <summary>
        /// Transforms a 4D vector by the given <see cref="SlimMath.Matrix4d"/>.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="transform">The transformation <see cref="SlimMath.Matrix4d"/>.</param>
        /// <returns>The transformed <see cref="SlimMath.Vector4d"/>.</returns>
        public static Vector4D Transform(Vector4D vector, Matrix4D transform)
        {
            Vector4D result;
            Transform(ref vector, ref transform, out result);
            return result;
        }

        /// <summary>
        /// Transforms an array of 4D vectors by the given <see cref="SlimMath.Matrix4d"/>.
        /// </summary>
        /// <param name="source">The array of vectors to transform.</param>
        /// <param name="transform">The transformation <see cref="SlimMath.Matrix4d"/>.</param>
        /// <param name="destination">The array for which the transformed vectors are stored.
        /// This array may be the same array as <paramref name="source"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="destination"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="destination"/> is shorter in length than <paramref name="source"/>.</exception>
        public static void Transform(Vector4D[] source, ref Matrix4D transform, Vector4D[] destination)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");
            if (destination.Length < source.Length)
                throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");

            for (int i = 0; i < source.Length; ++i)
            {
                Transform(ref source[i], ref transform, out destination[i]);
            }
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The sum of the two vectors.</returns>
        public static Vector4D operator +(Vector4D left, Vector4D right)
        {
            return new Vector4D(left.x + right.x, left.y + right.y, left.z + right.z, left.w + right.w);
        }

        /// <summary>
        /// Assert a vector (return it unchanged).
        /// </summary>
        /// <param name="value">The vector to assert (unchange).</param>
        /// <returns>The asserted (unchanged) vector.</returns>
        public static Vector4D operator +(Vector4D value)
        {
            return value;
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <returns>The difference of the two vectors.</returns>
        public static Vector4D operator -(Vector4D left, Vector4D right)
        {
            return new Vector4D(left.x - right.x, left.y - right.y, left.z - right.z, left.w - right.w);
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>A vector facing in the opposite direction.</returns>
        public static Vector4D operator -(Vector4D value)
        {
            return new Vector4D(-value.x, -value.y, -value.z, -value.w);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector4D operator *(double scalar, Vector4D value)
        {
            return new Vector4D(value.x * scalar, value.y * scalar, value.z * scalar, value.w * scalar);
        }

        public static Vector4D operator *(Vector4D left, Matrix4D right)
        {
            return right.Transpose() * left;
        }

        //Is a column vector
        public static double operator *(Vector4D left, Vector4D right)
        {
            double ret = 0;
            ret += left.x * right.x;
            ret += left.y * right.y;
            ret += left.z * right.z;
            ret += left.w * right.w;
            return ret;
        }
        

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector4D operator *(Vector4D value, double scalar)
        {
            return new Vector4D(value.x * scalar, value.y * scalar, value.z * scalar, value.w * scalar);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector4D operator /(Vector4D value, double scalar)
        {
            return new Vector4D(value.x / scalar, value.y / scalar, value.z / scalar, value.w / scalar);
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Vector4D left, Vector4D right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Vector4D left, Vector4D right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SlimMath.Vector4d"/> to <see cref="SlimMath.Vector2d"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector2D(Vector4D value)
        {
            return new Vector2D(value.x, value.y);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SlimMath.Vector4d"/> to <see cref="SlimMath.Vector3D"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector3D(Vector4D value)
        {
            return new Vector3D(value.x, value.y, value.z);
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
                ToString(formatProvider);

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
        /// Determines whether the specified <see cref="SlimMath.Vector4d"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="SlimMath.Vector4d"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="SlimMath.Vector4d"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Vector4D other)
        {
            return (this.x == other.x) && (this.y == other.y) && (this.z == other.z) && (this.w == other.w);
        }

        /// <summary>
        /// Determines whether the specified <see cref="SlimMath.Vector4d"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="SlimMath.Vector4d"/> to compare with this instance.</param>
        /// <param name="epsilon">The amount of error allowed.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="SlimMath.Vector4d"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Vector4D other, double epsilon)
        {
            return ((double)Math.Abs(other.x - x) < epsilon &&
                (double)Math.Abs(other.y - y) < epsilon &&
                (double)Math.Abs(other.z - z) < epsilon &&
                (double)Math.Abs(other.w - w) < epsilon);
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

            return Equals((Vector4D)obj);
        }

        

#if SlimDX1xInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimMath.Vector4"/> to <see cref="SlimDX.Vector4"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SlimDX.Vector4(Vector4 value)
        {
            return new SlimDX.Vector4(value.X, value.Y, value.Z, value.W);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimDX.Vector4"/> to <see cref="SlimMath.Vector4"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Vector4(SlimDX.Vector4 value)
        {
            return new Vector4(value.X, value.Y, value.Z, value.W);
        }
#endif

#if WPFInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimMath.Vector4"/> to <see cref="System.Windows.Media.Media3D.Point4D"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator System.Windows.Media.Media3D.Point4D(Vector4 value)
        {
            return new System.Windows.Media.Media3D.Point4D(value.X, value.Y, value.Z, value.W);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Windows.Media.Media3D.Point4D"/> to <see cref="SlimMath.Vector4"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector4(System.Windows.Media.Media3D.Point4D value)
        {
            return new Vector4((double)value.X, (double)value.Y, (double)value.Z, (double)value.W);
        }
#endif

#if XnaInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimMath.Vector4"/> to <see cref="Microsoft.Xna.Framework.Vector4"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Microsoft.Xna.Framework.Vector4(Vector4 value)
        {
            return new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, value.W);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Microsoft.Xna.Framework.Vector4"/> to <see cref="SlimMath.Vector4"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Vector4(Microsoft.Xna.Framework.Vector4 value)
        {
            return new Vector4(value.X, value.Y, value.Z, value.W);
        }
#endif
    }
}
