// ==================================================================
// Copyright 2012 Autodesk, Inc.  All rights reserved.
// 
// This computer source code  and related  instructions and comments are
// the unpublished confidential and proprietary information of Autodesk,
// Inc. and are  protected  under applicable  copyright and trade secret
// law. They may not  be disclosed to, copied or used by any third party
// without the prior written consent of Autodesk, Inc.
// ==================================================================


// Example Plugin: pluginCallbacks.cs
//
// This plug-in is an example of a user-defined callbacks for plugin loading/unloading.
// During load/unload specific user callbacks can be invoked to provide information about 
// the file path, and plugin names being manipulated.
//
// MSceneMessage::kBeforePluginLoad will provide the file name being loaded
// MSceneMessage::kAfterPluginLoad will provide the file name being loaded, and the plugin name
// MSceneMessage::kBeforePluginUnload will provide the plugin name 
// MSceneMessage::kAfterPluginUnload will provide the plugin name and the file name being unloaded
// 

using Autodesk.Maya.OpenMaya;

namespace MayaNetPlugin
{
	class pluginCallbacks
	{
		public static bool InitializePlugin()
		{
			MSceneMessage.BeforePluginLoad += prePluginLoadCallback;
			MSceneMessage.AfterPluginLoad += postPluginLoadCallback;
			MSceneMessage.BeforePluginUnload += prePluginUnloadCallback;
			MSceneMessage.AfterPluginUnload += postPluginUnloadCallback;

            eventTestCmd.initializePlugin();
            return true;
		}

		public static bool UninitializePlugin()
		{
            return true;
		}

		private static void prePluginLoadCallback(object sender, MStringArrayFunctionArgs args)
		{
			MStringArray str = args.strs;

			MGlobal.displayInfo("PRE plugin load callback with " + str.length + " items:");
			for (int i = 0; i < str.length; i++)
			{
				MGlobal.displayInfo("\tCallback item " + i + " is : " + str[i]);
			}
		}

		private static void postPluginLoadCallback(object sender, MStringArrayFunctionArgs args)
		{
			MStringArray str = args.strs;

			MGlobal.displayInfo("POST plugin load callback with " + str.length + " items:");
			for (int i = 0; i < str.length; i++)
			{
				MGlobal.displayInfo("\tCallback item " + i + " is : " + str[i]);
			}
		}

		private static void prePluginUnloadCallback(object sender, MStringArrayFunctionArgs args)
		{
			MStringArray str = args.strs;

			MGlobal.displayInfo("PRE plugin unload callback with " + str.length + " items:");
			for (int i = 0; i < str.length; i++)
			{
				MGlobal.displayInfo("\tCallback item " + i + " is : " + str[i]);
			}
		}

		private static void postPluginUnloadCallback(object sender, MStringArrayFunctionArgs args)
		{
			MStringArray str = args.strs;

			MGlobal.displayInfo("POST plugin unload callback with " + str.length + " items:");
			for (int i = 0; i < str.length; i++)
			{
				MGlobal.displayInfo("\tCallback item " + i + " is : " + str[i]);
			}
		}
	}
}
