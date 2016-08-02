/*
* Copyright (c) 2007-2010 SlimMath Group
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
using System.Collections.Generic;

namespace GraphicResearchHuiZhao
{
    /// <summary>
    /// Implements the Gilbert-Johnson-Keerthi algorithm for convex polyhedra collision detection.
    /// </summary>
    public sealed class Gjk
    {
        Vector3D[] simplex = new Vector3D[4];
        int count = 0;
        Vector3D direction = Vector3D.UnitX;

        /// <summary>
        /// Gets or sets the direction for which to search for a support point.
        /// </summary>
        public Vector3D Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        /// <summary>
        /// Resets the state of this instance.
        /// </summary>
        public void Reset()
        {
            count = 0;
            Direction = Vector3D.UnitX;
        }

        /// <summary>
        /// Prepares the simplex to an empty state.
        /// </summary>
        public void Initialize()
        {
            count = 0;
        }

        /// <summary>
        /// Prepares the simplex with one initial point.
        /// </summary>
        /// <param name="point1">The point to initialize the simplex with.</param>
        public void Initialize(Vector3D point1)
        {
            simplex[0] = point1;

            count = 1;
        }

        /// <summary>
        /// Prepares the simplex with two initial points.
        /// </summary>
        /// <param name="point1">The first point to initialize the simplex with.</param>
        /// <param name="point2">The second point to initialize the simplex with.</param>
        public void Initialize(Vector3D point1, Vector3D point2)
        {
            simplex[0] = point1;
            simplex[1] = point2;

            count = 2;
        }

        /// <summary>
        /// Prepares the simplex with three initial points.
        /// </summary>
        /// <param name="point1">The first point to initialize the simplex with.</param>
        /// <param name="point2">The second point to initialize the simplex with.</param>
        /// <param name="point3">The third point to initialize the simplex with.</param>
        public void Initialize(Vector3D point1, Vector3D point2, Vector3D point3)
        {
            simplex[0] = point1;
            simplex[1] = point2;
            simplex[2] = point3;

            count = 3;
        }
        /*
        /// <summary>
        /// Prepares the simplex with four initial points.
        /// </summary>
        /// <param name="point1">The first point to initialize the simplex with.</param>
        /// <param name="point2">The second point to initialize the simplex with.</param>
        /// <param name="point3">The third point to initialize the simplex with.</param>
        /// <param name="point4">The fourth point to initialize the simplex with.</param>
        public void Initialize(Vector3D point1, Vector3D point2, Vector3D point3, Vector3D point4)
        {
            simplex[0] = point1;
            simplex[1] = point2;
            simplex[2] = point3;
            simplex[3] = point4;

            count = 4;
        }
        */
        /// <summary>
        /// Checks for a collision between two convex polyhedra.
        /// </summary>
        /// <param name="supportFunction">The function delegate responsible for returning support points.</param>
        /// <param name="maxIterations">The maximum amount of iterations allowed.</param>
        /// <returns><c>true</c> if there was an intersection between the two convex polyhedra; otherwise, <c>falce</c>.</returns>

        //Follow lines is not available

        #region Unavailable Lines

        //public bool Intersects(Func<Vector3D, Vector3D> supportFunction, int maxIterations = 20)
        //{
        //    /*
        //     * This method uses the following psuedo code from the paper:
        //     * http://www.win.tue.nl/~gino/solid/jgt98convex.pdf
        //     * 
        //     * You will notice that this method does not follow the psuedo code exactly. Some
        //     * changes were made based on the fact that we do not use −v to get the new support
        //     * point. This is becuase in our simplex handling code, the new direction is already
        //     * pointing in the appropriate direction (pointing somewhere toward the origin). This
        //     * also means that we have to flip the > sign to a < sign in the if statement.
        //     * 
        //     * 
        //     * v := "arbitrary vector";
        //     * W := ∅;
        //     * 
        //     * repeat
        //     *      w := Sᴀ-ʙ(−v);
        //     *      if v⋅w > 0 then return false;
        //     *      v := υ(conv(W ∪ {w}));
        //     *      W := "smallest X ⊆ W ∪ {w} such that v ∈ conv(X)";
        //     * until v = 0;
        //     *  
        //     * return true
        //    */

        //    var currentiteration = 0;
        //    count = 0;

        //    do
        //    {
        //        currentiteration++;

        //        var w = supportFunction(Direction);
        //        simplex[count++] = w;

        //        if (Vector3D.Dot(Direction, w) < 0f)
        //            return false;

        //        switch (count)
        //        {
        //            case 2:
        //                DoLine(simplex[1], simplex[0]);
        //                break;

        //            case 3:
        //                DoTriangle(simplex[2], simplex[1], simplex[0]);
        //                break;

        //            case 4:
        //                if (DoTetrahedron(simplex[3], simplex[2], simplex[1], simplex[0]))
        //                    return true;
        //                break;
        //        }
        //    } while (!Direction.Equals(Vector3D.Zero, Utilities.ZeroTolerance) && currentiteration < maxIterations);

        //    return false;
        //}

        ///// <summary>
        ///// Gets the minimum distance between two convex polyhedra.
        ///// </summary>
        ///// <param name="supportFunction">The function delegate responsible for returning support points.</param>
        ///// <param name="epsilon">The amount of allotted error in the result.</param>
        ///// <param name="maxIterations"><c>true</c> if there was an intersection between the two convex polyhedra; otherwise, <c>falce</c>.</param>
        ///// <returns>The minimum distance between the two convex polyhedra.</returns>
        //public double GetMinimumDistance(Func<Vector3D, Vector3D> supportFunction, int maxIterations = 20, double epsilon = 0.1f)
        //{
        //    /*
        //     * This method uses the following psuedo code from the paper:
        //     * http://www.win.tue.nl/~gino/solid/jgt98convex.pdf
        //     * 
        //     * 
        //     * v := "arbitrary point in A − B";
        //     * W := ∅;
        //     * μ := 0;
        //     * close_enough := false;
        //     * 
        //     * while not close_enough and v ≠ 0 do begin
        //     *      w := Sᴀ-ʙ(−v);
        //     *      δ := v⋅w/‖v‖;
        //     *      μ := max{μ,δ};
        //     *      close_enough := ‖v‖ − μ ≤ ε;
        //     *      
        //     *      if not close_enough then begin
        //     *          v := υ(conv(W ∪ {w}));
        //     *          W := "smallest X ⊆ W ∪ {w} such that v ∈ conv(X)";
        //     *      end
        //     * end;
        //     *  
        //     * return ‖v‖
        //    */

        //    var currentiteration = 0;
        //    var u = 0f;
        //    var closeenough = false;

        //    Direction = supportFunction(Direction);
        //    count = 0;

        //    while (!closeenough && !Direction.Equals(Vector3D.Zero, Utilities.ZeroTolerance) && currentiteration < maxIterations)
        //    {
        //        currentiteration++;
        //        double directionlength = Direction.Length;

        //        var w = supportFunction(-Direction);
        //        var d = Vector3D.Dot(Direction, w) / directionlength;
        //        u = Math.Max(u, d);

        //        closeenough = directionlength - u <= epsilon;

        //        simplex[count++] = w;

        //        if (!closeenough)
        //        {
        //            switch (count)
        //            {
        //                case 2:
        //                    DoLineNoDirection(simplex[1], simplex[0]);
        //                    break;

        //                case 3:
        //                    DoTriangleNoDirection(simplex[2], simplex[1], simplex[0]);
        //                    break;

        //                case 4:
        //                    if (DoTetrahedronNoDirection(simplex[3], simplex[2], simplex[1], simplex[0]))
        //                    {
        //                        //Objects intersect, distance makes no sense
        //                        return Direction.Length;
        //                    }
        //                    break;
        //            }

        //            Vector3D origin = Vector3D.Zero;

        //            switch (count)
        //            {
        //                case 1:
        //                    Direction = simplex[0];
        //                    break;

        //                case 2:
        //                    Collision.ClosestPointOnSegmentToPoint(ref simplex[0], ref simplex[1], ref origin, out direction);
        //                    break;

        //                case 3:
        //                    Collision.ClosestPointOnTriangleToPoint(ref simplex[0], ref simplex[1], ref simplex[2], ref origin, out direction);
        //                    break;
        //            }
        //        }
        //    }

        //    return Direction.Length;
        //}

        #endregion

        /// <summary>
        /// Simplifies a line simplex.
        /// </summary>
        /// <param name="a">The newly added point in the simplex.</param>
        /// <param name="b">The old point in the simplex.</param>
        private void DoLineNoDirection(Vector3D a, Vector3D b)
        {
            Vector3D ab = b - a;

            if (Vector3D.Dot(ab, -a) > 0.0f)
            {
                //Do not simplify this code out, it is needed for the
                //DoTriangle and DoTetrahedron methods
                simplex[0] = b;
                simplex[1] = a;
                count = 2;
            }
            else
            {
                simplex[0] = a;
                count = 1;
            }
        }

        /// <summary>
        /// Simplifies a triangle simplex;
        /// </summary>
        /// <param name="a">The newly added point in the simplex.</param>
        /// <param name="b">The old point in the simplex.</param>
        /// <param name="c">The oldest point in the simplex.</param>
        private void DoTriangleNoDirection(Vector3D a, Vector3D b, Vector3D c)
        {
            Vector3D ab = b - a;
            Vector3D ac = c - a;

            Vector3D abc = Vector3D.Cross(ab, ac);
            Vector3D ababc = Vector3D.Cross(ab, abc);
            Vector3D abcac = Vector3D.Cross(abc, ac);

            if (Vector3D.Dot(abcac, -a) > 0.0f)
            {
                if (Vector3D.Dot(ac, -a) > 0.0f)
                {
                    simplex[0] = c;
                    simplex[1] = a;
                    count = 2;
                }
                else
                {
                    DoLine(a, b);
                }
            }
            else
            {
                if (Vector3D.Dot(ababc, -a) > 0.0f)
                {
                    DoLine(a, b);
                }
                else
                {
                    if (Vector3D.Dot(abc, -a) > 0.0f)
                    {
                        //Do not simplify this code out, it is needed for the
                        //DoTetrahedron methods
                        simplex[0] = c;
                        simplex[1] = b;
                        simplex[2] = a;
                        count = 3;
                    }
                    else
                    {
                        simplex[0] = b;
                        simplex[1] = c;
                        simplex[2] = a;
                        count = 3;
                    }
                }
            }
        }

        /// <summary>
        /// Simplifies a tetrahedron simplex.
        /// </summary>
        /// <param name="a">The newly added point in the simplex.</param>
        /// <param name="b">The old point in the simplex.</param>
        /// <param name="c">The older point in the simplex.</param>
        /// <param name="d">The oldest point in the simplex.</param>
        /// <returns><c>true</c> if the origin is contained by this simplex; otherwise, <c>false</c>.</returns>
        private bool DoTetrahedronNoDirection(Vector3D a, Vector3D b, Vector3D c, Vector3D d)
        {
            Vector3D ab = b - a;
            Vector3D ac = c - a;
            Vector3D ad = d - a;
            Vector3D abc = Vector3D.Cross(ab, ac);
            Vector3D acd = Vector3D.Cross(ac, ad);
            Vector3D adb = Vector3D.Cross(ad, ab);

            if (Vector3D.Dot(abc, -a) > 0.0f)
            {
                if (Vector3D.Dot(acd, -a) > 0.0f)
                {
                    if (Vector3D.Dot(adb, -a) > 0.0f)
                    {
                        simplex[0] = a;
                        count = 1;
                        return false;
                    }
                    else
                    {
                        DoLine(a, c);
                        return false;
                    }
                }
                else
                {
                    if (Vector3D.Dot(adb, -a) > 0.0f)
                    {
                        DoLine(a, b);
                        return false;
                    }
                    else
                    {
                        DoTriangle(a, b, c);
                        return false;
                    }
                }
            }
            else
            {
                if (Vector3D.Dot(acd, -a) > 0.0f)
                {
                    if (Vector3D.Dot(adb, -a) > 0.0f)
                    {
                        DoLine(a, d);
                        return false;
                    }
                    else
                    {
                        DoTriangle(a, c, d);
                        return false;
                    }
                }
                else
                {
                    if (Vector3D.Dot(adb, -a) > 0.0f)
                    {
                        DoTriangle(a, d, b);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        /// <summary>
        /// Simplifies a line simplex and finds the new direction.
        /// </summary>
        /// <param name="a">The newly added point in the simplex.</param>
        /// <param name="b">The old point in the simplex.</param>
        private void DoLine(Vector3D a, Vector3D b)
        {
            Vector3D ab = b - a;

            if (Vector3D.Dot(ab, -a) > 0.0f)
            {
                //Do not simplify this code out, it is needed for the
                //DoTriangle and DoTetrahedron methods
                simplex[0] = b;
                simplex[1] = a;
                count = 2;
                Direction = Vector3D.Cross(ab, Vector3D.Cross(-a, ab));
            }
            else
            {
                simplex[0] = a;
                count = 1;
                Direction = -a;
            }
        }

        /// <summary>
        /// Simplifies a triangle simplex and finds the new direction.
        /// </summary>
        /// <param name="a">The newly added point in the simplex.</param>
        /// <param name="b">The old point in the simplex.</param>
        /// <param name="c">The oldest point in the simplex.</param>
        private void DoTriangle(Vector3D a, Vector3D b, Vector3D c)
        {
            Vector3D ab = b - a;
            Vector3D ac = c - a;

            Vector3D abc = Vector3D.Cross(ab, ac);
            Vector3D ababc = Vector3D.Cross(ab, abc);
            Vector3D abcac = Vector3D.Cross(abc, ac);

            if (Vector3D.Dot(abcac, -a) > 0.0f)
            {
                if (Vector3D.Dot(ac, -a) > 0.0f)
                {
                    simplex[0] = c;
                    simplex[1] = a;
                    count = 2;
                    Direction = Vector3D.Cross(ac, Vector3D.Cross(-a, ac));
                }
                else
                {
                    DoLine(a, b);
                }
            }
            else
            {
                if (Vector3D.Dot(ababc, -a) > 0.0f)
                {
                    DoLine(a, b);
                }
                else
                {
                    if (Vector3D.Dot(abc, -a) > 0.0f)
                    {
                        //Do not simplify this code out, it is needed for the
                        //DoTetrahedron methods
                        simplex[0] = c;
                        simplex[1] = b;
                        simplex[2] = a;
                        count = 3;
                        Direction = abc;
                    }
                    else
                    {
                        simplex[0] = b;
                        simplex[1] = c;
                        simplex[2] = a;
                        count = 3;
                        Direction = -abc;
                    }
                }
            }
        }

        /// <summary>
        /// Simplifies a tetrahedron simplex and finds the new direction.
        /// </summary>
        /// <param name="a">The newly added point in the simplex.</param>
        /// <param name="b">The old point in the simplex.</param>
        /// <param name="c">The older point in the simplex.</param>
        /// <param name="d">The oldest point in the simplex.</param>
        /// <returns><c>true</c> if the origin is contained by this simplex; otherwise, <c>false</c>.</returns>
        private bool DoTetrahedron(Vector3D a, Vector3D b, Vector3D c, Vector3D d)
        {
            Vector3D ab = b - a;
            Vector3D ac = c - a;
            Vector3D ad = d - a;
            Vector3D abc = Vector3D.Cross(ab, ac);
            Vector3D acd = Vector3D.Cross(ac, ad);
            Vector3D adb = Vector3D.Cross(ad, ab);

            if (Vector3D.Dot(abc, -a) > 0.0f)
            {
                if (Vector3D.Dot(acd, -a) > 0.0f)
                {
                    if (Vector3D.Dot(adb, -a) > 0.0f)
                    {
                        simplex[0] = a;
                        count = 1;
                        Direction = -a;
                        return false;
                    }
                    else
                    {
                        DoLine(a, c);
                        return false;
                    }
                }
                else
                {
                    if (Vector3D.Dot(adb, -a) > 0.0f)
                    {
                        DoLine(a, b);
                        return false;
                    }
                    else
                    {
                        DoTriangle(a, b, c);
                        return false;
                    }
                }
            }
            else
            {
                if (Vector3D.Dot(acd, -a) > 0.0f)
                {
                    if (Vector3D.Dot(adb, -a) > 0.0f)
                    {
                        DoLine(a, d);
                        return false;
                    }
                    else
                    {
                        DoTriangle(a, c, d);
                        return false;
                    }
                }
                else
                {
                    if (Vector3D.Dot(adb, -a) > 0.0f)
                    {
                        DoTriangle(a, d, b);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
    }
}
