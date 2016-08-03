namespace GraphicResearchHuiZhao
{
    partial class FormDEC
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
            this.tabControlDEC = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.buttonTrivial = new System.Windows.Forms.Button();
            this.buttonGeo = new System.Windows.Forms.Button();
            this.buttonSpinForm = new System.Windows.Forms.Button();
            this.buttonDecompostion = new System.Windows.Forms.Button();
            this.buttonGenerator = new System.Windows.Forms.Button();
            this.buttonFlattern = new System.Windows.Forms.Button();
            this.buttonFairing = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.NnumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonProcess = new System.Windows.Forms.Button();
            this.numericUpDownAngle = new System.Windows.Forms.NumericUpDown();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.dataGridViewSingularity = new System.Windows.Forms.DataGridView();
            this.vertexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageTreeCotree = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.buttonTreeCotree = new System.Windows.Forms.Button();
            this.tabControlDEC.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NnumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSingularity)).BeginInit();
            this.tabPageTreeCotree.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlDEC
            // 
            this.tabControlDEC.Controls.Add(this.tabPage1);
            this.tabControlDEC.Controls.Add(this.tabPage2);
            this.tabControlDEC.Controls.Add(this.tabPageTreeCotree);
            this.tabControlDEC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlDEC.Location = new System.Drawing.Point(0, 0);
            this.tabControlDEC.Name = "tabControlDEC";
            this.tabControlDEC.SelectedIndex = 0;
            this.tabControlDEC.Size = new System.Drawing.Size(587, 361);
            this.tabControlDEC.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonTrivial);
            this.tabPage1.Controls.Add(this.buttonGeo);
            this.tabPage1.Controls.Add(this.buttonSpinForm);
            this.tabPage1.Controls.Add(this.buttonDecompostion);
            this.tabPage1.Controls.Add(this.buttonGenerator);
            this.tabPage1.Controls.Add(this.buttonFlattern);
            this.tabPage1.Controls.Add(this.buttonFairing);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(579, 335);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basic";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // buttonTrivial
            // 
            this.buttonTrivial.Location = new System.Drawing.Point(312, 181);
            this.buttonTrivial.Name = "buttonTrivial";
            this.buttonTrivial.Size = new System.Drawing.Size(116, 23);
            this.buttonTrivial.TabIndex = 13;
            this.buttonTrivial.Text = "TrivialConnection";
            this.buttonTrivial.UseVisualStyleBackColor = true;
            // 
            // buttonGeo
            // 
            this.buttonGeo.Location = new System.Drawing.Point(312, 135);
            this.buttonGeo.Name = "buttonGeo";
            this.buttonGeo.Size = new System.Drawing.Size(75, 23);
            this.buttonGeo.TabIndex = 12;
            this.buttonGeo.Text = "GeoDistance";
            this.buttonGeo.UseVisualStyleBackColor = true;
            this.buttonGeo.Click += new System.EventHandler(this.buttonGeo_Click);
            // 
            // buttonSpinForm
            // 
            this.buttonSpinForm.Location = new System.Drawing.Point(312, 93);
            this.buttonSpinForm.Name = "buttonSpinForm";
            this.buttonSpinForm.Size = new System.Drawing.Size(75, 23);
            this.buttonSpinForm.TabIndex = 11;
            this.buttonSpinForm.Text = "SpinForm";
            this.buttonSpinForm.UseVisualStyleBackColor = true;
            this.buttonSpinForm.Click += new System.EventHandler(this.buttonSpinForm_Click);
            // 
            // buttonDecompostion
            // 
            this.buttonDecompostion.Location = new System.Drawing.Point(312, 219);
            this.buttonDecompostion.Name = "buttonDecompostion";
            this.buttonDecompostion.Size = new System.Drawing.Size(116, 23);
            this.buttonDecompostion.TabIndex = 10;
            this.buttonDecompostion.Text = "Decompositon";
            this.buttonDecompostion.UseVisualStyleBackColor = true;
            // 
            // buttonGenerator
            // 
            this.buttonGenerator.Location = new System.Drawing.Point(150, 181);
            this.buttonGenerator.Name = "buttonGenerator";
            this.buttonGenerator.Size = new System.Drawing.Size(75, 23);
            this.buttonGenerator.TabIndex = 9;
            this.buttonGenerator.Text = "Cycle";
            this.buttonGenerator.UseVisualStyleBackColor = true;
            // 
            // buttonFlattern
            // 
            this.buttonFlattern.Location = new System.Drawing.Point(150, 136);
            this.buttonFlattern.Name = "buttonFlattern";
            this.buttonFlattern.Size = new System.Drawing.Size(75, 23);
            this.buttonFlattern.TabIndex = 8;
            this.buttonFlattern.Text = "Flattern";
            this.buttonFlattern.UseVisualStyleBackColor = true;
            this.buttonFlattern.Click += new System.EventHandler(this.buttonFlattern_Click);
            // 
            // buttonFairing
            // 
            this.buttonFairing.Location = new System.Drawing.Point(150, 93);
            this.buttonFairing.Name = "buttonFairing";
            this.buttonFairing.Size = new System.Drawing.Size(75, 23);
            this.buttonFairing.TabIndex = 7;
            this.buttonFairing.Text = "Fairing";
            this.buttonFairing.UseVisualStyleBackColor = true;
            this.buttonFairing.Click += new System.EventHandler(this.buttonFairing_Click_1);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(579, 335);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Trivial Connection";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.NnumericUpDown);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.buttonProcess);
            this.splitContainer1.Panel1.Controls.Add(this.numericUpDownAngle);
            this.splitContainer1.Panel1.Controls.Add(this.buttonRefresh);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridViewSingularity);
            this.splitContainer1.Size = new System.Drawing.Size(573, 329);
            this.splitContainer1.SplitterDistance = 191;
            this.splitContainer1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "N:";
            // 
            // NnumericUpDown
            // 
            this.NnumericUpDown.Location = new System.Drawing.Point(83, 180);
            this.NnumericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NnumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NnumericUpDown.Name = "NnumericUpDown";
            this.NnumericUpDown.Size = new System.Drawing.Size(75, 20);
            this.NnumericUpDown.TabIndex = 7;
            this.NnumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NnumericUpDown.ValueChanged += new System.EventHandler(this.NnumericUpDown_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 218);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Init angle:";
            // 
            // buttonProcess
            // 
            this.buttonProcess.Location = new System.Drawing.Point(70, 283);
            this.buttonProcess.Name = "buttonProcess";
            this.buttonProcess.Size = new System.Drawing.Size(75, 23);
            this.buttonProcess.TabIndex = 3;
            this.buttonProcess.Text = "Process";
            this.buttonProcess.UseVisualStyleBackColor = true;
            this.buttonProcess.Click += new System.EventHandler(this.buttonProcess_Click);
            // 
            // numericUpDownAngle
            // 
            this.numericUpDownAngle.Location = new System.Drawing.Point(83, 216);
            this.numericUpDownAngle.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericUpDownAngle.Name = "numericUpDownAngle";
            this.numericUpDownAngle.Size = new System.Drawing.Size(75, 20);
            this.numericUpDownAngle.TabIndex = 2;
            this.numericUpDownAngle.ValueChanged += new System.EventHandler(this.NnumericUpDown_ValueChanged);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(70, 254);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 1;
            this.buttonRefresh.Text = "Refersh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // dataGridViewSingularity
            // 
            this.dataGridViewSingularity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSingularity.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.vertexColumn,
            this.valueColumn});
            this.dataGridViewSingularity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSingularity.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewSingularity.Name = "dataGridViewSingularity";
            this.dataGridViewSingularity.Size = new System.Drawing.Size(378, 329);
            this.dataGridViewSingularity.TabIndex = 1;
            this.dataGridViewSingularity.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSingularity_CellEndEdit);
            // 
            // vertexColumn
            // 
            this.vertexColumn.HeaderText = "Vertex";
            this.vertexColumn.Name = "vertexColumn";
            this.vertexColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // valueColumn
            // 
            this.valueColumn.HeaderText = "Values";
            this.valueColumn.Name = "valueColumn";
            this.valueColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tabPageTreeCotree
            // 
            this.tabPageTreeCotree.Controls.Add(this.button1);
            this.tabPageTreeCotree.Controls.Add(this.buttonGenerate);
            this.tabPageTreeCotree.Controls.Add(this.buttonTreeCotree);
            this.tabPageTreeCotree.Location = new System.Drawing.Point(4, 22);
            this.tabPageTreeCotree.Name = "tabPageTreeCotree";
            this.tabPageTreeCotree.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTreeCotree.Size = new System.Drawing.Size(579, 335);
            this.tabPageTreeCotree.TabIndex = 2;
            this.tabPageTreeCotree.Text = "Tree-Cotree";
            this.tabPageTreeCotree.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 64);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Harmonic Basis";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(8, 35);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(121, 23);
            this.buttonGenerate.TabIndex = 1;
            this.buttonGenerate.Text = "Draw Generator";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // buttonTreeCotree
            // 
            this.buttonTreeCotree.Location = new System.Drawing.Point(8, 6);
            this.buttonTreeCotree.Name = "buttonTreeCotree";
            this.buttonTreeCotree.Size = new System.Drawing.Size(121, 23);
            this.buttonTreeCotree.TabIndex = 0;
            this.buttonTreeCotree.Text = "Draw Tree-Cotree";
            this.buttonTreeCotree.UseVisualStyleBackColor = true;
            this.buttonTreeCotree.Click += new System.EventHandler(this.buttonTreeCotree_Click);
            // 
            // FormDEC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 361);
            this.Controls.Add(this.tabControlDEC);
            this.Name = "FormDEC";
            this.Text = "FormDec";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDec_FormClosing);
            this.tabControlDEC.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NnumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSingularity)).EndInit();
            this.tabPageTreeCotree.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlDEC;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button buttonTrivial;
        private System.Windows.Forms.Button buttonGeo;
        private System.Windows.Forms.Button buttonSpinForm;
        private System.Windows.Forms.Button buttonDecompostion;
        private System.Windows.Forms.Button buttonGenerator;
        private System.Windows.Forms.Button buttonFlattern;
        private System.Windows.Forms.Button buttonFairing;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonProcess;
        private System.Windows.Forms.NumericUpDown numericUpDownAngle;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.DataGridView dataGridViewSingularity;
        private System.Windows.Forms.DataGridViewTextBoxColumn vertexColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueColumn;
        private System.Windows.Forms.TabPage tabPageTreeCotree;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.Button buttonTreeCotree;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NnumericUpDown;

    }
}