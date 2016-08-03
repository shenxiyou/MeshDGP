// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


using Autodesk.Maya.OpenMaya;

namespace MayaNetPlugin
{
	class apiMeshGeomUV
	{
		public MIntArray faceVertexIndex;
		public MFloatArray ucoord;
		public MFloatArray vcoord;

		public apiMeshGeomUV()
		{
			faceVertexIndex = new MIntArray();
			ucoord = new MFloatArray();
			vcoord = new MFloatArray();

			reset();
		}
		
		public void reset()
		{
			ucoord.clear();
			vcoord.clear();
			faceVertexIndex.clear();
		}

		public int uvId(int fvi)
		{
			return faceVertexIndex[fvi];
		}

		public void getUV(int uvId, ref float u, ref float v)
		{
			u = ucoord[uvId];
			v = vcoord[uvId]; 
		}

		public float u(int uvId)
		{
			return ucoord[uvId];
		}

		public float v(int uvId)
		{
			return vcoord[uvId];
		}

		public uint uvcount()
		{
			return ucoord.length;
		}

		public void append_uv(float u, float v)
		{
			ucoord.append( u ); 
			vcoord.append( v ); 
		}
	}

	class apiMeshGeom
	{
		public MPointArray vertices;
		public MIntArray face_counts;
		public MIntArray face_connects;
		public MVectorArray normals;
		public apiMeshGeomUV uvcoords;
		public uint faceCount;

		public apiMeshGeom()
		{
			vertices = new MPointArray();
			face_counts = new MIntArray();
			face_connects = new MIntArray();
			normals = new MVectorArray();
			uvcoords = new apiMeshGeomUV();
			faceCount = 0;
		}
	}
}
