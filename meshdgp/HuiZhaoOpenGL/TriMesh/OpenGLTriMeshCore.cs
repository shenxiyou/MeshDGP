using System;
using System.Collections.Generic;
using System.Drawing;
 
 
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public partial class OpenGLTriMesh
    {
        private static OpenGLTriMesh instance = null;
        public static OpenGLTriMesh Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OpenGLTriMesh();
                }
                return instance;
            }
        }

        public void DrawTriMesh(TriMesh mesh)
        {
            switch (GlobalSetting.DisplaySetting.DisplayMode)
            {
                case EnumDisplayMode.Basic:
                    DrawSelectedComplex(mesh);
                    break;
                case EnumDisplayMode.Vertex:
                    DrawPoints(mesh);
                    break;
                case EnumDisplayMode.WireFrame:
                    DrawWireFrame(mesh);
                    break;
                case EnumDisplayMode.DarkWireFrame:
                    DrawWireFrameDark(mesh);
                    break;
                case EnumDisplayMode.Flat:
                    DrawFlatShaded(mesh);
                    break;
                case EnumDisplayMode.FlatLine:
                    DrawFlatHiddenLine(mesh);
                    break;
                case EnumDisplayMode.Smooth:
                    DrawSmoothShaded(mesh);
                    break;
                case EnumDisplayMode.SmoothLine:
                    DrawSmoothHiddenLine(mesh);
                    break;
                case EnumDisplayMode.FaceColor:
                    DrawFaceColorRandom(mesh);
                    break;  
                case EnumDisplayMode.VertexNormal:
                    DrawSelectedComplex(mesh);
                    DrawVertexNormal(mesh);
                    break;
                case EnumDisplayMode.FaceNormal:
                    DrawSelectedComplex(mesh);
                    DrawFaceNormal(mesh);
                    break;
                case EnumDisplayMode.PricipalDirection:
                    DrawSelectedComplex(mesh);
                    DrawVertexPrincipleDirection(mesh);
                    break;
         

                case EnumDisplayMode.Hole: 
                  
                    DrawSelectedComplex(mesh);
                    break;

                case EnumDisplayMode.SelectedVertex:
                    DrawSmoothShaded(mesh);
                    DrawSelectedVerticeBySphere(mesh);
                    break;

                case EnumDisplayMode.SelectedFace:  
                    DrawSelectedFace(mesh);
                    break;

                
                case EnumDisplayMode.EdgesColorAndVertexColor:
                    DrawSmoothShaded(mesh);
                    DrawSelectedEdges(mesh);
                    DrawSelectedVerticeBySphere(mesh);
                    break;
                case EnumDisplayMode.SelectedEdge: 
                    DrawSelectedEdges(mesh);
                    break;

                case EnumDisplayMode.Dual:
                    DrawDual(mesh);
                    break;


                case EnumDisplayMode.VertexColor:
                    DrawVertexColor(mesh);
                    break;

                case EnumDisplayMode.PointsWithLine:
                    DrawPointsWithLine(mesh);
                    break;
                

                case EnumDisplayMode.DualMeshB:
                    DrawDualMesh(mesh);
                    break;

                case EnumDisplayMode.DualMeshC:
                    DrawDualMesh(mesh);
                    break;

                case EnumDisplayMode.DualOnly:
                    DrawDualMeshLine(GlobalData.Instance.DualMesh);
                    break;

                case EnumDisplayMode.Textured:
                    DrawTextureShaded(mesh);
                    break;

                

                case EnumDisplayMode.SegementationVertex:
                    DrawSegementationVertex(mesh);
                    break;

                case EnumDisplayMode.SegementationFace:
                    DrawSegementationWithBoundary(mesh);
                    break;
               

                case EnumDisplayMode.SelectedSmooth:
                    DrawSelectedSmooth(mesh);
                    break;

                case EnumDisplayMode.Vector:
                    DrawSelectedSmooth(mesh);
                    DrawTrivialConnection(mesh,GlobalData.Instance.FaceVectors,GlobalData.Instance.N);
                    break;

                case EnumDisplayMode.TreeCotree:
                    DrawSelectedComplex(mesh);
                    DrawTreeCotree(GlobalData.Instance.tree, GlobalData.Instance.cotree);
                    break;
                case EnumDisplayMode.Generator:
                    DrawSelectedComplex(mesh);
                    DrawGenerators(mesh,GlobalData.Instance.Generators);
                    break;

               

                case EnumDisplayMode.ColorVis:
                    DrawColorVis(mesh);
                    break;
                case EnumDisplayMode.NPRLine:
                    //DrawSelectedEdgeOnly(mesh);
                    //DrawDarkWireFrame(mesh);

                    DrawNPR(mesh);
                    break;

                case EnumDisplayMode.FaceColored :
                    DrawFaceColor(mesh);
                    break;


                case EnumDisplayMode.Accum:
                    DrawAccum(mesh);
                    break;
                case EnumDisplayMode.AccumAdd:
                    DrawAccumAdd(mesh);
                    break;
                case EnumDisplayMode.Stencil:
                    DrawStencil(mesh);
                    break;
                case EnumDisplayMode.Transparent :
                    DrawTransparent(mesh);
                    break;

                case EnumDisplayMode.TransparentTwo:
                    DrawTransparentTwo(mesh);
                    break;

                case EnumDisplayMode.Shadow:
                    DrawShadow(mesh);
                    break;

                case EnumDisplayMode.Mirror:
                    DrawMirror(mesh);
                    break;
                case EnumDisplayMode.OutSide:
                    DrawOutSide(mesh);
                    break;

                case EnumDisplayMode.Axis:
                    DrawAxis();
                    break;

                case EnumDisplayMode.Color:
                    DrawColor(mesh);
                    break; 

            }
        }


        public event EventHandler<TriMeshDrawEventArgs> OpenGLDraw;

        public void DrawOutSide(TriMesh mesh)
        {
            if (OpenGLDraw != null)
            {
                OpenGLDraw(this, new TriMeshDrawEventArgs { Mesh = mesh });
            }
        }


        public void DrawAxis()
        {
            GL.Begin(BeginMode.Lines);
            OpenGLManager.Instance.SetColor(Color.Red);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0.5, 0, 0);

            OpenGLManager.Instance.SetColor(Color.Green);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0.5, 0);

            OpenGLManager.Instance.SetColor(Color.Blue);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, 0.5);
            GL.End();
        }




        
    }
}
