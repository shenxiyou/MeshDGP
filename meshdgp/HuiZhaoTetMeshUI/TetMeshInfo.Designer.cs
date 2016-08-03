namespace GraphicResearchHuiZhao
{
    partial class TetMeshInfo
    {

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.infoPage = new System.Windows.Forms.TabPage();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.vertexPage = new System.Windows.Forms.TabPage();
            this.vertexView = new System.Windows.Forms.DataGridView();
            this.VIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VFlag = new DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn();
            this.VBoundary = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.edgePage = new System.Windows.Forms.TabPage();
            this.edgeView = new System.Windows.Forms.DataGridView();
            this.EIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EV0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EV1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EFlag = new DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn();
            this.EBoundary = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.facePage = new System.Windows.Forms.TabPage();
            this.faceView = new System.Windows.Forms.DataGridView();
            this.FIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FV0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FV1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FV2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FFlag = new DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn();
            this.FBoundary = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tetrahedronPage = new System.Windows.Forms.TabPage();
            this.tetrahedronView = new System.Windows.Forms.DataGridView();
            this.TIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TV0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TV1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TV2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TV3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TFlag = new DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn();
            this.TBoundary = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1.SuspendLayout();
            this.infoPage.SuspendLayout();
            this.vertexPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vertexView)).BeginInit();
            this.edgePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edgeView)).BeginInit();
            this.facePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.faceView)).BeginInit();
            this.tetrahedronPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tetrahedronView)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.infoPage);
            this.tabControl1.Controls.Add(this.vertexPage);
            this.tabControl1.Controls.Add(this.edgePage);
            this.tabControl1.Controls.Add(this.facePage);
            this.tabControl1.Controls.Add(this.tetrahedronPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(683, 451);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // infoPage
            // 
            this.infoPage.Controls.Add(this.propertyGrid1);
            this.infoPage.Location = new System.Drawing.Point(4, 22);
            this.infoPage.Name = "infoPage";
            this.infoPage.Padding = new System.Windows.Forms.Padding(3);
            this.infoPage.Size = new System.Drawing.Size(675, 425);
            this.infoPage.TabIndex = 0;
            this.infoPage.Text = "Info";
            this.infoPage.UseVisualStyleBackColor = true;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(669, 419);
            this.propertyGrid1.TabIndex = 0;
            // 
            // vertexPage
            // 
            this.vertexPage.Controls.Add(this.vertexView);
            this.vertexPage.Location = new System.Drawing.Point(4, 22);
            this.vertexPage.Name = "vertexPage";
            this.vertexPage.Padding = new System.Windows.Forms.Padding(3);
            this.vertexPage.Size = new System.Drawing.Size(675, 425);
            this.vertexPage.TabIndex = 1;
            this.vertexPage.Text = "Vertex";
            this.vertexPage.UseVisualStyleBackColor = true;
            // 
            // vertexView
            // 
            this.vertexView.AllowUserToAddRows = false;
            this.vertexView.AllowUserToDeleteRows = false;
            this.vertexView.AllowUserToResizeColumns = false;
            this.vertexView.AllowUserToResizeRows = false;
            this.vertexView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.vertexView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.vertexView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VIndex,
            this.VX,
            this.VY,
            this.VZ,
            this.VFlag,
            this.VBoundary});
            this.vertexView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vertexView.Location = new System.Drawing.Point(3, 3);
            this.vertexView.Name = "vertexView";
            this.vertexView.RowHeadersVisible = false;
            this.vertexView.RowTemplate.Height = 23;
            this.vertexView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.vertexView.Size = new System.Drawing.Size(669, 419);
            this.vertexView.TabIndex = 1;
            this.vertexView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.CellValueChanged);
            this.vertexView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.View_KeyDown);
            // 
            // VIndex
            // 
            this.VIndex.Frozen = true;
            this.VIndex.HeaderText = "Index";
            this.VIndex.Name = "VIndex";
            this.VIndex.ReadOnly = true;
            this.VIndex.Width = 58;
            // 
            // VX
            // 
            this.VX.HeaderText = "X";
            this.VX.Name = "VX";
            this.VX.ReadOnly = true;
            this.VX.Width = 39;
            // 
            // VY
            // 
            this.VY.HeaderText = "Y";
            this.VY.Name = "VY";
            this.VY.ReadOnly = true;
            this.VY.Width = 39;
            // 
            // VZ
            // 
            this.VZ.HeaderText = "Z";
            this.VZ.Name = "VZ";
            this.VZ.ReadOnly = true;
            this.VZ.Width = 39;
            // 
            // VFlag
            // 
            this.VFlag.HeaderText = "Flag";
            this.VFlag.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.VFlag.Name = "VFlag";
            this.VFlag.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.VFlag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.VFlag.Width = 52;
            // 
            // VBoundary
            // 
            this.VBoundary.HeaderText = "Boundary";
            this.VBoundary.Name = "VBoundary";
            this.VBoundary.ReadOnly = true;
            this.VBoundary.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.VBoundary.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.VBoundary.Width = 77;
            // 
            // edgePage
            // 
            this.edgePage.Controls.Add(this.edgeView);
            this.edgePage.Location = new System.Drawing.Point(4, 22);
            this.edgePage.Name = "edgePage";
            this.edgePage.Size = new System.Drawing.Size(675, 425);
            this.edgePage.TabIndex = 2;
            this.edgePage.Text = "Edge";
            this.edgePage.UseVisualStyleBackColor = true;
            // 
            // edgeView
            // 
            this.edgeView.AllowUserToAddRows = false;
            this.edgeView.AllowUserToDeleteRows = false;
            this.edgeView.AllowUserToResizeColumns = false;
            this.edgeView.AllowUserToResizeRows = false;
            this.edgeView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.edgeView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.edgeView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EIndex,
            this.EV0,
            this.EV1,
            this.EFlag,
            this.EBoundary});
            this.edgeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.edgeView.Location = new System.Drawing.Point(0, 0);
            this.edgeView.Name = "edgeView";
            this.edgeView.RowHeadersVisible = false;
            this.edgeView.RowTemplate.Height = 23;
            this.edgeView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.edgeView.Size = new System.Drawing.Size(675, 425);
            this.edgeView.TabIndex = 1;
            this.edgeView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.CellValueChanged);
            this.edgeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.View_KeyDown);
            // 
            // EIndex
            // 
            this.EIndex.Frozen = true;
            this.EIndex.HeaderText = "Index";
            this.EIndex.Name = "EIndex";
            this.EIndex.Width = 58;
            // 
            // EV0
            // 
            this.EV0.HeaderText = "V0";
            this.EV0.Name = "EV0";
            this.EV0.Width = 45;
            // 
            // EV1
            // 
            this.EV1.HeaderText = "V1";
            this.EV1.Name = "EV1";
            this.EV1.Width = 45;
            // 
            // EFlag
            // 
            this.EFlag.HeaderText = "Flag";
            this.EFlag.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.EFlag.Name = "EFlag";
            this.EFlag.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.EFlag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.EFlag.Width = 52;
            // 
            // EBoundary
            // 
            this.EBoundary.HeaderText = "Boundary";
            this.EBoundary.Name = "EBoundary";
            this.EBoundary.ReadOnly = true;
            this.EBoundary.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.EBoundary.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.EBoundary.Width = 77;
            // 
            // facePage
            // 
            this.facePage.Controls.Add(this.faceView);
            this.facePage.Location = new System.Drawing.Point(4, 22);
            this.facePage.Name = "facePage";
            this.facePage.Size = new System.Drawing.Size(675, 425);
            this.facePage.TabIndex = 3;
            this.facePage.Text = "Face";
            this.facePage.UseVisualStyleBackColor = true;
            // 
            // faceView
            // 
            this.faceView.AllowUserToAddRows = false;
            this.faceView.AllowUserToDeleteRows = false;
            this.faceView.AllowUserToResizeColumns = false;
            this.faceView.AllowUserToResizeRows = false;
            this.faceView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.faceView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.faceView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FIndex,
            this.FV0,
            this.FV1,
            this.FV2,
            this.FFlag,
            this.FBoundary});
            this.faceView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.faceView.Location = new System.Drawing.Point(0, 0);
            this.faceView.Name = "faceView";
            this.faceView.RowHeadersVisible = false;
            this.faceView.RowTemplate.Height = 23;
            this.faceView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.faceView.Size = new System.Drawing.Size(675, 425);
            this.faceView.TabIndex = 1;
            this.faceView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.CellValueChanged);
            this.faceView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.View_KeyDown);
            // 
            // FIndex
            // 
            this.FIndex.Frozen = true;
            this.FIndex.HeaderText = "Index";
            this.FIndex.Name = "FIndex";
            this.FIndex.ReadOnly = true;
            this.FIndex.Width = 58;
            // 
            // FV0
            // 
            this.FV0.HeaderText = "V0";
            this.FV0.Name = "FV0";
            this.FV0.ReadOnly = true;
            this.FV0.Width = 45;
            // 
            // FV1
            // 
            this.FV1.HeaderText = "V1";
            this.FV1.Name = "FV1";
            this.FV1.ReadOnly = true;
            this.FV1.Width = 45;
            // 
            // FV2
            // 
            this.FV2.HeaderText = "V2";
            this.FV2.Name = "FV2";
            this.FV2.ReadOnly = true;
            this.FV2.Width = 45;
            // 
            // FFlag
            // 
            this.FFlag.HeaderText = "Flag";
            this.FFlag.Name = "FFlag";
            this.FFlag.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.FFlag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.FFlag.Width = 52;
            // 
            // FBoundary
            // 
            this.FBoundary.HeaderText = "Boundary";
            this.FBoundary.Name = "FBoundary";
            this.FBoundary.ReadOnly = true;
            this.FBoundary.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.FBoundary.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.FBoundary.Width = 77;
            // 
            // tetrahedronPage
            // 
            this.tetrahedronPage.Controls.Add(this.tetrahedronView);
            this.tetrahedronPage.Location = new System.Drawing.Point(4, 22);
            this.tetrahedronPage.Name = "tetrahedronPage";
            this.tetrahedronPage.Size = new System.Drawing.Size(675, 425);
            this.tetrahedronPage.TabIndex = 4;
            this.tetrahedronPage.Text = "Tetrahedron";
            this.tetrahedronPage.UseVisualStyleBackColor = true;
            // 
            // tetrahedronView
            // 
            this.tetrahedronView.AllowUserToAddRows = false;
            this.tetrahedronView.AllowUserToDeleteRows = false;
            this.tetrahedronView.AllowUserToResizeColumns = false;
            this.tetrahedronView.AllowUserToResizeRows = false;
            this.tetrahedronView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.tetrahedronView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tetrahedronView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TIndex,
            this.TV0,
            this.TV1,
            this.TV2,
            this.TV3,
            this.TFlag,
            this.TBoundary});
            this.tetrahedronView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tetrahedronView.Location = new System.Drawing.Point(0, 0);
            this.tetrahedronView.Name = "tetrahedronView";
            this.tetrahedronView.RowHeadersVisible = false;
            this.tetrahedronView.RowTemplate.Height = 23;
            this.tetrahedronView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tetrahedronView.Size = new System.Drawing.Size(675, 425);
            this.tetrahedronView.TabIndex = 1;
            this.tetrahedronView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.CellValueChanged);
            this.tetrahedronView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.View_KeyDown);
            // 
            // TIndex
            // 
            this.TIndex.Frozen = true;
            this.TIndex.HeaderText = "Index";
            this.TIndex.Name = "TIndex";
            this.TIndex.ReadOnly = true;
            this.TIndex.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TIndex.Width = 58;
            // 
            // TV0
            // 
            this.TV0.HeaderText = "V0";
            this.TV0.Name = "TV0";
            this.TV0.ReadOnly = true;
            this.TV0.Width = 45;
            // 
            // TV1
            // 
            this.TV1.HeaderText = "V1";
            this.TV1.Name = "TV1";
            this.TV1.ReadOnly = true;
            this.TV1.Width = 45;
            // 
            // TV2
            // 
            this.TV2.HeaderText = "V2";
            this.TV2.Name = "TV2";
            this.TV2.ReadOnly = true;
            this.TV2.Width = 45;
            // 
            // TV3
            // 
            this.TV3.HeaderText = "V3";
            this.TV3.Name = "TV3";
            this.TV3.ReadOnly = true;
            this.TV3.Width = 45;
            // 
            // TFlag
            // 
            this.TFlag.HeaderText = "Flag";
            this.TFlag.Name = "TFlag";
            this.TFlag.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TFlag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.TFlag.Width = 52;
            // 
            // TBoundary
            // 
            this.TBoundary.HeaderText = "Boundary";
            this.TBoundary.Name = "TBoundary";
            this.TBoundary.ReadOnly = true;
            this.TBoundary.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.TBoundary.Width = 77;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 429);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(683, 22);
            this.statusStrip1.TabIndex = 1;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(171, 17);
            this.toolStripStatusLabel1.Text = "选中行后按左右方向键调整Flag";
            // 
            // TetMeshInfo
            // 
            this.ClientSize = new System.Drawing.Size(683, 451);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Name = "TetMeshInfo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TetMeshInfo_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.infoPage.ResumeLayout(false);
            this.vertexPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vertexView)).EndInit();
            this.edgePage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.edgeView)).EndInit();
            this.facePage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.faceView)).EndInit();
            this.tetrahedronPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tetrahedronView)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage infoPage;
        private System.Windows.Forms.TabPage vertexPage;
        private System.Windows.Forms.TabPage edgePage;
        private System.Windows.Forms.TabPage facePage;
        private System.Windows.Forms.TabPage tetrahedronPage;
        private System.Windows.Forms.DataGridView vertexView;
        private System.Windows.Forms.DataGridView edgeView;
        private System.Windows.Forms.DataGridView faceView;
        private System.Windows.Forms.DataGridView tetrahedronView;
        private System.Windows.Forms.DataGridViewTextBoxColumn EIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn EV0;
        private System.Windows.Forms.DataGridViewTextBoxColumn EV1;
        private DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn EFlag;
        private System.Windows.Forms.DataGridViewCheckBoxColumn EBoundary;
        private System.Windows.Forms.DataGridViewTextBoxColumn FIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn FV0;
        private System.Windows.Forms.DataGridViewTextBoxColumn FV1;
        private System.Windows.Forms.DataGridViewTextBoxColumn FV2;
        private DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn FFlag;
        private System.Windows.Forms.DataGridViewCheckBoxColumn FBoundary;
        private System.Windows.Forms.DataGridViewTextBoxColumn VIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn VX;
        private System.Windows.Forms.DataGridViewTextBoxColumn VY;
        private System.Windows.Forms.DataGridViewTextBoxColumn VZ;
        private DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn VFlag;
        private System.Windows.Forms.DataGridViewCheckBoxColumn VBoundary;
        private System.Windows.Forms.DataGridViewTextBoxColumn TIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn TV0;
        private System.Windows.Forms.DataGridViewTextBoxColumn TV1;
        private System.Windows.Forms.DataGridViewTextBoxColumn TV2;
        private System.Windows.Forms.DataGridViewTextBoxColumn TV3;
        private DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn TFlag;
        private System.Windows.Forms.DataGridViewCheckBoxColumn TBoundary;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}