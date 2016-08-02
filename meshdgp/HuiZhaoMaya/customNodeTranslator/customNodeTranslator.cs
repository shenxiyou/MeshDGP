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
//		This C# plugin is ported from: $(MAYADIR)\devkit\plug-ins\customNodeTranslator.py

//  How to use:
//		  Load the plugin, Click File->Export/Import, select spCustomNodeTranslator File type.

using System;
using System.IO;

 
using Autodesk.Maya.OpenMaya;


[assembly: MPxFileTranslatorClass(typeof(MayaNetPlugin.customNodeTranslator), 
	MayaNetPlugin.customNodeTranslator.kPluginTranslatorTypeName, null,null,null)]

namespace MayaNetPlugin
{
	public class customNodeTranslator : MPxFileTranslator
	{
		public const string kPluginTranslatorTypeName = "spCustomNodeTranslator";
		public override bool haveWriteMethod()
		{
			return true;
		}

		public override bool haveReadMethod()
		{
			return true;
		}

		public override string filter()
		{
			return "*.spcnt";
		}

		public override string defaultExtension()
		{
			return "spcnt";
		}

		public override void writer(MFileObject file, string optionsString, MPxFileTranslator.FileAccessMode mode)
		{
			string fullName = file.fullName;
			StreamWriter sWriter = new StreamWriter(fullName);
			sWriter.Write("# Simple text file of custom node information" + Environment.NewLine);

			MItDependencyNodes iterator = new MItDependencyNodes();
			while (!iterator.isDone)
			{
				MObject obj = iterator.thisNode;
				try
				{
					MFnDependencyNode dnFn = new MFnDependencyNode(obj);
					MPxNode userNode = dnFn.userNode;
					if (userNode != null)
						sWriter.Write("# custom node: " + dnFn.name + Environment.NewLine);
				}
				catch (System.Exception)
				{
				}

				iterator.next();
			}
			sWriter.Close();

			return;
		}

		void processLine(string lineStr)
		{
			MGlobal.displayInfo( "read:" +lineStr);
		}

		public override void reader(MFileObject file, string optionsString, MPxFileTranslator.FileAccessMode mode)
		{
			string fullName = file.fullName;
			StreamReader sReader = new StreamReader(fullName);
			while (!sReader.EndOfStream)
			{
				string lineStr = sReader.ReadLine();
				processLine(lineStr);
			}
			sReader.Close();

			return;
		}
	}
}
