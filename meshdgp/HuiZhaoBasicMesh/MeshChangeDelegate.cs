using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicResearchHuiZhao
{

    public class EventArgsDisplay : EventArgs
    {
        public int TypeDisplayMode = 0;

        public EventArgsDisplay(int type)
        {
            this.TypeDisplayMode = type;
        }

    }
    
 
    public delegate void MeshChangedDelegate(object sender, EventArgs e);
}
