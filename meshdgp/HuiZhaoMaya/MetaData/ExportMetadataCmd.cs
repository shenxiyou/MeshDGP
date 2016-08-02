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

[assembly: MPxCommandClass(typeof(MayaNetPlugin.ExportMetadataCmd), "exportMetadataCSharp")]
namespace MayaNetPlugin
{
    [MPxCommandSyntaxSelection(ObjectType = typeof(MSelectionList), UseSelectionAsDefault = true)]
    [MPxCommandSyntaxMode(MPxCommandSyntaxModeAttribute.CommandMode.kQuery)]
    [MPxCommandSyntaxFlag("-fn", "-filename", Arg1 = typeof(System.String))]
    public class ExportMetadataCmd : metadataBase
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
	        if( fMeshes.length != 1 )
            {
                throw new System.ApplicationException("ExportMetadataCmd: do not support less or more then 1 mesh.");
            }
	        MFnMesh mesh = new MFnMesh( fMeshes[0] );

	        displayInfo( mesh.fullPathName );

	        Associations associationsToWrite = mesh.metadata;
	        if( associationsToWrite == null)
            {
                throw new System.ApplicationException("ExportMetadataCmd: no association to write.");
            }

	        String	errors = "";

	        // Dump either to a file or the return string, depending on which was
	        // requested.
	        //
            MOStream destination = MStreamUtils.CreateOFStream(fileName);
		    if( fSerialize.write( associationsToWrite, destination, ref errors ) == 0 )
		    {
                setResult(fileName);
		    }
		    else
		    {
                String msg = MStringResource.getString(MetaDataRegisterMStringResources.kExportMetadataFailedFileWrite);
			    displayError(msg);
		    }
            MStreamUtils.Close(destination);
		    destination = null;

            if( errors != null && errors.Length > 0 )
	        {
		        displayError( errors );
		        return;
	        }

            return;
        }
    }
}
