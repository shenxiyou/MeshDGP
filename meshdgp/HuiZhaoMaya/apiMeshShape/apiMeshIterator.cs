// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


////////////////////////////////////////////////////////////////////////////////
//
// Point iterator for control-point based geometry
//
// This is used by the translate/rotate/scale manipulators to 
// determine where to place the manipulator when components are
// selected.
//
// As well deformers use this class to deform points of the shape.
//
////////////////////////////////////////////////////////////////////////////////

using Autodesk.Maya.OpenMaya;


namespace MayaNetPlugin
{
	class apiMeshGeomIterator : MPxGeometryIterator
	{
		public apiMeshGeom meshGeometry;

		public apiMeshGeomIterator(object userGeometry, MObjectArray components)
			: base(userGeometry, components)
		{
			meshGeometry = (apiMeshGeom)userGeometry;

			reset();
		}

		public apiMeshGeomIterator(object userGeometry, MObject component)
			: base(userGeometry, component)
		{
			meshGeometry = (apiMeshGeom)userGeometry;

			reset();
		}

		//////////////////////////////////////////////////////////
		//
		// Overrides
		//
		//////////////////////////////////////////////////////////

		public override void reset()
		//
		// Description
		//
		//   Resets the iterator to the start of the components so that another
		//   pass over them may be made.
		//
		{
			base.reset();
			currentPoint = 0;
			if (null != meshGeometry)
			{
				int maxVertex = (int)meshGeometry.vertices.length;
				maxPoints = maxVertex;
			}
		}

		public override MPoint point()
		//
		// Description
		//
		//    Returns the point for the current element in the iteration.
		//    This is used by the transform tools for positioning the
		//    manipulator in component mode. It is also used by deformers.	 
		//
		{
			MPoint pnt = new MPoint();
			if (null != meshGeometry)
			{
				pnt = meshGeometry.vertices[index()];
			}
			return pnt;
		}

		public override void setPoint(MPoint pnt)
		//
		// Description
		//
		//    Set the point for the current element in the iteration.
		//    This is used by deformers.
		//
		{
			if (null != meshGeometry)
			{
				meshGeometry.vertices.set(pnt, (uint)index());
			}
		}

		public override int iteratorCount()
		//
		// Description
		//
		//    Return the number of vertices in the iteration.
		//    This is used by deformers such as smooth skinning
		//
		{
			return (int)(meshGeometry.vertices.length);
		}

		public override bool hasPoints()
		//
		// Description
		//
		//    Returns true since the shape data has points.
		//
		{
			return true;
		}
	}
}
