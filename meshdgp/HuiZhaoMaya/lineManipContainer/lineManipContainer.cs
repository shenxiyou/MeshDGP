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
using Autodesk.Maya.OpenMayaRender;

using System.Runtime.InteropServices;

//
// This example is based on the lineManip.cpp example but
// incorporates a manip container so that two lines can
// be created as children.  The left line will change
// translateX of the selected nodes.  The right line
// will change scaleX of the selected node.  

/*
	 To show this example using MEL, run the following after loading the plug-in:

	lineManipContainerContextCSharp lineManipContainerContext1;
	setParent Shelf1;
	toolButton -cl toolCluster
				-i1 "moveManip.xpm"
				-t lineManipContainerContext1
				lineManipContainer1;

	If the preceding commands were used to create the manipulator context, 
	the following commands can destroy it:

		deleteUI lineManipContainerContext1;
		deleteUI lineManipContainer1;

*/
[assembly: MPxNodeClass(typeof(MayaNetPlugin.containerLineManip), "containerLineManipCSharp", 0x00081065, NodeType = MPxNode.NodeType.kManipulatorNode)]
[assembly: MPxNodeClass(typeof(MayaNetPlugin.lineManipContainer), "lineManipContainerCSharp", 0x0008106e, NodeType = MPxNode.NodeType.kManipContainer)]

[assembly: MPxContextCommandClass("lineManipContainerContextCSharp", typeof(MayaNetPlugin.lineManipContainerContext))]

namespace MayaNetPlugin
{
	class containerLineManip : MPxManipulatorNode
	{

		class planeMath
		{
			public planeMath()
			{
				a = b = c = d = 0.0;
			}
			public bool setPlane(MPoint pointOnPlane, MVector normalToPlane)
			{
				MVector _normalToPlane = new MVector(normalToPlane);
				_normalToPlane.normalize();

				// Calculate a,b,c,d based on input
				a = _normalToPlane.x;
				b = _normalToPlane.y;
				c = _normalToPlane.z;
				d = -(a * pointOnPlane.x + b * pointOnPlane.y + c * pointOnPlane.z);
				return true;
			}
			public bool intersect(MPoint linePoint, MVector lineDirection, ref MPoint intersectPoint)
			{
				double denominator = a * lineDirection.x + b * lineDirection.y + c * lineDirection.z;

				// Verify that the vector and the plane are not parallel.
				if (denominator < .00001)
				{
					return false;
				}

				double t = -(d + a * linePoint.x + b * linePoint.y + c * linePoint.z) / denominator;

				// Calculate the intersection point.
				intersectPoint = linePoint + t * lineDirection;

				return true;
			}
			public double a, b, c, d;
		}

		class lineMath
		{
			public lineMath()
			{
				point = new MPoint();
				direction = new MVector();
			}
			public bool setLine(MPoint linePoint, MVector lineDirection)
			{
				point.assign(linePoint);
				direction.assign(lineDirection);
				direction.normalize();
				return true;
			}
			public bool closestPoint(MPoint toPoint, ref MPoint closest)
			{
				double t = direction.x * (toPoint.x - point.x) +
							direction.y * (toPoint.y - point.y) +
							direction.z * (toPoint.z - point.z);
				closest = point + (direction * t);
				return true;
			}
			public MPoint point;
			public MVector direction;
		}

		class lineGeometry
		{
			public static MPoint topPoint(bool rightLine = true)
			{
				if (rightLine)
				{
					return new MPoint(1.0f, 1.0f, 0.0f);
				}
				return new MPoint(-1.0f, 1.0f, 0.0f);
			}

			public static MPoint bottomPoint(bool rightLine = true)
			{
				if (rightLine)
				{
					return new MPoint(1.0f, -1.0f, 0.0f);
				}
				return new MPoint(-1.0f, -1.0f, 0.0f);
			}

			public static MPoint otherPoint()
			{
				return new MPoint(2.0f, -1.0f, 0.0f);
			}
		};

		static double minOfThree(double a, double b, double c)
		{
			if (a < b && a < c)
				return a;
			if (b < a && b < c)
				return b;
			return c;
		}

