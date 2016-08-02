// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


// Note:
//		This C# plugin is ported from: $(MAYADIR)\devkit\plug-ins\polyTrgNode

/////////////////////////////////////////////////////////////////////////////
//
// polyTrgNodeCSharp
//
// Description:
//		This node register a user defined face triangulation function.
//		After the function is register it can be used by any mesh in 
//		the scene to do the triangulation (replace the mesh native 
//		triangulation). In order for the mesh to use this function,
//		an attribute on the mesh: 'userTrg' has to be set to the name
// 		of the function. 
//	
//		Different meshes may use different functions. Each of them 
//		needs to be register. The same node can provide more than 
//		one function.  
//
// Example:
//		createNode polyTrgNodeCSharp -n ptrg;
// 
//		polyPlane -w 1 -h 1 -sx 10 -sy 10 -ax 0 1 0 -tx 0 -ch 1 -n pp1;
//
//		select  -r pp1Shape;
//		setAttr pp1Shape.userTrg  -type "string" "triangulate";
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.InteropServices;

using Autodesk.Maya.OpenMaya;


[assembly: MPxNodeClass(typeof(MayaNetPlugin.polyTrgNode), "polyTrgNodeCSharp", 0x00081066)]

namespace MayaNetPlugin
{
    public class polyTrgNode : MPxPolyTrg, IMPxNode
    {
        private MxPolyTrgFnct del = null;
        ~polyTrgNode()
        //	Description:
        //		Destructor: unregister the triangulation function. 
        //
        {
            unregisterTrgFunction("triangulate");
        }
        public override bool compute(MPlug plug, MDataBlock dataBlock)
        {
            return true;
        }

        unsafe public override void postConstructor()
        //	Description:
        //		Constructor: register the triangulation function. 
        //
        {
            // Register the triangulation function.
            // The string given as a first parameter has to be
            // the same as the name given when setting the usrTrg
            // attribute on the mesh. See example above.
            // 
            HandleRef polyHandle = polyTrgNode.getCPtr(this);

            del = new MxPolyTrgFnct(polyTrgNode.triangulateFace);
            
            string name = "triangulate";
            registerTrgFunction(name, del);
        }

        public override bool isAbstractClass()
        {
            return false;
        } 

        unsafe public static void triangulateFace(
            float* vert, // I: face vertex position
            float* norm, // I: face normals per vertex
            int* loopSizes, // I: number of vertices per loop 
            int nbLoops, // I: number of loops in the face	
            int nbTrg, // I: number of triangles to generate
            ushort* trg // O: triangles - size = 3*nbTrg. 
                        //    Note: this array is already allocated.
            )
        //  Description:
        //		Triangulate a given face. Returns triangles given by 
        //		the relative vertex ids. Example:
        //	   		nbTrg = 2
        //	   		trg: 0, 1, 2,  2, 3, 0
        //
        {
            // Print the input.
            //
            MGlobal.displayInfo("polyTrgNode::triangulate() - This is an API registered triangulation.");

            // Dump the data - this is a good example.
            //
            MGlobal.displayInfo("Numb Loops =" + nbLoops);
            int nbVert = 0;
            MGlobal.displayInfo("Loop sizes");
            for (int i = 0; i < nbLoops; i++ )
            {
                nbVert += loopSizes[i];
                MGlobal.displayInfo(loopSizes[i] + " ");
            }
            MGlobal.displayInfo("Numb Vert="+ nbVert);
            MGlobal.displayInfo("Vertices:");
            for (int v = 0; v < nbVert; v++ )
            {
                MGlobal.displayInfo(vert[3 * v] + " " + vert[3 * v + 1] + " " + vert[3 * v + 2] + " ");
            }

            // Now triangulate.
            // 
            MGlobal.displayInfo("nbTrg=" + nbTrg);

            // Generate a triangulation for this face.
            // 
            ushort v0 = 0;
            ushort v1 = 1;
            ushort v2 = 2;
            for (int k = 0; k < nbTrg; k++ )
            {
                trg[3 * k] = v0;
                trg[3 * k + 1] = v1;
                trg[3 * k + 2] = v2;
                v1 = v2;
                v2++;
                if (v2 >= nbVert)
                {
                    v2 = 0;
                }
            }

            // Print the result.
            //
            MGlobal.displayInfo("Triangulation");
            for (int k1 = 0; k1 < nbTrg; k1++ )
            {
                MGlobal.displayInfo(trg[3 * k1] + " " + trg[3 * k1 + 1] + " " + trg[3 * k1 + 2]);
            }
        }
    }

   
}
