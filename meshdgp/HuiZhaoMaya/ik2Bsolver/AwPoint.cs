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
    class AwPoint
    {
        public double x, y, z, w;

        public AwPoint()
        {
            x = y = z = 0.0;
            w = 1.0;
        }
        public AwPoint(AwPoint point)
        {
            x = point.x;
            y = point.y;
            z = point.z;
            w = point.w;
        }
        public AwPoint(AwVector v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
            w = 1.0;
        }
        public AwPoint(double xx, double yy, double zz, double ww)
        {
            x = xx;
            y = yy;
            z = zz;
            w = ww;
        }

        public AwPoint set(double xx, double yy, double zz, double ww)
        {
            x = xx; y = yy; z = zz; w = ww;
            return this;
        }

        public AwPoint cartesian()
        { 
            AwPoint temp = new AwPoint(this); 
            return temp.cartesianize(); 
        }
        
        public AwPoint cartesianize()
        {
            if (w != 1.0)
            {
                double wInv = 1.0 / w;
                x = x * wInv;
                y = y * wInv;
                z = z * wInv;
                w = 1.0;
            }
            AwPoint point = new AwPoint(this);
            return point;
        }

        public AwVector cartesianSub( AwPoint otherPt) 
        {
	        AwPoint ptA = cartesian();
	        AwPoint ptB = otherPt.cartesian();
            AwVector v = new AwVector(ptA.x - ptB.x, ptA.y - ptB.y, ptA.z - ptB.z);
	        return v;
        }

        public AwPoint cartesianSub(AwVector v)
        {
            AwPoint ptA = cartesian();
            AwPoint p = new AwPoint(ptA.x - v.x, ptA.y - v.y, ptA.z - v.z, 1.0);
            return p;
        }

        public AwPoint cartesianAdd(AwVector v)
        {
            AwPoint pt = cartesian();
            AwPoint temp = new AwPoint(pt.x + v.x, pt.y + v.y, pt.z + v.z, 1.0);
            return temp;
        }

        public AwPoint add( AwVector v)
        {
            if ( w == 1.0)
            {
                AwPoint p = new AwPoint( x+v.x, y+v.y, z+v.z, 1.0 );
                return p;
            }
            else
                return cartesianAdd(v);
        }

        public AwPoint sub(AwVector v)
        {
            if (w == 1.0)
            {
                AwPoint p = new AwPoint(x - v.x, y - v.y, z -v.z, 1.0);
                return p;
            }
            else
                return cartesianSub(v);
        }

        public AwVector sub(AwPoint v)
        {
            if (w == 1.0)
            {
                AwVector p = new AwVector(x - v.x, y - v.y, z - v.z);
                return p;
            }
            else
                return cartesianSub(v);
        }
    }
}
