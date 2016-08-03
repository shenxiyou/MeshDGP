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


[assembly: MPxNodeClass(typeof(MayaNetPlugin.apiMeshCreator), "apiMeshCreatorCSharp", 0x0008105b)]

namespace MayaNetPlugin
{
	class apiMeshCreator : MPxNode, IMPxNode
	{
		/* Attribute initialization. */
		public static MObject size = null;
		public static MObject shapeType = null;
		public static MObject inputMesh = null;
		public static MObject outputSurface = null;

        [MPxNodeInitializer()]
		public static void initialize()
		//
		// Description
		//
		//    Attribute (static) initialization.
		//
		{
			MFnNumericAttribute numAttr = new MFnNumericAttribute();
			MFnEnumAttribute enumAttr = new MFnEnumAttribute();
			MFnTypedAttribute typedAttr = new MFnTypedAttribute();

			// ----------------------- INPUTS -------------------------

			size = numAttr.create("size", "sz", MFnNumericData.Type.kDouble, 1.0);
			numAttr.isArray = false;
			numAttr.usesArrayDataBuilder = false;
			numAttr.isHidden = false;
			numAttr.isKeyable = true;
			addAttribute(size);

			shapeType = enumAttr.create("shapeType", "st", 0);
			enumAttr.addField("cube", 0);
			enumAttr.addField("sphere", 1);
			enumAttr.isHidden = false;
			enumAttr.isKeyable = true;
			addAttribute(shapeType);

			inputMesh = typedAttr.create("inputMesh", "im", MFnData.Type.kMesh);
			typedAttr.isHidden = true;
			addAttribute(inputMesh);

			// ----------------------- OUTPUTS -------------------------
			outputSurface = typedAttr.create("outputSurface", "os", new MTypeId(apiMeshData.id));
			typedAttr.isWritable = false;
			addAttribute(outputSurface);

			// ---------- Specify what inputs affect the outputs ----------
			//
			attributeAffects(inputMesh, outputSurface);
			attributeAffects(size, outputSurface);
			attributeAffects(shapeType, outputSurface);
		}

		public override bool compute(MPlug plug, MDataBlock datablock)
		//
		// Description
		//
		//    When input attributes are dirty this method will be called to
		//    recompute the output attributes.
		//
		{
			if (plug.attribute.equalEqual(outputSurface))
			{
				// Create some user-defined geometry data and access the
				// geometry so that we can set it
				//
				MFnPluginData fnDataCreator = new MFnPluginData();
				fnDataCreator.create(new MTypeId(apiMeshData.id));
				apiMeshData meshData = (apiMeshData)fnDataCreator.data();
				apiMeshGeom meshGeom = meshData.fGeometry;

				// If there is an input mesh then copy it's values
				// and construct some apiMeshGeom for it.
				//
				bool hasHistory = computeInputMesh(plug,
												   datablock,
												   meshGeom.vertices,
												   meshGeom.face_counts,
												   meshGeom.face_connects,
												   meshGeom.normals,
												   meshGeom.uvcoords);

				// There is no input mesh so check the shapeType attribute
				// and create either a cube or a sphere.
				//
				if ( !hasHistory )
				{
					MDataHandle sizeHandle = datablock.inputValue(size);
					double shape_size = sizeHandle.asDouble;
					MDataHandle typeHandle = datablock.inputValue(shapeType);
					short shape_type = typeHandle.asShort;

					switch( shape_type )
					{
						case 0 : // build a cube
							buildCube(shape_size,
									  meshGeom.vertices,
									  meshGeom.face_counts,
									  meshGeom.face_connects,
									  meshGeom.normals,
									  meshGeom.uvcoords
								);
							break;
			
						case 1 : // build a sphere
							buildSphere(shape_size,
										32,
										meshGeom.vertices,
										meshGeom.face_counts,
										meshGeom.face_connects,
										meshGeom.normals,
										meshGeom.uvcoords
								);
							break;
					} // end switch
				}

				meshGeom.faceCount = meshGeom.face_counts.length;

				// Assign the new data to the outputSurface handle
				//
				MDataHandle outHandle = datablock.outputValue(outputSurface);
				outHandle.set(meshData);

				datablock.setClean(plug);
                return true;
			}

			return false;
		}

