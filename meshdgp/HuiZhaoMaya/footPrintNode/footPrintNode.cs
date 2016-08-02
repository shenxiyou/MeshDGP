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

using Autodesk.Maya.OpenMayaRender.MHWRender;
using System.Runtime.InteropServices;

[assembly: MPxNodeClass(typeof(MayaNetPlugin.footPrint), "footPrintCSharp", 0x0008105f,
    NodeType = Autodesk.Maya.OpenMaya.MPxNode.NodeType.kLocatorNode, Classification = "drawdb/geometry/footPrint")]
[assembly: MPxDrawOverrideClass("drawdb/geometry/footPrint", "FootprintNodePluginCSharp", typeof(MayaNetPlugin.FootPrintDrawOverride))]

namespace MayaNetPlugin
{
	public class footPrintData
	{
		public static float[,] sole = new float[21, 3]
		{
			{  0.00f, 0.0f, -0.70f },
			{  0.04f, 0.0f, -0.69f },
			{  0.09f, 0.0f, -0.65f },
			{  0.13f, 0.0f, -0.61f },
			{  0.16f, 0.0f, -0.54f },
			{  0.17f, 0.0f, -0.46f },
			{  0.17f, 0.0f, -0.35f },
			{  0.16f, 0.0f, -0.25f },
			{  0.15f, 0.0f, -0.14f },
			{  0.13f, 0.0f,  0.00f },
			{  0.00f, 0.0f,  0.00f },
			{ -0.13f, 0.0f,  0.00f },
			{ -0.15f, 0.0f, -0.14f },
			{ -0.16f, 0.0f, -0.25f },
			{ -0.17f, 0.0f, -0.35f },
			{ -0.17f, 0.0f, -0.46f },
			{ -0.16f, 0.0f, -0.54f },
			{ -0.13f, 0.0f, -0.61f },
			{ -0.09f, 0.0f, -0.65f },
			{ -0.04f, 0.0f, -0.69f },
			{ -0.00f, 0.0f, -0.70f }
		};
		public static float[,] heel = new float[17, 3]
		{
			{  0.00f, 0.0f,  0.06f },
			{  0.13f, 0.0f,  0.06f },
			{  0.14f, 0.0f,  0.15f },
			{  0.14f, 0.0f,  0.21f },
			{  0.13f, 0.0f,  0.25f },
			{  0.11f, 0.0f,  0.28f },
			{  0.09f, 0.0f,  0.29f },
			{  0.04f, 0.0f,  0.30f },
			{  0.00f, 0.0f,  0.30f },
			{ -0.04f, 0.0f,  0.30f },
			{ -0.09f, 0.0f,  0.29f },
			{ -0.11f, 0.0f,  0.28f },
			{ -0.13f, 0.0f,  0.25f },
			{ -0.14f, 0.0f,  0.21f },
			{ -0.14f, 0.0f,  0.15f },
			{ -0.13f, 0.0f,  0.06f },
			{ -0.00f, 0.0f,  0.06f }
		};
		public static int soleCount = 21;
		public static int heelCount = 17;
	}

	public class footPrint : MPxLocatorNode, IMPxNode
	{
		public static MObject size;
		public static MTypeId id = new MTypeId( 0x80007 );
		public static string drawDbClassification = "drawdb/geometry/footPrint";
		public static string drawRegistrantId = "FootprintNodePlugin";

		public footPrint()
		{
			MGlobal.displayInfo("test footPrintNode");
		}

		override public bool compute(MPlug plug, MDataBlock block)
		{
			return true;
		}

