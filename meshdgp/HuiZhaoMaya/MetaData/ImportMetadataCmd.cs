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

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.MetaData;

[assembly: MPxCommandClass(typeof(MayaNetPlugin.ImportMetadataCmd), "importMetadataCSharp")]
namespace MayaNetPlugin
{
    [MPxCommandSyntaxSelection(ObjectType = typeof(MSelectionList), UseSelectionAsDefault = true)]
    [MPxCommandSyntaxMode(MPxCommandSyntaxModeAttribute.CommandMode.kEdit)]
    [MPxCommandSyntaxFlag("-fn", "-filename", Arg1 = typeof(System.String))]
    public class ImportMetadataCmd : metadataBase
    {
        const string kFileNameFlag = "-fn";
        const string kFileNameFlagLong = "-fileName";
        string fileName = "";

        public override void checkArgs(MArgDatabase argsDb)
        {
            if (argsDb.isFlagSet(kFileNameFlag))
            {
                fileName = argsDb.flagArgumentString(kFileNameFlag, 0);

                if (fileName == null)
                {
                    throw new System.ApplicationException("ExportMetadataCmd: You must specify the output path ex: -fn c:/mypath/thefile.meta.");
                }
            }
            base.checkArgs(argsDb);
        }
        public override void doCreate()
        {
            Associations associationsRead = null;
            MIStream inStream = MStreamUtils.CreateIFStream(fileName);
            string errors = "";
		    associationsRead = fSerialize.read( inStream, ref errors );
		    if( associationsRead == null)
		    {
			    String fmt = MStringResource.getString(MetaDataRegisterMStringResources.kImportMetadataFileReadFailed);
			    String msg;
                if(errors == null || errors.Length < 1)
                {
                    errors = "No errors was given by the serializer when reading the metadata file.";
                }
                msg = String.Format(fmt, fileName, errors);
			    displayError(msg);
                throw new System.ApplicationException(msg);
		    }

            String resultFmt = MStringResource.getString(MetaDataRegisterMStringResources.kImportMetadataResult);
	        for( int i=0; i<fMeshes.length; ++i )
	        {
		        MFnMesh mesh = new MFnMesh(fMeshes[i]);
		        // Should have filtered out non-meshes already but check anyway
		        if( mesh == null ) continue;

		        displayInfo( mesh.fullPathName );
                //We dont have the correct interface on MDGModifier to assign metadata so no undo redo for now
                Associations associationsMesh = mesh.metadata;
                associationsMesh.assign(associationsRead);
		        
			    for( int c=0; c < associationsRead.channelCount; ++c )
			    {
				    Autodesk.Maya.MetaData.Channel channel = associationsRead.channelAt((uint)c);
                    String cName = channel.nameProperty;
				    for( int s=0; s<channel.dataStreamCount; ++s )
				    {
					    Autodesk.Maya.MetaData.Stream cStream = channel.dataStream((uint)s);
					    if( cStream != null)
					    {
						    String sName = cStream.name;
						    String msg = String.Format( resultFmt, mesh.fullPathName, cName, sName );
						    appendToResult( msg );
					    }
				    }
			    }
	        }
            MStreamUtils.Close(inStream);
            inStream = null;
        }
    }
}
