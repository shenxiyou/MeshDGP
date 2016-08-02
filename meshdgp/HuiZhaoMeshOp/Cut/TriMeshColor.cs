using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public partial class TriMeshModify
    {
        public static void ComputeSelectedEdgeFromColor(TriMesh mesh)
        {
            foreach (TriMesh.Edge edge in mesh.Edges)
            {
                if (!edge.OnBoundary)
                {

                    if (edge.Face0.Traits.SelectedFlag != edge.Face1.Traits.SelectedFlag)
                    {
                        edge.Traits.SelectedFlag = 1;
                    }
                }
            
            }
        }
    }
}
