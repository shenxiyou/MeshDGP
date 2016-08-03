// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================

/////////////////////////////////////////////////////////////////////////////
//
// RenderPassTest.cs
//
// Description:
//		This plug-in tests the render pass API by defining and registering
//		new render pass definitions and implementations. It also registers
//		a command to query information about passes and implementations.
//		Only one of -n/-g/-d/-c/-r/-dt/-nc will be evaluated. Please look
//		at the accompanying MEL script showing how to query the render passes.
//
//		-p/-pass <string>
//			The argument specifies a pass ID to query for existence
//
//		-n/-name
//			Must be specified with -p, prints out pass name
//
//		-g/-group
//			Must be specified with -p, prints out pass group
//
//		-d/-description
//			Must be specified with -p, prints out pass description
//
//		-r/-renderer <string>
//			Must be specified with -p, queries existence of implementation
//			for given pass and renderer implementation
//
//		-c/-compat <string>
//			Must be specified with -p and -r, true if implementation is
//			compatible with supplied renderer implementation
//
//		-t/-types
//			Must be specified with -p and -r, retrieve supported types for
//			given pass and renderer implementation
//
//		-dt/-defaultTypes
//			Must be specified with -p and -r, retrieve default type for
//			given pass and renderer implementation
//
//		-nc/-numChannels
//			Must be specified with -p and -r, retrieve number of channels
//			supported for given pass and renderer implementation
//
//		-s/-semantic
//			Must be specified with -p and -r, retrieve the frame buffer
//			semantic for given pass and renderer implementation
//
// Note:
//		This C# test is extracted from $(MAYADIR)\devkit\test\testRenderPassAPI.cpp.
//
/////////////////////////////////////////////////////////////////////////////


using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaRender;

[assembly: MPxRenderPassDefClass("testPassOne", "testRenderPassOne", "CustomPasses", "Test Render Pass (number one)", false)]
[assembly: MPxRenderPassImplClass(typeof(MayaNetPlugin.MyRenderPassImpl), "testRenderPassImpl", "testPassOne", false)]
[assembly: MPxCommandClass(typeof(MayaNetPlugin.RenderPassTest), "RenderPassTestCSharp")]

namespace MayaNetPlugin
{
	public class MyRenderPassImpl : MPxRenderPassImpl
	{
		private string fRenderer;
		private uint fTypes;
		private PassTypeBit fDefType;
		private uint fNumChans;
		private bool fPerLight;
		private PassSemantic fSemantic;

		public MyRenderPassImpl()
		{
			fRenderer = "mySoftwareRenderer";
			fTypes = (uint)PassTypeBit.kUInt8 | (uint)PassTypeBit.kUInt16 | (uint)PassTypeBit.kUInt32 | (uint)PassTypeBit.kUInt64;
			fDefType = PassTypeBit.kUInt8;
			fNumChans = 3;
			fPerLight = true;
			fSemantic = PassSemantic.kColorSemantic;
		}

		// inherited virtual functions:
		public override bool isCompatible(string renderer) { return fRenderer == renderer; }
		public override uint typesSupported() { return fTypes; }
		public override PassTypeBit getDefaultType() { return fDefType; }
		public override uint getNumChannels() { return fNumChans; }
		public override PassSemantic frameBufferSemantic() { return fSemantic; }
		public override bool perLightPassContributionSupported() { return fPerLight; }
	}


	//- Input syntax flags
	[MPxCommandSyntaxFlag("-p", "-pass", Arg1=typeof(System.String))]
    [MPxCommandSyntaxFlag("-r", "-renderer", Arg1 = typeof(System.String))]
    [MPxCommandSyntaxFlag("-c", "-compat", Arg1 = typeof(System.String))]
	//- Output syntax flags
    [MPxCommandSyntaxFlag("-n", "-name")]
    [MPxCommandSyntaxFlag("-g", "-group")]
    [MPxCommandSyntaxFlag("-d", "-description")]
    [MPxCommandSyntaxFlag("-t", "-types")]
    [MPxCommandSyntaxFlag("-dt", "-defaultType")]
    [MPxCommandSyntaxFlag("-nc", "-numChannels")]
    [MPxCommandSyntaxFlag("-s", "-semantic")]
    [MPxCommandSyntaxFlag("-pl", "-perLight")]
	public class RenderPassTest : MPxCommand, IMPxCommand
	{
		string[] PassFlag				= { "-p",  "-pass" };
		string[] RendererFlag			= { "-r",  "-renderer" };
		string[] NameFlag				= { "-n",  "-name" };
		string[] GroupFlag				= { "-g",  "-group" };
		string[] DescriptionFlag		= { "-d",  "-description" };
		string[] CompatFlag				= { "-c",  "-compat" };
		string[] TypesFlag				= { "-t",  "-types" };
		string[] DefaultTypeFlag		= { "-dt", "-defaultType" };
		string[] NumChannelsFlag		= { "-nc", "-numChannels" };
		string[] SemanticFlag			= { "-s",  "-semantic" };
		string[] PerLightFlag			= { "-pl", "-perLight" };