		//
		// Description
		//
		//     This function takes an input surface of type kMeshData and converts
		//     the geometry into this nodes attributes.
		//     Returns false if nothing is connected.
		//
		public bool computeInputMesh(MPlug plug,
									 MDataBlock datablock,
									 MPointArray vertices,
									 MIntArray counts,
									 MIntArray connects,
									 MVectorArray normals,
									 apiMeshGeomUV uvs)
		{
			// Get the input subdiv
			//
			MDataHandle inputData = datablock.inputValue( inputMesh );
			MObject surf = inputData.asMesh;

			// Check if anything is connected
			//
			MObject thisObj = thisMObject();
			MPlug surfPlug = new MPlug( thisObj, inputMesh );
			if ( !surfPlug.isConnected )
			{
				datablock.setClean( plug );
				return false;
			}

			// Extract the mesh data
			//
			MFnMesh surfFn = new MFnMesh(surf);
			surfFn.getPoints( vertices, MSpace.Space.kObject );

			// Check to see if we have UVs to copy.
			//
			bool hasUVs = surfFn.numUVsProperty > 0;
			surfFn.getUVs( uvs.ucoord, uvs.vcoord );

			for ( int i=0; i<surfFn.numPolygons; i++ )
			{
				MIntArray polyVerts = new MIntArray();
				surfFn.getPolygonVertices( i, polyVerts );
				int pvc = (int)polyVerts.length;
				counts.append( pvc );
				int uvId;
				for ( int v=0; v<pvc; v++ )
				{
					if ( hasUVs )
					{
						surfFn.getPolygonUVid( i, v, out uvId );
						uvs.faceVertexIndex.append( uvId );
					}
					connects.append( polyVerts[v] );
				}
			}

			for ( int n=0; n<(int)vertices.length; n++ )
			{
				MVector normal = new MVector();
				surfFn.getVertexNormal( n, normal );
				normals.append( normal );
			}

			return true;
		}

		//
		// Description
		//
		//    Constructs a cube
		//
		public void buildCube(double cube_size, 
							  MPointArray pa,
							  MIntArray faceCounts, 
							  MIntArray faceConnects,
							  MVectorArray normals, 
							  apiMeshGeomUV uvs)
		{
			const int num_faces			= 6;
			const int num_face_connects	= 24;
			const double normal_value   = 0.5775;
			const int uv_count			= 14; 
	
			pa.clear();
			faceCounts.clear();
			faceConnects.clear();
			uvs.reset();

			pa.append( new MPoint( -cube_size, -cube_size, -cube_size ) );
			pa.append( new MPoint(  cube_size, -cube_size, -cube_size ) );
			pa.append( new MPoint(  cube_size, -cube_size,  cube_size ) );
			pa.append( new MPoint( -cube_size, -cube_size,  cube_size ) );
			pa.append( new MPoint( -cube_size,  cube_size, -cube_size ) );
			pa.append( new MPoint( -cube_size,  cube_size,  cube_size ) );
			pa.append( new MPoint(  cube_size,  cube_size,  cube_size ) );
			pa.append( new MPoint(  cube_size,  cube_size, -cube_size ) );

			normals.append( new MVector( -normal_value, -normal_value, -normal_value ) );
			normals.append( new MVector(  normal_value, -normal_value, -normal_value ) );
			normals.append( new MVector(  normal_value, -normal_value,  normal_value ) );
			normals.append( new MVector( -normal_value, -normal_value,  normal_value ) );
			normals.append( new MVector( -normal_value,  normal_value, -normal_value ) );
			normals.append( new MVector( -normal_value,  normal_value,  normal_value ) );
			normals.append( new MVector(  normal_value,  normal_value,  normal_value ) );
			normals.append( new MVector(  normal_value,  normal_value, -normal_value ) );

			// Define the UVs for the cube. 
			//
			float[] uv_pts = new float[uv_count*2] { 0.375f, 0.0f,
													 0.625f, 0.0f,
													 0.625f, 0.25f,
													 0.375f, 0.25f,
													 0.625f, 0.5f,
													 0.375f, 0.5f,
													 0.625f, 0.75f,
													 0.375f, 0.75f,
													 0.625f, 1.0f,
													 0.375f, 1.0f,
													 0.875f, 0.0f,
													 0.875f, 0.25f,
													 0.125f, 0.0f,
													 0.125f, 0.25f };

			// UV Face Vertex Id.
			//
			int[] uv_fvid = new int[num_face_connects]{ 0, 1, 2, 3, 
														3, 2, 4, 5, 
														5, 4, 6, 7, 
														7, 6, 8, 9, 
														1, 10, 11, 2, 
														12, 0, 3, 13 };

			int i;
			for ( i = 0; i < uv_count; i ++ ) {
				uvs.append_uv( uv_pts[i*2], uv_pts[i*2 + 1] ); 
			}
	
			for ( i = 0; i < num_face_connects; i ++ ) { 
				uvs.faceVertexIndex.append( uv_fvid[i] ); 
			}

			// Set up an array containing the number of vertices
			// for each of the 6 cube faces (4 vertices per face)
			//
			int[] face_counts = new int[num_faces]{ 4, 4, 4, 4, 4, 4 };

			for ( i=0; i<num_faces; i++ )
			{
				faceCounts.append( face_counts[i] );
			}

			// Set up and array to assign vertices from pa to each face 
			//
			int[] face_connects = new int[ num_face_connects ]{ 0, 1, 2, 3,
																4, 5, 6, 7,
																3, 2, 6, 5,
																0, 3, 5, 4,
																0, 4, 7, 1,
																1, 7, 6, 2 };
			for ( i=0; i<num_face_connects; i++ )
			{
				faceConnects.append( face_connects[i] );
			}
		}

