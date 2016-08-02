using System;
using System.Collections.Generic;

 
 


namespace GraphicResearchHuiZhao 
{
    public class SelectRandomTool:BaseMeshTool
    {

        public SelectRandomTool(double width, double height, NonManifoldMesh mesh)
            : base(width,height , mesh)
        {
             
        }

        #region ITool Members


        public override void MouseUp(Vector2D mouseUpPos, EnumMouseButton button)
        {
            //if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            //{
            //    if (button == EnumMouseButton.Left)
            //    {
            //        if ((Control.ModifierKeys & Keys.Shift) != Keys.Shift)
            //        {
            //            NonManifoldMesh m =mesh;
            //            int n = m.VertexCount;
            //            for (int i = 0; i < n; i++)
            //            {
            //                m.VertexFlag[i] = (byte)0;
            //            }
            //        }
            //        SelectRandomPoint();
            //    }
            //}

            //if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt)
            //{ 
            //        NonManifoldMesh m = mesh;
            //        int n =  m.VertexCount;
            //        for (int i = 0; i < n; i++)
            //        {
            //            m.VertexFlag[i] = (byte)0;
            //        }
              
            //}
            //base.MouseUp(mouseUpPos,button);

          
       
        }

        #endregion

        protected virtual void SelectRandomPoint()
        {
            SelectRandomPoint(0.05);
        }


        protected void SelectRandomPoint(double percent)
        {
            NonManifoldMesh m = mesh;
            int n = m.VertexCount ;
            int randomCount =(int)( n * percent);
            int index = 0;
            Random random = new Random();
             
            for (int j = 0; j < randomCount; j++)
            {
                index = random.Next(0, n-1);
                m.VertexFlag[index] = (byte)1;
               
              
            }
        }

        

        public override void Draw()
        {

            OpenGLNonManifold.Instance.DrawSelectedVerticeBySphere(mesh);
 
        }

    }
}
