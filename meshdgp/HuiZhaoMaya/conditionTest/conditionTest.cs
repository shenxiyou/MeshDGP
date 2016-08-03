// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


// Example Plugin: conditionTest.cs
//
// This plug-in is an example of user-defined callbacks for state transition.
// During state transition specific user callbacks can be invoked.
// 
// Steps:
//
// 1. Input the following MEL command to add condition callback:
//      netConditionTestCSharp -m 1
//
// 2. MEL history will show state transition of all conditions.
//
// 3. Input the following MEL command to remove condition callback:
//      netConditionTestCSharp -m 0
//

using System;

using Autodesk.Maya.OpenMaya;

[assembly: MPxCommandClass(typeof(MayaNetPlugin.conditionTest), "netConditionTestCSharp")]

namespace MayaNetPlugin
{
    [MPxCommandSyntaxFlag("m", "message", Arg1 = typeof(Boolean))]
	class conditionTest : MPxCommand, IMPxCommand
	{
		public conditionTest()
		{
			// Initialize the static members at the first time the command is used.
			if (conditionNames == null)
			{
				conditionNames = new MStringArray();
				MConditionMessage.getConditionNames(conditionNames);

				uint conditionCount = conditionNames.length;
				MGlobal.displayInfo("netConditionTest: " + conditionCount + " conditions are defined.");

				conditionStates = new bool[conditionCount];
				conditionCallbacks = new bool[conditionCount];
				for (uint i = 0; i < conditionCount; i++)
				{
					conditionStates[i] = false;
					conditionCallbacks[i] = false;
				}
			}

			addMessage = false;
			delMessage = false;
			conditions.clear();
		}

		public override void doIt(MArgList args)
		{
			parseArgs(args);

			// Allocate an array of indices.  conditions[n] is a user provided
			// condition name.  Look it up in the static conditionNames array
			// and set indices[n] to the index of the entry in conditionNames.
			//
			// This maps the user specified conditions to the global conditions
			// so we can track callback adds and removes globally.
			//
			int[] indices = new int[conditions.length];

			for (int i = 0; i < conditions.length; ++i)
			{
				// Initialize the entry to "not found".
				//
				indices[i] = -1;

				// Search condition names for a match.
				//
				for (int j = 0; j < conditionNames.length; ++j)
				{
					if (conditions[i] == conditionNames[j])
					{
						// Found a match.  Store the index and stop looking for
						//
						indices[i] = (int)j;
						break;
					}
				}
			}

			for (int i = 0; i < conditions.length; ++i)
			{
				int j = indices[i];
				if (j == -1)
				{
					displayWarning(conditions[i] + "is not a valid condition name");
					break;
				}

				if (addMessage)
				{
					try
					{
						MConditionMessage.Condition[conditions[i]] += conditionChangedCB;
						conditionCallbacks[j] = true;
					}
					catch (Exception)
					{
						displayError("failed to add callback for " + conditions[i]);
						conditionCallbacks[j] = false;
					}
				}
				else if (delMessage)
				{
					try
					{
						MConditionMessage.Condition[conditions[i]] -= conditionChangedCB;
						conditionCallbacks[j] = false;
					}
					catch (Exception)
					{
						displayError("failed to remove callback for " + conditions[i]);
						conditionCallbacks[j] = true;
					}
				}
			}

			// Ok, we've made all the necessary changes.  Now show the status.
			//
			displayInfo("Condition Name        State  Msgs On");
			displayInfo("--------------------  -----  -------");

			for (int i = 0; i < conditions.length; ++i)
			{
				int j = indices[i];
				if (j == -1)
				{
					continue;
				}

				try
				{
					conditionStates[j] = MConditionMessage.getConditionState(conditions[i]);
				}
				catch (Exception)
				{
					displayError("failed to get status for " + conditions[i]);
					conditionStates[j] = false;
				}

				string tmpStr = string.Format("{0,-20}  {1,-5}  {2, -5}",
					conditions[i],
					conditionStates[j],
					conditionCallbacks[j] ? "yes" : "no");

				displayInfo(tmpStr);
			}

			return;
		}

		private void parseArgs(MArgList args)
		{
			const string kMessageFlag = "m";

			MArgDatabase argData = new MArgDatabase(syntax, args);
			
			if (argData.isFlagSet(kMessageFlag))
			{
				bool flag = false;

				try
				{
					flag = argData.flagArgumentBool(kMessageFlag, 0);
				}
				catch (Exception)
				{
					throw new ArgumentException("could not parse message flag", "args");
				}

				if (flag)
				{
					addMessage = true;
				}
				else
				{
					delMessage = true;
				}
			}
			
			try
			{
				argData.getObjects(conditions);
			}
			catch(Exception)
			{
				displayError("could not parse condition names");
			}
			
			// If there are no conditions specified, operate on all of them
			//
			if (conditions.length == 0)
			{
				// conditionNames is set in initializePlugin to all the
				// currently available condition names.
				//
				conditions = conditionNames;
			}
		}

		public static void conditionChangedCB(object sender, MStateFunctionArgs args)
		{
			// Client data isn't supported in Maya.NET, therefore we have to maintain an original
			// copy of the condition states and manually compare the states here in order to find
			// which state is being changed.
			for (int i = 0; i < conditionNames.length; ++i)
			{
				if (conditionCallbacks[i])
				{
					bool newState = MConditionMessage.getConditionState(conditionNames[i]);
					if (newState != conditionStates[i])
					{
						displayInfo("condition " + conditionNames[i] + " changed to " + (newState ? "true" : "false"));
						conditionStates[i] = newState;
					}
				}
			}
		}

		private bool addMessage = false;
		private bool delMessage = false;
		private MStringArray conditions = new MStringArray();

		// Static members to store data that should be retained between commands.
		private static MStringArray conditionNames = null;
		private static bool[] conditionStates = null;
		private static bool[] conditionCallbacks = null;
	}
}
