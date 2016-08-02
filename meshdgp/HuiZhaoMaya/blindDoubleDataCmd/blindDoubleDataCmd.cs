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
using Autodesk.Maya.OpenMaya;


[assembly: MPxCommandClass(typeof(MayaNetPlugin.blindDoubleDataCmd), "blindDoubleDataCmdCSharp")]


namespace MayaNetPlugin
{
	public class blindDoubleDataCmd : MPxCommand, IUndoMPxCommand
	{
		#region override
		public override void doIt(MArgList args)
		{
			MSelectionList list = new MSelectionList();
			MGlobal.getActiveSelectionList(list);

			iter = new MItSelectionList(list, MFn.Type.kInvalid);
			redoIt();
		}

		public override void redoIt()
		{
			MObject dependNode = new MObject();
			MOStream stdoutstream = MStreamUtils.stdOutStream();
			for(; !iter.isDone; iter.next())
			{
				// Get the selected dependency node and create
				// a function set for it
				//
				try
				{
					iter.getDependNode(dependNode);
				}
				catch (System.Exception)
				{
					MStreamUtils.writeCharBuffer(MStreamUtils.stdErrorStream(), "Error getting the dependency node");
					continue;
				}

				MFnDependencyNode fnDN;
				try
				{
					fnDN = new MFnDependencyNode(dependNode);
				}
				catch(System.Exception)
				{
					MStreamUtils.writeCharBuffer(MStreamUtils.stdErrorStream(), "Error creating MFnDependencyNode");
					continue;
				}

				MFnTypedAttribute fnAttr = new MFnTypedAttribute();
				MObject newAttr = fnAttr.create("blindDoubleData", "BDD", blindDoubleData.tid);

				try
				{
					fnDN.addAttribute(newAttr, MFnDependencyNode.MAttrClass.kLocalDynamicAttr);
				}
				catch (System.Exception)
				{
					// do nothing
					// addAttribute only need call once, the redundant calls will return false (throw exception) 
				}
				
				// Create a plug to set and retrieve value off the node.
				//
				MPlug plug = new MPlug(dependNode, newAttr);


				// ----------------------------------- Attention ------------------------------------
				// --------------------------------- Downcast Begin -----------------------------------
				// the following codes are used to get the c# object 
				// 
				MFnPluginData pdFnCreator = new MFnPluginData();

				// 1. you cannot gain blindDoubleData by the following code
				//    {code}
				//          blindDoubleData newData = new blindDoubleData()
				//    {code}
				//    As we need to keep the relationship between c# impl and c++ instance pointer
				//    We cannot use the above ctor codes, otherwise, the mandatory information used for down casting is omitted

				// 2. you cannot use the tempData gained by the following code
				//    {code}
				//          MObject tempData = pdFnCreator.create(blindDoubleData.tid); 
				//    {code}
				//    reason:
				//          tempData is useless, we cannot use tempData to do downcast
				//          the create function gains the tempData by the following code
				//
				//          {code}
				//              newHandle = new MObject(mayaHandle);    
				//          {code}
				//
				//     the mayaHandle is the actual pointer, which we store. But we have no information about the newHandle

				// the return object is useless. the data we needed is stored in pdFnCreator
				pdFnCreator.create(blindDoubleData.tid);

				// 3. get "the data" we needed
				blindDoubleData newData = pdFnCreator.data() as blindDoubleData;
				// ---------------------------------- Downcast End -----------------------------------
				if (newData == null)
					continue;

				newData.value = 3.2;

				plug.setValue(newData);
				
				// Now try to retrieve the value of the plug as an MObject.
				//
				MObject sData = new MObject();

				try
				{
					plug.getValue( sData );
				}
				catch (System.Exception)
				{
					continue;
				}

				// Convert the data back to MPxData.
				//
				MFnPluginData pdFn = new MFnPluginData( sData );

				blindDoubleData data = pdFn.data() as blindDoubleData;
		
				// Get the value.
				//
				if ( null == data ) {
					// error
					MStreamUtils.writeCharBuffer(MStreamUtils.stdErrorStream(), "error: failed to retrieve data.");
				}
				MStreamUtils.writeLine(stdoutstream);
				MStreamUtils.writeCharBuffer(stdoutstream, ">>>>>>>>>>>>>>>>>>>>>>>> blindDoubleData binary >>>>>>>>>>>>>>>>>>>>");
				MStreamUtils.writeLine(stdoutstream);
				data.writeBinary(stdoutstream);
				MStreamUtils.writeLine(stdoutstream);
				MStreamUtils.writeCharBuffer(stdoutstream, ">>>>>>>>>>>>>>>>>>>>>>>> blindDoubleData ascii >>>>>>>>>>>>>>>>>>>>");
				MStreamUtils.writeLine(stdoutstream);
				data.writeASCII(stdoutstream);
			}
			return;
		}

		public override void undoIt()
		{
			return;
		}

		public override bool isUndoable()
		{
			return true;
		}
		#endregion override

		private MItSelectionList iter;
	}
}
