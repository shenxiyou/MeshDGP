using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class MenuFunction : UserControl
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

        public MenuFunction()
        {
            InitializeComponent();

            InitFunction(functionToolStripMenuItem);
        }

        private void InitFunction(ToolStripMenuItem  toolStrip)
        { 
            toolStrip.Text = "Function";
            foreach (EnumFunction fucntion in Enum.GetValues(typeof(EnumFunction)))
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = fucntion.ToString() + "ToolStripMenuItem";
                item.Text = fucntion.ToString();
                item.Tag = fucntion;
                item.Click += Function_Click;
                toolStrip.DropDownItems.Add(item);
            }

        }

        private void Function_Click(object sender, EventArgs e)
        {
            EnumFunction funcType = (EnumFunction)((ToolStripMenuItem)sender).Tag; 
            TriMeshFunction.Instance.FunctionType = funcType; 
            ComputeFunction();

            
        }


       
        private void ComputeFunction()
        {


            double[] function = TriMeshFunction.Instance.ComputeFunction(Mesh);

            ConfigFunction.Instance.min = TriMeshFunction.Instance.ComputeMin(function);

            ConfigFunction.Instance.max = TriMeshFunction.Instance.ComputeMax(function);

            ConfigFunction.Instance.MinClamp = TriMeshFunction.Instance.ComputeMin(function);

            ConfigFunction.Instance.MaxClamp = TriMeshFunction.Instance.ComputeMax(function);


            TriMeshFunction.Instance.Clamp(function, ConfigFunction.Instance.MinClamp, ConfigFunction.Instance.MaxClamp);


           
            GlobalData.Instance.ColorVis = TriMeshFunction.Instance.Unit(function,ConfigFunction.Instance.Nodal);
          
            
            GlobalSetting.DisplaySetting.DisplayMode = EnumDisplayMode.ColorVis;

            GlobalGPU.Instance.SwitchGPURender(new GPURenderColorVis()); 
        

            OnChanged(EventArgs.Empty);
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfigFunc.Instance.BringToFront();
            FormConfigFunc.Instance.Show();

            FormConfigFunc.Instance.Changed += new MeshChangedDelegate(Instance_Changed);
          
        }

        void Instance_Changed(object sender, EventArgs e)
        {
            ComputeFunction();
        }

        private void drawToMinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[] function = TriMeshFunction.Instance.ComputeFunction(Mesh);
            Morse morse = new Morse(Mesh, function);
            morse.DrawSaddleToMin(Mesh, function);
            morse.ColorMorseVertice(Mesh, function);

            OnChanged(EventArgs.Empty);
        }

        private void drawToMaxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[] function = TriMeshFunction.Instance.ComputeFunction(Mesh);
            Morse morse = new Morse(Mesh, function);
            morse.DrawSaddleToMax(Mesh, function);
            morse.ColorMorseVertice(Mesh, function);
        }

        private void drawMinMaxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[] function = TriMeshFunction.Instance.ComputeFunction(Mesh);
            Morse morse = new Morse(Mesh, function);
            morse.DrawSaddleToMin(Mesh, function);
            morse.DrawSaddleToMax(Mesh, function);
            morse.ColorMorseVertice(Mesh, function);

            OnChanged(EventArgs.Empty);
        }

        private void showMinMaxSaddleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[] function = TriMeshFunction.Instance.ComputeFunction(Mesh);
            Morse morse = new Morse(Mesh, function);

            morse.ColorMorseVertice(Mesh, function);

            OnChanged(EventArgs.Empty);
        }

        private void baseDomain1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[] function = TriMeshFunction.Instance.ComputeFunction(Mesh);
            Morse morse = new Morse(Mesh, function);
            morse.DrawSaddleToMin(Mesh, function);


            MorseComplex bd = new MorseComplex(Mesh);
            Mesh = bd.BuildWithMax();

            bd.SetColor();


            TriMeshUtil.SetUpNormalVertex(Mesh);
        }

       
    }
}
