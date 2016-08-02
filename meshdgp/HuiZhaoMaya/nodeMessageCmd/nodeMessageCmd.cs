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
// nodeMessageCmd.cs
//
// Description:
//     Sample plug-in that demonstrates how to register/de-register
//     a callback with the MNodeMessage class.
//
//     This plug-in will register a new command in maya called
//     "nodeMessage" which adds a callback for the all nodes on
//     the active selection list. A message is printed to stdout 
//     whenever a connection is made or broken for those nodes.
//
using System;

using Autodesk.Maya.OpenMaya;


[assembly: MPxCommandClass(typeof(MayaNetPlugin.nodeMessageCmd), "nodeMessageCSharp")]

namespace MayaNetPlugin
{
	class nodeMessageCmd : MPxCommand, IMPxCommand
	{
		public nodeMessageCmd() { }

		public void userCB(object sender, MAttr2PlugFunctionArgs arg)
		{
			MNodeMessage.AttributeMessage msg = arg.msg;
			MPlug plug = arg.plug;
			MPlug otherPlug = arg.otherPlug;
			if ((msg & MNodeMessage.AttributeMessage.kConnectionMade) != 0)
			{
				MGlobal.displayInfo("Connection made ");
			}
			else if ((msg & MNodeMessage.AttributeMessage.kConnectionBroken) != 0)
			{
				MGlobal.displayInfo("Connection broken ");
			}
			else
			{
				return;
			}
			MGlobal.displayInfo(plug.info);
			if ((msg & MNodeMessage.AttributeMessage.kOtherPlugSet) != 0)
			{
				if ((msg & MNodeMessage.AttributeMessage.kIncomingDirection) != 0)
				{
					MGlobal.displayInfo("  <--  " + otherPlug.info);
				}
				else
				{
					MGlobal.displayInfo("  -->  " + otherPlug.info);
				}
			}
			MGlobal.displayInfo("\n");
		}
		public override void doIt(MArgList args)
		//
		// Takes the  nodes that are on the active selection list and adds an
		// attribute changed callback to each one.
		//
		{
			MObject 		node = new MObject();
			MSelectionList 	list = new MSelectionList();
	
			// Register node callbacks for all nodes on the active list.
			//
			MGlobal.getActiveSelectionList( list );

			for ( uint i=0; i<list.length; i++ )
			{
				list.getDependNode( i, node );
		
				try
				{
					node.AttributeChanged += userCB;
				}
				catch (Exception)
				{
					MGlobal.displayInfo("MNodeMessage.addCallback failed\n");
					continue;
				}

				// C# SDK will cleanup events, when this plugin is unloaded
				// callbacks.append(node);
			}
		
			return;
		}
	}
}
