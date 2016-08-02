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

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaRender;
using Autodesk.Maya.OpenMayaUI;

/////////////////////////////////////////////////////////////////////////////
// Steps:
//		1. Open Maya and load a mesh.
//      2. CreateNode testHLSLShaderCSharp
//      3. Assign testHLSLShaderCSharp to the Mesh.
//
/////////////////////////////////////////////////////////////////////////////

[assembly: MPxNodeClass(typeof(MayaNetPlugin.testHLSLShader), "testHLSLShaderCSharp", 0x0008106b, 
    NodeType = MPxNode.NodeType.kHardwareShader, Classification = "shader/surface/utility")]

namespace MayaNetPlugin
{
    public class testHLSLShader : MPxHardwareShader
    {
        public static MTypeId sId = new MTypeId(0xF3560C30);
        public static MRenderProfile sProfile = new MRenderProfile();

        [MPxNodeInitializer()]
        public static bool initialize()
        {
            sProfile.addRenderer(MRenderProfile.MStandardRenderer.kMayaD3D);
            return true;
        }

        public override void  postConstructor()
        {
            // Don't create any default varying parameters (e.g. position + normal) for empty
            // shaders as this just bloats the cache with a useless structure that is currently
            // not flushed out of the cache until the geometry changes. Instead, just render
            // the default geometry directly off the position array in the cache
            MGlobal.displayInfo("testHLSLShader::postConstructor");
        }

        public override bool getInternalValueInContext(MPlug plug, MDataHandle dataHandle, MDGContext ctx)
        {
            MGlobal.displayInfo("testHLSLShader::getInternalValueInContext");
            return base.getInternalValueInContext(plug, dataHandle, ctx);
        }

        public override bool setInternalValueInContext(MPlug plug, MDataHandle dataHandle, MDGContext ctx)
        {
            MGlobal.displayInfo("testHLSLShader::setInternalValueInContext");
            return base.setInternalValueInContext(plug, dataHandle, ctx);
        }

        public override MRenderProfile profile()
        {
            MGlobal.displayInfo("testHLSLShader::profile");
            return sProfile;
        }

        public override bool getAvailableImages( ShaderContext context, string uvSetName, MStringArray imageNames)
        {
            MGlobal.displayInfo("testHLSLShader::getAvailableImages");
            return base.getAvailableImages(context, uvSetName, imageNames);
        }
    }
}
