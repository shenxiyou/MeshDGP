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
using System.Collections;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

 
using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaUI;
using Autodesk.Maya.OpenMayaRender;


[assembly: MPxShapeClass(typeof(MayaNetPlugin.quadricShape), typeof(MayaNetPlugin.quadricShapeUI), "quadricShapeCSharp", 0x80111)]

namespace MayaNetPlugin
{
    class GLUFunctionInvoker
    {
        [DllImportAttribute("glu32.dll")]
        public static extern void gluQuadricDrawStyle(IntPtr qobj, uint drawStyle);

        [DllImportAttribute("glu32.dll")]
        public static extern IntPtr gluNewQuadric();

        [DllImportAttribute("glu32.dll")]
        public static extern void gluQuadricNormals(IntPtr qobj, uint normals);

        [DllImportAttribute("glu32.dll")]
        public static extern void gluQuadricTexture(IntPtr qobj, byte textureCoords);

        [DllImportAttribute("glu32.dll")]
        public static extern void gluCylinder(
            IntPtr qobj,
            double baseRadius,
            double topRadius,
            double height,
            int slices,
            int stacks);

        [DllImportAttribute("glu32.dll")]
        public static extern void gluDisk(
            IntPtr qobj,
            double innerRadius,
            double outerRadius,
            int slices,
            int loops);

        [DllImportAttribute("glu32.dll")]
        public static extern void gluPartialDisk(
            IntPtr qobj,
            double innerRadius,
            double outerRadius,
            int slices,
            int loops,
            double startAngle,
            double sweepAngle);

        [DllImportAttribute("glu32.dll")]
        public static extern void gluSphere(
            IntPtr qobj,
            double radius,
            int slices,
            int stacks);
    }

    class quadricGeom
    {
        public double radius1;
        public double radius2;
        public double height;
        public double startAngle;
        public double sweepAngle;
        public short slices;
        public short loops;
        public short stacks;
        public short shapeType;
    };

    class quadricShape : MPxSurfaceShape
    {
        private quadricGeom fGeometry;

        [MPxNodeNumeric("r1", "radius1", MFnNumericData.Type.k3Double,
            Keyable = true, Hidden = false, Internal = true)]
        [MPxNumericDefault(new Double[] {2.0, 1.0, 3.0})]
        public static MObject radius1 = null;

        [MPxNodeNumeric("r2", "radius2", MFnNumericData.Type.kDouble,
            Keyable = true, Hidden = false, Internal = true)]
        [MPxNumericDefault(2.0)]
        public static MObject radius2 = null;

        [MPxNodeNumeric("ht", "height", MFnNumericData.Type.kDouble,
            Keyable = true, Hidden = false, Internal = true)]
        [MPxNumericDefault(2.0)]
        public static MObject height = null;

        [MPxNodeNumeric("sta", "startAngle", MFnNumericData.Type.kDouble,
            Keyable = true, Hidden = false, Internal = true)]
        [MPxNumericDefault(0.0)]
        public static MObject startAngle = null;

        [MPxNodeNumeric("swa", "sweepAngle", MFnNumericData.Type.kDouble,
            Keyable = true, Hidden = false, Internal = true)]
        [MPxNumericDefault(90.0)]
        public static MObject sweepAngle = null;

        [MPxNodeNumeric("sl", "slices", MFnNumericData.Type.kShort,
            Keyable = true, Hidden = false, Internal = true)]
        [MPxNumericDefault(8)]
        public static MObject slices = null;

        [MPxNodeNumeric("lp", "loops", MFnNumericData.Type.kShort,
            Keyable = true, Hidden = false, Internal = true)]
        [MPxNumericDefault(6)]
        public static MObject loops = null;

        [MPxNodeNumeric("sk", "stacks", MFnNumericData.Type.kShort,
            Keyable = true, Hidden = false, Internal = true)]
        [MPxNumericDefault(4)]
        public static MObject stacks = null;

