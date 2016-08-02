// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


using System;

namespace MayaNetPlugin
{
    class AwQuaternion
    {
        public double x, y, z, w;
        
        public AwQuaternion()
        {
            w = 1.0;
            x = 0.0;
            y = 0.0;
            z = 0.0;
        }

        public AwQuaternion(AwQuaternion q)
        {
            w = q.w;
            x = q.x;
            y = q.y;
            z = q.z;
        }

        public AwQuaternion(double a, double b, double c, double d)
        {
            x = a;
            y = b;
            z = c;
            w = d;
        }

        public AwQuaternion(double angle, AwVector axis)
        {
            w = 1.0;
            x = 0.0;
            y = 0.0;
            z = 0.0;
            setAxisAngle(axis, angle);
        }

        public AwQuaternion(AwVector a, AwVector b)
        {
            w = 1.0;
            x = 0.0;
            y = 0.0;
            z = 0.0;

            double factor = a.length() * b.length();

            if (Math.Abs(factor) > AwMath.kFloatEpsilon)
            {
                // Vectors have length > 0
                AwVector pivotVector = new AwVector();
                double dot = a.dotProduct(b) / factor;
                double theta = Math.Acos(AwMath.clamp(dot, -1.0, 1.0));

                pivotVector = a.crossProduct(b);
                if (dot < 0.0 && pivotVector.length() < AwMath.kFloatEpsilon)
                {
                    // Vectors parallel and opposite direction, therefore a rotation
                    // of 180 degrees about any vector perpendicular to this vector
                    // will rotate vector a onto vector b.
                    //
                    // The following guarantees the dot-product will be 0.0.
                    //

                    uint dominantIndex = (uint)a.dominantAxis();
                    uint index = ( dominantIndex + 1) % 3;
                    double value = -a.getIndex( index) ;
                    pivotVector.setIndex(dominantIndex, value);
                    pivotVector.setIndex((dominantIndex + 1) % 3, a.getIndex(dominantIndex));
                    pivotVector.setIndex((dominantIndex + 2) % 3, 0);
                }
                setAxisAngle(pivotVector, theta);
            }
        }


        public AwQuaternion setAxisAngle(AwVector axis, double theta)
        {
            double sumOfSquares = 
		        (double) axis.x * axis.x +
		        (double) axis.y * axis.y +
		        (double) axis.z * axis.z;
		
            if (sumOfSquares <= AwMath.kDoubleEpsilon) 
            {
		        w = 1.0;
                x = 0.0;
                y = 0.0;
                z = 0.0;
            } 
            else 
            {
		        theta *= 0.5;
		        w = Math.Cos(theta);
		        double commonFactor = Math.Sin(theta);
		        if (!AwMath.equivalent(sumOfSquares, 1.0)) 
			        commonFactor /= Math.Sqrt(sumOfSquares);
		        
                x = commonFactor * (double) axis.x;
		        y = commonFactor * (double) axis.y;
		        z = commonFactor * (double) axis.z;
	        }
	        
            return this;
        }

        public bool getAxisAngle(AwVector axis, ref double theta)
        {
            bool result;
	        double inverseOfSinThetaByTwo, thetaExtended;
	
	        if (AwMath.equivalent(w, (double) 1.0)) 
            {
		        theta = 0.0;
		        if (axis.length() < AwMath.kDoubleEpsilon) 
                {
			        axis.set(0.0,0.0,1.0);
		        }
		        result = false;
	        }
	        else 
            {
		        thetaExtended = Math.Acos(AwMath.clamp(w,-1.0,1.0));
		        theta = thetaExtended * 2.0;
		
		        inverseOfSinThetaByTwo = 1.0 / Math.Sin(thetaExtended);
		        axis.x = x * inverseOfSinThetaByTwo;
		        axis.y = y * inverseOfSinThetaByTwo;
		        axis.z = z * inverseOfSinThetaByTwo;

		        result = true;
	        }

	        return result;
        }

        public AwQuaternion mul(AwQuaternion rhs)
        {
            AwQuaternion result = new AwQuaternion();
            result.w = rhs.w * w - (rhs.x * x + rhs.y * y + rhs.z * z);
            result.x = rhs.w * x + rhs.x * w + rhs.y * z - rhs.z * y;
            result.y = rhs.w * y + rhs.y * w + rhs.z * x - rhs.x * z;
            result.z = rhs.w * z + rhs.z * w + rhs.x * y - rhs.y * x;
            return result;
        }
    }

}