		public override void doIt(MArgList argList)
		{
			MArgDatabase argData = new MArgDatabase(syntax, argList);

			// Retrieve pass Id. The pass flag must be set.
			string passId = argData.isFlagSet(PassFlag[0]) ? argData.flagArgumentString(PassFlag[0], 0) : "";
			if (passId.Length <= 0)
				throw new System.ArgumentException("The pass flag is not set", "argList");

			MRenderPassDef def = null;
			try {
				def = MRenderPassRegistry.getRenderPassDefinition(passId);
			} catch (System.Exception) {
				setResult(false);
				return;
			}

			// implementation information
			string renderer = argData.isFlagSet(RendererFlag[0]) ? argData.flagArgumentString(RendererFlag[0], 0) : "";
			if (renderer.Length > 0) {
				MPxRenderPassImpl impl = null;
				try {
					impl = def.getImplementation(renderer);
				} catch (System.Exception) {
					// impl info requested but does not exist, stop here
					setResult(false);
                    return;
				}
				
				if (argData.isFlagSet(TypesFlag[0])) {
					uint types = impl.typesSupported();
					string result = getTypeStrings(types);
					setResult(result);
				} else if (argData.isFlagSet(DefaultTypeFlag[0])) {
					uint type = (uint)impl.getDefaultType();
					string result = getTypeStrings(type);
					setResult(result);
				} else if (argData.isFlagSet(NumChannelsFlag[0])) {
					uint result = impl.getNumChannels();
					setResult(result);
				} else if (argData.isFlagSet(SemanticFlag[0])) {
					string result = getSemanticString(impl.frameBufferSemantic());
					setResult(result);
				} else if (argData.isFlagSet(PerLightFlag[0])) {
					bool result = impl.perLightPassContributionSupported();
					setResult(result);
				} else if (argData.isFlagSet(CompatFlag[0])) {
					string fCompat = argData.flagArgumentString(CompatFlag[0], 0);
					bool result = impl.isCompatible(fCompat);
					setResult(result);
				} else {
					// just indicate the implementation exists
					setResult(true);
				}
			} else {
				// pass information
				if (argData.isFlagSet(NameFlag[0])) {
						setResult(def.getName());
				} else if (argData.isFlagSet(GroupFlag[0])) {
						setResult(def.getGroup());
				} else if (argData.isFlagSet(DescriptionFlag[0])) {
						setResult(def.getDescription());
				} else {
					// just indicate the definition exists
					setResult(true);
				}
			}

			return;
		}

		private string getTypeStrings(uint mask)
		{
			string result = "";

			if ((mask & (uint)MPxRenderPassImpl.PassTypeBit.kUInt8) != 0)   result += " kUInt8";
			if ((mask & (uint)MPxRenderPassImpl.PassTypeBit.kUInt16) != 0)  result += " kUInt16";
			if ((mask & (uint)MPxRenderPassImpl.PassTypeBit.kUInt32) != 0)  result += " kUInt32";
			if ((mask & (uint)MPxRenderPassImpl.PassTypeBit.kUInt64) != 0)  result += " kUInt64";
			if ((mask & (uint)MPxRenderPassImpl.PassTypeBit.kInt8) != 0)    result += " kInt8";
			if ((mask & (uint)MPxRenderPassImpl.PassTypeBit.kInt16) != 0)   result += " kInt16";
			if ((mask & (uint)MPxRenderPassImpl.PassTypeBit.kInt32) != 0)   result += " kInt32";
			if ((mask & (uint)MPxRenderPassImpl.PassTypeBit.kInt64) != 0)   result += " kInt64";
			if ((mask & (uint)MPxRenderPassImpl.PassTypeBit.kFloat16) != 0) result += " kFloat16";
			if ((mask & (uint)MPxRenderPassImpl.PassTypeBit.kFloat32) != 0) result += " kFloat32";
			if ((mask & (uint)MPxRenderPassImpl.PassTypeBit.kFloat64) != 0) result += " kFloat64";

			return result;
		}

		private string getSemanticString(MPxRenderPassImpl.PassSemantic sem)
		{
			string result = "unrecognizedValue";

			switch (sem)
			{
			case MPxRenderPassImpl.PassSemantic.kInvalidSemantic:
				result = "kInvalidSemantic";
				break;
			case MPxRenderPassImpl.PassSemantic.kColorSemantic:
				result = "kColorSemantic";
				break;
			case MPxRenderPassImpl.PassSemantic.kVectorSemantic:
				result = "kVectorSemantic";
				break;
			case MPxRenderPassImpl.PassSemantic.kDirectionVectorSemantic:
				result = "kDirectionVectorSemantic";
				break;
			case MPxRenderPassImpl.PassSemantic.kDepthSemantic:
				result = "kDepthSemantic";
				break;
			case MPxRenderPassImpl.PassSemantic.kLabelSemantic:
				result = "kLabelSemantic";
				break;
			case MPxRenderPassImpl.PassSemantic.kMaskSemantic:
				result = "kMaskSemantic";
				break;
			case MPxRenderPassImpl.PassSemantic.kOtherSemantic:
				result = "kOtherSemantic";
				break;
			}

			return result;
		}
	}
}
