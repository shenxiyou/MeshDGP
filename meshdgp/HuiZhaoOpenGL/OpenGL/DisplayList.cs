using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
 
 

namespace GraphicResearchHuiZhao 
{
    public class DisplayList
    {
        
        public DisplayLists Lists;
        private TriMesh mesh = null;
        const float vectorScale = 0.1f;


        public DisplayList(TriMesh mesh)
        {
            Lists = new DisplayLists(2);
            this.mesh = mesh;
        }

        public void Initialize()
        {
            //MainForm.Instance.MeshView3D.MakeCurrent();
            if (GL.IsList(Lists.Mesh) == false)
            {
                Lists = new DisplayLists(GL.GenLists(Lists.Count));
            }

            // Mesh
            GL.NewList(Lists.Mesh,ListMode.Compile);
            GL.Color3(1.0f, 1.0f, 1.0f);
            GL.Begin(BeginMode.Triangles);
            foreach (TriMesh.Face f in mesh.Faces)
            {
                foreach (TriMesh.HalfEdge h in f.Halfedges)
                {
                    TriMesh.Vertex v = h.ToVertex;
                    GL.Normal3(h.Traits.Normal.x, h.Traits.Normal.y, h.Traits.Normal.z);
                    GL.TexCoord2(h.Traits.TextureCoordinate.x, h.Traits.TextureCoordinate.y);
                    GL.Vertex3(v.Traits.Position.x, v.Traits.Position.y, v.Traits.Position.z);
                }
            }
            GL.End();
            GL.EndList();
 

            // Axes
            GL.NewList(Lists.Axes, ListMode.Compile);
            const int axesSize = 10; 
            GL.DepthMask(false);
            GL.Enable(EnableCap.Blend);
            GL.Begin(BeginMode.Lines); 
            for (int i = -axesSize; i <= axesSize; ++i)  // yz-plane
            {
                GL.Color4(1.0f, 0.3f, 0.3f, 0.0f);
                GL.Vertex3(0.0f, -axesSize, i);
                GL.Color4(1.0f, 0.3f, 0.3f, 0.3f);
                GL.Vertex3(0.0f, axesSize, i);
                GL.Color4(1.0f, 0.3f, 0.3f, 0.0f);
                GL.Vertex3(0.0f, i, -axesSize);
                GL.Color4(1.0f, 0.3f, 0.3f, 0.3f);
                GL.Vertex3(0.0f, i, axesSize);
            }
            for (int i = -axesSize; i <= axesSize; ++i)  // xz-plane
            {
                GL.Color4(0.3f, 1.0f, 0.3f, 0.0f);
                GL.Vertex3(-axesSize, 0.0f, i);
                GL.Color4(0.3f, 1.0f, 0.3f, 0.3f);
                GL.Vertex3(axesSize, 0.0f, i);
                GL.Color4(0.3f, 1.0f, 0.3f, 0.0f);
                GL.Vertex3(i, 0.0f, -axesSize);
                GL.Color4(0.3f, 1.0f, 0.3f, 0.3f);
                GL.Vertex3(i, 0.0f, axesSize);
            }
            for (int i = -axesSize; i <= axesSize; ++i)  // xy-plane
            {
                GL.Color4(0.3f, 0.3f, 1.0f, 0.0f);
                GL.Vertex3(i, -axesSize, 0.0f);
                GL.Color4(0.3f, 0.3f, 1.0f, 0.3f);
                GL.Vertex3(i, axesSize, 0.0f);
                GL.Color4(0.3f, 0.3f, 1.0f, 0.0f);
                GL.Vertex3(-axesSize, i, 0.0f);
                GL.Color4(0.3f, 0.3f, 1.0f, 0.3f);
                GL.Vertex3(axesSize, i, 0.0f);
            }
            GL.End();
            GL.Disable(EnableCap.Blend); 
            GL.LineWidth(2.0f);
            GL.Begin(BeginMode.Lines); 
            GL.Color3(1.0f, 0.3f, 0.3f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);  // x-axis
            GL.Vertex3(axesSize, 0.0f, 0.0f);
            GL.Color3(0.3f, 1.0f, 0.3f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);  // y-axis
            GL.Vertex3(0.0f, axesSize, 0.0f);
            GL.Color3(0.3f, 0.3f, 1.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);  // z-axis
            GL.Vertex3(0.0f, 0.0f, axesSize);
            GL.End();
            GL.LineWidth(1.0f);
            GL.DepthMask(true);
            GL.EndList();

            // Edges
            GL.NewList(Lists.Edges, ListMode.Compile);
            GL.Enable(EnableCap.Blend);
            GL.Begin(BeginMode.Lines);
            foreach (TriMesh.Edge e in mesh.Edges)
            {
                Vector3D edgeStart = e.Vertex0.Traits.Position;
                Vector3D edgeEnd = e.Vertex1.Traits.Position;
                if (e.OnBoundary)
                {
                    GL.Color4(1.0f, 0.2f, 0.2f, 0.5f);
                }
                else
                {
                    GL.Color4(0.5f, 1.0f, 0.0f, 0.5f);
                }
                GL.Vertex3(edgeStart.x, edgeStart.y, edgeStart.z);
                GL.Vertex3(edgeEnd.x, edgeEnd.y, edgeEnd.z);
            }
            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.EndList();

            // Vertex normals
            GL.NewList(Lists.VertexNormals, ListMode.Compile);
            GL.Color4(0.0f, 0.5f, 1.0f, 0.5f);
            GL.Enable(EnableCap.Blend);
            GL.Begin(BeginMode.Lines);
            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                Vector3D normalStart = v.Traits.Position;
                Vector3D normalEnd = v.Traits.Position + vectorScale * v.Traits.Normal;
                GL.Vertex3(normalStart.x, normalStart.y, normalStart.z);
                GL.Vertex3(normalEnd.x, normalEnd.y, normalEnd.z);
            }
            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.EndList();

