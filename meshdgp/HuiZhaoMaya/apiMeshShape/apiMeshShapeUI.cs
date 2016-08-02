// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


///////////////////////////////////////////////////////////////////////////////
//
// apiMeshShapeUI.h
//
// Encapsulates the UI portion of a user defined shape. All of the
// drawing and selection code goes here.
//
////////////////////////////////////////////////////////////////////////////////

using System;

using Autodesk.Maya.OpenMaya;

using Autodesk.Maya.OpenMayaUI;

namespace MayaNetPlugin
{
	class apiMeshUI : MPxSurfaceShapeUI
	{
		/////////////////////////////////////////////////////////////////////
		//
		// Overrides
		//
		/////////////////////////////////////////////////////////////////////

		public override void getDrawRequests( MDrawInfo info,
											  bool objectAndActiveOnly,
											  MDrawRequestQueue queue)
		//
		// Description:
		//
		//     Add draw requests to the draw queue
		//
		// Arguments:
		//
		//     info                 - current drawing state
		//     objectsAndActiveOnly - no components if true
		//     queue                - queue of draw requests to add to
		//
		{
			// Get the data necessary to draw the shape
			//
			MDrawData data = new MDrawData();
			apiMesh meshNode = (apiMesh)surfaceShape;
			apiMeshGeom geom = meshNode.meshGeom();
			if ( (null == geom) || (0 == geom.faceCount) )
			{
                MGlobal.displayInfo("NO DrawRequest for apiMesh");
				return;
			}

			// This call creates a prototype draw request that we can fill
			// in and then add to the draw queue.
			//
			MDrawRequest request = getDrawRequest(info); // info.getPrototype(this);
			getDrawData( geom, out data );
			request.setDrawData( data );

			// Decode the draw info and determine what needs to be drawn
			//

			M3dView.DisplayStyle  appearance    = info.displayStyle;
			M3dView.DisplayStatus displayStatus = info.displayStatus;
	
			// Are we displaying meshes?
			if ( ! info.objectDisplayStatus( M3dView.DisplayObjects.kDisplayMeshes ) )
				return;
	
			// Use this code to help speed up drawing. 
			// inUserInteraction() is true for any interaction with 
			// the viewport, including object or component TRS and camera changes. 
			// userChangingViewContext() is true only when the user is using view 
			// context tools (tumble, dolly, track, etc.)
			//
			if ( info.inUserInteraction || info.userChangingViewContext )
			{
				// User is using view context tools so
				// request fast draw and get out
				//
				request.token = (int)DrawToken.kDrawRedPointAtCenter;
				queue.add( request );
				return;
			}

			switch ( appearance )
			{
				case M3dView.DisplayStyle.kWireFrame :
				{
					request.token = (int)DrawToken.kDrawWireframe;

					int activeColorTable = (int)M3dView.ColorTable.kActiveColors;
					int dormantColorTable = (int)M3dView.ColorTable.kDormantColors;

					switch ( displayStatus )
					{
						case M3dView.DisplayStatus.kLead :
							request.setColor( LEAD_COLOR, activeColorTable );
							break;
						case M3dView.DisplayStatus.kActive :
							request.setColor( ACTIVE_COLOR, activeColorTable );
							break;
						case M3dView.DisplayStatus.kActiveAffected :
							request.setColor( ACTIVE_AFFECTED_COLOR, activeColorTable );
							break;
						case M3dView.DisplayStatus.kDormant :
							request.setColor( DORMANT_COLOR, dormantColorTable );
							break;
						case M3dView.DisplayStatus.kHilite :
							request.setColor( HILITE_COLOR, activeColorTable );
							break;
						default:	
							break;
					}

					queue.add( request );

					break;
				}

				case M3dView.DisplayStyle.kGouraudShaded :
				{
					// Create the smooth shaded draw request
					//
					request.token = (int)DrawToken.kDrawSmoothShaded;

					// Need to get the material info
					//
					MDagPath path = info.multiPath;	// path to your dag object 
					M3dView view = info.view; 		// view to draw to
					MMaterial material = base.material( path );

					// If the user currently has the default material enabled on the 
					// view then use the default material for shading. 
					// 
					if ( view.usingDefaultMaterial ) { 
						material = MMaterial.defaultMaterial; 
					}
			
					// Evaluate the material and if necessary, the texture.
					//
					material.evaluateMaterial( view, path );

					bool drawTexture = true;
					if ( drawTexture && material.materialIsTextured ) {
						material.evaluateTexture( data );
					}
			
					request.material = material;
			
					// request.setDisplayStyle( appearance );
			
					bool materialTransparent = false;
					material.getHasTransparency( ref materialTransparent );
					if ( materialTransparent ) {
						request.isTransparent = true;
					}

					queue.add( request );

					// create a draw request for wireframe on shaded if
					// necessary.
					//
					if ( (displayStatus == M3dView.DisplayStatus.kActive) ||
						 (displayStatus == M3dView.DisplayStatus.kLead) ||
						 (displayStatus == M3dView.DisplayStatus.kHilite) )
					{
						MDrawRequest wireRequest = getDrawRequest(info); // info.getPrototype(this);
						wireRequest.setDrawData( data );
						wireRequest.token = (int)DrawToken.kDrawWireframeOnShaded;
						wireRequest.displayStyle = M3dView.DisplayStyle.kWireFrame;

						int activeColorTable = (int)M3dView.ColorTable.kActiveColors;

						switch ( displayStatus )
						{
							case M3dView.DisplayStatus.kLead :
								wireRequest.setColor( LEAD_COLOR, activeColorTable );
								break;
							case M3dView.DisplayStatus.kActive :
								wireRequest.setColor( ACTIVE_COLOR, activeColorTable );
								break;
							case M3dView.DisplayStatus.kHilite :
								wireRequest.setColor( HILITE_COLOR, activeColorTable );
								break;
							default:
								break;
						}

						queue.add( wireRequest );
					}

					break;
				}

				case M3dView.DisplayStyle.kFlatShaded :
					request.token = (int)DrawToken.kDrawFlatShaded;
					queue.add( request );
					break;
				case M3dView.DisplayStyle.kBoundingBox :
					request.token = (int)DrawToken.kDrawBoundingBox;
					queue.add( request );
					break;
				default:
					break;
			}

			// Add draw requests for components
			//
			if ( !objectAndActiveOnly ) {

				// Inactive components
				//
				if ( (appearance == M3dView.DisplayStyle.kPoints) ||
					 (displayStatus == M3dView.DisplayStatus.kHilite) )
				{
					MDrawRequest vertexRequest = getDrawRequest(info); // info.getPrototype(this);
					vertexRequest.setDrawData( data );
					vertexRequest.token = (int)DrawToken.kDrawVertices;
					vertexRequest.setColor( DORMANT_VERTEX_COLOR,
											(int)M3dView.ColorTable.kActiveColors );

					queue.add( vertexRequest );
				}

				// Active components
				//
				if ( ((MPxSurfaceShape)surfaceShape).hasActiveComponents ) {

					MDrawRequest activeVertexRequest = getDrawRequest(info); // info.getPrototype(this); 
					activeVertexRequest.setDrawData( data );
					activeVertexRequest.token = (int)DrawToken.kDrawVertices;
					activeVertexRequest.setColor( ACTIVE_VERTEX_COLOR,
												  (int)M3dView.ColorTable.kActiveColors);

					MObjectArray clist = ((MPxSurfaceShape)surfaceShape).activeComponents;
					MObject vertexComponent = clist[0]; // Should filter list
					activeVertexRequest.component = vertexComponent;

					queue.add( activeVertexRequest );
				}
			}
		}

