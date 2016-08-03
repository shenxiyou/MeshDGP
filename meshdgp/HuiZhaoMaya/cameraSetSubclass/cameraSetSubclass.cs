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
//		This C# plugin is ported from: $(MAYADIR)\devkit\plug-ins\cameraSetSubclass
//
// Description:
//     Sample plug-in that exercises the exCameraSet class
//
//	   testExCameraSetCSharp -help will list the options.
//
// Example usages:
//     testExCameraSetCSharp -c;
//     testExCameraSetCSharp -ac persp exCameraSetCSharp1;
//     testExCameraSetCSharp -ac top exCameraSetCSharp1;
//     testExCameraSetCSharp -d 0 exCameraSetCSharp1;
//     testExCameraSetCSharp -q -camera -layer 0 exCameraSetCSharp1;
//     testExCameraSetCSharp -e -camera side -layer 0 exCameraSetCSharp1;
//     testExCameraSetCSharp -e -set "" -layer 0 exCameraSetCSharp1;
//     testExCameraSetCSharp -q -set -layer 0 exCameraSetCSharp1;
//     testExCameraSetCSharp -e -set "" -layer 0 exCameraSetCSharp1;
//     testExCameraSetCSharp -e -layerType Left -layer 0 exCameraSetCSharp1;
//     testExCameraSetCSharp -q -layerType -layer 0 exCameraSetCSharp1;
//     testExCameraSetCSharp -e -active true -layer 0 exCameraSetCSharp1;
//     testExCameraSetCSharp -q -active -layer 0 exCameraSetCSharp1;
//     testExCameraSetCSharp -q -numLayers exCameraSetCSharp1;
//

using System;
using System.Collections.Generic;

using Autodesk.Maya.OpenMaya;


[assembly: MPxCommandClass(typeof(MayaNetPlugin.testCameraSetCmd), "testExCameraSetCSharp")]
[assembly: MPxNodeClass(typeof(MayaNetPlugin.exCameraSet), "exCameraSetCSharp", 0x0008105c, 
    NodeType = Autodesk.Maya.OpenMaya.MPxNode.NodeType.kCameraSetNode)]

namespace MayaNetPlugin
{
	class exCameraSet : MPxCameraSet, IMPxNode
	{
		//[MPxNodeEnum("lt", "layerType")]
		//[MPxEnumField("Mono")]
		//[MPxEnumField("Left")]
		//[MPxEnumField("Right")]
		public static MObject layerTypeAttr = null;
		public static MTypeId type_id = new MTypeId(0x0008105c);
        private Dictionary<uint, int> layeTypeDic = new Dictionary<uint, int>();

        [MPxNodeInitializer()]
		public static bool initializer()
		{
			MFnEnumAttribute enumAttr = new MFnEnumAttribute();

			// Create the layerType attribute and define its enum values.
			//
			exCameraSet.layerTypeAttr = enumAttr.create("layerType", "lt", 0);

			enumAttr.addField("Mono", 0);
			enumAttr.addField("Left", 1);
			enumAttr.addField("Right", 2);

			// Make this attribute a multi so it can store a value per camera layer.
			//
			enumAttr.isArray = true;
			return true;
		}

		public void setLayerType(uint index, int layerType)
		{
			// Get the layerType plug for the given index.
			//
			MPlug layerTypePlug = null;

			// Set the value of the plug to the new value.
			//
			if (getLayerTypePlug(index, out layerTypePlug))
			{
                layeTypeDic.Add(index, layerType);
			}
			
		}

		public int getLayerType(uint index)
		{
			int layType = -1;
			MPlug layerTypePlug = null;

			// Get the layerType plug for the given index.
			//
			if (getLayerTypePlug(index, out layerTypePlug))
                layeTypeDic.TryGetValue(index, out layType);

			return layType;
		}

