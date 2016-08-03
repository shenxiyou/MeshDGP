namespace GraphicResearchHuiZhao
{
    partial class FormSimplify
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
            this.tabControlSimplify = new System.Windows.Forms.TabControl();
            this.tabPageMerge = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBoxType = new System.Windows.Forms.ListBox();
            this.simpificationButton = new System.Windows.Forms.Button();
            this.faceToPreserveLabel = new System.Windows.Forms.Label();
            this.textBoxfaceCount = new System.Windows.Forms.TextBox();
            this.tabPageCluster = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.minCubeLenthLabel = new System.Windows.Forms.Label();
            this.clusterButton = new System.Windows.Forms.Button();
            this.textBoxLength = new System.Windows.Forms.TextBox();
            this.tabPageProgressiveMesh = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.forwardBtn = new System.Windows.Forms.Button();
            this.backwardBtn = new System.Windows.Forms.Button();
            this.progressRunBtn = new System.Windows.Forms.Button();
            this.vertexToPreserveLabel = new System.Windows.Forms.Label();
            this.textBoxPreserved = new System.Windows.Forms.TextBox();
            this.MeshView = new System.Windows.Forms.ListView();
            this.MetroView = new System.Windows.Forms.ListView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.tabControlSimplify.SuspendLayout();
            this.tabPageMerge.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPageCluster.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPageProgressiveMesh.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlSimplify
            // 
            this.tabControlSimplify.Controls.Add(this.tabPageMerge);
            this.tabControlSimplify.Controls.Add(this.tabPageCluster);
            this.tabControlSimplify.Controls.Add(this.tabPageProgressiveMesh);
            this.tabControlSimplify.Location = new System.Drawing.Point(0, 0);
            this.tabControlSimplify.Name = "tabControlSimplify";
            this.tabControlSimplify.SelectedIndex = 0;
            this.tabControlSimplify.Size = new System.Drawing.Size(309, 190);
            this.tabControlSimplify.TabIndex = 0;
            // 
            // tabPageMerge
            // 
            this.tabPageMerge.Controls.Add(this.groupBox3);
            this.tabPageMerge.Location = new System.Drawing.Point(4, 22);
            this.tabPageMerge.Name = "tabPageMerge";
            this.tabPageMerge.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMerge.Size = new System.Drawing.Size(301, 164);
            this.tabPageMerge.TabIndex = 0;
            this.tabPageMerge.Text = "Merge";
            this.tabPageMerge.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonRefresh);
            this.groupBox3.Controls.Add(this.listBoxType);
            this.groupBox3.Controls.Add(this.simpificationButton);
            this.groupBox3.Controls.Add(this.faceToPreserveLabel);
            this.groupBox3.Controls.Add(this.textBoxfaceCount);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(295, 158);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Setting";
            // 
            // listBoxType
            // 
            this.listBoxType.FormattingEnabled = true;
            this.listBoxType.ItemHeight = 12;
            this.listBoxType.Location = new System.Drawing.Point(6, 15);
            this.listBoxType.Name = "listBoxType";
            this.listBoxType.Size = new System.Drawing.Size(135, 136);
            this.listBoxType.TabIndex = 4;
            // 
            // simpificationButton
            // 
            this.simpificationButton.Location = new System.Drawing.Point(186, 130);
            this.simpificationButton.Name = "simpificationButton";
            this.simpificationButton.Size = new System.Drawing.Size(87, 21);
            this.simpificationButton.TabIndex = 2;
            this.simpificationButton.Text = "Simpfication";
            this.simpificationButton.UseVisualStyleBackColor = true;
            this.simpificationButton.Click += new System.EventHandler(this.simpificationButton_Click);
            // 
            // faceToPreserveLabel
            // 
            this.faceToPreserveLabel.AutoSize = true;
            this.faceToPreserveLabel.Location = new System.Drawing.Point(166, 78);
            this.faceToPreserveLabel.Name = "faceToPreserveLabel";
            this.faceToPreserveLabel.Size = new System.Drawing.Size(107, 12);
            this.faceToPreserveLabel.TabIndex = 1;
            this.faceToPreserveLabel.Text = "Face to Preserved";
            // 
            // textBoxfaceCount
            // 
            this.textBoxfaceCount.Location = new System.Drawing.Point(173, 103);
            this.textBoxfaceCount.Name = "textBoxfaceCount";
            this.textBoxfaceCount.Size = new System.Drawing.Size(100, 21);
            this.textBoxfaceCount.TabIndex = 0;
            // 
            // tabPageCluster
            // 
            this.tabPageCluster.Controls.Add(this.groupBox1);
            this.tabPageCluster.Location = new System.Drawing.Point(4, 22);
            this.tabPageCluster.Name = "tabPageCluster";
            this.tabPageCluster.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCluster.Size = new System.Drawing.Size(301, 164);
            this.tabPageCluster.TabIndex = 1;
            this.tabPageCluster.Text = "Cluster";
            this.tabPageCluster.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.minCubeLenthLabel);
            this.groupBox1.Controls.Add(this.clusterButton);
            this.groupBox1.Controls.Add(this.textBoxLength);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(295, 158);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cluster Cube";
            // 
            // minCubeLenthLabel
            // 
            this.minCubeLenthLabel.AutoSize = true;
            this.minCubeLenthLabel.Location = new System.Drawing.Point(12, 24);
            this.minCubeLenthLabel.Name = "minCubeLenthLabel";
            this.minCubeLenthLabel.Size = new System.Drawing.Size(101, 12);
            this.minCubeLenthLabel.TabIndex = 2;
            this.minCubeLenthLabel.Text = "Min Cube Length:";
            // 
            // clusterButton
            // 
            this.clusterButton.Location = new System.Drawing.Point(217, 18);
            this.clusterButton.Name = "clusterButton";
            this.clusterButton.Size = new System.Drawing.Size(77, 25);
            this.clusterButton.TabIndex = 1;
            this.clusterButton.Text = "Cluster!";
            this.clusterButton.UseVisualStyleBackColor = true;
            this.clusterButton.Click += new System.EventHandler(this.clusterButton_Click);
            // 
            // textBoxLength
            // 
            this.textBoxLength.Location = new System.Drawing.Point(109, 21);
            this.textBoxLength.Name = "textBoxLength";
            this.textBoxLength.Size = new System.Drawing.Size(102, 21);
            this.textBoxLength.TabIndex = 0;
            // 
            // tabPageProgressiveMesh
            // 
            this.tabPageProgressiveMesh.Controls.Add(this.groupBox2);
            this.tabPageProgressiveMesh.Location = new System.Drawing.Point(4, 22);
            this.tabPageProgressiveMesh.Name = "tabPageProgressiveMesh";
            this.tabPageProgressiveMesh.Size = new System.Drawing.Size(301, 164);
            this.tabPageProgressiveMesh.TabIndex = 2;
            this.tabPageProgressiveMesh.Text = "ProgressiveMesh";
            this.tabPageProgressiveMesh.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.forwardBtn);
            this.groupBox2.Controls.Add(this.backwardBtn);
            this.groupBox2.Controls.Add(this.progressRunBtn);
            this.groupBox2.Controls.Add(this.vertexToPreserveLabel);
            this.groupBox2.Controls.Add(this.textBoxPreserved);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(301, 164);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ProgressiveMesh";
            // 
            // forwardBtn
            // 
            this.forwardBtn.Location = new System.Drawing.Point(141, 80);
            this.forwardBtn.Name = "forwardBtn";
            this.forwardBtn.Size = new System.Drawing.Size(61, 26);
            this.forwardBtn.TabIndex = 7;
            this.forwardBtn.Text = "Forward";
            this.forwardBtn.UseVisualStyleBackColor = true;
            this.forwardBtn.Click += new System.EventHandler(this.forwardBtn_Click);
            // 
            // backwardBtn
            // 
            this.backwardBtn.Location = new System.Drawing.Point(45, 80);
            this.backwardBtn.Name = "backwardBtn";
            this.backwardBtn.Size = new System.Drawing.Size(63, 27);
            this.backwardBtn.TabIndex = 6;
            this.backwardBtn.Text = "Backward";
            this.backwardBtn.UseVisualStyleBackColor = true;
            this.backwardBtn.Click += new System.EventHandler(this.backwardBtn_Click);
            // 
            // progressRunBtn
            // 
            this.progressRunBtn.Location = new System.Drawing.Point(225, 29);
            this.progressRunBtn.Name = "progressRunBtn";
            this.progressRunBtn.Size = new System.Drawing.Size(58, 22);
            this.progressRunBtn.TabIndex = 5;
            this.progressRunBtn.Text = "Run";
            this.progressRunBtn.UseVisualStyleBackColor = true;
            this.progressRunBtn.Click += new System.EventHandler(this.progressBtn_Click);
            // 
            // vertexToPreserveLabel
            // 
            this.vertexToPreserveLabel.AutoSize = true;
            this.vertexToPreserveLabel.Location = new System.Drawing.Point(8, 34);
            this.vertexToPreserveLabel.Name = "vertexToPreserveLabel";
            this.vertexToPreserveLabel.Size = new System.Drawing.Size(101, 12);
            this.vertexToPreserveLabel.TabIndex = 4;
            this.vertexToPreserveLabel.Text = "Face to Preserve";
            // 
            // textBoxPreserved
            // 
            this.textBoxPreserved.Location = new System.Drawing.Point(119, 31);
            this.textBoxPreserved.Name = "textBoxPreserved";
            this.textBoxPreserved.Size = new System.Drawing.Size(100, 21);
            this.textBoxPreserved.TabIndex = 3;
            // 
            // MeshView
            // 
            this.MeshView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MeshView.Location = new System.Drawing.Point(3, 17);
            this.MeshView.Name = "MeshView";
            this.MeshView.Size = new System.Drawing.Size(299, 104);
            this.MeshView.TabIndex = 1;
            this.MeshView.UseCompatibleStateImageBehavior = false;
            this.MeshView.View = System.Windows.Forms.View.Details;
            // 
            // MetroView
            // 
            this.MetroView.Dock = System.Windows.Forms.DockStyle.Top;
            this.MetroView.Location = new System.Drawing.Point(3, 17);
            this.MetroView.Name = "MetroView";
            this.MetroView.Size = new System.Drawing.Size(383, 249);
            this.MetroView.TabIndex = 2;
            this.MetroView.UseCompatibleStateImageBehavior = false;
            this.MetroView.View = System.Windows.Forms.View.Details;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.MeshView);
            this.groupBox4.Location = new System.Drawing.Point(4, 196);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(305, 124);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Mesh";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button1);
            this.groupBox5.Controls.Add(this.MetroView);
            this.groupBox5.Location = new System.Drawing.Point(341, 22);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(389, 295);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Metro";
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(3, 262);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(383, 30);
            this.button1.TabIndex = 5;
            this.button1.Text = "Metro";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(186, 21);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 5;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // FormSimplify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 371);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.tabControlSimplify);
            this.Name = "FormSimplify";
            this.Text = "FormSimplify";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSimplify_FormClosing);
            this.tabControlSimplify.ResumeLayout(false);
            this.tabPageMerge.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPageCluster.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPageProgressiveMesh.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlSimplify;
        private System.Windows.Forms.TabPage tabPageMerge;
        private System.Windows.Forms.TabPage tabPageCluster;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button simpificationButton;
        private System.Windows.Forms.Label faceToPreserveLabel;
        private System.Windows.Forms.TextBox textBoxfaceCount;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label minCubeLenthLabel;
        private System.Windows.Forms.Button clusterButton;
        private System.Windows.Forms.TextBox textBoxLength;
        private System.Windows.Forms.TabPage tabPageProgressiveMesh;
        private System.Windows.Forms.ListView MeshView;
        private System.Windows.Forms.ListView MetroView;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label vertexToPreserveLabel;
        private System.Windows.Forms.TextBox textBoxPreserved;
        private System.Windows.Forms.Button progressRunBtn;
        private System.Windows.Forms.Button forwardBtn;
        private System.Windows.Forms.Button backwardBtn;
        private System.Windows.Forms.ListBox listBoxType;
        private System.Windows.Forms.Button buttonRefresh;
    }
}