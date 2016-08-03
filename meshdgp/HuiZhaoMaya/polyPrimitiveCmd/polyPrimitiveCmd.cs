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
using Autodesk.Maya.OpenMayaUI;


[assembly: MPxCommandClass(typeof(MayaNetPlugin.polyPrimitive), "polyPrimitiveCSharp")]
[assembly: MPxUIClass("polyPrimitiveCreateUI", "polyPrimitiveDeleteUI", "", "")]

namespace MayaNetPlugin
{
	public class polyPrimitive : MPxCommand , IUndoMPxCommand
	{
		public const double M_PI = 3.14159265358979323846;
		public static int[] tetra_gons = new int[12]{
			1,2,3,
			2,4,3,
			1,3,4,
			1,4,2
		};
		public static int[] cube_gons = new int[24]{
			1,4,3,2,
			8,5,6,7,
			3,7,6,2,
			1,5,8,4,
			3,4,8,7,
			2,6,5,1
		};
		public static int[] octa_gons = new int[24]{
			2,3,1,
			3,4,1,
			4,5,1,
			1,5,2,
			6,3,2,
			6,4,3,
			6,5,4,
			6,2,5
		};
		public static int[] dodeca_gons = new int[60]{
			1,5,3,4,2,
			8,6,7,1,2,
			1,7,17,20,5,
			6,11,12,17,7,
			17,12,9,16,20,
			5,20,16,14,3,
			3,14,15,19,4,
			2,4,19,18,8,
			8,18,13,11,6,
			9,12,11,13,10,
			10,13,18,19,15,
			10,15,14,16,9
		};
		public static int[] icosa_gons = new int[60]{
			2,10,1,
			1,11,2,
			1,8,7,
			1,7,11,
			1,10,8,
			5,2,6,
			10,2,5,
			2,11,6,
			4,9,3,
			3,12,4,
			5,6,3,
			3,9,5,
			6,12,3,
			7,8,4,
			4,12,7,
			4,8,9,
			5,9,10,
			6,11,12,
			7,12,11,
			8,10,9
		};
		public static double[,] trunc_icosa_vtxArray = new double[60,3]{
			{ 0.00000000, 0.00000000, 1.00000000 },
			{ 0.39524780, 0.00000000, 0.91857395 },
			{ -0.22278550, 0.32647698, 0.91857395 },
			{ -0.14409696, -0.36804505, 0.91857395 },
			{ 0.56771009, 0.32647698, 0.75572282 },
			{ 0.49542801, -0.36804505, 0.78682468 },
			{ -0.58966797, 0.28490989, 0.75572282 },
			{ -0.05032380, 0.65295495, 0.75572282 },
			{ 0.16209500, -0.59550930, 0.78682468 },
			{ -0.51097943, -0.40961312, 0.75572282 },
			{ 0.84035260, 0.28490989, 0.46112243 },
			{ 0.34492458, 0.65295495, 0.67429775 },
			{ 0.76807052, -0.40961312, 0.49222429 },
			{ -0.64395201, 0.58569638, 0.49222429 },
			{ -0.73376494, -0.08313546, 0.67429775 },
			{ -0.31061900, 0.81316063, 0.49222429 },
			{ 0.10140353, -0.86454163, 0.49222429 },
			{ -0.57166993, -0.67864447, 0.46112243 },
			{ 0.78606856, 0.58569638, 0.19762390 },
			{ 0.94053281, -0.08313546, 0.32937316 },
			{ 0.47987757, 0.81316063, 0.32937316 },
			{ 0.70737904, -0.67864447, 0.19762390 },
			{ -0.84233203, 0.51843879, 0.14730069 },
			{ -0.93214496, -0.15039373, 0.32937316 },
			{ -0.17566601, 0.97336729, 0.14730069 },
			{ 0.37404603, -0.90610970, 0.19762390 },
			{ -0.26547894, -0.90610970, 0.32937316 },
			{ -0.83196474, -0.51843879, 0.19762390 },
			{ 0.83196474, 0.51843879, -0.19762390 },
			{ 0.98642507, -0.15039373, -0.06587463 },
			{ 0.21958178, 0.97336729, 0.06587463 },
			{ 0.84233203, -0.51843879, -0.14730069 },
			{ -0.70737904, 0.67864447, -0.19762390 },
			{ -0.98642507, 0.15039373, 0.06587463 },
			{ -0.37404603, 0.90610970, -0.19762390 },
			{ 0.17566601, -0.97336729, -0.14730069 },
			{ -0.21958178, -0.97336729, -0.06587463 },
			{ -0.78606856, -0.58569638, -0.19762390 },
			{ 0.57166993, 0.67864447, -0.46112243 },
			{ 0.93214496, 0.15039373, -0.32937316 },
			{ 0.26547894, 0.90610970, -0.32937316 },
			{ 0.64395201, -0.58569638, -0.49222429 },
			{ -0.76807052, 0.40961312, -0.49222429 },
			{ -0.94053281, 0.08313546, -0.32937316 },
			{ -0.10140353, 0.86454163, -0.49222429 },
			{ 0.31061900, -0.81316063, -0.49222429 },
			{ -0.47987757, -0.81316063, -0.32937316 },
			{ -0.84035260, -0.28490989, -0.46112243 },
			{ 0.51097943, 0.40961312, -0.75572282 },
			{ 0.73376494, 0.08313546, -0.67429775 },
			{ 0.58966797, -0.28490989, -0.75572282 },
			{ -0.49542801, 0.36804505, -0.78682468 },
			{ -0.16209500, 0.59550930, -0.78682468 },
			{ 0.05032380, -0.65295495, -0.75572282 },
			{ -0.34492458, -0.65295495, -0.67429775 },
			{ -0.56771009, -0.32647698, -0.75572282 },
			{ 0.14409696, 0.36804505, -0.91857395 },
			{ 0.22278550, -0.32647698, -0.91857395 },
			{ -0.39524780, 0.00000000, -0.91857395 },
			{ 0.00000000, 0.00000000, -1.00000000 }
		};
		public static int[] trunc_icosa_faceCountArray = new int[32]
		{
			6, 6, 5, 6, 5,
			6, 5, 6, 6, 6,
			5, 6, 5, 6, 5,
			6, 6, 6, 5, 6,
			5, 5, 6, 6, 6,
			5, 6, 5, 6, 6,
			5, 6 
		};
		public static int[] trunc_icosa_faceConnectArray = new int[180]
		{
			0,1,4,11,7,2,
			0,2,6,14,9,3,
			3,8,5,1,0,
			5,12,19,10,4,1,
			7,15,13,6,2,
			9,17,26,16,8,3,
			4,10,18,20,11,
			5,8,16,25,21,12,
			6,13,22,33,23,14,
			7,11,20,30,24,15,
			9,14,23,27,17,
			19,29,39,28,18,10,
			21,31,29,19,12,
			13,15,24,34,32,22,
			26,36,35,25,16,
			27,37,46,36,26,17,
			28,38,40,30,20,18,
			21,25,35,45,41,31,
			22,32,42,43,33,
			33,43,47,37,27,23,
			24,30,40,44,34,
			39,49,48,38,28,
			29,31,41,50,49,39,
			32,34,44,52,51,42,
			35,36,46,54,53,45,
			47,55,54,46,37,
			48,56,52,44,40,38,
			41,45,53,57,50,
			51,58,55,47,43,42,
			48,49,50,57,59,56,
			51,52,56,59,58,
			53,54,55,58,59,57
		};
		private int shapeFlag;
		private int num_verts;            // Number of vertices of polygon
		private int num_faces;            // Number of faces on polygon
		private int num_edges;            // Number of edges on polygon
		private int edges_per_face;        // Number of edges (or verticies) per face
		private int num_face_connects;    // Number of elements in face connect array
		private int[] p_gons;              // Pointer to static array of face connects
		private MFloatPointArray iarr;
		private MFloatPointArray pa;
		private MIntArray faceCounts;
		private MIntArray faceConnects;

