using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class MenuRendering : UserControl
    {
        public MenuRendering()
        {
            InitializeComponent();
            InitSurface(this.renderingToolStripMenuItem);

        }

        private void InitSurface(ToolStripMenuItem toolstrip)
        {
            ToolStripMenuItem item = new ToolStripMenuItem();
            toolstrip.DropDownItems.Add(item);
            item.Name = "toolToolStripMenuItem";
            item.Text = "Surface";
            foreach (EnumSurface type in Enum.GetValues(typeof(EnumSurface)))
            {
                ToolStripMenuItem subitem = new ToolStripMenuItem();
                subitem.Name = type.ToString() + "ToolStripMenuItem";
                subitem.Text = type.ToString();
                subitem.Tag = type;
                subitem.Click += new EventHandler(subitem_Click);
                item.DropDownItems.Add(subitem);
            }

        }

        void subitem_Click(object sender, EventArgs e)
        {
            EnumSurface surface=(EnumSurface)((ToolStripMenuItem)sender).Tag;
            switch (surface)
            {
                case EnumSurface.SmoothDiffuse: 
                    FormRendering.Instance.Grid.SelectedObject=ConfigRenderSmoothDiffuse.Instance ;
                    break;
                case EnumSurface.RoughDiffuse:
                    FormRendering.Instance.Grid.SelectedObject = ConfigRenderRoughDiffuse.Instance;
                    break;
            }
            FormRendering.Instance.BringToFront();
            FormRendering.Instance.Show();
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        
    }
}
