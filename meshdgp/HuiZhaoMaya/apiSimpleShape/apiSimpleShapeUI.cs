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

namespace MayaNetPlugin
{
    class apiSimpleShapeUI : MPxSurfaceShapeUI
    {
        const int LEAD_COLOR = 18; // green
        const int ACTIVE_COLOR = 15; // white
        const int ACTIVE_AFFECTED_COLOR = 8; // purple
        const int DORMANT_COLOR = 4; // blue
        const int HILITE_COLOR = 17; // pale blue
        const int DORMANT_VERTEX_COLOR = 8;   // purple
        const int ACTIVE_VERTEX_COLOR = 16;  // yellow
        public void ff(ref int[]p)
        {
            p = new int[10];
            p[0] = 1;
        }
        public override void getDrawRequests(MDrawInfo info,
                                     bool objectAndActiveOnly,
                                     MDrawRequestQueue queue)
        {
            apiSimpleShape shapeNode = surfaceShape as apiSimpleShape;
            if (shapeNode == null)
                return;

            // This call creates a prototype draw request that we can fill
            // in and then add to the draw queue.
            //
            MDrawRequest request = info.getPrototype(this);


            MDrawData data;
            MVectorArray geomPtr = shapeNode.controlPoints;

            // Stuff our data into the draw request, it'll be used when the drawing
            // actually happens
            getDrawData(geomPtr, out data);

            request.setDrawData(data);

            // Decode the draw info and determine what needs to be drawn
            //

            M3dView.DisplayStyle appearance = info.displayStyle;
            M3dView.DisplayStatus displayStatus = info.displayStatus;

            switch (appearance)
            {
                case M3dView.DisplayStyle.kWireFrame:
                    {
                        request.token = (int)DrawShapeStyle.kDrawWireframe;

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

                        queue.add(request);

                        break;
                    }

                case M3dView.DisplayStyle.kGouraudShaded:
                    {
                        // Create the smooth shaded draw request
                        //
                        request.token = (int)DrawShapeStyle.kDrawSmoothShaded;

                        // Need to get the material info
                        //
                        MDagPath path = info.multiPath;   // path to your dag object 
                        M3dView view = info.view;        // view to draw to
                        MMaterial material = base.material(path);

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
                        material.getHasTransparency(ref materialTransparent);
                        if (materialTransparent)
                        {
                            request.isTransparent = true;
                        }

                        queue.add(request);

                        // create a draw request for wireframe on shaded if
                        // necessary.
                        //
                        if ((displayStatus == M3dView.DisplayStatus.kActive) ||
                             (displayStatus == M3dView.DisplayStatus.kLead) ||
                             (displayStatus == M3dView.DisplayStatus.kHilite))
                        {
                            MDrawRequest wireRequest = request;
                            wireRequest.setDrawData(data);
                            wireRequest.token = (int)DrawShapeStyle.kDrawWireframeOnShaded;
                            wireRequest.displayStyle = M3dView.DisplayStyle.kWireFrame;

                            M3dView.ColorTable activeColorTable = M3dView.ColorTable.kActiveColors;

                            switch (displayStatus)
                            {
                                case M3dView.DisplayStatus.kLead:
                                    wireRequest.setColor(LEAD_COLOR, (int)activeColorTable);
                                    break;
                                case M3dView.DisplayStatus.kActive:
                                    wireRequest.setColor(ACTIVE_COLOR, (int)activeColorTable);
                                    break;
                                case M3dView.DisplayStatus.kHilite:
                                    wireRequest.setColor(HILITE_COLOR, (int)activeColorTable);
                                    break;

                                default:
                                    break;
                            }

                            queue.add(wireRequest);
                        }

                        break;
                    }

                case M3dView.DisplayStyle.kFlatShaded:
                    request.token = (int)DrawShapeStyle.kDrawFlatShaded;
                    break;

                default:
                    break;

            }

            // Add draw requests for components
            //
            if (!objectAndActiveOnly)
            {

                // Inactive components
                //
                if ((appearance == M3dView.DisplayStyle.kPoints) ||
                     (displayStatus == M3dView.DisplayStatus.kHilite))
                {
                    MDrawRequest vertexRequest = request;
                    vertexRequest.setDrawData(data);
                    vertexRequest.token = (int)DrawShapeStyle.kDrawVertices;
                    vertexRequest.setColor(DORMANT_VERTEX_COLOR, (int)M3dView.ColorTable.kActiveColors);

                    queue.add(vertexRequest);
                }

                // Active components
                //
                if (shapeNode.hasActiveComponents)
                {

                    MDrawRequest activeVertexRequest = request;
                    activeVertexRequest.setDrawData(data);
                    activeVertexRequest.token = (int)DrawShapeStyle.kDrawVertices;
                    activeVertexRequest.setColor(ACTIVE_VERTEX_COLOR, (int)M3dView.ColorTable.kActiveColors);

                    MObjectArray clist = shapeNode.activeComponents;
                    MObject vertexComponent = clist[0]; // Should filter list
                    activeVertexRequest.component = vertexComponent;

                    queue.add(activeVertexRequest);
                }
            }
        }

