// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


//////////////////////////////////////////////////////////////////
//
// Single-bone, single-plane IK-solver.
//
// This plugin demonstrates how to create and register an IK-solver.
// Due to the complex nature of IK solvers, this plugin only 
// works with 2-joint skeletons (1 bone) in the x-y plane.
//
// To use the solver, create a single bone (joint tool).
// Then type the following in the command window after loading the plug-in:
//
//   createNode simpleSolverNodeCSharp;
//   ikHandle -sol simpleSolverNodeCSharp1 -sj joint1 -ee joint2;
//
// This creates a handle that can be dragged around in the x-y
// plane.
//
// Alternatively, you can directly load the accompanying MEL script.
//
//////////////////////////////////////////////////////////////////

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaAnim;


[assembly: MPxNodeClass(typeof(MayaNetPlugin.simpleSolverNode),
	"simpleSolverNodeCSharp",
	0x00081067,
	NodeType = MPxNode.NodeType.kIkSolverNode)]



namespace MayaNetPlugin
{

	class simpleSolverNode : MPxIkSolverNode
	{
		public override string solverTypeName()
		{
			return "simpleSolverNode";
		}

		public override void doSolve()
		{
			doSimpleSolver();
		}

		private void doSimpleSolver()
		//
		// Solve single joint in the x-y plane
		//
		// - first it calculates the angle between the handle and the end-effector.
		// - then it determines which way to rotate the joint.
		//
		{
			// Get the handle and create a function set for it
			//
			MIkHandleGroup handle_group = handleGroup;
			if (null == handle_group) {
				throw new System.InvalidOperationException("invalid handle group.");
			}

			MObject handle = handle_group.handle( 0 );
			MDagPath handlePath = MDagPath.getAPathTo( handle );
			MFnIkHandle fnHandle = new MFnIkHandle( handlePath );

			// Get the position of the end_effector
			//
			MDagPath end_effector = new MDagPath();
			fnHandle.getEffector(end_effector);
			MFnTransform tran = new MFnTransform( end_effector );
			MPoint effector_position = tran.rotatePivot( MSpace.Space.kWorld );

			// Get the position of the handle
			//
			MPoint handle_position = fnHandle.rotatePivot( MSpace.Space.kWorld );

			// Get the start joint position
			//
			MDagPath start_joint = new MDagPath();
			fnHandle.getStartJoint( start_joint );
			MFnTransform start_transform = new MFnTransform( start_joint );
			MPoint start_position = start_transform.rotatePivot( MSpace.Space.kWorld );

			// Calculate the rotation angle
			//
			MVector v1 = start_position.minus( effector_position );
			MVector v2 = start_position.minus( handle_position );
			double angle = v1.angle( v2 );

			// -------- Figure out which way to rotate --------
			//
			//  define two vectors U and V as follows
			//  U   =   EndEffector(E) - StartJoint(S)
			//  N   =   Normal to U passing through EndEffector
			//
			//  Clip handle_position to half-plane U to determine the region it
			//  lies in. Use the region to determine  the rotation direction.
			//
			//             U
			//             ^              Region      Rotation
			//             |  B           
			//            (E)---N            A          C-C-W
			//         A   |                 B           C-W
			//             |  B
			//             |
			//            (S)
			//
			double rot = 0.0;	// Rotation about Z-axis

			// U and N define a half-plane to clip the handle against
			//
			MVector U = effector_position.minus( start_position );
			U.normalize();

			// Get a normal to U
			//
			MVector zAxis = new MVector( 0.0, 0.0, 1.0 );
			MVector N = U.crossProduct( zAxis );	// Cross product
			N.normalize();

			// P is the handle position vector
			//
			MVector P = handle_position.minus( effector_position );

			// Determine the rotation direction
			//
			double PdotN = P[0]*N[0] + P[1]*N[1];
			if ( PdotN < 0 ) {
				// counter-clockwise
				rot = angle;
			} else {
				// clockwise
				rot = -1.0 * angle;
			}

			// get and set the Joint Angles 
			//
			MDoubleArray jointAngles = new MDoubleArray();
			getJointAngles( jointAngles );
			jointAngles.set( jointAngles[0] + rot, 0 );
			setJointAngles( jointAngles );
		}
	}
}
