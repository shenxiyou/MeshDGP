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
using Autodesk.Maya.MetaData;
using System.Collections.Generic;

namespace MayaNetPlugin
{
    public enum CommandMode
    {
        kCreate = 0x01,
        kEdit = 0x02,
        kQuery = 0x04
    };

    //======================================================================
    //
    // Helper class for packaging up command options into a simple flag object.
    //
    public class OptFlag
    {
        System.Collections.Generic.List<int> toto = new System.Collections.Generic.List<int>();
        private bool	fIsSet;
	    private bool	fIsArgValid;
	    private String  fArg;
        private List<CommandMode> mValidModes;
        public List<CommandMode> ValidModes
        {
            get
            {
                return mValidModes;
            }
            set
            {
                mValidModes = value;
            }
        }
	    public OptFlag()
        {
            fIsSet = false;
            mValidModes = new List<CommandMode>();
        }

	    public void parse(ref MArgDatabase argDb, String name)
	    {
		    fIsSet = argDb.isFlagSet(name);
            if (fIsSet == false)
                return;
            argDb.getFlagArgument(name, 0, out fArg);
		    fIsArgValid = fArg.Length > 0;
	    }

	    public bool isModeValid(List<CommandMode> currentMode)
	    {
            if (!fIsSet || (mValidModes.Count > 0 && currentMode.Count > 0))
            {
                foreach (CommandMode cmMode in currentMode)
                {
                    if (mValidModes.Contains(cmMode) == false)
                    {
                        return false;
                    }
                }
                //All current mode are supported
                return true;
            }
            return false;
	    }
	
	    public bool isSet()
        {
            return fIsSet;
        }
	    public bool isArgValid()
        {
            return fIsArgValid;
        }
	    public String arg()
        {
            return fArg;
        }

	    public MArgList arg(ref MArgList defValue)
	    {
		    if (isSet())
		    {
			    if(isArgValid() == true)
                {
                    MArgList argList = new MArgList();
                    argList.addArg(fArg);
                    return argList;
                }
			    return null;
		    }
		    else
		    {
			    return defValue;
		    }
	    }
	
    }
}
