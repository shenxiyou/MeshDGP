using System;
using System.Collections.Generic;
using System.Text;


namespace GraphicResearchHuiZhao 
{
    public class SelectRectangleLaserTool:SelectRectangleTool
    {
        public SelectRectangleLaserTool(double width, double height, NonManifoldMesh mesh)
            : base(width, height, mesh)
        {
           
        }
        protected override  void SelectByRectangle()
        {
            SelectVertexByRect(true);
        }
    }
}
