// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


// Description:
//     Sample plug-in that demonstrates how to register/de-register
//     a callback with the MDagMessage class.
//
//     This plug-in will register a new command in maya called
//     "dagMessage" which adds a callback for the all nodes on
//     the active selection list. A message is printed to stdout 
//     whenever a connection is made or broken for those nodes. If
//		nothing is selected, the callback will be for all nodes.
//
//	   dagMessage -help will list the options.
//

using System;

using Autodesk.Maya.OpenMaya;


[assembly: MPxCommandClass(typeof(MayaNetPlugin.dagMessageCmd), "dagMessageCmdCSharp")]

namespace MayaNetPlugin
{

    [MPxCommandSyntaxFlag("-ad", "-allDag")]
    [MPxCommandSyntaxFlag("-pa", "-parentAdded")]
    [MPxCommandSyntaxFlag("-pr", "-parentRemoved")]
    [MPxCommandSyntaxFlag("-ca", "-childAdded")]
    [MPxCommandSyntaxFlag("-cr", "-childRemoved")]
    [MPxCommandSyntaxFlag("-cro", "-childReordered")]
    [MPxCommandSyntaxFlag("-h", "-help")]
    [MPxCommandSyntaxSelection(ObjectType = typeof(MSelectionList), UseSelectionAsDefault = true)]
	class dagMessageCmd : MPxCommand, IMPxCommand
	{
		public static readonly string kAllDagFlag = "-ad";
		public static readonly string kAllDagFlagLong = "-allDag";
		public static readonly string kParentAddedFlag = "-pa";
		public static readonly string kParentAddedFlagLong = "-parentAdded";
		public static readonly string kParentRemovedFlag = "-pr";
		public static readonly string kParentRemovedFlagLong = "-parentRemoved";
		public static readonly string kChildAddedFlag = "-ca";
		public static readonly string kChildAddedFlagLong = "-childAdded";
		public static readonly string kChildRemovedFlag = "-cr";
		public static readonly string kChildRemovedFlagLong = "-childRemoved";
		public static readonly string kChildReorderedFlag = "-cro";
		public static readonly string kChildReorderedFlagLong = "-childReordered";
		public static readonly string kHelpFlag = "-h";
		public static readonly string kHelpFlagLong = "-help";


		// Node added to model callback.
		#region UserCallbacks
		private static void userNodeRemovedCB(object sender, MNodeFunctionArgs arg)
		{
			MObject node = arg.node;
			if (!node.isNull)
			{
				MFnDagNode dagNode;
				try
				{
					dagNode = new MFnDagNode(node);
					string info = "DAG Model -  Node removed: ";
					info += dagNode.name;
					MGlobal.displayInfo(info);
				}
				catch (System.Exception)
				{
					MGlobal.displayInfo("Error: failed to get dag node.");
				}
			}

			// remove the callback
			node.NodeRemovedFromModel -= userNodeRemovedCB;
		}

		// Node added to model callback.
		private static void userNodeAddedCB(object sender, MNodeFunctionArgs args)
		{
			MObject node = args.node;

			if (!node.isNull)
			{
				MDagPath path = new MDagPath();
				try
				{
					MDagPath.getAPathTo(node, path);
					string info = "DAG Model -  Node added: ";
					info += path.fullPathName;

					try
					{
						MObject obj = path.transform;
					}
					catch (ArgumentException)
					{
						info += "(WORLD)";
					}
					catch (Exception)
					{
					}

					MGlobal.displayInfo(info);
				}
				catch (System.Exception)
				{
					MGlobal.displayInfo("Error: failed to get dag path to node.");
				}
			}

			// remove the callback
			node.NodeAddedToModel -= userNodeAddedCB;

			// listen for removal message
			try
			{
				node.NodeRemovedFromModel += userNodeRemovedCB;
			}
			catch (System.Exception)
			{
				MGlobal.displayError("Failed to install node removed from model callback.\n");
			}
		}

		// Install a node added callback for the node specified
		// by dagPath.
		private static void installNodeAddedCallback(MDagPath dagPath)
		{
			if (dagPath == null)
				return;

			MObject dagNode = dagPath.node;
			if (dagNode.isNull)
				return;

			try
			{

				dagNode.NodeAddedToModel += userNodeAddedCB;
			}
			catch (System.Exception)
			{
				MGlobal.displayError("Failed to install node added to model callback.\n");
			}
		}