		private MDagModifier dagMod;

		public polyPrimitive()
		{
			iarr = new MFloatPointArray();
			pa = new MFloatPointArray();
			faceCounts = new MIntArray();
			faceConnects = new MIntArray();
			dagMod = new MDagModifier();
			MGlobal.displayInfo("test polyPrimitive.");
		}

		private void FILL( double x, double y, double z )
		{
			MFloatPoint pnt = new MFloatPoint( (float)x, (float)y, (float)z );
			iarr.append( pnt );
		}

		private void assignShadingGroup(MObject transform, string groupName)
		{
			// Get the name of the mesh node.
			//
			// We need to use an MFnDagNode rather than an MFnMesh because the mesh
			// is not fully realized at this point and would be rejected by MFnMesh.
			MFnDagNode dagFn = new MFnDagNode(transform);
			dagFn.setObject(dagFn.child(0));

			string meshName = dagFn.name;

			// Use the DAG modifier to put the mesh into a shading group
			string cmd = "sets -e -fe ";
			cmd += groupName + " " + meshName;
			dagMod.commandToExecute(cmd);

			// Use the DAG modifier to select the new mesh.
			cmd = "select " + meshName;
			dagMod.commandToExecute(cmd);
		}

		private void create_icosa_points()
		{
			double a = Math.Sqrt((1.0 - Math.Sqrt(.2)) / 2.0);
			double b = Math.Sqrt((1.0 + Math.Sqrt(.2)) / 2.0);
			double z = 0.0;

			FILL(b, a, z); FILL(b, -a, z); FILL(-b, -a, z); FILL(-b, a, z);
			FILL(0, -b, -a); FILL(0, -b, a); FILL(0, b, a); FILL(0, b, -a);
			FILL(-a, 0, -b); FILL(a, 0, -b); FILL(a, 0, b); FILL(-a, 0, b);
		}

