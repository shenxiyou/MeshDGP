using System;
using System.Collections.Generic;

 
 

namespace GraphicResearchHuiZhao
{
    public class SelectionTool : BaseMeshTool
    {

        public SelectionTool(double width, double height, NonManifoldMesh mesh)
            : base(width,height, mesh)
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

            switch (ToolSetting.ToolsSetting.SelectionMethod)
            {
                case ToolsSettingRecord.EnumSelectingMethod.Rectangle:
                     SelectVertexByRect();
                    break;
                case ToolsSettingRecord.EnumSelectingMethod.Point:
                     SelectVertexByPoint();
                    break;
            }
 
            mesh.GroupingFlags();
            OnChanged(EventArgs.Empty);
        }

        #endregion


        private void SelectVertexByRect()
        {
            //GraphicResearchHuiZhao.Vector2D minV = GraphicResearchHuiZhao.Vector2D.Min(mouseDownPos, mouseCurrPos);
            //GraphicResearchHuiZhao.Vector2D size = GraphicResearchHuiZhao.Vector2D.Max(mouseDownPos, mouseCurrPos) - minV;
            //Rectangle rect = new Rectangle((int)minV.x, (int)minV.y, (int)size.x, (int)size.y);
            //Rectangle viewport = new Rectangle(0, 0,  Width,  Height);
            //OpenGLProjector projector = new OpenGLProjector();
            //NonManifoldMesh m = mesh;
            //bool laser =ToolSetting.ToolsSetting.Laser;
            //double eps = ToolSetting.ToolsSetting.DepthTolerance;

            //if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            //    for (int i = 0, j = 0; i < m.VertexCount; i++, j += 3)
            //    {
            //        GraphicResearchHuiZhao.Vector3D v = projector.Project(m.VertexPos, j);
            //        if (viewport.Contains((int)v.x, (int)v.y))
            //        {
            //            bool flag = rect.Contains((int)v.x, (int)v.y);
            //            flag &= (laser || projector.GetDepthValue((int)v.x, (int)v.y) - v.z >= eps);
            //            if (flag) m.VertexFlag[i] = (byte)1;
            //        }
            //    }

            //else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            //    for (int i = 0, j = 0; i < m.VertexCount; i++, j += 3)
            //    {
            //        GraphicResearchHuiZhao.Vector3D v = projector.Project(m.VertexPos, j);
            //        if (viewport.Contains((int)v.x, (int)v.y))
            //        {
            //            bool flag = rect.Contains((int)v.x, (int)v.y);
            //            flag &= (laser || projector.GetDepthValue((int)v.x, (int)v.y) - v.z >= eps);
            //            if (flag) m.VertexFlag[i] = (byte)0;
            //        }
            //    }

            //else
            //    for (int i = 0, j = 0; i < m.VertexCount; i++, j += 3)
            //    {
            //        GraphicResearchHuiZhao.Vector3D v = projector.Project(m.VertexPos, j);
            //        if (viewport.Contains((int)v.x, (int)v.y))
            //        {
            //            bool flag = rect.Contains((int)v.x, (int)v.y);
            //            flag &= (laser || projector.GetDepthValue((int)v.x, (int)v.y) - v.z >= eps);
            //            m.VertexFlag[i] = (byte)((flag) ? 1 : 0);
            //        }
            //    }
        }

        private void SelectVertexByPoint()
        {
            //Rectangle viewport = new Rectangle(0, 0, Width,  Height);
            //OpenGLProjector projector = new OpenGLProjector();
            //NonManifoldMesh m = mesh;
            //bool laser = ToolSetting.ToolsSetting.Laser;
            //double eps = ToolSetting.ToolsSetting.DepthTolerance;

            //double minDis = double.MaxValue;
            //int minIndex = -1;
            //for (int i = 0, j = 0; i < m.VertexCount; i++, j += 3)
            //{
            //    GraphicResearchHuiZhao.Vector3D v = projector.Project(m.VertexPos, j);
            //    GraphicResearchHuiZhao.Vector2D u = new GraphicResearchHuiZhao.Vector2D(v.x, v.y);
            //    if (!viewport.Contains((int)v.x, (int)v.y)) continue;
            //    if (!laser && projector.GetDepthValue((int)v.x, (int)v.y) - v.z < eps) continue;

            //    double dis = (u - mouseCurrPos).Length();
            //    if (dis < minDis)
            //    {
            //        minIndex = i;
            //        minDis = dis;
            //    }
            //}
            //if (minIndex == -1) return;
            ////GlobalFunction.Output("selected vertex: " + minIndex.ToString());

            //if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            //    m.VertexFlag[minIndex] = (byte)1;
            //else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            //    m.VertexFlag[minIndex] = (byte)0;
            //else
            //{
            //    for (int i = 0; i < m.VertexCount; i++) m.VertexFlag[i] = (byte)0;
            //    m.VertexFlag[minIndex] = (byte)1;
            //}
        }


        public override void Draw()
        {

            OpenGLNonManifold.Instance.DrawSelectedVerticeBySphere(mesh);

            if (this.IsMouseDown)
                OpenGLManager.Instance.DrawSelectionInterface(Width, Height, this.MouseDownPos.x, this.MouseDownPos.y, this.MouseCurrPos.x, this.MouseCurrPos.y, OpenGLFlatShape.Rectangle); 

        }
    
    }
}