        // Main draw routine. Gets called by maya with draw requests.
        //
        public override void draw(MDrawRequest request, M3dView view)
        {
            // Get the token from the draw request.
            // The token specifies what needs to be drawn.
            //
            DrawShapeStyle token = (DrawShapeStyle)request.token;
            switch (token)
            {
                case DrawShapeStyle.kDrawWireframe:
                case DrawShapeStyle.kDrawWireframeOnShaded:
                case DrawShapeStyle.kDrawVertices:
                    drawVertices(request, view);
                    break;

                case DrawShapeStyle.kDrawSmoothShaded:
                    break;          // Not implemented, left as exercise

                case DrawShapeStyle.kDrawFlatShaded: // Not implemented, left as exercise
                    break;
            }
        }

        // Main selection routine
        //
        public override bool select(MSelectInfo selectInfo,
                                MSelectionList selectionList,
                                MPointArray worldSpaceSelectPts)
        {
            bool selected = false;
            bool componentSelected = false;
            bool hilited = false;
            apiSimpleShape shapeNode = surfaceShape as apiSimpleShape;
            if (shapeNode == null)
                return false;
            hilited = (selectInfo.displayStatus == M3dView.DisplayStatus.kHilite);
            if (hilited)
            {
                componentSelected = selectVertices(selectInfo, selectionList, worldSpaceSelectPts);
                selected = selected || componentSelected;
            }

            if (!selected)
            {
                // NOTE: If the geometry has an intersect routine it should
                // be called here with the selection ray to determine if the
                // the object was selected.

                selected = true;
                MSelectionMask priorityMask = new MSelectionMask(MSelectionMask.SelectionType.kSelectNurbsSurfaces);
                MSelectionList item = new MSelectionList();
                item.add(selectInfo.selectPath);
                MPoint xformedPt;
                if (selectInfo.singleSelection)
                {
                    MPoint center = shapeNode.boundingBox().center;
                    xformedPt = center.multiply(selectInfo.selectPath.inclusiveMatrix);
                }
                else
                    xformedPt = new MPoint();

                selectInfo.addSelection(item, xformedPt, selectionList,
                                         worldSpaceSelectPts, priorityMask, false);
            }

            return selected;
        }

        // Vertex point size
        const float POINT_SIZE = 2.0f;
        private unsafe void getLastPointSize( out float size)
        {
            float[] temp = new float[1];
            OpenGL.glGetFloatv(OpenGL.GL_POINT_SIZE, temp);
            size = temp[0];
        }
        public void drawVertices(MDrawRequest request, M3dView view)
        {
            MDrawData data = request.drawData();
            MVectorArray geom = data.geometry() as MVectorArray;

            view.beginGL();

            // Query current state so it can be restored
            //
            bool lightingWasOn = OpenGL.glIsEnabled(OpenGL.GL_LIGHTING) != 0 ? true : false;
            if (lightingWasOn)
            {
                OpenGL.glDisable(OpenGL.GL_LIGHTING);
            }
            float lastPointSize;
            getLastPointSize(out lastPointSize);

            // Set the point size of the vertices
            //
            OpenGL.glPointSize(POINT_SIZE);

            // If there is a component specified by the draw request
            // then loop over comp (using an MFnComponent class) and draw the
            // active vertices, otherwise draw all vertices.
            //
            MObject comp = request.component;
            if (!comp.isNull)
            {
                MFnSingleIndexedComponent fnComponent = new MFnSingleIndexedComponent(comp);
                for (int i = 0; i < fnComponent.elementCount; i++)
                {
                    int index = fnComponent.element(i);
                    OpenGL.glBegin(OpenGL.GL_POINTS);
                    MVector point = geom[index];
                    OpenGL.glVertex3f((float)point[0],
                                (float)point[1],
                                (float)point[2]);
                    OpenGL.glEnd();

                    MPoint mp = new MPoint(point);
                    view.drawText(String.Format("{0}", index), mp);
                }
            }
            else
            {
                for (int i = 0; i < geom.length; i++)
                {
                    OpenGL.glBegin(OpenGL.GL_POINTS);
                    MVector point = geom[i];
                    OpenGL.glVertex3f((float)point[0], (float)point[1], (float)point[2]);
                    OpenGL.glEnd();
                }
            }

            // Restore the state
            //
            if (lightingWasOn)
            {
                OpenGL.glEnable(OpenGL.GL_LIGHTING);
            }
            OpenGL.glPointSize(lastPointSize);

            view.endGL();
        }

