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
using System.IO;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaAnim;

namespace MayaNetPlugin
{
	public class RegisterMStringResources
	{
		public static string kPluginId = "animImportExport";
		public static MStringResourceId kNothingSelected = new MStringResourceId(RegisterMStringResources.kPluginId, "kNothingSelected", "Nothing is selected for anim curve import.");

		public static MStringResourceId kPasteFailed = new MStringResourceId(RegisterMStringResources.kPluginId, "kPasteFailed", "Could not paste the anim curves.");

		public static MStringResourceId kAnimCurveNotFound = new MStringResourceId(RegisterMStringResources.kPluginId, "kAnimCurveNotFound", "No anim curves were found.");

		public static MStringResourceId kInvalidAngleUnits = new MStringResourceId(RegisterMStringResources.kPluginId, "kInvalidAngleUnits", "'{0}' is not a valid angular unit. Use rad|deg|min|sec instead.");

		public static MStringResourceId kInvalidLinearUnits = new MStringResourceId(RegisterMStringResources.kPluginId, "kInvalidLinearUnits", "'{0}' is not a valid linear unit. Use mm|cm|m|km|in|ft|yd|mi instead.");

		public static MStringResourceId kInvalidTimeUnits = new MStringResourceId(RegisterMStringResources.kPluginId, "kInvalidTimeUnits", "'{0}' is not a valid time unit. Use game|film|pal|ntsc|show|palf|ntscf|hour|min|sec|millisec instead.");

		public static MStringResourceId kInvalidVersion = new MStringResourceId(RegisterMStringResources.kPluginId, "kInvalidVersion", "Reading a version {0} file with a version {1} animImportPlugin.");

		public static MStringResourceId kSettingToUnit = new MStringResourceId(RegisterMStringResources.kPluginId, "kSettingToUnit", "Setting {0} to {1}.");

		public static MStringResourceId kMissingKeyword = new MStringResourceId(RegisterMStringResources.kPluginId, "kMissingKeyword", "Missing a required keyword: {0}");

		public static MStringResourceId kCouldNotReadAnim = new MStringResourceId(RegisterMStringResources.kPluginId, "kCouldNotReadAnim", "Could not read an anim curve.");

		public static MStringResourceId kCouldNotCreateAnim = new MStringResourceId(RegisterMStringResources.kPluginId, "kCouldNotCreateAnim", "Could not create the anim curve node.");

		public static MStringResourceId kUnknownKeyword = new MStringResourceId(RegisterMStringResources.kPluginId, "kUnknownKeyword", "{0} is an unrecognized keyword.");

		public static MStringResourceId kClipboardFailure = new MStringResourceId(RegisterMStringResources.kPluginId, "kClipboardFailure", "Could not add the anim curves to the clipboard.");

		public static MStringResourceId kSettingTanAngleUnit = new MStringResourceId(RegisterMStringResources.kPluginId, "kSettingTanAngleUnit", "Setting the tanAngleUnit to {0}.");

		public static MStringResourceId kUnknownNode = new MStringResourceId(RegisterMStringResources.kPluginId, "kUnknownNode", "Encountered an unknown anim curve type.");

		public static MStringResourceId kCouldNotKey = new MStringResourceId(RegisterMStringResources.kPluginId, "kCouldNotKey", "Could not add a keyframe.");

		public static MStringResourceId kMissingBrace = new MStringResourceId(RegisterMStringResources.kPluginId, "kMissingBrace", "Did not find an expected '}'");

		public static MStringResourceId kCouldNotExport = new MStringResourceId(kPluginId, "kCouldNotExport", "Could not read the anim curve for export.");

		public static MStringResourceId kCouldNotReadStatic = new MStringResourceId(RegisterMStringResources.kPluginId, "kCouldNotReadStatic", "Could not apply the static anim value: {0}.");

		static public void registerStringResources()
		{
			MStringResource.registerString(RegisterMStringResources.kNothingSelected);
			MStringResource.registerString(RegisterMStringResources.kPasteFailed);
			MStringResource.registerString(RegisterMStringResources.kAnimCurveNotFound);
			MStringResource.registerString(RegisterMStringResources.kInvalidAngleUnits);
			MStringResource.registerString(RegisterMStringResources.kInvalidLinearUnits);
			MStringResource.registerString(RegisterMStringResources.kInvalidTimeUnits);
			MStringResource.registerString(RegisterMStringResources.kInvalidVersion);
			MStringResource.registerString(RegisterMStringResources.kSettingToUnit);
			MStringResource.registerString(RegisterMStringResources.kMissingKeyword);
			MStringResource.registerString(RegisterMStringResources.kCouldNotReadAnim);
			MStringResource.registerString(RegisterMStringResources.kCouldNotCreateAnim);
			MStringResource.registerString(RegisterMStringResources.kUnknownKeyword);
			MStringResource.registerString(RegisterMStringResources.kClipboardFailure);
			MStringResource.registerString(RegisterMStringResources.kSettingTanAngleUnit);
			MStringResource.registerString(RegisterMStringResources.kUnknownNode);
			MStringResource.registerString(RegisterMStringResources.kCouldNotKey);
			MStringResource.registerString(RegisterMStringResources.kMissingBrace);
			MStringResource.registerString(RegisterMStringResources.kCouldNotExport);
		}
	}

	public class animUnitNames
	{
		//	String names for units.
		//
		const string kMmString = 		"mm";
		const string kCmString =			"cm";
		const string kMString =			"m";
		const string kKmString =			"km";
		const string kInString =			"in";
		const string kFtString =			"ft";
		const string kYdString =			"yd";
		const string kMiString =			"mi";

		const string kMmLString =		"millimeter";
		const string kCmLString =		"centimeter";
		const string kMLString =			"meter";
		const string kKmLString =		"kilometer";
		const string kInLString =		"inch";
		const string kFtLString =		"foot";
		const string kYdLString =		"yard";
		const string kMiLString =		"mile";

		const string kRadString =		"rad";
		const string kDegString =		"deg";
		const string kMinString =		"min";
		const string kSecString =		"sec";

		const string kRadLString =		"radian";
		const string kDegLString =		"degree";
		const string kMinLString =		"minute";
		const string kSecLString =		"second";

		const string kHourTString =		"hour";
		const string kMinTString =		"min";
		const string kSecTString =		"sec";
		const string kMillisecTString =	"millisec";

		const string kGameTString =		"game";
		const string kFileTString =		"film";
		const string kPalTString =		"pal";
		const string kNtscTString =		"ntsc";
		const string kShowTString =		"show";
		const string kPalFTString =		"palf";
		const string kNtscFTString =		"ntscf";

		public const string kUnitlessString = "unitless";
		const string kUnknownTimeString =	"Unknown Time Unit";
		const string kUnknownAngularString =	"Unknown Angular Unit";
		const string kUnknownLinearString = 	"Unknown Linear Unit";

		public static void setToLongName(MAngle.Unit unit, ref string name)
		//
		//	Description:
		//		Sets the string with the long text name of the angle unit.
		//
		{
			switch(unit) {
				case MAngle.Unit.kDegrees:
					name = kDegLString;
					break;
				case MAngle.Unit.kRadians:
					name = kRadLString;
					break;
				case MAngle.Unit.kAngMinutes:
					name = kMinLString;
					break;
				case MAngle.Unit.kAngSeconds:
					name = kSecLString;
					break;
				default:
					name = kUnknownAngularString;
					break;
			}
		}

		public static void setToShortName( MAngle.Unit unit, ref string name)
		//
		//	Description:
		//		Sets the string with the short text name of the angle unit.
		//
		{
			switch(unit) {
				case MAngle.Unit.kDegrees:
					name = kDegString;
					break;
				case MAngle.Unit.kRadians:
					name = kRadString;
					break;
				case MAngle.Unit.kAngMinutes:
					name = kMinString;
					break;
				case MAngle.Unit.kAngSeconds:
					name = kSecString;
					break;
				default:
					name = kUnknownAngularString;
					break;
			}
		}

		public static void setToLongName(MDistance.Unit unit, ref string name)
		//
		//	Description:
		//		Sets the string with the long text name of the distance unit.
		//
		{
			switch(unit) {
				case MDistance.Unit.kInches:
					name = (kInLString);
					break;
				case MDistance.Unit.kFeet:
					name = (kFtLString);
					break;
				case MDistance.Unit.kYards:
					name = (kYdLString);
					break;
				case MDistance.Unit.kMiles:
					name = (kMiLString);
					break;
				case MDistance.Unit.kMillimeters:
					name = (kMmLString);
					break;
				case MDistance.Unit.kCentimeters:
					name = (kCmLString);
					break;
				case MDistance.Unit.kKilometers:
					name = (kKmLString);
					break;
				case MDistance.Unit.kMeters:
					name = (kMLString);
					break;
				default:
					name = (kUnknownLinearString);
					break;
			}
		}

