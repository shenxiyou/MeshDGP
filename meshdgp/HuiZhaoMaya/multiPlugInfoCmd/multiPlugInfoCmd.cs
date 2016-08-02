// ==================================================================
// Copyright 2012 Autodesk, Inc. All rights reserved.
//
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


////////////////////////////////////////////////////////////////////////
//
// multiPlugInfoCSharp
//
// This command prints out the child plug information for a multiPlug.
// If the -index flag is used, the logical index values used by the plug
// will be returned.  Otherwise, the plug values will be returned.
//
////////////////////////////////////////////////////////////////////////

using System;

using Autodesk.Maya.OpenMaya;

[assembly: MPxCommandClass(typeof(MayaNetPlugin.multiPlugInfo), "multiPlugInfoCSharp")]

namespace MayaNetPlugin
{
    [MPxCommandSyntaxFlag("-i", "-index")]
    [MPxCommandSyntaxSelection(MinObjectCount = 1, MaxObjectCount = 1, ObjectType = typeof(MSelectionList))]
    [MPxCommandSyntaxModeAttribute(MPxCommandSyntaxModeAttribute.CommandMode.kNone)]
	class multiPlugInfo : MPxCommand, IMPxCommand
	{
		private bool __isIndex ;
		private MPlug __fPlug ;
		
		const string kIndexFlag	= "-i" ;
		const string kIndexFlagLong = "-index" ;

		public multiPlugInfo () {
			__isIndex =false ;
			__fPlug = null;
		}

		public override void doIt(MArgList args/*, MPxCommandClass cmd*/)
		{
			// This method is called from script when this command is called.
			// It should set up any class data necessary for redo/undo,
			// parse any given arguments, and then call redoIt.
			MArgDatabase argData = new MArgDatabase(/*cmd.*/syntax, args);

            __isIndex = argData.isFlagSet(kIndexFlag);

			// Get the plug specified on the command line.
			MSelectionList slist = argData.SelectionObjects;
			if ( slist.length == 0 ) {
				throw new ArgumentException("Must specify an array plug in the form <nodeName>.<multiPlugName>.", "args");
			}
			__fPlug = slist.getPlug (0) ;
			if ( __fPlug == null ) {
				throw new ArgumentException("Must specify an array plug in the form <nodeName>.<multiPlugName>.", "args");
			}

			// Construct a data handle containing the data stored in the plug.
			MDataHandle dh = new MDataHandle() ;

            try {
                __fPlug.getValue(dh);
            } catch (Exception) {
                throw new ApplicationException("Could not get the plug value.");
            }

			MArrayDataHandle adh ;
			uint indx =0 ;
			try {
				adh = new MArrayDataHandle(dh);
			} catch (Exception) {
				__fPlug.destructHandle(dh) ;
				throw new ApplicationException("Could not create the array data handle.");
			}

			// Iterate over the values in the multiPlug.  If the index flag has been used, just return
			// the logical indices of the child plugs.  Otherwise, return the plug values.
            string errorMsg = null;
            for (uint i = 0; i < adh.elementCount(); i++, adh.next())
            {
				try {
					indx = adh.elementIndex() ;
				} catch (Exception) {
					continue ;
				}
				if ( __isIndex ) {
					appendToResult ((int)indx) ;
				} else {
					MDataHandle h = adh.outputValue () ;
					if (h.isNumeric)
					{
						switch (h.numericType)
						{
							case MFnNumericData.Type.kBoolean: appendToResult (h.asBool) ; break;
							case MFnNumericData.Type.kShort: appendToResult (h.asShort) ; break;
							case MFnNumericData.Type.kInt: appendToResult (h.asInt) ; break;
							case MFnNumericData.Type.kFloat: appendToResult (h.asFloat) ; break;
							case MFnNumericData.Type.kDouble: appendToResult (h.asDouble) ; break;
							default:
                                errorMsg = string.Format("{0}This sample command only supports boolean, integer, and floating point values. Not {1}.\n",
                                    errorMsg != null ? errorMsg : "", h.numericType.ToString());

								break;
						}
					}
				}
			}
			__fPlug.destructHandle (dh) ;

            if (errorMsg != null)
                throw new ApplicationException(errorMsg); ;

			return;
		}
		
	}
}
