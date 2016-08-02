using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public class LightDemo
    {
        public static readonly LightDemo Instance;

        TriMesh room;
        TriMesh cube;
        TriMesh sphere;

        static LightDemo()
        {
            Instance = new LightDemo();
        }

        LightDemo()
        {
            this.room = CreateRoom();
            this.cube = CreateCube();
            this.sphere = CreateSphere();
        }

        public void Draw()
        {
            DrawRoom();

            OpenGLManager.Instance.SetMaterialInfo();
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(false);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            DrawMesh(cube);
            DrawMesh(sphere);

            GL.Disable(EnableCap.Blend);
            GL.DepthMask(true);
            GL.Disable(EnableCap.DepthTest);
        }

        static void DrawMesh(TriMesh mesh)
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.Normalize); 

            GL.Begin(BeginMode.Triangles);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                foreach (TriMesh.Vertex vertex in mesh.Faces[i].Vertices)
                {
                    GL.Normal3(vertex.Traits.Normal.ToArray());
                    GL.Vertex3(vertex.Traits.Position.ToArray());
                }
            }
            GL.End();
        }

        void DrawRoom()
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Emission, Color.Black);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, Color.Black);
            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.Diffuse);
            GL.Enable(EnableCap.Normalize);

            GL.Begin(BeginMode.Triangles);
            for (int i = 0; i < this.room.Faces.Count; i++)
            {
                if (i < 2)
                {
                    GL.Color3(Color.Green);
                }
                else if (i >= 2 && i < 4)
                {
                    GL.Color3(Color.Red);
                }
                else
                {
                    GL.Color3(Color.Yellow);
                }
                foreach (TriMesh.Vertex vertex in this.room.Faces[i].Vertices)
                {
                    GL.Normal3(vertex.Traits.Normal.ToArray());
                    GL.Vertex3(vertex.Traits.Position.ToArray());
                }
            }
            GL.End();
            
        }

        static TriMesh CreateCube()
        {
            TriMesh cube = TriMeshIO.ReadFile("cube.obj");
            TriMeshUtil.ScaleToUnit(cube, 0.2);
            TriMeshUtil.MoveToCenter(cube);
            Vector3D move = new Vector3D(0.2, 0.1, -0.3);
            foreach (var v in cube.Vertices)
            {
                v.Traits.Position -= move;
            }
            TriMeshUtil.SetUpNormalVertex(cube);
            return cube;
        }

        static TriMesh CreateSphere()
        {
            TriMesh sphere = TriMeshIO.ReadFile("sphere.obj");
            TriMeshUtil.ScaleToUnit(sphere, 0.4);
            TriMeshUtil.MoveToCenter(sphere);
            Vector3D move = new Vector3D(-0.2, -0.1, -0.1);
            foreach (var v in sphere.Vertices)
            {
                v.Traits.Position -= move;
            }
            TriMeshUtil.SetUpNormalVertex(sphere);
            return sphere;
        }

        static TriMesh CreateRoom()
        {
            int edge = 4;
            double height = Math.Sqrt(2) / 2;
            TriMesh room = new TriMesh();
            TriMesh.Vertex[] top = AddPolygon(room, edge, height / 2, false);
            TriMesh.Vertex[] bottom = AddPolygon(room, edge, -height / 2, true);
            for (int i = 0; i < edge - 1; i++)
            {
                room.Faces.AddTriangles(bottom[i], top[i], top[i + 1]);
                room.Faces.AddTriangles(bottom[i], top[i + 1], bottom[i + 1]);
            }
            TriMeshUtil.ScaleToUnit(room, 1);
            TriMeshUtil.SetUpNormalVertex(room);
            return room;
        }

        static TriMesh.Vertex[] AddPolygon(TriMesh mesh, int edge, double z, bool topOrBottom)
        {
            List<TriMesh.Vertex> list = new List<HalfEdgeMesh.Vertex>();
            for (int i = 0; i < edge; i++)
            {
                double x = 0.5 * Math.Cos(Math.PI * 2 / edge * i + Math.PI * 0.75);
                double y = 0.5 * Math.Sin(Math.PI * 2 / edge * i + Math.PI * 0.75);
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
