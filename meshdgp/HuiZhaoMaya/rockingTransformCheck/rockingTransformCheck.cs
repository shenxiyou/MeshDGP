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

[assembly: MPxTransformClass(typeof(MayaNetPlugin.rockingTransformCheckNode),
	"rockingTransformCheckCSharp",
	MayaNetPlugin.rockingTransformCheckNode.kRockingTransformCheckNodeID,
	typeof(MayaNetPlugin.rockingTransformCheckMatrix),
	MayaNetPlugin.rockingTransformCheckMatrix.kRockingTransformCheckMatrixID)]

namespace MayaNetPlugin 
{

    //
    // Example custom transform:
    //	This plug-in implements an example custom transform that
    //	can be used to perform a rocking motion around the X axis.
    //	Geometry of any rotation can be made a child of this transform
    //	to demonstrate the effect.
    //	The plug-in contains two pieces:
    //	1. The custom transform node -- rockingTransformCheckNode
    //	2. The custom transformation matrix -- rockingTransformCheckMatrix
    //	These classes are used together in order to implement the
    //	rocking motion.  Note that the rock attribute is stored outside
    //	of the regular transform attributes.
    //
    // For detailed usage, please refer to the accompanying MEL script.
	//
	class rockingTransformCheckMatrix : MPxTransformationMatrix
	{
		// A really simple implementation of MPxTransformationMatrix.
		// The methods include:
		// - Two accessors methods for getting and setting the rock
		// - The virtual asMatrix() method which passes the matrix 
		// back to Maya when the command "xform -q -ws -m" is invoked

		public const uint kRockingTransformCheckMatrixID = 0x81072;
		public static MTypeId idCheck = new MTypeId(kRockingTransformCheckMatrixID);
		protected double rockXValue;

		public rockingTransformCheckMatrix(bool ownMem = false)
			: base(ownMem)
		{
			rockXValue = 0.0;
		}
		public rockingTransformCheckMatrix(MPxTransformationMatrix obj) : base(obj, false)
		{
			rockXValue = 0.0;
		}
		
		public override MMatrix asMatrix() 
		{
			// Get the current transform matrix
			MMatrix m = base.asMatrix();
			// Initialize the new matrix we will calculate
			MTransformationMatrix tm = new MTransformationMatrix( m );
			// Find the current rotation as a quaternion
			MQuaternion quat = rotation();
			// Convert the rocking value in degrees to radians
			DegreeRadianConverter conv = new DegreeRadianConverter();
			double newTheta = conv.degreesToRadians( getRockInX() );
			quat.setToXAxis( newTheta );
			// Apply the rocking rotation to the existing rotation
			tm.addRotationQuaternion( quat.x, quat.y, quat.z, quat.w, MSpace.Space.kTransform);
			// Let Maya know what the matrix should be
			return tm.asMatrixProperty;
		}

		public override MMatrix	asMatrix(double percent)
		{
			MPxTransformationMatrix m = new MPxTransformationMatrix(this);
			//	Apply the percentage to the matrix components
			MVector trans = m.translation();
			trans *= percent;
			m.translateTo( trans );
			MPoint rotatePivotTrans = m.rotatePivot();
			rotatePivotTrans = rotatePivotTrans * percent;
			m.setRotatePivot( rotatePivotTrans );
			MPoint scalePivotTrans = new MPoint(m.scalePivotTranslation());
			scalePivotTrans = scalePivotTrans * percent;
			m.setScalePivotTranslation( new MVector(scalePivotTrans));

			//	Apply the percentage to the rotate value.  Same
			// as above + the percentage gets applied
			MQuaternion quat = rotation();
			DegreeRadianConverter conv = new DegreeRadianConverter();
			double newTheta = conv.degreesToRadians( getRockInX() );
			quat.setToXAxis( newTheta );
			m.rotateBy( quat );
			MEulerRotation eulRotate = m.eulerRotation();
			m.rotateTo(  eulRotate.multiply(percent), MSpace.Space.kTransform);

			//	Apply the percentage to the scale
			MVector s = new MVector(scale(MSpace.Space.kTransform));
			s.x = 1.0 + (s.x - 1.0)*percent;
			s.y = 1.0 + (s.y - 1.0)*percent;
			s.z = 1.0 + (s.z - 1.0)*percent;
			m.scaleTo(s, MSpace.Space.kTransform);

			return m.asMatrix();
		}
		public override MMatrix asRotateMatrix()
		{
			return base.asRotateMatrix();
		}

