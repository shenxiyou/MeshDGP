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
// apiMesh
//
// Implements a new type of shape node in Maya called apiMesh.
//
// INPUTS
//     inputSurface    - input apiMeshData
//     outputSurface   - output apiMeshData
//     worldSurface    - array of world space apiMeshData, each element
//                       represents an instance of the shape
// OUTPUTS
//     mControlPoints  - inherited control vertices for the mesh. These values
//                       are tweaks (offsets) that will be applied to the
//                       vertices of the input shape.
//     bboxCorner1     - bounding box upper left corner
//     bboxCorner2     - bounding box lower right corner
//
////////////////////////////////////////////////////////////////////////////////

using System;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaRender;
using Autodesk.Maya.OpenMayaRender.MHWRender;

[assembly: MPxShapeClass(typeof(MayaNetPlugin.apiMesh), typeof(MayaNetPlugin.apiMeshUI),
	"apiMeshCSharp", 0x8106f, Classification = "drawdb/geometry/apiMesh")]

namespace MayaNetPlugin
{
	class apiMesh : MPxSurfaceShape
	{
		//////////////////////////////////////////////////////////
		//
		// Attributes
		//
		//////////////////////////////////////////////////////////
		public static MObject inputSurface = null;
		public static MObject outputSurface = null;
		public static MObject worldSurface = null;

		// used to support tweaking of points, the inputSurface attribute data is
		// transferred into the cached surface when it is dirty. The control points
		// tweaks are added into it there.
		//
		public static MObject cachedSurface = null;

		public static MObject bboxCorner1 = null;
		public static MObject bboxCorner2 = null;

		public bool fHasHistoryOnCreate;

        [MPxNodeInitializer()]
		public static void initialize()
		{
			MFnNumericAttribute numAttr = new MFnNumericAttribute();
			MFnTypedAttribute typedAttr = new MFnTypedAttribute();

			// ----------------------- INPUTS --------------------------
			inputSurface = typedAttr.create("inputSurface", "is", new MTypeId(apiMeshData.id));
			typedAttr.isStorable = false;
			addAttribute( inputSurface );

			// ----------------------- OUTPUTS -------------------------

			// bbox attributes
			//
			bboxCorner1 = numAttr.create( "bboxCorner1", "bb1", MFnNumericData.Type.k3Double, 0 );
			numAttr.isArray = false;
			numAttr.usesArrayDataBuilder = false;
			numAttr.isHidden = false;
			numAttr.isKeyable = false;
			addAttribute( bboxCorner1 );

			bboxCorner2 = numAttr.create( "bboxCorner2", "bb2", MFnNumericData.Type.k3Double, 0 );
			numAttr.isArray = false;
			numAttr.usesArrayDataBuilder = false;
			numAttr.isHidden = false;
			numAttr.isKeyable = false;
			addAttribute( bboxCorner2 );

			// local/world output surface attributes
			//
			outputSurface = typedAttr.create("outputSurface", "os", new MTypeId(apiMeshData.id));
			addAttribute( outputSurface );
			typedAttr.isWritable = false;

			worldSurface = typedAttr.create("worldSurface", "ws", new MTypeId(apiMeshData.id));
			typedAttr.isCached = false;
			typedAttr.isWritable = false;
			typedAttr.isArray = true;
			typedAttr.usesArrayDataBuilder = true;
			typedAttr.disconnectBehavior = MFnAttribute.DisconnectBehavior.kDelete;
 			typedAttr.isWorldSpace = true;
			addAttribute( worldSurface );

			// Cached surface used for file IO
			//
			cachedSurface = typedAttr.create("cachedSurface", "cs", new MTypeId(apiMeshData.id));
			typedAttr.isReadable = true;
			typedAttr.isWritable = true;
			typedAttr.isStorable = true;
			addAttribute( cachedSurface );

			// ---------- Specify what inputs affect the outputs ----------
			//
			attributeAffects( inputSurface, outputSurface );
			attributeAffects( inputSurface, worldSurface );
			attributeAffects( outputSurface, worldSurface );
			attributeAffects( inputSurface, bboxCorner1 );
			attributeAffects( inputSurface, bboxCorner2 );
			attributeAffects( cachedSurface, outputSurface );
			attributeAffects( cachedSurface, worldSurface );

			attributeAffects( mControlPoints, outputSurface );
			attributeAffects( mControlValueX, outputSurface );
			attributeAffects( mControlValueY, outputSurface );
			attributeAffects( mControlValueZ, outputSurface );
			attributeAffects( mControlPoints, cachedSurface );
			attributeAffects( mControlValueX, cachedSurface );
			attributeAffects( mControlValueY, cachedSurface );
			attributeAffects( mControlValueZ, cachedSurface );
			attributeAffects( mControlPoints, worldSurface );
			attributeAffects( mControlValueX, worldSurface );
			attributeAffects( mControlValueY, worldSurface );
			attributeAffects( mControlValueZ, worldSurface );
		}

		public apiMesh()
		{
		}

		//////////////////////////////////////////////////////////////////
		//
		// Overrides from MPxNode
		//
		//////////////////////////////////////////////////////////////////

		public override void postConstructor()
		//
		// Description
		//
		//    When instances of this node are created internally, the MObject associated
		//    with the instance is not created until after the constructor of this class
		//    is called. This means that no member functions of MPxSurfaceShape can
		//    be called in the constructor.
		//    The postConstructor solves this problem. Maya will call this function
		//    after the internal object has been created.
		//    As a general rule do all of your initialization in the postConstructor.
		//
		{
			// This call allows the shape to have shading groups assigned
			//
			isRenderable = true;

			// Is there input history to this node
			//
			fHasHistoryOnCreate = false;
		}

		public override bool compute(MPlug plug, MDataBlock datablock)
		//
		// Description
		//
		//    When input attributes are dirty this method will be called to
		//    recompute the output attributes.
		//
		// Arguments
		//
		//    plug      - the attribute that triggered the compute
		//    datablock - the nodes data
		//
		// Returns
		//
		//    kSuccess          - this method could compute the dirty attribute,
		//    kUnknownParameter - the dirty attribute can not be handled at this level
		//
		{
			if ( plug.attribute.equalEqual(outputSurface) ) {
				computeOutputSurface( plug, datablock );
				return true;
			}
			else if ( plug.attribute.equalEqual(cachedSurface) ) {
				computeOutputSurface( plug, datablock );
				return true;
			}
			else if ( plug.attribute.equalEqual(worldSurface) ) {
				computeWorldSurface( plug, datablock );
				return true;
			}
			else {
				return false;
			}
		}

