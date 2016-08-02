using System;
using System.Collections.Generic; 
using System.Text; 
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
 

namespace GraphicResearchHuiZhao 
{

    public delegate void WolframChangedDelegate(object sender, EventArgs e);


    public partial class ControllerWolfRam
    {
        private static ControllerWolfRam singleton = new ControllerWolfRam();


        public static ControllerWolfRam Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new ControllerWolfRam();
                return singleton;
            }
        }


        public event  WolframChangedDelegate Changed;

        protected virtual void OnChanged(EventArgs e)
        {


            if (Changed != null)
                Changed(this, e);
        }

        public void Run(EnumWolfRam type)
        {
            switch(type)
            {
                
                case EnumWolfRam.SimpleLink:
                    SimpleLink();
                    break;
                case EnumWolfRam.MathKernel:
                    MathKernel();
                    break;
            }

        }

         
      

 
         public void SimpleLink()
        {
            SimpleLink simple = new SimpleLink();
            simple.Run(null);


        }


         public void MathKernel()
         {

             Form1 form = new Form1(null);
             form.Show();
         }

    }
}
