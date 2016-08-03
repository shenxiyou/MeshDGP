namespace GraphicResearchHuiZhao
{
    partial class MenuRendering
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
            this.renderingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subSurfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.participatingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.integratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sensorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.volumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.emitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renderingToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(150, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // renderingToolStripMenuItem
            // 
            this.renderingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configToolStripMenuItem,
            this.subSurfaceToolStripMenuItem,
            this.participatingToolStripMenuItem,
            this.integratorToolStripMenuItem,
            this.filmToolStripMenuItem,
            this.sensorToolStripMenuItem,
            this.volumeToolStripMenuItem,
            this.emitterToolStripMenuItem});
            this.renderingToolStripMenuItem.Name = "renderingToolStripMenuItem";
            this.renderingToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.renderingToolStripMenuItem.Text = "Rendering";
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.configToolStripMenuItem.Text = "Config";
            this.configToolStripMenuItem.Click += new System.EventHandler(this.configToolStripMenuItem_Click);
            // 
            // subSurfaceToolStripMenuItem
            // 
            this.subSurfaceToolStripMenuItem.Name = "subSurfaceToolStripMenuItem";
            this.subSurfaceToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.subSurfaceToolStripMenuItem.Text = "SubSurface";
            // 
            // participatingToolStripMenuItem
            // 
            this.participatingToolStripMenuItem.Name = "participatingToolStripMenuItem";
            this.participatingToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.participatingToolStripMenuItem.Text = "Participating";
            // 
            // integratorToolStripMenuItem
            // 
            this.integratorToolStripMenuItem.Name = "integratorToolStripMenuItem";
            this.integratorToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.integratorToolStripMenuItem.Text = "Integrator";
            // 
            // filmToolStripMenuItem
            // 
            this.filmToolStripMenuItem.Name = "filmToolStripMenuItem";
            this.filmToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.filmToolStripMenuItem.Text = "Film";
            // 
            // sensorToolStripMenuItem
            // 
            this.sensorToolStripMenuItem.Name = "sensorToolStripMenuItem";
            this.sensorToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.sensorToolStripMenuItem.Text = "Sensor";
            // 
            // volumeToolStripMenuItem
            // 
            this.volumeToolStripMenuItem.Name = "volumeToolStripMenuItem";
            this.volumeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.volumeToolStripMenuItem.Text = "Volume";
            // 
            // emitterToolStripMenuItem
            // 
            this.emitterToolStripMenuItem.Name = "emitterToolStripMenuItem";
            this.emitterToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.emitterToolStripMenuItem.Text = "Emitter";
            // 
            // MenuRendering
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.menuStrip1);
            this.Name = "MenuRendering";
            this.Size = new System.Drawing.Size(150, 36);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem renderingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem subSurfaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem participatingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem integratorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sensorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem volumeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem emitterToolStripMenuItem;
    }
}
