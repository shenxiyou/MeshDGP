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
using System.IO;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaUI;
 

// Example Plugin: maTranslator.cs
//
// This plugin is an example of a file translator.  Although this is not
// the actual code used by Maya when it creates files in MayaAscii format,
// it nonetheless produces a very close approximation of the of that same
// format.  Close enough that Maya can load the resulting files as if they
// were MayaAscii.
//
// Currently, the plugin does not support the following:
//
//   o  Export Selection.  The plugin will only export entire scenes.
//
//   o  Referencing files into the default namespace, or using a renaming
//      prefix.  It only supports referencing files into a separate
//      namespace.
//
//   o  MEL reference files.
//
//   o  Size hints for multi plugs.
//

[assembly: MPxFileTranslatorClass(typeof(MayaNetPlugin.maTranslator), "Maya ASCII (via csharp plugin)", null, null, null)]

namespace MayaNetPlugin
{
	public class maTranslator : MPxFileTranslator
	{
		private MPlugArray fBrokenConnSrcs;
		private MPlugArray fBrokenConnDests;

		private MObjectArray fDefaultNodes;

		private MDagPathArray fInstanceChildren;
		private MDagPathArray fInstanceParents;

		private MDagPathArray fParentingRequired;

		private uint fAttrFlag;
		private uint fCreateFlag;
		private uint fConnectionFlag;

		protected static string fExtension = "pma";
		protected static string fFileVersion = "4.5ff01";
        protected static string fPluginName = "examples";
		protected static string fTranslatorName = "Maya ASCII (via csharp plugin)";

		public maTranslator()
		{
			fBrokenConnSrcs = new MPlugArray();
			fBrokenConnDests = new MPlugArray();

			fDefaultNodes = new MObjectArray();

			fInstanceChildren = new MDagPathArray();
			fInstanceParents = new MDagPathArray();

			fParentingRequired = new MDagPathArray();
		}

		public override string defaultExtension()
		{
			return fExtension;
		}

		public override bool haveReadMethod()
		{
			return false;
		}

		public override bool haveWriteMethod()
		{
			return true;
		}

		public static void setPluginName(string name)
		{
			fPluginName = name;
		}

		public static string translatorName()
		{
			return fTranslatorName;
		}

		public static string comment(string text)
		{
			string result = "//";
			result += text;
			return result;
		}

		//
		// Maya calls this method to find out if this translator is capable of
		// handling the given file.
		//
		public override MPxFileTranslator.MFileKind identifyFile(MFileObject file, string buffer, short bufferLen)
		{
			string tagStr = comment(fTranslatorName);
			int	tagLen = tagStr.Length;

			//
			// If the buffer contains enough info to positively identify the file,
			// then use it.  Otherwise we'll base the identification on the file
			// extension.
			//
			if (bufferLen >= tagLen)
			{
				string initialContents = buffer.Substring(0,bufferLen);
				MStringArray initialLines = new MStringArray(initialContents.Split('\n'));

				//initialLines = initialContents.Split('\n');

				if (initialLines.length > 0)
				{
					if (((int)initialLines[0].Length >= tagLen)
					&&	(initialLines[0].Substring(0, tagLen-1) == tagStr))
					{
						return MPxFileTranslator.MFileKind.kIsMyFileType;
					}
				}
			}
			else
			{
				string fileName = file.name;
				int	fileNameLen = fileName.Length;
				int	startOfExtension = fileName.IndexOf('.') + 1;

				if ((startOfExtension > 0)
				&&	(startOfExtension < fileNameLen)
				&&	(fileName.Substring(startOfExtension, fileNameLen) == fExtension))
				{
					return MPxFileTranslator.MFileKind.kIsMyFileType;
				}
			}

			return MPxFileTranslator.MFileKind.kNotMyFileType;
		}

		//
		// Maya calls this method to have the translator write out a file.
		//
		public override void writer(MFileObject file, string optionsString, MPxFileTranslator.FileAccessMode mode)
		{
			//
			// For simplicity, we only do full saves/exports.
			//
			if ((mode != MPxFileTranslator.FileAccessMode.kSaveAccessMode) && (mode != MPxFileTranslator.FileAccessMode.kExportAccessMode))
                throw new System.NotImplementedException( "We only support support SaveAccessMode and ExportAccessMode");

			//
			// Let's see if we can open the output file.
			//
			FileStream output = null;
			try
			{
                output = new FileStream(file.fullName, FileMode.Create, FileAccess.Write);
			}
			catch (ArgumentException ex)
			{
				MGlobal.displayInfo("File access invalid!");
				if(output != null)
					output.Close();
				throw ex;
			}
			//
			// Get some node flags to keep track of those nodes for which we
			// have already done various stages of processing.
			//
			fCreateFlag = MFnDependencyNode.allocateFlag(fPluginName);

			fAttrFlag = MFnDependencyNode.allocateFlag(fPluginName);

			fConnectionFlag = MFnDependencyNode.allocateFlag(fPluginName);

			//
			// Run through all of the nodes in the scene and clear their flags.
			//
			MItDependencyNodes nodesIter = new MItDependencyNodes();

			for (; !nodesIter.isDone; nodesIter.next())
			{
				MObject node = nodesIter.item;
				MFnDependencyNode nodeFn = new MFnDependencyNode(node);

				nodeFn.setFlag(fCreateFlag, false);
				nodeFn.setFlag(fAttrFlag, false);
				nodeFn.setFlag(fConnectionFlag, false);
			}

			//
			// Write out the various sections of the file.
			//
			writeHeader(output, file.name);
			writeFileInfo(output);
			writeReferences(output);
			writeRequirements(output);
			writeUnits(output);
			writeDagNodes(output);
			writeNonDagNodes(output);
			writeDefaultNodes(output);
			writeReferenceNodes(output);
			writeConnections(output);
			writeFooter(output, file.name);

			output.Close();

			MFnDependencyNode.deallocateFlag(fPluginName, fCreateFlag);

			return;
		}