		public override void setDependentsDirty(MPlug plug, MPlugArray plugArray)
		//
		// Description
		//
		//	Horribly abuse the purpose of this method to notify the Viewport 2.0
		//  renderer that something about this shape has changed and that it should
		//  be retranslated.
		//
		{
			// if the dirty attribute is the output mesh then we need to signal the
			// the renderer that it needs to update the object

			if ( plug.attribute.equalEqual(inputSurface) ) {
				MRenderer.setGeometryDrawDirty(thisMObject());
			}
			return;
		}

		public override bool getInternalValue(MPlug plug, MDataHandle result)
		//
		// Description
		//
		//    Handle internal attributes.
		//
		//    Attributes that require special storage, bounds checking,
		//    or other non-standard behavior can be marked as "Internal" by
		//    using the "MFnAttribute.setInternal" method.
		//
		//    The get/setInternalValue methods will get called for internal
		//    attributes whenever the attribute values are stored or retrieved
		//    using getAttr/setAttr or MPlug getValue/setValue.
		//
		//    The inherited attribute mControlPoints is internal and we want
		//    its values to get stored only if there is input history. Otherwise
		//    any changes to the vertices are stored in the cachedMesh and outputMesh
		//    directly.
		//
		//    If values are retrieved then we want the controlPoints value
		//    returned if there is history, this will be the offset or tweak.
		//    In the case of no history, the vertex position of the cached mesh
		//    is returned.
		//
		{
			bool isOk = true;

			if( plug.attribute.equalEqual(mControlPoints) ||
				plug.attribute.equalEqual(mControlValueX) ||
				plug.attribute.equalEqual(mControlValueY) ||
				plug.attribute.equalEqual(mControlValueZ) )
			{
				// If there is input history then the control point value is
				// directly returned. This is the tweak or offset that
				// was applied to the vertex.
				//
				// If there is no input history then return the actual vertex
				// position and ignore the controlPoints attribute.
				//
				if ( hasHistory() ) {
					return base.getInternalValue( plug, result );
				}
				else {
					double val = 0.0;
					if ( plug.attribute.equalEqual(mControlPoints) && !plug.isArray ) {
						MPoint pnt = new MPoint();
						int index = (int)plug.logicalIndex;
						value( index, ref pnt );
						result.set( pnt[0], pnt[1], pnt[2] );
					}
					else if ( plug.attribute.equalEqual(mControlValueX) ) {
						MPlug parentPlug = plug.parent;
						int index = (int)parentPlug.logicalIndex;
						value( index, 0, ref val );
						result.set( val );
					}
					else if ( plug.attribute.equalEqual(mControlValueY) ) {
						MPlug parentPlug = plug.parent;
						int index = (int)parentPlug.logicalIndex;
						value( index, 1, ref val );
						result.set( val );
					}
					else if ( plug.attribute.equalEqual(mControlValueZ) ) {
						MPlug parentPlug = plug.parent;
						int index = (int)parentPlug.logicalIndex;
						value( index, 2, ref val );
						result.set( val );
					}
				}
			}
			// This inherited attribute is used to specify whether or
			// not this shape has history. During a file read, the shape
			// is created before any input history can get connected.
			// This attribute, also called "tweaks", provides a way to
			// for the shape to determine if there is input history
			// during file reads.
			//
			else if ( plug.attribute.equalEqual(mHasHistoryOnCreate) ) {
				result.set( fHasHistoryOnCreate );
			}
			else {
				isOk = base.getInternalValue( plug, result );
			}

			return isOk;
		}

		public override bool setInternalValue(MPlug plug, MDataHandle handle)
		{
			bool isOk = true;

			if( plug.attribute.equalEqual(mControlPoints) ||
				plug.attribute.equalEqual(mControlValueX) ||
				plug.attribute.equalEqual(mControlValueY) ||
				plug.attribute.equalEqual(mControlValueZ) )
			{
				// If there is input history then set the control points value
				// using the normal mechanism. In this case we are setting
				// the tweak or offset that will get applied to the input
				// history.
				//
				// If there is no input history then ignore the controlPoints
				// attribute and set the vertex position directly in the
				// cachedMesh.
				//
				if ( hasHistory() ) {
					verticesUpdated();
					return base.setInternalValue( plug, handle );
				}
				else {
					if( plug.attribute.equalEqual(mControlPoints) && !plug.isArray) {
						int index = (int)plug.logicalIndex;
						MPoint point = new MPoint();
                        double[] ptData = handle.Double3;
						point.x = ptData[0];
						point.y = ptData[1];
						point.z = ptData[2];
						setValue( index, point );
					}
					else if( plug.attribute.equalEqual(mControlValueX) ) {
						MPlug parentPlug = plug.parent;
						int index = (int)parentPlug.logicalIndex;
						setValue( index, 0, handle.asDouble );
					}
					else if( plug.attribute.equalEqual(mControlValueY) ) {
						MPlug parentPlug = plug.parent;
						int index = (int)parentPlug.logicalIndex;
						setValue( index, 1, handle.asDouble );
					}
					else if( plug.attribute.equalEqual(mControlValueZ) ) {
						MPlug parentPlug = plug.parent;
						int index = (int)parentPlug.logicalIndex;
						setValue( index, 2, handle.asDouble );
					}
				}
			}
			// This inherited attribute is used to specify whether or
			// not this shape has history. During a file read, the shape
			// is created before any input history can get connected.
			// This attribute, also called "tweaks", provides a way to
			// for the shape to determine if there is input history
			// during file reads.
			//
			else if ( plug.attribute.equalEqual(mHasHistoryOnCreate) ) {
				fHasHistoryOnCreate = handle.asBool;
			}
			else {
				isOk = base.setInternalValue( plug, handle );
			}

			return isOk;
		}

		public override bool connectionMade(MPlug plug, MPlug otherPlug, bool asSrc)
		//
		// Description
		//
		//    Whenever a connection is made to this node, this method
		//    will get called.
		//
		{
			if ( plug.attribute.equalEqual(inputSurface) ) {
				MObject thisObj = thisMObject();
				MPlug historyPlug = new MPlug( thisObj, mHasHistoryOnCreate );
				historyPlug.setValue( true );
			}

			return base.connectionMade( plug, otherPlug, asSrc );
		}

