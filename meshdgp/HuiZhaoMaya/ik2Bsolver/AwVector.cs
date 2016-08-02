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
    class AwVector
    {
        public double x, y, z;
        public enum Axis {
		    kXaxis,
		    kYaxis,
		    kZaxis
	    };
        public AwVector()
        {
            x = y = z = 0.0;
        }

        public AwVector(AwVector v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public AwVector(double xx, double yy, double zz)
        {
            x = xx;
            y = yy;
            z = zz;
        }

        public void set(double xx, double yy, double zz)
        {
            x = xx;
            y = yy;
            z = zz;
        }

        public double dotProduct(AwVector v)
        {
            return x * v.x + y * v.y + z * v.z;
        }

        public AwVector crossProduct( AwVector r)
        { 
            return new AwVector(y*r.z - z*r.y, z*r.x - x*r.z, x*r.y - y*r.x); 
        }

        public double norm()
        {
            return x * x + y * y + z * z;
        }

        public double length()
        {
            return Math.Sqrt(norm());
        }

        public AwVector normal()
        {
            AwVector tmp = new AwVector(this);
            tmp.normalize();
            return tmp;
        }

        public void normalize()
        {
            double n = norm();
            if (n > AwMath.kDoubleEpsilon && ! AwMath.equivalent(n,1.0,2.0*AwMath.kDoubleEpsilon) )
            {
                double factor = 1.0/Math.Sqrt(n);
                x *= factor;
                y *= factor;
                z *= factor;
            }
        }

        public AwVector rotateBy( AwQuaternion q)
        {
            double rw = -q.x * x - q.y * y - q.z * z;
            double rx = q.w * x + q.y * z - q.z * y;
            double ry = q.w * y + q.z * x - q.x * z;
            double rz = q.w * z + q.x * y - q.y * x;
            AwVector v = new AwVector(-rw * q.x + rx * q.w - ry * q.z + rz * q.y,
                            -rw * q.y + ry * q.w - rz * q.x + rx * q.z,
                            -rw * q.z + rz * q.w - rx * q.y + ry * q.x);
            return v;
        }

        public bool isParallel( AwVector otherVector, double tolerance) 
        {
	        AwVector v1, v2;
	        v1 = normal();
	        v2 = otherVector.normal();
            double dotPrd = v1.dotProduct(v2);
	        return (AwMath.equivalent( Math.Abs(dotPrd), (double) 1.0, tolerance));
        }

        public Axis dominantAxis()
        {
            double xx, yy;

            if ((xx = Math.Abs(x)) > (yy = Math.Abs(y)))
            {
                if (xx > Math.Abs(z))
                {
                    return Axis.kXaxis;
                }
                else
                {
                    return Axis.kZaxis;
                }
            }
            else
            {
                if (yy > Math.Abs(z))
                {
                    return Axis.kYaxis;
                }
                else
                {
                    return Axis.kZaxis;
                }
            }
        }

        public double angle(AwVector vec)
        {
            double cosine = normal().dotProduct(vec.normal());
            double angle;
            if (cosine >= 1.0)
                angle = 0.0;
            else if (cosine <= -1.0)
                angle = AwMath.kPi;
            else
                angle = Math.Acos(cosine);
            return angle;
        }

        public void setIndex(uint index, double value)
        {
            if (index == 0)
                x = value;
            else if (index == 1)
                y = value;
            else if (index == 2)
                z = value;
        }

        public double getIndex(uint index)
        {
            if (index == 0)
                return x;
            else if (index == 1)
                return y;
            else if (index == 2)
                return z;

            return 0;
        }

        public AwVector mulMatrix(AwMatrix matrix)
        {
            AwVector tmp = new AwVector();
            tmp.x = x * matrix.getIndex(0, 0) + y * matrix.getIndex(1, 0) + z * matrix.getIndex(2, 0);
            tmp.y = x * matrix.getIndex(0, 1) + y * matrix.getIndex(1, 1) + z * matrix.getIndex(2, 1);
            tmp.z = x * matrix.getIndex(0, 2) + y * matrix.getIndex(1, 2) + z * matrix.getIndex(2, 2);
            return tmp;
        }

        public AwVector sub(AwVector v)
        {
            AwVector tmp = new AwVector(this);
            tmp.x = tmp.x - v.x;
            tmp.y = tmp.y - v.y;
            tmp.z = tmp.z - v.z;
            return tmp;
        }

        public AwVector add(AwVector v)
        {
            AwVector tmp = new AwVector(this);
            tmp.x = tmp.x + v.x;
            tmp.y = tmp.y + v.y;
            tmp.z = tmp.z + v.z;
            return tmp;
        }

        public double mul(AwVector v)
        {
            return dotProduct(v);
        }

        public AwVector mul(double s)
        {
            AwVector tmp = new AwVector(this);
            tmp.x = tmp.x * s;
            tmp.y = tmp.y * s;
            tmp.z = tmp.z * s;
            return tmp;
        }

    }
}
