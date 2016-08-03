using System;
using System.Collections.Generic;
using System.Text;


namespace GraphicResearchHuiZhao
{
    public class BaseMeshTool:AbstractTool 
    {
        protected NonManifoldMesh mesh;
        public BaseMeshTool(double width, double height, NonManifoldMesh mesh)
            : base(width,height)
        {
            this.mesh = mesh;
        }
    }
}