        [MPxNodeEnum("st", "shapeType", Keyable = true, Hidden = false)]
        [MPxEnumField("cylinder")]
        [MPxEnumField("disk")]
        [MPxEnumField("partialDisk")]
        [MPxEnumField("sphere")]
        public static MObject shapeType = null;

        public quadricShape()
        {
            fGeometry = new quadricGeom();
            fGeometry.radius1 = 1.0;
            fGeometry.radius2 = 1.0;
            fGeometry.height = 2.0;
            fGeometry.startAngle = 0.0;
            fGeometry.sweepAngle = 90.0;
            fGeometry.slices = 8;
            fGeometry.loops = 6;
            fGeometry.stacks = 4;
            fGeometry.shapeType = 0;
        }

        public override void postConstructor()
        {
            isRenderable = true;
        }

        public override bool compute(MPlug plug, MDataBlock datablock)
        // Since there are no output attributes this is not necessary but
        // if we wanted to compute an output mesh for rendering it would
        // be done here base on the inputs.
        {
            return false;
        }

        public override bool getInternalValue(MPlug plug, MDataHandle datahandle)
        {
            bool isOk = true;

            if (plug.attribute.equalEqual(radius1))
            {
                datahandle.set(fGeometry.radius1);
                isOk = true;
            }
            else if (plug.attribute.equalEqual(radius2))
            {
                datahandle.set(fGeometry.radius2);
                isOk = true;
            }
            else if (plug.attribute.equalEqual(height))
            {
                datahandle.set(fGeometry.height);
                isOk = true;
            }
            else if (plug.attribute.equalEqual(startAngle))
            {
                datahandle.set(fGeometry.startAngle);
                isOk = true;
            }
            else if (plug.attribute.equalEqual(sweepAngle))
            {
                datahandle.set(fGeometry.sweepAngle);
                isOk = true;
            }
            else if (plug.attribute.equalEqual(slices))
            {
                datahandle.set(fGeometry.slices);
                isOk = true;
            }
            else if (plug.attribute.equalEqual(loops))
            {
                datahandle.set(fGeometry.loops);
                isOk = true;
            }
            else if (plug.attribute.equalEqual(stacks))
            {
                datahandle.set(fGeometry.stacks);
                isOk = true;
            }
            else
            {
                isOk = base.getInternalValue(plug, datahandle);
            }

            return isOk;
        }

        public override bool setInternalValue(MPlug plug, MDataHandle datahandle)
        {
            bool isOk = true;

            // In the case of a disk or partial disk the inner radius must
            // never exceed the outer radius and the minimum radius is 0
            //
            if (plug.attribute.equalEqual(radius1))
            {
                double innerRadius = datahandle.asDouble;
                double outerRadius = fGeometry.radius2;

                if (innerRadius > outerRadius)
                {
                    outerRadius = innerRadius;
                }

                if (innerRadius < 0)
                {
                    innerRadius = 0;
                }

                fGeometry.radius1 = innerRadius;
                fGeometry.radius2 = outerRadius;
                isOk = true;
            }
            else if (plug.attribute.equalEqual(radius2))
            {
                double outerRadius = datahandle.asDouble;
                double innerRadius = fGeometry.radius1;

                if (outerRadius <= 0)
                {
                    outerRadius = 0.1;
                }

                if (innerRadius > outerRadius)
                {
                    innerRadius = outerRadius;
                }

                if (innerRadius < 0)
                {
                    innerRadius = 0;
                }

                fGeometry.radius1 = innerRadius;
                fGeometry.radius2 = outerRadius;
                isOk = true;
            }
            else if (plug.attribute.equalEqual(height))
            {
                double val = datahandle.asDouble;
                if (val <= 0)
                {
                    val = 0.1;
                }
                fGeometry.height = val;
            }
            else if (plug.attribute.equalEqual(startAngle))
            {
                double val = datahandle.asDouble;
                fGeometry.startAngle = val;
            }
            else if (plug.attribute.equalEqual(sweepAngle))
            {
                double val = datahandle.asDouble;
                fGeometry.sweepAngle = val;
            }
            else if (plug.attribute.equalEqual(slices))
            {
                short val = datahandle.asShort;
                if (val < 3)
                {
                    val = 3;
                }
                fGeometry.slices = val;
            }
            else if (plug.attribute.equalEqual(loops))
            {
                short val = datahandle.asShort;
                if (val < 3)
                {
                    val = 3;
                }
                fGeometry.loops = val;
            }
            else if (plug.attribute.equalEqual(stacks))
            {
                short val = datahandle.asShort;
                if (val < 2)
                {
                    val = 2;
                }
                fGeometry.stacks = val;
            }
            else
            {
                isOk = base.setInternalValue(plug, datahandle);
            }

            return isOk;
        }

