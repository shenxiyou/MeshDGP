using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class MenuOperation : UserControl
    {
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

        public MenuOperation()
        {
            InitializeComponent();
            InitRemove(meshOperationToolStripMenuItem);
            InitSubdivion(meshOperationToolStripMenuItem);
         
              
            InitInverseFace(meshOperationToolStripMenuItem);

            InitCut(meshOperationToolStripMenuItem);
             
        }

        private void InitCut(ToolStripMenuItem toolstrip)
        {


            ToolStripMenuItem item = new ToolStripMenuItem();

            item.Name = "cutToolStripMenuItem";
            item.Text = "Cut";
            item.Click += Cut_Click;

            toolstrip.DropDownItems.Add(item);


        }

        private void Cut_Click(object sender, EventArgs e)
        {
            TriMeshModify.ComputeSelectedEdgeFromColor(Mesh);
            double move = TriMeshUtil.ComputeEdgeAvgLength(Mesh) * 0.1;
            TriMeshEdgeCutFromBoundary cut = new TriMeshEdgeCutFromBoundary(Mesh);
            cut.Cut(move);

            TriMeshUtil.ClearMeshColor(Mesh);
            TriMeshUtil.ShowBoundary(Mesh);
            TriMeshUtil.FixIndex(Mesh);
            TriMeshUtil.SetUpNormalVertex(Mesh);
            OnChanged(EventArgs.Empty);

        }


        private void InitInverseFace(ToolStripMenuItem toolstrip)
        {


            ToolStripMenuItem item = new ToolStripMenuItem();

            item.Name = "swapToolStripMenuItem";
            item.Text = "Inverse Face";
            item.Click += FaceInverse_Click;

            toolstrip.DropDownItems.Add(item);


        }

        private void FaceInverse_Click(object sender, EventArgs e)
        {
            TriMeshModify.InverseFace(Mesh);

            OnChanged(EventArgs.Empty);

        }

        private void InitEdgeSwap(ToolStripMenuItem toolstrip)
        {


            ToolStripMenuItem item = new ToolStripMenuItem();

            item.Name = "swapToolStripMenuItem";
            item.Text = "Edge Swap";
            item.Click += EdgeSwap_Click;

            toolstrip.DropDownItems.Add(item);
            

        }

        private void EdgeSwap_Click(object sender, EventArgs e)
        {
            TriMeshModify.EdgeSwap(Mesh);

            OnChanged(EventArgs.Empty);

        }

        

       



        




     



        private void InitRemove(ToolStripMenuItem toolstrip)
        {
            ToolStripMenuItem item = new ToolStripMenuItem();
            toolstrip.DropDownItems.Add(item);
            item.Name = "toolToolStripMenuItem";
            item.Text = "Remove";

            

            foreach (EnumRemove type in Enum.GetValues(typeof(EnumRemove)))
            {
                ToolStripMenuItem subitem = new ToolStripMenuItem();
                subitem.Name = type.ToString() + "ToolStripMenuItem";
                subitem.Text = type.ToString();
                subitem.Tag = type;
                subitem.Click += Remove_Click;
                item.DropDownItems.Add(subitem);
            }

        }

        private void InitSubdivion(ToolStripMenuItem toolstrip)
        {
            ToolStripMenuItem item = new ToolStripMenuItem();
            toolstrip.DropDownItems.Add(item);
            item.Name = "toolToolStripMenuItem";
            item.Text = "Subdivision";
            foreach (EnumSubdivision type in Enum.GetValues(typeof(EnumSubdivision)))
            {
                ToolStripMenuItem subitem = new ToolStripMenuItem();
                subitem.Name = type.ToString() + "ToolStripMenuItem";
                subitem.Text = type.ToString();
                subitem.Tag = type;
                subitem.Click += Sub_Click;
                item.DropDownItems.Add(subitem);
            }

        }

        private void Sub_Click(object sender, EventArgs e)
        {
            EnumSubdivision type = (EnumSubdivision)((ToolStripMenuItem)sender).Tag;

            TriMeshSubdivision sub = new TriMeshSubdivision(Mesh);
            Mesh=sub.SubDivision(type); 

            OnChanged(EventArgs.Empty);
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            EnumRemove type = (EnumRemove)((ToolStripMenuItem)sender).Tag;

            TriMeshModify.Remove(Mesh, type);
            OnChanged(EventArgs.Empty);
        }

        private void simplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSimplify.Instance.Show();  
            OnChanged(EventArgs.Empty);
        }

        private void taubinSmoothToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TriMeshOPController.Smooth(Mesh);

            TriMeshModify.SmoothTaubin(Mesh);

            OnChanged(EventArgs.Empty);
        }

        private void oneHoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshModify.RepaireOneHole(Mesh);
            OnChanged(EventArgs.Empty);
        }

        private void allHoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshModify.RepaireAllHole(Mesh);
            OnChanged(EventArgs.Empty);
        }

        private void dualAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlobalData.Instance.DualMesh = TriMeshDual2.BuildDual(Mesh, EnumDual.DualA);
            OnChanged(EventArgs.Empty);

               
        }

        private void dualBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlobalData.Instance.DualMesh = TriMeshDual2.BuildDual(Mesh, EnumDual.DualB);
            OnChanged(EventArgs.Empty);
        }

        private void curveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCurve.Instance.BringToFront();
            FormCurve.Instance.Show();
        }

        private void segementationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSegementation.Instance.BringToFront();
             FormSegementation.Instance.Show();
        }

        private void complexHoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshModify.RepairComplexHole(Mesh);
            OnChanged(EventArgs.Empty);
        }

        private void planeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuadMesh mesh = QuadShape.Instance.CreatePlane(10, 20);
            QuadMeshUtil.ScaleToUnit(mesh, 1.0);
            QuadMeshUtil.MoveToCenter(mesh);
            QuadMeshUtil.ComputeNormal(mesh);  
            GlobalData.Instance.QuadMesh = mesh;
        }

        private void cubeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuadMesh mesh = QuadShape.Instance.CreateCube();
            QuadMeshUtil.ScaleToUnit(mesh, 1.0);
            QuadMeshUtil.MoveToCenter(mesh);
            QuadMeshUtil.ComputeNormal(mesh);
            GlobalData.Instance.QuadMesh = mesh;
        }

        private void catMullClarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SubdivisionQuad sub = new SubdivisionQuad(GlobalData.Instance.QuadMesh);
            GlobalData.Instance.QuadMesh = sub.CatMullClark();
            QuadMeshUtil.ComputeNormal(GlobalData.Instance.QuadMesh);
        }

        private void dooSabinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SubdivisionQuad sub = new SubdivisionQuad(GlobalData.Instance.QuadMesh);
            GlobalData.Instance.QuadMesh = sub.DooSabin();
            QuadMeshUtil.ComputeNormal(GlobalData.Instance.QuadMesh);
        }

        private void swapEdgeToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            TriMeshModify.EdgeSwap(Mesh);
            OnChanged(EventArgs.Empty);
        }

        private void mergeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            TriMeshModify.MergeEdge(Mesh); 
            OnChanged(EventArgs.Empty);
        }

        private void splitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshModify.VertexSplit(Mesh);
            OnChanged(EventArgs.Empty);
        }

        private void show567ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Test567 test = new Test567(Mesh);
            test.SetColor();
            OnChanged(EventArgs.Empty);
        }


        private int faceCount = 100;
        private void remove567ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Test567 test = new Test567(Mesh);
            this.faceCount = test.Mesh.Faces.Count;
            test.Step3();
            test.Step4();
            test.Subdivision();
            test.StepGT7();
            TriMeshUtil.FixIndex(Mesh);

            TriMeshUtil.SetUpNormalVertex(Mesh);
            OnChanged(EventArgs.Empty);
            
        }

        private void simplify567ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Test567 test = new Test567(Mesh);
            test.CollapseAll(this.faceCount);
            TriMeshUtil.FixIndex(Mesh);
            TriMeshUtil.SetUpNormalVertex(Mesh);
            OnChanged(EventArgs.Empty);

            
        }

        

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var enhance = new TriMeshEnhancement(GlobalData.Instance.TriMesh);
            enhance.Show();
            OnChanged(EventArgs.Empty);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var enhance = new TriMeshEnhancement(Mesh);
            enhance.RemoveCapWithSplit();

            TriMeshUtil.FixIndex(Mesh);
            TriMeshUtil.SetUpNormalVertex(Mesh);
            OnChanged(EventArgs.Empty);
        }

        private void showToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var enhance = new TriMeshEnhancement(Mesh);
            enhance.PlaneCutTest();
            TriMeshUtil.FixIndex(Mesh);
            TriMeshUtil.SetUpNormalVertex(Mesh);
        }

        private void removeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var enhance = new TriMeshEnhancement(Mesh);
            enhance.RemoveCapWithPlane();
            TriMeshUtil.FixIndex(Mesh);
            TriMeshUtil.SetUpNormalVertex(Mesh);
        }

        private void noiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshModify.AddNoise(Mesh, 1.4);
            OnChanged(EventArgs.Empty);
        }

        private void reconEigenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LapalceRecon recon = new LapalceRecon();
            Mesh = recon.Recon(Mesh, ConfigMeshOP.Instance.EigenCount);
            OnChanged(EventArgs.Empty);

        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfigOP.Instance.BringToFront();
            FormConfigOP.Instance.Show();
        }

        private void randomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshUtil.SelectedVertexRandom(Mesh, ConfigMeshOP.Instance.RandomSelect);
            OnChanged(EventArgs.Empty);

        }

        private void reverseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            TriMeshUtil.SelectedVertexReverse(Mesh);
            TriMeshUtil.GroupVertice(Mesh);
            OnChanged(EventArgs.Empty);
        }

        private void moveToCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshUtil.MoveToCenter(Mesh);
            OnChanged(EventArgs.Empty);
        }

        private void scaleToUnitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshUtil.ScaleToUnit(Mesh,0.9);
            OnChanged(EventArgs.Empty);
        }

        private void expandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshModify.BoundaryExpand(Mesh);
            OnChanged(EventArgs.Empty);
        }

        private void shrinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshModify.BoundaryShrink(Mesh);
            OnChanged(EventArgs.Empty);
        }

        private void planeCutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriMeshPlaneCut cut=new TriMeshPlaneCut(Mesh);
            Plane plane = GlobalSetting.ClipPlaneSetting.Plane0; 
            
            if(GlobalSetting.ClipPlaneSetting.Enable0)
            {
              plane=GlobalSetting.ClipPlaneSetting.Plane0; 
              cut.Cut(plane);
            }
            if (GlobalSetting.ClipPlaneSetting.Enable1)
            {
                plane = GlobalSetting.ClipPlaneSetting.Plane1;
                cut.Cut(plane);
            }
            if (GlobalSetting.ClipPlaneSetting.Enable2)
            {
                plane = GlobalSetting.ClipPlaneSetting.Plane2;
                cut.Cut(plane);
            }
            if (GlobalSetting.ClipPlaneSetting.Enable3)
            {
                plane = GlobalSetting.ClipPlaneSetting.Plane3;
                cut.Cut(plane);
            }
            if (GlobalSetting.ClipPlaneSetting.Enable4)
            {
                plane = GlobalSetting.ClipPlaneSetting.Plane4;
                cut.Cut(plane);
            }
            if (GlobalSetting.ClipPlaneSetting.Enable5)
            {
                plane = GlobalSetting.ClipPlaneSetting.Plane5;
                cut.Cut(plane);
            }
            OnChanged(EventArgs.Empty);
        }

       

       
         
    }
}
