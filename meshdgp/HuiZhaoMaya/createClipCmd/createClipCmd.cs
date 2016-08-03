// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


// Note:
//		This C# plugin is ported from: $(MAYADIR)\devkit\plug-ins\createClipCmd

// Description:
//   This is an example of a command that creates a clip for a character.
//   Using "createClipCSharp -c characterName" command to create a clip for a character.
//   For more detail of animation clip, please see "Animate > Create Clip " in the User Guide help.
//

using System;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaAnim;

[assembly: MPxCommandClass(typeof(MayaNetPlugin.createClip), "createClipCSharp")]

namespace MayaNetPlugin
{
	public class createClip : MPxCommand, IMPxCommand, IUndoMPxCommand
	{
		MDGModifier fMod = new MDGModifier();
		MObject fCharacter = new MObject();

		public override bool isUndoable()
		{
			return true;
		}

		public override void undoIt()
		{
			fMod.undoIt();
			return;
		}

		void parseArgs(MArgList args)
		{
			string arg = "";
			MSelectionList list = new MSelectionList();
			bool charNameUsed = false;
			string charName;
			const string charFlag = "-c";
			const string charFlagLong = "-char";

			for (uint i = 0; i < args.length; i++)
			{
				arg = args.asString(i);
				if (arg == charFlag || arg == charFlagLong)
				{
					// get the char name
					//
					if (i == args.length - 1)
					{
						arg += ": must specify a character name";
						throw new ArgumentException(arg, "args"); 
					}
					i++;
					charName = args.asString(i);
					list.add(charName);

					charNameUsed = true;
				}
				else
				{
					arg += ": unknown argument";
					throw new ArgumentException(arg, "args"); 
				}
			}

			if (charNameUsed)
			{
				// get the character corresponding to the node name
				//
				MItSelectionList iter = new MItSelectionList(list);
				for (; iter.isDone; iter.next())
				{
					MObject node = new MObject();
					iter.getDependNode(node);
					if (node.apiType == MFn.Type.kCharacter)
					{
						fCharacter = node;
						break;
					}
				}

				if (fCharacter.isNull)
				{
					throw new ApplicationException("Unable to get the character corresponding to the node name.");
				}
			}
		}

		public override void doIt(MArgList args)
		{
			// parse the command arguments
			//
			parseArgs(args);

			uint count = 0;
			// if the character flag was used, create the clip on the specified
			// character, otherwise, create a character
			//
			MFnCharacter fnCharacter = new MFnCharacter();
			if (fCharacter.isNull)
			{
				MSelectionList activeList = new MSelectionList();
				MGlobal.getActiveSelectionList(activeList);
				if (0 == activeList.length)
				{
					throw new ApplicationException("Empty Active Selection List.");
				}

				// create a character using the selection list
				//
				fCharacter = fnCharacter.create(activeList, MFnSet.Restriction.kNone);
			}
			else
				fnCharacter.setObject(fCharacter);

			// Get the array of members of the character. We will create a clip
			// for them.
			//
			MPlugArray plugs = new MPlugArray();
			fnCharacter.getMemberPlugs(plugs);

			// Now create a animCurves to use as a clip for the character.
			// The curves will be set up between frames 0 and 10;
			//
			MTime start = new MTime(0.0);
			MTime duration = new MTime(10.0);
			MObjectArray clipCurves = new MObjectArray();

			for (count = 0; count < plugs.length; ++count)
			{
				// Now create a bunch of animCurves to use as a clip for the
				// character
				//
				MFnAnimCurve fnCurve = new MFnAnimCurve();
				MObject curve = fnCurve.create(MFnAnimCurve.AnimCurveType.kAnimCurveTL); // plugType);
				fnCurve.addKeyframe(start, 5.0);
				fnCurve.addKeyframe(duration, 15.0);
				clipCurves.append(curve);
			}

			// Create a source clip node and add the animation to it
			//
			MFnClip fnClipCreate = new MFnClip();
			MObject sourceClip = fnClipCreate.createSourceClip(start, duration, fMod);
			fnCharacter.attachSourceToCharacter(sourceClip, fMod);
			for (count = 0; count < plugs.length; ++count)
			{
				MPlug animPlug = plugs[(int)count];
				fnCharacter.addCurveToClip(clipCurves[(int)count], sourceClip, animPlug, fMod);
			}


			// instance the clip
			//
			MTime schedStart = new MTime(15.0);
			MObject instancedClip = fnClipCreate.createInstancedClip(sourceClip, schedStart, fMod);
			fnCharacter.attachInstanceToCharacter(instancedClip, fMod);

			// instance the clip a second time, at time 30
			//
			schedStart.value = 30.0;
			MObject instancedClip2 = fnClipCreate.createInstancedClip(sourceClip, schedStart, fMod);
			fnCharacter.attachInstanceToCharacter(instancedClip2, fMod);

			return;
		}

		public override void redoIt()
		{
			fMod.doIt();
			return;
		}
	}
}
