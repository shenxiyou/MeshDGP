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
//		This C# plugin is ported from: $(MAYADIR)\devkit\plug-ins\scanDagCmd

// How to use:
//            Command : scanDagCSharp
//            TraversalType parameter : -b    breadthFirst
//                                      -d    depthFirst
//            Filter parameter: -c cameras 
//                              -l lights
//                              -n nurbsSurfaces
//                              -q  quiet
//

using System;

 
using Autodesk.Maya.OpenMaya;

[assembly: MPxCommandClass(typeof(MayaNetPlugin.scanDag), "scanDagCSharp")]

namespace MayaNetPlugin
{
	public class scanDag: MPxCommand, IMPxCommand
	{
		public override void doIt(MArgList args)
		{
			MItDag.TraversalType	traversalType = MItDag.TraversalType.kDepthFirst;
			MFn.Type				filter        = MFn.Type.kInvalid;
			bool					quiet = false;

			parseArgs(args, ref traversalType, filter, ref quiet);

			doScan(traversalType, filter, quiet);
		}

		private void parseArgs( MArgList args,ref MItDag.TraversalType traversalType,
			MFn.Type filter, ref bool quiet)
		{
			string arg = "";
			const string breadthFlag = "-b";
			const string breadthFlagLong = "-breadthFirst";
			const string depthFlag = "-d";
			const string depthFlagLong = "-depthFirst";
			const string cameraFlag = "-c";
			const string cameraFlagLong = "-cameras";
			const string lightFlag = "-l";
			const string lightFlagLong = "-lights";
			const string nurbsSurfaceFlag = "-n";
			const string nurbsSurfaceFlagLong = "-nurbsSurfaces";
			const string quietFlag = "-q";
			const string quietFlagLong = "-quiet";

			for (uint i = 0; i < args.length; i++)
			{
				arg = args.asString(i);
				if ( arg == breadthFlag || arg == breadthFlagLong )
					traversalType = MItDag.TraversalType.kBreadthFirst;
				else if ( arg == depthFlag || arg == depthFlagLong )
					traversalType = MItDag.TraversalType.kDepthFirst;
				else if ( arg == cameraFlag || arg == cameraFlagLong )
					filter = MFn.Type.kCamera;
				else if ( arg == lightFlag || arg == lightFlagLong )
					filter = MFn.Type.kLight;
				else if ( arg == nurbsSurfaceFlag || arg == nurbsSurfaceFlagLong )
					filter = MFn.Type.kNurbsSurface;
				else if ( arg == quietFlag || arg == quietFlagLong )
					quiet = true;
				else {
					arg += ": unknown argument";
					throw new ArgumentException(arg, "args");
				}
			}
		}

		private string MPointToString(MPoint point)
		{
            string str = string.Format("(%f, %f, %f)", point.x, point.y, point.z);
			return str;

		}
		private string MVectorToString(MVector vector)
		{
            string str = string.Format("(%f, %f, %f)", vector.x, vector.y, vector.z); 
			return str;
		}

