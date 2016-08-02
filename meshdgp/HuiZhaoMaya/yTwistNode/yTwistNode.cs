// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================

//
//  File: yTwist.cpp
//
//  Description:
// 		Example implementation of a deformer. This node
//		twists the deformed vertices around the y-axis.
//
using System;
using System.Linq;
using System.Text;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaAnim;


[assembly: MPxNodeClass(typeof(MayaNetPlugin.yTwistNode), "yTwistCSharp", 0x0008106d, 
    NodeType = MPxNode.NodeType.kDeformerNode)]

namespace MayaNetPlugin
{
    [MPxNodeAffects("angle", "outputGeom")]
	class yTwistNode : MPxDeformerNode, IMPxNode
	{
        [MPxNodeNumeric("fa", "angle", MFnNumericData.Type.kDouble, Keyable = true)]
		public static MObject angle = null;

		public yTwistNode()
		{
		}

		override public void deform(MDataBlock block, MItGeometry iter, MMatrix m, uint multiIndex)
		{
			MDataHandle angleData = block.inputValue(angle);
			MDataHandle envData = block.inputValue(envelope);
			double magnitude = angleData.asDouble;

			float env = envData.asFloat;

			for (; !iter.isDone; iter.next())
			{
				MPoint pt = iter.position();

				// do the twist
				//

				double ff = magnitude * pt.y * env;
				if (ff != 0.0)
				{
					double cct = Math.Cos(ff);
					double cst = Math.Sin(ff);
					double tt = pt.x * cct - pt.z * cst;
					pt.z = pt.x * cst + pt.z * cct;
					pt.x = tt; ;
				}

				iter.setPosition(pt);
			}
		}
	}
}