		// Decide if the dag is in the model. Dag paths and names
		// may not be setup if the dag has not been added to
		// the model.
		private static bool dagNotInModel(MDagPath dagPath)
		{
			MFnDagNode dagFn;
			bool inModel;
			try
			{
				dagFn = new MFnDagNode(dagPath);
				inModel = dagFn.inModel;
			}
			catch (System.Exception)
			{
				return false;
			}
			return (inModel == false);
		}
		#region userGenericCB

		private void userDAGParentAddedCB(object sender, MParentChildFunctionArgs arg)
		{
			MDagPath child = arg.child;
			MDagPath parent = arg.parent;
			string dagStr = userDAGCBHelper("DAG Changed - Parent Added: ", child, parent);
			MGlobal.displayInfo(dagStr);
		}
		private void userDAGParentRemovedCB(object sender, MParentChildFunctionArgs arg)
		{
			MDagPath child = arg.child;
			MDagPath parent = arg.parent;
			string dagStr = userDAGCBHelper("DAG Changed - Parent Removed: ", child, parent);
			MGlobal.displayInfo(dagStr);
		}
		private void userDAGChildAddedCB(object sender, MParentChildFunctionArgs arg)
		{
			MDagPath child = arg.child;
			MDagPath parent = arg.parent;
			string dagStr = userDAGCBHelper("DAG Changed - Child Added: ", child, parent);
			MGlobal.displayInfo(dagStr);
		}
		private void userDAGChildRemovedCB(object sender, MParentChildFunctionArgs arg)
		{
			MDagPath child = arg.child;
			MDagPath parent = arg.parent;
			string dagStr = userDAGCBHelper("DAG Changed - Child Removed: ", child, parent);
			MGlobal.displayInfo(dagStr);
		}
		private void userDAGChildReorderedCB(object sender, MParentChildFunctionArgs arg)
		{
			MDagPath child = arg.child;
			MDagPath parent = arg.parent;
			string dagStr = userDAGCBHelper("DAG Changed - Child Reordered: ", child, parent);
			MGlobal.displayInfo(dagStr);
		}


		private void userDAGInstanceAddedCB(object sender, MParentChildFunctionArgs arg)
		{
			MDagPath child = arg.child;
			MDagPath parent = arg.parent;
			string dagStr = userDAGCBHelper("DAG Changed - Unknown Type: ", child, parent);
			MGlobal.displayInfo(dagStr);
		}
		private void userDAGInstanceRemovedCB(object sender, MParentChildFunctionArgs arg)
		{
			MDagPath child = arg.child;
			MDagPath parent = arg.parent;
			string dagStr = userDAGCBHelper("DAG Changed - Unknown Type: ", child, parent);
			MGlobal.displayInfo(dagStr);
		}
		private string userDAGCBHelper(string prefix, MDagPath child, MDagPath parent)
		{
			string dagStr = string.Format("{0} child = {1}, parent = {2}",
				prefix, child.fullPathName, parent.fullPathName);
			// Check to see if the parent is the world object.
			try
			{
				MObject obj = parent.transform;
			}
			catch (ArgumentException)
			{
				dagStr += "(WORLD)";
			}
			catch (Exception)
			{

			}

			// Install callbacks if node is not in the model.
			// Callback is for node added to model.
			bool incomplete = false;
			if (dagNotInModel(child))
			{
				installNodeAddedCallback(child);
				incomplete = true;
			}
			if (dagNotInModel(parent))
			{
				installNodeAddedCallback(parent);
				incomplete = true;
			}

			// Warn user that dag path info may be
			// incomplete
			if (incomplete)
				dagStr += "\t// May be incomplete!";
			return dagStr;
		}
		private void userDAGGenericCB(object sender, MMessageParentChildFunctionArgs arg)
		{
			MDagMessage.DagMessage msg = arg.msgType;
			MDagPath child = arg.child;
			MDagPath parent = arg.parent;
			string dagStr;
			switch (msg)
			{
				case MDagMessage.DagMessage.kParentAdded:
					dagStr = "DAG Changed - Parent Added: ";
					break;
				case MDagMessage.DagMessage.kParentRemoved:
					dagStr = "DAG Changed - Parent Removed: ";
					break;
				case MDagMessage.DagMessage.kChildAdded:
					dagStr = "DAG Changed - Child Added: ";
					break;
				case MDagMessage.DagMessage.kChildRemoved:
					dagStr = "DAG Changed - Child Removed: ";
					break;
				case MDagMessage.DagMessage.kChildReordered:
					dagStr = "DAG Changed - Child Reordered: ";
					break;
				default:
					dagStr = "DAG Changed - Unknown Type: ";
					break;
			}
			dagStr = userDAGCBHelper(dagStr, child, parent);

			MGlobal.displayInfo(dagStr);
		}
		#endregion userGenericCB
		#endregion