		static double maxOfThree(double a, double b, double c)
		{
			if (a > b && a > c)
				return a;
			if (b > a && b > c)
				return b;
			return c;
		}

		static double degreesToRadians(double degrees)
		{
			return degrees * (Math.PI / 180.0);
		}

		static double radiansToDegrees(double radians)
		{
			return radians * (180.0 / Math.PI);
		}

		// GL component name for drawing and picking
		private uint lineName;
		// Simple class for plane creation, intersection. Although
		// the manipulator is just a line we want it to move
		// within a plane
		private planeMath plane;
		// Modified mouse position used for updating manipulator
		private MPoint mousePointGlName;

		// Manipulator changes behavior based on the setting
		// of these two booleans
		public bool affectScale;
		public bool affectTranslate;

		public containerLineManip()
		{
			plane = new planeMath();
			mousePointGlName = new MPoint();

			// Setup the plane with a point on the
			// plane along with a normal
			MPoint pointOnPlane = lineGeometry.topPoint();
			// Normal = cross product of two vectors on the plane
			MVector _topPoint = new MVector(lineGeometry.topPoint());
			MVector _bottomPoint = new MVector(lineGeometry.bottomPoint());
			MVector _otherPoint = new MVector(lineGeometry.otherPoint());

			MVector normalToPlane = (_topPoint - _otherPoint).crossProduct(_otherPoint - _bottomPoint);
			// Necessary to normalize
			normalToPlane.normalize();
			// Plane defined by a point and a normal
			plane.setPlane(pointOnPlane, normalToPlane);

			//default case
			affectScale = affectTranslate = false;           
		}

		~containerLineManip()
		{
			// No-op
		}

		public override void draw(M3dView view, MDagPath path, M3dView.DisplayStyle style, M3dView.DisplayStatus status)
		{
			// Are we in the right view
			MDagPath dpath = new MDagPath();
			view.getCamera(dpath);
			MFnCamera viewCamera = new MFnCamera(dpath);
			string nameBuffer = viewCamera.name;
			if (nameBuffer == null)
				return;
			if (nameBuffer.IndexOf("persp") == -1 && nameBuffer.IndexOf("front") == -1)
				return;
			//
			bool rightLine = !affectTranslate;

			// Populate the point arrays which are in local space
			MPoint top = lineGeometry.topPoint();
			MPoint bottom = lineGeometry.bottomPoint();

			// Depending on what's active, we modify the
			// end points with mouse deltas in local
			// space
			uint active = 0;
			try
			{
				glActiveName(ref active);
			}
			catch (System.Exception)
			{
				return;
			}

			if (active == lineName && active != 0)
			{
				top[0] += (float)mousePointGlName.x; top[1] += (float)mousePointGlName.y; top[2] += (float)mousePointGlName.z;
				bottom[0] += (float)mousePointGlName.x; bottom[1] += (float)mousePointGlName.y; bottom[2] += (float)mousePointGlName.z;
			}

			// Begin the drawing
			view.beginGL();

			// Get the starting value of the pickable items
			uint glPickableItem = 0;
			glFirstHandle(ref glPickableItem);

			// Top
			lineName = glPickableItem;
			// Place before you draw the manipulator component that can
			// be pickable.
			colorAndName(view, glPickableItem, true, mainColor());
			OpenGL.glBegin((uint)libOpenMayaRenderNet.MGL_LINES);
			OpenGL.glVertex3d(top.x, top.y, top.z);
			OpenGL.glVertex3d(bottom.x, bottom.y, bottom.z);
			OpenGL.glEnd();

			// End the drawing
			view.endGL();
		}

		public override bool doPress(M3dView view)
		{
			// Reset the mousePoint information on
			// a new press
			mousePointGlName.assign(MPoint.origin);
			updateDragInformation();
			return true;
		}

		public override bool doDrag(M3dView view)
		{
			updateDragInformation();
			return true;
		}

