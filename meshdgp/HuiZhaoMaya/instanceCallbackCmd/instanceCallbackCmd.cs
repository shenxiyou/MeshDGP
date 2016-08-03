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
//		This C# plugin is ported from: $(MAYADIR)\devkit\plug-ins\instanceCallbackCmd

/*
	File: instanceCallbackCmd.cs
	Command: "instCallbackCSharpCmd"

    Description
		class InstanceCallbackCmd 
		This plugin demonstrates the functionality added to MDagMessage class.
		The new functions added allow callbacks to be added for
			1) Instance Added for a specified node(and its instances)
			2) Instance Removed for a specified node(and its instances)
			3) Instance Added for any node
			4) Instance Removed for any node

	
	  This plugin: 
	  i.   Draws a circle, 
	  ii.  Gets its dagPath using an iterator
	  iii. Adds callback for instance added and removed for this circle.

		The callback functions just displays a message indicating the 
		invocation of the registered callback function.

*/
using System;

using Autodesk.Maya.OpenMaya;


[assembly: MPxCommandClass(typeof(MayaNetPlugin.InstanceCallbackCmd), "instCallbackCSharpCmd")]

namespace MayaNetPlugin
{
    public class InstanceCallbackCmd : MPxCommand, IMPxCommand
    {

        private void userDAGChildAddedCB(object sender, MParentChildFunctionArgs arg)
        {
           MGlobal.displayInfo("CALLBACK-FUNCTION REGISTERED FOR INSTANCE ADDED INVOKED");	
        }

        private void userDAGChildRemovedCB(object sender, MParentChildFunctionArgs arg)
        {
           MGlobal.displayInfo("CALLBACK-FUNCTION REGISTERED FOR INSTANCE REMOVED INVOKED");
        }

        public override void doIt(MArgList args)
        {
            // Draw a circle and get its dagPath
	        // using an iterator
	        MGlobal.executeCommand("circle");
	        MFnNurbsCurve circle = new MFnNurbsCurve();

            MDagPath dagPath = new MDagPath();
	        MItDependencyNodes iter = new MItDependencyNodes( MFn.Type.kNurbsCurve);

            for (iter.reset(); !iter.isDone; iter.next())
            {
                MObject item = iter.item;
                if (item.hasFn(MFn.Type.kNurbsCurve))
                {
                    circle.setObject(item);
                    circle.getPath(dagPath);
                    MGlobal.displayInfo("DAG_PATH is " + dagPath.fullPathName);

                    if (dagPath.isValid)
                    {
                        // register callback for instance add AND remove
                        //
                        dagPath.InstanceAddedDagPath += userDAGChildAddedCB;
                        dagPath.InstanceRemovedDagPath += userDAGChildRemovedCB;

                        // C# SDK will cleanup events, when this plugin is unloaded
                        // callbacks.append(node);

                        MGlobal.displayInfo("CALLBACK ADDED FOR INSTANCE ADD/REMOVE");
                    }
                }
            }
        }
    }
}
