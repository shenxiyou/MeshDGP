using System;
using System.Collections.Generic;

 
 

namespace GraphicResearchHuiZhao 
{
    public class ExtendSelectionTool:BaseMeshTool
    {


        public ExtendSelectionTool(double width, double height, NonManifoldMesh mesh)
            : base(width,height, mesh)
        {
           
        }

        #region ITool Members

        public override void MouseDown(Vector2D mouseDownPos, EnumMouseButton button)
        {
            if (mesh == null)
                return;
            base.MouseDown(mouseDownPos, button);

            Extend();
            OnChanged(EventArgs.Empty);

        }

      

       
        #endregion

        protected virtual void Extend()
        {
            //NonManifoldMesh m = mesh;
            //int n=m.VertexCount;
            //byte[] vertexSelect = new byte[n];
            
            //for (int i = 0; i < n; i++)
            //{
            //    vertexSelect[i] = m.VertexFlag[i];
            //}
            //for (int i = 0; i < n; i++)
            //{
            //    if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            //    {
            //        if (vertexSelect[i] == (byte)1)
            //        {
            //            bool all = true;
            //            foreach (int j in m.AdjVV[i])
            //            {
            //                if (vertexSelect[j] != (byte)1)
            //                {
            //                    all = false;
            //                }
            //            }
            //            if (!all)
            //            {
            //                m.VertexFlag[i] = (byte)0;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (vertexSelect[i] == (byte)1)
            //        {                        
            //            foreach (int j in m.AdjVV[i])
            //            {
            //                m.VertexFlag[j] = (byte)1;
            //            }
            //        }
            //    }
                    
            //}
            
        }
        

 


        public override void Draw()
        {

            OpenGLNonManifold.Instance.DrawSelectedVerticeBySphere(mesh); 
            
        }
    }
}
