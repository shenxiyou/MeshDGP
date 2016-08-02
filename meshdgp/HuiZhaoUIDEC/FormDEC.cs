using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class FormDEC : Form
    {
        public FormDEC()
        {
            InitializeComponent();
        }

        public TriMesh Mesh = null;

        

        // Invoke the Changed event; called whenever list changes
        protected virtual void OnChanged(EventArgs e)
        {
            GlobalData.Instance.OnChanged(e);
        }

        private static FormDEC singleton = new FormDEC();


        public static FormDEC Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FormDEC();
                return singleton;
            }
        }



        private void FormDec_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;

            this.Hide();
        }



        private void buttonFairing_Click_1(object sender, EventArgs e)
        {
            Fairing fairing = new Fairing(Mesh);

            fairing.Run();

            OnChanged(EventArgs.Empty);
        }

        private void buttonFlattern_Click(object sender, EventArgs e)
        {
            Flattern flattern = new Flattern(Mesh);

            flattern.FlattenProcess();

            OnChanged(EventArgs.Empty);
        }

        private List<TriMesh.Vertex> selected;
        private List<double> selectedValues;

        private List<double> generatorsValues;

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new List<TriMesh.Vertex>();
                selectedValues = new List<double>();

            }
            else
            {
                selected.Clear();
                selectedValues.Clear();
            }

            if (connection == null)
            {
                connection = new PriorNonTrivalConnection(Mesh);

                singularites = new List<KeyValuePair<HalfEdgeMesh.Vertex, double>>();

                for (int i = 0; i < selected.Count; i++)
                {
                    singularites.Add(new KeyValuePair<HalfEdgeMesh.Vertex, double>(selected[i], selectedValues[i]));
                }

                connection.Singularities = singularites;
            }
            List<List<TriMesh.HalfEdge>> Generators = connection.Generators;

            if (generatorsValues == null)
            {
                generatorsValues = new List<double>();

                //Find the max dual
                int max = int.MinValue;
                int index = 0;
                int count = 0;
                foreach (List<TriMesh.HalfEdge> cycle in connection.Generators)
                {
                    if (cycle.Count > max)
                    {
                        max = cycle.Count;
                        index = count;
                    }
                    count++;
                }

                for (int i = 0; i < Generators.Count; i++)
                {
                    generatorsValues.Add(i == index ? 2 : 0);
                }
            }
            else
            {
                generatorsValues.Clear();

                //Find the max dual
                int max = int.MinValue;
                int index = 0;
                int count = 0;
                foreach (List<TriMesh.HalfEdge> cycle in connection.Generators)
                {
                    if (cycle.Count > max)
                    {
                        max = cycle.Count;
                        index = count;
                    }
                    count++;
                }

                for (int i = 0; i < Generators.Count; i++)
                {
                    generatorsValues.Add(i == index ? 2 : 0);
                }

            }

            //Add vertex singularities
            foreach (TriMesh.Vertex v in Mesh.Vertices)
            {
                if (v.Traits.selectedFlag > 0)
                {
                    selected.Add(v);
                    selectedValues.Add(0f);
                }
            }

            int selectedCount = selectedValues.Count;
            double initValue = 2.0f / (double)selectedCount;

            for (int i = 0; i < selectedCount; i++)
            {
                selectedValues[i] = initValue;
            }

            dataGridViewSingularity.Rows.Clear();

            for (int i = 0; i < selectedCount; i++)
            {
                dataGridViewSingularity.Rows.Add("Vertex" + selected[i].Index, selectedValues[i].ToString());
            }

            for (int i = selectedCount; i < selectedCount + Generators.Count; i++)
            {
                dataGridViewSingularity.Rows.Add("Generator" + (i - selectedCount), generatorsValues[i - selectedCount].ToString());
            }

        }

        private void dataGridViewSingularity_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewSingularity_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            double value = double.Parse(this.dataGridViewSingularity.Rows[index].Cells[1].Value.ToString());

            //double sum = 0;
            //double sumWithoutFirst = 0;
            //for (int i = 0; i < selectedValues.Count; i++)
            //{
            //    if (i == index)
            //    {
            //        continue;
            //    }

            //    sum += selectedValues[i];
            //    if (i > 0)
            //    {
            //        sumWithoutFirst+=selectedValues[i];
            //    }
            //}
            //sum += value; 
            //sumWithoutFirst += value;

            //if (Math.Abs(sum - 2.0f) > 1e-6)
            //{
            //    selectedValues[0] = 2 - sumWithoutFirst;
            //}
            if (index < selectedValues.Count)
            {
                selectedValues[index] = value;

                for (int i = 0; i < selectedValues.Count; i++)
                {
                    this.dataGridViewSingularity.Rows[i].Cells[1].Value = selectedValues[i].ToString();
                }
            }
            else
            {
                generatorsValues[index - selectedValues.Count] = value;
                this.dataGridViewSingularity.Rows[index].Cells[1].Value = value.ToString();
            }


        }

        public double Angle
        {
            get { return (double)Math.PI * ((double)numericUpDownAngle.Value / (double)180); }
        }


        PriorNonTrivalConnection connection;
        List<KeyValuePair<TriMesh.Vertex, double>> singularites;

        private void buttonProcess_Click(object sender, EventArgs e)
        {
            if (connection == null)
            {
                connection = new PriorNonTrivalConnection(Mesh);

                singularites = new List<KeyValuePair<HalfEdgeMesh.Vertex, double>>();
                //singularites.Add(new KeyValuePair<HalfEdgeMesh.Vertex, double>(Mesh.Vertices[0],2));


            }

            //Update Singularities
            connection.Singularities.Clear();
            for (int i = 0; i < selected.Count; i++)
            {
                connection.Singularities.Add(new KeyValuePair<HalfEdgeMesh.Vertex, double>(selected[i], selectedValues[i]));
            }


            //Update Generator
            connection.GeneratorValues.Clear();
            for (int i = 0; i < generatorsValues.Count; i++)
            {
                connection.GeneratorValues.Add(new KeyValuePair<List<HalfEdgeMesh.HalfEdge>, double>(connection.Generators[i], generatorsValues[i]));
            }

            connection.Update();

            Vector3D[] vectorFields = connection.ComputeVectorField(Angle);

            GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.Vector;
            GlobalData.Instance.FaceVectors = vectorFields;
            GlobalData.Instance.N = (int)NnumericUpDown.Value;
            OnChanged(EventArgs.Empty);

        }


        private void buttonTreeCotree_Click(object sender, EventArgs e)
        {
            TreeCoTree treeCotree = new TreeCoTree(Mesh);

            DynamicTree<TriMesh.Vertex> tree;
            DynamicTree<TriMesh.Face> cotree;

            treeCotree.GeneratorTreeCotree(Mesh, out tree, out cotree);

            GlobalData.Instance.tree = treeCotree.BuildTree(tree);
            GlobalData.Instance.cotree = treeCotree.BuildCoTree(cotree);

            
            GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.TreeCotree;
            OnChanged(EventArgs.Empty);
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            TreeCoTree treeCotree = new TreeCoTree(Mesh);

            List<List<TriMesh.HalfEdge>> loops = treeCotree.ExtractHonologyGenerator(Mesh);

            GlobalData.Instance.Generators = loops;
            GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.Generator;
            OnChanged(EventArgs.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TreeCoTree treeCotree = new TreeCoTree(Mesh);

            List<List<TriMesh.HalfEdge>> loops = treeCotree.ExtractHonologyGenerator(Mesh);

            HarmonicBasis basis = new HarmonicBasis(Mesh);
            basis.BuildHarmonicBasis(loops);

            GlobalData.Instance.Generators = loops;
            GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.Generator;
            OnChanged(EventArgs.Empty);

        }

        private void buttonGeo_Click(object sender, EventArgs e)
        {
            Geodistance distance = new Geodistance(Mesh);

            Vector3D[] vecotrFields = null;

            double maxDistance = 0;

            double[] distances = distance.Process(1, Mesh, out vecotrFields, out  maxDistance);

            //Precalculate distance
            //double minDistance = double.MaxValue;
            //double maxDistance = double.MinValue;
            //for (int i = 0; i < distances.Length; i++)
            //{
            //    double value = distances[i];
            //    if (minDistance > value)
            //    {
            //        minDistance = value;
            //    }
            //    if (maxDistance < value)
            //    {
            //        maxDistance = value;
            //    }
            //}

            //double span = maxDistance - minDistance;
            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = (distances[i]) / maxDistance;
               
            }




            GlobalData.Instance.ColorVis = distances;
            GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.ColorVis;

           
            
        }


        private void buttonSpinForm_Click(object sender, EventArgs e)
        {
            SpinForm from = new SpinForm(Mesh);
            from.UpdateDeformation("D:\\bumpy.tga", 5);
        }

        private void NnumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (connection != null)
            {

                Vector3D[] vectorFields = connection.ComputeVectorField(Angle);
                GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.Vector;
                GlobalData.Instance.FaceVectors = vectorFields;
                GlobalData.Instance.N = (int)NnumericUpDown.Value;
                OnChanged(EventArgs.Empty);
            } 
        }
    }
}
