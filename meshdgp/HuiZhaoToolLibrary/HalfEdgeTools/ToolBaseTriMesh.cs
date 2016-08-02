using System;
using System.Collections.Generic;
using System.Text;

 
 
 

namespace GraphicResearchHuiZhao
{
    public class ToolBaseTriMesh : AbstractTool
    {
        protected TriMesh mesh;
        public ToolBaseTriMesh(double width, double height, TriMesh mesh)
            : base(width,height)
        {
            this.mesh = mesh;
        }

        public override void MouseDown(Vector2D mouseDownPos, EnumMouseButton button)
        {
            if (mesh == null)
                return;
            base.MouseDown(mouseDownPos,button);

        }

        public override void MouseMove(Vector2D mouseMovePos, EnumMouseButton button)
        {
            if (mesh == null)
                return;
            base.MouseMove(mouseMovePos, button);

            OnChanged(EventArgs.Empty);
        }

        public override void MouseUp(Vector2D mouseUpPos, EnumMouseButton button)
        {
            if (mesh == null)
                return;

            base.MouseUp(mouseUpPos, button);

            OnChanged(EventArgs.Empty);
        }


        protected int SelectVertexByPoint(bool laser)
        {
            CRectangle viewport = new CRectangle(0, 0, Width, Height);
            OpenGLProjector projector = new OpenGLProjector();  
            double eps = ToolSetting.ToolsSetting.DepthTolerance; 
            double minDis = double.MaxValue;
            int minIndex = -1;
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                Vector3D v = projector.Project(mesh.Vertices[i].Traits.Position);
                Vector2D u = new  Vector2D(v.x, v.y);
                if (!viewport.Contains((int)v.x, (int)v.y)) continue;
                if (!laser && projector.GetDepthValue((int)v.x, 
                               (int)v.y) - v.z < eps) continue; 
                double dis = (u - mouseCurrPos).Length();
                if (dis < minDis)
                {
                    minIndex = i;
                    minDis = dis;
                }
            }
            if (minIndex == -1) return minIndex; 
            if (key == EnumKey.Shift)
                mesh.Vertices[minIndex].Traits.SelectedFlag = (byte)1;
            else if (key == EnumKey.Ctrl)
                mesh.Vertices[minIndex].Traits.SelectedFlag = (byte)0;
            else
            {

                for (int i = 0; i < mesh.Vertices.Count; i++)
                { 
                    mesh.Vertices[i].Traits.SelectedFlag = (byte)0; 
                } 

                mesh.Vertices[minIndex].Traits.SelectedFlag = (byte)1;
            }  
            return minIndex;
        }


        protected int SelectEdgeByPoint(bool laser)
        {
            CRectangle viewport = new CRectangle(0, 0, Width, Height);
            OpenGLProjector projector = new OpenGLProjector();
            TriMesh m = mesh;

            double eps = ToolSetting.ToolsSetting.DepthTolerance;

            double minDis = double.MaxValue;
            int minIndex = -1;
            for (int i = 0; i < m.Edges.Count; i++)
            {
                Vector3D v1 = projector.Project(m.Edges[i].Vertex0.Traits.Position);
                Vector3D v2 = projector.Project(m.Edges[i].Vertex1.Traits.Position);

                Vector2D u1 = new  Vector2D(v1.x, v1.y);
                Vector2D u2 = new  Vector2D(v2.x, v2.y);

                if (!viewport.Contains((int)v1.x, (int)v1.y) && !viewport.Contains((int)v2.x, (int)v2.y)) continue;

                //if (!laser&& projector.GetDepthValue((int)v1.x, (int)v1.y) - v1.z < eps ) continue;


                double dis = (u1 - mouseCurrPos).Length() + (u2 - mouseCurrPos).Length();
                if (dis < minDis)
                {
                    minIndex = i;
                    minDis = dis;
                }
            }
            if (minIndex == -1)
                return minIndex;


            if (key == EnumKey.Shift)
            {
                m.Edges[minIndex].Traits.SelectedFlag = (byte)1;
                
            }
            else if (key == EnumKey.Ctrl)
                m.Edges[minIndex].Traits.SelectedFlag = (byte)0;
            else
            {

                for (int i = 0; i < m.Edges.Count; i++)
                { 
                    m.Edges[i].Traits.SelectedFlag = (byte)0; 
                }


                m.Edges[minIndex].Traits.SelectedFlag = (byte)1;
            }
            return minIndex;
        }

