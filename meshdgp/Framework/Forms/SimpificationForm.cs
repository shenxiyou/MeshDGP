using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphicsResearch
{
    public partial class SimpificationForm : Form
    {
        public SimpificationForm()
        {
            InitializeComponent();
            GlobalData.Instance.HalfEdgeTriMesh.InitVertexDecimation(HalfEdgeMesh.TriMesh.VertexDecimationCriteriaType.GaussianCurvature);

        }

        public Form mainForm;

        private void button1_Click(object sender, EventArgs e)
        {
            int cubes = int.Parse(textBox1.Text);
            GlobalData.Instance.HalfEdgeTriMesh.BuildCubes(cubes);
            GlobalData.Instance.HalfEdgeTriMesh.AnalizeTriMeshHeightAndWeight();
            GlobalData.Instance.HalfEdgeTriMesh.ClusterProcess();
            mainForm.Refresh();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string type = (string)comboBox1.SelectedItem;
           switch (type)
            {
                case "Gaussian":
                    int numbers = int.Parse(textBox2.Text);
                    GlobalData.Instance.HalfEdgeTriMesh.criteriaType = HalfEdgeMesh.TriMesh.VertexDecimationCriteriaType.GaussianCurvature;
                    GlobalData.Instance.HalfEdgeTriMesh.VertexDecimation(numbers);
                    mainForm.Refresh();
                    break;
                default:
                    break;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void ClusterForm_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int num = int.Parse(textBox3.Text);
            GlobalData.Instance.HalfEdgeTriMesh.SimplifyQEM(num);
        }
    }
}
