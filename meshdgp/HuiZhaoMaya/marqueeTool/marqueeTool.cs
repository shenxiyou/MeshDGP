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
using System.Runtime.InteropServices;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaUI;

/*
Install:
	marqueeToolContextCSharp marqueeToolContext1;
	setParent General;
	toolButton -cl toolCluster
			-t marqueeToolContext1
			-i1 "C:/Program Files/Autodesk/Maya2012/devkit/plug-ins/marqueeTool.xpm"
			marqueeTool1;
	
Uninstall
	deleteUI marqueeToolContext1;
	deleteUI marqueeTool1;
*/

[assembly: MPxContextCommandClass("marqueeToolContextCSharp", typeof(MayaNetPlugin.marqueeContext))]

namespace MayaNetPlugin
{
	class marqueeContext : Autodesk.Maya.OpenMayaUI.MPxContext
	{
		[DllImport("opengl32")]
		public static extern void glVertex2i(int x, int y);

		[DllImport("opengl32")]
		public static extern void glVertex3f(float x, float y, float z);

		[DllImport("opengl32")]
		public static extern void glBegin(uint mode);

		[DllImport("opengl32")]
		public static extern void glEnd();

		private const uint GL_LINE_LOOP = 0x0002 ;

		private short start_x, start_y;
		private short last_x, last_y;
		private bool fsDrawn;
		private MGlobal.ListAdjustment listAdjustment;
		private M3dView view ;

		public marqueeContext() {
			titleString = "Marquee Tool";
			// Tell the context which XPM to use so the tool can properly
			// be a candidate for the 6th position on the toolbar.
            // path examples\assemblies\examples.nll.dll relative to examples\marqueeTool\marqueeTool.xpm 
            string fullPath = ExamplesPlugin.convertRefPathToFullPath(@"..\marqueeTool\marqueeTool.xpm");
            if (System.IO.File.Exists(fullPath))
            {
                setImage(fullPath, MPxContext.ImageIndex.kImage1);
            }
		}

		public override void toolOnSetup(MEvent eventArgs) {
			helpString = "Click with left button or drag with middle button to select";
		}

		public override void doEnterRegion (MEvent eventArg) {
			helpString = "Click with left button or drag with middle button to select";
			return;
		}

		public override void doPress(MEvent eventArg)
		{
			// Begin marquee drawing (using OpenGL)
			// Get the start position of the marquee

			// Figure out which modifier keys were pressed, and set up the
			// listAdjustment parameter to reflect what to do with the selected points.
			if (eventArg.isModifierShift || eventArg.isModifierControl ) {
				if ( eventArg.isModifierShift ) {
					if ( eventArg.isModifierControl ) {
						// both shift and control pressed, merge new selections
						listAdjustment = MGlobal.ListAdjustment.kAddToList;
					} else {
						// shift only, xor new selections with previous ones
						listAdjustment = MGlobal.ListAdjustment.kXORWithList;
					}
				} else if ( eventArg.isModifierControl ) {
					// control only, remove new selections from the previous list
					listAdjustment = MGlobal.ListAdjustment.kRemoveFromList; 
				}
			} else {
				listAdjustment = MGlobal.ListAdjustment.kReplaceList;
			}

			// Extract the eventArg information
			eventArg.getPosition (ref start_x, ref start_y) ;
			view = M3dView.active3dView;
			fsDrawn = false;
		}

		public override void doDrag(MEvent eventArg)
		{
			// Drag out the marquee (using OpenGL)
			view.beginXorDrawing();
			if ( fsDrawn )
				// Redraw the marquee at its old position to erase it.
				drawMarquee();
			fsDrawn = true;

			//	Get the marquee's new end position.
			eventArg.getPosition (ref last_x, ref last_y) ;

			// Draw the marquee at its new position.
			drawMarquee();

			view.endXorDrawing();
		}

		public override void doRelease(MEvent eventArg)
		{
			// Selects objects within the marquee box.
			if ( fsDrawn ) {
				view.beginXorDrawing();
				// Redraw the marquee at its old position to erase it.
				drawMarquee();
				view.endXorDrawing();
			}
			// Get the end position of the marquee
			eventArg.getPosition (ref last_x, ref last_y) ;

			// Save the state of the current selections.  The "selectFromSceen"
			// below will alter the active list, and we have to be able to put
			// it back.
			MSelectionList incomingList =MGlobal.activeSelectionList ;

			// If we have a zero dimension box, just do a point pick
			if ( Math.Abs(start_x - last_x) < 2 && Math.Abs(start_y - last_y) < 2 ) {
				// This will check to see if the active view is in wireframe or not.
				MGlobal.SelectionMethod selectionMethod = MGlobal.selectionMethod;
				MGlobal.selectFromScreen (start_x, start_y, MGlobal.ListAdjustment.kReplaceList, selectionMethod) ;
			} else {
				// The Maya select tool goes to wireframe select when doing a marquee, so
				// we will copy that behaviour.
				// Select all the objects or components within the marquee.
				MGlobal.selectFromScreen (start_x, start_y, last_x, last_y,
											MGlobal.ListAdjustment.kReplaceList, 
											MGlobal.SelectionMethod.kWireframeSelectMethod) ;
			}

			// Get the list of selected items
			MSelectionList marqueeList = MGlobal.activeSelectionList;

			// Restore the active selection list to what it was before
			// the "selectFromScreen"
			MGlobal.activeSelectionList = incomingList ; // MGlobal.ListAdjustment.kReplaceList

			// Update the selection list as indicated by the modifier keys.
			MGlobal.selectCommand (marqueeList, listAdjustment) ;
		}

		protected void drawMarquee ()
		{
			glBegin(GL_LINE_LOOP);
			glVertex2i(start_x, start_y);
			glVertex2i(last_x, start_y);
			glVertex2i(last_x, last_y);
			glVertex2i(start_x, last_y);
			glEnd();
		}
	}
}
