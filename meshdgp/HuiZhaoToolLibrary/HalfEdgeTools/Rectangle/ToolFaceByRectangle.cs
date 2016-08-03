using System;
using System.Collections.Generic;

 
 


namespace GraphicResearchHuiZhao
{
    public class ToolFaceByRectangle : ToolBaseTriMesh
    {


        public ToolFaceByRectangle(double width, double height, TriMesh mesh)
            : base(width,height, mesh)
        {

        }





        public override void MouseMove(Vector2D mouseMovePos, EnumMouseButton button)
        {
            if (mesh == null)
                return;
            base.MouseMove(mouseMovePos, button);

             SelectByRectangle();


             TriMeshUtil.GroupVertice(mesh);
            OnChanged(EventArgs.Empty);
        }

       

        protected virtual void SelectByRectangle()
        {
            SelectFaceByRect(false);
        }

        



        public override void Draw()
        {

            //OpenGLTriMesh.Instance.DrawSelectedFace(mesh);

            if (this.IsMouseDown)
                OpenGLManager.Instance.DrawSelectionInterface(Width, Height, this.MouseDownPos.x, this.MouseDownPos.y, this.MouseCurrPos.x, this.MouseCurrPos.y, OpenGLFlatShape.Rectangle); 
        }

    }
}
