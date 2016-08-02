namespace GraphicResearchHuiZhao
{
    partial class OpenGLViewer
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
            this.MeshView3D = new GraphicResearchHuiZhao.HuiZhaoMeshView3D();
            this.SuspendLayout();
            // 
            // MeshView3D
            // 
            this.MeshView3D.BackColor = System.Drawing.Color.Black;
            this.MeshView3D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MeshView3D.Location = new System.Drawing.Point(0, 0);
            this.MeshView3D.Name = "MeshView3D";
            this.MeshView3D.Render = null;
            this.MeshView3D.Size = new System.Drawing.Size(664, 376);
            this.MeshView3D.TabIndex = 0;
            this.MeshView3D.VSync = false;
            // 
            // OpenGLViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MeshView3D);
            this.Name = "OpenGLViewer";
            this.Size = new System.Drawing.Size(664, 376);
            this.ResumeLayout(false);

        }

        #endregion

        private HuiZhaoMeshView3D MeshView3D=null;
    }
}
