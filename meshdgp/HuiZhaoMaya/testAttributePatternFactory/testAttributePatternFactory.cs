// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


// Note:
//		This C# plugin is ported from: $(MAYADIR)\devkit\test\testMPxAttributePatternFactory.cpp.
//
// How to use:
//      createAttrPatterns -patternType "attributePatternCSharp" -pd "myAttributeName"
//      createAttrPatterns -patternType "attributePatternCSharp" -pf "myFileName"

/////////////////////////////////////////////////////////////////////////////

using Autodesk.Maya.OpenMaya;


[assembly: MPxAttributePatternFactoryClass(typeof(MayaNetPlugin.AttributePatternFactory), "attributePatternCSharp")]


namespace MayaNetPlugin
{
	public class AttributePatternFactory : MPxAttributePatternFactory
	{
		override public void createPatternsFromString(string patternString, MAttributePatternArray patternArray)
		{
			string patternName = "testAttrPatternString";
			MAttributePattern createdPattern = new MAttributePattern(patternName);
			MFnNumericAttribute nAttr = new MFnNumericAttribute();

			// Ignore the string for now and create a single float attribute
			//
			MObject patternFactoryAttr = nAttr.create("testAttrPatternFactoryByString", "tafs",
							   MFnNumericData.Type.kFloat, 0);
			nAttr.isKeyable = true;
			nAttr.isStorable = true;
			createdPattern.addRootAttr(patternFactoryAttr);
			patternArray.append(createdPattern);
		}

		override public void createPatternsFromFile(string patternFile, MAttributePatternArray patternArray)
		{
			string patternName = "testAttrPatternFile";
			MAttributePattern createdPattern = new MAttributePattern(patternName);
			MFnNumericAttribute nAttr = new MFnNumericAttribute();

			// Ignore the string for now and create a single float attribute
			//
			MObject patternFactoryAttr = nAttr.create("testAttrPatternFactoryByString", "tafs",
							   MFnNumericData.Type.kFloat, 0);
			nAttr.isKeyable = true;
			nAttr.isStorable = true;
			createdPattern.addRootAttr(patternFactoryAttr);
			patternArray.append(createdPattern);
		}
		// Get the hardcoded name of this pattern factory type
		//
		override public string name()
		{
			return "attributePatternCSharp";
		}
	}
}
