using System;
using System.Collections.Generic;
 
using System.Text;
 


namespace GraphicResearchHuiZhao
{

    public partial class TriMeshModify 
    {
        public static  void MergeEdge(TriMesh mesh)
        {
            for (int i = 0; i < mesh.Edges.Count; i++)
            {
                if (mesh.Edges[i].Traits.SelectedFlag != 0)
                {
                    if (IsMergeable(mesh.Edges[i]))
                    {
                        MergeEdge(mesh.Edges[i]);
                        i = Math.Max(0, i - 3);
                    }
                    else
                    {
                        mesh.Edges[i].Traits.SelectedFlag = 1;
                    }
                }
            }
            TriMeshUtil.FixIndex(mesh);
            TriMeshUtil.SetUpNormalVertex(mesh);
        }

        public static TriMesh.Vertex MergeEdge(TriMesh.Edge edge)
        {
            Vector3D position = TriMeshUtil.GetMidPoint(edge);
            return MergeEdge(edge, position);
        }

        public static TriMesh.Vertex MergeEdge(TriMesh.Edge edge, Vector3D position)
        {
            TriMesh mesh = (TriMesh)edge.Mesh;

            TriMesh.Vertex v0 = edge.Vertex0;
            TriMesh.Vertex v1 = edge.Vertex1;
            TriMesh.HalfEdge hf0 = edge.HalfEdge0;
            TriMesh.HalfEdge hf1 = edge.HalfEdge1;

            v0.Traits.Position = position;

            foreach (var item in v1.HalfEdges)
            {
                //if (item.ToVertex != v0)
                {
                    item.Opposite.ToVertex = v0;
                }
            }

            MergeOneSide(hf0);
            MergeOneSide(hf1);

            mesh.RemoveVertex(v1);
            mesh.RemoveEdge(edge);
            v1.HalfEdge = null;
            edge.HalfEdge0 = null;
            
            return v0;
        }

        private static void MergeOneSide(TriMesh.HalfEdge hf)
        {
            TriMesh mesh = (TriMesh)hf.Mesh;
            if (hf.OnBoundary)
            {
                hf.ToVertex.HalfEdge = hf.Next;
                hf.Previous.Next = hf.Next;
                hf.Next.Previous = hf.Previous;
            }
            else
            {
                TriMesh.Edge remain = hf.Next.Edge;
                TriMesh.Edge remove = hf.Previous.Edge;
                TriMesh.HalfEdge outerLeft = hf.Previous.Opposite;
                TriMesh.HalfEdge outerRight = hf.Next.Opposite;

                if (outerLeft.Next == outerRight || outerLeft.Previous == outerRight)
                {
                    outerLeft.Edge.Traits.SelectedFlag = 1;
                    hf.FromVertex.Traits.SelectedFlag = 1;
                    outerLeft.ToVertex.Traits.SelectedFlag = 1;
                    //throw new Exception();
                }

                remain.HalfEdge0 = outerRight;
                outerLeft.Edge = remain;

                outerLeft.Opposite = outerRight;
                outerRight.Opposite = outerLeft;

                hf.ToVertex.HalfEdge = outerRight.Next;
                //hf.ToVertex.HalfEdge = outerLeft;
                TriMesh.Vertex top = hf.Next.ToVertex;
                top.HalfEdge = outerRight;

                mesh.RemoveFace(hf.Face);
                mesh.RemoveHalfedge(hf.Previous);
                mesh.RemoveHalfedge(hf.Next);
                mesh.RemoveEdge(remove);

                hf.Previous.Previous = null;
                hf.Previous.Next = null;
                hf.Previous.Opposite = null;
                hf.Next.Previous = null;
                hf.Next.Next = null;
                hf.Next.Opposite = null;
                remove.HalfEdge0 = null;
            }
            mesh.RemoveHalfedge(hf);
            hf.ToVertex = null;
            hf.Next = null;
            hf.Previous = null;
            hf.Opposite = null;
            hf.Face = null;
        }

