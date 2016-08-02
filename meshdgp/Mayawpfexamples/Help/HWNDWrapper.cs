using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicResearchHuiZhao
{
    public class HWNDWrapper : System.Windows.Forms.IWin32Window
    {
        private IntPtr hwnd;

        public HWNDWrapper(IntPtr h)
        {
            hwnd = h;
        }

        public IntPtr Handle
        {
            get
            {
                return hwnd;
            }
        }
    }
}
