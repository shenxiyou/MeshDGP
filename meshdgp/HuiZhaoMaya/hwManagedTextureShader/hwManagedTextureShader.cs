// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


//////////////////////////////////////////////////////////////////
//
// This plugin demonstrates how to create and register an HW shader
// node.
//
// To use the shader, create a polygon and assign it with HW Managed
// Texture Shader. For more detail, please refer to the accompanying
// MEL script.
//
//////////////////////////////////////////////////////////////////

using System;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaUI;
using Autodesk.Maya.OpenMayaRender;

[assembly: MPxNodeClass(typeof(MayaNetPlugin.hwManagedTextureShader), "hwManagedTextureShaderCSharp",
	0x00081062,
	Classification = "shader/surface/utility/",
	NodeType = Autodesk.Maya.OpenMaya.MPxNode.NodeType.kHwShaderNode)]

namespace MayaNetPlugin
{
	class hwManagedTextureShader : MPxHwShaderNode, IMPxNode
	{
		public override void postConstructor()
		{
			_setMPSafe(false);
		}

		public override bool compute(MPlug plug, MDataBlock dataBlock)
		{
			return true;
		}

		public override void glBind(MDagPath shapePath)
		{
			// ONLY push and pop required attributes performance reasons...
			//
			OpenGL.glPushAttrib(OpenGL.GL_LIGHTING_BIT);

			lightingOn = OpenGL.glIsEnabled(OpenGL.GL_LIGHTING);
			if (lightingOn > 0)
			{
				OpenGL.glEnable(OpenGL.GL_COLOR_MATERIAL);
				OpenGL.glColorMaterial(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_DIFFUSE);
			}

			// Base colour is always white
			OpenGL.glColor4f(1.0f, 1.0f, 1.0f, 1.0f);

			// Bind texture
			if(libOpenMayaNet.MAYA_API_VERSION >= 800)
			{
				MObject l_object =  shapePath.node;
				MFnMesh mesh = new MFnMesh(l_object);
				String uvSetName = "map1";
				MObjectArray textures = new MObjectArray();

				boundTexture = false;
				mesh.getAssociatedUVSetTextures(uvSetName, textures);
				if (textures.length > 0)
				{
					MImageFileInfo.MHwTextureType hwType = new MImageFileInfo.MHwTextureType();
					MHwTextureManager.glBind(textures[0], ref hwType);
					boundTexture = true;
				}

				if( !boundTexture)
				{
					OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
					OpenGL.glBindTexture(OpenGL.GL_TEXTURE_2D, 0);
				}
			}
			else
			{
				// To get this code branch to compile, replace <change file name here>
				// with an appropriate file name
				if (id == 0)
				{
					MImage fileImage = new MImage();
					fileImage.readFromFile("<change file name here>");
					uint[] param = new uint[1];
					OpenGL.glGenTextures(1, param);
					id = param[0];
					OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
					OpenGL.glBindTexture(OpenGL.GL_TEXTURE_2D, id);
					unsafe
					{
						uint width = 0, height = 0;
						fileImage.getSize(out width, out height);
						byte* pPixels = fileImage.pixels();
						OpenGL.glTexImage2D(OpenGL.GL_TEXTURE_2D,
											0,
											(int)OpenGL.GL_RGBA8,
											(int)width,
											(int)height,
											0,
											OpenGL.GL_RGBA,
											OpenGL.GL_UNSIGNED_BYTE,
											pPixels);
					}
				}
				else
				{
					OpenGL.glEnable(OpenGL.GL_TEXTURE_2D);
					OpenGL.glBindTexture(OpenGL.GL_TEXTURE_2D, id);
				}
				boundTexture = true;

			}

			if( boundTexture)
			{
				OpenGL.glTexParameteri(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, (int)OpenGL.GL_REPEAT);
				OpenGL.glTexParameteri(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, (int)OpenGL.GL_REPEAT);
				OpenGL.glTexParameteri(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, (int)OpenGL.GL_LINEAR);
				OpenGL.glTexParameteri(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, (int)OpenGL.GL_LINEAR);
			}
			OpenGL.glEnableClientState(OpenGL.GL_VERTEX_ARRAY);

			return;
		}

		public override void glUnbind(MDagPath shapePath)
		{
			// Cleanup GL state, without using pushing and popping attributes
			//
			OpenGL.glDisableClientState(OpenGL.GL_VERTEX_ARRAY);
			if (lightingOn > 0)
			{
				OpenGL.glDisable(OpenGL.GL_COLOR_MATERIAL);
				OpenGL.glDisableClientState(OpenGL.GL_NORMAL_ARRAY);
			}
			if (boundTexture)
			{
				OpenGL.glDisableClientState( OpenGL.GL_TEXTURE_COORD_ARRAY );
				OpenGL.glDisable(OpenGL.GL_TEXTURE_2D);
				OpenGL.glBindTexture(OpenGL.GL_TEXTURE_2D, 0);
			}

			// ONLY push and pop required attributes performance reasons...
			//
			OpenGL.glPopAttrib();
			return;
		}

		public override bool supportsBatching()
		{
			return true;
		}

		public unsafe override void glGeometry(MDagPath shapePath,
			int glPrim,
			uint writeMask,
			int indexCount,
			uint* indexArray,
			int vertexCount,
			int* vertexIDs,
			float* vertexArray,
			int normalCount,
			float** normalArrays,
			int colorCount,
			float** colorArrays,
			int texCoordCount,
			float** texCoordArrays)
		{
			// converting from SWIG_p_xxx to the format gl needed.
			//
			OpenGL.glVertexPointer(3, OpenGL.GL_FLOAT, 0, vertexArray);

			if (boundTexture && texCoordCount > 0 && (texCoordArrays)[0] != (void*)0)
			{
				OpenGL.glEnableClientState(OpenGL.GL_TEXTURE_COORD_ARRAY);
				OpenGL.glTexCoordPointer(2, OpenGL.GL_FLOAT, 0, texCoordArrays[0]);
			}
			else
			{
				OpenGL.glDisableClientState(OpenGL.GL_TEXTURE_COORD_ARRAY);
			}

			if (lightingOn > 0 && normalCount > 0 && normalArrays[0][0] > 0)
			{
				// Don't route normals if we don't need them
				OpenGL.glEnableClientState(OpenGL.GL_NORMAL_ARRAY);
				OpenGL.glNormalPointer(OpenGL.GL_FLOAT, 0, normalArrays[0]);
			}
			else
			{
				OpenGL.glDisableClientState(OpenGL.GL_NORMAL_ARRAY);
			}

			OpenGL.glDrawElements((uint)glPrim, indexCount, OpenGL.GL_UNSIGNED_INT, indexArray);
			return;
		}

		public override int texCoordsPerVertex()
		{
			return 1;
		}

		public override int normalsPerVertex()
		{
			return 1;
		}

        [MPxNodeInitializer()]
		public static bool initialize()
		{
			return true;
		}

		public byte lightingOn;
		public bool boundTexture;
		static uint id = 0;
	}
}
