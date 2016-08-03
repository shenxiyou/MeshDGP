using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public static class TriMeshDual2
    {
        public static PolygonMesh BuildDual(TriMesh mesh, EnumDual type)
        {
            TriMeshModify.RepaireAllHole(mesh);

            PolygonMesh.Vertex[] faceMap = new HalfEdgeMesh.Vertex[mesh.Faces.Count];
            PolygonMesh.Vertex[] edgeMap = new HalfEdgeMesh.Vertex[mesh.Edges.Count];
            PolygonMesh pm = new PolygonMesh();

            foreach (var face in mesh.Faces)
            {
                Vector3D center = Vector3D.Zero;
                switch (type)
                {
                    case EnumDual.DualA:
                        center = TriMeshUtil.GetMidPoint(face);
                        break;
                    case EnumDual.DualB:
                        TriMesh.Vertex vertex0 = face.GetVertex(0);
                        TriMesh.Vertex vertex1 = face.GetVertex(1);
                        TriMesh.Vertex vertex2 = face.GetVertex(2);
                        Triangle triangle = new Triangle(vertex0.Traits.Position, 
                                                         vertex1.Traits.Position, 
                                                         vertex2.Traits.Position);
                        center = triangle.ComputeCircumCenter();
                        break;
                    default:
                        break;
                }
                PolygonMesh.Vertex v = new HalfEdgeMesh.Vertex(new VertexTraits(center));
                faceMap[face.Index] = v;
                pm.AppendToVertexList(v);
            }

            foreach (var edge in mesh.Edges)
            {
                VertexTraits trait = new VertexTraits(TriMeshUtil.GetMidPoint(edge));
                PolygonMesh.Vertex v = new HalfEdgeMesh.Vertex(trait);
                edgeMap[edge.Index] = v;
                pm.AppendToVertexList(v);
            }

            foreach (var v in mesh.Vertices)
            {
                List<PolygonMesh.Vertex> list = new List<HalfEdgeMesh.Vertex>();
                foreach (var hf in v.HalfEdges)
                {
                    list.Add(faceMap[hf.Face.Index]);
                    list.Add(edgeMap[hf.Edge.Index]);
                }
                list.Reverse();
                pm.Faces.Add(list.ToArray());
            }
            return pm;
        }
    }
}
