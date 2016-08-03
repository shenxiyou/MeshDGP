namespace GraphicResearchHuiZhao
{
    partial class MenuUIWorking
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.workingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dECToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.surfaceReconstructionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rayTracingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reconToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.workingToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(156, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // workingToolStripMenuItem
            // 
            this.workingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dECToolStripMenuItem,
            this.surfaceReconstructionToolStripMenuItem,
            this.rayTracingToolStripMenuItem,
            this.reconToolStripMenuItem});
            this.workingToolStripMenuItem.Name = "workingToolStripMenuItem";
            this.workingToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.workingToolStripMenuItem.Text = "Working";
            // 
            // dECToolStripMenuItem
            // 
            this.dECToolStripMenuItem.Name = "dECToolStripMenuItem";
            this.dECToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.dECToolStripMenuItem.Text = "DEC";
            this.dECToolStripMenuItem.Click += new System.EventHandler(this.dECToolStripMenuItem_Click);
            // 
            // surfaceReconstructionToolStripMenuItem
            // 
            this.surfaceReconstructionToolStripMenuItem.Name = "surfaceReconstructionToolStripMenuItem";
            this.surfaceReconstructionToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.surfaceReconstructionToolStripMenuItem.Text = "Surface Reconstruction";
            this.surfaceReconstructionToolStripMenuItem.Click += new System.EventHandler(this.surfaceReconstructionToolStripMenuItem_Click);
            // 
            // rayTracingToolStripMenuItem
            // 
            this.rayTracingToolStripMenuItem.Name = "rayTracingToolStripMenuItem";
            this.rayTracingToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.rayTracingToolStripMenuItem.Text = "RayTracing";
            this.rayTracingToolStripMenuItem.Click += new System.EventHandler(this.rayTracingToolStripMenuItem_Click);
            // 
            // reconToolStripMenuItem
            // 
            this.reconToolStripMenuItem.Name = "reconToolStripMenuItem";
            this.reconToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.reconToolStripMenuItem.Text = "Recon";
            this.reconToolStripMenuItem.Click += new System.EventHandler(this.reconToolStripMenuItem_Click);
            // 
            // MenuUIWorking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.menuStrip1);
            this.Name = "MenuUIWorking";
            this.Size = new System.Drawing.Size(156, 34);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem workingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dECToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem surfaceReconstructionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rayTracingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reconToolStripMenuItem;
    }
}