		/* static */
		public static void setToShortName( MDistance.Unit unit, ref string name)
		//
		//	Description:
		//		Sets the string with the short text name of the distance unit.
		//
		{
			switch(unit) {
				case MDistance.Unit.kInches:
					name = (kInString);
					break;
				case MDistance.Unit.kFeet:
					name = (kFtString);
					break;
				case MDistance.Unit.kYards:
					name = (kYdString);
					break;
				case MDistance.Unit.kMiles:
					name = (kMiString);
					break;
				case MDistance.Unit.kMillimeters:
					name = (kMmString);
					break;
				case MDistance.Unit.kCentimeters:
					name = (kCmString);
					break;
				case MDistance.Unit.kKilometers:
					name = (kKmString);
					break;
				case MDistance.Unit.kMeters:
					name = (kMString);
					break;
				default:
					name = (kUnknownLinearString);
					break;
			}
		}

		/* static */
		public static void setToLongName(MTime.Unit unit, ref string name)
		//
		//	Description:
		//		Sets the string with the long text name of the time unit.
		//
		{
			switch(unit) {
				case MTime.Unit.kHours:
					name = (kHourTString);
					break;
				case MTime.Unit.kMinutes:
					name = (kMinTString);
					break;
				case MTime.Unit.kSeconds:
					name = (kSecTString);
					break;
				case MTime.Unit.kMilliseconds:
					name = (kMillisecTString);
					break;
				case MTime.Unit.kGames:
					name = (kGameTString);
					break;
				case MTime.Unit.kFilm:
					name = (kFileTString);
					break;
				case MTime.Unit.kPALFrame:
					name = (kPalTString);
					break;
				case MTime.Unit.kNTSCFrame:
					name = (kNtscTString);
					break;
				case MTime.Unit.kShowScan:
					name = (kShowTString);
					break;
				case MTime.Unit.kPALField:
					name = (kPalFTString);
					break;
				case MTime.Unit.kNTSCField:
					name = (kNtscFTString);
					break;
				default:
					name = (kUnknownTimeString);
					break;
			}
		}

		/* static */
		public static void setToShortName(MTime.Unit unit, ref string name)
		//
		//	Description:
		//		Sets the string with the short text name of the time unit.
		//
		{
			setToLongName(unit, ref name);
		}

		/* static */
		public static bool setFromName(string str, ref MAngle.Unit unit)
		//
		//	Description:
		//		The angle unit is set based on the passed string. If the string
		//		is not recognized, the angle unit is set to MAngle::kInvalid.
		//
		{
			bool state = true;

			if(string.Compare(str,kDegString) == 0 || string.Compare(str,kDegLString) == 0)
				unit = MAngle.Unit.kDegrees;
			else if( string.Compare(str,kRadString) == 0 || string.Compare(str,kRadLString) == 0)
				unit = MAngle.Unit.kRadians;
			else if( string.Compare(str,kMinString) == 0 || string.Compare(str,kMinLString) == 0 )
				unit = MAngle.Unit.kAngMinutes;
			else if (string.Compare(str, kSecString) == 0 || string.Compare(str, kSecLString) == 0) 
				unit = MAngle.Unit.kAngSeconds;
		   else 
			{
				//	This is not a recognized angular unit.
				//
				unit = MAngle.Unit.kInvalid;
				// Use format to place variable string into message
				string msgFmt = MStringResource.getString(RegisterMStringResources.kInvalidAngleUnits);
				string msg = string.Format(msgFmt, str);
				MGlobal.displayError(msg);
				state = false;
			}

			return state;
		}

		/* static */
		public static bool setFromName(string name, ref MDistance.Unit unit)
		//
		//	Description:
		//		The distance unit is set based on the passed string. If the string
		//		is not recognized, the distance unit is set to MDistance::kInvalid.
		//
		{
			bool state = true;


			if ((string.Compare(name, kInString) == 0) ||
				(string.Compare(name, kInLString) == 0)) {
				unit = MDistance.Unit.kInches;
			} else if (	(string.Compare(name, kFtString) == 0) ||
						(string.Compare(name, kFtLString) == 0)) {
				unit = MDistance.Unit.kFeet;
			} else if (	(string.Compare(name, kYdString) == 0) ||
						(string.Compare(name, kYdLString) == 0)) {
				unit = MDistance.Unit.kYards;
			} else if (	(string.Compare(name, kMiString) == 0) ||
						(string.Compare(name, kMiLString) == 0)) {
				unit = MDistance.Unit.kMiles;
			} else if (	(string.Compare(name, kMmString) == 0) ||
						(string.Compare(name, kMmLString) == 0)) {
				unit = MDistance.Unit.kMillimeters;
			} else if (	(string.Compare(name, kCmString) == 0) ||
						(string.Compare(name, kCmLString) == 0)) {
				unit = MDistance.Unit.kCentimeters;
			} else if (	(string.Compare(name, kKmString) == 0) ||
						(string.Compare(name, kKmLString) == 0)) {
				unit = MDistance.Unit.kKilometers;
			} else if (	(string.Compare(name, kMString) == 0) ||
						(string.Compare(name, kMLString) == 0)) {
				unit = MDistance.Unit.kMeters;
			} else {
				//  This is not a recognized distance unit.
				//
				state = false;
				// Use format to place variable string into message
				string msgFmt = MStringResource.getString(RegisterMStringResources.kInvalidLinearUnits);
				string msg = string.Format(msgFmt, name);
				MGlobal.displayError(msg);
				unit = MDistance.Unit.kInvalid;
			}

			return state;
		}

		/* static */
		public static bool setFromName(string name, ref MTime.Unit unit)
		//
		//	Description:
		//		The time unit is set based on the passed string. If the string
		//		is not recognized, the time unit is set to MTime::kInvalid.
		//
		{
			bool state = true;

			if (string.Compare(name, kHourTString) == 0) {
				unit = MTime.Unit.kHours;
			} else if (string.Compare(name, kMinTString) == 0) {
				unit = MTime.Unit.kMinutes;
			} else if (string.Compare(name, kSecTString) == 0) {
				unit = MTime.Unit.kSeconds;
			} else if (string.Compare(name, kMillisecTString) == 0) {
				unit = MTime.Unit.kMilliseconds;
			} else if (string.Compare(name, kGameTString) == 0) {
				unit = MTime.Unit.kGames;
			} else if (string.Compare(name, kFileTString) == 0) {
				unit = MTime.Unit.kFilm;
			} else if (string.Compare(name, kPalTString) == 0) {
				unit = MTime.Unit.kPALFrame;
			} else if (string.Compare(name, kNtscTString) == 0) {
				unit = MTime.Unit.kNTSCFrame;
			} else if (string.Compare(name, kShowTString) == 0) {
				unit = MTime.Unit.kShowScan;
			} else if (string.Compare(name, kPalFTString) == 0) {
				unit = MTime.Unit.kPALField;
			} else if (string.Compare(name, kNtscFTString) == 0) {
				unit = MTime.Unit.kNTSCField;
			} else {
				//  This is not a recognized time unit.
				//
				unit = MTime.Unit.kInvalid;
				// Use format to place variable string into message
				string msgFmt = MStringResource.getString(RegisterMStringResources.kInvalidTimeUnits);
				string msg = string.Format(msgFmt, name);
				MGlobal.displayError(msg);
				state = false;
			}

			return state;
		}
	}
	
	public class StreamReaderExt:StreamReader
	{
		public StreamReaderExt(string path) : base(path) { }
		public StreamReaderExt(Stream stream) : base(stream) { }
		public char ReadChar()
		{
			int c=base.Read();
			if(c==-1)return '\0';
			else return char.ConvertFromUtf32(c).ToCharArray()[0];
		}

		string ReadNumber(bool hasfloat,bool sign)
		{
			string s1="";
			bool numberend=false;
			while(true){
				char c=ReadChar();
				if(!numberend && sign && c=='-'){ s1+=c;sign =false;}
				if(char.IsControl(c) && char.IsWhiteSpace(c)){ if(numberend)break;}
				else if(char.IsDigit(c) || hasfloat && c=='.'){ s1+=c;numberend=true;}
				else break;
			}
			return s1;
		}
		
		public double ReadDouble()
		{ 
			return double.Parse(ReadNumber(true,true));
		}
	}

	public class animBase
	{
		public enum AnimBaseType			
		{
			kAnimBaseUnitless, 
			kAnimBaseTime,
			kAnimBaseLinear,
			kAnimBaseAngular
		};
		// Tangent type words
		//
		public const string kWordTangentGlobal = "global";
		public const string kWordTangentFixed = "fixed";
		public const string kWordTangentLinear = "linear";
		public const string kWordTangentFlat = "flat";
		public const string kWordTangentSmooth = "spline";
		public const string kWordTangentStep = "step";
		public const string kWordTangentSlow = "slow";
		public const string kWordTangentFast = "fast";
		public const string kWordTangentClamped = "clamped";
		public const string kWordTangentPlateau = "plateau";
		public const string kWordTangentStepNext = "stepnext";
		public const string kWordTangentAuto = "auto";

		// Infinity type words
		//
		public const string kWordConstant = "constant";
		public const string kWordLinear = "linear";
		public const string kWordCycle = "cycle";
		public const string kWordCycleRelative = "cycleRelative";
		public const string kWordOscillate = "oscillate";

