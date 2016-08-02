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
//	Example: geometrySurfaceConstraint
//	This example demonstrates how to use the MPxConstraint and MPxConstraintCommand classes to
//	create a geometry constraint. This type of constraint will keep the constrained object attached
//	to the target as the target is moved. The constrained object can be constrained to one of
//	multiple targets. You can choose to constrain to the target of the highest or lowest weight.
//

using System;
using System.Windows.Forms;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaAnim;

[assembly: MPxConstraintCommandClass(typeof(MayaNetPlugin.GeometrySurfaceConstraintCommand), "geometrySurfaceConstraintCmdCSharp")]
[assembly: MPxNodeClass(typeof(MayaNetPlugin.GeometrySurfaceConstraint), "geometrySurfaceConstraintCSharp",
    MayaNetPlugin.GeometrySurfaceConstraint.id, NodeType = Autodesk.Maya.OpenMaya.MPxNode.NodeType.kConstraintNode)]

namespace MayaNetPlugin 
{
    [MPxCommandSyntaxFlag(GeometrySurfaceConstraintCommand.kConstrainToLargestWeightFlag, kConstrainToLargestWeightFlagLong)]
    [MPxCommandSyntaxFlag(GeometrySurfaceConstraintCommand.kConstrainToSmallestWeightFlag, kConstrainToSmallestWeightFlagLong)]
	public class GeometrySurfaceConstraintCommand : MPxConstraintCommand, IMPxCommand
	{
		const string kConstrainToLargestWeightFlag = "-lw";
		const string kConstrainToLargestWeightFlagLong = "-largestWeight";
		const string kConstrainToSmallestWeightFlag = "-sw";
		const string kConstrainToSmallestWeightFlagLong = "-smallestWeight";

		public enum ConstraintType{kLargestWeight, kSmallestWeight};
		protected ConstraintType weightType;

		public GeometrySurfaceConstraintCommand(){}

		public override bool doIt(MArgList argList)
		{
			parseArgs(argList);
			return false; // we're not done yet
		}
		protected override MTypeId constraintTypeId()
		{
			return new MTypeId(GeometrySurfaceConstraint.id);
		}
		protected override MPxConstraintCommand.TargetType targetType()
		{
			return MPxConstraintCommand.TargetType.kGeometryShape;
		}
		protected override MObject constraintInstancedAttribute()
		{
			return GeometrySurfaceConstraint.constraintParentInverseMatrix;
		}
		protected override MObject constraintOutputAttribute()
		{
			return GeometrySurfaceConstraint.constraintGeometry;
		}
		protected override MObject constraintTargetInstancedAttribute()
		{
			return GeometrySurfaceConstraint.targetGeometry;
		}
		protected override MObject constraintTargetAttribute()
		{
			return GeometrySurfaceConstraint.compoundTarget;
		}
		protected override MObject constraintTargetWeightAttribute()
		{
			return GeometrySurfaceConstraint.targetWeight;
		}

		protected override MObject objectAttribute()
		{
			return MPxTransform.geometry;
		}

		// The implementation here is a bit different from its counterpart C++ sample because
		// .NET SDK decides not to support the obsolete C++ API:
		//
		// bool connectTargetAttribute (void *opaqueTarget, int index, MObject &constraintAttr);
		//
		protected override bool connectTarget( MDagPath targetPath, int index )
		{
			try
			{
				MObject targetObject = new MObject(targetPath.node);
				MFnDagNode targetDagNode = new MFnDagNode(targetObject);
				connectTargetAttribute(targetPath, index, targetDagNode.attribute("worldMesh"),
					GeometrySurfaceConstraint.targetGeometry);
			}
			catch (Exception)
			{
				MGlobal.displayError("Failed to connect target.");
				return false;
			}
			return true;
		}

