using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public interface IRender 
    {
        
        void Resize(int width, int height);
        void Render();
        void Init(); 
   
    }
}
