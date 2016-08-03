// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


////////////////////////////////////////////////////////////////////////
//
//  Description:
//      Doubles the focal length for the camera of the current 3d view.
//
////////////////////////////////////////////////////////////////////////

using System;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaUI;

[assembly: MPxCommandClass(typeof(MayaNetPlugin.zoomCameraCmd), "zoomCameraCmdCSharp")]

namespace MayaNetPlugin
{

	public class zoomCameraCmd : MPxCommand, IUndoMPxCommand
	{
		private MDagPath camera = new MDagPath();

		#region override
		public override void doIt(MArgList args)
		//
		// Description
		//     Gets the zoomCamera for the current 3d view and calls
		//     the redoIt command to set the focal length.
		//
		// Note
		//     The doit method should collect whatever information is
		//     required to do the task, and store it in local class data.
		//     It should finally call redoIt to make the command happen.
		//
		{
			// Get the current zoomCamera
			//
			M3dView.active3dView.getCamera( camera );
			redoIt();
			return;
		}

		public override void redoIt()
		//
		// Description
		//     Doubles the focal length of current camera
		//
		// Note
		//     The redoIt method should do the actual work, based on the
		//     internal data only.
		//
		{
			MFnCamera fnCamera = new MFnCamera( camera );
			fnCamera.focalLength *= 2.0;
			return;
		}

		public override void undoIt()
		//
		// Description
		//     the undo routine
		//
		// Note
		//     The undoIt method should undo the actual work, based on the
		//     internal data only.
		//
		{
			MFnCamera fnCamera = new MFnCamera( camera );
            fnCamera.focalLength /= 2.0;
			return;
		}

		public override bool isUndoable()
		{
			return true;
		}
		#endregion
	}
}
