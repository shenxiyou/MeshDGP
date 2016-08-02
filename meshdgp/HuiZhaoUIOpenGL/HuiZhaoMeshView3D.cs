using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
 
 

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
	public   class HuiZhaoMeshView3D : GLControl
	{
        private bool loaded = false;

        public IRender render=null;
        [Browsable(false)]
        public IRender Render
        {
            get
            {
                if (this.DesignMode)
                    return null;
                return render;
            }
            set
            {
                if (this.DesignMode)
                    return;

                if (value == null)
                    return;
                
                render = value;
              
                render.Init();
                this.MakeCurrent();
                render.Resize(this.Width, this.Height);
            }
        }
         
 
 
	 	public HuiZhaoMeshView3D()	:   base(new OpenTK.Graphics.GraphicsMode(32,24,8,1,32,2,true)) 
       // public HuiZhaoMeshView3D() 
		{
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MeshView3D_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MeshView3D_KeyDown);

            ToolPool.Instance.Width = this.Width;
            ToolPool.Instance.Height = this.Height;
            ToolPool.Instance.Tool = new ToolView(Width,Height);
             

            ToolPool.Instance.ChangedTool += new MeshChangedDelegate(Instance_ChangedTool);
            render = new RenderBasic();

            
            
		}

        void Instance_ChangedTool(object sender, EventArgs e)
        {
            this.Refresh();
        }

      

       
        
         
       

		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}

        protected override void OnResize(EventArgs e)
        {
            if (!this.loaded)
                return;
            if (this.Width == 0 || this.Height == 0) return;

          
            render.Resize(this.Size.Width, this.Size.Height);

            ToolPool.Instance.Tool.ScaleRatio = (this.Width > this.Height) ? this.Height : this.Width; 
            ToolPool.Instance.Tool.Height = this.Height;
            ToolPool.Instance.Tool.Width = this.Width;
            ToolPool.Instance.Width = this.Width;
            ToolPool.Instance.Height = this.Height;
            ToolPool.Instance.Tool.Ball.SetBounds(this.Width * 0.9, this.Height * 0.9);
            
        }
        
		protected override void OnMouseDown(MouseEventArgs e)
		{
            
            base.OnMouseDown(e);

            Vector2D mousedownPos= new Vector2D(e.X, this.Height - e.Y);
            EnumMouseButton button = EnumMouseButton.Left; ;
            switch (e.Button)
            {
                case MouseButtons.Middle: button = EnumMouseButton.Middle; break;
                case MouseButtons.Left:   button = EnumMouseButton.Left;  break;
                case MouseButtons.Right:  button = EnumMouseButton.Right; break;
            }

            ToolPool.Instance.Tool.MouseDown(mousedownPos, button);
            
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
            base.OnMouseMove(e);


            if (ToolPool.Instance.Tool.IsMouseDown)
            {
                Vector2D mouseMovePos = new Vector2D(e.X, this.Height - e.Y);
                EnumMouseButton button = EnumMouseButton.Left;  
                switch (e.Button)
                {
                    case MouseButtons.Middle: button = EnumMouseButton.Middle; break;
                    case MouseButtons.Left: button = EnumMouseButton.Left; break;
                    case MouseButtons.Right: button = EnumMouseButton.Right; break;
                }
                ToolPool.Instance.Tool.MouseMove(mouseMovePos,button);
            }
 
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
           

            base.OnMouseUp(e);
            Vector2D mouseUpPos = new Vector2D(e.X, this.Height - e.Y);
            EnumMouseButton button = EnumMouseButton.Left;  
            switch (e.Button)
            {
                case MouseButtons.Middle: button = EnumMouseButton.Middle; break;
                case MouseButtons.Left: button = EnumMouseButton.Left; break;
                case MouseButtons.Right: button = EnumMouseButton.Right; break;
            }
            ToolPool.Instance.Tool.MouseUp(mouseUpPos,button);

            
		}
        protected override void OnLoad(EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }
            this.loaded = true;
           
            render.Init();
            this.MakeCurrent();
            this.OnResize(EventArgs.Empty);

        }

        
		protected override void  OnPaint(PaintEventArgs e) 
		{
            
            if (this.DesignMode)
            {
                base.OnPaint(e);
                return;
            }   

            render.Render();
            this.SwapBuffers();
           
		}

        private void InitializeComponent()
        {
            this.SuspendLayout(); 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "MeshView3D"; 
            this.ResumeLayout(false);

        }

       
        

        private void MeshView3D_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A :
                    ToolPool.Instance.Tool.key = EnumKey.Shift;
                    break;
                case Keys.B:
                    ToolPool.Instance.Tool.key = EnumKey.Ctrl;
                    break;
                case Keys.C:
                    ToolPool.Instance.Tool.key = EnumKey.Alt;
                    break;

                case Keys.S:
                    SaveScreen();
                    break;

            }
        }

        public void SaveScreen()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image files (*.jpeg)|*.jpeg|Image files (*.bmp)|*.bmp|All files (*.*)|*.*";
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.FileName = "";
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

           // Rectangle rectangle=new Rectangle(new Point(0,0),new Size(ToolPool.Instance.Width,ToolPool.Instance.Height);
            Bitmap image = OpenGLManager.Instance.GrabScreen(this.ClientRectangle);
            image.Save(saveFileDialog.FileName);

        }

        private void MeshView3D_KeyUp(object sender, KeyEventArgs e)
        {
            ToolPool.Instance.Tool.key = EnumKey.None;
        }

         
      	 

 
		 
    
	}
}
