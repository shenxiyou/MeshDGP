using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class ShortEdgeSimplication : MergeEdgeSimplicationBase
    {
        public ShortEdgeSimplication(TriMesh mesh)
            : base(mesh)
        {

        }

        protected override double GetValue(HalfEdgeMesh.Edge target)
        {
            return TriMeshUtil.ComputeEdgeLength(target);
        }
    }
}
