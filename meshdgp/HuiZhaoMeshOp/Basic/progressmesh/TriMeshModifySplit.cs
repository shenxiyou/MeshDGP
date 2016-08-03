using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class SplitContext
    {
        public TriMesh.Vertex NewVertex;

    }

    public partial class TriMeshModify
    {
        private static TriMesh.HalfEdge[] FindGroup(TriMesh.Vertex v, TriMesh.Vertex begin, TriMesh.Vertex end)
        {
            List<TriMesh.HalfEdge> group = new List<TriMesh.HalfEdge>();

            TriMesh.HalfEdge start = v.FindHalfedgeTo(begin);
            group.Add(start);
            TriMesh.HalfEdge current = start;

            while (current.ToVertex != end)
            {
                current = current.Opposite.Next;
                group.Add(current);
            }

            return group.ToArray();
        }

        private static void ConnectHalfEdge(params TriMesh.HalfEdge[] hfs)
        {
            for (int i = 0; i < hfs.Length; i++)
            {
                hfs[i].Next = hfs[(i + 1) % 3];
                hfs[i].Previous = hfs[(i + 2) % 3];
            }
        }

        private static TriMesh.HalfEdge[] AddInnerTriangle(TriMesh mesh, params TriMesh.Vertex[] verteces)
        {
            TriMesh.Face face = new TriMesh.Face();
            mesh.AppendToFaceList(face);

            TriMesh.HalfEdge[] hfs = new TriMesh.HalfEdge[3];
            for (int i = 0; i < hfs.Length; i++)
            {
                hfs[i] = new TriMesh.HalfEdge();
                hfs[i].ToVertex = verteces[(i + 1) % hfs.Length];
                hfs[i].Face = face;
                mesh.AppendToHalfedgeList(hfs[i]);
            }
            face.HalfEdge = hfs[0];
            ConnectHalfEdge(hfs);
            return hfs;
        }

        private static void InsertEdge(TriMesh mesh, TriMesh.HalfEdge inner, TriMesh.HalfEdge outer)
        {
            //TriMesh.Edge left = new TriMesh.Edge();
            //left.HalfEdge0 = outer;
            //outer.Edge = left;
            //inner.Next.Edge = left;

            //TriMesh.Edge right = outer.Edge;
            //right.HalfEdge0 = inner;
            //inner.Edge = right;
            //outer.Opposite.Edge = right;

            //inner.Opposite = outer.Opposite;
            //inner.Next.Opposite = outer;
            //outer.Opposite.Opposite = inner;
            //outer.Opposite = inner.Next;
            inner.Opposite = outer.Opposite;
            inner.Next.Opposite = outer;
            outer.Opposite.Opposite = inner;
            outer.Opposite = inner.Next;
            outer.Edge.HalfEdge0 = outer;

            TriMesh.Edge edge = new TriMesh.Edge();
            edge.HalfEdge0 = inner;
            inner.Edge = edge;
            inner.Opposite.Edge = edge;
            inner.Next.Edge = outer.Edge;

            mesh.AppendToEdgeList(edge);
        }

        public static TriMesh.Vertex VertexSplit(TriMesh.Vertex v1, TriMesh.Vertex share1,
            TriMesh.Vertex share2, Vector3D v1Position, Vector3D v2Position, int fixedIndex)
        {
            TriMesh.HalfEdge[] hfs = FindGroup(v1, share1, share2);

            TriMesh mesh = (TriMesh)v1.Mesh;

            v1.Traits.Position = v1Position;
            v1.HalfEdge = hfs[0];

            TriMesh.Vertex v2 = new TriMesh.Vertex();
            v2.Traits = new VertexTraits(v2Position);
            v2.Traits.FixedIndex = fixedIndex;
            v2.HalfEdge = hfs[1];
            mesh.AppendToVertexList(v2);

            for (int i = 0; i < hfs.Length - 1; i++)
            {
                hfs[i].Opposite.ToVertex = v2;
            }

            TriMesh.HalfEdge[] triangle1 = AddInnerTriangle(mesh, v1, v2, share1);
            InsertEdge(mesh, triangle1[1], hfs[0]);

            TriMesh.HalfEdge[] triangle2 = AddInnerTriangle(mesh, v2, v1, share2);
            InsertEdge(mesh, triangle2[1], hfs[hfs.Length - 1]);

            TriMesh.Edge edge = new TriMesh.Edge();
            edge.HalfEdge0 = triangle1[0];
            triangle1[0].Edge = edge;
            triangle2[0].Edge = edge;
            triangle1[0].Opposite = triangle2[0];
            triangle2[0].Opposite = triangle1[0];
            mesh.AppendToEdgeList(edge);

            return v2;
        }

        public static TriMesh.Vertex VertexSplit(TriMesh.Vertex v1, TriMesh.Vertex share1,
            TriMesh.Vertex share2, Vector3D v1Position, Vector3D v2Position)
        {
            return VertexSplit(v1, share1, share2, v1Position, v2Position, 0);
        }

        public static TriMesh.Vertex VertexSplit(TriMesh.Vertex v, TriMesh.Vertex vshard1, TriMesh.Vertex vshard2)
        {
            TriMesh.HalfEdge[] leftGroup = FindGroup(v, vshard1, vshard2);
            TriMesh.HalfEdge[] rightGroup = FindGroup(v, vshard2, vshard1);
            TriMesh.Vertex leftMid = leftGroup[leftGroup.Length / 2].ToVertex;
            TriMesh.Vertex rightMid = rightGroup[rightGroup.Length / 2].ToVertex;
            Vector3D leftPosition = v.Traits.Position * 0.7 + leftMid.Traits.Position * 0.3;
            Vector3D rightPosition = v.Traits.Position * 0.7 + rightMid.Traits.Position * 0.3;
            return VertexSplit(v, vshard1, vshard2, rightPosition, leftPosition);
        }

        public static TriMesh.Vertex VertexSplit(TriMesh.Vertex v)
        {
            TriMesh.Vertex[] arr = Extensions.ToArray(v.Vertices);
            return VertexSplit(v, arr[0], arr[arr.Length / 2]);
        }

        public static TriMesh.HalfEdge Split(TriMesh.HalfEdge hf, Vector3D v1Pos, Vector3D v2Pos)
        {
            TriMesh mesh = (TriMesh)hf.Mesh;

            List<TriMesh.HalfEdge> list = new List<HalfEdgeMesh.HalfEdge>();
            TriMesh.HalfEdge cur = hf;
            list.Add(cur);
            do
            {
                cur = cur.Opposite.Next;
                list.Add(cur);
            } while (cur.Opposite.Face != null && cur != hf);

           
            for (int i = 1; i < list.Count; i++)
            {
                TriMeshModify.RemoveEdge(list[i].Edge);
            }
            hf.FromVertex.Traits.Position = v1Pos;
            TriMesh.Vertex v2 = new HalfEdgeMesh.Vertex(new VertexTraits(v2Pos));
            mesh.AppendToVertexList(v2);

            for (int i = 1; i < list.Count; i++)
            {
                mesh.Faces.AddTriangles(list[i - 1].ToVertex, v2, list[i].ToVertex);
            }
            mesh.Faces.AddTriangles(list[0].ToVertex, hf.FromVertex, v2);
            return hf.FromVertex.FindHalfedgeTo(v2);
        }

         public static void VertexSplit(TriMesh mesh)
        {
            List<TriMesh.Vertex> newvertex = new List<HalfEdgeMesh.Vertex>();
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                if (mesh.Vertices[i].Traits.SelectedFlag != 0)
                {
                    TriMesh.Vertex vertex = VertexSplit(mesh.Vertices[i]);
                    newvertex.Add(vertex);
                }
            }

            foreach (TriMesh.Vertex vertex in newvertex)
            {
                vertex.Traits.selectedFlag = 1;
            }
            TriMeshUtil.FixIndex(mesh);
            TriMeshUtil.SetUpNormalVertex(mesh);
        }


        #region 原来的
        public TriMesh.Vertex VertexSplit1(TriMesh.Vertex v, TriMesh.Vertex vshard1, TriMesh.Vertex vshard2, Vector3D v1Position, Vector3D v2Position, int v2FixedIndex)
        {
            //1.Get two group of verties
            TriMesh.HalfEdge[] processGroup = FindGroup(v, vshard1, vshard2);

            TriMesh mesh = (TriMesh)v.Mesh;
            TriMesh.Vertex v1 = null;
            TriMesh.Vertex v2 = null;
            TriMesh.Vertex newVertex = null;

            v1 = v;
            v.Traits.Position = v1Position;
            v2 = new TriMesh.Vertex();
            v2.Traits = new VertexTraits(Vector3D.Zero);
            newVertex = v2;
            newVertex.Traits.FixedIndex = v2FixedIndex;
            v2.Mesh = v.Mesh;
            v2.Traits.Position = v2Position;

            //2.Process the Topology
            TriMesh.HalfEdge hf1Origin = processGroup[0];
            TriMesh.HalfEdge hf2Origin = processGroup[processGroup.Length - 1];

            //Add new edge
            TriMesh.HalfEdge hf3 = new TriMesh.HalfEdge();
            TriMesh.HalfEdge hf3Oppsite = new TriMesh.HalfEdge();
            TriMesh.Edge edge = new TriMesh.Edge();

            hf3.Opposite = hf3Oppsite;
            hf3Oppsite.Opposite = hf3;

            edge.HalfEdge0 = hf3;
            edge.HalfEdge1 = hf3Oppsite;
            hf3.Edge = edge;
            hf3Oppsite.Edge = edge;

            hf3.ToVertex = v2;
            hf3Oppsite.ToVertex = v1;

            //Handle hf1Origin which is outter hafledge [INNER]
            TriMesh.HalfEdge hf1 = new TriMesh.HalfEdge();
            hf1.Opposite = hf1Origin;
            hf1.ToVertex = v1;

            TriMesh.HalfEdge hf1Other = new TriMesh.HalfEdge();
            hf1Other.Opposite = hf1Origin.Opposite;
            hf1Other.ToVertex = hf1Origin.ToVertex;

            hf1.Previous = hf1Other;
            hf1Other.Next = hf1;

            hf1.Next = hf3;
            hf3.Previous = hf1;
            hf1Other.Previous = hf3;
            hf3.Next = hf1Other;

            //Handle hf2Origin which is inner hafledge [INNER]
            TriMesh.HalfEdge hf2 = new TriMesh.HalfEdge();
            hf2.Opposite = hf2Origin;
            hf2.ToVertex = v2;

            TriMesh.HalfEdge hf2Other = new TriMesh.HalfEdge();
            hf2Other.Opposite = hf2Origin.Opposite;
            hf2Other.ToVertex = hf2Origin.ToVertex;

            hf2.Previous = hf2Other;
            hf2Other.Next = hf2;

            hf2.Next = hf3Oppsite;
            hf3Oppsite.Previous = hf2;
            hf2Other.Previous = hf3Oppsite;
            hf3Oppsite.Next = hf2Other;


            TriMesh.Face face1 = new TriMesh.Face();
            TriMesh.Face face2 = new TriMesh.Face();

            face1.HalfEdge = hf3;
            hf3.Face = face1;
            hf1.Face = face1;
            hf1Other.Face = face1;

            face2.HalfEdge = hf3Oppsite;
            hf3Oppsite.Face = face2;
            hf2.Face = face2;
            hf2Other.Face = face2;

            //Process the outside
            TriMesh.Edge edge1 = new TriMesh.Edge();
            TriMesh.HalfEdge hf1OriginOppsite = hf1Origin.Opposite;

            hf1Origin.Opposite = hf1;
            hf1.Edge = hf1Origin.Edge;

            hf1OriginOppsite.Opposite = hf1Other;
            hf1OriginOppsite.ToVertex = v2;
            hf1OriginOppsite.Edge = edge1;
            hf1Other.Edge = edge1;
            edge1.HalfEdge0 = hf1Other;
            edge1.HalfEdge1 = hf1OriginOppsite;

            TriMesh.Edge edge2 = new TriMesh.Edge();
            TriMesh.HalfEdge hf2OriginOppsite = hf2Origin.Opposite;

            hf2Origin.Opposite = hf2;
            hf2.Edge = hf2Origin.Edge;

            hf2OriginOppsite.Opposite = hf2Other;
            hf2OriginOppsite.ToVertex = v1;
            hf2OriginOppsite.Edge = edge2;
            hf2Other.Edge = edge2;
            edge2.HalfEdge0 = hf2Other;
            edge2.HalfEdge1 = hf2OriginOppsite;

            v1.HalfEdge = hf1Origin;
            v2.HalfEdge = hf2Origin;

            mesh.AppendToEdgeList(edge);
            mesh.AppendToEdgeList(edge1);
            mesh.AppendToEdgeList(edge2);
            mesh.AppendToFaceList(face1);
            mesh.AppendToFaceList(face2);
            mesh.AppendToHalfedgeList(hf1);
            mesh.AppendToHalfedgeList(hf1Other);
            mesh.AppendToHalfedgeList(hf2);
            mesh.AppendToHalfedgeList(hf2Other);
            mesh.AppendToHalfedgeList(hf3);
            mesh.AppendToHalfedgeList(hf3Oppsite);
            mesh.AppendToVertexList(newVertex);

            for (int i = 1; i < processGroup.Length - 1; i++)
            {
                processGroup[i].Opposite.ToVertex = newVertex;
            }


            //mesh.FixIndex();

            return newVertex;
        }
        #endregion
    }
}
