using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class MenuHelp : UserControl
    {
        public MenuHelp()
        {
            InitializeComponent();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout.Instance.ShowDialog();
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("本软件由赵辉老师团队开发。版权所有，盗版必究，如你在教育，科研,商业中使用，请联系graphicsresearch@qq.com");

        }
    }
}