        public static bool IsMergeable(TriMesh.Edge edge)
        {
            TriMesh.Vertex top = edge.HalfEdge0.Next.ToVertex;
            TriMesh.Vertex buttom = edge.HalfEdge1.Next.ToVertex;
            //非流形：三面共边
            foreach (var v in edge.Vertex0.Vertices)
            {
                foreach (var v2 in edge.Vertex1.Vertices)
                {
                    if (v == v2 && v != top && v != buttom)
                    {
                        return false;
                    }
                }
            }

            bool v0OnBoundary = false;
            bool v1OnBoundary = false;
            //非流形：一点相连
            foreach (var hf in edge.Vertex0.HalfEdges)
            {
                if (hf.OnBoundary && hf != edge.HalfEdge1 && hf != edge.HalfEdge0.Next)
                {
                    v0OnBoundary = true;
                }
            }

            foreach (var hf in edge.Vertex1.HalfEdges)
            {
                if (hf.OnBoundary && hf != edge.HalfEdge0 && hf != edge.HalfEdge1.Next)
                {
                    v1OnBoundary = true;
                }
            }

            if (v0OnBoundary && v1OnBoundary)
            {
                return false;
            }
            //减少一个洞
            if (edge.HalfEdge0.Next.Next.ToVertex == edge.Vertex1 && edge.HalfEdge0.Face == null)
            {
                return false;
            }

            if (edge.HalfEdge1.Next.Next.ToVertex == edge.Vertex0 && edge.HalfEdge1.Face == null)
            {
                return false;
            }
            //正四面体
            List<TriMesh.HalfEdge> hfs = new List<TriMesh.HalfEdge>();
            foreach (var item in edge.Vertex0.HalfEdges)
            {
                hfs.Add(item.Next.Opposite);
            }

            if (hfs[0].Next == hfs[1] && hfs[1].Next == hfs[2] && hfs[2].Next == hfs[0])
            {
                return false;
            }

            return true;
        }

        public static void MergeTriangle(TriMesh mesh)
        {
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                if (mesh.Faces[i].Traits.SelectedFlag != 0)
                {
                    if (IsMergeable(mesh.Faces[i]))
                    {
                        MergeTriangle(mesh.Faces[i]);
                        i = Math.Max(0, i - 3);
                    }
                    else
                    {
                        mesh.Faces[i].Traits.SelectedFlag = 1;
                    }
                }
            }
            TriMeshUtil.FixIndex(mesh);
            TriMeshUtil.SetUpNormalVertex(mesh);
             
        }

        public static TriMesh.Vertex MergeTriangle(TriMesh.Face face)
        {
            Vector3D pos = TriMeshUtil.GetMidPoint(face);
            return MergeTriangle(face, pos);
        }

        public static TriMesh.Vertex MergeTriangle(TriMesh.Face face, Vector3D pos)
        {
            TriMesh.Edge edge1 = face.HalfEdge.Edge;
            TriMesh.Edge edge2 = face.HalfEdge.Next.Edge;
            TriMesh.Vertex v = MergeEdge(edge1, pos);
            if (IsMergeable(edge2))
            {
                v = MergeEdge(edge2, pos);
            }
            else
            {
                edge2.Traits.SelectedFlag = 1;
            }
            return v;
        }

        public static bool IsMergeable(TriMesh.Face face)
        {
            if (face.OnBoundary)
            {
                //return false;
            }
            foreach (var hf in face.Halfedges)
            {
                if (!TriMeshModify.IsMergeable(hf.Edge))
                {
                    return false;
                }
            }
            return true;
        }

        #region 原来的

