using System;
using System.Collections.Generic;





namespace GraphicResearchHuiZhao
{
    public class ToolEdgeByCurve : ToolBaseTriMesh
    {

        public int curVertex = -1;

        public ToolEdgeByCurve(double width, double height, TriMesh mesh)
            : base(width, height, mesh)
        {
            
        }




        public override void MouseMove(Vector2D mouseMovePos, EnumMouseButton button)
        {
            if (mesh == null)
                return;
            base.MouseMove(mouseMovePos, button);
            SelectByPoint();
            OnChanged(EventArgs.Empty);
        }



        protected virtual void SelectByPoint()
        {
            int minIndex = SelectVertexByPoint(false);
            if (curVertex != -1)
            {
                TriMesh.Edge edge = TriMeshUtil.FindEdge(mesh.Vertices[curVertex], mesh.Vertices[minIndex]);
                if(edge!=null)
                edge.Traits.SelectedFlag = 1; 
            }
            curVertex = minIndex;
        }  
        

    }
}
