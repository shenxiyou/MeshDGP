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
using System.Collections.Generic;
using System.Linq;
using System.Text;

 
using Autodesk.Maya.OpenMaya;


////////////////////////////////////////////////////////////////////////
// 
// 
// This plug-in is an example of a user-defined MPxMayaAsciiFilter. It
// allows users to segment scenes into files containing various types
// of nodes. Users can specify which node types are exported by 
// using the "includeType" and "excludeType" options in the file command.
// For example, the following would save only renderLayer nodes:
//
// mel: file -rename "myLayerData";
// mel: file -type "filteredAsciiFileCSharp";
// mel: file -save -options ";includeType kRenderLayer;";
//
// In addition, the type of connectAttrs written in the ASCII file are
// determined by the nodes types which are exported. If the source node
// in the connectAttr is to be output, then the connectAttr is written.
//
// In addition to allowing scenes to be segmented, this plugin also
// allows ASCII files to write a line which will source other ASCII
// files. This can be used to allow a single master file source in 
// all of the different files containing scene segments. Again, this is
// controlled via the file command's options. The following illustrates
// how to source in the render layer segment saved in the above example:
//
// mel: file -rename "myMasterFile";
// mel: file -save -options ";excludeType kRenderLayer; sourceFile myLayerData.faf"
// 
// Here, the excludeType argument is used to ensure no render layer 
// information is output in myMasterFile. The sourceFile option outputs
// the following line in myMasterFile.faf:
//
// customSourceFileCSharp -fileName "myLayerData.faf";
//
// When this line is parsed as myMasterFile.faf is loaded, the 
// customSourceFile command, also contained in this plugin, is executed.
// It simply sources myLayerData.faf, first prepending the 
// myMasterFile.faf's path.
//
////////////////////////////////////////////////////////////////////////

[assembly: MPxFileTranslatorClass(typeof(MayaNetPlugin.filteredAsciiFile), "filteredAsciiFileCSharp", null, null, null)]
[assembly: MPxCommandClass(typeof(MayaNetPlugin.sourceFileCmd), "customSourceFileCSharp")]

namespace MayaNetPlugin
{
    public class filteredAsciiFile : MPxMayaAsciiFilter
    {
        
        static string fExtension = "faf";
        static string fPluginName = "filteredAsciiFile";

	    private MStringArray	includedNodeTypesArray;
	    private MStringArray	excludedNodeTypesArray;
	    private MStringArray	sourcedFilesArray;
	    private MStringArray	excludedNamesArray;
	
	    private bool			outputRequirements;
	    private bool			outputReferences;

        private bool isNodeTypeIncluded(string nodeType)
        {
            if ((excludedNodeTypesArray.length == 0) &&
                 (includedNodeTypesArray.length == 0))
            {
                // there are no types specifically included or excluded,
                // so we assume everything is included.

                return true;
            }

            // if we aren't excluding any nodes at this point, then we're
            // only interested in nodes which are explicitly included.

            bool result = (excludedNodeTypesArray.length > 0);

            for (int i = 0; i < includedNodeTypesArray.length; i++)
            {
                if (nodeType == includedNodeTypesArray[i])
                {
                    result = true;
                    break;
                }
            }

            for (int j = 0; j < excludedNodeTypesArray.length; j++)
            {
                if (nodeType == excludedNodeTypesArray[j])
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        private bool isNodeNameExcluded(MObject node)
        {
            for (int i = 0; i < excludedNamesArray.length; i++)
            {
                MFnDependencyNode depNode = new MFnDependencyNode(node);

                if (excludedNamesArray[i] == depNode.name)
                {
                    return true;
                }
            }

            return false;
        }

        public override string defaultExtension()
        {
            return fExtension;
        }

        protected override void processReadOptions(string optionsString)
        {
            return;
        }
        protected override void processWriteOptions(string optionsString)
        {
	        includedNodeTypesArray = new MStringArray();
            excludedNodeTypesArray = new MStringArray();
            sourcedFilesArray = new MStringArray();
            excludedNamesArray = new MStringArray();
	
	        outputRequirements = true;
	        outputReferences = true;
	
            MStringArray optionsArray = new MStringArray(optionsString.Split(';'));

	        for (int i = 0; i < optionsArray.length; i++)
	        {
		        string option = optionsArray[i];
		        MStringArray optionArray = new MStringArray(option.Split(' '));

		        if (optionArray[0] == "includeNodeType" && optionArray.length > 1)
		        {
			        includedNodeTypesArray.append(optionArray[1]);
		        }
		        else if (optionArray[0] == "excludeNodeType" && optionArray.length > 1)
		        {
			        excludedNodeTypesArray.append(optionArray[1]);
		        }
		        else if (optionArray[0] == "sourceFile" && optionArray.length > 1)
		        {
			        sourcedFilesArray.append(optionArray[1]);
		        }
	        }

            return;
        }

        protected override bool writesRequirements()
        {
            return outputRequirements;
        }

        protected override bool writesCreateNode(MObject node)
        {
            bool result = false;

            MFnDependencyNode depNode = new MFnDependencyNode(node);

	        if (depNode.isFromReferencedFile && !outputReferences)
	        {
		        return false;
	        }

	        if (isNodeNameExcluded(node))
	        {
		        return false;
	        }

	        if (!result)
	        {
		        result = isNodeTypeIncluded(node.apiTypeStr);
	        }

	        return result;
        }

        protected override bool writesSelectNode(MObject node)
        {
            return writesCreateNode(node);
        }

        protected override bool writesFileReference(MFileObject referenceFile)
        {
            return outputReferences;
        }

        protected override bool writesConnectAttr(MPlug srcPlug, MPlug destPlug)
        {
            return (writesCreateNode(srcPlug.node) && !isNodeNameExcluded(destPlug.node));
        }

        protected override bool writesSetAttr(MPlug srcPlug)
        {
            return writesCreateNode(srcPlug.node);
        }

        protected override void writePostRequires(MPxMayaAsciiFilterOutput fileIO)
        {
            string data = string.Format("requires {0} \"1.0\";\n", fPluginName); 
            fileIO.Write(data);
            return;
        }

        protected override void writePostCreateNodesBlock(MPxMayaAsciiFilterOutput fileIO)
        {
            for (int i = 0; i < sourcedFilesArray.length; i++)
	        {
                fileIO.Write("eval (\"customSourceFile -fileName \\\"");
                fileIO.Write(sourcedFilesArray[i]);
                fileIO.Write("\\\";\");\n");
	        }
            return;
        }

    }


    [MPxCommandSyntaxFlag("-fn", "-fileName", Arg1 = typeof(System.String))]
    public class sourceFileCmd : MPxCommand, IMPxCommand
    {
        const string kFileNameFlag = "-fn";
        const string kFileNameFlagLong = "-fileName";

        public override void doIt(MArgList args)
        {
            string fileName;

            MArgDatabase argData = new MArgDatabase(syntax, args);
          
            if (argData.isFlagSet(kFileNameFlag))
            {
	            fileName = argData.flagArgumentString(kFileNameFlag, 0);

	            if (fileName != null)
	            {
		            string currFile = MFileIO.fileCurrentlyLoading;

		            MStringArray pathDirectories = new MStringArray(currFile.Split('/'));

		            if (pathDirectories.length > 0)
		            {
			            string expandedFileName = "";

			            for (int i = 0; i < pathDirectories.length-1; i++)
			            {
				            expandedFileName += pathDirectories[i];
				            expandedFileName += "/";
			            }

			            expandedFileName += fileName;

			            MGlobal.sourceFile(expandedFileName);
		            }
	            }
            }	

            return;
        }
    }
}