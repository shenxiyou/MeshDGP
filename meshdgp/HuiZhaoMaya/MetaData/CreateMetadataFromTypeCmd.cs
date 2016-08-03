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
using System.Collections.Generic;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.MetaData;

[assembly: MPxCommandClass(typeof(MayaNetPlugin.CreateMetadataFromDotNetTypeCmd), "createMetadataFromDotNetType")]
[assembly: StructureClass(typeof(MayaNetPlugin.MyStructureClass))]

namespace MayaNetPlugin
{
    public class MyStructureClass
    {
        public int a;
        public float b;
        public string c;
        public bool d;
        public int e { get; set; }
        private int _f;
        public int f { get { return _f; } set { _f = value; } }
        public float[] xyz = new float[3];
        public string[] abc = { "Brutus", "baseball" };
    }

    [MPxCommandSyntaxSelection(ObjectType = typeof(MSelectionList), UseSelectionAsDefault = true)]
    public class CreateMetadataFromDotNetTypeCmd : MPxCommand, IMPxCommand
	{ 
        private MObject fObj = null; // the MObject of the mesh itself
        private MObject fObjTransform = null; // the MObject of the selected object
        private MFnMesh fMesh = null; // the mesh
		private MDGModifier fDGModifier = new MDGModifier();// Information needed for undo/redo
        private static readonly string fStreamName = "TestStructureFromDotNetType";

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
                    fMesh = new MFnMesh(obj);
                    fObj = obj;
                    fObjTransform = dagPath.node;
                }
			}

			if( fMesh == null || fObj == null || fObjTransform == null )
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

            MColorArray myColorArray = new MColorArray();
			MFnDependencyNode node = new MFnDependencyNode(fObj);

			// Get the current metadata (empty if none yet)
			Associations newMetadata = new Associations(node.metadata);
			Channel newChannel = newMetadata.channel( "vertex" );

			// Check to see if the requested stream name already exists
			Stream oldStream = newChannel.dataStream(fStreamName);
			if (oldStream != null)
			{

				string fmt = MStringResource.getString(MetaDataRegisterMStringResources.kCreateMetadataHasStream);
				string msg = String.Format(fmt, fStreamName);
				MGlobal.displayError( msg );
				return;
			}

            StreamForType<MyStructureClass> newStream = new StreamForType<MyStructureClass>(fStreamName);

            int indexCount = fMesh.numVertices;

            Random rndIndexCount = new Random();

			// Fill specified stream ranges with random data
			int m;
            Random rnd = new Random();
            for (m = 0; m < indexCount; ++m)
			{
				// Walk each structure member and fill with random data
				// tailored to the member data type.

                MyStructureClass myClass = new MyStructureClass();

                // int
                myClass.a = rnd.Next(Int32.MinValue, Int32.MaxValue);
                // float
                myClass.b = (float)rnd.NextDouble();
                // string
                string[] randomStrings = new string[] { "banana", "tomatoe", "apple", "pineapple", "apricot", "pepper", "olive", "grapefruit" };
                int index = rnd.Next( randomStrings.Length );
    			myClass.c = randomStrings[index];
                // bool
				int randomInt = rnd.Next(0, 2);
                myClass.d = randomInt == 1 ? true : false;

                myClass.e = rnd.Next(Int32.MinValue, Int32.MaxValue);

                myClass.f = rnd.Next(Int32.MinValue, Int32.MaxValue);

                myClass.xyz[0] = (float)rnd.NextDouble();
                myClass.xyz[1] = (float)rnd.NextDouble();
                myClass.xyz[2] = (float)rnd.NextDouble();

				newStream[m] = myClass;						
			}

			newChannel.setDataStream(newStream);
			newMetadata.setChannel(newChannel);

            // Note: the following will not work if "obj" is a shape constructed by a source object
            // You need to delete the history of the shape before calling this...
            fDGModifier.setMetadata(fObj, newMetadata);
            fDGModifier.doIt();

            Associations meshMetadata = fMesh.metadata;

            // This code is for debugging only
            {
                Channel chn = meshMetadata["vertex"];

                Console.WriteLine("Channel : type = {0}, nbStreams = {1}", chn.nameProperty, chn.dataStreamCount);

                Stream chnStream = chn[fStreamName];

                Structure strct = chnStream.structure;

                var strm = new StreamForType<MyStructureClass>(chnStream);

                Console.WriteLine("Stream : name = {0}, nbElements = {1}", chnStream.name, chnStream.Count );

                var tomatoes = strm.Where( ( KeyValuePair<Index,MyStructureClass> keyvalue ) => keyvalue.Value.c == "tomatoe");

                foreach (var keyvalue in tomatoes)
//                        foreach (MyStructureClass myClass in strm.Values )
                {
                    Console.WriteLine("Vertex #{0}, a = {1}", keyvalue.Key.asString, keyvalue.Value.a.ToString());
                    Console.WriteLine("Vertex #{0}, b = {1}", keyvalue.Key.asString, keyvalue.Value.b.ToString());
                    Console.WriteLine("Vertex #{0}, c = {1}", keyvalue.Key.asString, keyvalue.Value.c.ToString());
                    Console.WriteLine("Vertex #{0}, d = {1}", keyvalue.Key.asString, keyvalue.Value.d.ToString());
                    Console.WriteLine("Vertex #{0}, e = {1}", keyvalue.Key.asString, keyvalue.Value.e.ToString());
                    Console.WriteLine("Vertex #{0}, f = {1}", keyvalue.Key.asString, keyvalue.Value.f.ToString());
                    Console.WriteLine("Vertex #{0}, xyz = {1},{2},{3}", keyvalue.Key.asString, keyvalue.Value.xyz[0].ToString(), keyvalue.Value.xyz[1].ToString(), keyvalue.Value.xyz[2].ToString());
                    Console.WriteLine("Vertex #{0}, abc = {1},{2}", keyvalue.Key.asString, keyvalue.Value.abc[0], keyvalue.Value.abc[1] );
                }
            }
		}
	}
}
