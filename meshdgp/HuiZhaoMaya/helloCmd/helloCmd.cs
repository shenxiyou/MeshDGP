// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================

// Example Plugin: helloCmd.cs
//
// This plugin in maya uses DeclareSimpleCommand macro
// to do the necessary initialization for a simple command
// plugin.
//
// We cannot support macro in C# and we are using assembly to load plugin.
// We can use assembly load to make an easy use of command.
using System;

using Autodesk.Maya.OpenMaya;

[assembly: MPxCommandClass(typeof(MayaNetPlugin.helloCmd), "helloCmdCSharp")]

namespace MayaNetPlugin
{
    public class helloCmd : MPxCommand, IMPxCommand
	{
		public override void doIt(MArgList argl)
		{
			MGlobal.displayInfo("Hello " + argl.asString(0) + "\n");
			return;
		}
	}
}
