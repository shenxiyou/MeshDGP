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
	public partial class FormMain :    Form
	{ 
	 

		#region controls


       
        private ToolBarUI toolBarUI;
        private GraphicResearchHuiZhao.Forms.MenuMain menuMain;
        private BarStatus barStatus1;
        private OpenGLViewer meshView3D;
 
 
 
 
 

 
		#endregion

		public FormMain()
		{
			InitializeComponent();

            menuMain.Init();

            
        }

       
         

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.toolBarUI = new GraphicResearchHuiZhao.ToolBarUI();
            this.meshView3D = new GraphicResearchHuiZhao.OpenGLViewer();
            this.barStatus1 = new GraphicResearchHuiZhao.BarStatus();
            this.menuMain = new GraphicResearchHuiZhao.Forms.MenuMain();
            this.SuspendLayout();
            // 
            // toolBarUI
            // 
            this.toolBarUI.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolBarUI.Location = new System.Drawing.Point(0, 87);
            this.toolBarUI.Name = "toolBarUI";
            this.toolBarUI.Size = new System.Drawing.Size(1424, 35);
            this.toolBarUI.TabIndex = 25;
            this.toolBarUI.Load += new System.EventHandler(this.toolBarUI_Load);
            // 
            // meshView3D
            // 
            this.meshView3D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.meshView3D.Location = new System.Drawing.Point(0, 0);
            this.meshView3D.Name = "meshView3D";
            this.meshView3D.Size = new System.Drawing.Size(1424, 768);
            this.meshView3D.TabIndex = 31;
            // 
            // barStatus1
            // 
            this.barStatus1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barStatus1.Location = new System.Drawing.Point(0, 748);
            this.barStatus1.Name = "barStatus1";
            this.barStatus1.Size = new System.Drawing.Size(1424, 20);
            this.barStatus1.TabIndex = 33;
            // 
            // menuMain
            // 
            this.menuMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(1424, 87);
            this.menuMain.TabIndex = 32;
            // 
            // FormMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1424, 768);
            this.Controls.Add(this.barStatus1);
            this.Controls.Add(this.toolBarUI);
            this.Controls.Add(this.menuMain);
            this.Controls.Add(this.meshView3D);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.ResumeLayout(false);

		}
		#endregion

        private void toolBarUI_Load(object sender, EventArgs e)
        {

        }

		 
		   
	 
         
        
	 
		 
        

       

         
         
        

        
       
       
        
       
      
        

       

       
       
       

        
 
       






    }
}

  