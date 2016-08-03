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

[assembly: MPxCommandClass(typeof(MayaNetPlugin.eventTestCmd), "eventTestCSharp")]

namespace MayaNetPlugin
{
    class eventCB
    {
        public int data
        {
            get; set;
        }

        public eventCB(int i)
        {
            data = i;
        }

        public void eventCallback(object sender, MBasicFunctionArgs args)
        {
            if (data >= 0 && data < (int)eventTestCmd.eventNames.Length)
            {
                MGlobal.displayInfo("event " +
                                     eventTestCmd.eventNames[data] +
                                     " occurred\n");
            }
            else
            {
                MGlobal.displayWarning("BOGUS client data in eventCB!\n");
            }
        }

    }

    [MPxCommandSyntaxFlag("-m", "-message", Arg1 = typeof(System.Boolean))]
    [MPxCommandSyntaxSelection(ObjectType = typeof(System.String))]
	public class eventTestCmd : MPxCommand, IMPxCommand {

		const string kMessageFlag = "-m" ;
		const string kMessageFlagLong = "-message" ;
        public static string[] eventNames;
        public static int[] eventsFlag;
        private bool addMessage;
        private bool delMessage;
        private string[] events;
        private static eventCB[] cbArray;
       
		public eventTestCmd () {
            addMessage = delMessage = false;
		}

        public static bool initializePlugin()
        {
            string[] evts;
            if (eventNames != null)
                return true;

            try
            {
                evts = MEventMessage.eventNames;
            }
            catch (System.Exception)
            {
                return false;
            }

            // Search for and remove idle and idleHigh events since they will
            // completely swamp the output.  They are tested by the idleTest plug-in
            List<string> tmp = new List<string>();
            foreach (string eventName in evts)
            {
                if (eventName == "idle" || eventName == "idleHigh")
                    continue;

                tmp.Add(eventName);
            }
            eventNames = tmp.ToArray();

            try
            {
                eventsFlag = new int[0];
            }
            catch (System.Exception ex)
            {
                System.Console.Out.WriteLine(ex.Message);
            }

            eventsFlag = new int[tmp.Count];
            cbArray = new eventCB[tmp.Count];

            for (int i = 0; i < tmp.Count; ++i)
                eventsFlag[i] = -1;
            MGlobal.displayInfo("eventTest: " + tmp.Count + " events are defined");

            return true;
        }

        private void parseArgs(MArgList args)
        {
            MArgDatabase argData = new MArgDatabase(syntax, args);

            if(argData.isFlagSet(kMessageFlag))
            {
                bool flag;
                try
                {
                    flag = argData.flagArgumentBool(kMessageFlag, 0);
                }
                catch (System.Exception)
                {
					throw new ArgumentException("could not parse message flag", "args");
                }

                addMessage = flag;
                delMessage = !flag;
            }

            MStringArray evts;
            try
            {
                evts = argData.objects;

                if (evts.length == 0)
                    events = eventNames.ToArray();
                else
                    evts.get(out events);
            }
            catch (System.Exception)
            {
                displayError("could not parse condition names");
            }
        }

        public override void doIt(MArgList args)
        {
            parseArgs(args);

			// Allocate an array of indices.  events[n] is a user provided
			// event name.  Look it up in the static eventNames array
			// and set indices[n] to the index of the entry in eventNames.
			//
			// This maps the user specified events to the global events
			// so we can track callback adds and removes globally.
            int [] indics = new int[events.Length];
            for (int j = 0; j < events.Length; ++j)
            {
                string userEvt = events[j];
                indics[j] = -1;

		        // Search event names for a match.
		        //
                for (int i = 0; i < eventNames.Length; ++i)
                {
                    if(userEvt == eventNames[i])
                    {
                        indics[j] = i;
                        if(addMessage && eventsFlag[i] == -1)
                        {
                            try
                            {
                                cbArray[i] = new eventCB(i);
                                MEventMessage.Event[userEvt] += cbArray[i].eventCallback;
                                eventsFlag[i] = j;
                            }
                            catch (System.Exception)
                            {
                                MGlobal.displayError("failed to add callback for" + userEvt);
                            }
                        }
                        else if(delMessage && eventsFlag[i] != -1)
                        {
                            try
                            {
                                MEventMessage.Event[userEvt] -= cbArray[i].eventCallback;
                                cbArray[i] = null;
                                eventsFlag[i] = -1; 
                            }
                            catch (System.Exception)
                            {
                                MGlobal.displayError("failed to remove callback for" + userEvt);
                            }

                        }

                        // Found a match.  Store the index and stop looking for
                        break;
                    }
                }
            }
			
			// Ok, we've made all the necessary changes. Now show the status.
			MGlobal.displayInfo ("Event Name                           Msgs On") ;
			MGlobal.displayInfo ("-----------------------------------  -------") ;
			for ( int i = 0 ; i < events.Length ; i++ ) {
                bool invalid = indics[i] == -1;
                bool msgs = invalid ? false : (eventsFlag[indics[i]] != -1);
				//- "%-35s  %s\n"
                MGlobal.displayInfo(string.Format("{0, -35}  {1}", events[i], (invalid ? "invalid" : msgs ? "yes" : "no")));
			}
            return;
		}

	}

}
