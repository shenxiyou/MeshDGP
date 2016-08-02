using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class FormConfigGu : Form
    {
        public FormConfigGu()
        {
            InitializeComponent();
            this.propertyGridGu.SelectedObject = ConfigGu.Instance;
        }


        private static FormConfigGu singleton = new FormConfigGu();

        public static FormConfigGu Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FormConfigGu();
                return singleton;
            }
        }

        private void FormAnimationConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;

            this.Hide();
        }


    }
}
