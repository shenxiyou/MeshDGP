using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics; 

namespace GraphicResearchHuiZhao
{
    public partial class OpenGLTriMesh
    {


        public void DrawColorVis(TriMesh mesh)
        {
            if (GlobalData.Instance.ColorVis == null)
                return;

            if (GlobalData.Instance.ColorVis.Length != mesh.Vertices.Count)
                return;

            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.Normalize);

            GL.Begin(BeginMode.Triangles);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {

                foreach (TriMesh.Vertex vertex in mesh.Faces[i].Vertices)
                {
                    float alpha = (float)GlobalData.Instance.ColorVis[vertex.Index];

                    GL.Color4(alpha, alpha, alpha, alpha);

                    GL.Normal3(vertex.Traits.Normal.x,
                               vertex.Traits.Normal.y,
                               vertex.Traits.Normal.z);
                    GL.Vertex3(vertex.Traits.Position.x,
                               vertex.Traits.Position.y,
                               vertex.Traits.Position.z);

                }
            }
            GL.End();

        }

        public void DrawColor(TriMesh mesh)
        {
            GL.Enable(EnableCap.ColorMaterial);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.Normalize);

            GL.Begin(BeginMode.Triangles);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {


                foreach (TriMesh.Vertex vertex in mesh.Faces[i].Vertices)
                {

                    GL.Color3(vertex.Traits.Color.R, vertex.Traits.Color.G, vertex.Traits.Color.B);

                    GL.Normal3(vertex.Traits.Normal.x,
                               vertex.Traits.Normal.y,
                               vertex.Traits.Normal.z);
                    GL.Vertex3(vertex.Traits.Position.x,
                               vertex.Traits.Position.y,
                               vertex.Traits.Position.z);

                }
            }
            GL.End();

        }

       

      
    }
}
