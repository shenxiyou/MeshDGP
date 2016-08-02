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
    /// Represents a color in the form of argb.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Color4 : IEquatable<Color4>, IFormattable
    {
        /// <summary>
        /// The red component of the color.
        /// </summary>
        public double R;

        /// <summary>
        /// The green component of the color.
        /// </summary>
        public double G;

        /// <summary>
        /// The blue component of the color.
        /// </summary>
        public double B;

        /// <summary>
        /// The alpha component of the color.
        /// </summary>
        public double Alpha;

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Color4"/> struct.
        /// </summary>
        /// <param name="value">The value that will be assigned to all components.</param>
        public Color4(double value)
        {
            Alpha = R = G = B = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Color4"/> struct.
        /// </summary>
        /// <param name="red">The red component of the color.</param>
        /// <param name="green">The green component of the color.</param>
        /// <param name="blue">The blue component of the color.</param>
        public Color4(double red, double green, double blue)
        {
            Alpha = 1.0f;
            R = red;
            G = green;
            B = blue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Color4"/> struct.
        /// </summary>
        /// <param name="alpha">The alpha component of the color.</param>
        /// <param name="red">The red component of the color.</param>
        /// <param name="green">The green component of the color.</param>
        /// <param name="blue">The blue component of the color.</param>
        public Color4(double red, double green, double blue,double alpha)
        {
            Alpha = alpha;
            R = red;
            G = green;
            B = blue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Color4"/> struct.
        /// </summary>
        /// <param name="value">The red, green, blue, and alpha components of the color.</param>
        public Color4(Vector4D value)
        {
            R = value.x;
            G = value.y;
            B = value.z;
            Alpha = value.w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Color4"/> struct.
        /// </summary>
        /// <param name="value">The red, green, and blue compoennts of the color.</param>
        /// <param name="alpha">The alpha component of the color.</param>
        public Color4(Vector3D value, double alpha)
        {
            R = value.x;
            G = value.y;
            B = value.z;
            Alpha = alpha;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Color4"/> struct.
        /// </summary>
        /// <param name="argb">A packed integer containing all four color components.</param>
        public Color4(int argb)
        {
            Alpha = ((argb >> 24) & 255) / 255.0f;
            R = ((argb >> 16) & 255) / 255.0f;
            G = ((argb >> 8) & 255) / 255.0f;
            B = (argb & 255) / 255.0f;
        }

        //Colors
  
        public static Color4 Pink
        {
            get { return new Color4(255, 192, 203, 0); }
        }

 


        public static Color4 Purple
        {
            get { return new Color4(128, 0, 128, 0); }
        }

        public static Color4 Blue
        {
            get { return new Color4(0, 0, 255, 0); }
        }

        public static Color4 DrakBlue
        {
            get { return new Color4(0, 0, 139, 0); }
        }

        public static Color4 SkyBlue
        {
            get { return new Color4(135, 206, 235, 0); }
        }

        public static Color4 Cyan
        {
            get { return new Color4(0, 255, 255, 0); }
        }

        public static Color4 LimeGreen
        {
            get { return new Color4(0, 255, 0, 0); }
        }

        public static Color4 Green
        {
            get { return new Color4(0, 128, 0, 0); }
        }

        public static Color4 DarkGreen
        {
            get { return new Color4(0, 100, 0, 0); }
        }

        public static Color4 Gold
        {
            get { return new Color4(255, 215, 0, 0); }
        }

        public static Color4 Yellow
        {
            get { return new Color4(255, 255, 0, 0); }
        }

        public static Color4 Orange
        {
            get { return new Color4(255, 165, 0, 0); }
        }

        public static Color4 Red
        {
            get { return new Color4(255, 0, 0, 0); }
        }

        public static Color4 Brown
        {
            get { return new Color4(165, 42, 42, 0); }
        }

        public static Color4 DarkRed
        {
            get { return new Color4(139, 0, 0, 0); }
        }

        public static Color4 Black
        {
            get { return new Color4(0, 0, 0, 0); }
        }

        public static Color4 White
        {
            get { return new Color4(255, 255, 255, 0); }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Color4"/> struct.
        /// </summary>
        /// <param name="red">The red component of the color.</param>
        /// <param name="green">The green component of the color.</param>
        /// <param name="blue">The blue component of the color.</param>
        public Color4(int red, int green, int blue)
        {
            Alpha = 1.0f;
            R = red / 255.0f;
            G = green / 255.0f;
            B = blue / 255.0f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Color4"/> struct.
        /// </summary>
        /// <param name="alpha">The alpha component of the color.</param>
        /// <param name="red">The red component of the color.</param>
        /// <param name="green">The green component of the color.</param>
        /// <param name="blue">The blue component of the color.</param>
        public Color4(int red, int green, int blue,int alpha)
        {
            Alpha = alpha / 255.0f;
            R = red / 255.0f;
            G = green / 255.0f;
            B = blue / 255.0f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.Color4"/> struct.
        /// </summary>
        /// <param name="values">The values to assign to the alpha, red, green, and blue components of the color. This must be an array with four elements.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than four elements.</exception>
        public Color4(double[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length != 4)
                throw new ArgumentOutOfRangeException("values", "There must be four and only four input values for Color4.");

            Alpha = values[0];
            R = values[1];
            G = values[2];
            B = values[3];
        }

        /// <summary>
        /// Gets or sets the component at the specified index.
        /// </summary>
        /// <value>The value of the alpha, red, green, or blue component, depending on the index.</value>
        /// <param name="index">The index of the component to access. Use 0 for the alpha component, 1 for the red component, 2 for the green component, and 3 for the blue component.</param>
        /// <returns>The value of the component at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="index"/> is out of the range [0, 3].</exception>
        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return Alpha;
                    case 1: return R;
                    case 2: return G;
                    case 3: return B;
                }

                throw new ArgumentOutOfRangeException("index", "Indices for Color4 run from 0 to 3, inclusive.");
            }

            set
            {
                switch (index)
                {
                    case 0: Alpha = value; break;
                    case 1: R = value; break;
                    case 2: G = value; break;
                    case 3: B = value; break;
                    default: throw new ArgumentOutOfRangeException("index", "Indices for Color4 run from 0 to 3, inclusive.");
                }
            }
        }

        /// <summary>
        /// Negates a color.
        /// </summary>
        public void Negate()
        {
            this.Alpha = -Alpha;
            this.R = -R;
            this.G = -G;
            this.B = -B;
        }

        /// <summary>
        /// Scales a color.
        /// </summary>
        /// <param name="scalar">The amount by which to scale.</param>
        public void Scale(double scalar)
        {
            this.Alpha *= scalar;
            this.R *= scalar;
            this.G *= scalar;
            this.B *= scalar;
        }

        /// <summary>
        /// Inverts the color (takes the complement of the color).
        /// </summary>
        public void Invert()
        {
            this.Alpha = 1.0f - Alpha;
            this.R = 1.0f - R;
            this.G = 1.0f - G;
            this.B = 1.0f - B;
        }

        /// <summary>
        /// Adjusts the contrast of a color.
        /// </summary>
        /// <param name="contrast">The amount by which to adjust the contrast.</param>
        public void AdjustContrast(double contrast)
        {
            this.R = 0.5f + contrast * (R - 0.5f);
            this.G = 0.5f + contrast * (G - 0.5f);
            this.B = 0.5f + contrast * (B - 0.5f);
        }

        /// <summary>
        /// Adjusts the saturation of a color.
        /// </summary>
        /// <param name="saturation">The amount by which to adjust the saturation.</param>
        public void AdjustSaturation(double saturation)
        {
            double grey = R * 0.2125f + G * 0.7154f + B * 0.0721f;

            this.R = grey + saturation * (R - grey);
            this.G = grey + saturation * (G - grey);
            this.B = grey + saturation * (B - grey);
        }

        /// <summary>
        /// Converts the color into a packed integer.
        /// </summary>
        /// <returns>A packed integer containing all four color components.</returns>
        public int ToArgb()
        {
            uint a = ((uint)(Alpha * 255.0f) & 0xFF);
            uint r = ((uint)(R * 255.0f) & 0xFF);
            uint g = ((uint)(G * 255.0f) & 0xFF);
            uint b = ((uint)(B * 255.0f) & 0xFF);

            uint value = b;
            value |= g << 8;
            value |= r << 16;
            value |= a << 24;

            return (int)value;
        }

        /// <summary>
        /// Converts the color into a packed integer.
        /// </summary>
        /// <returns>A packed integer containing all four color components.</returns>
        public int ToRgba()
        {
            uint a = ((uint)(Alpha * 255.0f) & 0xFF);
            uint r = ((uint)(R * 255.0f) & 0xFF);
            uint g = ((uint)(G * 255.0f) & 0xFF);
            uint b = ((uint)(B * 255.0f) & 0xFF);

            uint value = a;
            value |= b << 8;
            value |= g << 16;
            value |= r << 24;

            return (int)value;
        }

        /// <summary>
        /// Converts the color into a three component vector.
        /// </summary>
        /// <returns>A three component vector containing the red, green, and blue components of the color.</returns>
        public Vector3D ToVector3D()
        {
            return new Vector3D(R, G, B);
        }

        /// <summary>
        /// Converts the color into a four component vector.
        /// </summary>
        /// <returns>A four component vector containing all four color components.</returns>
        public Vector4D ToVector4d()
        {
            return new Vector4D(R, G, B, Alpha);
        }

        /// <summary>
        /// Creates an array containing the elements of the color.
        /// </summary>
        /// <returns>A four-element array containing the components of the color in ARGB order.</returns>
        public double[] ToArray()
        {
            return new double[] { Alpha, R, G, B };
        }

        /// <summary>
        /// Adds two colors.
        /// </summary>
        /// <param name="left">The first color to add.</param>
        /// <param name="right">The second color to add.</param>
        /// <param name="result">When the method completes, completes the sum of the two colors.</param>
        public static void Add(ref Color4 left, ref Color4 right, out Color4 result)
        {
            result.Alpha = left.Alpha + right.Alpha;
            result.R = left.R + right.R;
            result.G = left.G + right.G;
            result.B = left.B + right.B;
        }

        /// <summary>
        /// Adds two colors.
        /// </summary>
        /// <param name="left">The first color to add.</param>
        /// <param name="right">The second color to add.</param>
        /// <returns>The sum of the two colors.</returns>
        public static Color4 Add(Color4 left, Color4 right)
        {
            return new Color4(left.Alpha + right.Alpha, left.R + right.R, left.G + right.G, left.B + right.B);
        }

        /// <summary>
        /// Subtracts two colors.
        /// </summary>
        /// <param name="left">The first color to subtract.</param>
        /// <param name="right">The second color to subtract.</param>
        /// <param name="result">WHen the method completes, contains the difference of the two colors.</param>
        public static void Subtract(ref Color4 left, ref Color4 right, out Color4 result)
        {
            result.Alpha = left.Alpha - right.Alpha;
            result.R = left.R - right.R;
            result.G = left.G - right.G;
            result.B = left.B - right.B;
        }

        /// <summary>
        /// Subtracts two colors.
        /// </summary>
        /// <param name="left">The first color to subtract.</param>
        /// <param name="right">The second color to subtract</param>
        /// <returns>The difference of the two colors.</returns>
        public static Color4 Subtract(Color4 left, Color4 right)
        {
            return new Color4(left.Alpha - right.Alpha, left.R - right.R, left.G - right.G, left.B - right.B);
        }

        /// <summary>
        /// Modulates two colors.
        /// </summary>
        /// <param name="left">The first color to modulate.</param>
        /// <param name="right">The second color to modulate.</param>
        /// <param name="result">When the method completes, contains the modulated color.</param>
        public static void Modulate(ref Color4 left, ref Color4 right, out Color4 result)
        {
            result.Alpha = left.Alpha * right.Alpha;
            result.R = left.R * right.R;
            result.G = left.G * right.G;
            result.B = left.B * right.B;
        }

        /// <summary>
        /// Modulates two colors.
        /// </summary>
        /// <param name="left">The first color to modulate.</param>
        /// <param name="right">The second color to modulate.</param>
        /// <returns>The modulated color.</returns>
        public static Color4 Modulate(Color4 left, Color4 right)
        {
            return new Color4(left.Alpha * right.Alpha, left.R * right.R, left.G * right.G, left.B * right.B);
        }

        /// <summary>
        /// Scales a color.
        /// </summary>
        /// <param name="value">The color to scale.</param>
        /// <param name="scalar">The amount by which to scale.</param>
        /// <param name="result">When the method completes, contains the scaled color.</param>
        public static void Scale(ref Color4 value, double scalar, out Color4 result)
        {
            result.Alpha = value.Alpha * scalar;
            result.R = value.R * scalar;
            result.G = value.G * scalar;
            result.B = value.B * scalar;
        }

        /// <summary>
        /// Scales a color.
        /// </summary>
        /// <param name="value">The color to scale.</param>
        /// <param name="scalar">The amount by which to scale.</param>
        /// <returns>The scaled color.</returns>
        public static Color4 Scale(Color4 value, double scalar)
        {
            return new Color4(value.Alpha * scalar, value.R * scalar, value.G * scalar, value.B * scalar);
        }

        /// <summary>
        /// Negates a color.
        /// </summary>
        /// <param name="value">The color to negate.</param>
        /// <param name="result">When the method completes, contains the negated color.</param>
        public static void Negate(ref Color4 value, out Color4 result)
        {
            result.Alpha = -value.Alpha;
            result.R = -value.R;
            result.G = -value.G;
            result.B = -value.B;
        }

        /// <summary>
        /// Negates a color.
        /// </summary>
        /// <param name="value">The color to negate.</param>
        /// <returns>The negated color.</returns>
        public static Color4 Negate(Color4 value)
        {
            return new Color4(-value.Alpha, -value.R, -value.G, -value.B);
        }

        /// <summary>
        /// Inverts the color (takes the complement of the color).
        /// </summary>
        /// <param name="value">The color to invert.</param>
        /// <param name="result">When the method completes, contains the inverted color.</param>
        public static void Invert(ref Color4 value, out Color4 result)
        {
            result.Alpha = 1.0f - value.Alpha;
            result.R = 1.0f - value.R;
            result.G = 1.0f - value.G;
            result.B = 1.0f - value.B;
        }

        /// <summary>
        /// Inverts the color (takes the complement of the color).
        /// </summary>
        /// <param name="value">The color to invert.</param>
        /// <returns>The inverted color.</returns>
        public static Color4 Invert(Color4 value)
        {
            return new Color4(-value.Alpha, -value.R, -value.G, -value.B);
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="result">When the method completes, contains the clamped value.</param>
        public static void Clamp(ref Color4 value, ref Color4 min, ref Color4 max, out Color4 result)
        {
            double alpha = value.Alpha;
            alpha = (alpha > max.Alpha) ? max.Alpha : alpha;
            alpha = (alpha < min.Alpha) ? min.Alpha : alpha;

            double red = value.R;
            red = (red > max.R) ? max.R : red;
            red = (red < min.R) ? min.R : red;

            double green = value.G;
            green = (green > max.G) ? max.G : green;
            green = (green < min.G) ? min.G : green;

            double blue = value.B;
            blue = (blue > max.B) ? max.B : blue;
            blue = (blue < min.B) ? min.B : blue;

            result = new Color4(alpha, red, green, blue);
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static Color4 Clamp(Color4 value, Color4 min, Color4 max)
        {
            Color4 result;
            Clamp(ref value, ref min, ref max, out result);
            return result;
        }

        /// <summary>
        /// Performs a linear interpolation between two colors.
        /// </summary>
        /// <param name="start">Start color.</param>
        /// <param name="end">End color.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <param name="result">When the method completes, contains the linear interpolation of the two colors.</param>
        /// <remarks>
        /// This method performs the linear interpolation based on the following formula.
        /// <code>start + (end - start) * amount</code>
        /// Passing <paramref name="amount"/> a value of 0 will cause <paramref name="start"/> to be returned; a value of 1 will cause <paramref name="end"/> to be returned. 
        /// </remarks>
        public static void Lerp(ref Color4 start, ref Color4 end, double amount, out Color4 result)
        {
            result.Alpha = start.Alpha + amount * (end.Alpha - start.Alpha);
            result.R = start.R + amount * (end.R - start.R);
            result.G = start.G + amount * (end.G - start.G);
            result.B = start.B + amount * (end.B - start.B);
        }

        /// <summary>
        /// Performs a linear interpolation between two colors.
        /// </summary>
        /// <param name="start">Start color.</param>
        /// <param name="end">End color.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <returns>The linear interpolation of the two colors.</returns>
        /// <remarks>
        /// This method performs the linear interpolation based on the following formula.
        /// <code>start + (end - start) * amount</code>
        /// Passing <paramref name="amount"/> a value of 0 will cause <paramref name="start"/> to be returned; a value of 1 will cause <paramref name="end"/> to be returned. 
        /// </remarks>
        public static Color4 Lerp(Color4 start, Color4 end, double amount)
        {
            return new Color4(
                start.Alpha + amount * (end.Alpha - start.Alpha),
                start.R + amount * (end.R - start.R),
                start.G + amount * (end.G - start.G),
                start.B + amount * (end.B - start.B));
        }

        /// <summary>
        /// Performs a cubic interpolation between two colors.
        /// </summary>
        /// <param name="start">Start color.</param>
        /// <param name="end">End color.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <param name="result">When the method completes, contains the cubic interpolation of the two colors.</param>
        public static void SmoothStep(ref Color4 start, ref Color4 end, double amount, out Color4 result)
        {
            amount = (amount > 1.0f) ? 1.0f : ((amount < 0.0f) ? 0.0f : amount);
            amount = (amount * amount) * (3.0f - (2.0f * amount));

            result.Alpha = start.Alpha + ((end.Alpha - start.Alpha) * amount);
            result.R = start.R + ((end.R - start.R) * amount);
            result.G = start.G + ((end.G - start.G) * amount);
            result.B = start.B + ((end.B - start.B) * amount);
        }

        /// <summary>
        /// Performs a cubic interpolation between two colors.
        /// </summary>
        /// <param name="start">Start color.</param>
        /// <param name="end">End color.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <returns>The cubic interpolation of the two colors.</returns>
        public static Color4 SmoothStep(Color4 start, Color4 end, double amount)
        {
            amount = (amount > 1.0f) ? 1.0f : ((amount < 0.0f) ? 0.0f : amount);
            amount = (amount * amount) * (3.0f - (2.0f * amount));

            return new Color4(
                start.Alpha + ((end.Alpha - start.Alpha) * amount),
                start.R + ((end.R - start.R) * amount),
                start.G + ((end.G - start.G) * amount),
                start.B + ((end.B - start.B) * amount));
        }

        /// <summary>
        /// Returns a color containing the smallest components of the specified colorss.
        /// </summary>
        /// <param name="left">The first source color.</param>
        /// <param name="right">The second source color.</param>
        /// <param name="result">When the method completes, contains an new color composed of the largest components of the source colorss.</param>
        public static void Max(ref Color4 left, ref Color4 right, out Color4 result)
        {
            result.Alpha = (left.Alpha > right.Alpha) ? left.Alpha : right.Alpha;
            result.R = (left.R > right.R) ? left.R : right.R;
            result.G = (left.G > right.G) ? left.G : right.G;
            result.B = (left.B > right.B) ? left.B : right.B;
        }

        /// <summary>
        /// Returns a color containing the largest components of the specified colorss.
        /// </summary>
        /// <param name="left">The first source color.</param>
        /// <param name="right">The second source color.</param>
        /// <returns>A color containing the largest components of the source colors.</returns>
        public static Color4 Max(Color4 left, Color4 right)
        {
            Color4 result;
            Max(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Returns a color containing the smallest components of the specified colors.
        /// </summary>
        /// <param name="left">The first source color.</param>
        /// <param name="right">The second source color.</param>
        /// <param name="result">When the method completes, contains an new color composed of the smallest components of the source colors.</param>
        public static void Min(ref Color4 left, ref Color4 right, out Color4 result)
        {
            result.Alpha = (left.Alpha < right.Alpha) ? left.Alpha : right.Alpha;
            result.R = (left.R < right.R) ? left.R : right.R;
            result.G = (left.G < right.G) ? left.G : right.G;
            result.B = (left.B < right.B) ? left.B : right.B;
        }

        /// <summary>
        /// Returns a color containing the smallest components of the specified colors.
        /// </summary>
        /// <param name="left">The first source color.</param>
        /// <param name="right">The second source color.</param>
        /// <returns>A color containing the smallest components of the source colors.</returns>
        public static Color4 Min(Color4 left, Color4 right)
        {
            Color4 result;
            Min(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Adjusts the contrast of a color.
        /// </summary>
        /// <param name="value">The color whose contrast is to be adjusted.</param>
        /// <param name="contrast">The amount by which to adjust the contrast.</param>
        /// <param name="result">When the method completes, contains the adjusted color.</param>
        public static void AdjustContrast(ref Color4 value, double contrast, out Color4 result)
        {
            result.Alpha = value.Alpha;
            result.R = 0.5f + contrast * (value.R - 0.5f);
            result.G = 0.5f + contrast * (value.G - 0.5f);
            result.B = 0.5f + contrast * (value.B - 0.5f);
        }

        /// <summary>
        /// Adjusts the contrast of a color.
        /// </summary>
        /// <param name="value">The color whose contrast is to be adjusted.</param>
        /// <param name="contrast">The amount by which to adjust the contrast.</param>
        /// <returns>The adjusted color.</returns>
        public static Color4 AdjustContrast(Color4 value, double contrast)
        {
            return new Color4(
                value.Alpha,
                0.5f + contrast * (value.R - 0.5f),
                0.5f + contrast * (value.G - 0.5f),
                0.5f + contrast * (value.B - 0.5f));
        }

        /// <summary>
        /// Adjusts the saturation of a color.
        /// </summary>
        /// <param name="value">The color whose saturation is to be adjusted.</param>
        /// <param name="saturation">The amount by which to adjust the saturation.</param>
        /// <param name="result">When the method completes, contains the adjusted color.</param>
        public static void AdjustSaturation(ref Color4 value, double saturation, out Color4 result)
        {
            double grey = value.R * 0.2125f + value.G * 0.7154f + value.B * 0.0721f;

            result.Alpha = value.Alpha;
            result.R = grey + saturation * (value.R - grey);
            result.G = grey + saturation * (value.G - grey);
            result.B = grey + saturation * (value.B - grey);
        }

        /// <summary>
        /// Adjusts the saturation of a color.
        /// </summary>
        /// <param name="value">The color whose saturation is to be adjusted.</param>
        /// <param name="saturation">The amount by which to adjust the saturation.</param>
        /// <returns>The adjusted color.</returns>
        public static Color4 AdjustSaturation(Color4 value, double saturation)
        {
            double grey = value.R * 0.2125f + value.G * 0.7154f + value.B * 0.0721f;

            return new Color4(
                value.Alpha,
                grey + saturation * (value.R - grey),
                grey + saturation * (value.G - grey),
                grey + saturation * (value.B - grey));
        }

        /// <summary>
        /// Inverts the color (takes the complement of the color).
        /// </summary>
        /// <param name="value">The color to invert.</param>
        /// <returns>The inverted color.</returns>
        public static Color4 operator ~(Color4 value)
        {
            return new Color4(1.0f - value.Alpha, 1.0f - value.R, 1.0f - value.G, 1.0f - value.B);
        }

        /// <summary>
        /// Adds two colors.
        /// </summary>
        /// <param name="left">The first color to add.</param>
        /// <param name="right">The second color to add.</param>
        /// <returns>The sum of the two colors.</returns>
        public static Color4 operator +(Color4 left, Color4 right)
        {
            return new Color4(left.Alpha + right.Alpha, left.R + right.R, left.G + right.G, left.B + right.B);
        }

        /// <summary>
        /// Assert a color (return it unchanged).
        /// </summary>
        /// <param name="value">The color to assert (unchange).</param>
        /// <returns>The asserted (unchanged) color.</returns>
        public static Color4 operator +(Color4 value)
        {
            return value;
        }

        /// <summary>
        /// Subtracts two colors.
        /// </summary>
        /// <param name="left">The first color to subtract.</param>
        /// <param name="right">The second color to subtract.</param>
        /// <returns>The difference of the two colors.</returns>
        public static Color4 operator -(Color4 left, Color4 right)
        {
            return new Color4(left.Alpha - right.Alpha, left.R - right.R, left.G - right.G, left.B - right.B);
        }

        /// <summary>
        /// Negates a color.
        /// </summary>
        /// <param name="value">The color to negate.</param>
        /// <returns>A negated color.</returns>
        public static Color4 operator -(Color4 value)
        {
            return new Color4(-value.Alpha, -value.R, -value.G, -value.B);
        }

        /// <summary>
        /// Scales a color.
        /// </summary>
        /// <param name="scalar">The factor by which to scale the color.</param>
        /// <param name="value">The color to scale.</param>
        /// <returns>The scaled color.</returns>
        public static Color4 operator *(double scalar, Color4 value)
        {
            return new Color4(value.Alpha * scalar, value.R * scalar, value.G * scalar, value.B * scalar);
        }

        /// <summary>
        /// Scales a color.
        /// </summary>
        /// <param name="value">The factor by which to scale the color.</param>
        /// <param name="scalar">The color to scale.</param>
        /// <returns>The scaled color.</returns>
        public static Color4 operator *(Color4 value, double scalar)
        {
            return new Color4(value.Alpha * scalar, value.R * scalar, value.G * scalar, value.B * scalar);
        }

        /// <summary>
        /// Modulates two colors.
        /// </summary>
        /// <param name="left">The first color to modulate.</param>
        /// <param name="right">The second color to modulate.</param>
        /// <returns>The modulated color.</returns>
        public static Color4 operator *(Color4 left, Color4 right)
        {
            return new Color4(left.Alpha * right.Alpha, left.R * right.R, left.G * right.G, left.B * right.B);
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Color4 left, Color4 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Color4 left, Color4 right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SlimMath.Color4"/> to <see cref="SlimMath.Color3"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Color3(Color4 value)
        {
            return new Color3(value.R, value.G, value.B);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SlimMath.Color4"/> to <see cref="SlimMath.Vector3D"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector3D(Color4 value)
        {
            return new Vector3D(value.R, value.G, value.B);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SlimMath.Color4"/> to <see cref="SlimMath.Vector4d"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector4D(Color4 value)
        {
            return new Vector4D(value.R, value.G, value.B, value.Alpha);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SlimMath.Vector3D"/> to <see cref="SlimMath.Color4"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Color4(Vector3D value)
        {
            return new Color4(value.x, value.y, value.z, 1.0f);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SlimMath.Vector4d"/> to <see cref="SlimMath.Color4"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Color4(Vector4D value)
        {
            return new Color4(value.x, value.y, value.z, value.w);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Int32"/> to <see cref="SlimMath.Color4"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Color4(int value)
        {
            return new Color4(value);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "Alpha:{0} Red:{1} Green:{2} Blue:{3}", Alpha, R, G, B);
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

            return string.Format(CultureInfo.CurrentCulture, "Alpha:{0} Red:{1} Green:{2} Blue:{3}", Alpha.ToString(format, CultureInfo.CurrentCulture),
                R.ToString(format, CultureInfo.CurrentCulture), G.ToString(format, CultureInfo.CurrentCulture), B.ToString(format, CultureInfo.CurrentCulture));
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
            return string.Format(formatProvider, "Alpha:{0} Red:{1} Green:{2} Blue:{3}", Alpha, R, G, B);
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

            return string.Format(formatProvider, "Alpha:{0} Red:{1} Green:{2} Blue:{3}", Alpha.ToString(format, formatProvider),
                R.ToString(format, formatProvider), G.ToString(format, formatProvider), B.ToString(format, formatProvider));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Alpha.GetHashCode() + R.GetHashCode() + G.GetHashCode() + B.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="SlimMath.Color4"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="SlimMath.Color4"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="SlimMath.Color4"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Color4 other)
        {
            return (Alpha == other.Alpha) && (R == other.R) && (G == other.G) && (B == other.B);
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

            return Equals((Color4)obj);
        }


        public static Vector3D HSB2RGB(float h, float s, float v)
        {

            float r = 0, g = 0, b = 0;
            int i = (int)((h / 60) % 6);
            float f = (h / 60) - i;
            float p = v * (1 - s);
            float q = v * (1 - f * s);
            float t = v * (1 - (1 - f) * s);
            switch (i)
            {
                case 0:
                    r = v;
                    g = t;
                    b = p;
                    break;
                case 1:
                    r = q;
                    g = v;
                    b = p;
                    break;
                case 2:
                    r = p;
                    g = v;
                    b = t;
                    break;
                case 3:
                    r = p;
                    g = q;
                    b = v;
                    break;
                case 4:
                    r = t;
                    g = p;
                    b = v;
                    break;
                case 5:
                    r = v;
                    g = p;
                    b = q;
                    break;
                default:
                    break;
            }
            return
            new Vector3D((double)(r * 255), (double)(g * 255), (double)(b * 255));
        }

        //public static Color4 SetColorRamp(double min, double max, double v,Color4 minColor,Color4 maxColor)
        //{
        //    if (v < min) return minColor;

        //    if (v > max) return maxColor;



        //    int stepcount = 4;

        //    double step = (max - min) / stepcount;

        //    //return Color4.Lerp(Color4.Red, Color4.Yellow, (v-min) / step);




        //    v -= min;


        //    if (v < step)
        //    {
        //        return Color4.Lerp(Color4.Red, Color4.Yellow, v / step);
        //    }
        //    v -= step;
        //    if (v < step)
        //    {
        //        return Color4.Lerp(Color4.Yellow, Color4.Green, v / step);
        //    }
        //    v -= step;
        //    if (v < step)
        //    {
        //        return Color4.Lerp(Color4.Green, Color4.Cyan, v / step);
        //    }
        //    v -= step;
        //    if (v < step)
        //    {
        //        return Color4.Lerp(Color4.Cyan, Color4.Blue, v / step);
        //    }
        //    return Color4.Blue;

        //}

        public static Color4 SetColorRamp(double min, double max, double v)
        {
            if (v > max) return Color4.Red;
 

            int stepcount = 4;

            double step = (max - min) / stepcount;

            //return Color4.Lerp(Color4.Red, Color4.Yellow, (v-min) / step);

            v -= min;


            if (v < step)
            {
                return Color4.Lerp(Color4.Red, Color4.Yellow, v / step);
            }
            v -= step;
            if (v < step)
            {
                return Color4.Lerp(Color4.Yellow, Color4.Green, v / step);
            }
            v -= step;
            if (v < step)
            {
                return Color4.Lerp(Color4.Green, Color4.Cyan, v / step);
            }
            v -= step;
            if (v < step)
            {
                return Color4.Lerp(Color4.Cyan, Color4.SkyBlue, v / step);
            }
            return Color4.SkyBlue;
            
        }

        public static Color4 SetColorRampBetweenTwoColor(double min, double max, double v, Color4 color1, Color4 color2)
        {
            if (v <= min) return color1;
            if (v >= max) return color2;

            double range = max - min;
            v -= min;

            return Color4.Lerp(color1, color2, v / range);

        }

#if SlimDX1xInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimMath.Color4"/> to <see cref="SlimDX.Color4"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SlimDX.Color4(Color4 value)
        {
            return new SlimDX.Color4(value.Alpha, value.Red, value.Green, value.Blue);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimDX.Color4"/> to <see cref="SlimMath.Color4"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Color4(SlimDX.Color4 value)
        {
            return new Color4(value.Alpha, value.Red, value.Green, value.Blue);
        }
#endif

#if WPFInterop
        /// <summary>
        /// Performs an explicit conversion from <see cref="SlimMath.Color4"/> to <see cref="System.windows.Media.Color"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator System.windows.Media.Color(Color4 value)
        {
            return new System.windows.Media.Color()
            {
                A = (byte)(255f * value.Alpha),
                R = (byte)(255f * value.Red),
                G = (byte)(255f * value.Green),
                B = (byte)(255f * value.Blue)
            };
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.windows.Media.Color"/> to <see cref="SlimMath.Color4"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Color4(System.windows.Media.Color value)
        {
            return new Color4()
            {
                Alpha = (double)value.A / 255f,
                Red = (double)value.R / 255f,
                Green = (double)value.G / 255f,
                Blue = (double)value.B / 255f
            };
        }
#endif

#if WinFormsInterop
        /// <summary>
        /// Performs an explicit conversion from <see cref="SlimMath.Color4"/> to <see cref="System.Drawing.Color"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator System.Drawing.Color(Color4 value)
        {
            return System.Drawing.Color.FromArgb(
                (byte)(255f * value.Alpha),
                (byte)(255f * value.Red),
                (byte)(255f * value.Green),
                (byte)(255f * value.Blue));
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Drawing.Color"/> to <see cref="SlimMath.Color4"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Color4(System.Drawing.Color value)
        {
            return new Color4()
            {
                Alpha = (double)value.A / 255f,
                Red = (double)value.R / 255f,
                Green = (double)value.G / 255f,
                Blue = (double)value.B / 255f
            };
        }
#endif
    }
}
