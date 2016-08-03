// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


////////////////////////////////////////////////////////////////////////
//
//  Description:
//     
//  Print some useful information about the files referenced
//  in the main scene.
//
//  Output format is as follows:
// 
//  Referenced File: filename1
// 		Connections Made
// 			source -> destination
// 			...
// 
// 		Connections Broken
//			source ->destination
// 			...
//
//		Attributes Changed Since File Referenced
//			attribute1
//			attribute2
//			...
//
////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaUI;
using Autodesk.Maya.OpenMayaRender;


[assembly: MPxCommandClass(typeof(MayaNetPlugin.referenceQueryCmd), "refQueryCSharp")]


namespace MayaNetPlugin
{
    class referenceQueryCmd : MPxCommand, IMPxCommand
    {
        public override void doIt(MArgList args)
        {
            MStringArray referenceFiles = new MStringArray();
            MFileIO.getReferences(referenceFiles);

            string log = "";
            for (int i = 0; i < referenceFiles.length; i++)
            {
                MStringArray connectionsMade = new MStringArray();
                MFileIO.getReferenceConnectionsMade(referenceFiles[i],
                                                      connectionsMade);

                log = string.Format("{0} Referenced File: {1}:\nConnections Made:\n", log, referenceFiles[i]);
                int j;
                for (j = 0; j < connectionsMade.length; j += 2)
                {
                    log = string.Format("	 {0} -> ", connectionsMade[j]);
                    if (j + 1 < connectionsMade.length)
                    {
                        log = log + connectionsMade[j + 1];
                    }
                    log += "\n";
                }

                MStringArray connectionsBroken = new MStringArray();
                MFileIO.getReferenceConnectionsBroken(referenceFiles[i],
                                                      connectionsBroken);

                log += "\n	Connections Broken: \n";
                for (j = 0; j < connectionsBroken.length; j += 2)
                {
                    log = string.Format("{0}	{1} -> ", log, connectionsBroken[j]);
                    if (j + 1 < connectionsBroken.length)
                    {
                        log += connectionsBroken[j + 1];
                    }
                    log += "\n";
                }
                log += "\n";

                MStringArray referencedNodes = new MStringArray();

                log += "	Attrs Changed Since File Open:\n";
                MFileIO.getReferenceNodes(referenceFiles[i], referencedNodes);
                for (j = 0; j < referencedNodes.length; j++)
                {
                    // For each node, call a MEL command to get its
                    // attributes.  Say we're only interested in scalars.
                    //
                    string cmd = string.Format("listAttr -s -cfo {0}", referencedNodes[j]);

                    MStringArray referencedAttributes = new MStringArray();

                    MGlobal.executeCommand(cmd, referencedAttributes);
                    for (int k = 0; k < referencedAttributes.length; k++)
                    {
                        log += string.Format("		{0}.{1}\n",
                            referencedNodes[j], referencedAttributes[k]);
                    }
                }
                log += "\n";
            }

            // End of output 
            //
            log += "=====================================";
            MGlobal.displayInfo(log);
            return;
        }

    }
}
