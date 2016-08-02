using System;
using System.Collections.Generic;
using System.Text;

 
 

namespace GraphicResearchHuiZhao
{
    public class ToolLaserPointByPoint : ToolVertexByPoint
    {
        public ToolLaserPointByPoint(double width, double height, TriMesh mesh)
            : base(width, height, mesh)
        {
        }

        protected override int SelectByPoint()
        {
            return SelectVertexByPoint(true);
        }
    }
}