		// Main draw routine. Gets called by Maya with draw requests.
		//
		public override void draw( MDrawRequest request, M3dView view )
		//
		// Description:
		//
		//     Main (OpenGL) draw routine
		//
		// Arguments:
		//
		//     request - request to be drawn
		//     view    - view to draw into
		//
		{
			// Get the token from the draw request.
			// The token specifies what needs to be drawn.
			//
			int token = request.token;

			switch( token )
			{
				case (int)DrawToken.kDrawWireframe:
				case (int)DrawToken.kDrawWireframeOnShaded:
					drawWireframe( request, view );
					break;

				case (int)DrawToken.kDrawSmoothShaded:
					drawShaded( request, view );
					break;

				case (int)DrawToken.kDrawFlatShaded: // Not implemented
					break;

				case (int)DrawToken.kDrawVertices:
					drawVertices( request, view );
					break;

				case (int)DrawToken.kDrawBoundingBox:
					drawBoundingBox( request, view );
					break;
				// for userChangingViewContext example code 
				//
				case (int)DrawToken.kDrawRedPointAtCenter:
					drawRedPointAtCenter( request, view );
					break;
			}
		}

		// Main draw routine for UV editor. This is called by maya when the 
		// shape is selected and the UV texture window is visible. 
		// 
		public override void drawUV( M3dView view, MTextureEditorDrawInfo info )
		//
		// Description:
		//   Main entry point for UV drawing. This method is called by the UV
		//   texture editor when the shape is 'active'.
		//
		// Input:
		//   A 3dView.
		//
		{
			apiMesh meshNode = (apiMesh)surfaceShape;
			apiMeshGeom geom = meshNode.meshGeom();

			uint uv_len = geom.uvcoords.uvcount(); 
			if (uv_len > 0)
			{
				view.setDrawColor( new MColor( 1.0f, 0.0f, 0.0f ) ); 
	
				switch( info.drawingFunction ) { 
				case MTextureEditorDrawInfo.DrawingFunction.kDrawWireframe: 
					drawUVWireframe( geom, view, info ); 
					break; 
				case MTextureEditorDrawInfo.DrawingFunction.kDrawEverything: 
				case MTextureEditorDrawInfo.DrawingFunction.kDrawUVForSelect: 
					drawUVWireframe( geom, view, info ); 
					drawUVMapCoordNum( geom, view, info, false ); 
					break; 
				case MTextureEditorDrawInfo.DrawingFunction.kDrawVertexForSelect: 
				case MTextureEditorDrawInfo.DrawingFunction.kDrawEdgeForSelect:
				case MTextureEditorDrawInfo.DrawingFunction.kDrawFacetForSelect: 
				default:
					drawUVWireframe( geom, view, info );
					break;
				};
			}
		}

