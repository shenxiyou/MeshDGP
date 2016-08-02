using System;
using System.Collections.Generic;
using System.Text; 

namespace GraphicResearchHuiZhao
{
    public partial class TriMeshShape
    {
        private static TriMeshShape singleton = new TriMeshShape();

        public static TriMeshShape Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new TriMeshShape();
                return singleton;
            }
        }

        private  TriMeshShape()
        {
            
        }


        public TriMesh MeshInput
        {
            get
            {
                return GlobalData.Instance.TriMesh;
            }
        }

      


        public TriMesh CreateShape(EnumShape shape)
        {
            TriMesh mesh = null;
            switch (shape)
            {
                case EnumShape.Triangle:
                    mesh = CreatePolygon(3);
                    break;
                case EnumShape.Circle:
                    mesh = CreatePolygon(CircleVertex);
                    break;
                case EnumShape.Sqaure:
                    mesh = CreatePolygon(4);
                    break;
                case EnumShape.Pentagon:
                    mesh = CreatePolygon(5);
                    break;
                case EnumShape.Hexagon:
                    mesh = CreatePolygon(6);
                    break;
                case EnumShape.Octagon:
                    mesh = CreatePolygon(8);
                    break;
                case EnumShape.Grid:
                    mesh = CreateGrid(PlaneGridX,PlaneGridY,PlaneGridSizeX,PlaneGridSizeY);
                    break;
                //-----------------
                case EnumShape.Pyramid:
                    mesh = CreateCone(4, Math.Sqrt(2) / 2);
                    break;
                case EnumShape.ConeDemo:
                    mesh = CreateConeDemo(ConeNum, ConfigShape.Instance.ConeHeight);
                    break;
                case EnumShape.Cone:
                    mesh = CreateCone(ConeNum, Math.Sqrt(2) / 2);
                    break;
                case EnumShape.Tet:
                    mesh = CreateCone(3, Math.Sqrt(2) / 2);
                    break;
                //---------------
                case EnumShape.Cube:
                    mesh = CreateCylinder(4, Math.Sqrt(2) / 2);
                    break;
                case EnumShape.Cylinder:
                    mesh = CreateCylinder(50, 1);
                    break;
                case EnumShape.HexagonalPrism:
                    mesh = CreateCylinder(6, 1);
                    break;
                case EnumShape.TriangularPrism:
                    mesh = CreateCylinder(3, 1);
                    break;
                //---------------------
                case EnumShape.Sphere:
                    mesh = CreateSphere(SphereNum);
                    break;

                case EnumShape.CylinderUV:
                    mesh = CreateCylinder(CylinderUVm,CylinderUVn,CylinderUVr,CylinderUVl,CylinderUVMaxU,CylinderUVMaxV,CylinderUVDiff); 
                    break;
                case EnumShape.CylinderUVPlane:
                    mesh = CreateCylinderPlane(CylinderUVm, CylinderUVn, CylinderUVr, CylinderUVl, CylinderUVMaxU, CylinderUVMaxV, CylinderUVDiff);
                    break;
                case EnumShape.CylinderSquare:
                    mesh = CreateCylinderSquare(CylinderUVm, CylinderUVn, CylinderUVr);
                    break;
                case EnumShape.CylinderPlaneSquare:
                    mesh = CreateCylinderPlaneSquare(CylinderUVm, CylinderUVn, CylinderUVr);
                    break;
                case EnumShape.PlaneTest1:
                    mesh = CreatePlaneTest(CylinderUVm, CylinderUVm, CylinderUVr);
                    break;
                case EnumShape.PlaneFolded:
                    mesh = CreatePlaneFolded(CylinderUVm, CylinderUVm, CylinderUVr);
                    break;
             
                default:
                    break;
            }
            mesh.FileName = shape.ToString();
            TriMeshUtil.SetUpNormalVertex(mesh);
          //  TriMeshUtil.MoveToCenter(mesh);
           // TriMeshUtil.ScaleToUnit(mesh, 1.0);

            return mesh;
            
        }

        public static TriMesh CreateCylinderSquare(int m, int n, double r)
        {
            double x0 = r * Math.Cos(0);
            double y0 = r * Math.Sin(0);

            double x1 = r * Math.Cos(Math.PI * 2 / n);
            double y1 = r * Math.Sin(Math.PI * 2 / n);

            double length = Math.Sqrt((x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0));


            double l = length * n;


            TriMesh cylinder = CreateCylinderOpen(m, n, r, l, 1, 1, 1);
            return cylinder;
        }

        public static TriMesh CreateCylinderPlaneSquare(int m, int n, double r)
        {

            double x0 = r * Math.Cos(0);
            double y0 = r * Math.Sin(0);

            double x1 = r * Math.Cos(Math.PI * 2 / n );
            double y1 = r * Math.Sin(Math.PI * 2 / n);

            double length = Math.Sqrt((x1 - x0) * (x1 - x0) + (y1-y0)*(y1 - y0));


            double l = length * n;
            TriMesh plane = CreateCylinderPlaneOpen(m, n, r, l, 1, 1, 1);
            return plane;

        }



        public static TriMesh CreateCylinderOpen(int m, int n, double r,
                                double l, double maxU, double maxV, int diff)
        {
            TriMesh mesh = new TriMesh();
            TriMesh.Vertex[,] arr = new HalfEdgeMesh.Vertex[m+1, n+1];
            for (int i = 0; i <= m; i++)
            {
                double z = l / m * i - l / 2;
                double v = maxV / m * i;
                for (int j = 0; j <=n; j++)
                {
                    double x = r * Math.Cos(Math.PI * 2 / n * j);
                    double y = r * Math.Sin(Math.PI * 2 / n * j);
                    double u = maxU / n * j;
                    VertexTraits trait = new VertexTraits(x, y, z);
                    trait.UV = new Vector2D(u, v);
                    arr[i, j] = mesh.Vertices.Add(trait);
                }
            }
            for (int i = 0; i < m ; i++)
            {
                for (int j = 0; j < n ; j++)
                {
                    int ni = i + 1;
                    int nj =  j + 1 ;
                    mesh.Faces.AddTriangles(arr[i, nj], arr[ni, nj],
                                            arr[ni, j], arr[i, j]);
                }
            }
            return mesh;
        }

        public static TriMesh CreateCylinderPlaneOpen(int m, int n, double r,
                                double l, double maxU, double maxV, int diff)
        {
            TriMesh mesh = new TriMesh();
            TriMesh.Vertex[,] arr = new HalfEdgeMesh.Vertex[m+1, n+1];
            for (int i = 0; i <= m; i++)
            {
                double x = l / m * i - l / 2;

                for (int j = 0; j <= n; j++)
                {
                    double y = l / n * j - l / 2;
                    VertexTraits trait = new VertexTraits(x, y, 0);

                    trait.UV = new Vector2D(x, y);
                    arr[i, j] = mesh.Vertices.Add(trait);
                }
            }
            for (int i = 0; i < m  ; i++)
            {
                for (int j = 0; j < n  ; j++)
                {
                    int ni = i + 1;
                    int nj =  j + 1 ;
                    mesh.Faces.AddTriangles(arr[i, nj], arr[ni, nj],
                                            arr[ni, j], arr[i, j]);
                }
            }
            return mesh;
        }



        public static TriMesh CreatePlaneTest(int m, int n, double l)
        {
            TriMesh mesh = new TriMesh();
            TriMesh.Vertex[,] arr = new HalfEdgeMesh.Vertex[m + 1, n + 1];
            for (int i = 0; i <= m; i++)
            {
                double x = l / m * i  ;

                for (int j = 0; j <= n; j++)
                {
                    double y = l / n * j  ;
                    VertexTraits trait = new VertexTraits(x, y, 0);

                    trait.UV = new Vector2D(x, y);
                    arr[i, j] = mesh.Vertices.Add(trait);
                }
            }
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int ni = i + 1;
                    int nj = j + 1;
                    mesh.Faces.AddTriangles(arr[i, nj], arr[ni, nj],
                                            arr[ni, j], arr[i, j]);
                }
            }
            return mesh;
        }


        public static TriMesh CreatePlaneFolded(int m, int n, double l)
        {
            TriMesh mesh = new TriMesh();
            TriMesh.Vertex[,] arr = new HalfEdgeMesh.Vertex[m + 1, n + 1];
            for (int i = 0; i <= m; i++)
            {
                double x = l / m * i  ;

                double temp = 0;
                for (int j = 0; j <= n/2; j++)
                {
                    double y = l / n * j  ;
                    VertexTraits trait = new VertexTraits(x, y, 0);

                    trait.UV = new Vector2D(x, y);
                    arr[i, j] = mesh.Vertices.Add(trait);


                    temp = y;
                }


                for (int j = 1; j <= n / 2; j++)
                {
                    double y = l / n * j;
                    VertexTraits trait = new VertexTraits(x, temp, y);

                    trait.UV = new Vector2D(x, y);
                    arr[i,  n / 2+j] = mesh.Vertices.Add(trait);
                }

            }
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int ni = i + 1;
                    int nj = j + 1;
                    mesh.Faces.AddTriangles(arr[i, nj], arr[ni, nj],
                                            arr[ni, j], arr[i, j]);
                }
            }
            return mesh;
        }



        public static TriMesh CreateCylinder(int m, int n, double r, 
                                 double l, double maxU, double maxV,int diff)
        {
            TriMesh mesh = new TriMesh();
            TriMesh.Vertex[,] arr = new HalfEdgeMesh.Vertex[m, n];
            for (int i = 0; i < m; i++)
            {
                double z = l / m * i - l / 2;
                double v = maxV / m * i;
                for (int j = 0; j < n; j++)
                {
                    double x = r * Math.Cos(Math.PI * 2 / n * j);
                    double y = r * Math.Sin(Math.PI * 2 / n * j);
                    double u = maxU / n * j;
                    VertexTraits trait = new VertexTraits(x, y, z);
                    trait.UV = new Vector2D(u, v);
                    arr[i, j] = mesh.Vertices.Add(trait);
                }
            }
            for (int i = 0; i < m - 1; i++)
            {
                for (int j = 0; j < n-diff; j++)
                {
                    int ni = i + 1;
                    int nj = (j + 1) % n;
                    mesh.Faces.AddTriangles(arr[i, nj], arr[ni, nj], 
                                            arr[ni, j], arr[i, j]);
                }
            }
            return mesh;
        }

        public static TriMesh CreateCylinderPlane(int m, int n, double r,
                                double l, double maxU, double maxV, int diff)
        {
            TriMesh mesh = new TriMesh();
            TriMesh.Vertex[,] arr = new HalfEdgeMesh.Vertex[m, n];
            for (int i = 0; i < m; i++)
            {
                double x = l / m * i - l / 2;
              
                for (int j = 0; j < n; j++)
                { 
                    double y = l / n * j - l / 2;
                    VertexTraits trait = new VertexTraits(x,y, 0);

                    trait.UV = new Vector2D(x, y);
                    arr[i, j] = mesh.Vertices.Add(trait);
                }
            }
            for (int i = 0; i < m - 1; i++)
            {
                for (int j = 0; j < n - diff; j++)
                {
                    int ni = i + 1;
                    int nj = (j + 1) % n;
                    mesh.Faces.AddTriangles(arr[i, nj], arr[ni, nj],
                                            arr[ni, j], arr[i, j]);
                }
            }
            return mesh;
        }

        public static TriMesh CreateGrid(int m, int n, double lengthx,double lengthy)
        {
            TriMesh mesh = new TriMesh();
            TriMesh.Vertex[,] arr = new HalfEdgeMesh.Vertex[m, n];
            double x0 = -m * lengthx / 2d;
            double y0 = -n * lengthy / 2d;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    arr[i, j] = new HalfEdgeMesh.Vertex();
                    arr[i, j].Traits = new VertexTraits(x0 + i * lengthx, y0 + j * lengthy, 0d);
                    mesh.AppendToVertexList(arr[i, j]);
                }
            }

            for (int i = 0; i < m - 1; i++)
            {
                for (int j = 0; j < n - 1; j++)
                {
                    mesh.Faces.AddTriangles(arr[i + 1, j], arr[i, j + 1], arr[i, j]);
                    mesh.Faces.AddTriangles(arr[i + 1, j], arr[i + 1, j + 1], arr[i, j + 1]);
                }
            }
            return mesh;
        }

        public static TriMesh CreateSphere(int n)
        {
            
            TriMesh Shape = new TriMesh();
            for (int j = -n / 2; j < n / 2; j++)
            {
                double distance = Math.Sin(j * Math.PI / n);
                double r_circle = Math.Cos(j * Math.PI / n);
                for (int i = 0; i < n; i++)
                {
                    Shape.Vertices.Add(new VertexTraits(r_circle * Math.Cos(2 * i * Math.PI / n),
                             r_circle * Math.Sin(2 * i * Math.PI / n), distance));
                }
            }

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    TriMesh.Vertex topRight = Shape.Vertices[i * n + j];
                    TriMesh.Vertex topLeft = Shape.Vertices[i * n + (j + 1) % n];
                    TriMesh.Vertex bottomRight = Shape.Vertices[(i + 1) * n + j];
                    TriMesh.Vertex bottomLeft = Shape.Vertices[(i + 1) * n + (j + 1) % n];
                    Shape.Faces.AddTriangles(topRight, bottomLeft, bottomRight);
                    Shape.Faces.AddTriangles(topRight, topLeft, bottomLeft);
                }
            }
            int last = Shape.Vertices.Count - 1;
            TriMesh.Vertex tv = Shape.Vertices.Add(new VertexTraits(0, 0, 1)); tv.Traits.selectedFlag = 1;
           // TriMesh.Vertex bv = Shape.Vertices.Add(new VertexTraits(0, 0, 0)); bv.Traits.selectedFlag = 2;
            for (int i = 0; i < n; i++)
            {
               // Shape.Faces.AddTriangles(bv, Shape.Vertices[(i + 1) % n], Shape.Vertices[i]);
                Shape.Faces.AddTriangles(tv, Shape.Vertices[last - (i + 1) % n], Shape.Vertices[last - i]);
            }

            return Shape;

        }

        public static TriMesh CreatePolygon(int edge)
        {
            TriMesh poly = new TriMesh();
            AddPolygon(poly, edge, 0, true);
            return poly;
        }

        public static TriMesh CreateConeDemo(int edge, double height)
        {
            TriMesh cone = new TriMesh();
            TriMesh.Vertex[] bottom = AddPolygon(cone, edge, -height / 2, false);
            TriMesh.Vertex top = cone.Vertices.Add(new VertexTraits(0, 0, height / 2));
            
            return cone;
        }

        public static TriMesh CreateCone(int edge, double height)
        {
            TriMesh cone = new TriMesh();
            TriMesh.Vertex[] bottom = AddPolygon(cone, edge, -height / 2, false);
            TriMesh.Vertex top = cone.Vertices.Add(new VertexTraits(0, 0, height / 2));
            for (int i = 0; i < edge; i++)
            {
                int next = (i + 1) % edge;
                cone.Faces.AddTriangles(top, bottom[i], bottom[next]);
            }
            return cone;
        }

        public static TriMesh CreateCylinder(int edge, double height)
        {
            TriMesh cylinder = new TriMesh();
            TriMesh.Vertex[] top = AddPolygon(cylinder, edge, height / 2, true);
            TriMesh.Vertex[] bottom = AddPolygon(cylinder, edge, -height / 2, false);
            for (int i = 0; i < edge; i++)
            {
                int next = (i + 1) % edge;
                cylinder.Faces.AddTriangles(bottom[i], top[next], top[i]);
                cylinder.Faces.AddTriangles(bottom[i], bottom[next], top[next]);
            }
            return cylinder;
        }

        public static TriMesh.Vertex[] AddPolygon(TriMesh mesh, int edge, double z, bool topOrBottom)
        {
            List<TriMesh.Vertex> list = new List<HalfEdgeMesh.Vertex>();
            for (int i = 0; i < edge; i++)
            {
                double x = 0.5 * Math.Cos(Math.PI * 2 / edge * i);
                double y = 0.5 * Math.Sin(Math.PI * 2 / edge * i);
                list.Add(mesh.Vertices.Add(new VertexTraits(x, y, z)));
            }
            switch (edge)
            {
                case 3:
                    if (topOrBottom) mesh.Faces.AddTriangles(list[0], list[1], list[2]);
                    else mesh.Faces.AddTriangles(list[0], list[2], list[1]);
                    break;
                case 4:
                    int o = topOrBottom ? 3 : 1;
                    mesh.Faces.AddTriangles(list[0], list[2], list[o]);
                    mesh.Faces.AddTriangles(list[2], list[0], list[(o + 2) % 4]);
                    break;
                default:
                    TriMesh.Vertex center = mesh.Vertices.Add(new VertexTraits(0, 0, z));
                    for (int i = 0; i < edge; i++)
                    {
                        int next = topOrBottom ? (i + 1) % edge : (i + edge - 1) % edge;
                        mesh.Faces.AddTriangles(center, list[i], list[next]);
                    }
                    break;
            }
            return list.ToArray();
        }
    }
}