		private void create_dodecahedron()
		{
			// Generated from icosahedron points.
			create_icosa_points();
			MFloatPoint[] my_info = new MFloatPoint[12];
			int idx;
			for( idx = 0; idx < 12; idx++ ) {
				my_info[idx] = new MFloatPoint();
				my_info[idx].x = iarr[idx].x;
				my_info[idx].y = iarr[idx].y;
				my_info[idx].z = iarr[idx].z;
			}

			iarr.clear();

			// now generate the dodecahedron points:
			double x1,y1,z1,x2,y2,z2,x3,y3,z3;
			double xf, yf, zf;
			double len;

			for( idx = 0; idx < 20; idx++ ) {
				x1 = my_info[ icosa_gons[3*idx]-1 ].x;
				y1 = my_info[ icosa_gons[3*idx]-1 ].y;
				z1 = my_info[ icosa_gons[3*idx]-1 ].z;

				x2 = my_info[ icosa_gons[3*idx + 1]-1 ].x;
				y2 = my_info[ icosa_gons[3*idx + 1]-1 ].y;
				z2 = my_info[ icosa_gons[3*idx + 1]-1 ].z;

				x3 = my_info[ icosa_gons[3*idx + 2]-1 ].x;
				y3 = my_info[ icosa_gons[3*idx + 2]-1 ].y;
				z3 = my_info[ icosa_gons[3*idx + 2]-1 ].z;

				// the docecahedron vertex is the average of these points.
				xf = (x1+x2+x3)/3.0;
				yf = (y1+y2+y3)/3.0;
				zf = (z1+z2+z3)/3.0;

				// One more transformation: scale this point so it lies on the
				// unit sphere...
				len = Math.Sqrt( xf*xf + yf*yf + zf*zf );
				xf /= len; yf /= len; zf /= len;

				FILL( xf, yf, zf );
			}
		}

