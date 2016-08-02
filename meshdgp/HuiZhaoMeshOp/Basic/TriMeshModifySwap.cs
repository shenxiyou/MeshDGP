using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public partial class TriMeshModify
    {
        public static bool EdgeSwap(TriMesh.Edge edge)
        {
            if (edge.OnBoundary)
            {
                return false;
            }
            //逆时针90度，左右变为下上
            TriMesh.HalfEdge hf1 = edge.HalfEdge0;
            TriMesh.HalfEdge hf2 = edge.HalfEdge1;
            TriMesh.Vertex top = hf1.ToVertex;
            TriMesh.Vertex buttom = hf2.ToVertex;
            TriMesh.HalfEdge topLeft = hf1.Next;
            TriMesh.HalfEdge buttomLeft = hf1.Previous;
            TriMesh.HalfEdge topRight = hf2.Previous;
            TriMesh.HalfEdge buttomRight = hf2.Next;
            top.HalfEdge = topLeft;
            buttom.HalfEdge = buttomRight;
            hf1.ToVertex = topLeft.ToVertex;
            hf2.ToVertex = buttomRight.ToVertex;
            hf1.Face.HalfEdge = hf1;
            hf2.Face.HalfEdge = hf2;
            topLeft.Face = hf2.Face;
            buttomRight.Face = hf1.Face;
            ConnectHalfEdge(topLeft, hf2, topRight);
            ConnectHalfEdge(buttomRight, hf1, buttomLeft);
            return true;
        }

        #region 原来的
        public static void EdgeSwap(TriMesh mesh)
        {
            for (int i = 0; i < mesh.Edges.Count; i++)
            {
                if (mesh.Edges[i].Traits.selectedFlag > 0)
                {
                    EdgeSwap(mesh.Edges[i]);

                }

            }

        }


       


        
        #endregion
    }
}
