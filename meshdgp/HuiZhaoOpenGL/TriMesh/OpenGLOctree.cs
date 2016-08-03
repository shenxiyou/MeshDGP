using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace GraphicResearchHuiZhao 
{
    public class OpenGLOctree
    {
        private static OpenGLOctree instance = null;
        public static OpenGLOctree Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OpenGLOctree();
                }
                return instance;
            }
        }

        public void DrawOctree(OctTree<TriMesh.Vertex> octree)
        {
            Stack< Cell<TriMesh.Vertex>> cellStack =
                new Stack< Cell<TriMesh.Vertex>>();

            cellStack.Push(octree.Root);

            while (cellStack.Count != 0)
            {
                 Cell<TriMesh.Vertex> cell = cellStack.Pop();

                #region Draw box

                DrawBindBox(cell.Box);

                #endregion

                if (cell.Childrens != null)
                {
                    for (int i = 0; i < cell.Childrens.Length; i++)
                    {
                        cellStack.Push(cell.Childrens[i]);
                    }
                }
            }

            cellStack = null;

            GL.Flush();
        }

        //public void DrawBindBox( BindBox box)
        //{
        //    GL.Begin(BeginMode.Lines);

        //    //1
        //    GL.Vertex3(box.x, box.y, box.z);
        //    GL.Vertex3(box.x, box.y + box.ySize, box.z);

        //    GL.Vertex3(box.x, box.y + box.ySize, box.z);
        //    GL.Vertex3(box.x + box.xSize, box.y + box.ySize, box.z);

        //    GL.Vertex3(box.x, box.y + box.ySize, box.z);
        //    GL.Vertex3(box.x, box.y + box.ySize, box.z + box.zSize);

        //    //2
        //    GL.Vertex3(box.x + box.xSize, box.y, box.z);
        //    GL.Vertex3(box.x, box.y, box.z);

        //    GL.Vertex3(box.x + box.xSize, box.y, box.z);
        //    GL.Vertex3(box.x + box.xSize, box.y + box.ySize, box.z);

        //    GL.Vertex3(box.x + box.xSize, box.y, box.z);
        //    GL.Vertex3(box.x + box.xSize, box.y, box.z + box.zSize);

        //    //3
        //    GL.Vertex3(box.x + box.xSize, box.y + box.ySize, box.z + box.zSize);
        //    GL.Vertex3(box.x + box.xSize, box.y + box.ySize, box.z);

        //    GL.Vertex3(box.x + box.xSize, box.y + box.ySize, box.z + box.zSize);
        //    GL.Vertex3(box.x + box.xSize, box.y, box.z + box.zSize);

        //    GL.Vertex3(box.x + box.xSize, box.y + box.ySize, box.z + box.zSize);
        //    GL.Vertex3(box.x, box.y + box.ySize, box.z + box.zSize);

        //    //4
        //    GL.Vertex3(box.x, box.y, box.z + box.zSize);
        //    GL.Vertex3(box.x, box.y, box.z);

        //    GL.Vertex3(box.x, box.y, box.z + box.zSize);
        //    GL.Vertex3(box.x + box.xSize, box.y, box.z + box.zSize);

        //    GL.Vertex3(box.x, box.y, box.z + box.zSize);
        //    GL.Vertex3(box.x, box.y + box.ySize, box.z + box.zSize);

        //    GL.End();

        //}

        public void DrawDynamicOctree(List<BindBox> boxes)
        {
            GL.LineWidth(0.1f);
            GL.Enable(EnableCap.LineSmooth);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest); 
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse,GlobalSetting.DisplaySetting.MeshColor);
            for (int i = 0; i < boxes.Count; i++)
            {
                DrawBindBox(boxes[i]);
            }
        }

        public void DrawBindBox(BindBox box)
        {
            GL.Begin(BeginMode.Lines);

            //1
            GL.Vertex3(box.x, box.y, box.z);
            GL.Vertex3(box.x, box.y + box.ySize, box.z);

            GL.Vertex3(box.x, box.y + box.ySize, box.z);
            GL.Vertex3(box.x + box.xSize, box.y + box.ySize, box.z);

            GL.Vertex3(box.x, box.y + box.ySize, box.z);
            GL.Vertex3(box.x, box.y + box.ySize, box.z + box.zSize);

            //2
            GL.Vertex3(box.x + box.xSize, box.y, box.z);
            GL.Vertex3(box.x, box.y, box.z);

            GL.Vertex3(box.x + box.xSize, box.y, box.z);
            GL.Vertex3(box.x + box.xSize, box.y + box.ySize, box.z);

            GL.Vertex3(box.x + box.xSize, box.y, box.z);
            GL.Vertex3(box.x + box.xSize, box.y, box.z + box.zSize);

            //3
            GL.Vertex3(box.x + box.xSize, box.y + box.ySize, box.z + box.zSize);
            GL.Vertex3(box.x + box.xSize, box.y + box.ySize, box.z);

            GL.Vertex3(box.x + box.xSize, box.y + box.ySize, box.z + box.zSize);
            GL.Vertex3(box.x + box.xSize, box.y, box.z + box.zSize);

            GL.Vertex3(box.x + box.xSize, box.y + box.ySize, box.z + box.zSize);
            GL.Vertex3(box.x, box.y + box.ySize, box.z + box.zSize);

            //4
            GL.Vertex3(box.x, box.y, box.z + box.zSize);
            GL.Vertex3(box.x, box.y, box.z);

            GL.Vertex3(box.x, box.y, box.z + box.zSize);
            GL.Vertex3(box.x + box.xSize, box.y, box.z + box.zSize);

            GL.Vertex3(box.x, box.y, box.z + box.zSize);
            GL.Vertex3(box.x, box.y + box.ySize, box.z + box.zSize);

            GL.End();

        }

    }
}
