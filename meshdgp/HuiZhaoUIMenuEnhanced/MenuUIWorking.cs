using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class MenuUIWorking : UserControl
    {
        public MenuUIWorking()
        {
            InitializeComponent();
        }

        private void dECToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDEC.Instance.Show();
            FormDEC.Instance.Mesh = GlobalData.Instance.TriMesh;
        }

        private void surfaceReconstructionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSurfaceRec.Instance.Mesh = GlobalData.Instance.TriMesh;

            FormSurfaceRec.Instance.BringToFront();
            FormSurfaceRec.Instance.Show();
        }

        
  
 

        private void rayTracingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormScene form = new FormScene();
            form.Show();
        }

        private void reconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            FormRecon.Instance.BringToFront();
            FormRecon.Instance.Show();
        }

  

        
         
       
    }
}
