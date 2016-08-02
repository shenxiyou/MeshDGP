namespace GraphicResearchHuiZhao
{
    partial class FormEigenMatrix
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControlEigen = new System.Windows.Forms.TabControl();
            this.tabPageBasic = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.buttonDump = new System.Windows.Forms.Button();
            this.buttonLoadEigen = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.buttonDispaly = new System.Windows.Forms.Button();
            this.buttonAll = new System.Windows.Forms.Button();
            this.buttonMatrix = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxEigenNumber = new System.Windows.Forms.TextBox();
            this.dumpButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxLaplaceType = new System.Windows.Forms.ComboBox();
            this.tabPageInfo = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridViewEigenValue = new System.Windows.Forms.DataGridView();
            this.EigenValueIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EigenValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewEigenVector = new System.Windows.Forms.DataGridView();
            this.EigenIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EigenVector = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageMatrix = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dataGridViewMatrixInfo = new System.Windows.Forms.DataGridView();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Row = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewDetail = new System.Windows.Forms.DataGridView();
            this.MatrixItemIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemRow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageMatrixType = new System.Windows.Forms.TabPage();
            this.dataGridViewType = new System.Windows.Forms.DataGridView();
            this.TypeIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MatrixType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeRow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeWholeSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeZero = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeSysmetric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageExpand = new System.Windows.Forms.TabPage();
            this.textBoxMatrix = new System.Windows.Forms.TextBox();
            this.buttonSaveMatrix = new System.Windows.Forms.Button();
            this.buttonOpenMatrix = new System.Windows.Forms.Button();
            this.buttonReadVector = new System.Windows.Forms.Button();
            this.buttonSaveVector = new System.Windows.Forms.Button();
            this.tabPageLinearSystem = new System.Windows.Forms.TabPage();
            this.tabControlEigen.SuspendLayout();
            this.tabPageBasic.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPageInfo.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEigenValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEigenVector)).BeginInit();
            this.tabPageMatrix.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMatrixInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDetail)).BeginInit();
            this.tabPageMatrixType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewType)).BeginInit();
            this.tabPageExpand.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlEigen
            // 
            this.tabControlEigen.Controls.Add(this.tabPageBasic);
            this.tabControlEigen.Controls.Add(this.tabPageInfo);
            this.tabControlEigen.Controls.Add(this.tabPageMatrix);
            this.tabControlEigen.Controls.Add(this.tabPageMatrixType);
            this.tabControlEigen.Controls.Add(this.tabPageExpand);
            this.tabControlEigen.Controls.Add(this.tabPageLinearSystem);
            this.tabControlEigen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlEigen.Location = new System.Drawing.Point(0, 0);
            this.tabControlEigen.Name = "tabControlEigen";
            this.tabControlEigen.SelectedIndex = 0;
            this.tabControlEigen.Size = new System.Drawing.Size(741, 362);
            this.tabControlEigen.TabIndex = 0;
            // 
            // tabPageBasic
            // 
            this.tabPageBasic.Controls.Add(this.groupBox5);
            this.tabPageBasic.Controls.Add(this.groupBox4);
            this.tabPageBasic.Location = new System.Drawing.Point(4, 22);
            this.tabPageBasic.Name = "tabPageBasic";
            this.tabPageBasic.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBasic.Size = new System.Drawing.Size(733, 336);
            this.tabPageBasic.TabIndex = 0;
            this.tabPageBasic.Text = "Basic";
            this.tabPageBasic.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.buttonSaveVector);
            this.groupBox5.Controls.Add(this.buttonReadVector);
            this.groupBox5.Controls.Add(this.buttonOpenMatrix);
            this.groupBox5.Controls.Add(this.buttonSaveMatrix);
            this.groupBox5.Controls.Add(this.buttonDump);
            this.groupBox5.Controls.Add(this.buttonLoadEigen);
            this.groupBox5.Location = new System.Drawing.Point(48, 160);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(586, 87);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Matlab";
            // 
            // buttonDump
            // 
            this.buttonDump.Location = new System.Drawing.Point(60, 25);
            this.buttonDump.Name = "buttonDump";
            this.buttonDump.Size = new System.Drawing.Size(135, 23);
            this.buttonDump.TabIndex = 2;
            this.buttonDump.Text = "Save Eigen";
            this.buttonDump.UseVisualStyleBackColor = true;
            this.buttonDump.Click += new System.EventHandler(this.buttonDump_Click);
            // 
            // buttonLoadEigen
            // 
            this.buttonLoadEigen.Location = new System.Drawing.Point(60, 58);
            this.buttonLoadEigen.Name = "buttonLoadEigen";
            this.buttonLoadEigen.Size = new System.Drawing.Size(135, 23);
            this.buttonLoadEigen.TabIndex = 1;
            this.buttonLoadEigen.Text = "Open Eigen";
            this.buttonLoadEigen.UseVisualStyleBackColor = true;
            this.buttonLoadEigen.Click += new System.EventHandler(this.buttonLoadEigen_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.buttonDispaly);
            this.groupBox4.Controls.Add(this.buttonAll);
            this.groupBox4.Controls.Add(this.buttonMatrix);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.textBoxEigenNumber);
            this.groupBox4.Controls.Add(this.dumpButton);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.comboBoxLaplaceType);
            this.groupBox4.Location = new System.Drawing.Point(48, 36);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(586, 99);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Lib";
            // 
            // buttonDispaly
            // 
            this.buttonDispaly.Location = new System.Drawing.Point(464, 59);
            this.buttonDispaly.Name = "buttonDispaly";
            this.buttonDispaly.Size = new System.Drawing.Size(100, 23);
            this.buttonDispaly.TabIndex = 10;
            this.buttonDispaly.Text = "Display matrix";
            this.buttonDispaly.UseVisualStyleBackColor = true;
            this.buttonDispaly.Click += new System.EventHandler(this.buttonDispaly_Click);
            // 
            // buttonAll
            // 
            this.buttonAll.Location = new System.Drawing.Point(464, 28);
            this.buttonAll.Name = "buttonAll";
            this.buttonAll.Size = new System.Drawing.Size(100, 23);
            this.buttonAll.TabIndex = 9;
            this.buttonAll.Text = "Build All Matrix";
            this.buttonAll.UseVisualStyleBackColor = true;
            this.buttonAll.Click += new System.EventHandler(this.buttonAll_Click);
            // 
            // buttonMatrix
            // 
            this.buttonMatrix.Location = new System.Drawing.Point(365, 28);
            this.buttonMatrix.Name = "buttonMatrix";
            this.buttonMatrix.Size = new System.Drawing.Size(75, 23);
            this.buttonMatrix.TabIndex = 8;
            this.buttonMatrix.Text = "Build Matrix";
            this.buttonMatrix.UseVisualStyleBackColor = true;
            this.buttonMatrix.Click += new System.EventHandler(this.buttonMatrix_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Number Of Eigen:";
            // 
            // textBoxEigenNumber
            // 
            this.textBoxEigenNumber.Location = new System.Drawing.Point(128, 61);
            this.textBoxEigenNumber.Name = "textBoxEigenNumber";
            this.textBoxEigenNumber.Size = new System.Drawing.Size(183, 20);
            this.textBoxEigenNumber.TabIndex = 6;
            this.textBoxEigenNumber.Text = "50";
            // 
            // dumpButton
            // 
            this.dumpButton.Location = new System.Drawing.Point(365, 61);
            this.dumpButton.Name = "dumpButton";
            this.dumpButton.Size = new System.Drawing.Size(75, 23);
            this.dumpButton.TabIndex = 4;
            this.dumpButton.Text = "Build Eigen";
            this.dumpButton.UseVisualStyleBackColor = true;
            this.dumpButton.Click += new System.EventHandler(this.dumpButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Matrix Type:";
            // 
            // comboBoxLaplaceType
            // 
            this.comboBoxLaplaceType.FormattingEnabled = true;
            this.comboBoxLaplaceType.Items.AddRange(new object[] {
            "GraphicLaplace",
            "CotLaplace"});
            this.comboBoxLaplaceType.Location = new System.Drawing.Point(128, 28);
            this.comboBoxLaplaceType.Name = "comboBoxLaplaceType";
            this.comboBoxLaplaceType.Size = new System.Drawing.Size(183, 21);
            this.comboBoxLaplaceType.TabIndex = 2;
            // 
            // tabPageInfo
            // 
            this.tabPageInfo.Controls.Add(this.splitContainer1);
            this.tabPageInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageInfo.Name = "tabPageInfo";
            this.tabPageInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInfo.Size = new System.Drawing.Size(733, 336);
            this.tabPageInfo.TabIndex = 1;
            this.tabPageInfo.Text = "Eigen Info";
            this.tabPageInfo.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridViewEigenValue);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridViewEigenVector);
            this.splitContainer1.Size = new System.Drawing.Size(727, 330);
            this.splitContainer1.SplitterDistance = 364;
            this.splitContainer1.TabIndex = 0;
            // 
            // dataGridViewEigenValue
            // 
            this.dataGridViewEigenValue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEigenValue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EigenValueIndex,
            this.EigenValue});
            this.dataGridViewEigenValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewEigenValue.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewEigenValue.Name = "dataGridViewEigenValue";
            this.dataGridViewEigenValue.Size = new System.Drawing.Size(364, 330);
            this.dataGridViewEigenValue.TabIndex = 0;
            this.dataGridViewEigenValue.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewEigenValue_CellDoubleClick);
            // 
            // EigenValueIndex
            // 
            this.EigenValueIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.EigenValueIndex.HeaderText = "Index";
            this.EigenValueIndex.Name = "EigenValueIndex";
            // 
            // EigenValue
            // 
            this.EigenValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.EigenValue.HeaderText = "EigenValue";
            this.EigenValue.Name = "EigenValue";
            // 
            // dataGridViewEigenVector
            // 
            this.dataGridViewEigenVector.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEigenVector.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EigenIndex,
            this.EigenVector});
            this.dataGridViewEigenVector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewEigenVector.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewEigenVector.Name = "dataGridViewEigenVector";
            this.dataGridViewEigenVector.Size = new System.Drawing.Size(359, 330);
            this.dataGridViewEigenVector.TabIndex = 0;
            // 
            // EigenIndex
            // 
            this.EigenIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.EigenIndex.HeaderText = "Index";
            this.EigenIndex.Name = "EigenIndex";
            // 
            // EigenVector
            // 
            this.EigenVector.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.EigenVector.HeaderText = "EigenVector";
            this.EigenVector.Name = "EigenVector";
            // 
            // tabPageMatrix
            // 
            this.tabPageMatrix.Controls.Add(this.splitContainer2);
            this.tabPageMatrix.Location = new System.Drawing.Point(4, 22);
            this.tabPageMatrix.Name = "tabPageMatrix";
            this.tabPageMatrix.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMatrix.Size = new System.Drawing.Size(733, 336);
            this.tabPageMatrix.TabIndex = 2;
            this.tabPageMatrix.Text = "Matrix";
            this.tabPageMatrix.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dataGridViewMatrixInfo);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dataGridViewDetail);
            this.splitContainer2.Size = new System.Drawing.Size(727, 330);
            this.splitContainer2.SplitterDistance = 210;
            this.splitContainer2.TabIndex = 0;
            // 
            // dataGridViewMatrixInfo
            // 
            this.dataGridViewMatrixInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMatrixInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.Row});
            this.dataGridViewMatrixInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewMatrixInfo.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewMatrixInfo.Name = "dataGridViewMatrixInfo";
            this.dataGridViewMatrixInfo.Size = new System.Drawing.Size(210, 330);
            this.dataGridViewMatrixInfo.TabIndex = 0;
            // 
            // Index
            // 
            this.Index.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Index.HeaderText = "Index";
            this.Index.Name = "Index";
            // 
            // Row
            // 
            this.Row.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Row.HeaderText = "Item";
            this.Row.Name = "Row";
            // 
            // dataGridViewDetail
            // 
            this.dataGridViewDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MatrixItemIndex,
            this.Column,
            this.ItemRow,
            this.Value});
            this.dataGridViewDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDetail.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewDetail.Name = "dataGridViewDetail";
            this.dataGridViewDetail.Size = new System.Drawing.Size(513, 330);
            this.dataGridViewDetail.TabIndex = 0;
            // 
            // MatrixItemIndex
            // 
            this.MatrixItemIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MatrixItemIndex.HeaderText = "Index";
            this.MatrixItemIndex.Name = "MatrixItemIndex";
            // 
            // Column
            // 
            this.Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column.HeaderText = "Column";
            this.Column.Name = "Column";
            // 
            // ItemRow
            // 
            this.ItemRow.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemRow.HeaderText = "Row";
            this.ItemRow.Name = "ItemRow";
            // 
            // Value
            // 
            this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            // 
            // tabPageMatrixType
            // 
            this.tabPageMatrixType.Controls.Add(this.dataGridViewType);
            this.tabPageMatrixType.Location = new System.Drawing.Point(4, 22);
            this.tabPageMatrixType.Name = "tabPageMatrixType";
            this.tabPageMatrixType.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMatrixType.Size = new System.Drawing.Size(733, 336);
            this.tabPageMatrixType.TabIndex = 3;
            this.tabPageMatrixType.Text = "MatrixType";
            this.tabPageMatrixType.UseVisualStyleBackColor = true;
            // 
            // dataGridViewType
            // 
            this.dataGridViewType.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewType.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TypeIndex,
            this.MatrixType,
            this.TypeColumn,
            this.TypeRow,
            this.TypeSize,
            this.TypeWholeSize,
            this.TypeZero,
            this.TypeSysmetric});
            this.dataGridViewType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewType.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewType.Name = "dataGridViewType";
            this.dataGridViewType.Size = new System.Drawing.Size(727, 330);
            this.dataGridViewType.TabIndex = 0;
            // 
            // TypeIndex
            // 
            this.TypeIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TypeIndex.HeaderText = "Index";
            this.TypeIndex.Name = "TypeIndex";
            // 
            // MatrixType
            // 
            this.MatrixType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MatrixType.HeaderText = "Matrix Type";
            this.MatrixType.Name = "MatrixType";
            // 
            // TypeColumn
            // 
            this.TypeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TypeColumn.HeaderText = "Column";
            this.TypeColumn.Name = "TypeColumn";
            // 
            // TypeRow
            // 
            this.TypeRow.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TypeRow.HeaderText = "Row";
            this.TypeRow.Name = "TypeRow";
            // 
            // TypeSize
            // 
            this.TypeSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TypeSize.HeaderText = "NoZero";
            this.TypeSize.Name = "TypeSize";
            // 
            // TypeWholeSize
            // 
            this.TypeWholeSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TypeWholeSize.HeaderText = "WholeSize";
            this.TypeWholeSize.Name = "TypeWholeSize";
            // 
            // TypeZero
            // 
            this.TypeZero.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TypeZero.HeaderText = "Zero";
            this.TypeZero.Name = "TypeZero";
            // 
            // TypeSysmetric
            // 
            this.TypeSysmetric.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TypeSysmetric.HeaderText = "Symmetric";
            this.TypeSysmetric.Name = "TypeSysmetric";
            // 
            // tabPageExpand
            // 
            this.tabPageExpand.Controls.Add(this.textBoxMatrix);
            this.tabPageExpand.Location = new System.Drawing.Point(4, 22);
            this.tabPageExpand.Name = "tabPageExpand";
            this.tabPageExpand.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageExpand.Size = new System.Drawing.Size(733, 336);
            this.tabPageExpand.TabIndex = 4;
            this.tabPageExpand.Text = "Matrix";
            this.tabPageExpand.UseVisualStyleBackColor = true;
            // 
            // textBoxMatrix
            // 
            this.textBoxMatrix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMatrix.Location = new System.Drawing.Point(3, 3);
            this.textBoxMatrix.Multiline = true;
            this.textBoxMatrix.Name = "textBoxMatrix";
            this.textBoxMatrix.Size = new System.Drawing.Size(727, 330);
            this.textBoxMatrix.TabIndex = 0;
            // 
            // buttonSaveMatrix
            // 
            this.buttonSaveMatrix.Location = new System.Drawing.Point(230, 25);
            this.buttonSaveMatrix.Name = "buttonSaveMatrix";
            this.buttonSaveMatrix.Size = new System.Drawing.Size(136, 23);
            this.buttonSaveMatrix.TabIndex = 3;
            this.buttonSaveMatrix.Text = "Save Matrix";
            this.buttonSaveMatrix.UseVisualStyleBackColor = true;
            this.buttonSaveMatrix.Click += new System.EventHandler(this.buttonSaveMatrix_Click);
            // 
            // buttonOpenMatrix
            // 
            this.buttonOpenMatrix.Location = new System.Drawing.Point(230, 58);
            this.buttonOpenMatrix.Name = "buttonOpenMatrix";
            this.buttonOpenMatrix.Size = new System.Drawing.Size(136, 23);
            this.buttonOpenMatrix.TabIndex = 4;
            this.buttonOpenMatrix.Text = "Open Matrix";
            this.buttonOpenMatrix.UseVisualStyleBackColor = true;
            this.buttonOpenMatrix.Click += new System.EventHandler(this.buttonOpenMatrix_Click);
            // 
            // buttonReadVector
            // 
            this.buttonReadVector.Location = new System.Drawing.Point(415, 25);
            this.buttonReadVector.Name = "buttonReadVector";
            this.buttonReadVector.Size = new System.Drawing.Size(94, 23);
            this.buttonReadVector.TabIndex = 5;
            this.buttonReadVector.Text = "Read Vector";
            this.buttonReadVector.UseVisualStyleBackColor = true;
            // 
            // buttonSaveVector
            // 
            this.buttonSaveVector.Location = new System.Drawing.Point(415, 58);
            this.buttonSaveVector.Name = "buttonSaveVector";
            this.buttonSaveVector.Size = new System.Drawing.Size(94, 23);
            this.buttonSaveVector.TabIndex = 6;
            this.buttonSaveVector.Text = "Save Vector";
            this.buttonSaveVector.UseVisualStyleBackColor = true;
            // 
            // tabPageLinearSystem
            // 
            this.tabPageLinearSystem.Location = new System.Drawing.Point(4, 22);
            this.tabPageLinearSystem.Name = "tabPageLinearSystem";
            this.tabPageLinearSystem.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLinearSystem.Size = new System.Drawing.Size(733, 336);
            this.tabPageLinearSystem.TabIndex = 5;
            this.tabPageLinearSystem.Text = "Linear System";
            this.tabPageLinearSystem.UseVisualStyleBackColor = true;
            // 
            // FormEigenMatrix
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 362);
            this.Controls.Add(this.tabControlEigen);
            this.Name = "FormEigenMatrix";
            this.Text = "FormEigenMatrix";
            this.tabControlEigen.ResumeLayout(false);
            this.tabPageBasic.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPageInfo.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEigenValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEigenVector)).EndInit();
            this.tabPageMatrix.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMatrixInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDetail)).EndInit();
            this.tabPageMatrixType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewType)).EndInit();
            this.tabPageExpand.ResumeLayout(false);
            this.tabPageExpand.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlEigen;
        private System.Windows.Forms.TabPage tabPageBasic;
        private System.Windows.Forms.TabPage tabPageInfo;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button buttonLoadEigen;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxEigenNumber;
        private System.Windows.Forms.Button dumpButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxLaplaceType;
        private System.Windows.Forms.Button buttonDump;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridViewEigenValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn EigenValueIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn EigenValue;
        private System.Windows.Forms.DataGridView dataGridViewEigenVector;
        private System.Windows.Forms.DataGridViewTextBoxColumn EigenIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn EigenVector;
        private System.Windows.Forms.TabPage tabPageMatrix;
        private System.Windows.Forms.Button buttonMatrix;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dataGridViewMatrixInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn Row;
        private System.Windows.Forms.DataGridView dataGridViewDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn MatrixItemIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemRow;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.TabPage tabPageMatrixType;
        private System.Windows.Forms.DataGridView dataGridViewType;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn MatrixType;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeRow;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeWholeSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeZero;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeSysmetric;
        private System.Windows.Forms.Button buttonAll;
        private System.Windows.Forms.TabPage tabPageExpand;
        private System.Windows.Forms.TextBox textBoxMatrix;
        private System.Windows.Forms.Button buttonDispaly;
        private System.Windows.Forms.Button buttonOpenMatrix;
        private System.Windows.Forms.Button buttonSaveMatrix;
        private System.Windows.Forms.Button buttonSaveVector;
        private System.Windows.Forms.Button buttonReadVector;
        private System.Windows.Forms.TabPage tabPageLinearSystem;
    }
}