		protected override bool connectObjectAndConstraint(MDGModifier modifier)
		{
			MObject transform = transformObject();
			if ( transform.isNull )
			{
				throw new InvalidOperationException("Failed to get transformObject()");
			}

			MFnTransform transformFn = new MFnTransform(transform);
			MVector translate = transformFn.getTranslation(MSpace.Space.kTransform);
			MPlug translatePlug = transformFn.findPlug("translate");

			if (MPlug.FreeToChangeState.kFreeToChange == translatePlug.isFreeToChange())
			{
				MFnNumericData nd = new MFnNumericData();

				MObject translateData = nd.create(MFnNumericData.Type.k3Double);
				nd.setData3Double(translate.x, translate.y, translate.z);
				modifier.newPlugValue(translatePlug, translateData);
				connectObjectAttribute(MPxTransform.geometry, GeometrySurfaceConstraint.constraintGeometry, false);
			}

			connectObjectAttribute(MPxTransform.parentInverseMatrix, GeometrySurfaceConstraint.constraintParentInverseMatrix, true, true);

			return true; 
		}

		protected override void createdConstraint(MPxConstraint constraint)
		{
			if ( constraint != null )
			{
				GeometrySurfaceConstraint c = (GeometrySurfaceConstraint) constraint;
				c.weightType = weightType;
			}
			else
			{
				MGlobal.displayError("Failed to get created constraint.");
			}
		}
		protected override bool parseArgs( MArgList argList)
		{
			MArgDatabase argData;
			argData = new MArgDatabase(_syntax, argList);

			// Settings only work at creation time. Would need an
			// attribute on the node in order to push this state
			// into the node at any time.
			ConstraintType typ;
			if (argData.isFlagSet(kConstrainToLargestWeightFlag))
				typ = GeometrySurfaceConstraintCommand.ConstraintType.kLargestWeight;
			else if (argData.isFlagSet(kConstrainToSmallestWeightFlag))
				typ = GeometrySurfaceConstraintCommand.ConstraintType.kSmallestWeight;
			else
				typ = GeometrySurfaceConstraintCommand.ConstraintType.kLargestWeight;
			weightType = typ;

			// Need parent to process
			return false;
		}
	}

	class GeometrySurfaceConstraint : MPxConstraint, IMPxNode
	{
		public static MObject compoundTarget = null;
		public static MObject targetGeometry = null;
		public static MObject targetWeight = null;
		public static MObject constraintParentInverseMatrix = null;
		public static MObject constraintGeometry = null;
		public const uint id = 0x00081061;

		public GeometrySurfaceConstraintCommand.ConstraintType weightType;
		public GeometrySurfaceConstraint()
		{
			weightType = GeometrySurfaceConstraintCommand.ConstraintType.kLargestWeight;
		}
		public override void postConstructor() {}
		public override bool compute( MPlug plug, MDataBlock block)
		{
			if ( plug.equalEqual(constraintGeometry) )
			{
				//
				block.inputValue(constraintParentInverseMatrix);
				//
				MArrayDataHandle targetArray = block.inputArrayValue( compoundTarget );
				uint targetArrayCount = targetArray.elementCount();
				double weight,selectedWeight = 0;
				if ( weightType == GeometrySurfaceConstraintCommand.ConstraintType.kSmallestWeight )
					selectedWeight = float.MaxValue;
				MObject selectedMesh = null;
				uint i;
				for ( i = 0; i < targetArrayCount; i++ )
				{
					MDataHandle targetElement = targetArray.inputValue();
					weight = targetElement.child(targetWeight).asDouble;
					if ( !equivalent(weight,0.0))
					{
						if ( weightType == GeometrySurfaceConstraintCommand.ConstraintType.kLargestWeight )
						{
							if ( weight > selectedWeight )
							{
								MObject mesh = targetElement.child(targetGeometry).asMesh;
								if ( !mesh.isNull )
								{
									selectedMesh = mesh;
									selectedWeight =  weight;
								}
							}
						}
						else
						{
							if  ( weight < selectedWeight )
							{
								MObject mesh = targetElement.child(targetGeometry).asMesh;
								if ( !mesh.isNull )
								{
									selectedMesh = mesh;
									selectedWeight =  weight;
								}
							}
						}
					}
					targetArray.next();
				}
				//
				if( selectedMesh == null )
				{
					block.setClean(plug);
				}
				else
				{
					// The transform node via the geometry attribute will take care of
					// updating the location of the constrained geometry.
					MDataHandle outputConstraintGeometryHandle = block.outputValue(constraintGeometry);
					outputConstraintGeometryHandle.setMObject(selectedMesh);
				}
			} 
			else 
			{
				return false;
			}

			return true;
		}
		public override MObject weightAttribute()
		{
			return GeometrySurfaceConstraint.targetWeight;
		}
		public override MObject targetAttribute()
		{
			return GeometrySurfaceConstraint.compoundTarget;
		}

