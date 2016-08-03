using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class FormConfigOP : Form
    {
        private static FormConfigOP singleton = new FormConfigOP();


        public static FormConfigOP Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FormConfigOP();
                return singleton;
            }
        }

        public FormConfigOP()
        {
            InitializeComponent();

            this.propertyGridBasic.SelectedObject = ConfigMeshOP.Instance;
        }

        private void FormConfigOP_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;

            e.Cancel = true;
            this.Hide();
        }
    }
}
