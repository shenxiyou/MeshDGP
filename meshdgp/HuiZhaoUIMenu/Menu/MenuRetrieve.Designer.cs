namespace GraphicResearchHuiZhao
{
    partial class MenuRetrieve
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
            this.retrieveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBoxInput = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripComboBoxOutPut = new System.Windows.Forms.ToolStripComboBox();
            this.boundaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oneRingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oneRingSingleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.retrieveToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(102, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // retrieveToolStripMenuItem
            // 
            this.retrieveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBoxInput,
            this.toolStripComboBoxOutPut,
            this.boundaryToolStripMenuItem,
            this.oneRingToolStripMenuItem,
            this.oneRingSingleToolStripMenuItem});
            this.retrieveToolStripMenuItem.Name = "retrieveToolStripMenuItem";
            this.retrieveToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.retrieveToolStripMenuItem.Text = "Retrieve";
            // 
            // toolStripComboBoxInput
            // 
            this.toolStripComboBoxInput.Name = "toolStripComboBoxInput";
            this.toolStripComboBoxInput.Size = new System.Drawing.Size(121, 21);
            // 
            // toolStripComboBoxOutPut
            // 
            this.toolStripComboBoxOutPut.Name = "toolStripComboBoxOutPut";
            this.toolStripComboBoxOutPut.Size = new System.Drawing.Size(121, 21);
            // 
            // boundaryToolStripMenuItem
            // 
            this.boundaryToolStripMenuItem.Name = "boundaryToolStripMenuItem";
            this.boundaryToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.boundaryToolStripMenuItem.Text = "Boundary Patch";
            // 
            // oneRingToolStripMenuItem
            // 
            this.oneRingToolStripMenuItem.Name = "oneRingToolStripMenuItem";
            this.oneRingToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.oneRingToolStripMenuItem.Text = "One Ring Patch";
            // 
            // oneRingSingleToolStripMenuItem
            // 
            this.oneRingSingleToolStripMenuItem.Name = "oneRingSingleToolStripMenuItem";
            this.oneRingSingleToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.oneRingSingleToolStripMenuItem.Text = "One Ring Single";
            // 
            // MenuRetrieve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.menuStrip1);
            this.Name = "MenuRetrieve";
            this.Size = new System.Drawing.Size(102, 36);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem retrieveToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxInput;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxOutPut;
        private System.Windows.Forms.ToolStripMenuItem boundaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oneRingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oneRingSingleToolStripMenuItem;
    }
}