		//
		// Description
		//
		//    Create circles of vertices starting with 
		//    the top pole ending with the bottom pole
		//
		public void buildSphere(double rad,
								int div,
								MPointArray vertices,
								MIntArray counts,
								MIntArray connects,
								MVectorArray normals,
								apiMeshGeomUV uvs)
		{
			double u = -Math.PI / 2.0;
			double v = -Math.PI;
			double u_delta = Math.PI / ((double)div); 
			double v_delta = 2 * Math.PI / ((double)div); 

			MPoint topPole = new MPoint( 0.0, rad, 0.0 );
			MPoint botPole = new MPoint( 0.0, -rad, 0.0 );

			// Build the vertex and normal table
			//
			vertices.append( botPole );
			normals.append( botPole.minus(MPoint.origin) );
			int i;
			for ( i=0; i<(div-1); i++ )
			{
				u += u_delta;
				v = -Math.PI;

				for ( int j=0; j<div; j++ )
				{
					double x = rad * Math.Cos(u) * Math.Cos(v);
					double y = rad * Math.Sin(u);
					double z = rad * Math.Cos(u) * Math.Sin(v) ;
					MPoint pnt = new MPoint( x, y, z );
					vertices.append( pnt );
					normals.append( pnt.minus(MPoint.origin) );
					v += v_delta;
				}
			}
			vertices.append( topPole );
			normals.append( topPole.minus(MPoint.origin) );

			// Create the connectivity lists
			//
			int vid = 1;
			int numV = 0;
			for ( i=0; i<div; i++ )
			{
				for ( int j=0; j<div; j++ )
				{
					if ( i==0 )
					{
						counts.append( 3 );
						connects.append( 0 );
						connects.append( j+vid );
						connects.append( (j==(div-1)) ? vid : j+vid+1 );
					}
					else if ( i==(div-1) )
					{
						counts.append( 3 );
						connects.append( j+vid+1-div );
						connects.append( vid+1 );
						connects.append( j==(div-1) ? vid+1-div : j+vid+2-div );
					}
					else
					{
						counts.append( 4 );
						connects.append( j + vid+1-div );
						connects.append( j + vid+1 );
						connects.append( j == (div-1) ? vid+1 : j+vid+2 );
						connects.append( j == (div-1) ? vid+1-div : j+vid+2-div );
					}
					numV++;
				}
				vid = numV;
			}

			// TODO: Define UVs for sphere ...
			//
		}
	}
}
