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
    /// Represents a two dimensional mathematical vector.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    [TypeConverter(typeof(GraphicResearchHuiZhao.Vector2Converter))]
    public struct Vector2D : IEquatable<Vector2D>, IFormattable
    {
        /// <summary>
        /// The size of the <see cref="SlimMath.Vector2d"/> type, in bytes.
        /// </summary>
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(Vector2D));

        /// <summary>
        /// A <see cref="SlimMath.Vector2d"/> with all of its components set to zero.
        /// </summary>
        public static readonly Vector2D Zero = new Vector2D();

        /// <summary>
        /// The X unit <see cref="SlimMath.Vector2d"/> (1, 0).
        /// </summary>
        public static readonly Vector2D UnitX = new Vector2D(1.0f, 0.0f);

        /// <summary>
        /// The Y unit <see cref="SlimMath.Vector2d"/> (0, 1).
        /// </summary>
        public static readonly Vector2D UnitY = new Vector2D(0.0f, 1.0f);

        /// <summary>
        /// A <see cref="SlimMath.Vector2d"/> with all of its components set to one.
        /// </summary>
        public static readonly Vector2D One = new Vector2D(1.0f, 1.0f);

        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public double x;

        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public double y;

        public static Vector2D MinValue = new Vector2D(double.MinValue, double.MinValue);
        public static Vector2D MaxValue = new Vector2D(double.MaxValue, double.MaxValue);

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Vector2d"/> struct.
        /// </summary>
        /// <param name="value">The value that will be assigned to all components.</param>
        public Vector2D(double value)
        {
            x = value;
            y = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Vector2d"/> struct.
        /// </summary>
        /// <param name="x">Initial value for the X component of the vector.</param>
        /// <param name="y">Initial value for the Y component of the vector.</param>
        public Vector2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Vector2d"/> struct.
        /// </summary>
        /// <param name="values">The values to assign to the X and Y components of the vector. This must be an array with two elements.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than two elements.</exception>
        public Vector2D(double[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length != 2)
                throw new ArgumentOutOfRangeException("values", "There must be two and only two input values for Vector2.");

            x = values[0];
            y = values[1];
        }

        public Vector2D(double[] arr, int index)
        {
            x = arr[index];
            y = arr[index + 1];
        }

        /// <summary>
        /// Gets a value indicting whether this instance is normalized.
        /// </summary>
        public bool IsNormalized
        {
            get { return Math.Abs((x * x) + (y * y) - 1f) < Utilities.ZeroTolerance; }
        }

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        /// <remarks>
        /// <see cref="SlimMath.Vector2d.LengthSquared"/> may be preferred when only the relative length is needed
        /// and speed is of the essence.
        /// </remarks>
        public double Length()
        {
            return (double)Math.Sqrt((x * x) + (y * y));
        }

        /// <summary>
        /// Calculates the squared length of the vector.
        /// </summary>
        /// <remarks>
        /// This property may be preferred to <see cref="SlimMath.Vector2d.Length"/> when only a relative length is needed
        /// and speed is of the essence.
        /// </remarks>
        public double LengthSquared
        {
            get { return (x * x) + (y * y); }
        }

        /// <summary>
        /// Gets or sets the component at the specified index.
        /// </summary>
        /// <value>The value of the X or Y component, depending on the index.</value>
        /// <param name="index">The index of the component to access. Use 0 for the X component and 1 for the Y component.</param>
        /// <returns>The value of the component at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="index"/> is out of the range [0, 1].</exception>
        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                }

                throw new ArgumentOutOfRangeException("index", "Indices for Vector2 run from 0 to 1, inclusive.");
            }

            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    default: throw new ArgumentOutOfRangeException("index", "Indices for Vector2 run from 0 to 1, inclusive.");
                }
            }
        }

        /// <summary>
        /// Converts the vector into a unit vector.
        /// </summary>
        //public void Normalize()
        //{
        //    double length = Length;
        //    if (length > Utilities.ZeroTolerance)
        //    {
        //        double inv = 1.0f / length;
        //        x *= inv;
        //        y *= inv;
        //    }
        //}

        public Vector2D Normalize()
        {
            Vector2D v = new Vector2D(this.x, this.y);

            double length = Length();
            if (length > Utilities.ZeroTolerance)
            {
                double inv = 1.0f / length;
                v.x *= inv;
                v.y *= inv;
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
        }

        /// <summary>
        /// Takes the absolute value of each component.
        /// </summary>
        public void Abs()
        {
            this.x = Math.Abs(x);
            this.y = Math.Abs(y);
        }

        /// <summary>
        /// Creates an array containing the elements of the vector.
        /// </summary>
        /// <returns>A two-element array containing the components of the vector.</returns>
        public double[] ToArray()
        {
            return new double[] { x, y };
        }

        #region Transcendentals
        /// <summary>
        /// Takes the square root of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the square root of.</param>
        /// <param name="result">When the method completes, contains a vector that is the square root of the input vector.</param>
        public static void Sqrt(ref Vector2D value, out Vector2D result)
        {
            result.x = (double)Math.Sqrt(value.x);
            result.y = (double)Math.Sqrt(value.y);
        }

        /// <summary>
        /// Takes the square root of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the square root of.</param>
        /// <returns>A vector that is the square root of the input vector.</returns>
        public static Vector2D Sqrt(Vector2D value)
        {
            Vector2D temp;
            Sqrt(ref value, out temp);
            return temp;
        }

        /// <summary>
        /// Takes the reciprocal of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the reciprocal of.</param>
        /// <param name="result">When the method completes, contains a vector that is the reciprocal of the input vector.</param>
        public static void Reciprocal(ref Vector2D value, out Vector2D result)
        {
            result.x = 1.0f / value.x;
            result.y = 1.0f / value.y;
        }

        /// <summary>
        /// Takes the reciprocal of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the reciprocal of.</param>
        /// <returns>A vector that is the reciMaxprocal of the input vector.</returns>
        public static Vector2D Reciprocal(Vector2D value)
        {
            Vector2D temp;
            Reciprocal(ref value, out temp);
            return temp;
        }

        /// <summary>
        /// Takes the square root of each component in the vector and than takes the reciprocal of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the square root and recpirocal of.</param>
        /// <param name="result">When the method completes, contains a vector that is the square root and reciprocal of the input vector.</param>
        public static void ReciprocalSqrt(ref Vector2D value, out Vector2D result)
        {
            result.x = 1.0f / (double)Math.Sqrt(value.x);
            result.y = 1.0f / (double)Math.Sqrt(value.y);
        }

        /// <summary>
        /// Takes the square root of each component in the vector and than takes the reciprocal of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the square root and recpirocal of.</param>
        /// <returns>A vector that is the square root and reciprocal of the input vector.</returns>
        public static Vector2D ReciprocalSqrt(Vector2D value)
        {
            Vector2D temp;
            ReciprocalSqrt(ref value, out temp);
            return temp;
        }

        /// <summary>
        /// Takes e raised to the component in the vector.
        /// </summary>
        /// <param name="value">The value to take e raised to each component of.</param>
        /// <param name="result">When the method completes, contains a vector that has e raised to each of the components in the input vector.</param>
        public static void Exp(ref Vector2D value, out Vector2D result)
        {
            result.x = (double)Math.Exp(value.x);
            result.y = (double)Math.Exp(value.y);
        }

        /// <summary>
        /// Takes e raised to the component in the vector.
        /// </summary>
        /// <param name="value">The value to take e raised to each component of.</param>
        /// <returns>A vector that has e raised to each of the components in the input vector.</returns>
        public static Vector2D Exp(Vector2D value)
        {
            Vector2D temp;
            Exp(ref value, out temp);
            return temp;
        }

        /// <summary>
        /// Takes the sine and than the cosine of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the sine and cosine of.</param>
        /// <param name="sinResult">When the method completes, contains the sine of each component in the input vector.</param>
        /// <param name="cosResult">When the method completes, contains the cpsome pf each component in the input vector.</param>
        public static void SinCos(ref Vector2D value, out Vector2D sinResult, out Vector2D cosResult)
        {
            sinResult.x = (double)Math.Sin(value.x);
            sinResult.y = (double)Math.Sin(value.y);

            cosResult.x = (double)Math.Cos(value.x);
            cosResult.y = (double)Math.Cos(value.y);
        }

        /// <summary>
        /// Takes the sine of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the sine of.</param>
        /// <param name="result">When the method completes, a vector that contains the sine of each component in the input vector.</param>
        public static void Sin(ref Vector2D value, out Vector2D result)
        {
            result.x = (double)Math.Sin(value.x);
            result.y = (double)Math.Sin(value.y);
        }

        /// <summary>
        /// Takes the sine of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the sine of.</param>
        /// <returns>A vector that contains the sine of each component in the input vector.</returns>
        public static Vector2D Sin(Vector2D value)
        {
            Vector2D temp;
            Sin(ref value, out temp);
            return temp;
        }

        /// <summary>
        /// Takes the cosine of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the cosine of.</param>
        /// <param name="result">When the method completes, contains a vector that contains the cosine of each component in the input vector.</param>
        public static void Cos(ref Vector2D value, out Vector2D result)
        {
            result.x = (double)Math.Cos(value.x);
            result.y = (double)Math.Cos(value.y);
        }

        /// <summary>
        /// Takes the cosine of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the cosine of.</param>
        /// <returns>A vector that contains the cosine of each component in the input vector.</returns>
        public static Vector2D Cos(Vector2D value)
        {
            Vector2D temp;
            Cos(ref value, out temp);
            return temp;
        }

        /// <summary>
        /// Takes the tangent of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the tangent of.</param>
        /// <param name="result">When the method completes, contains a vector that contains the tangent of each component in the input vector.</param>
        public static void Tan(ref Vector2D value, out Vector2D result)
        {
            result.x = (double)Math.Tan(value.x);
            result.y = (double)Math.Tan(value.y);
        }

        /// <summary>
        /// Takes the tangent of each component in the vector.
        /// </summary>
        /// <param name="value">The vector to take the tangent of.</param>
        /// <returns>A vector that contains the tangent of each component in the input vector.</returns>
        public static Vector2D Tan(Vector2D value)
        {
            Vector2D temp;
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
        public static void Add(ref Vector2D left, ref Vector2D right, out Vector2D result)
        {
            result = new Vector2D(left.x + right.x, left.y + right.y);
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The sum of the two vectors.</returns>
        public static Vector2D Add(Vector2D left, Vector2D right)
        {
            return new Vector2D(left.x + right.x, left.y + right.y);
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <param name="result">When the method completes, contains the difference of the two vectors.</param>
        public static void Subtract(ref Vector2D left, ref Vector2D right, out Vector2D result)
        {
            result = new Vector2D(left.x - right.x, left.y - right.y);
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <returns>The difference of the two vectors.</returns>
        public static Vector2D Subtract(Vector2D left, Vector2D right)
        {
            return new Vector2D(left.x - right.x, left.y - right.y);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <param name="result">When the method completes, contains the scaled vector.</param>
        public static void Multiply(ref Vector2D value, double scalar, out Vector2D result)
        {
            result = new Vector2D(value.x * scalar, value.y * scalar);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector2D Multiply(Vector2D value, double scalar)
        {
            return new Vector2D(value.x * scalar, value.y * scalar);
        }

        /// <summary>
        /// Modulates a vector with another by performing component-wise multiplication.
        /// </summary>
        /// <param name="left">The first vector to modulate.</param>
        /// <param name="right">The second vector to modulate.</param>
        /// <param name="result">When the method completes, contains the modulated vector.</param>
        public static void Modulate(ref Vector2D left, ref Vector2D right, out Vector2D result)
        {
            result = new Vector2D(left.x * right.x, left.y * right.y);
        }

        /// <summary>
        /// Modulates a vector with another by performing component-wise multiplication.
        /// </summary>
        /// <param name="left">The first vector to modulate.</param>
        /// <param name="right">The second vector to modulate.</param>
        /// <returns>The modulated vector.</returns>
        public static Vector2D Modulate(Vector2D left, Vector2D right)
        {
            return new Vector2D(left.x * right.x, left.y * right.y);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <param name="result">When the method completes, contains the scaled vector.</param>
        public static void Divide(ref Vector2D value, double scalar, out Vector2D result)
        {
            result = new Vector2D(value.x / scalar, value.y / scalar);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector2D Divide(Vector2D value, double scalar)
        {
            return new Vector2D(value.x / scalar, value.y / scalar);
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <param name="result">When the method completes, contains a vector facing in the opposite direction.</param>
        public static void Negate(ref Vector2D value, out Vector2D result)
        {
            result = new Vector2D(-value.x, -value.y);
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>A vector facing in the opposite direction.</returns>
        public static Vector2D Negate(Vector2D value)
        {
            return new Vector2D(-value.x, -value.y);
        }

        /// <summary>
        /// Takes the absolute value of each component.
        /// </summary>
        /// <param name="value">The vector to take the absolute value of.</param>
        /// <param name="result">When the method completes, contains a vector that has all positive components.</param>
        public static void Abs(ref Vector2D value, out Vector2D result)
        {
            result = new Vector2D(Math.Abs(value.x), Math.Abs(value.y));
        }

        /// <summary>
        /// Takes the absolute value of each component.
        /// </summary>
        /// <param name="value">The vector to take the absolute value of.</param>
        /// <returns>A vector that has all positive components.</returns>
        public static Vector2D Abs(Vector2D value)
        {
            return new Vector2D(Math.Abs(value.x), Math.Abs(value.y));
        }

        /// <summary>
        /// Returns a <see cref="SlimMath.Vector2d"/> containing the 2D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 2D triangle.
        /// </summary>
        /// <param name="value1">A <see cref="SlimMath.Vector2d"/> containing the 2D Cartesian coordinates of vertex 1 of the triangle.</param>
        /// <param name="value2">A <see cref="SlimMath.Vector2d"/> containing the 2D Cartesian coordinates of vertex 2 of the triangle.</param>
        /// <param name="value3">A <see cref="SlimMath.Vector2d"/> containing the 2D Cartesian coordinates of vertex 3 of the triangle.</param>
        /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="value2"/>).</param>
        /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="value3"/>).</param>
        /// <param name="result">When the method completes, contains the 2D Cartesian coordinates of the specified point.</param>
        public static void Barycentric(ref Vector2D value1, ref Vector2D value2, ref Vector2D value3, double amount1, double amount2, out Vector2D result)
        {
            result = new Vector2D((value1.x + (amount1 * (value2.x - value1.x))) + (amount2 * (value3.x - value1.x)),
                (value1.y + (amount1 * (value2.y - value1.y))) + (amount2 * (value3.y - value1.y)));
        }

        /// <summary>
        /// Returns a <see cref="SlimMath.Vector2d"/> containing the 2D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 2D triangle.
        /// </summary>
        /// <param name="value1">A <see cref="SlimMath.Vector2d"/> containing the 2D Cartesian coordinates of vertex 1 of the triangle.</param>
        /// <param name="value2">A <see cref="SlimMath.Vector2d"/> containing the 2D Cartesian coordinates of vertex 2 of the triangle.</param>
        /// <param name="value3">A <see cref="SlimMath.Vector2d"/> containing the 2D Cartesian coordinates of vertex 3 of the triangle.</param>
        /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="value2"/>).</param>
        /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="value3"/>).</param>
        /// <returns>A new <see cref="SlimMath.Vector2d"/> containing the 2D Cartesian coordinates of the specified point.</returns>
        public static Vector2D Barycentric(Vector2D value1, Vector2D value2, Vector2D value3, double amount1, double amount2)
        {
            Vector2D result;
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
        public static void Clamp(ref Vector2D value, ref Vector2D min, ref Vector2D max, out Vector2D result)
        {
            double x = value.x;
            x = (x > max.x) ? max.x : x;
            x = (x < min.x) ? min.x : x;

            double y = value.y;
            y = (y > max.y) ? max.y : y;
            y = (y < min.y) ? min.y : y;

            result = new Vector2D(x, y);
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static Vector2D Clamp(Vector2D value, Vector2D min, Vector2D max)
        {
            Vector2D result;
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
        /// <see cref="SlimMath.Vector2d.DistanceSquared(ref Vector2d, ref Vector2d, out double)"/> may be preferred when only the relative distance is needed
        /// and speed is of the essence.
        /// </remarks>
        public static void Distance(ref Vector2D value1, ref Vector2D value2, out double result)
        {
            double x = value1.x - value2.x;
            double y = value1.y - value2.y;

            result = (double)Math.Sqrt((x * x) + (y * y));
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        /// <remarks>
        /// <see cref="SlimMath.Vector2d.DistanceSquared(Vector2d, Vector2d)"/> may be preferred when only the relative distance is needed
        /// and speed is of the essence.
        /// </remarks>
        public static double Distance(Vector2D value1, Vector2D value2)
        {
            double x = value1.x - value2.x;
            double y = value1.y - value2.y;

            return (double)Math.Sqrt((x * x) + (y * y));
        }

        /// <summary>
        /// Calculates the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector</param>
        /// <param name="result">When the method completes, contains the squared distance between the two vectors.</param>
        /// <remarks>Distance squared is the value before taking the square root. 
        /// Distance squared can often be used in place of distance if relative comparisons are being made. 
        /// For example, consider three points A, B, and C. To determine whether B or C is further from A, 
        /// compare the distance between A and B to the distance between A and C. Calculating the two distances 
        /// involves two square roots, which are computationally expensive. However, using distance squared 
        /// provides the same information and avoids calculating two square roots.
        /// </remarks>
        public static void DistanceSquared(ref Vector2D value1, ref Vector2D value2, out double result)
        {
            double x = value1.x - value2.x;
            double y = value1.y - value2.y;

            result = (x * x) + (y * y);
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
        public static double DistanceSquared(Vector2D value1, Vector2D value2)
        {
            double x = value1.x - value2.x;
            double y = value1.y - value2.y;

            return (x * x) + (y * y);
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="left">First source vector.</param>
        /// <param name="right">Second source vector.</param>
        /// <param name="result">When the method completes, contains the dot product of the two vectors.</param>
        public static void Dot(ref Vector2D left, ref Vector2D right, out double result)
        {
            result = (left.x * right.x) + (left.y * right.y);
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="left">First source vector.</param>
        /// <param name="right">Second source vector.</param>
        /// <returns>The dot product of the two vectors.</returns>
        public static double Dot(Vector2D left, Vector2D right)
        {
            return (left.x * right.x) + (left.y * right.y);
        }

        public double Dot(Vector2D right)
        {
            return Dot(this, right);
        }

        /// <summary>
        /// Calculates a vector that is perpendicular to the given vector.
        /// </summary>
        /// <param name="value">The vector to base the perpendicular vector on.</param>
        /// <param name="result">When the method completes, contains the perpendicular vector.</param>
        /// <remarks>
        /// This method finds the perpendicular vector using a 90 degree counterclockwise rotation.
        /// </remarks>
        public static void Perp(ref Vector2D value, out Vector2D result)
        {
            result.x = -value.y;
            result.y = value.x;
        }

        /// <summary>
        /// Calculates a vector that is perpendicular to the given vector.
        /// </summary>
        /// <param name="value">The vector to base the perpendicular vector on.</param>
        /// <returns>The perpendicular vector.</returns>
        /// <remarks>
        /// This method finds the perpendicular vector using a 90 degree counterclockwise rotation.
        /// </remarks>
        public static Vector2D Perp(Vector2D value)
        {
            Vector2D result;
            Perp(ref value, out result);
            return result;
        }

        /// <summary>
        /// Calculates the perp dot product.
        /// </summary>
        /// <param name="left">First source vector.</param>
        /// <param name="right">Second source vector.</param>
        /// <param name="result">When the method completes, contains the perp dot product of the two vectors.</param>
        /// <remarks>
        /// The perp dot product is defined as taking the dot product of the perpendicular vector
        /// of the left vector with the right vector.
        /// </remarks>
        public static void PerpDot(ref Vector2D left, ref Vector2D right, out double result)
        {
            Vector2D temp;
            Perp(ref left, out temp);

            Dot(ref temp, ref right, out result);
        }

        /// <summary>
        /// Calculates the perp dot product.
        /// </summary>
        /// <param name="left">First source vector.</param>
        /// <param name="right">Second source vector.</param>
        /// <returns>The perp dot product of the two vectors.</returns>
        /// <remarks>
        /// The perp dot product is defined as taking the dot product of the perpendicular vector
        /// of the left vector with the right vector.
        /// </remarks>
        public static double PerpDot(Vector2D left, Vector2D right)
        {
            double result;
            PerpDot(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Converts the vector into a unit vector.
        /// </summary>
        /// <param name="value">The vector to normalize.</param>
        /// <param name="result">When the method completes, contains the normalized vector.</param>
        public static void Normalize(ref Vector2D value, out Vector2D result)
        {
            result = value;
            result.Normalize();
        }

        /// <summary>
        /// Converts the vector into a unit vector.
        /// </summary>
        /// <param name="value">The vector to normalize.</param>
        /// <returns>The normalized vector.</returns>
        public static Vector2D Normalize(Vector2D value)
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
        public static void Lerp(ref Vector2D start, ref Vector2D end, double amount, out Vector2D result)
        {
            result.x = start.x + ((end.x - start.x) * amount);
            result.y = start.y + ((end.y - start.y) * amount);
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
        public static Vector2D Lerp(Vector2D start, Vector2D end, double amount)
        {
            Vector2D result;
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
        public static void SmoothStep(ref Vector2D start, ref Vector2D end, double amount, out Vector2D result)
        {
            amount = (amount > 1.0f) ? 1.0f : ((amount < 0.0f) ? 0.0f : amount);
            amount = (amount * amount) * (3.0f - (2.0f * amount));

            result.x = start.x + ((end.x - start.x) * amount);
            result.y = start.y + ((end.y - start.y) * amount);
        }

        /// <summary>
        /// Performs a cubic interpolation between two vectors.
        /// </summary>
        /// <param name="start">Start vector.</param>
        /// <param name="end">End vector.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <returns>The cubic interpolation of the two vectors.</returns>
        public static Vector2D SmoothStep(Vector2D start, Vector2D end, double amount)
        {
            Vector2D result;
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
        public static void Hermite(ref Vector2D value1, ref Vector2D tangent1, ref Vector2D value2, ref Vector2D tangent2, double amount, out Vector2D result)
        {
            double squared = amount * amount;
            double cubed = amount * squared;
            double part1 = ((2.0f * cubed) - (3.0f * squared)) + 1.0f;
            double part2 = (-2.0f * cubed) + (3.0f * squared);
            double part3 = (cubed - (2.0f * squared)) + amount;
            double part4 = cubed - squared;

            result.x = (((value1.x * part1) + (value2.x * part2)) + (tangent1.x * part3)) + (tangent2.x * part4);
            result.y = (((value1.y * part1) + (value2.y * part2)) + (tangent1.y * part3)) + (tangent2.y * part4);
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
        public static Vector2D Hermite(Vector2D value1, Vector2D tangent1, Vector2D value2, Vector2D tangent2, double amount)
        {
            Vector2D result;
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
        public static void CatmullRom(ref Vector2D value1, ref Vector2D value2, ref Vector2D value3, ref Vector2D value4, double amount, out Vector2D result)
        {
            double squared = amount * amount;
            double cubed = amount * squared;

            result.x = 0.5f * ((((2.0f * value2.x) + ((-value1.x + value3.x) * amount)) +
                (((((2.0f * value1.x) - (5.0f * value2.x)) + (4.0f * value3.x)) - value4.x) * squared)) +
                ((((-value1.x + (3.0f * value2.x)) - (3.0f * value3.x)) + value4.x) * cubed));

            result.y = 0.5f * ((((2.0f * value2.y) + ((-value1.y + value3.y) * amount)) +
                (((((2.0f * value1.y) - (5.0f * value2.y)) + (4.0f * value3.y)) - value4.y) * squared)) +
                ((((-value1.y + (3.0f * value2.y)) - (3.0f * value3.y)) + value4.y) * cubed));
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
        public static Vector2D CatmullRom(Vector2D value1, Vector2D value2, Vector2D value3, Vector2D value4, double amount)
        {
            Vector2D result;
            CatmullRom(ref value1, ref value2, ref value3, ref value4, amount, out result);
            return result;
        }

        /// <summary>
        /// Returns a vector containing the largest components of the specified vectors.
        /// </summary>
        /// <param name="value1">The first source vector.</param>
        /// <param name="value2">The second source vector.</param>
        /// <param name="result">When the method completes, contains an new vector composed of the largest components of the source vectors.</param>
        public static void Max(ref Vector2D value1, ref Vector2D value2, out Vector2D result)
        {
            result.x = (value1.x > value2.x) ? value1.x : value2.x;
            result.y = (value1.y > value2.y) ? value1.y : value2.y;
        }

        /// <summary>
        /// Returns a vector containing the largest components of the specified vectors.
        /// </summary>
        /// <param name="value1">The first source vector.</param>
        /// <param name="value2">The second source vector.</param>
        /// <returns>A vector containing the largest components of the source vectors.</returns>
        public static Vector2D Max(Vector2D value1, Vector2D value2)
        {
            Vector2D result;
            Max(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Returns a vector containing the smallest components of the specified vectors.
        /// </summary>
        /// <param name="value1">The first source vector.</param>
        /// <param name="value2">The second source vector.</param>
        /// <param name="result">When the method completes, contains an new vector composed of the smallest components of the source vectors.</param>
        public static void Min(ref Vector2D value1, ref Vector2D value2, out Vector2D result)
        {
            result.x = (value1.x < value2.x) ? value1.x : value2.x;
            result.y = (value1.y < value2.y) ? value1.y : value2.y;
        }

        /// <summary>
        /// Returns a vector containing the smallest components of the specified vectors.
        /// </summary>
        /// <param name="value1">The first source vector.</param>
        /// <param name="value2">The second source vector.</param>
        /// <returns>A vector containing the smallest components of the source vectors.</returns>
        public static Vector2D Min(Vector2D value1, Vector2D value2)
        {
            Vector2D result;
            Min(ref value1, ref value2, out result);
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
        public static void Reflect(ref Vector2D vector, ref Vector2D normal, out Vector2D result)
        {
            double dot = (vector.x * normal.x) + (vector.y * normal.y);

            result.x = vector.x - ((2.0f * dot) * normal.x);
            result.y = vector.y - ((2.0f * dot) * normal.y);
        }

        /// <summary>
        /// Returns the reflection of a vector off a surface that has the specified normal. 
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="normal">Normal of the surface.</param>
        /// <returns>The reflected vector.</returns>
        /// <remarks>Reflect only gives the direction of a reflection off a surface, it does not determine 
        /// whether the original vector was close enough to the surface to hit it.</remarks>
        public static Vector2D Reflect(Vector2D vector, Vector2D normal)
        {
            Vector2D result;
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
        public static void Refract(ref Vector2D vector, ref Vector2D normal, double index, out Vector2D result)
        {
            double cos1;
            Dot(ref vector, ref normal, out cos1);

            double radicand = 1.0f - (index * index) * (1.0f - (cos1 * cos1));

            if (radicand < 0.0f)
            {
                result = Vector2D.Zero;
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
        public static Vector2D Refract(Vector2D vector, Vector2D normal, double index)
        {
            Vector2D result;
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
        public static void Orthogonalize(Vector2D[] destination, params Vector2D[] source)
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
                Vector2D newvector = source[i];

                for (int r = 0; r < i; ++r)
                {
                    newvector -= (Vector2D.Dot(destination[r], newvector) / Vector2D.Dot(destination[r], destination[r])) * destination[r];
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
        public static void Orthonormalize(Vector2D[] destination, params Vector2D[] source)
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
                Vector2D newvector = source[i];

                for (int r = 0; r < i; ++r)
                {
                    newvector -= Vector2D.Dot(destination[r], newvector) * destination[r];
                }

                newvector.Normalize();
                destination[i] = newvector;
            }
        }

        /// <summary>
        /// Transforms a 2D vector by the given <see cref="SlimMath.Quaternion"/> rotation.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="rotation">The <see cref="SlimMath.Quaternion"/> rotation to apply.</param>
        /// <param name="result">When the method completes, contains the transformed <see cref="SlimMath.Vector4d"/>.</param>
        public static void Transform(ref Vector2D vector, ref Quaternion rotation, out Vector2D result)
        {
            double x = rotation.x + rotation.x;
            double y = rotation.y + rotation.y;
            double z = rotation.z + rotation.z;
            double wz = rotation.w * z;
            double xx = rotation.x * x;
            double xy = rotation.x * y;
            double yy = rotation.y * y;
            double zz = rotation.z * z;

            double num1 = (1.0f - yy - zz);
            double num2 = (xy - wz);
            double num3 = (xy + wz);
            double num4 = (1.0f - xx - zz);

            result = new Vector2D(
                (vector.x * num1) + (vector.y * num2),
                (vector.x * num3) + (vector.y * num4));
        }

        /// <summary>
        /// Transforms a 2D vector by the given <see cref="SlimMath.Quaternion"/> rotation.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="rotation">The <see cref="SlimMath.Quaternion"/> rotation to apply.</param>
        /// <returns>The transformed <see cref="SlimMath.Vector4d"/>.</returns>
        public static Vector2D Transform(Vector2D vector, Quaternion rotation)
        {
            Vector2D result;
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
        public static void Transform(Vector2D[] source, ref Quaternion rotation, Vector2D[] destination)
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
            double wz = rotation.w * z;
            double xx = rotation.x * x;
            double xy = rotation.x * y;
            double yy = rotation.y * y;
            double zz = rotation.z * z;

            double num1 = (1.0f - yy - zz);
            double num2 = (xy - wz);
            double num3 = (xy + wz);
            double num4 = (1.0f - xx - zz);

            for (int i = 0; i < source.Length; ++i)
            {
                destination[i] = new Vector2D(
                    (source[i].x * num1) + (source[i].y * num2),
                    (source[i].x * num3) + (source[i].y * num4));
            }
        }

        /// <summary>
        /// Transforms a 2D vector by the given <see cref="SlimMath.Matrix4d"/>.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="transform">The transformation <see cref="SlimMath.Matrix4d"/>.</param>
        /// <param name="result">When the method completes, contains the transformed <see cref="SlimMath.Vector4d"/>.</param>
        public static void Transform(ref Vector2D vector, ref Matrix4D transform, out Vector4D result)
        {
            result = new Vector4D(
                (vector.x * transform.M11) + (vector.y * transform.M21) + transform.M41,
                (vector.x * transform.M12) + (vector.y * transform.M22) + transform.M42,
                (vector.x * transform.M13) + (vector.y * transform.M23) + transform.M43,
                (vector.x * transform.M14) + (vector.y * transform.M24) + transform.M44);
        }

        /// <summary>
        /// Transforms a 2D vector by the given <see cref="SlimMath.Matrix4d"/>.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="transform">The transformation <see cref="SlimMath.Matrix4d"/>.</param>
        /// <returns>The transformed <see cref="SlimMath.Vector4d"/>.</returns>
        public static Vector4D Transform(Vector2D vector, Matrix4D transform)
        {
            Vector4D result;
            Transform(ref vector, ref transform, out result);
            return result;
        }

        /// <summary>
        /// Transforms an array of 2D vectors by the given <see cref="SlimMath.Matrix4d"/>.
        /// </summary>
        /// <param name="source">The array of vectors to transform.</param>
        /// <param name="transform">The transformation <see cref="SlimMath.Matrix4d"/>.</param>
        /// <param name="destination">The array for which the transformed vectors are stored.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="destination"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="destination"/> is shorter in length than <paramref name="source"/>.</exception>
        public static void Transform(Vector2D[] source, ref Matrix4D transform, Vector4D[] destination)
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
        public static void TransformCoordinate(ref Vector2D coordinate, ref Matrix4D transform, out Vector2D result)
        {
            Vector4D vector = new Vector4D();
            vector.x = (coordinate.x * transform.M11) + (coordinate.y * transform.M21) + transform.M41;
            vector.y = (coordinate.x * transform.M12) + (coordinate.y * transform.M22) + transform.M42;
            vector.z = (coordinate.x * transform.M13) + (coordinate.y * transform.M23) + transform.M43;
            vector.w = 1f / ((coordinate.x * transform.M14) + (coordinate.y * transform.M24) + transform.M44);

            result = new Vector2D(vector.x * vector.w, vector.y * vector.w);
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
        public static Vector2D TransformCoordinate(Vector2D coordinate, Matrix4D transform)
        {
            Vector2D result;
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
        public static void TransformCoordinate(Vector2D[] source, ref Matrix4D transform, Vector2D[] destination)
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
        public static void TransformNormal(ref Vector2D normal, ref Matrix4D transform, out Vector2D result)
        {
            result = new Vector2D(
                (normal.x * transform.M11) + (normal.y * transform.M21),
                (normal.x * transform.M12) + (normal.y * transform.M22));
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
        public static Vector2D TransformNormal(Vector2D normal, Matrix4D transform)
        {
            Vector2D result;
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
        public static void TransformNormal(Vector2D[] source, ref Matrix4D transform, Vector2D[] destination)
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
        public static Vector2D operator +(Vector2D left, Vector2D right)
        {
            return new Vector2D(left.x + right.x, left.y + right.y);
        }

        public static double operator *(Vector2D left, Vector2D right)
        {
            return left.x * right.x + left.y * right.y;
        }


        /// <summary>
        /// Assert a vector (return it unchanged).
        /// </summary>
        /// <param name="value">The vector to assert (unchange).</param>
        /// <returns>The asserted (unchanged) vector.</returns>
        public static Vector2D operator +(Vector2D value)
        {
            return value;
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <returns>The difference of the two vectors.</returns>
        public static Vector2D operator -(Vector2D left, Vector2D right)
        {
            return new Vector2D(left.x - right.x, left.y - right.y);
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>A vector facing in the opposite direction.</returns>
        public static Vector2D operator -(Vector2D value)
        {
            return new Vector2D(-value.x, -value.y);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector2D operator *(double scalar, Vector2D value)
        {
            return new Vector2D(value.x * scalar, value.y * scalar);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector2D operator *(Vector2D value, double scalar)
        {
            return new Vector2D(value.x * scalar, value.y * scalar);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scalar">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector2D operator /(Vector2D value, double scalar)
        {
            return new Vector2D(value.x / scalar, value.y / scalar);
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Vector2D left, Vector2D right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Vector2D left, Vector2D right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SlimMath.Vector2d"/> to <see cref="SlimMath.Vector3D"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector3D(Vector2D value)
        {
            return new Vector3D(value, 0.0f);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SlimMath.Vector2d"/> to <see cref="SlimMath.Vector4d"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector4D(Vector2D value)
        {
            return new Vector4D(value, 0.0f, 0.0f);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1}", x, y);
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

            return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1}", x.ToString(format, CultureInfo.CurrentCulture), y.ToString(format, CultureInfo.CurrentCulture));
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
            return string.Format(formatProvider, "X:{0} Y:{1}", x, y);
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

            return string.Format(formatProvider, "X:{0} Y:{1}", x.ToString(format, formatProvider), y.ToString(format, formatProvider));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return x.GetHashCode() + y.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="SlimMath.Vector2d"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="SlimMath.Vector2d"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="SlimMath.Vector2d"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Vector2D other)
        {
            return (this.x == other.x) && (this.y == other.y);
        }

        /// <summary>
        /// Determines whether the specified <see cref="SlimMath.Vector2d"/> is equal to this instance using an epsilon value.
        /// </summary>
        /// <param name="other">The <see cref="SlimMath.Vector2d"/> to compare with this instance.</param>
        /// <param name="epsilon">The amount of error allowed.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="SlimMath.Vector2d"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Vector2D other, double epsilon)
        {
            return ((double)Math.Abs(other.x - x) < epsilon &&
                (double)Math.Abs(other.y - y) < epsilon);
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

            return Equals((Vector2D)obj);
        }

#if SlimDX1xInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimMath.Vector2"/> to <see cref="SlimDX.Vector2"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SlimDX.Vector2(Vector2 value)
        {
            return new SlimDX.Vector2(value.X, value.Y);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimDX.Vector2"/> to <see cref="SlimMath.Vector2"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Vector2(SlimDX.Vector2 value)
        {
            return new Vector2(value.X, value.Y);
        }
#endif

#if WPFInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimMath.Vector2"/> to <see cref="System.Windows.Point"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator System.Windows.Point(Vector2 value)
        {
            return new System.Windows.Point(value.X, value.Y);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Windows.Point"/> to <see cref="SlimMath.Vector2"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector2(System.Windows.Point value)
        {
            return new Vector2((double)value.X, (double)value.Y);
        }
#endif

#if XnaInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimMath.Vector2"/> to <see cref="Microsoft.Xna.Framework.Vector2"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Microsoft.Xna.Framework.Vector2(Vector2 value)
        {
            return new Microsoft.Xna.Framework.Vector2(value.X, value.Y);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Microsoft.Xna.Framework.Vector2"/> to <see cref="SlimMath.Vector2"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Vector2(Microsoft.Xna.Framework.Vector2 value)
        {
            return new Vector2(value.X, value.Y);
        }
#endif
    }
}
