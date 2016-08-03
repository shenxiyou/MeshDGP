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
//	file : undoRedoMsgCmd.cpp
//	class: undoRedoMsg
//	----------------------
//	This is an example to demonstrate  how to listen to undo and redo
//	message events.  
	
//	The syntax of the command is:
//		undoRedoMsg add;	
//		undoRedoMsg remove;
//	The add argument causes listening to undo/redo to be turned on.
//	The remove argument causes undo/redo listening to be removed.
////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Linq;
using System.Text;

 
using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaUI;
using Autodesk.Maya.OpenMayaRender;

using System.Runtime.InteropServices;

[assembly: MPxCommandClass(typeof(MayaNetPlugin.undoRedoMsg), "undoRedoMsgCSharp")]


namespace MayaNetPlugin
{
    class undoRedoMsg : MPxCommand, IMPxCommand
    {
        static bool added = true;
        static readonly string UndoString = "Undo";
        static readonly string RedoString = "Redo";

        private static void undoCB(object sender, MBasicFunctionArgs arg)
        {
            MGlobal.displayInfo("undoCallback ");
        }

        private static void redoCB(object sender, MBasicFunctionArgs arg)
        {

            MGlobal.displayInfo("redoCallback ");
        }

        public override void doIt(MArgList args)
        {
            uint last = args.length;
            if (last > 0)
            {
                for (uint i = 0; i < last; i++)
                {
                    string argStr = args.asString(i);

                    if (argStr == "add")
                    {
                        if (added)
                            continue;

                        MEventMessage.Event[UndoString] += undoCB;
                        MEventMessage.Event[RedoString] += redoCB;
                    }
                    else if (argStr == "remove")
                    {
                        if (added)
                        {
                            MEventMessage.Event[UndoString] -= undoCB;
                            MEventMessage.Event[RedoString] -= redoCB;
                            added = false;
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Failure condition", "args");
                    }
                }
            }

            return;
        }
    }
}