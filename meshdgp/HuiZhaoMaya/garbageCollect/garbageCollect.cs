// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


// INTRODUCTION:
//	This class will let maya csharp environment perform a force garbage collection.
//
using System;
using System.Collections.Generic;

using Autodesk.Maya.OpenMaya;

[assembly: MPxCommandClass(typeof(MayaNetPlugin.GarbageCollect), "GCCmdCSharp")]

namespace MayaNetPlugin
{
    class GarbageCollect : MPxCommand, IMPxCommand
    {
        public override void doIt(MArgList args)
        {
            MGlobal.displayInfo("Garbage Collection begin.");

            GC.Collect();
            GC.WaitForPendingFinalizers();

            MGlobal.displayInfo("Garbage Collection complete.");

            return;
        }
    };
}


//