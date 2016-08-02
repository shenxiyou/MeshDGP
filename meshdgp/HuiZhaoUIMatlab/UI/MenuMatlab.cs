using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class MenuMatlab : UserControl
    {
        public MenuMatlab()
        {
            InitializeComponent();

            InitTest(this.testToolStripMenuItem);
        }

 


        private void InitTest(ToolStripMenuItem toolStrip)
        {

            foreach (EnumMatlabTest type in Enum.GetValues(typeof(EnumMatlabTest)))
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = type.ToString() + "ToolStripMenuItem";
                item.Text = type.ToString();
                item.Tag = type;
                item.Click += matrix_Click;
                toolStrip.DropDownItems.Add(item);
            }

        }

        private void matrix_Click(object sender, EventArgs e)
        {
            EnumMatlabTest type = (EnumMatlabTest)((ToolStripMenuItem)sender).Tag; 

            MatlabProxy.Instance.TestCore(type); 

            MessageBox.Show("Test  finished!!");
        }

        private void testToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MatlabProxy.Instance.ComputeLaplace(GlobalData.Instance.TriMesh);

           SparseMatrix sparse=  MatlabProxy.Instance.GetSparseMatrix("L");
           SparseMatrix sparse1 = LaplaceBuilder.Instance.ComputeLaplaceCotHalf(GlobalData.Instance.TriMesh);
           sparse.Multiply(-1d);
           TestUtil.Instance.TestEqual(sparse, sparse1);


            MessageBox.Show("Test  finished!!");
        }

        private void outLapToolStripMenuItem_Click(object sender, EventArgs e)
        {  
            SparseMatrix sparse= LaplaceBuilder.Instance.ComputeLaplaceCotHalf(GlobalData.Instance.TriMesh); 
            MatlabProxy.Instance.OutPut(sparse, "LLL"); 
          
            MessageBox.Show("Test  finished!!");
        }


    
        private void testEignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SparseMatrix sparse = LaplaceBuilder.Instance.ComputeLaplaceCotHalf(GlobalData.Instance.TriMesh);
            MatlabProxy.Instance.eigen = MatlabProxy.Instance.GetEigen(sparse, ConfigMatlab.Instance.EigenNum);

            InitEigen(this.testEignToolStripMenuItem,MatlabProxy.Instance.eigen);
            MessageBox.Show("Test  finished!!");
        }

        private void InitEigen(ToolStripMenuItem toolStrip, Eigen eigen)
        {
            toolStrip.DropDownItems.Clear();
            int i = 0;
            foreach (EigenPair pair in eigen.SortedEigens)
            {
               
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = i.ToString() + "ToolStripMenuItem";
                item.Text = i.ToString();
                item.Tag = i;
                item.Click += eigen_Click;
                toolStrip.DropDownItems.Add(item);
                i++;
            } 
        }

        private void eigen_Click(object sender, EventArgs e)
        {
            int type = (int)((ToolStripMenuItem)sender).Tag;

            double[] fun = MatlabProxy.Instance.eigen.SortedEigens[type].EigenVector.ToArray();
            TriMeshUtil.SetVertexColor(GlobalData.Instance.TriMesh, fun);
            GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.Color;
            GlobalData.Instance.OnChanged(EventArgs.Empty);

            
        }

 

        private void positiveDefiniteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SparseMatrix sparse = LaplaceBuilder.Instance.ComputeLaplaceCotHalf(GlobalData.Instance.TriMesh);
            string info = MatlabProxy.Instance.IsPSD(sparse).ToString();

           FormMatlab.Instance.OutPut("Matrix is Positive Definite:"+info);

            MessageBox.Show("Test  finished!!");
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMatlab.Instance.BringToFront();
            FormMatlab.Instance.Show();
        }
    }
}
