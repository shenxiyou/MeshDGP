using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class MenuGu : UserControl
    {


        public MenuGu()
        {
            InitializeComponent();
            InitMenuGu(this.guToolStripMenuItem);

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

        private void InitMenuGu(ToolStripMenuItem toolStrip)
        {

            toolStrip.Name = "toolToolStripMenuItem";
            toolStrip.Text = "Gu";
            foreach (EnumGu type in Enum.GetValues(typeof(EnumGu)))
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
            EnumGu type = (EnumGu)((ToolStripMenuItem)sender).Tag;

            if (Mesh == null)
            {
                return;
            } 
          
            ControllerGu.Instance.Run(Mesh, type);
            OnChanged(EventArgs.Empty);
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfigGu.Instance.BringToFront();
            FormConfigGu.Instance.Show();
        }

        
    }
}
