using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class SmallFaceSimplication : MergeTriangleSimplicationBase
    {
        public SmallFaceSimplication(TriMesh mesh)
            : base(mesh)
        {

        }

        protected override double GetValue(HalfEdgeMesh.Face target)
        {
            return TriMeshUtil.ComputeAreaFace(target);
        }
    }
}
