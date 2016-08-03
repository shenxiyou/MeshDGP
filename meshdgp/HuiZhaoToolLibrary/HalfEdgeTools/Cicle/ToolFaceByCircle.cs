using System;
using System.Collections.Generic;

 
 
 

namespace GraphicResearchHuiZhao
{
    public class ToolFaceByCircle : ToolBaseTriMesh
    {


        public ToolFaceByCircle(double width, double height, TriMesh mesh)
            : base(width, height, mesh)
        {

        }



        public override void MouseMove(Vector2D mouseMovePos, EnumMouseButton button)
        {
            if (mesh == null)
                return;

            base.MouseMove(mouseMovePos,button); 
            SelectByCircle();
            TriMeshUtil.GroupVertice(mesh);
            OnChanged(EventArgs.Empty);
        }

         
        protected virtual void SelectByCircle()
        {
            SelectFaceByCircle(false);
        }

        



        public override void Draw()
        {

           // OpenGLTriMesh.Instance.DrawSelectedFace(mesh);

            if (this.IsMouseDown)
                OpenGLManager.Instance.DrawSelectionInterface(Width, Height, 
                                                           this.MouseDownPos.x, this.MouseDownPos.y,
                                                           this.MouseCurrPos.x, this.MouseCurrPos.y, OpenGLFlatShape.Circle);
        

        }

    }
}
