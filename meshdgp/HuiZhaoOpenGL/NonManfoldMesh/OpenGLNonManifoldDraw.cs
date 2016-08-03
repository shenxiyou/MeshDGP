using System;
using System.Collections.Generic;
using System.Drawing;
 
 


using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao
{
    public partial class OpenGLNonManifold
    {
        public  void DrawMesh(NonManifoldMesh mesh)
        {
            switch (GlobalSetting.DisplaySetting.DisplayMode)
            {
                case EnumDisplayMode.Vertex:
                    OpenGLNonManifold.Instance.DrawPoints(mesh);
                    break;
                case EnumDisplayMode.WireFrame:
                    OpenGLNonManifold.Instance.DrawWireframe(mesh);
                    break;

                case EnumDisplayMode.Flat:
                    OpenGLNonManifold.Instance.DrawFlatShaded(mesh);
                    break;
                case EnumDisplayMode.FlatLine:
                    //  OpenGLOperators.Instance.DrawFlatHiddenLine(mesh);
                    OpenGLNonManifold.Instance.DrawTangentShaded(mesh);
                    break;

                case EnumDisplayMode.Smooth:
                    OpenGLNonManifold.Instance.DrawSmoothShaded(mesh);
                   // OpenGLOperators.Instance.DrawSilhouetteShaded(mesh);
                    break;
                case EnumDisplayMode.SmoothLine:
                    OpenGLNonManifold.Instance.DrawSmoothHiddenLine(mesh);
                    break;
                case EnumDisplayMode.Transparent:
                    OpenGLNonManifold.Instance.DrawTransparentShaded(mesh);
                    break;
                
             
                   
                case EnumDisplayMode.FaceColored:
                    OpenGLNonManifold.Instance.DrawFaceColorFlatShaded(mesh);
                    break;
                case EnumDisplayMode.Boundary:
                    OpenGLNonManifold.Instance.DrawBoundaryVertice(mesh);
                    break;

                case EnumDisplayMode.Laplacian:
                    OpenGLNonManifold.Instance.DrawSmoothShaded(mesh);
                    OpenGLNonManifold.Instance.DrawLapCoord(mesh);
                    break;
                case EnumDisplayMode.VertexColor:

                    OpenGLNonManifold.Instance.DrawVertexColorShaded(mesh);
                    break;
                
              


                case EnumDisplayMode.DualMesh:

                    OpenGLNonManifold.Instance.DrawSmoothHiddenLine(mesh.DualMesh);
                    break;

               
            }
        }


        public static void DrawShape( )
        {

        }
    }
}
