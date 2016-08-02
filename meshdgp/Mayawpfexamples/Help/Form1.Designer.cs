namespace GraphicResearchHuiZhao
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.PropGrid = new System.Windows.Forms.PropertyGrid();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.containerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dagPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.specificToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // PropGrid
            // 
            resources.ApplyResources(this.PropGrid, "PropGrid");
            this.PropGrid.Name = "PropGrid";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.containerToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // containerToolStripMenuItem
            // 
            this.containerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dagPathToolStripMenuItem,
            this.nodeToolStripMenuItem,
            this.specificToolStripMenuItem});
            this.containerToolStripMenuItem.Name = "containerToolStripMenuItem";
            resources.ApplyResources(this.containerToolStripMenuItem, "containerToolStripMenuItem");
            // 
            // dagPathToolStripMenuItem
            // 
            this.dagPathToolStripMenuItem.Checked = true;
            this.dagPathToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dagPathToolStripMenuItem.Name = "dagPathToolStripMenuItem";
            resources.ApplyResources(this.dagPathToolStripMenuItem, "dagPathToolStripMenuItem");
            this.dagPathToolStripMenuItem.Click += new System.EventHandler(this.dagPathToolStripMenuItem_Click);
            // 
            // nodeToolStripMenuItem
            // 
            this.nodeToolStripMenuItem.Name = "nodeToolStripMenuItem";
            resources.ApplyResources(this.nodeToolStripMenuItem, "nodeToolStripMenuItem");
            this.nodeToolStripMenuItem.Click += new System.EventHandler(this.nodeToolStripMenuItem_Click);
            // 
            // specificToolStripMenuItem
            // 
            this.specificToolStripMenuItem.Name = "specificToolStripMenuItem";
            resources.ApplyResources(this.specificToolStripMenuItem, "specificToolStripMenuItem");
            this.specificToolStripMenuItem.Click += new System.EventHandler(this.specificToolStripMenuItem_Click);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PropGrid);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid PropGrid;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem containerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dagPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem specificToolStripMenuItem;
        private System.Windows.Forms.BindingSource bindingSource1;

    }
}