		protected void writeHeader(FileStream f, string fileName)
		{
			//
			// Get the current time into the same format as used by Maya ASCII
			// files.
			//
			string result = "";
			string curTime = DateTime.Now.ToString();
			//
			// Write out the header information.
			//
			result += comment(fTranslatorName) + " ";
			result += fFileVersion + " scene";
			result += "\n";
			result += comment("Name: ") + fileName;
			result += "\n";
			result += comment("Last modified: ") + curTime;
			result += "\n";

			writeString(f, result);
		}

		//
		// Write out the "fileInfo" command for the freeform information associated
		// with the scene.
		//
		protected void writeFileInfo(FileStream f)
		{
			//
			// There's no direct access to the scene's fileInfo from within the API,
			// so we have to call MEL's 'fileInfo' command.
			//
			MStringArray fileInfo = new MStringArray();

			try
			{
				MGlobal.executeCommand("fileInfo -q", fileInfo);
				uint numEntries = fileInfo.length;
				int i;
				string result = "";
				for (i = 0; i < numEntries; i += 2)
				{
					result += ("fileInfo " + quote(fileInfo[i]) + " " + quote(fileInfo[i+1]) + ";" + "\n");
				}
				writeString(f, result);
			}
			catch (Exception)
			{
				MGlobal.displayWarning("Could not get scene's fileInfo.");
			}	
		}

		//
		// Write out the "file" commands which specify the reference files used by
		// the scene.
		//
		protected void writeReferences(FileStream f)
		{
			MStringArray files = new MStringArray();

			MFileIO.getReferences(files);

			uint numRefs = files.length;
			int i;

			for (i = 0; i < numRefs; i++)
			{
				string	refCmd = "file -r";
				string	fileName = files[i];
				string	nsName = "";

				//
				// For simplicity, we assume that namespaces are always used when
				// referencing.
				//
				string tempCmd = "file -q -ns \"";
				tempCmd += fileName + "\"";

				try
				{
					MGlobal.executeCommand(tempCmd, out nsName);
					refCmd += " -ns \"";
					refCmd += nsName + "\"";
				}
				catch (Exception)
				{
					MGlobal.displayWarning("Could not get namespace name.");
				}
				//
				// Is this a deferred reference?
				//
				tempCmd = "file -q -dr \"";
				tempCmd += fileName + "\"";

				int	isDeferred;

				try
				{
					MGlobal.executeCommand(tempCmd, out isDeferred);
					if (Convert.ToBoolean(isDeferred)) refCmd += " -dr 1";
				}
				catch (Exception)
				{
					MGlobal.displayWarning("Could not get deferred reference info.");
				}
				//
				// Get the file's reference node, if it has one.
				//
				tempCmd = "file -q -rfn \"";
				tempCmd += fileName + "\"";

				string refNode;

				try
				{
					MGlobal.executeCommand(tempCmd, out refNode);
					if (refNode.Length > 0)
					{
						refCmd += " -rfn \"";
						refCmd += refNode + "\"";
					}
				}
				catch (Exception)
				{
					MGlobal.displayInfo("Could not query reference node name.");
				}
				//
				// Write out the reference command.
				//
				string result = "";
				result += (refCmd + "\"" + fileName + "\";");

				writeString(f, result);
			}
		}

		//
		// Write out the "requires" lines which specify the plugins needed by the
		// scene.
		//
		protected void writeRequirements(FileStream f)
		{
			//
			// Every scene requires Maya itself.
			//
			string result = "";
			result += ("requires maya \"" + fFileVersion + "\";\n");

			//
			// Write out requirements for each plugin.
			//
			MStringArray pluginsUsed = new MStringArray();

			try
			{
			    MGlobal.executeCommand("pluginInfo -q -pluginsInUse", pluginsUsed);
			    uint numPlugins = pluginsUsed.length;
			    int i;

			    for (i = 0; i < numPlugins; i += 2)
			    {
				    result += ("requires " + quote(pluginsUsed[i]) + " "
						    + quote(pluginsUsed[i+1]) + ";" + "\n");
			    }
			}
			catch (Exception)
			{
				MGlobal.displayWarning(
					"Could not get list of plugins currently in use."
				);
			}
			writeString(f, result);
		}