		// Degrees
		public double getRockInX()
		{
			return rockXValue;
		}
		public void setRockInX(double rock)
		{
			rockXValue = rock;
		}
	}

	class rockingTransformCheckNode : MPxTransform
	{
		public const uint kRockingTransformCheckNodeID = 0x81071;
		static MTypeId id = new MTypeId(kRockingTransformCheckNodeID);
		protected static	MObject aRockInX;
		protected double rockXValue;

		public rockingTransformCheckNode():base()
		{
			rockXValue = 0;
		}
	   public  rockingTransformCheckNode(MPxTransformationMatrix matrx)
			: base(matrx)
		{
			rockXValue = 0;
		}
		~rockingTransformCheckNode()
		{

		}
		public override MPxTransformationMatrix createTransformationMatrix()
		{
			return new rockingTransformCheckMatrix() ;
		}
		public override void postConstructor()
		{
			//	Make sure the parent takes care of anything it needs.
			//
			base.postConstructor();

			// 	The baseTransformationMatrix pointer should be setup properly
			//	at this point, but just in case, set the value if it is missing.
			//
			if (null == baseTransformationMatrix)
			{
				MGlobal.displayWarning("NULL baseTransformationMatrix found!");
				baseTransformationMatrix = new rockingTransformCheckMatrix();
			}

			MPlug aRockInXPlug = new MPlug(thisMObject(), aRockInX);
		}
		public override void validateAndSetValue(MPlug plug, MDataHandle handle, MDGContext context)
		{
			//	Make sure that there is something interesting to process.
			//
			if (plug.isNull)
				throw new ArgumentNullException("plug");

			if (plug.equalEqual(aRockInX))
			{
				MDataBlock block = _forceCache(context);
				MDataHandle blockHandle = block.outputValue(plug);

				// Update our new rock in x value
				double rockInX = handle.asDouble;
				blockHandle.set(rockInX);
				rockXValue = rockInX;

				// Update the custom transformation matrix to the
				// right rock value.
				rockingTransformCheckMatrix ltm = getRockingTransformMatrix();
				if (ltm != null)
				{
					ltm.setRockInX(rockXValue);
				}
				else
				{
					MGlobal.displayError("Failed to get rock transform matrix");
				}

				blockHandle.setClean();

				// Mark the matrix as dirty so that DG information
				// will update.
				dirtyMatrix();

				return;
			}
			base.validateAndSetValue(plug, handle, context);
		}
		public override void resetTransformation(MPxTransformationMatrix matrix)
		{
			base.resetTransformation(matrix);
		}
		public override void resetTransformation(MMatrix matrix)
		{
			base.resetTransformation(matrix);
		}
					
		// Utility for getting the related rock matrix pointer
		public rockingTransformCheckMatrix getRockingTransformMatrix()
		{
			//This make sure the correct downcast.
			rockingTransformCheckMatrix ret = new rockingTransformCheckMatrix(baseTransformationMatrix);
			baseTransformationMatrix.Dispose();
			baseTransformationMatrix = ret;
			return ret;
		}
		new static public string className  ()
		{
			return "rockingTransformCheckCSharp";
		}
		protected override MEulerRotation applyRotationLocks(MEulerRotation toTest, MEulerRotation savedRotation)
		{
			return toTest;
		}
		protected override MEulerRotation applyRotationLimits(MEulerRotation unlimitedRotation, MDataBlock block)
		{
			//
			// For demonstration purposes we limit the rotation to 60
			// degrees and bypass the rotation limit attributes
			// 
			DegreeRadianConverter conv = new DegreeRadianConverter();
			double degrees = conv.radiansToDegrees( unlimitedRotation.x );
			if ( degrees < 60 )
				return unlimitedRotation;
			MEulerRotation euler = new MEulerRotation();
			euler.x = conv.degreesToRadians( 60.0 );
			return euler;
		}

