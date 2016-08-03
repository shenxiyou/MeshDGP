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
using System.Runtime.InteropServices;

 
using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaRender;
using Autodesk.Maya.OpenMayaUI;

/*

// To show this example using MEL, run the following after loading the plug-in:
lineManipCmdCSharp -create;

// To delete the manipulator using MEL:
lineManipCmdCSharp -delete;

*/

[assembly: MPxNodeClass(typeof(MayaNetPlugin.lineManip), "simpleLineManipCSharp", 0x00081064, NodeType = Autodesk.Maya.OpenMaya.MPxNode.NodeType.kManipulatorNode)]
[assembly: MPxCommandClass(typeof(MayaNetPlugin.lineManipCmd), "lineManipCmdCSharp")]

namespace MayaNetPlugin
{
	class lineManip : MPxManipulatorNode
	{
		public const double M_PI = 3.14159265358979323846;

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
			public double a,b,c,d;
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
			public static MPoint topPoint()
			{
				return new MPoint(1.0f, 1.0f, 0.0f);
			}

			public static MPoint bottomPoint()
			{
				return new MPoint(1.0f, -1.0f, 0.0f);
			}

			public static MPoint otherPoint()
			{
				return new MPoint(2.0f, -1.0f, 0.0f);
			}
		};

		static double minOfThree( double a, double b, double c )
		{
			if ( a < b && a < c )
				return a;
			if ( b < a && b < c )
				return b;
			return c;
		}

		static double maxOfThree( double a, double b, double c )
		{
			if ( a > b && a > c )
				return a;
			if ( b > a && b > c )
				return b;
			return c;
		}

		static double degreesToRadians( double degrees )
		{
			 return degrees * ( M_PI/ 180.0 );
		}

		static double radiansToDegrees( double radians )
		{
			return radians * (180.0/M_PI);
		}

		// GL component name for drawing and picking
		private uint lineName;
		// Simple class for plane creation, intersection. Although
		// the manipulator is just a line we want it to move
		// within a plane
		private planeMath plane;
		// Modified mouse position used for updating manipulator
		private MPoint mousePointGlName;

		public lineManip()
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
			plane.setPlane( pointOnPlane, normalToPlane );
		}

		~lineManip()
		{
			// No-op
		}

		public override void draw(M3dView view, MDagPath path, M3dView.DisplayStyle style, M3dView.DisplayStatus status)
		{
			// Initialize GL function table

			// Are we in the right view
			MDagPath dpath = new MDagPath();
			view.getCamera(dpath);
			MFnCamera viewCamera = new MFnCamera(dpath);
			string nameBuffer = viewCamera.name;
			if (nameBuffer == null)
				return;
			if (nameBuffer.IndexOf("persp") == -1 && nameBuffer.IndexOf("front") == -1)
				return;

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

			if (active == lineName && active != 0 )
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
			OpenGL.glBegin((uint)OpenGL.GL_LINES);
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
			for (MItSelectionList iter = new MItSelectionList( list ); !iter.isDone; iter.next()) 
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
				// Find a vector on the plane
				MPoint a = lineGeometry.topPoint();
				MPoint b = lineGeometry.bottomPoint();
				MVector vab = a.minus(b);
				// Define line with a point and a vector on the plane
				line.setLine(a, vab);
				MPoint cpt = new MPoint();
				// Find the closest point so that we can get the
				// delta change of the mouse in local space
				if (line.closestPoint(mousePointGlName, ref cpt ))
				{
					mousePointGlName.x -= cpt.x;
					mousePointGlName.y -= cpt.y;
					mousePointGlName.z -= cpt.z;
				}
			}
		}
	}

    [MPxCommandSyntaxFlag("-c", "-create")]
    [MPxCommandSyntaxFlag("-d", "-delete")]
	class lineManipCmd : MPxCommand, IUndoMPxCommand, IMPxCommand
	{
		const string kCreateFlag = "-create";
		const string kDeleteFlag = "-delete";
		static private MObject lineManipObj;
		MDagModifier modifier;

		public lineManipCmd()
		{
			modifier = new MDagModifier();
		}

		public override void doIt(MArgList args)
		{
			MArgDatabase argData = new MArgDatabase(syntax, args);

			bool creating = true;
			if ( argData.isFlagSet( kCreateFlag ) )
				creating = true;
			else if ( argData.isFlagSet( kDeleteFlag ) )
				creating = false;
			else
				throw new ArgumentException("Command Syntax is incorrect", "args");

			if (creating)
			{
				lineManipObj = modifier.createNode("simpleLineManipCSharp", MObject.kNullObj);
			}
			else
			{
				if (lineManipObj != null)
				{
					modifier.deleteNode(lineManipObj);
				}
				lineManipObj = null;
			}

			redoIt();
		}

		public override void redoIt()
		{
			modifier.doIt();
			return;
		}

		public override void undoIt()
		{
			modifier.undoIt();
			return;
		}
	}
}
