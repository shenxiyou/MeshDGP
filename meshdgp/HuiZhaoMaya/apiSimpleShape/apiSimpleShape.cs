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

[assembly: MPxShapeClass(typeof(MayaNetPlugin.apiSimpleShape), typeof(MayaNetPlugin.apiSimpleShapeUI), "apiSimpleShapeCSharp", 0x8009a)]

namespace MayaNetPlugin
{
    public class apiSimpleShape : MPxComponentShape
    {
        public static MTypeId id
        {
            get { return new MTypeId(0x8009a); }
        }

        public override MPxGeometryIterator geometryIteratorSetup(MObjectArray componentList, MObject components, bool forReadOnly)
        {
            apiSimpleShapeIterator result;
            if (components.isNull)
            {
                result = new apiSimpleShapeIterator(controlPoints, componentList);
            }
            else
            {
                result = new apiSimpleShapeIterator(controlPoints, components);
            }
            return result;
        }

        public override bool acceptsGeometryIterator(bool writeable)
        {
            return true;
        }

        public override bool acceptsGeometryIterator(MObject obj, bool writeable, bool forReadOnly)
        {
            return true;
        }
    }
}
