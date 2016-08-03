using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class MenuAnimation : UserControl
    {
        public MenuAnimation()
        {
            InitializeComponent();
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

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfigAnimation.Instance.BringToFront();
            FormConfigAnimation.Instance.Show();
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AniController.Instance.SetTimer();
            AniController.Instance.Play();
 
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AniController.Instance.Stop();
        }
    }
}