		//	Param Curve types
		//
		public const string kWordTypeUnknown = "unknown";
		public const string kWordTypeLinear = "linear";
		public const string kWordTypeAngular = "angular";
		public const string kWordTypeTime = "time";
		public const string kWordTypeUnitless = "unitless";

		//	Keywords
		//
		public const string kAnim = "anim";
		public const string kAnimData = "animData";
		public const string kMovData = "movData";
		public const string kMayaVersion = "mayaVersion";
		public const string kAnimVersion = "animVersion";

		public const string kTimeUnit = "timeUnit";
		public const string kLinearUnit = "linearUnit";
		public const string kAngularUnit = "angularUnit";
		public const string kStartTime = "startTime";
		public const string kEndTime = "endTime";
		public const string kStartUnitless = "startUnitless";
		public const string kEndUnitless = "endUnitless";

		// animVersions:
		//
		public const string kAnimVersionString = "1.1";
		// const double kVersion1 = 1.0;						// initial release
		public const double kVersionNonWeightedAndBreakdowns = 1.1;	// added support for non-weighted curves and breakdowns

		public const string kTwoSpace = "  ";

		//	animData keywords
		//
		public const string kInputString = "input";
		public const string kOutputString = "output";
		public const string kWeightedString = "weighted";
		public const string kPreInfinityString = "preInfinity";
		public const string kPostInfinityString = "postInfinity";
		public const string kInputUnitString = "inputUnit";
		public const string kOutputUnitString = "outputUnit";
		public const string kTanAngleUnitString = "tangentAngleUnit";
		public const string kKeysString = "keys";

		//	special characters
		//
		public const char kSemiColonChar = ';';
		public const char kSpaceChar = ' ';
		public const char kTabChar = '\t';
		public const char kHashChar = '#';
		public const char kNewLineChar = '\n';
		public const char kSlashChar = '/';
		public const char kBraceLeftChar = '{';
		public const char kBraceRightChar = '}';
		public const char kDoubleQuoteChar = '"';

		protected MTime.Unit timeUnit;
		protected MAngle.Unit angularUnit;
		protected MDistance.Unit linearUnit;

		public animBase()
		{
			resetUnits();
		}

		//	Description:
		//		Reset the units used by this class to the ui units.
		//
		public void resetUnits()
		{
			timeUnit = MTime.uiUnit;
			linearUnit = MDistance.uiUnit;
			angularUnit = MAngle.uiUnit;
		}

		public string tangentTypeAsWord(MFnAnimCurve.TangentType type)
		//	Description:
		//		Returns a string with a test based desription of the passed
		//		MFnAnimCurve::TangentType. 
		//
		{
			switch (type)
			{
				case MFnAnimCurve.TangentType.kTangentGlobal:
					return kWordTangentGlobal;
				case MFnAnimCurve.TangentType.kTangentFixed:
					return (kWordTangentFixed);
				case MFnAnimCurve.TangentType.kTangentLinear:
					return (kWordTangentLinear);
				case MFnAnimCurve.TangentType.kTangentFlat:
					return (kWordTangentFlat);
				case MFnAnimCurve.TangentType.kTangentSmooth:
					return (kWordTangentSmooth);
				case MFnAnimCurve.TangentType.kTangentStep:
					return (kWordTangentStep);
				case MFnAnimCurve.TangentType.kTangentStepNext:
					return (kWordTangentStepNext);
				case MFnAnimCurve.TangentType.kTangentSlow:
					return (kWordTangentSlow);
				case MFnAnimCurve.TangentType.kTangentFast:
					return (kWordTangentFast);
				case MFnAnimCurve.TangentType.kTangentClamped:
					return (kWordTangentClamped);
				case MFnAnimCurve.TangentType.kTangentPlateau:
					return (kWordTangentPlateau);
				case MFnAnimCurve.TangentType.kTangentAuto:
					return (kWordTangentAuto);
				default:
					break;
			}

			return (kWordTangentGlobal);
		}

		public MFnAnimCurve.TangentType wordAsTangentType(string type)
		//	Description:
		//		Returns a MFnAnimCurve::TangentType based on the passed string.
		//		If the string is not a recognized tangent type, tnen
		//		MFnAnimCurve::kTangentGlobal is returned.
		//
		{
			if (string.Compare(type, kWordTangentGlobal) == 0)
			{
				return (MFnAnimCurve.TangentType.kTangentGlobal);
			}
			if (string.Compare(type, kWordTangentFixed) == 0)
			{
				return (MFnAnimCurve.TangentType.kTangentFixed);
			}
			if (string.Compare(type, kWordTangentLinear) == 0)
			{
				return (MFnAnimCurve.TangentType.kTangentLinear);
			}
			if (string.Compare(type, kWordTangentFlat) == 0)
			{
				return (MFnAnimCurve.TangentType.kTangentFlat);
			}
			if (string.Compare(type, kWordTangentSmooth) == 0)
			{
				return (MFnAnimCurve.TangentType.kTangentSmooth);
			}
			if (string.Compare(type, kWordTangentStep) == 0)
			{
				return (MFnAnimCurve.TangentType.kTangentStep);
			}
			if (string.Compare(type, kWordTangentStepNext) == 0)
			{
				return (MFnAnimCurve.TangentType.kTangentStepNext);
			}
			if (string.Compare(type, kWordTangentSlow) == 0)
			{
				return (MFnAnimCurve.TangentType.kTangentSlow);
			}
			if (string.Compare(type, kWordTangentFast) == 0)
			{
				return (MFnAnimCurve.TangentType.kTangentFast);
			}
			if (string.Compare(type, kWordTangentClamped) == 0)
			{
				return (MFnAnimCurve.TangentType.kTangentClamped);
			}
			if (string.Compare(type, kWordTangentPlateau) == 0)
			{
				return (MFnAnimCurve.TangentType.kTangentPlateau);
			}
			if (string.Compare(type, kWordTangentAuto) == 0)
			{
				return (MFnAnimCurve.TangentType.kTangentAuto);
			}
			return (MFnAnimCurve.TangentType.kTangentGlobal);
		}

		public string infinityTypeAsWord(MFnAnimCurve.InfinityType type)
		//	Description:
		//		Returns a string containing the name of the passed 
		//		MFnAnimCurve::InfinityType type.
		//
		{
			switch (type)
			{
				case MFnAnimCurve.InfinityType.kConstant:
					return (kWordConstant);
				case MFnAnimCurve.InfinityType.kLinear:
					return (kWordLinear);
				case MFnAnimCurve.InfinityType.kCycle:
					return (kWordCycle);
				case MFnAnimCurve.InfinityType.kCycleRelative:
					return (kWordCycleRelative);
				case MFnAnimCurve.InfinityType.kOscillate:
					return (kWordOscillate);
				default:
					break;
			}
			return (kWordConstant);
		}

		public MFnAnimCurve.InfinityType wordAsInfinityType(string type)
		//	Description:
		//		Returns a MFnAnimCurve::InfinityType from the passed string.
		//		If the string does not match a recognized infinity type,
		//		MFnAnimCurve::kConstant is returned.
		//
		{
			if (string.Compare(type, kWordConstant) == 0)
			{
				return (MFnAnimCurve.InfinityType.kConstant);
			}
			else if (string.Compare(type, kWordLinear) == 0)
			{
				return (MFnAnimCurve.InfinityType.kLinear);
			}
			else if (string.Compare(type, kWordCycle) == 0)
			{
				return (MFnAnimCurve.InfinityType.kCycle);
			}
			else if (string.Compare(type, kWordCycleRelative) == 0)
			{
				return (MFnAnimCurve.InfinityType.kCycleRelative);
			}
			else if (string.Compare(type, kWordOscillate) == 0)
			{
				return (MFnAnimCurve.InfinityType.kOscillate);
			}

			return (MFnAnimCurve.InfinityType.kConstant);
		}

		public string outputTypeAsWord(MFnAnimCurve.AnimCurveType type)
		//
		//	Description:
		//		Returns a string identifying the output type of the
		//		passed MFnAnimCurve::AnimCurveType.
		//
		{
			switch (type)
			{
				case MFnAnimCurve.AnimCurveType.kAnimCurveTL:
				case MFnAnimCurve.AnimCurveType.kAnimCurveUL:
					return (kWordTypeLinear);
				case MFnAnimCurve.AnimCurveType.kAnimCurveTA:
				case MFnAnimCurve.AnimCurveType.kAnimCurveUA:
					return (kWordTypeAngular);
				case MFnAnimCurve.AnimCurveType.kAnimCurveTT:
				case MFnAnimCurve.AnimCurveType.kAnimCurveUT:
					return (kWordTypeTime);
				case MFnAnimCurve.AnimCurveType.kAnimCurveTU:
				case MFnAnimCurve.AnimCurveType.kAnimCurveUU:
					return (kWordTypeUnitless);
				case MFnAnimCurve.AnimCurveType.kAnimCurveUnknown:
					return (kWordTypeUnitless);
			}
			return (kWordTypeUnknown);
		}

