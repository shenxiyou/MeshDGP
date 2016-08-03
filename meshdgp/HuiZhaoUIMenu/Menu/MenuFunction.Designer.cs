namespace GraphicResearchHuiZhao
{
    partial class MenuFunction
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
            this.functionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.morseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawToMinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawToMaxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawMinMaxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showMinMaxSaddleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.baseDomain1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.functionToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(75, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // functionToolStripMenuItem
            // 
            this.functionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configToolStripMenuItem,
            this.morseToolStripMenuItem});
            this.functionToolStripMenuItem.Name = "functionToolStripMenuItem";
            this.functionToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.functionToolStripMenuItem.Text = "Function";
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.configToolStripMenuItem.Text = "Config";
            this.configToolStripMenuItem.Click += new System.EventHandler(this.configToolStripMenuItem_Click);
            // 
            // morseToolStripMenuItem
            // 
            this.morseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drawToMinToolStripMenuItem,
            this.drawToMaxToolStripMenuItem,
            this.drawMinMaxToolStripMenuItem,
            this.showMinMaxSaddleToolStripMenuItem,
            this.baseDomain1ToolStripMenuItem});
            this.morseToolStripMenuItem.Name = "morseToolStripMenuItem";
            this.morseToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.morseToolStripMenuItem.Text = "Morse";
            // 
            // drawToMinToolStripMenuItem
            // 
            this.drawToMinToolStripMenuItem.Name = "drawToMinToolStripMenuItem";
            this.drawToMinToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.drawToMinToolStripMenuItem.Text = "DrawToMin";
            this.drawToMinToolStripMenuItem.Click += new System.EventHandler(this.drawToMinToolStripMenuItem_Click);
            // 
            // drawToMaxToolStripMenuItem
            // 
            this.drawToMaxToolStripMenuItem.Name = "drawToMaxToolStripMenuItem";
            this.drawToMaxToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.drawToMaxToolStripMenuItem.Text = "DrawToMax";
            this.drawToMaxToolStripMenuItem.Click += new System.EventHandler(this.drawToMaxToolStripMenuItem_Click);
            // 
            // drawMinMaxToolStripMenuItem
            // 
            this.drawMinMaxToolStripMenuItem.Name = "drawMinMaxToolStripMenuItem";
            this.drawMinMaxToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.drawMinMaxToolStripMenuItem.Text = "Draw Min Max";
            this.drawMinMaxToolStripMenuItem.Click += new System.EventHandler(this.drawMinMaxToolStripMenuItem_Click);
            // 
            // showMinMaxSaddleToolStripMenuItem
            // 
            this.showMinMaxSaddleToolStripMenuItem.Name = "showMinMaxSaddleToolStripMenuItem";
            this.showMinMaxSaddleToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.showMinMaxSaddleToolStripMenuItem.Text = "Show Min Max Saddle";
            this.showMinMaxSaddleToolStripMenuItem.Click += new System.EventHandler(this.showMinMaxSaddleToolStripMenuItem_Click);
            // 
            // baseDomain1ToolStripMenuItem
            // 
            this.baseDomain1ToolStripMenuItem.Name = "baseDomain1ToolStripMenuItem";
            this.baseDomain1ToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.baseDomain1ToolStripMenuItem.Text = "Morse Simplify";
            this.baseDomain1ToolStripMenuItem.Click += new System.EventHandler(this.baseDomain1ToolStripMenuItem_Click);
            // 
            // MenuFunction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.menuStrip1);
            this.Name = "MenuFunction";
            this.Size = new System.Drawing.Size(75, 25);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem functionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem morseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem drawToMinToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem drawToMaxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem drawMinMaxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showMinMaxSaddleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem baseDomain1ToolStripMenuItem;
    }
}
