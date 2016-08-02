using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Autodesk.Maya.OpenMaya;
namespace GraphicResearchHuiZhao
{
    public partial class Form1 : Form
    {
        MDagPath mdp;

        public Form1(MDagPath dp)
        {
            InitializeComponent();
            mdp = dp;
        }

        private void ResetCheck()
        {
            dagPathToolStripMenuItem.Checked = false;
            nodeToolStripMenuItem.Checked = false;
            specificToolStripMenuItem.Checked = false;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            dagPathToolStripMenuItem_Click(sender, e);
        }

        private void dagPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetCheck();

            if (mdp != null)
            {
                dagPathToolStripMenuItem.Checked = true;
                Text = "Properties of DagPath " + mdp.partialPathName;
                PropGrid.SelectedObject = mdp;
            }
            else
                MessageBox.Show("This dagpath is null", "DAG Explorer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void nodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetCheck();

            if ((mdp != null) && (mdp.node != null))
            {
                nodeToolStripMenuItem.Checked = true;
                Text = "Properties of Node " + mdp.partialPathName;
                PropGrid.SelectedObject = mdp.node;
            }
            else
                MessageBox.Show("This dagpath node is null", "DAG Explorer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void specificToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetCheck();

            if (mdp != null)
            {
                Object spec = MayaProxy.Instance.SpecializeObject(mdp);

                if (spec != null)
                {
                    specificToolStripMenuItem.Checked = true;
                    Text = "Properties of MFn object " + mdp.partialPathName;
                    PropGrid.SelectedObject = spec;
                }
                else
                    MessageBox.Show("This object is not currently supported", "DAG Explorer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is it !");
        }
    }
}
