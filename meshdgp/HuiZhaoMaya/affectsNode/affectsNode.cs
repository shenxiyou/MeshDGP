// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


/////////////////////////////////
// Plugin Affects Class		   //
/////////////////////////////////

// INTRODUCTION:
//	This class will create an "affects" node. This node is used for
//	demonstrating attributeAffects relationships involving dynamic
//	attributes.
//
// WHAT THIS PLUG-IN DEMONSTRATES:
//	This plug-in creates a node called "affects". Add two dynamic
//	integer attributes called "A" and "B". When you change the value on
//	A, note that B will recompute.
//
// HOW TO USE THIS PLUG-IN:
//	(1) Compile the plug-in
//	(2) Load the compiled plug-in into Maya via the plug-in manager
//	(3) Create an "affects" node by typing the MEL command:
//			createNode affectsCSharp;
//	(4) Add two integer dynamic attributes to the newly created
//		affects node by typing the MEL command:
//			addAttr -ln A -at long  affects1;
//			addAttr -ln B -at long  affects1;
//	(5) Change the value of "A" to 10 by typing the MEL command:
//			setAttr affects1.A 10;
//		At this point, the affectsNode::setDependentsDirty() method
//		gets called which causes "B" to be marked dirty.
//	(6) Compute the value on "B" by doing a getAttr:
//			getAttr affects1.B;
//		The affectsNode::compute() method is entered which copies the
//		value from "A" (i.e. 10) to "B".
//

using System;
using System.Collections.Generic;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaUI;
using Autodesk.Maya.OpenMayaRender;

[assembly: MPxNodeClass(typeof(MayaNetPlugin.affects), "affectsCSharp", 0x00081059)]

namespace MayaNetPlugin
{
	public class affects : MPxNode
	{
		public affects()
		{
		}

		~affects()
		{
		}

		// The compute() method does the actual work of the node using the inputs
		// of the node to generate its output.
		//
		// Compute takes two parameters: plug and data.
		// - Plug is the the data value that needs to be recomputed
		// - Data provides handles to all of the nodes attributes, only these
		//   handles should be used when performing computations.
		//
		public override bool compute(MPlug plug, MDataBlock dataBlock)
		{
			MObject thisNode = thisMObject();
			MFnDependencyNode fnThisNode = new MFnDependencyNode(thisNode);
			MGlobal.displayInfo("affects::compute(), plug being computed is \"" + plug.name + "\"");
 
			if (plug.partialName() == "B") {
				// Plug "B" is being computed. Assign it the value on plug "A"
				// if "A" exists.
				//
				MPlug pA  = fnThisNode.findPlug("A");
			  
				MGlobal.displayInfo("\t\t... found dynamic attribute \"A\", copying its value to \"B\"");
				MDataHandle inputData = dataBlock.inputValue(pA);
				
				int value = inputData.asInt;

				MDataHandle outputHandle = dataBlock.outputValue( plug );

				outputHandle.set(value);
				dataBlock.setClean(plug);

			} else {
				return false;
			}
			return true;
		}


		// The setDependentsDirty() method allows attributeAffects relationships
		// in a much more general way than via MPxNode::attributeAffects
		// which is limited to static attributes only.
		// The setDependentsDirty() method allows relationships to be established
		// between any combination of dynamic and static attributes.
		//
		// Within a setDependentsDirty() implementation you get passed in the
		// plug which is being set dirty, and then, based upon which plug it is,
		// you may choose to dirty any other plugs by adding them to the
		// affectedPlugs list.
		//
		// In almost all cases, the relationships you set up will be fixed for
		// the duration of Maya, such as "A affects B". However, you can also
		// set up relationships which depend upon some external factor, such
		// as the current frame number, the time of day, if maya was invoked in
		// batch mode, etc. These sorts of relationships are straightforward to
		// implement in your setDependentsDirty() method.
		//
		// There may also be situations where you need to look at values in the
		// dependency graph. It is VERY IMPORTANT that when accessing DG values
		// you do not cause a DG evaluation. This is because your setDependentsDirty()
		// method is called during dirty processing and causing an evalutaion could
		// put Maya into an infinite loop. The only safe way to look at values
		// on plugs is via the MDataBlock::outputValue() which does not trigger
		// an evaluation. It is recommeneded that you only look at plugs whose
		// values are constant or you know have already been computed.
		//
		// For this example routine, we will only implement the simplest case
		// of a relationship.
		//
		public override void setDependentsDirty(MPlug plugBeingDirtied, MPlugArray affectedPlugs)
		{
			MObject thisNode = thisMObject();
			MFnDependencyNode fnThisNode = new MFnDependencyNode(thisNode);

			if (plugBeingDirtied.partialName() == "A") {
				// "A" is dirty, so mark "B" dirty if "B" exists.
				// This implements the relationship "A affects B".
				//
				MGlobal.displayInfo("affects::setDependentsDirty, \"A\" being dirtied");
				MPlug pB = fnThisNode.findPlug("B");
				MGlobal.displayInfo("\t\t... dirtying \"B\"\n");

				affectedPlugs.append(pB);
			}
			return;
		}

		// The initialize method is called only once when the node is first
		// registered with Maya. Use the MPxNodeInitializer attribute class
		// to specify the callback for the initialization purpose.
		//
        [MPxNodeInitializer()]
		public static void initialize()
		{
			// Do nothing
		}

	}
}
