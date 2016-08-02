using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class FormMatlab : Form
    {
        public FormMatlab()
        {
            InitializeComponent();
            this.propertyGridMatlab.SelectedObject = ConfigMatlab.Instance;
        }


        private static FormMatlab singleton = new FormMatlab();

        public static FormMatlab Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FormMatlab();
                return singleton;
            }
        }

        private void FormMatlab_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;

            this.Hide();
        }

        public void OutPut(string info)
        {
            if (!ConfigMatlab.Instance.OutPutInfo)
                return;

            this.richTextBoxInfo.AppendText(info);
            this.richTextBoxInfo.AppendText("\n");
            this.richTextBoxInfo.ScrollToCaret();
            this.richTextBoxInfo.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.richTextBoxInfo.Clear();

        }

    }
}