		public override void draw(Autodesk.Maya.OpenMayaUI.M3dView view, MDagPath path, Autodesk.Maya.OpenMayaUI.M3dView.DisplayStyle style, Autodesk.Maya.OpenMayaUI.M3dView.DisplayStatus status)
		{
			// Get the size
			//
			MObject thisNode = thisMObject();
			MPlug plug = new MPlug( thisNode, size );
			MDistance sizeVal = new MDistance();
			plug.getValue( sizeVal );

			float multiplier = (float) sizeVal.asCentimeters;

			view.beginGL();

			if ( ( style == M3dView.DisplayStyle.kFlatShaded ) ||
				 ( style == M3dView.DisplayStyle.kGouraudShaded ) )
			{
				// Push the color settings
				//
				OpenGL.glPushAttrib( (uint)OpenGL.GL_CURRENT_BIT );

				if ( status == M3dView.DisplayStatus.kActive ) {
					view.setDrawColor( 13, (int)M3dView.ColorTable.kActiveColors );
				} else {
					view.setDrawColor(13, (int)M3dView.ColorTable.kDormantColors);
				}

				OpenGL.glBegin((uint)OpenGL.GL_TRIANGLE_FAN);
					int i;
					int last = footPrintData.soleCount - 1;
					for ( i = 0; i < last; ++i ) {
						OpenGL.glVertex3f(footPrintData.sole[i,0] * multiplier,
										footPrintData.sole[i,1] * multiplier,
										footPrintData.sole[i,2] * multiplier);
					}
				OpenGL.glEnd();
				OpenGL.glBegin((uint)OpenGL.GL_TRIANGLE_FAN);
				last = footPrintData.heelCount - 1;
					for ( i = 0; i < last; ++i ) {
						OpenGL.glVertex3f(footPrintData.heel[i,0] * multiplier,
										footPrintData.heel[i,1] * multiplier,
										footPrintData.heel[i,2] * multiplier);
					}
				OpenGL.glEnd();

				OpenGL.glPopAttrib();
			}

			// Draw the outline of the foot
			//
			OpenGL.glBegin((uint)OpenGL.GL_LINES);
			{
				int last = footPrintData.soleCount - 1;
				for (int i = 0; i < last; ++i)
				{
					OpenGL.glVertex3f(footPrintData.sole[i, 0] * multiplier,
									footPrintData.sole[i, 1] * multiplier,
									footPrintData.sole[i, 2] * multiplier);
					OpenGL.glVertex3f(footPrintData.sole[i + 1, 0] * multiplier,
									footPrintData.sole[i + 1, 1] * multiplier,
									footPrintData.sole[i + 1, 2] * multiplier);
				}
				last = footPrintData.heelCount - 1;
				for (int i = 0; i < last; ++i)
				{
					OpenGL.glVertex3f(footPrintData.heel[i, 0] * multiplier,
									footPrintData.heel[i, 1] * multiplier,
									footPrintData.heel[i, 2] * multiplier);
					OpenGL.glVertex3f(footPrintData.heel[i + 1, 0] * multiplier,
									footPrintData.heel[i + 1, 1] * multiplier,
									footPrintData.heel[i + 1, 2] * multiplier);
				}
			}
			OpenGL.glEnd();


			view.endGL();
		}

		public override bool isBounded()
		{
			return true;
		}

		public override MBoundingBox boundingBox()
		{
			// Get the size
			//
			MObject thisNode = thisMObject();
			MPlug plug = new MPlug( thisNode, size );
			MDistance sizeVal = new MDistance();
			plug.getValue( sizeVal );

			double multiplier = sizeVal.asCentimeters;

			MPoint corner1 = new MPoint( -0.17, 0.0, -0.7 );
			MPoint corner2 = new MPoint( 0.17, 0.0, 0.3 );

			corner1 = corner1 * multiplier;
			corner2 = corner2 * multiplier;

			return new MBoundingBox( corner1, corner2 );
		}

        [MPxNodeInitializer()]
		public static bool initialize()
		{
			MFnUnitAttribute unitFn = new MFnUnitAttribute();

			size = unitFn.create( "size", "sz", MFnUnitAttribute.Type.kDistance );
			unitFn.setDefault( 1.0 );

			addAttribute( size );

			return true;
		}
	}

