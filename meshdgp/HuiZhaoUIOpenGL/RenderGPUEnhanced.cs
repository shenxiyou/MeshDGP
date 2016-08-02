using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class RenderGPU:RenderBasic 
    {
        public override  void Render()
        {  

            OpenGLManager.Instance.Init();

            GlobalGPU.Instance.ResetGPURender();

            Matrix4D m = ToolPool.Instance.Tool.Ball.CreateMatrix() * TransformController.Instance.ModelMatrix;

            OpenGLManager.Instance.SetModelViewMatrix(m);
            RenderData(); 

            ToolPool.Instance.Tool.Draw();




        }
    }
}
