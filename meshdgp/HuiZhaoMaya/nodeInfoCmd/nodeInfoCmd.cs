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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Xml;

 
using Autodesk.Maya.OpenMaya;

// Before loading the plugin, please execute $(MAYADIR)\devkit\plug-ins\nodeInfoCmd\nodeInfoCmdInitStrings.mel;
// otherwise Maya would prompt that it couldn't find the "nodeInfoCmdInitStrings" procedure.
[assembly: MPxUIStringClass(typeof(MayaNetPlugin.NodeCmdUIStringRegisterClass), "nodeInfoCmdInitStrings", "registerUIString")]
[assembly: MPxCommandClass(typeof(MayaNetPlugin.NodeInfoCmd), "nodeinfocmdCSharp")]

namespace MayaNetPlugin
{
    public class NodeCmdUIStringRegisterClass
    {
        static public void registerUIString()
        {
            MStringResource.registerString(NodeInfoCmd.rConnFound);
            MStringResource.registerString(NodeInfoCmd.rPlugInfo);
            MStringResource.registerString(NodeInfoCmd.rPlugDestOf);
        }
    }

   [MPxCommandSyntaxFlag("-q", "-quiet")]

    public class NodeInfoCmd: MPxCommand, IMPxCommand
    {
        public static MStringResourceId rConnFound = new MStringResourceId("nodeInfoCmd", "rConnFound", "Number of connections found: {0}");
        public static MStringResourceId rPlugInfo = new MStringResourceId("nodeInfoCmd", "rPlugInfo", " Plug Info");
        public static MStringResourceId rPlugDestOf = new MStringResourceId("nodeInfoCmd", "rPlugDestOf", " This plug is a dest of:");

        const string kQuietFlag = "-q";
        const string kQuietFlagLong = "-quiet";

        override public void doIt(MArgList args)
        {
            MArgDatabase argData;

            MPxCommandSyntaxFlagAttribute MyAttribute =
                (MPxCommandSyntaxFlagAttribute)Attribute.GetCustomAttribute(typeof(NodeInfoCmd), typeof(MPxCommandSyntaxFlagAttribute));

            MSyntax syntax = new MSyntax();
            if (MyAttribute != null)
            {
                syntax.addFlag(MyAttribute.ShortFlag, MyAttribute.LongFlag);
            }
            else
            {
                syntax.addFlag(kQuietFlag, kQuietFlagLong);
            }


            try
            {
                argData = new MArgDatabase(syntax, args);
            }
            catch (System.Exception ex)
            {
                MGlobal.displayInfo(ex.Message);
            }
 
            MSelectionList selectList = MGlobal.activeSelectionList;

            foreach (MObject node in selectList.DependNodes())
            {
                MFnDependencyNode depFn = new MFnDependencyNode();
			    depFn.setObject(node);

                string nodename = depFn.name;
                nodename +=":";
                printType(node, nodename);

                MPlugArray connectedPlugs = new MPlugArray();
                try
                {
                    depFn.getConnections(connectedPlugs);
                }
                catch (System.Exception ex)
                {
                    MGlobal.displayInfo(ex.Message);
                }
                
                uint numberOfPlugs = connectedPlugs.length;

                string msgFmt = MStringResource.getString(NodeInfoCmd.rConnFound);
                MGlobal.displayInfo( String.Format(msgFmt, Convert.ToString(numberOfPlugs)) );
		        
                string pInfoMsg = MStringResource.getString(NodeInfoCmd.rPlugInfo);
                for (int i = 0; i < numberOfPlugs; i++ )
                {
                    MPlug plug = connectedPlugs[i];
                    string pInfo = plug.info;
                    
                    MGlobal.displayInfo(pInfoMsg+pInfo);

                    MPlugArray array = new MPlugArray();
                    plug.connectedTo(array, true, false);
                    string dInfoMsg = MStringResource.getString(rPlugDestOf);
                    for (int j = 0; j < array.length; j++ )
                    {
                        MObject mnode = array[j].node;
                        printType(mnode, dInfoMsg);
                    }
                }
			    
			}
            return;
        }

        void printType( MObject node, string prefix)
        {
            MGlobal.displayInfo(prefix+node.apiTypeStr);
        }
    }
}
