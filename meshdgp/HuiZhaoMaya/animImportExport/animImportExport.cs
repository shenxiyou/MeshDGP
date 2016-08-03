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
//		This C# plugin is ported from: $(MAYADIR)\devkit\plug-ins\animImportExport

//	Description:
//		Imports and export an anim curves into .anim file.
//  How to use:
//		  Load the plugin, Click File->Export/Import, select animExport/animImport File type.
//

using System;
using System.IO;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaAnim;



// Before loading the plugin, please execute $(MAYADIR)\devkit\plug-ins\animImportExport\animImportExportInitStrings.mel;
// otherwise Maya would prompt that it couldn't find the "animImportExportInitStrings" procedure.

[assembly: MPxUIStringClass(typeof(MayaNetPlugin.RegisterMStringResources), "animImportExportInitStrings", "registerStringResources")]
[assembly: MPxFileTranslatorClass(typeof(MayaNetPlugin.animImport), "animImportCSharp", null,
	MayaNetPlugin.animImport.animImportOptionScript, MayaNetPlugin.animImport.animImportDefaultOptions)]
[assembly: MPxFileTranslatorClass(typeof(MayaNetPlugin.animExport), "animExportCSharp", null,
    MayaNetPlugin.animExport.animExportOptionScript, MayaNetPlugin.animExport.animExportDefaultOptions)]

namespace MayaNetPlugin
{
	public class animImport : MPxFileTranslator
	{
		animReader fReader = new animReader();
		string pasteFlags = "";
		public const string animImportOptionScript = "animImportOptions";
		public const string animImportDefaultOptions = "targetTime=4;copies=1;option=replace;pictures=0;connect=0;";

		public override void reader(MFileObject file, string optionsString, MPxFileTranslator.FileAccessMode mode)
		{
			string fileName = file.fullName;
			// 	Parse the options. The options syntax is in the form of
			//	"flag=val;flag1=val;flag2=val"
			//

			if (optionsString.Length > 0)
			{
				//	Set up the flags for the paste command.
				//
				const string flagTime = "time";
				const string flagCopies = "copies";
				const string flagOption = "option";
				const string flagConnect = "connect";

				string copyValue = "";
				string flagValue = "";
				string connectValue = "";
				string timeValue = "";

				//	Start parsing.
				//
				string[] optionList = optionsString.Split(new Char[] { ';' });
				int nOptions = optionList.Length;
				for (int i = 0; i < nOptions; i++)
				{
					string[] theOption = optionList[i].Split(new Char[] { '=' });
					if (theOption.Length < 1)
						continue;

					if (theOption[0] == flagCopies && theOption.Length > 1)
					{
						copyValue = theOption[1]; ;
					}
					else if (theOption[0] == flagOption && theOption.Length > 1)
					{
						flagValue = theOption[1];
					}
					else if (theOption[0] == flagConnect && theOption.Length > 1)
					{
						if (Convert.ToInt32(theOption[1]) != 0)
							connectValue += theOption[1];
					}
					else if (theOption[0] == flagTime && theOption.Length > 1)
					{
						timeValue += theOption[1];
					}
				}

				if (copyValue.Length > 0)
				{
					pasteFlags += " -copies ";
					pasteFlags += copyValue;
					pasteFlags += " ";
				}
				if (flagValue.Length > 0)
				{
					pasteFlags += " -option \"";
					pasteFlags += flagValue;
					pasteFlags += "\" ";
				}
				if (connectValue.Length > 0)
				{
					pasteFlags += " -connect ";
					pasteFlags += connectValue;
					pasteFlags += " ";
				}
				if (timeValue.Length > 0)
				{
					bool useQuotes = false;
					try
					{
						Convert.ToDouble(timeValue);
					}
					catch (System.Exception)
					{
						useQuotes = true;
					}
					pasteFlags += " -time ";
					if (useQuotes) pasteFlags += "\"";
					pasteFlags += timeValue;
					if (useQuotes) pasteFlags += "\"";
					pasteFlags += " ";
				}
			}

			bool isImported = false;
			if (mode == MPxFileTranslator.FileAccessMode.kImportAccessMode)
				isImported = importAim(fileName, pasteFlags);
			else
				throw new ArgumentException("Invalid File Access mode.", "mode");

			if (!isImported)
			{
				throw new ApplicationException("Importing Anim Failed.");
			}

			return;
		}

		public override bool haveReadMethod()
		{
			return true;
		}

		public override bool haveWriteMethod()
		{
			return false;
		}

		public override bool canBeOpened()
		{
			return false;
		}

		public override string defaultExtension()
		{
			return "anim";
		}

		public override MFileKind identifyFile(MFileObject file, string buffer, short size)
		{
			string name = file.name;
			int namelength = name.Length;
			string tmpStr = ".aim";
			if (namelength > 5 && string.Compare(name, namelength - 5, tmpStr, 0, 5, true) == 0)
				return MFileKind.kIsMyFileType;

			//	Check the buffer to see if this contains the correct keywords
			//	to be a anim file.
			//
			tmpStr = "animVersion";
			if (string.Compare(buffer, 0, tmpStr, 0, 11) == 0)
				return MFileKind.kIsMyFileType;

			return MFileKind.kNotMyFileType;
		}