		private void create_tetrahedron( )
		{
			// First, create the points:
			double sq = Math.Sqrt(3.0);
			FILL(0.0,0.0,1.0); FILL(sq/2.0,0.0,-.5);
			FILL(-sq/4.0,.75,-.5); FILL(-sq/4.0,-.75,-.5);
		}

		private void create_cube( )
		{
			// First, create the points:
			double a = Math.Sqrt( 1.0/3.0 );

			FILL(a,a,a); FILL(a,-a,a); FILL(-a,-a,a); FILL(-a,a,a);
			FILL(a,a,-a); FILL(a,-a,-a); FILL(-a,-a,-a); FILL(-a,a,-a);
		}

		private void create_octahedron( )
		{
			FILL(0.0,0.0,1.0); FILL(1.0,0.0,0.0);
			FILL(0.0,1.0,0.0); FILL(-1.0,0.0,0.0);
			FILL(0.0,-1.0,0.0); FILL(0.0,0.0,-1.0);
		}

		private void create_truncated_icosahedron( )
		{
			num_verts = 60;
			int idx;
			for ( idx = 0; idx < num_verts; idx++ )
			{
				FILL(trunc_icosa_vtxArray[idx,0], 
					 trunc_icosa_vtxArray[idx,1],
					 trunc_icosa_vtxArray[idx,2]);
			}

			num_faces = 32;
			for ( idx = 0; idx < num_faces; idx++ )
			{
				faceCounts.append( trunc_icosa_faceCountArray[idx] );
			}

			for ( idx = 0; idx < 180; idx++ )
			{
				faceConnects.append( trunc_icosa_faceConnectArray[idx] );
			}
		}

		private void createPlane( )
		{
			int w = 2;
			int h = 2;
			double size = 2.0;
			double hSize, wSize;

			// Initialize class data
			//
			num_verts      = 0;
			num_faces      = 0;
			edges_per_face = 4;

			if ( w < 1 ) w = 1;
			if ( h < 1 ) h = 1;
			if ( size < 0.0001 ) size = 1.0;

			wSize = size / w;
			hSize = size / h;

			// create vertices
			//
			double x, z;
			for ( z = -size/2.0; z <= size / 2.0; z += hSize )
			{
				for ( x = -size/2.0; x <= size/2.0; x += wSize )
				{
					FILL( x, 0, z );
					num_verts++;
				}
			}

			// create polys
			//
			int v0, v1, v2, v3;
			for ( int i = 0; i < h; i++ )
			{
				for ( int j = 0; j < w; j++ )
				{
					v0 = j + (w+1) * i;
					v1 = j + 1 + (w+1) * i;
					v2 = j + 1 + (w+1) * (i+1);
					v3 = j + (w+1) * (i+1);

					faceConnects.append( v0 );
					faceConnects.append( v3 );
					faceConnects.append( v2 );
					faceConnects.append( v1 );
					num_faces++;
					faceCounts.append( edges_per_face );
				}
			}

			num_face_connects = num_faces * edges_per_face;
			num_edges = num_face_connects / 2;
		}

