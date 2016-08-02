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
using Autodesk.Maya.OpenMayaFX;

[assembly: MPxNodeClass(typeof(MayaNetPlugin.dynExprField), "dynExprFieldCSharp", 0x0008105e, NodeType = Autodesk.Maya.OpenMaya.MPxNode.NodeType.kFieldNode)]

namespace MayaNetPlugin
{
	class dynExprField : MPxFieldNode
	{
        [MPxNodeNumeric("dir", "direction", MFnNumericData.Type.k3Double, Keyable = true, NumericAttribType = MPxNodeNumericAttribute.Type.kPoint)]
        [MPxNumericDefault(new double[] { 1, 2, 3 })]
		public static MObject mDirection = null;

        [MPxNodeInitializer()]
		public static bool initializer()
		{
			attributeAffects(mDirection, mOutputForce);
			return true;
		}


		public override bool compute(MPlug plug, MDataBlock dataBlock)
		//
		//	Descriptions:
		//		compute output force.
		//
		{
			if (plug.notEqual(mOutputForce))
                return false;

			// get the logical index of the element this plug refers to.
			//
			uint multiIndex = plug.logicalIndex;

			// Get input data handle, use outputArrayValue since we do not
			// want to evaluate both inputs, only the one related to the
			// requested multiIndex. Evaluating both inputs at once would cause
			// a dependency graph loop.

			MArrayDataHandle hInputArray = dataBlock.outputArrayValue(mInputData);
			hInputArray.jumpToElement(multiIndex);

			// get children of aInputData.

			MDataHandle hCompond = hInputArray.inputValue();

			MDataHandle hPosition = hCompond.child(mInputPositions);
			MObject dPosition = hPosition.data();
			MFnVectorArrayData fnPosition = new MFnVectorArrayData(dPosition);
			MVectorArray points = fnPosition.array();

			// The attribute mInputPPData contains the attribute in an array form 
			// prepared by the particleShape if the particleShape has per particle 
			// attribute fieldName_attrName.  
			//
			// Suppose a field with the name dynExprField1 is connecting to 
			// particleShape1, and the particleShape1 has per particle float attribute
			// dynExprField1_magnitude and vector attribute dynExprField1_direction,
			// then hInputPPArray will contains a MdoubleArray with the corresponding
			// name "magnitude" and a MvectorArray with the name "direction".  This 
			// is a mechanism to allow the field attributes being driven by dynamic 
			// expression.
			MArrayDataHandle mhInputPPData = dataBlock.inputArrayValue(mInputPPData);
			mhInputPPData.jumpToElement(multiIndex);
			
			MDataHandle hInputPPData = mhInputPPData.inputValue();
			MObject dInputPPData = hInputPPData.data();
			MFnArrayAttrsData inputPPArray = new MFnArrayAttrsData(dInputPPData);

			MDataHandle hOwnerPPData = dataBlock.inputValue(mOwnerPPData);
			MObject dOwnerPPData = hOwnerPPData.data();
			MFnArrayAttrsData ownerPPArray = new MFnArrayAttrsData(dOwnerPPData);

			string magString = "magnitude";
			MFnArrayAttrsData.Type doubleType = MFnArrayAttrsData.Type.kDoubleArray;

			bool arrayExist;
			MDoubleArray magnitudeArray;
			arrayExist = inputPPArray.checkArrayExist(magString, out doubleType);
            if (arrayExist)
            {
                magnitudeArray = inputPPArray.getDoubleData(magString);
            }
            else
            {
                magnitudeArray = new MDoubleArray();
            }
		   
			MDoubleArray magnitudeOwnerArray;
			arrayExist = ownerPPArray.checkArrayExist(magString, out doubleType);
			if (arrayExist)
			{
				magnitudeOwnerArray = ownerPPArray.getDoubleData(magString);
            }
            else
            {
                magnitudeOwnerArray = new MDoubleArray();
            }

			string dirString = "direction";
			MFnArrayAttrsData.Type vectorType = MFnArrayAttrsData.Type.kVectorArray;
			MVectorArray directionArray;
			arrayExist = inputPPArray.checkArrayExist(dirString, out vectorType);
            if (arrayExist)
            {
                directionArray = inputPPArray.getVectorData(dirString);
            }
            else
            {
                directionArray = new MVectorArray();
            }
		 
			MVectorArray directionOwnerArray;
			arrayExist = ownerPPArray.checkArrayExist(dirString, out vectorType);
            if (arrayExist)
            {
                directionOwnerArray = ownerPPArray.getVectorData(dirString);
            }
            else
            {
                directionOwnerArray = new MVectorArray();
            }

			// Compute the output force.
			//
			MVectorArray forceArray = new MVectorArray();

			apply(dataBlock, points.length, magnitudeArray, magnitudeOwnerArray,
				   directionArray, directionOwnerArray, forceArray);

			// get output data handle
			//
			MArrayDataHandle hOutArray = dataBlock.outputArrayValue(mOutputForce);
			MArrayDataBuilder bOutArray = hOutArray.builder();

			// get output force array from block.
			//
			MDataHandle hOut = bOutArray.addElement(multiIndex);
			MFnVectorArrayData fnOutputForce = new MFnVectorArrayData();
			MObject dOutputForce = fnOutputForce.create(forceArray);

			// update data block with new output force data.
			//
			hOut.set(dOutputForce);
			dataBlock.setClean(plug);

			return true;
		}

