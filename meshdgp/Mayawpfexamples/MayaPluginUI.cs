using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

using Autodesk.Maya.OpenMaya;
using Autodesk.Maya;

[assembly: ExtensionPlugin(typeof(GraphicResearchHuiZhao.DockWPFPlugin), "Any")]
[assembly: MPxCommandClass(typeof(GraphicResearchHuiZhao.DockWPF), "HuiZhaoMaya")]

// This example demonstrates the following:
// - Creation of a .NET plugin for Maya
// - Embedding of a WPF interface within Maya's main window
// - Interaction between Maya and .NET
//   - Taking data from .NET and filling .NET controls
//   - Taking input from .NET controls and altering Maya's state
namespace GraphicResearchHuiZhao
{

    // This command class will stay around
    public class DockWPF : MPxCommand, IMPxCommand
    {
        // Objects to keep around
        MDockingStation mI;
        // DAGExplorer wnd;
         FormMainMaya wnd;
        override public void doIt(MArgList args)
        {
            // Create the window to dock
            wnd = new FormMainMaya();
            wnd.Show();

            // Extract the window handle of the window we want to dock
           // IntPtr mWindowHandle = new System.Windows.Interop.WindowInteropHelper(wnd).Handle;

           IntPtr mWindowHandle = wnd.Handle;

            // Dock it in Maya using the docking station
            mI = new MDockingStation(mWindowHandle, true, MDockingStation.BottomDock | MDockingStation.TopDock, MDockingStation.BottomDock);
        }

        ~DockWPF()
        {
            mI = null;
            wnd = null;
        }
    }

    // This class is instantiated by Maya once and kept alive for the duration of the session.
    public class DockWPFPlugin : IExtensionPlugin
    {
        bool IExtensionPlugin.InitializePlugin()
        {
            return true;
        }

        bool IExtensionPlugin.UninitializePlugin()
        {
            return true;
        }

        string IExtensionPlugin.GetMayaDotNetSdkBuildVersion()
        {
            String version = "201353";
            return version;
        }
    }
}

