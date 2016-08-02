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
// This small example illustrates that a node of a particular type can be found
// by walking through the DG graph.  In this example, the nodes of type
// fileTexture has to be found.  The plugin will start from the shape node that
// the user selected, or the ones passing in as parameters.  It then look for a
// connection to the shadingEngine.  Once found, it will use a dependency graph
// iterator with a filter that matches the type desire to iterate through the
// connections to the shadingEngine.
//

using System;

using Autodesk.Maya.OpenMaya;
 

[assembly: MPxCommandClass(typeof(MayaNetPlugin.findFileTexturesCmd), "netFindFileTexturesCmdCSharp")]

namespace MayaNetPlugin
{
	public class findFileTexturesCmd : MPxCommand, IMPxCommand
	{
		public override void doIt(MArgList args)
		{
			MSelectionList list = new MSelectionList();

			if ( args.length > 0 ) {
				// Arg list is > 0 so use objects that were passes in
				//
				uint last = args.length;
				for ( uint i = 0; i < last; i++ ) {
					// Attempt to find all of the objects matched
					// by the string and add them to the list
					//
					string argStr = args.asString(i);
					list.add(argStr);
				}
			} else {
				// Get arguments from Maya's selection list.
				MGlobal.getActiveSelectionList(list);
			}

			MObject node = new MObject();
			MObjectArray nodePath = new MObjectArray();
			MFnDependencyNode nodeFn = new MFnDependencyNode();
			MFnDependencyNode dgNodeFnSet = new MFnDependencyNode();

			for (MItSelectionList iter = new MItSelectionList(list); !iter.isDone; iter.next()) {

				iter.getDependNode(node);

				//
				// The following code shows how to navigate the DG manually without
				// using an iterator.  First, find the attribute that you are
				// interested.  Then connect a plug to it and see where the plug
				// connected to.  Once you get all the connections, you can choose
				// which route you want to go.
				//
				// In here, we wanted to get to the nodes that instObjGroups connected
				// to since we know that the shadingEngine connects to the instObjGroup
				// attribute.
				//

				nodeFn.setObject( node );
				MObject iogAttr = null;
				try {
					iogAttr = nodeFn.attribute("instObjGroups");
				} catch (Exception) {
					MGlobal.displayInfo(nodeFn.name + ": is not a renderable object, skipping");
					continue;
				}

				MPlug iogPlug = new MPlug(node, iogAttr);
				MPlugArray iogConnections = new MPlugArray();

				//
				// instObjGroups is a multi attribute.  In this example, just the
				// first connection will be tried.
				//
				if (!iogPlug.elementByLogicalIndex(0).connectedTo(iogConnections, false, true)) {
					MGlobal.displayInfo(nodeFn.name + ": is not in a shading group, skipping");
					continue;
				}

				//
				// Now we would like to traverse the DG starting from the shadingEngine
				// since most likely all file texture nodes will be found.  Note the
				// filter used to initialize the DG iterator.  There are lots of filter
				// type available in MF.Type that you can choose to suite your needs.
				//
				bool foundATexture = false;
				for ( int i=0; i < iogConnections.length; i++ ) {

					MObject currentNode = iogConnections[i].node;

					//
					// Note that upon initialization, the current pointer of the
					// iterator already points to the first valid node.
					//
					MItDependencyGraph dgIt = new MItDependencyGraph(currentNode,
																	 MFn.Type.kFileTexture,
																	 MItDependencyGraph.Direction.kUpstream,
																	 MItDependencyGraph.Traversal.kBreadthFirst,
																	 MItDependencyGraph.Level.kNodeLevel);
					if (dgIt == null)
					{
						continue;
					}

					dgIt.disablePruningOnFilter();

					for ( ; !dgIt.isDone; dgIt.next() ) {

						MObject thisNode = dgIt.thisNode();
						dgNodeFnSet.setObject(thisNode);
						try {
							dgIt.getNodePath(nodePath);
						} catch (Exception) {
							MGlobal.displayInfo("getNodePath");
							continue;
						}

						//
						// append the starting node.
						//
						nodePath.append(node);
						dumpInfo( thisNode, dgNodeFnSet, nodePath );
						foundATexture = true;
					}
				}

				if ( !foundATexture ) {
					MGlobal.displayInfo(nodeFn.name + ": is not connected to a file texture");
				}
			}
			return;
		}

		private void dumpInfo(MObject fileNode, MFnDependencyNode nodeFn, MObjectArray nodePath)
		{
			MObject fileAttr = nodeFn.attribute("fileTextureName");
			MPlug plugToFile = new MPlug(fileNode, fileAttr);
			MFnDependencyNode dgFn = new MFnDependencyNode();

			MGlobal.displayInfo("Name:    " + nodeFn.name);

			MObject fnameValue = new MObject();
			try
			{
				plugToFile.getValue(fnameValue);
			}
			catch (Exception)
			{
				MGlobal.displayInfo("error getting value from plug");
				return;
			}

			MFnStringData stringFn = new MFnStringData(fnameValue);
			MGlobal.displayInfo("Texture: " + stringFn.stringProperty);

			string path = "Path:    ";
			for (int i = (int)nodePath.length - 1; i >= 0; i--)
			{
				MObject currentNode = nodePath[i];
				dgFn.setObject(currentNode);

				path += dgFn.name + "(" + dgFn.typeName + ")";
				if (i > 0) path += " -> ";
			}
			MGlobal.displayInfo(path);
		}
	}
}
