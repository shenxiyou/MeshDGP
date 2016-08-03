using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao 
{
    public partial class UserControlMesh : UserControl
    {
        public UserControlMesh()
        {
            InitializeComponent();
        }


        public  TriMesh Mesh
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

        public virtual void OnChanged(EventArgs e)
        {
            GlobalData.Instance.OnChanged(e);
        }
    }
}
