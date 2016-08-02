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

[assembly: MPxDeviceClass("jlcVcrCSharp", typeof(MayaNetPlugin.jlcVcr))]

namespace MayaNetPlugin
{
	public class jlcVcr: MPxMidiInputDevice
	{
		public const string kVCRRewindButtonName = "rewind";
		public const ushort kVCRRewindButtonIndex = 0;
		public const string kVCRForwardButtonName = "forward";
		public const ushort kVCRForwardButtonIndex = 1;
		public const string kVCRStopButtonName = "stop";
		public const ushort kVCRStopButtonIndex = 2;
		public const string kVCRPlayButtonName = "play";
		public const ushort kVCRPlayButtonIndex = 3;
		public const string kVCRRecordButtonName = "record";
		public const ushort kVCRRecordButtonIndex = 4;
		public const ushort kVCRLastButtonIndex = 5;

		public const string kVCRScrubWheelName = "scrub";
		public const ushort kVCRScrubWheelIndex = 0;
		public const ushort kVCRLastAxisIndex = 1;

		public const int kVCRRewindUp	 = 0x07;
		public const int kVCRRewindDown  = 0x47;
		public const int kVCRForwardUp   = 0x06;
		public const int kVCRForwardDown = 0x46;
		public const int kVCRStopUp      = 0x05;
		public const int kVCRStopDown    = 0x45;
		public const int kVCRPlayUp      = 0x04;
		public const int kVCRPlayDown    = 0x44;
		public const int kVCRRecordUp    = 0x00;
		public const int kVCRRecordDown  = 0x40;

		public jlcVcr(){}

		public override void nameButtons()
		{
			string name;

			name = kVCRRewindButtonName;
			setNamedButton(name, kVCRRewindButtonIndex);

			name = kVCRForwardButtonName;
			setNamedButton(name, kVCRForwardButtonIndex);

			name = kVCRStopButtonName;
			setNamedButton(name, kVCRStopButtonIndex);

			name = kVCRPlayButtonName;
			setNamedButton(name, kVCRPlayButtonIndex);

			name = kVCRRecordButtonName;
			setNamedButton(name, kVCRRecordButtonIndex);
		}

		public override void nameAxes()
		{
			string name = kVCRScrubWheelName;

			MDeviceChannel scrubChannel =
				new MDeviceChannel(name, null, kVCRScrubWheelIndex);

			addChannel(scrubChannel);
		}

		public override MDeviceState deviceState()
		{
			return base.deviceState();
		}
	}
}
