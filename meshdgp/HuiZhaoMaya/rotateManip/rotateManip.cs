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

[assembly: MPxNodeClass(typeof(MayaNetPlugin.exampleRotateManip), "exampleRotateManipCSharp", 
    0x00081073, NodeType = MPxNode.NodeType.kManipContainer)]
[assembly: MPxContextCommandClass("rotateContextCSharp", typeof(MayaNetPlugin.RotateManipContext))]

namespace MayaNetPlugin
{
    class exampleRotateManip : MPxManipContainer
    {

        public override void createChildren()
        {
            // Add the rotation manip
            //
            fRotateManip = addRotateManip("RotateManip", "rotation");

            // Add the state manip.  The state manip is used to cycle through the 
            // rotate manipulator modes to demonstrate how they work.
            //
            fStateManip = addStateManip("StateManip", "state");

            // The state manip permits 4 states.  These correspond to:
            // 0 - Rotate manip in objectSpace mode
            // 1 - Rotate manip in worldSpace mode
            // 2 - Rotate manip in gimbal mode
            // 3 - Rotate manip in objectSpace mode with snapping on
            //
            // Note that while the objectSpace and gimbal modes will operator similar 
            // to the built-in Maya rotate manipulator, the worldSpace mode will 
            // produce unusual rotations because the plugin does not convert worldSpace
            // rotations to object space.
            //
            MFnStateManip stateManip = new MFnStateManip(fStateManip);
            stateManip.maxStates = 4;
            stateManip.setInitialState(0);
        }

        // This function is a utility that can be used to extract vector values from
        // plugs.
        //
        private MVector vectorPlugValue(MPlug plug)
        {
            if (plug.numChildren == 3)
            {
                double x, y, z;
                MPlug rx = plug.child(0);
                MPlug ry = plug.child(1);
                MPlug rz = plug.child(2);
                x = rx.asDouble();
                y = ry.asDouble();
                z = rz.asDouble();
                MVector result = new MVector(x, y, z);
                return result;
            }
            else
            {
                MGlobal.displayError("Expected 3 children for plug " + plug.name);
                MVector result = new MVector(0, 0, 0);
                return result;
            }
        }

        public override void connectToDependNode(MObject node)
        {

            // Find the rotate and rotatePivot plugs on the node.  These plugs will 
            // be attached either directly or indirectly to the manip values on the
            // rotate manip.
            //
            MFnDependencyNode nodeFn = new MFnDependencyNode(node);
            MPlug rPlug = nodeFn.findPlug("rotate");
            MPlug rcPlug = nodeFn.findPlug("rotatePivot");

            // If the translate pivot exists, it will be used to move the state manip
            // to a convenient location.
            //
            MPlug tPlug = nodeFn.findPlug("translate");

            // To avoid having the object jump back to the default rotation when the
            // manipulator is first used, extract the existing rotation from the node
            // and set it as the initial rotation on the manipulator.
            //
            MEulerRotation existingRotation = new MEulerRotation(vectorPlugValue(rPlug));
            MVector existingTranslation = new MVector(vectorPlugValue(tPlug));

            // 
            // The following code configures default settings for the rotate 
            // manipulator.
            //

            MFnRotateManip rotateManip = new MFnRotateManip(fRotateManip);
            rotateManip.setInitialRotation(existingRotation);
            rotateManip.setRotateMode(MFnRotateManip.RotateMode.kObjectSpace);
            rotateManip.displayWithNode(node);

            // Add a callback function to be called when the rotation value changes
            //

            //rotatePlugIndex = addManipToPlugConversionCallback( rPlug, (manipToPlugConversionCallback)&exampleRotateManip::rotationChangedCallback );
            ManipToPlugConverion[rPlug] = rotationChangedCallback;
            // get the index of plug
            rotatePlugIndex = this[rPlug];

            // Create a direct (1-1) connection to the rotation center plug
            //
            rotateManip.connectToRotationCenterPlug(rcPlug);

            // Place the state manip at a distance of 2.0 units away from the object
            // along the X-axis.
            //
            MFnStateManip stateManip = new MFnStateManip(fStateManip);
            MVector delta = new MVector(2, 0, 0);
            stateManip.setTranslation(existingTranslation + delta,
                MSpace.Space.kTransform);

            finishAddingManips();
            base.connectToDependNode(node);
        }

        #region memberdata
        private MDagPath fRotateManip;
        private MDagPath fStateManip;
        private uint rotatePlugIndex;
        #endregion

