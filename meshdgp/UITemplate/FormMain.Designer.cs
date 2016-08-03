namespace GraphicResearchHuiZhao
{
    partial class FormMain
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.barStatus1 = new GraphicResearchHuiZhao.BarStatus();
            this.menuTemplate1 = new GraphicResearchHuiZhao.MenuTemplate();
            this.openGLViewer1 = new GraphicResearchHuiZhao.OpenGLViewer();
            this.toolBarUI1 = new GraphicResearchHuiZhao.ToolBarUI();
            this.SuspendLayout();
            // 
            // barStatus1
            // 
            this.barStatus1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barStatus1.Location = new System.Drawing.Point(0, 607);
            this.barStatus1.Name = "barStatus1";
            this.barStatus1.Size = new System.Drawing.Size(1259, 18);
            this.barStatus1.TabIndex = 0;
            // 
            // menuTemplate1
            // 
            this.menuTemplate1.Dock = System.Windows.Forms.DockStyle.Top;
            this.menuTemplate1.Location = new System.Drawing.Point(0, 0);
            this.menuTemplate1.Name = "menuTemplate1";
            this.menuTemplate1.Size = new System.Drawing.Size(1259, 41);
            this.menuTemplate1.TabIndex = 2;
            // 
            // openGLViewer1
            // 
            this.openGLViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.openGLViewer1.Location = new System.Drawing.Point(0, 41);
            this.openGLViewer1.Name = "openGLViewer1";
            this.openGLViewer1.Size = new System.Drawing.Size(1259, 566);
            this.openGLViewer1.TabIndex = 3;
            // 
            // toolBarUI1
            // 
            this.toolBarUI1.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolBarUI1.Location = new System.Drawing.Point(0, 41);
            this.toolBarUI1.Name = "toolBarUI1";
            this.toolBarUI1.Size = new System.Drawing.Size(1259, 25);
            this.toolBarUI1.TabIndex = 4;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1259, 625);
            this.Controls.Add(this.toolBarUI1);
            this.Controls.Add(this.openGLViewer1);
            this.Controls.Add(this.menuTemplate1);
            this.Controls.Add(this.barStatus1);
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.ResumeLayout(false);

        }

        #endregion

        private GraphicResearchHuiZhao.BarStatus barStatus1;
        private GraphicResearchHuiZhao.MenuTemplate menuTemplate1;
        private GraphicResearchHuiZhao.OpenGLViewer openGLViewer1;
        private GraphicResearchHuiZhao.ToolBarUI toolBarUI1;
    }
}

