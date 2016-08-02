using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class MenuColor : UserControl
    {
        public MenuColor()
        {
            InitializeComponent();
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfigColor.Instance.BringToFront();
            FormConfigColor.Instance.Show();
        }
    }
}