		public bool getLayerTypePlug(uint index, out MPlug layerTypePlug)
		{
			try
			{
				MPlug enumPlug = new MPlug(thisMObject(), exCameraSet.layerTypeAttr);
				layerTypePlug = enumPlug.elementByLogicalIndex(index);
				return true;
			}
			catch (System.Exception)
			{
				layerTypePlug = null;
				return false;
			}
		}
	}


	class testCameraSetCmd : MPxCommand, IMPxCommand
	{
		const string kEditFlag = "-e";
		const string kEditFlagLong = "-edit";
		const string kQueryFlag = "-q";
		const string kQueryFlagLong = "-query";
		const string kActiveFlag = "-a";
		const string kActiveFlagLong = "-active";
		const string kCameraFlag = "-cam";
		const string kCameraFlagLong = "-camera";
		const string kCreateFlag = "-c";
		const string kCreateFlagLong = "-create";
		const string kAppendCameraFlag = "-ac";
		const string kAppendCameraFlagLong = "-appendCamera";
		const string kAppendCameraAndSetFlag = "-acs";
		const string kAppendCameraAndSetFlagLong = "-appendCameraAndSet";
		const string kDeleteLayerFlag = "-d";
		const string kDeleteLayerFlagLong = "-deleteLayer";
		const string kNumLayersFlag = "-nl";
		const string kNumLayersFlagLong = "-numLayers";
		const string kLayerFlag = "-l";
		const string kLayerFlagLong = "-layer";
		const string kLayerTypeFlag = "-lt";
		const string kLayerTypeFlagLong = "-layerType";
		const string kSetFlag = "-s";
		const string kSetFlagLong = "-set";
		const string kHelpFlag = "-h";
		const string kHelpFlagLong = "-help";

		bool createdUsed;
		bool editUsed;
		bool queryUsed;
		bool activeUsed;
		bool appendCameraUsed;
		bool appendCameraAndSetUsed;
		bool cameraUsed;
		bool deleteLayerUsed;
		bool numLayersUsed;
		bool layerUsed;
		bool layerTypeUsed;
		bool setUsed;
		bool helpUsed;

		int cameraLayer;
		bool activeVal;
		string camName;
		string setName;
		string layerTypeVal;
		MSelectionList list = new MSelectionList();

