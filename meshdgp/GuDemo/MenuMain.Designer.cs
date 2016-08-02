namespace GraphicResearchHuiZhao
{
    partial class MenuMain
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
            this.menuTool1 = new GraphicResearchHuiZhao.MenuTool();
            this.menuGu1 = new GraphicResearchHuiZhao.MenuGu();
            this.SuspendLayout();
            // 
            // menuFile1
            // 
            this.menuFile1.Location = new System.Drawing.Point(4, 4);
            this.menuFile1.Name = "menuFile1";
            this.menuFile1.Size = new System.Drawing.Size(64, 23);
            this.menuFile1.TabIndex = 0;
            // 
            // menuTool1
            // 
            this.menuTool1.Location = new System.Drawing.Point(86, 3);
            this.menuTool1.Name = "menuTool1";
            this.menuTool1.Size = new System.Drawing.Size(77, 24);
            this.menuTool1.TabIndex = 1;
            // 
            // menuGu1
            // 
            this.menuGu1.Location = new System.Drawing.Point(184, 4);
            this.menuGu1.Name = "menuGu1";
            this.menuGu1.Size = new System.Drawing.Size(122, 42);
            this.menuGu1.TabIndex = 2;
            // 
            // MenuMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.menuGu1);
            this.Controls.Add(this.menuTool1);
            this.Controls.Add(this.menuFile1);
            this.Name = "MenuMain";
            this.Size = new System.Drawing.Size(408, 46);
            this.ResumeLayout(false);

        }

        #endregion

        private MenuFile menuFile1;
        private MenuTool menuTool1;
        private MenuGu menuGu1;
    }
}
