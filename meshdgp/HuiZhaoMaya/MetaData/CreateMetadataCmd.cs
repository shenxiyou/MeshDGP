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

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.MetaData;

[assembly: MPxCommandClass(typeof(MayaNetPlugin.CreateMetadataCmd), "createMetadataCSharp")]
namespace MayaNetPlugin
{
    [MPxCommandSyntaxFlag("-ct", "-channelType", Arg1 = typeof(string))]
    [MPxCommandSyntaxFlag("-sn", "-streamName", Arg1 = typeof(string))]
    [MPxCommandSyntaxFlag("-s", "-structure", Arg1 = typeof(string))]
    [MPxCommandSyntaxSelection(ObjectType = typeof(MSelectionList), UseSelectionAsDefault = true)]
	public class CreateMetadataCmd : MPxCommand, IMPxCommand
	{
		public static readonly string flagChannelType = "-ct";
		public static readonly string flagChannelTypeLong = "-channelType";
		public static readonly string flagStreamName = "-sn";
		public static readonly string flagStreamNameLong = "-streamName";
		public static readonly string flagStructure = "-s";
		public static readonly string flagStructureLong = "-structure";

		private OptFlag fChannelTypeFlag = new OptFlag();
		private OptFlag fStreamNameFlag = new OptFlag();
		private OptFlag fStructureFlag = new OptFlag();

		private string fChannelType = "";// Channel on which to create metadata
		private Structure fStructure = null;// Structure to use for creation
		private MObjectArray fNodes = new MObjectArray();// Node(s) on which to create metadata
		private MDGModifier fDGModifier = new MDGModifier();// Information needed for undo/redo
		private string fStreamName= "";  // Name to give the new stream

		override public bool isUndoable()
		{
			return true;
		}

		override public void redoIt()
		{
			fDGModifier.doIt();
		}