        protected void SelectEdgeByRect(bool laser)
        {
            Vector2D minV =  Vector2D.Min(mouseDownPos, mouseCurrPos);
            Vector2D size =  Vector2D.Max(mouseDownPos, mouseCurrPos) - minV;
            CRectangle rect = new CRectangle((int)minV.x, (int)minV.y, (int)size.x, (int)size.y);
            CRectangle viewport = new CRectangle(0, 0, Width, Height);
            OpenGLProjector projector = new OpenGLProjector();
            TriMesh m = mesh;

            double eps = ToolSetting.ToolsSetting.DepthTolerance;


            for (int i = 0; i < m.Edges.Count; i++)
            {

                Vector3D v1 = projector.Project(m.Edges[i].Vertex0.Traits.Position);
                Vector3D v2 = projector.Project(m.Edges[i].Vertex1.Traits.Position);
                if (viewport.Contains((int)v1.x, (int)v1.y) && viewport.Contains((int)v2.x, (int)v2.y))
                {
                    bool flag = rect.Contains((int)v1.x, (int)v1.y) && rect.Contains((int)v2.x, (int)v2.y);

                    laser = ToolLaser();
                    flag &= (laser || projector.GetDepthValue((int)v1.x, (int)v1.y) - v1.z >= eps);

                    m.Edges[i].Traits.SelectedFlag = ToolKey(m.Edges[i].Traits.SelectedFlag,flag);

                }

            }

           
        }

        protected void SelectEdgeByCircle(bool laser)
        {  
            double radius = (mouseDownPos - mouseCurrPos).Length()/2; 
            Vector2D center = mouseDownPos + (mouseCurrPos - mouseDownPos) / 2;
            CRectangle viewport = new CRectangle(0, 0,  Width,  Height);
            OpenGLProjector projector = new OpenGLProjector(); 
            double eps = ToolSetting.ToolsSetting.DepthTolerance; 
           
            for (int i = 0; i < mesh.Edges.Count; i++)
            {
               
                Vector3D v1 = projector.Project(mesh.Edges[i].Vertex0.Traits.Position);
                Vector3D v2 = projector.Project(mesh.Edges[i].Vertex1.Traits.Position);

                Vector2D u1 = new  Vector2D(v1.x, v1.y);
                Vector2D u2 = new  Vector2D(v2.x, v2.y);
                if (viewport.Contains((int)v1.x, (int)v1.y) && viewport.Contains((int)v2.x, (int)v2.y))
                {
                    bool flag = (u1 - center).Length() <= radius && (u2 - center).Length() <= radius;

                    laser = ToolLaser(); 
                    flag &= (laser || projector.GetDepthValue((int)v1.x, (int)v1.y) - v1.z >= eps);

                    mesh.Edges[i].Traits.SelectedFlag = ToolKey(mesh.Edges[i].Traits.SelectedFlag,flag);

                }
            } 
        }



        private byte ToolKey(byte selectedFlag, bool flag)
        {
            byte meshFlag = selectedFlag;
            if (key == EnumKey.Shift)
            {
                if (flag)
                {
                    meshFlag = (byte)1;
                }
            }
            else if (key == EnumKey.Ctrl)
            {
                if (flag)
                    meshFlag = (byte)0;
            }
            else
            {
                meshFlag = (byte)((flag) ? 1 : 0);
            }

            return meshFlag;
        }

        private bool ToolLaser()
        {
            bool laser = true;
            if (key == EnumKey.Alt)
            {
                laser = true;
            }
            else
            {
                laser = false;
            }
            return laser;
        }
         