		public override bool canDrawUV()
		//
		// Description: 
		//  Tells Maya that this surface shape supports uv drawing. 
		//
		{
			apiMesh meshNode = (apiMesh)surfaceShape;
			apiMeshGeom geom = meshNode.meshGeom();
			return geom.uvcoords.uvcount() > 0; 
		}


		// Main selection routine
		//
		public override bool select(MSelectInfo selectInfo,
									MSelectionList selectionList,
									MPointArray worldSpaceSelectPts )
		//
		// Description:
		//
		//     Main selection routine
		//
		// Arguments:
		//
		//     selectInfo           - the selection state information
		//     selectionList        - the list of selected items to add to
		//     worldSpaceSelectPts  -
		//
		{
			bool selected = false;
			bool componentSelected = false;
			bool hilited = false;

			hilited = (selectInfo.displayStatus == M3dView.DisplayStatus.kHilite);
			if ( hilited ) {
				componentSelected = selectVertices( selectInfo, selectionList,
													worldSpaceSelectPts );
				selected = selected || componentSelected;
			}

			if ( !selected ) {

				apiMesh meshNode = (apiMesh)surfaceShape;

				// NOTE: If the geometry has an intersect routine it should
				// be called here with the selection ray to determine if the
				// the object was selected.

				selected = true;
				MSelectionMask priorityMask = new MSelectionMask( MSelectionMask.SelectionType.kSelectNurbsSurfaces );
				MSelectionList item = new MSelectionList();
				item.add( selectInfo.selectPath );
				MPoint xformedPt = new MPoint();
				if ( selectInfo.singleSelection ) {
					MPoint center = meshNode.boundingBox().center;
					xformedPt = center;
					xformedPt.multiplyEqual( selectInfo.selectPath.inclusiveMatrix );
				}

				selectInfo.addSelection( item, xformedPt, selectionList,
										 worldSpaceSelectPts, priorityMask, false );
			}

			return selected;
		}

