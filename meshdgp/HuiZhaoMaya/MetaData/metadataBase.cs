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
using System.Collections.Generic;

namespace MayaNetPlugin
{
    public class metadataBase : MPxCommand, IUndoMPxCommand
    {
        protected CommandMode	 fMode;	 	// Command mode
	    protected MObjectArray fMeshes;	// Mesh(es) to which the command applies
	    protected AssociationsSerializer fSerialize; // Serialization format
        
        public metadataBase() : base()
        {
            fMode = CommandMode.kCreate;
            fMeshes = new MObjectArray();
        }

        public override void doIt(MArgList args)
        {
	        MArgDatabase argsDb = new MArgDatabase(syntax, args);

            checkArgs(argsDb);
		    clearResult();
		    switch (fMode)
		    {
                case CommandMode.kCreate: doCreate(); break;
                case CommandMode.kEdit: doEdit(); break;
                case CommandMode.kQuery: doQuery(); break;
		    }
        }

        //======================================================================
        //
        // Since this isn't a real command it doesn't do anything. This method
        // is defined anyway so that the derived commands can choose to override
        // or not.
        //
        public virtual void doCreate()
        {
        }

        //======================================================================
        //
        // Since this isn't a real command it doesn't do anything. This method
        // is defined anyway so that the derived commands can choose to override
        // or not.
        //
        public virtual void doEdit()
        {
        }

        //======================================================================
        //
        // Since this isn't a real command it doesn't do anything. This method
        // is defined anyway so that the derived commands can choose to override
        // or not.
        //
        public virtual void doQuery()
        {
        }



        public override void redoIt()
        {
        }

        public override void undoIt()
        {
        }

        public override bool isUndoable()
        {
            return false;
        }


        //======================================================================
        //
        // Look through the arg database and verify that the arguments are
        // valid. Only checks the common flags so derived classes should call
        // this parent method first before checking their own flags.
        //
        public virtual void checkArgs(MArgDatabase argsDb)
        {
            String formatType = "raw";
	        fSerialize = AssociationsSerializer.formatByName( formatType );
	        if( fSerialize == null)
	        {
		        String fmt = MStringResource.getString(MetaDataRegisterMStringResources.kMetadataFormatNotFound);
                String msg = String.Format(fmt, formatType);
			    displayError(msg);
			    throw new System.ArgumentException(msg);
	        }

	        //----------------------------------------
	        // (selection list)
	        //
	        // Commands need at least one mesh object on which to operate so gather up
	        // the list of meshes specified and/or selected.
	        //

	        // Empty out the list of meshes on which to operate so that it can be
	        // populated from the selection or specified lists.
	        fMeshes.clear();

	        MSelectionList objects = new MSelectionList();
	        argsDb.getObjects(objects);
	        for (int i = 0; i<objects.length; ++i)
	        {
		        MDagPath dagPath = new MDagPath();
		        objects.getDagPath((uint)i, dagPath);

		        MFnDagNode dagNode = new MFnDagNode( dagPath.node );
                MObject obj = dagNode.child(0);
                if (obj.apiTypeStr == "kMesh")
		        {
                    MFnMesh mesh = new MFnMesh(obj);
                    if(mesh != null)
                        fMeshes.append(obj);
		        }
		        else
		        {
			        String fmt = MStringResource.getString(MetaDataRegisterMStringResources.kObjectTypeError);
                    String msg = String.Format(fmt, dagPath.fullPathName + "[" + obj.apiTypeStr + "]");
			        displayError(msg);
			        throw new System.InvalidOperationException(msg);
		        }
	        }

	        if( fMeshes.length == 0 )
	        {
		        String msg = MStringResource.getString(MetaDataRegisterMStringResources.kObjectNotFoundError);
		        displayError(msg);
                throw new System.InvalidOperationException(msg);
	        }
        }
    }
}