        protected void SelectVertexByCircle(bool laser)
        { 
            double radius = (mouseDownPos - mouseCurrPos).Length()/2;
            Vector2D center = mouseDownPos+(mouseCurrPos - mouseDownPos) / 2;
            CRectangle viewport = new CRectangle(0, 0, Width, Height);
            OpenGLProjector projector = new OpenGLProjector();  
            double eps = ToolSetting.ToolsSetting.DepthTolerance; 

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                Vector3D v = projector.Project(mesh.Vertices[i].Traits.Position);
                Vector2D u = new Vector2D(v.x, v.y);
                if (viewport.Contains((int)v.x, (int)v.y))
                {
                    bool flag = (u - center).Length() <= radius; 
                    laser = ToolLaser();
                    flag &= (laser || 
                        projector.GetDepthValue((int)v.x, (int)v.y) - v.z >= eps);
                    mesh.Vertices[i].Traits.SelectedFlag = 
                        ToolKey(mesh.Vertices[i].Traits.SelectedFlag,flag);

                }
            } 
        }

        protected void SelectFaceByCircle(bool laser)
        {

            double radius = (mouseDownPos - mouseCurrPos).Length() / 2;
            Vector2D center = mouseDownPos + (mouseCurrPos - mouseDownPos) / 2;
            CRectangle viewport = new CRectangle(0, 0, Width, Height);
            OpenGLProjector projector = new OpenGLProjector();
            TriMesh m = mesh;

            double eps = ToolSetting.ToolsSetting.DepthTolerance;


            for (int i = 0; i < m.Faces.Count; i++)
            {

                Vector3D v1 = projector.Project(m.Faces[i].GetVertex(0).Traits.Position);
                Vector3D v2 = projector.Project(m.Faces[i].GetVertex(1).Traits.Position);
                Vector3D v3 = projector.Project(m.Faces[i].GetVertex(2).Traits.Position);

                Vector2D u1 = new Vector2D(v1.x, v1.y);
                Vector2D u2 = new Vector2D(v2.x, v2.y);
                Vector2D u3 = new Vector2D(v3.x, v3.y);

                if (viewport.Contains((int)v1.x, (int)v1.y) && viewport.Contains((int)v2.x, (int)v2.y) && viewport.Contains((int)v3.x, (int)v3.y))
                {
                    bool flag = (u1 - center).Length() <= radius && (u2 - center).Length() <= radius && (u3 - center).Length() <= radius;

                    laser = ToolLaser();
                    flag &= (laser || projector.GetDepthValue((int)v1.x, (int)v1.y) - v1.z >= eps);

                    m.Faces[i].Traits.SelectedFlag=ToolKey(m.Faces[i].Traits.SelectedFlag, flag);
                     

                }
            }
                

            
        }

        protected void SelectFaceByRect(bool laser)
        {
            Vector2D minV = GraphicResearchHuiZhao.Vector2D.Min(mouseDownPos, mouseCurrPos);
            Vector2D size = GraphicResearchHuiZhao.Vector2D.Max(mouseDownPos, mouseCurrPos) - minV;
            CRectangle rect = new CRectangle((int)minV.x, (int)minV.y, (int)size.x, (int)size.y);
            CRectangle viewport = new CRectangle(0, 0,  Width,  Height);
            OpenGLProjector projector = new OpenGLProjector();
            TriMesh m = mesh;

            double eps = ToolSetting.ToolsSetting.DepthTolerance;


            for (int i = 0; i < m.Faces.Count; i++)
            {

                Vector3D v1 = projector.Project(m.Faces[i].GetVertex(0).Traits.Position);
                Vector3D v2 = projector.Project(m.Faces[i].GetVertex(1).Traits.Position);
                Vector3D v3 = projector.Project(m.Faces[i].GetVertex(2).Traits.Position);

                if (viewport.Contains((int)v1.x, (int)v1.y) && viewport.Contains((int)v2.x, (int)v2.y) && viewport.Contains((int)v3.x, (int)v3.y))
                {
                    bool flag = rect.Contains((int)v1.x, (int)v1.y) && rect.Contains((int)v2.x, (int)v2.y) && rect.Contains((int)v3.x, (int)v3.y);

                    laser = ToolLaser();
                    flag &= (laser || projector.GetDepthValue((int)v1.x, (int)v1.y) - v1.z >= eps);

                    m.Faces[i].Traits.SelectedFlag = ToolKey(m.Faces[i].Traits.SelectedFlag,flag);
                     

                }
            }
        }

             



        protected void SelectVertexByRect(bool laser)
        {
            Vector2D minV =  Vector2D.Min(mouseDownPos, mouseCurrPos);
            Vector2D size =  Vector2D.Max(mouseDownPos, mouseCurrPos) - minV;
            CRectangle rect = new CRectangle((int)minV.x, (int)minV.y, 
                                              (int)size.x, (int)size.y);
            CRectangle viewport = new CRectangle(0, 0, Width, Height); 
            OpenGLProjector projector = new OpenGLProjector();  
            double eps = ToolSetting.ToolsSetting.DepthTolerance;  
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                Vector3D v = projector.Project(mesh.Vertices[i].Traits.Position);
                if (viewport.Contains((int)v.x, (int)v.y))
                {
                    bool flag = rect.Contains((int)v.x, (int)v.y); 
                    laser = ToolLaser();
                    flag &= (laser || projector.GetDepthValue((int)v.x, 
                                               (int)v.y) - v.z >= eps);
                    mesh.Vertices[i].Traits.SelectedFlag = 
                        ToolKey(mesh.Vertices[i].Traits.SelectedFlag,flag);
                   
                }
            } 
        }



        protected int SelectFaceByPoint(bool laser)
        {
            CRectangle viewport = new CRectangle(0, 0,  Width,  Height);
            OpenGLProjector projector = new OpenGLProjector();
            TriMesh m = mesh;

            double eps = ToolSetting.ToolsSetting.DepthTolerance;

            bool isInFace = false;
            int minIndex = -1;
            for (int i = 0; i < m.Faces.Count; i++)
            {

                Vector3D v1 = projector.Project(m.Faces[i].GetVertex(0).Traits.Position);
                Vector3D v2 = projector.Project(m.Faces[i].GetVertex(1).Traits.Position);
                Vector3D v3 = projector.Project(m.Faces[i].GetVertex(2).Traits.Position);


                Vector2D u1 = new GraphicResearchHuiZhao.Vector2D(v1.x, v1.y);
                Vector2D u2 = new GraphicResearchHuiZhao.Vector2D(v2.x, v2.y);
                Vector2D u3 = new GraphicResearchHuiZhao.Vector2D(v3.x, v3.y);

                if (!viewport.Contains((int)v1.x, (int)v1.y) && !viewport.Contains((int)v2.x, (int)v2.y) && !viewport.Contains((int)v2.x, (int)v2.y)) continue;

                //  if (!laser && projector.GetDepthValue((int)v1.x, (int)v1.y) - v1.z < eps) continue;
                //isInFace=PointinTriangle(u1,u2,u3,mouseCurrPos);
                isInFace = IsInTriangle(u1, u2, u3, mouseCurrPos);
                if (isInFace)
                {
                    minIndex = i;
                }


            }
            if (minIndex == -1) return minIndex;



            if (key == EnumKey.Shift)
                m.Faces[minIndex].Traits.SelectedFlag = (byte)1;
            else if (key == EnumKey.Ctrl)
                m.Faces[minIndex].Traits.SelectedFlag = (byte)0;
            else
            {

                for (int i = 0; i < m.Faces.Count; i++)
                {

                    m.Faces[i].Traits.SelectedFlag = (byte)0;

                }


                m.Faces[minIndex].Traits.SelectedFlag = (byte)1;
            }
            return minIndex;
        }

        protected  bool PointinTriangle( Vector2D A,  Vector2D B, Vector2D C,  Vector2D P)
        {
            Vector2D v0 = C - A;
            Vector2D v1 = B - A;
            Vector2D v2 = P - A;

            float dot00 = (float)v0.Dot(v0);
            float dot01 = (float)v0.Dot(v1);
            float dot02 = (float)v0.Dot(v2);
            float dot11 = (float)v1.Dot(v1);
            float dot12 = (float)v1.Dot(v2);

            float inverDeno = 1 / (dot00 * dot11 - dot01 * dot01);

            float u = (dot11 * dot02 - dot01 * dot12) * inverDeno;
            if (u < 0 || u > 1) // if u out of range, return directly
            {
                return false;
            }

            float v = (dot00 * dot12 - dot01 * dot02) * inverDeno;
            if (v < 0 || v > 1) // if v out of range, return directly
            {
                return false;
            }

            return u + v <= 1;
        }

        protected double GetTriangleSquar( Vector2D a,  Vector2D b,  Vector2D c)
        {
            Vector2D AB, BC;
            AB.x = b.x - a.x;
            AB.y = b.y - a.y;
            BC.x = c.x - b.x;
            BC.y = c.y - b.y;
            return Math.Abs((AB.x * BC.y - AB.y * BC.x)) / 2.0;
        }

        protected bool IsInTriangle( Vector2D A,  Vector2D B,  Vector2D C,  Vector2D P)
        {
            double SABC, SADB, SBDC, SADC;
            SABC = GetTriangleSquar(A, B, C);
            SADB = GetTriangleSquar(A, P, B);
            SBDC = GetTriangleSquar(B, P, C);
            SADC = GetTriangleSquar(A, P, C);

            double SumSuqar = SADB + SBDC + SADC;

            if ((-0.0001 < (SABC - SumSuqar)) && ((SABC - SumSuqar) < 0.0001))
            {
                return true;
            }
            else
            {
                return false;
            }
        }  

    }
}