		/////////////////////////////////////////////////////////////////////
		//
		// Helper routines
		//
		/////////////////////////////////////////////////////////////////////

		public void drawWireframe( MDrawRequest request, M3dView view )
		//
		// Description:
		//
		//     Wireframe drawing routine
		//
		// Arguments:
		//
		//     request - request to be drawn
		//     view    - view to draw into
		//
		{
			MDrawData data = request.drawData();
			apiMeshGeom geom = (apiMeshGeom)data.geometry();
			if (geom == null) return;

			int token = request.token;

			bool wireFrameOnShaded = false;
			if ((int)DrawToken.kDrawWireframeOnShaded == token)
			{
				wireFrameOnShaded = true;
			}

			view.beginGL(); 

			// Query current state so it can be restored
			//
			bool lightingWasOn = OpenGL.glIsEnabled(OpenGL.GL_LIGHTING) != 0;
			if ( lightingWasOn ) {
				OpenGL.glDisable(OpenGL.GL_LIGHTING);
			}

			if ( wireFrameOnShaded ) {
				OpenGL.glDepthMask(0);
			}

			// Draw the wireframe mesh
			//
			int vid = 0;
			for ( int i=0; i<geom.faceCount; i++ )
			{
				OpenGL.glBegin(OpenGL.GL_LINE_LOOP);
				for ( int v=0; v<geom.face_counts[i]; v++ )
				{
					MPoint vertex = geom.vertices[ geom.face_connects[vid++] ];
					OpenGL.glVertex3f((float)vertex[0], (float)vertex[1], (float)vertex[2]);
				}
				OpenGL.glEnd();
			}

			// Restore the state
			//
			if ( lightingWasOn ) {
				OpenGL.glEnable(OpenGL.GL_LIGHTING);
			}
			if ( wireFrameOnShaded ) {
				OpenGL.glDepthMask(1);
			}

			view.endGL(); 
		}

		public void drawShaded( MDrawRequest request, M3dView view )
		//
		// Description:
		//
		//     Shaded drawing routine
		//
		// Arguments:
		//
		//     request - request to be drawn
		//     view    - view to draw into
		//
		{
			MDrawData data = request.drawData();
			apiMeshGeom geom = (apiMeshGeom)data.geometry();
			if (geom == null) return;

			view.beginGL();

			OpenGL.glEnable(OpenGL.GL_POLYGON_OFFSET_FILL);

			// Set up the material
			//
			MMaterial material = request.material;
			material.setMaterial( request.multiPath, request.isTransparent );

			// Enable texturing ... 
			// 
			// Note, Maya does not enable texturing if useDefaultMaterial is enabled. 
			// However, you can choose to ignore this in your draw routine.  
			//
			bool drawTexture = material.materialIsTextured && !view.usingDefaultMaterial;

			if (drawTexture)
			{
				OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
			}

			// Apply the texture to the current view
			//
			if ( drawTexture )
			{
				material.applyTexture( view, data );
			}

			// Draw the polygons
			//
			int vid = 0;
			uint uv_len = geom.uvcoords.uvcount();
			for ( int i=0; i<geom.faceCount; i++ )
			{
				OpenGL.glBegin(OpenGL.GL_POLYGON);
				for ( int v=0; v < geom.face_counts[i]; v++ )
				{
					MPoint vertex = geom.vertices[ geom.face_connects[vid] ];
					MVector normal = geom.normals[ geom.face_connects[vid] ];
					if (uv_len > 0)
					{
						// If we are drawing the texture, make sure the coord 
						// arrays are in bounds.
						if ( drawTexture ) {
							int uvId1 = geom.uvcoords.uvId(vid);
							if ( uvId1 < uv_len )
							{
								float tu = 0.0f;
								float tv = 0.0f;
								geom.uvcoords.getUV( uvId1, ref tu, ref tv );
								OpenGL.glTexCoord2f( tu, tv );
							}
						}
					}

					OpenGL.glNormal3f((float)normal[0], (float)normal[1], (float)normal[2]);
					OpenGL.glVertex3f((float)vertex[0], (float)vertex[1], (float)vertex[2]);
					vid++;
				}
				OpenGL.glEnd();
			}

			// Turn off texture mode
			//
			if (drawTexture)
			{
				OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
			}

			view.endGL(); 
		}

