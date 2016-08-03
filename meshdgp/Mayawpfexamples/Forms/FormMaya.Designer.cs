namespace GraphicResearchHuiZhao
{
    partial class FormMaya
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mayaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainMaya1 = new GraphicResearchHuiZhao.Forms.MenuMainMaya();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mayaToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(873, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mayaToolStripMenuItem
            // 
            this.mayaToolStripMenuItem.Name = "mayaToolStripMenuItem";
            this.mayaToolStripMenuItem.Size = new System.Drawing.Size(52, 21);
            this.mayaToolStripMenuItem.Text = "Maya";
            // 
            // menuMainMaya1
            // 
            this.menuMainMaya1.Dock = System.Windows.Forms.DockStyle.Top;
            this.menuMainMaya1.Location = new System.Drawing.Point(0, 25);
            this.menuMainMaya1.Name = "menuMainMaya1";
            this.menuMainMaya1.Size = new System.Drawing.Size(873, 71);
            this.menuMainMaya1.TabIndex = 1;
            // 
            // FormMaya
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 366);
            this.Controls.Add(this.menuMainMaya1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMaya";
            this.Text = "FormMaya";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mayaToolStripMenuItem;
        private Forms.MenuMainMaya menuMainMaya1;
    }
}