		public override bool doRelease(M3dView view)
		{
			// Scale nodes on the selection list.
			// Simple implementation that does not
			// support undo.
			MSelectionList list = new MSelectionList();
			MGlobal.getActiveSelectionList(list);
			MObject node = new MObject();
			for (MItSelectionList iter = new MItSelectionList(list); !iter.isDone; iter.next())
			{
				iter.getDependNode(node);
				MFnTransform xform;
				try
				{
					xform = new MFnTransform(node);
				}
				catch (System.Exception)
				{
					continue;
				}

				double[] newScale = new double[3];
				newScale[0] = mousePointGlName.x + 1;
				newScale[1] = mousePointGlName.y + 1;
				newScale[2] = mousePointGlName.z + 1;
				xform.setScale(newScale);
			}
			return true;
		}

		public void updateDragInformation()
		{
			// Find the mouse point in local space
			MPoint localMousePoint = new MPoint();
			MVector localMouseDirection = new MVector();
			try
			{
				mouseRay(localMousePoint, localMouseDirection);
			}
			catch (System.Exception)
			{
				return;
			}

			// Find the intersection of the mouse point with the
			// manip plane
			MPoint mouseIntersectionWithManipPlane = new MPoint();
			if (!plane.intersect(localMousePoint, localMouseDirection, ref mouseIntersectionWithManipPlane))
				return;

			mousePointGlName.assign(mouseIntersectionWithManipPlane);

			uint active = 0;
			try
			{
				glActiveName(ref active);
			}
			catch (System.Exception)
			{
				return;
			}

			if (active == lineName && active != 0)
			{
				lineMath line = new lineMath();

				//
				bool rightLine = true;
				if (affectTranslate)
					rightLine = false;

				// Find a vector on the plane
				MPoint a = lineGeometry.topPoint(rightLine);
				MPoint b = lineGeometry.bottomPoint(rightLine);

				MVector vab = a.minus(b);
				// Define line with a point and a vector on the plane
				line.setLine(a, vab);
				MPoint cpt = new MPoint();
				// Find the closest point so that we can get the
				// delta change of the mouse in local space
				if (line.closestPoint(mousePointGlName, ref cpt))
				{
					mousePointGlName.x -= cpt.x;
					mousePointGlName.y -= cpt.y;
					mousePointGlName.z -= cpt.z;
				}
			}
		}
	}
   
	class lineManipContainerContext : MPxSelectionContext
	{
		public lineManipContainerContext()
		{
			_setTitleString("Plug-in manipulator: lineManipContainerContext");
		}
		~lineManipContainerContext() {}

		public override void toolOnSetup(MEvent arg0)
		{
			_setHelpString("Move the object using the manipulator");

			//Todo after message implement
			updateManipulators();
			try
			{
				MModelMessage.ActiveListModified += updateManipulatorsFunc;
			}
			catch (System.Exception)
			{
				MGlobal.displayError("Model addCallback failed");
			}

			base.toolOnSetup(arg0);
		}

		//! Removes the callback
		public override void toolOffCleanup()
		{
			try
			{
			MModelMessage.ActiveListModified -= updateManipulatorsFunc;
			}
			catch (System.Exception)
			{
				MGlobal.displayError("Model remove callback failed");
			}
			
			base.toolOffCleanup();
		}

		/*! Override to set manipulator initial state.

		\note manipulatorClassPtr and firstObjectSelected will be set on
		entry. manipulatorClassPtr is the manipulator created and
		firstObjectSelected can be used to position the manipulator in the
		correct position.
		*/
		public void setInitialState()
		{
			// No-op in default state
		}

		//! Ensure that valid geometry is selected
		bool validGeometrySelected()
		{
			MSelectionList list = new MSelectionList();
			MGlobal.getActiveSelectionList(list);
			MItSelectionList iter = new MItSelectionList(list, MFn.Type.kInvalid);

			for (; !iter.isDone; iter.next())
			{
				MObject dependNode = new MObject();
				iter.getDependNode(dependNode);
				if (dependNode.isNull || !dependNode.hasFn(MFn.Type.kTransform))
				{
					MGlobal.displayWarning("Object in selection list is not right type of node");
					return false;
				}

				MFnDependencyNode dependNodeFn = new MFnDependencyNode(dependNode);
				MStringArray attributeNames = new MStringArray();
				attributeNames.append("scaleX");
				attributeNames.append("translateX");

				int i;
				for ( i = 0; i < attributeNames.length; i++ )
				{
					MPlug plug = dependNodeFn.findPlug(attributeNames[i]);
					if ( plug.isNull )
					{
						MGlobal.displayWarning("Object cannot be manipulated: " +
							dependNodeFn.name);
						return false;
					}
				}
			}
			return true;
		}

