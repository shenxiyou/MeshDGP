using System;
using System.Collections.Generic;

 
 
namespace GraphicResearchHuiZhao
{
    public class ToolView:AbstractTool 
    {


        public ToolView(double width, double height)
            : base(width, height)
        {
          
           
        }

        #region ITool Members

        public override void MouseDown(Vector2D mouseDownPos, EnumMouseButton button)
        {
            base.MouseDown(mouseDownPos, button);

            switch (button)
            {
                case EnumMouseButton.Left: this.Ball.Click( mouseDownPos, ArcBall.MotionType.Rotation); break;
                case EnumMouseButton.Middle: this.Ball.Click(mouseDownPos / this.ScaleRatio, ArcBall.MotionType.Pan); break;
                case EnumMouseButton.Right: this.Ball.Click(mouseDownPos, ArcBall.MotionType.Scale); break;
            }
        }

        public override void MouseMove(Vector2D mouseMovePos, EnumMouseButton button)
        {
            base.MouseMove(mouseMovePos, button);
            switch (button)
            {
                case EnumMouseButton.Left: this.Ball.Drag(mouseCurrPos); break;
                case EnumMouseButton.Middle: this.Ball.Drag(mouseCurrPos / this.ScaleRatio); break;
                case EnumMouseButton.Right: this.Ball.Drag(mouseCurrPos); break;
            }

            OnChanged(EventArgs.Empty);
        }

        public override void MouseUp(Vector2D mouseUpPos, EnumMouseButton button)
        {
            base.MouseUp(mouseUpPos, button);
            Matrix4D m = this.Ball.CreateMatrix();
            TransformController.Instance.ModelMatrix = m * TransformController.Instance.ModelMatrix;
            this.Ball.End();
            OnChanged(EventArgs.Empty);
        }

        #endregion

        
    }
}