	public class FootPrintData : MUserData
	{
        public FootPrintData(bool deleteAfterUse) : base(deleteAfterUse) {}
		public float fMultiplier;
		public float[] fColor = new float[3];
		public bool fCustomBoxDraw;
		public MBoundingBox fCurrentBoundingBox;
		public MDAGDrawOverrideInfo fDrawOV;
	}

	// Helper class declaration for the object drawing
	abstract public class FootPrintDrawAgent
	{
		public FootPrintDrawAgent(){}
		~FootPrintDrawAgent(){}

		public abstract void drawShaded( float multiplier );
		public abstract void drawBoundingBox(MPoint min, MPoint max);
		public abstract void drawWireframe( float multiplier );
		public abstract void beginDraw();
		public abstract void endDraw();

		public void setMatrix(MMatrix wvMatrix, MMatrix projMatrix)
		{
			mWorldViewMatrix = wvMatrix;
			mProjectionMatrix = projMatrix;
		}

		public void setColor(MColor color)
		{
			mColor = color;
		}

		protected MMatrix mWorldViewMatrix;
		protected MMatrix mProjectionMatrix;
		protected MColor  mColor;
	};

	// GL draw agent declaration
	public class FootPrintDrawAgentGL : FootPrintDrawAgent
	{
		public static FootPrintDrawAgentGL singleton = null;
		public static FootPrintDrawAgentGL getDrawAgent(){
			if(null == singleton)
				singleton = new FootPrintDrawAgentGL();
			return singleton;
		}

		public override void drawShaded(float multiplier)
		{
			// set color
			OpenGL.glColor4fv(TransferFloatsToFloat4(mColor.r, mColor.g, mColor.b, mColor.a));

			OpenGL.glBegin((uint)OpenGL.GL_TRIANGLE_FAN);
			int i;
			int last = footPrintData.soleCount - 1;
			for (i = 0; i < last; ++i)
			{
				OpenGL.glVertex3f(footPrintData.sole[i,0] * multiplier,
					footPrintData.sole[i,1] * multiplier,
					footPrintData.sole[i,2] * multiplier);
			}
			OpenGL.glEnd();
			OpenGL.glBegin((uint)OpenGL.GL_TRIANGLE_FAN);
			last = footPrintData.heelCount - 1;
			for (i = 0; i < last; ++i)
			{
				OpenGL.glVertex3f(footPrintData.heel[i,0] * multiplier,
					footPrintData.heel[i,1] * multiplier,
					footPrintData.heel[i,2] * multiplier);
			}
			OpenGL.glEnd();
		}