        public bool selectVertices(MSelectInfo selectInfo,
                    MSelectionList selectionList,
                    MPointArray worldSpaceSelectPts)
        {
            bool selected = false;
            M3dView view = selectInfo.view;

            MPoint xformedPoint;
            MPoint currentPoint = new MPoint();
            MPoint selectionPoint = new MPoint();
            double z, previousZ = 0.0;
            int closestPointVertexIndex = -1;

            MDagPath path = selectInfo.multiPath;

            // Create a component that will store the selected vertices
            //
            MFnSingleIndexedComponent fnComponent = new MFnSingleIndexedComponent();
            MObject surfaceComponent = fnComponent.create(MFn.Type.kMeshVertComponent);
            int vertexIndex;

            // if the user did a single mouse click and we find > 1 selection
            // we will use the alignmentMatrix to find out which is the closest
            //
            MMatrix alignmentMatrix = new MMatrix();
            MPoint singlePoint = new MPoint();
            bool singleSelection = selectInfo.singleSelection;
            if (singleSelection)
            {
                alignmentMatrix = selectInfo.alignmentMatrix;
            }

            // Get the geometry information
            //
            apiSimpleShape shape = surfaceShape as apiSimpleShape;
            MVectorArray geom = shape.controlPoints;


            // Loop through all vertices of the mesh and
            // see if they lie withing the selection area
            //
            int numVertices = (int)geom.length;
            for (vertexIndex = 0; vertexIndex < numVertices; vertexIndex++)
            {
                MVector point = geom[vertexIndex];

                // Sets OpenGL's render mode to select and stores
                // selected items in a pick buffer
                //
                view.beginSelect();

                OpenGL.glBegin(OpenGL.GL_POINTS);
                OpenGL.glVertex3f((float)point[0],
                            (float)point[1],
                            (float)point[2]);
                OpenGL.glEnd();

                if (view.endSelect() > 0) // Hit count > 0
                {
                    selected = true;

                    if (singleSelection)
                    {
                        xformedPoint = currentPoint;
                        xformedPoint.homogenize();
                        xformedPoint = xformedPoint.multiply(alignmentMatrix);
                        z = xformedPoint.z;
                        if (closestPointVertexIndex < 0 || z > previousZ)
                        {
                            closestPointVertexIndex = vertexIndex;
                            singlePoint = currentPoint;
                            previousZ = z;
                        }
                    }
                    else
                    {
                        // multiple selection, store all elements
                        //
                        fnComponent.addElement(vertexIndex);
                    }
                }
            }

            // If single selection, insert the closest point into the array
            //
            if (selected && selectInfo.singleSelection)
            {
                fnComponent.addElement(closestPointVertexIndex);

                // need to get world space position for this vertex
                //
                selectionPoint = singlePoint;
                selectionPoint = selectionPoint.multiply(path.inclusiveMatrix);
            }

            // Add the selected component to the selection list
            //
            if (selected)
            {
                MSelectionList selectionItem = new MSelectionList();
                selectionItem.add(path, surfaceComponent);

                MSelectionMask mask = new MSelectionMask(MSelectionMask.SelectionType.kSelectComponentsMask);
                selectInfo.addSelection(
                    selectionItem, selectionPoint,
                    selectionList, worldSpaceSelectPts,
                    mask, true);
            }

            return selected;
        }

        private enum DrawShapeStyle
        {
            kDrawVertices, // component token
            kDrawWireframe,
            kDrawWireframeOnShaded,
            kDrawSmoothShaded,
            kDrawFlatShaded,
            kLastToken
        };

    }
}
