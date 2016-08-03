namespace GraphicResearchHuiZhao
{
    partial class MenuOpenGL
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
            this.menuStripOpenGl = new System.Windows.Forms.MenuStrip();
            this.openGLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openGLInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openGLMatrixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorVisulizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripOpenGl.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripOpenGl
            // 
            this.menuStripOpenGl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openGLToolStripMenuItem});
            this.menuStripOpenGl.Location = new System.Drawing.Point(0, 0);
            this.menuStripOpenGl.Name = "menuStripOpenGl";
            this.menuStripOpenGl.Size = new System.Drawing.Size(67, 24);
            this.menuStripOpenGl.TabIndex = 0;
            this.menuStripOpenGl.Text = "menuStrip1";
            // 
            // openGLToolStripMenuItem
            // 
            this.openGLToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayModeToolStripMenuItem,
            this.openGLInfoToolStripMenuItem,
            this.openGLMatrixToolStripMenuItem,
            this.colorVisulizationToolStripMenuItem,
            this.saveScreenToolStripMenuItem});
            this.openGLToolStripMenuItem.Name = "openGLToolStripMenuItem";
            this.openGLToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.openGLToolStripMenuItem.Text = "OpenGL";
            // 
            // displayModeToolStripMenuItem
            // 
            this.displayModeToolStripMenuItem.Name = "displayModeToolStripMenuItem";
            this.displayModeToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.displayModeToolStripMenuItem.Text = "Display Mode";
            // 
            // openGLInfoToolStripMenuItem
            // 
            this.openGLInfoToolStripMenuItem.Name = "openGLInfoToolStripMenuItem";
            this.openGLInfoToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.openGLInfoToolStripMenuItem.Text = "OpenGL Info";
            this.openGLInfoToolStripMenuItem.Click += new System.EventHandler(this.openGLInfoToolStripMenuItem_Click);
            // 
            // openGLMatrixToolStripMenuItem
            // 
            this.openGLMatrixToolStripMenuItem.Name = "openGLMatrixToolStripMenuItem";
            this.openGLMatrixToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.openGLMatrixToolStripMenuItem.Text = "OpenGL Matrix";
            this.openGLMatrixToolStripMenuItem.Click += new System.EventHandler(this.openGLMatrixToolStripMenuItem_Click);
            // 
            // colorVisulizationToolStripMenuItem
            // 
            this.colorVisulizationToolStripMenuItem.Name = "colorVisulizationToolStripMenuItem";
            this.colorVisulizationToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.colorVisulizationToolStripMenuItem.Text = "Color Visulization";
            this.colorVisulizationToolStripMenuItem.Click += new System.EventHandler(this.colorVisulizationToolStripMenuItem_Click);
            // 
            // saveScreenToolStripMenuItem
            // 
            this.saveScreenToolStripMenuItem.Name = "saveScreenToolStripMenuItem";
            this.saveScreenToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.saveScreenToolStripMenuItem.Text = "Save Screen(Key S)";
            this.saveScreenToolStripMenuItem.Click += new System.EventHandler(this.saveScreenToolStripMenuItem_Click);
            // 
            // MenuOpenGL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.menuStripOpenGl);
            this.Name = "MenuOpenGL";
            this.Size = new System.Drawing.Size(67, 26);
            this.menuStripOpenGl.ResumeLayout(false);
            this.menuStripOpenGl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripOpenGl;
        private System.Windows.Forms.ToolStripMenuItem openGLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displayModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openGLInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openGLMatrixToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorVisulizationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveScreenToolStripMenuItem;
    }
}
