using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class MenuOpenGL : UserControl
    {
        public MenuOpenGL()
        {
            InitializeComponent();
            InitSubMenu();
        }

        protected virtual void OnChanged(EventArgs e)
        {
            GlobalData.Instance.OnChanged(e);
        }

        private void InitSubMenu()
        {
            foreach (EnumDisplayMode display in Enum.GetValues(typeof(EnumDisplayMode)))
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = display.ToString() + "ToolStripMenuItem";
                item.Text = display.ToString();
                item.Tag = display;
                item.Click += new EventHandler(item_Click);
                this.displayModeToolStripMenuItem.DropDownItems.Add(item);
            } 
        }

        private void item_Click(object sender, EventArgs e)
        {
           GlobalSetting.DisplaySetting.DisplayMode =  (EnumDisplayMode)((ToolStripMenuItem)sender).Tag;

           OnChanged(EventArgs.Empty);
        }

        private void openGLInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            FormOpenGL.Instance.Show();
            FormOpenGL.Instance.BringToFront();
        }

        private void openGLMatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
             
 
            FormMVPMatrix.Instance.Show();
            FormMVPMatrix.Instance.BringToFront();
        }

        private void colorVisulizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
     
            FormColorVis.Instance.Show();
            FormColorVis.Instance.BringToFront();
        }

        

        

        

        private void saveScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {

           
        }

       
        
    }
}
