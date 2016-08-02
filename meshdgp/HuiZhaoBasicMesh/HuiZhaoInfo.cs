using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{
    public class HuiZhaoInfo
    {
        private static HuiZhaoInfo instance = null;
        public static HuiZhaoInfo Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HuiZhaoInfo();
                }
                return instance;
            }
        }


        public String Title = "\"All Right is Reserved to Hui Zhao 赵辉。版权所有,盗版必究,如你在教育,科研,商业中使用，请联系graphicsresearch@qq.com\"";
        public String CopyRight = "";
    }
}
