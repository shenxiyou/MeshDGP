using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class FormConfigAnimation : Form
    {
        public FormConfigAnimation()
        {
            InitializeComponent();
            this.propertyGridAni.SelectedObject = ConfigAniamtion.Instance;
        }


        private static FormConfigAnimation singleton = new FormConfigAnimation();

        public static FormConfigAnimation Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FormConfigAnimation();
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
