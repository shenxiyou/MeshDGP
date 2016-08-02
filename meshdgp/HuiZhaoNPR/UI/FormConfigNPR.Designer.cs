namespace GraphicResearchHuiZhao
{
    partial class FormConfigNPR
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
            this.flowLine = new System.Windows.Forms.FlowLayoutPanel();
            this.basicGrid = new System.Windows.Forms.PropertyGrid();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.colorGrid = new System.Windows.Forms.PropertyGrid();
            this.tabPage1.SuspendLayout();
            //((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(506, 338);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Parameter Config";
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
            this.splitContainer1.Panel1.Controls.Add(this.flowLine);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.basicGrid);
            this.splitContainer1.Size = new System.Drawing.Size(500, 332);
            this.splitContainer1.SplitterDistance = 219;
            this.splitContainer1.TabIndex = 0;
            // 
            // flowLine
            // 
            this.flowLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLine.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLine.Location = new System.Drawing.Point(0, 0);
            this.flowLine.Name = "flowLine";
            this.flowLine.Size = new System.Drawing.Size(219, 332);
            this.flowLine.TabIndex = 1;
            // 
            // basicGrid
            // 
            this.basicGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicGrid.Location = new System.Drawing.Point(0, 0);
            this.basicGrid.Name = "basicGrid";
            this.basicGrid.Size = new System.Drawing.Size(277, 332);
            this.basicGrid.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(514, 364);
            this.tabControl.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.colorGrid);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(506, 338);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Color";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // colorGrid
            // 
            this.colorGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colorGrid.Location = new System.Drawing.Point(3, 3);
            this.colorGrid.Name = "colorGrid";
            this.colorGrid.Size = new System.Drawing.Size(500, 332);
            this.colorGrid.TabIndex = 0;
            // 
            // FormConfigNPR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 364);
            this.Controls.Add(this.tabControl);
            this.Name = "FormConfigNPR";
            this.Text = "FormConfigNPR";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormConfigNPR_FormClosing);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            //((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PropertyGrid basicGrid;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.FlowLayoutPanel flowLine;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PropertyGrid colorGrid;

    }
}