		public override void drawBoundingBox(MPoint min, MPoint max)
		{
			// set color
			OpenGL.glColor4fv(TransferFloatsToFloat4(mColor.r, mColor.g, mColor.b, mColor.a));

			float[] bottomLeftFront = new float[3]{ (float)min[0], (float)min[1], (float)min[2] };
			float[] topLeftFront = new float[3] { (float)min[0], (float)max[1], (float)min[2] }; //1
			float[] bottomRightFront = new float[3] { (float)max[0], (float)min[1], (float)min[2] }; //2
			float[] topRightFront = new float[3] { (float)max[0], (float)max[1], (float)min[2] }; //3
			float[] bottomLeftBack = new float[3] { (float)min[0], (float)min[1], (float)max[2] }; //4
			float[] topLeftBack = new float[3] { (float)min[0], (float)max[1], (float)max[2] }; //5
			float[] bottomRightBack = new float[3] { (float)max[0], (float)min[1], (float)max[2] }; //6
			float[] topRightBack = new float[3] { (float)max[0], (float)max[1], (float)max[2] }; //7

			OpenGL.glBegin((uint)OpenGL.GL_LINES);

			// 4 Bottom lines
			//
			OpenGL.glVertex3fv(bottomLeftFront);
			OpenGL.glVertex3fv(bottomRightFront);
			OpenGL.glVertex3fv(bottomRightFront);
			OpenGL.glVertex3fv(bottomRightBack);
			OpenGL.glVertex3fv(bottomRightBack);
			OpenGL.glVertex3fv(bottomLeftBack);
			OpenGL.glVertex3fv(bottomLeftBack);
			OpenGL.glVertex3fv(bottomLeftFront);

			// 4 Top lines
			//
			OpenGL.glVertex3fv(topLeftFront);
			OpenGL.glVertex3fv(topRightFront);
			OpenGL.glVertex3fv(topRightFront);
			OpenGL.glVertex3fv(topRightBack);
			OpenGL.glVertex3fv(topRightBack);
			OpenGL.glVertex3fv(topLeftBack);
			OpenGL.glVertex3fv(topLeftBack);
			OpenGL.glVertex3fv(topLeftFront);

			// 4 Side lines
			//
			OpenGL.glVertex3fv(bottomLeftFront);
			OpenGL.glVertex3fv(topLeftFront);
			OpenGL.glVertex3fv(bottomRightFront);
			OpenGL.glVertex3fv(topRightFront);
			OpenGL.glVertex3fv(bottomRightBack);
			OpenGL.glVertex3fv(topRightBack);
			OpenGL.glVertex3fv(bottomLeftBack);
			OpenGL.glVertex3fv(topLeftBack);

			OpenGL.glEnd();
		}
		public override void drawWireframe(float multiplier)
		{
			// set color
			OpenGL.glColor4fv(TransferFloatsToFloat4(mColor.r, mColor.g, mColor.b, mColor.a));

			// draw wire
			OpenGL.glBegin((uint)OpenGL.GL_LINES);
			int i;
			int last = footPrintData.soleCount - 1;
			for (i = 0; i < last; ++i)
			{
				OpenGL.glVertex3f(footPrintData.sole[i,0] * multiplier,
					footPrintData.sole[i,1] * multiplier,
					footPrintData.sole[i,2] * multiplier);
				OpenGL.glVertex3f(footPrintData.sole[i + 1,0] * multiplier,
					footPrintData.sole[i + 1,1] * multiplier,
					footPrintData.sole[i + 1,2] * multiplier);
			}
			last = footPrintData.heelCount - 1;
			for (i = 0; i < last; ++i)
			{
				OpenGL.glVertex3f(footPrintData.heel[i,0] * multiplier,
					footPrintData.heel[i,1] * multiplier,
					footPrintData.heel[i,2] * multiplier);
				OpenGL.glVertex3f(footPrintData.heel[i + 1,0] * multiplier,
					footPrintData.heel[i + 1,1] * multiplier,
					footPrintData.heel[i + 1,2] * multiplier);
			}
			OpenGL.glEnd();
		}

		public override void beginDraw()
		{
			// set world view matrix
			OpenGL.glMatrixMode((uint)OpenGL.GL_MODELVIEW);
			OpenGL.glPushMatrix();
			OpenGL.glLoadMatrixd(TransferMatrixToDouble(mWorldViewMatrix));
			// set projection matrix
			OpenGL.glMatrixMode((uint)OpenGL.GL_PROJECTION);
			OpenGL.glPushMatrix();
			OpenGL.glLoadMatrixd(TransferMatrixToDouble(mProjectionMatrix));
			OpenGL.glPushAttrib((uint)OpenGL.GL_CURRENT_BIT);
		}

		public override void endDraw()
		{
			OpenGL.glPopAttrib();
			OpenGL.glPopMatrix();
			OpenGL.glMatrixMode((uint)OpenGL.GL_MODELVIEW);
			OpenGL.glPopMatrix();
		}

		private FootPrintDrawAgentGL(){}
		~FootPrintDrawAgentGL(){}
		private FootPrintDrawAgentGL( FootPrintDrawAgentGL v ){}

		private static double[] TransferMatrixToDouble(MMatrix mm)
		{
			double[] res = new double[16] { mm[0, 0], mm[0, 1], mm[0, 2], mm[0, 3],
										   mm[1, 0], mm[1, 1], mm[1, 2], mm[1, 3],
										   mm[2, 0], mm[2, 1], mm[2, 2], mm[2, 3],
										   mm[3, 0], mm[3, 1], mm[3, 2], mm[3, 3] };
			return res;
		}