        public override bool isBounded()
        {
            return true;
        }
        public override MBoundingBox boundingBox()
        {
            MBoundingBox result = new MBoundingBox();
            quadricGeom geom = this.geometry();

            double r = geom.radius1;
            MPoint t = new MPoint(r, r, r);
            MPoint nt = new MPoint(-r, -r, -r);
            result.expand(t); result.expand(nt);
            r = geom.radius2;
            result.expand(t); result.expand(nt);
            r = geom.height;
            result.expand(t); result.expand(nt);

            return result;
        }

        public quadricGeom geometry()
        {
            MObject this_object = thisMObject();
            MPlug plug = new MPlug(this_object, radius1); 
            plug.getValue(ref fGeometry.radius1);
            plug.attribute=radius2;     plug.getValue(ref fGeometry.radius2);
            plug.attribute = height; plug.getValue(ref fGeometry.height);
            plug.attribute = startAngle; plug.getValue(ref fGeometry.startAngle);
            plug.attribute = sweepAngle; plug.getValue(ref fGeometry.sweepAngle);
            plug.attribute = slices; plug.getValue(ref fGeometry.slices);
            plug.attribute = loops; plug.getValue(ref fGeometry.loops);
            plug.attribute = stacks; plug.getValue(ref fGeometry.stacks);
            plug.attribute = shapeType; plug.getValue(ref fGeometry.shapeType);

            return fGeometry;
        }

    }

    class quadricShapeUI : MPxSurfaceShapeUI
    {
        public override void getDrawRequests(MDrawInfo info,
                                     bool objectAndActiveOnly,
                                     MDrawRequestQueue queue)
        {
            quadricShape shapeNode = surfaceShape as quadricShape;
            if (shapeNode == null)
                return;

            // The following line will be removed when all the .net assemblies will be merged into one
            // We would then be able to call info.getPrototype(this) 

            MDrawRequest request = info.getPrototype(this);

            quadricGeom geom = shapeNode.geometry();

            MDrawData data;
            getDrawData(geom, out data);
            request.setDrawData(data);

            // Are we displaying meshes?
            if (!info.objectDisplayStatus(M3dView.DisplayObjects.kDisplayMeshes))
                return;

            // Use display status to determine what color to draw the object
            //
            switch (info.displayStyle)
            {
                case M3dView.DisplayStyle.kWireFrame:
                    getDrawRequestsWireframe(request, info);
                    queue.add(request);
                    break;

                case M3dView.DisplayStyle.kGouraudShaded:
                    request.token = (int)DrawShapeStyle.kDrawSmoothShaded;
                    getDrawRequestsShaded(request, info, queue, data);
                    queue.add(request);
                    break;

                case M3dView.DisplayStyle.kFlatShaded:
                    request.token = (int)DrawShapeStyle.kDrawFlatShaded;
                    getDrawRequestsShaded(request, info, queue, data);
                    queue.add(request);
                    break;
                default:
                    break;
            }
        }


