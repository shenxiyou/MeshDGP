// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


//
// Description:
//   This is an example of a command that exports smooth skin data from
//   maya to an alternate format.
//
//   To use, type the command:
//
//      exportSkinClusterDataCSharp -f <fileName>
//
//   For example:
//
//      exportSkinClusterDataCSharp -f "C:/temp/skinData"
//
//   The output format used is:
// 
//	   skin_path_name vertex_count influence_count
//     influence_1 influence_2 influence_3 .... influence_n
//     vertex_index weight_1 weight_2 weight_3 ... weight_n
//
//   Notes:
//   For each vertex index, a weight value is written out corresponding
//   to each of the n influence objects, even if the weight is 0.
//
//   If all of the vertices in the skin are bound as skin, the vertex_count
//   will equal the total number of vertices in the skin object, and
//   the vertex_index values will be sequential.  
//   However, if only some of the vertices were bound as skin, the vertex_count
//   will only reflect the count of skin vertices, and the vertex_index
//   values will be non-sequential.
//
//   

using System;
using System.Linq;
using System.Text;
using System.IO;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaAnim;

[assembly: MPxCommandClass(typeof(MayaNetPlugin.exportSkinClusterDataCmd), "exportSkinClusterDataCSharp")]

namespace MayaNetPlugin
{
	class exportSkinClusterDataCmd : MPxCommand, IUndoMPxCommand
	{
		private FileStream file;

		public exportSkinClusterDataCmd() { file = null; }

		public void parseArgs(MArgList args)
		{
			string arg;
			string fileName		= "";
			string fileFlag		= "-f";
			string fileFlagLong = "-file";

			// Parse the arguments.
			for ( uint i = 0; i < args.length; i++ ) 
			{
				try
				{
					arg = args.asString(i);
				}
				catch (Exception e) 
				{
					MGlobal.displayInfo(e.Message);
					continue;
				}
				
				if ( arg == fileFlag || arg == fileFlagLong ) 
				{
					// get the file name
					//
					if (i == args.length - 1) 
					{
						arg += ": must specify a file name";
						throw new ArgumentException(arg, "args"); 
					}
					i++;
					fileName = args.asString(i);
				}
				else 
				{
					arg += ": unknown argument";
					throw new ArgumentException(arg, "args"); 
				}
			}

			file = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
	
			if (file == null)
			{
				string openError = "Could not open: ";
				openError += fileName;
				throw new InvalidOperationException(openError);
			}
		}

		#region override
		public override void doIt(MArgList args)
		{
			// parse args to get the file name from the command-line
			//
			parseArgs(args);
			uint count = 0;
	

			// Iterate through graph and search for skinCluster nodes
			//
			MItDependencyNodes iter = new MItDependencyNodes( MFn.Type.kInvalid);
			for ( ; !iter.isDone; iter.next() ) 
			{
				MObject obj = iter.item;
				if (obj.apiType == MFn.Type.kSkinClusterFilter) 
				{
					count++;
			
					// For each skinCluster node, get the list of influence objects
					//
					MFnSkinCluster skinCluster = new MFnSkinCluster(obj);
					MDagPathArray infs = new MDagPathArray();
					uint nInfs;
					try
					{
						nInfs = skinCluster.influenceObjects(infs);
					}
					catch (Exception)
					{
						MGlobal.displayInfo("Error getting influence objects.");
						continue;
					}
					if (0 == nInfs) 
					{
						MGlobal.displayInfo("Error: No influence objects found.");
						continue;
					}
			
					// loop through the geometries affected by this cluster
					//
					uint nGeoms = skinCluster.numOutputConnections;
					for (uint ii = 0; ii < nGeoms; ++ii) 
					{
						uint index;
						try
						{
							index = skinCluster.indexForOutputConnection(ii);
						}
						catch (Exception)
						{
							MGlobal.displayInfo("Error getting geometry index.");
							continue;
						}

						// get the dag path of the ii'th geometry
						//
						MDagPath skinPath = new MDagPath();
						try{
						skinCluster.getPathAtIndex(index,skinPath);
						}
						catch (Exception)
						{
							MGlobal.displayInfo("Error getting geometry path.");
							continue;
						}

						// iterate through the components of this geometry
						//
						MItGeometry gIter = new MItGeometry(skinPath);

						// print out the path name of the skin, vertexCount & influenceCount
						//
						UnicodeEncoding uniEncoding = new UnicodeEncoding();
						string res = String.Format("{0} {1} {2}\n",skinPath.partialPathName,gIter.count,nInfs);
						file.Write(uniEncoding.GetBytes(res),0,uniEncoding.GetByteCount(res));
				
						// print out the influence objects
						//
						for (int kk = 0; kk < nInfs; ++kk) 
						{
							res = String.Format("{0} ", infs[kk].partialPathName);
							file.Write(uniEncoding.GetBytes(res),0,uniEncoding.GetByteCount(res));
						}
						res = "\n";
						file.Write(uniEncoding.GetBytes(res), 0, uniEncoding.GetByteCount(res));
			
						for ( /* nothing */ ; !gIter.isDone; gIter.next() ) {
							MObject comp;
							try
							{
								comp = gIter.component;
							}
							catch (Exception)
							{
								MGlobal.displayInfo("Error getting geometry path.");
								continue;
							}
							
							
							// Get the weights for this vertex (one per influence object)
							//
							MDoubleArray wts = new MDoubleArray();
							uint infCount = 0;
							try
							{
								skinCluster.getWeights(skinPath, comp, wts, ref infCount);
							}
							catch (Exception)
							{
								displayError("Error getting weights.");
								continue;
							}
							if (0 == infCount) 
							{
								displayError("Error: 0 influence objects.");
							}

							// Output the weight data for this vertex
							//
							res = String.Format("{0} ",gIter.index);
							file.Write(uniEncoding.GetBytes(res), 0, uniEncoding.GetByteCount(res));
							for (int jj = 0; jj < infCount ; ++jj ) 
							{
								res = String.Format("{0} ", wts[jj]);
								file.Write(uniEncoding.GetBytes(res), 0, uniEncoding.GetByteCount(res));
							}
							file.Write(uniEncoding.GetBytes("\n"), 0, uniEncoding.GetByteCount("\n"));
						}
					}
				}
			}

			if (0 == count) 
			{
				displayError("No skinClusters found in this scene.");
			}
			file.Close();
			return;
		}

		public override void redoIt()
		{
			clearResult();
			setResult((int)1);
			return;
		}

		public override void undoIt()
		{
			return;
		}

		public override bool isUndoable()
		{
			return false;
		}
		#endregion override
	}
}