        // Callback function
        MManipData rotationChangedCallback(object sender, ManipConversionArgs args)
        {
            MObject obj = MObject.kNullObj;

            // If we entered the callback with an invalid index, print an error and
            // return.  Since we registered the callback only for one plug, all 
            // invocations of the callback should be for that plug.
            //

            MFnNumericData numericData = new MFnNumericData();
            if (args.ManipIndex != rotatePlugIndex)
            {
                MGlobal.displayError("Invalid index in rotation changed callback!");

                // For invalid indices, return vector of 0's
                obj = numericData.create(MFnNumericData.Type.k3Double);
                numericData.setData(0.0, 0.0, 0.0);

                return new MManipData(obj);
            }

            // Assign function sets to the manipulators
            //
            MFnStateManip stateManip = new MFnStateManip(fStateManip);
            MFnRotateManip rotateManip = new MFnRotateManip(fRotateManip);

            // Adjust settings on the rotate manip based on the state of the state 
            // manip.
            //
            uint mode = stateManip.state;
            if (mode != 3)
            {
                rotateManip.setRotateMode((MFnRotateManip.RotateMode)stateManip.state);
                rotateManip.setSnapMode(false);
            }
            else
            {
                // State 3 enables snapping for an object space manip.  In this case,
                // we snap every 15.0 degrees.
                //
                rotateManip.setRotateMode(MFnRotateManip.RotateMode.kObjectSpace);
                rotateManip.setSnapMode(true);
                rotateManip.snapIncrement = 15.0;
            }

            // The following code creates a data object to be returned in the 
            // MManipData.  In this case, the plug to be computed must be a 3-component
            // vector, so create data as MFnNumericData::k3Double
            //
            obj = numericData.create(MFnNumericData.Type.k3Double);

            // Retrieve the value for the rotation from the manipulator and return it
            // directly without modification.  If the manipulator should eg. slow down
            // rotation, this method would need to do some math with the value before
            // returning it.
            //
            MEulerRotation manipRotation = new MEulerRotation();
            try
            {
                getConverterManipValue(rotateManip.rotationIndex, manipRotation);
                numericData.setData(manipRotation.x, manipRotation.y, manipRotation.z);
            }
            catch (System.Exception)
            {
                MGlobal.displayError("Error retrieving manip value");
                numericData.setData(0.0, 0.0, 0.0);
            }

            return new MManipData(obj);
        }

        public override void draw(M3dView view, MDagPath path, M3dView.DisplayStyle style, M3dView.DisplayStatus status)
        {
            base.draw(view, path, style, status);
        }
    }

    // delegate
    class updateManipulatorBridge
    {
        public WeakReference Data
        {
            get;
            private set;
        }

        public updateManipulatorBridge(RotateManipContext data)
        {
            Data = new WeakReference(data);
        }

        // Callback issued when selection list changes
        public void updateManipulators(object sender, MBasicFunctionArgs args)
        {
            RotateManipContext ctxPtr = Data.Target as RotateManipContext;
            RotateManipContext.updateManipulators(ctxPtr);
        }
    }

    class RotateManipContext : MPxSelectionContext
    {
        private updateManipulatorBridge bridge;
        public static void updateManipulators(RotateManipContext ctx)
        {
            if (ctx == null)
                return;

            ctx.deleteManipulators();

            // Add the rotate manipulator to each selected object.  This produces 
            // behavior different from the default rotate manipulator behavior.  Here,
            // a distinct rotate manipulator is attached to every object.
            // 
            try
            {
                MSelectionList list = MGlobal.activeSelectionList;

                MItSelectionList iter = new MItSelectionList(list, MFn.Type.kInvalid);
                for (; !iter.isDone; iter.next())
                {

                    // Make sure the selection list item is a depend node and has the
                    // required plugs before manipulating it.
                    //
                    MObject dependNode = new MObject();
                    iter.getDependNode(dependNode);
                    if (dependNode.isNull || !dependNode.hasFn(MFn.Type.kDependencyNode))
                    {
                        MGlobal.displayWarning("Object in selection list is not a depend node.");
                        continue;
                    }

                    MFnDependencyNode dependNodeFn = new MFnDependencyNode(dependNode);
                    try
                    {
                        /* MPlug rPlug = */
                        dependNodeFn.findPlug("rotate");
                    }
                    catch (System.Exception)
                    {
                        MGlobal.displayWarning("Object cannot be manipulated: " + dependNodeFn.name);
                        continue;
                    }

                    // Add manipulator to the selected object
                    //
                    MObject manipObject = new MObject();

                    exampleRotateManip manipulator;
                    try
                    {
                        manipulator = exampleRotateManip.newManipulator("exampleRotateManipCSharp", manipObject) as exampleRotateManip;

                        // Add the manipulator
                        //
                        ctx.addManipulator(manipObject);

                        // Connect the manipulator to the object in the selection list.
                        //
                        try
                        {
                            manipulator.connectToDependNode(dependNode);
                        }
                        catch (System.Exception)
                        {
                            MGlobal.displayWarning("Error connecting manipulator to object: " + dependNodeFn.name);
                        }
                    }
                    catch (System.Exception)
                    {

                    }
                }
            }
            catch (System.Exception)
            {

            }

        }
        public RotateManipContext()
        {
            titleString = "Plugin Rotate Manipulator";
        }
        public override void toolOnSetup(MEvent evt)
        {
            helpString = "Rotate the object using the rotation handles";

            updateManipulators(this);
            try
            {
                if (bridge == null)
                    bridge = new updateManipulatorBridge(this);
                MModelMessage.ActiveListModified += bridge.updateManipulators;
            }
            catch (System.Exception)
            {
                MGlobal.displayError("Model addCallback failed");
            }
        }
        public override void toolOffCleanup()
        {
            try
            {
                if (bridge != null)
                {
                    MModelMessage.ActiveListModified -= bridge.updateManipulators;
                    bridge = null;
                }
            }
            catch (System.Exception)
            {
                MGlobal.displayError("Model remove callback failed");

            }
            base.toolOffCleanup();
        }

    }

}
