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

/*
Install:
	moveToolContextCSharp moveToolContext1;
	setParent General;
	toolButton	-cl toolCluster
		-t moveToolContext1
		-i1 " C:/Program Files/Autodesk/Maya2012/devkit/plug-ins/moveTool.xpm"
		moveTool1;

Uninstall:			
	deleteUI moveToolContext1;
	deleteUI moveTool1;
*/

[assembly: MPxContextCommandClass("moveToolContextCSharp", typeof(MayaNetPlugin.MoveContext),
	ToolType = typeof(MayaNetPlugin.moveCmd), ToolName = "moveToolCmdCSharp")]

namespace MayaNetPlugin
{
	class moveCmd : MPxToolCommand, IUndoMPxCommand, IMPxToolCommand
	{
		protected MVector __delta;
		protected enum MoveToolAction { kDoIt, kRedoIt, kUndoIt } ;

		public moveCmd() {
			commandString = "moveToolCmdCSharp";
			__delta =new MVector () ;
		}

		override public void doIt(MArgList args)
		{
			MSyntax syntax = new MSyntax();
			syntax.addArg(MSyntax.MArgType.kDouble);
			syntax.addArg(MSyntax.MArgType.kDouble);
			syntax.addArg(MSyntax.MArgType.kDouble);

			MArgDatabase argData = new MArgDatabase(syntax, args);

			MVector vector = MVector.xAxis ;
			if ( args.length == 1 ) {
				vector.x =args.asDouble (0) ;
			} else if ( args.length == 2 ) {
				vector.x =args.asDouble (0) ;
				vector.y =args.asDouble (1) ;
			} else if ( args.length == 3 ) {
				uint i =0 ;
				vector = args.asVector(ref i);
			}
			__delta = vector;
			__action (MoveToolAction.kDoIt) ;
			return;
		}

		public override void redoIt()
		{
			__action (MoveToolAction.kRedoIt) ;
			return;
		}

		public override void undoIt()
		{
			__action (MoveToolAction.kUndoIt) ;
			return;
		}

		protected void __action (MoveToolAction flag) {


			//- Do the actual work here to move the objects	by vector
			MVector vector = __delta;
			if ( flag == MoveToolAction.kUndoIt ) { 
				vector =-vector ;
			} else {
				// all other cases identical
			}

			//- Create a selection list iterator
			MSelectionList slist = MGlobal.activeSelectionList;

            foreach (MDagPath mdagPath in slist.DagPaths())
			{
				MObject mComponent = mdagPath.node;

				try {
					MFnTransform transFn =new MFnTransform(mdagPath);
					try {
					transFn.translateBy (vector, MSpace.Space.kWorld) ;
					continue ;
					} catch (Exception) {
						MGlobal.displayInfo("Error doing translate on transform");
				}
				} catch (Exception) {
					//- Not a transform
				}

                try
                {
                    var CVs = new MCCurveCV(mdagPath, mComponent);
                    try
                    {
                        foreach (MItCurveCV cvFn2 in CVs)
                            cvFn2.translateBy(vector, MSpace.Space.kWorld);
                        CVs.iter.updateCurve();
                    }
                    catch (System.Exception)
                    {
                        MGlobal.displayInfo("Error setting Curve CV");
                    }
                }
                catch (System.Exception )
                {
                    //- No Curve CV
                }

				try {
				MItSurfaceCV sCvFn =new MItSurfaceCV (mdagPath, mComponent, true) ;
					try {
				while ( !sCvFn.isDone  ) {
					while ( !sCvFn.isRowDone ) {
						sCvFn.translateBy (vector, MSpace.Space.kWorld) ;
						sCvFn.next () ;
					}
					sCvFn.nextRow () ;
				}
				sCvFn.updateSurface () ;
					} catch (Exception) {
						MGlobal.displayInfo("Error setting Surface CV");
					}
				} catch (Exception) {
					//- No Surface CV
				}

				try {
                    var meshVertexEnum = new MCMeshVertex(mdagPath, mComponent);
					try {
                        foreach (MItMeshVertex vtxFn in meshVertexEnum)
                            vtxFn.translateBy(vector, MSpace.Space.kWorld);
                        meshVertexEnum.iter.updateSurface();
                    }
                    catch (Exception)
                    {
						MGlobal.displayInfo("Error setting Mesh Vertex");
					}
				} catch (Exception) {
					//- No Mesh Vertex
				}
			}

		}

		public override void cancel()
		{
			//- nothing
			MGlobal.displayInfo("Command cancelled");
			return;
		}

		public override void finalize()
		{
			MArgList command =new MArgList () ;
			command.addArg (commandString) ;
			command.addArg (__delta.x) ;
			command.addArg (__delta.y) ;
			command.addArg (__delta.z) ;

			// This call adds the command to the undo queue and sets
			// the journal string for the command.
			base._doFinalize (command) ;
			return;
		}

		public void setVector (double x, double y, double z) {
			__delta.x =x ;
			__delta.y =y ;
			__delta.z =z ;
		}

	}