            // Face vertex normals
            GL.NewList(Lists.FaceVertexNormals, ListMode.Compile);
            GL.Color4(0.5f, 0.0f, 1.0f, 0.5f);
            GL.Enable(EnableCap.Blend);
            GL.Begin(BeginMode.Lines);
            foreach (TriMesh.HalfEdge h in mesh.HalfEdges)
            {
                Vector3D normalStart = h.ToVertex.Traits.Position;
                Vector3D normalEnd = normalStart + vectorScale * h.Traits.Normal;
                GL.Vertex3(normalStart.x, normalStart.y, normalStart.z);
                GL.Vertex3(normalEnd.x, normalEnd.y, normalEnd.z);
            }
            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.EndList();

            // Principle curvature
            GL.NewList(Lists.PrincipleCurvatures, ListMode.Compile);
            GL.Enable(EnableCap.Blend);
            GL.LineWidth(1.0f);
            GL.Begin(BeginMode.Lines);
            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                // Min curvature
                double scale = ClampRange(v.Traits.MinCurvature, -1.0f, 1.0f);
                GL.Color4(1.0f, 0.0f, 0.5f, 0.5f);
                Vector3D vectorStart = v.Traits.Position;
                Vector3D vectorEnd = v.Traits.Position + (vectorScale * scale) * v.Traits.MinCurvatureDirection;
                GL.Vertex3(vectorStart.x, vectorStart.y, vectorStart.z);
                GL.Vertex3(vectorEnd.x, vectorEnd.y, vectorEnd.z);
            }
            foreach (TriMesh.Vertex v in mesh.Vertices)
            {
                // Max curvature
                double scale = ClampRange(v.Traits.MaxCurvature, -1.0f, 1.0f);
                GL.Color4(1.0f, 0.5f, 0.0f, 0.5f);
                Vector3D vectorStart = v.Traits.Position;
                Vector3D vectorEnd = v.Traits.Position + (vectorScale * scale) * v.Traits.MaxCurvatureDirection;
                GL.Vertex3(vectorStart.x, vectorStart.y, vectorStart.z);
                GL.Vertex3(vectorEnd.x, vectorEnd.y, vectorEnd.z);
            }
            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.EndList();

            //// Bounding sphere
            //GL.NewList(displayLists.BoundingSphere, ListMode.Compile);
            //GL.Enable(EnableCap.Blend);
            //OpenTK.Graphics.Glu.Sphere sphere = new OpenTK.Graphics.Glu.Sphere();  
            //GL.PushMatrix();
            //Geometry.Vector3d sphereCenter = mesh.Traits.BoundingSphere.Center;
            //GL.Translate(sphereCenter.x, sphereCenter.y, sphereCenter.z);
            //GL.Rotate (90.0f, 1.0f, 0.0f, 0.0f);  // Rotate so poles are on y-axis
            //OpenTK.Graphics.Glu.QuadricDrawStyle(sphere, );
            //GL.Color4(0.0f, 1.0f, 0.5f, 0.25f);
            //OpenTK.Graphics.Glu.gluSphere(sphere, mesh.Traits.BoundingSphere.Radius, 24, 24);   // Draw filled
            //GL.Color4(0.0f, 1.0f, 0.5f, 0.5f);
            //OpenTK.Graphics.Glu.gluQuadricDrawStyle(sphere, Glu.GLU_SILHOUETTE);
            //OpenTK.Graphics.Glu.gluSphere(sphere, mesh.Traits.BoundingSphere.Radius, 24, 24);   // Draw edges
            //GL.PopMatrix();
            //OpenTK.Graphics.Glu.gluDeleteQuadric(sphere);
            //GL.Disable(EnableCap.Blend);
            //GL.EndList();

