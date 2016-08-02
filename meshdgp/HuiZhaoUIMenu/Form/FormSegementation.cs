using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class FormSegementation : Form
    {




        public TriMesh Mesh
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


        private static FormSegementation singleton = new FormSegementation();

        public static FormSegementation Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new FormSegementation();
                return singleton;
            }
        }

    

      

 
        protected virtual void OnChanged(EventArgs e)
        {
            GlobalData.Instance.OnChanged(e);
        }

        public FormSegementation()
        {
            InitializeComponent();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            SegementationAreaGrow seg = new SegementationAreaGrow(Mesh);
            seg.Clear();
            OnChanged(EventArgs.Empty);
        }

        private void buttonByVertex_Click(object sender, EventArgs e)
        {
            SegementationGrow.GrowByVertexArea(Mesh);

            GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.SegementationVertex;

            OnChanged(EventArgs.Empty);
        }

        private void buttonByFace_Click(object sender, EventArgs e)
        {
            GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.SegementationFace;
            SegementationGrow.Vertex2Face(Mesh);
            SegementationGrow.GrowByFaceAngle(Mesh);
            OnChanged(EventArgs.Empty);
        }

        private void buttonSelectVertex_Click(object sender, EventArgs e)
        {
            ToolPool.Instance.SwitchTool(EnumTool.VertexByPoint);
            GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.SelectedVertex;
        }

        private void buttonView_Click(object sender, EventArgs e)
        {
            ToolPool.Instance.SwitchTool(EnumTool.View);
        }

        private void buttonV2F_Click(object sender, EventArgs e)
        {
            SegementationGrow.Vertex2Face(Mesh);

            GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.SegementationFace;
            ToolPool.Instance.SwitchTool(EnumTool.View);
            OnChanged(EventArgs.Empty);
        }

        private void buttonOneStepFace_Click(object sender, EventArgs e)
        {
            SegementationAreaGrow seg = new SegementationAreaGrow(Mesh);
            seg.AreaGrowByFace();
            GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.SegementationFace;

            ToolPool.Instance.SwitchTool(EnumTool.View);
            OnChanged(EventArgs.Empty);
        }

        private void buttonNotSelected_Click(object sender, EventArgs e)
        {
            SegementationAreaGrow seg = new SegementationAreaGrow(Mesh);
            seg.ByFacesNotSelect();
            GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.SegementationFace;
            ToolPool.Instance.SwitchTool(EnumTool.View);
            OnChanged(EventArgs.Empty);
        }

        private void buttonKmeans_Click(object sender, EventArgs e)
        {
            //SegementationKMean seg = new SegementationKMean(Mesh);

            //seg.Init();
            //seg.K_means();
            //GlobalSetting.MeshDisplaySetting.MeshDisplayMode = EnumDisplayMode.SegementationVertex;

            SegementationGrow.Vertex2Face(Mesh);
            SegementationGrow.KMean(Mesh);
            GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.SegementationFace;

            ToolPool.Instance.SwitchTool(EnumTool.View);
            OnChanged(EventArgs.Empty);
        }

        private void buttonRegion_Click(object sender, EventArgs e)
        {
            SegementationRegion seg = new SegementationRegion(Mesh);
            seg.Run();
            //SegementationGrow.RegionGrow(Mesh);
            GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.SegementationFace;

            ToolPool.Instance.SwitchTool(EnumTool.View);
            OnChanged(EventArgs.Empty);
        }
    }
}
