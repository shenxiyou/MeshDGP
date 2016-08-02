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

using Autodesk.Maya.OpenMaya;


[assembly: MPxCommandClass(typeof(MayaNetPlugin.WhatIsCmd), "netWhatIsCSharp")]

namespace MayaNetPlugin
{
	public class WhatIsCmd : MPxCommand, IMPxCommand
	{
		override public void doIt(MArgList args)
		{
			MGlobal.displayInfo("doIt...");
			MSelectionList selectList = MGlobal.activeSelectionList;

            foreach( MObject node in selectList.DependNodes() )
            {
    			MFnDependencyNode depFn = new MFnDependencyNode();
				depFn.setObject(node);
				MGlobal.displayInfo("Name: " + depFn.name);
				MGlobal.displayInfo("Type: " + node.apiTypeStr);
				MGlobal.displayInfo("Function Sets: ");
				foreach (string st in MGlobal.getFunctionSetList(node))
					MGlobal.displayInfo(st + ", ");
				MGlobal.displayInfo("");
			}

			return;
		}
	}
}