	class MoveContext : MPxSelectionContext
	{

		private int currWin ;
		private MEvent.MouseButtonType downButton ;
		private M3dView view ;
		private short startPos_x, endPos_x ;
		private short startPos_y, endPos_y ;
		private moveCmd cmd ;

		public MoveContext()
		{
			titleString = "moveTool";

			// Tell the context which XPM to use so the tool can properly
			// be a candidate for the 6th position on the mini-bar.
            string fullPath = ExamplesPlugin.convertRefPathToFullPath(@"..\moveTool\moveTool.xpm");
            if (System.IO.File.Exists(fullPath))
            {
                setImage(fullPath, MPxContext.ImageIndex.kImage1);
            }
		}

		public override void toolOnSetup(MEvent eventArgs) {
			helpString = "drag to move selected object";
		}

		public override void doEnterRegion(MEvent eventArg)
		{
			helpString = "drag to move selected object";
			return;
		}

		public override void doPress(MEvent eventArg)
		{
			base.doPress (eventArg) ;
			
			// If we are not in selecting mode (i.e. an object has been selected)
			// then set up for the translation.
			if ( !_isSelecting () ) {
				eventArg.getPosition (ref startPos_x, ref startPos_y) ;

				view = M3dView.active3dView;

				MDagPath camera = view.Camera ;
				MFnCamera fnCamera =new MFnCamera (camera) ;
				MVector upDir =fnCamera.upDirection (MSpace.Space.kWorld) ;
				MVector rightDir =fnCamera.rightDirection (MSpace.Space.kWorld) ;

				// Determine the camera used in the current view
				if ( fnCamera.isOrtho ) {
					if ( upDir.isEquivalent (MVector.zNegAxis, 1e-3) )
						currWin =0 ; // TOP
					else if ( rightDir.isEquivalent (MVector.xAxis, 1e-3) )
						currWin =1 ; // FRONT
					else
						currWin =2 ; // SIDE
				} else {
					currWin =3 ; // PERSP
					MGlobal.displayWarning ("moveTool only works in top, front and side views") ;
				}

				// Create an instance of the move tool command.
				cmd = _newToolCommand () as moveCmd;
				cmd.setVector (0.0, 0.0, 0.0) ;
			}
		}

		public override void doDrag(MEvent eventArg)
		{
			base.doDrag (eventArg) ;

			// If we are not in selecting mode (i.e. an object has been selected)
			// then do the translation.
			if ( !_isSelecting () ) {
				eventArg.getPosition (ref endPos_x, ref endPos_y) ;
				MPoint endW =new MPoint () ;
				MPoint startW =new MPoint () ;
				MVector vec =new MVector () ;
				view.viewToWorld (startPos_x, startPos_y, startW, vec) ;
				view.viewToWorld (endPos_x, endPos_y, endW, vec) ;
				downButton =eventArg.mouseButton;

				// We reset the the move vector each time a drag event occurs 
				// and then recalculate it based on the start position. 
				cmd.undoIt () ;

				switch ( currWin ) {
					case 0: // TOP
						switch ( downButton ) {
							case MEvent.MouseButtonType.kMiddleMouse:
								cmd.setVector (endW.x - startW.x, 0.0, 0.0) ;
								break ;
							case MEvent.MouseButtonType.kLeftMouse:
							default:
								cmd.setVector (endW.x - startW.x, 0.0, endW.z - startW.z) ;
								break ;
						}
						break ;
					case 1: // FRONT
						switch ( downButton ) {
							case MEvent.MouseButtonType.kMiddleMouse:
								cmd.setVector (endW.x - startW.x, 0.0, 0.0) ;
								break ;
							case MEvent.MouseButtonType.kLeftMouse:
							default:
								cmd.setVector (endW.x - startW.x, endW.y - startW.y, 0.0) ;
								break ;
						}
						break ;
					case 2: //SIDE
						switch ( downButton ) {
							case MEvent.MouseButtonType.kMiddleMouse:
								cmd.setVector (0.0, 0.0, endW.z - startW.z) ;
								break ;
							case MEvent.MouseButtonType.kLeftMouse:
							default:
								cmd.setVector (0.0, endW.y - startW.y, endW.z - startW.z) ;
								break ;
						}
						break ;
					default:
					case 3: // PERSP
						break;
				}

				cmd.redoIt () ;
				view.refresh (true,false) ;
			}
		}

		public override void doRelease(MEvent eventArg)
		{
			base.doRelease (eventArg) ;
			if ( !_isSelecting () ) {
				eventArg.getPosition (ref endPos_x, ref endPos_y) ;
				// Delete the move command if we have moved less then 2 pixels
				// otherwise call finalize to set up the journal and add the command to the undo queue.
				if ( Math.Abs (startPos_x - endPos_x) < 2 && Math.Abs (startPos_y - endPos_y) < 2 ) {
					cmd =null ;
					view.refresh (true,false) ;
				} else {
					cmd.finalize () ;
					view.refresh (true,false) ;
				}
			}
		}
	}
}
