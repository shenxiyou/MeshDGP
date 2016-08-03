namespace GraphicResearchHuiZhao
{
    partial class FormColorVis
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.propertyGridCriteria = new System.Windows.Forms.PropertyGrid();
            this.propertyGridRange = new System.Windows.Forms.PropertyGrid();
            this.dataGridViewColor = new System.Windows.Forms.DataGridView();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControlColor = new System.Windows.Forms.TabControl();
            this.tabPage1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewColor)).BeginInit();
            this.tabControlColor.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(787, 605);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "ColorByCriteria(Face,Edge,Vertex)";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridViewColor);
            this.splitContainer1.Size = new System.Drawing.Size(781, 599);
            this.splitContainer1.SplitterDistance = 311;
            this.splitContainer1.TabIndex = 4;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.propertyGridCriteria);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.propertyGridRange);
            this.splitContainer3.Size = new System.Drawing.Size(311, 599);
            this.splitContainer3.SplitterDistance = 460;
            this.splitContainer3.TabIndex = 0;
            // 
            // propertyGridCriteria
            // 
            this.propertyGridCriteria.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridCriteria.Location = new System.Drawing.Point(0, 0);
            this.propertyGridCriteria.Name = "propertyGridCriteria";
            this.propertyGridCriteria.Size = new System.Drawing.Size(311, 460);
            this.propertyGridCriteria.TabIndex = 2;
            this.propertyGridCriteria.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridCriteria_PropertyValueChanged);
            // 
            // propertyGridRange
            // 
            this.propertyGridRange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridRange.Location = new System.Drawing.Point(0, 0);
            this.propertyGridRange.Name = "propertyGridRange";
            this.propertyGridRange.Size = new System.Drawing.Size(311, 135);
            this.propertyGridRange.TabIndex = 0;
            this.propertyGridRange.ToolbarVisible = false;
            this.propertyGridRange.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridRange_PropertyValueChanged);
            // 
            // dataGridViewColor
            // 
            this.dataGridViewColor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewColor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Type,
            this.Index,
            this.Value});
            this.dataGridViewColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewColor.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewColor.Name = "dataGridViewColor";
            this.dataGridViewColor.Size = new System.Drawing.Size(466, 599);
            this.dataGridViewColor.TabIndex = 0;
            // 
            // Type
            // 
            this.Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            // 
            // Index
            // 
            this.Index.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Index.HeaderText = "Index";
            this.Index.Name = "Index";
            // 
            // Value
            // 
            this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            // 
            // tabControlColor
            // 
            this.tabControlColor.Controls.Add(this.tabPage1);
            this.tabControlColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlColor.Location = new System.Drawing.Point(0, 0);
            this.tabControlColor.Name = "tabControlColor";
            this.tabControlColor.SelectedIndex = 0;
            this.tabControlColor.Size = new System.Drawing.Size(795, 631);
            this.tabControlColor.TabIndex = 0;
            // 
            // FormColorVis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 631);
            this.Controls.Add(this.tabControlColor);
            this.Name = "FormColorVis";
            this.Text = "FormColorVis";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormColorVis_FormClosing);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewColor)).EndInit();
            this.tabControlColor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.PropertyGrid propertyGridCriteria;
        private System.Windows.Forms.PropertyGrid propertyGridRange;
        private System.Windows.Forms.DataGridView dataGridViewColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.TabControl tabControlColor;



    }
}