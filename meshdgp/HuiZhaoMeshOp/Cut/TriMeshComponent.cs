using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class TriMeshModify
    {
      

        public static List<TriMesh> SeperateComponent(TriMesh mesh)
        {
            List<TriMesh> meshes = new List<TriMesh>();
            Dictionary<int, TriMesh.Vertex> map = new Dictionary<int, HalfEdgeMesh.Vertex>();
            bool[] visited = new bool[mesh.Faces.Count];
            Queue<TriMesh.Face> queue = new Queue<HalfEdgeMesh.Face>();

            TriMesh newMesh = new TriMesh();
            queue.Enqueue(mesh.Faces[0]);
            visited[0] = true;
            while (queue.Count != 0)
            {
                TriMesh.Face face = queue.Dequeue();
                foreach (var hf in face.Halfedges)
                {
                    if (!map.ContainsKey(hf.ToVertex.Index))
                    {
                        TriMesh.Vertex v = 
                            new HalfEdgeMesh.Vertex(new VertexTraits(hf.ToVertex.Traits.Position));
                        newMesh.AppendToVertexList(v);
                        map[hf.ToVertex.Index] = v;
                    }
                    if (hf.Opposite.Face != null && !visited[hf.Opposite.Face.Index])
                    {
                        queue.Enqueue(hf.Opposite.Face);
                        visited[hf.Opposite.Face.Index] = true;
                    }
                }

                newMesh.Faces.AddTriangles(
                    map[face.HalfEdge.FromVertex.Index],
                    map[face.HalfEdge.ToVertex.Index],
                    map[face.HalfEdge.Next.ToVertex.Index]);

                if (queue.Count == 0)
                {
                    
                    meshes.Add(newMesh);

                    for (int i = 0; i < visited.Length; i++)
                    {
                        if (!visited[i])
                        {
                            newMesh = new TriMesh();
                            queue.Enqueue(mesh.Faces[i]);
                            visited[i] = true;
                            break;
                        }
                    }
                }
            }


            foreach (TriMesh child in meshes)
            {
                TriMeshUtil.SetUpNormalVertex(child);
            }
            return meshes ;
        }

        //public static List<TriMesh> SeperateComponentTwo(TriMesh mesh)
        //{
        //    TriMesh.Vertex[] vf;
        //    TriMesh.Edge[] ef;
        //    TriMesh.Face[] ff;
        //    TriMesh.HalfEdge[] nf;
        //    TriMesh.HalfEdge[] of;

            
        //    List<TriMesh> list = new List<TriMesh>();
        //    vf = new TriMesh.Vertex[mesh.Vertices.Count];
        //    ef = new TriMesh.Edge[mesh.Edges.Count];
        //    ff = new TriMesh.Face[mesh.Faces.Count];
        //    nf = new TriMesh.HalfEdge[mesh.HalfEdges.Count];
        //    of = new TriMesh.HalfEdge[mesh.HalfEdges.Count];

        //    foreach (var oldHf in mesh.HalfEdges)
        //    {
        //        if (nf[oldHf.Index] == null)
        //        {
        //            TriMesh newMesh = new TriMesh();
        //            TriMesh.HalfEdge newHf = new HalfEdgeMesh.HalfEdge();
        //            newMesh.AppendToHalfedgeList(newHf);
        //            DFS(oldHf, newHf, newMesh);
        //            list.Add(newMesh);
        //        }
        //    }

        //    foreach (TriMesh child in list)
        //    {
        //        TriMeshOP.SetUpVertexNormal(child);
        //    }
        //    return list ;
        //}

        //private static void DFS(TriMesh.HalfEdge oldHf, TriMesh.HalfEdge newHf, TriMesh newMesh)
        //{
        //    if (this.vf[oldHf.ToVertex.Index] == null)
        //    {
        //        TriMesh.Vertex v = new HalfEdgeMesh.Vertex(new VertexTraits(oldHf.ToVertex.Traits.Position));
        //        newHf.ToVertex = v;
        //        v.HalfEdge = newHf;
        //        newMesh.AppendToVertexList(v);
        //        this.vf[oldHf.ToVertex.Index] = v;
        //    }
        //    else
        //    {
        //        newHf.ToVertex = this.vf[oldHf.ToVertex.Index];
        //    }

        //    if (this.ef[oldHf.Edge.Index] == null)
        //    {
        //        TriMesh.Edge e = new HalfEdgeMesh.Edge();
        //        newHf.Edge = e;
        //        e.HalfEdge0 = newHf;
        //        newMesh.AppendToEdgeList(e);
        //        this.ef[oldHf.Edge.Index] = e;
        //    }
        //    else
        //    {
        //        newHf.Edge = this.ef[oldHf.Edge.Index];
        //    }

        //    if (oldHf.Face != null)
        //    {
        //        if (this.ff[oldHf.Face.Index] == null)
        //        {
        //            TriMesh.Face f = new HalfEdgeMesh.Face();
        //            newHf.Face = f;
        //            f.HalfEdge = newHf;
        //            newMesh.AppendToFaceList(f);
        //            this.ff[oldHf.Face.Index] = f;
        //        }
        //        else
        //        {
        //            newHf.Face = this.ff[oldHf.Face.Index];
        //        }
        //    }

        //    if (this.nf[oldHf.Next.Index] == null)
        //    {
        //        TriMesh.HalfEdge next = new HalfEdgeMesh.HalfEdge();
        //        newHf.Next = next;
        //        next.Previous = newHf;
        //        newMesh.AppendToHalfedgeList(next);
        //        this.nf[oldHf.Next.Index] = next;
        //        this.DFS(oldHf.Next, next, newMesh);
        //    }

        //    if (this.of[oldHf.Opposite.Index] == null)
        //    {
        //        TriMesh.HalfEdge oppo = new HalfEdgeMesh.HalfEdge();
        //        newHf.Opposite = oppo;
        //        oppo.Opposite = newHf;
        //        newMesh.AppendToHalfedgeList(oppo);
        //        this.of[oldHf.Opposite.Index] = oppo;
        //        this.DFS(oldHf.Opposite, oppo, newMesh);
        //    }
        //}
    }
}
