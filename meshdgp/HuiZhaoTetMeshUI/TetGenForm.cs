using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class TetGenForm : Form
    {

        protected virtual void OnChanged(EventArgs e)
        {
            GlobalData.Instance.OnChanged(e);
        }

        private static TetGenForm singleton = new TetGenForm();


        public static TetGenForm Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new TetGenForm();
                return singleton;
            }
        }


        TetGen tetgen; 

        public TetGenForm()
        {
            InitializeComponent(); 
            this.propertyGrid1.SelectedObject = ConfigTet.Instance;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TriMesh mesh = GlobalData.Instance.TriMesh;
            this.tetgen = new TetGen();
            this.tetgen.Exited += tetgen_Exited;
            this.tetgen.OutputDataReceived += tetgen_OutputDataReceived;
            this.tetgen.RunAsync(mesh, this.textBox1.Text);
            this.button1.Enabled = false;
        }

        void tetgen_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            this.textBox2.Invoke(new MethodInvoker(() => { this.textBox2.AppendText(e.Data + "\n"); }));
        }

        void tetgen_Exited(object sender, EventArgs e)
        {
            this.button1.Invoke(new MethodInvoker(() => { this.button1.Enabled = true; }));
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox1.Enabled = this.checkBox1.Checked;
        }

        private void propertyGrid1_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            this.textBox1.Text = ConfigTet.Instance.ToString();
        }

        private void TetGenForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;

            this.Hide();
        }
    }
}
