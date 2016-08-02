using System;
using System.Collections.Generic;
 
 
 

namespace GraphicResearchHuiZhao 
{
    public class SelectRectangleTool : BaseMeshTool
    {


        public SelectRectangleTool(double width, double height, NonManifoldMesh mesh)
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

            SelectByRectangle();
                  
      
            mesh.GroupingFlags();
            OnChanged(EventArgs.Empty);
        }

        #endregion

        protected virtual void SelectByRectangle()
        {
            SelectVertexByRect(false);
        }

        protected  void SelectVertexByRect(bool laser)
        {
            //GraphicResearchHuiZhao.Vector2D minV = GraphicResearchHuiZhao.Vector2D.Min(mouseDownPos, mouseCurrPos);
            //GraphicResearchHuiZhao.Vector2D size = GraphicResearchHuiZhao.Vector2D.Max(mouseDownPos, mouseCurrPos) - minV;
            //Rectangle rect = new Rectangle((int)minV.x, (int)minV.y, (int)size.x, (int)size.y);
            //Rectangle viewport = new Rectangle(0, 0,  Width,  Height);
            //OpenGLProjector projector = new OpenGLProjector();
            //NonManifoldMesh m = mesh;

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

        

        public override void Draw()
        {

            OpenGLNonManifold.Instance.DrawSelectedVerticeBySphere(mesh);

            if (this.IsMouseDown)
                OpenGLManager.Instance.DrawSelectionInterface(Width, Height, this.MouseDownPos.x, this.MouseDownPos.y, this.MouseCurrPos.x, this.MouseCurrPos.y, OpenGLFlatShape.Rectangle); 

        }

    }
}
