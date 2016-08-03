using System;
using System.Collections.Generic;

 
 

namespace GraphicResearchHuiZhao 
{
    public class SelectPointTool : BaseMeshTool
    {
       
        protected  int selectedPoint = -1;
        public SelectPointTool(double width, double height, NonManifoldMesh mesh)
            : base(width, height, mesh)
        {
            
        }

        #region ITool Members

        public override void MouseDown(Vector2D mouseDownPos, EnumMouseButton button)
        {
            if (mesh == null)
                return;
            base.MouseDown(mouseDownPos, button);

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
            if (button == EnumMouseButton.Left)
            {
                selectedPoint = SelectByPoint();
            }
          
            mesh.GroupingFlags();
            OnChanged(EventArgs.Empty);
        }

        #endregion

        protected virtual int SelectByPoint()
        {
            return SelectVertexByPoint(false);
        }
        

        protected  int SelectVertexByPoint(bool laser)
        {
           // Rectangle viewport = new Rectangle(0, 0,  Width,  Height);
           // OpenGLProjector projector = new OpenGLProjector();
           // NonManifoldMesh m =mesh;

           // double eps = ToolSetting.ToolsSetting.DepthTolerance;

           // double minDis = double.MaxValue;
           // int minIndex = -1;
           // for (int i = 0, j = 0; i < m.VertexCount; i++, j += 3)
           // {
           //     GraphicResearchHuiZhao.Vector3D v = projector.Project(m.VertexPos, j);
           //     GraphicResearchHuiZhao.Vector2D u = new GraphicResearchHuiZhao.Vector2D(v.x, v.y);
           //     if (!viewport.Contains((int)v.x, (int)v.y)) continue;
           //     if (!laser && projector.GetDepthValue((int)v.x, (int)v.y) - v.z < eps) continue;

           //     double dis = (u - mouseCurrPos).Length();
           //     if (dis < minDis)
           //     {
           //         minIndex = i;
           //         minDis = dis;
           //     }
           // }
           // if (minIndex == -1) return minIndex ;
           //// GlobalFunction.Output("selected vertex: " + minIndex.ToString());

           // if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
           //     m.VertexFlag[minIndex] = (byte)1;
           // else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
           //     m.VertexFlag[minIndex] = (byte)0;
           // else
           // {
           //     for (int i = 0; i < m.VertexCount; i++) m.VertexFlag[i] = (byte)0;
           //     m.VertexFlag[minIndex] = (byte)1;
           // }
           // return minIndex;
            return 0;
        }


        public override void Draw()
        {

            OpenGLNonManifold.Instance.DrawSelectedVerticeBySphere(mesh); 
            
        }

    }
}