		/*!
		  Callback that creates the manipulator if valid geometry is
		  selected. Also removes the manipulator if no geometry is
		  selected. Handles connecting the manipulator to multiply
		  selected nodes.

		  \param[in] data Pointer to the current context class.
		*/
		void updateManipulators()
		{
			deleteManipulators();

			if ( ! validGeometrySelected() )
			{
				return;
			}

			// Clear info
			owner = null;
			firstObjectSelected = MObject.kNullObj;

			MSelectionList list = new MSelectionList();
			MGlobal.getActiveSelectionList(list);
			MItSelectionList iter = new MItSelectionList(list, MFn.Type.kInvalid);

			string manipName = "lineManipContainerCSharp";
			MObject manipObject = new MObject();
			lineManipContainer manipulator = MPxManipContainer.newManipulator(manipName, manipObject) as lineManipContainer;

			if (null != manipulator)
			{
				// Save state
				owner = manipulator;
				// Add the manipulator
				addManipulator(manipObject);
				//
				for (; !iter.isDone; iter.next())
				{
					MObject dependNode = new MObject();
					iter.getDependNode(dependNode);
					MFnDependencyNode dependNodeFn = new MFnDependencyNode(dependNode);
					// Connect the manipulator to the object in the selection list.
					manipulator.connectToDependNode(dependNode);
					//
					if (MObject.kNullObj == firstObjectSelected)
					{
						firstObjectSelected = dependNode;
					}
				}

				// Allow the manipulator to set initial state
				setInitialState();
			}
		}

		public void updateManipulatorsFunc(Object sender, MBasicFunctionArgs args)
		{
			updateManipulators();
		}

		protected MObject firstObjectSelected;
		protected lineManipContainer owner;
	}

	class lineManipContainerContextCommand :  MPxCommand, IMPxCommand
	{
		override public void doIt(MArgList args)
		{
			Console.WriteLine("lineManipContainerContextCommand...");

			return;
		}
	}
	//
	// Manipulator container which will hold two lineManip nodes
	//
	class lineManipContainer : MPxManipContainer
	{
		public lineManipContainer()
		{
		}
		~lineManipContainer()
		{}

		// Important virtuals to implement
		public unsafe override void createChildren()
		{
			MPxManipulatorNode proxyManip = null;

			String manipTypeName = "containerLineManipCSharp";
			String manipName = "rightLineManip";
			addMPxManipulatorNode(manipTypeName, manipName, out proxyManip);

			if (proxyManip is containerLineManip)
			{
				((containerLineManip)proxyManip).affectScale = true;
			}

			manipTypeName = "containerLineManipCSharp";
			manipName = "leftLineManip";
			addMPxManipulatorNode( manipTypeName, manipName, out proxyManip );

			if (proxyManip is containerLineManip)
			{
				((containerLineManip)proxyManip).affectTranslate = true;
			}

			return;    
		}

		public override void connectToDependNode(MObject node)
		{
		}

		public override void draw(M3dView view, MDagPath path, M3dView.DisplayStyle style, M3dView.DisplayStatus status)
		{
			base.draw(view, path, style, status);

			//
			view.beginGL(); 

			//MPoint textPos = new MPoint(nodeTranslation());
			MPoint textPos = new MPoint(0,0,0);
			String distanceText = "Two custom line manipulators";
			view.drawText(distanceText, textPos, M3dView.TextPosition.kLeft);

			// 
			view.endGL();
		}

		// Standard required methods
        [MPxNodeInitializer()]
		new public static void initialize()
		{
			MPxManipContainer.initialize();
		}
	}
}