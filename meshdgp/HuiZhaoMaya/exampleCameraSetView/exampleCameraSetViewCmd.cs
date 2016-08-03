using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.Maya.Runtime;
using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaUI;
using Autodesk.Maya.OpenMayaMPx;

[assembly: MPxModelEditorCommandClass(typeof(MayaNetPlugin.exampleCameraSetViewCmd),
	typeof(MayaNetPlugin.exampleCameraSetView), "exampleCameraSetViewCSharp")]

namespace MayaNetPlugin
{
	[MPxCommandSyntaxFlag("-i", "-initialize")]
	[MPxCommandSyntaxFlag("-r", "-results")]
	[MPxCommandSyntaxFlag("-cl", "-clear")]
	class exampleCameraSetViewCmd : MPxModelEditorCommand
	{
		public exampleCameraSetViewCmd()
		{
			fCameraList = new MDagPathArray();
		}

		public override bool doEditFlags()
		{
			MPx3dModelView user3dModelView = modelView();
			if (user3dModelView == null)
				throw new InvalidOperationException("Failed to create modelView for exampleCameraSetViewCmd");

			MArgParser argData = _parser();
			if (argData.isFlagSet("-i"))
			{
				initTests(user3dModelView);
			}
			else if (argData.isFlagSet("-r"))
			{
				testResults(user3dModelView);
			}
			else if (argData.isFlagSet("-cl"))
			{
				clearResults(user3dModelView);
			}
			return false;
		}

		public override MPx3dModelView makeModelView()
		{
			// only 
			return base.makeModelView();
		}

		protected void initTests(MPx3dModelView view)
		{
			MGlobal.displayInfo("exampleCameraSetViewCmd::initTests");

			clearResults(view);

			//	Add every camera into the scene. Don't change the main camera,
			//	it is OK that it gets reused.
			//
			MFnCameraSet cstFn = new MFnCameraSet();
			MObject cstObj = cstFn.create();
			MDagPath cameraPath = null;
			
			MItDag dagIterator = new MItDag(MItDag.TraversalType.kDepthFirst, MFn.Type.kCamera);
			for (; !dagIterator.isDone; dagIterator.next())
			{
				cameraPath = new MDagPath();

				MFnCamera camera;

				try
				{
					dagIterator.getPath(cameraPath);
					camera = new MFnCamera(cameraPath);
				}
				catch (Exception)
				{
					continue;
				}

				fCameraList.append(cameraPath);

				cstFn.appendLayer(cameraPath, MObject.kNullObj);

				MGlobal.displayInfo(camera.fullPathName());
			}

			view.setCameraSet(cstObj);
			view.refresh();
		}

		protected void testResults(MPx3dModelView view)
		{
			MObject cstObj = MObject.kNullObj;

			view.getCameraSet(cstObj);

			MGlobal.displayInfo("fCameraList.length() = " + fCameraList.length);
			MGlobal.displayInfo("fCameraList = " + fCameraList);

			MFnCameraSet cstFn = new MFnCameraSet(cstObj);
			uint numLayers = cstFn.getNumLayers();

			MGlobal.displayInfo("view.cameraSet.numLayers = " + numLayers);
			MGlobal.displayInfo("Cameras:");
			for (uint i = 0; i < numLayers; i++)
			{
				MDagPath camPath = new MDagPath();
				cstFn.getLayerCamera(i, camPath);
				camPath.extendToShape();
				MGlobal.displayInfo("    " + camPath.fullPathName());
			}
		}

		protected void clearResults(MPx3dModelView view)
		{
			MObject cstObj = MObject.kNullObj;

			try
			{
				view.getCameraSet(cstObj);
				view.setCameraSet(MObject.kNullObj);
				MGlobal.deleteNode(cstObj);
			}
			catch (Exception)
			{
			}
			fCameraList.clear();
		}

		protected MDagPathArray fCameraList;

	}
}