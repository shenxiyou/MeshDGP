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


[assembly: MPxDataClass(typeof(MayaNetPlugin.apiMeshData),
	MayaNetPlugin.apiMeshData.id, "apiMeshData", MPxData.DataType.kGeometryData)]

namespace MayaNetPlugin
{
	class apiMeshData : MPxGeometryData
	{
		public static readonly string TypeName = "apiMeshData";
		public const uint id = 0x00081070;

		// This is the geometry our data will pass though the DG
		//
		public apiMeshGeom fGeometry;

		public apiMeshData()
		{
			fGeometry = new apiMeshGeom();
		}

		//////////////////////////////////////////////////////////////////
		//
		// Overrides from MPxData
		//
		//////////////////////////////////////////////////////////////////

		public override void readASCII(MArgList argList, ref uint index)
		//
		// Description
		//    NOT IMPLEMENTED
		//
		{
            MGlobal.displayInfo("apiMeshData.readASCII is called.");
			return;
		}

		public override void readBinary(MIStream stream, uint length)
		//
		// Description
		//     NOT IMPLEMENTED
		//
		{
            MGlobal.displayInfo("apiMeshData.readBinary is called.");
			return;
		}

		public override void writeASCII(MOStream stream)
		//
		// Description
		//    NOT IMPLEMENTED
		//
		{
            MGlobal.displayInfo("apiMeshData.writeASCII is called.");
			return;
		}

		public override void writeBinary(MOStream stream)
		//
		// Description
		//    NOT IMPLEMENTED
		//
		{
            MGlobal.displayInfo("apiMeshData.writeBinary is called.");
			return;
		}

		public override void copy(MPxData src)
		{
			fGeometry = ((apiMeshData)src).fGeometry;
		}

		public override MTypeId typeId()
		//
		// Description
		//    Binary tag used to identify this kind of data
		//
		{
			return new MTypeId(apiMeshData.id);
		}

		public override string name()
		//
		// Description
		//    String name used to identify this kind of data
		//
		{
			return TypeName;
		}

		//////////////////////////////////////////////////////////////////
		//
		// Overrides from MPxGeometryData
		//
		//////////////////////////////////////////////////////////////////

		public override MPxGeometryIterator iterator( MObjectArray componentList,
													  MObject component,
													  bool useComponents )
		{
			apiMeshGeomIterator result = null;
			if ( useComponents ) {
				result = new apiMeshGeomIterator( fGeometry, componentList );
			}
			else {
				result = new apiMeshGeomIterator( fGeometry, component );
			}
			return result;
		}

		public override MPxGeometryIterator iterator( MObjectArray componentList,
													  MObject component,
													  bool useComponents,
													  bool world )
		{
			apiMeshGeomIterator result = null;
			if ( useComponents ) {
				result = new apiMeshGeomIterator( fGeometry, componentList );
			}
			else {
				result = new apiMeshGeomIterator( fGeometry, component );
			}
			return result;
		}

		public override bool updateCompleteVertexGroup( MObject component )
		//
		// Description
		//     Make sure complete vertex group data is up-to-date.
		//     Returns true if the component was updated, false if it was already ok.
		//
		//     This is used by deformers when deforming the "whole" object and
		//     not just selected components.
		//
		{
			MFnSingleIndexedComponent fnComponent = new MFnSingleIndexedComponent( component );

			// Make sure there is non-null geometry and that the component
			// is "complete". A "complete" component represents every 
			// vertex in the shape.
			//
			if ( (null != fGeometry) && (fnComponent.isComplete) ) {
	
				int maxVerts ;
				fnComponent.getCompleteData( out maxVerts );
				int numVertices = (int)fGeometry.vertices.length;

				if ( (numVertices > 0) && (maxVerts != numVertices) ) {
					// Set the component to be complete, i.e. the elements in
					// the component will be [0:numVertices-1]
					//
					fnComponent.setCompleteData( numVertices );
					return true;
				}
			}

			return false;
		}
	}
}