        public bool IsMergeable1(TriMesh.Edge edge)
        {
            TriMesh.Vertex v0 = edge.Vertex0;
            TriMesh.Vertex v1 = edge.Vertex1;
            //Check Verteices
            List<TriMesh.Vertex> LkAandBVertex = new List<TriMesh.Vertex>();
            foreach (TriMesh.Vertex va in v0.Vertices)
            {
                foreach (TriMesh.Vertex vb in v1.Vertices)
                {
                    if (va == vb)
                    {
                        LkAandBVertex.Add(va);
                    }
                }
            }
            //Check Edge
            List<TriMesh.Edge> LkAandBEdge = new List<TriMesh.Edge>();
            TriMesh.Edge temp;
            foreach (TriMesh.Vertex v in LkAandBVertex)
            {
                foreach (TriMesh.Vertex v2 in LkAandBVertex)
                {
                    temp = null;

                    if (v == v2) continue;
                    if ((temp = v.FindEdgeTo(v2)) != null)
                    {
                        LkAandBEdge.Add(temp);
                    }
                }
            }
            //Check ab
            TriMesh.HalfEdge atob = edge.HalfEdge0;
            TriMesh.HalfEdge btoa = edge.HalfEdge1;

            TriMesh.Vertex abn = atob.Next.ToVertex;
            TriMesh.Vertex ban = btoa.Next.ToVertex;
            List<TriMesh.Vertex> abV = new List<TriMesh.Vertex>();
            abV.Add(abn);
            abV.Add(ban);
            if (abV.Count != LkAandBVertex.Count)
            {
                return false;
            }
            else
            {
                int counter2 = 0;
                foreach (TriMesh.Vertex v in LkAandBVertex)
                {
                    foreach (TriMesh.Vertex v2 in abV)
                    {
                        if (v == v2) break;
                        if (counter2 == abV.Count - 1)
                        {
                            return false;
                        }
                    }
                    counter2 = 0;
                }
            }
            return true;
        }

