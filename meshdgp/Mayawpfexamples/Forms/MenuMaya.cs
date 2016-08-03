using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao 
{
    public partial class MenuMaya : UserControl
    {
        public MenuMaya()
        {
            InitializeComponent();
            InitMenu(this.selectToolStripMenuItem);
            //this.mayaToolStripMenuItem.Text = "Maya";
            //this.selectToolStripMenuItem.Text = "Select";

            InitMenuBasic(this.basicToolStripMenuItem);
            //this.basicToolStripMenuItem.Text = "Basic";
            InitMenuDeform(this.mayaToolStripMenuItem);
        }


        private void InitMenuBasic(ToolStripMenuItem toolStrip)
        {

            toolStrip.Name = "toolToolStripMenuItem"; 
            foreach (EnumMayaBasic type in Enum.GetValues(typeof(EnumMayaBasic)))
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = type.ToString() + "ToolStripMenuItem";
                item.Text = type.ToString();
                item.Tag = type;
                item.Click += item_ClickBasic;
                toolStrip.DropDownItems.Add(item);
            }

        }

        void item_ClickBasic(object sender, EventArgs e)
        {
            EnumMayaBasic type = (EnumMayaBasic)((ToolStripMenuItem)sender).Tag;

            MayaController.Instance.RunBasic(type);

            MessageBox.Show("Operation finished!");
        }

        private void InitMenuDeform(ToolStripMenuItem toolStrip)
        {

            toolStrip.Name = "toolToolStripMenuItem"; 
            foreach (EnumMayaDeform type in Enum.GetValues(typeof(EnumMayaDeform)))
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = type.ToString() + "ToolStripMenuItem";
                item.Text = type.ToString();
                item.Tag = type;
                item.Click += item_ClickDeform;
                toolStrip.DropDownItems.Add(item);
            }

        }

        void item_ClickDeform(object sender, EventArgs e)
        {
            EnumMayaDeform type = (EnumMayaDeform)((ToolStripMenuItem)sender).Tag;

            MayaController.Instance.RunDeform(type);

            MessageBox.Show("Operation finished!");
        }



        private void InitMenu(ToolStripMenuItem toolStrip)
        {

            toolStrip.Name = "toolToolStripMenuItem"; 
            foreach (EnumMayaCommand type in Enum.GetValues(typeof(EnumMayaCommand)))
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
            //EnumMayaCommand type = (EnumMayaCommand)((ToolStripMenuItem)sender).Tag;

            //MayaController.Instance.Run(type);

            //MessageBox.Show("Operation finished!");
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfigMaya.Instance.BringToFront();
            FormConfigMaya.Instance.Show();
        }
    }
}