		private static float[] TransferFloatsToFloat4(float f1, float f2, float f3, float f4)
		{
			float[] res = new float[4] { f1, f2, f3, f4 };
			return res;
		}
	};

	public class FootPrintDrawOverride : MPxDrawOverride
	{
		protected MBoundingBox mCurrentBoundingBox;
		protected bool mCustomBoxDraw;

		private static MBlendState blendState = null;
		private static MRasterizerState rasterState = null;

		public FootPrintDrawOverride(MObject obj)
            : base(obj, draw)
		{
			mCustomBoxDraw = true;
			mCurrentBoundingBox = new MBoundingBox();
		}

		private float getMultiplier(MDagPath objPath)
		{
			// Retrieve value of the size attribute from the node
			MObject footprintNode = objPath.node;
			if (!footprintNode.isNull)
			{
				MPlug plug = new MPlug(footprintNode, footPrint.size);
				if (!plug.isNull)
				{
					MDistance sizeVal = new MDistance();
					try
					{
						plug.getValue(sizeVal);
						return (float)sizeVal.asCentimeters;
					}
					catch (Exception)
					{
						MGlobal.displayInfo("Error doing getValue on plugin");
					}
				}
			}

			return 1.0f;
		}

		public override MBoundingBox boundingBox(MDagPath objPath, MDagPath cameraPath)
		{
			MPoint corner1 = new MPoint( -0.17, 0.0, -0.7 );
			MPoint corner2 = new MPoint( 0.17, 0.0, 0.3 );

			float multiplier = getMultiplier(objPath);
			corner1 = corner1 * multiplier;
			corner2 = corner2 * multiplier;

			mCurrentBoundingBox.clear();
			mCurrentBoundingBox.expand(corner1);
			mCurrentBoundingBox.expand(corner2);

			return mCurrentBoundingBox;
		}

		public override bool disableInternalBoundingBoxDraw()
		{
			return mCustomBoxDraw;
		}

		public override MUserData prepareForDraw(MDagPath objPath, MDagPath cameraPath, MFrameContext frameContext, MUserData oldData)
		{
            // This function is called by maya internal, .Net SDK has transfered MUserData to the derived one
            // Users don't need do the MUserData.getData(oldData) by themselves
			FootPrintData data = oldData as FootPrintData;
			if (data == null)
			{
			    // Retrieve data cache (create if does not exist)
				data = new FootPrintData(false);
				data.OwnerShip = false;
			}

			// compute data and cache it
			data.fMultiplier = getMultiplier(objPath);
			MColor color = MGeometryUtilities.wireframeColor(objPath);
			data.fColor[0] = color.r;
			data.fColor[1] = color.g;
			data.fColor[2] = color.b;
			data.fCustomBoxDraw = mCustomBoxDraw;
			data.fCurrentBoundingBox = mCurrentBoundingBox;
			// Get the draw override information
			data.fDrawOV = objPath.drawOverrideInfo;

            return data;
		}

		public override bool hasUIDrawables()
		{
			return true;
		}

		public override void addUIDrawables(MDagPath objPath, MUIDrawManager drawManager, MFrameContext frameContext, MUserData data)
		{
			// Draw a text "Foot"
			MPoint pos = new MPoint( 0.0, 0.0, 0.0 ); // Position of the text
			MColor textColor = new MColor( 0.1f, 0.8f, 0.8f, 1.0f ); // Text color

			drawManager.beginDrawable();

			drawManager.setColor( textColor );
			drawManager.setFontSize( (uint)MUIDrawManager.FontSize.kSmallFontSize );
			drawManager.text( pos,  "Footprint", MUIDrawManager.TextAlignment.kCenter );

			drawManager.endDrawable();
		}

