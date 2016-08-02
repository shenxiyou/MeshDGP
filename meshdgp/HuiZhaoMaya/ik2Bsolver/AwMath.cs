// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


namespace MayaNetPlugin
{
    class AwMath
    {
        public const float kFloatEpsilon = 1.0e-5F;
        public const double kDoubleEpsilon = 1.0e-10;
        public const double kExtendedEpsilon = kDoubleEpsilon;
        public const double kPi = 3.14159265;

        public static bool equivalent(double x, double y, double fudge = kDoubleEpsilon) 
        {
	            return ((x > y) ? (x - y <= fudge) : (y - x <= fudge));
        }

        public static double clamp(double a, double l, double h)
        {
	        return ((a) < (l) ? (l) : (a) > (h) ? (h) : (a));
        }
    }
}
