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

[assembly: MPxNodeClass(typeof(MayaNetPlugin.sineNode), "sineCSharp", 0x00081069)]

namespace MayaNetPlugin
{
    //[MPxNodeAffects("input", "output")]
    class sineNode : MPxNode, IMPxNode
    {
        [MPxNodeNumeric("in", "input", MFnNumericData.Type.kFloat, Storable = true)]
        public static MObject input = null;

        [MPxNodeNumeric("out", "output", MFnNumericData.Type.kFloat, Storable = false, Writable = false)]
        [MPxNodeAffectedBy("input")]
        public static MObject output = null;

        public sineNode() { }

        override public bool compute(MPlug plug, MDataBlock dataBlock)
        {
            bool res = plug.attribute.equalEqual(output);

            if (res)
            {
                MDataHandle inputData;
                inputData = dataBlock.inputValue(input);

                MDataHandle outputHandle = dataBlock.outputValue(output);
                outputHandle.asFloat = 10 * (float)Math.Sin((double)inputData.asFloat);
                dataBlock.setClean(plug);
                return true;
            }

            return false;
        }
    }
}
