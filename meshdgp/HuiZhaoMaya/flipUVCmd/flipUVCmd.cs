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
//		This C# plugin is ported from: $(MAYADIR)\devkit\plug-ins\flipUVCmd
//

using System;

 
using Autodesk.Maya.OpenMaya;


[assembly: MPxPolyTweakUVCommandClass(typeof(MayaNetPlugin.flipUVcmd), "flipUVCmdCSharp")]

namespace MayaNetPlugin
{
    [MPxCommandSyntaxFlag(flipUVcmd.horizFlag, flipUVcmd.horizFlagLong, Arg1 = typeof(System.Boolean))]
    [MPxCommandSyntaxFlag(flipUVcmd.globalFlag, flipUVcmd.globalFlagLong, Arg1 = typeof(System.Boolean))]
    [MPxCommandSyntaxFlag(flipUVcmd.extendFlag, flipUVcmd.extendFlagLong, Arg1 = typeof(System.Boolean))]

    public class flipUVcmd : MPxPolyTweakUVCommand, IMPxCommand
    {
        // Flags for this command
        const string horizFlag			= "-h";
        const string horizFlagLong		= "-horizontal";
        const string globalFlag = "-fg";
        const string globalFlagLong = "-flipGlobal";
        const string extendFlag = "-es";
        const string extendFlagLong = "-extendToShell";

        const string cmdName = "flipUV";
        bool horizontal = false;						// Axis to flip
        bool extendToShell = false;						// Extend selection to the
        // whole shell
        bool flipGlobal = false;						// Flip globally or per shell


        public override void parseSyntax(MArgDatabase argData)
        {
            if (argData.isFlagSet(horizFlag))
                horizontal =argData.flagArgumentBool(horizFlag, 0);

            if (argData.isFlagSet(globalFlag))
                flipGlobal = argData.flagArgumentBool(globalFlag, 0);

            if (argData.isFlagSet(extendFlag))
                extendToShell = argData.flagArgumentBool(extendFlag, 0);

            return;
        }

        unsafe public override void getTweakedUVs(MObject meshObj, MIntArray uvList, MFloatArray uPos, MFloatArray vPos)
        {
            int i = 0;
            MFloatArray uArray = new MFloatArray();
            MFloatArray vArray = new MFloatArray();
            MFnMesh mesh = new MFnMesh(meshObj);
            mesh.getUVs(uArray, vArray);

            uint nbUvShells = 0;
            MIntArray uvShellIds = new MIntArray();

            if ((!flipGlobal) || extendToShell)
                // First, extract the UV shells.
                mesh.getUvShellsIds(uvShellIds, ref nbUvShells);

            if (extendToShell)
            {
                bool[] selected = new bool[nbUvShells];
                for (i = 0; i < nbUvShells; i++)
                {
                    selected[i] = false;
                }

                for (i = 0; i < nbUvShells; i++)
                {
                    int index = uvList[i];
                    index = uvShellIds[index];
                    selected[index] = true;
                }

                uint numUvs = (uint)mesh.numUVsProperty;
                uint numSelUvs = 0;

                // Preallocate a buffer, large enough to hold all Ids. This
                // prevents multiple reallocation from happening when growing
                // the array.
                uvList.length = numUvs;

                for (i = 0; i < numUvs; i++)
                {
                    int index = uvShellIds[i];
                    if (selected[index])
                    {
                        uvList.set((int)i, numSelUvs);
                        numSelUvs++;
                    }
                }

                // clamp the array to the proper size.
                uvList.length = numSelUvs;
            }

            int nbUvShellsInt = (int)nbUvShells;
            // For global flips, just pretend there is only one shell
            if (flipGlobal)
                nbUvShellsInt = 1;

            float[] minMax = new float[nbUvShellsInt * 4];

            for (i = 0; i < nbUvShellsInt; i++)
            {
                minMax[4 * i + 0] = 1e30F;				// Min U
                minMax[4 * i + 1] = 1e30F;				// Min V
                minMax[4 * i + 2] = -1e30F;				// Max U
                minMax[4 * i + 3] = -1e30F;				// Max V
            }

            // Get the bounding box of the UVs, for each shell if flipGlobal
            // is true, or for the whole selection if false.
            for (i = 0; i < uvList.length; i++)
            {
                int indx = uvList[i];
                int shellId = 0;

                if (!flipGlobal)
                {
                    shellId = uvShellIds[indx];
                }

                float value = uArray[indx];
                
                if (value < minMax[4 * shellId + 0])
                    minMax[4 * shellId + 0] = value;

                value = vArray[indx];
                if (value < minMax[4 * shellId + 1])
                    minMax[4 * shellId + 1] = value;

                value = uArray[indx];
                if (value > minMax[4 * shellId + 2])
                    minMax[4 * shellId + 2] = value;

                value = vArray[indx];
                if (value > minMax[4 * shellId + 3])
                    minMax[4 * shellId + 3] = value;
            }

            // Adjust the size of the output arrays
            uPos.length = uvList.length;
            vPos.length = uvList.length;

            for (i = 0; i < uvList.length; i++)
            {
                int shellId = 0;

                int indx = uvList[i];

                if (!flipGlobal)
                    shellId = uvShellIds[indx];

                // Flip U or V along the bounding box center.
                if (horizontal)
                {
                    float value = uArray[indx];
                    value = minMax[4 * shellId + 0] + minMax[4 * shellId + 2] - value;

                    uPos.set(value, (uint)i);
                    value = vArray[indx];
                    vPos.set(value, (uint)i);
                }
                else
                {
                    float value = uArray[indx];
                    uPos.set(value, (uint)i);

                    value = vArray[indx];
                    value = minMax[4 * shellId + 1] + minMax[4 * shellId + 3] - value;
                    vPos.set(value, (uint)i);
                }
            }
            return;
        }
    }

    
}
