using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    /// <summary>
    /// 基于法向的网格简化_蒋遂平
    /// </summary>
    public class LengthAndAngleSimplication : MergeEdgeSimplicationBase
    {
        public LengthAndAngleSimplication(TriMesh mesh)
            : base(mesh)
        {

        }

        protected override double GetValue(TriMesh.Edge edge)
        {
            Vector3D n0 = this.traits.VertexAreaWeightNormal[edge.Vertex0.Index];
            Vector3D n1 = this.traits.VertexAreaWeightNormal[edge.Vertex1.Index];
            double angle = Math.Acos(n0.Dot(n1));
            if (angle < 0)
            {
                throw new Exception();
            }
            return angle * this.traits.EdgeLength[edge.Index];
        }
    }
}
