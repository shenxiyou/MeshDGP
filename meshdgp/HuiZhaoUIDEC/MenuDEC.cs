using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class MenuDEC : UserControl
    {


        public MenuDEC()
        {
            InitializeComponent();
            InitDECMenu(this.dECToolStripMenuItem);

        }


        private TriMesh Mesh
        {
            get
            {
                return GlobalData.Instance.TriMesh;
            }
            set
            {
                GlobalData.Instance.TriMesh = value;
            }
        }

        protected virtual void OnChanged(EventArgs e)
        {
            GlobalData.Instance.OnChanged(e);
        }

        private void InitDECMenu(ToolStripMenuItem toolStrip)
        {

            toolStrip.Name = "toolToolStripMenuItem";
            toolStrip.Text = "DEC";
            foreach (EnumDEC type in Enum.GetValues(typeof(EnumDEC)))
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
            EnumDEC type = (EnumDEC)((ToolStripMenuItem)sender).Tag;

            if (Mesh == null)
            {
                return;
            }

            DECController.Instance.Run(Mesh, type);

            OnChanged(EventArgs.Empty);
        }

        
    }
}