		public override bool connectionBroken(MPlug plug, MPlug otherPlug, bool asSrc)
		//
		// Description
		//
		//    Whenever a connection to this node is broken, this method
		//    will get called.
		//
		{
			if ( plug.attribute.equalEqual(inputSurface) ) {
				MObject thisObj = thisMObject();
				MPlug historyPlug = new MPlug( thisObj, mHasHistoryOnCreate );
				historyPlug.setValue( false );
			}

			return base.connectionBroken( plug, otherPlug, asSrc );
		}

		public override bool shouldSave(MPlug plug, ref bool result)
		//
		// Description
		//
		//    During file save this method is called to determine which
		//    attributes of this node should get written. The default behavior
		//    is to only save attributes whose values differ from the default.
		//
		//
		//
		{
			if( plug.attribute.equalEqual(mControlPoints) ||
				plug.attribute.equalEqual(mControlValueX) ||
				plug.attribute.equalEqual(mControlValueY) ||
				plug.attribute.equalEqual(mControlValueZ) )
			{
				if( hasHistory() ) {
					// Calling this will only write tweaks if they are
					// different than the default value.
					//
					return base.shouldSave( plug, ref result );
				}
				else {
					result = false;
				}
			}
			else if ( plug.attribute.equalEqual(cachedSurface) ) {
				if ( hasHistory() ) {
					result = false;
				}
				else {
					MObject data = new MObject();
					plug.getValue( data );
					result = ( ! data.isNull );
				}
			}
			else {
				return base.shouldSave( plug, ref result );
			}

			return true;
		}

		public override void componentToPlugs(MObject component, MSelectionList list)
		//
		// Description
		//
		//    Converts the given component values into a selection list of plugs.
		//    This method is used to map components to attributes.
		//
		// Arguments
		//
		//    component - the component to be translated to a plug/attribute
		//    list      - a list of plugs representing the passed in component
		//
		{
			if ( component.hasFn(MFn.Type.kSingleIndexedComponent) ) {

				MFnSingleIndexedComponent fnVtxComp = new MFnSingleIndexedComponent( component );
				MObject thisNode = thisMObject();
				MPlug plug = new MPlug( thisNode, mControlPoints );
				// If this node is connected to a tweak node, reset the
				// plug to point at the tweak node.
				//
				convertToTweakNodePlug(plug);

				int len = fnVtxComp.elementCount;

				for ( int i = 0; i < len; i++ )
				{
					plug.selectAncestorLogicalIndex((uint)fnVtxComp.element(i), plug.attribute);
					list.add(plug);
				}
			}
		}

		public override MatchResult matchComponent(MSelectionList item, MAttributeSpecArray spec, MSelectionList list)
		//
		// Description:
		//
		//    Component/attribute matching method.
		//    This method validates component names and indices which are
		//    specified as a string and adds the corresponding component
		//    to the passed in selection list.
		//
		//    For instance, select commands such as "select shape1.vtx[0:7]"
		//    are validated with this method and the corresponding component
		//    is added to the selection list.
		//
		// Arguments
		//
		//    item - DAG selection item for the object being matched
		//    spec - attribute specification object
		//    list - list to add components to
		//
		// Returns
		//
		//    the result of the match
		//
		{
			MatchResult result = MatchResult.kMatchOk;
			MAttributeSpec attrSpec = spec[0];
			int dim = attrSpec.dimensions;

			// Look for attributes specifications of the form :
			//     vtx[ index ]
			//     vtx[ lower:upper ]
			//
			if ( (1 == spec.length) && (dim > 0) && (attrSpec.name == "vtx") ) {
				int numVertices = (int)meshGeom().vertices.length;
				MAttributeIndex attrIndex = attrSpec[0];

				int upper = 0;
				int lower = 0;
				if ( attrIndex.hasLowerBound ) {
					attrIndex.getLower( out lower );
				}
				if ( attrIndex.hasUpperBound ) {
					attrIndex.getUpper( out upper );
				}

				// Check the attribute index range is valid
				//
				if ( (lower > upper) || (upper >= numVertices) ) {
					result = MatchResult.kMatchInvalidAttributeRange;
				}
				else {
					MDagPath path = new MDagPath();
					item.getDagPath( 0, path );
					MFnSingleIndexedComponent fnVtxComp = new MFnSingleIndexedComponent();
					MObject vtxComp = fnVtxComp.create( MFn.Type.kMeshVertComponent );

					for ( int i=lower; i<=upper; i++ )
					{
						fnVtxComp.addElement( i );
					}
					list.add( path, vtxComp );
				}
			}
			else {
				// Pass this to the parent class
				return base.matchComponent( item, spec, list );
			}

			return result;
		}

