using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data; 
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class MenuWolfram : UserControl
    {
        public MenuWolfram()
        {
            InitializeComponent();
            InitMenu(this.imageMagicToolStripMenuItem);
        }



        public SaveFileDialog saveFileDialog = new SaveFileDialog();
        public OpenFileDialog openFileDialog = new OpenFileDialog();

        public string SetUpSaveDialog()
        {
            this.saveFileDialog.Filter = "Image files (*.png)|*.png|Mesh files (*.jpg)|*.jpg|Mesh files (*.bmp)|*.bmp|All files (*.*)|*.*";
            this.saveFileDialog.OverwritePrompt = true;
            this.saveFileDialog.FileName = "";
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return null;
            }
            else
            {
                return this.saveFileDialog.FileName.ToLower();
            }
        }


        public string SetUpOpenDialog()
        {
            this.openFileDialog.Filter = "Wolfram files (*.nb)|*.nb|All files (*.*)|*.*";
            this.openFileDialog.CheckFileExists = true;
            this.openFileDialog.FileName = "";
            this.openFileDialog.Title = "Open Images";
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return null;
            }
            else
            {
                return this.openFileDialog.FileName.ToLower();
            }
        }

        private void InitMenu(ToolStripMenuItem toolStrip)
        {

            toolStrip.Name = "toolToolStripMenuItem"; 
            foreach (EnumWolfRam type in Enum.GetValues(typeof(EnumWolfRam)))
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = type.ToString() + "ToolStripMenuItem";
                item.Text = type.ToString();
                item.Tag = type;
                item.Click += item_Click;
                toolStrip.DropDownItems.Add(item);
            }

        }

        void item_Click(object sender, EventArgs e)
        {
            EnumWolfRam type = (EnumWolfRam)((ToolStripMenuItem)sender).Tag;

            ControllerWolfRam.Instance.Run(type);

          //  MessageBox.Show("Operation finished!");
        }


        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormWolfram.Instance.BringToFront();
            FormWolfram.Instance.Show();
        }

      

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file = this.SetUpOpenDialog();
            if (file != null)
            {
                ConfigWolfram.Instance.DefaultFile = file;
            }
        }

       
    }
}
