// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


/*
 Simple Image File plugin. This plugin registers a new image file format against file extension ".moo".
 Loading any ".moo" image file will produce a procedurally generated color spectrum including values
 outside 0 to 1. Steps to use this example:

   1. Build and load the plug-in.
   2. Create a file with extension "moo".
   3. From the menu of a Maya viewport, click View->Image Plane->Import Image...
   4. Open the file being created in step 2.
   5. The image will be drawn with an Image Plane.

 For more detail, please refer to the accompanying MEL script.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using Autodesk.Maya.OpenMaya;

[assembly: MPxImageFileClass(typeof(MayaNetPlugin.simpleImageFile), "SimpleImageFileCSharp")]

namespace MayaNetPlugin
{
    [MPxImageExtension("moo")]
    class simpleImageFile : MPxImageFile
    {
        [DllImport("opengl32")]
        unsafe static public extern void glTexImage2D(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, void* pixels);

        private const uint GL_TEXTURE_2D = 0x0DE1;
        private const uint GL_RGB = 0x1907;
        private const uint GL_FLOAT = 0x1406;
        private const float RAINBOW_SCALE = 4.0f;

	    public override void open(String pathname, MImageFileInfo info)
        {
	        if(info != null)
	        {
		        info.width = 512;
		        info.height = 512;
		        info.channels = 3;
                info.pixelType = MImage.MPixelType.kFloat;

		        // Only necessary if your format defines a native
		        // hardware texture loader
                info.hardwareType = MImageFileInfo.MHwTextureType.kHwTexture2D;
	        }
        }

        public override void load(MImage image, uint idx)
        {
	        uint w = 512;
	        uint h = 512;

	        // Create a floating point image and fill it with
	        // a pretty rainbow test image.
	        //
	        image.create( w, h, 3, MImage.MPixelType.kFloat);
            unsafe
            {
                float* pixels = image.floatPixels();
                populateTestImage(pixels, w, h);
            }

            return;
        }

        unsafe private void populateTestImage( float* pixels, uint w, uint h)
        {
            uint x, y;
            for (y = 0; y < h; y++)
            {
                float g = RAINBOW_SCALE * y / (float)h;
                for (x = 0; x < w; x++)
                {
                    float r = RAINBOW_SCALE * x / (float)w;
                    *pixels++ = r;
                    *pixels++ = g;
                    *pixels++ = RAINBOW_SCALE * 1.5f - r - g;
                }
            }
        }
 
        unsafe public override void glLoad(MImageFileInfo info, uint imageNumber)
        {
            unsafe
            {
	            // Create a floating point image
	            uint w = info.width;
	            uint h = info.height;
	            float[] csPixels = new float[ w * h * 3];
                IntPtr ptr = new IntPtr();
                Marshal.Copy(ptr, csPixels, 0, csPixels.Length);
                float* pixels = (float*)ptr.ToPointer();
	            populateTestImage(pixels, w, h);
	
	            // Now load it into OpenGL as a floating point image
                glTexImage2D(GL_TEXTURE_2D, 0, (int)GL_RGB, (int)w, (int)h, 0, GL_RGB, GL_FLOAT, pixels);
            }
            return;
        }
    };
}
