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
    /// Represents a three dimensional mathematical vector.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
   // [TypeConverter(typeof(GraphicResearchHuiZhao.Vector3Converter))]
    public struct Vector3D : IEquatable<Vector3D>, IFormattable
    {
        /// <summary>
        /// The size of the <see cref="SlimMath.Vector3D"/> type, in bytes.
        /// </summary>
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(Vector3D));

        /// <summary>
        /// A <see cref="SlimMath.Vector3D"/> with all of its components set to zero.
        /// </summary>
        public static readonly Vector3D Zero = new Vector3D();

        /// <summary>
        /// The X unit <see cref="SlimMath.Vector3D"/> (1, 0, 0).
        /// </summary>
        public static readonly Vector3D UnitX = new Vector3D(1.0f, 0.0f, 0.0f);

        /// <summary>
        /// The Y unit <see cref="SlimMath.Vector3D"/> (0, 1, 0).
        /// </summary>
        public static readonly Vector3D UnitY = new Vector3D(0.0f, 1.0f, 0.0f);

        /// <summary>
        /// The Z unit <see cref="SlimMath.Vector3D"/> (0, 0, 1).
        /// </summary>
        public static readonly Vector3D UnitZ = new Vector3D(0.0f, 0.0f, 1.0f);

        /// <summary>
        /// A <see cref="SlimMath.Vector3D"/> with all of its components set to one.
        /// </summary>
        public static readonly Vector3D One = new Vector3D(1.0f, 1.0f, 1.0f);

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


        public static Vector3D MinValue = new Vector3D(double.MinValue, double.MinValue, double.MinValue);
        public static Vector3D MaxValue = new Vector3D(double.MaxValue, double.MaxValue, double.MaxValue);


        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Vector3D"/> struct.
        /// </summary>
        /// <param name="value">The value that will be assigned to all components.</param>
        public Vector3D(double value)
        {
            x = value;
            y = value;
            z = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Vector3D"/> struct.
        /// </summary>
        /// <param name="x">Initial value for the X component of the vector.</param>
        /// <param name="y">Initial value for the Y component of the vector.</param>
        /// <param name="z">Initial value for the Z component of the vector.</param>
        public Vector3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Vector3D"/> struct.
        /// </summary>
        /// <param name="value">A vector containing the values with which to initialize the X and Y components.</param>
        /// <param name="z">Initial value for the Z component of the vector.</param>
        public Vector3D(Vector2D value, double z)
        {
            this.x = value.x;
            this.y = value.y;
            this.z = z;
        }

        public Vector3D(double[] arr, int index)
        {
            x = arr[index];
            y = arr[index + 1];
            z = arr[index + 2];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Vector3D"/> struct.
        /// </summary>
        /// <param name="values">The values to assign to the X, Y, and Z components of the vector. This must be an array with three elements.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than three elements.</exception>
        public Vector3D(double[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length != 3)
                throw new ArgumentOutOfRangeException("values", "There must be three and only three input values for Vector3.");

            x = values[0];
            y = values[1];
            z = values[2];
        }

        /// <summary>
        /// Gets a value indicting whether this instance is normalized.
        /// </summary>
        public bool IsNormalized
        {
            get { return Math.Abs((x * x) + (y * y) + (z * z) - 1f) < Utilities.ZeroTolerance; }
        }

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        /// <remarks>
        /// <see cref="SlimMath.Vector3D.LengthSquared"/> may be preferred when only the relative length is needed
        /// and speed is of the essence.
        /// </remarks>
        public double Length()
        {
            return (double)Math.Sqrt((x * x) + (y * y) + (z * z));
        }

        //AddOn
        public Matrix3D OuterCross(Vector3D v)
        {
            Matrix3D m = new Matrix3D();
            m[0, 0] = x * v.x;
            m[0, 1] = x * v.y;
            m[0, 2] = x * v.z;
            m[1, 0] = y * v.x;
            m[1, 1] = y * v.y;
            m[1, 2] = y * v.z;
            m[2, 0] = z * v.x;
            m[2, 1] = z * v.y;
            m[2, 2] = z * v.z;
            return m;
        }

        /// <summary>
        /// Calculates the squared length of the vector.
        /// </summary>
        /// <remarks>
        /// This property may be preferred to <see cref="SlimMath.Vector3D.Length"/> when only a relative length is needed
        /// and speed is of the essence.
        /// </remarks>
        public double LengthSquared
        {
            get { return (x * x) + (y * y) + (z * z); }
        }

        /// <summary>
        /// Gets or sets the component at the specified index.
        /// </summary>
        /// <value>The value of the X, Y, or Z component, depending on the index.</value>
        /// <param name="index">The index of the component to access. Use 0 for the X component, 1 for the Y component, and 2 for the Z component.</param>
        /// <returns>The value of the component at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="index"/> is out of the range [0, 2].</exception>
        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                }

                throw new ArgumentOutOfRangeException("index", "Indices for Vector3 run from 0 to 2, inclusive.");
            }

            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    default: throw new ArgumentOutOfRangeException("index", "Indices for Vector3 run from 0 to 2, inclusive.");
                }
            }
        }

        /// <summary>
        /// Converts the vector into a unit vector.
        /// </summary>
        public Vector3D Normalize()
        {
            double length = Length();
            if (length > Utilities.ZeroTolerance)
            {
                double inv = 1.0f / length;
                x *= inv;
                y *= inv;
                z *= inv;
            }
            return this;
        }

        //public Vector3D Normalize()
        //{
        //    Vector3D v = new Vector3D(this.x, this.y, this.z);

        //    double length = Length();
        //    if (length > Utilities.ZeroTolerance)
        //    {
        //        double inv = 1.0f / length;
        //        v.x *= inv;
        //        v.y *= inv;
        //        v.z *= inv;
        //    }
        //    return v;
        //}

       
        public  static double Cotangent(Vector3D v1, Vector3D v2, Vector3D v3)
        {
            double cot = (v2 - v1).Dot(v3 - v1) / (v2 - v1).Cross(v3 - v1).Length();
            return cot;
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        public void Negate()
        {
            x = -x;
            y = -y;
            z = -z;
        }

        /// <summary>
        /// Takes the absolute value of each component.
        /// </summary>
        public void Abs()
        {
            this.x = Math.Abs(x);
            this.y = Math.Abs(y);
            this.z = Math.Abs(z);
        }

        /// <summary>
        /// Creates an array containing the elements of the vector.
        /// </summary>
        /// <returns>A three-element array containing the components of the vector.</returns>
        public double[] ToArray()
        {
            return new double[] { x, y, z };
        }

        #region Transcendentals
        /// <summary>
        /// Takes the square root of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the square root of.</param>
        /// <param name="result">When the method completes, contains a vector that is the square root of the input vector.</param>
        public static void Sqrt(ref Vector3D value, out Vector3D result)
        {
            result.x = (double)Math.Sqrt(value.x);
            result.y = (double)Math.Sqrt(value.y);
            result.z = (double)Math.Sqrt(value.z);
        }

        /// <summary>
        /// Takes the square root of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the square root of.</param>
        /// <returns>A vector that is the square root of the input vector.</returns>
        public static Vector3D Sqrt(Vector3D value)
        {
            Vector3D temp;
            Sqrt(ref value, out temp);
            return temp;
        }

        /// <summary>
        /// Takes the reciprocal of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the reciprocal of.</param>
        /// <param name="result">When the method completes, contains a vector that is the reciprocal of the input vector.</param>
        public static void Reciprocal(ref Vector3D value, out Vector3D result)
        {
            result.x = 1.0f / value.x;
            result.y = 1.0f / value.y;
            result.z = 1.0f / value.z;
        }

        /// <summary>
        /// Takes the reciprocal of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the reciprocal of.</param>
        /// <returns>A vector that is the reciprocal of the input vector.</returns>
        public static Vector3D Reciprocal(Vector3D value)
        {
            Vector3D temp;
            Reciprocal(ref value, out temp);
            return temp;
        }

        /// <summary>
        /// Takes the square root of each component in the vector and than takes the reciprocal of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the square root and recpirocal of.</param>
        /// <param name="result">When the method completes, contains a vector that is the square root and reciprocal of the input vector.</param>
        public static void ReciprocalSqrt(ref Vector3D value, out Vector3D result)
        {
            result.x = 1.0f / (double)Math.Sqrt(value.x);
            result.y = 1.0f / (double)Math.Sqrt(value.y);
            result.z = 1.0f / (double)Math.Sqrt(value.z);
        }

        /// <summary>
        /// Takes the square root of each component in the vector and than takes the reciprocal of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the square root and recpirocal of.</param>
        /// <returns>A vector that is the square root and reciprocal of the input vector.</returns>
        public static Vector3D ReciprocalSqrt(Vector3D value)
        {
            Vector3D temp;
            ReciprocalSqrt(ref value, out temp);
            return temp;
        }

        /// <summary>
        /// Takes e raised to the component in the vector.
        /// </summary>
        /// <param name="value">The value to take e raised to each component of.</param>
        /// <param name="result">When the method completes, contains a vector that has e raised to each of the components in the input vector.</param>
        public static void Exp(ref Vector3D value, out Vector3D result)
        {
            result.x = (double)Math.Exp(value.x);
            result.y = (double)Math.Exp(value.y);
            result.z = (double)Math.Exp(value.z);
        }

        /// <summary>
        /// Takes e raised to the component in the vector.
        /// </summary>
        /// <param name="value">The value to take e raised to each component of.</param>
        /// <returns>A vector that has e raised to each of the components in the input vector.</returns>
        public static Vector3D Exp(Vector3D value)
        {
            Vector3D temp;
            Exp(ref value, out temp);
            return temp;
        }

        /// <summary>
        /// Takes the sine and than the cosine of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the sine and cosine of.</param>
        /// <param name="sinResult">When the method completes, contains the sine of each component in the input vector.</param>
        /// <param name="cosResult">When the method completes, contains the cpsome pf each component in the input vector.</param>
        public static void SinCos(ref Vector3D value, out Vector3D sinResult, out Vector3D cosResult)
        {
            sinResult.x = (double)Math.Sin(value.x);
            sinResult.y = (double)Math.Sin(value.y);
            sinResult.z = (double)Math.Sin(value.z);

            cosResult.x = (double)Math.Cos(value.x);
            cosResult.y = (double)Math.Cos(value.y);
            cosResult.z = (double)Math.Cos(value.z);
        }

        /// <summary>
        /// Takes the sine of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the sine of.</param>
        /// <param name="result">When the method completes, a vector that contains the sine of each component in the input vector.</param>
        public static void Sin(ref Vector3D value, out Vector3D result)
        {
            result.x = (double)Math.Sin(value.x);
            result.y = (double)Math.Sin(value.y);
            result.z = (double)Math.Sin(value.z);
        }

        /// <summary>
        /// Takes the sine of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the sine of.</param>
        /// <returns>A vector that contains the sine of each component in the input vector.</returns>
        public static Vector3D Sin(Vector3D value)
        {
            Vector3D temp;
            Sin(ref value, out temp);
            return temp;
        }

        /// <summary>
        /// Takes the cosine of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the cosine of.</param>
        /// <param name="result">When the method completes, contains a vector that contains the cosine of each component in the input vector.</param>
        public static void Cos(ref Vector3D value, out Vector3D result)
        {
            result.x = (double)Math.Cos(value.x);
            result.y = (double)Math.Cos(value.y);
            result.z = (double)Math.Cos(value.z);
        }

        /// <summary>
        /// Takes the cosine of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the cosine of.</param>
        /// <returns>A vector that contains the cosine of each component in the input vector.</returns>
        public static Vector3D Cos(Vector3D value)
        {
            Vector3D temp;
            Cos(ref value, out temp);
            return temp;
        }

        /// <summary>
        /// Takes the tangent of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the tangent of.</param>
        /// <param name="result">When the method completes, contains a vector that contains the tangent of each component in the input vector.</param>
        public static void Tan(ref Vector3D value, out Vector3D result)
        {
            result.x = (double)Math.Tan(value.x);
            result.y = (double)Math.Tan(value.y);
            result.z = (double)Math.Tan(value.z);
        }

        /// <summary>
        /// Takes the tangent of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the tangent of.</param>
        /// <returns>A vector that contains the tangent of each component in the input vector.</returns>
        public static Vector3D Tan(Vector3D value)
        {
            Vector3D temp;
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
        public static void Add(ref Vector3D left, ref Vector3D right, out Vector3D result)
        {
            result = new Vector3D(left.x + right.x, left.y + right.y, left.z + right.z);
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The sum of the two vectors.</returns>
        public static Vector3D Add(Vector3D left, Vector3D right)
        {
            return new Vector3D(left.x + right.x, left.y + right.y, left.z + right.z);
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <param name="result">When the method completes, contains the difference of the two vectors.</param>
        public static void Subtract(ref Vector3D left, ref Vector3D right, out Vector3D result)
        {
            result = new Vector3D(left.x - right.x, left.y - right.y, left.z - right.z);
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <returns>The difference of the two vectors.</returns>
        public static Vector3D Subtract(Vector3D left, Vector3D right)
        {
            return new Vector3D(left.x - right.x, left.y - right.y, left.z - right.z);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <param name="result">When the method completes, contains the scaled vector.</param>
        public static void Multiply(ref Vector3D value, double scalar, out Vector3D result)
        {
            result = new Vector3D(value.x * scalar, value.y * scalar, value.z * scalar);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector3D Multiply(Vector3D value, double scalar)
        {
            return new Vector3D(value.x * scalar, value.y * scalar, value.z * scalar);
        }

        /// <summary>
        /// Modulates a vector with another by performing component-wise multiplication.
        /// </summary>
        /// <param name="left">The first vector to modulate.</param>
        /// <param name="right">The second vector to modulate.</param>
        /// <param name="result">When the method completes, contains the modulated vector.</param>
        public static void Modulate(ref Vector3D left, ref Vector3D right, out Vector3D result)
        {
            result = new Vector3D(left.x * right.x, left.y * right.y, left.z * right.z);
        }

        /// <summary>
        /// Modulates a vector with another by performing component-wise multiplication.
        /// </summary>
        /// <param name="left">The first vector to modulate.</param>
        /// <param name="right">The second vector to modulate.</param>
        /// <returns>The modulated vector.</returns>
        public static Vector3D Modulate(Vector3D left, Vector3D right)
        {
            return new Vector3D(left.x * right.x, left.y * right.y, left.z * right.z);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <param name="result">When the method completes, contains the scaled vector.</param>
        public static void Divide(ref Vector3D value, double scalar, out Vector3D result)
        {
            result = new Vector3D(value.x / scalar, value.y / scalar, value.z / scalar);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector3D Divide(Vector3D value, double scalar)
        {
            return new Vector3D(value.x / scalar, value.y / scalar, value.z / scalar);
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <param name="result">When the method completes, contains a vector facing in the opposite direction.</param>
        public static void Negate(ref Vector3D value, out Vector3D result)
        {
            result = new Vector3D(-value.x, -value.y, -value.z);
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>A vector facing in the opposite direction.</returns>
        public static Vector3D Negate(Vector3D value)
        {
            return new Vector3D(-value.x, -value.y, -value.z);
        }

        /// <summary>
        /// Takes the absolute value of each component.
        /// </summary>
        /// <param name="value">The vector to take the absolute value of.</param>
        /// <param name="result">When the method completes, contains a vector that has all positive components.</param>
        public static void Abs(ref Vector3D value, out Vector3D result)
        {
            result = new Vector3D(Math.Abs(value.x), Math.Abs(value.y), Math.Abs(value.z));
        }

        /// <summary>
        /// Takes the absolute value of each component.
        /// </summary>
        /// <param name="value">The vector to take the absolute value of.</param>
        /// <returns>A vector that has all positive components.</returns>
        public static Vector3D Abs(Vector3D value)
        {
            return new Vector3D(Math.Abs(value.x), Math.Abs(value.y), Math.Abs(value.z));
        }

        /// <summary>
        /// Returns a <see cref="SlimMath.Vector3D"/> containing the 3D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 3D triangle.
        /// </summary>
        /// <param name="value1">A <see cref="SlimMath.Vector3D"/> containing the 3D Cartesian coordinates of vertex 1 of the triangle.</param>
        /// <param name="value2">A <see cref="SlimMath.Vector3D"/> containing the 3D Cartesian coordinates of vertex 2 of the triangle.</param>
        /// <param name="value3">A <see cref="SlimMath.Vector3D"/> containing the 3D Cartesian coordinates of vertex 3 of the triangle.</param>
        /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="value2"/>).</param>
        /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="value3"/>).</param>
        /// <param name="result">When the method completes, contains the 3D Cartesian coordinates of the specified point.</param>
        public static void Barycentric(ref Vector3D value1, ref Vector3D value2, ref Vector3D value3, double amount1, double amount2, out Vector3D result)
        {
            result = new Vector3D((value1.x + (amount1 * (value2.x - value1.x))) + (amount2 * (value3.x - value1.x)),
                (value1.y + (amount1 * (value2.y - value1.y))) + (amount2 * (value3.y - value1.y)),
                (value1.z + (amount1 * (value2.z - value1.z))) + (amount2 * (value3.z - value1.z)));
        }

        /// <summary>
        /// Returns a <see cref="SlimMath.Vector3D"/> containing the 3D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 3D triangle.
        /// </summary>
        /// <param name="value1">A <see cref="SlimMath.Vector3D"/> containing the 3D Cartesian coordinates of vertex 1 of the triangle.</param>
        /// <param name="value2">A <see cref="SlimMath.Vector3D"/> containing the 3D Cartesian coordinates of vertex 2 of the triangle.</param>
        /// <param name="value3">A <see cref="SlimMath.Vector3D"/> containing the 3D Cartesian coordinates of vertex 3 of the triangle.</param>
        /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="value2"/>).</param>
        /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="value3"/>).</param>
        /// <returns>A new <see cref="SlimMath.Vector3D"/> containing the 3D Cartesian coordinates of the specified point.</returns>
        public static Vector3D Barycentric(Vector3D value1, Vector3D value2, Vector3D value3, double amount1, double amount2)
        {
            Vector3D result;
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
        public static void Clamp(ref Vector3D value, ref Vector3D min, ref Vector3D max, out Vector3D result)
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

            result = new Vector3D(x, y, z);
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static Vector3D Clamp(Vector3D value, Vector3D min, Vector3D max)
        {
            Vector3D result;
            Clamp(ref value, ref min, ref max, out result);
            return result;
        }

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="left">First source vector.</param>
        /// <param name="right">Second source vector.</param>
        /// <param name="result">When the method completes, contains the cross product of the two vectors.</param>
        public static void Cross(ref Vector3D left, ref Vector3D right, out Vector3D result)
        {
            result = new Vector3D(
                (left.y * right.z) - (left.z * right.y),
                (left.z * right.x) - (left.x * right.z),
                (left.x * right.y) - (left.y * right.x));
        }

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="left">First source vector.</param>
        /// <param name="right">Second source vector.</param>
        /// <returns>The cross product of the two vectors.</returns>
        public static Vector3D Cross(Vector3D left, Vector3D right)
        {
            Vector3D result;
            Cross(ref left, ref right, out result);
            return result;
        }

        public Vector3D Cross(Vector3D right)
        {
            Vector3D result;
            Cross(ref this, ref right, out result);
            return result;      
        }

        /// <summary>
        /// Calculates the tripple cross product of three vectors.
        /// </summary>
        /// <param name="value1">First source vector.</param>
        /// <param name="value2">Second source vector.</param>
        /// <param name="value3">Third source vector.</param>
        /// <param name="result">When the method completes, contains the tripple cross product of the three vectors.</param>
        public static void TripleProduct(ref Vector3D value1, ref Vector3D value2, ref Vector3D value3, out double result)
        {
            Vector3D temp;
            Vector3D.Cross(ref value2, ref value3, out temp);
            Vector3D.Dot(ref value1, ref temp, out result);
        }

        /// <summary>
        /// Calculates the tripple cross product of three vectors.
        /// </summary>
        /// <param name="value1">First source vector.</param>
        /// <param name="value2">Second source vector.</param>
        /// <param name="value3">Third source vector.</param>
        /// <returns>The tripple cross product of the three vectors.</returns>
        public static double TripleProduct(Vector3D value1, Vector3D value2, Vector3D value3)
        {
            double result;
            TripleProduct(ref value1, ref value2, ref value3, out result);
            return result;
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">When the method completes, contains the distance between the two vectors.</param>
        /// <remarks>
        /// <see cref="SlimMath.Vector3D.DistanceSquared(ref Vector3D, ref Vector3D, out double)"/> may be preferred when only the relative distance is needed
        /// and speed is of the essence.
        /// </remarks>
        public static void Distance(ref Vector3D value1, ref Vector3D value2, out double result)
        {
            double x = value1.x - value2.x;
            double y = value1.y - value2.y;
            double z = value1.z - value2.z;

            result = (double)Math.Sqrt((x * x) + (y * y) + (z * z));
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        /// <remarks>
        /// <see cref="SlimMath.Vector3D.DistanceSquared(Vector3D, Vector3D)"/> may be preferred when only the relative distance is needed
        /// and speed is of the essence.
        /// </remarks>
        public static double Distance(Vector3D value1, Vector3D value2)
        {
            double x = value1.x - value2.x;
            double y = value1.y - value2.y;
            double z = value1.z - value2.z;

            return (double)Math.Sqrt((x * x) + (y * y) + (z * z));
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
        public static void DistanceSquared(ref Vector3D value1, ref Vector3D value2, out double result)
        {
            double x = value1.x - value2.x;
            double y = value1.y - value2.y;
            double z = value1.z - value2.z;

            result = (x * x) + (y * y) + (z * z);
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
        public static double DistanceSquared(Vector3D value1, Vector3D value2)
        {
            double x = value1.x - value2.x;
            double y = value1.y - value2.y;
            double z = value1.z - value2.z;

            return (x * x) + (y * y) + (z * z);
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="left">First source vector.</param>
        /// <param name="right">Second source vector.</param>
        /// <param name="result">When the method completes, contains the dot product of the two vectors.</param>
        public static void Dot(ref Vector3D left, ref Vector3D right, out double result)
        {
            result = (left.x * right.x) + (left.y * right.y) + (left.z * right.z);
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="left">First source vector.</param>
        /// <param name="right">Second source vector.</param>
        /// <returns>The dot product of the two vectors.</returns>
        public static double Dot(Vector3D left, Vector3D right)
        {
            return (left.x * right.x) + (left.y * right.y) + (left.z * right.z);
        }

        public double Dot(Vector3D right)
        {
            return Dot(this, right);
        }

        /// <summary>
        /// Converts the vector into a unit vector.
        /// </summary>
        /// <param name="value">The vector to normalize.</param>
        /// <param name="result">When the method completes, contains the normalized vector.</param>
        public static void Normalize(ref Vector3D value, out Vector3D result)
        {
            result = value;
            result.Normalize();
        }

        /// <summary>
        /// Converts the vector into a unit vector.
        /// </summary>
        /// <param name="value">The vector to normalize.</param>
        /// <returns>The normalized vector.</returns>
        public static Vector3D Normalize(Vector3D value)
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
        public static void Lerp(ref Vector3D start, ref Vector3D end, double amount, out Vector3D result)
        {
            result.x = start.x + ((end.x - start.x) * amount);
            result.y = start.y + ((end.y - start.y) * amount);
            result.z = start.z + ((end.z - start.z) * amount);
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
        public static Vector3D Lerp(Vector3D start, Vector3D end, double amount)
        {
            Vector3D result;
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
        public static void SmoothStep(ref Vector3D start, ref Vector3D end, double amount, out Vector3D result)
        {
            amount = (amount > 1.0f) ? 1.0f : ((amount < 0.0f) ? 0.0f : amount);
            amount = (amount * amount) * (3.0f - (2.0f * amount));

            result.x = start.x + ((end.x - start.x) * amount);
            result.y = start.y + ((end.y - start.y) * amount);
            result.z = start.z + ((end.z - start.z) * amount);
        }

        /// <summary>
        /// Performs a cubic interpolation between two vectors.
        /// </summary>
        /// <param name="start">Start vector.</param>
        /// <param name="end">End vector.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <returns>The cubic interpolation of the two vectors.</returns>
        public static Vector3D SmoothStep(Vector3D start, Vector3D end, double amount)
        {
            Vector3D result;
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
        public static void Hermite(ref Vector3D value1, ref Vector3D tangent1, ref Vector3D value2, ref Vector3D tangent2, double amount, out Vector3D result)
        {
            double squared = amount * amount;
            double cubed = amount * squared;
            double part1 = ((2.0f * cubed) - (3.0f * squared)) + 1.0f;
            double part2 = (-2.0f * cubed) + (3.0f * squared);
            double part3 = (cubed - (2.0f * squared)) + amount;
            double part4 = cubed - squared;

            result.x = (((value1.x * part1) + (value2.x * part2)) + (tangent1.x * part3)) + (tangent2.x * part4);
            result.y = (((value1.y * part1) + (value2.y * part2)) + (tangent1.y * part3)) + (tangent2.y * part4);
            result.z = (((value1.z * part1) + (value2.z * part2)) + (tangent1.z * part3)) + (tangent2.z * part4);
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
        public static Vector3D Hermite(Vector3D value1, Vector3D tangent1, Vector3D value2, Vector3D tangent2, double amount)
        {
            Vector3D result;
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
        public static void CatmullRom(ref Vector3D value1, ref Vector3D value2, ref Vector3D value3, ref Vector3D value4, double amount, out Vector3D result)
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
        public static Vector3D CatmullRom(Vector3D value1, Vector3D value2, Vector3D value3, Vector3D value4, double amount)
        {
            Vector3D result;
            CatmullRom(ref value1, ref value2, ref value3, ref value4, amount, out result);
            return result;
        }

        /// <summary>
        /// Returns a vector containing the largest components of the specified vectors.
        /// </summary>
        /// <param name="value1">The first source vector.</param>
        /// <param name="value2">The second source vector.</param>
        /// <param name="result">When the method completes, contains an new vector composed of the largest components of the source vectors.</param>
        public static void Max(ref Vector3D value1, ref Vector3D value2, out Vector3D result)
        {
            result.x = (value1.x > value2.x) ? value1.x : value2.x;
            result.y = (value1.y > value2.y) ? value1.y : value2.y;
            result.z = (value1.z > value2.z) ? value1.z : value2.z;
        }

        /// <summary>
        /// Returns a vector containing the largest components of the specified vectors.
        /// </summary>
        /// <param name="value1">The first source vector.</param>
        /// <param name="value2">The second source vector.</param>
        /// <returns>A vector containing the largest components of the source vectors.</returns>
        public static Vector3D Max(Vector3D value1, Vector3D value2)
        {
            Vector3D result;
            Max(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Returns a vector containing the smallest components of the specified vectors.
        /// </summary>
        /// <param name="value1">The first source vector.</param>
        /// <param name="value2">The second source vector.</param>
        /// <param name="result">When the method completes, contains an new vector composed of the smallest components of the source vectors.</param>
        public static void Min(ref Vector3D value1, ref Vector3D value2, out Vector3D result)
        {
            result.x = (value1.x < value2.x) ? value1.x : value2.x;
            result.y = (value1.y < value2.y) ? value1.y : value2.y;
            result.z = (value1.z < value2.z) ? value1.z : value2.z;
        }

        /// <summary>
        /// Returns a vector containing the smallest components of the specified vectors.
        /// </summary>
        /// <param name="value1">The first source vector.</param>
        /// <param name="value2">The second source vector.</param>
        /// <returns>A vector containing the smallest components of the source vectors.</returns>
        public static Vector3D Min(Vector3D value1, Vector3D value2)
        {
            Vector3D result;
            Min(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Projects a 3D vector from object space into screen space. 
        /// </summary>
        /// <param name="vector">The vector to project.</param>
        /// <param name="x">The X position of the viewport.</param>
        /// <param name="y">The Y position of the viewport.</param>
        /// <param name="width">The width of the viewport.</param>
        /// <param name="height">The height of the viewport.</param>
        /// <param name="minZ">The minimum depth of the viewport.</param>
        /// <param name="maxZ">The maximum depth of the viewport.</param>
        /// <param name="worldViewProjection">The combined world-view-projection matrix.</param>
        /// <param name="result">When the method completes, contains the vector in screen space.</param>
        public static void Project(ref Vector3D vector, double x, double y, double width, double height, double minZ, double maxZ, ref Matrix4D worldViewProjection, out Vector3D result)
        {
            Vector3D v = new Vector3D();
            TransformCoordinate(ref vector, ref worldViewProjection, out v);

            result = new Vector3D(((1.0f + v.x) * 0.5f * width) + x, ((1.0f - v.y) * 0.5f * height) + y, (v.z * (maxZ - minZ)) + minZ);
        }

        /// <summary>
        /// Projects a 3D vector from object space into screen space. 
        /// </summary>
        /// <param name="vector">The vector to project.</param>
        /// <param name="x">The X position of the viewport.</param>
        /// <param name="y">The Y position of the viewport.</param>
        /// <param name="width">The width of the viewport.</param>
        /// <param name="height">The height of the viewport.</param>
        /// <param name="minZ">The minimum depth of the viewport.</param>
        /// <param name="maxZ">The maximum depth of the viewport.</param>
        /// <param name="worldViewProjection">The combined world-view-projection matrix.</param>
        /// <returns>The vector in screen space.</returns>
        public static Vector3D Project(Vector3D vector, double x, double y, double width, double height, double minZ, double maxZ, Matrix4D worldViewProjection)
        {
            Vector3D result;
            Project(ref vector, x, y, width, height, minZ, maxZ, ref worldViewProjection, out result);
            return result;
        }

        /// <summary>
        /// Projects a 3D vector from screen space into object space. 
        /// </summary>
        /// <param name="vector">The vector to project.</param>
        /// <param name="x">The X position of the viewport.</param>
        /// <param name="y">The Y position of the viewport.</param>
        /// <param name="width">The width of the viewport.</param>
        /// <param name="height">The height of the viewport.</param>
        /// <param name="minZ">The minimum depth of the viewport.</param>
        /// <param name="maxZ">The maximum depth of the viewport.</param>
        /// <param name="worldViewProjection">The combined world-view-projection matrix.</param>
        /// <param name="result">When the method completes, contains the vector in object space.</param>
        public static void Unproject(ref Vector3D vector, double x, double y, double width, double height, double minZ, double maxZ, ref Matrix4D worldViewProjection, out Vector3D result)
        {
            Vector3D v = new Vector3D();
            Matrix4D matrix = new Matrix4D();
            Matrix4D.Inverse(ref worldViewProjection, out matrix);

            v.x = (((vector.x - x) / width) * 2.0f) - 1.0f;
            v.y = -((((vector.y - y) / height) * 2.0f) - 1.0f);
            v.z = (vector.z - minZ) / (maxZ - minZ);

            TransformCoordinate(ref v, ref matrix, out result);
        }

        /// <summary>
        /// Projects a 3D vector from screen space into object space. 
        /// </summary>
        /// <param name="vector">The vector to project.</param>
        /// <param name="x">The X position of the viewport.</param>
        /// <param name="y">The Y position of the viewport.</param>
        /// <param name="width">The width of the viewport.</param>
        /// <param name="height">The height of the viewport.</param>
        /// <param name="minZ">The minimum depth of the viewport.</param>
        /// <param name="maxZ">The maximum depth of the viewport.</param>
        /// <param name="worldViewProjection">The combined world-view-projection matrix.</param>
        /// <returns>The vector in object space.</returns>
        public static Vector3D Unproject(Vector3D vector, double x, double y, double width, double height, double minZ, double maxZ, Matrix4D worldViewProjection)
        {
            Vector3D result;
            Unproject(ref vector, x, y, width, height, minZ, maxZ, ref worldViewProjection, out result);
            return result;
        }

        /// <summary>
        /// Returns the reflection of a vector off a surface that has the specified normal. 
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="normal">Normal of the surface.</param>
        /// <param name="result">When the method completes, contains the reflected vector.</param>
        /// <remarks>Reflect only gives the direction of a reflection off a surface, it does not determine 
        /// whether the original vector was close enough to the surface to hit it.</remarks>
        public static void Reflect(ref Vector3D vector, ref Vector3D normal, out Vector3D result)
        {
            double dot = (vector.x * normal.x) + (vector.y * normal.y) + (vector.z * normal.z);

            result.x = vector.x - ((2.0f * dot) * normal.x);
            result.y = vector.y - ((2.0f * dot) * normal.y);
            result.z = vector.z - ((2.0f * dot) * normal.z);
        }

        /// <summary>
        /// Returns the reflection of a vector off a surface that has the specified normal. 
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="normal">Normal of the surface.</param>
        /// <returns>The reflected vector.</returns>
        /// <remarks>Reflect only gives the direction of a reflection off a surface, it does not determine 
        /// whether the original vector was close enough to the surface to hit it.</remarks>
        public static Vector3D Reflect(Vector3D vector, Vector3D normal)
        {
            Vector3D result;
            Reflect(ref vector, ref normal, out result);
            return result;
        }

        /// <summary>
        /// Returns the fraction of a vector off a surface that has the specified normal and index.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="normal">Normal of the surface.</param>
        /// <param name="index">Index of refraction.</param>
        /// <param name="result">When the method completes, contains the refracted vector.</param>
        public static void Refract(ref Vector3D vector, ref Vector3D normal, double index, out Vector3D result)
        {
            double cos1;
            Dot(ref vector, ref normal, out cos1);

            double radicand = 1.0f - (index * index) * (1.0f - (cos1 * cos1));

            if (radicand < 0.0f)
            {
                result = Vector3D.Zero;
            }
            else
            {
                double cos2 = (double)Math.Sqrt(radicand);
                result = (index * vector) + ((cos2 - index * cos1) * normal);
            }
        }

        /// <summary>
        /// Returns the fraction of a vector off a surface that has the specified normal and index.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="normal">Normal of the surface.</param>
        /// <param name="index">Index of refraction.</param>
        /// <returns>The refracted vector.</returns>
        public static Vector3D Refract(Vector3D vector, Vector3D normal, double index)
        {
            Vector3D result;
            Refract(ref vector, ref normal, index, out result);
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
        public static void Orthogonalize(Vector3D[] destination, params Vector3D[] source)
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
                Vector3D newvector = source[i];

                for (int r = 0; r < i; ++r)
                {
                    newvector -= (Vector3D.Dot(destination[r], newvector) / Vector3D.Dot(destination[r], destination[r])) * destination[r];
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
        public static void Orthonormalize(Vector3D[] destination, params Vector3D[] source)
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
                Vector3D newvector = source[i];

                for (int r = 0; r < i; ++r)
                {
                    newvector -= Vector3D.Dot(destination[r], newvector) * destination[r];
                }

                newvector.Normalize();
                destination[i] = newvector;
            }
        }

        /// <summary>
        /// Transforms a 3D vector by the given <see cref="SlimMath.Quaternion"/> rotation.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="rotation">The <see cref="SlimMath.Quaternion"/> rotation to apply.</param>
        /// <param name="result">When the method completes, contains the transformed <see cref="SlimMath.Vector4d"/>.</param>
        public static void Transform(ref Vector3D vector, ref Quaternion rotation, out Vector3D result)
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

            result = new Vector3D(
                ((vector.x * num1) + (vector.y * num2)) + (vector.z * num3),
                ((vector.x * num4) + (vector.y * num5)) + (vector.z * num6),
                ((vector.x * num7) + (vector.y * num8)) + (vector.z * num9));
        }

        /// <summary>
        /// Transforms a 3D vector by the given <see cref="SlimMath.Quaternion"/> rotation.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="rotation">The <see cref="SlimMath.Quaternion"/> rotation to apply.</param>
        /// <returns>The transformed <see cref="SlimMath.Vector4d"/>.</returns>
        public static Vector3D Transform(Vector3D vector, Quaternion rotation)
        {
            Vector3D result;
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
        public static void Transform(Vector3D[] source, ref Quaternion rotation, Vector3D[] destination)
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
                destination[i] = new Vector3D(
                    ((source[i].x * num1) + (source[i].y * num2)) + (source[i].z * num3),
                    ((source[i].x * num4) + (source[i].y * num5)) + (source[i].z * num6),
                    ((source[i].x * num7) + (source[i].y * num8)) + (source[i].z * num9));
            }
        }

        /// <summary>
        /// Transforms a 3D vector by the given <see cref="SlimMath.Matrix4d"/>.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="transform">The transformation <see cref="SlimMath.Matrix4d"/>.</param>
        /// <param name="result">When the method completes, contains the transformed <see cref="SlimMath.Vector4d"/>.</param>
        public static void Transform(ref Vector3D vector, ref Matrix4D transform, out Vector4D result)
        {
            result = new Vector4D(
                (vector.x * transform.M11) + (vector.y * transform.M21) + (vector.z * transform.M31) + transform.M41,
                (vector.x * transform.M12) + (vector.y * transform.M22) + (vector.z * transform.M32) + transform.M42,
                (vector.x * transform.M13) + (vector.y * transform.M23) + (vector.z * transform.M33) + transform.M43,
                (vector.x * transform.M14) + (vector.y * transform.M24) + (vector.z * transform.M34) + transform.M44);
        }

        /// <summary>
        /// Transforms a 3D vector by the given <see cref="SlimMath.Matrix4d"/>.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="transform">The transformation <see cref="SlimMath.Matrix4d"/>.</param>
        /// <returns>The transformed <see cref="SlimMath.Vector4d"/>.</returns>
        public static Vector4D Transform(Vector3D vector, Matrix4D transform)
        {
            Vector4D result;
            Transform(ref vector, ref transform, out result);
            return result;
        }

        /// <summary>
        /// Transforms an array of 3D vectors by the given <see cref="SlimMath.Matrix4d"/>.
        /// </summary>
        /// <param name="source">The array of vectors to transform.</param>
        /// <param name="transform">The transformation <see cref="SlimMath.Matrix4d"/>.</param>
        /// <param name="destination">The array for which the transformed vectors are stored.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="destination"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="destination"/> is shorter in length than <paramref name="source"/>.</exception>
        public static void Transform(Vector3D[] source, ref Matrix4D transform, Vector4D[] destination)
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
        /// Performs a coordinate transformation using the given <see cref="SlimMath.Matrix4d"/>.
        /// </summary>
        /// <param name="coordinate">The coordinate vector to transform.</param>
        /// <param name="transform">The transformation <see cref="SlimMath.Matrix4d"/>.</param>
        /// <param name="result">When the method completes, contains the transformed coordinates.</param>
        /// <remarks>
        /// A coordinate transform performs the transformation with the assumption that the w component
        /// is one. The four dimensional vector obtained from the transformation operation has each
        /// component in the vector divided by the w component. This forces the wcomponent to be one and
        /// therefore makes the vector homogeneous. The homogeneous vector is often prefered when working
        /// with coordinates as the w component can safely be ignored.
        /// </remarks>
        public static void TransformCoordinate(ref Vector3D coordinate, ref Matrix4D transform, out Vector3D result)
        {
            Vector4D vector = new Vector4D();
            vector.x = (coordinate.x * transform.M11) + (coordinate.y * transform.M21) + (coordinate.z * transform.M31) + transform.M41;
            vector.y = (coordinate.x * transform.M12) + (coordinate.y * transform.M22) + (coordinate.z * transform.M32) + transform.M42;
            vector.z = (coordinate.x * transform.M13) + (coordinate.y * transform.M23) + (coordinate.z * transform.M33) + transform.M43;
            vector.w = 1f / ((coordinate.x * transform.M14) + (coordinate.y * transform.M24) + (coordinate.z * transform.M34) + transform.M44);

            result = new Vector3D(vector.x * vector.w, vector.y * vector.w, vector.z * vector.w);
        }

        /// <summary>
        /// Performs a coordinate transformation using the given <see cref="SlimMath.Matrix4d"/>.
        /// </summary>
        /// <param name="coordinate">The coordinate vector to transform.</param>
        /// <param name="transform">The transformation <see cref="SlimMath.Matrix4d"/>.</param>
        /// <returns>The transformed coordinates.</returns>
        /// <remarks>
        /// A coordinate transform performs the transformation with the assumption that the w component
        /// is one. The four dimensional vector obtained from the transformation operation has each
        /// component in the vector divided by the w component. This forces the wcomponent to be one and
        /// therefore makes the vector homogeneous. The homogeneous vector is often prefered when working
        /// with coordinates as the w component can safely be ignored.
        /// </remarks>
        public static Vector3D TransformCoordinate(Vector3D coordinate, Matrix4D transform)
        {
            Vector3D result;
            TransformCoordinate(ref coordinate, ref transform, out result);
            return result;
        }

        /// <summary>
        /// Performs a coordinate transformation on an array of vectors using the given <see cref="SlimMath.Matrix4d"/>.
        /// </summary>
        /// <param name="source">The array of coordinate vectors to trasnform.</param>
        /// <param name="transform">The transformation <see cref="SlimMath.Matrix4d"/>.</param>
        /// <param name="destination">The array for which the transformed vectors are stored.
        /// This array may be the same array as <paramref name="source"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="destination"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="destination"/> is shorter in length than <paramref name="source"/>.</exception>
        /// <remarks>
        /// A coordinate transform performs the transformation with the assumption that the w component
        /// is one. The four dimensional vector obtained from the transformation operation has each
        /// component in the vector divided by the w component. This forces the wcomponent to be one and
        /// therefore makes the vector homogeneous. The homogeneous vector is often prefered when working
        /// with coordinates as the w component can safely be ignored.
        /// </remarks>
        public static void TransformCoordinate(Vector3D[] source, ref Matrix4D transform, Vector3D[] destination)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");
            if (destination.Length < source.Length)
                throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");

            for (int i = 0; i < source.Length; ++i)
            {
                TransformCoordinate(ref source[i], ref transform, out destination[i]);
            }
        }

        /// <summary>
        /// Performs a normal transformation using the given <see cref="SlimMath.Matrix4d"/>.
        /// </summary>
        /// <param name="normal">The normal vector to transform.</param>
        /// <param name="transform">The transformation <see cref="SlimMath.Matrix4d"/>.</param>
        /// <param name="result">When the method completes, contains the transformed normal.</param>
        /// <remarks>
        /// A normal transform performs the transformation with the assumption that the w component
        /// is zero. This causes the fourth row and fourth collumn of the matrix to be unused. The
        /// end result is a vector that is not translated, but all other transformation properties
        /// apply. This is often prefered for normal vectors as normals purely represent direction
        /// rather than location because normal vectors should not be translated.
        /// </remarks>
        public static void TransformNormal(ref Vector3D normal, ref Matrix4D transform, out Vector3D result)
        {
            result = new Vector3D(
                (normal.x * transform.M11) + (normal.y * transform.M21) + (normal.z * transform.M31),
                (normal.x * transform.M12) + (normal.y * transform.M22) + (normal.z * transform.M32),
                (normal.x * transform.M13) + (normal.y * transform.M23) + (normal.z * transform.M33));
        }

        /// <summary>
        /// Performs a normal transformation using the given <see cref="SlimMath.Matrix4d"/>.
        /// </summary>
        /// <param name="normal">The normal vector to transform.</param>
        /// <param name="transform">The transformation <see cref="SlimMath.Matrix4d"/>.</param>
        /// <returns>The transformed normal.</returns>
        /// <remarks>
        /// A normal transform performs the transformation with the assumption that the w component
        /// is zero. This causes the fourth row and fourth collumn of the matrix to be unused. The
        /// end result is a vector that is not translated, but all other transformation properties
        /// apply. This is often prefered for normal vectors as normals purely represent direction
        /// rather than location because normal vectors should not be translated.
        /// </remarks>
        public static Vector3D TransformNormal(Vector3D normal, Matrix4D transform)
        {
            Vector3D result;
            TransformNormal(ref normal, ref transform, out result);
            return result;
        }

        /// <summary>
        /// Performs a normal transformation on an array of vectors using the given <see cref="SlimMath.Matrix4d"/>.
        /// </summary>
        /// <param name="source">The array of normal vectors to transform.</param>
        /// <param name="transform">The transformation <see cref="SlimMath.Matrix4d"/>.</param>
        /// <param name="destination">The array for which the transformed vectors are stored.
        /// This array may be the same array as <paramref name="source"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="destination"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="destination"/> is shorter in length than <paramref name="source"/>.</exception>
        /// <remarks>
        /// A normal transform performs the transformation with the assumption that the w component
        /// is zero. This causes the fourth row and fourth collumn of the matrix to be unused. The
        /// end result is a vector that is not translated, but all other transformation properties
        /// apply. This is often prefered for normal vectors as normals purely represent direction
        /// rather than location because normal vectors should not be translated.
        /// </remarks>
        public static void TransformNormal(Vector3D[] source, ref Matrix4D transform, Vector3D[] destination)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");
            if (destination.Length < source.Length)
                throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");

            for (int i = 0; i < source.Length; ++i)
            {
                TransformNormal(ref source[i], ref transform, out destination[i]);
            }
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The sum of the two vectors.</returns>
        public static Vector3D operator +(Vector3D left, Vector3D right)
        {
            return new Vector3D(left.x + right.x, left.y + right.y, left.z + right.z);
        }

        /// <summary>
        /// Assert a vector (return it unchanged).
        /// </summary>
        /// <param name="value">The vector to assert (unchange).</param>
        /// <returns>The asserted (unchanged) vector.</returns>
        public static Vector3D operator +(Vector3D value)
        {
            return value;
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <returns>The difference of the two vectors.</returns>
        public static Vector3D operator -(Vector3D left, Vector3D right)
        {
            return new Vector3D(left.x - right.x, left.y - right.y, left.z - right.z);
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>A vector facing in the opposite direction.</returns>
        public static Vector3D operator -(Vector3D value)
        {
            return new Vector3D(-value.x, -value.y, -value.z);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector3D operator *(double scalar, Vector3D value)
        {
            return new Vector3D(value.x * scalar, value.y * scalar, value.z * scalar);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector3D operator *(Vector3D value, double scalar)
        {
            return new Vector3D(value.x * scalar, value.y * scalar, value.z * scalar);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector3D operator /(Vector3D value, double scalar)
        {
            return new Vector3D(value.x / scalar, value.y / scalar, value.z / scalar);
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Vector3D left, Vector3D right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Vector3D left, Vector3D right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SlimMath.Vector3D"/> to <see cref="SlimMath.Vector2d"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector2D(Vector3D value)
        {
            return new Vector2D(value.x, value.y);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SlimMath.Vector3D"/> to <see cref="SlimMath.Vector4d"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector4D(Vector3D value)
        {
            return new Vector4D(value, 0.0f);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2}", x, y, z);
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

            return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2}", x.ToString(format, CultureInfo.CurrentCulture),
                y.ToString(format, CultureInfo.CurrentCulture), z.ToString(format, CultureInfo.CurrentCulture));
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
            return string.Format(formatProvider, "X:{0} Y:{1} Z:{2}", x, y, z);
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

            return string.Format(formatProvider, "X:{0} Y:{1} Z:{2}", x.ToString(format, formatProvider),
                y.ToString(format, formatProvider), z.ToString(format, formatProvider));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return x.GetHashCode() + y.GetHashCode() + z.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="SlimMath.Vector3D"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="SlimMath.Vector3D"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="SlimMath.Vector3D"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Vector3D other)
        {
            return (this.x == other.x) && (this.y == other.y) && (this.z == other.z);
        }

        /// <summary>
        /// Determines whether the specified <see cref="SlimMath.Vector3D"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="SlimMath.Vector3D"/> to compare with this instance.</param>
        /// <param name="epsilon">The amount of error allowed.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="SlimMath.Vector3D"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Vector3D other, double epsilon)
        {
            return ((double)Math.Abs(other.x - x) < epsilon &&
                (double)Math.Abs(other.y - y) < epsilon &&
                (double)Math.Abs(other.z - z) < epsilon);
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

            return Equals((Vector3D)obj);
        }

#if SlimDX1xInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimMath.Vector3"/> to <see cref="SlimDX.Vector3"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SlimDX.Vector3(Vector3 value)
        {
            return new SlimDX.Vector3(value.X, value.Y, value.Z);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimDX.Vector3"/> to <see cref="SlimMath.Vector3"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Vector3(SlimDX.Vector3 value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }
#endif

#if WPFInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimMath.Vector3"/> to <see cref="System.Windows.Media.Media3D.Vector3D"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator System.Windows.Media.Media3D.Vector3D(Vector3 value)
        {
            return new System.Windows.Media.Media3D.Vector3D(value.X, value.Y, value.Z);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Windows.Media.Media3D.Vector3D"/> to <see cref="SlimMath.Vector3"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector3(System.Windows.Media.Media3D.Vector3D value)
        {
            return new Vector3((double)value.X, (double)value.Y, (double)value.Z);
        }
#endif

#if XnaInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimMath.Vector3"/> to <see cref="Microsoft.Xna.Framework.Vector3"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Microsoft.Xna.Framework.Vector3(Vector3 value)
        {
            return new Microsoft.Xna.Framework.Vector3(value.X, value.Y, value.Z);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Microsoft.Xna.Framework.Vector3"/> to <see cref="SlimMath.Vector3"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Vector3(Microsoft.Xna.Framework.Vector3 value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }
#endif
    }
}