		public bool importAim(string fileName, string flags)
		{
			MAnimCurveClipboard.theAPIClipboard.clear();

			//	If the selection list is empty, there is nothing to import.
			//
			MSelectionList sList = new MSelectionList();
			MGlobal.getActiveSelectionList(sList);
			if (sList.isEmpty)
			{
				string msg = MStringResource.getString(RegisterMStringResources.kNothingSelected);
				MGlobal.displayError(msg);
				return false;
			}

			StreamReaderExt readExt = new StreamReaderExt(fileName);
			fReader.readClipboard(ref readExt, MAnimCurveClipboard.theAPIClipboard);

			if (MAnimCurveClipboard.theAPIClipboard.isEmpty)
				return false;

			string command = "pasteKey -cb api ";
			command += pasteFlags;

			try
			{
				int result = -1;
				MGlobal.executeCommand(command, out result, false, true);
			}
			catch (System.Exception)
			{
				string msg = MStringResource.getString(RegisterMStringResources.kPasteFailed);
				MGlobal.displayError(msg);
				return false;
			}

            readExt.Close();

			return true;
		}
	}

	public class animExport : MPxFileTranslator
	{
		animWriter fWriter = new animWriter();
		public const string animExportOptionScript = "animExportOptions";
		public const string animExportDefaultOptions = "precision=8;nodeNames=1;verboseUnits=0;whichRange=1;range=0:10;options=keys;hierarchy=none;controlPoints=0;shapes=1;helpPictures=0;useChannelBox=0;copyKeyCmd=";
		const int kDefaultPrecision = 8;

		public override void writer(MFileObject file, string options, MPxFileTranslator.FileAccessMode mode)
		{
			string fileName = file.fullName;
			StreamWriter animFile = new StreamWriter(fileName);

			//	Defaults.
			//
			string copyFlags = "copyKey -cb api -fea 1 ";
			int precision = kDefaultPrecision;
			bool nodeNames = true;
			bool verboseUnits = false;

			//	Parse the options. The options syntax is in the form of
			//	"flag=val;flag1=val;flag2=val"
			//
			if (options.Length > 0)
			{
				const string flagPrecision = "precision";
				const string flagNodeNames = "nodeNames";
				const string flagVerboseUnits = "verboseUnits";
				const string flagCopyKeyCmd = "copyKeyCmd";

				//	Start parsing.
				//
				string[] optionList = options.Split(new Char[] { ';' });

				int nOptions = optionList.Length;
				for (int i = 0; i < nOptions; i++)
				{
					string[] theOption = optionList[i].Split(new Char[] { '=' });
					if (theOption.Length < 1)
					{
						continue;
					}

					if (theOption[0] == flagPrecision && theOption.Length > 1)
					{
						precision = Convert.ToInt32(theOption[1]);
					}
					else if (theOption[0] == flagNodeNames && theOption.Length > 1)
					{
						if (Convert.ToInt32(theOption[1]) == 0)
							nodeNames = false;
						else
							nodeNames = true;
					}
					else if (theOption[0] == flagVerboseUnits && theOption.Length > 1)
					{
						if (Convert.ToInt32(theOption[1]) == 0)
							verboseUnits = false;
						else
							verboseUnits = true;
					}
					else if (theOption[0] == flagCopyKeyCmd && theOption.Length > 1)
					{

						//	Replace any '>' characters with '"'. This is needed
						//	since the file translator option boxes do not handle
						//	escaped quotation marks.
						//
						string optStr = theOption[1];
						string copyStr = "";
						for (int j = 0; j < optStr.Length; j++)
						{
							if (optStr[j] == '>')
								copyStr += '"';
							else
								copyStr += optStr[j];
						}

						copyFlags += copyStr;
					}
				}
			}

			//	Set the precision of the ofstream.
			//
			bool isExported = exportSelected(ref animFile, ref copyFlags, nodeNames, verboseUnits);
			animFile.Flush();
			animFile.Close();

			if (!isExported)
			{
				throw new ApplicationException("Exporting Anim Failed.");
			}

			return;
		}

		public override bool haveReadMethod()
		{
			return false;
		}

		public override bool haveWriteMethod()
		{
			return true;
		}

		public override string defaultExtension()
		{
			return "anim";
		}

		public override MFileKind identifyFile(MFileObject file, string buffer, short size)
		{
			string name = file.name;
			if (name.Length > 5 && string.Compare(name, name.Length - 5, defaultExtension(), 0, 5, true) == 0)
				return MFileKind.kIsMyFileType;

			return MFileKind.kNotMyFileType;
		}

		private bool exportSelected(ref StreamWriter animFile,
									ref string copyFlags,
									bool nodeNames = false,
									bool verboseUnits = false)
		{
			//	If the selection list is empty, then there are no anim curves
			//	to export.
			//
			MSelectionList sList = new MSelectionList();
			MGlobal.getActiveSelectionList(sList);
			if (sList.isEmpty)
			{
				string msg = MStringResource.getString(RegisterMStringResources.kNothingSelected);
				MGlobal.displayError(msg);
				return false;
			}

			//	Copy any anim curves to the API clipboard.
			//
			int result = 0;
			string command = copyFlags;
			try
			{
				MGlobal.executeCommand(command, out result, false, true);
			}
			catch (System.Exception)
			{
				string msg = MStringResource.getString(RegisterMStringResources.kAnimCurveNotFound);
				MGlobal.displayError(msg);
				return false;
			}

			if (result == 0 || MAnimCurveClipboard.theAPIClipboard.isEmpty)
			{
				string msg = MStringResource.getString(RegisterMStringResources.kAnimCurveNotFound);
				MGlobal.displayError(msg);
				return false;
			}
			fWriter.writeClipboard(ref animFile, MAnimCurveClipboard.theAPIClipboard, nodeNames, verboseUnits);

			return true;
		}
	}
}
