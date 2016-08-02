namespace GraphicResearchHuiZhao
{
    partial class FormCurve
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
            this.tabControlCurve = new System.Windows.Forms.TabControl();
            this.tabPageOne = new System.Windows.Forms.TabPage();
            this.labelName = new System.Windows.Forms.Label();
            this.comboBoxCurve = new System.Windows.Forms.ComboBox();
            this.tabPageBeizer = new System.Windows.Forms.TabPage();
            this.buttonView = new System.Windows.Forms.Button();
            this.buttonSP = new System.Windows.Forms.Button();
            this.comboBoxCurveComplexType = new System.Windows.Forms.ComboBox();
            this.tabPageConfig = new System.Windows.Forms.TabPage();
            this.textBoxVertexNum = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.hScrollBarNum = new System.Windows.Forms.HScrollBar();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridViewControlPoint = new System.Windows.Forms.DataGridView();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.X = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Z = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.tabControlCurve.SuspendLayout();
            this.tabPageOne.SuspendLayout();
            this.tabPageBeizer.SuspendLayout();
            this.tabPageConfig.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewControlPoint)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlCurve
            // 
            this.tabControlCurve.Controls.Add(this.tabPageOne);
            this.tabControlCurve.Controls.Add(this.tabPageBeizer);
            this.tabControlCurve.Controls.Add(this.tabPageConfig);
            this.tabControlCurve.Controls.Add(this.tabPage1);
            this.tabControlCurve.Controls.Add(this.tabPage2);
            this.tabControlCurve.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlCurve.Location = new System.Drawing.Point(0, 0);
            this.tabControlCurve.Name = "tabControlCurve";
            this.tabControlCurve.SelectedIndex = 0;
            this.tabControlCurve.Size = new System.Drawing.Size(503, 246);
            this.tabControlCurve.TabIndex = 0;
            // 
            // tabPageOne
            // 
            this.tabPageOne.Controls.Add(this.labelName);
            this.tabPageOne.Controls.Add(this.comboBoxCurve);
            this.tabPageOne.Location = new System.Drawing.Point(4, 22);
            this.tabPageOne.Name = "tabPageOne";
            this.tabPageOne.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOne.Size = new System.Drawing.Size(495, 220);
            this.tabPageOne.TabIndex = 0;
            this.tabPageOne.Text = "Simple";
            this.tabPageOne.UseVisualStyleBackColor = true;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(34, 32);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "Name";
            // 
            // comboBoxCurve
            // 
            this.comboBoxCurve.FormattingEnabled = true;
            this.comboBoxCurve.Location = new System.Drawing.Point(106, 32);
            this.comboBoxCurve.Name = "comboBoxCurve";
            this.comboBoxCurve.Size = new System.Drawing.Size(185, 21);
            this.comboBoxCurve.TabIndex = 0;
            this.comboBoxCurve.SelectedValueChanged += new System.EventHandler(this.comboBoxCurve_SelectedValueChanged);
            // 
            // tabPageBeizer
            // 
            this.tabPageBeizer.Controls.Add(this.buttonView);
            this.tabPageBeizer.Controls.Add(this.buttonSP);
            this.tabPageBeizer.Controls.Add(this.comboBoxCurveComplexType);
            this.tabPageBeizer.Location = new System.Drawing.Point(4, 22);
            this.tabPageBeizer.Name = "tabPageBeizer";
            this.tabPageBeizer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBeizer.Size = new System.Drawing.Size(495, 220);
            this.tabPageBeizer.TabIndex = 1;
            this.tabPageBeizer.Text = "Complex";
            this.tabPageBeizer.UseVisualStyleBackColor = true;
            // 
            // buttonView
            // 
            this.buttonView.Location = new System.Drawing.Point(210, 97);
            this.buttonView.Name = "buttonView";
            this.buttonView.Size = new System.Drawing.Size(75, 23);
            this.buttonView.TabIndex = 4;
            this.buttonView.Text = "View";
            this.buttonView.UseVisualStyleBackColor = true;
            this.buttonView.Click += new System.EventHandler(this.buttonView_Click);
            // 
            // buttonSP
            // 
            this.buttonSP.Location = new System.Drawing.Point(24, 97);
            this.buttonSP.Name = "buttonSP";
            this.buttonSP.Size = new System.Drawing.Size(130, 23);
            this.buttonSP.TabIndex = 3;
            this.buttonSP.Text = "Select Point";
            this.buttonSP.UseVisualStyleBackColor = true;
            this.buttonSP.Click += new System.EventHandler(this.buttonSP_Click);
            // 
            // comboBoxCurveComplexType
            // 
            this.comboBoxCurveComplexType.FormattingEnabled = true;
            this.comboBoxCurveComplexType.Location = new System.Drawing.Point(24, 32);
            this.comboBoxCurveComplexType.Name = "comboBoxCurveComplexType";
            this.comboBoxCurveComplexType.Size = new System.Drawing.Size(251, 21);
            this.comboBoxCurveComplexType.TabIndex = 2;
            this.comboBoxCurveComplexType.SelectedValueChanged += new System.EventHandler(this.comboBoxCurveComplexType_SelectedValueChanged);
            // 
            // tabPageConfig
            // 
            this.tabPageConfig.Controls.Add(this.buttonRefresh);
            this.tabPageConfig.Controls.Add(this.textBoxVertexNum);
            this.tabPageConfig.Controls.Add(this.buttonOK);
            this.tabPageConfig.Controls.Add(this.hScrollBarNum);
            this.tabPageConfig.Location = new System.Drawing.Point(4, 22);
            this.tabPageConfig.Name = "tabPageConfig";
            this.tabPageConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConfig.Size = new System.Drawing.Size(495, 220);
            this.tabPageConfig.TabIndex = 2;
            this.tabPageConfig.Text = "Config";
            this.tabPageConfig.UseVisualStyleBackColor = true;
            // 
            // textBoxVertexNum
            // 
            this.textBoxVertexNum.Location = new System.Drawing.Point(35, 78);
            this.textBoxVertexNum.Name = "textBoxVertexNum";
            this.textBoxVertexNum.Size = new System.Drawing.Size(100, 20);
            this.textBoxVertexNum.TabIndex = 2;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(35, 115);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // hScrollBarNum
            // 
            this.hScrollBarNum.Location = new System.Drawing.Point(35, 34);
            this.hScrollBarNum.Maximum = 20000;
            this.hScrollBarNum.Minimum = 10;
            this.hScrollBarNum.Name = "hScrollBarNum";
            this.hScrollBarNum.Size = new System.Drawing.Size(396, 17);
            this.hScrollBarNum.TabIndex = 0;
            this.hScrollBarNum.Value = 10;
            this.hScrollBarNum.ValueChanged += new System.EventHandler(this.hScrollBarNum_ValueChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridViewControlPoint);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(495, 220);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "ControlPoint";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridViewControlPoint
            // 
            this.dataGridViewControlPoint.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewControlPoint.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.X,
            this.Y,
            this.Z});
            this.dataGridViewControlPoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewControlPoint.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewControlPoint.Name = "dataGridViewControlPoint";
            this.dataGridViewControlPoint.Size = new System.Drawing.Size(489, 214);
            this.dataGridViewControlPoint.TabIndex = 0;
            // 
            // Index
            // 
            this.Index.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Index.HeaderText = "Index";
            this.Index.Name = "Index";
            // 
            // X
            // 
            this.X.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.X.HeaderText = "X";
            this.X.Name = "X";
            // 
            // Y
            // 
            this.Y.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Y.HeaderText = "Y";
            this.Y.Name = "Y";
            // 
            // Z
            // 
            this.Z.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Z.HeaderText = "Z";
            this.Z.Name = "Z";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.propertyGrid);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(495, 220);
            this.tabPage2.TabIndex = 4;
            this.tabPage2.Text = "Config";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(489, 214);
            this.propertyGrid.TabIndex = 0;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(224, 114);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(176, 23);
            this.buttonRefresh.TabIndex = 3;
            this.buttonRefresh.Text = "Refresh Control Point";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // FormCurve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 246);
            this.Controls.Add(this.tabControlCurve);
            this.Name = "FormCurve";
            this.Text = "FormCurve";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCurve_FormClosing);
            this.tabControlCurve.ResumeLayout(false);
            this.tabPageOne.ResumeLayout(false);
            this.tabPageOne.PerformLayout();
            this.tabPageBeizer.ResumeLayout(false);
            this.tabPageConfig.ResumeLayout(false);
            this.tabPageConfig.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewControlPoint)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlCurve;
        private System.Windows.Forms.TabPage tabPageOne;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.ComboBox comboBoxCurve;
        private System.Windows.Forms.TabPage tabPageBeizer;
        private System.Windows.Forms.TabPage tabPageConfig;
        private System.Windows.Forms.TextBox textBoxVertexNum;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.HScrollBar hScrollBarNum;
        private System.Windows.Forms.ComboBox comboBoxCurveComplexType;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridViewControlPoint;
        private System.Windows.Forms.Button buttonView;
        private System.Windows.Forms.Button buttonSP;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn X;
        private System.Windows.Forms.DataGridViewTextBoxColumn Y;
        private System.Windows.Forms.DataGridViewTextBoxColumn Z;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button buttonRefresh;
    }
}