		//
		//	Calls applyRotationLocks && applyRotationLimits
		//	This method verifies that the passed value can be set on the 
		//	rotate plugs. In the base class, limits as well as locking are
		//	checked by this method.
		//
		//	The compute, validateAndSetValue, and rotateTo functions
		//	all use this method.
		//
		protected override void checkAndSetRotation(MDataBlock block, MPlug plug, MEulerRotation newRotation, MSpace.Space space)
		{
			MDGContext context = block.context;
			updateMatrixAttrs(context);

			MEulerRotation outRotation = newRotation;
			if (context.isNormal) {
				//	For easy reading.
				//
				MPxTransformationMatrix xformMat = baseTransformationMatrix;

				//	Get the current translation in transform space for 
				//	clamping and locking.
				//
				MEulerRotation savedRotation = 
					xformMat.eulerRotation(MSpace.Space.kTransform);

				//	Translate to transform space, since the limit test needs the
				//	values in transform space. The locking test needs the values
				//	in the same space as the savedR value - which is transform 
				//	space as well.
				//
				baseTransformationMatrix.rotateTo(newRotation, space);

				outRotation = xformMat.eulerRotation(MSpace.Space.kTransform);

				//	Now that everything is in the same space, apply limits 
				//	and change the value to adhere to plug locking.
				//
				outRotation = applyRotationLimits(outRotation, block);
				outRotation = applyRotationLocks(outRotation, savedRotation);

				//	The value that remain is in transform space.
				//
				xformMat.rotateTo(outRotation, MSpace.Space.kTransform);

				//	Get the value that was just set. It needs to be in transform
				//	space since it is used to set the datablock values at the
				//	end of this method. Getting the vaolue right before setting
				//	ensures that the transformation matrix and data block will
				//	be synchronized.
				//
				outRotation = xformMat.eulerRotation(MSpace.Space.kTransform);
			} 
			else 
			{
				//	Get the rotation for clamping and locking. This will get the
				//	rotate value in transform space.
				//
                double[] s3 = block.inputValue(rotate).Double3;
				MEulerRotation savedRotation = new MEulerRotation(s3[0], s3[1], s3[2]);

				//	Create a local transformation matrix for non-normal context
				//	calculations.
				//
				MPxTransformationMatrix local = createTransformationMatrix();
				if (null == local)
				{
					throw new InvalidOperationException("rockingTransformCheck::checkAndSetRotation internal error");
				}

				//	Fill the newly created transformation matrix.
				//
				computeLocalTransformation(local, block);

				//	Translate the values to transform space. This will allow the 
				//	limit and locking tests to work properly.
				//
				local.rotateTo(newRotation, space);

				outRotation = local.eulerRotation(MSpace.Space.kTransform);

				//	Apply limits
				//
				outRotation = applyRotationLimits(outRotation, block);

				outRotation = applyRotationLocks(outRotation, savedRotation);

				local.rotateTo(outRotation, MSpace.Space.kTransform);

				//	Get the rotate value in transform space for placement in the
				//	data block.
				//
				outRotation = local.eulerRotation(MSpace.Space.kTransform);

				local.Dispose();
			}

			MDataHandle handle = block.outputValue(plug);

			if (plug.equalEqual(rotate)) {
				handle.set(outRotation.x, outRotation.y, outRotation.z);
			} else if (plug.equalEqual(rotateX)) {
				handle.set(outRotation.x);
			} else if (plug.equalEqual(rotateY)) {
				handle.set(outRotation.y);
			} else {
				handle.set(outRotation.z);
			}

			return;
		}   

		[MPxMatrixInitializer()]
		public static bool initialize()
		{
			MFnNumericAttribute numFn = new MFnNumericAttribute();
			aRockInX = numFn.create("RockInX", "rockx", MFnNumericData.Type.kDouble, 0.0);
			numFn.isKeyable = true;
			numFn.isAffectsWorldSpace = true;
			addAttribute(aRockInX);

			//	This is required so that the validateAndSet method is called
			mustCallValidateAndSet(aRockInX);
			return true;
		}
	}

	class DegreeRadianConverter
	{
		public double degreesToRadians( double degrees )
		{
			return degrees * (Math.PI / 180.0);
		}
		
		public double radiansToDegrees( double radians )
		{
			return radians * (180.0 / Math.PI);
		}
	}
 }
