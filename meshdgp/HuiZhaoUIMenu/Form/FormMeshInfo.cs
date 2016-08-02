using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
 
namespace GraphicResearchHuiZhao
{
    public partial class FormMeshInfo : Form
    {
        private static FormMeshInfo singleton = new FormMeshInfo();


        
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

        protected virtual void OnChanged(EventArgs e)
        {
            GlobalData.Instance.OnChanged(e);
        }

        public static FormMeshInfo Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FormMeshInfo();
                return singleton;
            }
        }

       

        public FormMeshInfo()
        {
            InitializeComponent();
           
        }

                
        #region Basic

     

        private void InitVertexInfo()
        {
            this.dataGridViewVertex.Rows.Clear();
            
            foreach(TriMesh.Vertex vertex in Mesh.Vertices)
            {
                this.dataGridViewVertex.Rows.Add(vertex.Index, vertex.Traits.Position.x, vertex.Traits.Position.y, vertex.Traits.Position.z);

            }
            foreach (TriMesh.Face face in Mesh.Faces)
            {
                this.dataGridViewFace.Rows.Add(face.Index, face.GetVertex(0).Index, face.GetVertex(1).Index, face.GetVertex(2).Index, TriMeshUtil.ComputeAreaFace(face));
            }

            foreach (TriMesh.Edge edge in Mesh.Edges)
            {
                string face0 = "null";
                string face1 = "null";
                if (edge.Face0 != null)
                {
                    face0 = edge.Face0.Index.ToString();
                }
                if (edge.Face1 != null)
                {
                    face1 = edge.Face1.Index.ToString();
                }
                this.dataGridViewEdge.Rows.Add(edge.Index, 
                                               edge.Vertex0.Index, 
                                               edge.Vertex1.Index, face0, 
                                               face1,TriMeshUtil.ComputeEdgeLength(edge),
                                               TriMeshUtil.ComputeDihedralAngle(edge)/3.14*180);
            }
             
        }

        private void dataGridViewVertex_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            double x = double.Parse(this.dataGridViewVertex.Rows[index].Cells[1].Value.ToString());
            double y = double.Parse(this.dataGridViewVertex.Rows[index].Cells[2].Value.ToString());
            double z = double.Parse(this.dataGridViewVertex.Rows[index].Cells[3].Value.ToString());
            Mesh.Vertices[index].Traits.Position.x = x;
            Mesh.Vertices[index].Traits.Position.y = y;
            Mesh.Vertices[index].Traits.Position.z = z;
         
            
        }

       

       

    

 
 

 

        #endregion

        private void MeshForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;

            this.Hide();
        }
        

        


        

        private void InitCurvatureInfoGrid() {


            this.dataGridViewCurvature.Rows.Clear();
            double[] curvature = TriMeshUtil.ComputeGaussianCurvature(Mesh);

            double[] meanCurv = TriMeshUtil.ComputeMeanCurvature(Mesh);
            PrincipalCurvature[] basic = TriMeshUtil.ComputePricipalCurvature(Mesh);

            PrincipalCurvature[] pg = TriMeshUtil.ComputePricipalCurvaturePG07(Mesh);
            CurvatureLib.Init(Mesh);
            PrincipalCurvature[] ccc = CurvatureLib.ComputeCurvature();

            for (int i = 0; i < curvature.Length; i++)
            {
                this.dataGridViewCurvature.Rows.Add(i,
                                                    curvature[i],
                                                    meanCurv[i],
                                                    ccc[i].max,
                                                    ccc[i].min,
                                                    pg[i].max,
                                                    pg[i].min);

            }

            
            
        }

         
 

     

        public void InitMeshInfo()
        {
            dataGridViewGeneral.Rows.Clear();

            Dictionary<string, string> meshinfo = TriMeshUtil.BuildMeshInfo(Mesh);

            int index=0;
            foreach (KeyValuePair<string, string> pair in meshinfo)
            {                
                dataGridViewGeneral.Rows.Add(index, pair.Key , pair.Value);
                index++;
            }
            
        }

       

        

        private void dataGridViewVertex_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

            this.propertyGridVertex.SelectedObject = Mesh.Vertices[e.RowIndex].Traits;
            
        }

        private void dataGridViewEdge_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.propertyGridEdge.SelectedObject = Mesh.Edges[e.RowIndex].Traits;
            
        }

        

    

        private void dataGridViewFace_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.propertyGridFace.SelectedObject = Mesh.Faces[e.RowIndex].Traits;
             
        }

         

        

        public void InitSelectedInfo(TriMesh mesh)
        {
            this.dataGridViewSelected.Rows.Clear();
            int index=0;
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                if (mesh.Vertices[i].Traits.SelectedFlag > 0)
                {
                    this.dataGridViewSelected.Rows.Add(index, i, "Vertex", mesh.Vertices[i].Traits.SelectedFlag.ToString(), mesh.Vertices[i].Traits.Color.ToString());
                    index++;
                }
            }

           
            for (int i = 0; i < mesh.Edges.Count; i++)
            {
                if (mesh.Edges[i].Traits.SelectedFlag > 0)
                {
                    this.dataGridViewSelected.Rows.Add(index, i, "Edge", mesh.Edges[i].Traits.SelectedFlag.ToString(), mesh.Edges[i].Traits.Color.ToString());
                    index++;
                }
            }

           
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                if (mesh.Faces[i].Traits.SelectedFlag > 0)
                {
                    this.dataGridViewSelected.Rows.Add(index, i, "Face", mesh.Faces[i].Traits.SelectedFlag.ToString(), mesh.Faces[i].Traits.Color.ToString());
                    index++;
                }
            }
        }

        public void RefreshInfo()
        {
            if (Mesh != null)
            {
                InitCurvatureInfoGrid();
                
                InitVertexInfo();
                InitSelectedInfo(Mesh);
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (Mesh != null)
            {
                
                InitSelectedInfo(Mesh);
            }
        }

        private void buttonBasic_Click(object sender, EventArgs e)
        {
            if (Mesh != null)
            {
                InitMeshInfo();
            }
        }

        

        private void dataGridViewSelected_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string type = this.dataGridViewSelected.Rows[e.RowIndex].Cells["Type"].Value.ToString();


            int index = int.Parse(this.dataGridViewSelected.Rows[e.RowIndex].Cells["SelectedIndex"].Value.ToString());
            if (type == "Edge")
                this.propertyGridSelected.SelectedObject = Mesh.Edges[index].Traits;
            if (type == "Face")
                this.propertyGridSelected.SelectedObject = Mesh.Faces[index].Traits;
            if (type == "Vertex")
                this.propertyGridSelected.SelectedObject = Mesh.Vertices[index].Traits;


        }

        private void propertyGridSelected_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            InitSelectedInfo(Mesh);

            OnChanged(EventArgs.Empty);
        }

        private void buttonVertex_Click(object sender, EventArgs e)
        {
            if (Mesh != null)
            {
               
                InitVertexInfo();
              
            }

        }

        private void buttonCurvature_Click(object sender, EventArgs e)
        {
            if (Mesh != null)
            {
                InitCurvatureInfoGrid();

               
            }
        }

       
    }
}
