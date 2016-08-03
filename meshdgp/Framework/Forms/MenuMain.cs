using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao.Forms
{
    public partial class MenuMain : UserControl
    {
        public MenuMain()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.menuFile.Init();
        }
    }
}
