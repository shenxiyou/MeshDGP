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
using System.Diagnostics;
using System.Linq;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.MetaData;

[assembly: MPxCommandClass(typeof(MayaNetPlugin.WriteMetadataToConsoleCmd), "WriteMetadataToConsoleCSharp")]
namespace MayaNetPlugin
{
    [MPxCommandSyntaxSelection(ObjectType = typeof(MSelectionList), UseSelectionAsDefault = true)]
    public class WriteMetadataToConsoleCmd : MPxCommand, IMPxCommand
	{ 
		private MObjectArray fNodes = new MObjectArray();// Node(s) on which to create metadata

		override public bool isUndoable()
		{
			return false;
		}

        //======================================================================
		//
		// Check the parsed arguments and do/undo/redo the command as appropriate
		//
		void checkArgs(ref MArgDatabase argsDb)
		{
			//----------------------------------------
			// (selection list)
			//
			// Commands need at least one node on which to operate so gather up
			// the list of nodes specified and/or selected.
			//

			// Empty out the list of nodes on which to operate so that it can be
			// populated from the selection or specified lists.
			fNodes.clear();
			MSelectionList objects = new MSelectionList();
			argsDb.getObjects(objects);

			for (uint i = 0; i < objects.length; ++i)
			{
                MDagPath dagPath = new MDagPath();
                objects.getDagPath((uint)i, dagPath);
                MFnDagNode dagNode = new MFnDagNode(dagPath.node);
                MObject obj = dagNode.child(0);
                if (obj.apiTypeStr == "kMesh")
                {
                    if (obj == MObject.kNullObj)
                    {
                        throw new ApplicationException("Error: objects.getDependNode() ");
                    }
                    fNodes.append(dagPath.node);
                }
                else
                {
                    String fmt = MStringResource.getString(MetaDataRegisterMStringResources.kObjectTypeError);
                    String msg = String.Format(fmt, dagPath.fullPathName + "[" + obj.apiTypeStr + "]");
                    displayError(msg);
                    throw new System.InvalidOperationException(msg);
                }
			}

			if (fNodes.length == 0)
			{
				string errMsg = MStringResource.getString(MetaDataRegisterMStringResources.kObjectNotFoundError);
				throw new ArgumentException(errMsg, "argsDb");
			}
		}

		//======================================================================
		//
		// Do the metadata creation. The metadata will be randomly initialized
		// based on the channel type and the structure specified. For recognized
		// components the number of metadata elements will correspond to the count
		// of components in the selected mesh, otherwise a random number of metadata
		// elements between 1 and 100 will be created (at consecutive indices).
		//
		// The previously existing metadata is preserved for later undo.
		//
		override public void doIt(MArgList args)
		{
			MArgDatabase argsDb = new MArgDatabase(syntax, args);

			checkArgs(ref argsDb);
			
			clearResult();

			uint numNodes = fNodes.length;
			for ( int i = 0; i < numNodes; ++i)
			{
                // fNodes[i] is the transform not the shape itself
                MFnDagNode dagNode = new MFnDagNode(fNodes[i]);
                MObject obj = dagNode.child(0);
                // obj is the shape, which is where we can add the meta data
				MFnDependencyNode node = new MFnDependencyNode(obj);

                Console.WriteLine( "METADATA for node " + dagNode.fullPathName );
                Console.WriteLine( "=====================================================================" );

                foreach( Channel chn in node.metadata)
                {
                    Console.WriteLine("Channel: type = {0}, nbStreams = {1}", chn.nameProperty, chn.dataStreamCount);
                    foreach (Stream strm in chn)
                    {
                        Console.WriteLine("Stream: name = {0}, nbElements = {1}", strm.name, strm.elementCount() );

                        Structure strct = strm.structure;

                        Console.WriteLine("Structure: name = {0}, nbMembers = {1}", strct.name, strct.memberCount );

                        string[] memberNames = new string[strct.memberCount];
                        int memberID = -1;
                        foreach( Member member in strct )
                        {
                            ++memberID;
                            Console.WriteLine("Structure member: name = {0}, type = {1}, array size = {2}", member.nameProperty, member.typeProperty.ToString(), member.lengthProperty );
                            memberNames[memberID] = member.nameProperty;
                        }
                        
                        int k = -1;

                        foreach (Handle handle in strm)
                        {
                            ++k;
                            for (uint n = 0; n < strct.memberCount; ++n)
                            {
                                handle.setPositionByMemberIndex(n);

                                Array data = handle.asType;

                                if( data.Length < 1 )
                                    throw new ApplicationException( "Handle data seems corrupted" );

                                string line = string.Format( "Handle #{0}, member = {1}, data = {2}", k, memberNames[n], data.GetValue(0).ToString() );

                                if( data.Length > 1 )
                                {
                                    for( int d = 1; d < data.Length; ++d )
                                    {
                                        line = line + "," + data.GetValue(d).ToString();
                                    }
                                }

                                Console.WriteLine( line );
                            }
                        }
                    }
                }
			}
		}
	}
}
