using System;
using System.Collections.Generic;
 
 
 


namespace GraphicResearchHuiZhao
{
    public class ToolVertexByPoint : ToolBaseTriMesh
    {


        public ToolVertexByPoint(double width, double height, TriMesh mesh)
            : base(width, height, mesh)
        {

        }




        public override void MouseUp(Vector2D mouseUpPos, EnumMouseButton button)
        {
            if (mesh == null)
                return;

            base.MouseUp(mouseUpPos, button);
            if (button == EnumMouseButton.Left)
            {
                 SelectByPoint();
            }

            TriMeshUtil.GroupVertice(mesh);
            OnChanged(EventArgs.Empty);
        }

  

        protected virtual int SelectByPoint()
        {
            return SelectVertexByPoint(false);
        }


       

        public override void Draw()
        {

           // OpenGLTriMesh.Instance.DrawSelectedVerticeBySphere(mesh);

        }

    }
}
