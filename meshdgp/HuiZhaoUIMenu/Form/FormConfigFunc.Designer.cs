namespace GraphicResearchHuiZhao
{
    partial class FormConfigFunc
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
            this.tabControlFunc = new System.Windows.Forms.TabControl();
            this.tabPageConfig = new System.Windows.Forms.TabPage();
            this.propertyGridConfig = new System.Windows.Forms.PropertyGrid();
            this.Histogram = new System.Windows.Forms.TabPage();
            this.histogramControl = new GraphicResearchHuiZhao.HistogramControl();
            this.tabPageValue = new System.Windows.Forms.TabPage();
            this.dataGridViewFuncvalue = new System.Windows.Forms.DataGridView();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MorseType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MorseCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageMorse = new System.Windows.Forms.TabPage();
            this.textBoxMorse = new System.Windows.Forms.TextBox();
            this.tabPageRefresh = new System.Windows.Forms.TabPage();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.tabControlFunc.SuspendLayout();
            this.tabPageConfig.SuspendLayout();
            this.Histogram.SuspendLayout();
            this.tabPageValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFuncvalue)).BeginInit();
            this.tabPageMorse.SuspendLayout();
            this.tabPageRefresh.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlFunc
            // 
            this.tabControlFunc.Controls.Add(this.tabPageConfig);
            this.tabControlFunc.Controls.Add(this.Histogram);
            this.tabControlFunc.Controls.Add(this.tabPageValue);
            this.tabControlFunc.Controls.Add(this.tabPageMorse);
            this.tabControlFunc.Controls.Add(this.tabPageRefresh);
            this.tabControlFunc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlFunc.Location = new System.Drawing.Point(0, 0);
            this.tabControlFunc.Name = "tabControlFunc";
            this.tabControlFunc.SelectedIndex = 0;
            this.tabControlFunc.Size = new System.Drawing.Size(660, 345);
            this.tabControlFunc.TabIndex = 0;
            // 
            // tabPageConfig
            // 
            this.tabPageConfig.Controls.Add(this.propertyGridConfig);
            this.tabPageConfig.Location = new System.Drawing.Point(4, 22);
            this.tabPageConfig.Name = "tabPageConfig";
            this.tabPageConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConfig.Size = new System.Drawing.Size(652, 319);
            this.tabPageConfig.TabIndex = 0;
            this.tabPageConfig.Text = "Para Config";
            this.tabPageConfig.UseVisualStyleBackColor = true;
            // 
            // propertyGridConfig
            // 
            this.propertyGridConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridConfig.Location = new System.Drawing.Point(3, 3);
            this.propertyGridConfig.Name = "propertyGridConfig";
            this.propertyGridConfig.Size = new System.Drawing.Size(646, 313);
            this.propertyGridConfig.TabIndex = 0;
            this.propertyGridConfig.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridConfig_PropertyValueChanged);
            // 
            // Histogram
            // 
            this.Histogram.Controls.Add(this.histogramControl);
            this.Histogram.Location = new System.Drawing.Point(4, 22);
            this.Histogram.Name = "Histogram";
            this.Histogram.Padding = new System.Windows.Forms.Padding(3);
            this.Histogram.Size = new System.Drawing.Size(652, 319);
            this.Histogram.TabIndex = 1;
            this.Histogram.Text = "Histogram";
            this.Histogram.UseVisualStyleBackColor = true;
            // 
            // histogramControl
            // 
            this.histogramControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.histogramControl.Location = new System.Drawing.Point(3, 3);
            this.histogramControl.Name = "histogramControl";
            this.histogramControl.Size = new System.Drawing.Size(646, 313);
            this.histogramControl.TabIndex = 0;
            // 
            // tabPageValue
            // 
            this.tabPageValue.Controls.Add(this.dataGridViewFuncvalue);
            this.tabPageValue.Location = new System.Drawing.Point(4, 22);
            this.tabPageValue.Name = "tabPageValue";
            this.tabPageValue.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageValue.Size = new System.Drawing.Size(652, 319);
            this.tabPageValue.TabIndex = 2;
            this.tabPageValue.Text = "Function Value";
            this.tabPageValue.UseVisualStyleBackColor = true;
            // 
            // dataGridViewFuncvalue
            // 
            this.dataGridViewFuncvalue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFuncvalue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.Value,
            this.MorseType,
            this.MorseCount});
            this.dataGridViewFuncvalue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewFuncvalue.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewFuncvalue.Name = "dataGridViewFuncvalue";
            this.dataGridViewFuncvalue.Size = new System.Drawing.Size(646, 313);
            this.dataGridViewFuncvalue.TabIndex = 0;
            // 
            // Index
            // 
            this.Index.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Index.HeaderText = "Index";
            this.Index.Name = "Index";
            // 
            // Value
            // 
            this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            // 
            // MorseType
            // 
            this.MorseType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MorseType.HeaderText = "MorseType";
            this.MorseType.Name = "MorseType";
            // 
            // MorseCount
            // 
            this.MorseCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MorseCount.HeaderText = "MorseCount";
            this.MorseCount.Name = "MorseCount";
            // 
            // tabPageMorse
            // 
            this.tabPageMorse.Controls.Add(this.textBoxMorse);
            this.tabPageMorse.Location = new System.Drawing.Point(4, 22);
            this.tabPageMorse.Name = "tabPageMorse";
            this.tabPageMorse.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMorse.Size = new System.Drawing.Size(652, 319);
            this.tabPageMorse.TabIndex = 3;
            this.tabPageMorse.Text = "Morse Theory";
            this.tabPageMorse.UseVisualStyleBackColor = true;
            // 
            // textBoxMorse
            // 
            this.textBoxMorse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMorse.Location = new System.Drawing.Point(3, 3);
            this.textBoxMorse.Multiline = true;
            this.textBoxMorse.Name = "textBoxMorse";
            this.textBoxMorse.Size = new System.Drawing.Size(646, 313);
            this.textBoxMorse.TabIndex = 0;
            // 
            // tabPageRefresh
            // 
            this.tabPageRefresh.Controls.Add(this.buttonRefresh);
            this.tabPageRefresh.Location = new System.Drawing.Point(4, 22);
            this.tabPageRefresh.Name = "tabPageRefresh";
            this.tabPageRefresh.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRefresh.Size = new System.Drawing.Size(652, 319);
            this.tabPageRefresh.TabIndex = 4;
            this.tabPageRefresh.Text = "Refresh";
            this.tabPageRefresh.UseVisualStyleBackColor = true;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(197, 121);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 0;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // FormFuncConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 345);
            this.Controls.Add(this.tabControlFunc);
            this.Name = "FormFuncConfig";
            this.Text = "FormFuncConfig";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFuncConfig_FormClosing);
            this.tabControlFunc.ResumeLayout(false);
            this.tabPageConfig.ResumeLayout(false);
            this.Histogram.ResumeLayout(false);
            this.tabPageValue.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFuncvalue)).EndInit();
            this.tabPageMorse.ResumeLayout(false);
            this.tabPageMorse.PerformLayout();
            this.tabPageRefresh.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlFunc;
        private System.Windows.Forms.TabPage tabPageConfig;
        private System.Windows.Forms.TabPage Histogram;
        private System.Windows.Forms.PropertyGrid propertyGridConfig;
        private HistogramControl histogramControl;
        private System.Windows.Forms.TabPage tabPageValue;
        private System.Windows.Forms.DataGridView dataGridViewFuncvalue;
        private System.Windows.Forms.TabPage tabPageMorse;
        private System.Windows.Forms.TextBox textBoxMorse;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn MorseType;
        private System.Windows.Forms.DataGridViewTextBoxColumn MorseCount;
        private System.Windows.Forms.TabPage tabPageRefresh;
        private System.Windows.Forms.Button buttonRefresh;
    }
}