		public override void iconSizeAndOrigin(ref uint width,
					ref uint height,
					ref uint xbo,
					ref uint ybo)
		{
			width = 32;
			height = 32;
			xbo = 4;
			ybo = 4;
			return;
		}

		public unsafe override void iconBitmap(byte* bitmap)
		{
			bitmap[0] = 0x18;
			bitmap[4] = 0x18;
			bitmap[8] = 0x18;
			bitmap[12] = 0x18;
			bitmap[16] = 0x18;
			bitmap[20] = 0x5A;
			bitmap[24] = 0x3C;
			bitmap[28] = 0x18;
			return;
		}

		private double magnitude(MDataBlock block)
		{
			MDataHandle hValue;
			double value = 0.0;
			try
			{
				hValue = block.inputValue(mMagnitude);
				value = hValue.asDouble;
			}
			catch
			{

			}

			return (value);
		}

		private MVector direction(MDataBlock block)
		{
			MFloatVector fV = block.inputValue(mDirection).asFloatVector;
			return new MVector(fV.x, fV.y, fV.z);
		}

		private unsafe void apply(
		MDataBlock block,
		uint receptorSize,
		MDoubleArray magnitudeArray,
		MDoubleArray magnitudeOwnerArray,
		MVectorArray directionArray,
		MVectorArray directionOwnerArray,
		MVectorArray outputForce
		)
		//
		//      Compute output force for each particle.  If there exists the 
		//      corresponding per particle attribute, use the data passed from
		//      particle shape (stored in magnitudeArray and directionArray).  
		//      Otherwise, use the attribute value from the field.
		//
		{
			// get the default values
			MVector defaultDir = direction(block);
			double defaultMag = magnitude(block);
			uint magArraySize = magnitudeArray.length;
			uint dirArraySize = directionArray.length;
			uint magOwnerArraySize = magnitudeOwnerArray.length;
			uint dirOwnerArraySize = directionOwnerArray.length;
			uint numOfOwner = magOwnerArraySize;
			if (dirOwnerArraySize > numOfOwner)
				numOfOwner = dirOwnerArraySize;

			double m_magnitude = defaultMag;
			MVector m_direction = defaultDir;

			for (int ptIndex = 0; ptIndex < receptorSize; ptIndex++)
			{
				if (receptorSize == magArraySize)
					m_magnitude = magnitudeArray[ptIndex];
				if (receptorSize == dirArraySize)
					m_direction = directionArray[ptIndex];
				if (numOfOwner > 0)
				{
					for (int nthOwner = 0; nthOwner < numOfOwner; nthOwner++)
					{
						if (magOwnerArraySize == numOfOwner)
							m_magnitude = magnitudeOwnerArray[nthOwner];
						if (dirOwnerArraySize == numOfOwner)
							m_direction = directionOwnerArray[nthOwner];
						outputForce.append(m_direction * m_magnitude);
					}
				}
				else
				{
					outputForce.append(m_direction * m_magnitude);
				}
			}
		}
	}
}
