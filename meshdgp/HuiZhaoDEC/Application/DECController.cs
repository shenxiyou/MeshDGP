using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class DECController
    {
        private static DECController singleton = new DECController();
        public static DECController Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new DECController();
                return singleton;
            }
        }


        public void Run(TriMesh mesh, EnumDEC type)
        {

            SpinForm from = new SpinForm(mesh);
            from.UpdateDeformation("D:\\bumpy.tga", 5);
        }

    }
}
