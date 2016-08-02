using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao 
{
    public class GlobalTetSetting
    {
        static TetDisplayFlag tetDisplayFlag = TetDisplayFlag.Edge | TetDisplayFlag.Face;
        public static TetDisplayFlag TetDisplayFlag
        {
            get { return tetDisplayFlag; }
            set { tetDisplayFlag = value; }
        }
    }
}