		//
		// Write out the units of measurement currently being used by the scene.
		//
		protected void writeUnits(FileStream f)
		{
			string args = "";
			string result;

			//
			// Linear units.
			//
			try
			{
				MGlobal.executeCommand("currentUnit -q -fullName -linear", out result);
				args += " -l " + result;
			}
			catch (Exception)
			{
				MGlobal.displayWarning("Could not get current linear units.");
			}
			//
			// Angular units.
			//
			try
			{
				MGlobal.executeCommand("currentUnit -q -fullName -angle",out result);
				args += " -a " + result;
			}
			catch (Exception)
			{
				MGlobal.displayWarning("Could not get current linear units.");
			}
			//
			// Time units.
			//
			try
			{
				MGlobal.executeCommand("currentUnit -q -fullName -time", out result);
				args += " -t " + result;
			}
			catch (Exception)
			{
				MGlobal.displayWarning("Could not get current linear units.");	
			}
			if (args != "")
			{
                args = "currentUnit" + args + " ;\n ";
				writeString(f, args);
			}
		}

		protected void writeDagNodes(FileStream f)
		{
			fParentingRequired.clear();

			MItDag	dagIter = new MItDag();

			dagIter.traverseUnderWorld(true);

			MDagPath worldPath = new MDagPath();

			dagIter.getPath(worldPath);

			//
			// We step over the world node before starting the loop, because it
			// doesn't get written out.
			//
			for (dagIter.next(); !dagIter.isDone; dagIter.next())
			{
				MDagPath	path = new MDagPath();
				dagIter.getPath(path);

				//
				// If the node has already been written, then all of its descendants
				// must have been written, or at least checked, as well, so prune
				// this branch of the tree from the iteration.
				//
				MFnDagNode	dagNodeFn = new MFnDagNode(path);

				if (dagNodeFn.isFlagSet(fCreateFlag))
				{
					dagIter.prune();
					continue;
				}

				//
				// If this is a default node, it will be written out later, so skip
				// it.
				//
				if (dagNodeFn.isDefaultNode) continue;

				//
				// If this node is not writable, and is not a shared node, then mark
				// it as having been written, and skip it.
				//
				if (!dagNodeFn.canBeWritten && !dagNodeFn.isShared)
				{
					dagNodeFn.setFlag(fCreateFlag, true);
					continue;
				}

				uint numParents = dagNodeFn.parentCount;

				if (dagNodeFn.isFromReferencedFile)
				{
					//
					// We don't issue 'creatNode' commands for nodes from referenced
					// files, but if the node has any parents which are not from
					// referenced files, other than the world, then make a note that
					// we'll need to issue extra 'parent' commands for it later on.
					//
					uint i;

					for (i = 0; i < numParents; i++)
					{
						MObject		altParent = dagNodeFn.parent(i);
						MFnDagNode	altParentFn = new MFnDagNode(altParent);

						if (!altParentFn.isFromReferencedFile
						&&	(altParentFn.objectProperty.notEqual(worldPath.node)))
						{
							fParentingRequired.append(path);
							break;
						}
					}
				}
				else
				{
					//
					// Find the node's parent.
					//
					MDagPath parentPath = new MDagPath(worldPath);

					if (path.length > 1)
					{
						//
						// Get the parent's path.
						//
						parentPath.assign(path);
						parentPath.pop();

						//
						// If the parent is in the underworld, then find the closest
						// ancestor which is not.
						//
						if (parentPath.pathCount > 1)
						{
							//
							// The first segment of the path contains whatever
							// portion of the path exists in the world.  So the closest
							// worldly ancestor is simply the one at the end of that
							// first path segment.
							//
							path.getPath(parentPath, 0);
						}
					}

					MFnDagNode	parentNodeFn = new MFnDagNode(parentPath);

					if (parentNodeFn.isFromReferencedFile)
					{
						//
						// We prefer to parent to a non-referenced node.  So if this
						// node has any other parents, which are not from referenced
						// files and have not already been processed, then we'll
						// skip this instance and wait for an instance through one
						// of those parents.
						//
						uint i;

						for (i = 0; i < numParents; i++)
						{
							if (dagNodeFn.parent(i).notEqual(parentNodeFn.objectProperty))
							{
								MObject		altParent = dagNodeFn.parent(i);
								MFnDagNode	altParentFn = new MFnDagNode(altParent);

								if (!altParentFn.isFromReferencedFile
								&&	!altParentFn.isFlagSet(fCreateFlag))
								{
									break;
								}
							}
						}

						if (i < numParents) continue;

						//
						// This node only has parents within referenced files, so
						// create it without a parent and note that we need to issue
						// 'parent' commands for it later on.
						//
						writeCreateNode(f, path, worldPath);

						fParentingRequired.append(path);
					}
					else
					{
						writeCreateNode(f, path, parentPath);

						//
						// Let's see if this node has any parents from referenced
						// files, or any parents other than this one which are not
						// from referenced files.
						//
						uint i;
						bool hasRefParents = false;
						bool hasOtherNonRefParents = false;

						for (i = 0; i < numParents; i++)
						{
							if (dagNodeFn.parent(i).notEqual(parentNodeFn.objectProperty))
							{
								MObject		altParent = dagNodeFn.parent(i);
								MFnDagNode	altParentFn = new MFnDagNode(altParent);

								if (altParentFn.isFromReferencedFile)
									hasRefParents = true;
								else
									hasOtherNonRefParents = true;

								//
								// If we've already got positives for both tests,
								// then there's no need in continuing.
								//
								if (hasRefParents && hasOtherNonRefParents) break;
							}
						}

						//
						// If this node has parents from referenced files, then
						// make note that we will have to issue 'parent' commands
						// later on.
						//
						if (hasRefParents) fParentingRequired.append(path);

						//
						// If this node has parents other than this one which are
						// not from referenced files, then make note that the
						// parenting for the other instances still has to be done.
						//
						if (hasOtherNonRefParents)
						{
							fInstanceChildren.append(path);
							fInstanceParents.append(parentPath);
						}
					}

					//
					// Write out the node's 'addAttr', 'setAttr' and 'lockNode'
					// commands.
					//
					writeNodeAttrs(f, path.node, true);
					writeLockNode(f, path.node);
				}

				//
				// Mark the node as having been written.
				//
				dagNodeFn.setFlag(fCreateFlag, true);
			}

			//
			// Write out the parenting for instances.
			//
			writeInstances(f);
		}

