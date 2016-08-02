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
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Xml;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaFX;

[assembly: MPxNodeClass(typeof(MayaNetPlugin.SimpleSpring), "simpleSpringCSharp", 
    0x00081068, NodeType = MPxNode.NodeType.kSpringNode)]

namespace MayaNetPlugin
{
    public class SimpleSpring : MPxSpringNode, IMPxNode
    {
        double factor;

        [MPxNodeNumeric("sf", "springFactor", MFnNumericData.Type.kDouble, Keyable = true)]
        [MPxNumericDefault(1.0)]
        public static MObject aSpringFactor = null;

        double springFactor( MDataBlock block )
        {
            MDataHandle handle = block.inputValue(aSpringFactor);
            double value = 0.0;
            value = handle.asDouble;
            return value;
        }

        public double end1WeightValue( MDataBlock block )
        {
	        MDataHandle hValue = block.inputValue( mEnd1Weight );

	        double value = 0.0;
		    value = hValue.asDouble;
	        return value;
        }

        double end2WeightValue( MDataBlock block )
        {
	        MDataHandle hValue = block.inputValue( mEnd2Weight );

	        double value = 0.0;
		    value = hValue.asDouble;
	        return value;
        }

        public override bool compute(MPlug plug, MDataBlock dataBlock)
        {
            factor = springFactor(dataBlock);
            // Note: return "kUnknownParameter" so that Maya spring node can
            // compute spring force for this plug-in simple spring node.
            return false;
        }

        public override void applySpringLaw(double stiffness, double damping, double restLength, double endMass1, 
            double endMass2, MVector endP1, MVector endP2, MVector endV1, MVector endV2, MVector forceV1, MVector forceV2)
        {
            MVector distV = endP1 - endP2;
	        double L = distV.length;
	        distV.normalize();

	        double F = factor * (L - restLength);
	        forceV1 = - F * distV;
	        forceV2 = - forceV1;

	        return;
        }
    }

   
}
