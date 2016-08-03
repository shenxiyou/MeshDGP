using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao 
{
    public partial class FormAbout : Form
    {

        private static FormAbout singleton = new FormAbout();

        public static FormAbout Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FormAbout();
                return singleton;
            }
        }
        private FormAbout()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
