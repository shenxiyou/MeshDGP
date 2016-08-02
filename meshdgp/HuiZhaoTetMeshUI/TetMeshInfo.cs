using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class TetMeshInfo : Form
    {

        protected virtual void OnChanged(EventArgs e)
        {
            GlobalData.Instance.OnChanged(e);
        }

        private static TetMeshInfo singleton = new TetMeshInfo();


        public static TetMeshInfo Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new TetMeshInfo();
                return singleton;
            }
        }


        bool onLoad = true;
        bool onPatch = false;

        public TetMeshInfo()
        {
            InitializeComponent();
            TetMesh tet = GlobalData.Instance.TetMesh;
            this.propertyGrid1.SelectedObject = new TetMeshSettings();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            switch (e.TabPage.Text)
            {
                case "Info":
                    break;
                case "Vertex":
                    this.RefreshVertex();
                    break;
                case "Edge":
                    this.RefreshEdge();
                    break;
                case "Face":
                    this.RefreshFace();
                    break;
                case "Tetrahedron":
                    this.RefreshTetrahedron();
                    break;
                default:
                    break;
            }
        }

        void RefreshVertex()
        {
            TetMesh mesh = GlobalData.Instance.TetMesh;
            this.onLoad = true;
            this.vertexView.Rows.Clear();
            this.vertexView.Rows.Add(mesh.Vertices.Count);
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                TetVertex v = mesh.Vertices[i];
                DataGridViewRow row = this.vertexView.Rows[i];
                row.Cells["VIndex"].Value = v.Index;
                row.Cells["VFlag"].Value = v.Flag;
                row.Cells["VBoundary"].Value = v.OnBoundary;

                row.Cells["VX"].Value = v.Pos.x;
                row.Cells["VY"].Value = v.Pos.y;
                row.Cells["VZ"].Value = v.Pos.z;
            }
            this.onLoad = false;
        }

        void RefreshEdge()
        {
            TetMesh mesh = GlobalData.Instance.TetMesh;
            this.onLoad = true;
            this.edgeView.Rows.Clear();
            this.edgeView.Rows.Add(mesh.Edges.Count);
            for (int i = 0; i < mesh.Edges.Count; i++)
            {
                TetEdge e = mesh.Edges[i];
                DataGridViewRow row = this.edgeView.Rows[i];
                row.Cells["EIndex"].Value = e.Index;
                row.Cells["EFlag"].Value = e.Flag;
                row.Cells["EBoundary"].Value = e.OnBoundary;

                row.Cells["EV0"].Value = e.Vertices[0].Index;
                row.Cells["EV1"].Value = e.Vertices[1].Index;
            }
            this.onLoad = false;
        }

        void RefreshFace()
        {
            TetMesh mesh = GlobalData.Instance.TetMesh;
            this.onLoad = true;
            this.faceView.Rows.Clear();
            this.faceView.Rows.Add(mesh.Faces.Count);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                TetFace f = mesh.Faces[i];
                DataGridViewRow row = this.faceView.Rows[i];
                row.Cells["FIndex"].Value = f.Index;
                row.Cells["FFlag"].Value = f.Flag;
                row.Cells["FBoundary"].Value = f.OnBoundary;

                row.Cells["FV0"].Value = f.Vertices[0].Index;
                row.Cells["FV1"].Value = f.Vertices[1].Index;
                row.Cells["FV2"].Value = f.Vertices[2].Index;
            }
            this.onLoad = false;
        }

        void RefreshTetrahedron()
        {
            TetMesh mesh = GlobalData.Instance.TetMesh;
            this.onLoad = true;
            this.tetrahedronView.Rows.Clear();
            this.tetrahedronView.Rows.Add(mesh.Tetras.Count);
            for (int i = 0; i < mesh.Tetras.Count; i++)
            {
                Tetrahedron tet = mesh.Tetras[i];
                DataGridViewRow row = this.tetrahedronView.Rows[i];
                row.Cells["TIndex"].Value = tet.Index;
                row.Cells["TFlag"].Value = tet.Flag;
                row.Cells["TBoundary"].Value = tet.OnBoundary;

                row.Cells["TV0"].Value = tet.Vertices[0].Index;
                row.Cells["TV1"].Value = tet.Vertices[1].Index;
                row.Cells["TV2"].Value = tet.Vertices[2].Index;
                row.Cells["TV3"].Value = tet.Vertices[3].Index;
            }
            this.onLoad = false;
        }

        private void CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.onLoad)
            {
                TetMesh mesh = GlobalData.Instance.TetMesh;
                DataGridView dgv = (DataGridView)sender;
                DataGridViewColumn column = dgv.Columns[e.ColumnIndex];
                DataGridViewRow row = dgv.Rows[e.RowIndex];
                int index = (int)row.Cells[0].Value;
                switch (column.Name)
                {
                    case "VFlag":
                        mesh.Vertices[index].Flag = Convert.ToInt32(row.Cells[e.ColumnIndex].Value);
                        break;
                    case "EFlag":
                        mesh.Edges[index].Flag = Convert.ToInt32(row.Cells[e.ColumnIndex].Value);
                        break;
                    case "FFlag":
                        mesh.Faces[index].Flag = Convert.ToInt32(row.Cells[e.ColumnIndex].Value);
                        break;
                    case "TFlag":
                        mesh.Tetras[index].Flag = Convert.ToInt32(row.Cells[e.ColumnIndex].Value);
                        break;
                    default:
                        break;
                }
                if (!this.onPatch)
                {
                    mesh.ComputeSelectedNormal();
                    GlobalData.Instance.OnChanged(new EventArgs());
                }
            }
        }

        private void View_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right || e.KeyCode==Keys.Left)
            {
                this.onPatch = true;
                TetMesh mesh = GlobalData.Instance.TetMesh;
                DataGridView dgv = (DataGridView)sender;
                int columnIndex = 0;
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    if (column.Name.EndsWith("Flag"))
                    {
                        columnIndex = column.Index;
                    }
                }
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Selected)
                    {
                        DataGridViewCell cell = row.Cells[columnIndex];
                        if (e.KeyCode == Keys.Right)
                        {
                            cell.Value = (int)cell.Value + 1;
                        }
                        else if (e.KeyCode == Keys.Left)
                        {
                            cell.Value = (int)cell.Value > 0 ? (int)cell.Value - 1 : 0;
                        }
                    }
                }
                this.onPatch = false;
                mesh.ComputeSelectedNormal();
                GlobalData.Instance.OnChanged(new EventArgs());
            }
        }

        private void TetMeshInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;

            this.Hide();
        }
    }
}