		public override bool match(MSelectionMask mask, MObjectArray componentList)
		//
		// Description:
		//
		//		Check for matches between selection type / component list, and
		//		the type of this shape / or it's components
		//
		//      This is used by sets and deformers to make sure that the selected
		//      components fall into the "vertex only" category.
		//
		// Arguments
		//
		//		mask          - selection type mask
		//		componentList - possible component list
		//
		// Returns
		//		true if matched any
		//
		{
			bool result = false;

			if( componentList.length == 0 ) {
				result = mask.intersects( MSelectionMask.SelectionType.kSelectMeshes );
			}
			else {
				for ( int i=0; i<componentList.length; i++ ) {
					if ( (componentList[i].apiType == MFn.Type.kMeshVertComponent) &&
						 (mask.intersects(MSelectionMask.SelectionType.kSelectMeshVerts))
					) {
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Support deformers (components)
		//
		public override MObject createFullVertexGroup()
		//
		// Description
		//     This method is used by Maya when it needs to create a component
		//     containing every vertex (or control point) in the shape.
		//     This will get called if you apply some deformer to the whole
		//     shape, i.e. select the shape in object mode and add a deformer to it.
		//
		// Returns
		//
		//    A "complete" component representing all vertices in the shape.
		//
		{
			// Create a vertex component
			//
			MFnSingleIndexedComponent fnComponent = new MFnSingleIndexedComponent();
			MObject fullComponent = fnComponent.create( MFn.Type.kMeshVertComponent );

			// Set the component to be complete, i.e. the elements in
			// the component will be [0:numVertices-1]
			//
			int numVertices = (int)meshGeom().vertices.length;
			fnComponent.setCompleteData( numVertices );

			return fullComponent;
		}

		public override MObject localShapeInAttr()
		//
		// Description
		//
		//    Returns the input attribute of the shape. This is used by
		//    Maya to establish input connections for deformers etc.
		//    This attribute must be data of type kGeometryData.
		//
		// Returns
		//
		//    input attribute for the shape
		//
		{
			return inputSurface;
		}

		public override MObject localShapeOutAttr()
		//
		// Description
		//
		//    Returns the output attribute of the shape. This is used by
		//    Maya to establish out connections for deformers etc.
		//    This attribute must be data of type kGeometryData.
		//
		// Returns
		//
		//    output attribute for the shape
		//
		//
		{
			return outputSurface;
		}

		public override MObject worldShapeOutAttr()
		//
		// Description
		//
		//    Returns the world space output "array" attribute of the shape.
		//    This is used by Maya to establish out connections for deformers etc.
		//    This attribute must be an array attribute, each element representing
		//    a particular instance of the shape.
		//    This attribute must be data of type kGeometryData.
		//
		// Returns
		//
		//    world space "array" attribute for the shape
		//
		{
			return worldSurface;
		}

		public override MObject cachedShapeAttr()
		//
		// Description
		//
		//    Returns the cached shape attribute of the shape.
		//    This attribute must be data of type kGeometryData.
		//
		// Returns
		//
		//    cached shape attribute
		//
		{
			return cachedSurface;
		}

		public override MObject geometryData()
		//
		// Description
		//
		//    Returns the data object for the surface. This gets
		//    called internally for grouping (set) information.
		//
		{
			apiMesh nonConstThis = (apiMesh)this;
			MDataBlock datablock = nonConstThis._forceCache();
			MDataHandle handle = datablock.inputValue( inputSurface );
			return handle.data();
		}

		public override void closestPoint(MPoint toThisPoint, MPoint theClosestPoint, double tolerance)
		//
		// Description
		//
		//		Returns the closest point to the given point in space.
		//		Used for rigid bind of skin.  Currently returns wrong results;
		//		override it by implementing a closest point calculation.
		{
			// Iterate through the geometry to find the closest point within
			// the given tolerance.
			//
			apiMeshGeom geomPtr = meshGeom();
			uint numVertices = geomPtr.vertices.length;
			for (int ii=0; ii<numVertices; ii++)
			{
				MPoint tryThisOne = geomPtr.vertices[ii];
			}

			// Set the output point to the result (hardcode for debug just now)
			//
			theClosestPoint = geomPtr.vertices[0];
		}


		// Support the translate/rotate/scale tool (components)
		//
		public override void transformUsing(MMatrix mat, MObjectArray componentList)
		//
		// Description
		//
		//    Transforms by the matrix the given components, or the entire shape
		//    if the componentList is empty. This method is used by the freezeTransforms command.
		//
		// Arguments
		//
		//    mat           - matrix to transform the components by
		//    componentList - list of components to be transformed,
		//                    or an empty list to indicate the whole surface
		//
		{
			// Let the other version of transformUsing do the work for us.
			//
			transformUsing( mat,
							componentList,
							MVertexCachingMode.kNoPointCaching,
							null);
		}


		public override void transformUsing(MMatrix mat,
											MObjectArray componentList,
											MVertexCachingMode cachingMode,
											MPointArray pointCache)
		//
		// Description
		//
		//    Transforms the given components. This method is used by
		//    the move, rotate, and scale tools in component mode.
		//    The bounding box has to be updated here, so do the normals and
		//    any other attributes that depend on vertex positions.
		//
		// Arguments
		//    mat           - matrix to transform the components by
		//    componentList - list of components to be transformed,
		//                    or an empty list to indicate the whole surface
		//    cachingMode   - how to use the supplied pointCache
		//    pointCache    - if non-null, save or restore points from this list base
		//					  on the cachingMode
		//
		{
			apiMeshGeom geomPtr = meshGeom();

			bool savePoints = (cachingMode == MVertexCachingMode.kSavePoints);
			int i = 0, j = 0;
			uint len = componentList.length;

			if (cachingMode == MVertexCachingMode.kRestorePoints) {
				// restore the points based on the data provided in the pointCache attribute
				//
				uint cacheLen = pointCache.length;
				if (len > 0) {
					// traverse the component list
					//
					for ( i = 0; i < len && j < cacheLen; i++ )
					{
						MObject comp = componentList[i];
						MFnSingleIndexedComponent fnComp = new MFnSingleIndexedComponent( comp );
						int elemCount = fnComp.elementCount;
						for ( int idx=0; idx<elemCount && j < cacheLen; idx++, ++j ) {
							int elemIndex = fnComp.element( idx );
							geomPtr.vertices[elemIndex] = pointCache[j];
						}
					}
				} else {
					// if the component list is of zero-length, it indicates that we
					// should transform the entire surface
					//
					len = geomPtr.vertices.length;
					for ( int idx = 0; idx < len && j < cacheLen; ++idx, ++j ) {
						geomPtr.vertices[idx] = pointCache[j];
					}
				}
			} else {
				// Transform the surface vertices with the matrix.
				// If savePoints is true, save the points to the pointCache.
				//
				if (len > 0) {
					// Traverse the componentList
					//
					for ( i=0; i<len; i++ )
					{
						MObject comp = componentList[i];
						MFnSingleIndexedComponent fnComp = new MFnSingleIndexedComponent( comp );
						uint elemCount = (uint)fnComp.elementCount;

						if (savePoints && 0 == i) {
							pointCache.sizeIncrement = elemCount;
						}
						for ( int idx=0; idx<elemCount; idx++ )
						{
							int elemIndex = fnComp.element( (int)idx );
							if (savePoints) {
								pointCache.append(geomPtr.vertices[elemIndex]);
							}

							geomPtr.vertices[elemIndex].multiplyEqual( mat );
							geomPtr.normals[idx] = geomPtr.normals[idx].transformAsNormal( mat );
						}
					}
				} else {
					// If the component list is of zero-length, it indicates that we
					// should transform the entire surface
					//
					len = geomPtr.vertices.length;
					if (savePoints) {
						pointCache.sizeIncrement = len;
					}
					for ( int idx = 0; idx < len; ++idx ) {
						if (savePoints) {
							pointCache.append(geomPtr.vertices[idx]);
						}
						geomPtr.vertices[idx].multiplyEqual( mat );
						geomPtr.normals[idx] = geomPtr.normals[idx].transformAsNormal( mat );

					}
				}
			}
			// Retrieve the value of the cached surface attribute.
			// We will set the new geometry data into the cached surface attribute
			//
			// Access the datablock directly. This code has to be efficient
			// and so we bypass the compute mechanism completely.
			// NOTE: In general we should always go though compute for getting
			// and setting attributes.
			//
			MDataBlock datablock = _forceCache();

			MDataHandle cachedHandle = datablock.outputValue( cachedSurface );
			apiMeshData cached = cachedHandle.asPluginData as apiMeshData;

			MDataHandle dHandle = datablock.outputValue( mControlPoints );

			// If there is history then calculate the tweaks necessary for
			// setting the final positions of the vertices.
			//
			if ( hasHistory() && (null != cached) ) {
				// Since the shape has history, we need to store the tweaks (deltas)
				// between the input shape and the tweaked shape in the control points
				// attribute.
				//
				buildControlPoints( datablock, (int)geomPtr.vertices.length );

				MArrayDataHandle cpHandle = new MArrayDataHandle( dHandle );

				// Loop through the component list and transform each vertex.
				//
				for ( i=0; i<len; i++ )
				{
					MObject comp = componentList[i];
					MFnSingleIndexedComponent fnComp = new MFnSingleIndexedComponent( comp );
					int elemCount = fnComp.elementCount;
					for ( int idx=0; idx<elemCount; idx++ )
					{
						int elemIndex = fnComp.element( idx );
						cpHandle.jumpToElement( (uint)elemIndex );
						MDataHandle pntHandle = cpHandle.outputValue();
                        double[] pnt = pntHandle.Double3;

						MPoint oldPnt = cached.fGeometry.vertices[elemIndex];
						MPoint newPnt = geomPtr.vertices[elemIndex];
						MVector offset = newPnt.minus( oldPnt );

						pnt[0] += offset[0];
						pnt[1] += offset[1];
						pnt[2] += offset[2];

                        pntHandle.Double3 = pnt;
					}
				}
			}

			// Copy outputSurface to cachedSurface
			//
			if ( null == cached ) {
                MGlobal.displayInfo("NULL cachedSurface data found");
			}
			else {
				cached.fGeometry = geomPtr;
			}

			MPlug pCPs = new MPlug( thisMObject(), mControlPoints );
			pCPs.setValue(dHandle);

			// Moving vertices will likely change the bounding box.
			//
			computeBoundingBox( datablock );

			// Tell Maya the bounding box for this object has changed
			// and thus "boundingBox()" needs to be called.
			//
			childChanged( MChildChanged.kBoundingBoxChanged );
		}

		public override void tweakUsing( MMatrix mat,
										 MObjectArray componentList,
										 MVertexCachingMode cachingMode,
										 MPointArray pointCache,
										 MArrayDataHandle handle )
		//
		// Description
		//
		//    Transforms the given components. This method is used by
		//    the move, rotate, and scale tools in component mode when the
		//    tweaks for the shape are stored on a separate tweak node.
		//    The bounding box has to be updated here, so do the normals and
		//    any other attributes that depend on vertex positions.
		//
		// Arguments
		//    mat           - matrix to transform the components by
		//    componentList - list of components to be transformed,
		//                    or an empty list to indicate the whole surface
		//    cachingMode   - how to use the supplied pointCache
		//    pointCache    - if non-null, save or restore points from this list base
		//					  on the cachingMode
		//    handle	    - handle to the attribute on the tweak node where the
		//					  tweaks should be stored
		//
		{
			apiMeshGeom geomPtr = meshGeom();

			bool savePoints    = (cachingMode == MVertexCachingMode.kSavePoints);
			bool updatePoints  = (cachingMode == MVertexCachingMode.kUpdatePoints);

			MArrayDataBuilder builder = handle.builder();

			MPoint delta = new MPoint();
			MPoint currPt = new MPoint();
			MPoint newPt = new MPoint();
			int i=0;
			uint len = componentList.length;
			int cacheIndex = 0;
			uint cacheLen = (null != pointCache) ? pointCache.length : 0;

			if (cachingMode == MVertexCachingMode.kRestorePoints) {
				// restore points from the pointCache
				//
				if (len > 0) {
					// traverse the component list
					//
					for ( i=0; i<len; i++ )
					{
						MObject comp = componentList[i];
						MFnSingleIndexedComponent fnComp = new MFnSingleIndexedComponent( comp );
						int elemCount = fnComp.elementCount;
						for ( int idx=0; idx<elemCount && cacheIndex < cacheLen; idx++, cacheIndex++) {
							int elemIndex = fnComp.element( idx );
                            MDataHandle hdl = builder.addElement((uint)elemIndex);
							double[] pt = hdl.Double3;
							MPoint cachePt = pointCache[cacheIndex];
							pt[0] += cachePt.x;
							pt[1] += cachePt.y;
							pt[2] += cachePt.z;
                            hdl.Double3 = pt;
						}
					}
				} else {
					// if the component list is of zero-length, it indicates that we
					// should transform the entire surface
					//
					len = geomPtr.vertices.length;
					for ( uint idx = 0; idx < len && idx < cacheLen; ++idx ) {
                        MDataHandle hdl = builder.addElement(idx);
                        double[] pt = hdl.Double3;
						MPoint cachePt = pointCache[cacheIndex];
						pt[0] += cachePt.x;
						pt[1] += cachePt.y;
						pt[2] += cachePt.z;
                        hdl.Double3 = pt;
					}
				}
			} else {
				// Tweak the points. If savePoints is true, also save the tweaks in the
				// pointCache. If updatePoints is true, add the new tweaks to the existing
				// data in the pointCache.
				//
				if (len > 0) {
					for ( i=0; i<len; i++ )
					{
						MObject comp = componentList[i];
						MFnSingleIndexedComponent fnComp = new MFnSingleIndexedComponent( comp );
						int elemCount = fnComp.elementCount;
						if (savePoints) {
							pointCache.sizeIncrement = (uint)elemCount;
						}
						for ( int idx=0; idx<elemCount; idx++ )
						{
							int elemIndex = fnComp.element( idx );
                            MDataHandle hdl = builder.addElement((uint)elemIndex);
							double[] pt = hdl.Double3;
							currPt = newPt = geomPtr.vertices[elemIndex];
							newPt.multiplyEqual( mat );
							delta.x = newPt.x - currPt.x;
							delta.y = newPt.y - currPt.y;
							delta.z = newPt.z - currPt.z;
							pt[0] += delta.x;
							pt[1] += delta.y;
							pt[2] += delta.z;
                            hdl.Double3 = pt;
							if (savePoints) {
								// store the points in the pointCache for undo
								//
								pointCache.append(delta*(-1.0));
							} else if (updatePoints && cacheIndex < cacheLen) {
								MPoint cachePt = pointCache[cacheIndex];
								cachePt[0] -= delta.x;
								cachePt[1] -= delta.y;
								cachePt[2] -= delta.z;
								cacheIndex++;
							}
						}
					}
				} else {
					// if the component list is of zero-length, it indicates that we
					// should transform the entire surface
					//
					len = geomPtr.vertices.length;
					if (savePoints) {
						pointCache.sizeIncrement = len;
					}
					for ( int idx = 0; idx < len; ++idx ) {
                        MDataHandle hdl = builder.addElement((uint)idx);
						double[] pt = hdl.Double3;
						currPt = newPt = geomPtr.vertices[idx];
						newPt.multiplyEqual( mat );
						delta.x = newPt.x - currPt.x;
						delta.y = newPt.y - currPt.y;
						delta.z = newPt.z - currPt.z;
						pt[0] += delta.x;
						pt[1] += delta.y;
						pt[2] += delta.z;
                        hdl.Double3 = pt;
						if (savePoints) {
							// store the points in the pointCache for undo
							//
							pointCache.append(delta*-1.0);
						} else if (updatePoints && idx < cacheLen) {
							MPoint cachePt = pointCache[idx];
							cachePt[0] -= delta.x;
							cachePt[1] -= delta.y;
							cachePt[2] -= delta.z;
						}
					}
				}
			}
			// Set the builder into the handle.
			//
			handle.set(builder);

			// Tell Maya the bounding box for this object has changed
			// and thus "boundingBox()" needs to be called.
			//
			childChanged( MChildChanged.kBoundingBoxChanged );
		}

		// Support the move tools normal/u/v mode (components)
		//
		public override bool vertexOffsetDirection( MObject component,
													MVectorArray direction,
													MVertexOffsetMode mode,
													bool normalize )
		//
		// Description
		//
		//    Returns offsets for the given components to be used my the
		//    move tool in normal/u/v mode.
		//
		// Arguments
		//
		//    component - components to calculate offsets for
		//    direction - array of offsets to be filled
		//    mode      - the type of offset to be calculated
		//    normalize - specifies whether the offsets should be normalized
		//
		// Returns
		//
		//    true if the offsets could be calculated, false otherwise
		//
		{
			bool offsetOkay = false ;

			MFnSingleIndexedComponent fnComp = new MFnSingleIndexedComponent( component );
			if ( component.apiType != MFn.Type.kMeshVertComponent ) {
				return false;
			}

			offsetOkay = true ;

			apiMeshGeom geomPtr = meshGeom();
			if ( null == geomPtr ) {
				return false;
			}

			// For each vertex add the appropriate offset
			//
			int count = fnComp.elementCount;
			for ( int idx=0; idx<count; idx++ )
			{
				MVector normal = geomPtr.normals[ fnComp.element(idx) ];

				if( mode == MVertexOffsetMode.kNormal ) {
					if( normalize ) normal.normalize() ;
					direction.append( normal );
				}
				else {
					// Construct an orthonormal basis from the normal
					// uAxis, and vAxis are the new vectors.
					//
					MVector uAxis = new MVector();
					MVector vAxis = new MVector();
					uint i, j, k;
					double a;
					normal.normalize();

					i = 0;
					a = Math.Abs( normal[0] );
					if ( a < Math.Abs(normal[1]) )
					{
						i = 1;
						a = Math.Abs(normal[1]);
					}

					if ( a < Math.Abs(normal[2]) )
					{
						i = 2;
					}

					j = (i+1)%3;
					k = (j+1)%3;

					a = Math.Sqrt(normal[i]*normal[i] + normal[j]*normal[j]);
					uAxis[i] = -normal[j]/a;
					uAxis[j] = normal[i]/a;
					uAxis[k] = 0.0;
					vAxis = normal.crossProduct( uAxis );

					if ( mode == MVertexOffsetMode.kUTangent ||
						 mode == MVertexOffsetMode.kUVNTriad )
					{
						if( normalize ) uAxis.normalize() ;
						direction.append( uAxis );
					}

					if ( mode == MVertexOffsetMode.kVTangent ||
						 mode == MVertexOffsetMode.kUVNTriad )
					{
						if( normalize ) vAxis.normalize() ;
						direction.append( vAxis );
					}

					if ( mode == MVertexOffsetMode.kUVNTriad ) {
						if( normalize ) normal.normalize() ;
						direction.append( normal );
					}
				}
			}

			return offsetOkay;
		}

		// Bounding box methods
		//
		public override bool isBounded()
		//
		// Description
		//
		//    Specifies that this object has a boundingBox.
		//
		{
			return true;
		}

		public override MBoundingBox boundingBox()
		//
		// Description
		//
		//    Returns the bounding box for this object.
		//    It is a good idea not to recompute here as this funcion is called often.
		//
		{
			MObject thisNode = thisMObject();
			MPlug c1Plug = new MPlug( thisNode, bboxCorner1 );
			MPlug c2Plug = new MPlug( thisNode, bboxCorner2 );
			MObject corner1Object = new MObject();
			MObject corner2Object = new MObject();
			c1Plug.getValue( corner1Object );
			c2Plug.getValue( corner2Object );

			double[] corner1 = new double[3];
			double[] corner2 = new double[3];

			MFnNumericData fnData = new MFnNumericData();
			fnData.setObject( corner1Object );
			fnData.getData(out corner1[0], out corner1[1], out corner1[2]);
			fnData.setObject( corner2Object );
			fnData.getData(out corner2[0], out corner2[1], out corner2[2]);

			MPoint corner1Point = new MPoint( corner1[0], corner1[1], corner1[2] );
			MPoint corner2Point = new MPoint( corner2[0], corner2[1], corner2[2] );

			return new MBoundingBox( corner1Point, corner2Point );
		}

		// Associates a user defined iterator with the shape (components)
		//
		public override MPxGeometryIterator geometryIteratorSetup( MObjectArray componentList,
																   MObject components,
																   bool forReadOnly = false )
		//
		// Description
		//
		//    Creates a geometry iterator compatible with his shape.
		//
		// Arguments
		//
		//    componentList - list of components to be iterated
		//    components    - component to be iterator
		//    forReadOnly   -
		//
		// Returns
		//
		//    An iterator for the components
		//
		{
			apiMeshGeomIterator result = null;
			if ( components.isNull ) {
				result = new apiMeshGeomIterator( meshGeom(), componentList );
			}
			else {
				result = new apiMeshGeomIterator( meshGeom(), components );
			}
			return result;
		}

		public override bool acceptsGeometryIterator( bool writeable = true )
		//
		// Description
		//
		//    Specifies that this shape can provide an iterator for getting/setting
		//    control point values.
		//
		// Arguments
		//
		//    writable - maya asks for an iterator that can set points if this is true
		//
		{
			return true;
		}

		public override bool acceptsGeometryIterator( MObject obj,
													  bool writeable = true,
													  bool forReadOnly = false )
		//
		// Description
		//
		//    Specifies that this shape can provide an iterator for getting/setting
		//    control point values.
		//
		// Arguments
		//
		//    writable   - Maya asks for an iterator that can set points if this is true
		//    forReadOnly - Maya asking for an iterator for querying only
		//
		{
			return true;
		}

		//////////////////////////////////////////////////////////
		//
		// Helper methods
		//
		//////////////////////////////////////////////////////////

		public bool hasHistory()
		//
		// Description
		//
		//    Returns true if the shape has input history, false otherwise.
		//
		{
			return fHasHistoryOnCreate;
		}

		public void computeInputSurface( MPlug plug, MDataBlock datablock )
		//
		// Description
		//
		//    If there is input history, evaluate the input attribute
		//
		{
			// Get the input surface if there is history
			//
			if ( hasHistory() ) {
				MDataHandle inputHandle = datablock.inputValue( inputSurface );
				apiMeshData surf = inputHandle.asPluginData as apiMeshData;
				if ( null == surf ) {
					throw new ArgumentException("NULL inputSurface data found", "datablock");
				}

				apiMeshGeom geomPtr = surf.fGeometry;

				// Create the cachedSurface and copy the input surface into it
				//
				MFnPluginData fnDataCreator = new MFnPluginData();
				fnDataCreator.create(new MTypeId(apiMeshData.id));
				apiMeshData newCachedData = (apiMeshData)fnDataCreator.data();
				newCachedData.fGeometry = geomPtr;

				MDataHandle cachedHandle = datablock.outputValue( cachedSurface );
				cachedHandle.set( newCachedData );
			}
		}

		public void computeOutputSurface( MPlug plug, MDataBlock datablock )
		//
		// Description
		//
		//    Compute the outputSurface attribute.
		//
		//    If there is no history, use cachedSurface as the
		//    input surface. All tweaks will get written directly
		//    to it. Output is just a copy of the cached surface
		//    that can be connected etc.
		//
		{
			// Check for an input surface. The input surface, if it
			// exists, is copied to the cached surface.
			//
			computeInputSurface( plug, datablock );
			
			// Get a handle to the cached data
			//
			MDataHandle cachedHandle = datablock.outputValue( cachedSurface );
			apiMeshData cached = cachedHandle.asPluginData as apiMeshData;
			if ( null == cached ) {
				MGlobal.displayInfo( "NULL cachedSurface data found" );
			}

			datablock.setClean( plug );

			// Apply any vertex offsets.
			//
			if ( hasHistory() ) {
				applyTweaks( datablock, cached.fGeometry );
			}
			else {
				MArrayDataHandle cpHandle = datablock.inputArrayValue( mControlPoints );
				cpHandle.setAllClean();
			}

			// Create some output data
			//
			MFnPluginData fnDataCreator = new MFnPluginData();
			fnDataCreator.create(new MTypeId(apiMeshData.id));
			apiMeshData newData = (apiMeshData)fnDataCreator.data();

			// Copy the data
			//
			if ( null != cached ) {
				newData.fGeometry = cached.fGeometry;
			}
			else {
				MGlobal.displayInfo( "computeOutputSurface: NULL cachedSurface data" );
			}

			// Assign the new data to the outputSurface handle
			//
			MDataHandle outHandle = datablock.outputValue( outputSurface );
			outHandle.set( newData );

			// Update the bounding box attributes
			//
			computeBoundingBox( datablock );
		}

		public void computeWorldSurface( MPlug plug, MDataBlock datablock )
		//
		// Description
		//
		//    Compute the worldSurface attribute.
		//
		{
			computeOutputSurface( plug, datablock );
			MDataHandle inHandle = datablock.outputValue( outputSurface );
			apiMeshData outSurf = inHandle.asPluginData as apiMeshData;
			if ( null == outSurf ) {
				throw new ArgumentException("computeWorldSurface: outSurf NULL", "datablock");
			}

			// Create some output data
			//
			MFnPluginData fnDataCreator = new MFnPluginData();
			fnDataCreator.create(new MTypeId(apiMeshData.id));
			apiMeshData newData = (apiMeshData)fnDataCreator.data();

			// Get worldMatrix from MPxSurfaceShape and set it to MPxGeometryData
			MMatrix worldMat = getWorldMatrix(datablock, 0);
            newData.matrix(worldMat);

			// Copy the data
			//
			newData.fGeometry = outSurf.fGeometry;

			// Assign the new data to the outputSurface handle
			//
			uint arrayIndex = plug.logicalIndex;

			MArrayDataHandle worldHandle = datablock.outputArrayValue( worldSurface );
			MArrayDataBuilder builder = worldHandle.builder();
			MDataHandle outHandle = builder.addElement( arrayIndex );
			outHandle.set( newData );
		}

		public void computeBoundingBox( MDataBlock datablock )
		//
		// Description
		//
		//    Use the larges/smallest vertex positions to set the corners
		//    of the bounding box.
		//
		{
			// Update bounding box
			//
			MDataHandle lowerHandle = datablock.outputValue( bboxCorner1 );
			MDataHandle upperHandle = datablock.outputValue( bboxCorner2 );

            double[] lower = lowerHandle.Double3;
			double[] upper = upperHandle.Double3;

			apiMeshGeom geomPtr = meshGeom();
			uint cnt = geomPtr.vertices.length;
			if ( cnt == 0 ) return;

			// This clears any old bbox values
			//
			MPoint tmppnt = geomPtr.vertices[0];
			lower[0] = tmppnt[0]; lower[1] = tmppnt[1]; lower[2] = tmppnt[2];
			upper[0] = tmppnt[0]; upper[1] = tmppnt[1]; upper[2] = tmppnt[2];

			for ( int i=0; i<cnt; i++ )
			{
				MPoint pnt = geomPtr.vertices[i];

				if ( pnt[0] < lower[0] ) lower[0] = pnt[0];
				if ( pnt[1] < lower[1] ) lower[1] = pnt[1];
				if ( pnt[2] > lower[2] ) lower[2] = pnt[2];
				if ( pnt[0] > upper[0] ) upper[0] = pnt[0];
				if ( pnt[1] > upper[1] ) upper[1] = pnt[1];
				if ( pnt[2] < upper[2] ) upper[2] = pnt[2];
			}

			lowerHandle.Double3 = lower;
			lowerHandle.setClean();

            upperHandle.Double3 = upper;
			upperHandle.setClean();

			// Signal that the bounding box has changed.
			//
			childChanged(MPxSurfaceShape.MChildChanged.kBoundingBoxChanged);
		}

		public void applyTweaks(MDataBlock datablock, apiMeshGeom meshGeom)
		//
		// Description
		//
		//    If the shape has history, apply any tweaks (offsets) made
		//    to the control points.
		//
		{
			MArrayDataHandle cpHandle = datablock.inputArrayValue( mControlPoints );

			// Loop through the component list and transform each vertex.
			//
			uint elemCount = cpHandle.elementCount();
			for ( uint idx=0; idx<elemCount; idx++ )
			{
				int elemIndex = (int)cpHandle.elementIndex();
				MDataHandle pntHandle = cpHandle.outputValue();
				double[] pnt = pntHandle.Double3;
				MVector offset = new MVector(pnt[0], pnt[1], pnt[2]);

				// Apply the tweaks to the output surface
				//
				MPoint oldPnt = meshGeom.vertices[elemIndex];
				oldPnt = oldPnt.plus( offset );

				cpHandle.next();
			}

			return;
		}

		public bool value(int pntInd, int vlInd, ref double val)
		//
		// Description
		//
		//    Helper function to return the value of a given vertex
		//    from the cachedMesh.
		//
		{
			bool result = false;

			apiMesh nonConstThis = (apiMesh)this;
			apiMeshGeom meshGeom = nonConstThis.cachedGeom();
			if (null != meshGeom)
			{
				MPoint point = meshGeom.vertices[pntInd];
				val = point[(uint)vlInd];
				result = true;
			}

			return result;
		}

		public bool value( int pntInd, ref MPoint val )
		//
		// Description
		//
		//    Helper function to return the value of a given vertex
		//    from the cachedMesh.
		//
		{
			bool result = false;

			apiMesh nonConstThis = (apiMesh)this;
			apiMeshGeom meshGeom = nonConstThis.cachedGeom();
			if (null != meshGeom)
			{
				MPoint point = meshGeom.vertices[pntInd];
				val = point;
				result = true;
			}

			return result;
		}

		public bool setValue(int pntInd, int vlInd, double val)
		//
		// Description
		//
		//    Helper function to set the value of a given vertex
		//    in the cachedMesh.
		//
		{
			bool result = false;

			apiMesh nonConstThis = (apiMesh)this;
			apiMeshGeom meshGeom = nonConstThis.cachedGeom();
			if (null != meshGeom)
			{
				MPoint point = meshGeom.vertices[pntInd];
				point[(uint)vlInd] = val;
				result = true;
			}

			verticesUpdated();

			return result;
		}

		public bool setValue(int pntInd, MPoint val)
		//
		// Description
		//
		//    Helper function to set the value of a given vertex
		//    in the cachedMesh.
		//
		{
			bool result = false;

			apiMesh nonConstThis = (apiMesh)this;
			apiMeshGeom meshGeom = nonConstThis.cachedGeom();
			if (null != meshGeom)
			{
				meshGeom.vertices[pntInd] = val;
				result = true;
			}

			verticesUpdated();

			return result;
		}

		public MObject meshDataRef()
		//
		// Description
		//
		//    Get a reference to the mesh data (outputSurface)
		//    from the datablock. If dirty then an evaluation is
		//    triggered.
		//
		{
			// Get the datablock for this node
			//
			MDataBlock datablock = _forceCache();

			// Calling inputValue will force a recompute if the
			// connection is dirty. This means the most up-to-date
			// mesh data will be returned by this method.
			//
			MDataHandle handle = datablock.inputValue(outputSurface);
			return handle.data();
		}

		public apiMeshGeom meshGeom()
		//
		// Description
		//
		//    Returns a pointer to the apiMeshGeom underlying the shape.
		//
		{
			apiMeshGeom result = null;

			MObject tmpObj = meshDataRef();
			MFnPluginData fnData = new MFnPluginData( tmpObj );
			apiMeshData data = (apiMeshData)fnData.data();

			if ( null != data ) {
				result = data.fGeometry;
			}

			return result;
		}

		public MObject cachedDataRef()
		//
		// Description
		//
		//    Get a reference to the mesh data (cachedSurface)
		//    from the datablock. No evaluation is triggered.
		//
		{
			// Get the datablock for this node
			//
			MDataBlock datablock = _forceCache();
			MDataHandle handle = datablock.outputValue(cachedSurface);
			return handle.data();
		}

		public apiMeshGeom cachedGeom()
		//
		// Description
		//
		//    Returns a pointer to the apiMeshGeom underlying the shape.
		//
		{
			apiMeshGeom result = null;

			MObject tmpObj = cachedDataRef();
			MFnPluginData fnData = new MFnPluginData( tmpObj );
			apiMeshData data = (apiMeshData)fnData.data();

			if ( null != data ) {
				result = data.fGeometry;
			}

			return result;
		}

		public void buildControlPoints(MDataBlock datablock, int count)
		//
		// Description
		//
		//    Check the controlPoints array. If there is input history
		//    then we will use this array to store tweaks (vertex movements).
		//
		{
			MArrayDataHandle cpH = datablock.outputArrayValue( mControlPoints );

			MArrayDataBuilder oldBuilder = cpH.builder();
			if ( count != (int)oldBuilder.elementCount )
			{
				// Make and set the new builder based on the
				// info from the old builder.
				MArrayDataBuilder builder = new MArrayDataBuilder( oldBuilder );
				cpH.set( builder );
			}

			cpH.setAllClean();
		}

		public void verticesUpdated()
		//
		// Description
		//
		//    Helper function to tell Maya that this shape's
		//    vertices have updated and that the bbox needs
		//    to be recalculated and the shape redrawn.
		//
		{
			childChanged( MPxSurfaceShape.MChildChanged.kBoundingBoxChanged );
			childChanged(MPxSurfaceShape.MChildChanged.kObjectChanged);
		}
	}
}