        public TriMesh.Vertex MergeEdge1(TriMesh.Edge edge, Vector3D position)
        {
            TriMesh mesh = (TriMesh)edge.Mesh;

            TriMesh.Vertex v1 = null;
            TriMesh.Vertex v2 = null;
            if (edge.HalfEdge0 == null || edge.HalfEdge1 == null)
            {
                throw new Exception("Error!");
            }
            if (edge.HalfEdge0.ToVertex == null || edge.HalfEdge1.ToVertex == null)
            {
                throw new Exception("Error!");
            }
            else
            {
                v1 = edge.HalfEdge0.ToVertex;
                v2 = edge.HalfEdge1.ToVertex;
                if (v1.HalfEdge == null || v2.HalfEdge == null)
                {
                    throw new Exception("Error!");
                }
            }
            int count = 0;
            foreach (TriMesh.HalfEdge hf1 in v1.HalfEdges)
            {
                foreach (TriMesh.HalfEdge hf2 in v2.HalfEdges)
                {
                    if (hf1.ToVertex == hf2.ToVertex)
                    {
                        count++;
                    }
                }
            }
            //Find Halfedge which share same to Vertex
            v1.Traits.Position.x = position.x;
            v1.Traits.Position.y = position.y;
            v1.Traits.Position.z = position.z;
            TriMesh.HalfEdge v1Tov2 = null;
            TriMesh.HalfEdge v2Tov1 = null;
            if (edge.HalfEdge0.ToVertex == v2)
            {
                v1Tov2 = edge.HalfEdge0;
                v2Tov1 = edge.HalfEdge1;
            }
            else if (edge.HalfEdge0.ToVertex == v1)
            {
                v2Tov1 = edge.HalfEdge0;
                v1Tov2 = edge.HalfEdge1;
            }
            foreach (TriMesh.HalfEdge hf1 in v1.HalfEdges)
            {
                if (hf1.ToVertex != v2)
                {
                    v1.HalfEdge = hf1;
                    break;
                }
            }
            List<TriMesh.HalfEdge> v2Neibors = new List<TriMesh.HalfEdge>();
            foreach (TriMesh.HalfEdge hfitem in v2.HalfEdges)
            {
                if (hfitem.ToVertex != v1)
                {
                    v2Neibors.Add(hfitem);
                }
            }
            foreach (TriMesh.HalfEdge hfitem in v2Neibors)
            {
                hfitem.Opposite.ToVertex = v1;
            }
            if (!v1Tov2.OnBoundary)
            {
                TriMesh.HalfEdge v1HF1 = v1Tov2.Previous.Opposite;
                TriMesh.HalfEdge v2HF1 = v1Tov2.Next.Opposite;

                v1HF1.Opposite.Next = v2HF1.Next;
                v2HF1.Next.Previous = v1HF1.Opposite;
                v1HF1.Opposite.Previous = v2HF1.Previous;
                v2HF1.Previous.Next = v1HF1.Opposite;
                v1HF1.Opposite.ToVertex = v1;
                v1HF1.Opposite.Face = v2HF1.Face;
                if (v2HF1.Face != null)
                {
                    v1HF1.Opposite.Face.HalfEdge = v1HF1.Opposite;
                }
                v1HF1.ToVertex.HalfEdge = v1HF1.Opposite;
                mesh.RemoveHalfedge(v2HF1);
                v2HF1.Next = null;
                v2HF1.Previous = null;

                mesh.RemoveHalfedge(v2HF1.Opposite);
                v2HF1.Opposite.Opposite = null;
                v2HF1.Opposite.Next = null;
                v2HF1.Opposite.Previous = null;
                v2HF1.Opposite = null;
                mesh.RemoveHalfedge(v1Tov2);
                v1Tov2.Next = null;
                v1Tov2.Previous = null;
                mesh.RemoveEdge(v2HF1.Edge);
                v2HF1.Edge.HalfEdge0 = null;
                mesh.RemoveFace(v1Tov2.Face);
                v1Tov2.Face = null;
            }
            else if (v1Tov2.OnBoundary)
            {
                v1Tov2.Next.Previous = v1Tov2.Previous;
                v1Tov2.Previous.Next = v1Tov2.Next;
            }
            if (!v2Tov1.OnBoundary)
            {
                TriMesh.HalfEdge v1HF2 = v2Tov1.Next.Opposite;
                TriMesh.HalfEdge v2HF2 = v2Tov1.Previous.Opposite;
                v1HF2.Opposite.Next = v2HF2.Next;
                v2HF2.Next.Previous = v1HF2.Opposite;
                v1HF2.Opposite.Previous = v2HF2.Previous;
                v2HF2.Previous.Next = v1HF2.Opposite;
                v1HF2.ToVertex = v1;
                v1HF2.Opposite.Face = v2HF2.Face;
                if (v2HF2.Face != null)
                {
                    v1HF2.Opposite.Face.HalfEdge = v1HF2.Opposite;
                }
                v2HF2.ToVertex.HalfEdge = v1HF2;
                mesh.RemoveHalfedge(v2HF2);
                v2HF2.Next = null;
                v2HF2.Previous = null;
                mesh.RemoveHalfedge(v2HF2.Opposite);
                v2HF2.Opposite.Opposite = null;
                v2HF2.Opposite.Next = null;
                v2HF2.Opposite.Previous = null;
                v2HF2.Opposite = null;
                mesh.RemoveHalfedge(v2Tov1);
                v2Tov1.Next = null;
                v2Tov1.Previous = null;
                v1Tov2.Opposite = null;
                v2Tov1.Opposite = null;
                mesh.RemoveEdge(v2HF2.Edge);
                v2HF2.Edge.HalfEdge0 = null;
                mesh.RemoveFace(v2Tov1.Face);
                v2Tov1.Face = null;
            }
            else if (v2Tov1.OnBoundary)
            {
                v2Tov1.Previous.Next = v2Tov1.Next;
                v2Tov1.Next.Previous = v2Tov1.Previous;
            }
            mesh.RemoveEdge(v2Tov1.Edge);
            v2Tov1.Edge.HalfEdge0 = null;
            mesh.RemoveVertex(v2);
            v2.HalfEdge = null;
            // FixIndex();
            return v1;
        }

        #endregion
    }
}
