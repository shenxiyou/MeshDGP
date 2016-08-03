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

[assembly: MPxDragAndDropBehavior("slopeShaderBehaviorCSharp", typeof(MayaNetPlugin.slopeShaderBehavior))]
[assembly: MPxNodeClass(typeof(MayaNetPlugin.slopeShaderNode), "slopeShaderNodeCSharp", 0x0008106a)]

namespace MayaNetPlugin
{
	public class slopeShaderBehavior : MPxDragAndDropBehavior
	{
		public slopeShaderBehavior()
		{
		}

		public override bool shouldBeUsedFor(MObject sourceNode, MObject destinationNode, MPlug sourcePlug, MPlug destinationPlug)
		{
			bool result = false;

			if(sourceNode.hasFn(MFn.Type.kLambert))
			{
				//if the source node was a lambert
				//than we will check the downstream connections to see 
				//if a slope shader is assigned to it.
				//
				MObject shaderNode = new MObject();
				MFnDependencyNode src = new MFnDependencyNode(sourceNode);
				MPlugArray connections = new MPlugArray();

				src.getConnections(connections);
				for(int i = 0; i < connections.length; i++)
				{
					//check the incoming connections to this plug
					//
					MPlugArray connectedPlugs = new MPlugArray();
					connections[i].connectedTo(connectedPlugs, true, false);
					for(int j = 0; j < connectedPlugs.length; j++)
					{
						//if the incoming node is a slope shader than 
						//set shaderNode equal to it and break out of the inner 
						//loop
						//
						if(new MFnDependencyNode(connectedPlugs[j].node).typeName == "slopeShaderNodeCSharp")
						{
							shaderNode = connectedPlugs[j].node;
							break;
						}
					}

					//if the shaderNode is not null
					//than we have found a slopeShaderNodeCSharp
					//
					if(!shaderNode.isNull)
					{
						//if the destination node is a mesh than we will take
						//care of this connection so set the result to true
						//and break out of the outer loop
						//
						if(destinationNode.hasFn(MFn.Type.kMesh))
							result = true;

						break;
					}
				}
			}
			if (new MFnDependencyNode(sourceNode).typeName == "slopeShaderNodeCSharp")
			//if the sourceNode is a slope shader than check what we
			//are dropping on to
			//
			{
				if(destinationNode.hasFn(MFn.Type.kLambert))
					result = true;
				else if (destinationNode.hasFn(MFn.Type.kMesh))
					result = true;
			}	

			return result;
		}
		//
		//	Description:
		//		Overloaded function from MPxDragAndDropBehavior
		//	this method will handle the connection between the slopeShaderNodeCSharp and the shader it is
		//	assigned to as well as any meshes that it is assigned to. 
		//
		public override void connectNodeToNode(MObject sourceNode, MObject destinationNode, bool force)
		{
			MFnDependencyNode src = new MFnDependencyNode(sourceNode);

			//if we are dragging from a lambert
			//we want to check what we are dragging
			//onto.
			if(sourceNode.hasFn(MFn.Type.kLambert))
			{
				//MObject shaderNode;
				MPlugArray connections = new MPlugArray();
				MObjectArray shaderNodes = new MObjectArray();
				shaderNodes.clear();

				//if the source node was a lambert
				//than we will check the downstream connections to see 
				//if a slope shader is assigned to it.
				//
				src.getConnections(connections);
				int i;
				for(i = 0; i < connections.length; i++)
				{
					//check the incoming connections to this plug
					//
					MPlugArray connectedPlugs = new MPlugArray();
					connections[i].connectedTo(connectedPlugs, true, false);
					for(uint j = 0; j < connectedPlugs.length; j++)
					{
						//if the incoming node is a slope shader than 
						//append the node to the shaderNodes array
						//
						MObject currentnode = connectedPlugs[i].node;
						if (new MFnDependencyNode(currentnode).typeName == "slopeShaderNodeCSharp")
						{
							shaderNodes.append(currentnode);
						}
					}
				}

				//if we found a shading node
				//than check the destination node 
				//type to see if it is a mesh
				//
				if(shaderNodes.length > 0)
				{
					MFnDependencyNode dest = new MFnDependencyNode(destinationNode);
					if(destinationNode.hasFn(MFn.Type.kMesh))
					{
						//if the node is a mesh than for each slopeShaderNodeCSharp
						//connect the worldMesh attribute to the dirtyShaderPlug
						//attribute to force an evaluation of the node when the mesh
						//changes
						//
						for(i = 0; i < shaderNodes.length; i++)
						{
							MPlug srcPlug = dest.findPlug("worldMesh");
							MPlug destPlug = new MFnDependencyNode(shaderNodes[i]).findPlug("dirtyShaderPlug");

							if(!srcPlug.isNull && !destPlug.isNull)
							{
								string cmd = "connectAttr -na ";
								cmd += srcPlug.name + " ";
								cmd += destPlug.name;
								
								try
								{
									// in slopeShaderBehavior.cpp, this may excute failed but continue on the following code, so we catch it.
									MGlobal.executeCommand(cmd);
								}
								catch (System.Exception)
								{
									MGlobal.displayError("ExcuteCommand (" + cmd + ") failed.");
								}
							}
						}

						//get the shading engine so we can assign the shader
						//to the mesh after doing the connection
						//
						MObject shadingEngine = findShadingEngine(sourceNode);

						//if there is a valid shading engine than make
						//the connection
						//
						if(!shadingEngine.isNull)
						{
							string cmd = "sets -edit -forceElement ";
							cmd += new MFnDependencyNode(shadingEngine).name + " ";
							cmd += new MFnDagNode(destinationNode).partialPathName;
							MGlobal.executeCommand(cmd);
						}
					}
				}
			}
			else if (src.typeName == "slopeShaderNodeCSharp")
			//if we are dragging from a slope shader
			//than we want to see what we are dragging onto
			//
			{
				if(destinationNode.hasFn(MFn.Type.kMesh))
				{
					//if the user is dragging onto a mesh
					//than make the connection from the worldMesh
					//to the dirtyShader plug on the slopeShaderNodeCSharp
					//
					MFnDependencyNode dest = new MFnDependencyNode(destinationNode);
					MPlug srcPlug = dest.findPlug("worldMesh");
					MPlug destPlug = src.findPlug("dirtyShaderPlug");
					if(!srcPlug.isNull && !destPlug.isNull)
					{
						string cmd = "connectAttr -na ";
						cmd += srcPlug.name + " ";
						cmd += destPlug.name;
						MGlobal.executeCommand(cmd);
					}
				}
			}

			return;
		}