            // Bounding Box
            GL.NewList(Lists.BoundingBox, ListMode.Compile);
            GL.Enable(EnableCap.Blend);
            Vector3D boxMax = mesh.Traits.BoundingBox.Max;
            Vector3D boxMin = mesh.Traits.BoundingBox.Min;
            // Draw filled
            GL.Color4(0.0f, 0.666f, 0.666f, 0.25f);
            GL.Begin(BeginMode.Quads);
            GL.Vertex3(boxMin.x, boxMin.y, boxMin.z);    // Bottom
            GL.Vertex3(boxMax.x, boxMin.y, boxMin.z);
            GL.Vertex3(boxMax.x, boxMin.y, boxMax.z);
            GL.Vertex3(boxMin.x, boxMin.y, boxMax.z);
            GL.Vertex3(boxMin.x, boxMax.y, boxMax.z);    // Top
            GL.Vertex3(boxMax.x, boxMax.y, boxMax.z);
            GL.Vertex3(boxMax.x, boxMax.y, boxMin.z);
            GL.Vertex3(boxMin.x, boxMax.y, boxMin.z);
            GL.Vertex3(boxMin.x, boxMax.y, boxMax.z);    // Left
            GL.Vertex3(boxMin.x, boxMax.y, boxMin.z);
            GL.Vertex3(boxMin.x, boxMin.y, boxMin.z);
            GL.Vertex3(boxMin.x, boxMin.y, boxMax.z);
            GL.Vertex3(boxMax.x, boxMax.y, boxMin.z);    // Right
            GL.Vertex3(boxMax.x, boxMax.y, boxMax.z);
            GL.Vertex3(boxMax.x, boxMin.y, boxMax.z);
            GL.Vertex3(boxMax.x, boxMin.y, boxMin.z);
            GL.Vertex3(boxMax.x, boxMax.y, boxMax.z);    // Near
            GL.Vertex3(boxMin.x, boxMax.y, boxMax.z);
            GL.Vertex3(boxMin.x, boxMin.y, boxMax.z);
            GL.Vertex3(boxMax.x, boxMin.y, boxMax.z);
            GL.Vertex3(boxMin.x, boxMax.y, boxMin.z);    // Far
            GL.Vertex3(boxMax.x, boxMax.y, boxMin.z);
            GL.Vertex3(boxMax.x, boxMin.y, boxMin.z);
            GL.Vertex3(boxMin.x, boxMin.y, boxMin.z);
            GL.End();
            // Draw edges
            GL.Color4(0.0f, 0.666f, 0.666f, 0.5f);
            GL.Begin(BeginMode.LineLoop);    // Bottom
            GL.Vertex3(boxMin.x, boxMin.y, boxMin.z);
            GL.Vertex3(boxMax.x, boxMin.y, boxMin.z);
            GL.Vertex3(boxMax.x, boxMin.y, boxMax.z);
            GL.Vertex3(boxMin.x, boxMin.y, boxMax.z);
            GL.End();
            GL.Begin(BeginMode.LineLoop);    // Top
            GL.Vertex3(boxMin.x, boxMax.y, boxMax.z);
            GL.Vertex3(boxMax.x, boxMax.y, boxMax.z);
            GL.Vertex3(boxMax.x, boxMax.y, boxMin.z);
            GL.Vertex3(boxMin.x, boxMax.y, boxMin.z);
            GL.End();
            GL.Begin(BeginMode.Lines);    // Sides
            GL.Vertex3(boxMin.x, boxMin.y, boxMax.z);
            GL.Vertex3(boxMin.x, boxMax.y, boxMax.z);
            GL.Vertex3(boxMax.x, boxMin.y, boxMax.z);
            GL.Vertex3(boxMax.x, boxMax.y, boxMax.z);
            GL.Vertex3(boxMax.x, boxMin.y, boxMin.z);
            GL.Vertex3(boxMax.x, boxMax.y, boxMin.z);
            GL.Vertex3(boxMin.x, boxMin.y, boxMin.z);
            GL.Vertex3(boxMin.x, boxMax.y, boxMin.z);
            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.EndList();
        }

        public static double ClampRange(double value, double min, double max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

    }
}