		public void drawVertices( MDrawRequest request, M3dView view )
		//
		// Description:
		//
		//     Component (vertex) drawing routine
		//
		// Arguments:
		//
		//     request - request to be drawn
		//     view    - view to draw into
		//
		{
			MDrawData data = request.drawData();
			apiMeshGeom geom = (apiMeshGeom)data.geometry();
			if (geom == null) return;

			view.beginGL(); 

			// Query current state so it can be restored
			//
			bool lightingWasOn = OpenGL.glIsEnabled(OpenGL.GL_LIGHTING) != 0;
			if ( lightingWasOn ) {
				OpenGL.glDisable(OpenGL.GL_LIGHTING);
			}
			float[] lastPointSize = new float[1];
			OpenGL.glGetFloatv(OpenGL.GL_POINT_SIZE, lastPointSize);

			// Set the point size of the vertices
			//
			OpenGL.glPointSize(POINT_SIZE);

			// If there is a component specified by the draw request
			// then loop over comp (using an MFnComponent class) and draw the
			// active vertices, otherwise draw all vertices.
			//
			MObject comp = request.component;
			if ( ! comp.isNull ) {
				MFnSingleIndexedComponent fnComponent = new MFnSingleIndexedComponent( comp );
				for ( int i=0; i<fnComponent.elementCount; i++ )
				{
					int index = fnComponent.element( i );
					OpenGL.glBegin(OpenGL.GL_POINTS);
					MPoint vertex = geom.vertices[ index ];
					OpenGL.glVertex3f((float)vertex[0], 
								(float)vertex[1], 
								(float)vertex[2] );
					OpenGL.glEnd();

					string annotation = index.ToString();
					view.drawText( annotation, vertex );
				}
			}
			else {
				int vid = 0;
				for ( int i=0; i<geom.faceCount; i++ )
				{
					OpenGL.glBegin(OpenGL.GL_POINTS);
					for ( int v=0; v<geom.face_counts[i]; v++ )
					{
						MPoint vertex = geom.vertices[ geom.face_connects[vid++] ];
						OpenGL.glVertex3f((float)vertex[0], 
									(float)vertex[1], 
									(float)vertex[2] );
					}
					OpenGL.glEnd();
				}
			}

			// Restore the state
			//
			if ( lightingWasOn ) {
				OpenGL.glEnable(OpenGL.GL_LIGHTING);
			}
			OpenGL.glPointSize(lastPointSize[0]);

			view.endGL(); 
		}