		public override void connectNodeToAttr(MObject sourceNode, MPlug destinationPlug, bool force)
		//
		//	Description:
		//		Overloaded function from MPxDragAndDropBehavior
		//	this method will assign the correct output from the slope shader 
		//	onto the given attribute.
		//
		{
			MFnDependencyNode src = new MFnDependencyNode(sourceNode);

			//if we are dragging from a slopeShaderNodeCSharp
			//to a shader than connect the outColor
			//plug to the plug being passed in
			//
			if(destinationPlug.node.hasFn(MFn.Type.kLambert)) {
				if (src.typeName == "slopeShaderNodeCSharp")
				{
					MPlug srcPlug = src.findPlug("outColor");
					if(!srcPlug.isNull && !destinationPlug.isNull)
					{
						string cmd = "connectAttr ";
						cmd += srcPlug.name + " ";
						cmd += destinationPlug.name;
						MGlobal.executeCommand(cmd);
					}
				}
			} else {
				//in all of the other cases we do not need the plug just the node
				//that it is on
				//
				MObject destinationNode = destinationPlug.node;
				connectNodeToNode(sourceNode, destinationNode, force);
			}
		}

		public MObject findShadingEngine(MObject node)
		//
		//	Description:
		//		Given the material MObject this method will
		//	return the shading group that it is assigned to.
		//	if there is no shading group associated with
		//	the material than a null MObject is apssed back.
		//
		{
			MFnDependencyNode nodeFn = new MFnDependencyNode(node);
			MPlug srcPlug = nodeFn.findPlug("outColor");
			MPlugArray nodeConnections = new MPlugArray();
			srcPlug.connectedTo(nodeConnections, false, true);
			//loop through the connections
			//and find the shading engine node that
			//it is connected to
			//
			for(int i = 0; i < nodeConnections.length; i++)
			{
				if(nodeConnections[i].node.hasFn(MFn.Type.kShadingEngine))
					return nodeConnections[i].node;
			}

			//no shading engine associated so return a
			//null MObject
			//
			return new MObject();
		}
	}

    [MPxNodeAffects("aAngle", "aOutColor")]
	[MPxNodeAffects("aColor1", "aOutColor")]
    [MPxNodeAffects("aColor2", "aOutColor")]
    [MPxNodeAffects("aTriangleNormalCamera", "aOutColor")]
    [MPxNodeAffects("aDirtyShaderAttr", "aOutColor")]
	public class slopeShaderNode : MPxNode, IMPxNode
	{
		public static MObject aAngle = null;

		public static MObject aColor1 = null;

		public static MObject aColor2 = null;

		public static MObject aTriangleNormalCamera = null;

		public static MObject aMatrixEyeToWorld = null;

		public static MObject aOutColor = null;

		public static MObject aDirtyShaderAttr = null;

		public const float AWdegreesToRadians = 0.0174532925199432957692369076848f;
		public slopeShaderNode()
		{
			MGlobal.displayInfo("test slopshadernode");
		}

		public override void postConstructor()
		{
			_setMPSafe(true);
		}