		public animBase.AnimBaseType wordAsInputType(string input)
		//	Description:
		//		Returns an input type based on the passed string.
		//
		{
			if (string.Compare(input, kWordTypeTime) == 0) {
				return AnimBaseType.kAnimBaseTime;
			} else {
				return AnimBaseType.kAnimBaseUnitless;
			}
		}


		public AnimBaseType wordAsOutputType(string output) 
		//
		//	Description:
		//		Returns a output type based on the passed string.
		//
		{
			if (string.Compare(output, kWordTypeLinear) == 0) {
				return AnimBaseType.kAnimBaseLinear;
			} else if (string.Compare(output, kWordTypeAngular) == 0) {
				return AnimBaseType.kAnimBaseAngular;
			} else if (string.Compare(output, kWordTypeTime) == 0) {
				return AnimBaseType.kAnimBaseTime;
			} else {
				return AnimBaseType.kAnimBaseUnitless;
			}
		}

		public string boolInputTypeAsWord(bool isUnitless) 
		//
		//	Description:
		//		Returns a string based on a bool. 
		//
		{
			if (isUnitless) {
				return (kWordTypeUnitless);
			} else {
				return (kWordTypeTime);
			}
		}

		public MFnAnimCurve.AnimCurveType typeAsAnimCurveType(	AnimBaseType input, AnimBaseType output)
		//
		//	Description:
		//		Returns a MFnAnimCurve::AnimCurveType based on the passed
		//		input and output types. If the input and output types do
		//		not create a valid MFnAnimCurve::AnimCurveType, then a
		//		MFnAnimCurve::kAnimCurveUnknown is returned.
		//
		{
			MFnAnimCurve.AnimCurveType type = MFnAnimCurve.AnimCurveType.kAnimCurveUnknown;

			switch (output) {
				case AnimBaseType.kAnimBaseLinear:
					if (AnimBaseType.kAnimBaseUnitless == input) {
						type = MFnAnimCurve.AnimCurveType.kAnimCurveUL;
					} else {
						type = MFnAnimCurve.AnimCurveType.kAnimCurveTL;
					}
					break;
				case AnimBaseType.kAnimBaseAngular:
					if (AnimBaseType.kAnimBaseUnitless == input) {
						type = MFnAnimCurve.AnimCurveType.kAnimCurveUA;
					} else {
						type = MFnAnimCurve.AnimCurveType.kAnimCurveTA;
					}
					break;
				case AnimBaseType.kAnimBaseTime:
					if (AnimBaseType.kAnimBaseUnitless == input) {
						type = MFnAnimCurve.AnimCurveType.kAnimCurveUT;
					} else {
						type = MFnAnimCurve.AnimCurveType.kAnimCurveTT;
					}
					break;
				case AnimBaseType.kAnimBaseUnitless:
					if (AnimBaseType.kAnimBaseUnitless == input) {
						type = MFnAnimCurve.AnimCurveType.kAnimCurveUU;
					} else {
						type = MFnAnimCurve.AnimCurveType.kAnimCurveTU;
					}
					break;
				default:
					//	An unknown anim curve type.
					//
					break;
			}

			return type;
		}

		public double asDouble(ref StreamReaderExt clipFile)
		//	Description:
		//		Reads the next bit of valid data as a double.
		//
		{
			double value = 0.0;
			try
			{
				advance(ref clipFile);
				value = clipFile.ReadDouble();
			}
			catch (System.Exception )
			{
			}
			

			return (value);
		}

		public bool isNextNumeric(ref StreamReaderExt  clipFile)
		//	Description:
		//		The method skips past whitespace and comments and checks if
		//		the next character is numeric.
		//		
		//		true is returned if the character is numeric.
		//
		{
			bool numeric = false;
			advance(ref clipFile);

			char next = (char)clipFile.Peek();
			if (next >= '0' && next <= '9')
			{
				numeric = true;
			}

			return numeric;
		}

		public void advance(ref StreamReaderExt clipFile)
		//	Description:
		//		The method skips past all of the whitespace and commented lines
		//		in the stream. It will also ignore semi-colons.
		//
		{
			while (!clipFile.EndOfStream)
			{
				try
				{
					char next = (char)clipFile.Peek();
					if (char.IsWhiteSpace(next))
					{
						clipFile.ReadChar();
						continue;
					}
					if (next == kSemiColonChar)
					{
						clipFile.ReadChar();
						continue;
					}
					if (next == kSlashChar || next == kHashChar)
					{
						clipFile.ReadLine();
						continue;
					}
					break;
				}
				catch (System.Exception )
				{
					return;
				}
				
			}
		}

		public string asWord(ref StreamReaderExt clipFile, bool includeWS = false)
		//	Description:
		//		Returns the next string of characters in an stream. The string
		//		ends when whitespace or a semi-colon is encountered. If the 
		//		includeWS argument is true, the string will not end if a white
		//		space character is encountered.
		//
		//		If a double quote is detected '"', then very thing up to the next 
		//		double quote will be returned.
		//
		//		This method returns a pointer to a static variable, so its contents
		//		should be used immediately.
		//	
		{
			string str = "";
			advance(ref clipFile);
			char c = clipFile.ReadChar();
			if (c == kDoubleQuoteChar)
			{
				c = clipFile.ReadChar();
				str += c;
				while (!clipFile.EndOfStream && c != kDoubleQuoteChar)
				{
					c = clipFile.ReadChar();
					str += c;
				}
			}
			else
			{
				//	Get the case of the '{' or '}' character
				//
				if (c == kBraceLeftChar || c == kBraceRightChar)
					str+=c;
				else
				{
					while (!clipFile.EndOfStream && c != kSemiColonChar)
					{
						if (!includeWS && (c == kSpaceChar || c == kTabChar))
							break;
						str += c;
						c = clipFile.ReadChar();
					}
				}
			}
			return str;
		}

		char asChar (ref StreamReaderExt clipFile)
		//	Description:
		//		Returns the next character of interest in the stream. All 
		//		whitespace and commented lines are ignored.
		//
		{
			advance(ref clipFile);
			return clipFile.ReadChar();
		}

		public bool isEquivalent(double a, double b)
		//
		//	Description:
		//		Returns true if the doubles are within the tolerance.
		//
		{
			const double tolerance = 1.0e-10;
			return ((a > b) ? (a - b <= tolerance) : (b - a <= tolerance));
		}
	}

	public class animReader : animBase
	{
		bool convertAnglesFromV2To3;
		bool convertAnglesFromV3To2;
		double animVersion;

		public animReader()
		{
			animVersion = 1.0;
			convertAnglesFromV2To3 = false;
			convertAnglesFromV3To2 = false;
		}