		//
		// If a DAG node is instanced (i.e. has multiple parents), this method
		// will put it under its remaining parents.  It will already have been put
		// under its first parent when it was created.
		//
		protected void writeInstances(FileStream f)
		{
			uint numInstancedNodes = fInstanceChildren.length;
			int i;

			for (i = 0; i < numInstancedNodes; i++)
			{
                MFnDagNode nodeFn = new MFnDagNode(fInstanceChildren[i]);

                uint numParents = nodeFn.parentCount;
                uint p;

                for (p = 0; p < numParents; p++)
                {
                    //
                    // We don't want to issue a 'parent' command for the node's
                    // existing parent.
                    //
                    try
                    {
                        if (nodeFn.parent((uint)i).notEqual(fInstanceParents[i].node))
                        {
                            MObject parent = nodeFn.parent((uint)i);
                            MFnDagNode parentFn = new MFnDagNode(parent);

                            if (!parentFn.isFromReferencedFile)
                            {
                                //
                                // Get the first path to the parent node.
                                //
                                MDagPath parentPath = new MDagPath();

                                MDagPath.getAPathTo(parentFn.objectProperty, parentPath);

                                writeParent(f, parentPath, fInstanceChildren[i], true);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }

			//
			// We don't need this any more, so free up the space.
			//
			fInstanceChildren.clear();
			fInstanceParents.clear();
		}

		//
		// Write out a 'parent' command to parent one DAG node under another.
		//
		protected void writeParent(
				FileStream f, MDagPath parent, MDagPath child, bool addIt
		)
		{
			string result = "";
			result += "parent -s -nc -r ";
 
			//
			// If this is not the first parent then we have to include the "-a/add"
			// flag.
			//
			if (addIt) 
				result += "-a ";

			//
			// If the parent is the world, then we must include the "-w/world" flag.
			//
			if (parent.length == 0) 
				result += "-w ";

			result += ("\"" + child.partialPathName + "\"");

			//
			// If the parent is NOT the world, then give the parent's name.
			//
			if (parent.length != 0)
				result += (" \"" + parent.partialPathName + "\"");

			result +=  (";" + "\n");

			writeString(f, result);
		}

		protected void writeNonDagNodes(FileStream f)
		{
			MItDependencyNodes	nodeIter = new MItDependencyNodes();

			for (; !nodeIter.isDone; nodeIter.next())
			{
				MObject				node = nodeIter.item;
				MFnDependencyNode	nodeFn = new MFnDependencyNode(node);

				//
				// Save default nodes for later processing.
				//
				if (nodeFn.isDefaultNode)
				{
					fDefaultNodes.append(node);
				}
				else if (!nodeFn.isFromReferencedFile
				&&	!nodeFn.isFlagSet(fCreateFlag))
				{
					//
					// If this node is either writable or shared, then write it out.
					// Otherwise don't, but still mark it as having been written so
					// that we don't end up processing it again at some later time.
					//
					if (nodeFn.canBeWritten || nodeFn.isShared)
					{
						writeCreateNode(f, node);
						writeNodeAttrs(f, node, true);
						writeLockNode(f, node);
					}

					nodeFn.setFlag(fCreateFlag, true);
					nodeFn.setFlag(fAttrFlag, true);
				}
			}
		}

		protected void writeDefaultNodes(FileStream f)
		{
			uint numNodes = fDefaultNodes.length;
			int i;

			for (i = 0; i < numNodes; i++)
			{
				writeNodeAttrs(f, fDefaultNodes[i], false);

				MFnDependencyNode	nodeFn = new MFnDependencyNode(fDefaultNodes[i]);

				nodeFn.setFlag(fAttrFlag, true);
			}
		}

		//
		// Write out the 'addAttr' and 'setAttr' commands for a node.
		//
		protected void writeNodeAttrs(FileStream f, MObject node, bool isSelected)
		{
			MFnDependencyNode	nodeFn = new MFnDependencyNode(node);

			if (nodeFn.canBeWritten)
			{
				MStringArray	addAttrCmds = new MStringArray();
				MStringArray	setAttrCmds = new MStringArray();

				getAddAttrCmds(node, addAttrCmds);
				getSetAttrCmds(node, setAttrCmds);

				uint numAddAttrCmds = addAttrCmds.length;
				uint numSetAttrCmds = setAttrCmds.length;

				if (numAddAttrCmds + numSetAttrCmds > 0)
				{
					//
					// If the node is not already selected, then issue a command to
					// select it.
					//
					if (!isSelected) writeSelectNode(f, node);

					int i;
					string result = "";
					for (i = 0; i < numAddAttrCmds; i++)
						result += (addAttrCmds[i] + "\n");

					for (i = 0; i < numSetAttrCmds; i++)
						result += (setAttrCmds[i] + "\n");

					writeString(f, result);
				}
			}
		}

		protected void writeReferenceNodes(FileStream f)
		{
			//
			// We don't write out createNode commands for reference nodes, but
			// we do write out parenting between them and non-reference nodes,
			// as well as attributes added and attribute values changed after the
			// referenced file was loaded
			//
			writeRefNodeParenting(f);

			//
			// Output the commands for DAG nodes first.
			//
			MItDag	dagIter = new MItDag();

			for (dagIter.next(); !dagIter.isDone; dagIter.next())
			{
				MObject				node = dagIter.item();
				MFnDependencyNode	nodeFn = new MFnDependencyNode(node);

				if (nodeFn.isFromReferencedFile
				&&	!nodeFn.isFlagSet(fAttrFlag))
				{
					writeNodeAttrs(f, node, false);

					//
					// Make note of any connections to this node which have been
					// broken by the main scene.
					//
					MFileIO.getReferenceConnectionsBroken(
						node, fBrokenConnSrcs, fBrokenConnDests, true, true
					);

					nodeFn.setFlag(fAttrFlag, true);
				}
			}

			//
			// Now do the remaining, non-DAG nodes.
			//
			MItDependencyNodes	nodeIter = new MItDependencyNodes();

			for (; !nodeIter.isDone; nodeIter.next())
			{
				MObject				node = nodeIter.item;
				MFnDependencyNode	nodeFn = new MFnDependencyNode(node);

				if (nodeFn.isFromReferencedFile
				&&	!nodeFn.isFlagSet(fAttrFlag))
				{
					writeNodeAttrs(f, node, false);

					//
					// Make note of any connections to this node which have been
					// broken by the main scene.
					//
					MFileIO.getReferenceConnectionsBroken(
						node, fBrokenConnSrcs, fBrokenConnDests, true, true
					);

					nodeFn.setFlag(fAttrFlag, true);
				}
			}
		}

		//
		// Write out all of the connections in the scene.
		//
		protected void writeConnections(FileStream f)
		{
			//
			// If the scene has broken any connections which were made in referenced
			// files, handle those first so that the attributes are free for any new
			// connections which may come along.
			//
			writeBrokenRefConnections(f);

			//
			// We're about to write out the scene's connections in three parts: DAG
			// nodes, non-DAG non-default nodes, then default nodes.
			//
			// It's really not necessary that we group them like this and would in
			// fact be more efficient to do them all in one MItDependencyNodes
			// traversal.  However, this is the order in which the normal MayaAscii
			// translator does them, so this makes it easier to compare the output
			// of this translator to Maya's output.
			//

			//
			// Write out connections for the DAG nodes first.
			//
			MItDag	dagIter = new MItDag();
			dagIter.traverseUnderWorld(true);

			for (dagIter.next(); !dagIter.isDone; dagIter.next())
			{
				MObject		node = dagIter.item();
				MFnDagNode	dagNodeFn = new MFnDagNode(node);

				if (!dagNodeFn.isFlagSet(fConnectionFlag)
				&&	dagNodeFn.canBeWritten
				&&	!dagNodeFn.isDefaultNode)
				{
					writeNodeConnections(f, dagIter.item());
					dagNodeFn.setFlag(fConnectionFlag, true);
				}
			}

			//
			// Now do the non-DAG, non-default nodes.
			//
			MItDependencyNodes	nodeIter = new MItDependencyNodes();

			for (; !nodeIter.isDone; nodeIter.next())
			{
				MFnDependencyNode	nodeFn = new MFnDependencyNode(nodeIter.item);

				if (!nodeFn.isFlagSet(fConnectionFlag)
				&&	nodeFn.canBeWritten
				&&	!nodeFn.isDefaultNode)
				{
					writeNodeConnections(f, nodeIter.item);
					nodeFn.setFlag(fConnectionFlag, true);
				}
			}

			//
			// And finish up with the default nodes.
			//
			uint numNodes = fDefaultNodes.length;
			int i;

			for (i = 0; i < numNodes; i++)
			{
				MFnDependencyNode	nodeFn = new MFnDependencyNode(fDefaultNodes[i]);

				if (!nodeFn.isFlagSet(fConnectionFlag)
				&&	nodeFn.canBeWritten
				&&	nodeFn.isDefaultNode)
				{
					writeNodeConnections(f, fDefaultNodes[i]);
					nodeFn.setFlag(fConnectionFlag, true);
				}
			}
		}

		//
		// Write the 'disconnectAttr' statements for those connections which were
		// made in referenced files, but broken in the main scene.
		//
		protected void writeBrokenRefConnections(FileStream f)
		{
			uint numBrokenConnections = fBrokenConnSrcs.length;
			int i;
			string result = "";
			for (i = 0; i < numBrokenConnections; i++)
			{
				result += ("disconnectAttr \"" 
					+  fBrokenConnSrcs[i].partialName(true) 
					+ "\" \"" + fBrokenConnDests[i].partialName(true) 
					+ "\"");
				//
				// If the destination plug is a multi for which index does not
				// matter, then we must add a "-na/nextAvailable" flag to the
				// command.
				//
				MObject			attr = fBrokenConnDests[i].attribute;
				MFnAttribute	attrFn = new MFnAttribute(attr);

				if (!attrFn.indexMatters) result += " -na";

				result += (";" + "\n");
			}
			writeString(f, result);
		}

		//
		// Write the 'connectAttr' commands for all of a node's incoming
		// connections.
		//
		protected void writeNodeConnections(FileStream f, MObject node)
		{
			MFnDependencyNode	nodeFn = new MFnDependencyNode(node);
			MPlugArray			plugs = new MPlugArray();

            try
            {
                nodeFn.getConnections(plugs);
            }
            catch (Exception)
            {
            }

			uint numBrokenConns = fBrokenConnSrcs.length;
			uint numPlugs = plugs.length;
			int i;
			string result = "";
			for (i = 0; i < numPlugs; i++)
			{
				//
				// We only care about connections where we are the destination.
				//
				MPlug		destPlug = plugs[i];
				MPlugArray	srcPlug = new MPlugArray();

				destPlug.connectedTo(srcPlug, true, false);

				if (srcPlug.length > 0)
				{
					MObject				srcNode = srcPlug[0].node;
					MFnDependencyNode	srcNodeFn = new MFnDependencyNode(srcNode);

					//
					// Don't write the connection if the source is not writable...
					//
					if (!srcNodeFn.canBeWritten) continue;

					//
					// or the connection was made in a referenced file...
					//
					if (destPlug.isFromReferencedFile) continue;

					//
					// or the plug is procedural...
					//
					if (destPlug.isProcedural) continue;

					//
					// or it is a connection between a default node and a shared
					// node (because those will get set up automatically).
					//
					if (srcNodeFn.isDefaultNode && nodeFn.isShared) continue;

					result += "connectAttr \"";

					//
					// Default nodes get a colon at the start of their names.
					//
					if (srcNodeFn.isDefaultNode) result += ":";

					result += (srcPlug[0].partialName(true)
					  + "\" \"");

					if (nodeFn.isDefaultNode) result += ":";

					result += (destPlug.partialName(true)
					  + "\"");

					//
					// If the src plug is also one from which a broken
					// connection originated, then add the "-rd/referenceDest" flag
					// to the command.  That will help Maya to better adjust if the
					// referenced file has changed the next time it is loaded.
					//
					if (srcNodeFn.isFromReferencedFile)
					{
						int j;

						for (j = 0; j < numBrokenConns; j++)
						{
							if (fBrokenConnSrcs[j] == srcPlug[0])
							{
								result += (" -rd \""
								  + fBrokenConnDests[j].partialName(true)
								  + "\"");

								break;
							}
						}
					}

					//
					// If the plug is locked, then add a "-l/lock" flag to the
					// command.
					//
					if (destPlug.isLocked) result += " -l on";

					//
					// If the destination attribute is a multi for which index
					// does not matter, then we must add the "-na/nextAvailable"
					// flag to the command.
					//
					MObject			attr = destPlug.attribute;
					MFnAttribute	attrFn = new MFnAttribute(attr);

					if (!attrFn.indexMatters) result += " -na";

					result += (";" + "\n");
				}
			}
			writeString(f, result);
		}

		//
		// Write out a 'createNode' command for a DAG node.
		//
		protected void writeCreateNode(FileStream f, MDagPath nodePath, MDagPath parentPath)
		{
			MObject node = new MObject(nodePath.node);
			MFnDagNode	nodeFn = new MFnDagNode(node);

			string result = "";
			//
			// Write out the 'createNode' command for this node.
			//
			result += ("createNode " + nodeFn.typeName);

			//
			// If the node is shared, then add a "-s/shared" flag to the command.
			//
			if (nodeFn.isShared) result += " -s";

			result += (" -n \"" + nodeFn.name + "\"");

			//
			// If this is not a top-level node, then include its first parent in the
			// command.
			//
			if (parentPath.length > 0)
				result += (" -p \"" + parentPath.partialPathName + "\"");

			result += (";" + "\n");

			writeString(f, result);
		}

		//
		// Write out a 'createNode' command for a non-DAG node.
		//
		protected void writeCreateNode(FileStream f, MObject node)
		{
			MFnDependencyNode	nodeFn = new MFnDependencyNode(node);
			string result = "";
			//
			// Write out the 'createNode' command for this node.
			//
			result += ("createNode " + nodeFn.typeName);

			//
			// If the node is shared, then add a "-s/shared" flag to the command.
			//
			if (nodeFn.isShared) result += " -s";

			result += (" -n \"" + nodeFn.name + "\";" + "\n");
			writeString(f, result);
		}

		//
		// Write out a "lockNode" command.
		//
		protected void writeLockNode(FileStream f, MObject node)
		{
			MFnDependencyNode	nodeFn = new MFnDependencyNode(node);

			//
			// By default, nodes are not locked, so we only have to issue a
			// "lockNode" command if the node is locked.
			//
			if (nodeFn.isLocked) 
				writeString(f, "lockNode;\n");
		}

		//
		// Write out a "select" command.
		//
		protected void writeSelectNode(FileStream f, MObject node)
		{
			MFnDependencyNode	nodeFn = new MFnDependencyNode(node);
			string				nodeName;

			//
			// If the node has a unique name, then we can just go ahead and use
			// that.  Otherwise we will have to use part of its DAG path to to
			// distinguish it from the others with the same name.
			//
			if (nodeFn.hasUniqueName)
				nodeName = nodeFn.name;
			else
			{
				//
				// Only DAG nodes are allowed to have duplicate names.
				//
				try
				{
					MFnDagNode dagNodeFn = new MFnDagNode(node);
					nodeName = dagNodeFn.partialPathName;
				}
				catch (Exception)
				{
					MGlobal.displayWarning(
						"Node '" + nodeFn.name
						+ "' has a non-unique name but claimes to not be a DAG node.\n"
						+ "Using non-unique name."
					);

					nodeName = nodeFn.name;
				}
			}
			string result = "";
			//
			// We use the "-ne/noExpand" flag so that if the node is a set, we
			// actually select the set itself, rather than its members.
			//
			result += "select -ne ";

			//
			// Default nodes get a colon slapped onto the start of their names.
			//
			if (nodeFn.isDefaultNode) result += ":";

			result += (nodeName + ";\n");
			writeString(f, result);
		}

		//
		// Deal with nodes whose parenting is between referenced and non-referenced
		// nodes.
		//
		protected void writeRefNodeParenting(FileStream f)
		{
			uint numNodes = fParentingRequired.length;
			int i;

			for (i = 0; i < numNodes; i++)
			{
				MFnDagNode	nodeFn = new MFnDagNode(fParentingRequired[i]);

				//
				// Find out if this node has any parents from referenced or
				// non-referenced files.
				//
				bool hasRefParents = false;
				bool hasNonRefParents = false;
				uint numParents = nodeFn.parentCount;
				uint p;

				for (p = 0; p < numParents; p++)
				{
					MObject		parent = nodeFn.parent(p);
					MFnDagNode	parentFn = new MFnDagNode(parent);

					if (parentFn.isFromReferencedFile)
						hasRefParents = true;
					else
						hasNonRefParents = true;

					if (hasRefParents && hasNonRefParents) break;
				}

				//
				// If this node is from a referenced file and it has parents which
				// are also from a referenced file, then it already has its first
				// parent and all others are added instances.
				//
				// Similarly if the node is not from a referenced file and has
				// parents which are also not from referenced files.
				//
				bool	alreadyHasFirstParent =
					(nodeFn.isFromReferencedFile ? hasRefParents : hasNonRefParents);

				//
				// Now run through the parents again and output any parenting
				// which involves a non-referenced node, either as parent or child.
				//
				for (p = 0; p < numParents; p++)
				{
					MObject		parent = nodeFn.parent(p);
					MFnDagNode	parentFn = new MFnDagNode(parent);

					if (parentFn.isFromReferencedFile != nodeFn.isFromReferencedFile)
					{
						//
						// Get the first path to the parent.
						//
						MDagPath	parentPath = new MDagPath();
						MDagPath.getAPathTo(parentFn.objectProperty, parentPath);

						writeParent(
							f, parentPath, fParentingRequired[i], alreadyHasFirstParent
						);

						//
						// If it didn't have its first parent before, it does now.
						//
						alreadyHasFirstParent = true;
					}
				}
			}
		}

		protected void writeFooter(FileStream f, string fileName)
		{
			writeString(f, comment(" End of ") + fileName + "\n");
		}

		protected void getAddAttrCmds(MObject node, MStringArray cmds)
		{
			//
			// Run through the node's attributes.
			//
			MFnDependencyNode	nodeFn = new MFnDependencyNode(node);
			uint numAttrs = nodeFn.attributeCount;
			uint i;

			for (i = 0; i < numAttrs; i++)
			{
				//
				// Use the attribute ordering which Maya uses when doing I/O.
				//
				MObject	attr = nodeFn.reorderedAttribute(i);

				//
				// If this attribute has been added since the node was created,
				// then we may want to write out an addAttr statement for it.
				//
				if (nodeFn.isNewAttribute(attr))
				{
					MFnAttribute	attrFn = new MFnAttribute(attr);

					//
					// If the attribute has a parent then ignore it because it will
					// be processed when we process the parent.
					//
                    bool bFound;
					attrFn.parent(out bFound);
					if ( !bFound )
					{
							//
						// If the attribute is a compound, then we can do its entire
						// tree at once.
						//
						try
						{
							MFnCompoundAttribute	cAttrFn = new MFnCompoundAttribute(attr);
							MStringArray	newCmds = new MStringArray();

							cAttrFn.getAddAttrCmds(newCmds);

							uint	numCommands = newCmds.length;
							int	c;

							for (c = 0; c < numCommands; c++)
							{
								if (newCmds[c] != "")
									cmds.append(newCmds[c]);
							}
						}
						catch (Exception)
						{
							string	newCmd = attrFn.getAddAttrCmd();
						
							if (newCmd != "") cmds.append(newCmd);
						}
					}
				}
			}
		}

		protected void getSetAttrCmds(MObject node, MStringArray cmds)
		{
			//
			// Get rid of any garbage already in the array.
			//
			cmds.clear();

			//
			// Run through the node's attributes.
			//
			MFnDependencyNode	nodeFn = new MFnDependencyNode(node);
			uint numAttrs = nodeFn.attributeCount;
			uint i;

			for (i = 0; i < numAttrs; i++)
			{
				//
				// Use the attribute ordering which Maya uses when doing I/O.
				//
				MObject			attr = nodeFn.reorderedAttribute(i);
				MFnAttribute	attrFn = new MFnAttribute(attr);
                bool            isChild;
				attrFn.parent(out isChild);
				
				//
				// We don't want attributes which are children of other attributes
				// because they will be processed when we process the parent.
				//
				// And we only want storable attributes which accept inputs.
				//
				if (!isChild && attrFn.isStorable && attrFn.isWritable)
				{
					//
					// Get a plug for the attribute.
					//
					MPlug	plug = new MPlug(node, attr);

					//
					// Get setAttr commands for this attribute, and any of its
					// children, which have had their values changed by the scene.
					//
					MStringArray	newCmds = new MStringArray();

					plug.getSetAttrCmds(newCmds, MPlug.MValueSelector.kChanged, false);

					uint numCommands = newCmds.length;
					int c;

					for (c = 0; c < numCommands; c++)
					{
						if (newCmds[c] != "")
							cmds.append(newCmds[c]);
					}
				}
			}
		}

		public override void reader(MFileObject file, string optionsString, MPxFileTranslator.FileAccessMode mode)
		{
            throw new System.NotImplementedException("We only support support writer not reader");
        }
		//
		// Convert a string into a quoted, printable string.
		//
		protected static string quote(string str)
		{
			string cstr = str;
			int	strLen = str.Length;
			int i;

			string	result = "\"";

			for (i = 0; i < strLen; i++)
			{
				int c = cstr[i];

				if (c >= 0x20 && c <= 0x7e)
				{
					//
					// Because backslash and double-quote have special meaning
					// within a printable string, we have to turn those into escape
					// sequences.
					//
					switch (c)
					{
						case '"':
							result += "\\\"";
							break;

						case '\\':
							result += "\\\\";
							break;

						default:
							result += cstr[i];
							break;
					}
				}
				else
				{
					//
					// Convert non-printable characters into escape sequences.
					//
					switch (c)
					{
						case '\n':
							result += "\\n";
						break;

						case '\t':
							result += "\\t";
						break;

						case '\b':
							result += "\\b";
						break;

						case '\r':
							result += "\\r";
						break;

						case '\f':
							result += "\\f";
						break;

						case '\v':
							result += "\\v";
						break;

						case '\a':
							result += "\\a";
							break;

						default:
						{
							//
							// Encode it as an octal escape sequence.
							//
							string res = string.Format("\\{0:o}", c);
							result += res;
							break;
						}
					}
				}
			}

			//
			// Add closing quote.
			//
			result += "\"";

			return result;
		}

		private void writeString(FileStream f, string s)
		{
			byte[] byteData = new ASCIIEncoding().GetBytes(s);
			f.Write(byteData, 0, byteData.Length);
		}
	}
}
