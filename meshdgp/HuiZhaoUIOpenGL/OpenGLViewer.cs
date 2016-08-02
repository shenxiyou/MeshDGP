using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class OpenGLViewer : UserControl
    {
        public OpenGLViewer()
        {
            InitializeComponent();
            if (this.DesignMode)
                return  ;
            GlobalData.Instance.Changed += new MeshChangedDelegate(Instance_Changed);
        }

        public void Instance_Changed(object sender, EventArgs e)
        {
            MeshView3D.Refresh();
        }
    }
}