		public void readClipboard(ref StreamReaderExt readAnim, MAnimCurveClipboard cb)
		//	Description:
		//		Given a clipboard and an ifstream, read the ifstream and add
		//		all of the anim curves described in ther stream into the 
		//		API clipboard.
		//
		{
			//	Set the default values for the start and end of the clipboard.
			//	The MAnimCurveClipboard::set() method will examine all of the
			//	anim curves are determine the proper start and end values, if the
			//	start time is greater than the end value.
			//
			//	By default, the start values are greater than the end values to 
			//	ensure correct behavior if the file does not specify the start and
			//	end values.
			//
			double startTime = 1.0;
			double endTime = 0.0;
			double startUnitless = 1.0;
			double endUnitless = 0.0;

			resetUnits();
			convertAnglesFromV2To3 = false;
			convertAnglesFromV3To2 = false;

			//	Read the header. The header officially ends when the first non-header
			//	keyword is found. The header contains clipboard specific information
			//	where the body is anim curve specific.
			//
			string dataType = "";
			bool hasVersionString = false;
			while (!readAnim.EndOfStream)
			{
				advance(ref readAnim);
				dataType = asWord(ref readAnim);

				if (string.Compare(dataType, kAnimVersion) == 0)
				{
					string version = asWord(ref readAnim);
					animVersion = Convert.ToDouble(version);
					string thisVersion = kAnimVersionString;

					hasVersionString = true;

					//	Add versioning control here.
					//
					if (version != thisVersion) 
					{
						// Use format to place variable string into message
						string msgFmt = MStringResource.getString(RegisterMStringResources.kInvalidVersion);
						string msg = string.Format(msgFmt, thisVersion, version);
						MGlobal.displayWarning(msg);
					}
				}
				else if (string.Compare(dataType, kMayaVersion) == 0)
				{
					string version = asWord(ref readAnim, true);
					string currentVersion = MGlobal.mayaVersion;
					if (currentVersion.Substring(0, 2) == "2.")
					{
						string vCheck = version.Substring(0, 2);
						if (vCheck != "2.")
							convertAnglesFromV3To2 = true;
					}
					else
					{
						//	If this is a pre-Maya 3.0 file, then the tangent angles 
						//	will need to be converted to work in Maya 3.0+
						//
						string vCheck = version.Substring(0, 2);
						if (vCheck == "2.")
						{
							convertAnglesFromV2To3 = true;
						}
					}
				}
				else if (string.Compare(dataType, kTimeUnit) == 0)
				{
					string timeUnitString = asWord(ref readAnim);
					if (!animUnitNames.setFromName(timeUnitString, ref timeUnit))
					{
						string unitName = "";
						timeUnit = MTime.uiUnit;
						animUnitNames.setToShortName(timeUnit, ref unitName);
						// Use format to place variable string into message
						string msgFmt = MStringResource.getString(RegisterMStringResources.kSettingToUnit);
						string msg = string.Format(msgFmt, kTimeUnit, unitName);
						MGlobal.displayWarning(msg);
					}
				}
				else if (string.Compare(dataType, kLinearUnit) == 0) 
				{
					string linearUnitString = asWord(ref readAnim);
					if (!animUnitNames.setFromName(linearUnitString, ref linearUnit)) 
					{
						string unitName = "";
						linearUnit = MDistance.uiUnit;
						animUnitNames.setToShortName(linearUnit, ref unitName);
					   
						// Use format to place variable string into message
						string msgFmt = MStringResource.getString(RegisterMStringResources.kSettingToUnit);
						string msg = string.Format(msgFmt, kLinearUnit, unitName);
						MGlobal.displayWarning(msg);
					}
				} 
				else if (string.Compare(dataType, kAngularUnit) == 0) 
				{
					string angularUnitString = asWord( ref readAnim);
					if (!animUnitNames.setFromName(angularUnitString, ref angularUnit)) 
					{
						string unitName = "";
						angularUnit = MAngle.uiUnit;
						animUnitNames.setToShortName(angularUnit, ref unitName);
						
						// Use format to place variable string into message
						string msgFmt = MStringResource.getString(RegisterMStringResources.kSettingToUnit);
						string msg = string.Format(msgFmt, kAngularUnit, unitName);
						MGlobal.displayWarning(msg);
					}
				} 
				else if (string.Compare(dataType, kStartTime) == 0) 
					startTime = asDouble(ref readAnim);
				else if (string.Compare(dataType, kEndTime) == 0)
					endTime = asDouble(ref readAnim);
				else if (string.Compare(dataType, kStartUnitless) == 0) 
					startUnitless = asDouble(ref readAnim);
				else if (string.Compare(dataType, kEndUnitless) == 0) 
					endUnitless = asDouble(ref readAnim);
				else {	
					//	The header should be finished. Begin to parse the body.
					//
					break;
				}
			}

			//	The animVersion string is required.
			//
			if (!hasVersionString) 
			{
				// Use format to place variable string into message
				string msgFmt = MStringResource.getString(RegisterMStringResources.kMissingKeyword);
				string msg = string.Format(msgFmt, kAnimVersion);
				throw new ArgumentException(msg, "cb"); 
			}

			//	Set the linear and angular units to be the same as the file
			//	being read. This will allow fixed tangent data to be read
			//	in properly if the scene has different units than the .anim file.
			//
			MDistance.Unit oldDistanceUnit = MDistance.uiUnit;
			MTime.Unit oldTimeUnit = MTime.uiUnit;

			MDistance.uiUnit = linearUnit;
			MTime.uiUnit = timeUnit;

			MAnimCurveClipboardItemArray clipboardArray = new MAnimCurveClipboardItemArray();
			while (!readAnim.EndOfStream) 
			{

				if ("" == dataType) {
					dataType = asWord(ref readAnim);
				}

				if (string.Compare(dataType, kAnim) == 0) 
				{
					string fullAttributeName = "", leafAttributeName="", nodeName = "";

					//	If this is from an unconnected anim curve, then there
					//	will not be an attribute name.
					//
					if (!isNextNumeric(ref readAnim)) 
					{
						fullAttributeName = asWord(ref readAnim);

						//	If the node names were specified, then the next two
						//	words should be the leaf attribute and the node name.
						//
						if (!isNextNumeric(ref readAnim)) 
						{
							leafAttributeName = asWord(ref readAnim);
							nodeName = asWord(ref readAnim);
						}
					}

					uint rowCount, childCount, attrCount;
					rowCount = (uint)asDouble(ref readAnim);
					childCount = (uint)asDouble(ref readAnim);
					attrCount = (uint)asDouble(ref readAnim);

					//	If the next keyword is not an animData, then this is 
					//	a place holder for the API clipboard.
					//
					dataType = asWord(ref readAnim);
					if (string.Compare(dataType, kAnimData) == 0) 
					{
						MAnimCurveClipboardItem clipboardItem = new MAnimCurveClipboardItem();
						if (readAnimCurve(ref readAnim, ref clipboardItem)) 
						{
							clipboardItem.setAddressingInfo(rowCount, 
															childCount, attrCount);
							clipboardItem.setNameInfo(	nodeName, 
														fullAttributeName, 
														leafAttributeName);
							clipboardArray.append(clipboardItem);
						} 
						else 
						{
							//	Could not read the anim curve.
							//
							string msg = MStringResource.getString(RegisterMStringResources.kCouldNotReadAnim);
							MGlobal.displayError(msg);
						}
					} 
					else 
					{
						//	This must be a place holder object for the clipboard.
						//
						MAnimCurveClipboardItem clipboardItem = new MAnimCurveClipboardItem();
						clipboardItem.setAddressingInfo(rowCount, 
														childCount, attrCount);

						//	Since there is no anim curve specified, what is 
						//	in the fullAttributeName string is really the node 
						//	name.
						//
						clipboardItem.setNameInfo(	fullAttributeName, 
													nodeName, 
													leafAttributeName);
						clipboardArray.append(clipboardItem);

						//	dataType already contains the next keyword. 
						//	
						continue;
					}
				} 
				else 
				{
					if (!readAnim.EndOfStream)
					{
						string warnStr = dataType;
						
						// Use format to place variable string into message
						string msgFmt = MStringResource.getString(RegisterMStringResources.kUnknownKeyword);
						string msg = string.Format(msgFmt, warnStr);
						MGlobal.displayError(msg);

						//	Skip to the next line, this one is invalid.
						//
						readAnim.ReadLine();
					}
					else
					{
						//	The end of the file was reached. 
						//
						break;
					}
				}

				//	Skip any whitespace.
				//
				dataType = "";
			}

			try
			{
				cb.set(	clipboardArray, 
						new MTime(startTime, timeUnit), new MTime(endTime, timeUnit), 
						(float) startUnitless, (float) endUnitless) ;
			}
			catch (System.Exception)
			{
				string msg = MStringResource.getString(RegisterMStringResources.kClipboardFailure);
				MGlobal.displayError(msg);
			}

			//	Restore the old units.
			//
			MDistance.uiUnit = oldDistanceUnit;
			MTime.uiUnit = oldTimeUnit;
		}

		protected void convertAnglesAndWeights3To2(MFnAnimCurve.AnimCurveType type,
								bool isWeighted, ref MAngle angle, ref double weight)
		//
		//	Description:
		//		Converts the tangent angles from Maya 3.0 to Maya2.* formats.
		//
		{
			double oldAngle = angle.asRadians;
			double newAngle = oldAngle;

			//	Calculate the scale values for the conversion.
			//
			double xScale = 1.0;
			double yScale = 1.0;

			MTime tOne = new MTime(1.0, MTime.Unit.kSeconds);
			if (type == MFnAnimCurve.AnimCurveType.kAnimCurveTT ||
				type == MFnAnimCurve.AnimCurveType.kAnimCurveTL ||
				type == MFnAnimCurve.AnimCurveType.kAnimCurveTA ||
				type == MFnAnimCurve.AnimCurveType.kAnimCurveTU) {

				xScale = tOne.asUnits(MTime.uiUnit);
			}

			switch (type) 
			{
				case MFnAnimCurve.AnimCurveType.kAnimCurveTT:
				case MFnAnimCurve.AnimCurveType.kAnimCurveUT:
					yScale = tOne.asUnits(MTime.uiUnit);
					break;
				case MFnAnimCurve.AnimCurveType.kAnimCurveTL:
				case MFnAnimCurve.AnimCurveType.kAnimCurveUL:
					{
						MDistance dOne = new MDistance(1.0, MDistance.internalUnit);
						yScale = dOne.asUnits(linearUnit);
					}
					break;
				case MFnAnimCurve.AnimCurveType.kAnimCurveTA:
				case MFnAnimCurve.AnimCurveType.kAnimCurveUA:
					{
						MAngle aOne = new MAngle(1.0, MAngle.internalUnit);
						yScale = aOne.asUnits(angularUnit);
					}
					break;
				case MFnAnimCurve.AnimCurveType.kAnimCurveTU:
				case MFnAnimCurve.AnimCurveType.kAnimCurveUU:
				default:
					break;
			}

			double tanAngle = Math.Tan(oldAngle);
			newAngle = Math.Atan((xScale*tanAngle)/yScale);

			if (isWeighted) 
			{
				double sinAngle = Math.Sin(oldAngle);
				double cosAngle = Math.Cos(oldAngle);

				double denominator = (yScale*yScale*sinAngle*sinAngle) +
										(xScale*xScale*cosAngle*cosAngle);
				weight = Math.Sqrt(weight/denominator);
			}

			MAngle finalAngle = new MAngle(newAngle, MAngle.Unit.kRadians);
			angle = finalAngle;
		}

