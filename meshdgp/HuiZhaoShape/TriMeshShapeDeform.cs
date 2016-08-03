using System;
using System.Collections.Generic; 
using System.Text;

namespace GraphicResearchHuiZhao
{
    public partial class TriMeshShape
    {
       
         
        public TriMesh CreateShape(EnumShapeDeform shape)
        {
            TriMesh mesh = null;
            switch(shape)
            {
                case EnumShapeDeform.Cylinder:
                    mesh =  CreateCylinder(CylinderLength, CylinderWidth, CylinderHeight);
                    break;
                case EnumShapeDeform.CylinderV2:
                    mesh = CreateCylinderV2(CylinderLength, CylinderWidth, CylinderHeight);
                    break;
                case EnumShapeDeform.Plane2D:
                    mesh =  CreateSquare(SquareLength, SquareWidth);
                    break;
                case EnumShapeDeform.PlaneSpoke:
                    mesh= CreateSquareSpoke(SquareLength, SquareWidth);
                    break;


            }
            mesh.FileName = shape.ToString();
            TriMeshUtil.SetUpNormalVertex(mesh);
            //TriMeshUtil.MoveToCenter(mesh);
            //TriMeshUtil.ScaleToUnit(mesh, 1.0);
            return mesh;
                 
        }
 
 