		private void addGenericCallback(MDagPath dagPath, MDagMessage.DagMessage msg, string cbName)
		{
			if (dagPath == null || MDagPath.getCPtr(dagPath).Handle == IntPtr.Zero)
			{
				// we don't have genericCallback
				// MDagMessage.addDagCallback(DagMessage, MDagMessage::MMessageParentChildFunction)
				try
				{
					switch (msg)
					{
					case MDagMessage.DagMessage.kChildAdded:
						MDagMessage.ChildAdded += userDAGGenericCB;
						break;
					case MDagMessage.DagMessage.kChildRemoved:
						MDagMessage.ChildRemoved += userDAGGenericCB;
						break;
					case MDagMessage.DagMessage.kChildReordered:
						MDagMessage.ChildReordered += userDAGGenericCB;
						break;
					case MDagMessage.DagMessage.kInstanceAdded:
						MDagMessage.InstanceAdded += userDAGGenericCB;
						break;

					case MDagMessage.DagMessage.kInstanceRemoved:
						MDagMessage.InstanceRemoved += userDAGGenericCB;
						break;

					case MDagMessage.DagMessage.kParentAdded:
						MDagMessage.ParentAdded += userDAGGenericCB;
						break;

					case MDagMessage.DagMessage.kParentRemoved:
						MDagMessage.ParentRemoved += userDAGGenericCB;
						break;
					default:
						throw new ArgumentException("Failed to add generic callback", "msg"); 
					}

					string info = string.Format("Adding a callback for {0} on all nodes", cbName);
					MGlobal.displayInfo(info);
				}
				catch (System.Exception)
				{
					string err = string.Format("Could not add callback to {0}", dagPath.fullPathName);
					MGlobal.displayError(err);
				}
			}
			else
			{
				string msgName = "Unknown";
				try
				{
					// we doesn't support obsolete
					// MDagMessage.addDagCallback(DagMessage, MDagMessage::MMessageParentChildFunction)
					switch (msg)
					{
						case MDagMessage.DagMessage.kChildAdded:
							msgName = "ChildAdded";
							dagPath.ChildAddedDagPath += userDAGChildAddedCB;
							break;
						case MDagMessage.DagMessage.kChildRemoved:
							msgName = "ChildRemoved";
							dagPath.ChildRemovedDagPath += userDAGChildRemovedCB;
							break;
						case MDagMessage.DagMessage.kChildReordered:
							msgName = "ChildRecordered";
                            dagPath.ChildReorderedDagPath += userDAGChildRemovedCB;
							break;
						case MDagMessage.DagMessage.kInstanceAdded:
							msgName = "InstanceAdded";
							dagPath.InstanceAddedDagPath += userDAGChildAddedCB;
							break;

						case MDagMessage.DagMessage.kInstanceRemoved:
							msgName = "InstanceRemoved";
							dagPath.InstanceRemovedDagPath += userDAGInstanceRemovedCB;
							break;

						case MDagMessage.DagMessage.kParentAdded:
							msgName = "ParentAdded";
							dagPath.ParentAddedDagPath += userDAGParentAddedCB;
							break;

						case MDagMessage.DagMessage.kParentRemoved:
							msgName = "ParentRemoved";
							dagPath.ParentRemovedDagPath += userDAGParentRemovedCB;
							break;
						default:
							throw new ArgumentException("Failed to add generic callback", "msg"); 
					}
					string info = string.Format("Adding a callback listening msg {0} for {1} on {2}",
						msgName, cbName, dagPath.fullPathName);
					MGlobal.displayInfo(info);
				}
				catch (System.Exception)
				{
					string err = string.Format("Could not add callback listening msg {0} to {1}",
						msgName, dagPath.fullPathName);
					throw new ApplicationException(err);
				}
			}
		}
		public override void doIt(MArgList args)
		{
			MSelectionList list = new MSelectionList();

			MArgDatabase argData;
			argData = new MArgDatabase(syntax, args);
			argData.getObjects(list);
			
			//	Get the flags
			//
			bool allDagUsed = argData.isFlagSet(kAllDagFlag);
			bool parentAddedUsed = argData.isFlagSet(kParentAddedFlag);
			bool parentRemovedUsed = argData.isFlagSet(kParentRemovedFlag);
			bool childAddedUsed = argData.isFlagSet(kChildAddedFlag);
			bool childRemovedUsed = argData.isFlagSet(kChildRemovedFlag);
			bool childReorderedUsed = argData.isFlagSet(kChildReorderedFlag);
			bool helpUsed = argData.isFlagSet(kHelpFlag);

			bool nothingSet = (!allDagUsed && !parentAddedUsed &&
								!parentRemovedUsed && !childAddedUsed &&
								!childRemovedUsed && !childReorderedUsed &&
								!helpUsed);

			if (nothingSet)
			{
				throw new ArgumentException("-A flag must be used. dagMessage -help for available flags.", "args");
			}

			if (argData.isFlagSet(kHelpFlag))
			{
				MGlobal.displayInfo("dagMessage -help");
				MGlobal.displayInfo("\tdagMessage adds a callback to the selected nodes,");
				MGlobal.displayInfo("\tor if no nodes are selected, to all nodes. The callback");
				MGlobal.displayInfo("\tprints a message when called. When the plug-in is unloaded");
				MGlobal.displayInfo("\tthe callbacks are removed.");
				MGlobal.displayInfo("");
				MGlobal.displayInfo("\t-h -help : This message is printed");
				MGlobal.displayInfo("\t-ad -allDag : parent changes and child reorders");
				MGlobal.displayInfo("\t-pa -parentAdded : A parent is added");
				MGlobal.displayInfo("\t-pr -parentRemoved : A parent is removed");
				MGlobal.displayInfo("\t-ca -childAdded : A child is added (only for individual nodes)");
				MGlobal.displayInfo("\t-cr -childRemoved : A child is removed (only for individual nodes)");
				MGlobal.displayInfo("\t-cro -childReordered : A child is reordered");
				MGlobal.displayInfo("");
			}

			uint nObjs = list.length;
			if (nObjs == 0)
			{
				//	Add the callback for all changes of the specified type.
				//
				if (allDagUsed)
				{
					try
					{
						MDagMessage.AllDagChangesEvent += userDAGGenericCB;
					}
					catch (Exception)
					{
						throw new ApplicationException("Could not add a -allDag callback");
					}
				}

				if (parentAddedUsed)
				{
					addGenericCallback(null, MDagMessage.DagMessage.kParentAdded, " parent added ");
				}

				if (parentRemovedUsed)
				{
					addGenericCallback(null, MDagMessage.DagMessage.kParentRemoved, " parent removed ");
				}

				if (childAddedUsed)
				{
					throw new ArgumentException("-childAdded can only be used when a node is selected", "args");
				}

				if (childRemovedUsed)
				{
					throw new ArgumentException("-childRemoved can only be used when a node is selected", "args");
				}

				if (childReorderedUsed)
				{
					addGenericCallback(null, MDagMessage.DagMessage.kChildReordered, " child reordered ");
				}
			}
			else
			{
				for (uint i = 0; i < nObjs; i++)
				{
					MDagPath dagPath = new MDagPath();
					list.getDagPath(i, dagPath);

					if (!dagPath.isValid)
					{
						continue;
					}

					//	Add the callback for all changes of the specified type.
					//
					if (allDagUsed)
					{
						// we don't use obsolete function 
						//  addAllDagChangesCallback
						// use addAllDagChangesDagPathCallback instead
						try
						{
							dagPath.AllDagChangesDagPath += userDAGGenericCB;
							string infoStr = string.Format("Added a callback for all Dag changes on {0}",
								dagPath.fullPathName);
							MGlobal.displayInfo(infoStr);
						}
						catch (Exception)
						{
							throw new ApplicationException("Could not add a -allDag callback");
						}
					}

					if (parentAddedUsed)
					{
						addGenericCallback(dagPath, MDagMessage.DagMessage.kParentAdded, " parent added ");   
					}

					if (parentRemovedUsed)
					{
						addGenericCallback(dagPath, MDagMessage.DagMessage.kParentRemoved, " parent removed ");
					}

					if (childAddedUsed)
					{
						addGenericCallback(dagPath,  MDagMessage.DagMessage.kChildAdded, " child added ");
					}

					if (childRemovedUsed)
					{
						addGenericCallback(dagPath,  MDagMessage.DagMessage.kChildRemoved,  " child removed ");
					}

					if (childReorderedUsed)
					{
						addGenericCallback(dagPath, MDagMessage.DagMessage.kChildReordered,  " child reordered ");
					}
				}
			}

			return;
		}

	}
}
