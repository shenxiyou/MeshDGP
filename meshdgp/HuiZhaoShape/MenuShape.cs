using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class MenuShape : UserControl
    {
        public MenuShape()
        {
            InitializeComponent();
            InitShape(this.shapeToolStripMenuItem);

            InitTestShapeType(this.shapeToolStripMenuItem);
            InitShapeSel(this.shapeToolStripMenuItem);
        }

        private TriMesh Mesh
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

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfigShape.Instance.BringToFront();
            FormConfigShape.Instance.Show();
        }

        private void InitShapeSel(ToolStripMenuItem toolstrip)
        {
            ToolStripMenuItem item = new ToolStripMenuItem();

            item.Name = "toolToolStripMenuItem";
            item.Text = "Sel";

            toolstrip.DropDownItems.Add(item);
            foreach (EnumShapeSel type in Enum.GetValues(typeof(EnumShapeSel)))
            {
                ToolStripMenuItem subitem = new ToolStripMenuItem();
                subitem.Name = type.ToString() + "ToolStripMenuItem";
                subitem.Text = type.ToString();
                subitem.Tag = type;
                subitem.Click += ShapeSel_Click;
                item.DropDownItems.Add(subitem);
            }

        }

        private void ShapeSel_Click(object sender, EventArgs e)
        {
            EnumShapeSel type = (EnumShapeSel)((ToolStripMenuItem)sender).Tag;
            TriMesh mesh = TriMeshShape.Instance.CreateShapeSel(type);
            if(mesh!=null)
            {
                Mesh = mesh;
            }

            OnChanged(EventArgs.Empty);

        }

        private void InitShape(ToolStripMenuItem toolstrip)
        {
            ToolStripMenuItem item = new ToolStripMenuItem();

            item.Name = "toolToolStripMenuItem";
            item.Text = "Basic";

            toolstrip.DropDownItems.Add(item);
            foreach (EnumShape type in Enum.GetValues(typeof(EnumShape)))
            {
                ToolStripMenuItem subitem = new ToolStripMenuItem();
                subitem.Name = type.ToString() + "ToolStripMenuItem";
                subitem.Text = type.ToString();
                subitem.Tag = type;
                subitem.Click += Shape_Click;
                item.DropDownItems.Add(subitem);
            }

        }

        private void Shape_Click(object sender, EventArgs e)
        {
            EnumShape type = (EnumShape)((ToolStripMenuItem)sender).Tag;
            Mesh = TriMeshShape.Instance.CreateShape(type);

            OnChanged(EventArgs.Empty);

        }


        private void InitTestShapeType(ToolStripMenuItem toolstrip)
        {
            ToolStripMenuItem item = new ToolStripMenuItem();

            item.Name = "toolToolStripMenuItem";
            item.Text = "Test";

            toolstrip.DropDownItems.Add(item);
            foreach (EnumShapeDeform type in Enum.GetValues(typeof(EnumShapeDeform)))
            {
                ToolStripMenuItem subitem = new ToolStripMenuItem();
                subitem.Name = type.ToString() + "ToolStripMenuItem";
                subitem.Text = type.ToString();
                subitem.Tag = type;
                subitem.Click += shapeType_Click;
                item.DropDownItems.Add(subitem);
            }


           

        }

        private void shapeType_Click(object sender, EventArgs e)
        {
            EnumShapeDeform type = (EnumShapeDeform)((ToolStripMenuItem)sender).Tag;

            Mesh = TriMeshShape.Instance.CreateShape(type);

            OnChanged(EventArgs.Empty);
        }

       
    }
}
