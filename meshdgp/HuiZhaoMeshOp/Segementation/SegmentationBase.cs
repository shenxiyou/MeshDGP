using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class SegmentationBase
    {
        protected TriMesh mesh = null;

        public SegmentationBase(TriMesh mesh)
        {
            this.mesh = mesh;
        }


      


        public void Clear()
        {
            for (int i = 0; i < this.mesh.Faces.Count; i++)
            {
                this.mesh.Faces[i].Traits.SelectedFlag = 0;

            }
            for (int i = 0; i < this.mesh.Vertices.Count; i++)
            {
                this.mesh.Vertices[i].Traits.SelectedFlag = 0;

            }
        }


        public void ByVertex()
        {
            List<TriMesh.Vertex> selectedVertices = new List<TriMesh.Vertex>();
            Boolean n = true;
            for (; n; )
            {
                n = false;
                for (int i = 0; i < this.mesh.Vertices.Count; i++)
                {
                    if (this.mesh.Vertices[i].Traits.SelectedFlag > 0)//大于0，选中；等于0，没选中
                    {
                        selectedVertices.Add(this.mesh.Vertices[i]); //把选中的点放入列表中
                    }
                    if (this.mesh.Vertices[i].Traits.SelectedFlag == 0)
                    {
                        n = true;
                    }
                }

                for (int i = 0; i < selectedVertices.Count; i++)
                {
                    foreach (TriMesh.Vertex vertex in selectedVertices[i].Vertices)//得到选中的点的周围的点
                    {
                        if (vertex.Traits.SelectedFlag == 0)
                        {
                            vertex.Traits.SelectedFlag = selectedVertices[i].Traits.SelectedFlag;
                        }
                    }
                }
            }
        }
        public void ByFace()
        {
            List<TriMesh.Face> selectedFaces = new List<TriMesh.Face>();

            for (int i = 0; i < this.mesh.Faces.Count; i++)
            {
                if (this.mesh.Faces[i].Traits.SelectedFlag > 0)
                {
                    selectedFaces.Add(this.mesh.Faces[i]);
                }
            }

            for (int i = 0; i < selectedFaces.Count; i++)
            {
                foreach (TriMesh.Face face in selectedFaces[i].Faces)
                {
                    if (face.Traits.SelectedFlag == 0)
                    {
                        face.Traits.SelectedFlag = selectedFaces[i].Traits.SelectedFlag;
                    }
                }
            }

        }


        public virtual void Run()
        {
        }

    }
}
