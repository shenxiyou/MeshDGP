using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public partial class OpenGLTriMesh
    {
        


        public void DrawTreeCotree(List<TriMesh.Edge> treeMarks,List<TriMesh.Edge> cotreeMarks )
        {

            
            OpenGLManager.Instance.SetColor(System.Drawing.Color.Green);
            //Draw Tree
            foreach (TriMesh.Edge item in treeMarks)
            {
                GL.Begin(BeginMode.Lines);
                Vector3D a = item.Vertex0.Traits.Position;
                Vector3D b = item.Vertex1.Traits.Position;

               
                GL.Vertex3(a.x, a.y, a.z);
                GL.Vertex3(b.x, b.y, b.z);
                GL.End();
            }

            OpenGLManager.Instance.SetColor(System.Drawing.Color.Red);
            //Draw CoTree
            foreach (TriMesh.Edge item in cotreeMarks)
            {

                Vector3D a = TriMeshUtil.GetMidPoint(item.Face0);
                Vector3D b = TriMeshUtil.GetMidPoint(item.Face1);
                Vector3D centerOfEdge = (item.Vertex0.Traits.Position + item.Vertex1.Traits.Position) / 2;

               
                GL.Begin(BeginMode.Lines);
                GL.Vertex3(a.x, a.y, a.z);
                GL.Vertex3(centerOfEdge.x, centerOfEdge.y, centerOfEdge.z);
                GL.End();

                GL.Begin(BeginMode.Lines);
                GL.Vertex3(centerOfEdge.x, centerOfEdge.y, centerOfEdge.z);
                GL.Vertex3(b.x, b.y, b.z);
                GL.End();
            }

        }



        public void DrawGenerators(TriMesh mesh,List<List<TriMesh.HalfEdge>> Generators)
        {
            if (Generators == null)
            {
                return;
            }


            Vector3D[] bycenters = new Vector3D[mesh.Faces.Count];
            foreach (TriMesh.Face face in mesh.Faces)
            {
                bycenters[face.Index] = TriMeshUtil.GetMidPoint(face);
            }
     

            bool[] edgeFlags = new bool[mesh.Edges.Count];

            int i = 0;
            System.Drawing.Color color = System.Drawing.Color.Orange;

            foreach (List<TriMesh.HalfEdge> loop in Generators)
            {
                switch (i)
                {
                    case 1:
                        color = System.Drawing.Color.Green;
                        break;
                    case 2:
                        color = System.Drawing.Color.Red;
                        break;
                    case 3:
                        color = System.Drawing.Color.Purple;
                        break;
                    default:
                        color = System.Drawing.Color.Orange;
                        break;
                }


                OpenGLManager.Instance.SetColor(color);
                foreach (TriMesh.HalfEdge loopItem in loop)
                {
                    TriMesh.Edge edge = loopItem.Edge;

                    if (edgeFlags[edge.Index] == false)
                    {
                        Vector3D a = bycenters[edge.Face0.Index];
                        Vector3D b = bycenters[edge.Face1.Index];
                        Vector3D centerOfEdge = (edge.Vertex0.Traits.Position + edge.Vertex1.Traits.Position) / 2;

                        GL.Begin(BeginMode.Lines);
                        GL.Vertex3(a.x, a.y, a.z);
                        GL.Vertex3(centerOfEdge.x, centerOfEdge.y, centerOfEdge.z);
                        GL.End();

                        GL.Begin(BeginMode.Lines);
                        GL.Vertex3(centerOfEdge.x, centerOfEdge.y, centerOfEdge.z);
                        GL.Vertex3(b.x, b.y, b.z);
                        GL.End();
                    }

                    edgeFlags[edge.Index] = true;
                }
                i++;
            }

        }

    }
}