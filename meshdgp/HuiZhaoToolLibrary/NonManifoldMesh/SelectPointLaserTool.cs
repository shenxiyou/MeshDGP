using System;
using System.Collections.Generic;
using System.Text;
 

namespace GraphicResearchHuiZhao 
{
    public class SelectPointLaserTool:SelectPointTool
    {
        public SelectPointLaserTool(double width, double height, NonManifoldMesh mesh)
            : base(width,height, mesh)
        {
        }

        protected override  int SelectByPoint()
        {
            return SelectVertexByPoint(true);
        }
    }
}
