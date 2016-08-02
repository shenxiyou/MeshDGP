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
using System.Linq;
using System.Text;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaUI;

[assembly: MPxNodeClass(typeof(MayaNetPlugin.genericAttributeNode), "genericAttributeNodeCSharp", 0x00081060)]

namespace MayaNetPlugin
{
	public class genericAttributeNode : MPxNode, IMPxNode
	{
		static MObject gOutputFloat_2Float_3Float = new MObject();
		static MObject gInputInt;

		public override bool compute(MPlug plug, MDataBlock dataBlock)
		{
			if ( plug.equalEqual(gOutputFloat_2Float_3Float) )
			{
				// attribute affecting generic attribute case.  Based on the
				// input attribute, we modify the output generic attribute
				MDataHandle inputDataInt = dataBlock.inputValue( gInputInt );
				int inputInt = inputDataInt.asInt;
		
				// Get the output handle
				MDataHandle outputData = dataBlock.outputValue( plug );	
				bool isGenericNumeric = false;
				bool isGenericNull = false;
		
				// Is the output handle generic data
				if ( outputData.isGeneric( ref isGenericNumeric, ref isGenericNull ) )
				{
					// Based on the inputHandle, update the generic
					// output handle
					if ( inputInt == 1 )
						outputData.setGenericBool( false, true );
					else if ( inputInt == 2 )
						outputData.setGenericBool( true, true );
					else if ( inputInt == 3 )
						outputData.setGenericChar( 127, true );
					else if ( inputInt == 4 )
						outputData.setGenericDouble( 3.145, true );
					else if ( inputInt == 5 )
						outputData.setGenericFloat( (float)9.98, true );	
					else if ( inputInt == 6 )
						outputData.setGenericShort( 3245, true );
					else if ( inputInt == 7 )
						outputData.setGenericInt( 32768, true );
					else if ( inputInt == 8 )
					{
						MFnNumericData numericData = new MFnNumericData();
						MObject obj = numericData.create( MFnNumericData.Type.k2Float);
						numericData.setData( (float)1.5, (float)6.7 );
						outputData.set( obj );
					}
					else if ( inputInt == 9 )
					{
						MFnNumericData numericData = new MFnNumericData();
						MObject obj = numericData.create( MFnNumericData.Type.k3Float);
						numericData.setData( (float)2.5, (float)8.7, (float)2.3345 );
						outputData.set( obj );
					}
					else if ( inputInt == 10 )
					{
						outputData.setGenericInt( 909, true );
					}							

					// Mark the data clean
					outputData.setClean();
					dataBlock.setClean( gOutputFloat_2Float_3Float );
				}
			} 
			else 
			{
				return false;
			}

			return true;
		}
		
		// Adds a generic attribute that accepts a float, float2, float3
		public static void addComplexFloatGenericAttribute(ref MObject attrObject, string longName, string shortName)
		{
			// Create the generic attribute and set the 3 accepts types
			MFnGenericAttribute gAttr = new MFnGenericAttribute(); 
			attrObject = gAttr.create( longName, shortName ); 
			try
			{
				gAttr.addAccept(MFnNumericData.Type.kFloat); 

				gAttr.addAccept(MFnNumericData.Type.k2Float); 

				gAttr.addAccept(MFnNumericData.Type.k3Float);
			}
			catch (System.Exception)
			{
				MGlobal.displayError("error happens in addAccept");
			}

			gAttr.isWritable = false;
			gAttr.isStorable = false;

			// Add the attribute to the node
			try
			{
				addAttribute(attrObject);
			}
			catch (System.Exception)
			{
				MGlobal.displayError("error happens in addAttribute");
			}
		}

        [MPxNodeInitializer()]
		public static void initialize()
		{
			MFnNumericAttribute nAttr = new MFnNumericAttribute();

			// single float attribute affecting a generic attribute
			try{
				gInputInt = nAttr.create( "gInputInt", "gii",
					MFnNumericData.Type.kInt, 0 );
			}
			catch (System.Exception)
			{
				MGlobal.displayError("error happens in addAccept");
			}
            nAttr.isStorable = true;
            nAttr.isKeyable = true;

			try{
				addAttribute( gInputInt );
			}
			catch (System.Exception)
			{
				MGlobal.displayError("error happens in addAccept");
			}

			addComplexFloatGenericAttribute(ref gOutputFloat_2Float_3Float, "gOutputFloat_2Float_3Float", "gof2f3f" );

			try{
				attributeAffects( gInputInt, gOutputFloat_2Float_3Float );
			}
			catch (System.Exception)
			{
				MGlobal.displayError("error happens in addAccept");
			}
			return;
		}


	}
}
