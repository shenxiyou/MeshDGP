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
    /// Represents a bounding frustum in three dimensional space.
    /// </summary>
    [Serializable]
    public class BoundingFrustum
    {                                                                           
        Plane near;
        Plane far;
        Plane top;
        Plane bottom;
        Plane left;
        Plane right;

        Matrix4D Matrix;
        Gjk gjk = new Gjk();

        /// <summary>
        /// Initializes a new instance of the <see cref="SlimMath.BoundingFrustum"/> class.
        /// </summary>
        /// <param name="value">The <see cref="SlimMath.Matrix4d"/> to extract the planes from.</param>
        public BoundingFrustum(Matrix4D value)
        {
            SetMatrix(ref value);
        }

        /// <summary>
        /// Sets the Matrix4d that represents this instance of <see cref="SlimMath.BoundingFrustum"/>.
        /// </summary>
        /// <param name="value">The <see cref="SlimMath.Matrix4d"/> to extract the planes from.</param>
        public void SetMatrix(ref Matrix4D value)
        {
            this.Matrix = value;

            //Near
            near.Normal.x = value.M13;
            near.Normal.y = value.M23;
            near.Normal.z = value.M33;
            near.D = value.M43;

            //Far
            far.Normal.x = value.M14 - value.M13;
            far.Normal.y = value.M24 - value.M23;
            far.Normal.z = value.M34 - value.M33;
            far.D = value.M44 - value.M43;

            //Top
            top.Normal.x = value.M14 - value.M12;
            top.Normal.y = value.M24 - value.M22;
            top.Normal.z = value.M34 - value.M32;
            top.D = value.M44 - value.M42;

            //Bottom
            bottom.Normal.x = value.M14 + value.M12;
            bottom.Normal.y = value.M24 + value.M22;
            bottom.Normal.z = value.M34 + value.M32;
            bottom.D = value.M44 + value.M42;

            //Left
            left.Normal.x = value.M14 + value.M11;
            left.Normal.y = value.M24 + value.M21;
            left.Normal.z = value.M34 + value.M31;
            left.D = value.M44 + value.M41;

            //Right
            right.Normal.x = value.M14 - value.M11;
            right.Normal.y = value.M24 - value.M21;
            right.Normal.z = value.M34 - value.M31;
            right.D = value.M44 - value.M41;
        }
    }
}
