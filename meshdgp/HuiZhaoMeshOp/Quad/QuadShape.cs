using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public   class QuadShape
    {

        private static QuadShape singleton = new QuadShape();

        public static QuadShape Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new QuadShape();
                return singleton;
            }
        }

        private QuadShape()
        {
            
        }

        public QuadMesh CreateCube()
        {
            QuadMesh shape = new QuadMesh();
            shape.Vertices.Add(new VertexTraits(0, 0, 0));
            shape.Vertices.Add(new VertexTraits(0, 0.5, 0));
            shape.Vertices.Add(new VertexTraits(0.5, 0.5, 0));
            shape.Vertices.Add(new VertexTraits(0.5, 0, 0));
            shape.Vertices.Add(new VertexTraits(0, 0, 0.5));
            shape.Vertices.Add(new VertexTraits(0, 0.5, 0.5));
            shape.Vertices.Add(new VertexTraits(0.5, 0.5, 0.5));
            shape.Vertices.Add(new VertexTraits(0.5, 0, 0.5));

            QuadMesh.Vertex[] faceVertices = new QuadMesh.Vertex[4];
            faceVertices[0] = shape.Vertices[0];
            faceVertices[1] = shape.Vertices[1];
            faceVertices[2] = shape.Vertices[2];
            faceVertices[3] = shape.Vertices[3];
            shape.Faces.AddQuads(faceVertices);

            faceVertices[0] = shape.Vertices[5];
            faceVertices[1] = shape.Vertices[4];
            faceVertices[2] = shape.Vertices[7];
            faceVertices[3] = shape.Vertices[6];
            shape.Faces.AddQuads(faceVertices);

            faceVertices[0] = shape.Vertices[1];
            faceVertices[1] = shape.Vertices[0];
            faceVertices[2] = shape.Vertices[4];
            faceVertices[3] = shape.Vertices[5];
            shape.Faces.AddQuads(faceVertices);

            faceVertices[0] = shape.Vertices[2];
            faceVertices[1] = shape.Vertices[1];
            faceVertices[2] = shape.Vertices[5];
            faceVertices[3] = shape.Vertices[6];
            shape.Faces.AddQuads(faceVertices);

            faceVertices[0] = shape.Vertices[3];
            faceVertices[1] = shape.Vertices[2];
            faceVertices[2] = shape.Vertices[6];
            faceVertices[3] = shape.Vertices[7];
            shape.Faces.AddQuads(faceVertices);

            faceVertices[0] = shape.Vertices[0];
            faceVertices[1] = shape.Vertices[3];
            faceVertices[2] = shape.Vertices[7];
            faceVertices[3] = shape.Vertices[4];
            shape.Faces.AddQuads(faceVertices);

            //shape.Vertices.Add(new VertexTraits(0.25, 0.25, 0));
            //shape.Vertices.Add(new VertexTraits(0, 0.25, 0));
            //shape.Vertices.Add(new VertexTraits(0.25, 0, 0));
            //faceVertices[0] = shape.Vertices[0];
            //faceVertices[1] = shape.Vertices[3];
            //faceVertices[2] = shape.Vertices[7];
            //faceVertices[3] = shape.Vertices[4];
            //shape.Faces.AddQuads(faceVertices);


            //ChangTopolopy(shape);









            return shape;
        }



        public void ComputeNormal(QuadMesh quadMesh)
        {
            foreach (QuadMesh.Face face in quadMesh.Faces)
            {
                QuadMesh.Vertex v0 = face.GetVertex(0);
                QuadMesh.Vertex v1 = face.GetVertex(1);
                QuadMesh.Vertex v2 = face.GetVertex(2);
                QuadMesh.Vertex v3 = face.GetVertex(3);


                Vector3D normal1 = (v2.Traits.Position - v1.Traits.Position).Cross(v0.Traits.Position - v2.Traits.Position);
                normal1.Normalize();

                Vector3D normal2 = (v0.Traits.Position - v3.Traits.Position).Cross(v2.Traits.Position - v0.Traits.Position);
                normal2.Normalize();

                face.Traits.Normal = (normal1 + normal2) / 2;
            }
            foreach (QuadMesh.Vertex vertex in quadMesh.Vertices)
            {
                vertex.Traits.Normal = Vector3D.Zero;
                foreach (QuadMesh.Face face in vertex.Faces)
                {
                    vertex.Traits.Normal += face.Traits.Normal;
                }
                vertex.Traits.Normal /= vertex.FaceCount;
            }
        }


        public QuadMesh CreatePlane(int column, int row)
        {
            QuadMesh shape = new QuadMesh();
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    shape.Vertices.Add(new VertexTraits((double)i / (double)row, (double)j / (double)column, 0));
                }
            }

            QuadMesh.Vertex[] faceVertices = new QuadMesh.Vertex[4];
            for (int i = 0; i < row - 1; i++)
            {
                for (int j = 0; j < column - 1; j++)
                {
                    faceVertices[0] = shape.Vertices[i * column + j];
                    faceVertices[1] = shape.Vertices[i * column + j + 1];
                    faceVertices[2] = shape.Vertices[(i + 1) * column + j + 1];
                    faceVertices[3] = shape.Vertices[(i + 1) * column + j];
                    shape.Faces.AddQuads(faceVertices);

                }
            }



            return shape;
        }
    }
}
