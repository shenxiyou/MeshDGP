// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


////////////////////////////////////////////////////////////////////////
//
//  Description:
//  file : userMsgCmd.cs
//    class: userMessage
//    ----------------------
//    This is an example to demonstrate the usages of the MUserEventMessage class.
//    It allows the user to create, destroy, and post to user-defined events
//    identified by strings.

//    The command "userMessageCSharp" supports the following options:

//        -r/-register string : Register a new event type with the given name.
//            Registration also attaches two callback functions to the event,
//            userCallback1 and userCallback2.

//        -d/-deregister string : Deregister an existing event with the given name

//        -p/-post string : Post the event.  In this case, it simply notifies
//            userCallback1 and userCallback2, which print info messages.

//        -t/-test : Run a basic set of tests that demonstrate how the user event
//            types can be used.  See userMessage::runTests()

//    Only one option should be specified per invocation. For more detail, please
//    refer to the accompanying MEL script.
//
////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Linq;
using System.Text;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaUI;
using Autodesk.Maya.OpenMayaRender;

using System.Runtime.InteropServices;

[assembly: MPxCommandClass(typeof(MayaNetPlugin.userMessage), "userMessageCSharp")]

namespace MayaNetPlugin
{
    class userCB
    {
        public object clientData
        {
            get; set;
        }

        public void userCallback1(object sender, MBasicFunctionArgs args)
        {
            MGlobal.displayInfo("Entered userMessage.userCallback1");
            if(clientData != null)
            {
                string receivedDataMsg = clientData as string;
                if (receivedDataMsg != null)
                    MGlobal.displayInfo("Received data: " + receivedDataMsg);
            }
        }

        public void userCallback2(object sender, MBasicFunctionArgs args)
        {
            MGlobal.displayInfo("Entered userMessage.userCallback2");
            if (clientData != null)
            {
                string receivedDataMsg = clientData as string;
                if (receivedDataMsg != null)
                    MGlobal.displayInfo("Received data: " + receivedDataMsg);
            }
        }
    }

    [MPxCommandSyntaxFlag("-p", "-post", Arg1 = typeof(string))]
    [MPxCommandSyntaxFlag("-r", "-register", Arg1 = typeof(string))]
    [MPxCommandSyntaxFlag("-d", "-deregister", Arg1 = typeof(string))]
    [MPxCommandSyntaxFlag("-t", "-test")]
	class userMessage : MPxCommand, IMPxCommand
	{
		static readonly string postFlag = "-p";
		static readonly string registerFlag = "-r";
		static readonly string deregisterFlag = "-d";
		static readonly string testFlag = "-t";

		public override void doIt(MArgList args)
		{
			MArgDatabase argData = new MArgDatabase(syntax, args);

			if (argData.isFlagSet(deregisterFlag))
			{
				string eventName = argData.flagArgumentString(deregisterFlag, 0);
				MUserEventMessage.deregisterUserEvent(eventName);
			}
			else if (argData.isFlagSet(registerFlag))
			{
				// Register the new event and add two fixed callbacks to it.
				string eventName = argData.flagArgumentString(registerFlag, 0);
				if (!MUserEventMessage.isUserEvent(eventName))
				{
					MUserEventMessage.registerUserEvent(eventName);
                    
                    userCB cb1 = new userCB();
                    cb1.clientData = "Sample Client Data (an MString object)";
                    MUserEventMessage.UserEvent[eventName] += cb1.userCallback1;
                    MUserEventMessage.UserEvent[eventName] += cb1.userCallback2;
				}
			}
			else if (argData.isFlagSet(postFlag))
			{
				string eventName = argData.flagArgumentString(postFlag, 0);
				MUserEventMessage.postUserEvent(eventName);
			}
			else if (argData.isFlagSet(testFlag))
			{
				runTests();
			}

			return;
		}

		public void runTests()
		{

			// Test 1: Try to register callback for nonexistent event

			MGlobal.displayInfo("Starting Test 1");

            userCB cb = new userCB();
			try
            {
                MUserEventMessage.UserEvent["TestEvent"] += cb.userCallback1;
				MGlobal.displayInfo("Test 1 failed");
			}
			catch (System.Exception)
			{
				MGlobal.displayInfo("Test 1 passed");
			}

			// Test 2: Register and deregister an event 
			// - Expected output: Entered userMessage.userCallback1

			MGlobal.displayInfo("Starting Test 2");

            MUserEventMessage.registerUserEvent("TestEvent");

            MUserEventMessage.UserEvent["TestEvent"] += cb.userCallback1;
			MUserEventMessage.postUserEvent("TestEvent");
			MUserEventMessage.deregisterUserEvent("TestEvent");

			// Test 3: The event should be gone
			MGlobal.displayInfo("Starting Test 3");
			try
			{
                MUserEventMessage.UserEvent["TestEvent"] += cb.userCallback1;
				MGlobal.displayInfo("Test 3 failed");
			}
			catch (System.Exception)
			{

				MGlobal.displayInfo("Test 3 passed");
			}

			// Test 4: Try adding multiple callbacks to an event
			// Expected output: Entered userMessage.userCallback1
			//					Entered userMessage.userCallback2

			MGlobal.displayInfo("Starting Test 4");

			MUserEventMessage.registerUserEvent("TestEvent");
			MUserEventMessage.UserEvent["TestEvent"] += cb.userCallback1;
            MUserEventMessage.UserEvent["TestEvent"] += cb.userCallback2;
			MUserEventMessage.postUserEvent("TestEvent");
			MUserEventMessage.deregisterUserEvent("TestEvent");

			// Test 5: Try adding and posting to multiple events
			// Expected output: Posting first event
			//					Entered userMessage.userCallback1
			//					Entered userMessage.userCallback2
			//					Posting second event
			//					Entered userMessage.userCallback1
			//					Entered userMessage.userCallback2

			MGlobal.displayInfo("Starting Test 5");

			MUserEventMessage.registerUserEvent("TestEvent");
			MUserEventMessage.registerUserEvent("TestEvent2");
            MUserEventMessage.UserEvent["TestEvent"] += cb.userCallback1;
            MUserEventMessage.UserEvent["TestEvent"] += cb.userCallback2;
            MUserEventMessage.UserEvent["TestEvent2"] += cb.userCallback1;
            MUserEventMessage.UserEvent["TestEvent2"] += cb.userCallback2;
			MGlobal.displayInfo("Posting first event");
			MUserEventMessage.postUserEvent("TestEvent");
			MGlobal.displayInfo("Posting second event");
			MUserEventMessage.postUserEvent("TestEvent2");
			MUserEventMessage.deregisterUserEvent("TestEvent");
			MUserEventMessage.deregisterUserEvent("TestEvent2");

			MGlobal.displayInfo("Completed all tests");
		}
	}
}
