using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class FormConfigShape : Form
    {
        public FormConfigShape()
        {
            InitializeComponent();
            this.propertyGridShape.SelectedObject = ConfigShape.Instance;
        }


        private static FormConfigShape singleton = new FormConfigShape();

        public static FormConfigShape Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FormConfigShape();
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
