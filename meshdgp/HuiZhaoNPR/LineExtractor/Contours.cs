using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class Contours : LineBase
    {

        //public override TriMesh Extract(double threshold)
        //{
        //    TriMesh lineMesh = new TriMesh();
        //    TriMesh.Vertex[] map = new HalfEdgeMesh.Vertex[mesh.Vertices.Count];          
        //    for (int i = 0; i < mesh.Faces.Count; i++)
        //    {
        //        if (ComputFont(mesh.Faces[i]) < 0)
        //        {
        //            List<TriMesh.Vertex> list = new List<TriMesh.Vertex>();
        //            foreach (var v in mesh.Faces[i].Vertices)
        //            {
        //                if (map[v.Index] == null)
        //                {
        //                    map[v.Index] = lineMesh.Vertices.Add(new VertexTraits(v.Traits.Position));
        //                }
        //                list.Add(map[v.Index]);
        //            }
        //            lineMesh.Faces.AddTriangles(list.ToArray());
        //        }
        //    }

        //    for (int i = 0; i < lineMesh.Edges.Count; i++)
        //    {
        //        if (lineMesh.Edges[i].Face0 == null || lineMesh.Edges[i].Face1 == null)
        //        {
        //            lineMesh.Edges[i].Traits.selectedFlag = 1;
        //        }
        //    }

        //    TriMeshUtil.FixIndex(lineMesh);
        //    TriMeshUtil.SetUpNormalVertex(lineMesh);
        //    return lineMesh;

        //}

        //public override bool[] Extract(TriMesh mesh)
        //{
        //    Vector3D viewPoint = ToolPool.Instance.Tool.ComputeViewPoint();
        //    bool[] flags = new bool[mesh.Edges.Count];
        //    for (int i = 0; i < mesh.Edges.Count; i++)
        //    {
        //        if (LineCurvature.ComputeValue(viewPoint, mesh.Edges[i]) < ConfigNPR.Instance.Contourthreshold)
        //        {
        //            flags[i] = true;
        //        }
        //    }
        //    return flags;
        //}

        public Contours()
        {
            this.Type = EnumLine.Contours;
        }
    }
}