        private enum DrawShapeType
        {
            kDrawCylinder,
            kDrawDisk,
            kDrawPartialDisk,
            kDrawSphere
        };

        // Draw Tokens
        //
        private enum DrawShapeStyle
        {
            kDrawWireframe,
            kDrawWireframeOnShaded,
            kDrawSmoothShaded,
            kDrawFlatShaded,
            kLastToken
        };

        public override void draw(MDrawRequest request, M3dView view)
        //
        // From the given draw request, get the draw data and determine
        // which quadric to draw and with what values.
        //
        {
            MDrawData data = request.drawData();

            quadricGeom geom = data.geometry() as quadricGeom;

            DrawShapeStyle token = (DrawShapeStyle)request.token;
            bool drawTexture = false;

            view.beginGL();

            if ((token == DrawShapeStyle.kDrawSmoothShaded) || (token == DrawShapeStyle.kDrawFlatShaded))
            {
                OpenGL.glEnable((uint)OpenGL.GL_POLYGON_OFFSET_FILL);

                // Set up the material
                //
                MMaterial material = request.material;
                material.setMaterial(request.multiPath, request.isTransparent);

                // Enable texturing
                //
                drawTexture = material.materialIsTextured;
                if (drawTexture) OpenGL.glEnable((uint)OpenGL.GL_TEXTURE_2D);

                // Apply the texture to the current view
                //
                if (drawTexture)
                {
                    material.applyTexture(view, data);
                }
            }

            IntPtr qobj = GLUFunctionInvoker.gluNewQuadric();

            switch (token)
            {
                case DrawShapeStyle.kDrawWireframe:
                case DrawShapeStyle.kDrawWireframeOnShaded:
                    GLUFunctionInvoker.gluQuadricDrawStyle(qobj, GLU_LINE);
                    break;

                case DrawShapeStyle.kDrawSmoothShaded:
                    GLUFunctionInvoker.gluQuadricNormals(qobj, GLU_SMOOTH);
                    GLUFunctionInvoker.gluQuadricTexture(qobj, GLtrue);
                    GLUFunctionInvoker.gluQuadricDrawStyle(qobj, GLU_FILL);
                    break;

                case DrawShapeStyle.kDrawFlatShaded:
                    GLUFunctionInvoker.gluQuadricNormals(qobj, GLU_FLAT);
                    GLUFunctionInvoker.gluQuadricTexture(qobj, GLtrue);
                    GLUFunctionInvoker.gluQuadricDrawStyle(qobj, GLU_FILL);
                    break;
            }

            switch (geom.shapeType)
            {
                case (short)DrawShapeType.kDrawCylinder:
                    GLUFunctionInvoker.gluCylinder(qobj, geom.radius1, geom.radius2, geom.height,
                                 geom.slices, geom.stacks);
                    break;
                case (short)DrawShapeType.kDrawDisk:
                    GLUFunctionInvoker.gluDisk(qobj, geom.radius1, geom.radius2, geom.slices, geom.loops);
                    break;
                case (short)DrawShapeType.kDrawPartialDisk:
                    GLUFunctionInvoker.gluPartialDisk(qobj, geom.radius1, geom.radius2, geom.slices,
                                    geom.loops, geom.startAngle, geom.sweepAngle);
                    break;
                case (short)DrawShapeType.kDrawSphere:
                default:
                    GLUFunctionInvoker.gluSphere(qobj, geom.radius1, geom.slices, geom.stacks);
                    break;
            }

            // Turn off texture mode
            //
            if (drawTexture) OpenGL.glDisable((uint)OpenGL.GL_TEXTURE_2D);

            view.endGL();
        }

        const int LEAD_COLOR = 18; // green
        const int ACTIVE_COLOR = 15; // white
        const int ACTIVE_AFFECTED_COLOR = 8; // purple
        const int DORMANT_COLOR = 4; // blue
        const int HILITE_COLOR = 17; // pale blue

