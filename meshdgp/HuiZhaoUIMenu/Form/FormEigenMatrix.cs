using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class FormEigenMatrix : Form
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

        public FormEigenMatrix()
        {
            InitializeComponent();
            InitLapalceMatrix();
        }

        private void InitLapalceMatrix()
        {
            this.comboBoxLaplaceType.Items.Clear();
            foreach (EnumLaplaceMatrix type in Enum.GetValues(typeof(EnumLaplaceMatrix)))
            {
                this.comboBoxLaplaceType.Items.Add(type);
                 
            }
        }


        Eigen eigen = null;
        private void dumpButton_Click(object sender, EventArgs e)
        {
            EnumLaplaceMatrix type = (EnumLaplaceMatrix)this.comboBoxLaplaceType.SelectedItem;

            int count = int.Parse(textBoxEigenNumber.Text);

            eigen=EigenManager.Instance.ComputeEigen(Mesh, type, count);

            InitEigenValue(eigen);
        }


        private void InitEigenValue(Eigen eigen)
        {
            this.dataGridViewEigenValue.Rows.Clear();

            for (int i = 0; i < eigen.Count; i++)
            {
                this.dataGridViewEigenValue.Rows.Add(i.ToString(), eigen.GetEigenValue(i).ToString());
            }
        }

        private void InitEigenVector(Eigen eigen,int index)
        {
            this.dataGridViewEigenVector.Rows.Clear();

            double[] vector = eigen.GetEigenVector(index).ToArray();

            for (int i = 0; i < vector.Length ; i++)
            {
                this.dataGridViewEigenVector.Rows.Add(i.ToString(), vector[i].ToString());
            }
        }

        private void dataGridViewEigenValue_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;

            InitEigenVector(eigen, i);
        }

        private void buttonLoadEigen_Click(object sender, EventArgs e)
        {
            OpenFileDialog opfDialog = new OpenFileDialog();
            opfDialog.Filter = "Eigens File (*.eigens)|*.eigens|All files (*.*)|*.*";
            opfDialog.DefaultExt = ".eigens";
            // opfDialog.FileName = ModelName + ".eigens";
            // opfDialog.InitialDirectory = WorkspacePath;

            DialogResult result = opfDialog.ShowDialog();

            if (result ==  DialogResult.OK)
            {
                
                string choosedEigenFileName = opfDialog.FileName;

                IOHuiZhao.Instance.ReadEigen(choosedEigenFileName);
            }
        }

        SparseMatrix matrix = null;
        
        private void buttonMatrix_Click(object sender, EventArgs e)
        {
            EnumLaplaceMatrix type = (EnumLaplaceMatrix)this.comboBoxLaplaceType.SelectedItem;
            matrix = LaplaceManager.Instance.GenerateLaplaceMatrix(type, Mesh);

            InitMatrixInfo();
            InitMatrixDetail();
        }

        private void InitMatrixDetail()
        {
            this.dataGridViewDetail.Rows.Clear();
            int num=matrix.NumOfElements();
            int index = 0;
            foreach (List<SparseMatrix.Element> r in matrix.Rows)
            {
                foreach (SparseMatrix.Element e in r)
                {
                    this.dataGridViewDetail.Rows.Add(index, e.i, e.j, e.value);
                    index++;
                }
            }
             
        }

        private void InitMatrixExpand()
        {
            this.textBoxMatrix.Clear();

            this.textBoxMatrix.Text += "\r\n";
            this.textBoxMatrix.Text += "\r\n";
            this.textBoxMatrix.Text += "\r\n";

            for (int i = 0; i < matrix.RowSize; i++)
            {
                this.textBoxMatrix.Text += "       ";
                for (int j = 0; j < matrix.ColumnSize; j++)
                {
                    this.textBoxMatrix.Text += matrix[i, j]+ "     ";
                }
                this.textBoxMatrix.Text += "\r\n";
            }
        }

        private void InitMatrixType()
        {
            SparseMatrix matrix = null;
            this.dataGridViewType.Rows.Clear();
            int index=0;
            foreach (EnumLaplaceMatrix type in Enum.GetValues(typeof(EnumLaplaceMatrix)))
            { 
                matrix = LaplaceManager.Instance.GenerateLaplaceMatrix(type, Mesh);

                if (matrix != null)
                {
                    this.dataGridViewType.Rows.Add(index, type, matrix.ColumnSize, matrix.RowSize, matrix.NumOfElements(),
                                                   matrix.ZeroSize, matrix.WholeSize, matrix.IsSymmetric());

                    index++;
                }
            }
        }

        private void InitMatrixInfo()
        {
            EnumLaplaceMatrix type = (EnumLaplaceMatrix)this.comboBoxLaplaceType.SelectedItem;
            this.dataGridViewMatrixInfo.Rows.Clear();
            this.dataGridViewMatrixInfo.Rows.Add("Type", type.ToString());
            this.dataGridViewMatrixInfo.Rows.Add("Column", matrix.ColumnSize );
            this.dataGridViewMatrixInfo.Rows.Add("Row", matrix.RowSize );
            this.dataGridViewMatrixInfo.Rows.Add("NoZero", matrix.NumOfElements() );
            this.dataGridViewMatrixInfo.Rows.Add("Zero",  matrix.ZeroSize  );
            this.dataGridViewMatrixInfo.Rows.Add("Whole Size", (matrix.WholeSize));
            this.dataGridViewMatrixInfo.Rows.Add("Symmetric", (matrix.IsSymmetric()));
        }

        private void buttonAll_Click(object sender, EventArgs e)
        {
            InitMatrixType();
        }

        private void buttonDispaly_Click(object sender, EventArgs e)
        {
            InitMatrixExpand();
        }

        private void buttonDump_Click(object sender, EventArgs e)
        {

        }

        private void buttonSaveMatrix_Click(object sender, EventArgs e)
        {

        }

        private void buttonOpenMatrix_Click(object sender, EventArgs e)
        {
            
        }
    }
}
