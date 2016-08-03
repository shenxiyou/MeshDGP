using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class TriMeshUtil
    {
        public static void SelectedVertexRandom(TriMesh mesh,int count)
        {
             
            int num =  mesh.Vertices.Count /count;
            int k = 0;
            while (k < mesh.Vertices.Count)
            {
                mesh.Vertices[k].Traits.SelectedFlag = 1;
                k += num;
            }

        }

        
        public static void SelectedVertexReverse(TriMesh mesh)
        {  
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {

                if (mesh.Vertices[i].Traits.SelectedFlag > 0)
                {
                    mesh.Vertices[i].Traits.SelectedFlag = 0;
                }
                else
                {
                    mesh.Vertices[i].Traits.SelectedFlag = 1;
                } 
            } 
            
        }


        public static List<TriMesh.Vertex> RetrieveTwoVertex(TriMesh mesh)
        {

            List<TriMesh.Vertex> two = new List<HalfEdgeMesh.Vertex>();
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {

                if (mesh.Vertices[i].Traits.SelectedFlag > 0)
                {
                    two.Add(mesh.Vertices[i]);
                }

                if (two.Count >= 2)
                    break;
            }

            return two;
        }


        public static List<TriMesh.Vertex> RetrieveTwoVertexMust(TriMesh mesh)
        {

            List<TriMesh.Vertex> two = new List<HalfEdgeMesh.Vertex>();
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {

                if (mesh.Vertices[i].Traits.SelectedFlag > 0)
                {
                    two.Add(mesh.Vertices[i]);
                }

                if (two.Count >= 2)
                    break;
            }

            if (two.Count < 2)
            {
                two.Add(mesh.Vertices[0]);
                two.Add(mesh.Vertices[mesh.Vertices.Count/2]);
            }

            return two;
        }

        public static TriMesh.Vertex RetrieveSelectedVertexFirst(TriMesh mesh)
        {
            TriMesh.Vertex target  =null;
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                if (mesh.Vertices[i].Traits.SelectedFlag > 0)
                {
                    target = mesh.Vertices[i];
                    break;
                }
            }
            if (target == null)
                target = mesh.Vertices[0];
            return target ;
        }

       
        public static List<TriMesh.Vertex> RetrieveSelectedVertex(TriMesh mesh)
        {
            List<TriMesh.Vertex> selected = new List<TriMesh.Vertex>();
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                if (mesh.Vertices[i].Traits.SelectedFlag > 0)
                {
                    selected.Add(mesh.Vertices[i]);
                }
            }
            return selected;
        }


        public static List<TriMesh.Vertex> RetrieveVertexGroup(TriMesh mesh,int groupIndex)
        {
            TriMeshUtil.GroupVertice(mesh);
            List<TriMesh.Vertex> group = new List<TriMesh.Vertex>();
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                if (mesh.Vertices[i].Traits.SelectedFlag ==groupIndex)
                {
                    group.Add(mesh.Vertices[i]);
                }
            }
            return group;
        }


        public static List<TriMesh.Edge> RetrieveSelectedEdge(TriMesh mesh)
        {
            List<TriMesh.Edge> selected = new List<TriMesh.Edge>();
            for (int i = 0; i < mesh.Edges.Count; i++)
            {
                if (mesh.Edges[i].Traits.SelectedFlag > 0)
                {
                    selected.Add(mesh.Edges[i]);
                }
            }
            return selected;
        }

        public static List<TriMesh.Face> RetrieveSelectedFace(TriMesh mesh)
        {
            List<TriMesh.Face> selected = new List<TriMesh.Face>();
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                if (mesh.Faces[i].Traits.SelectedFlag > 0)
                {
                    selected.Add(mesh.Faces[i]);
                }
            }
            return selected;
        }


       

        

        public static void RetrieveOneRingFaceByPatch(TriMesh mesh, bool[] selectedFlags)
        {
            List<TriMesh.Face> result = new List<HalfEdgeMesh.Face>();

            TriMesh.HalfEdge[] list = RetrieveOneRingHalfEdgeByPatch(mesh, selectedFlags);
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].Face != null)
                {
                    result.Add(list[i].Face);
                } 

            }
        }

        public static void ClearMeshColor(TriMesh mesh)
        {
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                mesh.Vertices[i].Traits.SelectedFlag = 0;
                mesh.Vertices[i].Traits.Color = Color4.Black;
            }
            for (int i = 0; i < mesh.Edges.Count; i++)
            {
                mesh.Edges[i].Traits.SelectedFlag = 0;
                mesh.Edges[i].Traits.Color = Color4.Black;
            }

            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                mesh.Faces[i].Traits.SelectedFlag = 0;
                mesh.Faces[i].Traits.Color = Color4.Black;
            }
        }
       


    }
}
