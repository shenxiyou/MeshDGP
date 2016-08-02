using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class FormConfigMaya : Form
    {
        public FormConfigMaya()
        {
            InitializeComponent();
            this.propertyGridConfig.SelectedObject =  ConfigMaya.Instance;
        }

        private static FormConfigMaya singleton = new FormConfigMaya();

        public static FormConfigMaya Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FormConfigMaya();
                return singleton;
            }
        }

        public DataGridView Datagrid
        {
            get
            {
                return this.dataGridView1;
            }
        }

        public Bitmap PicBox
        {
            set 
            {
                this.pictureBox.Image = value;
                this.pictureBox.Refresh();
            }
            get
            {
                return (Bitmap)this.pictureBox.Image;
            }
        }

        public string Info
        {
            get
            {
                return this.richTextBox1.Text;
            }
            set
            {
                this.richTextBox1.Text = value;
            }
        }

        private void propertyGridConfig_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {

        }

        private void FormConfigMaya_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;

            this.Hide();
        }

    }
}