        const int GLU_POINT = 100010;
        const int GLU_LINE = 100011;
        const int GLU_FILL = 100012;
        const int GLU_SILHOUETTE = 100013;

        const int GLU_SMOOTH = 100000;
        const int GLU_FLAT = 100001;
        const int GLU_NONE = 100002;
        const byte GLtrue = 1;
        const byte GLfalse = 0;

        /* override */
        public override bool select(MSelectInfo selectInfo,
                                     MSelectionList selectionList,
                                     MPointArray worldSpaceSelectPts)
        //
        // Select function. Gets called when the bbox for the object is selected.
        // This function just selects the object without doing any intersection tests.
        //
        {
            MSelectionMask priorityMask = new MSelectionMask(MSelectionMask.SelectionType.kSelectObjectsMask);
            MSelectionList item = new MSelectionList();
            item.add(selectInfo.selectPath);
            MPoint xformedPt = new MPoint();
            selectInfo.addSelection(item, xformedPt, selectionList,
                                     worldSpaceSelectPts, priorityMask, false);
            return true;
        }


        public void getDrawRequestsWireframe(MDrawRequest request,
                                                       MDrawInfo info)
        {
            request.token = (int)DrawShapeStyle.kDrawWireframe;

            M3dView.DisplayStatus displayStatus = info.displayStatus;
            M3dView.ColorTable activeColorTable = M3dView.ColorTable.kActiveColors;
            M3dView.ColorTable dormantColorTable = M3dView.ColorTable.kDormantColors;
            switch (displayStatus)
            {
                case M3dView.DisplayStatus.kLead:
                    request.setColor(LEAD_COLOR, (int)activeColorTable);
                    break;
                case M3dView.DisplayStatus.kActive:
                    request.setColor(ACTIVE_COLOR, (int)activeColorTable);
                    break;
                case M3dView.DisplayStatus.kActiveAffected:
                    request.setColor(ACTIVE_AFFECTED_COLOR, (int)activeColorTable);
                    break;
                case M3dView.DisplayStatus.kDormant:
                    request.setColor(DORMANT_COLOR, (int)dormantColorTable);
                    break;
                case M3dView.DisplayStatus.kHilite:
                    request.setColor(HILITE_COLOR, (int)activeColorTable);
                    break;
                default:
                    break;
            }
        }

        public void getDrawRequestsShaded(MDrawRequest request,
                                                    MDrawInfo info,
                                                    MDrawRequestQueue queue,
                                                    MDrawData data)
        {
            // Need to get the material info
            //
            MDagPath path = info.multiPath;	// path to your dag object 
            M3dView view = info.view; 		// view to draw to
            MMaterial material = base.material(path);
            M3dView.DisplayStatus displayStatus = info.displayStatus;

            // Evaluate the material and if necessary, the texture.
            //
            material.evaluateMaterial(view, path);

            bool drawTexture = true;
            if (drawTexture && material.materialIsTextured)
            {
                material.evaluateTexture(data);
            }

            request.material = material;

            bool materialTransparent = false;
            material.getHasTransparency( ref materialTransparent);
            if ( materialTransparent ) {
                request.isTransparent = true;
            }

            // create a draw request for wireframe on shaded if
            // necessary.
            //
            if ((displayStatus == M3dView.DisplayStatus.kActive) ||
                 (displayStatus == M3dView.DisplayStatus.kLead) ||
                 (displayStatus == M3dView.DisplayStatus.kHilite))
            {
                MDrawRequest wireRequest = info.getPrototype(this);
                wireRequest.setDrawData(data);
                getDrawRequestsWireframe(wireRequest, info);
                wireRequest.token = (int)DrawShapeStyle.kDrawWireframeOnShaded;
                wireRequest.displayStyle = M3dView.DisplayStyle.kWireFrame;
                queue.add(wireRequest);
            }
        }
    }
}
