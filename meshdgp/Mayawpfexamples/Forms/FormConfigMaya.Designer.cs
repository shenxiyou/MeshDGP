namespace GraphicResearchHuiZhao
{
    partial class FormConfigMaya
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
            this.tabControlCage = new System.Windows.Forms.TabControl();
            this.Config = new System.Windows.Forms.TabPage();
            this.propertyGridConfig = new System.Windows.Forms.PropertyGrid();
            this.Basic = new System.Windows.Forms.TabPage();
            this.Image = new System.Windows.Forms.TabPage();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.DataGrid = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabControlCage.SuspendLayout();
            this.Config.SuspendLayout();
            this.Basic.SuspendLayout();
            this.Image.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.DataGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlCage
            // 
            this.tabControlCage.Controls.Add(this.Config);
            this.tabControlCage.Controls.Add(this.Basic);
            this.tabControlCage.Controls.Add(this.Image);
            this.tabControlCage.Controls.Add(this.DataGrid);
            this.tabControlCage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlCage.Location = new System.Drawing.Point(0, 0);
            this.tabControlCage.Name = "tabControlCage";
            this.tabControlCage.SelectedIndex = 0;
            this.tabControlCage.Size = new System.Drawing.Size(754, 539);
            this.tabControlCage.TabIndex = 0;
            // 
            // Config
            // 
            this.Config.Controls.Add(this.propertyGridConfig);
            this.Config.Location = new System.Drawing.Point(4, 22);
            this.Config.Name = "Config";
            this.Config.Padding = new System.Windows.Forms.Padding(3);
            this.Config.Size = new System.Drawing.Size(746, 513);
            this.Config.TabIndex = 0;
            this.Config.Text = "Config";
            this.Config.UseVisualStyleBackColor = true;
            // 
            // propertyGridConfig
            // 
            this.propertyGridConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridConfig.Location = new System.Drawing.Point(3, 3);
            this.propertyGridConfig.Name = "propertyGridConfig";
            this.propertyGridConfig.Size = new System.Drawing.Size(740, 507);
            this.propertyGridConfig.TabIndex = 0;
            this.propertyGridConfig.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridConfig_PropertyValueChanged);
            // 
            // Basic
            // 
            this.Basic.Controls.Add(this.richTextBox1);
            this.Basic.Location = new System.Drawing.Point(4, 22);
            this.Basic.Name = "Basic";
            this.Basic.Padding = new System.Windows.Forms.Padding(3);
            this.Basic.Size = new System.Drawing.Size(746, 513);
            this.Basic.TabIndex = 1;
            this.Basic.Text = "Basic";
            this.Basic.UseVisualStyleBackColor = true;
            // 
            // Image
            // 
            this.Image.Controls.Add(this.pictureBox);
            this.Image.Location = new System.Drawing.Point(4, 22);
            this.Image.Name = "Image";
            this.Image.Padding = new System.Windows.Forms.Padding(3);
            this.Image.Size = new System.Drawing.Size(746, 513);
            this.Image.TabIndex = 2;
            this.Image.Text = "Image";
            this.Image.UseVisualStyleBackColor = true;
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(3, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(740, 507);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(740, 507);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // DataGrid
            // 
            this.DataGrid.Controls.Add(this.dataGridView1);
            this.DataGrid.Location = new System.Drawing.Point(4, 22);
            this.DataGrid.Name = "DataGrid";
            this.DataGrid.Size = new System.Drawing.Size(746, 513);
            this.DataGrid.TabIndex = 3;
            this.DataGrid.Text = "DataGrid";
            this.DataGrid.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(746, 513);
            this.dataGridView1.TabIndex = 0;
            // 
            // FormConfigMaya
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 539);
            this.Controls.Add(this.tabControlCage);
            this.Name = "FormConfigMaya";
            this.Text = "FormMaya";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormConfigMaya_FormClosing);
            this.tabControlCage.ResumeLayout(false);
            this.Config.ResumeLayout(false);
            this.Basic.ResumeLayout(false);
            this.Image.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.DataGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlCage;
        private System.Windows.Forms.TabPage Config;
        private System.Windows.Forms.TabPage Basic;
        private System.Windows.Forms.PropertyGrid propertyGridConfig;
        private System.Windows.Forms.TabPage Image;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TabPage DataGrid;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}