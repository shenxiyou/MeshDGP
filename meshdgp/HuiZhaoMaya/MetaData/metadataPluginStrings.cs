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
using System.IO;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaAnim;

namespace MayaNetPlugin
{
    public class MetaDataRegisterMStringResources
    {
        public static string kPluginId = "adskmetadata";

        public static MStringResourceId kInvalidComponentType = new MStringResourceId(RegisterMStringResources.kPluginId, "kInvalidComponentType", "Component type '{0}' not one of the legal values ('edge', 'face', 'vertex', 'faceVertex')");
        public static MStringResourceId kFlagMandatory = new MStringResourceId(RegisterMStringResources.kPluginId, "kFlagMandatory", "Missing mandatory flag '{0}'.");
        public static MStringResourceId kOnlyCreateModeMsg = new MStringResourceId(RegisterMStringResources.kPluginId, "kOnlyCreateModeMsg", "'{0}'flag can only be used in create mode.");
        public static MStringResourceId kInvalidFlag = new MStringResourceId(RegisterMStringResources.kPluginId, "kInvalidFileFlag", "Value for flag '{0}' is invalid.");
        public static MStringResourceId kFileIgnored = new MStringResourceId(RegisterMStringResources.kPluginId, "kFileIgnored", "The '{0}' flag is overridden by the '{1}' flag. It will be ignored.");
        public static MStringResourceId kFileNotFound = new MStringResourceId(RegisterMStringResources.kPluginId, "kFileNotFound", "File '{0}' does not exist, import aborted.");
        public static MStringResourceId kFileOrStringNeeded = new MStringResourceId(RegisterMStringResources.kPluginId, "kFileOrStringNeeded", "If no file is specified the '{0}' flag must have a value.");
        public static MStringResourceId kInvalidStream = new MStringResourceId(RegisterMStringResources.kPluginId, "kInvalidStream", "Stream name is not legal.");
        public static MStringResourceId kInvalidString = new MStringResourceId(RegisterMStringResources.kPluginId, "kInvalidString", "String is not legal.");
        public static MStringResourceId kEditQueryFlagErrorMsg = new MStringResourceId(RegisterMStringResources.kPluginId, "kEditQueryFlagErrorMsg", "Can't specify edit and query flags simultanously.");
        public static MStringResourceId kNometadataError = new MStringResourceId(RegisterMStringResources.kPluginId, "kNometadataError", "Object '{0}' does not have data stream on component '{1}'.");
        public static MStringResourceId kObjectNotFoundError = new MStringResourceId(RegisterMStringResources.kPluginId, "kObjectNotFoundError", "Object '{0}' not found.");
        public static MStringResourceId kObjectTypeError = new MStringResourceId(RegisterMStringResources.kPluginId, "kObjectTypeError", "Object '{0}' is not a legal type. Only meshes are valid.");
        public static MStringResourceId kTypeUnspecified = new MStringResourceId(RegisterMStringResources.kPluginId, "kTypeUnspecified", "(unspecified).");
        public static MStringResourceId kMetadataFormatNotFound = new MStringResourceId(RegisterMStringResources.kPluginId, "kMetadataFormatNotFound", "Metadata format type '{0}' was not found.");
        public static MStringResourceId kCreateMetadataCreateFailed = new MStringResourceId(RegisterMStringResources.kPluginId, "kCreateMetadataCreateFailed", "Could not create new metadata.");
        public static MStringResourceId kCreateMetadataHasStream = new MStringResourceId(RegisterMStringResources.kPluginId, "kCreateMetadataHasStream", "A stream named '{0}' already exists - skipping creation.");
        public static MStringResourceId kCreateMetadataStructureNotFound = new MStringResourceId(RegisterMStringResources.kPluginId, "kCreateMetadataStructureNotFound", "Could not find structure '{0}'.");
        public static MStringResourceId kCreateMetadataNoStructureName = new MStringResourceId(RegisterMStringResources.kPluginId, "kCreateMetadataNoStructureName", "The 'structureName' flag is mandatory.");
        public static MStringResourceId kCreateMetadataNoStreamName = new MStringResourceId(RegisterMStringResources.kPluginId, "kCreateMetadataNoStreamName", "The 'streamName' flag is mandatory.");
        public static MStringResourceId kCreateMetadataNoChannelType = new MStringResourceId(RegisterMStringResources.kPluginId, "kCreateMetadataNoChannelType", "The 'channelType' flag is mandatory.");
        public static MStringResourceId kImportMetadataStringReadFailed = new MStringResourceId(RegisterMStringResources.kPluginId, "kImportMetadataStringReadFailed", "Metadata read from string arg failed with '{0}'.");
        public static MStringResourceId kImportMetadataFileReadFailed = new MStringResourceId(RegisterMStringResources.kPluginId, "kImportMetadataFileReadFailed", "Metadata read from file '{0}' failed with '{1}'.");
        public static MStringResourceId kImportMetadataSetMetadataFailed = new MStringResourceId(RegisterMStringResources.kPluginId, "kImportMetadataSetMetadataFailed", "Metadata could not be set on mesh '{0}'.");
        public static MStringResourceId kImportMetadataUndoMissing = new MStringResourceId(RegisterMStringResources.kPluginId, "kImportMetadataUndoMissing", "Undo information not present for importMetadata.");
        public static MStringResourceId kImportMetadataResult = new MStringResourceId(RegisterMStringResources.kPluginId, "kImportMetadataResult", "{0}/{1}/{2}");
        public static MStringResourceId kExportMetadataFailedFileWrite = new MStringResourceId(RegisterMStringResources.kPluginId, "kExportMetadataFailedFileWrite", "Failed while exporting metadata to file");
        public static MStringResourceId kExportMetadataFailedStringWrite = new MStringResourceId(RegisterMStringResources.kPluginId, "kExportMetadataFailedStringWrite", "Failed while exporting metadata to string");
        public static MStringResourceId kExportMetadataUndoMissing = new MStringResourceId(RegisterMStringResources.kPluginId, "kExportMetadataUndoMissing", "Undo information not present for exportMetadata");
        public static MStringResourceId kStructureXMLStructureNameNotFound = new MStringResourceId(RegisterMStringResources.kPluginId, "kStructureXMLStructureNameNotFound", "Structure name not found at line {0}.");
        public static MStringResourceId kStructureXMLMemberNameNotFound = new MStringResourceId(RegisterMStringResources.kPluginId, "kStructureXMLMemberNameNotFound", "Member name not found at line {0}.");
        public static MStringResourceId kStructureXMLMemberTypeNotFound = new MStringResourceId(RegisterMStringResources.kPluginId, "kStructureXMLMemberTypeNotFound", "Member type not found at line {0}.");
        public static MStringResourceId kStructureXMLInfoPre = new MStringResourceId(RegisterMStringResources.kPluginId, "kStructureXMLInfoPre",
            "\nThe file adskDataStructure.xsd contains the official" +
	        "\nstructure  validation  format. This is a less formal" +
	        "\ndescription more useful to a non-technical user." +
	        "\n" +
	        "\nThe first line is always a standard boilerplate" +
	        "\n    <?xml version='1.0' encoding='UTF-8'?>" +
	        "\n" +
	        "\nThe second line is always the same identifier along" +
	        "\nwith the name of the structure being defined" +
	        "\n    <structure name='MyStructure'>" +
	        "\n" +
	        "\nAt the end of the file is a matching closing tag" +
	        "\n    </structure>" +
	        "\n" +
	        "\nIn between these tags are a list of member tags" +
	        "\n    <member [dim=DIM] name='MyMember' type='TYPE' />" +
	        "\n" +
	        "\nTYPE is one  of the valid  structure member types" +
	        "\ntaken from adsk::Data::Structure:" +
	        "\n" +
	        "\n    (");
        public static MStringResourceId kStructureXMLInfoPost = new MStringResourceId(RegisterMStringResources.kPluginId, "kStructureXMLInfoPost",
	        ")\n\n" +
	        "\nDIM is an optional attribute denoting the length" +
	        "\nof the data  member  type  with a  default of 1." +
	        "\ne.g. dim='3' type='float' corresponds to  a data" +
	        "\nmember consisting of 3 float values." +
	        "\n" +
	        "\nAs in all XML  the tags may  be separated by any" +
	        "\namount  of  whitespace,  including  none.  Files" +
	        "\nwritten out in  this  format  will use  standard" +
	        "\nindentiation  for  nested  tags and  newlines to" +
	        "\nseparate tags.  The  original formatting will be" +
	        "\nlost after the file is read." );
        public static MStringResourceId kStructureDotNetTypeInfoPre = new MStringResourceId(RegisterMStringResources.kPluginId, "kStructureDotNetTypeInfoPre",
            "\nThe file adskDataStructure.xsd contains the official" +
            "\nstructure  validation  format. This is a less formal" +
            "\ndescription more useful to a non-technical user." +
            "\n" +
            "\nThis contains the name of the DotNet type used to describe the meta data Structure" +
            "\n" +
            "\n    (");
        public static MStringResourceId kStructureDotNetTypeInfoPost = new MStringResourceId(RegisterMStringResources.kPluginId, "kStructureDotNetTypeInfoPost",
            ")\n\n" );

