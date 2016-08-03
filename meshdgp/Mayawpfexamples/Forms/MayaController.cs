using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Diagnostics;

using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;
using System.Data;

using Autodesk.Maya;
using Autodesk.Maya.OpenMaya;

namespace GraphicResearchHuiZhao 
{
    public class MayaController
    {
        private static MayaController singleton = new MayaController();


        public static MayaController Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new MayaController();
                return singleton;
            }
        }




        public void RunBasic(EnumMayaBasic type)
        {
            string para = ConfigMaya.Instance.SelectAll;
            switch (type)
            {

                case EnumMayaBasic.Info:
                    MGlobal.displayInfo("Hello World\n");
                    break;

                case EnumMayaBasic.FirstSelect:
                    MayaBridge.Instance.ShowMeshInfo();
                    break;

                case EnumMayaBasic.ToTrimesh:
                    MayaBridge.Instance.ConvertToTriMesh();
                    break;

                
                case EnumMayaBasic.SelectVertice:
                    MayaBridge.Instance.GetVerticeSelected();
                    break;
                case EnumMayaBasic.HookUp:
                    MayaBridge.Instance.HookUp();
                    break; 

                case EnumMayaBasic.Create:
                    MayaBridge.Instance.HookUpCreate();
                    break;
                case EnumMayaBasic.Delete:
                    TriMeshIO.Copy(GlobalData.Instance.TriMesh);
                    break;

            }


           
        }



        public void RunDeform(EnumMayaDeform type)
        { 
            switch (type)
            {

                case EnumMayaDeform.Init:
                    MayaBridge.Instance.DeformInit();
                    break;

                case EnumMayaDeform.Deform:
                    MayaBridge.Instance.DeformIterative();
                    break;
 
            }
             

        }


        //public void Run(EnumMayaCommand type)
        //{
        //    string para = ConfigMaya.Instance.SelectAll;
        //    switch(type)
        //    {
               
        //        case EnumMayaCommand.All:
        //            para = ConfigMaya.Instance.SelectAll;
        //            break;

        //        case EnumMayaCommand.Mesh:
        //            para = ConfigMaya.Instance.SelectMesh;
        //            break;

        //        case EnumMayaCommand.Polygon:
        //            para = ConfigMaya.Instance.SelectPolygon;
        //            break;

        //        case EnumMayaCommand.Name:
        //            para = ConfigMaya.Instance.SelectName;
        //            break;

        //    }


        //    Object objects = MayaData.Instance.GetObject(ConfigMaya.Instance.SelectAll);
        //    MayaData.Instance.DisplayObjecty(objects);
        //}

    }
}