		override public void doIt(MArgList args)
		// Parses the given command line arguments and executes them.
		//
		{
			parseArgs(args);

			bool nothingSet = (!createdUsed && !appendCameraUsed && !appendCameraAndSetUsed && !deleteLayerUsed && !cameraUsed &&
						!layerUsed && !helpUsed && !setUsed && !layerTypeUsed && !numLayersUsed);

			if (nothingSet)
			{
				throw new ArgumentException("A flag must be used. testCameraSet -help for available flags", "args"); 
			}

			if (helpUsed)
			{
				MGlobal.displayInfo("testExCameraSet -help");
				MGlobal.displayInfo("\ttestExCameraSet tests the functionality of the exCameraSet node.");
				MGlobal.displayInfo("");
				MGlobal.displayInfo("\t-h -help : This message is printed");
				MGlobal.displayInfo("\t-a -active [true/false]: Set/get whether a particular layer is active");
				MGlobal.displayInfo("\t-ac -appendCamera <cameraName>: Append a new camera layer to the cameraSet using the specified camera");
				MGlobal.displayInfo("\t-acs -appendCameraAndSet <cameraName> <setName>: Append a new camera layer to the cameraSet using the specified camera and set");
				MGlobal.displayInfo("\t-cam -camera [<cameraName>]: Set/get the camera for a particular layer");
				MGlobal.displayInfo("\t-c -create : Create a new cameraSet node");
				MGlobal.displayInfo("\t-d -deleteLayer <layerIndex>: Delete the layer at the given index");
				MGlobal.displayInfo("\t-nl -numLayers: Returns the number of layers defined in the specified cameraSet");
				MGlobal.displayInfo("\t-l -layer <layerIndex>: Specifies the layer index to be used when accessing layer information");
				MGlobal.displayInfo("\t-lt -layerType [<layerTypeName>]: Set/get the layer type for a particular layer.  Possible values are Mono, Left, and Right.");
				MGlobal.displayInfo("\t-s -set [<setName>]: Set/get the set for a particular layer");
				MGlobal.displayInfo("\t-e -edit : Perform an edit operation");
				MGlobal.displayInfo("\t-q -query : Perform a query operation");
				MGlobal.displayInfo("");
			}

			uint nObjs = list.length;
			if (nObjs == 0)
			{
				if (createdUsed)
				{
					// Create a new cameraSet node.
					//
					MFnDependencyNode dirFn = new MFnDependencyNode();
					string noName = "";
					try
					{
						MObject dirObj = dirFn.create(exCameraSet.type_id, noName);
						MGlobal.select(dirObj, MGlobal.ListAdjustment.kReplaceList);
					}
					catch (System.Exception ex)
					{
						throw new ApplicationException("Could not create a cameraSet node", ex);
					}
					return;
				}

				if (appendCameraUsed || appendCameraAndSetUsed || deleteLayerUsed || editUsed || cameraUsed ||
				setUsed || layerTypeUsed || activeUsed || numLayersUsed)
				{
					throw new ArgumentException("Must specify a cameraSet node", "args"); 
				}
			}
			else
			{
				if (createdUsed)
				{
					throw new ArgumentException("-create cannot have any object specified", "args"); 
				}

				if (appendCameraUsed)
				{
					if (nObjs != 1)
					{
						throw new ArgumentException("-appendCamera must have a single cameraSet node specified", "args"); 
					}

					// Get the specified cameraSet node.
					//
					MObject dirNode = MObject.kNullObj;
					if (!getExCameraSetNode(dirNode))
					{
						throw new ArgumentException("-appendCamera must have a valid exCameraSet node specified", "args"); 
					}

					// Get a dag path to the specified camera.
					//
					MSelectionList camList = new MSelectionList();
					camList.add(camName);
					MDagPath camPath = new MDagPath();
					camList.getDagPath(0, camPath);
					if (!camPath.isValid)
					{
						throw new ArgumentException("-appendCamera must have a valid camera node specified", "args"); 
					}

					// Call the MFnCameraSet method to append the layer.
					//
					MFnCameraSet dirFn = new MFnCameraSet(dirNode);
					dirFn.appendLayer(camPath, MObject.kNullObj);

					return;
				}

				if (appendCameraAndSetUsed)
				{
					if (nObjs != 1)
					{
						throw new ArgumentException("-appendCameraAndSet must have a single cameraSet node specified", "args"); 
					}

					// Get the specified cameraSet node.
					//
					MObject dirNode = MObject.kNullObj;
					if (!getExCameraSetNode(dirNode))
					{
						throw new ArgumentException("-appendCameraAndSet must have a valid exCameraSet node specified", "args"); 
					}

					// Get a dag path to the specified camera.
					//
					MSelectionList camList = new MSelectionList();
					camList.add(camName);
					MDagPath camPath = new MDagPath();
					camList.getDagPath(0, camPath);
					if (!camPath.isValid)
					{
						throw new ArgumentException("-appendCameraAndSet must have a valid camera node specified", "args"); 
					}

					// Get the specified set node.
					//
					MSelectionList setList = new MSelectionList();
					setList.add(setName);
					MObject setObj = MObject.kNullObj;
					setList.getDependNode(0, setObj);
					if (setObj == MObject.kNullObj)
					{
						throw new ArgumentException("-appendCameraAndSet must have a valid set node specified", "args"); 
					}

					// Call the MFnCameraSet method to append the layer.
					//
					MFnCameraSet dirFn = new MFnCameraSet(dirNode);
					dirFn.appendLayer(camPath, setObj);

					return;
				}

				if (deleteLayerUsed)
				{
					if (nObjs != 1)
					{
						throw new ArgumentException("-deleteLayer must have a single cameraSet node specified", "args"); 
					}

					// Get the specified cameraSet node.
					//
					MObject dirNode = MObject.kNullObj;
					if (!getExCameraSetNode(dirNode))
					{
						throw new ArgumentException("-deleteLayer must have a valid exCameraSet node specified", "args"); 
					}

					// Call the MFnCameraSet method to delete the layer.
					//
					MFnCameraSet dirFn = new MFnCameraSet(dirNode);
					dirFn.deleteLayer((uint)cameraLayer);

					return;
				}

				if (numLayersUsed)
				{
					if (queryUsed)
					{
						// Get the specified cameraSet node.
						//
						MObject dirNode = MObject.kNullObj;
						if (!getExCameraSetNode(dirNode))
						{
							throw new ArgumentException("-numLayers must have a valid exCameraSet node specified", "args"); 
						}

						// Call the MFnCameraSet method to get the number of layers.
						//
						MFnCameraSet dirFn = new MFnCameraSet(dirNode);
						uint numLayers = dirFn.numLayers;
						setResult((int)numLayers);
					}
					else
					{
						throw new ArgumentException("-numLayers requires the query flag to be used", "args"); 
					}

					return;
				}

				if (cameraUsed)
				{
					if ((nObjs != 1) || (!layerUsed))
					{
						throw new ArgumentException("-camera must have a cameraSet node and layer specified", "args"); 
					}

					// Get the specified cameraSet node.
					//
					MObject dirNode = MObject.kNullObj;
					if (!getExCameraSetNode(dirNode))
					{
						throw new ArgumentException("-camera must have a valid exCameraSet node specified", "args"); 
					}

					if (editUsed)
					{
						// Get a dag path to the specified camera.
						//
						MSelectionList camList = new MSelectionList();
						camList.add(camName);
						MDagPath camPath = new MDagPath();
						camList.getDagPath(0, camPath);
						if (!camPath.isValid)
						{
							throw new ArgumentException("-camera must have a valid camera node specified", "args"); 
						}

						// Call the MFnCameraSet method to set the camera.
						//
						MFnCameraSet dirFn = new MFnCameraSet(dirNode);
						dirFn.setLayerCamera((uint)cameraLayer, camPath);
					}
					else if (queryUsed)
					{
						// Call the MFnCameraSet method to get the camera.
						//
						MDagPath camPath = new MDagPath();
						MFnCameraSet dirFn = new MFnCameraSet(dirNode);

						dirFn.getLayerCamera((uint)cameraLayer, camPath);
						MObject camNode = camPath.node;
						MFnDependencyNode nodeFn = new MFnDependencyNode(camNode);
						setResult(nodeFn.name);
					}
				}

				if (setUsed)
				{
					if ((nObjs != 1) || (!layerUsed))
					{
						throw new ArgumentException("-set must have a cameraSet node and layer specified", "args"); 
					}

					// Get the specified cameraSet node.
					//
					MObject dirNode = MObject.kNullObj;
					if (!getExCameraSetNode(dirNode))
					{
						throw new ArgumentException("-set must have a valid exCameraSet node specified", "args"); 
					}

					if (editUsed)
					{
						// Get the specified set node.
						//
						MObject setObj = MObject.kNullObj;
						if (setName != "")
						{
							MSelectionList setList = new MSelectionList();
							setList.add(setName);
							setList.getDependNode(0, setObj);
							if (setObj == MObject.kNullObj)
							{
								throw new ArgumentException("-set must have a valid set node specified", "args"); 
							}
						}

						// Call the MFnCameraSet method to set the set node.
						//
						MFnCameraSet dirFn = new MFnCameraSet(dirNode);
						dirFn.setLayerSceneData((uint)cameraLayer, setObj);
					}
					else if (queryUsed)
					{
						// Call the MFnCameraSet method to get the set node.
						//
						MObject setObj = new MObject();
						MFnCameraSet dirFn = new MFnCameraSet(dirNode);

						dirFn.getLayerSceneData((uint)cameraLayer, setObj);
						MFnDependencyNode nodeFn = new MFnDependencyNode(setObj);
						setResult(nodeFn.name); 
					}
				}

				if (layerTypeUsed)
				{
					if ((nObjs != 1) || (!layerUsed))
					{
						throw new ArgumentException("-layerType must have a cameraSet node and layer specified", "args"); 
					}

					// Get the specified cameraSet node.
					//
					MObject dirNode = MObject.kNullObj;
					if (!getExCameraSetNode(dirNode))
					{
						throw new ArgumentException("-layerType must have a valid exCameraSet node specified", "args"); 
					}

					MFnDependencyNode nodeFn = new MFnDependencyNode(dirNode);
					
					exCameraSet exDir = nodeFn.userNode as exCameraSet;

					if (editUsed)
					{
						// Get the specified layer type.
						//
						int pt = -1;
						if (layerTypeVal == "Mono")
							pt = 0;
						else if (layerTypeVal == "Left")
							pt = 1;
						else if (layerTypeVal == "Right")
							pt = 2;
						else
						{
							throw new ArgumentException("-layerType must have a valid type specified", "args"); 
						}

						// Call the exCameraSet method to set the layer type.
						//
						exDir.setLayerType((uint)cameraLayer, pt);
					}
					else if (queryUsed)
					{
						// Call the exCameraSet method to get the layer type.
						//
						try
						{
							int lt = exDir.getLayerType((uint)cameraLayer);
							if (lt == 0)
								setResult("Mono");
							else if (lt == 1)
								setResult("Left");
							else if (lt == 2)
								setResult("Right");
						}
						catch (System.Exception ex)
						{
							throw new ApplicationException("exCameraSet node does not have a valid layer type", ex);
						}
					}
				}

				if (activeUsed)
				{
					if ((nObjs != 1) || (!layerUsed))
					{
						throw new ArgumentException("-active must have a cameraSet node and layer specified", "args"); 
					}

					// Get the specified cameraSet node.
					//
					MObject dirNode = MObject.kNullObj;
					if (!getExCameraSetNode(dirNode))
					{
						throw new ArgumentException("-active must have a valid exCameraSet node specified", "args"); 
					}

					if (editUsed)
					{
						// Call the MFnCameraSet method to set the set node.
						//
						MFnCameraSet dirFn = new MFnCameraSet(dirNode);
						dirFn.setLayerActive((uint)cameraLayer, activeVal);
					}
					else if (queryUsed)
					{
						// Call the MFnCameraSet method to get the active value.
						//
						MFnCameraSet dirFn = new MFnCameraSet(dirNode);
						activeVal = dirFn.isLayerActive((uint)cameraLayer);
						setResult(activeVal);
					}
				}

			}
			return;
		}

