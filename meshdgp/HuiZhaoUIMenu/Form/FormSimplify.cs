using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace GraphicResearchHuiZhao
{
    public partial class FormSimplify : Form
    {
        

       
       

        Metro metro = new Metro();
 

        public TriMesh Mesh
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

        private static FormSimplify singleton = new FormSimplify();

        public static FormSimplify Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FormSimplify();
                return singleton;
            }
        }
     
        
        protected virtual void OnChanged(EventArgs e)
        {
            GlobalData.Instance.OnChanged(e);
        }

        public FormSimplify()
        {
            InitializeComponent();

            //this.methods = MethodCollector.GetAllMethods<IErrorGoalSimplication>();
            //foreach (var item in this.methods)
            //{
            //    string name = item.Name;
            //    int tail = name.IndexOf("Simplication");
            //    string shortName = name.Substring(0, tail);
            //    this.listBox1.Items.Add(shortName);
            //}
            //this.listBox1.SelectedIndex = 0;
            InitInfo();
            InitSimplicationType();

          

            metro.Completed += m_Completed;
        }

        private void InitSimplicationType()
        {
            foreach (EnumSimplicationType type in Enum.GetValues(typeof(EnumSimplicationType)))
            {
                this.listBoxType.Items.Add(type);
          
            }

            this.listBoxType.SelectedIndex = 0;
        }
        
        public void InitForm()
        {
            if (Mesh != null)
            {
                this.textBoxfaceCount.Text = Mesh.Faces.Count.ToString();
                this.textBoxPreserved.Text = Mesh.Faces.Count.ToString();
            }
        }

        private void simpificationButton_Click(object sender, EventArgs e)
        {
            this.DoBefore();
            int num = int.Parse(this.textBoxfaceCount.Text);

            EnumSimplicationType type =(EnumSimplicationType) this.listBoxType.SelectedItem;


            TriMeshSimplifyController.Simplify(Mesh, type, num);

     
           

            

            OnChanged(EventArgs.Empty);
           
            this.DoAfter();
        }

        ProgressiveMesh pm;

        private void progressBtn_Click(object sender, EventArgs e)
        {
            if (this.pm == null)
            {
                this.pm = new ProgressiveMesh(this.Mesh);
            }
            this.DoBefore();
            int numbers = int.Parse(this.textBoxPreserved.Text);
            this.pm.Run(numbers);
           
            this.DoAfter();

            OnChanged(EventArgs.Empty);
        }

        private void backwardBtn_Click(object sender, EventArgs e)
        {
            if (this.pm == null)
            {
                this.pm = new ProgressiveMesh(this.Mesh);
            }
            this.DoBefore();
            this.pm.Backward();
            
            this.pm.Show();
          
            this.DoAfter();

            OnChanged(EventArgs.Empty);
        }

        private void forwardBtn_Click(object sender, EventArgs e)
        {
            if (this.pm == null)
            {
                this.pm = new ProgressiveMesh(this.Mesh);
            }
            this.DoBefore();
            this.pm.Forward();
           
            this.pm.Show();
          
            this.DoAfter();

            OnChanged(EventArgs.Empty);
        }

        private void clusterButton_Click(object sender, EventArgs e)
        {
            this.DoBefore();
            int cubes = int.Parse(textBoxLength.Text);
            TriMeshSimplicationCluster cluster = new TriMeshSimplicationCluster(Mesh);

            cluster.BuildCubes(cubes);
            cluster.ClusterProcess();
           
            this.DoAfter();

            OnChanged(EventArgs.Empty);
        }

        private void FormSimplify_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;

            this.Hide();
        }

        private void InitInfo()
        {
            this.MeshView.Columns.Add("Property ");
            this.MeshView.Columns.Add("Before");
            this.MeshView.Columns.Add("After");
            this.MeshView.Items.Add("vertex");
            this.MeshView.Items.Add("edge");
            this.MeshView.Items.Add("face");
            this.MeshView.Items[0].SubItems.Add("");
            this.MeshView.Items[1].SubItems.Add("");
            this.MeshView.Items[2].SubItems.Add("");
            this.MeshView.Items[0].SubItems.Add("");
            this.MeshView.Items[1].SubItems.Add("");
            this.MeshView.Items[2].SubItems.Add("");

            this.MetroView.Columns.Add("Property");
            this.MetroView.Columns.Add("Forward");
            this.MetroView.Columns.Add("Backward");
            this.MetroView.Items.Add("Hausdorff");
            this.MetroView.Items.Add("Max");
            this.MetroView.Items.Add("Mean");
            this.MetroView.Items.Add("RMS");
            foreach (ListViewItem item in this.MetroView.Items)
            {
                item.SubItems.Add("");
                item.SubItems.Add("");
            }
        }

        private void DoBefore()
        {
            this.MeshView.Items[0].SubItems[1].Text = this.Mesh.Vertices.Count.ToString();
            this.MeshView.Items[1].SubItems[1].Text = this.Mesh.Edges.Count.ToString();
            this.MeshView.Items[2].SubItems[1].Text = this.Mesh.Faces.Count.ToString();

            string file = @"AutoBefore.obj";
            TriMeshIO.WriteToObjFile(file, Mesh);
        }

        private void DoAfter()
        {
            this.MeshView.Items[0].SubItems[2].Text = this.Mesh.Vertices.Count.ToString();
            this.MeshView.Items[1].SubItems[2].Text = this.Mesh.Edges.Count.ToString();
            this.MeshView.Items[2].SubItems[2].Text = this.Mesh.Faces.Count.ToString();

            string file = @"AutoAfter.obj";
            TriMeshIO.WriteToObjFile(file, Mesh);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.metro.Run("AutoBefore.obj", "AutoAfter.obj");
            foreach (ListViewItem item in this.MetroView.Items)
            {
                item.SubItems[1].Text = "";
                item.SubItems[2].Text = "";
            }
        }

        private void m_Completed(object sender, EventArgs e)
        {
            this.MetroView.Invoke(new EventHandler(delegate
            {
                this.MetroView.Items[0].SubItems[1].Text = this.metro.Hausdorff.ToString();
                this.MetroView.Items[1].SubItems[1].Text = this.metro.Forward.Max.ToString();
                this.MetroView.Items[2].SubItems[1].Text = this.metro.Forward.Mean.ToString();
                this.MetroView.Items[3].SubItems[1].Text = this.metro.Forward.RMS.ToString();

                this.MetroView.Items[0].SubItems[2].Text = "-";
                this.MetroView.Items[1].SubItems[2].Text = this.metro.Backward.Max.ToString();
                this.MetroView.Items[2].SubItems[2].Text = this.metro.Backward.Mean.ToString();
                this.MetroView.Items[3].SubItems[2].Text = this.metro.Backward.RMS.ToString();
            }));
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            InitForm();
        }
    }
}