		public void drawBoundingBox( MDrawRequest request, M3dView view )
		//
		// Description:
		//
		//     Bounding box drawing routine
		//
		// Arguments:
		//
		//     request - request to be drawn
		//     view    - view to draw into
		//
		{
			// Get the surface shape
			MPxSurfaceShape shape = (MPxSurfaceShape)surfaceShape;
			if ( shape == null )
				return;
		
			// Get the bounding box	
			MBoundingBox box = shape.boundingBox();
			float w = (float) box.width;
			float h = (float) box.height;
			float d = (float) box.depth;
			view.beginGL(); 
	
			// Below we just two sides and then connect
			// the edges together

			MPoint minVertex = box.min;
	
			// Draw first side
			OpenGL.glBegin(OpenGL.GL_LINE_LOOP);
			MPoint vertex = minVertex;
			OpenGL.glVertex3f((float)vertex[0], (float)vertex[1], (float)vertex[2]);
			OpenGL.glVertex3f((float)vertex[0] + w, (float)vertex[1], (float)vertex[2]);
			OpenGL.glVertex3f((float)vertex[0] + w, (float)vertex[1] + h, (float)vertex[2]);
			OpenGL.glVertex3f((float)vertex[0], (float)vertex[1] + h, (float)vertex[2]);
			OpenGL.glVertex3f((float)vertex[0], (float)vertex[1], (float)vertex[2]);
			OpenGL.glEnd();

			// Draw second side
			MVector sideFactor = new MVector(0, 0, d);
			MPoint vertex2 = minVertex.plus( sideFactor );
			OpenGL.glBegin(OpenGL.GL_LINE_LOOP);
			OpenGL.glVertex3f((float)vertex2[0], (float)vertex2[1], (float)vertex2[2]);
			OpenGL.glVertex3f((float)vertex2[0] + w, (float)vertex2[1], (float)vertex2[2]);
			OpenGL.glVertex3f((float)vertex2[0] + w, (float)vertex2[1] + h, (float)vertex2[2]);
			OpenGL.glVertex3f((float)vertex2[0], (float)vertex2[1] + h, (float)vertex2[2]);
			OpenGL.glVertex3f((float)vertex2[0], (float)vertex2[1], (float)vertex2[2]);
			OpenGL.glEnd();

			// Connect the edges together
			OpenGL.glBegin(OpenGL.GL_LINES);
			OpenGL.glVertex3f((float)vertex2[0], (float)vertex2[1], (float)vertex2[2]);
			OpenGL.glVertex3f((float)vertex[0], (float)vertex[1], (float)vertex[2]);

			OpenGL.glVertex3f((float)vertex2[0] + w, (float)vertex2[1], (float)vertex2[2]);
			OpenGL.glVertex3f((float)vertex[0] + w, (float)vertex[1], (float)vertex[2]);

			OpenGL.glVertex3f((float)vertex2[0] + w, (float)vertex2[1] + h, (float)vertex2[2]);
			OpenGL.glVertex3f((float)vertex[0] + w, (float)vertex[1] + h, (float)vertex[2]);

			OpenGL.glVertex3f((float)vertex2[0], (float)vertex2[1] + h, (float)vertex2[2]);
			OpenGL.glVertex3f((float)vertex[0], (float)vertex[1] + h, (float)vertex[2]);
			OpenGL.glEnd();
	
			view.endGL(); 
		}

		// for userInteraction example code
		//
		public void drawRedPointAtCenter( MDrawRequest request, M3dView view )
		//
		// Description:
		//
		//     Simple very fast draw routine
		//
		// Arguments:
		//
		//     request - request to be drawn
		//     view    - view to draw into
		//
		{
			// Draw point
			// 
			view.beginGL();
		
			// save state
			//
			OpenGL.glPushAttrib(OpenGL.GL_CURRENT_BIT | OpenGL.GL_POINT_BIT);
			OpenGL.glPointSize(20.0f);
			OpenGL.glBegin(OpenGL.GL_POINTS);
			OpenGL.glColor3f(1.0f, 0.0f, 0.0f);
			OpenGL.glVertex3f(0.0f, 0.0f, 0.0f);
			OpenGL.glEnd();
							
			// restore state
			//
			OpenGL.glPopAttrib();
			view.endGL();

		}

