using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class FormRendering : Form
    {
        public FormRendering()
        {
            InitializeComponent();

            this.propertyGrid.SelectedObject = ConfigRenderSmoothDiffuse.Instance;
        }

        private static FormRendering singleton = new FormRendering();


        public static FormRendering Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FormRendering();
                return singleton;
            }
        }

        public PropertyGrid Grid
        {
            get
            {
                return this.propertyGrid;
            }
        }

        private void FormRendering_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

    }
}