		private void createCylinder()
		{
			double r        = 1.0;
			double height   = 2.0;
			int sides       = 8;
			int sections    = 2;

			// Initialize class data
			//
			num_verts       = 0;
			num_faces       = 0;
			edges_per_face  = 0;


			if ( sides < 3 ) sides = 3;
			if ( sections < 1 ) sections = 1;
			if ( height <= 0 ) height = 1.0;
			if ( r <= 0 ) r = 1.0;

			// create verts
			//
			double angle, deg = 360.0 / (double) sides;
			double hSize = height / (double) sections;
			double x, z, y = height / 2.0;
			int i,j;

			for ( i = 0; i <= sections; i++ )
			{
				for ( j = sides - 1; j >= 0; j-- )
				{
					angle = deg * j / 180.0 * M_PI;
					x = Math.Cos( angle );
					z = Math.Sin( angle );
					FILL( x, y, z );
					num_verts++;
				} // for j
		
				y -= hSize;
			} // for i
	

			// create polys
			//
			for ( i = 0; i < sides; i++ )
			{
				faceConnects.append( i );
				edges_per_face++;
			}
			num_faces++;
			faceCounts.append( edges_per_face );
			edges_per_face = 0;
		
			for ( i = sides-1; i >= 0; i-- )
			{
				faceConnects.append( i + sides * sections );
				edges_per_face++;
			}
			num_faces++;
			faceCounts.append( edges_per_face );
			edges_per_face = 0;

			int v0, v1, v2, v3;
			for ( i = 0; i < sections; i++ )
			{
				for ( j = 0; j < sides; j++ )
				{
					if ( j == 0 )    // use last vtx on this section
					{
						v0 = sides - 1 + sides * i;
						v3 = sides - 1 + sides * (i+1);
					}
					else            // use prev vtx
					{
						v0 = j - 1 + sides * i;
						v3 = j - 1 + sides * (i+1);
					}
					v1 = j + sides * i;
					v2 = j + sides * (i+1);
			
					faceConnects.append( v0 );
					faceConnects.append( v3 );
					faceConnects.append( v2 );
					faceConnects.append( v1 );
					num_faces++;
					faceCounts.append( 4 );
				} // for j
			} // for i

			num_face_connects = (int)faceConnects.length;
			num_edges = num_face_connects/2;
		}

		private void generatePrimitiveData()
		{
			// Decide which type of primitive to create
			//
			iarr.clear();
			faceCounts.clear();
			faceConnects.clear();

			switch( shapeFlag ) {
				case 1:
				default:
					create_icosa_points();
					num_verts         = 12;
					num_faces         = 20;
					edges_per_face    =  3;
					p_gons = icosa_gons;
					break;
				case 2:
					create_dodecahedron();
					num_verts         = 20;
					num_faces         = 12;
					edges_per_face    =  5;
					p_gons = dodeca_gons;
					break;
				case 3:
					create_tetrahedron();
					num_verts         = 4;
					num_faces         = 4;
					edges_per_face    = 3;
					p_gons = tetra_gons;
					break;
				case 4:
					create_cube();
					num_verts         = 8;
					num_faces         = 6;
					edges_per_face    = 4;
					p_gons = cube_gons;
					break;
				case 5:
					create_octahedron();
					num_verts         = 6;
					num_faces         = 8;
					edges_per_face    = 3;
					p_gons = octa_gons;
					break;
				case 6:
					createPlane();
					p_gons = null;
					break;
				case 7:
					createCylinder();
					p_gons = null;
					break;
				case 8:
					create_truncated_icosahedron();
					p_gons = null;
					break;
			}
	
			// Construct the point array
			//
			pa.clear();
			int i;
			for ( i=0; i<num_verts; i++ )
				pa.append( iarr[i]);

			// If we are using polygon data then set up the face connect array
			// here. Otherwise, the create function will do it.
			//
			if (null != p_gons)
			{
				num_face_connects = num_faces * edges_per_face;
				num_edges = num_face_connects/2;

				for ( i=0; i<num_faces; i++ )
					faceCounts.append( edges_per_face );

				for ( i=0; i<(num_faces*edges_per_face); i++ )
					faceConnects.append( p_gons[i]-1 );
			}
		}

