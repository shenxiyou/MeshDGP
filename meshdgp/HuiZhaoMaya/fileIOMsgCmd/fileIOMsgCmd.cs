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
using System.Collections;
using System.Linq;
using System.Text;

 
using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaUI;
using Autodesk.Maya.OpenMayaRender;


using System.Runtime.InteropServices;

/* 

	file : fileIOMsgCmd.cs
	class: PreLoad
	----------------------
	This is an example to demonstrate the usages of :

	string MFileIO.beforeImportFile(); 
	string MFileIO.beforeOpenFile();
	string MFileIO.beforeExportFile();
	string MFileIO.beforeReferenceFile();

	Return value of kSuccess indicates correct value returns
		
*/
[assembly: MPxCommandClass(typeof(MayaNetPlugin.PreLoad), "fileIOMsgCSharpCmdCSharp")]

namespace MayaNetPlugin
{
    class PreLoad : MPxCommand, IMPxCommand
    {
        static bool added = false;

        #region callbacks
        public static void preOpenFunc(object sender, MBasicFunctionArgs args)
        {
            string msg = "FILE TO BE OPENED IS ";

            try
            {
                msg += MFileIO.beforeOpenFilename;
            }
            catch (System.Exception)
            {
                msg += "ERROR: Could not be retrieved";
            }

            MGlobal.displayInfo(msg);
        }
        public static void preImportFunc(object sender, MBasicFunctionArgs args)
        {
            string msg = "PRE IMPORT FILE IS ";

            try
            {
                msg += MFileIO.beforeImportFilename;
            }
            catch (System.Exception)
            {
                msg += "ERROR: Could not be retrieved";
            }

            MGlobal.displayInfo(msg);
        }
        public static void preSaveFunc(object sender, MBasicFunctionArgs args)
        {
            string msg = "FILE TO BE SAVED IS ";

            try
            {
                msg += MFileIO.beforeSaveFilename;
            }
            catch (System.Exception)
            {
                msg += "ERROR: Could not be retrieved";
            }

            MGlobal.displayInfo(msg);
        }
        public static void preExportFunc(object sender, MBasicFunctionArgs args)
        {

            string msg = "FILE TO BE EXPORTED IS ";

            try
            {
                msg += MFileIO.beforeSaveFilename;
            }
            catch (System.Exception)
            {
                msg += "ERROR: Could not be retrieved";
            }

            MGlobal.displayInfo(msg);
        }
        public static void preReferenceFunc(object sender, MBasicFunctionArgs args)
        {

            string msg = "FILE TO BE REFERENCED IS ";

            try
            {
                msg += MFileIO.beforeReferenceFilename;
            }
            catch (System.Exception)
            {
                msg += "ERROR: Could not be retrieved";
            }

            MGlobal.displayInfo(msg);
        }
        #endregion
        public override void doIt(MArgList args)
        {
            MGlobal.displayInfo("PLUGIN LOADED");

            if (added)
                return;

            // add the function call backs
            // and store call back ids for removal later
            MSceneMessage.BeforeOpen += preOpenFunc;
            MSceneMessage.BeforeImport += preImportFunc;
            MSceneMessage.BeforeSave += preSaveFunc;
            MSceneMessage.BeforeExport += preExportFunc;
            // kBeforeReference is deprecated 
            // we use AfterCreateReference/kAfterCreateReferenceAndRecordEdits to instead
            MSceneMessage.AfterCreateReference += preReferenceFunc;

            return;
        }
    };
}