		protected void convertAnglesAndWeights2To3(MFnAnimCurve.AnimCurveType type,
								bool isWeighted, ref MAngle angle, ref double weight)
		//
		//	Description:
		//		Converts the tangent angles from Maya 2.* to Maya3.0+ formats.
		//
		{
			double oldAngle = angle.asRadians;

			double newAngle = oldAngle;
			double newWeight = weight;

			//	Calculate the scale values for the conversion.
			//
			double xScale = 1.0;
			double yScale = 1.0;

			MTime tOne = new MTime(1.0, MTime.Unit.kSeconds);
			if (type == MFnAnimCurve.AnimCurveType.kAnimCurveTT ||
				type == MFnAnimCurve.AnimCurveType.kAnimCurveTL ||
				type == MFnAnimCurve.AnimCurveType.kAnimCurveTA ||
				type == MFnAnimCurve.AnimCurveType.kAnimCurveTU) {

				xScale = tOne.asUnits(MTime.uiUnit);
			}

			switch (type) 
			{
				case MFnAnimCurve.AnimCurveType.kAnimCurveTT:
				case MFnAnimCurve.AnimCurveType.kAnimCurveUT:
					yScale = tOne.asUnits(MTime.uiUnit);
					break;
				case MFnAnimCurve.AnimCurveType.kAnimCurveTL:
				case MFnAnimCurve.AnimCurveType.kAnimCurveUL:
					{
						MDistance dOne = new MDistance(1.0, MDistance.internalUnit);
						yScale = dOne.asUnits(linearUnit);
					}
					break;
				case MFnAnimCurve.AnimCurveType.kAnimCurveTA:
				case MFnAnimCurve.AnimCurveType.kAnimCurveUA:
					{
						MAngle aOne = new MAngle(1.0, MAngle.internalUnit);
						yScale = aOne.asUnits(angularUnit);
					}
					break;
				case MFnAnimCurve.AnimCurveType.kAnimCurveTU:
				case MFnAnimCurve.AnimCurveType.kAnimCurveUU:
				default:
					break;
			}

			const double quarter = Math.PI/2;
			if (isEquivalent(oldAngle, 0.0) ||
				isEquivalent(oldAngle, quarter) ||
				isEquivalent(oldAngle, -quarter)) {
		
				newAngle = oldAngle;

				if (isWeighted) {
					newWeight = yScale*oldAngle;
				}
			} else {
				double tanAngle = Math.Tan(oldAngle);
				newAngle = Math.Atan((yScale*tanAngle)/xScale);
			
				if (isWeighted) {
					double cosAngle = Math.Cos(oldAngle);
					double cosSq = cosAngle*cosAngle;

					double wSq = (weight*weight) * 
							(((xScale*xScale - yScale*yScale)*cosSq) + (yScale*yScale));

					newWeight = Math.Sqrt(wSq);
				}
			}

			weight = newWeight;

			MAngle finalAngle = new MAngle(newAngle, MAngle.Unit.kRadians);
			angle = finalAngle;
		}