		public bool selectVertices( MSelectInfo selectInfo,
									MSelectionList selectionList,
									MPointArray worldSpaceSelectPts )
		//
		// Description:
		//
		//     Vertex selection.
		//
		// Arguments:
		//
		//     selectInfo           - the selection state information
		//     selectionList        - the list of selected items to add to
		//     worldSpaceSelectPts  -
		//
		{
			bool selected = false;
			M3dView view = selectInfo.view;

			MPoint xformedPoint = new MPoint();
			MPoint selectionPoint = new MPoint();
			double z = 0.0;
			double previousZ = 0.0;
 			int closestPointVertexIndex = -1;

			MDagPath path = selectInfo.multiPath;

			// Create a component that will store the selected vertices
			//
			MFnSingleIndexedComponent fnComponent = new MFnSingleIndexedComponent();
			MObject surfaceComponent = fnComponent.create( MFn.Type.kMeshVertComponent );
			uint vertexIndex;

			// if the user did a single mouse click and we find > 1 selection
			// we will use the alignmentMatrix to find out which is the closest
			//
			MMatrix alignmentMatrix = new MMatrix();
			MPoint singlePoint = new MPoint(); 
			bool singleSelection = selectInfo.singleSelection;
			if( singleSelection ) {
				alignmentMatrix = selectInfo.alignmentMatrix;
			}

			// Get the geometry information
			//
			apiMesh meshNode = (apiMesh)surfaceShape;
			apiMeshGeom geom = meshNode.meshGeom();

			// Loop through all vertices of the mesh and
			// see if they lie withing the selection area
			//
			uint numVertices = geom.vertices.length;
			for ( vertexIndex=0; vertexIndex<numVertices; vertexIndex++ )
			{
				MPoint currentPoint = geom.vertices[ (int)vertexIndex ];

				// Sets OpenGL's render mode to select and stores
				// selected items in a pick buffer
				//
				view.beginSelect();

				OpenGL.glBegin(OpenGL.GL_POINTS);
				OpenGL.glVertex3f((float)currentPoint[0], 
							(float)currentPoint[1], 
							(float)currentPoint[2] );
				OpenGL.glEnd();

				if ( view.endSelect() > 0 )	// Hit count > 0
				{
					selected = true;

					if ( singleSelection ) {
						xformedPoint = currentPoint;
						xformedPoint.homogenize();
						xformedPoint.multiplyEqual( alignmentMatrix );
						z = xformedPoint.z;
						if ( closestPointVertexIndex < 0 || z > previousZ ) {
							closestPointVertexIndex = (int)vertexIndex;
							singlePoint = currentPoint;
							previousZ = z;
						}
					} else {
						// multiple selection, store all elements
						//
						fnComponent.addElement( (int)vertexIndex );
					}
				}
			}

			// If single selection, insert the closest point into the array
			//
			if ( selected && selectInfo.singleSelection ) {
				fnComponent.addElement(closestPointVertexIndex);

				// need to get world space position for this vertex
				//
				selectionPoint = singlePoint;
				selectionPoint.multiplyEqual( path.inclusiveMatrix );
			}

			// Add the selected component to the selection list
			//
			if ( selected ) {
				MSelectionList selectionItem = new MSelectionList();
				selectionItem.add( path, surfaceComponent );

				MSelectionMask mask = new MSelectionMask( MSelectionMask.SelectionType.kSelectComponentsMask );
				selectInfo.addSelection(
					selectionItem, selectionPoint,
					selectionList, worldSpaceSelectPts,
					mask, true );
			}

			return selected;
		}

