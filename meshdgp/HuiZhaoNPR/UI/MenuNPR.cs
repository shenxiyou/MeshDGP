using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class MenuNPR : UserControl
    {
        public MenuNPR()
        {
            InitializeComponent();
            InitLine(this.linesToolStripMenuItem);
        }

        private  TriMesh Mesh
        {
            get
            {
                return GlobalData.Instance.TriMesh;
            }
            set
            {
                GlobalData.Instance.TriMesh = value;
            }
        }



        protected virtual void OnChanged(EventArgs e)
        {
            GlobalData.Instance.OnChanged(e);
        }

        

       

         private void clearToolStripMenuItem_Click(object sender, EventArgs e)
         {
             TriMeshUtil.ClearMeshColor(Mesh);
             GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.Basic;
             OnChanged(EventArgs.Empty);

         }

         private void configToolStripMenuItem_Click(object sender, EventArgs e)
         {
            
             FormConfigNPR.Instance.BringToFront();
             FormConfigNPR.Instance.Show();
             GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.OutSide;
             OpenGLTriMesh.Instance.OpenGLDraw += NPRDraw.DrawAllLines;
             OnChanged(EventArgs.Empty);
         }


         private void InitLine(ToolStripMenuItem toolStrip)
         {

             foreach (EnumLine type in Enum.GetValues(typeof(EnumLine)))
             {
                 ToolStripMenuItem item = new ToolStripMenuItem();
                 item.Name = type.ToString() + "ToolStripMenuItem";
                 item.Text = type.ToString();
                 item.Tag = type;
                 item.Click += quadratic_Click;
                 toolStrip.DropDownItems.Add(item);
             }

         }

         private void quadratic_Click(object sender, EventArgs e)
         {
             EnumLine type = (EnumLine)((ToolStripMenuItem)sender).Tag;

             if (Mesh == null)
             {
                 return;
             }

            
         }
    }
}
