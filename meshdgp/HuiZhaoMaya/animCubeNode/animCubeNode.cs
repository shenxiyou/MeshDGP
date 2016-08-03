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


// Note:
//		This C# plugin is ported from: $(MAYADIR)\devkit\plug-ins\animCubeNode
[assembly: MPxNodeClass(typeof(MayaNetPlugin.animCube), "animCubeCSharp", 0x0008105a)]

namespace MayaNetPlugin
{
    class animCube : MPxNode, IMPxNode
    {
        static MObject time;
        static MObject outputMesh;

        [MPxNodeInitializer()]
        public static void initialize()
        {
            MFnUnitAttribute unitAttr = new MFnUnitAttribute();
            MFnTypedAttribute typedAttr = new MFnTypedAttribute();

            animCube.time = unitAttr.create("time", "tm", MFnUnitAttribute.Type.kTime, 0.0);

            animCube.outputMesh = typedAttr.create("outputMesh", "out", MFnData.Type.kMesh);
            typedAttr.isStorable = false;

            addAttribute(animCube.time);
            addAttribute(animCube.outputMesh);

            attributeAffects(animCube.time, animCube.outputMesh);
        }

        protected MObject createMesh(MTime time, ref MObject outData)
        {
            int numVertices, frame;
            float cubeSize;
            MFloatPointArray points = new MFloatPointArray();
            MFnMesh meshFS = new MFnMesh();

            // Scale the cube on the frame number, wrap every 10 frames.
            frame = (int)time.asUnits(MTime.Unit.kFilm);
            if (frame == 0)
                frame = 1;
            cubeSize = 0.5f * (float)(frame % 10);

            const int numFaces = 6;
            numVertices = 8;

            MFloatPoint vtx_1 = new MFloatPoint(-cubeSize, -cubeSize, -cubeSize);
            MFloatPoint vtx_2 = new MFloatPoint(cubeSize, -cubeSize, -cubeSize);
            MFloatPoint vtx_3 = new MFloatPoint(cubeSize, -cubeSize, cubeSize);
            MFloatPoint vtx_4 = new MFloatPoint(-cubeSize, -cubeSize, cubeSize);
            MFloatPoint vtx_5 = new MFloatPoint(-cubeSize, cubeSize, -cubeSize);
            MFloatPoint vtx_6 = new MFloatPoint(-cubeSize, cubeSize, cubeSize);
            MFloatPoint vtx_7 = new MFloatPoint(cubeSize, cubeSize, cubeSize);
            MFloatPoint vtx_8 = new MFloatPoint(cubeSize, cubeSize, -cubeSize);
            points.append(vtx_1);
            points.append(vtx_2);
            points.append(vtx_3);
            points.append(vtx_4);
            points.append(vtx_5);
            points.append(vtx_6);
            points.append(vtx_7);
            points.append(vtx_8);


            // Set up an array containing the number of vertices
            // for each of the 6 cube faces (4 verticies per face)
            //
            int[] face_counts = { 4, 4, 4, 4, 4, 4 };
            MIntArray faceCounts = new MIntArray(face_counts);

            // Set up and array to assign vertices from points to each face 
            //
            int[] face_connects = {	0, 1, 2, 3,
									4, 5, 6, 7,
									3, 2, 6, 5,
									0, 3, 5, 4,
									0, 4, 7, 1,
									1, 7, 6, 2	};
            MIntArray faceConnects = new MIntArray(face_connects);

            MObject newMesh = meshFS.create(numVertices, numFaces, points, faceCounts, faceConnects, outData);

            return newMesh;
        }

        public override bool compute(MPlug plug, MDataBlock dataBlock)
        {
            if (plug.equalEqual(animCube.outputMesh))
            {
                /* Get time */
                MDataHandle timeData = dataBlock.inputValue(animCube.time);
                MTime time = timeData.asTime;

                /* Get output object */

                MDataHandle outputHandle = dataBlock.outputValue(outputMesh);

                MFnMeshData dataCreator = new MFnMeshData();
                MObject newOutputData = dataCreator.create();

                createMesh(time, ref newOutputData);

                outputHandle.set(newOutputData);
                dataBlock.setClean(plug);
            }
            else
                return false;

            return true;
        }
    }
}