        [MPxNodeInitializer()]
		public static void initialize()
		//
		//	Description:
		//		Initializes the attributes for this node.
		//
		{
			MFnNumericAttribute nAttr = new MFnNumericAttribute(); 
			MFnMatrixAttribute nMAttr = new MFnMatrixAttribute();
			//MFnTypedAttribute nTAttr;
			MFnGenericAttribute nGAttr = new MFnGenericAttribute();


			// Input Attributes
			//
			aAngle = nAttr.create("angle", "ang", MFnNumericData.Type.kFloat);
			nAttr.setDefault(30.0f);
			nAttr.setMin(0.0f);
			nAttr.setMax(100.0f);
			nAttr.isKeyable = true;
			nAttr.isStorable = true;
			nAttr.isReadable = true;
			nAttr.isWritable = true;

			aColor1 = nAttr.createColor("walkableColor", "w");
			nAttr.setDefault(0.0f, 1.0f, 0.0f);
			nAttr.isKeyable = true;
			nAttr.isStorable = true;
			nAttr.isUsedAsColor = true;
			nAttr.isReadable = true;
			nAttr.isWritable = true;

			aColor2 = nAttr.createColor("nonWalkableColor", "nw");
			nAttr.setDefault(1.0f, 0.0f, 0.0f);
			nAttr.isKeyable = true;
			nAttr.isStorable = true;
			nAttr.isUsedAsColor = true;
			nAttr.isReadable = true;
			nAttr.isWritable = true;


			// Surface Normal supplied by the render sampler
			
			aTriangleNormalCamera = nAttr.createPoint("triangleNormalCamera", "n");
			nAttr.isStorable = false;
			nAttr.isHidden = true;
			nAttr.isReadable = true;
			nAttr.isWritable = true;

			//View matrix from the camera into world space

			aMatrixEyeToWorld = nMAttr.create("matrixEyeToWorld", "mew",
												MFnMatrixAttribute.Type.kFloat);
			nAttr.isHidden = true;
			nMAttr.isWritable = true;

			// Output Attributes

			aOutColor = nAttr.createColor("outColor", "oc");
			nAttr.isStorable = false;
			nAttr.isHidden = false;
			nAttr.isReadable = true;
			nAttr.isWritable = false;

			//dummy plug for forcing evaluation

			aDirtyShaderAttr = nGAttr.create("dirtyShaderPlug", "dsp");
			nGAttr.isArray = true;
			nGAttr.isHidden = false;
			nGAttr.usesArrayDataBuilder = true;
			nGAttr.isReadable = false;
			nGAttr.isStorable = true;
			nGAttr.indexMatters = false;
			nGAttr.addAccept(MFnData.Type.kMesh);

			//Add attribues
			addAttribute(aAngle);
			addAttribute(aColor1);
			addAttribute(aColor2);
			addAttribute(aTriangleNormalCamera);
			addAttribute(aOutColor);
			addAttribute(aMatrixEyeToWorld);
			addAttribute(aDirtyShaderAttr);

			attributeAffects(aAngle, aOutColor);
			attributeAffects(aColor1, aOutColor);
			attributeAffects(aColor2, aOutColor);
			attributeAffects(aTriangleNormalCamera, aOutColor);
			attributeAffects(aDirtyShaderAttr, aOutColor);
		}

		public override bool compute(MPlug plug, MDataBlock dataBlock)
		//
		//	Description:
		//		Computes a color value  
		//	from a surface noraml angle.
		//
		{
			if ((plug.notEqual(aOutColor)) && (plug.parent.notEqual(aOutColor)))
				return false;

			MFloatVector resultColor;

			MFloatVector  walkable = dataBlock.inputValue( aColor1 ).asFloatVector;
			MFloatVector  nonWalkable = dataBlock.inputValue( aColor2 ).asFloatVector;
			MFloatVector  surfaceNormal = dataBlock.inputValue( aTriangleNormalCamera ).asFloatVector;
			MFloatMatrix  viewMatrix = dataBlock.inputValue( aMatrixEyeToWorld ).asFloatMatrix;
			float angle = dataBlock.inputValue( aAngle ).asFloat;

			// Normalize the view vector
			//
			surfaceNormal.normalize();
			MFloatVector WSVector = surfaceNormal.multiply(viewMatrix);
	
			// find dot product
			//
			float scalarNormal = WSVector.multiply(new MFloatVector(0, 1, 0));

			// take the absolute value
			//
			if (scalarNormal < 0.0) 
				scalarNormal *= -1.0f;

			if(Math.Cos(angle*AWdegreesToRadians) < scalarNormal)
				resultColor = walkable;
			else
				resultColor = nonWalkable;

			// set ouput color attribute
			//
			MDataHandle outColorHandle = dataBlock.outputValue( aOutColor );
			MFloatVector outColor = outColorHandle.asFloatVector;
			outColor = resultColor;
			outColorHandle.setClean();

			return true;
		}
	}

}