		override public void undoIt()
		{
			fDGModifier.undoIt();
		}
		//======================================================================
		//
		// Check the parsed arguments and do/undo/redo the command as appropriate
		//
		void checkArgs(ref MArgDatabase argsDb)
		{
			//----------------------------------------
			// -structure flag
			//
			fStructureFlag.parse(ref argsDb, flagStructure);
			if (fStructureFlag.isSet())
			{
				if (!fStructureFlag.isArgValid())
				{
					string errMsg = MStringResource.getString(MetaDataRegisterMStringResources.kInvalidString);
					throw new ArgumentException(errMsg, "argsDb");
				}

				string structureName = fStructureFlag.arg();
				try
				{
					fStructure = Structure.structureByName(structureName);
				}
				catch (System.Exception)
				{
					string msgFmt = MStringResource.getString(MetaDataRegisterMStringResources.kCreateMetadataStructureNotFound);
					throw new ArgumentException(String.Format(msgFmt, structureName), "argsDb");
				}

			}
			else
			{
				string errMsg = MStringResource.getString(MetaDataRegisterMStringResources.kCreateMetadataNoStructureName);
				throw new ArgumentException(errMsg, "argsDb");
			}

			//----------------------------------------
			// -streamName flag
			//
			fStreamNameFlag.parse(ref argsDb, flagStreamName);
			if (fStreamNameFlag.isSet())
			{
				if (!fStreamNameFlag.isArgValid())
				{
					string errMsg = MStringResource.getString(MetaDataRegisterMStringResources.kInvalidString);
					throw new ArgumentException(errMsg, "argsDb");
				}
				fStreamName = fStreamNameFlag.arg();
			}
			else
			{
				string errMsg = MStringResource.getString(MetaDataRegisterMStringResources.kCreateMetadataNoStructureName);
				throw new ArgumentException(errMsg, "argsDb");

			}

			//----------------------------------------
			// -channelType flag
			//
			fChannelTypeFlag.parse(ref argsDb, flagChannelType);
			if (fChannelTypeFlag.isSet())
			{
				if (!fChannelTypeFlag.isArgValid())
				{
					string errMsg = MStringResource.getString(MetaDataRegisterMStringResources.kInvalidString);
					throw new ArgumentException(errMsg, "argsDb");
				}
				fChannelType = fChannelTypeFlag.arg();
			}
			else
			{
				string errMsg = MStringResource.getString(MetaDataRegisterMStringResources.kCreateMetadataNoStructureName);
				throw new ArgumentException(errMsg, "argsDb");
			}

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
			int i;
			for (i = 0; i < numNodes; ++i)
			{
                // fNodes[i] is the transform not the shape itself
                MFnDagNode dagNode = new MFnDagNode(fNodes[i]);
                MObject obj = dagNode.child(0);
                // obj is the shape, which is where we can add the meta data
				MFnDependencyNode node = new MFnDependencyNode(obj);
				// Get the current metadata (empty if none yet)
				Associations newMetadata = new Associations(node.metadata);
				Channel newChannel = newMetadata.channel(fChannelType);

				// Check to see if the requested stream name already exists
				Stream oldStream = newChannel.dataStream(fStreamName);
				if (oldStream != null)
				{

					string fmt = MStringResource.getString(MetaDataRegisterMStringResources.kCreateMetadataHasStream);
					string msg = String.Format(fmt, fStreamName);
					MGlobal.displayError( msg );
					continue;
				}

				Stream newStream = new Stream(fStructure, fStreamName);

                string strmName = newStream.name;

				int indexCount = 0;
                MFnMesh mesh = null;
                Random rndIndexCount = new Random();
                // Treat the channel type initializations different for meshes
				if (obj.hasFn(MFn.Type.kMesh))
				{
                    mesh = new MFnMesh(obj);
					// Get mesh-specific channel type parameters
					if (fChannelType == "face")
					{
						indexCount = mesh.numPolygons;
					}
					else if (fChannelType == "edge")
					{
						indexCount = mesh.numEdges;
					}
					else if (fChannelType == "vertex")
					{
						indexCount = mesh.numVertices;
					}
					else if (fChannelType == "vertexFace")
					{
						indexCount = mesh.numFaceVertices;
					}
					else
					{
						// Set a random number between 1 to 100
                        indexCount = rndIndexCount.Next(1, 100);
					}
				}
				else
				{
					// Create generic channel type information
                    indexCount = rndIndexCount.Next(1, 100);
				}

				// Fill specified stream ranges with random data
				int structureMemberCount = fStructure.memberCount;
				uint m,n,d;
                Random rnd = new Random();
                for (m = 0; m < indexCount; ++m)
				{
					// Walk each structure member and fill with random data
					// tailored to the member data type.
					Handle handle = new Handle(fStructure);
					for (n = 0; n < structureMemberCount; ++n)
					{
						handle.setPositionByMemberIndex(n);

						switch (handle.dataType)
						{
						case Member.eDataType.kBoolean:
							{
                                bool[] data = new bool[handle.dataLength];
						        for (d = 0; d < handle.dataLength; ++d)
						        {
									int randomInt = rnd.Next(0, 2);
									bool randomBool = randomInt == 1 ? true : false;
                                    data[d] = randomBool;
                                }
                                handle.asBooleanArray = data;
								break;
							}
						case Member.eDataType.kDouble:
							{
                                double[] data = new double[handle.dataLength];
						        for (d = 0; d < handle.dataLength; ++d)
						        {
									// Set a random number between -2000000000.0.0 and 2000000000.0.0
									data[d] = rnd.NextDouble()*4000000000.0 - 2000000000.0 ;
                                }
                                handle.asDoubleArray = data;
								break;
							}
						case Member.eDataType.kDoubleMatrix4x4:
							{
                                double[] data = new double[handle.dataLength * 16];
						        for (d = 0; d < handle.dataLength; ++d)
						        {
									data[d*16+0] = rnd.NextDouble()*4000000000.0 - 2000000000.0 ;
									data[d*16+1] = rnd.NextDouble()*4000000000.0 - 2000000000.0 ;
									data[d*16+2] = rnd.NextDouble()*4000000000.0 - 2000000000.0 ;
									data[d*16+3] = rnd.NextDouble()*4000000000.0 - 2000000000.0 ;
									data[d*16+4] = rnd.NextDouble()*4000000000.0 - 2000000000.0 ;
									data[d*16+5] = rnd.NextDouble()*4000000000.0 - 2000000000.0 ;
									data[d*16+6] = rnd.NextDouble()*4000000000.0 - 2000000000.0 ;
									data[d*16+7] = rnd.NextDouble()*4000000000.0 - 2000000000.0 ;
									data[d*16+8] = rnd.NextDouble()*4000000000.0 - 2000000000.0 ;
									data[d*16+9] = rnd.NextDouble()*4000000000.0 - 2000000000.0 ;
									data[d*16+10] = rnd.NextDouble()*4000000000.0 - 2000000000.0 ;
									data[d*16+11] = rnd.NextDouble()*4000000000.0 - 2000000000.0 ;
									data[d*16+12] = rnd.NextDouble()*4000000000.0 - 2000000000.0 ;
									data[d*16+13] = rnd.NextDouble()*4000000000.0 - 2000000000.0 ;
									data[d*16+14] = rnd.NextDouble()*4000000000.0 - 2000000000.0 ;
									data[d*16+15] = rnd.NextDouble()*4000000000.0 - 2000000000.0 ;
                                }
                                handle.asDoubleMatrix4x4 = data;
								break;
							}
						case Member.eDataType.kFloat:
							{
                                float[] data = new float[handle.dataLength];
						        for (d = 0; d < handle.dataLength; ++d)
						        {
    								// Set a random number between -2000000.0 and 2000000.0
	    							data[d] = (float)rnd.NextDouble()*4000000.0f - 2000000.0f ;
                                }
                                handle.asFloatArray = data;
								break;
							}
						case Member.eDataType.kFloatMatrix4x4:
							{
                                float[] data = new float[handle.dataLength * 16];
						        for (d = 0; d < handle.dataLength; ++d)
						        {
									// Set a random number between -2000000.0 and 2000000.0
									data[d*16+0] = (float)rnd.NextDouble()*4000000.0f - 2000000.0f ;
									data[d*16+1] = (float)rnd.NextDouble()*4000000.0f - 2000000.0f ;
									data[d*16+2] = (float)rnd.NextDouble()*4000000.0f - 2000000.0f ;
									data[d*16+3] = (float)rnd.NextDouble()*4000000.0f - 2000000.0f ;
									data[d*16+4] = (float)rnd.NextDouble()*4000000.0f - 2000000.0f ;
									data[d*16+5] = (float)rnd.NextDouble()*4000000.0f - 2000000.0f ;
									data[d*16+6] = (float)rnd.NextDouble()*4000000.0f - 2000000.0f ;
									data[d*16+7] = (float)rnd.NextDouble()*4000000.0f - 2000000.0f ;
									data[d*16+8] = (float)rnd.NextDouble()*4000000.0f - 2000000.0f ;
									data[d*16+9] = (float)rnd.NextDouble()*4000000.0f - 2000000.0f ;
									data[d*16+10] = (float)rnd.NextDouble()*4000000.0f - 2000000.0f ;
									data[d*16+11] = (float)rnd.NextDouble()*4000000.0f - 2000000.0f ;
									data[d*16+12] = (float)rnd.NextDouble()*4000000.0f - 2000000.0f ;
									data[d*16+13] = (float)rnd.NextDouble()*4000000.0f - 2000000.0f ;
									data[d*16+14] = (float)rnd.NextDouble()*4000000.0f - 2000000.0f ;
									data[d*16+15] = (float)rnd.NextDouble()*4000000.0f - 2000000.0f ;
                                }
                                handle.asFloatMatrix4x4 = data;
								break;
							}
						case Member.eDataType.kInt8:
							{
                                sbyte[] data = new sbyte[handle.dataLength];
						        for (d = 0; d < handle.dataLength; ++d)
						        {
                                    data[d] = (sbyte)rnd.Next(SByte.MinValue, SByte.MaxValue+1);
                                }
                                handle.asInt8Array = data;
								break;
							}
						case Member.eDataType.kInt16:
							{
								short[] data = new short[handle.dataLength];
						        for (d = 0; d < handle.dataLength; ++d)
						        {
                                    data[d] = (short)rnd.Next(Int16.MinValue, Int16.MaxValue+1);
                                }
                                handle.asInt16Array = data;
								break;
							}
						case Member.eDataType.kInt32:
							{
								int[] data = new int[handle.dataLength];
						        for (d = 0; d < handle.dataLength; ++d)
						        {
                                    // rnd.Next returns a number between [arg1,arg2[
                                    // but unfortunately I can't pass Int32.MaxValue+1 here....
                                    data[d] = rnd.Next(Int32.MinValue, Int32.MaxValue);
                                }
                                handle.asInt32Array = data;
								break;
							}
						case Member.eDataType.kInt64:
							{
								long[] data = new long[handle.dataLength];
						        for (d = 0; d < handle.dataLength; ++d)
						        {
                                    // rnd.Next() gives a number between [0,Int32
                                    data[d] = (long)rnd.Next(Int32.MinValue, Int32.MaxValue);
                                    if( data[d] >= 0 )
                                        data[d] *= Int64.MaxValue / Int32.MaxValue;
                                    else
                                        data[d] *= Int64.MinValue / Int32.MinValue;
                                }
                                handle.asInt64Array = data;
								break;
							}
						case Member.eDataType.kUInt8:
							{
								byte[] data = new byte[handle.dataLength];
						        for (d = 0; d < handle.dataLength; ++d)
						        {
                                    data[d] = (byte)rnd.Next(0, Byte.MaxValue + 1);
                                }
                                handle.asUInt8Array = data;
								break;
							}
						case Member.eDataType.kUInt16:
							{
								ushort[] data = new ushort[handle.dataLength];
						        for (d = 0; d < handle.dataLength; ++d)
						        {
                                    data[d] = (ushort)rnd.Next(0, UInt16.MaxValue + 1);
                                }
                                handle.asUInt16Array = data;
								break;
							}
						case Member.eDataType.kUInt32:
							{
								uint[] data = new uint[handle.dataLength];
						        for (d = 0; d < handle.dataLength; ++d)
						        {
                                    data[d] = (uint)rnd.Next();
                                }
                                handle.asUInt32Array = data;
								break;
							}
						case Member.eDataType.kUInt64:
							{
								ulong[] data = new ulong[handle.dataLength];
						        for (d = 0; d < handle.dataLength; ++d)
						        {
    								data[d] = ((ulong)rnd.Next()) * UInt64.MaxValue / UInt32.MaxValue;
                                }
                                handle.asUInt64Array = data;
								break;
							}
						case Member.eDataType.kString:
							{
                                string[] randomStrings = new string[] { "banana", "tomatoe", "apple", "pineapple", "apricot", "pepper", "olive", "grapefruit" };
                                string[] data = new string[handle.dataLength];
						        for (d = 0; d < handle.dataLength; ++d)
						        {
                                    int index = rnd.Next( randomStrings.Length );
    								data[d] = randomStrings[index];
                                }
                                handle.asStringArray = data;
								break;
							}
						default:
							{
								Debug.Assert(false, "This should never happen");
								break;
							}
						}
					}
					newStream.setElement(new Index(m), handle);
				}
				newChannel.setDataStream(newStream);
				newMetadata.setChannel(newChannel);

                // Note: the following will not work if "obj" is a shape constructed by a source object
                // You need to delete the history of the shape before calling this...
                fDGModifier.setMetadata(obj, newMetadata);
                fDGModifier.doIt();

				// Set the result to the number of actual metadata values set as a
				// triple value:
				//	 	(# nodes, # metadata elements, # members per element)
				//
				MIntArray theResult = new MIntArray();
				theResult.append( (int) fNodes.length );
				theResult.append( (int) indexCount );
				theResult.append( (int) structureMemberCount );
				setResult( theResult );

			}
		}
	}
}
