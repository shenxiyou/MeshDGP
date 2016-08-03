using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class MenuTool : UserControl
    {
       

        public MenuTool()
        {
            InitializeComponent();

            InitTool(this.toolStripMenuItem);
        }

        private void InitTool(ToolStripMenuItem toolStrip)
        {

            toolStrip.Name = "toolToolStripMenuItem";
            toolStrip.Text = "Tool";
            foreach (EnumTool tool in Enum.GetValues(typeof(EnumTool)))
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = tool.ToString() + "ToolStripMenuItem";
                item.Text = tool.ToString();
                item.Tag = tool;
                item.Click += tool_Click;
                toolStrip.DropDownItems.Add(item);
            } 
            
        }

        private void tool_Click(object sender, EventArgs e)
        {
            EnumTool tool = (EnumTool)((ToolStripMenuItem)sender).Tag; 
            ToolPool.Instance.SwitchTool(tool); 
        }

        private void reverseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshUtil.SelectedVertexReverse(GlobalData.Instance.TriMesh);
            GlobalData.Instance.OnChanged(EventArgs.Empty);
        }
 
    }
}
