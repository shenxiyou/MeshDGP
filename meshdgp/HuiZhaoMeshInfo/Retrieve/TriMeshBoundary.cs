


using System;
using System.Collections.Generic; 
using System.Text;
 

namespace GraphicResearchHuiZhao
{
   
    public partial class TriMeshUtil
    {

        public static List<TriMesh.Vertex> RetrieveTwoFixBoundaryPoint(TriMesh mesh)
        {
            List<TriMesh.Vertex> two = new List<HalfEdgeMesh.Vertex>();
            List<TriMesh.Vertex> bound = TriMeshUtil.RetrieveBoundarySingle(mesh);
            foreach (TriMesh.Vertex vertex in bound)
            {
                if (vertex.Traits.SelectedFlag >0)
                {
                    two.Add(vertex);
                }
            }
            if (two.Count < 2)
            {
                two.Add(bound[0]);
                two.Add(bound[bound.Count / 2]);
            }
            return two;
        }

        


        public static List<TriMesh.Vertex> RetrieveAllBoundaryVertex (TriMesh mesh)
        {
            List<TriMesh.Vertex> boundary = new List<TriMesh.Vertex>();
            foreach (TriMesh.Vertex vertex in mesh.Vertices)
            {
                if (vertex.OnBoundary)
                {
                    boundary.Add(vertex);
                     
                }

            }
           
            return boundary;
        }

        public static List<TriMesh.Vertex> RetrieveBoundarySingle(TriMesh mesh)
        {
            List<TriMesh.Vertex> boundary = new List<TriMesh.Vertex>();
            foreach (TriMesh.Vertex vertex in mesh.Vertices)
            {
                if (vertex.OnBoundary)
                {
                    boundary.Add(vertex);
                    break;
                }

            }
            RetrieveBoundarySingle(boundary);
            return boundary;
        }

        public static void RetrieveBoundarySingle(List<TriMesh.Vertex> boundary)
        {
            if (boundary.Count > 0)
            {
                TriMesh.HalfEdge halfedgeOnBoundary = null;
                foreach (TriMesh.HalfEdge halfedge in boundary[0].HalfEdges)
                {
                    if (halfedge.OnBoundary)
                    {
                        halfedgeOnBoundary = halfedge;
                        break;
                    }
                }
                if (halfedgeOnBoundary != null)
                {
                    while (halfedgeOnBoundary.ToVertex != boundary[0])
                    {
                        boundary.Add(halfedgeOnBoundary.ToVertex);
                        halfedgeOnBoundary = halfedgeOnBoundary.Next;
                    }
                }
            }
        }

        public static void RetrieveBoundaryEdge(List<TriMesh.HalfEdge> boundary)
        {
            if (boundary.Count > 0)
            {
                TriMesh.HalfEdge halfedgeOnBoundary = null;
                foreach (TriMesh.HalfEdge halfedge in boundary[0].ToVertex.HalfEdges)
                {
                    if (halfedge.OnBoundary)
                    {
                        halfedgeOnBoundary = halfedge;
                        break;
                    }
                }
                if (halfedgeOnBoundary != null)
                {
                    while (halfedgeOnBoundary.ToVertex != boundary[0].ToVertex)
                    {
                        boundary.Add(halfedgeOnBoundary);
                        halfedgeOnBoundary = halfedgeOnBoundary.Next;
                    }
                }
            }
        }

        public static List<List<TriMesh.Vertex>> RetrieveBoundaryAllVertex(TriMesh mesh)
        {
            List<List<TriMesh.Vertex>> allBoundary = new List<List<TriMesh.Vertex>>();
            foreach (TriMesh.Vertex vertex in mesh.Vertices)
            {
                if (vertex.OnBoundary)
                {
                    bool exists = false;
                    foreach (List<TriMesh.Vertex> existsHole in allBoundary)
                    {
                        if (existsHole.Contains(vertex))
                            exists = true;
                    }

                    if (!exists)
                    {
                        List<TriMesh.Vertex> hole = new List<TriMesh.Vertex>();
                        hole.Add(vertex);
                        RetrieveBoundarySingle(hole);
                        allBoundary.Add(hole);
                    }
                }

            }

            return allBoundary;

        }

