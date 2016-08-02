using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.Maya.Runtime;
using Autodesk.Maya.OpenMaya;
using Autodesk.Maya.OpenMayaUI;
using Autodesk.Maya.OpenMayaMPx;


namespace MayaNetPlugin
{
    class exampleCameraSetView : MPx3dModelView
    {
        public exampleCameraSetView()
        {
            setMultipleDrawEnable(true);
        }

        public override string viewType()
        {
            return "exampleCameraSetViewCSharp";
        }
    }
}
