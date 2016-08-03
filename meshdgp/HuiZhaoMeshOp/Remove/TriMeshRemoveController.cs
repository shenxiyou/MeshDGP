using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public partial class TriMeshModify
    {





        public static void Remove(TriMesh mesh,EnumRemove enumRemove)
        {
            switch (enumRemove)
            {
                case EnumRemove.Vertex:
                    RemoveOneRingOfVertex(mesh);
                    break;
               
                case EnumRemove.Face:
                    RemoveSelectedFaces(mesh);
                    break;
                case EnumRemove.Edge :
                    RemoveSelectedEdges(mesh);
                    break;

                case EnumRemove.TwoRingOfVertex:
                    RemoveTwoRingOfVertex(mesh);
                    break;

                case EnumRemove.OneRingOfEdge:
                    RemoveOneRingOfEdge(mesh);
                    break;

                case EnumRemove.MergeEdge:
                    MergeEdge(mesh);
                    break;
                case EnumRemove.Test:
                    
                    break;

            }
        }
    }
}
