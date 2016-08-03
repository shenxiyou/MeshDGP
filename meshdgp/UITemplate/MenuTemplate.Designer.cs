namespace GraphicResearchHuiZhao
{
    partial class MenuTemplate
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
            this.menuFile1 = new GraphicResearchHuiZhao.MenuFile();
            this.menuMeshInfo1 = new GraphicResearchHuiZhao.MenuMeshInfo();
            this.menuTool1 = new GraphicResearchHuiZhao.MenuTool();
            this.SuspendLayout();
            // 
            // menuFile1
            // 
            this.menuFile1.Location = new System.Drawing.Point(14, 3);
            this.menuFile1.Name = "menuFile1";
            this.menuFile1.Size = new System.Drawing.Size(58, 23);
            this.menuFile1.TabIndex = 0;
            // 
            // menuMeshInfo1
            // 
            this.menuMeshInfo1.Location = new System.Drawing.Point(94, 4);
            this.menuMeshInfo1.Name = "menuMeshInfo1";
            this.menuMeshInfo1.Size = new System.Drawing.Size(107, 24);
            this.menuMeshInfo1.TabIndex = 1;
            // 
            // menuTool1
            // 
            this.menuTool1.Location = new System.Drawing.Point(227, 3);
            this.menuTool1.Name = "menuTool1";
            this.menuTool1.Size = new System.Drawing.Size(109, 37);
            this.menuTool1.TabIndex = 2;
            // 
            // MenuTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.menuTool1);
            this.Controls.Add(this.menuMeshInfo1);
            this.Controls.Add(this.menuFile1);
            this.Name = "MenuTemplate";
            this.Size = new System.Drawing.Size(621, 41);
            this.ResumeLayout(false);

        }

        #endregion

        private MenuFile menuFile1;
        private MenuMeshInfo menuMeshInfo1;
        private MenuTool menuTool1;
    }
}
