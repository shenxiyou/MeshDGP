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
using Autodesk.Maya.OpenMayaUI;
using Autodesk.Maya.OpenMayaRender;

[assembly: MPxNodeClass(typeof(MayaNetPlugin.customImagePlane), "customImagePlaneCSharp", 0x0008105d,
    NodeType = Autodesk.Maya.OpenMaya.MPxNode.NodeType.kImagePlaneNode)]

namespace MayaNetPlugin
{
	class customImagePlane : MPxImagePlane
	{
		private double fTransparency;

        [MPxNodeNumeric("tp", "transparency", MFnNumericData.Type.kDouble, Keyable = true, Internal = true, SoftMin = 0.0, SoftMax = 1.0)]
        [MPxNumericDefault( 0.0)]
		public static MObject aTransparency = null;

		private unsafe void blendPixel(MImage image, uint size)
		{
			byte* pixels = image.pixels();
			uint i;
			for (i = 0; i < size; i++, pixels += 4)
			{
				pixels[3] = (byte)(pixels[3] * (1.0 - fTransparency));
			}
		}

		private void setDepthMap(MImage image, uint width, uint height)
		{
			float [] buffer = new float[width * height];
			uint c, j, i;
			for (c = i = 0; i < height; i++)
			{
				for (j = 0; j < width; j++, c++)
				{
					if (i > height / 2)
					{
						buffer[c] = -1.0f;
					}
					else
					{
						buffer[c] = 0.0f;
					}
				}
			}
			image.setDepthMap(buffer, width, height);
		}
		public override void loadImageMap(string fileName, int frame, MImage image)
		{
			image.readFromFile(fileName);

			uint width;
			uint height;
			image.getSize(out width, out height);
			uint size = width * height;

			blendPixel(image, size);

			MPlug depthMap = new MPlug(thisMObject(), useDepthMap);
			bool value = false;
			depthMap.getValue(ref value);

			if (value)
			{
				setDepthMap(image, width, height);
			}
			return;
		}
		public override bool setInternalValueInContext(MPlug plug, MDataHandle dataHandle, MDGContext ctx)
		{
			if (plug.equalEqual(aTransparency))
			{
				fTransparency = dataHandle.asDouble;
				setImageDirty();
				return true;
			}

			return base.setInternalValueInContext(plug, dataHandle, ctx);
		}
		public override bool getInternalValueInContext(MPlug plug, MDataHandle dataHandle, MDGContext ctx)
		{
			if (plug.equalEqual(aTransparency))
			{
				dataHandle.set(fTransparency);
				return true;
			}
			return base.getInternalValueInContext(plug, dataHandle, ctx);
		}
	}
}