		protected override void getOutputAttributes(MObjectArray attributeArray)
		{
			attributeArray.clear();
			attributeArray.append(GeometrySurfaceConstraint.constraintGeometry);
		}

        [MPxNodeInitializer()]
		public static void initialize()
		{
			// constraint attributes

			{	// Geometry: mesh, readable, not writable, delete on disconnect
				MFnTypedAttribute typedAttrNotWritable = new MFnTypedAttribute();
				GeometrySurfaceConstraint.constraintGeometry = typedAttrNotWritable.create( "constraintGeometry", "cg", MFnData.Type.kMesh); 	
				typedAttrNotWritable.isReadable = true;
				typedAttrNotWritable.isWritable = false;
				typedAttrNotWritable.disconnectBehavior = MFnAttribute.DisconnectBehavior.kDelete;
			}
			{	// Parent inverse matrix: delete on disconnect
				MFnTypedAttribute typedAttr = new MFnTypedAttribute();
				GeometrySurfaceConstraint.constraintParentInverseMatrix = typedAttr.create( "constraintPim", "ci", MFnData.Type.kMatrix); 	
				typedAttr.disconnectBehavior = MFnAttribute.DisconnectBehavior.kDelete;

				// Target geometry: mesh, delete on disconnect
				GeometrySurfaceConstraint.targetGeometry = typedAttr.create( "targetGeometry", "tg", MFnData.Type.kMesh); 	
				typedAttr.disconnectBehavior = MFnAttribute.DisconnectBehavior.kDelete;
			}
			{	// Target weight: double, min 0, default 1.0, keyable, delete on disconnect
				MFnNumericAttribute typedAttrKeyable = new MFnNumericAttribute();
				GeometrySurfaceConstraint.targetWeight = typedAttrKeyable.create( "weight", "wt", MFnNumericData.Type.kDouble, 1.0);
				typedAttrKeyable.setMin( (double) 0 );
				typedAttrKeyable.isKeyable = true;
				typedAttrKeyable.disconnectBehavior = MFnAttribute.DisconnectBehavior.kDelete;
			}
			{	// Compound target(geometry,weight): array, delete on disconnect
				MFnCompoundAttribute compoundAttr = new MFnCompoundAttribute();
				GeometrySurfaceConstraint.compoundTarget = compoundAttr.create( "target", "tgt");
				compoundAttr.addChild(GeometrySurfaceConstraint.targetGeometry);
				compoundAttr.addChild(GeometrySurfaceConstraint.targetWeight);
				compoundAttr.isArray = true;
				compoundAttr.disconnectBehavior = MFnAttribute.DisconnectBehavior.kDelete;
			}

			addAttribute(GeometrySurfaceConstraint.constraintParentInverseMatrix);
			addAttribute(GeometrySurfaceConstraint.constraintGeometry);
			addAttribute(GeometrySurfaceConstraint.compoundTarget);

			attributeAffects(compoundTarget, constraintGeometry);
			attributeAffects(targetGeometry, constraintGeometry);
			attributeAffects(targetWeight, constraintGeometry);
			attributeAffects(constraintParentInverseMatrix, constraintGeometry);
		}

		public static bool equivalent(double a, double b)
		{
			return Math.Abs(a - b) < .000001;
		}
	}
}


