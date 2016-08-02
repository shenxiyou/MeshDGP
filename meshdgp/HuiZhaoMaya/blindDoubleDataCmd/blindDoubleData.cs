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


[assembly: MPxDataClass(typeof(MayaNetPlugin.blindDoubleData), 0x80003, MayaNetPlugin.blindDoubleData.typeName,
						MPxData.DataType.kData)]

namespace MayaNetPlugin
{
	public class blindDoubleData : MPxData
	{
		public blindDoubleData()
		{
			fValue = 0;
		}
		#region override
		public override void readASCII(MArgList argList, ref uint endOfTheLastParsedElement)
		{
			if (argList.length == 0)
			{
				throw new System.ArgumentException("The MArgList argument is empty", "argList");
			}

			value = argList.asDouble(endOfTheLastParsedElement++);
			return;
		}

		public override void readBinary(MIStream arg0, uint length)
		{
			MIStream instream = new MIStream(MIStream.getCPtr(arg0).Handle, false);
			MStreamUtils.readDouble(arg0, out fValue, true);
			return;
		}

		public override void writeASCII(MOStream outstream)
		{
			MOStream instream = new MOStream(MOStream.getCPtr(outstream).Handle, false);
			MStreamUtils.writeDouble(outstream, fValue);
			MStreamUtils.writeChar(outstream, ' ');
			return;
		}

		public override void writeBinary(MOStream outstream)
		{
			MOStream instream = new MOStream(MOStream.getCPtr(outstream).Handle, false);
			MStreamUtils.writeDouble(outstream, fValue, true);
			return;
		}

		public override void copy(MPxData src)
		{
			blindDoubleData srcdata = src as blindDoubleData;
			if (srcdata != null)
				fValue = srcdata.fValue;
		}

		public override MTypeId typeId()
		{
			return blindDoubleData.tid;
		}

		public override string name()
		{
			return blindDoubleData.typeName;
		}
		#endregion

		public double value { 
			get  { return fValue; } 
			set  { fValue = value; } 
		}

		public const string typeName = "blindDoubleDataCSharp";
		public static readonly MTypeId tid = new MTypeId(0x80003);

		private double fValue;
	}
}
