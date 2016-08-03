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
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.Maya.OpenMaya;


namespace MayaNetPlugin
{
    class apiSimpleShapeIterator : MPxGeometryIterator
    {

        public apiSimpleShapeIterator(object userGeometry, MObjectArray components) :
            base(userGeometry, components)
        {
            mGeometry = userGeometry as MVectorArray;
            reset();
        }
        public apiSimpleShapeIterator(object userGeometry, MObject components) :
            base(userGeometry, components)
        {
            mGeometry = userGeometry as MVectorArray;
            reset();
        }

        public override void reset()
        {
            base.reset();
            currentPoint = 0;

            if (mGeometry != null)
            {
                uint maxVertex = mGeometry.length;
                maxPoints = (int)maxVertex;
            }
        }

        public override MPoint point()
        {
            MPoint pnt;
            if (mGeometry != null)
                pnt = new MPoint(mGeometry[index()]);
            else
                pnt = new MPoint();
            return pnt;
        }

        public override void setPoint(MPoint pnt)
        {
            if (mGeometry != null)
            {
                mGeometry[index()] = new MVector(pnt);
            }
        }

        public override int iteratorCount()
        {
            if (mGeometry == null)
                return -1;
            return (int)mGeometry.length;
        }

        public override bool hasPoints()
        {
            return true;
        }

        public MVectorArray mGeometry;
    }
}
