using System;
using System.Collections.Generic;

 
using System.Data;
 
 

namespace GraphicResearchHuiZhao
{
    public class ToolFaceByPoint : ToolBaseTriMesh
    {
        protected int selectedEdge = -1;
        private List<List<int>> lines = new List<List<int>>();
        public ToolFaceByPoint(double width, double height, TriMesh mesh)
            : base(width,height, mesh)
        {

        }
       

        public override void MouseDown(Vector2D mouseDownPos, EnumMouseButton button)
        {
            if (mesh == null)
                return;
            base.MouseDown(mouseDownPos, button); 

        }



        public override void MouseUp(Vector2D mouseUpPos, EnumMouseButton button)
        {
            if (mesh == null)
                return;

            base.MouseUp(mouseUpPos, button);
            if (button == EnumMouseButton.Left)
            {
                selectedEdge = SelectByPoint();
            }

            TriMeshUtil.GroupVertice(mesh);
            OnChanged(EventArgs.Empty);
        }

        
        protected virtual int SelectByPoint()
        {
            return SelectFaceByPoint(false);
        }

        public override void Draw()
        {
             
          // OpenGLHalfEdge.Instance.DrawSelectedFace(mesh);
                

        }
        
       
    }
}
