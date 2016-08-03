using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class BarStatus : UserControl
    {
        public BarStatus()
        {
            InitializeComponent();

            GlobalData.Instance.Changed += new MeshChangedDelegate(Instance_Changed);
        }

        public void Instance_Changed(object sender, EventArgs e)
        {
            try
            {
                this.status.Text = TriMeshUtil.BuildStatus(GlobalData.Instance.TriMesh);
                this.Refresh();
            }
            catch
            {
            }
        }
    }
}
