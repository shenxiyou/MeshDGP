using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Collections.Generic ;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
 


namespace GraphicResearchHuiZhao
{
	public partial class FormMainMaya :    Form
	{ 
	 

		#region controls


        private System.ComponentModel.IContainer components;
        private ToolBarUI toolBarUI;
        private GraphicResearchHuiZhao.Forms.MenuMainMaya menuMain;
 
        private OpenGLViewer openGLViewer;
        private BarStatus barStatus1;
 
 
 
 
 

 
		#endregion

        public FormMainMaya()
		{
			InitializeComponent();

            menuMain.Init();

            
        }

       
         

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMainMaya));
            this.openGLViewer = new GraphicResearchHuiZhao.OpenGLViewer();
            this.barStatus1 = new GraphicResearchHuiZhao.BarStatus();
            this.toolBarUI = new GraphicResearchHuiZhao.ToolBarUI();
            this.menuMain = new GraphicResearchHuiZhao.Forms.MenuMainMaya();
            this.SuspendLayout();
            // 
            // openGLViewer
            // 
            this.openGLViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.openGLViewer.Location = new System.Drawing.Point(0, 99);
            this.openGLViewer.Name = "openGLViewer";
            this.openGLViewer.Size = new System.Drawing.Size(1124, 431);
            this.openGLViewer.TabIndex = 34;
            // 
            // barStatus1
            // 
            this.barStatus1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barStatus1.Location = new System.Drawing.Point(0, 530);
            this.barStatus1.Name = "barStatus1";
            this.barStatus1.Size = new System.Drawing.Size(1124, 20);
            this.barStatus1.TabIndex = 33;
            // 
            // toolBarUI
            // 
            this.toolBarUI.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolBarUI.Location = new System.Drawing.Point(0, 58);
            this.toolBarUI.Name = "toolBarUI";
            this.toolBarUI.Size = new System.Drawing.Size(1124, 41);
            this.toolBarUI.TabIndex = 25;
            // 
            // menuMain
            // 
            this.menuMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(1124, 58);
            this.menuMain.TabIndex = 32;
            // 
            // FormMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1124, 550);
            this.Controls.Add(this.openGLViewer);
            this.Controls.Add(this.barStatus1);
            this.Controls.Add(this.toolBarUI);
            this.Controls.Add(this.menuMain);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.ResumeLayout(false);

		}
		#endregion

		 
		   
	 
         
        
	 
		 
        

       

         
         
        

        
       
       
        
       
      
        

       

       
       
       

        
 
       






    }
}

  