using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class FormColorVis : Form
    {

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
        private CriteriaInfo criteriaInfo = new CriteriaInfo();

        private CriteriaRange criteriaRange = new CriteriaRange();

        public FormColorVis()
        {
            InitializeComponent();

            Init();
        }

     

        // Invoke the Changed event; called whenever list changes
        protected virtual void OnChanged(EventArgs e)
        {
            GlobalData.Instance.OnChanged(e);
        }

        private static FormColorVis singleton = new FormColorVis();


        public static FormColorVis Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FormColorVis();
                return singleton;
            }
        }

        public void Init()
        {
            

            this.propertyGridCriteria.SelectedObject = criteriaInfo;
            this.propertyGridRange.SelectedObject = criteriaRange;
            
        }

        

        private void propertyGridCriteria_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            double[] value=TriMeshColorByCriteria.Instance.SetColor(e.ChangedItem.Label, Mesh, criteriaInfo);

            ReSetDataGrid(value);

            GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.Basic;
            OnChanged(EventArgs.Empty);
        }


        private void ReSetDataGrid(double[] value)
        {
            dataGridViewColor.Rows.Clear();
            for (int i = 0; i < Mesh.Vertices.Count; i++)
            {
                if(Mesh.Vertices[i].Traits.SelectedFlag>0)
                {
                    dataGridViewColor.Rows.Add("Vertex", Mesh.Vertices[i].Index.ToString(),value[i].ToString());
                }

            }

            for (int i = 0; i < Mesh.Edges.Count; i++)
            {
                if (Mesh.Edges[i].Traits.SelectedFlag > 0)
                {
                    dataGridViewColor.Rows.Add("Edge", Mesh.Edges[i].Index.ToString(), value[i].ToString());
                }
            }

            for (int i = 0; i < Mesh.Faces.Count; i++)
            {
                if (Mesh.Faces[i].Traits.SelectedFlag > 0)
                {
                    dataGridViewColor.Rows.Add("Face", Mesh.Faces[i].Index.ToString(), value[i].ToString());
                }
            }
        }

        private void propertyGridRange_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == "Item")
            {
                TriMeshColorByCriteria.Instance.SetRange(Mesh,(EnumColorItem)e.ChangedItem.Value, ref criteriaRange); 
                
            }
        }

        private void FormColorVis_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;

            this.Hide();


        }


 

    
         

        

    }
}
