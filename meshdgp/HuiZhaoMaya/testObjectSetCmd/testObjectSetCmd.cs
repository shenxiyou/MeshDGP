// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


// Example usages:
//          loadPlugin  testObjectSetCmd.nll.dll;
//          testObjectSetCSharp;
//


using System;

using Autodesk.Maya.OpenMaya;

//  This test is used to test MPxObjectSetNode
//

[assembly: MPxNodeClass(typeof(MayaNetPlugin.testMPxObjectSetNode), "testMPxObjectSetNodeCSharp",
    0x0008106c, NodeType = MPxNode.NodeType.kObjectSet)]
[assembly: MPxCommandClass(typeof(MayaNetPlugin.testObjectSetCmd), "testObjectSetCSharp")]

namespace MayaNetPlugin
{
    public class testMPxObjectSetNode : MPxObjectSet, IMPxNode
    {
    }

    public class testObjectSetCmd : MPxCommand, IMPxCommand
    {
        MDGModifier fDGMod = new MDGModifier();

        public override void doIt(MArgList args)
        {
            MTypeId id = new MTypeId(0x0008106c);
            MObject setNode = fDGMod.createNode(id);
            fDGMod.doIt();

            MSelectionList selList = new MSelectionList();
	        MGlobal.getActiveSelectionList( selList );
	        if( selList.length > 0 )
	        {
                try
                {
                    MFnSet setFn = new MFnSet(setNode);
                    setFn.addMembers(selList);
                }
                catch (System.Exception)
                {
                }
	        }
	        MFnDependencyNode depNodeFn = new MFnDependencyNode( setNode );
	        setResult( depNodeFn.name );
	        
            return;
        }
    }
}
