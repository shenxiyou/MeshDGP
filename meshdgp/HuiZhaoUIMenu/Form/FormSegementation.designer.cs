namespace GraphicResearchHuiZhao
{
    partial class FormSegementation
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageConfig = new System.Windows.Forms.TabPage();
            this.Seg = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonRegion = new System.Windows.Forms.Button();
            this.buttonKmeans = new System.Windows.Forms.Button();
            this.buttonNotSelected = new System.Windows.Forms.Button();
            this.buttonOneStepFace = new System.Windows.Forms.Button();
            this.buttonV2F = new System.Windows.Forms.Button();
            this.buttonView = new System.Windows.Forms.Button();
            this.buttonSelectVertex = new System.Windows.Forms.Button();
            this.buttonByFace = new System.Windows.Forms.Button();
            this.buttonByVertex = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.propertyGridConfig = new System.Windows.Forms.PropertyGrid();
            this.tabControl1.SuspendLayout();
            this.tabPageConfig.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageConfig);
            this.tabControl1.Controls.Add(this.Seg);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(710, 350);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageConfig
            // 
            this.tabPageConfig.Controls.Add(this.splitContainer1);
            this.tabPageConfig.Location = new System.Drawing.Point(4, 22);
            this.tabPageConfig.Name = "tabPageConfig";
            this.tabPageConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConfig.Size = new System.Drawing.Size(702, 324);
            this.tabPageConfig.TabIndex = 0;
            this.tabPageConfig.Text = "Config";
            this.tabPageConfig.UseVisualStyleBackColor = true;
            // 
            // Seg
            // 
            this.Seg.Location = new System.Drawing.Point(4, 22);
            this.Seg.Name = "Seg";
            this.Seg.Padding = new System.Windows.Forms.Padding(3);
            this.Seg.Size = new System.Drawing.Size(702, 324);
            this.Seg.TabIndex = 1;
            this.Seg.Text = "Seg";
            this.Seg.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.propertyGridConfig);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.buttonRegion);
            this.splitContainer1.Panel2.Controls.Add(this.buttonKmeans);
            this.splitContainer1.Panel2.Controls.Add(this.buttonNotSelected);
            this.splitContainer1.Panel2.Controls.Add(this.buttonOneStepFace);
            this.splitContainer1.Panel2.Controls.Add(this.buttonV2F);
            this.splitContainer1.Panel2.Controls.Add(this.buttonView);
            this.splitContainer1.Panel2.Controls.Add(this.buttonSelectVertex);
            this.splitContainer1.Panel2.Controls.Add(this.buttonByFace);
            this.splitContainer1.Panel2.Controls.Add(this.buttonByVertex);
            this.splitContainer1.Panel2.Controls.Add(this.buttonClear);
            this.splitContainer1.Size = new System.Drawing.Size(696, 318);
            this.splitContainer1.SplitterDistance = 232;
            this.splitContainer1.TabIndex = 0;
            // 
            // buttonRegion
            // 
            this.buttonRegion.Location = new System.Drawing.Point(168, 237);
            this.buttonRegion.Name = "buttonRegion";
            this.buttonRegion.Size = new System.Drawing.Size(75, 23);
            this.buttonRegion.TabIndex = 19;
            this.buttonRegion.Text = "Region";
            this.buttonRegion.UseVisualStyleBackColor = true;
            this.buttonRegion.Click += buttonRegion_Click;
            // 
            // buttonKmeans
            // 
            this.buttonKmeans.Location = new System.Drawing.Point(14, 237);
            this.buttonKmeans.Name = "buttonKmeans";
            this.buttonKmeans.Size = new System.Drawing.Size(75, 23);
            this.buttonKmeans.TabIndex = 18;
            this.buttonKmeans.Text = "KMeans";
            this.buttonKmeans.UseVisualStyleBackColor = true;
            this.buttonKmeans.Click+=buttonKmeans_Click;
            // 
            // buttonNotSelected
            // 
            this.buttonNotSelected.Location = new System.Drawing.Point(168, 191);
            this.buttonNotSelected.Name = "buttonNotSelected";
            this.buttonNotSelected.Size = new System.Drawing.Size(203, 23);
            this.buttonNotSelected.TabIndex = 17;
            this.buttonNotSelected.Text = "By Face Not Selected";
            this.buttonNotSelected.UseVisualStyleBackColor = true;
            this.buttonNotSelected.Click+=buttonNotSelected_Click;
            // 
            // buttonOneStepFace
            // 
            this.buttonOneStepFace.Location = new System.Drawing.Point(168, 149);
            this.buttonOneStepFace.Name = "buttonOneStepFace";
            this.buttonOneStepFace.Size = new System.Drawing.Size(149, 23);
            this.buttonOneStepFace.TabIndex = 16;
            this.buttonOneStepFace.Text = "One Step Face";
            this.buttonOneStepFace.UseVisualStyleBackColor = true;
            this.buttonOneStepFace.Click+=buttonOneStepFace_Click;
            // 
            // buttonV2F
            // 
            this.buttonV2F.Location = new System.Drawing.Point(168, 105);
            this.buttonV2F.Name = "buttonV2F";
            this.buttonV2F.Size = new System.Drawing.Size(149, 23);
            this.buttonV2F.TabIndex = 15;
            this.buttonV2F.Text = "Vertex To Face";
            this.buttonV2F.UseVisualStyleBackColor = true;
            this.buttonV2F.Click+=buttonV2F_Click;
            // 
            // buttonView
            // 
            this.buttonView.Location = new System.Drawing.Point(348, 58);
            this.buttonView.Name = "buttonView";
            this.buttonView.Size = new System.Drawing.Size(102, 23);
            this.buttonView.TabIndex = 14;
            this.buttonView.Text = "View";
            this.buttonView.UseVisualStyleBackColor = true;
            this.buttonView.Click+=buttonView_Click;
            // 
            // buttonSelectVertex
            // 
            this.buttonSelectVertex.Location = new System.Drawing.Point(168, 59);
            this.buttonSelectVertex.Name = "buttonSelectVertex";
            this.buttonSelectVertex.Size = new System.Drawing.Size(149, 23);
            this.buttonSelectVertex.TabIndex = 13;
            this.buttonSelectVertex.Text = "Select Vertex";
            this.buttonSelectVertex.UseVisualStyleBackColor = true;
            this.buttonSelectVertex.Click += buttonSelectVertex_Click;
            // 
            // buttonByFace
            // 
            this.buttonByFace.Location = new System.Drawing.Point(14, 149);
            this.buttonByFace.Name = "buttonByFace";
            this.buttonByFace.Size = new System.Drawing.Size(106, 23);
            this.buttonByFace.TabIndex = 12;
            this.buttonByFace.Text = "Auto Face";
            this.buttonByFace.UseVisualStyleBackColor = true;
            this.buttonByFace.Click+=buttonByFace_Click;
            // 
            // buttonByVertex
            // 
            this.buttonByVertex.Location = new System.Drawing.Point(14, 105);
            this.buttonByVertex.Name = "buttonByVertex";
            this.buttonByVertex.Size = new System.Drawing.Size(106, 23);
            this.buttonByVertex.TabIndex = 11;
            this.buttonByVertex.Text = "One Step Vertex";
            this.buttonByVertex.UseVisualStyleBackColor = true;
            this.buttonByVertex.Click+=buttonByVertex_Click;
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(14, 59);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 10;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click+=buttonClear_Click;
            // 
            // propertyGridConfig
            // 
            this.propertyGridConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridConfig.Location = new System.Drawing.Point(0, 0);
            this.propertyGridConfig.Name = "propertyGridConfig";
            this.propertyGridConfig.Size = new System.Drawing.Size(232, 318);
            this.propertyGridConfig.TabIndex = 0;
            // 
            // FormSegementation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 350);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormSegementation";
            this.Text = "FormSegementation";
            this.tabControl1.ResumeLayout(false);
            this.tabPageConfig.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageConfig;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PropertyGrid propertyGridConfig;
        private System.Windows.Forms.Button buttonRegion;
        private System.Windows.Forms.Button buttonKmeans;
        private System.Windows.Forms.Button buttonNotSelected;
        private System.Windows.Forms.Button buttonOneStepFace;
        private System.Windows.Forms.Button buttonV2F;
        private System.Windows.Forms.Button buttonView;
        private System.Windows.Forms.Button buttonSelectVertex;
        private System.Windows.Forms.Button buttonByFace;
        private System.Windows.Forms.Button buttonByVertex;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.TabPage Seg;

    }
}