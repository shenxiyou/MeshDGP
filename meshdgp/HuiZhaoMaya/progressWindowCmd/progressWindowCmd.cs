// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


/* 

	file : progressWindowCmd.cs
	class: progressWindowCmd

	----------------------

	This class is an example of how to use a progress window from within a
	plugin.  The command "progressWindowCmd" displays a simple progress 
	window which updates every second.  The progress window can be terminated
	by hitting escape.
	
*/

using System;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaUI;

[assembly: MPxCommandClass(typeof(MayaNetPlugin.progressWindowCmd), "progressWindowCmdCSharp")]

namespace MayaNetPlugin
{
	class progressWindowCmd : MPxCommand, IMPxCommand
	{
		public progressWindowCmd() {}

		public override void doIt(MArgList args)
		{
			string title = "Doing Nothing";
			string sleeping = "Sleeping: ";
	
			int amount = 0;
			int maxProgress = 10;
	
			// First reserve the progress window.  If a progress window is already
			// active (eg. through the mel "progressWindow" command), this command
			// fails.
			//
			if (!MProgressWindow.reserve)
			{
				throw new ApplicationException("Progress window already in use.");
			}

			//
			// Set up and print progress window state
			//
			try {
			MProgressWindow.setProgressRange(amount, maxProgress);
			MProgressWindow.title = title;
			MProgressWindow.isInterruptable = true;
			MProgressWindow.progress = amount;
			} catch(Exception) {
				MGlobal.displayInfo("API error detected.");
			}
			

			string progressWindowState = "Progress Window Info:" +
				"\nMin: " + MProgressWindow.progressMin +
				"\nMax: " + MProgressWindow.progressMax + 
				"\nTitle: " + MProgressWindow.title + 
				"\nInterruptible: " + MProgressWindow.isInterruptable;

			MGlobal.displayInfo(progressWindowState);
	
			try {
			MProgressWindow.startProgress();
			} catch (Exception) {
				MGlobal.displayInfo("API error detected.");
			}

			// Count 10 seconds
			//
			for (int i = amount; i < maxProgress; i++)
			{
				if (i != 0 && MProgressWindow.isCancelled) {
					MGlobal.displayInfo("Progress interrupted!");
					break;
				}

				string statusStr = sleeping;
				statusStr += i;
				try {
				MProgressWindow.progressStatus = statusStr;
				MProgressWindow.advanceProgress(1);
				} catch(Exception) {
					MGlobal.displayInfo("API error detected.");
				}

				MGlobal.displayInfo("Current progress: " + MProgressWindow.progress);

				MGlobal.executeCommand("pause -sec 1", false, false);
			}
	
			// End the progress, unreserving the progress window so it can be used
			// elsewhere.
			//
			try {
			MProgressWindow.endProgress();
			} catch (Exception) {
				MGlobal.displayInfo("API error detected.");
			}
		}
	}
}