        public static List<List<TriMesh.HalfEdge>> RetrieveBoundaryEdgeAll(TriMesh mesh)
        {
            List<List<TriMesh.HalfEdge>> allBoundary = new List<List<TriMesh.HalfEdge>>();
            foreach (TriMesh.HalfEdge edge in mesh.HalfEdges)
            {
                if (edge.OnBoundary)
                {
                    bool exists = false;
                    foreach (List<TriMesh.HalfEdge> existsHole in allBoundary)
                    {
                        if (existsHole.Contains(edge))
                            exists = true;
                    }

                    if (!exists)
                    {
                        List<TriMesh.HalfEdge> hole = new List<TriMesh.HalfEdge>();
                        hole.Add(edge);
                        RetrieveBoundaryEdge(hole);
                        allBoundary.Add(hole);
                    }
                }

            }

            return allBoundary;

        }


        public static List<TriMesh.HalfEdge> RetreiveHalfEdgeFromBoundaryVertex(List<TriMesh.Vertex> vertice)
        {
              List<TriMesh.HalfEdge> result=new List<HalfEdgeMesh.HalfEdge>();
              TriMesh.HalfEdge startHF = vertice[0].FindHalfedgeTo(vertice[1]);
              TriMesh.HalfEdge currentHF = startHF;
              do
              {
                  result.Add(currentHF);
                  currentHF = currentHF.Next;
              }while (currentHF != startHF);
              return result;
        }


        



        public static List<List<TriMesh.Face>> RetrieveFacePatchBySelectedEdge(TriMesh mesh)
        {
            bool[] faceFlag = new bool[mesh.Faces.Count];
            bool[] hfFlag = new bool[mesh.HalfEdges.Count];
            List<List<TriMesh.Face>> all = new List<List<TriMesh.Face>>();

            foreach (TriMesh.HalfEdge  hf in mesh.HalfEdges)
            {
                if (!hfFlag[hf.Index])
                {
                    List<TriMesh.Face> list = new List<HalfEdgeMesh.Face>();
                    Stack<TriMesh.HalfEdge> stack = new Stack<HalfEdgeMesh.HalfEdge>();
                    stack.Push(hf);
                    while (stack.Count != 0)
                    {
                        TriMesh.HalfEdge cur = stack.Pop();
                        hfFlag[cur.Index] = true;

                        if (!faceFlag[cur.Face.Index])
                        {
                            list.Add(cur.Face);
                            faceFlag[cur.Face.Index] = true;
                        }

                        TriMesh.HalfEdge[] arr = new HalfEdgeMesh.HalfEdge[]
                        {
                            cur.Opposite,
                            cur.Next,
                            cur.Previous
                        };
                        foreach (var item in arr)
                        {
                            if (!hfFlag[item.Index] &&
                                item.Face != null &&
                                item.Edge.Traits.SelectedFlag == 0)
                            {
                                stack.Push(item);
                            }
                        }
                    }
                    if (list.Count != 0)
                    {
                        all.Add(list);
                    }
                }
            }

            return all;
        }


        public static void ShowFacePatchColor(List<List<TriMesh.Face>> patch)
        {
           
            for (int i = 0; i < patch.Count; i++)
            {
                for(int j=0;j<patch[i].Count ;j++) 
                {
                    patch[i][j].Traits.SelectedFlag = (byte)(i + 1);
                    patch[i][j].Traits.Color = SetRandomColor(i + 1);
                }
            }
        }

        public static void ShowMeshPatchColor(TriMesh mesh)
        {
            List<List<TriMesh.Face>> patch = RetrieveFacePatchBySelectedEdge(mesh);
            ShowFacePatchColor(patch);
        }


        
    }
}