        public TriMesh CreateSquare(int length, int width)
        {
            TriMesh mesh = new TriMesh();
            double x0 = -this.GridSizeX * length / 2d;
            double y0 = -this.GridSizeX * width / 2d;
            double z0 = 0;
            double d = this.GridSizeX / 2;

            TriMesh.Vertex[,] v = new TriMesh.Vertex[length + 1, width + 1];
            for (int i = 0; i < length + 1; i++)
            {
                for (int j = 0; j < width + 1; j++)
                {
                    double x=x0 + this.GridSizeX * i;
                    double y=y0 + this.GridSizeX * j;
                    double z = i % 2 == 1 && j % 2 == 1 ? d : z0;
                    v[i, j] = mesh.Vertices.Add(new VertexTraits(x, y, z));
                }
            }

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    mesh.Faces.AddTriangles(v[i, j], v[i + 1, j], v[i + 1, j + 1], v[i, j + 1]);
                }
            }
            TriMeshUtil.SetUpNormalVertex(mesh);
            return mesh;
        }

        public TriMesh CreateCylinder(int length, int width, int height)
        {
            TriMesh mesh = new TriMesh();
            double x0 = -this.GridSizeX * length / 2d;
            double y0 = -this.GridSizeY * width / 2d;
            double z0 = -this.GridSizeZ * height / 2d;

            TriMesh.Vertex[,] top = new TriMesh.Vertex[length + 1, width + 1];
            TriMesh.Vertex[,] btm = new TriMesh.Vertex[length + 1, width + 1];
            for (int i = 0; i < length + 1; i++)
            {
                for (int j = 0; j < width + 1; j++)
                {
                    double x = x0 + this.GridSizeX * i;
                    double y = y0 + this.GridSizeY * j;
                    top[i, j] = mesh.Vertices.Add(new VertexTraits(x, y, -z0));
                    btm[i, j] = mesh.Vertices.Add(new VertexTraits(x, y, z0));
                }
            }

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    mesh.Faces.AddTriangles(top[i, j], top[i + 1, j], top[i + 1, j + 1], top[i, j + 1]);
                    mesh.Faces.AddTriangles(btm[i, j], btm[i, j + 1], btm[i + 1, j + 1], btm[i + 1, j]);
                }
            }

            int round = (length + width) * 2;
            TriMesh.Vertex[,] side = new TriMesh.Vertex[round, height + 1];
            int s = 0, t = 0;
            for (int i = 0; i < round; i++)
            {
                int ol = i < length ? 1 : i >= round / 2d && i < round - width ? -1 : 0;
                int ow = i >= round - width ? -1 : i >= length && i < round / 2d ? 1 : 0;
                Vector3D v = btm[s, t].Traits.Position;
                side[i, 0] = btm[s, t];
                for (int j = 1; j < height; j++)
                {
                    side[i, j] = mesh.Vertices.Add(new VertexTraits(v.x, v.y, z0 + this.GridSizeZ * j));
                }
                side[i, height] = top[s, t];
                s += ol; t += ow;
            }

            for (int i = 0; i < round; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    mesh.Faces.AddTriangles(side[i, j], side[(i + 1) % round, j], side[(i + 1) % round, j + 1], side[i, j + 1]);
                }
            }
            
            TriMeshUtil.SetUpNormalVertex(mesh);
            return mesh;
        }


        public TriMesh CreateCylinderV2(int length, int width, int height)
        {
            TriMesh mesh = new TriMesh();
            double x0 = -this.GridSizeX * length / 2d;
            double y0 = -this.GridSizeY * width / 2d;
            double z0 = -this.GridSizeZ * height / 2d;

            TriMesh.Vertex[,] top = new TriMesh.Vertex[length + 1, width + 1];
            TriMesh.Vertex[,] btm = new TriMesh.Vertex[length + 1, width + 1];
            for (int i = 0; i < length + 1; i++)
            {
                for (int j = 0; j < width + 1; j++)
                {
                    double x = x0 + this.GridSizeX * i;
                    double y = y0 + this.GridSizeY * j;
                    top[i, j] = mesh.Vertices.Add(new VertexTraits(x, y, -z0));
                    btm[i, j] = mesh.Vertices.Add(new VertexTraits(x, y, z0));
                }
            }

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    mesh.Faces.AddTriangles(top[i, j], top[i + 1, j], top[i + 1, j + 1], top[i, j + 1]);
                    mesh.Faces.AddTriangles(btm[i, j], btm[i, j + 1], btm[i + 1, j + 1], btm[i + 1, j]);
                }
            }

            int round = (length + width) * 2;
            TriMesh.Vertex[,] side = new TriMesh.Vertex[round, height + 1];
            int s = 0, t = 0;
            for (int i = 0; i < round; i++)
            {
                int ol = i < length ? 1 : i >= round / 2d && i < round - width ? -1 : 0;
                int ow = i >= round - width ? -1 : i >= length && i < round / 2d ? 1 : 0;
                Vector3D v = btm[s, t].Traits.Position;
                side[i, 0] = btm[s, t];
                for (int j = 1; j < height/2; j++)
                {
                    side[i, j] = mesh.Vertices.Add(new VertexTraits(v.x, v.y, z0 + this.GridSizeZ/4d * j));
                }
                for (int j = height / 2; j < height; j++)
                {
                    side[i, j] = mesh.Vertices.Add(new VertexTraits(v.x, v.y, z0 +(this.GridSizeZ/4d) *(height/2)+ this.GridSizeZ*(7d/6d) * (j-height/2+1)));
                }
                side[i, height] = top[s, t];
                s += ol; t += ow;
            }

            for (int i = 0; i < round; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    mesh.Faces.AddTriangles(side[i, j], side[(i + 1) % round, j], side[(i + 1) % round, j + 1], side[i, j + 1]);
                }
            }

            TriMeshUtil.SetUpNormalVertex(mesh);
            return mesh;
        }


        public TriMesh CreateSquareSpoke(int length, int width)
        {
            TriMesh mesh = new TriMesh();
            double x0 = -this.GridSizeX * length / 2d;
            double y0 = -this.GridSizeX * width / 2d;
            double z0 = 0;
            double d = this.GridSizeX / 2;
            int t = 4;

            TriMesh.Vertex[,] v = new TriMesh.Vertex[length + 1, width + 1];
            for (int i = 0; i < length + 1; i++)
            {
                for (int j = 0; j < width + 1; j++)
                {
                    double x = x0 + this.GridSizeX * i;
                    double y = y0 + this.GridSizeX * j;
                    int r = i % t == j % t ? i % t : 0;
                    double z = z0 - (Math.Abs(r - t / 2) - t / 2) * d;
                    v[i, j] = mesh.Vertices.Add(new VertexTraits(x, y, z));
                }
            }

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    mesh.Faces.AddTriangles(v[i, j], v[i + 1, j], v[i + 1, j + 1], v[i, j + 1]);
                }
            }
            TriMeshUtil.SetUpNormalVertex(mesh);
            return mesh;
        }

    }
}
