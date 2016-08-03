using System;
using System.Collections.Generic;
using System.Drawing; 
  



namespace GraphicResearchHuiZhao
{
    public class RenderBasic:IRender 
    {  

         
        public void Init()
        {
             
        } 
        

        public  void Resize(int width, int height)
        {

            SettingTransform.Instance.ViewportWidth = width;
            SettingTransform.Instance.ViewportHeight = height; 
            
        }
          

        public virtual void Render()
        {  

            Matrix4D m = TransformController.Instance.ViewMatrix 
                       * ToolPool.Instance.Tool.Ball.CreateMatrix()
                       * TransformController.Instance.ModelMatrix;
             
            OpenGLManager.Instance.SetModelViewMatrix(m);

            OpenGLManager.Instance.SetProjectionMatrix(); 

            OpenGLManager.Instance.SetViewPort();

            OpenGLManager.Instance.Init();

            GlobalGPU.Instance.ResetGPURender();
            RenderData();

            //OpenGLManager.Instance.SetViewPortFirst();
            //RenderData();


            //OpenGLManager.Instance.SetViewPortSecond();
            //GlobalSetting.DisplaySetting.MeshColor = Color.CornflowerBlue;
            //RenderData();

            //OpenGLManager.Instance.SetViewPortThird();
            //GlobalSetting.DisplaySetting.MeshColor = Color.Tomato;
            //RenderData();

            //OpenGLManager.Instance.SetViewPortForth();
            //GlobalSetting.DisplaySetting.MeshColor = Color.DarkOrchid;
            //RenderData();
          
            ToolPool.Instance.Tool.Draw();       
             
           
        }


        public void RenderData()
        {
            OpenGLManager.Instance.RenderData();

        }

        

    }
}
