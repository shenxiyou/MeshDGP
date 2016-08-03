using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class MenuMeshInfo : UserControl
    {


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

        public MenuMeshInfo()
        {
            InitializeComponent();

            InitRetireveSimple();
          
            InitNormal();

          

            
        }


        public void InitRetireveSimple()
        {
            foreach (var item in Enum.GetValues(typeof(EnumPatchType)))
            {
                this.toolStripComboBoxInput.Items.Add(item);
                this.toolStripComboBoxOutput.Items.Add(item);
            }
            this.toolStripComboBoxInput.SelectedIndex = 0;
            this.toolStripComboBoxOutput.SelectedIndex = 0;
        }

        
 

        private void InitComponent(ToolStripMenuItem toolstrip)
        {

            ToolStripMenuItem item = new ToolStripMenuItem();

            item.Name = "componentToolStripMenuItem";
            item.Text = "Component";

            toolstrip.DropDownItems.Add(item);

            item.Click += new EventHandler(component_Click);



        }

        List<TriMesh> meshes = null;

        private void component_Click(object sender, EventArgs e)
        {
            if (Mesh == null)
                return;

            if (meshes != null) 
                return ;
            
            meshes = TriMeshModify.SeperateComponent(Mesh);
            ToolStripMenuItem item = ((ToolStripMenuItem)sender);
            item.DropDownItems.Clear();

            int num = TriMeshUtil.CountComponents(Mesh,false);
            for (int i = 0; i < num; i++)
            {
                ToolStripMenuItem subitem = new ToolStripMenuItem();
                subitem.Name = i.ToString() + "ToolStripMenuItem";
                subitem.Text = "Component " + i.ToString();
                subitem.Tag = i;
                subitem.Click += Component_Click;
                item.DropDownItems.Add(subitem);

            }

            
        }

       

        private void Component_Click(object sender, EventArgs e)
        {
            int index = (int)((ToolStripMenuItem)sender).Tag;

            Mesh = meshes[index];

            OnChanged(EventArgs.Empty);

        }

        private void InitNormal()
        {
          
            normalToolStripMenuItem.Name = "toolToolStripMenuItem";
            normalToolStripMenuItem.Text = "Vertex Normal";
            foreach (EnumNormal type in Enum.GetValues(typeof(EnumNormal)))
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = type.ToString() + "ToolStripMenuItem";
                item.Text = type.ToString();
                item.Tag = type;
                item.Click += Normal_Click;
                normalToolStripMenuItem.DropDownItems.Add(item);
            }

        }

        private void Normal_Click(object sender, EventArgs e)
        {
            EnumNormal type = (EnumNormal)((ToolStripMenuItem)sender).Tag;

            TriMeshUtil.SetUpVertexNormal(Mesh, type);
        }


        

        private void InitRetrieve()
        {
             
            retrieveToolStripMenuItem.Name = "toolToolStripMenuItem";
            retrieveToolStripMenuItem.Text = "Retrieve";
            foreach (EnumRetrieve type in Enum.GetValues(typeof(EnumRetrieve)))
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = type.ToString() + "ToolStripMenuItem";
                item.Text = type.ToString();
                item.Tag = type;
                item.Click += Retrieve_Click;
                retrieveToolStripMenuItem.DropDownItems.Add(item);
            }

        }

        private void Retrieve_Click(object sender, EventArgs e)
        {
            EnumRetrieve type = (EnumRetrieve)((ToolStripMenuItem)sender).Tag;

            TriMeshUtil.Retrieve(Mesh,type);
        }


        private void tool_Click(object sender, EventArgs e)
        {
            EnumFunction type = (EnumFunction)((ToolStripMenuItem)sender).Tag;
            TriMeshFunction.Instance.FunctionType = type;
            
        }

        private void basicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
             FormMeshInfo.Instance.Show();
        }

        

        

        private void setColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshUtil.ShowMeshPatchColor(Mesh);
            
        }

        private void distanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DistanceDijkstra djkstra = new DistanceDijkstra(Mesh);
            double dis = djkstra.Run();
            MessageBox.Show("Congratulation,min distance is  " + dis.ToString());
        }

       

        private void eigenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEigenMatrix eigen = new FormEigenMatrix();
            eigen.Show();
        }

        private void colorComponentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshUtil.CountComponents(Mesh, true);
            OnChanged(EventArgs.Empty);
        }

       

        private void clearColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshUtil.ClearMeshColor(Mesh);
            OnChanged(EventArgs.Empty);
        }

        private void colorPatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshUtil.ShowMeshPatchColor(Mesh);
            OnChanged(EventArgs.Empty);
        }

        private void curvatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurvatureLib.Init(Mesh);
            PrincipalCurvature[] cur = CurvatureLib.ComputeCurvature();
            TriMeshUtil.SetUpCurvature(Mesh,cur);

        }

        private void faceNormalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshUtil.SetUpNormalFace(Mesh);
        }

        private void oneRingSingleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnumPatchType input = (EnumPatchType)this.toolStripComboBoxInput.SelectedItem;
            EnumPatchType output = (EnumPatchType)this.toolStripComboBoxOutput.SelectedItem;
            TriMeshUtil.RetrieveSingle(GlobalData.Instance.TriMesh, input, output);
            OnChanged(EventArgs.Empty);
        }

        private void boundayPatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnumPatchType input = (EnumPatchType)this.toolStripComboBoxInput.SelectedItem;
            EnumPatchType output = (EnumPatchType)this.toolStripComboBoxOutput.SelectedItem;
            TriMeshUtil.ShowBoundaryOfPatch(GlobalData.Instance.TriMesh, input, output);
            OnChanged(EventArgs.Empty);
        }

        private void oneRingPatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnumPatchType input = (EnumPatchType)this.toolStripComboBoxInput.SelectedItem;
            EnumPatchType output = (EnumPatchType)this.toolStripComboBoxOutput.SelectedItem;
            TriMeshUtil.ShowOneRingOfPatch(GlobalData.Instance.TriMesh, input, output);
            OnChanged(EventArgs.Empty);
        }

        private void neighborFaceOfFaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshUtil.ShowNeighborFaceOfFace(Mesh);
            OnChanged(EventArgs.Empty);
        }

        private void twoRingOfVertexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshUtil.ShowTwoRingOfVertex(Mesh);
            OnChanged(EventArgs.Empty);
        }

        private void boundaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshUtil.ShowBoundary(Mesh);
            OnChanged(EventArgs.Empty);
        }

        private void faceBySplitLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshUtil.ShowMeshPatchColor(Mesh);
            OnChanged(EventArgs.Empty);
        }

        private void treeCoTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeCoTree treeCotree = new TreeCoTree(Mesh);

            List<List<TriMesh.HalfEdge>> loops = treeCotree.ExtractHonologyGenerator(Mesh);
 
            OnChanged(EventArgs.Empty);
        }

        private void initToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Mesh == null)
                return;

            if (meshes != null)
                return;

            meshes = TriMeshModify.SeperateComponent(Mesh);
            ToolStripMenuItem item = subToolStripMenuItem;
            item.DropDownItems.Clear();

            int num = TriMeshUtil.CountComponents(Mesh, false);
            for (int i = 0; i < num; i++)
            {
                ToolStripMenuItem subitem = new ToolStripMenuItem();
                subitem.Name = i.ToString() + "ToolStripMenuItem";
                subitem.Text = "Component " + i.ToString();
                subitem.Tag = i;
                subitem.Click += Component_Click;
                item.DropDownItems.Add(subitem);

            }

           

        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.meshes = null;
            this.subToolStripMenuItem.DropDownItems.Clear();
            OnChanged(EventArgs.Empty);
        }

         
      
    }
}