		void parseArgs(MArgList args)
		{
			createdUsed = (args.flagIndex(kCreateFlag, kCreateFlagLong) != MArgList.kInvalidArgIndex);
			editUsed = (args.flagIndex(kEditFlag, kEditFlagLong) != MArgList.kInvalidArgIndex);
			queryUsed = (args.flagIndex(kQueryFlag, kQueryFlagLong) != MArgList.kInvalidArgIndex);
			helpUsed = (args.flagIndex(kHelpFlag, kHelpFlagLong) != MArgList.kInvalidArgIndex);
			numLayersUsed = (args.flagIndex(kNumLayersFlag, kNumLayersFlagLong) != MArgList.kInvalidArgIndex);

			// If flags are used which require no other information, return now.
			//
			if (createdUsed || helpUsed)
				return;

			uint maxArg = args.length - 1;
			uint activeIndex = args.flagIndex(kActiveFlag, kActiveFlagLong);
			uint appendCameraIndex = args.flagIndex(kAppendCameraFlag, kAppendCameraFlagLong);
			uint appendCameraAndSetIndex = args.flagIndex(kAppendCameraAndSetFlag, kAppendCameraAndSetFlagLong);
			uint cameraIndex = args.flagIndex(kCameraFlag, kCameraFlagLong);
			uint deleteLayerIndex = args.flagIndex(kDeleteLayerFlag, kDeleteLayerFlagLong);
			uint layerIndex = args.flagIndex(kLayerFlag, kLayerFlagLong);
			uint layerTypeIndex = args.flagIndex(kLayerTypeFlag, kLayerTypeFlagLong);
			uint setIndex = args.flagIndex(kSetFlag, kSetFlagLong);

			activeUsed = (activeIndex != MArgList.kInvalidArgIndex);
			appendCameraUsed = (appendCameraIndex != MArgList.kInvalidArgIndex);
			appendCameraAndSetUsed = (appendCameraAndSetIndex != MArgList.kInvalidArgIndex);
			cameraUsed = (cameraIndex != MArgList.kInvalidArgIndex);
			deleteLayerUsed = (deleteLayerIndex != MArgList.kInvalidArgIndex);
			layerUsed = (layerIndex != MArgList.kInvalidArgIndex);
			layerTypeUsed = (layerTypeIndex != MArgList.kInvalidArgIndex);
			setUsed = (setIndex != MArgList.kInvalidArgIndex);

			// Process each flag.
			//
			bool maxArgUsed = false;
			if (activeUsed)
			{
				if (editUsed)
				{
					activeVal = args.asBool((activeIndex + 1));
					if ((layerTypeIndex + 1) == maxArg)
						maxArgUsed = true;
				}
			}

			if (appendCameraUsed)
			{
				camName = args.asString((appendCameraIndex + 1));
				if ((appendCameraIndex + 1) == maxArg)
					maxArgUsed = true;
			}

			if (appendCameraAndSetUsed)
			{
				camName = args.asString((appendCameraAndSetIndex + 1));
				setName = args.asString((appendCameraAndSetIndex + 2));
				if ((appendCameraAndSetIndex + 2) == maxArg)
					maxArgUsed = true;
			}

			if (cameraUsed)
			{
				if (editUsed)
				{
					camName = args.asString(cameraIndex + 1);
					if ((cameraIndex + 1) == maxArg)
						maxArgUsed = true;
				}
			}

			if (deleteLayerUsed)
			{
				cameraLayer = args.asInt(deleteLayerIndex + 1);
				if ((deleteLayerIndex + 1) == maxArg)
					maxArgUsed = true;
			}

			if (layerUsed)
			{
				cameraLayer = args.asInt(layerIndex + 1);
				if ((layerIndex + 1) == maxArg)
					maxArgUsed = true;
			}

			if (layerTypeUsed)
			{
				if (editUsed)
				{
					layerTypeVal = args.asString(layerTypeIndex + 1);
					if ((layerTypeIndex + 1) == maxArg)
						maxArgUsed = true;
				}
			}

			if (setUsed)
			{
				if (editUsed)
				{
					setName = args.asString(setIndex + 1);
					if ((setIndex + 1) == maxArg)
						maxArgUsed = true;
				}
			}


			// If all of the arguments have been used, get the cameraSet node from the selection list.
			// Otherwise, get it from the last argument.
			//
			if (maxArgUsed)
				MGlobal.getActiveSelectionList(list);
			else
				list.add(args.asString(maxArg));
		}

		public bool getExCameraSetNode(MObject dirObj)
		{
			// Get the specified cameraSet node.
			//
			MObject dirNode = MObject.kNullObj;
			list.getDependNode(0, dirNode);
			if (dirNode == MObject.kNullObj)
				return false;

			MFnDependencyNode nodeFn = new MFnDependencyNode(dirNode);
			if (nodeFn.typeId.id != exCameraSet.type_id.id)
				return false;

			dirObj = dirNode;
			return true;
		}
	}
}