		private static double[] TransferMatrixToDouble(MMatrix mm)
		{
			double[] res = new double[16] { mm[0, 0], mm[0, 1], mm[0, 2], mm[0, 3],
										   mm[1, 0], mm[1, 1], mm[1, 2], mm[1, 3],
										   mm[2, 0], mm[2, 1], mm[2, 2], mm[2, 3],
										   mm[3, 0], mm[3, 1], mm[3, 2], mm[3, 3] };
			return res;
		}

        public static void draw(MDrawContext context, MUserData userData)
		{
            // This function is called by maya internal, .Net SDK has transfered MUserData to the derived one
            // Users don't need do the MUserData.getData(oldData) by themselves
			FootPrintData footData = MUserData.getData(userData) as FootPrintData;
            if (footData == null)
                return;

			MDAGDrawOverrideInfo objectOverrideInfo = footData.fDrawOV;

			if (objectOverrideInfo.fOverrideEnabled && !objectOverrideInfo.fEnableVisible)
				return;

			uint displayStyle = context.getDisplayStyle();

			bool drawAsBoundingBox = (displayStyle & (uint)MFrameContext.DisplayStyle.kBoundingBox)!= 0 ||
				(footData.fDrawOV.fLOD == MDAGDrawOverrideInfo.DrawOverrideLOD.kLODBoundingBox);
			if (drawAsBoundingBox && !footData.fCustomBoxDraw)
				return;

			// get renderer
			Autodesk.Maya.OpenMayaRender.MHWRender.MRenderer theRenderer = Autodesk.Maya.OpenMayaRender.MHWRender.MRenderer.theRenderer();
			if (theRenderer == null)
				return;

			// get state data
			MMatrix transform =
				context.getMatrix(MDrawContext.MatrixType.kWorldViewMtx);

			MMatrix projection =
				context.getMatrix(MDrawContext.MatrixType.kProjectionMtx);

			// Check to see if we are drawing in a shadow pass.
			// If so then we keep the shading simple which in this
			// example means to disable any extra blending state changes
			//
			MPassContext passCtx = context.passContext;
			MStringArray passSem = passCtx.passSemantics;
			bool castingShadows = false;
			for (int i=0; i<passSem.length; i++)
			{
				if (passSem[i] == MPassContext.kShadowPassSemantic)
					castingShadows = true;
			}
			bool debugPassInformation = false;
			if (debugPassInformation)
			{
				string passId = passCtx.passIdentifier;
				MGlobal.displayInfo("footprint node drawing in pass[" + passId + "], semantic[");
				for (int i=0; i<passSem.length; i++)
					MGlobal.displayInfo(passSem[i]);
				MGlobal.displayInfo("\n");
			}

			// get cached data
			float multiplier = footData.fMultiplier;
			float[] color = new float[4]{
					footData.fColor[0],
					footData.fColor[1],
					footData.fColor[2],
					1.0f
			};

			bool requireBlending = false;

			// If we're not casting shadows then do extra work
			// for display styles
			if (!castingShadows)
			{

				// Use some monotone version of color to show "default material mode"
				//
				if ((displayStyle & (uint)MFrameContext.DisplayStyle.kDefaultMaterial) != 0)
				{
					color[0] = color[1] = color[2] = (color[0] + color[1] + color[2]) / 3.0f;
				}
				// Do some alpha blending if in x-ray mode
				//
				else if ((displayStyle & (uint)MFrameContext.DisplayStyle.kXray) != 0)
				{
					requireBlending = true;
					color[3] = 0.3f;
				}
			}

			// Set blend and raster state
			//
			MStateManager stateMgr = context.stateManager;
			MBlendState pOldBlendState = null;
			MRasterizerState pOldRasterState = null;
			bool rasterStateModified = false;

			if((stateMgr != null) && ((displayStyle & (uint)MFrameContext.DisplayStyle.kGouraudShaded) != 0))
			{
				// draw filled, and with blending if required
				if (requireBlending)
				{
					if (blendState == null)
					{
						MBlendStateDesc desc = new MBlendStateDesc();
						desc.targetBlends.blendEnable = true;
						desc.targetBlends.destinationBlend = MBlendState.BlendOption.kInvSourceAlpha;
						desc.targetBlends.alphaDestinationBlend = MBlendState.BlendOption.kInvSourceAlpha;
						blendState = MStateManager.acquireBlendState(desc);
					}

					if (blendState != null)
					{
						pOldBlendState = stateMgr.blendState;
						stateMgr.blendState = blendState;
					}
				}

				// Override culling mode since we always want double-sided
				//
				pOldRasterState = (stateMgr != null) ? stateMgr.rasterizerState : null;
				if (pOldRasterState != null)
				{
					MRasterizerStateDesc desc = new MRasterizerStateDesc( pOldRasterState.desc );
					// It's also possible to change this to kCullFront or kCullBack if we
					// wanted to set it to that.
					MRasterizerState.CullMode cullMode = MRasterizerState.CullMode.kCullNone;
					if (desc.cullMode != cullMode)
					{
						if (rasterState != null)
						{
							// Just override the cullmode
							desc.cullMode = cullMode;
							rasterState = MStateManager.acquireRasterizerState(desc);
						}
						if (rasterState == null)
						{
							rasterStateModified = true;
							stateMgr.rasterizerState = rasterState;
						}
					}
				}
			}

			//========================
			// Start the draw work
			//========================

			// Prepare draw agent, default using OpenGL
			FootPrintDrawAgentGL drawAgentRef = FootPrintDrawAgentGL.getDrawAgent();
			FootPrintDrawAgent drawAgentPtr = drawAgentRef;

			if ( drawAgentPtr != null ){

				// Set color
				drawAgentPtr.setColor( new MColor(color[0],color[1],color[2],color[3]) );
				// Set matrix
				drawAgentPtr.setMatrix( transform, projection );

				drawAgentPtr.beginDraw();

				if ( drawAsBoundingBox )
				{
					// If it is in bounding bode, draw only bounding box wireframe, nothing else
					MPoint min = footData.fCurrentBoundingBox.min;
					MPoint max = footData.fCurrentBoundingBox.max;

					drawAgentPtr.drawBoundingBox( min, max );
				} else {
					// Templated, only draw wirefame and it is not selectale
					bool overideTemplated = objectOverrideInfo.fOverrideEnabled &&
						(objectOverrideInfo.fDisplayType == MDAGDrawOverrideInfo.DrawOverrideDisplayType.kDisplayTypeTemplate);
					// Override no shaded, only show wireframe
					bool overrideNoShaded = objectOverrideInfo.fOverrideEnabled && !objectOverrideInfo.fEnableShading;

					if ( overideTemplated || overrideNoShaded ){
						drawAgentPtr.drawWireframe( multiplier );
					} else {
						if( ((displayStyle & (uint)MFrameContext.DisplayStyle.kGouraudShaded)!=0) ||
							((displayStyle & (uint)MFrameContext.DisplayStyle.kTextured))!=0)
						{
							drawAgentPtr.drawShaded( multiplier );
						}

						if((displayStyle & (uint)MFrameContext.DisplayStyle.kWireFrame) != 0)
						{
							drawAgentPtr.drawWireframe( multiplier );
						}
					}
				}

				drawAgentPtr.endDraw();
			}

			//========================
			// End the draw work
			//========================

			// Restore old blend state and old raster state
			if((stateMgr != null) && ((displayStyle & (uint)MFrameContext.DisplayStyle.kGouraudShaded) != 0))
			{
				if ((stateMgr != null) && (pOldBlendState != null))
				{
					stateMgr.setBlendState(pOldBlendState);
				}
				if (rasterStateModified && (pOldBlendState != null))
				{
					stateMgr.setRasterizerState(pOldRasterState);
				}
			}
		}

		public override Autodesk.Maya.OpenMayaRender.MHWRender.DrawAPI supportedDrawAPIs()
		{
			return Autodesk.Maya.OpenMayaRender.MHWRender.DrawAPI.kOpenGL;
		}
	}

}
