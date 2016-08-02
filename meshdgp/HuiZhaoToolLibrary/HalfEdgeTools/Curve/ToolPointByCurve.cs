using System;
using System.Collections.Generic;

 
 
 

namespace GraphicResearchHuiZhao
{
    public class ToolPointByCurve : ToolBaseTriMesh
    {

        private List<List<int>> lines = new List<List<int>>();

        public ToolPointByCurve(double width, double height, TriMesh mesh)
            : base(width,height, mesh)
        {
            List<int> line = new List<int>();
            lines.Add(line);
        }




        public override void MouseMove(Vector2D mouseMovePos, EnumMouseButton button)
        {
            if (mesh == null)
                return;
            base.MouseMove(mouseMovePos ,button );
            SelectByPoint();
            OnChanged(EventArgs.Empty);
        }

        

        protected virtual void SelectByPoint()
        {
          int minIndex=  SelectVertexByPoint(false);
          lines[lines.Count - 1].Add(minIndex);
         
        }


        public override void Draw()
        {
            OpenGLTriMesh.Instance.DrawCurves(mesh, lines);

        }

    }
}
