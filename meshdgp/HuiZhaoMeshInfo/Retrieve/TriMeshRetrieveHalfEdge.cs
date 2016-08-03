using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public partial class TriMeshUtil
    {


        public static bool[] CollectVertex(TriMesh mesh, EnumPatchType type)
        {
            bool[] selectedFlags = new bool[mesh.Vertices.Count];
            switch (type)
            {
                case EnumPatchType.Vertex:
                    foreach (var v in mesh.Vertices)
                    {
                        if (v.Traits.SelectedFlag > 0)
                        {
                            selectedFlags[v.Index] = true;
                        }
                    }
                    break;
                case EnumPatchType.Edge:
                    foreach (var e in mesh.Edges)
                    {
                        if (e.Traits.SelectedFlag > 0)
                        {
                            selectedFlags[e.Vertex0.Index] = true;
                            selectedFlags[e.Vertex1.Index] = true;
                        }
                    }
                    break;
                case EnumPatchType.Face:
                    foreach (var f in mesh.Faces)
                    {
                        if (f.Traits.SelectedFlag > 0)
                        {
                            foreach (var v in f.Vertices)
                            {
                                selectedFlags[v.Index] = true;
                            }
                        }
                    }
                    break;
                default:
                    return null;
            }
            return selectedFlags;
        }

        public static bool[] CollectFace(TriMesh mesh)
        {
            bool[] selectedFlags = new bool[mesh.Faces.Count];
            foreach (var f in mesh.Faces)
            {
                if (f.Traits.SelectedFlag > 0)
                {
                    selectedFlags[f.Index] = true;
                }
            }
            return selectedFlags;
        }

        public static TriMesh.HalfEdge GetStart(TriMesh mesh, bool[] selectedFlags)
        {
            foreach (TriMesh.HalfEdge hf in mesh.HalfEdges)
            {
                int from = hf.FromVertex.Index;
                int to = hf.ToVertex.Index;
                int next = hf.Next.ToVertex.Index;
                if (selectedFlags[from] && selectedFlags[to] && !selectedFlags[next])
                {
                    return hf;
                }
            }
            return null;
        }

        public static List<TriMesh.HalfEdge> RetrieveOneRingEdgeOfEdge(TriMesh.Edge edge)
        {
            TriMesh mesh = (TriMesh)edge.Mesh;
            List<TriMesh.HalfEdge> list = new List<TriMesh.HalfEdge>();
            TriMesh.HalfEdge hf0 = edge.HalfEdge0;
            TriMesh.HalfEdge currentHalfedge = hf0.Next.Opposite;
            while (currentHalfedge.Next.Opposite != hf0)
            {
                if (!currentHalfedge.OnBoundary)
                {
                    list.Add(currentHalfedge.Previous);
                }
                currentHalfedge = currentHalfedge.Next.Opposite;
            };
            TriMesh.HalfEdge hf1 = edge.HalfEdge1;
            currentHalfedge = hf1.Next.Opposite;
            while (currentHalfedge.Next.Opposite != hf1)
            {
                if (!currentHalfedge.OnBoundary)
                {
                    list.Add(currentHalfedge.Previous);
                }
                currentHalfedge = currentHalfedge.Next.Opposite;
            };
            return list;
        }

        public static List<TriMesh.Vertex> RetrieveOneRingVertexOfEdge(TriMesh.Edge edge)
        {
            List<TriMesh.HalfEdge> oneRingEdges =  RetrieveOneRingEdgeOfEdge(edge);

            List<TriMesh.Vertex> oneRingVertice = new List<TriMesh.Vertex>();
            for (int i = 0; i < oneRingEdges.Count; i++)
            {
                oneRingVertice.Add(oneRingEdges[i].ToVertex);
            }
            return oneRingVertice;
        } 


        public static TriMesh.HalfEdge[] RetrieveOneRingHalfEdgeByPatch(TriMesh mesh, bool[] selectedFlags)
        {
            List<TriMesh.HalfEdge> boundaryHFs = new List<TriMesh.HalfEdge>();
            TriMesh.HalfEdge start =  GetStart(mesh, selectedFlags); 
            if (start == null)
            {
                return boundaryHFs.ToArray();
            } 
            TriMesh.HalfEdge cur = start;
            do
            { 
                do
                {
                    if (!selectedFlags[cur.FromVertex.Index])
                    {
                        boundaryHFs.Add(cur.Previous);
                    }

                    cur = cur.Next.Opposite;

                } while (!(selectedFlags[cur.Next.FromVertex.Index]
                        && selectedFlags[cur.Next.ToVertex.Index]));

                do
                {
                    cur = cur.Next;

                } while (cur.OnBoundary &&
                        selectedFlags[cur.Next.ToVertex.Index] &&
                        selectedFlags[cur.Next.FromVertex.Index]);
            } while (cur != start);

            return boundaryHFs.ToArray();
        }

        public static TriMesh.HalfEdge[] RetrieveBoundaryHalfEdgeByPatch(TriMesh mesh, bool[] selectedFlags)
        {
            List<TriMesh.HalfEdge> boundaryHFs = new List<TriMesh.HalfEdge>();
            TriMesh.HalfEdge start =  GetStart(mesh, selectedFlags); 
            if (start == null)
            {
                return boundaryHFs.ToArray();
            } 
            TriMesh.HalfEdge cur = start; 
            do
            { 
                do
                {
                    cur = cur.Next.Opposite;

                } while (!(selectedFlags[cur.Next.FromVertex.Index]
                        && selectedFlags[cur.Next.ToVertex.Index]));

                do
                {
                    cur = cur.Next;
                    boundaryHFs.Add(cur);

                } while (cur.OnBoundary &&
                        selectedFlags[cur.Next.ToVertex.Index] &&
                        selectedFlags[cur.Next.FromVertex.Index]);
            } while (cur != start);

            return boundaryHFs.ToArray();
        }

        /// <summary>
        /// 逆时针一圈
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="faceFlags"></param>
        /// <returns></returns>
        public static TriMesh.HalfEdge[] RetrieveRegionBoundaryHalfEdge(TriMesh mesh, bool[] faceFlags)
        {
            TriMesh.HalfEdge start = null;
            foreach (var hf in mesh.HalfEdges)
            {
                if ( IsBoundary(hf, faceFlags))
                {
                    start = hf;
                    break;
                }
            } 
            List<TriMesh.HalfEdge> list = new List<HalfEdgeMesh.HalfEdge>();
            TriMesh.HalfEdge cur = start;
            do
            {
                list.Add(cur);
                foreach (var hf in cur.ToVertex.HalfEdges)
                {
                    if ( IsBoundary(hf, faceFlags))
                    {
                        cur = hf;
                        break;
                    }
                }
            } while (cur != start); 
            return list.ToArray();
        }

        public static bool IsBoundary(TriMesh.HalfEdge hf, bool[] faceFlags)
        {
            TriMesh.Face left = hf.Face;
            TriMesh.Face right = hf.Opposite.Face;
            if (left == null)
            {
                return false;
            }
            if (right == null)
            {
                return faceFlags[left.Index];
            }
            return faceFlags[left.Index] && !faceFlags[right.Index];
        }

        public static IEnumerable<TriMesh.Face> RetrieveOneRingFaceOfFace(TriMesh.Face center)
        {
            TriMesh.HalfEdge start = center.HalfEdge;
            TriMesh.HalfEdge hf = start;
            while (hf != start.Opposite)
            {
                hf = hf.Opposite.Next;
                if (hf.Face != null)
                {
                    yield return hf.Face;
                }
                if (hf == start.Next.Opposite)
                {
                    hf = start.Next;
                }
                if (hf == start.Previous.Opposite)
                {
                    hf = start.Previous;
                }
            }
        }
    }
}