		protected bool readAnimCurve(ref StreamReaderExt clipFile, ref MAnimCurveClipboardItem item)
		//	Description:
		//		Read a block of the stream that should contain anim curve
		//		data in the format determined by the animData keyword.
		//
		{
			MFnAnimCurve animCurve = new MFnAnimCurve();
			MObject animCurveObj = new MObject(); 

			//	Anim curve defaults.
			//
			animBase.AnimBaseType input = wordAsInputType(kWordTypeTime);
			animBase.AnimBaseType output = wordAsOutputType(kWordTypeLinear);
			MFnAnimCurve.InfinityType preInf = wordAsInfinityType(kWordConstant);
			MFnAnimCurve.InfinityType postInf = wordAsInfinityType(kWordConstant);

			string inputUnitName = "";
			animUnitNames.setToShortName(timeUnit, ref inputUnitName);
			string outputUnitName = "";
			MAngle.Unit tanAngleUnit = angularUnit;
			bool isWeighted = false;

			string dataType = "";

			while (!clipFile.EndOfStream) 
			{
				advance(ref clipFile);

				dataType = asWord(ref clipFile);

				if (string.Compare(dataType, kInputString) == 0) {
					input = wordAsInputType(asWord(ref clipFile));
				} else if (string.Compare(dataType, kOutputString) == 0) {
					output = wordAsOutputType(asWord(ref clipFile));
				} else if (string.Compare(dataType, kWeightedString) == 0) {
					isWeighted = (asDouble(ref clipFile) == 1.0);
				} else if (string.Compare(dataType, kPreInfinityString) == 0) {
					preInf = wordAsInfinityType(asWord(ref clipFile));
				} else if (string.Compare(dataType, kPostInfinityString) == 0) {
					postInf = wordAsInfinityType(asWord(ref clipFile));
				} else if (string.Compare(dataType, kInputUnitString) == 0) {
					inputUnitName = asWord(ref clipFile);
				} else if (string.Compare(dataType, kOutputUnitString) == 0) {
					outputUnitName = asWord(ref clipFile);
				} else if (string.Compare(dataType, kTanAngleUnitString) == 0) {
					string tUnit = asWord(ref clipFile);
					if (!animUnitNames.setFromName(tUnit, ref tanAngleUnit)) {
						string unitName="";
						tanAngleUnit = angularUnit;
						animUnitNames.setToShortName(tanAngleUnit, ref unitName);
 
						// Use format to place variable string into message
						string msgFmt = MStringResource.getString(RegisterMStringResources.kSettingTanAngleUnit);
						string msg = string.Format(msgFmt, unitName);
						MGlobal.displayError(msg);
					}
				} else if (string.Compare(dataType, kKeysString) == 0) {
					//	Ignore the rest of this line.
					//
					clipFile.ReadLine();
					break;
				} else if (string.Compare(dataType, "{") == 0) {
					//	Skippping the '{' character. Just ignore it.
					//
					continue;
				} else {
					//	An unrecogized keyword was found.
					//
					string warnStr = (dataType);
					// Use format to place variable string into message
					string msgFmt = MStringResource.getString(RegisterMStringResources.kUnknownKeyword);
					string msg = string.Format(msgFmt, warnStr);
					MGlobal.displayError(msg);
					continue;
				}
			}

			// Read the animCurve
			//
			MFnAnimCurve.AnimCurveType type = typeAsAnimCurveType(input, output);
			try
			{
				animCurveObj = animCurve.create(type,null);
			}
			catch (System.Exception )
			{
				string msg = MStringResource.getString(RegisterMStringResources.kCouldNotCreateAnim);
				MGlobal.displayError(msg);
				return false;
			}

			animCurve.setIsWeighted(isWeighted);
			animCurve.setPreInfinityType(preInf);
			animCurve.setPostInfinityType(postInf);
			
			//	Set the appropriate units.
			//
			MTime.Unit inputTimeUnit = MTime.Unit.kInvalid;
			if (input == AnimBaseType.kAnimBaseTime)
			{
				if (!animUnitNames.setFromName(inputUnitName, ref inputTimeUnit)) 
				{
					string unitName = "";
					inputTimeUnit = timeUnit;
					animUnitNames.setToShortName(inputTimeUnit, ref unitName);
					// Use format to place variable string into message
					string msgFmt = MStringResource.getString(RegisterMStringResources.kSettingToUnit);
					string msg = string.Format(msgFmt, kInputUnitString, unitName);
					MGlobal.displayWarning(msg);
				}
			}
			
			MTime.Unit outputTimeUnit= MTime.Unit.kInvalid;
			if (output == AnimBaseType.kAnimBaseTime) 
			{
				if (!animUnitNames.setFromName(outputUnitName, ref outputTimeUnit))
				{
					string unitName = "";
					outputTimeUnit = timeUnit;
					animUnitNames.setToShortName(outputTimeUnit, ref unitName);
					// Use format to place variable string into message
					string msgFmt = MStringResource.getString(RegisterMStringResources.kSettingToUnit);
					string msg = string.Format(msgFmt, kOutputUnitString, unitName);
					MGlobal.displayWarning(msg);
				}
			}

			uint index = 0;
			double conversion = 1.0;
			if (output == AnimBaseType.kAnimBaseLinear)
			{
				MDistance.Unit unit = MDistance.Unit.kInvalid;
				if (outputUnitName.Length != 0) 
				{
					if (!animUnitNames.setFromName(outputUnitName, ref unit)) 
					{
						string unitName = "";
						unit = linearUnit;
						animUnitNames.setToShortName(unit, ref unitName);
						// Use format to place variable string into message
						string msgFmt = MStringResource.getString(RegisterMStringResources.kSettingToUnit);
						string msg = string.Format(msgFmt, kOutputUnitString, unitName);
						MGlobal.displayWarning(msg);
					}
				} 
				else 
					unit = linearUnit;

				if (unit != MDistance.Unit.kCentimeters)
				{
					MDistance one = new MDistance(1.0, unit);
					conversion = one.asCentimeters;
				}
			} 
			else if (output == AnimBaseType.kAnimBaseAngular) 
			{
				MAngle.Unit unit = MAngle.Unit.kInvalid;
				if (outputUnitName.Length != 0) 
				{
					if (!animUnitNames.setFromName(outputUnitName, ref unit)) 
					{
						string unitName = "";
						unit = angularUnit;
						animUnitNames.setToShortName(unit, ref unitName);
						// Use format to place variable string into message
						string msgFmt = MStringResource.getString(RegisterMStringResources.kSettingToUnit);
						string msg = string.Format(msgFmt, kOutputUnitString, unitName);
						MGlobal.displayWarning(msg);
					}
				} 
				else
					unit = angularUnit;

				if (unit != MAngle.Unit.kRadians) 
				{
					MAngle one = new MAngle(1.0, unit);
					conversion = one.asRadians;
				}
			}
			
			// Now read each keyframe
			//
			advance(ref clipFile);
			char c = (char)clipFile.Peek();
			while (!clipFile.EndOfStream && c != kBraceRightChar) 
			{
				double t = asDouble(ref clipFile);
				double val = asDouble(ref clipFile);

				MFnAnimCurve.TangentType tanIn = wordAsTangentType(asWord(ref clipFile));
				MFnAnimCurve.TangentType tanOut = wordAsTangentType(asWord(ref clipFile));

				switch (type) 
				{
					case MFnAnimCurve.AnimCurveType.kAnimCurveTT:
						index = animCurve.addKey(	new MTime(val, inputTimeUnit),
													new MTime(val, outputTimeUnit),
													tanIn, tanOut, null);
						break;
					case MFnAnimCurve.AnimCurveType.kAnimCurveTL:
					case MFnAnimCurve.AnimCurveType.kAnimCurveTA:
					case MFnAnimCurve.AnimCurveType.kAnimCurveTU:
						index = animCurve.addKey(	new MTime(t, inputTimeUnit),
													val*conversion, tanIn, tanOut,
													null);
						break;
					case MFnAnimCurve.AnimCurveType.kAnimCurveUL:
					case MFnAnimCurve.AnimCurveType.kAnimCurveUA:
					case MFnAnimCurve.AnimCurveType.kAnimCurveUU:
						index = animCurve.addKey(	t, val*conversion, 
													tanIn, tanOut,
													null);
						break;
					case MFnAnimCurve.AnimCurveType.kAnimCurveUT:
						index = animCurve.addKey(	t, new MTime(val, outputTimeUnit),
													tanIn, tanOut,
													null);
						break;
					default:
						string msg = MStringResource.getString(RegisterMStringResources.kUnknownNode);
						MGlobal.displayError(msg);
						return false;
				}

				//	Tangent locking needs to be called after the weights and 
				//	angles are set for the fixed tangents.
				//
				bool tLocked = asDouble(ref clipFile) == 1.0;
				bool swLocked = asDouble(ref clipFile) == 1.0;
				bool isBreakdown = false;
				if (animVersion >= kVersionNonWeightedAndBreakdowns) 
				{
					isBreakdown = (asDouble(ref clipFile) == 1.0);
				}

				//	Only fixed tangents need additional information.
				//
				if (tanIn == MFnAnimCurve.TangentType.kTangentFixed) 
				{
					MAngle inAngle = new MAngle(asDouble(ref clipFile), tanAngleUnit);
					double inWeight = asDouble(ref clipFile);

					//	If this is from a pre-Maya3.0 file, the tangent angles will 
					//	need to be converted.
					//
					if (convertAnglesFromV2To3) {
						convertAnglesAndWeights2To3(type,isWeighted,ref inAngle,ref inWeight);
					} else if (convertAnglesFromV3To2) {
						convertAnglesAndWeights3To2(type,isWeighted,ref inAngle,ref inWeight);
					}

					//  By default, the tangents are locked. When the tangents
					//	are locked, setting the angle and weight of a fixed in
					//	tangent may change the tangent type of the out tangent.
					//
					animCurve.setTangentsLocked(index, false);
					animCurve.setTangent(index, inAngle, inWeight, true);
				}

				//	Only fixed tangents need additional information.
				//
				if (tanOut == MFnAnimCurve.TangentType.kTangentFixed)
				{
					MAngle outAngle = new MAngle(asDouble(ref clipFile), tanAngleUnit);
					double outWeight = asDouble(ref clipFile);

					//	If this is from a pre-Maya3.0 file, the tangent angles will 
					//	need to be converted.
					//
					if (convertAnglesFromV2To3) {
						convertAnglesAndWeights2To3(type,isWeighted,ref outAngle,ref outWeight);
					} else if (convertAnglesFromV3To2) {
						convertAnglesAndWeights3To2(type,isWeighted,ref outAngle,ref outWeight);
					}
			
					//  By default, the tangents are locked. When the tangents
					//	are locked, setting the angle and weight of a fixed out 
					//	tangent may change the tangent type of the in tangent.
					//
					animCurve.setTangentsLocked(index, false);
					animCurve.setTangent(index, outAngle, outWeight, false);
				}

				//	To prevent tangent types from unexpectedly changing, tangent 
				//	locking should be the last operation. See the above comments
				//	about fixed tangent types for more information.
				//
				animCurve.setWeightsLocked(index, swLocked);
				animCurve.setTangentsLocked(index, tLocked);
				animCurve.setIsBreakdown (index, isBreakdown);

				//	There should be no additional data on this line. Go to the
				//	next line of data.
				//
				clipFile.ReadLine();

				//	Skip any comments.
				//
				advance(ref clipFile);
				c = (char)clipFile.Peek();
			}

			//	Ignore the brace that marks the end of the keys block.
			//
			if (c == kBraceRightChar) {
				clipFile.ReadLine();
			}

			//	Ignore the brace that marks the end of the animData block.
			//
			advance(ref clipFile);
			if ( (char)clipFile.Peek() == kBraceRightChar) {
				clipFile.ReadLine();
			} 
			else {
				//	Something is wrong.
				//
				string msg = MStringResource.getString(RegisterMStringResources.kMissingBrace);
				MGlobal.displayError(msg);
			}

			//	Do not set the clipboard with an empty clipboard item.
			//
			if (!animCurveObj.isNull) {
				item.animCurve = animCurveObj;
			}

			//	Delete the copy of the anim curve.
			//
			MGlobal.deleteNode(animCurveObj);

			return true;
		}
	}

	public class animWriter : animBase
	{
		public void writeClipboard(	ref StreamWriter animFile,  MAnimCurveClipboard cb, 
			bool nodeNames  = false ,
			bool verboseUnits = false)
		//	Description:
		//		Write the contents of the clipboard to the stream.
		//
		{
			// Check to see if there is anything on the clipboard at all
			//
			if (cb.isEmpty)
			{
				throw new ArgumentException( "clipboard is empty", "cb" );
			}

			resetUnits();

			// Write out the clipboard information
			//
			writeHeader(ref animFile);

			// Now write out each animCurve
			//
			MAnimCurveClipboardItemArray clipboardArray = cb.clipboardItems;
			for (int i = 0; i < clipboardArray.length; i++)
			{
				MAnimCurveClipboardItem clipboardItem = clipboardArray[i];

				MObject animCurveObj = clipboardItem.animCurve;

				//	The clipboard may contain Null anim curves. If a Null anim 
				//	curve is returned, it is safe to ignore the error message
				//	and continue to the next anim curve in the list.
				//
				bool placeHolder = false;
				if (animCurveObj.isNull)
				{
					placeHolder = true;
				}

				// Write out animCurve information
				//
				writeAnim(ref animFile, clipboardItem, placeHolder, nodeNames);

				if (placeHolder)
				{
					continue;
				}

				//	Write out each curve in its specified format.
				//	For now, only the anim curve format.
				//
				writeAnimCurve(ref animFile, animCurveObj, clipboardItem.animCurveType, verboseUnits);
			}
		}

