using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class MenuTet : UserControl
    {
        public MenuTet()
        {
            InitializeComponent();
            InitDisplay();
        }

        protected virtual void OnChanged(EventArgs e)
        {
            GlobalData.Instance.OnChanged(e);
        }


        private void InitDisplay()
        {
            Array arr = Enum.GetValues(typeof(TetDisplayFlag));
            for (int i = 1; i < arr.Length; i++)
            {
                TetDisplayFlag type = (TetDisplayFlag)arr.GetValue(i);
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = type.ToString() + "Flag";
                item.Text = type.ToString();
                item.Tag = type;
                if ((GlobalTetSetting.TetDisplayFlag & type) == type)
                {
                    item.Checked = true;
                }
                item.Click += DisplayFlag_Click;
                this.displayMenuItem.DropDownItems.Add(item);
            }
        }

        void DisplayFlag_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            TetDisplayFlag flag = (TetDisplayFlag)item.Tag;
            GlobalTetSetting.TetDisplayFlag ^= flag;
            item.Checked = !item.Checked;
            OnChanged(EventArgs.Empty);
        }

        private void genToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TetGenForm.Instance.BringToFront();
            TetGenForm.Instance.Show();
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TetMeshInfo.Instance.BringToFront();
            TetMeshInfo.Instance.Show();
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // TetGenParser parser = new TetGenParser(IOHuiZhao.Instance.GetPath() + "/" + "tettmp.1");

            TetGenParser parser = new TetGenParser( "tettmp.1");
            TetMesh mesh = parser.ReadAll();
            mesh.BuildAdj();
            mesh.ComputeTraits();
            mesh.ComputeNormal();
            //mesh.Check();

            GlobalData.Instance.TetMesh = mesh;
            GlobalData.Instance.TriMesh = null;
            this.OnChanged(new EventArgs());
        }

        private void releaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlobalData.Instance.TetMesh = null;
            this.OnChanged(new EventArgs());
        }
    }
}
