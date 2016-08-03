using System;
using System.Collections.Generic;
  

namespace GraphicResearchHuiZhao
{
    public  class  AbstractTool:IDisposable 
    {

       
        public delegate void ChangedEventHandlerTool(object sender, EventArgs e);

       
        public event ChangedEventHandlerTool Changed;

       
        protected virtual void OnChanged(EventArgs e)
        {
            if (Changed != null)
                Changed(this, e);
        }

        // for viewing and selecting
        private ArcBall ball = null;
        public ArcBall Ball
        {
            get
            {
                return ball;
            }
            set
            {
                ball = value;
            }
        }

        private double scaleRatio;
        public double ScaleRatio
        {
            get
            {
                return scaleRatio;
            }

            set
            {
                scaleRatio = value;
                
            }
        }

        
        


        protected  Vector2D mouseDownPos, mouseCurrPos;

        public int Width;
        public int Height;

        public EnumKey key = EnumKey.None;

        public  Vector2D MouseDownPos
        {
            get
            {
                return mouseDownPos;
            }
        }

        public  Vector2D MouseCurrPos
        {
            get
            {
                return mouseCurrPos;
            }
        }

        protected bool isMouseDown = false;
        public bool IsMouseDown
        {
            get
            {
                return isMouseDown;
            }
        }

         
        public virtual void MouseDown(Vector2D mouseDownPos,EnumMouseButton button)
        {
            this.mouseDownPos = mouseDownPos;
            this.isMouseDown = true;

        }

        public virtual void MouseMove(Vector2D mouseMovePos, EnumMouseButton button)
        {
            if (this.isMouseDown == false) return;

            this.mouseCurrPos = mouseMovePos;

        }
        public virtual void MouseUp(Vector2D mouseUpPos, EnumMouseButton button)
        {
            this.isMouseDown = false;
            this.mouseCurrPos = mouseUpPos;
        }

        public virtual void Draw()
        {
        }

        public   AbstractTool(double width,double height)
        {

            this.Width =(int) width;
            this.Height =(int) height;
            this.ball = new ArcBall(width * 0.9, height * 0.9);
            this.ScaleRatio = (width > height) ? height : width;
       
        } 
        

        public virtual void Dispose()
        {
             
        }

        


        public Vector3D ComputeViewPoint()
        {
            Matrix4D m =TransformController.Instance.ViewMatrix 
                      * ToolPool.Instance.Tool.Ball.CreateMatrix()
                      * TransformController.Instance.ModelMatrix;
            Vector4D view = new Vector4D(0, 0, 4, 1);
            view = m * view;
            return new Vector3D(view.x, view.y, view.z);

            return Vector3D.Zero;
        }
    }
}
