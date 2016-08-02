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

[assembly: ExtensionPlugin(typeof(MayaNetPlugin.ExamplesPlugin), "Any")]

namespace MayaNetPlugin
{
    class ExamplesPlugin : IExtensionPlugin
    {
        public bool InitializePlugin()
        {
            return pluginCallbacks.InitializePlugin();
        }

        public bool UninitializePlugin()
        {
            return pluginCallbacks.UninitializePlugin();
        }

        public string GetMayaDotNetSdkBuildVersion()
        {
            String version = "201353";
            return version;
        }
		
        // if the ref path doesn't specify a file, then return null
        // dll path is {exampleDir}/assemblies/example.nll.dll
        public static string convertRefPathToFullPath(string dllref)
		{
            string dllPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(dllPath), dllref);
		}
    }
}
