using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GraphicResearchHuiZhao 
{
    public partial class OpenGLTetMesh
    {
        private static OpenGLTetMesh instance = null;
        public static OpenGLTetMesh Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OpenGLTetMesh();
                }
                return instance;
            }
        }

        Method[] methods;

        private OpenGLTetMesh()
        {
            methods = new Method[]{
                new Method(TetDisplayFlag.SelectedTetrahedron, DrawSelectedTetrahedron),
                new Method(TetDisplayFlag.SelectedFace, DrawSelectedFaces),
                new Method(TetDisplayFlag.SelectedEdge, DrawSelectedEdges),
                new Method(TetDisplayFlag.SelectedVertex, DrawSelectedVertices),
                new Method(TetDisplayFlag.SelectedFaceNormal, DrawSelectedFaceNormal),
                new Method(TetDisplayFlag.SelectedVertexNormal, DrawSelectedVertexNormal),
                new Method(TetDisplayFlag.Transparent, BeginTransparent),
                new Method(TetDisplayFlag.Face, DrawSmoothShaded),
                new Method(TetDisplayFlag.Edge, DrawWireFrame),
                new Method(TetDisplayFlag.Vertex, DrawPoints),
                new Method(TetDisplayFlag.FaceNormal, DrawFaceNormal),
                new Method(TetDisplayFlag.VertexNormal, DrawVertexNormal),
                new Method(TetDisplayFlag.Transparent, EndTransparent),
            };
        }

        public void DrawFlag(TetMesh mesh)
        {
            TetDisplayFlag mode = GlobalTetSetting.TetDisplayFlag;
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.PolygonOffsetFill);
            foreach (var item in methods)
            {
                if ((mode & item.Flag) == item.Flag)
                {
                    item.Action(mesh);
                }
            }
            GL.Disable(EnableCap.PolygonOffsetFill);
        }

        struct Method
        {
            public TetDisplayFlag Flag;
            public Action<TetMesh> Action;

            public Method(TetDisplayFlag flag, Action<TetMesh> action)
            {
                this.Flag = flag;
                this.Action = action;
            }
        }
    }
}