        static public void registerStringResources()
        {
            MStringResource.registerString(MetaDataRegisterMStringResources.kCreateMetadataCreateFailed);
            MStringResource.registerString(MetaDataRegisterMStringResources.kCreateMetadataHasStream);
            MStringResource.registerString(MetaDataRegisterMStringResources.kCreateMetadataNoChannelType);
            MStringResource.registerString(MetaDataRegisterMStringResources.kCreateMetadataNoStreamName);
            MStringResource.registerString(MetaDataRegisterMStringResources.kCreateMetadataNoStructureName);
            MStringResource.registerString(MetaDataRegisterMStringResources.kCreateMetadataStructureNotFound);
            MStringResource.registerString(MetaDataRegisterMStringResources.kEditQueryFlagErrorMsg);
            MStringResource.registerString(MetaDataRegisterMStringResources.kExportMetadataFailedFileWrite);
            MStringResource.registerString(MetaDataRegisterMStringResources.kExportMetadataFailedStringWrite);
            MStringResource.registerString(MetaDataRegisterMStringResources.kExportMetadataUndoMissing);
            MStringResource.registerString(MetaDataRegisterMStringResources.kFileIgnored);
            MStringResource.registerString(MetaDataRegisterMStringResources.kFileNotFound);
            MStringResource.registerString(MetaDataRegisterMStringResources.kFileOrStringNeeded);
            MStringResource.registerString(MetaDataRegisterMStringResources.kFlagMandatory);
            MStringResource.registerString(MetaDataRegisterMStringResources.kImportMetadataFileReadFailed);
            MStringResource.registerString(MetaDataRegisterMStringResources.kImportMetadataResult);
            MStringResource.registerString(MetaDataRegisterMStringResources.kImportMetadataSetMetadataFailed);
            MStringResource.registerString(MetaDataRegisterMStringResources.kImportMetadataStringReadFailed);
            MStringResource.registerString(MetaDataRegisterMStringResources.kImportMetadataUndoMissing);
            MStringResource.registerString(MetaDataRegisterMStringResources.kInvalidComponentType);
            MStringResource.registerString(MetaDataRegisterMStringResources.kInvalidFlag);
            MStringResource.registerString(MetaDataRegisterMStringResources.kInvalidStream);
            MStringResource.registerString(MetaDataRegisterMStringResources.kInvalidString);
            MStringResource.registerString(MetaDataRegisterMStringResources.kMetadataFormatNotFound);
            MStringResource.registerString(MetaDataRegisterMStringResources.kNometadataError);
            MStringResource.registerString(MetaDataRegisterMStringResources.kObjectNotFoundError);
            MStringResource.registerString(MetaDataRegisterMStringResources.kObjectTypeError);
            MStringResource.registerString(MetaDataRegisterMStringResources.kOnlyCreateModeMsg);
            MStringResource.registerString(MetaDataRegisterMStringResources.kStructureXMLInfoPost);
            MStringResource.registerString(MetaDataRegisterMStringResources.kStructureXMLInfoPre);
            MStringResource.registerString(MetaDataRegisterMStringResources.kStructureXMLMemberNameNotFound);
            MStringResource.registerString(MetaDataRegisterMStringResources.kStructureXMLMemberTypeNotFound);
            MStringResource.registerString(MetaDataRegisterMStringResources.kStructureXMLStructureNameNotFound);
            MStringResource.registerString(MetaDataRegisterMStringResources.kStructureDotNetTypeInfoPost);
            MStringResource.registerString(MetaDataRegisterMStringResources.kStructureDotNetTypeInfoPre);
            MStringResource.registerString(MetaDataRegisterMStringResources.kTypeUnspecified);
        }
    }

    
}