		private void doScan(MItDag.TraversalType traversalType, MFn.Type filter, bool quiet)
		{
			MItDag dagIterator = new MItDag(traversalType, filter);

			//	Scan the entire DAG and output the name and depth of each node

			if (traversalType == MItDag.TraversalType.kBreadthFirst)
				if (!quiet)
					MGlobal.displayInfo(Environment.NewLine + "Starting Breadth First scan of the Dag");
			else
				if (!quiet)
					MGlobal.displayInfo(Environment.NewLine + "Starting Depth First scan of the Dag");

			switch (filter) 
			{
				case MFn.Type.kCamera:
					if (!quiet)
						MGlobal.displayInfo( ": Filtering for Cameras\n");
					break;
				case MFn.Type.kLight:
					if (!quiet)
						MGlobal.displayInfo(": Filtering for Lights\n");
					break;
				case MFn.Type.kNurbsSurface:
					if (!quiet)
						MGlobal.displayInfo(": Filtering for Nurbs Surfaces\n");
					break;
				default:
					MGlobal.displayInfo(Environment.NewLine);
					break;
			}


			int objectCount = 0;
			for ( ; !dagIterator.isDone; dagIterator.next() ) {

				MDagPath dagPath = new MDagPath();

				try
				{
					dagIterator.getPath(dagPath);
				}
				catch (System.Exception )
				{
					continue;
				}

				MFnDagNode dagNode = null;
				try
				{
					dagNode = new MFnDagNode(dagPath);
				}
				catch (System.Exception )
				{
					continue;
				}

				if (!quiet)
					MGlobal.displayInfo(dagNode.name + ": " + dagNode.typeName + Environment.NewLine);

				if (!quiet)
					MGlobal.displayInfo( "  dagPath: " + dagPath.fullPathName + Environment.NewLine);

				objectCount += 1;
				if (dagPath.hasFn(MFn.Type.kCamera)) 
				{
					MFnCamera camera  =  null;
					try
					{
						camera  = new MFnCamera(dagPath);
					}
					catch (System.Exception)
					{
						continue;
					}

					// Get the translation/rotation/scale data
					//
					printTransformData(dagPath, quiet);

					// Extract some interesting Camera data
					//
					if (!quiet)
					{
						
						MGlobal.displayInfo("  eyePoint: " + MPointToString(camera.eyePoint(MSpace.Space.kWorld)) + Environment.NewLine);

						MGlobal.displayInfo("  upDirection: " + MVectorToString( camera.upDirection(MSpace.Space.kWorld)) + Environment.NewLine);

						MGlobal.displayInfo("  viewDirection: " + MVectorToString( camera.viewDirection(MSpace.Space.kWorld)) + Environment.NewLine);
					   
						MGlobal.displayInfo("  aspectRatio: " + Convert.ToString( camera.aspectRatio ) + Environment.NewLine);
						
						MGlobal.displayInfo("  horizontalFilmAperture: " + Convert.ToString(camera.horizontalFilmAperture ) + Environment.NewLine);

						MGlobal.displayInfo("  verticalFilmAperture: " + Convert.ToString(camera.verticalFilmAperture ) + Environment.NewLine);
					}
				} 
				else if (dagPath.hasFn(MFn.Type.kLight)) 
				{
					MFnLight light = null;
					try
					{
						light = new MFnLight(dagPath);
					}
					catch (System.Exception)
					{
						continue;
					}
					
					// Get the translation/rotation/scale data
					//
					printTransformData(dagPath, quiet);

					// Extract some interesting Light data
					//
					MColor color = light.color;
					if (!quiet)
					{
                        MGlobal.displayInfo(string.Format("  color: [%f, %f, %f]\n", color.r, color.g, color.b));
					}
					color = light.shadowColor;
					if (!quiet)
					{

                        MGlobal.displayInfo(string.Format("  shadowColor: [%f, %f, %f]\n", color.r, color.g, color.b));
						MGlobal.displayInfo("  intensity: "+Convert.ToString(light.intensity) + Environment.NewLine);
						
					}
				} 
				else if (dagPath.hasFn(MFn.Type.kNurbsSurface))
				{
					MFnNurbsSurface surface= null;
					try
					{
						surface = new MFnNurbsSurface(dagPath);
					}
					catch (System.Exception )
					{
						continue;
					}

					// Get the translation/rotation/scale data
					//
					printTransformData(dagPath, quiet);

					// Extract some interesting Surface data
					//
					if (!quiet)
					{
                        MGlobal.displayInfo(string.Format("  numCVs: %d * %s", surface.numCVsInU, surface.numCVsInV) + Environment.NewLine);
                        MGlobal.displayInfo(string.Format("  numKnots: %d * %s\n", surface.numKnotsInU, surface.numKnotsInV) + Environment.NewLine);
                        MGlobal.displayInfo(string.Format("  numSpans: %d * %s\n", surface.numSpansInU, surface.numSpansInV) + Environment.NewLine);
					}
				} else 
				{
					// Get the translation/rotation/scale data
					//
					printTransformData(dagPath, quiet);
				}
			}

			setResult(objectCount);
		}


		void printTransformData(MDagPath dagPath, bool quiet)
		{

			MObject transformNode = null;
			try
			{
				transformNode = dagPath.transform;
			}
			catch (System.Exception)
			{
				// This node has no transform - i.e., it's the world node
				//
				return;
			}
 
			MFnDagNode	transform = null; 
			
			try
			{
				transform = new MFnDagNode(transformNode);
			}
			catch (System.Exception )
			{
				return;
			}

			MTransformationMatrix	matrix  = new MTransformationMatrix(transform.transformationMatrix);

			if (!quiet)
			{
				MGlobal.displayInfo("  translation: " +
					MVectorToString(matrix.translation(MSpace.Space.kWorld)) + Environment.NewLine);
			}

			double[] threeDoubles = new double[3];
			int rOrder = 0;
			matrix.getRotation (threeDoubles, out rOrder, MSpace.Space.kWorld);

			if (!quiet)
			{
				MGlobal.displayInfo(string.Format("  rotation: [%f, %f, %f]\n", threeDoubles[0], threeDoubles[1], threeDoubles[2]));
			}
			matrix.getScale (threeDoubles, MSpace.Space.kWorld);
			if (!quiet)
			{
                MGlobal.displayInfo(string.Format("  scale: [%f, %f, %f]\n", threeDoubles[0], threeDoubles[1], threeDoubles[2]));
			}
		}
	}
}