		protected void writeHeader(ref StreamWriter clip)
		//
		//	Description:
		//		Writes the header for the file. The header contains the clipboard
		//		specific data. 
		//
		{
			if (clip == null)
			{
				throw new ArgumentNullException( "clip" );
			}

			clip.Write(kAnimVersion + kSpaceChar + kAnimVersionString + kSemiColonChar +  Environment.NewLine);
			clip.Write(kMayaVersion + kSpaceChar + MGlobal.mayaVersion + kSemiColonChar + Environment.NewLine);

			MAnimCurveClipboard clipboard = MAnimCurveClipboard.theAPIClipboard;

			double startTime = clipboard.startTime.asUnits(timeUnit);
			double endTime = clipboard.endTime.asUnits(timeUnit);

			if (startTime != endTime) 
			{
				string unit = "";
				animUnitNames.setToShortName(timeUnit, ref unit);
				clip.Write(kTimeUnit + kSpaceChar + unit + kSemiColonChar + Environment.NewLine);
				animUnitNames.setToShortName(linearUnit, ref unit);
				clip.Write(kLinearUnit + kSpaceChar + unit + kSemiColonChar + Environment.NewLine);
				animUnitNames.setToShortName(angularUnit, ref unit);
				clip.Write(kAngularUnit + kSpaceChar + unit + kSemiColonChar + Environment.NewLine);
				clip.Write( kStartTime + kSpaceChar + Convert.ToString(startTime) + kSemiColonChar + Environment.NewLine);
				clip.Write( kEndTime + kSpaceChar + Convert.ToString(endTime) + kSemiColonChar + Environment.NewLine);
			}

			float startUnitless = clipboard.startUnitlessInput;
			float endUnitless = clipboard.endUnitlessInput;

			if (startUnitless != endUnitless)
			{
				clip.Write(kStartUnitless + kSpaceChar + Convert.ToString(startUnitless) + 
							kSemiColonChar + Environment.NewLine);
				clip.Write(kEndUnitless + kSpaceChar + Convert.ToString(endUnitless) + 
							kSemiColonChar + Environment.NewLine);
			}
		}

		protected void writeAnim( ref StreamWriter clip, 
							 MAnimCurveClipboardItem clipboardItem,
							bool placeHolder = false,
							bool nodeNames = false)
		//	Description:
		//		Write out the anim curve from the clipboard item into the 
		//		stream. The position of the anim curve in the clipboard
		//		and the attribute to which it is attached is written out in this
		//		method.
		//
		//		This method returns true if the write was successful.
		//
		{
			if (clip == null)
			{
				throw new ArgumentNullException( "clip" );
			}

			clip.Write(kAnim);

			//	If this is a clipboard place holder then there will be no full
			//	or leaf attribute names.
			//
			if (placeHolder)
			{
				clip.Write(kSpaceChar + clipboardItem.nodeName );
			} 
			else 
			{
				clip.Write(kSpaceChar + clipboardItem.fullAttributeName);

				if (nodeNames) 
				{
					clip.Write( kSpaceChar + clipboardItem.leafAttributeName);
					clip.Write( kSpaceChar + clipboardItem.nodeName );
				}
			}

			uint rowCount = 0, childCount = 0, attrCount = 0;
			clipboardItem.getAddressingInfo(ref rowCount, ref childCount, ref attrCount);

			clip.Write( kSpaceChar);
			clip.Write(rowCount);
			clip.Write(kSpaceChar);
			clip.Write(childCount);
			clip.Write(kSpaceChar);
			clip.Write(attrCount);
			clip.Write(kSemiColonChar);
			clip.Write(Environment.NewLine);
		}
		protected void writeAnimCurve(ref StreamWriter clip, 
								 MObject animCurveObj,
								MFnAnimCurve.AnimCurveType type,
								bool verboseUnits = false)
		//	Description:
		//		Write out the anim curve from the clipboard item into the
		//		stream. The actual anim curve data is written out.
		//
		//		This method returns true if the write was successful.
		//
		{
			if (clip == null)
			{
				throw new ArgumentNullException( "clip" );
			}
			
			if (null == animCurveObj || animCurveObj.isNull)
			{
				throw new ArgumentNullException( "animCurveObj" );
			}

			MFnAnimCurve animCurve = new MFnAnimCurve(animCurveObj);

			clip.Write(kAnimData + kSpaceChar + kBraceLeftChar + Environment.NewLine);

			clip.Write(kTwoSpace + kInputString + kSpaceChar +
					boolInputTypeAsWord(animCurve.isUnitlessInput) +
					kSemiColonChar + Environment.NewLine);

			clip.Write( kTwoSpace + kOutputString + kSpaceChar +
					outputTypeAsWord(type) + kSemiColonChar + Environment.NewLine);

			clip.Write(kTwoSpace + kWeightedString + kSpaceChar +
					Convert.ToString(animCurve.isWeighted ? 1 : 0) + kSemiColonChar +Environment.NewLine);
			
			//	These units default to the units in the header of the file.
			//	
			if (verboseUnits) 
			{
				clip.Write(kTwoSpace + kInputUnitString + kSpaceChar);
				if (animCurve.isTimeInput)
				{
					string unitName = "";
					animUnitNames.setToShortName(timeUnit, ref unitName);
					clip.Write(unitName);
				} 
				else 
				{
					//	The anim curve has unitless input.
					//
					clip.Write(animUnitNames.kUnitlessString);
				}

				clip.Write(kSemiColonChar + Environment.NewLine);

				clip.Write(kTwoSpace + kOutputUnitString + kSpaceChar);
			}


			double conversion = 1.0;
			switch (type) 
			{
				case MFnAnimCurve.AnimCurveType.kAnimCurveTA:
				case MFnAnimCurve.AnimCurveType.kAnimCurveUA:
					{
						string unitName = "";
						animUnitNames.setToShortName(angularUnit, ref unitName);
						if (verboseUnits) clip.Write(unitName);
						{
							MAngle angle = new MAngle(1.0);
							conversion = angle.asUnits(angularUnit);
						}
						break;
					}
				case MFnAnimCurve.AnimCurveType.kAnimCurveTL:
				case MFnAnimCurve.AnimCurveType.kAnimCurveUL:
					{
						string unitName = "";
						animUnitNames.setToShortName(linearUnit, ref unitName);
						if (verboseUnits) clip.Write(unitName);
						{
							MDistance distance = new MDistance(1.0);
							conversion = distance.asUnits(linearUnit);
						}
						break;
					}
				case MFnAnimCurve.AnimCurveType.kAnimCurveTT:
				case MFnAnimCurve.AnimCurveType.kAnimCurveUT:
					{
						string unitName = "";
						animUnitNames.setToShortName(timeUnit, ref unitName);
						if (verboseUnits) clip.Write(unitName);
						break;
					}
				default:
					if (verboseUnits) clip.Write(animUnitNames.kUnitlessString);
					break;
			}

			if (verboseUnits) clip.Write(kSemiColonChar +Environment.NewLine);

			if (verboseUnits) 
			{
				string angleUnitName = "";
				animUnitNames.setToShortName(angularUnit, ref angleUnitName);
				clip.Write(kTwoSpace + kTanAngleUnitString + 
						kSpaceChar + angleUnitName + kSemiColonChar + Environment.NewLine);
			}

			clip.Write(kTwoSpace + kPreInfinityString + kSpaceChar +
					infinityTypeAsWord(animCurve.preInfinityType) + 
					kSemiColonChar + Environment.NewLine);

			clip.Write(kTwoSpace + kPostInfinityString + kSpaceChar +
					infinityTypeAsWord(animCurve.postInfinityType) + 
					kSemiColonChar + Environment.NewLine);

			clip.Write(kTwoSpace + kKeysString + kSpaceChar + kBraceLeftChar + Environment.NewLine);

			// And then write out each keyframe
			//
			uint numKeys = animCurve.numKeyframes;
			for (uint i = 0; i < numKeys; i++) 
			{
				clip.Write(kTwoSpace + kTwoSpace);
				if (animCurve.isUnitlessInput) 
				{
					clip.Write(animCurve.unitlessInput(i));
				}
				else 
				{
					clip.Write(animCurve.time(i).value);
				}

				// clamp tiny values so that it isn't so small it can't be read in
				//
				double animValue = (conversion*animCurve.value(i));
				if (isEquivalent(animValue,0.0)) 
					animValue = 0.0;
				clip.Write(kSpaceChar + Convert.ToString(animValue));

				clip.Write(kSpaceChar + tangentTypeAsWord(animCurve.inTangentType(i)) );
				clip.Write(kSpaceChar + tangentTypeAsWord(animCurve.outTangentType(i)) );

				clip.Write( kSpaceChar + Convert.ToString(animCurve.tangentsLocked(i) ? 1 : 0));
				clip.Write(kSpaceChar + Convert.ToString(animCurve.weightsLocked(i) ? 1 : 0));
				clip.Write(kSpaceChar + Convert.ToString(animCurve.isBreakdown(i) ? 1 : 0));

				if (animCurve.inTangentType(i) == MFnAnimCurve.TangentType.kTangentFixed) 
				{
					MAngle angle = new MAngle();
					double weight = 0.0;
					animCurve.getTangent(i, angle, ref weight, true);

					clip.Write(kSpaceChar + Convert.ToString(angle.asUnits(angularUnit)));
					clip.Write(kSpaceChar + Convert.ToString(weight));
				}
				if (animCurve.outTangentType(i) == MFnAnimCurve.TangentType.kTangentFixed) 
				{
					MAngle angle = new MAngle();
					double weight = 0.0;
					animCurve.getTangent(i, angle, ref weight, false);

					clip.Write(kSpaceChar +  Convert.ToString(angle.asUnits(angularUnit)));
					clip.Write(kSpaceChar + Convert.ToString(weight));
				}

				clip.Write(kSemiColonChar + Environment.NewLine);
			}
			clip.Write(kTwoSpace + kBraceRightChar + Environment.NewLine);

			clip.Write(kBraceRightChar + Environment.NewLine);
		}
	}
}