		private void drawUVWireframe( apiMeshGeom geom, M3dView view,
									  MTextureEditorDrawInfo info )
		//
		// Description: 
		//  Draws the UV layout in wireframe mode. 
		// 
		{
			view.beginGL(); 
	
			// Draw the polygons
			//
			int vid = 0;
			int vid_start = 0;
			for ( int i=0; i<geom.faceCount; i++ )
			{
				OpenGL.glBegin(OpenGL.GL_LINES);
				uint v;

				float du1 = 0.0f;
				float dv1 = 0.0f;
				float du2 = 0.0f;
				float dv2 = 0.0f;
				int uvId1, uvId2;

				vid_start = vid; 
				for ( v=0; v<geom.face_counts[i]-1; v++ )
				{
					uvId1 = geom.uvcoords.uvId(vid);
					uvId2 = geom.uvcoords.uvId(vid + 1); 
					geom.uvcoords.getUV( uvId1, ref du1, ref dv1 ); 
					geom.uvcoords.getUV( uvId2, ref du2, ref dv2 );
					OpenGL.glVertex3f( du1, dv1, 0.0f );
					OpenGL.glVertex3f( du2, dv2, 0.0f ); 
					vid++;
				}

				uvId1 = geom.uvcoords.uvId(vid);
				uvId2 = geom.uvcoords.uvId(vid_start);
				geom.uvcoords.getUV( uvId1, ref du1, ref dv1 );
				geom.uvcoords.getUV( uvId2, ref du2, ref dv2 );
				OpenGL.glVertex3f(du1, dv1, 0.0f);
				OpenGL.glVertex3f(du2, dv2, 0.0f);
				vid ++ ;
				OpenGL.glEnd();
			} 
	
			view.endGL();
	
		}

		private void drawUVMapCoord( M3dView view, int uv, float u, float v, bool drawNum )
		//
		// Description:
		//  Draw the specified uv value into the port view. If drawNum is true
		//  It will also draw the UV id for the the UV.
		//
		{
			if ( drawNum ) { 
				string s = uv.ToString();
				view.drawText(s, new MPoint(u, v, 0), M3dView.TextPosition.kCenter);
			}
			OpenGL.glVertex3f(u, v, 0.0f);
		}

		private void drawUVMapCoordNum( apiMeshGeom geom, M3dView view,
										MTextureEditorDrawInfo info, bool drawNumbers )
		//
		// Description: 
		//  Draw the UV points for all uvs on this surface shape. 
		//
		{
			view.beginGL(); 

			float[] ptSize = new float[1];
			OpenGL.glGetFloatv(OpenGL.GL_POINT_SIZE, ptSize);
			OpenGL.glPointSize(UV_POINT_SIZE);

			int uv; 
			uint uv_len = geom.uvcoords.uvcount();
			for ( uv = 0; uv < uv_len; uv ++ ) { 
				float du = 0.0f;
				float dv = 0.0f;
				geom.uvcoords.getUV( uv, ref du, ref dv );
				drawUVMapCoord( view, uv, du, dv, drawNumbers );
			}

			OpenGL.glPointSize(ptSize[0]);
			view.endGL(); 
		}

		private MDrawRequest getDrawRequest(MDrawInfo info)
		//
		// Description:
		//
		//     Helper function. It should be removed after SWIGTYPE_p_* classes are eliminated.
		//
		{
			return info.getPrototype(this);
		}

		// Object and component color definitions
		//
		private const int LEAD_COLOR = 18; // green
		private const int ACTIVE_COLOR = 15; // white
		private const int ACTIVE_AFFECTED_COLOR = 8; // purple
		private const int DORMANT_COLOR = 4; // blue
		private const int HILITE_COLOR = 17; // pale blue
		private const int DORMANT_VERTEX_COLOR = 8; // purple
		private const int ACTIVE_VERTEX_COLOR = 16; // yellow

		// Vertex point size
		//
		private const float POINT_SIZE = 2.0f;
		private const float UV_POINT_SIZE = 4.0f;

		// Draw Tokens
		//
		private enum DrawToken
		{
			kDrawVertices = 0, // component token
			kDrawWireframe = 1,
			kDrawWireframeOnShaded = 2,
			kDrawSmoothShaded = 3,
			kDrawFlatShaded = 4,
			kDrawBoundingBox = 5,
			kDrawRedPointAtCenter = 6,  // for userInteraction example code
			kLastToken = 7
		};
	}
}