		private void createNodes()
		{
			// Generate the raw data for the requested primitive.
			generatePrimitiveData();

			// Create a mesh data wrapper to hold the new geometry.
			MFnMeshData    dataFn = new MFnMeshData();
			MObject        dataWrapper = dataFn.create();

			// Create the mesh geometry and put it into the wrapper.
			MFnMesh        meshFn = new MFnMesh();
			MObject        dataObj = meshFn.create(
										num_verts,
										num_faces,
										pa,
										faceCounts,
										faceConnects,
										dataWrapper
									);

			// Use the DAG modifier to create an empty mesh node and its parent
			// transform.
			MObject transform = dagMod.createNode("mesh", MObject.kNullObj);

			// Commit the creation so that the transform and its child will be
			// valid below.
			dagMod.doIt();

			// At the moment we have a transform named something like 'transform1'
			// and a mesh named something like 'polySurfaceShape1'. Let's tidy that
			// up by renaming them as 'pPrimitive#' and 'pPrimitiveShape#', where
			// '#' is a number to ensure uniqueness.
			renameNodes(transform, "pPrimitive");
			// Commit the rename so that assignShadingGroup() can get the new name.
			dagMod.doIt();

			// Assign the mesh to a shading group.
			assignShadingGroup(transform, "initialShadingGroup");

			// Commit the changes.
			dagMod.doIt();

			// Set the mesh node to use the geometry we created for it.
			setMeshData(transform, dataWrapper);
		}

		private void renameNodes(MObject transform, string baseName)
		{
			//  Rename the transform to something we know no node will be using.
			dagMod.renameNode(transform, "polyPrimitiveCmdTemp");
			
			//  Rename the mesh to the same thing but with 'Shape' on the end.
			MFnDagNode dagFn = new MFnDagNode(transform);

			dagMod.renameNode(dagFn.child(0), "polyPrimitiveCmdTempShape");

			//  Now that they are in the 'something/somethingShape' format, any
			//  changes we make to the name of the transform will automatically be
			//  propagated to the shape as well.
			//
			//  Maya will replace the '#' in the string below with a number which
			//  ensures uniqueness.
			string transformName = baseName + "#";
			dagMod.renameNode(transform, transformName);
		}

		private void setMeshData(MObject transform, MObject dataWrapper)
		{
			// Get the mesh node.
			MFnDagNode  dagFn = new MFnDagNode(transform);
			MObject     mesh = dagFn.child(0);

			// The mesh node has two geometry inputs: 'inMesh' and 'cachedInMesh'.
			// 'inMesh' is only used when it has an incoming connection, otherwise
			// 'cachedInMesh' is used. Unfortunately, the docs say that 'cachedInMesh'
			// is for internal use only and that changing it may render Maya
			// unstable.
			//
			// To get around that, we do the little dance below...

			// Use a temporary MDagModifier to create a temporary mesh attribute on
			// the node.
			MFnTypedAttribute  tAttr = new MFnTypedAttribute();
			MObject tempAttr = tAttr.create("tempMesh", "tmpm", MFnData.Type.kMesh);
			MDagModifier tempMod = new MDagModifier();

			tempMod.addAttribute(mesh, tempAttr);

			tempMod.doIt();

			// Set the geometry data onto the temp attribute.
			dagFn.setObject(mesh);

			MPlug  tempPlug = dagFn.findPlug(tempAttr);

			tempPlug.setValue(dataWrapper);

			// Use the temporary MDagModifier to connect the temp attribute to the
			// node's 'inMesh'.
			MPlug  inMeshPlug = dagFn.findPlug("inMesh");
	
			tempMod.connect(tempPlug, inMeshPlug);

			tempMod.doIt();

			// Force the mesh to update by grabbing its output geometry.
			dagFn.findPlug("outMesh").asMObject();

			// Undo the temporary modifier.
			tempMod.undoIt();
		}

		public override void doIt(MArgList args)
		{
			// Command line argument specifies type of primitive to create
			//
			shapeFlag = 1;
			if (args.length > 0)
				shapeFlag = args.asInt(0);

			// Create the mesh and its transform.
			try
			{
				createNodes();
			}
			catch (System.Exception)
			{
				// If the creation failed, clean up any partial changes made.
				dagMod.undoIt();
				throw;//Rethrow the same exception
			}

			return;
		}

		public override void redoIt()
		{
			dagMod.doIt();
			return;
		}

		public override void undoIt()
		{
			dagMod.undoIt();
			return;
		}

		public bool checkErr(bool stat, string msg)
		{
			if (!stat)
				displayError(msg);
			return stat;
		}
	}
}
