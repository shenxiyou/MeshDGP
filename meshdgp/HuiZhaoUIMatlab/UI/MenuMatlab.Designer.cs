namespace GraphicResearchHuiZhao
{
    partial class MenuMatlab
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.matlabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.outLapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testEignToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.positiveDefiniteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.matlabToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(105, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // matlabToolStripMenuItem
            // 
            this.matlabToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configToolStripMenuItem,
            this.testToolStripMenuItem,
            this.testToolStripMenuItem1,
            this.outLapToolStripMenuItem,
            this.testEignToolStripMenuItem,
            this.positiveDefiniteToolStripMenuItem});
            this.matlabToolStripMenuItem.Name = "matlabToolStripMenuItem";
            this.matlabToolStripMenuItem.Size = new System.Drawing.Size(61, 21);
            this.matlabToolStripMenuItem.Text = "Matlab";
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.testToolStripMenuItem.Text = "Test";
            // 
            // testToolStripMenuItem1
            // 
            this.testToolStripMenuItem1.Name = "testToolStripMenuItem1";
            this.testToolStripMenuItem1.Size = new System.Drawing.Size(168, 22);
            this.testToolStripMenuItem1.Text = "GetLap";
            this.testToolStripMenuItem1.Click += new System.EventHandler(this.testToolStripMenuItem1_Click);
            // 
            // outLapToolStripMenuItem
            // 
            this.outLapToolStripMenuItem.Name = "outLapToolStripMenuItem";
            this.outLapToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.outLapToolStripMenuItem.Text = "OutLap";
            this.outLapToolStripMenuItem.Click += new System.EventHandler(this.outLapToolStripMenuItem_Click);
            // 
            // testEignToolStripMenuItem
            // 
            this.testEignToolStripMenuItem.Name = "testEignToolStripMenuItem";
            this.testEignToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.testEignToolStripMenuItem.Text = "TestEign";
            this.testEignToolStripMenuItem.Click += new System.EventHandler(this.testEignToolStripMenuItem_Click);
            // 
            // positiveDefiniteToolStripMenuItem
            // 
            this.positiveDefiniteToolStripMenuItem.Name = "positiveDefiniteToolStripMenuItem";
            this.positiveDefiniteToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.positiveDefiniteToolStripMenuItem.Text = "Positive Definite";
            this.positiveDefiniteToolStripMenuItem.Click += new System.EventHandler(this.positiveDefiniteToolStripMenuItem_Click);
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.configToolStripMenuItem.Text = "Config";
            this.configToolStripMenuItem.Click += new System.EventHandler(this.configToolStripMenuItem_Click);
            // 
            // MenuMatlab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.menuStrip1);
            this.Name = "MenuMatlab";
            this.Size = new System.Drawing.Size(105, 30);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem matlabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem outLapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testEignToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem positiveDefiniteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
    }
}
