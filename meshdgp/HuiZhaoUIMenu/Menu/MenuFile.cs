using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class MenuFile : UserControl
    {
        public MenuFile()
        {
            InitializeComponent(); 

        }


        public void Init()
        {
            this.Parent.Parent.Text = HuiZhaoInfo.Instance.Title;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            MenuUIIO.Instance.OpenManiFoldFile();

            Init();

            ToolPool.Instance.SwitchTool(EnumTool.View); 
        }

        private void saveScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openBigMeshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MenuUIIO.Instance.OpenBigMesh();

            ToolPool.Instance.SwitchTool(EnumTool.View); 
        }

        private void openQuadMeshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MenuUIIO.Instance.OpenQuadMesh();

            ToolPool.Instance.SwitchTool(EnumTool.View); 
        }

        private void saveMeshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MenuUIIO.Instance.SaveMesh();
        }

        private void releaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MenuUIIO.Instance.Release();
        }

        private void saveMeshWithTextureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MenuUIIO.Instance.SaveMeshWithTexture();
        }

        private void loadSecondMeshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MenuUIIO.Instance.OpenManiFoldFileTwo();
        }

        

        private void clearQuadMeshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MenuUIIO.Instance.ClearQuad();
        }

        private void clearTriMeshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MenuUIIO.Instance.Release();
        }

        private void saveSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MenuUIIO.Instance.SaveSelection();
        }

        private void addOneMoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MenuUIIO.Instance.AddOneMore();
        }

        private void allMeshesToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            this.allMeshesToolStripMenuItem.DropDownItems.Clear();
            for (int i = 0; i < GlobalData.Instance.AllMeshes.Count;i++ )
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = i.ToString() + "ToolStripMenuItem";
                item.Text = i.ToString() + "--" + GlobalData.Instance.AllMeshes[i].FileName;
                item.Tag = i;
                item.Click += Mesh_Click;
                this.allMeshesToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        private void Mesh_Click(object sender, EventArgs e)
        {
           int index = (int)((ToolStripMenuItem)sender).Tag;

           GlobalData.Instance.TriMesh = GlobalData.Instance.AllMeshes[index];
           GlobalData.Instance.OnChanged(EventArgs.Empty);

        }

        private void clearAllMeshesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlobalData.Instance.AllMeshes.Clear();

            GlobalData.Instance.OnChanged(EventArgs.Empty);
        }


    }
}
