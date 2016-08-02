using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public  class ToolSetting
    {
          private static ToolsSettingRecord toolsSetting = new ToolsSettingRecord("Tools Setting");
 
         


      
        public static ToolsSettingRecord ToolsSetting
        {
            get
            {
                return toolsSetting;
            }
        }

        
        

    }
}
